namespace Aspose.PSD.FileFormats.Psd;

/// <summary>
/// Provides a read-only summary of the PSD Color Mode Data section.
/// </summary>
internal sealed class PsdColorDataInfo
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PsdColorDataInfo"/> class.
    /// </summary>
    /// <param name="kind">The interpreted kind of the payload.</param>
    /// <param name="rawDataLength">The raw payload length in bytes.</param>
    /// <param name="indexedPalette">The parsed indexed palette, when present.</param>
    public PsdColorDataInfo(PsdColorDataKind kind, int rawDataLength, IndexedColorPaletteInfo? indexedPalette)
    {
        Kind = kind;
        RawDataLength = rawDataLength;
        IndexedPalette = indexedPalette;
    }

    /// <summary>
    /// Gets the interpreted kind of the payload.
    /// </summary>
    public PsdColorDataKind Kind { get; }

    /// <summary>
    /// Gets the raw payload length in bytes.
    /// </summary>
    public int RawDataLength { get; }

    /// <summary>
    /// Gets the parsed indexed palette, when present.
    /// </summary>
    public IndexedColorPaletteInfo? IndexedPalette { get; }
}
