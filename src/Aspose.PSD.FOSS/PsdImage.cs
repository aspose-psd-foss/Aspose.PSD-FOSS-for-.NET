using System.Drawing;
using System.IO;

namespace Aspose.PSD.FOSS;

public sealed class PsdImage : IDisposable
{
    private readonly Stream _stream;
    private readonly bool _leaveOpen;
    private bool _disposed;

    public int Width => _header?.Width ?? 0;
    public int Height => _header?.Height ?? 0;
    public int Channels => _header?.Channels ?? 0;
    public int BitsPerChannel => _header?.BitDepth ?? 0;
    public ColorModes ColorMode => _header?.ColorMode ?? ColorModes.Rgb;
    public int Version => _header?.Version ?? 1;
    public Layer[] Layers => _layers ?? [];
    public bool HasLayers => _layers?.Length > 0;

    private PsdHeader? _header;
    private Layer[]? _layers;
    private byte[] _colorData = [];
    private ResourceBlock[] _resources = [];
    private byte[] _layerAndMaskInfoRaw = [];
    private byte[] _imageData = [];
    private int _imageDataCompression;

    private PsdImage(Stream stream, bool leaveOpen)
    {
        _stream = stream;
        _leaveOpen = leaveOpen;
    }

    public static PsdImage Load(string filePath)
    {
        if (filePath == null) throw new ArgumentNullException(nameof(filePath));
        if (!File.Exists(filePath)) throw new FileNotFoundException($"File not found: {filePath}");

        using var stream = File.OpenRead(filePath);
        return Load(stream, leaveOpen: false);
    }

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
            return;
        }

        long sectionEnd = reader.Position + (int)sectionLength;
        byte[] rawSectionBytes = reader.ReadBytes((int)sectionLength);
        
        var memReader = new BigEndianReader(new System.IO.MemoryStream(rawSectionBytes), leaveOpen: true);
        
        int layerInfoLength = memReader.ReadInt32();
        short layerCount = memReader.ReadInt16();
        if (layerCount < 0) layerCount = (short)-layerCount;

        if (layerCount > 0)
        {
            var layers = new Layer[layerCount];
            for (int i = 0; i < layerCount; i++)
            {
                layers[i] = Layer.Load(memReader, Channels);
            }
            _layers = layers;
        }

        long layerInfoParsed = (int)memReader.Position;
        int layerInfoSize = layerInfoLength - 4 - 2;
        long remainingLayerInfo = layerInfoSize - layerInfoParsed;

        memReader.Seek(remainingLayerInfo, SeekOrigin.Current);

        long globalMaskEnd = memReader.Position;
        int remainingInRaw = (int)sectionLength - (int)globalMaskEnd;
        if (remainingInRaw > 0)
        {
            memReader.ReadBytes(remainingInRaw);
        }

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

    public void Save(string filePath)
    {
        if (filePath == null) throw new ArgumentNullException(nameof(filePath));

        using var stream = File.Create(filePath);
        Save(stream, leaveOpen: false);
    }

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
        if (_layerAndMaskInfoRaw.Length > 0)
        {
            writer.Write((uint)_layerAndMaskInfoRaw.Length);
            writer.Write(_layerAndMaskInfoRaw);
        }
        else
        {
            writer.Write((uint)0);
        }
    }

    private void WriteImageData(BigEndianWriter writer)
    {
        writer.Write((ushort)_imageDataCompression);
        writer.Write(_imageData);
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        if (!_leaveOpen)
        {
            _stream.Dispose();
        }
    }

    private struct ResourceBlock
    {
        public short ResourceId;
        public string Name;
        public byte[] Data;
    }
}
