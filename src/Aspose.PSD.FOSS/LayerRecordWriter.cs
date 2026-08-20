namespace Aspose.PSD.FOSS;

/// <summary>
/// Writes PSD/PSB layer records from the in-memory <see cref="Layer"/> model.
/// </summary>
internal static class LayerRecordWriter
{
    /// <summary>
    /// Stores the Adobe layer record signature value "8BIM".
    /// </summary>
    private const uint AdobeLayerSignature = 0x3842494D;

    /// <summary>
    /// Stores the PSD flag bit that marks a layer as hidden when set.
    /// </summary>
    private const byte LayerInvisibleFlag = 0x02;

    /// <summary>
    /// Stores the reserved trailing byte in the fixed layer record fields.
    /// </summary>
    private const byte LayerRecordReservedByte = 0;

    /// <summary>
    /// Writes one layer record using PSD- or PSB-sized channel lengths.
    /// </summary>
    /// <param name="layer">The layer to write.</param>
    /// <param name="writer">The destination writer.</param>
    /// <param name="isLargeDocument">true for PSB-sized layer channel lengths; otherwise, false.</param>
    public static void Write(Layer layer, BigEndianWriter writer, bool isLargeDocument)
    {
        writer.Write(layer.Bounds.Top);
        writer.Write(layer.Bounds.Left);
        writer.Write(layer.Bounds.Bottom);
        writer.Write(layer.Bounds.Right);
        writer.Write((ushort)layer.ChannelInfo.Length);

        for (int i = 0; i < layer.ChannelInfo.Length; i++)
        {
            writer.Write(layer.ChannelInfo[i].ChannelId);
            if (isLargeDocument)
            {
                writer.Write(layer.ChannelInfo[i].DataLength);
            }
            else
            {
                writer.Write((uint)layer.ChannelInfo[i].DataLength);
            }
        }

        writer.Write(AdobeLayerSignature);
        writer.Write(System.Text.Encoding.ASCII.GetBytes(layer.BlendModeKey));
        writer.Write(layer.Opacity);
        writer.Write(layer.Clipping);
        writer.Write(GetFlagsForWrite(layer));
        writer.Write(LayerRecordReservedByte);
        writer.Write(GetExtraDataLength(layer));
        writer.Write(layer.LayerMaskData.RawData);
        writer.Write(layer.BlendingRangesData.RawData);
        writer.WritePascalStringAlignedTo4(layer.Name);
        writer.Write(layer.AdditionalLayerData);
    }

    /// <summary>
    /// Combines the original layer flags with the current public visibility state.
    /// </summary>
    /// <param name="layer">The layer whose flags should be written.</param>
    /// <returns>The PSD layer flags byte to write.</returns>
    private static byte GetFlagsForWrite(Layer layer)
    {
        byte flags = layer.RawFlags;
        return layer.IsVisible
            ? (byte)(flags & ~LayerInvisibleFlag)
            : (byte)(flags | LayerInvisibleFlag);
    }

    /// <summary>
    /// Calculates the layer extra data byte count written after the fixed layer record fields.
    /// </summary>
    /// <param name="layer">The layer whose extra data should be measured.</param>
    /// <returns>The extra data byte count.</returns>
    private static int GetExtraDataLength(Layer layer)
    {
        return layer.LayerMaskData.RawData.Length
            + layer.BlendingRangesData.RawData.Length
            + BigEndianWriter.GetPascalStringStorageLengthAlignedTo4(layer.Name)
            + layer.AdditionalLayerData.Length;
    }
}
