namespace Aspose.PSD.FileFormats.Psd;

/// <summary>
/// Provides bounded reads for PSD/PSB sections that are stored in memory by this implementation.
/// </summary>
internal static class PsdSectionReader
{
    /// <summary>
    /// Reads a declared section payload after validating stream boundaries and the in-memory size limit.
    /// </summary>
    /// <param name="reader">The reader positioned at the start of the section payload.</param>
    /// <param name="length">The declared payload length.</param>
    /// <param name="sectionName">The PSD/PSB section name used in error messages.</param>
    /// <returns>The section payload bytes.</returns>
    /// <exception cref="PsdLoadException">Thrown when the declared length cannot be read safely.</exception>
    public static byte[] ReadBytes(BigEndianReader reader, ulong length, string sectionName)
    {
        int boundedLength = GetMemoryBackedLength(reader, length, sectionName);
        return boundedLength == 0 ? [] : reader.ReadBytes(boundedLength);
    }

    /// <summary>
    /// Validates a declared section length and returns the equivalent in-memory array size.
    /// </summary>
    /// <param name="reader">The reader positioned at the start of the section payload.</param>
    /// <param name="length">The declared payload length.</param>
    /// <param name="sectionName">The PSD/PSB section name used in error messages.</param>
    /// <returns>The validated length as an <see cref="int"/>.</returns>
    /// <exception cref="PsdLoadException">Thrown when the declared length cannot be represented or exceeds available bytes.</exception>
    public static int GetMemoryBackedLength(BigEndianReader reader, ulong length, string sectionName)
    {
        if (length > int.MaxValue)
        {
            throw new PsdLoadException($"{sectionName} length {length} exceeds the current in-memory parser limit of {int.MaxValue} bytes.");
        }

        ulong remaining = GetRemainingBytes(reader, sectionName);
        if (length > remaining)
        {
            throw new PsdLoadException($"{sectionName} length {length} exceeds the remaining stream data ({remaining} bytes).");
        }

        return (int)length;
    }

    /// <summary>
    /// Validates a signed length field that belongs to a bounded section.
    /// </summary>
    /// <param name="length">The declared signed length.</param>
    /// <param name="sectionName">The PSD/PSB section name used in error messages.</param>
    /// <returns>The non-negative length.</returns>
    /// <exception cref="PsdLoadException">Thrown when the length is negative.</exception>
    public static long ValidateSignedLength(long length, string sectionName)
    {
        if (length < 0)
        {
            throw new PsdLoadException($"{sectionName} length cannot be negative.");
        }

        return length;
    }

    /// <summary>
    /// Validates that a nested payload stays inside the already-bounded enclosing section.
    /// </summary>
    /// <param name="reader">The reader positioned at the nested payload start.</param>
    /// <param name="length">The nested payload length.</param>
    /// <param name="sectionEnd">The byte position of the enclosing section end.</param>
    /// <param name="sectionName">The PSD/PSB subsection name used in error messages.</param>
    /// <returns>The validated length as an <see cref="int"/>.</returns>
    /// <exception cref="PsdLoadException">Thrown when the nested payload exceeds its enclosing section.</exception>
    public static int GetNestedMemoryBackedLength(BigEndianReader reader, ulong length, long sectionEnd, string sectionName)
    {
        if (length > int.MaxValue)
        {
            throw new PsdLoadException($"{sectionName} length {length} exceeds the current in-memory parser limit of {int.MaxValue} bytes.");
        }

        long remaining = sectionEnd - reader.Position;
        if (remaining < 0 || length > (ulong)remaining)
        {
            throw new PsdLoadException($"{sectionName} length {length} exceeds the enclosing section boundary.");
        }

        return (int)length;
    }

    /// <summary>
    /// Returns the remaining bytes in a seekable reader stream.
    /// </summary>
    /// <param name="reader">The reader to inspect.</param>
    /// <param name="sectionName">The PSD/PSB section name used in error messages.</param>
    /// <returns>The number of remaining bytes.</returns>
    /// <exception cref="PsdLoadException">Thrown when the reader position is outside the stream.</exception>
    private static ulong GetRemainingBytes(BigEndianReader reader, string sectionName)
    {
        long remaining = reader.Length - reader.Position;
        if (remaining < 0)
        {
            throw new PsdLoadException($"{sectionName} reader position is beyond the stream length.");
        }

        return (ulong)remaining;
    }
}
