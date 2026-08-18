namespace Aspose.PSD.FOSS;

/// <summary>
/// Stores the PSD Color Mode Data section as raw bytes plus mode-aware parsed structure when available.
/// </summary>
internal sealed class ColorData
{
    /// <summary>
    /// Gets the semantic interpretation applied to the raw color mode payload.
    /// </summary>
    public ColorDataKind Kind { get; }

    /// <summary>
    /// Gets an empty color data section.
    /// </summary>
    public static ColorData Empty { get; } = new([], ColorDataKind.None);

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorData"/> class.
    /// </summary>
    /// <param name="rawData">The raw color mode data payload.</param>
    /// <param name="kind">The semantic interpretation of the payload.</param>
    /// <param name="indexedPalette">The parsed indexed palette, when present.</param>
    public ColorData(byte[] rawData, ColorDataKind kind, IndexedColorPalette? indexedPalette = null)
    {
        RawData = rawData;
        Kind = kind;
        IndexedPalette = indexedPalette;
    }

    /// <summary>
    /// Gets the raw color mode data payload.
    /// </summary>
    public byte[] RawData { get; }

    /// <summary>
    /// Gets the parsed indexed color palette when the payload represents a standard PSD indexed palette.
    /// </summary>
    public IndexedColorPalette? IndexedPalette { get; }

    /// <summary>
    /// Loads the Color Mode Data section from the reader.
    /// </summary>
    /// <param name="reader">The reader positioned at the section length field.</param>
    /// <param name="colorMode">The color mode declared in the PSD header.</param>
    /// <returns>The loaded <see cref="ColorData"/> instance.</returns>
    public static ColorData Load(BigEndianReader reader, ColorModes colorMode)
    {
        uint length = reader.ReadUInt32();
        if (length == 0)
        {
            return Empty;
        }

        byte[] rawData = reader.ReadBytes((int)length);
        return colorMode switch
        {
            ColorModes.Indexed when rawData.Length == IndexedColorPalette.ExpectedRawLength
                => new ColorData(rawData, ColorDataKind.IndexedPalette, IndexedColorPalette.Parse(rawData)),
            ColorModes.Rgb => new ColorData(rawData, ColorDataKind.RgbPayload),
            ColorModes.CMYK => new ColorData(rawData, ColorDataKind.CmykPayload),
            _ => new ColorData(rawData, ColorDataKind.RawPreserved)
        };
    }

    /// <summary>
    /// Writes the Color Mode Data section to the writer.
    /// </summary>
    /// <param name="writer">The destination writer.</param>
    public void Save(BigEndianWriter writer)
    {
        writer.Write((uint)RawData.Length);
        if (RawData.Length > 0)
        {
            writer.Write(RawData);
        }
    }

    /// <summary>
    /// Creates a read-only public summary for the current Color Mode Data section.
    /// </summary>
    /// <returns>The public color data summary.</returns>
    public PsdColorDataInfo ToPublicInfo()
    {
        return new PsdColorDataInfo(Kind.ToPublicKind(), RawData.Length, IndexedPalette?.ToPublicInfo());
    }
}

/// <summary>
/// Describes how the color mode data payload was interpreted for the current document mode.
/// </summary>
internal enum ColorDataKind
{
    /// <summary>
    /// No color mode payload was present.
    /// </summary>
    None,

    /// <summary>
    /// The payload was parsed as a standard 256-entry indexed palette.
    /// </summary>
    IndexedPalette,

    /// <summary>
    /// The payload belongs to an RGB document, where Photoshop normally stores no color mode data.
    /// </summary>
    RgbPayload,

    /// <summary>
    /// The payload belongs to a CMYK document and is preserved as opaque mode-specific data.
    /// </summary>
    CmykPayload,

    /// <summary>
    /// The payload is preserved raw because this implementation does not parse it further.
    /// </summary>
    RawPreserved
}

/// <summary>
/// Represents the standard 256-entry palette stored in indexed-color PSD documents.
/// </summary>
internal sealed class IndexedColorPalette
{
    /// <summary>
    /// The PSD raw payload size for a 256-color indexed palette.
    /// </summary>
    public const int ExpectedRawLength = 768;

    /// <summary>
    /// Initializes a new instance of the <see cref="IndexedColorPalette"/> class.
    /// </summary>
    /// <param name="entries">The decoded palette entries in RGB order.</param>
    public IndexedColorPalette(System.Drawing.Color[] entries)
    {
        Entries = entries;
    }

    /// <summary>
    /// Gets the decoded 256 palette entries.
    /// </summary>
    public System.Drawing.Color[] Entries { get; }

    /// <summary>
    /// Parses a PSD indexed palette from the raw non-interleaved RGB payload.
    /// </summary>
    /// <param name="rawData">The raw 768-byte palette payload.</param>
    /// <returns>The parsed palette.</returns>
    public static IndexedColorPalette Parse(byte[] rawData)
    {
        var entries = new System.Drawing.Color[256];
        for (int i = 0; i < entries.Length; i++)
        {
            entries[i] = System.Drawing.Color.FromArgb(rawData[i], rawData[i + 256], rawData[i + 512]);
        }

        return new IndexedColorPalette(entries);
    }

    /// <summary>
    /// Creates a read-only public summary of the indexed palette.
    /// </summary>
    /// <returns>The public indexed palette summary.</returns>
    public IndexedColorPaletteInfo ToPublicInfo()
    {
        return new IndexedColorPaletteInfo(Entries);
    }
}

internal static class ColorDataKindExtensions
{
    public static PsdColorDataKind ToPublicKind(this ColorDataKind kind)
    {
        return kind switch
        {
            ColorDataKind.IndexedPalette => PsdColorDataKind.IndexedPalette,
            ColorDataKind.RgbPayload => PsdColorDataKind.RgbPayload,
            ColorDataKind.CmykPayload => PsdColorDataKind.CmykPayload,
            ColorDataKind.RawPreserved => PsdColorDataKind.RawPreserved,
            _ => PsdColorDataKind.None
        };
    }
}
