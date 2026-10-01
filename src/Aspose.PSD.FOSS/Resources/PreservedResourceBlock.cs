namespace Aspose.PSD.FileFormats.Psd;

/// <summary>
/// Adapts a raw-preserved image resource to the official <see cref="ResourceBlock"/> surface.
/// </summary>
internal sealed class PreservedResourceBlock : ResourceBlock
{
    /// <summary>
    /// Stores the preserved resource payload.
    /// </summary>
    private readonly byte[] _data;

    /// <summary>
    /// Initializes a new instance of the <see cref="PreservedResourceBlock"/> class.
    /// </summary>
    /// <param name="resource">The parsed raw resource block.</param>
    public PreservedResourceBlock(UnknownResource resource)
    {
        ID = resource.ResourceId;
        Name = resource.Name;
        _data = resource.Data;
    }

    /// <summary>
    /// Gets the resource data size in bytes.
    /// </summary>
    public override int DataSize => _data.Length;

    /// <summary>
    /// Gets the minimal required PSD version.
    /// </summary>
    public override int MinimalVersion => (int)PsdVersion.Psd;

    /// <summary>
    /// Saves the resource block to the specified stream container.
    /// </summary>
    /// <param name="stream">The stream container to save to.</param>
    public override void Save(StreamContainer stream)
    {
        throw new NotSupportedException("Saving individual image resource blocks is not supported by this FOSS build.");
    }
}
