namespace Aspose.PSD.FOSS;

/// <summary>
/// Stores one channel metadata entry from a layer record.
/// </summary>
internal struct LayerChannelInfo
{
    /// <summary>
    /// Gets or sets the PSD channel identifier.
    /// </summary>
    public short ChannelId;

    /// <summary>
    /// Gets or sets the declared byte length of the channel data payload.
    /// </summary>
    public ulong DataLength;
}
