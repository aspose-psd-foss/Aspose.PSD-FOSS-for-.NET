namespace Aspose.PSD.FOSS;

internal sealed class BigEndianWriter : IDisposable
{
    private readonly Stream _stream;
    private readonly bool _leaveOpen;
    private bool _disposed;

    public BigEndianWriter(Stream stream, bool leaveOpen = false)
    {
        _stream = stream;
        _leaveOpen = leaveOpen;
    }

    public long Position => _stream.Position;

    public void Seek(long offset, SeekOrigin origin)
    {
        _stream.Seek(offset, origin);
    }

    public void Write(byte value)
    {
        _stream.WriteByte(value);
    }

    public void Write(byte[] buffer)
    {
        _stream.Write(buffer, 0, buffer.Length);
    }

    public void Write(byte[] buffer, int offset, int count)
    {
        _stream.Write(buffer, offset, count);
    }

    public void Write(sbyte value)
    {
        _stream.WriteByte((byte)value);
    }

    public void Write(short value)
    {
        byte[] buffer = new byte[2];
        buffer[0] = (byte)((value >> 8) & 0xFF);
        buffer[1] = (byte)(value & 0xFF);
        _stream.Write(buffer, 0, 2);
    }

    public void Write(ushort value)
    {
        byte[] buffer = new byte[2];
        buffer[0] = (byte)((value >> 8) & 0xFF);
        buffer[1] = (byte)(value & 0xFF);
        _stream.Write(buffer, 0, 2);
    }

    public void Write(int value)
    {
        byte[] buffer = new byte[4];
        buffer[0] = (byte)((value >> 24) & 0xFF);
        buffer[1] = (byte)((value >> 16) & 0xFF);
        buffer[2] = (byte)((value >> 8) & 0xFF);
        buffer[3] = (byte)(value & 0xFF);
        _stream.Write(buffer, 0, 4);
    }

    public void Write(uint value)
    {
        byte[] buffer = new byte[4];
        buffer[0] = (byte)((value >> 24) & 0xFF);
        buffer[1] = (byte)((value >> 16) & 0xFF);
        buffer[2] = (byte)((value >> 8) & 0xFF);
        buffer[3] = (byte)(value & 0xFF);
        _stream.Write(buffer, 0, 4);
    }

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

    public void WritePascalString(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
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

    public void WriteAllBytes(byte[] data)
    {
        Write(data);
    }

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
