namespace Aspose.PSD.FOSS;

/// <summary>
/// Provides a read-only summary of one parsed layer channel record.
/// </summary>
public sealed class PsdLayerChannelInfo
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PsdLayerChannelInfo"/> class.
    /// </summary>
    /// <param name="channelId">The PSD channel identifier.</param>
    /// <param name="dataLength">The declared payload length in bytes.</param>
    public PsdLayerChannelInfo(short channelId, ulong dataLength)
    {
        ChannelId = channelId;
        DataLength = dataLength;
    }

    /// <summary>
    /// Gets the PSD channel identifier.
    /// </summary>
    public short ChannelId { get; }

    /// <summary>
    /// Gets the declared payload length in bytes.
    /// </summary>
    public ulong DataLength { get; }
}
