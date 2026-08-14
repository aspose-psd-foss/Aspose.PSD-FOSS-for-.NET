namespace Aspose.PSD.FOSS;

/// <summary>
/// Writes big-endian data to a stream.
/// Used for writing PSD file format which uses big-endian byte order.
/// </summary>
internal sealed class BigEndianWriter : IDisposable
{
    private readonly Stream _stream;
    private readonly bool _leaveOpen;
    private bool _disposed;

    /// <summary>
    /// Gets the current position within the stream.
    /// </summary>
    public long Position => _stream.Position;

    /// <summary>
    /// Initializes a new instance of the <see cref="BigEndianWriter"/> class.
    /// </summary>
    /// <param name="stream">The stream to write to.</param>
    /// <param name="leaveOpen">true to keep the stream open; otherwise, false.</param>
    public BigEndianWriter(Stream stream, bool leaveOpen = false)
    {
        _stream = stream;
        _leaveOpen = leaveOpen;
    }

    /// <summary>
    /// Seeks to a position in the stream.
    /// </summary>
    /// <param name="offset">The byte offset relative to the origin.</param>
    /// <param name="origin">The reference point used to obtain the new position.</param>
    public void Seek(long offset, SeekOrigin origin)
    {
        _stream.Seek(offset, origin);
    }

    /// <summary>
    /// Writes a single byte to the stream.
    /// </summary>
    /// <param name="value">The byte value to write.</param>
    public void Write(byte value)
    {
        _stream.WriteByte(value);
    }

    /// <summary>
    /// Writes a byte array to the stream.
    /// </summary>
    /// <param name="buffer">The byte array to write.</param>
    public void Write(byte[] buffer)
    {
        _stream.Write(buffer, 0, buffer.Length);
    }

    /// <summary>
    /// Writes a subarray of bytes to the stream.
    /// </summary>
    /// <param name="buffer">The byte array containing the data to write.</param>
    /// <param name="offset">The zero-based byte offset in buffer at which to begin writing bytes.</param>
    /// <param name="count">The number of bytes to write.</param>
    public void Write(byte[] buffer, int offset, int count)
    {
        _stream.Write(buffer, offset, count);
    }

    /// <summary>
    /// Writes a signed byte (sbyte) to the stream.
    /// </summary>
    /// <param name="value">The sbyte value to write.</param>
    public void Write(sbyte value)
    {
        _stream.WriteByte((byte)value);
    }

    /// <summary>
    /// Writes a 16-bit signed integer in big-endian format.
    /// </summary>
    /// <param name="value">The 16-bit signed integer to write.</param>
    public void Write(short value)
    {
        byte[] buffer = new byte[2];
        buffer[0] = (byte)((value >> 8) & 0xFF);
        buffer[1] = (byte)(value & 0xFF);
        _stream.Write(buffer, 0, 2);
    }

    /// <summary>
    /// Writes a 16-bit unsigned integer in big-endian format.
    /// </summary>
    /// <param name="value">The 16-bit unsigned integer to write.</param>
    public void Write(ushort value)
    {
        byte[] buffer = new byte[2];
        buffer[0] = (byte)((value >> 8) & 0xFF);
        buffer[1] = (byte)(value & 0xFF);
        _stream.Write(buffer, 0, 2);
    }

    /// <summary>
    /// Writes a 32-bit signed integer in big-endian format.
    /// </summary>
    /// <param name="value">The 32-bit signed integer to write.</param>
    public void Write(int value)
    {
        byte[] buffer = new byte[4];
        buffer[0] = (byte)((value >> 24) & 0xFF);
        buffer[1] = (byte)((value >> 16) & 0xFF);
        buffer[2] = (byte)((value >> 8) & 0xFF);
        buffer[3] = (byte)(value & 0xFF);
        _stream.Write(buffer, 0, 4);
    }

    /// <summary>
    /// Writes a 32-bit unsigned integer in big-endian format.
    /// </summary>
    /// <param name="value">The 32-bit unsigned integer to write.</param>
    public void Write(uint value)
    {
        byte[] buffer = new byte[4];
        buffer[0] = (byte)((value >> 24) & 0xFF);
        buffer[1] = (byte)((value >> 16) & 0xFF);
        buffer[2] = (byte)((value >> 8) & 0xFF);
        buffer[3] = (byte)(value & 0xFF);
        _stream.Write(buffer, 0, 4);
    }

    /// <summary>
    /// Writes a 64-bit signed integer in big-endian format.
    /// </summary>
    /// <param name="value">The 64-bit signed integer to write.</param>
    public void Write(long value)
    {
        byte[] buffer = new byte[8];
        buffer[0] = (byte)((value >> 56) & 0xFF);
        buffer[1] = (byte)((value >> 48) & 0xFF);
        buffer[2] = (byte)((value >> 40) & 0xFF);
        buffer[3] = (byte)((value >> 32) & 0xFF);
        buffer[4] = (byte)((value >> 24) & 0xFF);
        buffer[5] = (byte)((value >> 16) & 0xFF);
        buffer[6] = (byte)((value >> 8) & 0xFF);
        buffer[7] = (byte)(value & 0xFF);
        _stream.Write(buffer, 0, 8);
    }

    /// <summary>
    /// Writes a 64-bit unsigned integer in big-endian format.
    /// </summary>
    /// <param name="value">The 64-bit unsigned integer to write.</param>
    public void Write(ulong value)
    {
        byte[] buffer = new byte[8];
        buffer[0] = (byte)((value >> 56) & 0xFF);
        buffer[1] = (byte)((value >> 48) & 0xFF);
        buffer[2] = (byte)((value >> 40) & 0xFF);
        buffer[3] = (byte)((value >> 32) & 0xFF);
        buffer[4] = (byte)((value >> 24) & 0xFF);
        buffer[5] = (byte)((value >> 16) & 0xFF);
        buffer[6] = (byte)((value >> 8) & 0xFF);
        buffer[7] = (byte)(value & 0xFF);
        _stream.Write(buffer, 0, 8);
    }

    /// <summary>
    /// Writes a Pascal-style string to the stream.
    /// </summary>
    /// <param name="value">The string to write.</param>
    public void WritePascalString(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            Write((byte)0);
            Write((byte)0);
            Write((byte)0);
            Write((byte)0);
            return;
        }

        byte[] bytes = System.Text.Encoding.ASCII.GetBytes(value);
        Write((byte)bytes.Length);
        Write(bytes);

        int padding = (4 - (bytes.Length + 1) % 4) % 4;
        for (int i = 0; i < padding; i++)
        {
            Write((byte)0);
        }
    }

    /// <summary>
    /// Writes all bytes from the specified array to the stream.
    /// </summary>
    /// <param name="data">The byte array to write.</param>
    public void WriteAllBytes(byte[] data)
    {
        Write(data);
    }

    /// <summary>
    /// Releases the stream resources.
    /// </summary>
    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        if (!_leaveOpen)
        {
            _stream.Dispose();
        }
    }
}
