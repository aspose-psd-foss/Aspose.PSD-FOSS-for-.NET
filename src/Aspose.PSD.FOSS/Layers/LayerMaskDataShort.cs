namespace Aspose.PSD.FileFormats.Psd.Layers;

/// <summary>
/// Defines layer mask data for layers that have only a raster or vector mask.
/// </summary>
public class LayerMaskDataShort : LayerMaskData
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LayerMaskDataShort"/> class.
    /// </summary>
    public LayerMaskDataShort()
    {
    }

    /// <summary>
    /// Gets or sets the layer mask padding.
    /// </summary>
    public short Padding { get; set; }
}
