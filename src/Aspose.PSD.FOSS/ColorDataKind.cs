namespace Aspose.PSD.FOSS;

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
