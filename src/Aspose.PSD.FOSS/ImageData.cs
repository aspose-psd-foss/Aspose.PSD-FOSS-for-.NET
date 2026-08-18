namespace Aspose.PSD.FOSS;

/// <summary>
/// Stores the PSD Image Data section as a compression header plus raw payload bytes.
/// </summary>
internal sealed class ImageData
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ImageData"/> class for callers that only preserve raw payload bytes.
    /// </summary>
    /// <param name="compression">The PSD image compression method.</param>
    /// <param name="rawData">The raw image data payload after the compression field.</param>
    public ImageData(CompressionMethod compression, byte[] rawData)
        : this(compression, rawData, ParseStructure(compression, rawData, isLargeDocument: false, height: 0, channelCount: 0))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ImageData"/> class.
    /// </summary>
    /// <param name="compression">The PSD image compression method.</param>
    /// <param name="rawData">The raw image data payload after the compression field.</param>
    /// <param name="structure">The parsed structural view of the payload.</param>
    private ImageData(CompressionMethod compression, byte[] rawData, ImageDataStructure structure)
    {
        Compression = compression;
        RawData = rawData;
        Structure = structure;
    }

    /// <summary>
    /// Gets the parsed structural interpretation of the payload.
    /// </summary>
    public ImageDataStructure Structure { get; }

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
    /// <param name="isLargeDocument">true for PSB row-length sizing; otherwise, false.</param>
    /// <param name="height">The document height used to interpret row-oriented compression payloads.</param>
    /// <param name="channelCount">The document channel count used to interpret row-oriented compression payloads.</param>
    /// <returns>The loaded <see cref="ImageData"/> instance.</returns>
    public static ImageData Load(BigEndianReader reader, bool isLargeDocument, int height, int channelCount)
    {
        CompressionMethod compression = (CompressionMethod)reader.ReadUInt16();

        long imageDataStart = reader.Position;
        reader.Seek(0, SeekOrigin.End);
        long imageDataEnd = reader.Position;
        reader.Seek(imageDataStart, SeekOrigin.Begin);

        int imageDataLength = (int)(imageDataEnd - imageDataStart);
        byte[] rawData = reader.ReadBytes(imageDataLength);
        ImageDataStructure structure = ParseStructure(compression, rawData, isLargeDocument, height, channelCount);
        return new ImageData(compression, rawData, structure);
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

    /// <summary>
    /// Creates a read-only public summary of the parsed merged image data structure.
    /// </summary>
    /// <returns>The public image data summary.</returns>
    public PsdImageDataInfo ToPublicInfo()
    {
        return new PsdImageDataInfo(
            Structure.Kind,
            Structure.RowLengthFieldSize,
            Structure.RowByteCounts,
            Structure.CompressedPayloadLength,
            Structure.UsesPrediction);
    }

    /// <summary>
    /// Builds a structural view over the raw image-data payload without decoding pixels.
    /// </summary>
    /// <param name="compression">The stored compression mode.</param>
    /// <param name="rawData">The raw image-data payload after the compression field.</param>
    /// <param name="isLargeDocument">true for PSB row-length sizing; otherwise, false.</param>
    /// <param name="height">The document height.</param>
    /// <param name="channelCount">The document channel count.</param>
    /// <returns>The parsed structure descriptor.</returns>
    /// <exception cref="PsdLoadException">Thrown when the payload cannot match the declared compression structure.</exception>
    private static ImageDataStructure ParseStructure(CompressionMethod compression, byte[] rawData, bool isLargeDocument, int height, int channelCount)
    {
        return compression switch
        {
            CompressionMethod.Raw => ImageDataStructure.CreateRaw(rawData.Length),
            CompressionMethod.RLE => ParseRleStructure(rawData, isLargeDocument, height, channelCount),
            CompressionMethod.ZIP => ImageDataStructure.CreateZip(rawData.Length, usesPrediction: false),
            CompressionMethod.RZ => ImageDataStructure.CreateZip(rawData.Length, usesPrediction: true),
            _ => ImageDataStructure.CreateUnknown(rawData.Length),
        };
    }

    /// <summary>
    /// Parses the RLE row-length table so the payload has a validated structural model.
    /// </summary>
    /// <param name="rawData">The raw image-data payload after the compression field.</param>
    /// <param name="isLargeDocument">true for PSB row-length sizing; otherwise, false.</param>
    /// <param name="height">The document height.</param>
    /// <param name="channelCount">The document channel count.</param>
    /// <returns>The parsed structure descriptor.</returns>
    /// <exception cref="PsdLoadException">Thrown when the payload is shorter than the declared row-length table.</exception>
    private static ImageDataStructure ParseRleStructure(byte[] rawData, bool isLargeDocument, int height, int channelCount)
    {
        if (height < 0 || channelCount < 0)
        {
            throw new PsdLoadException("PSD image dimensions are invalid for RLE image data.");
        }

        int rowCount = checked(height * channelCount);
        int rowLengthFieldSize = isLargeDocument ? sizeof(uint) : sizeof(ushort);
        int tableLength = checked(rowCount * rowLengthFieldSize);

        if (rawData.Length < tableLength)
        {
            throw new PsdLoadException("PSD RLE image data is truncated before the row-length table completes.");
        }

        if (rowCount == 0)
        {
            return ImageDataStructure.CreateRle([], rowLengthFieldSize, 0);
        }

        int offset = 0;
        var rowByteCounts = new uint[rowCount];
        for (int i = 0; i < rowCount; i++)
        {
            rowByteCounts[i] = isLargeDocument
                ? ReadUInt32BigEndian(rawData, offset)
                : ReadUInt16BigEndian(rawData, offset);
            offset += rowLengthFieldSize;
        }

        return ImageDataStructure.CreateRle(rowByteCounts, rowLengthFieldSize, rawData.Length - tableLength);
    }

    /// <summary>
    /// Reads a big-endian 16-bit unsigned integer from a byte span.
    /// </summary>
    /// <param name="buffer">The payload buffer.</param>
    /// <param name="offset">The current read offset.</param>
    /// <returns>The decoded value.</returns>
    private static ushort ReadUInt16BigEndian(byte[] buffer, int offset)
    {
        return (ushort)((buffer[offset] << 8) | buffer[offset + 1]);
    }

    /// <summary>
    /// Reads a big-endian 32-bit unsigned integer from a byte span.
    /// </summary>
    /// <param name="buffer">The payload buffer.</param>
    /// <param name="offset">The current read offset.</param>
    /// <returns>The decoded value.</returns>
    private static uint ReadUInt32BigEndian(byte[] buffer, int offset)
    {
        return ((uint)buffer[offset] << 24)
            | ((uint)buffer[offset + 1] << 16)
            | ((uint)buffer[offset + 2] << 8)
            | buffer[offset + 3];
    }
}

