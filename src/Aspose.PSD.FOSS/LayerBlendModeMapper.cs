using Aspose.PSD.FileFormats.Core.Blending;

namespace Aspose.PSD.FileFormats.Psd;

/// <summary>
/// Maps between PSD blend mode keys and the public <see cref="BlendMode"/> enum.
/// </summary>
internal static class LayerBlendModeMapper
{
    /// <summary>
    /// Gets the PSD blend mode key for the normal blend mode.
    /// </summary>
    public const string NormalBlendModeKey = "norm";

    /// <summary>
    /// Maps a PSD blend mode key to the public <see cref="BlendMode"/> enum.
    /// </summary>
    /// <param name="key">The 4-byte PSD blend mode key.</param>
    /// <returns>The mapped blend mode value.</returns>
    public static BlendMode ParseBlendModeKey(byte[] key)
    {
        if (key.Length < 4)
        {
            return BlendMode.Normal;
        }

        string modeKey = System.Text.Encoding.ASCII.GetString(key);
        return modeKey switch
        {
            NormalBlendModeKey => BlendMode.Normal,
            "mul " => BlendMode.Multiply,
            "scrn" => BlendMode.Screen,
            "diss" => BlendMode.Dissolve,
            "over" => BlendMode.Overlay,
            "dark" => BlendMode.Darken,
            "lite" => BlendMode.Lighten,
            "div " => BlendMode.ColorDodge,
            "idiv" => BlendMode.ColorBurn,
            "burn" => BlendMode.ColorBurn,
            "hLit" => BlendMode.HardLight,
            "hlit" => BlendMode.HardLight,
            "sLit" => BlendMode.SoftLight,
            "slit" => BlendMode.SoftLight,
            "diff" => BlendMode.Difference,
            "smud" => BlendMode.Exclusion,
            "hue " => BlendMode.Hue,
            "sat " => BlendMode.Saturation,
            "colr" => BlendMode.Color,
            "lum " => BlendMode.Luminosity,
            "lbrn" => BlendMode.LinearBurn,
            "lddg" => BlendMode.LinearDodge,
            "vLit" => BlendMode.VividLight,
            "lLit" => BlendMode.LinearLight,
            "pLit" => BlendMode.PinLight,
            "hMix" => BlendMode.HardMix,
            "pass" => BlendMode.PassThrough,
            "dkCl" => BlendMode.DarkerColor,
            "lgCl" => BlendMode.LighterColor,
            "fsub" => BlendMode.Subtract,
            "fdiv" => BlendMode.Divide,
            _ => BlendMode.Normal
        };
    }

    /// <summary>
    /// Maps the public <see cref="BlendMode"/> value back to a 4-byte PSD blend mode key.
    /// </summary>
    /// <param name="mode">The blend mode value to encode.</param>
    /// <returns>The encoded PSD blend mode key.</returns>
    public static string GetBlendModeKey(BlendMode mode)
    {
        return mode switch
        {
            BlendMode.Normal => NormalBlendModeKey,
            BlendMode.Multiply => "mul ",
            BlendMode.Screen => "scrn",
            BlendMode.Dissolve => "diss",
            BlendMode.Overlay => "over",
            BlendMode.Darken => "dark",
            BlendMode.Lighten => "lite",
            BlendMode.ColorDodge => "div ",
            BlendMode.ColorBurn => "idiv",
            BlendMode.HardLight => "hLit",
            BlendMode.SoftLight => "sLit",
            BlendMode.Difference => "diff",
            BlendMode.Exclusion => "smud",
            BlendMode.Hue => "hue ",
            BlendMode.Saturation => "sat ",
            BlendMode.Color => "colr",
            BlendMode.Luminosity => "lum ",
            BlendMode.LinearBurn => "lbrn",
            BlendMode.LinearDodge => "lddg",
            BlendMode.VividLight => "vLit",
            BlendMode.LinearLight => "lLit",
            BlendMode.PinLight => "pLit",
            BlendMode.HardMix => "hMix",
            BlendMode.PassThrough => "pass",
            BlendMode.DarkerColor => "dkCl",
            BlendMode.LighterColor => "lgCl",
            BlendMode.Subtract => "fsub",
            BlendMode.Divide => "fdiv",
            _ => NormalBlendModeKey
        };
    }
}
