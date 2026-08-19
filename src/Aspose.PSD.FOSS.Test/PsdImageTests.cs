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
    /// Tests that unexpected RGB color mode data is classified explicitly and preserved.
    /// </summary>
    [Test]
    public void Load_RgbColorModeData_ClassifiesExplicitRgbPayload()
    {
        byte[] payload = [0x10, 0x20, 0x30, 0x40];
        byte[] bytes = BuildMinimalDocument(psb: false, colorMode: ColorModes.Rgb, colorDataPayload: payload);
        using var stream = new MemoryStream(bytes);
        using var image = PsdImage.Load(stream);

        Assert.That(image.ParsedColorData.Kind, Is.EqualTo(ColorDataKind.RgbPayload));
        Assert.That(image.ParsedColorData.RawData, Is.EqualTo(payload));
        Assert.That(image.ParsedColorData.IndexedPalette, Is.Null);
    }

    /// <summary>
    /// Tests that indexed color mode data is parsed into a structured 256-color palette.
    /// </summary>
    [Test]
    public void Load_IndexedColorModeData_ParsesPalette()
    {
        byte[] payload = BuildIndexedPalettePayload();
        byte[] bytes = BuildMinimalDocument(psb: false, colorMode: ColorModes.Indexed, colorDataPayload: payload);
        using var stream = new MemoryStream(bytes);
        using var image = PsdImage.Load(stream);

        Assert.That(image.ParsedColorData.Kind, Is.EqualTo(ColorDataKind.IndexedPalette));
        Assert.That(image.ParsedColorData.RawData, Is.EqualTo(payload));
        Assert.That(image.ParsedColorData.IndexedPalette, Is.Not.Null);
        Assert.That(image.ParsedColorData.IndexedPalette!.Entries, Has.Length.EqualTo(256));
        Assert.That(image.ParsedColorData.IndexedPalette.Entries[0], Is.EqualTo(Color.FromArgb(0x00, 0xFF, 0x80)));
        Assert.That(image.ParsedColorData.IndexedPalette.Entries[17], Is.EqualTo(Color.FromArgb(0x11, 0xEE, 0x91)));
    }

    /// <summary>
    /// Tests that CMYK color mode data is classified explicitly and preserved.
    /// </summary>
    [Test]
    public void Load_CmykColorModeData_ClassifiesExplicitCmykPayload()
    {
        byte[] payload = [0xCA, 0xFE, 0xBA, 0xBE];
        byte[] bytes = BuildMinimalDocument(psb: false, colorMode: ColorModes.CMYK, colorDataPayload: payload);
        using var stream = new MemoryStream(bytes);
        using var image = PsdImage.Load(stream);

        Assert.That(image.ParsedColorData.Kind, Is.EqualTo(ColorDataKind.CmykPayload));
        Assert.That(image.ParsedColorData.RawData, Is.EqualTo(payload));
        Assert.That(image.ParsedColorData.IndexedPalette, Is.Null);
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
        byte[] bytes = BuildMinimalDocument(psb: false, resourcesPayload: resourcesPayload);
        using var stream = new MemoryStream(bytes);
        using var image = PsdImage.Load(stream);

        Assert.That(image.ParsedResources, Has.Length.EqualTo(3));
        Assert.That(image.ParsedResources[0].ResourceId, Is.EqualTo(ImageResourceIds.GlobalAngle));
        Assert.That(image.ParsedResources[0].Data, Is.EqualTo(new byte[] { 0x00, 0x00, 0x00, 0x2D }));
        Assert.That(image.ParsedResources[1].ResourceId, Is.EqualTo(ImageResourceIds.IccProfile));
        Assert.That(image.ParsedResources[1].Data, Is.EqualTo(new byte[] { 0x49, 0x43, 0x43, 0x50 }));
        Assert.That(image.ParsedResources[2].ResourceId, Is.EqualTo(ImageResourceIds.IccUntaggedProfile));
        Assert.That(image.ParsedResources[2].Data, Is.EqualTo(new byte[] { 0x01 }));
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
    /// Tests that simple document-level inspection properties expose the parsed structural state.
    /// </summary>
    [Test]
    public void Load_DocumentSimpleInspectionProperties_ReturnExpectedValues()
    {
        byte[] resourcesPayload = BuildResourcesPayload(
            (ImageResourceIds.GlobalAngle, "glba", [0x00, 0x00, 0x00, 0x2D]));
        byte[] bytes = BuildMinimalDocument(
            psb: true,
            colorMode: ColorModes.CMYK,
            colorDataPayload: [0xCA, 0xFE],
            resourcesPayload: resourcesPayload,
            compression: CompressionMethod.RZ,
            imageDataPayload: [0x78, 0xDA, 0x01, 0x02]);
        using var stream = new MemoryStream(bytes);
        using var image = PsdImage.Load(stream);

        Assert.That(image.Header, Is.Not.Null);
        Assert.That(image.IsLargeDocument, Is.True);
        Assert.That(image.IsPsb, Is.True);
        Assert.That(image.LayerCount, Is.EqualTo(0));
        Assert.That(image.HasImageResources, Is.True);
        Assert.That(image.ResourceCount, Is.EqualTo(1));
        Assert.That(image.HasColorModeData, Is.True);
        Assert.That(image.HasMergedImageData, Is.True);
        Assert.That(image.Compression, Is.EqualTo(CompressionMethod.RZ));
        Assert.That(image.ImageDataKind, Is.EqualTo(ImageDataKind.Zip));
        Assert.That(image.UsesPrediction, Is.True);
        Assert.That(image.Header.Version, Is.EqualTo(image.Version));
        Assert.That(image.Header.ColorMode, Is.EqualTo(image.ColorMode));
    }

    /// <summary>
    /// Tests that saving a document with image resources preserves the raw Image Resources section bytes.
    /// </summary>
    [Test]
    public void Save_DocumentWithImageResources_PreservesRawResourcesSection()
    {
        byte[] resourcesPayload = BuildResourcesPayload(
            (ImageResourceIds.GlobalAngle, "glba", [0x00, 0x00, 0x00, 0x2D]),
            (ImageResourceIds.IccProfile, "icc", [0x49, 0x43, 0x43, 0x50]),
            (ImageResourceIds.IccUntaggedProfile, string.Empty, [0x01]));
        byte[] originalBytes = BuildMinimalDocument(
            psb: false,
            colorMode: ColorModes.Rgb,
            resourcesPayload: resourcesPayload,
            compression: CompressionMethod.Raw,
            imageDataPayload: [0xAA, 0xBB, 0xCC, 0xDD]);
        string outputFile = GetPersistentArtifactPath("resources_roundtrip_test.psd");

        using (var stream = new MemoryStream(originalBytes))
        using (var image = PsdImage.Load(stream))
        {
            image.Save(outputFile);
            LogArtifactDirectory(outputFile);
        }

        byte[] savedBytes = File.ReadAllBytes(outputFile);
        byte[] originalResourcesSection = ExtractImageResourcesSection(originalBytes);
        byte[] savedResourcesSection = ExtractImageResourcesSection(savedBytes);

        Assert.That(savedResourcesSection, Is.EqualTo(originalResourcesSection));

        using var reloaded = PsdImage.Load(outputFile);
        Assert.That(reloaded.HasImageResources, Is.True);
        Assert.That(reloaded.ResourceCount, Is.EqualTo(3));
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
        byte[] originalBytes = BuildPsdWithSingleLayer("pass", 0x11, "Layer 1");
        string outputFile = GetPersistentArtifactPath("preserve_flags_and_blend.psd");

        using (var stream = new MemoryStream(originalBytes))
        using (var image = PsdImage.Load(stream))
        {
            image.Layers[0].Name = "Renamed";
            image.Save(outputFile);
            LogArtifactDirectory(outputFile);
        }

        using var reloaded = PsdImage.Load(outputFile);
        Assert.That(reloaded.Layers[0].BlendModeKey, Is.EqualTo("pass"));
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
    }

    /// <summary>
    /// Tests that changing a layer's clipping value persists after save and reload.
    /// </summary>
    [Test]
    public void Save_AfterChangingLayerClipping_SavesCorrectly()
    {
        byte[] originalBytes = BuildPsdWithSingleLayer("norm", 0x00, "Layer 1");
        string outputFile = GetPersistentArtifactPath("layer_clipping_test.psd");

        using (var stream = new MemoryStream(originalBytes))
        using (var image = PsdImage.Load(stream))
        {
            image.Layers[0].Clipping = 1;
            image.Save(outputFile);
            LogArtifactDirectory(outputFile);
        }

        using var reloaded = PsdImage.Load(outputFile);
        Assert.That(reloaded.Layers[0].Clipping, Is.EqualTo(1));
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
        byte[] originalBytes = BuildPsdWithSingleLayer("norm", 0x00, "Layer 1");
        string outputFile = GetPersistentArtifactPath("layer_coordinates_test.psd");

        using (var stream = new MemoryStream(originalBytes))
        using (var image = PsdImage.Load(stream))
        {
            Layer layer = image.Layers[0];
            layer.Top = 10;
            layer.Left = 20;
            layer.Bottom = 30;
            layer.Right = 50;
            image.Save(outputFile);
            LogArtifactDirectory(outputFile);
        }

        using var reloaded = PsdImage.Load(outputFile);
        Assert.That(reloaded.Layers[0].Bounds, Is.EqualTo(Rectangle.FromLTRB(20, 10, 50, 30)));
    }

    /// <summary>
    /// Tests that document-level DTO inspection API exposes unknown-only resource summaries and other structural metadata.
    /// </summary>
    [Test]
    public void Load_DocumentInspectionDtos_ReturnExpectedValues()
    {
        byte[] palettePayload = BuildIndexedPalettePayload();
        byte[] resourcesPayload = BuildResourcesPayload(
            (ImageResourceIds.GlobalAngle, "glba", [0x00, 0x00, 0x00, 0x2D]),
            (ImageResourceIds.IccProfile, "icc", [0x49, 0x43, 0x43, 0x50]),
            (ImageResourceIds.IccUntaggedProfile, string.Empty, [0x01]));
        byte[] bytes = BuildMinimalDocument(
            psb: false,
            colorMode: ColorModes.Indexed,
            colorDataPayload: palettePayload,
            resourcesPayload: resourcesPayload,
            compression: CompressionMethod.RLE,
            imageDataPayload: [0x00, 0x02, 0xAB, 0xCD, 0x00, 0x01, 0xEF]);

        using var stream = new MemoryStream(bytes);
        using var image = PsdImage.Load(stream);

        Assert.That(image.Resources, Has.Count.EqualTo(3));
        Assert.That(image.Resources[0].Kind, Is.EqualTo(PsdResourceKind.Unknown));
        Assert.That(image.Resources[1].Kind, Is.EqualTo(PsdResourceKind.Unknown));
        Assert.That(image.Resources[2].Kind, Is.EqualTo(PsdResourceKind.Unknown));
        Assert.That(image.Resources[0].GlobalAngle, Is.Null);
        Assert.That(image.Resources[1].GlobalAngle, Is.Null);
        Assert.That(image.Resources[2].GlobalAngle, Is.Null);
        Assert.That(image.HasIccProfile, Is.False);
        Assert.That(image.IsIccProfileUntagged, Is.Null);
        Assert.That(image.GlobalAngle, Is.Null);

        Assert.That(image.ColorDataInfo.Kind, Is.EqualTo(PsdColorDataKind.IndexedPalette));
        Assert.That(image.ColorDataInfo.RawDataLength, Is.EqualTo(IndexedColorPalette.ExpectedRawLength));
        Assert.That(image.IndexedPalette, Is.Not.Null);
        Assert.That(image.IndexedPalette!.Entries, Has.Count.EqualTo(256));

        Assert.That(image.ImageDataInfo.Kind, Is.EqualTo(ImageDataKind.Rle));
        Assert.That(image.ImageDataInfo.RowLengthFieldSize, Is.EqualTo(sizeof(ushort)));
        Assert.That(image.ImageDataInfo.RowByteCounts, Has.Count.EqualTo(3));
        Assert.That(image.ImageDataInfo.CompressedPayloadLength, Is.EqualTo(1));
        Assert.That(image.ImageDataInfo.UsesPrediction, Is.False);
    }

    /// <summary>
    /// Tests that layer-level DTO inspection API exposes channel and subsection summaries.
    /// </summary>
    [Test]
    public void Load_LayerInspectionDtos_ReturnExpectedValues()
    {
        byte[] originalBytes = BuildPsdWithSingleLayer("pass", 0x11, "Layer 1");

        using var stream = new MemoryStream(originalBytes);
        using var image = PsdImage.Load(stream);
        Layer layer = image.Layers[0];

        Assert.That(layer.Channels, Has.Count.EqualTo(1));
        Assert.That(layer.Channels[0].ChannelId, Is.EqualTo(0));
        Assert.That(layer.Channels[0].DataLength, Is.EqualTo((ulong)2));

        Assert.That(layer.MaskInfo.IsPresent, Is.False);
        Assert.That(layer.MaskInfo.RawDataLength, Is.EqualTo(4));
        Assert.That(layer.BlendingRangesInfo.IsPresent, Is.False);
        Assert.That(layer.BlendingRangesInfo.RawDataLength, Is.EqualTo(4));
    }

    /// <summary>
    /// Tests that PSD image data compressed with RLE round-trips without mutation.
    /// </summary>
    [Test]
    public void Save_PsdWithRleImageData_RoundTripByteForByte()
    {
        byte[] originalBytes = BuildMinimalDocument(
            psb: false,
            compression: CompressionMethod.RLE,
            imageDataPayload: [0x00, 0x02, 0xAB, 0xCD, 0x00, 0x01, 0xEF]);
        string outputFile = GetPersistentArtifactPath("minimal_rle.psd");

        using (var stream = new MemoryStream(originalBytes))
        using (var image = PsdImage.Load(stream))
        {
            image.Save(outputFile);
            LogArtifactDirectory(outputFile);
        }

        byte[] savedBytes = File.ReadAllBytes(outputFile);
        Assert.That(savedBytes, Is.EqualTo(originalBytes));
    }

    /// <summary>
    /// Tests that PSB image data compressed with RLE round-trips without mutation.
    /// </summary>
    [Test]
    public void Save_PsbWithRleImageData_RoundTripByteForByte()
    {
        byte[] originalBytes = BuildMinimalDocument(
            psb: true,
            compression: CompressionMethod.RLE,
            imageDataPayload:
            [
                0x00, 0x00, 0x00, 0x02,
                0x00, 0x00, 0x00, 0x01,
                0x00, 0x00, 0x00, 0x03,
                0xAB, 0xCD, 0xEF, 0x10, 0x11, 0x12
            ]);
        string outputFile = GetPersistentArtifactPath("minimal_rle.psb");

        using (var stream = new MemoryStream(originalBytes))
        using (var image = PsdImage.Load(stream))
        {
            image.Save(outputFile);
            LogArtifactDirectory(outputFile);
        }

        byte[] savedBytes = File.ReadAllBytes(outputFile);
        Assert.That(savedBytes, Is.EqualTo(originalBytes));
    }

    /// <summary>
    /// Tests that ZIP-compressed image data round-trips without mutation.
    /// </summary>
    [Test]
    public void Save_PsdWithZipImageData_RoundTripByteForByte()
    {
        byte[] originalBytes = BuildMinimalDocument(
            psb: false,
            compression: CompressionMethod.ZIP,
            imageDataPayload: [0x78, 0x9C, 0x63, 0x60, 0x04, 0x00, 0x00, 0xFF, 0x00]);
        string outputFile = GetPersistentArtifactPath("minimal_zip.psd");

        using (var stream = new MemoryStream(originalBytes))
        using (var image = PsdImage.Load(stream))
        {
            image.Save(outputFile);
            LogArtifactDirectory(outputFile);
        }

        byte[] savedBytes = File.ReadAllBytes(outputFile);
        Assert.That(savedBytes, Is.EqualTo(originalBytes));
    }

    /// <summary>
    /// Tests that a truncated RLE row-length table is rejected.
    /// </summary>
    [Test]
    public void Load_RleImageDataWithTruncatedRowLengthTable_ThrowsPsdLoadException()
    {
        byte[] bytes = BuildMinimalDocument(
            psb: false,
            compression: CompressionMethod.RLE,
            imageDataPayload: [0x00, 0x02, 0xAB, 0xCD, 0x00]);
        using var stream = new MemoryStream(bytes);

        Assert.That(() => PsdImage.Load(stream), Throws.InstanceOf<PsdLoadException>());
    }

    /// <summary>
    /// Tests that a minimal PSB file loads and round-trips correctly without mutations.
    /// </summary>
    [Test]
    public void Save_PsbWithoutLayers_RoundTripByteForByte()
    {
        byte[] originalBytes = BuildMinimalDocument(psb: true);
        string outputFile = GetPersistentArtifactPath("minimal.psb");

        using (var stream = new MemoryStream(originalBytes))
        using (var image = PsdImage.Load(stream))
        {
            Assert.That(image.Version, Is.EqualTo(PsdHeader.PsbVersion));
            Assert.That(image.Layers, Is.Empty);
            image.Save(outputFile);
            LogArtifactDirectory(outputFile);
        }

        byte[] savedBytes = File.ReadAllBytes(outputFile);
        Assert.That(savedBytes, Is.EqualTo(originalBytes));
    }

    /// <summary>
    /// Tests that a PSB fixture with a real layer record loads layer metadata and round-trips unchanged.
    /// </summary>
    [Test]
    public void Save_PsbWithLayerRecord_RoundTripByteForByte()
    {
        byte[] originalBytes = BuildPsbWithSingleLayerDocument();
        string outputFile = GetPersistentArtifactPath("layered.psb");

        using (var stream = new MemoryStream(originalBytes))
        using (var image = PsdImage.Load(stream))
        {
            Assert.That(image.Version, Is.EqualTo(PsdHeader.PsbVersion));
            Assert.That(image.Layers, Has.Length.EqualTo(1));
            Assert.That(image.Layers[0].Name, Is.EqualTo("Layer 1"));
            Assert.That(image.Layers[0].Bounds, Is.EqualTo(new Rectangle(0, 0, 1, 1)));
            Assert.That(image.Layers[0].Opacity, Is.EqualTo(200));
            Assert.That(image.Layers[0].IsVisible, Is.True);

            image.Save(outputFile);
            LogArtifactDirectory(outputFile);
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

    private static byte[] BuildMinimalDocument(
        bool psb,
        CompressionMethod compression = CompressionMethod.Raw,
        byte[]? imageDataPayload = null,
        ColorModes colorMode = ColorModes.Rgb,
        byte[]? colorDataPayload = null,
        byte[]? resourcesPayload = null)
    {
        using var stream = new MemoryStream();
        imageDataPayload ??= [0x00, 0x00, 0x00];
        colorDataPayload ??= [];
        resourcesPayload ??= [];

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
        WriteUInt16((ushort)colorMode);

        WriteUInt32((uint)colorDataPayload.Length);
        stream.Write(colorDataPayload);
        WriteUInt32((uint)resourcesPayload.Length);
        stream.Write(resourcesPayload);
        if (psb)
        {
            WriteUInt64(0);
        }
        else
        {
            WriteUInt32(0);
        }

        WriteUInt16((ushort)compression);
        stream.Write(imageDataPayload);

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

    private static byte[] BuildPsbWithSingleLayerDocument()
    {
        using var stream = new MemoryStream();
        using var writer = new BigEndianWriter(stream, leaveOpen: true);

        stream.Write(System.Text.Encoding.ASCII.GetBytes("8BPS"));
        writer.Write((ushort)PsdHeader.PsbVersion);
        writer.Write(new byte[6]);
        writer.Write((ushort)3);
        writer.Write(1);
        writer.Write(1);
        writer.Write((ushort)8);
        writer.Write((ushort)ColorModes.Rgb);
        writer.Write((uint)0);
        writer.Write((uint)0);

        byte[] layerAndMaskSection = BuildPsbLayerAndMaskSection();
        writer.Write((ulong)layerAndMaskSection.Length);
        writer.Write(layerAndMaskSection);

        writer.Write((ushort)CompressionMethod.Raw);
        writer.Write(new byte[] { 0x00, 0x00, 0x00 });

        return stream.ToArray();
    }

    private static byte[] BuildPsbLayerAndMaskSection()
    {
        using var layerInfoPayloadStream = new MemoryStream();
        using var layerInfoWriter = new BigEndianWriter(layerInfoPayloadStream, leaveOpen: true);

        layerInfoWriter.Write((short)1);
        layerInfoWriter.Write(0);
        layerInfoWriter.Write(0);
        layerInfoWriter.Write(1);
        layerInfoWriter.Write(1);
        layerInfoWriter.Write((ushort)1);
        layerInfoWriter.Write((short)0);
        layerInfoWriter.Write((ulong)2);
        layerInfoWriter.Write(0x3842494D);
        layerInfoWriter.Write(System.Text.Encoding.ASCII.GetBytes("norm"));
        layerInfoWriter.Write((byte)200);
        layerInfoWriter.Write((byte)0);
        layerInfoWriter.Write((byte)0);
        layerInfoWriter.Write((byte)0);

        using var extraDataStream = new MemoryStream();
        using var extraDataWriter = new BigEndianWriter(extraDataStream, leaveOpen: true);
        extraDataWriter.Write((uint)0);
        extraDataWriter.Write((uint)0);
        extraDataWriter.WritePascalString("Layer 1");

        byte[] extraData = extraDataStream.ToArray();
        layerInfoWriter.Write(extraData.Length);
        layerInfoWriter.Write(extraData);

        layerInfoWriter.Write((ushort)CompressionMethod.Raw);

        byte[] layerInfoPayload = layerInfoPayloadStream.ToArray();

        using var sectionStream = new MemoryStream();
        using var sectionWriter = new BigEndianWriter(sectionStream, leaveOpen: true);
        sectionWriter.Write((long)layerInfoPayload.Length);
        sectionWriter.Write(layerInfoPayload);

        return sectionStream.ToArray();
    }

    private static byte[] BuildPsdWithSingleLayer(string blendModeKey, byte flags, string layerName)
    {
        using var stream = new MemoryStream();
        using var writer = new BigEndianWriter(stream, leaveOpen: true);

        stream.Write(System.Text.Encoding.ASCII.GetBytes("8BPS"));
        writer.Write((ushort)PsdHeader.PsdVersion);
        writer.Write(new byte[6]);
        writer.Write((ushort)1);
        writer.Write(1);
        writer.Write(1);
        writer.Write((ushort)8);
        writer.Write((ushort)ColorModes.Rgb);
        writer.Write((uint)0);
        writer.Write((uint)0);

        byte[] layerAndMaskSection = BuildPsdLayerAndMaskSection(blendModeKey, flags, layerName);
        writer.Write((uint)layerAndMaskSection.Length);
        writer.Write(layerAndMaskSection);

        writer.Write((ushort)CompressionMethod.Raw);
        writer.Write(new byte[] { 0x00 });

        return stream.ToArray();
    }

    private static byte[] BuildPsdLayerAndMaskSection(string blendModeKey, byte flags, string layerName)
    {
        using var layerInfoPayloadStream = new MemoryStream();
        using var layerInfoWriter = new BigEndianWriter(layerInfoPayloadStream, leaveOpen: true);

        layerInfoWriter.Write((short)1);
        layerInfoWriter.Write(0);
        layerInfoWriter.Write(0);
        layerInfoWriter.Write(1);
        layerInfoWriter.Write(1);
        layerInfoWriter.Write((ushort)1);
        layerInfoWriter.Write((short)0);
        layerInfoWriter.Write((uint)2);
        layerInfoWriter.Write(0x3842494D);
        layerInfoWriter.Write(System.Text.Encoding.ASCII.GetBytes(blendModeKey));
        layerInfoWriter.Write((byte)255);
        layerInfoWriter.Write((byte)0);
        layerInfoWriter.Write(flags);
        layerInfoWriter.Write((byte)0);

        using var extraDataStream = new MemoryStream();
        using var extraDataWriter = new BigEndianWriter(extraDataStream, leaveOpen: true);
        extraDataWriter.Write((uint)0);
        extraDataWriter.Write((uint)0);
        extraDataWriter.WritePascalString(layerName);

        byte[] extraData = extraDataStream.ToArray();
        layerInfoWriter.Write(extraData.Length);
        layerInfoWriter.Write(extraData);
        layerInfoWriter.Write((ushort)CompressionMethod.Raw);

        byte[] layerInfoPayload = layerInfoPayloadStream.ToArray();

        using var sectionStream = new MemoryStream();
        using var sectionWriter = new BigEndianWriter(sectionStream, leaveOpen: true);
        sectionWriter.Write(layerInfoPayload.Length);
        sectionWriter.Write(layerInfoPayload);

        return sectionStream.ToArray();
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
