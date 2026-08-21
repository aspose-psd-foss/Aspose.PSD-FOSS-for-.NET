using Aspose.PSD.FileFormats.Psd;
using System.IO;
using NUnit.Framework;

namespace Aspose.PSD.FOSS.Tests;

/// <summary>
/// Contains ImageData tests.
/// </summary>
public sealed class ImageDataTests : PsdTestFixtureBase
{

    /// <summary>
    /// Tests that the RLE fixture exposes expected compression, row-length table, and compressed payload metadata.
    /// </summary>
    [Test]
    public void Load_RleFixture_ReadsImageData()
    {
        using var image = PsdImage.Load(GetTestDataPath("rle.psd"));

        Assert.That(image.Compression, Is.EqualTo(CompressionMethod.RLE));
        Assert.That(image.ImageDataKind, Is.EqualTo(ImageDataKind.Rle));
        Assert.That(image.ImageDataInfo.RowLengthFieldSize, Is.EqualTo(sizeof(ushort)));
        Assert.That(image.ImageDataInfo.RowByteCounts, Has.Count.EqualTo(600));
        Assert.That(image.ImageDataInfo.CompressedPayloadLength, Is.EqualTo(5219));
    }


    /// <summary>
    /// Tests that saving the RLE fixture without mutations preserves the file byte-for-byte.
    /// </summary>
    [Test]
    public void Save_RleFixture_IsByteExact()
    {
        AssertByteExactRoundTrip("rle.psd");
    }


    /// <summary>
    /// Tests that the ZIP-named fixture exposes the expected preserved RLE image-data structure.
    /// </summary>
    [Test]
    public void Load_ZipFixture_ReadsImageData()
    {
        using var image = PsdImage.Load(GetTestDataPath("zip.psd"));

        Assert.That(image.Compression, Is.EqualTo(CompressionMethod.RLE));
        Assert.That(image.ImageDataKind, Is.EqualTo(ImageDataKind.Rle));
        Assert.That(image.ImageDataInfo.RowLengthFieldSize, Is.EqualTo(sizeof(ushort)));
        Assert.That(image.ImageDataInfo.RowByteCounts, Has.Count.EqualTo(600));
        Assert.That(image.ImageDataInfo.CompressedPayloadLength, Is.EqualTo(5219));
    }


    /// <summary>
    /// Tests that saving the ZIP-named fixture without mutations preserves the file byte-for-byte.
    /// </summary>
    [Test]
    public void Save_ZipFixture_IsByteExact()
    {
        AssertByteExactRoundTrip("zip.psd");
    }


    /// <summary>
    /// Tests that a minimal synthetic PSD with RLE image data exposes the expected structural metadata.
    /// </summary>
    [Test]
    public void Load_PsdRle_ReadsStructure()
    {
        using var stream = new MemoryStream(BuildImageDataSection(CompressionMethod.RLE, [0x00, 0x02, 0xAB, 0xCD, 0x00, 0x01, 0xEF]));
        using var reader = new BigEndianReader(stream, leaveOpen: true);

        ImageData imageData = ImageData.Load(reader, isLargeDocument: false, height: 1, channelCount: 3);

        Assert.That(imageData.Compression, Is.EqualTo(CompressionMethod.RLE));
        Assert.That(imageData.Structure.Kind, Is.EqualTo(ImageDataKind.Rle));
        Assert.That(imageData.Structure.RowLengthFieldSize, Is.EqualTo(sizeof(ushort)));
        Assert.That(imageData.Structure.RowByteCounts, Has.Length.EqualTo(3));
        Assert.That(imageData.Structure.RowByteCounts[0], Is.EqualTo(2U));
        Assert.That(imageData.Structure.RowByteCounts[1], Is.EqualTo(0xABCDU));
        Assert.That(imageData.Structure.RowByteCounts[2], Is.EqualTo(1U));
        Assert.That(imageData.Structure.CompressedPayloadLength, Is.EqualTo(1));
    }


    /// <summary>
    /// Tests that a minimal synthetic PSB with RLE image data exposes the expected structural metadata.
    /// </summary>
    [Test]
    public void Load_PsbRle_ReadsStructure()
    {
        using var stream = new MemoryStream(BuildImageDataSection(
            CompressionMethod.RLE,
            [
                0x00, 0x00, 0x00, 0x02,
                0x00, 0x00, 0x00, 0x01,
                0x00, 0x00, 0x00, 0x03,
                0xAB, 0xCD, 0xEF, 0x10, 0x11, 0x12
            ]));
        using var reader = new BigEndianReader(stream, leaveOpen: true);

        ImageData imageData = ImageData.Load(reader, isLargeDocument: true, height: 1, channelCount: 3);

        Assert.That(imageData.Compression, Is.EqualTo(CompressionMethod.RLE));
        Assert.That(imageData.Structure.Kind, Is.EqualTo(ImageDataKind.Rle));
        Assert.That(imageData.Structure.RowLengthFieldSize, Is.EqualTo(sizeof(uint)));
        Assert.That(imageData.Structure.RowByteCounts, Has.Length.EqualTo(3));
        Assert.That(imageData.Structure.RowByteCounts[0], Is.EqualTo(2U));
        Assert.That(imageData.Structure.RowByteCounts[1], Is.EqualTo(1U));
        Assert.That(imageData.Structure.RowByteCounts[2], Is.EqualTo(3U));
        Assert.That(imageData.Structure.CompressedPayloadLength, Is.EqualTo(6));
    }


    /// <summary>
    /// Tests that a minimal synthetic PSD with ZIP image data exposes the expected structural metadata.
    /// </summary>
    [Test]
    public void Load_PsdZip_ReadsStructure()
    {
        using var stream = new MemoryStream(BuildImageDataSection(CompressionMethod.ZipWithoutPrediction, [0x78, 0x9C, 0x63, 0x60, 0x04, 0x00, 0x00, 0xFF, 0x00]));
        using var reader = new BigEndianReader(stream, leaveOpen: true);

        ImageData imageData = ImageData.Load(reader, isLargeDocument: false, height: 1, channelCount: 3);

        Assert.That(imageData.Compression, Is.EqualTo(CompressionMethod.ZipWithoutPrediction));
        Assert.That(imageData.Structure.Kind, Is.EqualTo(ImageDataKind.Zip));
        Assert.That(imageData.Structure.UsesPrediction, Is.False);
        Assert.That(imageData.Structure.CompressedPayloadLength, Is.EqualTo(9));
    }


    /// <summary>
    /// Tests that a truncated RLE row-length table is rejected.
    /// </summary>
    [Test]
    public void Load_TruncatedRleRows_Throws()
    {
        using var stream = new MemoryStream(BuildImageDataSection(CompressionMethod.RLE, [0x00, 0x02, 0xAB, 0xCD, 0x00]));
        using var reader = new BigEndianReader(stream, leaveOpen: true);

        Assert.That(() => ImageData.Load(reader, isLargeDocument: false, height: 1, channelCount: 3), Throws.InstanceOf<PsdLoadException>());
    }
}
