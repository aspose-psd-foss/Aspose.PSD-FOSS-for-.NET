using System.IO;
using NUnit.Framework;
using Aspose.PSD.FileFormats.Core.Blending;
using Aspose.PSD.FileFormats.Psd;
using Aspose.PSD.FileFormats.Psd.Layers;

namespace Aspose.PSD.FOSS.Tests;

/// <summary>
/// Contains source-compatibility tests for the documented Aspose.PSD API subset implemented by the FOSS package.
/// </summary>
public sealed class AsposeCompatibilityTests : PsdTestFixtureBase
{
    /// <summary>
    /// Tests that official-style code can load a PSD through <see cref="Image.Load(string)"/> and cast it to <see cref="PsdImage"/>.
    /// </summary>
    [Test]
    public void ImageLoad_WithOfficialNamespaces_ReturnsPsdImage()
    {
        using Image image = Image.Load(GetTestDataPath("test.psd"));
        var psdImage = (PsdImage)image;

        Assert.That(psdImage.Width, Is.EqualTo(100));
        Assert.That(psdImage.ChannelsCount, Is.EqualTo(3));
        Assert.That(psdImage.Layers, Has.Length.EqualTo(2));
    }

    /// <summary>
    /// Tests that official-style layer metadata access uses Aspose.PSD Rectangle and BlendMode types.
    /// </summary>
    [Test]
    public void LayerMetadata_WithOfficialTypes_ExposesCompatibleSubset()
    {
        using var image = (PsdImage)Image.Load(GetTestDataPath("test.psd"));
        Layer layer = image.Layers[0];

        Rectangle bounds = layer.Bounds;
        BlendMode blendModeKey = layer.BlendModeKey;

        Assert.That(bounds, Is.EqualTo(Rectangle.FromLTRB(0, 0, 100, 100)));
        Assert.That(blendModeKey, Is.EqualTo(BlendMode.Normal));
        Assert.That(layer.Opacity, Is.EqualTo(255));
        Assert.That(layer.IsVisible, Is.True);
    }

    /// <summary>
    /// Tests that official-style PSD image setters compile and update the implemented structural metadata subset.
    /// </summary>
    [Test]
    public void PsdImageSetters_WithOfficialShape_UpdateImplementedMetadata()
    {
        using var image = (PsdImage)Image.Load(GetTestDataPath("test.psd"));
        Layer[] layers = image.Layers;

        image.ColorMode = ColorModes.Rgb;
        image.Layers = layers;
        image.GlobalAngle = 45;

        Assert.That(image.ColorMode, Is.EqualTo(ColorModes.Rgb));
        Assert.That(image.Layers, Has.Length.EqualTo(layers.Length));
        Assert.That(image.GlobalAngle, Is.EqualTo(45));
    }

    /// <summary>
    /// Tests that official-style layer mutation and save flow works through the implemented compatibility subset.
    /// </summary>
    [Test]
    public void Save_AfterOfficialStyleLayerMutation_PersistsCompatibleSubset()
    {
        string outputFile = GetPersistentArtifactPath("official_style_layer_mutation.psd");

        using (var image = (PsdImage)Image.Load(GetTestDataPath("test.psd")))
        {
            image.Layers[0].Name = "Official style";
            image.Layers[0].Bounds = Rectangle.FromLTRB(10, 20, 40, 60);
            image.Layers[0].BlendModeKey = BlendMode.Multiply;
            image.Save(outputFile);
        }

        using var reloaded = (PsdImage)Image.Load(outputFile);
        Assert.That(reloaded.Layers[0].Name, Is.EqualTo("Official style"));
        Assert.That(reloaded.Layers[0].Bounds, Is.EqualTo(Rectangle.FromLTRB(10, 20, 40, 60)));
        Assert.That(reloaded.Layers[0].BlendModeKey, Is.EqualTo(BlendMode.Multiply));
    }
}
