namespace Aspose.PSD.FOSS;

/// <summary>
/// Maps internal Color Mode Data interpretations to public inspection values.
/// </summary>
internal static class ColorDataKindExtensions
{
    /// <summary>
    /// Converts an internal color data kind to the public API enum.
    /// </summary>
    /// <param name="kind">The internal color data kind.</param>
    /// <returns>The public color data kind.</returns>
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
