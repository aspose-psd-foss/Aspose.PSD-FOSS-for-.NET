using Aspose.PSD.FileFormats.Psd;
using Aspose.PSD.FileFormats.Psd.Layers;
using System.IO;
using NUnit.Framework;

namespace Aspose.PSD.FOSS.Tests;

/// <summary>
/// Contains InspectionDTO tests.
/// </summary>
public sealed class InspectionDtoTests : PsdTestFixtureBase
{

    /// <summary>
    /// Tests that document-level DTO inspection API exposes unknown-only resource summaries and other structural metadata.
    /// </summary>
    [Test]
    public void Load_DocumentDtos_ReadValues()
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
        Assert.That(image.GlobalAngle, Is.EqualTo(0));

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
    public void Load_LayerDtos_ReadValues()
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
}
