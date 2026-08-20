namespace Aspose.PSD.FileFormats.Psd.Layers;

/// <summary>
/// Provides a read-only summary of the parsed layer blending ranges subsection.
/// </summary>
internal sealed class LayerBlendingRangesInfo
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LayerBlendingRangesInfo"/> class.
    /// </summary>
    /// <param name="isPresent">Whether the subsection contains payload bytes.</param>
    /// <param name="rawDataLength">The raw subsection length including the leading length field.</param>
    public LayerBlendingRangesInfo(bool isPresent, int rawDataLength)
    {
        IsPresent = isPresent;
        RawDataLength = rawDataLength;
    }

    /// <summary>
    /// Gets a value indicating whether the subsection contains payload bytes.
    /// </summary>
    public bool IsPresent { get; }

    /// <summary>
    /// Gets the raw subsection length including the leading length field.
    /// </summary>
    public int RawDataLength { get; }
}
