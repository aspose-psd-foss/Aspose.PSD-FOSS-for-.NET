namespace Aspose.PSD.FileFormats.Psd.Layers;

/// <summary>
/// Represents a PSD layer resource.
/// </summary>
public abstract class LayerResource
{
    /// <summary>
    /// The common layer resource signature.
    /// </summary>
    public const int ResourceSignature = 0x3842494D;

    /// <summary>
    /// The PSB-specific layer resource signature.
    /// </summary>
    public const int PsbResourceSignature = 0x38425053;

    /// <summary>
    /// Gets the layer resource key.
    /// </summary>
    public abstract int Key { get; }

    /// <summary>
    /// Gets the layer resource length in bytes.
    /// </summary>
    public abstract int Length { get; }

    /// <summary>
    /// Gets the minimal PSD version required for the layer resource.
    /// </summary>
    public virtual int PsdVersion => 0;

    /// <summary>
    /// Gets the layer resource signature.
    /// </summary>
    public virtual int Signature => ResourceSignature;

    /// <summary>
    /// Saves the layer resource to the specified stream container.
    /// </summary>
    /// <param name="streamContainer">The stream container to save to.</param>
    /// <param name="psdVersion">The PSD version.</param>
    public abstract void Save(StreamContainer streamContainer, int psdVersion);
}
