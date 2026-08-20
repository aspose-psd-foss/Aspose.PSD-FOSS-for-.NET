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

        byte[] rawData = PsdSectionReader.ReadBytes(reader, length, "Color Mode Data section");
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
