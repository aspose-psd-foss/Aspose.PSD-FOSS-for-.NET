using Aspose.PSD.FileFormats.Core.Blending;
using Aspose.PSD.FileFormats.Psd;
using System.IO;
using NUnit.Framework;

namespace Aspose.PSD.FOSS.Tests;

/// <summary>
/// Contains FixtureDocument tests.
/// </summary>
public sealed class FixtureDocumentTests : PsdTestFixtureBase
{

    /// <summary>
    /// Tests that the basic RGB fixture exposes expected PSD document metadata, layers, resources, and compression.
    /// </summary>
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


    /// <summary>
    /// Tests that saving the basic RGB fixture without mutations preserves the file byte-for-byte.
    /// </summary>
    [Test]
    public void Save_BasicRgbFixturePsd_RoundTripWithoutMutation_ByteExact()
    {
        AssertByteExactRoundTrip("basic-rgb.psd");
    }


    /// <summary>
    /// Tests that renaming a layer in the basic RGB fixture persists after save and reload.
    /// </summary>
    [Test]
    public void Save_BasicRgbFixturePsd_AfterChangingLayerName_SavesCorrectly()
    {
        AssertRenameSave("basic-rgb.psd", layerIndex: 1, newName: "Renamed Rectangle");
    }


    /// <summary>
    /// Tests that the basic CMYK fixture exposes expected channels, resources, layers, compression, and row metadata.
    /// </summary>
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


    /// <summary>
    /// Tests that saving the basic CMYK fixture without mutations preserves the file byte-for-byte.
    /// </summary>
    [Test]
    public void Save_BasicCmykFixturePsd_RoundTripWithoutMutation_ByteExact()
    {
        AssertByteExactRoundTrip("basic-cmyk.psd");
    }


    /// <summary>
    /// Tests that the basic PSB fixture exposes PSB version, large-document state, resources, and RLE row-length sizing.
    /// </summary>
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


    /// <summary>
    /// Tests that saving the basic PSB fixture without mutations preserves the file byte-for-byte.
    /// </summary>
    [Test]
    public void Save_BasicPsbFixture_RoundTripWithoutMutation_ByteExact()
    {
        AssertByteExactRoundTrip("basic.psb");
    }


    /// <summary>
    /// Tests that the layered PSB fixture exposes expected layer names, layer count, and opacity metadata.
    /// </summary>
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


    /// <summary>
    /// Tests that renaming a layer in the layered PSB fixture persists after save and reload.
    /// </summary>
    [Test]
    public void Save_LayeredPsbFixture_AfterChangingLayerName_SavesCorrectly()
    {
        AssertRenameSave("layered.psb", layerIndex: 1, newName: "Renamed Rectangle");
    }


    /// <summary>
    /// Tests that changing blend mode in the layered PSB fixture persists with the expected PSD blend key.
    /// </summary>
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
        Assert.That(reloaded.Layers[1].BlendModeKey, Is.EqualTo(BlendMode.Multiply));
    }


    /// <summary>
    /// Tests that the layer variants fixture exposes visibility, unknown blend key, clipping, and additional data flags.
    /// </summary>
    [Test]
    public void Load_LayerVariantsFixturePsd_ReturnsExpectedLayerFlags()
    {
        using var image = PsdImage.Load(GetTestDataPath("layer-variants.psd"));

        Assert.That(image.LayerCount, Is.EqualTo(4));
        Assert.That(image.Layers[1].IsVisible, Is.False);
        Assert.That(image.Layers[2].BlendModeKey, Is.EqualTo(BlendMode.LinearBurn));
        Assert.That(image.Layers[2].RawBlendModeKey, Is.EqualTo("lbrn"));
        Assert.That(image.Layers[3].Clipping, Is.EqualTo(1));
        Assert.That(image.Layers[3].HasAdditionalLayerData, Is.True);
    }


    /// <summary>
    /// Tests that changing clipping in the layer variants fixture persists after save and reload.
    /// </summary>
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


    /// <summary>
    /// Tests that mutating a layer in the variants fixture preserves the layer-and-mask tail bytes.
    /// </summary>
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
}
