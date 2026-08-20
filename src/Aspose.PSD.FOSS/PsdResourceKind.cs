namespace Aspose.PSD.FileFormats.Psd;

/// <summary>
/// Identifies the semantic classification exposed for an image resource block.
/// </summary>
internal enum PsdResourceKind
{
    /// <summary>
    /// The resource is not parsed semantically and remains raw-preserved only.
    /// </summary>
    Unknown,

    /// <summary>
    /// Reserved for future semantic parsing of the global layer-effects lighting angle resource.
    /// </summary>
    GlobalAngle,

    /// <summary>
    /// Reserved for future semantic parsing of an embedded ICC profile payload.
    /// </summary>
    IccProfile,

    /// <summary>
    /// Reserved for future semantic parsing of the intentionally-untagged ICC profile flag.
    /// </summary>
    IccUntaggedProfile
}
