using System.IO;
using NUnit.Framework;

namespace Aspose.PSD.FOSS.Tests;

/// <summary>
/// Provides common temporary-file helpers and binary fixture builders for PSD tests.
/// </summary>
public abstract class PsdTestFixtureBase : IDisposable
{
    /// <summary>
    /// Stores the temporary directory used for per-test missing-file and output scenarios.
    /// </summary>
    protected readonly string _testDir;


    /// <summary>
    /// Initializes a new instance of the <see cref="PsdTestFixtureBase"/> class.
    /// Creates a temporary test directory for output files.
    /// </summary>
    protected PsdTestFixtureBase()
    {
        _testDir = Path.Combine(Path.GetTempPath(), $"AsposePsdTest_{Guid.NewGuid()}");
        Directory.CreateDirectory(_testDir);
    }


    /// <summary>
    /// Releases all resources used by the test fixture.
    /// Deletes the temporary test directory and its contents.
    /// </summary>
    public void Dispose()
    {
        try
        {
            Directory.Delete(_testDir, recursive: true);
        }
        catch
        {
        }
    }


    /// <summary>
    /// Gets a stable artifact path for the current test under the NUnit work directory.
    /// </summary>
    /// <param name="fileName">The artifact file name.</param>
    /// <returns>The full output path for the artifact.</returns>
    protected static string GetPersistentArtifactPath(string fileName)
    {
        string testName = SanitizePathSegment(TestContext.CurrentContext.Test.Name);
        string artifactDirectory = Path.Combine(
            TestContext.CurrentContext.WorkDirectory,
            "artifacts",
            nameof(PsdTestFixtureBase),
            testName);

        Directory.CreateDirectory(artifactDirectory);
        return Path.Combine(artifactDirectory, fileName);
    }


    /// <summary>
    /// Resolves a test fixture path from the copied test output or the repository testdata folder.
    /// </summary>
    /// <param name="fileName">The fixture file name.</param>
    /// <returns>The resolved fixture path.</returns>
    protected static string GetTestDataPath(string fileName)
    {
        string testDirectoryPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "testdata", fileName);
        if (File.Exists(testDirectoryPath))
        {
            return testDirectoryPath;
        }

