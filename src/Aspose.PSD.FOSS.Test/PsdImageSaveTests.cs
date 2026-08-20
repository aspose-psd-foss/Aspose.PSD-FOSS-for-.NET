using System.IO;
using NUnit.Framework;

namespace Aspose.PSD.FOSS.Tests;

/// <summary>
/// Contains PSD ImageSave tests.
/// </summary>
public sealed class PsdImageSaveTests : PsdTestFixtureBase
{

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
}
