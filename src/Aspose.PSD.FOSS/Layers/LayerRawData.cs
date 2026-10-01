using Aspose.PSD.FileFormats.Psd.Layers;

namespace Aspose.PSD.FileFormats.Psd;

/// <summary>
/// Stores raw PSD layer record data required for byte-preserving saves.
/// </summary>
internal sealed class LayerRawData
{
    /// <summary>
    /// Gets an empty raw layer data block.
    /// </summary>
    public static LayerRawData Empty { get; } = new(
        0,
        LayerBlendModeMapper.NormalBlendModeKey,
        [],
        RawLayerMaskSection.Empty,
        RawLayerBlendingRangesSection.Empty,
        []);

    /// <summary>
    /// Initializes a new instance of the <see cref="LayerRawData"/> class.
    /// </summary>
    /// <param name="flags">The original PSD layer flags byte.</param>
    /// <param name="blendModeKey">The original 4-byte PSD blend mode key.</param>
    /// <param name="channelInfo">The parsed layer channel metadata.</param>
    /// <param name="layerMaskSection">The raw layer mask subsection.</param>
    /// <param name="blendingRangesSection">The raw blending ranges subsection.</param>
    /// <param name="additionalLayerData">The remaining additional layer data bytes.</param>
    public LayerRawData(
        byte flags,
        string blendModeKey,
        LayerChannelInfo[] channelInfo,
        RawLayerMaskSection layerMaskSection,
        RawLayerBlendingRangesSection blendingRangesSection,
        byte[] additionalLayerData)
    {
        Flags = flags;
        BlendModeKey = blendModeKey;
        ChannelInfo = channelInfo;
        LayerMaskSection = layerMaskSection;
        BlendingRangesSection = blendingRangesSection;
        AdditionalLayerData = additionalLayerData;
    }

    /// <summary>
    /// Gets the original PSD layer flags byte.
    /// </summary>
    public byte Flags { get; }

    /// <summary>
    /// Gets the original 4-byte PSD blend mode key.
    /// </summary>
    public string BlendModeKey { get; }

    /// <summary>
    /// Gets the parsed per-channel metadata from the layer record.
    /// </summary>
    public LayerChannelInfo[] ChannelInfo { get; }

    /// <summary>
    /// Gets the raw layer mask subsection including its length field.
    /// </summary>
    public RawLayerMaskSection LayerMaskSection { get; }

    /// <summary>
    /// Gets the raw blending ranges subsection including its length field.
    /// </summary>
    public RawLayerBlendingRangesSection BlendingRangesSection { get; }

    /// <summary>
    /// Gets all remaining additional layer data after the Pascal layer name.
    /// </summary>
    public byte[] AdditionalLayerData { get; }

    /// <summary>
    /// Creates a raw data copy with a replacement blend mode key.
    /// </summary>
    /// <param name="blendModeKey">The replacement 4-byte PSD blend mode key.</param>
    /// <returns>The updated raw layer data.</returns>
    public LayerRawData WithBlendModeKey(string blendModeKey)
    {
        return new LayerRawData(
            Flags,
            blendModeKey,
            ChannelInfo,
            LayerMaskSection,
            BlendingRangesSection,
            AdditionalLayerData);
    }
}
