namespace Aspose.PSD.FOSS;

/// <summary>
/// Stores the PSD Image Data section as a compression header plus raw payload bytes.
/// </summary>
internal sealed class ImageData
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ImageData"/> class.
    /// </summary>
    /// <param name="compression">The PSD image compression method.</param>
    /// <param name="rawData">The raw image data payload after the compression field.</param>
    public ImageData(CompressionMethod compression, byte[] rawData)
    {
        Compression = compression;
        RawData = rawData;
    }

    /// <summary>
    /// Gets the stored compression method.
    /// </summary>
    public CompressionMethod Compression { get; }

    /// <summary>
    /// Gets the raw image data payload after the compression field.
    /// </summary>
    public byte[] RawData { get; }

    /// <summary>
    /// Loads the final Image Data section from the reader.
    /// </summary>
    /// <param name="reader">The reader positioned at the compression field.</param>
    /// <returns>The loaded <see cref="ImageData"/> instance.</returns>
    public static ImageData Load(BigEndianReader reader)
    {
        CompressionMethod compression = (CompressionMethod)reader.ReadUInt16();

        long imageDataStart = reader.Position;
        reader.Seek(0, SeekOrigin.End);
        long imageDataEnd = reader.Position;
        reader.Seek(imageDataStart, SeekOrigin.Begin);

        int imageDataLength = (int)(imageDataEnd - imageDataStart);
        byte[] rawData = reader.ReadBytes(imageDataLength);
        return new ImageData(compression, rawData);
    }

    /// <summary>
    /// Writes the Image Data section to the writer.
    /// </summary>
    /// <param name="writer">The destination writer.</param>
    public void Save(BigEndianWriter writer)
    {
        writer.Write((ushort)Compression);
        writer.Write(RawData);
    }
}
