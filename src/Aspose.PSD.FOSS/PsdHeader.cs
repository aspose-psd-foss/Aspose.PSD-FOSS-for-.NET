namespace Aspose.PSD.FOSS;

/// <summary>
/// Contains the header information of a Photoshop document.
/// </summary>
public sealed class PsdHeader
{
    /// <summary>
    /// PSD file signature value (0x38425053 = '8BPS').
    /// </summary>
    public const int PsdSignature = 0x38425053;
    /// <summary>
    /// PSB (Large Document Format) file signature value (0x3842494D = '8BIM').
    /// </summary>
    public const int PsbSignature = 0x3842494D;

    /// <summary>
    /// Gets the width of the image in pixels.
    /// </summary>
    public int Width { get; private set; }
    /// <summary>
    /// Gets the height of the image in pixels.
    /// </summary>
    public int Height { get; private set; }
    /// <summary>
    /// Gets the number of channels in the image (e.g., 3 for RGB).
    /// </summary>
    public int Channels { get; private set; }
    /// <summary>
    /// Gets the bit depth per channel (e.g., 8, 16, 32).
    /// </summary>
    public int BitDepth { get; private set; }
    /// <summary>
    /// Gets the color mode of the image.
    /// </summary>
    public ColorModes ColorMode { get; private set; }
    /// <summary>
    /// Gets the version of the PSD file format.
    /// </summary>
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
