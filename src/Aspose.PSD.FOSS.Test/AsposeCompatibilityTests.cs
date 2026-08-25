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
    public void ImageLoad_Official_ReturnsPsd()
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
    public void Metadata_OfficialTypes_Reads()
    {
        using var image = (PsdImage)Image.Load(GetTestDataPath("test.psd"));
        Layer layer = image.Layers[0];

        Rectangle bounds = layer.Bounds;
        BlendMode blendModeKey = layer.BlendModeKey;

        Assert.That(bounds, Is.EqualTo(Rectangle.FromLeftTopRightBottom(0, 0, 100, 100)));
        Assert.That(blendModeKey, Is.EqualTo(BlendMode.Normal));
        Assert.That(layer.Opacity, Is.EqualTo(255));
        Assert.That(layer.IsVisible, Is.True);
    }

    /// <summary>
    /// Tests that official-style layer channel properties expose parsed channel metadata and reject unsupported channel rewrites.
    /// </summary>
    [Test]
    public void Channels_OfficialShape_ReadsOnly()
    {
        using var image = (PsdImage)Image.Load(GetTestDataPath("test.psd"));
        Layer layer = image.Layers[0];

        ChannelInformation[] channelInformation = layer.ChannelInformation;

        Assert.That(layer.ChannelsCount, Is.EqualTo(channelInformation.Length));
        Assert.That(channelInformation, Is.Not.Empty);
        Assert.That(channelInformation[0].Length, Is.GreaterThan(0));
        Assert.That(() => layer.ChannelInformation = channelInformation, Throws.TypeOf<NotSupportedException>());
    }

    /// <summary>
    /// Tests that official-style layer mask and blending range properties expose supported metadata and reject unsupported semantic rewrites.
    /// </summary>
    [Test]
    public void LayerMask_OfficialShape_Reads()
    {
        using var image = (PsdImage)Image.Load(GetTestDataPath("test.psd"));
        Layer layer = image.Layers[0];

        Assert.That(layer.LayerMaskData, Is.Null);
        Assert.That(layer.LayerBlendingRangesData.Length, Is.GreaterThanOrEqualTo(4));
        Assert.That(() => layer.LayerMaskData = null, Throws.TypeOf<NotSupportedException>());
        Assert.That(() => layer.LayerBlendingRangesData = new Aspose.PSD.FileFormats.Psd.Layers.LayerBlendingRangesData(), Throws.TypeOf<NotSupportedException>());
    }

    /// <summary>
    /// Tests that the Aspose.PSD-compatible rectangle value types expose mutable location and size members.
    /// </summary>
    [Test]
    public void Rectangle_MutableShape_Updates()
    {
        var rectangle = new Rectangle(new Point(10, 20), new Size(30, 40));

        rectangle.Location = new Point(15, 25);
        rectangle.Size = new Size(35, 45);
        rectangle.Right = 60;
        rectangle.Bottom = 80;

        Assert.That(rectangle.X, Is.EqualTo(15));
        Assert.That(rectangle.Y, Is.EqualTo(25));
        Assert.That(rectangle.Width, Is.EqualTo(45));
        Assert.That(rectangle.Height, Is.EqualTo(55));
        Assert.That(rectangle.Contains(new Point(30, 40)), Is.True);
        Assert.That(rectangle.IntersectsWith(Rectangle.FromLeftTopRightBottom(50, 70, 70, 90)), Is.True);

        rectangle.Offset(new Point(5, -5));
        rectangle.Inflate(new Size(1, 2));

        Assert.That(rectangle, Is.EqualTo(Rectangle.FromLeftTopRightBottom(19, 18, 66, 77)));
    }

    /// <summary>
    /// Tests that official-style PSD image setters compile and update the implemented structural metadata subset.
    /// </summary>
    [Test]
    public void ImageSetters_UpdateMetadata()
    {
        using var image = (PsdImage)Image.Load(GetTestDataPath("test.psd"));
        Layer[] layers = image.Layers;
        int version = image.Version;

        image.ColorMode = ColorModes.Rgb;
        image.Layers = layers;
        image.GlobalAngle = 45;
        image.Version = version;

        Assert.That(image.ColorMode, Is.EqualTo(ColorModes.Rgb));
        Assert.That(image.Layers, Has.Length.EqualTo(layers.Length));
        Assert.That(image.GlobalAngle, Is.EqualTo(45));
        Assert.That(image.Version, Is.EqualTo(version));
    }

    /// <summary>
    /// Tests that official-style PSD image resource and document state properties expose the supported compatibility subset.
    /// </summary>
    [Test]
    public void Resources_OfficialShape_Reads()
    {
        using var image = (PsdImage)Image.Load(GetTestDataPath("test.psd"));

        ResourceBlock[] imageResources = image.ImageResources;
        LayerResource[] globalLayerResources = image.GlobalLayerResources;
        GlobalLayerMaskInfo globalLayerMaskInfo = image.GlobalLayerMaskInfo;
        Layer? activeLayer = image.ActiveLayer;

        Assert.That(image.Size, Is.EqualTo(new Size(image.Width, image.Height)));
        Assert.That(imageResources, Is.Not.Empty);
        Assert.That(imageResources[0].ID, Is.Not.EqualTo(0));
        Assert.That(imageResources[0].DataSize, Is.GreaterThanOrEqualTo(0));
        Assert.That(globalLayerResources, Is.Empty);
        Assert.That(globalLayerMaskInfo, Is.Not.Null);
        Assert.That(activeLayer, Is.SameAs(image.Layers[0]));
        Assert.That(image.IsFlatten, Is.False);
        Assert.That(image.HasTransparencyData, Is.False);
        Assert.That(() => image.ImageResources = imageResources, Throws.TypeOf<NotSupportedException>());
        Assert.That(() => image.GlobalLayerResources = globalLayerResources, Throws.TypeOf<NotSupportedException>());
        Assert.That(() => image.ActiveLayer = activeLayer, Throws.TypeOf<NotSupportedException>());
        Assert.That(() => image.HasTransparencyData = true, Throws.TypeOf<NotSupportedException>());
    }

    /// <summary>
    /// Tests that official-style layer mutation and save flow works through the implemented compatibility subset.
    /// </summary>
    [Test]
    public void Save_OfficialMutation_Persists()
    {
        string outputFile = GetPersistentArtifactPath("official_style_layer_mutation.psd");

        using (var image = (PsdImage)Image.Load(GetTestDataPath("test.psd")))
        {
            image.Layers[0].Name = "Official style";
            image.Layers[0].Left = 10;
            image.Layers[0].Top = 20;
            image.Layers[0].Right = 40;
            image.Layers[0].Bottom = 60;
            image.Layers[0].BlendModeKey = BlendMode.Multiply;
            image.Save(outputFile);
        }

        using var reloaded = (PsdImage)Image.Load(outputFile);
        Assert.That(reloaded.Layers[0].Name, Is.EqualTo("Official style"));
        Assert.That(reloaded.Layers[0].Bounds, Is.EqualTo(Rectangle.FromLeftTopRightBottom(10, 20, 40, 60)));
        Assert.That(reloaded.Layers[0].BlendModeKey, Is.EqualTo(BlendMode.Multiply));
    }
}
