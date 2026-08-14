namespace Aspose.PSD.FOSS;

/// <summary>
/// Writes PSD/PSB structures to a destination stream in the required section order.
/// </summary>
internal sealed class PsdWriter : IDisposable
{
    /// <summary>
    /// Stores the underlying big-endian writer used for PSD output.
    /// </summary>
    private readonly BigEndianWriter _writer;

    /// <summary>
    /// Initializes a new instance of the <see cref="PsdWriter"/> class.
    /// </summary>
    /// <param name="stream">The destination stream.</param>
    /// <param name="leaveOpen">true to leave the stream open after disposal; otherwise, false.</param>
    public PsdWriter(Stream stream, bool leaveOpen)
    {
        _writer = new BigEndianWriter(stream, leaveOpen);
    }

    /// <summary>
    /// Gets the underlying big-endian writer.
    /// </summary>
    public BigEndianWriter Writer => _writer;

    /// <summary>
    /// Writes the shared PSD/PSB file signature.
    /// </summary>
    public void WriteSignature()
    {
        _writer.Write((uint)PsdHeader.PsdSignature);
    }

    /// <summary>
    /// Releases resources held by the writer.
    /// </summary>
    public void Dispose()
    {
        _writer.Dispose();
    }
}
