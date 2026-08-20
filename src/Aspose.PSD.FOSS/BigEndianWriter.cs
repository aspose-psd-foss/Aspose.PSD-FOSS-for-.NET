using System.Buffers.Binary;

namespace Aspose.PSD.FOSS;

/// <summary>
/// Writes big-endian data to a stream.
/// Used for writing PSD file format which uses big-endian byte order.
/// </summary>
internal sealed class BigEndianWriter : IDisposable
{
    /// <summary>
    /// Stores the underlying destination stream.
    /// </summary>
    private readonly Stream _stream;

    /// <summary>
    /// Indicates whether disposing the writer should leave the stream open.
    /// </summary>
    private readonly bool _leaveOpen;

    /// <summary>
    /// Tracks whether the writer has already been disposed.
    /// </summary>
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
        Span<byte> buffer = stackalloc byte[sizeof(short)];
        BinaryPrimitives.WriteInt16BigEndian(buffer, value);
        _stream.Write(buffer);
    }

    /// <summary>
    /// Writes a 16-bit unsigned integer in big-endian format.
    /// </summary>
    /// <param name="value">The 16-bit unsigned integer to write.</param>
    public void Write(ushort value)
    {
        Span<byte> buffer = stackalloc byte[sizeof(ushort)];
        BinaryPrimitives.WriteUInt16BigEndian(buffer, value);
        _stream.Write(buffer);
    }

    /// <summary>
    /// Writes a 32-bit signed integer in big-endian format.
    /// </summary>
    /// <param name="value">The 32-bit signed integer to write.</param>
    public void Write(int value)
    {
        Span<byte> buffer = stackalloc byte[sizeof(int)];
        BinaryPrimitives.WriteInt32BigEndian(buffer, value);
        _stream.Write(buffer);
    }

    /// <summary>
    /// Writes a 32-bit unsigned integer in big-endian format.
    /// </summary>
    /// <param name="value">The 32-bit unsigned integer to write.</param>
    public void Write(uint value)
    {
        Span<byte> buffer = stackalloc byte[sizeof(uint)];
        BinaryPrimitives.WriteUInt32BigEndian(buffer, value);
        _stream.Write(buffer);
    }

    /// <summary>
    /// Writes a 64-bit signed integer in big-endian format.
    /// </summary>
    /// <param name="value">The 64-bit signed integer to write.</param>
    public void Write(long value)
    {
        Span<byte> buffer = stackalloc byte[sizeof(long)];
        BinaryPrimitives.WriteInt64BigEndian(buffer, value);
        _stream.Write(buffer);
    }

    /// <summary>
    /// Writes a 64-bit unsigned integer in big-endian format.
    /// </summary>
    /// <param name="value">The 64-bit unsigned integer to write.</param>
    public void Write(ulong value)
    {
        Span<byte> buffer = stackalloc byte[sizeof(ulong)];
        BinaryPrimitives.WriteUInt64BigEndian(buffer, value);
        _stream.Write(buffer);
    }

    /// <summary>
    /// Writes a PSD Pascal string whose length byte and payload are padded to a 4-byte boundary.
    /// </summary>
    /// <param name="value">The string to write.</param>
    public void WritePascalString(string value)
    {
        WritePascalStringAlignedTo4(value);
    }

    /// <summary>
    /// Writes a PSD resource Pascal string whose length byte and payload are padded to a 2-byte boundary.
    /// </summary>
    /// <param name="value">The ASCII string to write.</param>
    /// <exception cref="PsdSaveException">Thrown when the encoded string is too long for a PSD Pascal string.</exception>
    public void WritePascalStringAlignedTo2(string value)
    {
        WritePascalStringAlignedTo(value, boundary: 2);
    }

    /// <summary>
    /// Writes a PSD layer Pascal string whose length byte and payload are padded to a 4-byte boundary.
    /// </summary>
    /// <param name="value">The ASCII string to write.</param>
    /// <exception cref="PsdSaveException">Thrown when the encoded string is too long for a PSD Pascal string.</exception>
    public void WritePascalStringAlignedTo4(string value)
    {
        WritePascalStringAlignedTo(value, boundary: 4);
    }

    /// <summary>
    /// Returns the stored byte count for a PSD Pascal string aligned to a 4-byte boundary.
    /// </summary>
    /// <param name="value">The ASCII string to measure.</param>
    /// <returns>The length byte, payload, and padding byte count.</returns>
    public static int GetPascalStringStorageLengthAlignedTo4(string value)
    {
        int length = string.IsNullOrEmpty(value) ? 0 : System.Text.Encoding.ASCII.GetByteCount(value);
        return 1 + length + GetPaddingLength(length, boundary: 4);
    }

    /// <summary>
    /// Writes a Pascal string and pads it according to the enclosing PSD structure.
    /// </summary>
    /// <param name="value">The ASCII string to write.</param>
    /// <param name="boundary">The byte boundary used by the enclosing structure.</param>
    private void WritePascalStringAlignedTo(string value, int boundary)
    {
        byte[] bytes = System.Text.Encoding.ASCII.GetBytes(value);
        if (bytes.Length > byte.MaxValue)
        {
            throw new PsdSaveException($"PSD Pascal string length {bytes.Length} exceeds the supported maximum {byte.MaxValue}.");
        }

        Write((byte)bytes.Length);
        if (bytes.Length > 0)
        {
            Write(bytes);
        }

        int padding = GetPaddingLength(bytes.Length, boundary);
        for (int i = 0; i < padding; i++)
        {
            Write((byte)0);
        }
    }

    /// <summary>
    /// Calculates padding after a Pascal string length byte and payload.
    /// </summary>
    /// <param name="payloadLength">The stored Pascal string payload length.</param>
    /// <param name="boundary">The byte boundary used by the enclosing structure.</param>
    /// <returns>The number of padding bytes to write.</returns>
    private static int GetPaddingLength(int payloadLength, int boundary)
    {
        return (boundary - ((payloadLength + 1) % boundary)) % boundary;
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
