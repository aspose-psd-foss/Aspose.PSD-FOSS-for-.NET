namespace Aspose.PSD.FileFormats.Core.Blending;

/// <summary>
/// Defines the blend modes used for layers in PSD files.
/// </summary>
public enum BlendMode : uint
{
    /// <summary>
    /// Normal blend mode (no blending).
    /// </summary>
    Normal = 1852797549,

    /// <summary>
    /// Multiply blend mode (multiply colors, darkens image).
    /// </summary>
    Multiply = 1836411936,

    /// <summary>
    /// Screen blend mode (screen colors, lightens image).
    /// </summary>
    Screen = 1935897198,

    /// <summary>
    /// Overlay blend mode (combines multiply and screen).
    /// </summary>
    Overlay = 1870030194,

    /// <summary>
    /// Darken blend mode (keeps darker pixels).
    /// </summary>
    Darken = 1684107883,

    /// <summary>
    /// Lighten blend mode (keeps lighter pixels).
    /// </summary>
    Lighten = 1818850405,

    /// <summary>
    /// Color Dodge blend mode (dodges colors).
    /// </summary>
    ColorDodge = 1684633120,

    /// <summary>
    /// Color Burn blend mode (burns colors).
    /// </summary>
    ColorBurn = 1768188278,

    /// <summary>
    /// Hard Light blend mode (hard light blending).
    /// </summary>
    HardLight = 1749838196,

    /// <summary>
    /// Soft Light blend mode (soft light blending).
    /// </summary>
    SoftLight = 1934387572,

    /// <summary>
    /// Difference blend mode (subtract colors).
    /// </summary>
    Difference = 1684629094,

    /// <summary>
    /// Exclusion blend mode (exclude colors).
    /// </summary>
    Exclusion = 1936553316,

    /// <summary>
    /// Hue blend mode (apply hue).
    /// </summary>
    Hue = 1752524064,

    /// <summary>
    /// Saturation blend mode (apply saturation).
    /// </summary>
    Saturation = 1935766560,

    /// <summary>
    /// Color blend mode (apply color).
    /// </summary>
    Color = 1668246642,

    /// <summary>
    /// Luminosity blend mode (apply luminosity).
    /// </summary>
    Luminosity = 1819634976,

    /// <summary>
    /// Darker Color blend mode (darker of colors).
    /// </summary>
    DarkerColor = 1684751212,

    /// <summary>
    /// Lighter Color blend mode (lighter of colors).
    /// </summary>
    LighterColor = 1818706796,

    /// <summary>
    /// Linear Burn blend mode (linear burn).
    /// </summary>
    LinearBurn = 1818391150,

    /// <summary>
    /// Linear Dodge blend mode (linear dodge).
    /// </summary>
    LinearDodge = 1818518631,

    /// <summary>
    /// Linear Light blend mode (linear light).
    /// </summary>
    LinearLight = 1816947060,

    /// <summary>
    /// Vivid Light blend mode (vivid light).
    /// </summary>
    VividLight = 1984719220,

    /// <summary>
    /// Pin Light blend mode (pin light).
    /// </summary>
    PinLight = 1884055924,

    /// <summary>
    /// Hard Mix blend mode (hard mix).
    /// </summary>
    HardMix = 1749903736,

    /// <summary>
    /// Subtract blend mode (subtract colors).
    /// </summary>
    Subtract = 1718842722,

    /// <summary>
    /// Divide blend mode (divide colors).
    /// </summary>
    Divide = 1717856630,

    /// <summary>
    /// Dissolve blend mode.
    /// </summary>
    Dissolve = 1684632435,

    /// <summary>
    /// Pass-through blend mode.
    /// </summary>
    PassThrough = 1885434739,

    /// <summary>
    /// Blend mode is absent or not set yet.
    /// </summary>
    Absent = 0
}
