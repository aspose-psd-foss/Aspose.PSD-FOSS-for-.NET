namespace Aspose.PSD.FileFormats.Psd;

/// <summary>
/// Provides helper methods for reading big-endian primitive values from byte arrays.
/// </summary>
internal static class BigEndianBitConverter
{
    /// <summary>
    /// Reads a 32-bit signed integer in big-endian format from the specified byte array.
    /// </summary>
    /// <param name="bytes">The source byte array.</param>
    /// <param name="offset">The zero-based offset of the integer.</param>
    /// <returns>The parsed 32-bit signed integer.</returns>
    public static int ToInt32(byte[] bytes, int offset)
    {
        return (bytes[offset] << 24) |
               (bytes[offset + 1] << 16) |
               (bytes[offset + 2] << 8) |
               bytes[offset + 3];
    }
}
