namespace Aspose.PSD.FileFormats.Psd;

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
