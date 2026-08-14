using System.Drawing;
using System.IO;

namespace Aspose.PSD.FOSS;

/// <summary>
/// Represents a PSD image that can be loaded, inspected, and saved without rendering.
/// </summary>
public sealed class PsdImage : IDisposable
{
    /// <summary>
    /// Stores the underlying source or destination stream.
    /// </summary>
    private readonly Stream _stream;

    /// <summary>
    /// Indicates whether the underlying stream must remain open after disposal.
    /// </summary>
    private readonly bool _leaveOpen;

    /// <summary>
    /// Tracks whether the instance has already been disposed.
    /// </summary>
    private bool _disposed;

    /// <summary>
    /// Gets the document width in pixels.
    /// </summary>
    public int Width => _header?.Width ?? 0;

    /// <summary>
    /// Gets the document height in pixels.
    /// </summary>
    public int Height => _header?.Height ?? 0;

    /// <summary>
    /// Gets the document channel count from the PSD header.
    /// </summary>
    public int Channels => _header?.Channels ?? 0;

    /// <summary>
    /// Gets the number of bits stored per channel.
    /// </summary>
    public int BitsPerChannel => _header?.BitDepth ?? 0;

    /// <summary>
    /// Gets the PSD color mode reported by the header.
    /// </summary>
    public ColorModes ColorMode => _header?.ColorMode ?? ColorModes.Rgb;

    /// <summary>
    /// Gets the PSD or PSB version value from the header.
    /// </summary>
    public int Version => _header?.Version ?? 1;

    /// <summary>
    /// Gets the parsed layer collection.
    /// </summary>
    public Layer[] Layers => _layers ?? [];

    /// <summary>
    /// Gets a value indicating whether the document contains at least one parsed layer.
    /// </summary>
    public bool HasLayers => _layers?.Length > 0;

    /// <summary>
    /// Stores the parsed PSD header.
    /// </summary>
    private PsdHeader? _header;

    /// <summary>
    /// Stores the parsed layer records.
    /// </summary>
    private Layer[]? _layers;

    /// <summary>
    /// Stores the raw color mode data section.
    /// </summary>
    private byte[] _colorData = [];

    /// <summary>
    /// Stores parsed image resource blocks.
    /// </summary>
    private ResourceBlock[] _resources = [];

    /// <summary>
    /// Stores the raw Layer and Mask Information section for byte-exact no-mutation saves.
    /// </summary>
    private byte[] _layerAndMaskInfoRaw = [];

    /// <summary>
    /// Stores the raw layer channel image data part of the layer info payload.
    /// </summary>
    private byte[] _layerChannelImageDataRaw = [];

    /// <summary>
    /// Stores the raw global mask info and any trailing bytes after the layer info payload.
    /// </summary>
    private byte[] _layerGlobalMaskAndTailRaw = [];

    /// <summary>
    /// Stores the original signed layer count value so the save path can preserve its sign.
    /// </summary>
    private short _layerCountRaw;

    /// <summary>
    /// Stores the raw image data section after its compression field.
    /// </summary>
    private byte[] _imageData = [];

    /// <summary>
    /// Stores the image data compression method as read from the file.
    /// </summary>
    private int _imageDataCompression;

    private PsdImage(Stream stream, bool leaveOpen)
    {
        _stream = stream;
        _leaveOpen = leaveOpen;
    }

    /// <summary>
    /// Loads a PSD image from a file path.
    /// </summary>
    /// <param name="filePath">The path to the PSD file.</param>
    /// <returns>A loaded <see cref="PsdImage"/> instance.</returns>
    public static PsdImage Load(string filePath)
    {
        if (filePath == null) throw new ArgumentNullException(nameof(filePath));
        if (!File.Exists(filePath)) throw new FileNotFoundException($"File not found: {filePath}");

        using var stream = File.OpenRead(filePath);
        return Load(stream, leaveOpen: false);
    }

    /// <summary>
    /// Loads a PSD image from a readable stream.
    /// </summary>
    /// <param name="stream">The input stream containing PSD data.</param>
    /// <returns>A loaded <see cref="PsdImage"/> instance.</returns>
    public static PsdImage Load(Stream stream)
    {
        if (stream == null) throw new ArgumentNullException(nameof(stream));
        return Load(stream, leaveOpen: true);
    }

    private static PsdImage Load(Stream stream, bool leaveOpen)
    {
        var image = new PsdImage(stream, leaveOpen);
        image.LoadInternal();
        return image;
    }

    private void LoadInternal()
    {
        if (_header != null) return;

        var reader = new BigEndianReader(_stream, _leaveOpen);
        try
        {
            _header = PsdHeader.Load(reader);

            LoadColorData(reader);
            LoadResources(reader);
            LoadLayerAndMaskInfo(reader);
            LoadImageData(reader);
        }
        finally
        {
            reader.Dispose();
        }
    }

