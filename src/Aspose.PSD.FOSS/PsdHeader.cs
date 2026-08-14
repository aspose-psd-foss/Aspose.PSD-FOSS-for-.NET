namespace Aspose.PSD.FOSS;

/// <summary>
/// Contains the header information of a Photoshop document.
/// </summary>
public sealed class PsdHeader
{
    /// <summary>
    /// Shared PSD/PSB file signature value (0x38425053 = '8BPS').
    /// </summary>
    public const int PsdSignature = 0x38425053;

    /// <summary>
    /// Shared PSD/PSB file signature value (0x38425053 = '8BPS').
    /// </summary>
    public const int PsbSignature = PsdSignature;

    /// <summary>
    /// PSD version value.
    /// </summary>
    public const ushort PsdVersion = 1;

    /// <summary>
    /// PSB large-document version value.
    /// </summary>
    public const ushort PsbVersion = 2;

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

    /// <summary>
    /// Gets a value indicating whether the document is a PSB large document.
    /// </summary>
    public bool IsLargeDocument => Version == PsbVersion;

    internal static PsdHeader Load(BigEndianReader reader)
    {
        uint signature = reader.ReadUInt32();
        if (signature != PsdSignature)
        {
            throw new PsdLoadException("Invalid PSD signature. Expected '8BPS' (0x38425053).");
        }

        int version = reader.ReadUInt16();
        if (version != PsdVersion && version != PsbVersion)
        {
            throw new PsdLoadException($"Unsupported PSD version: {version}. Supported versions: {PsdVersion} (PSD) and {PsbVersion} (PSB).");
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
