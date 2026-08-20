namespace Aspose.PSD.FileFormats.Psd;

/// <summary>
/// Represents the PSD Image Resources section as raw-preserved bytes plus unknown-only parsed summaries.
/// </summary>
internal sealed class ImageResourcesSection
{
    /// <summary>
    /// Gets an empty Image Resources section.
    /// </summary>
    public static ImageResourcesSection Empty { get; } = new([], []);

    /// <summary>
    /// Initializes a new instance of the <see cref="ImageResourcesSection"/> class.
    /// </summary>
    /// <param name="rawData">The raw Image Resources payload without the section length field.</param>
    /// <param name="resources">The parsed unknown resource summaries.</param>
    private ImageResourcesSection(byte[] rawData, UnknownResource[] resources)
    {
        RawData = rawData;
        Resources = resources;
    }

    /// <summary>
    /// Gets the raw Image Resources payload without the leading section length field.
    /// </summary>
    public byte[] RawData { get; }

    /// <summary>
    /// Gets the parsed unknown-only resource blocks.
    /// </summary>
    public UnknownResource[] Resources { get; }

    /// <summary>
    /// Gets a value indicating whether the section contains raw bytes or parsed resources.
    /// </summary>
    public bool HasResources => RawData.Length > 0 || Resources.Length > 0;

    /// <summary>
    /// Loads the Image Resources section from the current reader position.
    /// </summary>
    /// <param name="reader">The reader positioned at the section length field.</param>
    /// <returns>The loaded section.</returns>
    public static ImageResourcesSection Load(BigEndianReader reader)
    {
        uint resourcesLength = reader.ReadUInt32();
        if (resourcesLength == 0)
        {
            return Empty;
        }

        byte[] rawData = PsdSectionReader.ReadBytes(reader, resourcesLength, "Image Resources section");
        long resourcesEnd = rawData.Length;
        using var resourcesReader = new BigEndianReader(new MemoryStream(rawData, writable: false), leaveOpen: true);
        var resourcesList = new List<UnknownResource>();

        while (resourcesReader.Position < resourcesEnd)
        {
            UnknownResource? resource = UnknownResource.Load(resourcesReader, resourcesEnd);
            if (resource == null)
            {
                break;
            }

            resourcesList.Add(resource);
        }

        return new ImageResourcesSection(rawData, resourcesList.ToArray());
    }

    /// <summary>
    /// Writes the Image Resources section with raw preservation when available.
    /// </summary>
    /// <param name="writer">The destination writer.</param>
    public void Save(BigEndianWriter writer)
    {
        if (RawData.Length > 0)
        {
            writer.Write((uint)RawData.Length);
            writer.Write(RawData);
            return;
        }

        if (Resources.Length == 0)
        {
            writer.Write((uint)0);
            return;
        }

        long resourcesStart = writer.Position;
        writer.Write((uint)0);

        foreach (var resource in Resources)
        {
            writer.Write((uint)0x3842494D);
            writer.Write((short)resource.ResourceId);
            writer.WritePascalStringAlignedTo2(resource.Name);
            writer.Write((int)resource.Data.Length);
            writer.Write(resource.Data);

            if (resource.Data.Length % 2 == 1)
            {
                writer.Write((byte)0);
            }
        }

        long resourcesEnd = writer.Position;
        writer.Seek(resourcesStart, SeekOrigin.Begin);
        writer.Write((int)(resourcesEnd - resourcesStart - sizeof(uint)));
        writer.Seek(resourcesEnd, SeekOrigin.Begin);
    }
}
