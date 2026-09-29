using System.Linq;

namespace Aspose.PSD.FileFormats.Psd;

/// <summary>
/// Writes PSD/PSB Layer and Mask Information sections.
/// </summary>
internal static class LayerAndMaskSectionWriter
{
    /// <summary>
    /// Writes the section while preserving raw bytes for no-mutation saves.
    /// </summary>
    /// <param name="section">The section to write.</param>
    /// <param name="writer">The destination writer.</param>
    /// <param name="isLargeDocument">true for PSB-sized lengths; otherwise, false.</param>
    public static void Save(LayerAndMaskSection section, BigEndianWriter writer, bool isLargeDocument)
    {
        if (section.RawSectionBytes.Length > 0
            && !section.HasLayerCollectionMutated
            && !section.Layers.Any(layer => layer.HasMutated))
        {
            WriteSectionLength(writer, section.RawSectionBytes.Length, isLargeDocument);
            writer.Write(section.RawSectionBytes);
            return;
        }

        if (section.Layers.Length == 0)
        {
            WriteSectionLength(writer, 0, isLargeDocument);
            return;
        }

        WriteLayerSectionWithMutations(section, writer, isLargeDocument);
    }

    /// <summary>
    /// Rebuilds the layer info payload when at least one parsed layer has been mutated.
    /// </summary>
    /// <param name="section">The section being rebuilt.</param>
    /// <param name="writer">The destination writer positioned at the Layer and Mask Information section.</param>
    /// <param name="isLargeDocument">true for PSB-sized section lengths; otherwise, false.</param>
    private static void WriteLayerSectionWithMutations(LayerAndMaskSection section, BigEndianWriter writer, bool isLargeDocument)
    {
        using var layerInfoPayloadStream = new MemoryStream();
        using var layerInfoPayloadWriter = new BigEndianWriter(layerInfoPayloadStream, leaveOpen: true);
        short layerCount = section.LayerCountRaw < 0 ? (short)-section.Layers.Length : (short)section.Layers.Length;
        layerInfoPayloadWriter.Write(layerCount);

        foreach (var layer in section.Layers)
        {
            LayerRecordWriter.Write(layer, layerInfoPayloadWriter, isLargeDocument);
        }

        layerInfoPayloadWriter.Write(section.LayerChannelImageDataRaw);
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
        sectionWriter.Write(GetLayerGlobalMaskAndTailBytesForWrite(section));

        byte[] sectionBytes = sectionStream.ToArray();
        WriteSectionLength(writer, sectionBytes.Length, isLargeDocument);
        writer.Write(sectionBytes);
    }

    /// <summary>
    /// Gets the global mask and trailing bytes to emit when rebuilding the layer section.
    /// </summary>
    /// <param name="section">The source section.</param>
    /// <returns>The preserved tail bytes, or an empty global mask block when the original section had no tail.</returns>
    private static byte[] GetLayerGlobalMaskAndTailBytesForWrite(LayerAndMaskSection section)
    {
        return section.LayerGlobalMaskAndTailRaw.Length > 0
            ? section.LayerGlobalMaskAndTailRaw
            : [0, 0, 0, 0];
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
