using System.Buffers.Binary;

namespace Aspose.PSD.FileFormats.Psd;

/// <summary>
/// Reads big-endian data from a stream.
/// Used for parsing PSD file format which uses big-endian byte order.
/// </summary>
internal sealed class BigEndianReader : IDisposable
{
    /// <summary>
    /// Stores the underlying source stream.
    /// </summary>
    private readonly Stream _stream;

    /// <summary>
    /// Indicates whether disposing the reader should leave the stream open.
    /// </summary>
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
        int totalRead = 0;

        while (totalRead < count)
        {
            int read = _stream.Read(buffer, totalRead, count - totalRead);
            if (read == 0)
            {
                throw new EndOfStreamException();
            }

            totalRead += read;
        }

        return buffer;
    }

    /// <summary>
    /// Reads a 16-bit signed integer in big-endian format.
    /// </summary>
    /// <returns>The 16-bit signed integer read from the stream.</returns>
    public short ReadInt16()
    {
        Span<byte> buffer = stackalloc byte[sizeof(short)];
        ReadExactly(buffer);
        return BinaryPrimitives.ReadInt16BigEndian(buffer);
    }

    /// <summary>
    /// Reads a 16-bit unsigned integer in big-endian format.
    /// </summary>
    /// <returns>The 16-bit unsigned integer read from the stream.</returns>
    public ushort ReadUInt16()
    {
        Span<byte> buffer = stackalloc byte[sizeof(ushort)];
        ReadExactly(buffer);
        return BinaryPrimitives.ReadUInt16BigEndian(buffer);
    }

    /// <summary>
    /// Reads a 32-bit signed integer in big-endian format.
    /// </summary>
    /// <returns>The 32-bit signed integer read from the stream.</returns>
    public int ReadInt32()
    {
        Span<byte> buffer = stackalloc byte[sizeof(int)];
        ReadExactly(buffer);
        return BinaryPrimitives.ReadInt32BigEndian(buffer);
    }

    /// <summary>
    /// Reads a 32-bit unsigned integer in big-endian format.
    /// </summary>
    /// <returns>The 32-bit unsigned integer read from the stream.</returns>
    public uint ReadUInt32()
    {
        Span<byte> buffer = stackalloc byte[sizeof(uint)];
        ReadExactly(buffer);
        return BinaryPrimitives.ReadUInt32BigEndian(buffer);
    }

    /// <summary>
    /// Reads a 64-bit signed integer in big-endian format.
    /// </summary>
    /// <returns>The 64-bit signed integer read from the stream.</returns>
    public long ReadInt64()
    {
        Span<byte> buffer = stackalloc byte[sizeof(long)];
        ReadExactly(buffer);
        return BinaryPrimitives.ReadInt64BigEndian(buffer);
    }

    /// <summary>
    /// Reads a 64-bit unsigned integer in big-endian format.
    /// </summary>
    /// <returns>The 64-bit unsigned integer read from the stream.</returns>
    public ulong ReadUInt64()
    {
        Span<byte> buffer = stackalloc byte[sizeof(ulong)];
        ReadExactly(buffer);
        return BinaryPrimitives.ReadUInt64BigEndian(buffer);
    }

    /// <summary>
    /// Reads a 32-bit floating-point number in big-endian format.
    /// </summary>
    /// <returns>The 32-bit floating-point number read from the stream.</returns>
    public float ReadSingle()
    {
        Span<byte> buffer = stackalloc byte[sizeof(float)];
        ReadExactly(buffer);
        int value = BinaryPrimitives.ReadInt32BigEndian(buffer);
        return BitConverter.Int32BitsToSingle(value);
    }

    /// <summary>
    /// Reads a 64-bit floating-point number in big-endian format.
    /// </summary>
    /// <returns>The 64-bit floating-point number read from the stream.</returns>
    public double ReadDouble()
    {
        Span<byte> buffer = stackalloc byte[sizeof(double)];
        ReadExactly(buffer);
        long value = BinaryPrimitives.ReadInt64BigEndian(buffer);
        return BitConverter.Int64BitsToDouble(value);
    }

    /// <summary>
    /// Reads a PSD Pascal string whose length byte and payload are padded to a 4-byte boundary.
    /// </summary>
    /// <param name="maxLength">The maximum length of the string to read.</param>
    /// <returns>The string read from the stream.</returns>
    public string ReadPascalString(int maxLength = 256)
    {
        return ReadPascalStringAlignedTo4(maxLength);
    }

    /// <summary>
    /// Reads a PSD resource Pascal string whose length byte and payload are padded to a 2-byte boundary.
    /// </summary>
    /// <param name="maxLength">The maximum payload length to accept.</param>
    /// <returns>The decoded ASCII string.</returns>
    /// <exception cref="PsdLoadException">Thrown when the stored string length exceeds <paramref name="maxLength"/>.</exception>
    public string ReadPascalStringAlignedTo2(int maxLength = byte.MaxValue)
    {
        return ReadPascalStringAlignedTo(boundary: 2, maxLength);
    }

    /// <summary>
    /// Reads a PSD layer Pascal string whose length byte and payload are padded to a 4-byte boundary.
    /// </summary>
    /// <param name="maxLength">The maximum payload length to accept.</param>
    /// <returns>The decoded ASCII string.</returns>
    /// <exception cref="PsdLoadException">Thrown when the stored string length exceeds <paramref name="maxLength"/>.</exception>
    public string ReadPascalStringAlignedTo4(int maxLength = byte.MaxValue)
    {
        return ReadPascalStringAlignedTo(boundary: 4, maxLength);
    }

    /// <summary>
    /// Reads a Pascal string and consumes alignment padding according to the enclosing PSD structure.
    /// </summary>
    /// <param name="boundary">The byte boundary used by the enclosing structure.</param>
    /// <param name="maxLength">The maximum payload length to accept.</param>
    /// <returns>The decoded ASCII string.</returns>
    private string ReadPascalStringAlignedTo(int boundary, int maxLength)
    {
        int length = ReadByte();
        if (length > maxLength)
        {
            throw new PsdLoadException($"PSD Pascal string length {length} exceeds the supported maximum {maxLength}.");
        }

        string result = string.Empty;
        if (length > 0)
        {
            byte[] bytes = ReadBytes(length);
            result = System.Text.Encoding.ASCII.GetString(bytes);
        }

        int padding = GetPaddingLength(length, boundary);
        if (padding > 0)
        {
            ReadBytes(padding);
        }

        return result;
    }

    /// <summary>
    /// Reads exactly the requested number of bytes into a stack or array span.
    /// </summary>
    /// <param name="buffer">The destination buffer.</param>
    /// <exception cref="EndOfStreamException">Thrown when the stream ends before the buffer is filled.</exception>
    private void ReadExactly(Span<byte> buffer)
    {
        int totalRead = 0;
        while (totalRead < buffer.Length)
        {
            int read = _stream.Read(buffer[totalRead..]);
            if (read == 0)
            {
                throw new EndOfStreamException();
            }

            totalRead += read;
        }
    }

    /// <summary>
    /// Calculates padding after a Pascal string length byte and payload.
    /// </summary>
    /// <param name="payloadLength">The stored Pascal string payload length.</param>
    /// <param name="boundary">The byte boundary used by the enclosing structure.</param>
    /// <returns>The number of padding bytes to consume.</returns>
    private static int GetPaddingLength(int payloadLength, int boundary)
    {
        return (boundary - ((payloadLength + 1) % boundary)) % boundary;
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
