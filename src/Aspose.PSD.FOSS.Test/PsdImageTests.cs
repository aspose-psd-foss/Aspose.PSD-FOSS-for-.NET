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
        Assert.That(image.ColorMode, Is.EqualTo(ColorModes.Rgb));
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
        string outputFile = GetPersistentArtifactPath("roundtrip_test.psd");

        using var image1 = PsdImage.Load(testFile);
        int originalLength = image1.Layers.Length;
        string originalLayerName = image1.Layers.Length > 0 ? image1.Layers[0].Name : string.Empty;
        image1.Save(outputFile);
        LogArtifactDirectory(outputFile);

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
    /// Gets a stable artifact path for the current test under the NUnit work directory.
    /// </summary>
    /// <param name="fileName">The artifact file name.</param>
    /// <returns>The full output path for the artifact.</returns>
    private static string GetPersistentArtifactPath(string fileName)
    {
        string testName = SanitizePathSegment(TestContext.CurrentContext.Test.Name);
        string artifactDirectory = Path.Combine(
            TestContext.CurrentContext.WorkDirectory,
            "artifacts",
            nameof(PsdImageTests),
            testName);

        Directory.CreateDirectory(artifactDirectory);
        return Path.Combine(artifactDirectory, fileName);
    }

    private static string GetTestDataPath(string fileName)
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
    private static void LogArtifactDirectory(string outputFile)
    {
        string outputDirectory = Path.GetDirectoryName(outputFile) ?? string.Empty;
        TestContext.Out.WriteLine($"Saved test artifact directory: {outputDirectory}");
    }

    /// <summary>
    /// Replaces characters that are invalid in file-system path segments.
    /// </summary>
    /// <param name="value">The path segment candidate.</param>
    /// <returns>A file-system-safe path segment.</returns>
    private static string SanitizePathSegment(string value)
    {
        char[] invalidCharacters = Path.GetInvalidFileNameChars();
        var builder = new System.Text.StringBuilder(value.Length);

        foreach (char character in value)
        {
            builder.Append(Array.IndexOf(invalidCharacters, character) >= 0 ? '_' : character);
        }

        return builder.ToString();
    }

    private static void AssertByteExactRoundTrip(string fileName)
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

    private static void AssertRenameSave(string fileName, int layerIndex, string newName)
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
    /// Tests strict no-mutation round-trip with byte-for-byte comparison.
    /// Verifies that saving a PSD file without mutations produces a file
    /// that is byte-for-byte identical to the original.
    /// </summary>
    [Test]
    public void Save_RoundTripWithoutMutation_ByteForByteIdentical()
    {
        string testFile = Path.Combine(TestContext.CurrentContext.TestDirectory, "testdata", "test.psd");
        string outputFile = GetPersistentArtifactPath("roundtrip_byteexact_test.psd");

        byte[] originalBytes = File.ReadAllBytes(testFile);

        using var image = PsdImage.Load(testFile);
        image.Save(outputFile);
        LogArtifactDirectory(outputFile);

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
        string outputFile = GetPersistentArtifactPath("layer_name_test.psd");

        using var image = PsdImage.Load(testFile);

        Assert.That(image.Layers.Length, Is.GreaterThan(0));

        string originalName = image.Layers[0].Name;
        image.Layers[0].Name = "Test Layer Name";

        image.Save(outputFile);
        LogArtifactDirectory(outputFile);

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
        string outputFile = GetPersistentArtifactPath("layer_visible_test.psd");

        using var image = PsdImage.Load(testFile);

        Assert.That(image.Layers.Length, Is.GreaterThan(0));

        bool originalVisible = image.Layers[0].IsVisible;
        image.Layers[0].IsVisible = !originalVisible;

        image.Save(outputFile);
        LogArtifactDirectory(outputFile);

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
        string outputFile = GetPersistentArtifactPath("layer_opacity_test.psd");

        using var image = PsdImage.Load(testFile);

        Assert.That(image.Layers.Length, Is.GreaterThan(0));

        byte originalOpacity = image.Layers[0].Opacity;
        byte newOpacity = (byte)Math.Max(0, originalOpacity - 50);
        image.Layers[0].Opacity = newOpacity;

        image.Save(outputFile);
        LogArtifactDirectory(outputFile);

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
        using var stream = new MemoryStream(BuildHeaderBytes(PsdHeader.PsdVersion));
        using var reader = new BigEndianReader(stream, leaveOpen: true);
        stream.Position = 0;
        stream.WriteByte((byte)'B');
        stream.Position = 0;

        Assert.That(() => PsdHeader.Load(reader), Throws.InstanceOf<PsdLoadException>());
    }

    /// <summary>
    /// Tests that non-zero reserved header bytes are rejected.
    /// </summary>
    [Test]
    public void Load_HeaderWithNonZeroReservedBytes_ThrowsPsdLoadException()
    {
        byte[] bytes = BuildHeaderBytes(PsdHeader.PsdVersion);
        bytes[6] = 1;
        using var stream = new MemoryStream(bytes);
        using var reader = new BigEndianReader(stream, leaveOpen: true);

        Assert.That(() => PsdHeader.Load(reader), Throws.InstanceOf<PsdLoadException>());
    }

    /// <summary>
    /// Tests that invalid PSD header field ranges are rejected at the file boundary.
    /// </summary>
    /// <param name="channels">The channel count to encode.</param>
    /// <param name="width">The document width to encode.</param>
    /// <param name="height">The document height to encode.</param>
    /// <param name="bitDepth">The bit depth to encode.</param>
    /// <param name="colorMode">The raw color mode to encode.</param>
    [TestCase(0, 1, 1, 8, (ushort)ColorModes.Rgb)]
    [TestCase(57, 1, 1, 8, (ushort)ColorModes.Rgb)]
    [TestCase(3, 0, 1, 8, (ushort)ColorModes.Rgb)]
    [TestCase(3, 1, 0, 8, (ushort)ColorModes.Rgb)]
    [TestCase(3, 30001, 1, 8, (ushort)ColorModes.Rgb)]
    [TestCase(3, 1, 1, 12, (ushort)ColorModes.Rgb)]
    [TestCase(3, 1, 1, 8, 99)]
    public void Load_HeaderWithInvalidField_ThrowsPsdLoadException(
        int channels,
        int width,
        int height,
        int bitDepth,
        int colorMode)
    {
        byte[] bytes = BuildHeaderBytes(PsdHeader.PsdVersion, (ushort)channels, width, height, (ushort)bitDepth, (ColorModes)colorMode);
        using var stream = new MemoryStream(bytes);
        using var reader = new BigEndianReader(stream, leaveOpen: true);

        Assert.That(() => PsdHeader.Load(reader), Throws.InstanceOf<PsdLoadException>());
    }

    /// <summary>
    /// Tests that a malformed color mode length field is rejected with <see cref="PsdLoadException"/>.
    /// </summary>
    [Test]
    public void Load_ColorModeLengthExceedsAvailableBytes_ThrowsPsdLoadException()
    {
        byte[] bytes = File.ReadAllBytes(Path.Combine(TestContext.CurrentContext.TestDirectory, "testdata", "test.psd"));
        WriteUInt32BigEndian(bytes, 26, 100000);
        using var stream = new MemoryStream(bytes);

        Assert.That(() => PsdImage.Load(stream), Throws.InstanceOf<PsdLoadException>());
    }

    /// <summary>
    /// Tests that unexpected RGB color mode data is classified explicitly and preserved.
    /// </summary>
    [Test]
    public void Load_RgbColorModeData_ClassifiesExplicitRgbPayload()
    {
        byte[] payload = [0x10, 0x20, 0x30, 0x40];
        using var stream = new MemoryStream(BuildColorDataSection(payload));
        using var reader = new BigEndianReader(stream, leaveOpen: true);

        ColorData colorData = ColorData.Load(reader, ColorModes.Rgb);

        Assert.That(colorData.Kind, Is.EqualTo(ColorDataKind.RgbPayload));
        Assert.That(colorData.RawData, Is.EqualTo(payload));
        Assert.That(colorData.IndexedPalette, Is.Null);
    }

    /// <summary>
    /// Tests that indexed color mode data is parsed into a structured 256-color palette.
    /// </summary>
    [Test]
    public void Load_IndexedColorModeData_ParsesPalette()
    {
        byte[] payload = BuildIndexedPalettePayload();
        using var stream = new MemoryStream(BuildColorDataSection(payload));
        using var reader = new BigEndianReader(stream, leaveOpen: true);

        ColorData colorData = ColorData.Load(reader, ColorModes.Indexed);

        Assert.That(colorData.Kind, Is.EqualTo(ColorDataKind.IndexedPalette));
        Assert.That(colorData.RawData, Is.EqualTo(payload));
        Assert.That(colorData.IndexedPalette, Is.Not.Null);
        Assert.That(colorData.IndexedPalette!.Entries, Has.Length.EqualTo(256));
        Assert.That(colorData.IndexedPalette.Entries[0], Is.EqualTo(Color.FromArgb(0x00, 0xFF, 0x80)));
        Assert.That(colorData.IndexedPalette.Entries[17], Is.EqualTo(Color.FromArgb(0x11, 0xEE, 0x91)));
    }

    /// <summary>
    /// Tests that CMYK color mode data is classified explicitly and preserved.
    /// </summary>
    [Test]
    public void Load_CmykColorModeData_ClassifiesExplicitCmykPayload()
    {
        byte[] payload = [0xCA, 0xFE, 0xBA, 0xBE];
        using var stream = new MemoryStream(BuildColorDataSection(payload));
        using var reader = new BigEndianReader(stream, leaveOpen: true);

        ColorData colorData = ColorData.Load(reader, ColorModes.CMYK);

        Assert.That(colorData.Kind, Is.EqualTo(ColorDataKind.CmykPayload));
        Assert.That(colorData.RawData, Is.EqualTo(payload));
        Assert.That(colorData.IndexedPalette, Is.Null);
    }

    /// <summary>
    /// Tests that image resources are loaded as unknown blocks without ID-specific semantics.
    /// </summary>
    [Test]
    public void Load_ImageResources_ParsesUnknownBlocksWithoutSemantics()
    {
        byte[] resourcesPayload = BuildResourcesPayload(
            (ImageResourceIds.GlobalAngle, "glba", [0x00, 0x00, 0x00, 0x2D]),
            (ImageResourceIds.IccProfile, "icc", [0x49, 0x43, 0x43, 0x50]),
            (ImageResourceIds.IccUntaggedProfile, string.Empty, [0x01]));
        using var stream = new MemoryStream(resourcesPayload);
        using var reader = new BigEndianReader(stream, leaveOpen: true);

        var resources = new List<UnknownResource>();
        while (reader.Position < stream.Length)
        {
            UnknownResource? resource = UnknownResource.Load(reader, stream.Length);
            Assert.That(resource, Is.Not.Null);
            resources.Add(resource!);
        }

        Assert.That(resources, Has.Count.EqualTo(3));
        Assert.That(resources[0].ResourceId, Is.EqualTo(ImageResourceIds.GlobalAngle));
        Assert.That(resources[0].Data, Is.EqualTo(new byte[] { 0x00, 0x00, 0x00, 0x2D }));
        Assert.That(resources[1].ResourceId, Is.EqualTo(ImageResourceIds.IccProfile));
        Assert.That(resources[1].Data, Is.EqualTo(new byte[] { 0x49, 0x43, 0x43, 0x50 }));
        Assert.That(resources[2].ResourceId, Is.EqualTo(ImageResourceIds.IccUntaggedProfile));
        Assert.That(resources[2].Data, Is.EqualTo(new byte[] { 0x01 }));
    }

    /// <summary>
    /// Tests that a malformed layer and mask section length is rejected with <see cref="PsdLoadException"/>.
    /// </summary>
    [Test]
    public void Load_LayerAndMaskLengthExceedsAvailableBytes_ThrowsPsdLoadException()
    {
        byte[] bytes = File.ReadAllBytes(Path.Combine(TestContext.CurrentContext.TestDirectory, "testdata", "test.psd"));
        int colorModeLength = BigEndianBitConverter.ToInt32(bytes, 26);
        int resourcesLengthOffset = 26 + 4 + colorModeLength;
        int resourcesPayloadLength = BigEndianBitConverter.ToInt32(bytes, resourcesLengthOffset);
        int layerAndMaskLengthOffset = resourcesLengthOffset + 4 + resourcesPayloadLength;
        WriteUInt32BigEndian(bytes, layerAndMaskLengthOffset, 100000);
        using var stream = new MemoryStream(bytes);

        Assert.That(() => PsdImage.Load(stream), Throws.InstanceOf<PsdLoadException>());
    }

    /// <summary>
    /// Tests that malformed layer extra data is rejected instead of being silently normalized.
    /// </summary>
    [Test]
    public void Load_LayerExtraDataLengthExceedsBoundary_ThrowsPsdLoadException()
    {
        byte[] layerBytes = BuildLayerRecordBytesWithExtraData([
            0x00, 0x00, 0x00, 0x10,
            0x00, 0x00, 0x00, 0x00
        ]);
        using var stream = new MemoryStream(layerBytes);
        using var reader = new BigEndianReader(stream, leaveOpen: true);

        Assert.That(() => Layer.Load(reader, isLargeDocument: false), Throws.InstanceOf<PsdLoadException>());
    }

    /// <summary>
    /// Tests that a negative layer extra data length is rejected.
    /// </summary>
    [Test]
    public void Load_LayerExtraDataNegativeLength_ThrowsPsdLoadException()
    {
        byte[] layerBytes = BuildLayerRecordBytesWithExtraData([]);
        WriteUInt32BigEndian(layerBytes, 30, 0xFFFFFFFF);
        using var stream = new MemoryStream(layerBytes);
        using var reader = new BigEndianReader(stream, leaveOpen: true);

        Assert.That(() => Layer.Load(reader, isLargeDocument: false), Throws.InstanceOf<PsdLoadException>());
    }

    /// <summary>
    /// Tests that a minimal PSD without layers loads correctly.
    /// </summary>
    [Test]
    public void Load_PsdHeader_ReturnsExpectedMetadata()
    {
        using var stream = new MemoryStream(BuildHeaderBytes(PsdHeader.PsdVersion));
        using var reader = new BigEndianReader(stream, leaveOpen: true);

        PsdHeader header = PsdHeader.Load(reader);

        Assert.That(header.Version, Is.EqualTo(PsdHeader.PsdVersion));
        Assert.That(header.Width, Is.EqualTo(1));
        Assert.That(header.Height, Is.EqualTo(1));
        Assert.That(header.Channels, Is.EqualTo(3));
        Assert.That(header.BitDepth, Is.EqualTo(8));
        Assert.That(header.ColorMode, Is.EqualTo(ColorModes.Rgb));
    }

    /// <summary>
    /// Tests that simple document-level inspection properties expose the parsed structural state.
    /// </summary>
    [Test]
    public void Load_DocumentSimpleInspectionProperties_ReturnExpectedValues()
    {
        using var image = PsdImage.Load(Path.Combine(TestContext.CurrentContext.TestDirectory, "testdata", "test.psd"));

        Assert.That(image.Header, Is.Not.Null);
        Assert.That(image.IsLargeDocument, Is.False);
        Assert.That(image.IsPsb, Is.False);
        Assert.That(image.LayerCount, Is.EqualTo(2));
        Assert.That(image.HasImageResources, Is.True);
        Assert.That(image.ResourceCount, Is.EqualTo(26));
        Assert.That(image.HasColorModeData, Is.False);
        Assert.That(image.HasMergedImageData, Is.True);
        Assert.That(image.Compression, Is.EqualTo(CompressionMethod.RLE));
        Assert.That(image.ImageDataKind, Is.EqualTo(ImageDataKind.Rle));
        Assert.That(image.UsesPrediction, Is.False);
        Assert.That(image.Header.Version, Is.EqualTo(image.Version));
        Assert.That(image.Header.ColorMode, Is.EqualTo(image.ColorMode));
    }

    /// <summary>
    /// Tests that saving a document with image resources preserves the raw Image Resources section bytes.
    /// </summary>
    [Test]
    public void Save_DocumentWithImageResources_PreservesRawResourcesSection()
    {
        string testFile = Path.Combine(TestContext.CurrentContext.TestDirectory, "testdata", "test.psd");
        byte[] originalBytes = File.ReadAllBytes(testFile);
        string outputFile = GetPersistentArtifactPath("resources_roundtrip_test.psd");

        using var image = PsdImage.Load(testFile);
        image.Save(outputFile);
        LogArtifactDirectory(outputFile);

        byte[] savedBytes = File.ReadAllBytes(outputFile);
        byte[] originalResourcesSection = ExtractImageResourcesSection(originalBytes);
        byte[] savedResourcesSection = ExtractImageResourcesSection(savedBytes);

        Assert.That(savedResourcesSection, Is.EqualTo(originalResourcesSection));

        using var reloaded = PsdImage.Load(outputFile);
        Assert.That(reloaded.HasImageResources, Is.True);
        Assert.That(reloaded.ResourceCount, Is.EqualTo(26));
    }

    /// <summary>
    /// Tests that simple layer-level inspection properties expose stored geometry and subsection presence.
    /// </summary>
    [Test]
    public void Load_LayerSimpleInspectionProperties_ReturnExpectedValues()
    {
        using var image = PsdImage.Load(Path.Combine(TestContext.CurrentContext.TestDirectory, "testdata", "test.psd"));
        Layer firstLayer = image.Layers[0];

        Assert.That(firstLayer.Width, Is.EqualTo(firstLayer.Bounds.Width));
        Assert.That(firstLayer.Height, Is.EqualTo(firstLayer.Bounds.Height));
        Assert.That(firstLayer.Top, Is.EqualTo(firstLayer.Bounds.Top));
        Assert.That(firstLayer.Left, Is.EqualTo(firstLayer.Bounds.Left));
        Assert.That(firstLayer.Bottom, Is.EqualTo(firstLayer.Bounds.Bottom));
        Assert.That(firstLayer.Right, Is.EqualTo(firstLayer.Bounds.Right));
        Assert.That(firstLayer.ChannelCount, Is.GreaterThan(0));
        Assert.That(firstLayer.BlendModeKey, Has.Length.EqualTo(4));
        Assert.That(firstLayer.HasMaskData, Is.False);
        Assert.That(firstLayer.HasBlendingRangesData, Is.EqualTo(firstLayer.BlendingRangesInfo.RawDataLength > 4));
        Assert.That(firstLayer.HasAdditionalLayerData, Is.True);
    }

    /// <summary>
    /// Tests that saving after a supported mutation preserves raw layer flags and the original blend mode key.
    /// </summary>
    [Test]
    public void Save_AfterSupportedMutation_PreservesRawFlagsAndBlendModeKey()
    {
        string testFile = Path.Combine(TestContext.CurrentContext.TestDirectory, "testdata", "test.psd");
        string outputFile = GetPersistentArtifactPath("preserve_flags_and_blend.psd");

        using var image = PsdImage.Load(testFile);
        image.Layers[0].Name = "Renamed";
        image.Save(outputFile);
        LogArtifactDirectory(outputFile);

        using var reloaded = PsdImage.Load(outputFile);
        Assert.That(reloaded.Layers[0].BlendModeKey, Is.EqualTo("norm"));
        Assert.That(reloaded.Layers[0].IsVisible, Is.True);
    }

    /// <summary>
    /// Tests that changing a layer's blend mode persists after save and reload.
    /// </summary>
    [Test]
    public void Save_AfterChangingLayerBlendMode_SavesCorrectly()
    {
        string testFile = Path.Combine(TestContext.CurrentContext.TestDirectory, "testdata", "test.psd");
        string outputFile = GetPersistentArtifactPath("layer_blend_mode_test.psd");

        using var image = PsdImage.Load(testFile);
        image.Layers[0].BlendMode = BlendMode.Multiply;
        image.Save(outputFile);
        LogArtifactDirectory(outputFile);

        using var reloaded = PsdImage.Load(outputFile);
        Assert.That(reloaded.Layers[0].BlendMode, Is.EqualTo(BlendMode.Multiply));
        Assert.That(reloaded.Layers[0].BlendModeKey, Is.EqualTo("mul "));

        byte[] originalBytes = File.ReadAllBytes(testFile);
        byte[] savedBytes = File.ReadAllBytes(outputFile);
        Assert.That(ReadLayerInfoLength(originalBytes), Is.EqualTo(ReadLayerInfoLength(savedBytes)));
    }

    /// <summary>
    /// Tests that changing a layer's clipping value persists after save and reload.
    /// </summary>
    [Test]
    public void Save_AfterChangingLayerClipping_SavesCorrectly()
    {
        string testFile = Path.Combine(TestContext.CurrentContext.TestDirectory, "testdata", "test.psd");
        string outputFile = GetPersistentArtifactPath("layer_clipping_test.psd");
        byte[] originalBytes = File.ReadAllBytes(testFile);

        using var image = PsdImage.Load(testFile);
        image.Layers[0].Clipping = 1;
        image.Save(outputFile);
        LogArtifactDirectory(outputFile);

        using var reloaded = PsdImage.Load(outputFile);
        Assert.That(reloaded.Layers[0].Clipping, Is.EqualTo(1));

        byte[] savedBytes = File.ReadAllBytes(outputFile);
        Assert.That(ReadLayerAndMaskTail(originalBytes), Is.EqualTo(ReadLayerAndMaskTail(savedBytes)));
    }

    /// <summary>
    /// Tests that changing a layer's bounds persists after save and reload.
    /// </summary>
    [Test]
    public void Save_AfterChangingLayerBounds_SavesCorrectly()
    {
        string testFile = Path.Combine(TestContext.CurrentContext.TestDirectory, "testdata", "test.psd");
        string outputFile = GetPersistentArtifactPath("layer_bounds_test.psd");

        using var image = PsdImage.Load(testFile);
        Rectangle newBounds = Rectangle.FromLTRB(10, 20, 40, 60);
        image.Layers[0].Bounds = newBounds;
        image.Save(outputFile);
        LogArtifactDirectory(outputFile);

        using var reloaded = PsdImage.Load(outputFile);
        Assert.That(reloaded.Layers[0].Bounds, Is.EqualTo(newBounds));
    }

    /// <summary>
    /// Tests that changing coordinate properties updates bounds and persists after save and reload.
    /// </summary>
    [Test]
    public void Save_AfterChangingLayerCoordinates_SavesCorrectly()
    {
        string testFile = Path.Combine(TestContext.CurrentContext.TestDirectory, "testdata", "test.psd");
        string outputFile = GetPersistentArtifactPath("layer_coordinates_test.psd");

        using var image = PsdImage.Load(testFile);
        Layer layer = image.Layers[0];
        layer.Top = 10;
        layer.Left = 20;
        layer.Bottom = 30;
        layer.Right = 50;
        image.Save(outputFile);
        LogArtifactDirectory(outputFile);

        using var reloaded = PsdImage.Load(outputFile);
        Assert.That(reloaded.Layers[0].Bounds, Is.EqualTo(Rectangle.FromLTRB(20, 10, 50, 30)));
    }

    [Test]
    public void Load_BasicRgbFixturePsd_ReturnsExpectedDocumentMetadata()
    {
        using var image = PsdImage.Load(GetTestDataPath("basic-rgb.psd"));

        Assert.That(image.Width, Is.EqualTo(200));
        Assert.That(image.Height, Is.EqualTo(200));
        Assert.That(image.Channels, Is.EqualTo(3));
        Assert.That(image.BitsPerChannel, Is.EqualTo(8));
        Assert.That(image.ColorMode, Is.EqualTo(ColorModes.Rgb));
        Assert.That(image.Version, Is.EqualTo(PsdHeader.PsdVersion));
        Assert.That(image.LayerCount, Is.EqualTo(3));
        Assert.That(image.ResourceCount, Is.EqualTo(26));
        Assert.That(image.Compression, Is.EqualTo(CompressionMethod.RLE));
    }

    [Test]
    public void Save_BasicRgbFixturePsd_RoundTripWithoutMutation_ByteExact()
    {
        AssertByteExactRoundTrip("basic-rgb.psd");
    }

    [Test]
    public void Save_BasicRgbFixturePsd_AfterChangingLayerName_SavesCorrectly()
    {
        AssertRenameSave("basic-rgb.psd", layerIndex: 1, newName: "Renamed Rectangle");
    }

    [Test]
    public void Load_BasicIndexedFixturePsd_ReturnsExpectedColorDataInfo()
    {
        using var image = PsdImage.Load(GetTestDataPath("basic-indexed.psd"));

        Assert.That(image.Width, Is.EqualTo(200));
        Assert.That(image.Height, Is.EqualTo(200));
        Assert.That(image.Channels, Is.EqualTo(1));
        Assert.That(image.ColorMode, Is.EqualTo(ColorModes.Indexed));
        Assert.That(image.HasColorModeData, Is.True);
        Assert.That(image.ColorDataInfo.Kind, Is.EqualTo(PsdColorDataKind.IndexedPalette));
        Assert.That(image.ColorDataInfo.RawDataLength, Is.EqualTo(768));
        Assert.That(image.LayerCount, Is.EqualTo(0));
        Assert.That(image.ResourceCount, Is.EqualTo(24));
    }

    [Test]
    public void Save_BasicIndexedFixturePsd_RoundTripWithoutMutation_ByteExact()
    {
        AssertByteExactRoundTrip("basic-indexed.psd");
    }

    [Test]
    public void Load_BasicCmykFixturePsd_ReturnsExpectedDocumentMetadata()
    {
        using var image = PsdImage.Load(GetTestDataPath("basic-cmyk.psd"));

        Assert.That(image.Width, Is.EqualTo(200));
        Assert.That(image.Height, Is.EqualTo(200));
        Assert.That(image.Channels, Is.EqualTo(4));
        Assert.That(image.ColorMode, Is.EqualTo(ColorModes.CMYK));
        Assert.That(image.LayerCount, Is.EqualTo(3));
        Assert.That(image.ResourceCount, Is.EqualTo(28));
        Assert.That(image.Compression, Is.EqualTo(CompressionMethod.RLE));
        Assert.That(image.ImageDataInfo.RowByteCounts, Has.Count.EqualTo(800));
    }

    [Test]
    public void Save_BasicCmykFixturePsd_RoundTripWithoutMutation_ByteExact()
    {
        AssertByteExactRoundTrip("basic-cmyk.psd");
    }

    [Test]
    public void Load_RleFixturePsd_ReturnsExpectedImageDataInfo()
    {
        using var image = PsdImage.Load(GetTestDataPath("rle.psd"));

        Assert.That(image.Compression, Is.EqualTo(CompressionMethod.RLE));
        Assert.That(image.ImageDataKind, Is.EqualTo(ImageDataKind.Rle));
        Assert.That(image.ImageDataInfo.RowLengthFieldSize, Is.EqualTo(sizeof(ushort)));
        Assert.That(image.ImageDataInfo.RowByteCounts, Has.Count.EqualTo(600));
        Assert.That(image.ImageDataInfo.CompressedPayloadLength, Is.EqualTo(5219));
    }

    [Test]
    public void Save_RleFixturePsd_RoundTripWithoutMutation_ByteExact()
    {
        AssertByteExactRoundTrip("rle.psd");
    }

    [Test]
    public void Load_ZipFixturePsd_ReturnsExpectedImageDataInfo()
    {
        using var image = PsdImage.Load(GetTestDataPath("zip.psd"));

        Assert.That(image.Compression, Is.EqualTo(CompressionMethod.RLE));
        Assert.That(image.ImageDataKind, Is.EqualTo(ImageDataKind.Rle));
        Assert.That(image.ImageDataInfo.RowLengthFieldSize, Is.EqualTo(sizeof(ushort)));
        Assert.That(image.ImageDataInfo.RowByteCounts, Has.Count.EqualTo(600));
        Assert.That(image.ImageDataInfo.CompressedPayloadLength, Is.EqualTo(5219));
    }

    [Test]
    public void Save_ZipFixturePsd_RoundTripWithoutMutation_ByteExact()
    {
        AssertByteExactRoundTrip("zip.psd");
    }

    [Test]
    public void Load_BasicPsbFixture_ReturnsExpectedDocumentMetadata()
    {
        using var image = PsdImage.Load(GetTestDataPath("basic.psb"));

        Assert.That(image.Version, Is.EqualTo(PsdHeader.PsbVersion));
        Assert.That(image.IsLargeDocument, Is.True);
        Assert.That(image.IsPsb, Is.True);
        Assert.That(image.Width, Is.EqualTo(200));
        Assert.That(image.Height, Is.EqualTo(200));
        Assert.That(image.LayerCount, Is.EqualTo(0));
        Assert.That(image.ResourceCount, Is.EqualTo(24));
        Assert.That(image.Compression, Is.EqualTo(CompressionMethod.RLE));
        Assert.That(image.ImageDataInfo.RowLengthFieldSize, Is.EqualTo(sizeof(uint)));
    }

    [Test]
    public void Save_BasicPsbFixture_RoundTripWithoutMutation_ByteExact()
    {
        AssertByteExactRoundTrip("basic.psb");
    }

    [Test]
    public void Load_LayeredPsbFixture_ReturnsExpectedLayerMetadata()
    {
        using var image = PsdImage.Load(GetTestDataPath("layered.psb"));

        Assert.That(image.Version, Is.EqualTo(PsdHeader.PsbVersion));
        Assert.That(image.LayerCount, Is.EqualTo(3));
        Assert.That(image.Layers[0].Name, Is.EqualTo("Background"));
        Assert.That(image.Layers[1].Name, Is.EqualTo("Rectangle 1"));
        Assert.That(image.Layers[2].Name, Is.EqualTo("Ellipse 1"));
        Assert.That(image.Layers[2].Opacity, Is.EqualTo(191));
    }

    [Test]
    public void Save_LayeredPsbFixture_AfterChangingLayerName_SavesCorrectly()
    {
        AssertRenameSave("layered.psb", layerIndex: 1, newName: "Renamed Rectangle");
    }

    [Test]
    public void Save_LayeredPsbFixture_AfterChangingLayerBlendMode_SavesCorrectly()
    {
        string testFile = GetTestDataPath("layered.psb");
        string outputFile = GetPersistentArtifactPath("layered_psb_blend_mode_test.psb");

        using var image = PsdImage.Load(testFile);
        image.Layers[1].BlendMode = BlendMode.Multiply;
        image.Save(outputFile);
        LogArtifactDirectory(outputFile);

        using var reloaded = PsdImage.Load(outputFile);
        Assert.That(reloaded.Layers[1].BlendMode, Is.EqualTo(BlendMode.Multiply));
        Assert.That(reloaded.Layers[1].BlendModeKey, Is.EqualTo("mul "));
    }

    [Test]
    public void Load_ResourcesFixturePsd_ReturnsExpectedResourceSummaries()
    {
        using var image = PsdImage.Load(GetTestDataPath("resources.psd"));

        Assert.That(image.ResourceCount, Is.EqualTo(28));
        Assert.That(image.Resources[0].Kind, Is.EqualTo(PsdResourceKind.Unknown));
        Assert.That(image.HasImageResources, Is.True);
        Assert.That(image.Layers, Has.Length.EqualTo(3));
    }

    [Test]
    public void Save_ResourcesFixturePsd_PreservesRawResourcesSection()
    {
        string testFile = GetTestDataPath("resources.psd");
        byte[] originalBytes = File.ReadAllBytes(testFile);
        string outputFile = GetPersistentArtifactPath("resources_fixture_roundtrip_test.psd");

        using var image = PsdImage.Load(testFile);
        image.Save(outputFile);
        LogArtifactDirectory(outputFile);

        byte[] savedBytes = File.ReadAllBytes(outputFile);
        Assert.That(ExtractImageResourcesSection(savedBytes), Is.EqualTo(ExtractImageResourcesSection(originalBytes)));
    }

    [Test]
    public void Load_LayerVariantsFixturePsd_ReturnsExpectedLayerFlags()
    {
        using var image = PsdImage.Load(GetTestDataPath("layer-variants.psd"));

        Assert.That(image.LayerCount, Is.EqualTo(4));
        Assert.That(image.Layers[1].IsVisible, Is.False);
        Assert.That(image.Layers[2].BlendModeKey, Is.EqualTo("lbrn"));
        Assert.That(image.Layers[3].Clipping, Is.EqualTo(1));
        Assert.That(image.Layers[3].HasAdditionalLayerData, Is.True);
    }

    [Test]
    public void Save_LayerVariantsFixturePsd_AfterChangingClipping_SavesCorrectly()
    {
        string testFile = GetTestDataPath("layer-variants.psd");
        string outputFile = GetPersistentArtifactPath("layer_variants_clipping_test.psd");

        using var image = PsdImage.Load(testFile);
        image.Layers[3].Clipping = 0;
        image.Save(outputFile);
        LogArtifactDirectory(outputFile);

        using var reloaded = PsdImage.Load(outputFile);
        Assert.That(reloaded.Layers[3].Clipping, Is.EqualTo(0));
    }

    [Test]
    public void Save_LayerVariantsFixturePsd_PreservesLayerAndMaskTail()
    {
        string testFile = GetTestDataPath("layer-variants.psd");
        byte[] originalBytes = File.ReadAllBytes(testFile);
        string outputFile = GetPersistentArtifactPath("layer_variants_tail_test.psd");

        using var image = PsdImage.Load(testFile);
        image.Layers[2].Name = "Ellipse 1 Updated";
        image.Save(outputFile);
        LogArtifactDirectory(outputFile);

        byte[] savedBytes = File.ReadAllBytes(outputFile);
        Assert.That(ReadLayerAndMaskTail(savedBytes), Is.EqualTo(ReadLayerAndMaskTail(originalBytes)));
    }

    /// <summary>
    /// Tests that document-level DTO inspection API exposes unknown-only resource summaries and other structural metadata.
    /// </summary>
    [Test]
    public void Load_DocumentInspectionDtos_ReturnExpectedValues()
    {
        using var image = PsdImage.Load(Path.Combine(TestContext.CurrentContext.TestDirectory, "testdata", "test.psd"));

        Assert.That(image.Resources, Has.Count.EqualTo(26));
        Assert.That(image.Resources[0].Kind, Is.EqualTo(PsdResourceKind.Unknown));
        Assert.That(image.Resources[1].Kind, Is.EqualTo(PsdResourceKind.Unknown));
        Assert.That(image.Resources[2].Kind, Is.EqualTo(PsdResourceKind.Unknown));
        Assert.That(image.Resources[0].GlobalAngle, Is.Null);
        Assert.That(image.Resources[1].GlobalAngle, Is.Null);
        Assert.That(image.Resources[2].GlobalAngle, Is.Null);
        Assert.That(image.HasIccProfile, Is.False);
        Assert.That(image.IsIccProfileUntagged, Is.Null);
        Assert.That(image.GlobalAngle, Is.Null);

        Assert.That(image.ColorDataInfo.Kind, Is.EqualTo(PsdColorDataKind.None));
        Assert.That(image.ColorDataInfo.RawDataLength, Is.EqualTo(0));
        Assert.That(image.IndexedPalette, Is.Null);

        Assert.That(image.ImageDataInfo.Kind, Is.EqualTo(ImageDataKind.Rle));
        Assert.That(image.ImageDataInfo.RowLengthFieldSize, Is.EqualTo(sizeof(ushort)));
        Assert.That(image.ImageDataInfo.RowByteCounts, Has.Count.EqualTo(300));
        Assert.That(image.ImageDataInfo.CompressedPayloadLength, Is.GreaterThan(0));
        Assert.That(image.ImageDataInfo.UsesPrediction, Is.False);
    }

    /// <summary>
    /// Tests that layer-level DTO inspection API exposes channel and subsection summaries.
    /// </summary>
    [Test]
    public void Load_LayerInspectionDtos_ReturnExpectedValues()
    {
        using var image = PsdImage.Load(Path.Combine(TestContext.CurrentContext.TestDirectory, "testdata", "test.psd"));
        Layer layer = image.Layers[0];

        Assert.That(layer.Channels, Has.Count.EqualTo(4));
        Assert.That(layer.Channels[0].ChannelId, Is.EqualTo((short)-1));
        Assert.That(layer.Channels[1].ChannelId, Is.EqualTo((short)0));
        Assert.That(layer.Channels[2].ChannelId, Is.EqualTo((short)1));
        Assert.That(layer.Channels[3].ChannelId, Is.EqualTo((short)2));
        Assert.That(layer.Channels[0].DataLength, Is.GreaterThan(0UL));

        Assert.That(layer.MaskInfo.IsPresent, Is.False);
        Assert.That(layer.MaskInfo.RawDataLength, Is.EqualTo(4));
        Assert.That(layer.BlendingRangesInfo.IsPresent, Is.True);
        Assert.That(layer.BlendingRangesInfo.RawDataLength, Is.EqualTo(44));
    }

    /// <summary>
    /// Tests that a minimal synthetic PSD with RLE image data exposes the expected structural metadata.
    /// </summary>
    [Test]
    public void Load_PsdWithRleImageData_ReturnsExpectedStructure()
    {
        using var stream = new MemoryStream(BuildImageDataSection(CompressionMethod.RLE, [0x00, 0x02, 0xAB, 0xCD, 0x00, 0x01, 0xEF]));
        using var reader = new BigEndianReader(stream, leaveOpen: true);

        ImageData imageData = ImageData.Load(reader, isLargeDocument: false, height: 1, channelCount: 3);

        Assert.That(imageData.Compression, Is.EqualTo(CompressionMethod.RLE));
        Assert.That(imageData.Structure.Kind, Is.EqualTo(ImageDataKind.Rle));
        Assert.That(imageData.Structure.RowLengthFieldSize, Is.EqualTo(sizeof(ushort)));
        Assert.That(imageData.Structure.RowByteCounts, Has.Length.EqualTo(3));
        Assert.That(imageData.Structure.RowByteCounts[0], Is.EqualTo(2U));
        Assert.That(imageData.Structure.RowByteCounts[1], Is.EqualTo(0xABCDU));
        Assert.That(imageData.Structure.RowByteCounts[2], Is.EqualTo(1U));
        Assert.That(imageData.Structure.CompressedPayloadLength, Is.EqualTo(1));
    }

    /// <summary>
    /// Tests that a minimal synthetic PSB with RLE image data exposes the expected structural metadata.
    /// </summary>
    [Test]
    public void Load_PsbWithRleImageData_ReturnsExpectedStructure()
    {
        using var stream = new MemoryStream(BuildImageDataSection(
            CompressionMethod.RLE,
            [
                0x00, 0x00, 0x00, 0x02,
                0x00, 0x00, 0x00, 0x01,
                0x00, 0x00, 0x00, 0x03,
                0xAB, 0xCD, 0xEF, 0x10, 0x11, 0x12
            ]));
        using var reader = new BigEndianReader(stream, leaveOpen: true);

        ImageData imageData = ImageData.Load(reader, isLargeDocument: true, height: 1, channelCount: 3);

        Assert.That(imageData.Compression, Is.EqualTo(CompressionMethod.RLE));
        Assert.That(imageData.Structure.Kind, Is.EqualTo(ImageDataKind.Rle));
        Assert.That(imageData.Structure.RowLengthFieldSize, Is.EqualTo(sizeof(uint)));
        Assert.That(imageData.Structure.RowByteCounts, Has.Length.EqualTo(3));
        Assert.That(imageData.Structure.RowByteCounts[0], Is.EqualTo(2U));
        Assert.That(imageData.Structure.RowByteCounts[1], Is.EqualTo(1U));
        Assert.That(imageData.Structure.RowByteCounts[2], Is.EqualTo(3U));
        Assert.That(imageData.Structure.CompressedPayloadLength, Is.EqualTo(6));
    }

    /// <summary>
    /// Tests that a minimal synthetic PSD with ZIP image data exposes the expected structural metadata.
    /// </summary>
    [Test]
    public void Load_PsdWithZipImageData_ReturnsExpectedStructure()
    {
        using var stream = new MemoryStream(BuildImageDataSection(CompressionMethod.ZIP, [0x78, 0x9C, 0x63, 0x60, 0x04, 0x00, 0x00, 0xFF, 0x00]));
        using var reader = new BigEndianReader(stream, leaveOpen: true);

        ImageData imageData = ImageData.Load(reader, isLargeDocument: false, height: 1, channelCount: 3);

        Assert.That(imageData.Compression, Is.EqualTo(CompressionMethod.ZIP));
        Assert.That(imageData.Structure.Kind, Is.EqualTo(ImageDataKind.Zip));
        Assert.That(imageData.Structure.UsesPrediction, Is.False);
        Assert.That(imageData.Structure.CompressedPayloadLength, Is.EqualTo(9));
    }

    /// <summary>
    /// Tests that a truncated RLE row-length table is rejected.
    /// </summary>
    [Test]
    public void Load_RleImageDataWithTruncatedRowLengthTable_ThrowsPsdLoadException()
    {
        using var stream = new MemoryStream(BuildImageDataSection(CompressionMethod.RLE, [0x00, 0x02, 0xAB, 0xCD, 0x00]));
        using var reader = new BigEndianReader(stream, leaveOpen: true);

        Assert.That(() => ImageData.Load(reader, isLargeDocument: false, height: 1, channelCount: 3), Throws.InstanceOf<PsdLoadException>());
    }

    /// <summary>
    /// Tests that a minimal synthetic PSB file loads expected document metadata without layers.
    /// </summary>
    [Test]
    public void Load_PsbHeader_ReturnsExpectedMetadata()
    {
        using var stream = new MemoryStream(BuildHeaderBytes(PsdHeader.PsbVersion));
        using var reader = new BigEndianReader(stream, leaveOpen: true);

        PsdHeader header = PsdHeader.Load(reader);

        Assert.That(header.Version, Is.EqualTo(PsdHeader.PsbVersion));
        Assert.That(header.IsLargeDocument, Is.True);
        Assert.That(header.Width, Is.EqualTo(1));
        Assert.That(header.Height, Is.EqualTo(1));
        Assert.That(header.Channels, Is.EqualTo(3));
    }

    /// <summary>
    /// Tests that a synthetic PSB fixture with one layer record loads expected layer metadata.
    /// </summary>
    [Test]
    public void Load_PsbWithLayerRecord_ReturnsExpectedLayerMetadata()
    {
        using var stream = new MemoryStream(BuildPsbLayerRecordBytes());
        using var reader = new BigEndianReader(stream, leaveOpen: true);

        Layer layer = Layer.Load(reader, isLargeDocument: true);

        Assert.That(layer.Name, Is.EqualTo("Layer 1"));
        Assert.That(layer.Bounds, Is.EqualTo(new Rectangle(0, 0, 1, 1)));
        Assert.That(layer.Opacity, Is.EqualTo(200));
        Assert.That(layer.IsVisible, Is.True);
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

    private static byte[] BuildHeaderBytes(
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

    private static byte[] BuildColorDataSection(byte[] payload)
    {
        using var stream = new MemoryStream();
        using var writer = new BigEndianWriter(stream, leaveOpen: true);
        writer.Write((uint)payload.Length);
        writer.Write(payload);
        return stream.ToArray();
    }

    private static byte[] BuildImageDataSection(CompressionMethod compression, byte[] payload)
    {
        using var stream = new MemoryStream();
        using var writer = new BigEndianWriter(stream, leaveOpen: true);
        writer.Write((ushort)compression);
        writer.Write(payload);
        return stream.ToArray();
    }

    private static byte[] BuildIndexedPalettePayload()
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

    private static byte[] BuildResourcesPayload(params (short resourceId, string name, byte[] data)[] resources)
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
    private static byte[] BuildLayerRecordBytesWithExtraData(byte[] extraData)
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
    private static byte[] ExtractImageResourcesSection(byte[] documentBytes)
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
    private static int ReadLayerInfoLength(byte[] documentBytes)
    {
        const int headerLength = 26;
        int colorModeLength = BigEndianBitConverter.ToInt32(documentBytes, headerLength);
        int resourcesLengthOffset = headerLength + 4 + colorModeLength;
        int resourcesPayloadLength = BigEndianBitConverter.ToInt32(documentBytes, resourcesLengthOffset);
        int layerAndMaskLengthOffset = resourcesLengthOffset + 4 + resourcesPayloadLength;
        int layerInfoLengthOffset = layerAndMaskLengthOffset + 4;
        return BigEndianBitConverter.ToInt32(documentBytes, layerInfoLengthOffset);
    }

    private static byte[] ReadLayerAndMaskTail(byte[] documentBytes)
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

    private static byte[] BuildPsbLayerRecordBytes()
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
