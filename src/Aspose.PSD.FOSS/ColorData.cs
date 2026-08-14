namespace Aspose.PSD.FOSS;

/// <summary>
/// Stores the PSD Color Mode Data section as raw bytes.
/// </summary>
internal sealed class ColorData
{
    /// <summary>
    /// Gets an empty color data section.
    /// </summary>
    public static ColorData Empty { get; } = new([]);

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorData"/> class.
    /// </summary>
    /// <param name="rawData">The raw color mode data payload.</param>
    public ColorData(byte[] rawData)
    {
        RawData = rawData;
    }

    /// <summary>
    /// Gets the raw color mode data payload.
    /// </summary>
    public byte[] RawData { get; }

    /// <summary>
    /// Loads the Color Mode Data section from the reader.
    /// </summary>
    /// <param name="reader">The reader positioned at the section length field.</param>
    /// <returns>The loaded <see cref="ColorData"/> instance.</returns>
    public static ColorData Load(BigEndianReader reader)
    {
        uint length = reader.ReadUInt32();
        return length == 0 ? Empty : new ColorData(reader.ReadBytes((int)length));
    }

    /// <summary>
    /// Writes the Color Mode Data section to the writer.
    /// </summary>
    /// <param name="writer">The destination writer.</param>
    public void Save(BigEndianWriter writer)
    {
        writer.Write((uint)RawData.Length);
        if (RawData.Length > 0)
        {
            writer.Write(RawData);
        }
    }
}
