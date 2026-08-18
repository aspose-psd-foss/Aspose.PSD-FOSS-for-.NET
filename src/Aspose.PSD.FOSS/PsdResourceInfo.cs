namespace Aspose.PSD.FOSS;

/// <summary>
/// Provides a read-only summary of one parsed PSD image resource block.
/// </summary>
public sealed class PsdResourceInfo
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PsdResourceInfo"/> class.
    /// </summary>
    /// <param name="resourceId">The PSD resource identifier.</param>
    /// <param name="name">The decoded Pascal resource name.</param>
    /// <param name="kind">The known semantic classification for the resource.</param>
    /// <param name="dataLength">The raw payload length in bytes.</param>
    /// <param name="globalAngle">The parsed global angle, when available.</param>
    /// <param name="isIccProfileUntagged">The parsed untagged-profile flag, when available.</param>
    public PsdResourceInfo(
        short resourceId,
        string name,
        PsdResourceKind kind,
        int dataLength,
        int? globalAngle,
        bool? isIccProfileUntagged)
    {
        ResourceId = resourceId;
        Name = name;
        Kind = kind;
        DataLength = dataLength;
        GlobalAngle = globalAngle;
        IsIccProfileUntagged = isIccProfileUntagged;
    }

    /// <summary>
    /// Gets the PSD resource identifier.
    /// </summary>
    public short ResourceId { get; }

    /// <summary>
    /// Gets the decoded Pascal resource name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the known semantic classification for the resource.
    /// </summary>
    public PsdResourceKind Kind { get; }

    /// <summary>
    /// Gets the raw payload length in bytes.
    /// </summary>
    public int DataLength { get; }

    /// <summary>
    /// Gets the parsed global angle, when this resource carries that value.
    /// </summary>
    public int? GlobalAngle { get; }

    /// <summary>
    /// Gets the parsed untagged-profile flag, when this resource carries that value.
    /// </summary>
    public bool? IsIccProfileUntagged { get; }
}
