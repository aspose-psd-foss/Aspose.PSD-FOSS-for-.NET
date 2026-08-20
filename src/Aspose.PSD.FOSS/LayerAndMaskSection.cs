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
    private LayerAndMaskSection(
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
        ulong sectionLength = ReadSectionLength(reader, isLargeDocument);
        if (sectionLength == 0)
        {
            return Empty;
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
        Layer[] layers = [];

        if (layerCount > 0)
        {
            layers = new Layer[layerCount];
            for (int i = 0; i < layerCount; i++)
            {
                layers[i] = LayerRecordReader.Load(memReader, isLargeDocument);
            }
        }

        if (memReader.Position > layerInfoEnd)
        {
            throw new PsdLoadException("Layer records exceed the declared Layer Info section length.");
        }

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
    /// Writes the section while preserving raw bytes for no-mutation saves.
    /// </summary>
    /// <param name="writer">The destination writer.</param>
    /// <param name="isLargeDocument">true for PSB-sized lengths; otherwise, false.</param>
    public void Save(BigEndianWriter writer, bool isLargeDocument)
    {
        if (RawSectionBytes.Length > 0 && !HasLayerCollectionMutated && !Layers.Any(layer => layer.HasMutated))
        {
            WriteSectionLength(writer, RawSectionBytes.Length, isLargeDocument);
            writer.Write(RawSectionBytes);
            return;
        }

        if (Layers.Length == 0)
        {
            WriteSectionLength(writer, 0, isLargeDocument);
            return;
        }

        WriteLayerSectionWithMutations(writer, isLargeDocument);
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

    /// <summary>
    /// Rebuilds the layer info payload when at least one parsed layer has been mutated.
    /// </summary>
    /// <param name="writer">The destination writer positioned at the Layer and Mask Information section.</param>
    /// <param name="isLargeDocument">true for PSB-sized section lengths; otherwise, false.</param>
    private void WriteLayerSectionWithMutations(BigEndianWriter writer, bool isLargeDocument)
    {
        using var layerInfoPayloadStream = new MemoryStream();
        using var layerInfoPayloadWriter = new BigEndianWriter(layerInfoPayloadStream, leaveOpen: true);
        short layerCount = LayerCountRaw < 0 ? (short)-Layers.Length : (short)Layers.Length;
        layerInfoPayloadWriter.Write(layerCount);

        foreach (var layer in Layers)
        {
            LayerRecordWriter.Write(layer, layerInfoPayloadWriter, isLargeDocument);
        }

        layerInfoPayloadWriter.Write(LayerChannelImageDataRaw);
        byte[] layerInfoPayload = layerInfoPayloadStream.ToArray();

        using var sectionStream = new MemoryStream();
        using var sectionWriter = new BigEndianWriter(sectionStream, leaveOpen: true);
        if (isLargeDocument)
        {
            sectionWriter.Write((long)layerInfoPayload.Length);
        }
        else
        {
            sectionWriter.Write(layerInfoPayload.Length);
        }

        sectionWriter.Write(layerInfoPayload);
        sectionWriter.Write(GetLayerGlobalMaskAndTailBytesForWrite());

        byte[] sectionBytes = sectionStream.ToArray();
        WriteSectionLength(writer, sectionBytes.Length, isLargeDocument);
        writer.Write(sectionBytes);
    }

    /// <summary>
    /// Gets the global mask and trailing bytes to emit when rebuilding the layer section.
    /// </summary>
    /// <returns>The preserved tail bytes, or an empty global mask block when the original section had no tail.</returns>
    private byte[] GetLayerGlobalMaskAndTailBytesForWrite()
    {
        return LayerGlobalMaskAndTailRaw.Length > 0
            ? LayerGlobalMaskAndTailRaw
            : [0, 0, 0, 0];
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

    /// <summary>
    /// Writes the outer Layer and Mask Information section length using PSD or PSB field size.
    /// </summary>
    /// <param name="writer">The destination writer.</param>
    /// <param name="length">The section payload length to write.</param>
    /// <param name="isLargeDocument">true to write an 8-byte PSB length; otherwise, false.</param>
    private static void WriteSectionLength(BigEndianWriter writer, int length, bool isLargeDocument)
    {
        if (isLargeDocument)
        {
            writer.Write((ulong)length);
        }
        else
        {
            writer.Write((uint)length);
        }
    }
}
