namespace Aspose.PSD.FOSS;

/// <summary>
/// Identifies the structural shape of the PSD Image Data payload.
/// </summary>
public enum ImageDataKind
{
    /// <summary>
    /// The payload is raw pixel data.
    /// </summary>
    Raw,

    /// <summary>
    /// The payload starts with an RLE row-length table followed by compressed scan data.
    /// </summary>
    Rle,

    /// <summary>
    /// The payload is ZIP-compressed scan data, optionally with prediction.
    /// </summary>
    Zip,

    /// <summary>
    /// The payload uses an unrecognized compression code and is preserved as-is.
    /// </summary>
    Unknown,
}
