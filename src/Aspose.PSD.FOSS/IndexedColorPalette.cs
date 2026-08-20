namespace Aspose.PSD.FOSS;

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
