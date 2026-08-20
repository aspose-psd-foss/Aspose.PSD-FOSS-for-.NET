namespace Aspose.PSD.FileFormats.Psd;

/// <summary>
/// Represents a PSD image resource block.
/// </summary>
public abstract class ResourceBlock
{
    /// <summary>
    /// The regular Photoshop resource signature.
    /// </summary>
    public const int ResouceBlockSignature = 0x3842494D;

    /// <summary>
    /// The ImageReady resource signature.
    /// </summary>
    public const int ResouceBlockMeSaSignature = 0x3842494D;

    /// <summary>
    /// Gets the resource data size in bytes.
    /// </summary>
    public abstract int DataSize { get; }

    /// <summary>
    /// Gets or sets the unique identifier for the resource.
    /// </summary>
    public short ID { get; set; }

    /// <summary>
    /// Gets the minimal required PSD version.
    /// </summary>
    public abstract int MinimalVersion { get; }

    /// <summary>
    /// Gets or sets the resource name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets the resource signature.
    /// </summary>
    public int Signature => ResouceBlockSignature;

    /// <summary>
    /// Gets the resource block size in bytes including its data.
    /// </summary>
    public int Size => DataSize;

    /// <summary>
    /// Saves the resource block to the specified stream container.
    /// </summary>
    /// <param name="stream">The stream container to save to.</param>
    public abstract void Save(StreamContainer stream);

    /// <summary>
    /// Validates the resource values.
    /// </summary>
    public virtual void ValidateValues()
    {
    }

    /// <summary>
    /// Represents resource block state.
    /// </summary>
    public enum ResourceBlockState
    {
        /// <summary>
        /// The resource block is ready.
        /// </summary>
        Ready,

        /// <summary>
        /// The resource block is disposed.
        /// </summary>
        Disposed
    }
}
