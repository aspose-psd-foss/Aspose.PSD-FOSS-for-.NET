using System.IO;
using NUnit.Framework;

namespace Aspose.PSD.FOSS.Tests;

/// <summary>
/// Contains PascalString tests.
/// </summary>
public sealed class PascalStringTests : PsdTestFixtureBase
{

    /// <summary>
    /// Tests that 2-byte-aligned PSD Pascal strings consume padding even when the payload is empty.
    /// </summary>
    [Test]
    public void ReadPascalStringAlignedTo2_EmptyString_ConsumesPadding()
    {
        using var stream = new MemoryStream([0x00, 0x00, 0x7F]);
        using var reader = new BigEndianReader(stream, leaveOpen: true);

        string value = reader.ReadPascalStringAlignedTo2();

        Assert.That(value, Is.Empty);
        Assert.That(reader.Position, Is.EqualTo(2));
        Assert.That(reader.ReadByte(), Is.EqualTo(0x7F));
    }


    /// <summary>
    /// Tests that 4-byte-aligned PSD Pascal strings consume padding even when the payload is empty.
    /// </summary>
    [Test]
    public void ReadPascalStringAlignedTo4_EmptyString_ConsumesPadding()
    {
        using var stream = new MemoryStream([0x00, 0x00, 0x00, 0x00, 0x7F]);
        using var reader = new BigEndianReader(stream, leaveOpen: true);

        string value = reader.ReadPascalStringAlignedTo4();

        Assert.That(value, Is.Empty);
        Assert.That(reader.Position, Is.EqualTo(4));
        Assert.That(reader.ReadByte(), Is.EqualTo(0x7F));
    }
}
