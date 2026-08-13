namespace Aspose.PSD.FOSS;

public sealed class PsdHeader
{
    public const int PsdSignature = 0x38425053;
    public const int PsbSignature = 0x3842494D;

    public int Width { get; private set; }
    public int Height { get; private set; }
    public int Channels { get; private set; }
    public int BitDepth { get; private set; }
    public ColorModes ColorMode { get; private set; }
    public int Version { get; private set; }

    internal static PsdHeader Load(BigEndianReader reader)
    {
        uint signature = reader.ReadUInt32();
        if (signature != PsdSignature && signature != PsbSignature)
        {
            throw new PsdLoadException("Invalid PSD signature. Expected '8BPS' (0x38425053).");
        }

        int version = reader.ReadUInt16();
        if (version < 1 || version > 6)
        {
            throw new PsdLoadException($"Unsupported PSD version: {version}. Supported versions: 1-6 (PSB > 6).");
        }

        reader.Skip(6);

        int channels = reader.ReadUInt16();
        int height = reader.ReadInt32();
        int width = reader.ReadInt32();
        int bitDepth = reader.ReadUInt16();
        ColorModes colorMode = (ColorModes)reader.ReadUInt16();

        return new PsdHeader
        {
            Width = width,
            Height = height,
            Channels = channels,
            BitDepth = bitDepth,
            ColorMode = colorMode,
            Version = version
        };
    }
}
