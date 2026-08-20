using System.IO;
using NUnit.Framework;

namespace Aspose.PSD.FOSS.Tests;

/// <summary>
/// Contains ImageResourcesSection tests.
/// </summary>
public sealed class ImageResourcesSectionTests : PsdTestFixtureBase
{

    /// <summary>
    /// Tests that image resources are loaded as unknown blocks without ID-specific semantics.
    /// </summary>
    [Test]
    public void Load_ImageResources_ParsesUnknownBlocksWithoutSemantics()
    {
        byte[] resourcesPayload = BuildResourcesPayload(
            (ImageResourceIds.GlobalAngle, "glba", [0x00, 0x00, 0x00, 0x2D]),
            (ImageResourceIds.IccProfile, "icc", [0x49, 0x43, 0x43, 0x50]),
            (ImageResourceIds.IccUntaggedProfile, string.Empty, [0x01]));
        using var stream = new MemoryStream(resourcesPayload);
        using var reader = new BigEndianReader(stream, leaveOpen: true);

        var resources = new List<UnknownResource>();
        while (reader.Position < stream.Length)
        {
            UnknownResource? resource = UnknownResource.Load(reader, stream.Length);
            Assert.That(resource, Is.Not.Null);
            resources.Add(resource!);
        }

        Assert.That(resources, Has.Count.EqualTo(3));
        Assert.That(resources[0].ResourceId, Is.EqualTo(ImageResourceIds.GlobalAngle));
        Assert.That(resources[0].Data, Is.EqualTo(new byte[] { 0x00, 0x00, 0x00, 0x2D }));
        Assert.That(resources[1].ResourceId, Is.EqualTo(ImageResourceIds.IccProfile));
        Assert.That(resources[1].Data, Is.EqualTo(new byte[] { 0x49, 0x43, 0x43, 0x50 }));
        Assert.That(resources[2].ResourceId, Is.EqualTo(ImageResourceIds.IccUntaggedProfile));
        Assert.That(resources[2].Data, Is.EqualTo(new byte[] { 0x01 }));
    }


    /// <summary>
    /// Tests that saving a document with image resources preserves the raw Image Resources section bytes.
    /// </summary>
    [Test]
    public void Save_DocumentWithImageResources_PreservesRawResourcesSection()
    {
        string testFile = Path.Combine(TestContext.CurrentContext.TestDirectory, "testdata", "test.psd");
        byte[] originalBytes = File.ReadAllBytes(testFile);
        string outputFile = GetPersistentArtifactPath("resources_roundtrip_test.psd");

        using var image = PsdImage.Load(testFile);
        image.Save(outputFile);
        LogArtifactDirectory(outputFile);

        byte[] savedBytes = File.ReadAllBytes(outputFile);
        byte[] originalResourcesSection = ExtractImageResourcesSection(originalBytes);
        byte[] savedResourcesSection = ExtractImageResourcesSection(savedBytes);

        Assert.That(savedResourcesSection, Is.EqualTo(originalResourcesSection));

        using var reloaded = PsdImage.Load(outputFile);
        Assert.That(reloaded.HasImageResources, Is.True);
        Assert.That(reloaded.ResourceCount, Is.EqualTo(26));
    }


    /// <summary>
    /// Tests that the resources fixture exposes unknown resource summaries and expected layer presence.
    /// </summary>
    [Test]
    public void Load_ResourcesFixturePsd_ReturnsExpectedResourceSummaries()
    {
        using var image = PsdImage.Load(GetTestDataPath("resources.psd"));

        Assert.That(image.ResourceCount, Is.EqualTo(28));
        Assert.That(image.Resources[0].Kind, Is.EqualTo(PsdResourceKind.Unknown));
        Assert.That(image.HasImageResources, Is.True);
        Assert.That(image.Layers, Has.Length.EqualTo(3));
    }


    /// <summary>
    /// Tests that saving the resources fixture preserves the raw Image Resources section bytes.
    /// </summary>
    [Test]
    public void Save_ResourcesFixturePsd_PreservesRawResourcesSection()
    {
        string testFile = GetTestDataPath("resources.psd");
        byte[] originalBytes = File.ReadAllBytes(testFile);
        string outputFile = GetPersistentArtifactPath("resources_fixture_roundtrip_test.psd");

        using var image = PsdImage.Load(testFile);
        image.Save(outputFile);
        LogArtifactDirectory(outputFile);

        byte[] savedBytes = File.ReadAllBytes(outputFile);
        Assert.That(ExtractImageResourcesSection(savedBytes), Is.EqualTo(ExtractImageResourcesSection(originalBytes)));
    }
}
