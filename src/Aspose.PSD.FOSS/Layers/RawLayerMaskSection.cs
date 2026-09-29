namespace Aspose.PSD.FileFormats.Psd;

/// <summary>
/// Stores the raw layer mask subsection including its leading length field.
/// </summary>
internal sealed class RawLayerMaskSection
{
    /// <summary>
    /// Gets an empty layer mask subsection.
    /// </summary>
    public static RawLayerMaskSection Empty { get; } = new([]);

    /// <summary>
    /// Initializes a new instance of the <see cref="RawLayerMaskSection"/> class.
    /// </summary>
    /// <param name="rawData">The raw subsection bytes including the length field.</param>
    public RawLayerMaskSection(byte[] rawData)
    {
        RawData = rawData;
    }

    /// <summary>
    /// Gets the raw subsection bytes including the length field.
    /// </summary>
    public byte[] RawData { get; }

    /// <summary>
    /// Loads the layer mask subsection from the reader.
    /// </summary>
    /// <param name="reader">The reader positioned at the subsection length field.</param>
    /// <param name="sectionEnd">The byte position of the end of the enclosing layer extra data.</param>
    /// <returns>The loaded <see cref="RawLayerMaskSection"/> instance.</returns>
    public static RawLayerMaskSection Load(BigEndianReader reader, long sectionEnd)
    {
        uint length = reader.ReadUInt32();
        int payloadLength = PsdSectionReader.GetNestedMemoryBackedLength(reader, length, sectionEnd, "Layer mask subsection");
        if (payloadLength > int.MaxValue - sizeof(uint))
        {
            throw new PsdLoadException("Layer mask subsection is too large to preserve in memory with its length field.");
        }

        byte[] rawData = new byte[4 + payloadLength];
        WriteUInt32BigEndian(rawData, 0, length);
        if (payloadLength > 0)
        {
            byte[] payload = reader.ReadBytes(payloadLength);
            Buffer.BlockCopy(payload, 0, rawData, 4, payloadLength);
        }

        return new RawLayerMaskSection(rawData);
    }

    /// <summary>
    /// Writes a 32-bit unsigned integer into a byte buffer in big-endian byte order.
    /// </summary>
    /// <param name="buffer">The target buffer.</param>
    /// <param name="offset">The destination offset in the buffer.</param>
    /// <param name="value">The value to encode.</param>
    private static void WriteUInt32BigEndian(byte[] buffer, int offset, uint value)
    {
        buffer[offset] = (byte)((value >> 24) & 0xFF);
        buffer[offset + 1] = (byte)((value >> 16) & 0xFF);
        buffer[offset + 2] = (byte)((value >> 8) & 0xFF);
        buffer[offset + 3] = (byte)(value & 0xFF);
    }
}