    private void LoadColorData(BigEndianReader reader)
    {
        uint length = reader.ReadUInt32();
        if (length > 0)
        {
            _colorData = reader.ReadBytes((int)length);
        }
    }

    private void LoadResources(BigEndianReader reader)
    {
        uint resourcesLength = reader.ReadUInt32();
        if (resourcesLength == 0) return;

        long resourcesEnd = reader.Position + (int)resourcesLength;

        var resourcesList = new List<ResourceBlock>();

        while (reader.Position < resourcesEnd)
        {
            long startPos = reader.Position;
            if (startPos + 8 > resourcesEnd)
            {
                break;
            }
            
            uint signature = reader.ReadUInt32();
            if (signature != 0x3842494D)
            {
                break;
            }

            short resourceId = reader.ReadInt16();

            byte nameLength = reader.ReadByte();
            
            // Validate name length is reasonable
            if (nameLength > 255)
            {
                break;
            }
            
            // Read name with odd padding (not 4-byte padding!)
            string resourceName = string.Empty;
            if (nameLength > 0)
            {
                byte[] nameBytes = reader.ReadBytes(nameLength);
                resourceName = System.Text.Encoding.ASCII.GetString(nameBytes);
            }
            
            // Odd padding: if (nameLength + 1) % 2 != 0, add 1 byte padding
            if ((nameLength + 1) % 2 != 0)
            {
                reader.Skip(1);
            }

            int dataLength = reader.ReadInt32();
            
            // Validate data length
            if (dataLength < 0 || dataLength > 10000000)
            {
                break;
            }
            
            if (reader.Position + dataLength > resourcesEnd)
            {
                break;
            }
            
            byte[] data = reader.ReadBytes(dataLength);

            if (dataLength % 2 == 1)
            {
                reader.Skip(1);
            }

            resourcesList.Add(new ResourceBlock
            {
                ResourceId = resourceId,
                Name = resourceName,
                Data = data
            });
        }

        _resources = resourcesList.ToArray();
    }

    private void LoadLayerAndMaskInfo(BigEndianReader reader)
    {
        uint sectionLength = reader.ReadUInt32();
        if (sectionLength == 0)
        {
            _layerAndMaskInfoRaw = [];
            _layerChannelImageDataRaw = [];
            _layerGlobalMaskAndTailRaw = [];
            return;
        }

        byte[] rawSectionBytes = reader.ReadBytes((int)sectionLength);

        var memReader = new BigEndianReader(new MemoryStream(rawSectionBytes, writable: false), leaveOpen: true);

        int layerInfoLength = memReader.ReadInt32();
        _layerCountRaw = memReader.ReadInt16();
        short layerCount = _layerCountRaw < 0 ? (short)-_layerCountRaw : _layerCountRaw;

        if (layerCount > 0)
        {
            var layers = new Layer[layerCount];
            for (int i = 0; i < layerCount; i++)
            {
                layers[i] = Layer.Load(memReader, Channels);
            }
            _layers = layers;
        }

        int channelImageDataLength = Math.Max(0, layerInfoLength - (int)memReader.Position);
        _layerChannelImageDataRaw = channelImageDataLength > 0
            ? memReader.ReadBytes(channelImageDataLength)
            : [];

        int globalMaskAndTailLength = Math.Max(0, rawSectionBytes.Length - (int)memReader.Position);
        _layerGlobalMaskAndTailRaw = globalMaskAndTailLength > 0
            ? memReader.ReadBytes(globalMaskAndTailLength)
            : [];

        _layerAndMaskInfoRaw = rawSectionBytes;
    }

    private void LoadImageData(BigEndianReader reader)
    {
        _imageDataCompression = reader.ReadUInt16();

        long imageDataStart = reader.Position;
        reader.Seek(0, SeekOrigin.End);
        long imageDataEnd = reader.Position;
        reader.Seek(imageDataStart, SeekOrigin.Begin);

        int imageDataLength = (int)(imageDataEnd - imageDataStart);
        _imageData = reader.ReadBytes(imageDataLength);
    }

    /// <summary>
    /// Saves the image to a file path.
    /// </summary>
    /// <param name="filePath">The destination file path.</param>
    public void Save(string filePath)
    {
        if (filePath == null) throw new ArgumentNullException(nameof(filePath));

        using var stream = File.Create(filePath);
        Save(stream, leaveOpen: false);
    }

    /// <summary>
    /// Saves the image to a writable stream.
    /// </summary>
    /// <param name="stream">The destination stream.</param>
    public void Save(Stream stream)
    {
        if (stream == null) throw new ArgumentNullException(nameof(stream));
        Save(stream, leaveOpen: true);
    }

