namespace Aspose.PSD.FOSS;

/// <summary>
/// Identifies the small set of image resources that this library understands semantically.
/// </summary>
public enum PsdResourceKind
{
    /// <summary>
    /// The resource is not parsed semantically and remains raw-preserved only.
    /// </summary>
    Unknown,

    /// <summary>
    /// The resource stores the global layer-effects lighting angle.
    /// </summary>
    GlobalAngle,

    /// <summary>
    /// The resource stores an embedded ICC profile payload.
    /// </summary>
    IccProfile,

    /// <summary>
    /// The resource stores the intentionally-untagged ICC profile flag.
    /// </summary>
    IccUntaggedProfile
}
