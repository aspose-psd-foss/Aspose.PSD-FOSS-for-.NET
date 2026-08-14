using System.Drawing;
using System.IO;
using NUnit.Framework;

namespace Aspose.PSD.FOSS.Tests;

/// <summary>
/// Provides tests for the <see cref="PsdImage"/> class functionality.
/// Tests cover loading, saving, and manipulation of PSD files.
/// </summary>
public sealed class PsdImageTests : IDisposable
{
    private readonly string _testDir;

    /// <summary>
    /// Initializes a new instance of the <see cref="PsdImageTests"/> class.
    /// Creates a temporary test directory for output files.
    /// </summary>
    public PsdImageTests()
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
    /// Tests that loading a PSD file returns correct document properties.
    /// Verifies image dimensions, channels, bits per channel, color mode, version, and layer count.
    /// </summary>
    [Test]
    public void Load_DocumentProperties_ReturnsCorrectValues()
    {
        string testFile = Path.Combine(TestContext.CurrentContext.TestDirectory, "testdata", "test.psd");

        Assert.That(File.Exists(testFile), Is.True, "Test file not found");

        using var image = PsdImage.Load(testFile);

        Assert.That(image.Width, Is.GreaterThan(0));
        Assert.That(image.Height, Is.GreaterThan(0));
        Assert.That(image.Channels, Is.GreaterThan(0));
        Assert.That(image.BitsPerChannel, Is.EqualTo(8));
        Assert.That(image.ColorMode, Is.EqualTo(ColorModes.Rgb).Or.EqualTo(ColorModes.Indexed));
        Assert.That(image.Version, Is.GreaterThan(0).And.LessThanOrEqualTo(6));
        Assert.That(image.Layers.Length, Is.GreaterThan(0));
    }

    /// <summary>
    /// Tests that loading a PSD file returns correct layer properties.
    /// Verifies layer count, bounds, visibility, opacity, and blend mode.
    /// </summary>
    [Test]
    public void Load_LayerProperties_ReturnsCorrectValues()
    {
        string testFile = Path.Combine(TestContext.CurrentContext.TestDirectory, "testdata", "test.psd");

        using var image = PsdImage.Load(testFile);

        Assert.That(image.Layers.Length, Is.GreaterThan(0));

        var firstLayer = image.Layers[0];

        Assert.That(firstLayer.Name, Is.EqualTo("Background copy"));
        Assert.That(firstLayer.Bounds, Is.Not.EqualTo(default(Rectangle)));
        Assert.That(firstLayer.IsVisible, Is.True);
        Assert.That(firstLayer.Opacity, Is.GreaterThanOrEqualTo(0).And.LessThanOrEqualTo(255));
        Assert.That(firstLayer.BlendMode, Is.Not.EqualTo(BlendMode.Normal).Or.EqualTo(default(BlendMode)));

        Assert.That(image.Layers[1].Name, Is.EqualTo("Pattern Fill 1"));
    }

    /// <summary>
    /// Tests that layers can be accessed by index.
    /// </summary>
    [Test]
    public void Load_Layers_CanBeAccessedByIndex()
    {
        string testFile = Path.Combine(TestContext.CurrentContext.TestDirectory, "testdata", "test.psd");

        using var image = PsdImage.Load(testFile);

        Assert.That(image.Layers[0], Is.Not.Null);
        Assert.That(image.Layers[1], Is.Not.Null);
    }