    private void Save(Stream stream, bool leaveOpen)
    {
        if (_disposed) throw new ObjectDisposedException(nameof(PsdImage));

        var writer = new BigEndianWriter(stream, leaveOpen);
        try
        {
            WriteSignature(writer);
            WriteHeader(writer);
            WriteColorData(writer);
            WriteResources(writer);
            WriteLayerAndMaskInfo(writer);
            WriteImageData(writer);
        }
        finally
        {
            writer.Dispose();
        }
    }

    private void WriteSignature(BigEndianWriter writer)
    {
        writer.Write((uint)0x38425053);
    }

    private void WriteHeader(BigEndianWriter writer)
    {
        writer.Write((ushort)Version);
        writer.Write(new byte[6]);
        writer.Write((ushort)Channels);
        writer.Write(Height);
        writer.Write(Width);
        writer.Write((ushort)BitsPerChannel);
        writer.Write((ushort)ColorMode);
    }

    private void WriteColorData(BigEndianWriter writer)
    {
        writer.Write((uint)_colorData.Length);
        if (_colorData.Length > 0)
        {
            writer.Write(_colorData);
        }
    }

    private void WriteResources(BigEndianWriter writer)
    {
        if (_resources.Length == 0)
        {
            writer.Write((uint)0);
            return;
        }

        long resourcesStart = writer.Position;
        writer.Write((uint)0);

        foreach (var resource in _resources)
        {
            writer.Write((uint)0x3842494D);
            writer.Write((short)resource.ResourceId);

            byte[] nameBytes = System.Text.Encoding.ASCII.GetBytes(resource.Name);
            writer.Write((byte)nameBytes.Length);
            writer.Write(nameBytes);

            // Odd padding: if (nameLength + 1) % 2 != 0, add 1 byte padding
            if ((nameBytes.Length + 1) % 2 != 0)
            {
                writer.Write((byte)0);
            }

            writer.Write((int)resource.Data.Length);
            writer.Write(resource.Data);

            if (resource.Data.Length % 2 == 1)
            {
                writer.Write((byte)0);
            }
        }

        long resourcesEnd = writer.Position;
        writer.Seek(resourcesStart, SeekOrigin.Begin);
        writer.Write((int)(resourcesEnd - resourcesStart - 4));
        writer.Seek(resourcesEnd, SeekOrigin.Begin);
    }

    private void WriteLayerAndMaskInfo(BigEndianWriter writer)
    {
        if (_layerAndMaskInfoRaw.Length > 0 && (_layers == null || !_layers.Any(l => l.HasMutated)))
        {
            writer.Write((uint)_layerAndMaskInfoRaw.Length);
            writer.Write(_layerAndMaskInfoRaw);
        }
        else if (_layers != null && _layers.Length > 0)
        {
            WriteLayerSectionWithMutations(writer);
        }
        else
        {
            writer.Write((uint)0);
        }
    }

    private void WriteLayerSectionWithMutations(BigEndianWriter writer)
    {
        using var layerInfoPayloadStream = new MemoryStream();
        using var layerInfoPayloadWriter = new BigEndianWriter(layerInfoPayloadStream, leaveOpen: true);

        Layer[] layers = _layers ?? [];
        short layerCount = _layerCountRaw < 0 ? (short)-layers.Length : (short)layers.Length;
        layerInfoPayloadWriter.Write(layerCount);

        foreach (var layer in layers)
        {
            layer.Write(layerInfoPayloadWriter);
        }

        layerInfoPayloadWriter.Write(_layerChannelImageDataRaw);
        byte[] layerInfoPayload = layerInfoPayloadStream.ToArray();

        using var sectionStream = new MemoryStream();
        using var sectionWriter = new BigEndianWriter(sectionStream, leaveOpen: true);
        sectionWriter.Write(layerInfoPayload.Length);
        sectionWriter.Write(layerInfoPayload);
        sectionWriter.Write(_layerGlobalMaskAndTailRaw);

        byte[] sectionBytes = sectionStream.ToArray();
        writer.Write((uint)sectionBytes.Length);
        writer.Write(sectionBytes);
    }

    private void WriteImageData(BigEndianWriter writer)
    {
        writer.Write((ushort)_imageDataCompression);
        writer.Write(_imageData);
    }

    /// <summary>
    /// Releases the image and optionally the underlying stream.
    /// </summary>
    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        if (!_leaveOpen)
        {
            _stream.Dispose();
        }
    }

    /// <summary>
    /// Represents a parsed image resource block.
    /// </summary>
    private struct ResourceBlock
    {
        /// <summary>
        /// Gets or sets the PSD resource identifier.
        /// </summary>
        public short ResourceId;

        /// <summary>
        /// Gets or sets the resource Pascal name converted to text.
        /// </summary>
        public string Name;

        /// <summary>
        /// Gets or sets the raw resource payload bytes.
        /// </summary>
        public byte[] Data;
    }
}
