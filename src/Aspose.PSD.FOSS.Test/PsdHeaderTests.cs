using Aspose.PSD.FileFormats.Psd;
using System.IO;
using NUnit.Framework;

namespace Aspose.PSD.FOSS.Tests;

/// <summary>
/// Contains PSD Header tests.
/// </summary>
public sealed class PsdHeaderTests : PsdTestFixtureBase
{

    /// <summary>
    /// Tests that non-zero reserved header bytes are rejected.
    /// </summary>
    [Test]
    public void Load_NonZeroReserved_Throws()
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
    public void Load_InvalidHeaderField_Throws(
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
    /// Tests that a minimal PSD without layers loads correctly.
    /// </summary>
    [Test]
    public void Load_PsdHeader_ReadsMetadata()
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
    /// Tests that a minimal synthetic PSB file loads expected document metadata without layers.
    /// </summary>
    [Test]
    public void Load_PsbHeader_ReadsMetadata()
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
}
