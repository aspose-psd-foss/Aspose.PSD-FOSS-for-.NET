namespace Aspose.PSD.FileFormats.Psd.Layers;

/// <summary>
/// Defines PSD layer mask flags.
/// </summary>
[Flags]
public enum LayerMaskFlags : byte
{
    /// <summary>
    /// No layer mask flags are defined.
    /// </summary>
    None = 0,

    /// <summary>
    /// The layer mask position is relative to the layer.
    /// </summary>
    RelativeToLayer = 1,

    /// <summary>
    /// The layer mask is disabled.
    /// </summary>
    Disabled = 2,

    /// <summary>
    /// The layer mask is inverted when blending.
    /// </summary>
    InvertedWhenBlending = 4,

    /// <summary>
    /// The user mask comes from rendering other data.
    /// </summary>
    UserMaskFromRenderingOtherData = 8,

    /// <summary>
    /// The user or vector masks have parameters applied.
    /// </summary>
    UserOrVectorMasksHaveParameters = 16
}
