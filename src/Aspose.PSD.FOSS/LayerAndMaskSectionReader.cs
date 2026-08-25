using Aspose.PSD.FileFormats.Psd.Layers;

namespace Aspose.PSD.FileFormats.Psd;

/// <summary>
/// Reads PSD/PSB Layer and Mask Information sections.
/// </summary>
internal static class LayerAndMaskSectionReader
{
    /// <summary>
    /// Loads the section using PSD- or PSB-sized outer and layer-info lengths.
    /// </summary>
    /// <param name="reader">The reader positioned at the outer section length field.</param>
    /// <param name="isLargeDocument">true for PSB-sized lengths; otherwise, false.</param>
    /// <returns>The loaded section.</returns>
    public static LayerAndMaskSection Load(BigEndianReader reader, bool isLargeDocument)
    {
        ulong sectionLength = ReadSectionLength(reader, isLargeDocument);
        if (sectionLength == 0)
        {
            return LayerAndMaskSection.Empty;
        }

        byte[] rawSectionBytes = PsdSectionReader.ReadBytes(reader, sectionLength, "Layer and Mask Information section");
        using var memReader = new BigEndianReader(new MemoryStream(rawSectionBytes, writable: false), leaveOpen: true);

        long layerInfoLength = PsdSectionReader.ValidateSignedLength(
            isLargeDocument ? memReader.ReadInt64() : memReader.ReadInt32(),
            "Layer Info section");
        if (layerInfoLength > rawSectionBytes.Length - memReader.Position)
        {
            throw new PsdLoadException("Layer Info section length exceeds the enclosing Layer and Mask Information section.");
        }

        long layerInfoEnd = (isLargeDocument ? sizeof(long) : sizeof(int)) + layerInfoLength;
        if (layerInfoLength == 0)
        {
            byte[] tail = rawSectionBytes.Length > memReader.Position
                ? memReader.ReadBytes(checked((int)(rawSectionBytes.Length - memReader.Position)))
                : [];
            return new LayerAndMaskSection(rawSectionBytes, [], tail, 0, []);
        }

        if (layerInfoLength < sizeof(short))
        {
            throw new PsdLoadException("Layer Info section is too short to contain the layer count field.");
        }

        short layerCountRaw = memReader.ReadInt16();
        short layerCount = layerCountRaw < 0 ? (short)-layerCountRaw : layerCountRaw;
        Layer[] layers = ReadLayers(memReader, layerCount, layerInfoEnd, isLargeDocument);

        int channelImageDataLength = checked((int)(layerInfoEnd - memReader.Position));
        byte[] layerChannelImageDataRaw = channelImageDataLength > 0
            ? memReader.ReadBytes(channelImageDataLength)
            : [];

        int globalMaskAndTailLength = Math.Max(0, rawSectionBytes.Length - (int)memReader.Position);
        byte[] layerGlobalMaskAndTailRaw = globalMaskAndTailLength > 0
            ? memReader.ReadBytes(globalMaskAndTailLength)
            : [];

        return new LayerAndMaskSection(
            rawSectionBytes,
            layerChannelImageDataRaw,
            layerGlobalMaskAndTailRaw,
            layerCountRaw,
            layers);
    }

    /// <summary>
    /// Reads layer records and validates the declared Layer Info boundary.
    /// </summary>
    /// <param name="reader">The reader positioned after the layer count field.</param>
    /// <param name="layerCount">The absolute layer count to read.</param>
    /// <param name="layerInfoEnd">The absolute end of the Layer Info subsection.</param>
    /// <param name="isLargeDocument">true for PSB-sized layer channel lengths; otherwise, false.</param>
    /// <returns>The parsed layer records.</returns>
    private static Layer[] ReadLayers(BigEndianReader reader, short layerCount, long layerInfoEnd, bool isLargeDocument)
    {
        Layer[] layers = [];
        if (layerCount > 0)
        {
            layers = new Layer[layerCount];
            for (int i = 0; i < layerCount; i++)
            {
                layers[i] = LayerRecordReader.Load(reader, isLargeDocument);
            }
        }

        if (reader.Position > layerInfoEnd)
        {
            throw new PsdLoadException("Layer records exceed the declared Layer Info section length.");
        }

        return layers;
    }

    /// <summary>
    /// Reads the outer Layer and Mask Information section length using PSD or PSB field size.
    /// </summary>
    /// <param name="reader">The reader positioned at the outer section length field.</param>
    /// <param name="isLargeDocument">true to read an 8-byte PSB length; otherwise, false.</param>
    /// <returns>The declared section payload length.</returns>
    private static ulong ReadSectionLength(BigEndianReader reader, bool isLargeDocument)
    {
        return isLargeDocument ? reader.ReadUInt64() : reader.ReadUInt32();
    }
}
