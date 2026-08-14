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
    public int Version => (int)FormatVersion;

    /// <summary>
    /// Gets the strongly typed PSD container version.
    /// </summary>
    internal PsdVersion FormatVersion { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the document is a PSB large document.
    /// </summary>
    public bool IsLargeDocument => FormatVersion == global::Aspose.PSD.FOSS.PsdVersion.Psb;

    /// <summary>
    /// Loads the fixed PSD/PSB file header from the reader.
    /// </summary>
    /// <param name="reader">The reader positioned at the start of the file header.</param>
    /// <returns>The parsed <see cref="PsdHeader"/> instance.</returns>
    internal static PsdHeader Load(BigEndianReader reader)
    {
        uint signature = reader.ReadUInt32();
        if (signature != PsdSignature)
        {
            throw new PsdLoadException("Invalid PSD signature. Expected '8BPS' (0x38425053).");
        }

        ushort rawVersion = reader.ReadUInt16();
        if (rawVersion != (ushort)global::Aspose.PSD.FOSS.PsdVersion.Psd && rawVersion != (ushort)global::Aspose.PSD.FOSS.PsdVersion.Psb)
        {
            throw new PsdLoadException($"Unsupported PSD version: {rawVersion}. Supported versions: {PsdVersion} (PSD) and {PsbVersion} (PSB).");
        }

        global::Aspose.PSD.FOSS.PsdVersion version = (global::Aspose.PSD.FOSS.PsdVersion)rawVersion;

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
            FormatVersion = version
        };
    }

    /// <summary>
    /// Writes the fixed PSD/PSB file header to the writer.
    /// </summary>
    /// <param name="writer">The destination writer.</param>
    internal void Save(BigEndianWriter writer)
    {
        writer.Write((ushort)FormatVersion);
        writer.Write(new byte[6]);
        writer.Write((ushort)Channels);
        writer.Write(Height);
        writer.Write(Width);
        writer.Write((ushort)BitDepth);
        writer.Write((ushort)ColorMode);
    }
}
