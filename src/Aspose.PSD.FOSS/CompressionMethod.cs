namespace Aspose.PSD.FileFormats.Psd;

/// <summary>
/// Defines the compression methods used for image data in PSD files.
/// </summary>
public enum CompressionMethod : short
{
    /// <summary>
    /// Raw (uncompressed) data.
    /// </summary>
    Raw = 0,

    /// <summary>
    /// RLE (Run-Length Encoded) compression.
    /// </summary>
    RLE = 1,

    /// <summary>
    /// ZIP (lossless) compression.
    /// </summary>
    ZipWithoutPrediction = 2,

    /// <summary>
    /// RZ (a variant of RLE) compression.
    /// </summary>
    ZipWithPrediction = 3,

    /// <summary>
    /// Compatibility alias for ZIP without prediction.
    /// </summary>
    ZIP = ZipWithoutPrediction,

    /// <summary>
    /// Compatibility alias for ZIP with prediction.
    /// </summary>
    RZ = ZipWithPrediction
}
