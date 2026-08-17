namespace Aspose.PSD.FOSS;

/// <summary>
/// Represents one PSD image resource block and its raw payload.
/// </summary>
internal sealed class ResourceBlock
{
    /// <summary>
    /// The PSD resource ID for the global lighting angle metadata.
    /// </summary>
    public const short GlobalAngleResourceId = 1037;

    /// <summary>
    /// The PSD resource ID for an embedded ICC profile payload.
    /// </summary>
    public const short IccProfileResourceId = 1039;

    /// <summary>
    /// The PSD resource ID for the intentionally-untagged ICC profile flag.
    /// </summary>
    public const short IccUntaggedProfileResourceId = 1041;

    /// <summary>
    /// Initializes a new instance of the <see cref="ResourceBlock"/> class.
    /// </summary>
    /// <param name="resourceId">The PSD resource identifier.</param>
    /// <param name="name">The decoded Pascal resource name.</param>
    /// <param name="data">The raw resource payload.</param>
    public ResourceBlock(short resourceId, string name, byte[] data)
    {
        ResourceId = resourceId;
        Name = name;
        Data = data;
        Kind = GetKnownKind(resourceId);
        GlobalAngle = TryParseGlobalAngle(resourceId, data);
        IsIccProfileUntagged = TryParseIccUntaggedFlag(resourceId, data);
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
    /// Gets the raw resource payload bytes.
    /// </summary>
    public byte[] Data { get; }

    /// <summary>
    /// Gets the known semantic classification for this resource ID.
    /// </summary>
    public KnownResourceKind Kind { get; }

    /// <summary>
    /// Gets the parsed global angle, when this block contains that resource.
    /// </summary>
    public int? GlobalAngle { get; }

    /// <summary>
    /// Gets a value indicating whether the document explicitly disables assumed ICC profile handling.
    /// </summary>
    public bool? IsIccProfileUntagged { get; }

    /// <summary>
    /// Attempts to load one resource block from the reader.
    /// </summary>
    /// <param name="reader">The reader positioned at the start of a resource block.</param>
    /// <param name="sectionEnd">The byte position of the end of the resources section.</param>
    /// <returns>The parsed <see cref="ResourceBlock"/>, or <see langword="null"/> if parsing cannot continue safely.</returns>
    public static ResourceBlock? Load(BigEndianReader reader, long sectionEnd)
    {
        long startPos = reader.Position;
        if (startPos + 8 > sectionEnd)
        {
            return null;
        }

        uint signature = reader.ReadUInt32();
        if (signature != 0x3842494D)
        {
            return null;
        }

        short resourceId = reader.ReadInt16();
        byte nameLength = reader.ReadByte();
        string name = string.Empty;

        if (nameLength > 0)
        {
            byte[] nameBytes = reader.ReadBytes(nameLength);
            name = System.Text.Encoding.ASCII.GetString(nameBytes);
        }

        if ((nameLength + 1) % 2 != 0)
        {
            reader.Skip(1);
        }

        int dataLength = reader.ReadInt32();
        if (dataLength < 0 || reader.Position + dataLength > sectionEnd)
        {
            return null;
        }

        byte[] data = reader.ReadBytes(dataLength);

        if (dataLength % 2 == 1)
        {
            reader.Skip(1);
        }

        return new ResourceBlock(resourceId, name, data);
    }

    private static KnownResourceKind GetKnownKind(short resourceId)
    {
        return resourceId switch
        {
            GlobalAngleResourceId => KnownResourceKind.GlobalAngle,
            IccProfileResourceId => KnownResourceKind.IccProfile,
            IccUntaggedProfileResourceId => KnownResourceKind.IccUntaggedProfile,
            _ => KnownResourceKind.Unknown
        };
    }

    private static int? TryParseGlobalAngle(short resourceId, byte[] data)
    {
        if (resourceId != GlobalAngleResourceId || data.Length != 4)
        {
            return null;
        }

        return (data[0] << 24) |
               (data[1] << 16) |
               (data[2] << 8) |
               data[3];
    }

    private static bool? TryParseIccUntaggedFlag(short resourceId, byte[] data)
    {
        if (resourceId != IccUntaggedProfileResourceId || data.Length != 1)
        {
            return null;
        }

        return data[0] != 0;
    }
}

/// <summary>
/// Identifies the small set of image resources that this library understands semantically.
/// </summary>
internal enum KnownResourceKind
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
