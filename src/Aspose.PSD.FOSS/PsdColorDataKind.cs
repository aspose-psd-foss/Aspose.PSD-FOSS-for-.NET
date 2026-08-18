namespace Aspose.PSD.FOSS;

/// <summary>
/// Describes how the Color Mode Data payload was interpreted for the loaded document.
/// </summary>
public enum PsdColorDataKind
{
    /// <summary>
    /// No color mode payload was present.
    /// </summary>
    None,

    /// <summary>
    /// The payload was parsed as a standard indexed palette.
    /// </summary>
    IndexedPalette,

    /// <summary>
    /// The payload belongs to an RGB document.
    /// </summary>
    RgbPayload,

    /// <summary>
    /// The payload belongs to a CMYK document.
    /// </summary>
    CmykPayload,

    /// <summary>
    /// The payload is preserved as opaque raw data.
    /// </summary>
    RawPreserved
}
