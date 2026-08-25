namespace Aspose.PSD.FileFormats.Psd;

/// <summary>
/// Stores parsed PSD/PSB document sections used by <see cref="PsdImage"/>.
/// </summary>
internal sealed class PsdImageDocumentState
{
    /// <summary>
    /// Gets an empty document state before a PSD/PSB stream has been parsed.
    /// </summary>
    public static PsdImageDocumentState Empty { get; } = new(
        null,
        ColorData.Empty,
        ImageResourcesSection.Empty,
        LayerAndMaskSection.Empty,
        new ImageData(CompressionMethod.Raw, []));

    /// <summary>
    /// Initializes a new instance of the <see cref="PsdImageDocumentState"/> class.
    /// </summary>
    /// <param name="header">The parsed PSD/PSB header.</param>
    /// <param name="colorData">The parsed Color Mode Data section.</param>
    /// <param name="imageResourcesSection">The parsed Image Resources section.</param>
    /// <param name="layerAndMaskSection">The parsed Layer and Mask Information section.</param>
    /// <param name="imageData">The parsed merged Image Data section.</param>
    public PsdImageDocumentState(
        PsdHeader? header,
        ColorData colorData,
        ImageResourcesSection imageResourcesSection,
        LayerAndMaskSection layerAndMaskSection,
        ImageData imageData)
    {
        Header = header;
        ColorData = colorData;
        ImageResourcesSection = imageResourcesSection;
        LayerAndMaskSection = layerAndMaskSection;
        ImageData = imageData;
    }

    /// <summary>
    /// Gets the parsed PSD/PSB header.
    /// </summary>
    public PsdHeader? Header { get; }

    /// <summary>
    /// Gets the parsed Color Mode Data section.
    /// </summary>
    public ColorData ColorData { get; }

    /// <summary>
    /// Gets the parsed and raw-preserved Image Resources section.
    /// </summary>
    public ImageResourcesSection ImageResourcesSection { get; }

    /// <summary>
    /// Gets the parsed and raw-preserved Layer and Mask Information section.
    /// </summary>
    public LayerAndMaskSection LayerAndMaskSection { get; }

    /// <summary>
    /// Gets the parsed merged Image Data section.
    /// </summary>
    public ImageData ImageData { get; }

    /// <summary>
    /// Creates a state copy with a replaced Layer and Mask Information section.
    /// </summary>
    /// <param name="layerAndMaskSection">The replacement Layer and Mask Information section.</param>
    /// <returns>The updated document state.</returns>
    public PsdImageDocumentState WithLayerAndMaskSection(LayerAndMaskSection layerAndMaskSection)
    {
        return new PsdImageDocumentState(
            Header,
            ColorData,
            ImageResourcesSection,
            layerAndMaskSection,
            ImageData);
    }
}
