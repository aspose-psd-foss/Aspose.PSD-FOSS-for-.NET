using Aspose.PSD.FileFormats.Psd.Layers;

namespace Aspose.PSD.FileFormats.Psd;

/// <summary>
/// Represents the PSD/PSB Layer and Mask Information section and its raw-preserved save state.
/// </summary>
internal sealed class LayerAndMaskSection
{
    /// <summary>
    /// Gets an empty Layer and Mask Information section.
    /// </summary>
    public static LayerAndMaskSection Empty { get; } = new([], [], [], 0, []);

    /// <summary>
    /// Initializes a new instance of the <see cref="LayerAndMaskSection"/> class.
    /// </summary>
    /// <param name="rawSectionBytes">The raw section payload without the outer length field.</param>
    /// <param name="layerChannelImageDataRaw">The raw channel image data payload after layer records.</param>
    /// <param name="layerGlobalMaskAndTailRaw">The raw global mask and trailing section bytes.</param>
    /// <param name="layerCountRaw">The original signed layer count value.</param>
    /// <param name="layers">The parsed layer records.</param>
    /// <param name="hasLayerCollectionMutated">true when the public layer collection was replaced; otherwise, false.</param>
    public LayerAndMaskSection(
        byte[] rawSectionBytes,
        byte[] layerChannelImageDataRaw,
        byte[] layerGlobalMaskAndTailRaw,
        short layerCountRaw,
        Layer[] layers,
        bool hasLayerCollectionMutated = false)
    {
        RawSectionBytes = rawSectionBytes;
        LayerChannelImageDataRaw = layerChannelImageDataRaw;
        LayerGlobalMaskAndTailRaw = layerGlobalMaskAndTailRaw;
        LayerCountRaw = layerCountRaw;
        Layers = layers;
        HasLayerCollectionMutated = hasLayerCollectionMutated;
    }

    /// <summary>
    /// Gets the raw section payload without the outer length field.
    /// </summary>
    public byte[] RawSectionBytes { get; }

    /// <summary>
    /// Gets the raw layer channel image data payload after layer records.
    /// </summary>
    public byte[] LayerChannelImageDataRaw { get; }

    /// <summary>
    /// Gets the raw global mask info and trailing section bytes.
    /// </summary>
    public byte[] LayerGlobalMaskAndTailRaw { get; }

    /// <summary>
    /// Gets the original signed layer count so transparency-protected layers can preserve sign.
    /// </summary>
    public short LayerCountRaw { get; }

    /// <summary>
    /// Gets the parsed layer records.
    /// </summary>
    public Layer[] Layers { get; }

    /// <summary>
    /// Gets a value indicating whether the public layer collection was replaced.
    /// </summary>
    public bool HasLayerCollectionMutated { get; }

    /// <summary>
    /// Loads the section using PSD- or PSB-sized outer and layer-info lengths.
    /// </summary>
    /// <param name="reader">The reader positioned at the outer section length field.</param>
    /// <param name="isLargeDocument">true for PSB-sized lengths; otherwise, false.</param>
    /// <returns>The loaded section.</returns>
    public static LayerAndMaskSection Load(BigEndianReader reader, bool isLargeDocument)
    {
        return LayerAndMaskSectionReader.Load(reader, isLargeDocument);
    }

    /// <summary>
    /// Writes the section while preserving raw bytes for no-mutation saves.
    /// </summary>
    /// <param name="writer">The destination writer.</param>
    /// <param name="isLargeDocument">true for PSB-sized lengths; otherwise, false.</param>
    public void Save(BigEndianWriter writer, bool isLargeDocument)
    {
        LayerAndMaskSectionWriter.Save(this, writer, isLargeDocument);
    }

    /// <summary>
    /// Creates a section copy with a replaced layer collection and marks it for rewriting on save.
    /// </summary>
    /// <param name="layers">The replacement layer collection.</param>
    /// <returns>The section copy.</returns>
    public LayerAndMaskSection WithLayers(Layer[] layers)
    {
        return new LayerAndMaskSection(
            RawSectionBytes,
            LayerChannelImageDataRaw,
            LayerGlobalMaskAndTailRaw,
            LayerCountRaw,
            layers.ToArray(),
            hasLayerCollectionMutated: true);
    }

}