        string repositoryPath = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, "../../../testdata", fileName));
        return repositoryPath;
    }


    /// <summary>
    /// Writes the artifact directory path for the current test to the NUnit output log.
    /// </summary>
    /// <param name="outputFile">The saved artifact file path.</param>
    protected static void LogArtifactDirectory(string outputFile)
    {
        string outputDirectory = Path.GetDirectoryName(outputFile) ?? string.Empty;
        TestContext.Out.WriteLine($"Saved test artifact directory: {outputDirectory}");
    }


    /// <summary>
    /// Replaces characters that are invalid in file-system path segments.
    /// </summary>
    /// <param name="value">The path segment candidate.</param>
    /// <returns>A file-system-safe path segment.</returns>
    protected static string SanitizePathSegment(string value)
    {
        char[] invalidCharacters = Path.GetInvalidFileNameChars();
        var builder = new System.Text.StringBuilder(value.Length);

        foreach (char character in value)
        {
            builder.Append(Array.IndexOf(invalidCharacters, character) >= 0 ? '_' : character);
        }

        return builder.ToString();
    }


    /// <summary>
    /// Asserts that saving a fixture without mutations produces byte-for-byte identical output.
    /// </summary>
    /// <param name="fileName">The fixture file name.</param>
    protected static void AssertByteExactRoundTrip(string fileName)
    {
        string testFile = GetTestDataPath(fileName);
        string outputFile = GetPersistentArtifactPath(fileName);
        byte[] originalBytes = File.ReadAllBytes(testFile);

        using var image = PsdImage.Load(testFile);
        image.Save(outputFile);
        LogArtifactDirectory(outputFile);

        byte[] savedBytes = File.ReadAllBytes(outputFile);
        Assert.That(savedBytes, Is.EqualTo(originalBytes));
    }


    /// <summary>
    /// Asserts that renaming a layer persists after saving and reloading a fixture.
    /// </summary>
    /// <param name="fileName">The fixture file name.</param>
    /// <param name="layerIndex">The zero-based layer index to rename.</param>
    /// <param name="newName">The replacement layer name.</param>
    protected static void AssertRenameSave(string fileName, int layerIndex, string newName)
    {
        string testFile = GetTestDataPath(fileName);
        string outputFile = GetPersistentArtifactPath($"renamed_{fileName}");

        using var image = PsdImage.Load(testFile);
        image.Layers[layerIndex].Name = newName;
        image.Save(outputFile);
        LogArtifactDirectory(outputFile);

        using var reloaded = PsdImage.Load(outputFile);
        Assert.That(reloaded.Layers[layerIndex].Name, Is.EqualTo(newName));
    }


    /// <summary>
    /// Writes a 32-bit unsigned integer into the specified buffer in big-endian byte order.
    /// </summary>
    /// <param name="buffer">The target byte buffer.</param>
    /// <param name="offset">The offset at which to write the value.</param>
    /// <param name="value">The value to encode.</param>
    protected static void WriteUInt32BigEndian(byte[] buffer, int offset, uint value)
    {
        buffer[offset] = (byte)(value >> 24);
        buffer[offset + 1] = (byte)(value >> 16);
        buffer[offset + 2] = (byte)(value >> 8);
        buffer[offset + 3] = (byte)value;
    }


    /// <summary>
    /// Builds a minimal PSD or PSB header byte sequence for parser-boundary tests.
    /// </summary>
    /// <param name="version">The PSD container version to encode.</param>
    /// <param name="channels">The channel count to encode.</param>
    /// <param name="width">The document width to encode.</param>
    /// <param name="height">The document height to encode.</param>
    /// <param name="bitDepth">The bits per channel to encode.</param>
    /// <param name="colorMode">The color mode to encode.</param>
    /// <returns>The encoded header bytes.</returns>
    protected static byte[] BuildHeaderBytes(
        ushort version,
        ushort channels = 3,
        int width = 1,
        int height = 1,
        ushort bitDepth = 8,
        ColorModes colorMode = ColorModes.Rgb)
    {
        using var stream = new MemoryStream();
        using var writer = new BigEndianWriter(stream, leaveOpen: true);

        stream.Write(System.Text.Encoding.ASCII.GetBytes("8BPS"));
        writer.Write(version);
        writer.Write(new byte[6]);
        writer.Write(channels);
        writer.Write(height);
        writer.Write(width);
        writer.Write(bitDepth);
        writer.Write((ushort)colorMode);

        return stream.ToArray();
    }


    /// <summary>
    /// Builds a Color Mode Data section with a 4-byte length field and caller-provided payload.
    /// </summary>
    /// <param name="payload">The Color Mode Data payload bytes.</param>
    /// <returns>The encoded section bytes.</returns>
    protected static byte[] BuildColorDataSection(byte[] payload)
    {
        using var stream = new MemoryStream();
        using var writer = new BigEndianWriter(stream, leaveOpen: true);
        writer.Write((uint)payload.Length);
        writer.Write(payload);
        return stream.ToArray();
    }


    /// <summary>
    /// Builds an Image Data section with a compression field and caller-provided payload.
    /// </summary>
    /// <param name="compression">The compression mode to encode.</param>
    /// <param name="payload">The image data payload bytes after the compression field.</param>
    /// <returns>The encoded image data section bytes.</returns>
    protected static byte[] BuildImageDataSection(CompressionMethod compression, byte[] payload)
    {
        using var stream = new MemoryStream();
        using var writer = new BigEndianWriter(stream, leaveOpen: true);
        writer.Write((ushort)compression);
        writer.Write(payload);
        return stream.ToArray();
    }


    /// <summary>
    /// Builds a deterministic 256-entry indexed palette payload for Color Mode Data tests.
    /// </summary>
    /// <returns>The 768-byte indexed palette payload.</returns>
    protected static byte[] BuildIndexedPalettePayload()
    {
        byte[] payload = new byte[IndexedColorPalette.ExpectedRawLength];
        for (int i = 0; i < 256; i++)
        {
            payload[i] = (byte)i;
            payload[i + 256] = (byte)(255 - i);
            payload[i + 512] = (byte)(128 + (i % 64));
        }

        return payload;
    }


    /// <summary>
    /// Builds an Image Resources payload containing one or more unknown resource blocks.
    /// </summary>
    /// <param name="resources">The resource identifiers, Pascal names, and payload bytes to encode.</param>
    /// <returns>The encoded Image Resources payload without the outer section length field.</returns>
    protected static byte[] BuildResourcesPayload(params (short resourceId, string name, byte[] data)[] resources)
    {
        using var stream = new MemoryStream();

        void WriteInt16(short value)
        {
            stream.WriteByte((byte)(value >> 8));
            stream.WriteByte((byte)value);
        }

        void WriteInt32(int value)
        {
            stream.WriteByte((byte)(value >> 24));
            stream.WriteByte((byte)(value >> 16));
            stream.WriteByte((byte)(value >> 8));
            stream.WriteByte((byte)value);
        }

        foreach ((short resourceId, string name, byte[] data) in resources)
        {
            stream.Write(System.Text.Encoding.ASCII.GetBytes("8BIM"));
            WriteInt16(resourceId);

            byte[] nameBytes = System.Text.Encoding.ASCII.GetBytes(name);
            stream.WriteByte((byte)nameBytes.Length);
            stream.Write(nameBytes);
            if ((nameBytes.Length + 1) % 2 != 0)
            {
                stream.WriteByte(0);
            }

            WriteInt32(data.Length);
            stream.Write(data);
            if (data.Length % 2 == 1)
            {
                stream.WriteByte(0);
            }
        }

        return stream.ToArray();
    }


    /// <summary>
    /// Builds a minimal PSD layer record with caller-controlled extra data bytes.
    /// </summary>
    /// <param name="extraData">The layer extra data payload to append after the fixed layer record fields.</param>
    /// <returns>The encoded layer record bytes.</returns>
    protected static byte[] BuildLayerRecordBytesWithExtraData(byte[] extraData)
    {
        using var stream = new MemoryStream();
        using var writer = new BigEndianWriter(stream, leaveOpen: true);

        writer.Write(0);
        writer.Write(0);
        writer.Write(1);
        writer.Write(1);
        writer.Write((ushort)0);
        writer.Write(0x3842494D);
        writer.Write(System.Text.Encoding.ASCII.GetBytes("norm"));
        writer.Write((byte)255);
        writer.Write((byte)0);
        writer.Write((byte)0);
        writer.Write((byte)0);
        writer.Write(extraData.Length);
        writer.Write(extraData);

        return stream.ToArray();
    }


    /// <summary>
    /// Extracts the full Image Resources section, including its 4-byte length field.
    /// </summary>
    /// <param name="documentBytes">The complete PSD document bytes.</param>
    /// <returns>The raw Image Resources section bytes, including the section length field.</returns>
    protected static byte[] ExtractImageResourcesSection(byte[] documentBytes)
    {
        const int headerLength = 26;
        int colorModeLength = BigEndianBitConverter.ToInt32(documentBytes, headerLength);
        int resourcesLengthOffset = headerLength + 4 + colorModeLength;
        int resourcesPayloadLength = BigEndianBitConverter.ToInt32(documentBytes, resourcesLengthOffset);
        int totalSectionLength = 4 + resourcesPayloadLength;
        byte[] sectionBytes = new byte[totalSectionLength];
        Array.Copy(documentBytes, resourcesLengthOffset, sectionBytes, 0, totalSectionLength);
        return sectionBytes;
    }


    /// <summary>
    /// Reads the PSD layer info payload length field from a complete document byte array.
    /// </summary>
    /// <param name="documentBytes">The complete PSD document bytes.</param>
    /// <returns>The stored layer info payload length.</returns>
    protected static int ReadLayerInfoLength(byte[] documentBytes)
    {
        const int headerLength = 26;
        int colorModeLength = BigEndianBitConverter.ToInt32(documentBytes, headerLength);
        int resourcesLengthOffset = headerLength + 4 + colorModeLength;
        int resourcesPayloadLength = BigEndianBitConverter.ToInt32(documentBytes, resourcesLengthOffset);
        int layerAndMaskLengthOffset = resourcesLengthOffset + 4 + resourcesPayloadLength;
        int layerInfoLengthOffset = layerAndMaskLengthOffset + 4;
        return BigEndianBitConverter.ToInt32(documentBytes, layerInfoLengthOffset);
    }


    /// <summary>
    /// Reads the preserved Layer and Mask Information tail bytes from a complete PSD document.
    /// </summary>
    /// <param name="documentBytes">The complete PSD document bytes.</param>
    /// <returns>The global mask and trailing layer/mask bytes after the Layer Info subsection.</returns>
    protected static byte[] ReadLayerAndMaskTail(byte[] documentBytes)
    {
        const int headerLength = 26;
        int colorModeLength = BigEndianBitConverter.ToInt32(documentBytes, headerLength);
        int resourcesLengthOffset = headerLength + 4 + colorModeLength;
        int resourcesPayloadLength = BigEndianBitConverter.ToInt32(documentBytes, resourcesLengthOffset);
        int layerAndMaskLengthOffset = resourcesLengthOffset + 4 + resourcesPayloadLength;
        int layerAndMaskLength = BigEndianBitConverter.ToInt32(documentBytes, layerAndMaskLengthOffset);
        int layerInfoLength = ReadLayerInfoLength(documentBytes);
        int tailOffset = layerAndMaskLengthOffset + 4 + 4 + layerInfoLength;
        int tailLength = (layerAndMaskLengthOffset + 4 + layerAndMaskLength) - tailOffset;

        if (tailLength <= 0)
        {
            return [];
        }

        byte[] tailBytes = new byte[tailLength];
        Buffer.BlockCopy(documentBytes, tailOffset, tailBytes, 0, tailLength);
        return tailBytes;
    }


    /// <summary>
    /// Builds a synthetic PSB layer record with 64-bit channel lengths.
    /// </summary>
    /// <returns>The encoded PSB layer record bytes.</returns>
    protected static byte[] BuildPsbLayerRecordBytes()
    {
        using var stream = new MemoryStream();
        using var writer = new BigEndianWriter(stream, leaveOpen: true);

        writer.Write(0);
        writer.Write(0);
        writer.Write(1);
        writer.Write(1);
        writer.Write((ushort)4);
        writer.Write((short)-1);
        writer.Write((ulong)3);
        writer.Write((short)0);
        writer.Write((ulong)3);
        writer.Write((short)1);
        writer.Write((ulong)3);
        writer.Write((short)2);
        writer.Write((ulong)3);
        writer.Write(0x3842494D);
        writer.Write(System.Text.Encoding.ASCII.GetBytes("norm"));
        writer.Write((byte)200);
        writer.Write((byte)0);
        writer.Write((byte)0);
        writer.Write((byte)0);

        using var extraDataStream = new MemoryStream();
        using var extraDataWriter = new BigEndianWriter(extraDataStream, leaveOpen: true);
        extraDataWriter.Write((uint)0);
        extraDataWriter.Write((uint)0);
        extraDataWriter.WritePascalString("Layer 1");

        byte[] extraData = extraDataStream.ToArray();
        writer.Write(extraData.Length);
        writer.Write(extraData);

        return stream.ToArray();
    }
}
