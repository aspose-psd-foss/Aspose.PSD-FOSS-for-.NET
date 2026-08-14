namespace Aspose.PSD.FOSS;

/// <summary>
/// Defines the compression methods used for image data in PSD files.
/// </summary>
public enum CompressionMethod
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
    ZIP = 2,

    /// <summary>
    /// RZ (a variant of RLE) compression.
    /// </summary>
    RZ = 3
}
