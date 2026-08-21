using Aspose.PSD;
using Aspose.PSD.FileFormats.Core.Blending;
using Aspose.PSD.FileFormats.Psd;
using Aspose.PSD.FileFormats.Psd.Layers;
using System.IO;
using NUnit.Framework;

namespace Aspose.PSD.FOSS.Tests;

/// <summary>
/// Contains LayerSection tests.
/// </summary>
public sealed class LayerSectionTests : PsdTestFixtureBase
{

    /// <summary>
    /// Tests that a malformed layer and mask section length is rejected with <see cref="PsdLoadException"/>.
    /// </summary>
    [Test]
    public void Load_TooLongLayerMask_Throws()
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
    public void Load_TooLongLayerExtra_Throws()
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
    public void Load_NegativeLayerExtra_Throws()
    {
        byte[] layerBytes = BuildLayerRecordBytesWithExtraData([]);
        WriteUInt32BigEndian(layerBytes, 30, 0xFFFFFFFF);
        using var stream = new MemoryStream(layerBytes);
        using var reader = new BigEndianReader(stream, leaveOpen: true);

        Assert.That(() => Layer.Load(reader, isLargeDocument: false), Throws.InstanceOf<PsdLoadException>());
    }


    /// <summary>
    /// Tests that simple layer-level inspection properties expose stored geometry and subsection presence.
    /// </summary>
    [Test]
    public void Load_LayerInspection_ReadsValues()
    {
        using var image = PsdImage.Load(Path.Combine(TestContext.CurrentContext.TestDirectory, "testdata", "test.psd"));
        Layer firstLayer = image.Layers[0];

        Assert.That(firstLayer.Width, Is.EqualTo(firstLayer.Bounds.Width));
        Assert.That(firstLayer.Height, Is.EqualTo(firstLayer.Bounds.Height));
        Assert.That(firstLayer.Top, Is.EqualTo(firstLayer.Bounds.Top));
        Assert.That(firstLayer.Left, Is.EqualTo(firstLayer.Bounds.Left));
        Assert.That(firstLayer.Bottom, Is.EqualTo(firstLayer.Bounds.Bottom));
        Assert.That(firstLayer.Right, Is.EqualTo(firstLayer.Bounds.Right));
        Assert.That(firstLayer.ChannelsCount, Is.GreaterThan(0));
        Assert.That(firstLayer.RawBlendModeKey, Has.Length.EqualTo(4));
        Assert.That(firstLayer.LayerMaskData, Is.Null);
        Assert.That(firstLayer.LayerBlendingRangesData.Length, Is.EqualTo(firstLayer.BlendingRangesInfo.RawDataLength));
        Assert.That(firstLayer.HasAdditionalLayerData, Is.True);
    }


    /// <summary>
    /// Tests that changing a layer's blend mode persists after save and reload.
    /// </summary>
    [Test]
    public void Save_ChangesBlendMode()
    {
        string testFile = Path.Combine(TestContext.CurrentContext.TestDirectory, "testdata", "test.psd");
        string outputFile = GetPersistentArtifactPath("layer_blend_mode_test.psd");

        using var image = PsdImage.Load(testFile);
        image.Layers[0].BlendMode = BlendMode.Multiply;
        image.Save(outputFile);
        LogArtifactDirectory(outputFile);

        using var reloaded = PsdImage.Load(outputFile);
        Assert.That(reloaded.Layers[0].BlendMode, Is.EqualTo(BlendMode.Multiply));
        Assert.That(reloaded.Layers[0].BlendModeKey, Is.EqualTo(BlendMode.Multiply));

        byte[] originalBytes = File.ReadAllBytes(testFile);
        byte[] savedBytes = File.ReadAllBytes(outputFile);
        Assert.That(ReadLayerInfoLength(originalBytes), Is.EqualTo(ReadLayerInfoLength(savedBytes)));
    }


    /// <summary>
    /// Tests that changing a layer's clipping value persists after save and reload.
    /// </summary>
    [Test]
    public void Save_ChangesClipping()
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
    public void Save_ChangesBounds()
    {
        string testFile = Path.Combine(TestContext.CurrentContext.TestDirectory, "testdata", "test.psd");
        string outputFile = GetPersistentArtifactPath("layer_bounds_test.psd");

        using var image = PsdImage.Load(testFile);
        Rectangle newBounds = Rectangle.FromLeftTopRightBottom(10, 20, 40, 60);
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
    public void Save_ChangesCoordinates()
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
        Assert.That(reloaded.Layers[0].Bounds, Is.EqualTo(Rectangle.FromLeftTopRightBottom(20, 10, 50, 30)));
    }


    /// <summary>
    /// Tests that a synthetic PSB fixture with one layer record loads expected layer metadata.
    /// </summary>
    [Test]
    public void Load_PsbLayerRecord_ReadsMetadata()
    {
        using var stream = new MemoryStream(BuildPsbLayerRecordBytes());
        using var reader = new BigEndianReader(stream, leaveOpen: true);

        Layer layer = Layer.Load(reader, isLargeDocument: true);

        Assert.That(layer.Name, Is.EqualTo("Layer 1"));
        Assert.That(layer.Bounds, Is.EqualTo(Rectangle.FromLeftTopRightBottom(0, 0, 1, 1)));
        Assert.That(layer.Opacity, Is.EqualTo(200));
        Assert.That(layer.IsVisible, Is.True);
    }
}
