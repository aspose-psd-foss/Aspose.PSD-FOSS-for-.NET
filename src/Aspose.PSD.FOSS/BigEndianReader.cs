namespace Aspose.PSD.FOSS;

/// <summary>
/// Reads big-endian data from a stream.
/// Used for parsing PSD file format which uses big-endian byte order.
/// </summary>
internal sealed class BigEndianReader : IDisposable
{
    private readonly Stream _stream;
    private readonly bool _leaveOpen;

    /// <summary>
    /// Gets the current position within the stream.
    /// </summary>
    public long Position => _stream.Position;

    /// <summary>
    /// Gets the total length of the stream.
    /// </summary>
    public long Length => _stream.Length;

    /// <summary>
    /// Initializes a new instance of the <see cref="BigEndianReader"/> class.
    /// </summary>
    /// <param name="stream">The stream to read from.</param>
    /// <param name="leaveOpen">true to keep the stream open; otherwise, false.</param>
    public BigEndianReader(Stream stream, bool leaveOpen = false)
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
    /// Reads a single byte from the stream.
    /// </summary>
    /// <returns>The byte read from the stream.</returns>
    /// <exception cref="EndOfStreamException">Thrown when the end of the stream is reached.</exception>
    public byte ReadByte()
    {
        int b = _stream.ReadByte();
        if (b == -1) throw new EndOfStreamException();
        return (byte)b;
    }

    /// <summary>
    /// Reads a signed byte (sbyte) from the stream.
    /// </summary>
    /// <returns>The sbyte read from the stream.</returns>
    /// <exception cref="EndOfStreamException">Thrown when the end of the stream is reached.</exception>
    public sbyte ReadSByte()
    {
        int b = _stream.ReadByte();
        if (b == -1) throw new EndOfStreamException();
        return (sbyte)b;
    }

    /// <summary>
    /// Reads a specified number of bytes from the stream.
    /// </summary>
    /// <param name="count">The number of bytes to read.</param>
    /// <returns>The byte array containing the data read from the stream.</returns>
    /// <exception cref="EndOfStreamException">Thrown when the end of the stream is reached before reading the requested count.</exception>
    public byte[] ReadBytes(int count)
    {
        byte[] buffer = new byte[count];
        int read = _stream.Read(buffer, 0, count);
        if (read < count) throw new EndOfStreamException();
        return buffer;
    }

    /// <summary>
    /// Reads a 16-bit signed integer in big-endian format.
    /// </summary>
    /// <returns>The 16-bit signed integer read from the stream.</returns>
    public short ReadInt16()
    {
        byte[] buffer = ReadBytes(2);
        return (short)((buffer[0] << 8) | buffer[1]);
    }

    /// <summary>
    /// Reads a 16-bit unsigned integer in big-endian format.
    /// </summary>
    /// <returns>The 16-bit unsigned integer read from the stream.</returns>
    public ushort ReadUInt16()
    {
        byte[] buffer = ReadBytes(2);
        return (ushort)((buffer[0] << 8) | buffer[1]);
    }

    /// <summary>
    /// Reads a 32-bit signed integer in big-endian format.
    /// </summary>
    /// <returns>The 32-bit signed integer read from the stream.</returns>
    public int ReadInt32()
    {
        byte[] buffer = ReadBytes(4);
        return (buffer[0] << 24) | (buffer[1] << 16) | (buffer[2] << 8) | buffer[3];
    }

    /// <summary>
    /// Reads a 32-bit unsigned integer in big-endian format.
    /// </summary>
    /// <returns>The 32-bit unsigned integer read from the stream.</returns>
    public uint ReadUInt32()
    {
        byte[] buffer = ReadBytes(4);
        return ((uint)buffer[0] << 24) | ((uint)buffer[1] << 16) | ((uint)buffer[2] << 8) | buffer[3];
    }

    /// <summary>
    /// Reads a 64-bit signed integer in big-endian format.
    /// </summary>
    /// <returns>The 64-bit signed integer read from the stream.</returns>
    public long ReadInt64()
    {
        byte[] buffer = ReadBytes(8);
        return ((long)buffer[0] << 56) | ((long)buffer[1] << 48) | ((long)buffer[2] << 40) | ((long)buffer[3] << 32) |
               ((long)buffer[4] << 24) | ((long)buffer[5] << 16) | ((long)buffer[6] << 8) | buffer[7];
    }

    /// <summary>
    /// Reads a 64-bit unsigned integer in big-endian format.
    /// </summary>
    /// <returns>The 64-bit unsigned integer read from the stream.</returns>
    public ulong ReadUInt64()
    {
        byte[] buffer = ReadBytes(8);
        return ((ulong)buffer[0] << 56) | ((ulong)buffer[1] << 48) | ((ulong)buffer[2] << 40) | ((ulong)buffer[3] << 32) |
               ((ulong)buffer[4] << 24) | ((ulong)buffer[5] << 16) | ((ulong)buffer[6] << 8) | buffer[7];
    }

    /// <summary>
    /// Reads a 32-bit floating-point number in big-endian format.
    /// </summary>
    /// <returns>The 32-bit floating-point number read from the stream.</returns>
    public float ReadSingle()
    {
        byte[] bytes = ReadBytes(4);
        if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
        return BitConverter.ToSingle(bytes, 0);
    }

    /// <summary>
    /// Reads a 64-bit floating-point number in big-endian format.
    /// </summary>
    /// <returns>The 64-bit floating-point number read from the stream.</returns>
    public double ReadDouble()
    {
        byte[] bytes = ReadBytes(8);
        if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
        return BitConverter.ToDouble(bytes, 0);
    }

    /// <summary>
    /// Reads a Pascal-style string from the stream.
    /// </summary>
    /// <param name="maxLength">The maximum length of the string to read.</param>
    /// <returns>The string read from the stream.</returns>
    public string ReadPascalString(int maxLength = 256)
    {
        int length = ReadByte();
        if (length == 0) return string.Empty;

        byte[] bytes = ReadBytes(length);
        string result = System.Text.Encoding.ASCII.GetString(bytes);

        int padding = (4 - (length + 1) % 4) % 4;
        if (padding > 0) ReadBytes(padding);

        return result;
    }

    /// <summary>
    /// Skips the specified number of bytes in the stream.
    /// </summary>
    /// <param name="count">The number of bytes to skip.</param>
    public void Skip(int count)
    {
        _stream.Seek(count, SeekOrigin.Current);
    }

    /// <summary>
    /// Releases the stream resources.
    /// </summary>
    public void Dispose()
    {
        if (!_leaveOpen)
            _stream.Dispose();
    }
}
