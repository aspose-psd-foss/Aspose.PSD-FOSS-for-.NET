namespace Aspose.PSD.FOSS.Tests;

/// <summary>
/// Wraps a readable in-memory stream and intentionally disables seeking.
/// </summary>
internal sealed class NonSeekableReadStream : Stream
{
    /// <summary>
    /// Stores the wrapped in-memory stream that provides the readable payload.
    /// </summary>
    private readonly MemoryStream _innerStream;

    /// <summary>
    /// Initializes a new instance of the <see cref="NonSeekableReadStream"/> class.
    /// </summary>
    /// <param name="data">The bytes exposed by the stream.</param>
    public NonSeekableReadStream(byte[] data)
    {
        _innerStream = new MemoryStream(data, writable: false);
    }

    /// <summary>
    /// Gets a value indicating whether the stream supports reading.
    /// </summary>
    public override bool CanRead => true;

    /// <summary>
    /// Gets a value indicating whether the stream supports seeking.
    /// </summary>
    public override bool CanSeek => false;

    /// <summary>
    /// Gets a value indicating whether the stream supports writing.
    /// </summary>
    public override bool CanWrite => false;

    /// <summary>
    /// Gets the total length of the stream.
    /// </summary>
    public override long Length => _innerStream.Length;

    /// <summary>
    /// Gets or sets the current stream position.
    /// </summary>
    public override long Position
    {
        get => _innerStream.Position;
        set => throw new NotSupportedException();
    }

    /// <summary>
    /// Flushes buffered state.
    /// </summary>
    public override void Flush()
    {
    }

    /// <summary>
    /// Reads bytes from the stream.
    /// </summary>
    /// <param name="buffer">The destination buffer.</param>
    /// <param name="offset">The zero-based destination offset.</param>
    /// <param name="count">The requested byte count.</param>
    /// <returns>The number of bytes actually read.</returns>
    public override int Read(byte[] buffer, int offset, int count)
    {
        return _innerStream.Read(buffer, offset, count);
    }

    /// <summary>
    /// Seeks within the stream.
    /// </summary>
    /// <param name="offset">The byte offset relative to the origin.</param>
    /// <param name="origin">The reference origin.</param>
    /// <returns>The new stream position.</returns>
    public override long Seek(long offset, SeekOrigin origin)
    {
        throw new NotSupportedException();
    }

    /// <summary>
    /// Changes the stream length.
    /// </summary>
    /// <param name="value">The new stream length.</param>
    public override void SetLength(long value)
    {
        throw new NotSupportedException();
    }

    /// <summary>
    /// Writes bytes to the stream.
    /// </summary>
    /// <param name="buffer">The source buffer.</param>
    /// <param name="offset">The zero-based source offset.</param>
    /// <param name="count">The number of bytes to write.</param>
    public override void Write(byte[] buffer, int offset, int count)
    {
        throw new NotSupportedException();
    }

    /// <summary>
    /// Releases resources used by the wrapped stream.
    /// </summary>
    /// <param name="disposing">true when called from <see cref="Dispose()"/>; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _innerStream.Dispose();
        }

        base.Dispose(disposing);
    }
}