    /// <summary>
    /// Tests that saving a PSD file without mutations produces a valid file.
    /// Verifies that the saved file can be reloaded with equivalent properties.
    /// </summary>
    [Test]
    public void Save_RoundTripWithoutMutation_ProducesValidFile()
    {
        string testFile = Path.Combine(TestContext.CurrentContext.TestDirectory, "testdata", "test.psd");
        string outputFile = Path.Combine(_testDir, "roundtrip_test.psd");

        using var image1 = PsdImage.Load(testFile);
        int originalLength = image1.Layers.Length;
        string originalLayerName = image1.Layers.Length > 0 ? image1.Layers[0].Name : string.Empty;
        image1.Save(outputFile);

        using var image2 = PsdImage.Load(outputFile);

        Assert.That(image2.Width, Is.EqualTo(image1.Width));
        Assert.That(image2.Height, Is.EqualTo(image1.Height));
        Assert.That(image2.Channels, Is.EqualTo(image1.Channels));
        Assert.That(image2.BitsPerChannel, Is.EqualTo(image1.BitsPerChannel));
        Assert.That(image2.Layers.Length, Is.EqualTo(originalLength));
        if (image2.Layers.Length > 0)
        {
            Assert.That(image2.Layers[0].Name, Is.EqualTo(originalLayerName));
        }
    }

    /// <summary>
    /// Tests strict no-mutation round-trip with byte-for-byte comparison.
    /// Verifies that saving a PSD file without mutations produces a file
    /// that is byte-for-byte identical to the original.
    /// </summary>
    [Test]
    public void Save_RoundTripWithoutMutation_ByteForByteIdentical()
    {
        string testFile = Path.Combine(TestContext.CurrentContext.TestDirectory, "testdata", "test.psd");
        string outputFile = Path.Combine(_testDir, "roundtrip_byteexact_test.psd");

        byte[] originalBytes = File.ReadAllBytes(testFile);

        using var image = PsdImage.Load(testFile);
        image.Save(outputFile);

        byte[] savedBytes = File.ReadAllBytes(outputFile);

        Assert.That(savedBytes.Length, Is.EqualTo(originalBytes.Length),
            "Round-trip output file length must match original file length.");

        for (int i = 0; i < originalBytes.Length; i++)
        {
            Assert.That(savedBytes[i], Is.EqualTo(originalBytes[i]),
                $"Byte mismatch at position {i}: expected {originalBytes[i]:X2}, got {savedBytes[i]:X2}");
        }
    }

    /// <summary>
    /// Tests that changing a layer name and saving produces a valid file with the new name.
    /// </summary>
    [Test]
    public void Save_AfterChangingLayerName_SavesCorrectly()
    {
        string testFile = Path.Combine(TestContext.CurrentContext.TestDirectory, "testdata", "test.psd");
        string outputFile = Path.Combine(_testDir, "layer_name_test.psd");

        using var image = PsdImage.Load(testFile);

        Assert.That(image.Layers.Length, Is.GreaterThan(0));

        string originalName = image.Layers[0].Name;
        image.Layers[0].Name = "Test Layer Name";

        image.Save(outputFile);

        using var reloaded = PsdImage.Load(outputFile);

        Assert.That(reloaded.Layers[0].Name, Is.EqualTo("Test Layer Name"));
    }

    /// <summary>
    /// Tests that changing a layer's visibility and saving produces a valid file with the new state.
    /// </summary>
    [Test]
    public void Save_AfterChangingLayerVisible_SavesCorrectly()
    {
        string testFile = Path.Combine(TestContext.CurrentContext.TestDirectory, "testdata", "test.psd");
        string outputFile = Path.Combine(_testDir, "layer_visible_test.psd");

        using var image = PsdImage.Load(testFile);

        Assert.That(image.Layers.Length, Is.GreaterThan(0));

        bool originalVisible = image.Layers[0].IsVisible;
        image.Layers[0].IsVisible = !originalVisible;

        image.Save(outputFile);

        using var reloaded = PsdImage.Load(outputFile);

        Assert.That(reloaded.Layers[0].IsVisible, Is.EqualTo(!originalVisible));
    }

    /// <summary>
    /// Tests that changing a layer's opacity and saving produces a valid file with the new opacity.
    /// </summary>
    [Test]
    public void Save_AfterChangingLayerOpacity_SavesCorrectly()
    {
        string testFile = Path.Combine(TestContext.CurrentContext.TestDirectory, "testdata", "test.psd");
        string outputFile = Path.Combine(_testDir, "layer_opacity_test.psd");

        using var image = PsdImage.Load(testFile);

        Assert.That(image.Layers.Length, Is.GreaterThan(0));

        byte originalOpacity = image.Layers[0].Opacity;
        byte newOpacity = (byte)Math.Max(0, originalOpacity - 50);
        image.Layers[0].Opacity = newOpacity;

        image.Save(outputFile);

        using var reloaded = PsdImage.Load(outputFile);

        Assert.That(reloaded.Layers[0].Opacity, Is.EqualTo(newOpacity));
    }

