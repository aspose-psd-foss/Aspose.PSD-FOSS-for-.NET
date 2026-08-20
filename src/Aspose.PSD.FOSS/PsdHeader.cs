namespace Aspose.PSD.FileFormats.Psd;

/// <summary>
/// Contains the header information of a Photoshop document.
/// </summary>
internal sealed class PsdHeader
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
    /// Gets the raw PSD container version: 1 for PSD and 2 for PSB.
    /// </summary>
    public int Version => (int)FormatVersion;

    /// <summary>
    /// Gets the strongly typed PSD container version.
    /// </summary>
    internal PsdVersion FormatVersion { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the document uses the PSB large-document container.
    /// </summary>
    public bool IsLargeDocument => FormatVersion == Psd.PsdVersion.Psb;

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
        if (rawVersion != (ushort)Psd.PsdVersion.Psd && rawVersion != (ushort)Psd.PsdVersion.Psb)
        {
            throw new PsdLoadException($"Unsupported PSD version: {rawVersion}. Supported versions: {PsdVersion} (PSD) and {PsbVersion} (PSB).");
        }

        global::Aspose.PSD.FileFormats.Psd.PsdVersion version = (global::Aspose.PSD.FileFormats.Psd.PsdVersion)rawVersion;

        byte[] reserved = reader.ReadBytes(6);
        if (reserved.Any(value => value != 0))
        {
            throw new PsdLoadException("PSD header reserved bytes must be zero.");
        }

        int channels = reader.ReadUInt16();
        int height = reader.ReadInt32();
        int width = reader.ReadInt32();
        int bitDepth = reader.ReadUInt16();
        ColorModes colorMode = (ColorModes)reader.ReadUInt16();

        ValidateHeaderFields(version, channels, height, width, bitDepth, colorMode);

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
    /// Validates PSD/PSB header invariants before exposing the parsed document state.
    /// </summary>
    /// <param name="version">The parsed PSD container version.</param>
    /// <param name="channels">The declared channel count.</param>
    /// <param name="height">The declared document height.</param>
    /// <param name="width">The declared document width.</param>
    /// <param name="bitDepth">The declared bits per channel.</param>
    /// <param name="colorMode">The declared PSD color mode.</param>
    /// <exception cref="PsdLoadException">Thrown when a header field is outside the supported PSD/PSB range.</exception>
    private static void ValidateHeaderFields(
        PsdVersion version,
        int channels,
        int height,
        int width,
        int bitDepth,
        ColorModes colorMode)
    {
        if (channels is < 1 or > 56)
        {
            throw new PsdLoadException($"PSD header channel count {channels} is outside the supported range 1-56.");
        }

        int maxDimension = version == global::Aspose.PSD.FileFormats.Psd.PsdVersion.Psb ? 300000 : 30000;
        if (height < 1 || height > maxDimension)
        {
            throw new PsdLoadException($"PSD header height {height} is outside the supported range 1-{maxDimension} for this document version.");
        }

        if (width < 1 || width > maxDimension)
        {
            throw new PsdLoadException($"PSD header width {width} is outside the supported range 1-{maxDimension} for this document version.");
        }

        if (bitDepth is not (1 or 8 or 16 or 32))
        {
            throw new PsdLoadException($"PSD header bit depth {bitDepth} is not supported. Supported values are 1, 8, 16, and 32.");
        }

        if (!Enum.IsDefined(colorMode))
        {
            throw new PsdLoadException($"PSD header color mode {(ushort)colorMode} is not recognized by this implementation.");
        }
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

    /// <summary>
    /// Updates the header color mode for the compatibility subset that rewrites header metadata.
    /// </summary>
    /// <param name="colorMode">The color mode to store.</param>
    internal void SetColorMode(ColorModes colorMode)
    {
        ColorMode = colorMode;
    }

    /// <summary>
    /// Updates the PSD container version.
    /// </summary>
    /// <param name="version">The PSD container version to store.</param>
    internal void SetVersion(int version)
    {
        if (version != (int)Psd.PsdVersion.Psd && version != (int)Psd.PsdVersion.Psb)
        {
            throw new ArgumentOutOfRangeException(nameof(version), version, "Supported PSD versions are 1 (PSD) and 2 (PSB).");
        }

        FormatVersion = (Psd.PsdVersion)version;
    }
}
