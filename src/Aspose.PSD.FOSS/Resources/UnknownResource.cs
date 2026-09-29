namespace Aspose.PSD.FileFormats.Psd;

/// <summary>
/// Represents a parsed PSD image resource block without any ID‑specific semantics.
/// </summary>
internal sealed class UnknownResource
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UnknownResource"/> class.
    /// </summary>
    /// <param name="resourceId">The PSD resource identifier.</param>
    /// <param name="name">The decoded Pascal resource name.</param>
    /// <param name="data">The raw resource payload.</param>
    public UnknownResource(short resourceId, string name, byte[] data)
    {
        ResourceId = resourceId;
        Name = name;
        Data = data;
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
    /// Attempts to load one resource block from the reader.
    /// </summary>
    /// <param name="reader">The reader positioned at the start of a resource block.</param>
    /// <param name="sectionEnd">The byte position of the end of the resources section.</param>
    /// <returns>An <see cref="UnknownResource"/> instance or <c>null</c> if parsing cannot continue safely.</returns>
    public static UnknownResource? Load(BigEndianReader reader, long sectionEnd)
    {
        long startPos = reader.Position;
        if (startPos + 8 > sectionEnd)
        {
            return null;
        }

        uint signature = reader.ReadUInt32();
        if (signature != 0x3842494D) // "8BIM"
        {
            return null;
        }

        short resourceId = reader.ReadInt16();
        string name = reader.ReadPascalStringAlignedTo2();

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

        return new UnknownResource(resourceId, name, data);
    }

    /// <summary>
    /// Creates a public read‑only summary for this resource block.
    /// </summary>
    /// <returns>The public <see cref="PsdResourceInfo"/> summary with <see cref="PsdResourceKind.Unknown"/>.</returns>
    public PsdResourceInfo ToPublicInfo()
    {
        return new PsdResourceInfo(ResourceId, Name, PsdResourceKind.Unknown, Data.Length, null, null);
    }
}