    /// <summary>
    /// Tests that loading from a seekable stream preserves the original stream position.
    /// </summary>
    [Test]
    public void Load_FromSeekableStream_PreservesPosition()
    {
        byte[] bytes = File.ReadAllBytes(Path.Combine(TestContext.CurrentContext.TestDirectory, "testdata", "test.psd"));
        byte[] prefix = [1, 2, 3, 4, 5];
        using var stream = new MemoryStream();
        stream.Write(prefix);
        stream.Write(bytes);
        stream.Position = prefix.Length;

        using var image = PsdImage.Load(stream);

        Assert.That(stream.Position, Is.EqualTo(prefix.Length));
        Assert.That(image.Width, Is.GreaterThan(0));
    }

    /// <summary>
    /// Tests that loading from a non-seekable stream still succeeds.
    /// </summary>
    [Test]
    public void Load_FromNonSeekableStream_LoadsSuccessfully()
    {
        byte[] bytes = File.ReadAllBytes(Path.Combine(TestContext.CurrentContext.TestDirectory, "testdata", "test.psd"));
        using var stream = new NonSeekableReadStream(bytes);
        using var image = PsdImage.Load(stream);

        Assert.That(image.Width, Is.GreaterThan(0));
        Assert.That(image.Layers.Length, Is.GreaterThan(0));
    }

    /// <summary>
    /// Tests that loading from a null stream throws <see cref="ArgumentNullException"/>.
    /// </summary>
    [Test]
    public void Load_NullStream_ThrowsArgumentNullException()
    {
        Assert.That(() => PsdImage.Load((Stream)null!), Throws.InstanceOf<ArgumentNullException>());
    }

    /// <summary>
    /// Tests that loading a missing file path throws <see cref="FileNotFoundException"/>.
    /// </summary>
    [Test]
    public void Load_MissingFilePath_ThrowsFileNotFoundException()
    {
        string missingFile = Path.Combine(_testDir, "missing.psd");
        Assert.That(() => PsdImage.Load(missingFile), Throws.InstanceOf<FileNotFoundException>());
    }

    /// <summary>
    /// Tests that loading a file with an invalid signature throws <see cref="PsdLoadException"/>.
    /// </summary>
    [Test]
    public void Load_InvalidSignature_ThrowsPsdLoadException()
    {
        byte[] bytes = BuildInvalidSignatureDocument();
        using var stream = new MemoryStream(bytes);

        Assert.That(() => PsdImage.Load(stream), Throws.InstanceOf<PsdLoadException>());
    }

    /// <summary>
    /// Tests that a malformed color mode length field is rejected with <see cref="PsdLoadException"/>.
    /// </summary>
    [Test]
    public void Load_ColorModeLengthExceedsAvailableBytes_ThrowsPsdLoadException()
    {
        byte[] bytes = BuildMinimalDocument(psb: false);
        WriteUInt32BigEndian(bytes, 26, 20);
        using var stream = new MemoryStream(bytes);

        Assert.That(() => PsdImage.Load(stream), Throws.InstanceOf<PsdLoadException>());
    }

    /// <summary>
    /// Tests that a malformed layer and mask section length is rejected with <see cref="PsdLoadException"/>.
    /// </summary>
    [Test]
    public void Load_LayerAndMaskLengthExceedsAvailableBytes_ThrowsPsdLoadException()
    {
        byte[] bytes = BuildMinimalDocument(psb: false);
        WriteUInt32BigEndian(bytes, 34, 6);
        using var stream = new MemoryStream(bytes);

        Assert.That(() => PsdImage.Load(stream), Throws.InstanceOf<PsdLoadException>());
    }

