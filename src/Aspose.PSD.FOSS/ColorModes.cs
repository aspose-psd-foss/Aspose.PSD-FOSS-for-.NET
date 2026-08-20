namespace Aspose.PSD.FileFormats.Psd;

/// <summary>
/// Defines the color modes supported by PSD files.
/// </summary>
public enum ColorModes : short
{
    /// <summary>
    /// Bitmap color mode.
    /// </summary>
    Bitmap = 0,

    /// <summary>
    /// Grayscale color mode.
    /// </summary>
    Grayscale = 1,

    /// <summary>
    /// Indexed color mode (using a color palette).
    /// </summary>
    Indexed = 2,

    /// <summary>
    /// RGB color mode (Red, Green, Blue).
    /// </summary>
    Rgb = 3,

    /// <summary>
    /// CMYK color mode (Cyan, Magenta, Yellow, Black).
    /// </summary>
    Cmyk = 4,

    /// <summary>
    /// Multichannel color mode.
    /// </summary>
    Multichannel = 7,

    /// <summary>
    /// Duotone color mode (two-color gradient).
    /// </summary>
    Duotone = 8,

    /// <summary>
    /// LAB color mode.
    /// </summary>
    Lab = 9
}
