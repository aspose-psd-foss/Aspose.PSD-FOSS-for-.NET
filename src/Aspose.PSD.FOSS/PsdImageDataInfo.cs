using System.Collections.ObjectModel;

namespace Aspose.PSD.FileFormats.Psd;

/// <summary>
/// Provides a read-only summary of the PSD merged image data structure.
/// </summary>
internal sealed class PsdImageDataInfo
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PsdImageDataInfo"/> class.
    /// </summary>
    /// <param name="kind">The structural kind of the payload.</param>
    /// <param name="rowLengthFieldSize">The size of one row-length entry in bytes.</param>
    /// <param name="rowByteCounts">The parsed row byte counts for RLE payloads.</param>
    /// <param name="compressedPayloadLength">The payload length after any structural headers.</param>
    /// <param name="usesPrediction">Whether ZIP prediction is in effect.</param>
    public PsdImageDataInfo(
        ImageDataKind kind,
        int rowLengthFieldSize,
        uint[] rowByteCounts,
        int compressedPayloadLength,
        bool usesPrediction)
    {
        Kind = kind;
        RowLengthFieldSize = rowLengthFieldSize;
        RowByteCounts = Array.AsReadOnly((uint[])rowByteCounts.Clone());
        CompressedPayloadLength = compressedPayloadLength;
        UsesPrediction = usesPrediction;
    }

    /// <summary>
    /// Gets the structural kind of the payload.
    /// </summary>
    public ImageDataKind Kind { get; }

    /// <summary>
    /// Gets the size of one RLE row-length entry in bytes.
    /// </summary>
    public int RowLengthFieldSize { get; }

    /// <summary>
    /// Gets the parsed row byte counts for RLE payloads.
    /// </summary>
    public ReadOnlyCollection<uint> RowByteCounts { get; }

    /// <summary>
    /// Gets the payload length after structural headers such as the RLE row-length table.
    /// </summary>
    public int CompressedPayloadLength { get; }

    /// <summary>
    /// Gets a value indicating whether ZIP prediction is in effect.
    /// </summary>
    public bool UsesPrediction { get; }
}
