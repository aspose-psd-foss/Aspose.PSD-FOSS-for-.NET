namespace Aspose.PSD.FOSS;

/// <summary>
/// Stores the raw blending ranges subsection including its leading length field.
/// </summary>
internal sealed class LayerBlendingRangesData
{
    /// <summary>
    /// Gets an empty blending ranges subsection.
    /// </summary>
    public static LayerBlendingRangesData Empty { get; } = new([]);

    /// <summary>
    /// Initializes a new instance of the <see cref="LayerBlendingRangesData"/> class.
    /// </summary>
    /// <param name="rawData">The raw subsection bytes including the length field.</param>
    public LayerBlendingRangesData(byte[] rawData)
    {
        RawData = rawData;
    }

    /// <summary>
    /// Gets the raw subsection bytes including the length field.
    /// </summary>
    public byte[] RawData { get; }

    /// <summary>
    /// Loads the blending ranges subsection from the reader.
    /// </summary>
    /// <param name="reader">The reader positioned at the subsection length field.</param>
    /// <param name="sectionEnd">The byte position of the end of the enclosing layer extra data.</param>
    /// <returns>The loaded <see cref="LayerBlendingRangesData"/> instance.</returns>
    public static LayerBlendingRangesData Load(BigEndianReader reader, long sectionEnd)
    {
        uint length = reader.ReadUInt32();
        byte[] rawData = new byte[4 + length];
        WriteUInt32BigEndian(rawData, 0, length);
        if (length > 0)
        {
            if (reader.Position + length > sectionEnd)
            {
                throw new EndOfStreamException();
            }

            byte[] payload = reader.ReadBytes((int)length);
            Buffer.BlockCopy(payload, 0, rawData, 4, (int)length);
        }

        return new LayerBlendingRangesData(rawData);
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
