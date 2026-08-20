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
    /// Gets the parsed PSD/PSB header object.
    /// </summary>
    public PsdHeader Header => _header ?? throw new InvalidOperationException("PSD/PSB header is not loaded.");

    /// <summary>
    /// Gets a value indicating whether the loaded document is a PSB large document.
    /// </summary>
    public bool IsLargeDocument => _header?.IsLargeDocument == true;

    /// <summary>
    /// Gets a value indicating whether the loaded document is a PSB file.
    /// </summary>
    public bool IsPsb => IsLargeDocument;

    /// <summary>
    /// Gets the parsed layer collection.
    /// </summary>
    public Layer[] Layers => _layers?.ToArray() ?? [];

    /// <summary>
    /// Gets the number of parsed layers in the document.
    /// </summary>
    public int LayerCount => Layers.Length;

    /// <summary>
    /// Gets a value indicating whether the document contains at least one parsed layer.
    /// </summary>
    public bool HasLayers => _layers?.Length > 0;

    /// <summary>
    /// Gets a value indicating whether the document contains any parsed image resources.
    /// </summary>
    public bool HasImageResources => _resourcesRaw.Length > 0 || _resources.Length > 0;

    /// <summary>
    /// Gets the number of parsed image resource blocks.
    /// </summary>
    public int ResourceCount => _resources.Length;

    /// <summary>
    /// Gets a read-only summary of the parsed image resource blocks.
    /// </summary>
    public IReadOnlyList<PsdResourceInfo> Resources => _resources.Select(resource => resource.ToPublicInfo()).ToArray();

    /// <summary>
    /// Gets a value indicating whether the document contains Color Mode Data bytes.
    /// </summary>
    public bool HasColorModeData => _colorData.RawData.Length > 0;

    /// <summary>
    /// Gets a read-only summary of the parsed Color Mode Data section.
    /// </summary>
    public PsdColorDataInfo ColorDataInfo => _colorData.ToPublicInfo();

    /// <summary>
    /// Gets the parsed indexed palette summary, when the Color Mode Data section contains one.
    /// </summary>
    public IndexedColorPaletteInfo? IndexedPalette => ColorDataInfo.IndexedPalette;

    /// <summary>
    /// Gets a value indicating whether the document contains merged image data payload bytes.
    /// </summary>
    public bool HasMergedImageData => _imageData.RawData.Length > 0;

    /// <summary>
    /// Gets the compression method used by the merged image data section.
    /// </summary>
    public CompressionMethod Compression => _imageData.Compression;

    /// <summary>
    /// Gets a read-only summary of the parsed merged image data structure.
    /// </summary>
    public PsdImageDataInfo ImageDataInfo => _imageData.ToPublicInfo();

    /// <summary>
    /// Gets the structural kind of the merged image data payload.
    /// </summary>
    public ImageDataKind ImageDataKind => _imageData.Structure.Kind;

    /// <summary>
    /// Gets a value indicating whether ZIP prediction is used by the merged image data payload.
    /// </summary>
    public bool UsesPrediction => _imageData.Structure.UsesPrediction;

    /// <summary>
    /// Gets the parsed global angle from the image resources, when present.
    /// The current lightweight unknown-only parser does not reconstruct ID-specific semantic values.
    /// </summary>
    public int? GlobalAngle => null;

    /// <summary>
    /// Gets a value indicating whether an embedded ICC profile resource is present.
    /// The current lightweight unknown-only parser does not reconstruct ID-specific semantic values.
    /// </summary>
    public bool HasIccProfile => false;

    /// <summary>
    /// Gets the parsed untagged ICC profile flag, when the corresponding resource is present.
    /// The current lightweight unknown-only parser does not reconstruct ID-specific semantic values.
    /// </summary>
    public bool? IsIccProfileUntagged => null;

    /// <summary>
    /// Gets the parsed color mode data details for internal verification and tests.
    /// </summary>
    internal ColorData ParsedColorData => _colorData;

    /// <summary>
    /// Gets the parsed image resources for internal verification and tests.
    /// </summary>
    internal UnknownResource[] ParsedResources => _resources;

    /// <summary>
    /// Stores the parsed PSD header.
    /// </summary>
    private PsdHeader? _header;

    /// <summary>
    /// Stores the parsed layer records.
    /// </summary>
    private Layer[]? _layers;

    /// <summary>
    /// Stores the parsed Color Mode Data section.
    /// </summary>
    private ColorData _colorData = ColorData.Empty;

    /// <summary>
    /// Stores parsed image resource blocks.
    /// </summary>
    private UnknownResource[] _resources = [];

    /// <summary>
    /// Stores the raw Image Resources section payload.
    /// </summary>
    private byte[] _resourcesRaw = [];

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
    /// Stores the parsed Image Data section.
    /// </summary>
    private ImageData _imageData = new(CompressionMethod.Raw, []);

    /// <summary>
    /// Initializes a new instance of the <see cref="PsdImage"/> class over an internal working stream.
    /// </summary>
    /// <param name="stream">The buffered stream that stores PSD/PSB bytes for this instance.</param>
    /// <param name="leaveOpen">true to leave the stream open after disposal; otherwise, false.</param>
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
        return Load(stream);
    }

    /// <summary>
    /// Loads a PSD image from a readable stream.
    /// </summary>
    /// <param name="stream">The input stream containing PSD data.</param>
    /// <returns>A loaded <see cref="PsdImage"/> instance.</returns>
    public static PsdImage Load(Stream stream)
    {
        if (stream == null) throw new ArgumentNullException(nameof(stream));

        long originalPosition = 0;
        bool restorePosition = stream.CanSeek;
        if (restorePosition)
        {
            originalPosition = stream.Position;
        }

        try
        {
            var bufferedStream = new MemoryStream();
            stream.CopyTo(bufferedStream);
            bufferedStream.Position = 0;
            return Load(bufferedStream, leaveOpen: false);
        }
        finally
        {
            if (restorePosition)
            {
                stream.Position = originalPosition;
            }
        }
    }

    /// <summary>
    /// Creates a loaded image instance over the provided stream.
    /// </summary>
    /// <param name="stream">The buffered PSD/PSB stream.</param>
    /// <param name="leaveOpen">true to leave the stream open after disposal; otherwise, false.</param>
    /// <returns>The loaded <see cref="PsdImage"/> instance.</returns>
    private static PsdImage Load(Stream stream, bool leaveOpen)
    {
        var image = new PsdImage(stream, leaveOpen);
        image.LoadInternal();
        return image;
    }

    /// <summary>
    /// Loads all supported PSD/PSB sections into memory.
    /// </summary>
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
        catch (PsdLoadException)
        {
            throw;
        }
        catch (EndOfStreamException exception)
        {
            throw new PsdLoadException("Unexpected end of PSD/PSB data while reading the file structure.", exception);
        }
        catch (IOException exception)
        {
            throw new PsdLoadException("Failed to read PSD/PSB data from the source stream.", exception);
        }
        finally
        {
            reader.Dispose();
        }
    }

    /// <summary>
    /// Loads the raw Color Mode Data section.
    /// </summary>
    /// <param name="reader">The reader positioned at the section length field.</param>
    private void LoadColorData(BigEndianReader reader)
    {
        _colorData = ColorData.Load(reader, ColorMode);
    }

    /// <summary>
    /// Loads the Image Resources section and preserves its raw payload for round-trip saves.
    /// </summary>
    /// <param name="reader">The reader positioned at the section length field.</param>
    private void LoadResources(BigEndianReader reader)
    {
        uint resourcesLength = reader.ReadUInt32();
        if (resourcesLength == 0) return;

        _resourcesRaw = PsdSectionReader.ReadBytes(reader, resourcesLength, "Image Resources section");
        long resourcesEnd = _resourcesRaw.Length;
        var resourcesReader = new BigEndianReader(new MemoryStream(_resourcesRaw, writable: false), leaveOpen: true);

        var resourcesList = new List<UnknownResource>();

        while (resourcesReader.Position < resourcesEnd)
        {
            UnknownResource? resource = UnknownResource.Load(resourcesReader, resourcesEnd);
            if (resource == null)
            {
                break;
            }

            resourcesList.Add(resource);
        }

        _resources = resourcesList.ToArray();
    }

    /// <summary>
    /// Loads the Layer and Mask Information section and splits it into parsed and raw-preserved parts.
    /// </summary>
    /// <param name="reader">The reader positioned at the section length field.</param>
    private void LoadLayerAndMaskInfo(BigEndianReader reader)
    {
        ulong sectionLength = ReadLayerAndMaskSectionLength(reader);
        if (sectionLength == 0)
        {
            _layerAndMaskInfoRaw = [];
            _layerChannelImageDataRaw = [];
            _layerGlobalMaskAndTailRaw = [];
            return;
        }

        byte[] rawSectionBytes = PsdSectionReader.ReadBytes(reader, sectionLength, "Layer and Mask Information section");

        var memReader = new BigEndianReader(new MemoryStream(rawSectionBytes, writable: false), leaveOpen: true);

        long layerInfoLength = PsdSectionReader.ValidateSignedLength(
            _header?.IsLargeDocument == true ? memReader.ReadInt64() : memReader.ReadInt32(),
            "Layer Info section");
        if (layerInfoLength > rawSectionBytes.Length - memReader.Position)
        {
            throw new PsdLoadException("Layer Info section length exceeds the enclosing Layer and Mask Information section.");
        }

        long layerInfoEnd = (_header?.IsLargeDocument == true ? sizeof(long) : sizeof(int)) + layerInfoLength;
        if (layerInfoLength == 0)
        {
            _layerChannelImageDataRaw = [];
            _layerGlobalMaskAndTailRaw = rawSectionBytes.Length > memReader.Position
                ? memReader.ReadBytes(checked((int)(rawSectionBytes.Length - memReader.Position)))
                : [];
            _layerAndMaskInfoRaw = rawSectionBytes;
            return;
        }

        if (layerInfoLength < sizeof(short))
        {
            throw new PsdLoadException("Layer Info section is too short to contain the layer count field.");
        }

        _layerCountRaw = memReader.ReadInt16();
        short layerCount = _layerCountRaw < 0 ? (short)-_layerCountRaw : _layerCountRaw;
        if (layerCount > 0)
        {
            var layers = new Layer[layerCount];
            for (int i = 0; i < layerCount; i++)
            {
                layers[i] = Layer.Load(memReader, _header?.IsLargeDocument == true);
            }
            _layers = layers;
        }

        if (memReader.Position > layerInfoEnd)
        {
            throw new PsdLoadException("Layer records exceed the declared Layer Info section length.");
        }

        int channelImageDataLength = checked((int)(layerInfoEnd - memReader.Position));
        _layerChannelImageDataRaw = channelImageDataLength > 0
            ? memReader.ReadBytes(channelImageDataLength)
            : [];

        int globalMaskAndTailLength = Math.Max(0, rawSectionBytes.Length - (int)memReader.Position);
        _layerGlobalMaskAndTailRaw = globalMaskAndTailLength > 0
            ? memReader.ReadBytes(globalMaskAndTailLength)
            : [];

        _layerAndMaskInfoRaw = rawSectionBytes;
    }

    /// <summary>
    /// Loads the final Image Data section as raw bytes after reading the compression field.
    /// </summary>
    /// <param name="reader">The reader positioned at the image data compression field.</param>
    private void LoadImageData(BigEndianReader reader)
    {
        _imageData = ImageData.Load(
            reader,
            _header?.IsLargeDocument == true,
            Height,
            Channels);
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

    /// <summary>
    /// Saves the current document to a stream with configurable stream ownership.
    /// </summary>
    /// <param name="stream">The destination stream.</param>
    /// <param name="leaveOpen">true to leave the stream open after saving; otherwise, false.</param>
    private void Save(Stream stream, bool leaveOpen)
    {
        if (_disposed) throw new ObjectDisposedException(nameof(PsdImage));

        var writer = new PsdWriter(stream, leaveOpen);
        try
        {
            writer.WriteSignature();
            WriteHeader(writer.Writer);
            WriteColorData(writer.Writer);
            WriteResources(writer.Writer);
            WriteLayerAndMaskInfo(writer.Writer);
            WriteImageData(writer.Writer);
        }
        finally
        {
            writer.Dispose();
        }
    }

    /// <summary>
    /// Writes the fixed PSD/PSB header block.
    /// </summary>
    /// <param name="writer">The destination writer.</param>
    private void WriteHeader(BigEndianWriter writer)
    {
        _header?.Save(writer);
    }

    /// <summary>
    /// Writes the Color Mode Data section.
    /// </summary>
    /// <param name="writer">The destination writer.</param>
    private void WriteColorData(BigEndianWriter writer)
    {
        _colorData.Save(writer);
    }

    /// <summary>
    /// Writes the Image Resources section using either raw-preserved data or parsed resource blocks.
    /// </summary>
    /// <param name="writer">The destination writer.</param>
    private void WriteResources(BigEndianWriter writer)
    {
        if (_resourcesRaw.Length > 0)
        {
            writer.Write((uint)_resourcesRaw.Length);
            writer.Write(_resourcesRaw);
            return;
        }

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

    /// <summary>
    /// Writes the Layer and Mask Information section.
    /// </summary>
    /// <param name="writer">The destination writer.</param>
    private void WriteLayerAndMaskInfo(BigEndianWriter writer)
    {
        if (_layerAndMaskInfoRaw.Length > 0 && (_layers == null || !_layers.Any(l => l.HasMutated)))
        {
            WriteLayerAndMaskSectionLength(writer, _layerAndMaskInfoRaw.Length);
            writer.Write(_layerAndMaskInfoRaw);
        }
        else if (_layers != null && _layers.Length > 0)
        {
            WriteLayerSectionWithMutations(writer);
        }
        else
        {
            WriteLayerAndMaskSectionLength(writer, 0);
        }
    }

    /// <summary>
    /// Rebuilds the minimal mutable part of the Layer and Mask Information section.
    /// </summary>
    /// <param name="writer">The destination writer.</param>
    private void WriteLayerSectionWithMutations(BigEndianWriter writer)
    {
        using var layerInfoPayloadStream = new MemoryStream();
        using var layerInfoPayloadWriter = new BigEndianWriter(layerInfoPayloadStream, leaveOpen: true);

        Layer[] layers = _layers ?? [];
        short layerCount = _layerCountRaw < 0 ? (short)-layers.Length : (short)layers.Length;
        layerInfoPayloadWriter.Write(layerCount);

        foreach (var layer in layers)
        {
            layer.Write(layerInfoPayloadWriter, _header?.IsLargeDocument == true);
        }

        layerInfoPayloadWriter.Write(_layerChannelImageDataRaw);
        byte[] layerInfoPayload = layerInfoPayloadStream.ToArray();

        using var sectionStream = new MemoryStream();
        using var sectionWriter = new BigEndianWriter(sectionStream, leaveOpen: true);
        if (_header?.IsLargeDocument == true)
        {
            sectionWriter.Write((long)layerInfoPayload.Length);
        }
        else
        {
            sectionWriter.Write(layerInfoPayload.Length);
        }
        sectionWriter.Write(layerInfoPayload);
        sectionWriter.Write(GetLayerGlobalMaskAndTailBytesForWrite());

        byte[] sectionBytes = sectionStream.ToArray();
        WriteLayerAndMaskSectionLength(writer, sectionBytes.Length);
        writer.Write(sectionBytes);
    }

    /// <summary>
    /// Returns the raw global layer mask and trailing bytes, synthesizing an empty global mask block when absent.
    /// </summary>
    /// <returns>The bytes to append after layer info inside the Layer and Mask section.</returns>
    private byte[] GetLayerGlobalMaskAndTailBytesForWrite()
    {
        if (_layerGlobalMaskAndTailRaw.Length > 0)
        {
            return _layerGlobalMaskAndTailRaw;
        }

        return [0, 0, 0, 0];
    }

    /// <summary>
    /// Writes the final Image Data section.
    /// </summary>
    /// <param name="writer">The destination writer.</param>
    private void WriteImageData(BigEndianWriter writer)
    {
        _imageData.Save(writer);
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
    /// Reads the outer Layer and Mask section length using PSD- or PSB-sized integers.
    /// </summary>
    /// <param name="reader">The reader positioned at the section length field.</param>
    /// <returns>The declared section length in bytes.</returns>
    private ulong ReadLayerAndMaskSectionLength(BigEndianReader reader)
    {
        return _header?.IsLargeDocument == true ? reader.ReadUInt64() : reader.ReadUInt32();
    }

    /// <summary>
    /// Writes the outer Layer and Mask section length using PSD- or PSB-sized integers.
    /// </summary>
    /// <param name="writer">The writer positioned at the section length field.</param>
    /// <param name="length">The section length in bytes.</param>
    private void WriteLayerAndMaskSectionLength(BigEndianWriter writer, int length)
    {
        if (_header?.IsLargeDocument == true)
        {
            writer.Write((ulong)length);
        }
        else
        {
            writer.Write((uint)length);
        }
    }
}
