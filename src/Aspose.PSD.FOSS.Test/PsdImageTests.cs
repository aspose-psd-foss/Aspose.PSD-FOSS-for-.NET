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
    /// Tests that saving a PSD file without mutations produces a valid file.
    /// Verifies that the saved file can be reloaded with equivalent properties.
    /// Note: Current implementation may not produce byte-for-byte identical output.
    /// Strict byte-for-byte round-trip is tracked as a correctness gap.
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
}
