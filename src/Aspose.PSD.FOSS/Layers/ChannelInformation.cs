using Aspose.PSD.FileFormats.Psd;

namespace Aspose.PSD.FileFormats.Psd.Layers;

/// <summary>
/// Represents PSD layer channel information.
/// </summary>
public class ChannelInformation
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChannelInformation"/> class.
    /// </summary>
    /// <param name="compressionMethod">The channel compression method.</param>
    /// <param name="bitDepth">The channel bit depth.</param>
    /// <param name="psdVersion">The PSD version number.</param>
    public ChannelInformation(CompressionMethod compressionMethod, int bitDepth, int psdVersion)
    {
        CompressionMethod = compressionMethod;
        Length = GetHeaderLength(psdVersion);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ChannelInformation"/> class from parsed layer record metadata.
    /// </summary>
    /// <param name="channelId">The channel identifier.</param>
    /// <param name="length">The declared channel data length.</param>
    private ChannelInformation(short channelId, int length)
    {
        ChannelID = channelId;
        CompressionMethod = CompressionMethod.Raw;
        Length = length;
    }

    /// <summary>
    /// Gets or sets the channel identifier.
    /// </summary>
    public short ChannelID { get; set; }

    /// <summary>
    /// Gets or sets the channel compression method.
    /// </summary>
    public CompressionMethod CompressionMethod { get; set; }

    /// <summary>
    /// Gets the channel length in bytes.
    /// </summary>
    public int Length { get; private set; }

    /// <summary>
    /// Creates public channel information from the internal layer channel metadata.
    /// </summary>
    /// <param name="channelInfo">The internal layer channel metadata.</param>
    /// <returns>The public channel information.</returns>
    internal static ChannelInformation FromLayerChannelInfo(LayerChannelInfo channelInfo)
    {
        int length = channelInfo.DataLength > int.MaxValue
            ? int.MaxValue
            : (int)channelInfo.DataLength;

        return new ChannelInformation(channelInfo.ChannelId, length);
    }

    /// <summary>
    /// Gets the minimum channel data header length for the specified PSD version.
    /// </summary>
    /// <param name="psdVersion">The PSD version number.</param>
    /// <returns>The channel data header length.</returns>
    private static int GetHeaderLength(int psdVersion)
    {
        return psdVersion == (int)PsdVersion.Psb ? sizeof(long) : sizeof(short);
    }
}
