using Aspose.PSD.FileFormats.Core.Blending;
using Aspose.PSD.FileFormats.Psd.Layers;

namespace Aspose.PSD.FileFormats.Psd;

/// <summary>
/// Reads PSD/PSB layer records into <see cref="Layer"/> instances.
/// </summary>
internal static class LayerRecordReader
{
    /// <summary>
    /// Stores the Adobe layer record signature value "8BIM".
    /// </summary>
    private const uint AdobeLayerSignature = 0x3842494D;

    /// <summary>
    /// Stores the Adobe layer record signature text used in diagnostics.
    /// </summary>
    private const string AdobeLayerSignatureText = "8BIM";

    /// <summary>
    /// Stores the PSD flag bit that marks a layer as hidden when set.
    /// </summary>
    private const byte LayerInvisibleFlag = 0x02;

    /// <summary>
    /// Loads a layer record from the current reader position.
    /// </summary>
    /// <param name="reader">The reader positioned at the start of a layer record.</param>
    /// <param name="isLargeDocument">true for PSB-sized layer channel lengths; otherwise, false.</param>
    /// <returns>The parsed layer.</returns>
    public static Layer Load(BigEndianReader reader, bool isLargeDocument)
    {
        int top = reader.ReadInt32();
        int left = reader.ReadInt32();
        int bottom = reader.ReadInt32();
        int right = reader.ReadInt32();

        ushort actualChannelCount = reader.ReadUInt16();
        var channelInfoArray = new LayerChannelInfo[actualChannelCount];

        for (int i = 0; i < actualChannelCount; i++)
        {
            short channelId = reader.ReadInt16();
            ulong dataLength = isLargeDocument ? reader.ReadUInt64() : reader.ReadUInt32();

            channelInfoArray[i] = new LayerChannelInfo
            {
                ChannelId = channelId,
                DataLength = dataLength
            };
        }

        int signature = reader.ReadInt32();
        if (signature != AdobeLayerSignature)
        {
            throw new PsdLoadException($"Invalid layer blend mode signature. Expected '{AdobeLayerSignatureText}'.");
        }

        byte[] blendModeKey = reader.ReadBytes(4);
        string originalBlendModeKey = System.Text.Encoding.ASCII.GetString(blendModeKey);
        BlendMode blendMode = LayerBlendModeMapper.ParseBlendModeKey(blendModeKey);

        byte opacity = reader.ReadByte();
        byte clipping = reader.ReadByte();
        byte flags = reader.ReadByte();
        reader.ReadByte();

        int extraLength = reader.ReadInt32();
        if (extraLength < 0)
        {
            throw new PsdLoadException("Layer extra data length cannot be negative.");
        }

        string layerName = string.Empty;
        RawLayerMaskSection layerMaskData = RawLayerMaskSection.Empty;
        RawLayerBlendingRangesSection blendingRangesData = RawLayerBlendingRangesSection.Empty;
        byte[] additionalLayerData = [];

        if (extraLength > 0)
        {
            long extraStart = reader.Position;
            long extraEnd = extraStart + extraLength;

            layerMaskData = RawLayerMaskSection.Load(reader, extraEnd);
            if (reader.Position + sizeof(uint) > extraEnd)
            {
                throw new PsdLoadException("Layer extra data is truncated before the blending ranges length field.");
            }

            blendingRangesData = RawLayerBlendingRangesSection.Load(reader, extraEnd);
            layerName = reader.ReadPascalStringAlignedTo4();

            long remaining = extraEnd - reader.Position;
            if (remaining < 0)
            {
                throw new PsdLoadException("Layer extra data parser read beyond the declared extra data boundary.");
            }

            if (remaining > 0)
            {
                additionalLayerData = reader.ReadBytes(PsdSectionReader.GetNestedMemoryBackedLength(reader, (ulong)remaining, extraEnd, "Additional layer data"));
            }
        }

        Rectangle bounds = Rectangle.FromLTRB(left, top, right, bottom);
        bool visible = (flags & LayerInvisibleFlag) == 0;

        return Layer.CreateParsed(
            layerName,
            bounds,
            visible,
            opacity,
            flags,
            originalBlendModeKey,
            clipping,
            blendMode,
            channelInfoArray,
            layerMaskData,
            blendingRangesData,
            additionalLayerData);
    }
}