/// <summary>
/// Describes the validated structural layout of the PSD Image Data payload.
/// </summary>
internal sealed class ImageDataStructure
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ImageDataStructure"/> class.
    /// </summary>
    /// <param name="kind">The structural kind implied by the compression method.</param>
    /// <param name="rowLengthFieldSize">The size of each RLE row-length entry, or zero for non-RLE payloads.</param>
    /// <param name="rowByteCounts">The parsed row byte counts for RLE payloads.</param>
    /// <param name="compressedPayloadLength">The number of bytes after any structural headers.</param>
    /// <param name="usesPrediction">Whether ZIP prediction is in effect.</param>
    private ImageDataStructure(
        ImageDataKind kind,
        int rowLengthFieldSize,
        uint[] rowByteCounts,
        int compressedPayloadLength,
        bool usesPrediction)
    {
        Kind = kind;
        RowLengthFieldSize = rowLengthFieldSize;
        RowByteCounts = rowByteCounts;
        CompressedPayloadLength = compressedPayloadLength;
        UsesPrediction = usesPrediction;
    }

    /// <summary>
    /// Gets the structural kind implied by the compression mode.
    /// </summary>
    public ImageDataKind Kind { get; }

    /// <summary>
    /// Gets the size of one RLE row-length entry in bytes.
    /// </summary>
    public int RowLengthFieldSize { get; }

    /// <summary>
    /// Gets the parsed row byte counts for RLE payloads.
    /// </summary>
    public uint[] RowByteCounts { get; }

    /// <summary>
    /// Gets the number of payload bytes after structural headers such as the RLE row-length table.
    /// </summary>
    public int CompressedPayloadLength { get; }

    /// <summary>
    /// Gets a value indicating whether ZIP prediction is in effect.
    /// </summary>
    public bool UsesPrediction { get; }

    /// <summary>
    /// Creates a structure descriptor for raw image data.
    /// </summary>
    /// <param name="payloadLength">The stored payload length.</param>
    /// <returns>The structure descriptor.</returns>
    public static ImageDataStructure CreateRaw(int payloadLength)
    {
        return new ImageDataStructure(ImageDataKind.Raw, 0, [], payloadLength, usesPrediction: false);
    }

    /// <summary>
    /// Creates a structure descriptor for RLE image data.
    /// </summary>
    /// <param name="rowByteCounts">The parsed per-row byte counts.</param>
    /// <param name="rowLengthFieldSize">The size of one row-length entry in bytes.</param>
    /// <param name="compressedPayloadLength">The payload length after the row-length table.</param>
    /// <returns>The structure descriptor.</returns>
    public static ImageDataStructure CreateRle(uint[] rowByteCounts, int rowLengthFieldSize, int compressedPayloadLength)
    {
        return new ImageDataStructure(ImageDataKind.Rle, rowLengthFieldSize, rowByteCounts, compressedPayloadLength, usesPrediction: false);
    }

    /// <summary>
    /// Creates a structure descriptor for ZIP-based image data.
    /// </summary>
    /// <param name="payloadLength">The stored payload length.</param>
    /// <param name="usesPrediction">Whether ZIP prediction is used.</param>
    /// <returns>The structure descriptor.</returns>
    public static ImageDataStructure CreateZip(int payloadLength, bool usesPrediction)
    {
        return new ImageDataStructure(ImageDataKind.Zip, 0, [], payloadLength, usesPrediction);
    }

    /// <summary>
    /// Creates a structure descriptor for unsupported compression values while preserving the payload.
    /// </summary>
    /// <param name="payloadLength">The stored payload length.</param>
    /// <returns>The structure descriptor.</returns>
    public static ImageDataStructure CreateUnknown(int payloadLength)
    {
        return new ImageDataStructure(ImageDataKind.Unknown, 0, [], payloadLength, usesPrediction: false);
    }
}
