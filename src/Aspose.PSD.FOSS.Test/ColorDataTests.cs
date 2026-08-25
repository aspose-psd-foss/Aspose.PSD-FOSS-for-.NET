using Aspose.PSD.FileFormats.Psd;
using System.IO;
using NUnit.Framework;

namespace Aspose.PSD.FOSS.Tests;

/// <summary>
/// Contains ColorData tests.
/// </summary>
public sealed class ColorDataTests : PsdTestFixtureBase
{

    /// <summary>
    /// Tests that a malformed color mode length field is rejected with <see cref="PsdLoadException"/>.
    /// </summary>
    [Test]
    public void Load_TooLongColorMode_Throws()
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
    public void Load_RgbPayload_Classifies()
    {
        byte[] payload = [0x10, 0x20, 0x30, 0x40];
        using var stream = new MemoryStream(BuildColorDataSection(payload));
        using var reader = new BigEndianReader(stream, leaveOpen: true);

        ColorData colorData = ColorData.Load(reader, ColorModes.Rgb);

        Assert.That(colorData.Kind, Is.EqualTo(PsdColorDataKind.RgbPayload));
        Assert.That(colorData.RawData, Is.EqualTo(payload));
        Assert.That(colorData.IndexedPalette, Is.Null);
    }


    /// <summary>
    /// Tests that indexed color mode data is parsed into a structured 256-color palette.
    /// </summary>
    [Test]
    public void Load_IndexedData_ParsesPalette()
    {
        byte[] payload = BuildIndexedPalettePayload();
        using var stream = new MemoryStream(BuildColorDataSection(payload));
        using var reader = new BigEndianReader(stream, leaveOpen: true);

        ColorData colorData = ColorData.Load(reader, ColorModes.Indexed);

        Assert.That(colorData.Kind, Is.EqualTo(PsdColorDataKind.IndexedPalette));
        Assert.That(colorData.RawData, Is.EqualTo(payload));
        Assert.That(colorData.IndexedPalette, Is.Not.Null);
        Assert.That(colorData.IndexedPalette!.Entries, Has.Length.EqualTo(256));
        Assert.That(colorData.IndexedPalette.Entries[0], Is.EqualTo(System.Drawing.Color.FromArgb(0x00, 0xFF, 0x80)));
        Assert.That(colorData.IndexedPalette.Entries[17], Is.EqualTo(System.Drawing.Color.FromArgb(0x11, 0xEE, 0x91)));
    }


    /// <summary>
    /// Tests that CMYK color mode data is classified explicitly and preserved.
    /// </summary>
    [Test]
    public void Load_CmykPayload_Classifies()
    {
        byte[] payload = [0xCA, 0xFE, 0xBA, 0xBE];
        using var stream = new MemoryStream(BuildColorDataSection(payload));
        using var reader = new BigEndianReader(stream, leaveOpen: true);

        ColorData colorData = ColorData.Load(reader, ColorModes.Cmyk);

        Assert.That(colorData.Kind, Is.EqualTo(PsdColorDataKind.CmykPayload));
        Assert.That(colorData.RawData, Is.EqualTo(payload));
        Assert.That(colorData.IndexedPalette, Is.Null);
    }


    /// <summary>
    /// Tests that the basic indexed fixture exposes indexed color mode data and palette metadata.
    /// </summary>
    [Test]
    public void Load_IndexedFixture_ReadsData()
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


    /// <summary>
    /// Tests that saving the basic indexed fixture without mutations preserves the file byte-for-byte.
    /// </summary>
    [Test]
    public void Save_IndexedFixture_IsByteExact()
    {
        AssertByteExactRoundTrip("basic-indexed.psd");
    }
}
