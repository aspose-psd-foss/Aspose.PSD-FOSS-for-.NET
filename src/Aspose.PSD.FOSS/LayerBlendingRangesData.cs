namespace Aspose.PSD.FileFormats.Psd.Layers;

/// <summary>
/// Represents PSD layer blending ranges data.
/// </summary>
public class LayerBlendingRangesData
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LayerBlendingRangesData"/> class.
    /// </summary>
    public LayerBlendingRangesData()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LayerBlendingRangesData"/> class with a known raw length.
    /// </summary>
    /// <param name="length">The raw blending ranges subsection length in bytes.</param>
    private LayerBlendingRangesData(int length)
    {
        Length = length;
    }

    /// <summary>
    /// Gets or sets the composite blend range.
    /// </summary>
    public BlendRange CompositeBlendRange { get; set; } = new();

    /// <summary>
    /// Gets or sets the per-channel blend ranges.
    /// </summary>
    public BlendRange[] ChannelBlendRanges { get; set; } = [];

    /// <summary>
    /// Gets the blending ranges data length in bytes.
    /// </summary>
    public int Length { get; private set; }

    /// <summary>
    /// Creates public blending ranges data from a preserved raw subsection length.
    /// </summary>
    /// <param name="length">The raw subsection length in bytes.</param>
    /// <returns>The public blending ranges data.</returns>
    internal static LayerBlendingRangesData FromRawLength(int length)
    {
        return new LayerBlendingRangesData(length);
    }
}