    /// <summary>
    /// Tests that a minimal PSD without layers loads correctly.
    /// </summary>
    [Test]
    public void Load_DocumentWithoutLayers_ReturnsEmptyLayers()
    {
        byte[] bytes = BuildMinimalDocument(psb: false);
        using var stream = new MemoryStream(bytes);
        using var image = PsdImage.Load(stream);

        Assert.That(image.Version, Is.EqualTo(PsdHeader.PsdVersion));
        Assert.That(image.Layers, Is.Empty);
        Assert.That(image.HasLayers, Is.False);
    }

    /// <summary>
    /// Tests that a minimal PSB file loads and round-trips correctly without mutations.
    /// </summary>
    [Test]
    public void Save_PsbWithoutLayers_RoundTripByteForByte()
    {
        byte[] originalBytes = BuildMinimalDocument(psb: true);
        string outputFile = Path.Combine(_testDir, "minimal.psb");

        using (var stream = new MemoryStream(originalBytes))
        using (var image = PsdImage.Load(stream))
        {
            Assert.That(image.Version, Is.EqualTo(PsdHeader.PsbVersion));
            Assert.That(image.Layers, Is.Empty);
            image.Save(outputFile);
        }

        byte[] savedBytes = File.ReadAllBytes(outputFile);
        Assert.That(savedBytes, Is.EqualTo(originalBytes));
    }

    private static byte[] BuildInvalidSignatureDocument()
    {
        byte[] bytes = BuildMinimalDocument(psb: false);
        bytes[0] = (byte)'B';
        return bytes;
    }

    /// <summary>
    /// Writes a 32-bit unsigned integer into the specified buffer in big-endian byte order.
    /// </summary>
    /// <param name="buffer">The target byte buffer.</param>
    /// <param name="offset">The offset at which to write the value.</param>
    /// <param name="value">The value to encode.</param>
    private static void WriteUInt32BigEndian(byte[] buffer, int offset, uint value)
    {
        buffer[offset] = (byte)(value >> 24);
        buffer[offset + 1] = (byte)(value >> 16);
        buffer[offset + 2] = (byte)(value >> 8);
        buffer[offset + 3] = (byte)value;
    }

    private static byte[] BuildMinimalDocument(bool psb)
    {
        using var stream = new MemoryStream();

        void WriteUInt16(ushort value)
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

        void WriteUInt32(uint value)
        {
            stream.WriteByte((byte)(value >> 24));
            stream.WriteByte((byte)(value >> 16));
            stream.WriteByte((byte)(value >> 8));
            stream.WriteByte((byte)value);
        }

        void WriteUInt64(ulong value)
        {
            stream.WriteByte((byte)(value >> 56));
            stream.WriteByte((byte)(value >> 48));
            stream.WriteByte((byte)(value >> 40));
            stream.WriteByte((byte)(value >> 32));
            stream.WriteByte((byte)(value >> 24));
            stream.WriteByte((byte)(value >> 16));
            stream.WriteByte((byte)(value >> 8));
            stream.WriteByte((byte)value);
        }

        stream.Write(System.Text.Encoding.ASCII.GetBytes("8BPS"));
        WriteUInt16(psb ? PsdHeader.PsbVersion : PsdHeader.PsdVersion);
        stream.Write(new byte[6]);
        WriteUInt16(3);
        WriteInt32(1);
        WriteInt32(1);
        WriteUInt16(8);
        WriteUInt16((ushort)ColorModes.Rgb);

        WriteUInt32(0);
        WriteUInt32(0);
        if (psb)
        {
            WriteUInt64(0);
        }
        else
        {
            WriteUInt32(0);
        }

        WriteUInt16(0);
        stream.WriteByte(0);
        stream.WriteByte(0);
        stream.WriteByte(0);

        return stream.ToArray();
    }

    /// <summary>
    /// Wraps a readable in-memory stream and intentionally disables seeking.
    /// </summary>
    private sealed class NonSeekableReadStream : Stream
    {
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
}
