// Description:
// This sample loads a PSD/PSB document and prints document-level metadata,
// unknown-only image resource summaries, color mode data details, and merged image data structure.

using Aspose.PSD;
using Aspose.PSD.FileFormats.Core.Blending;
using Aspose.PSD.FileFormats.Psd;
using Aspose.PSD.FileFormats.Psd.Layers;
using Aspose.PSD.FOSS.Samples.Common;

namespace Aspose.PSD.FOSS.Samples.Basic;

/// <summary>
/// Hosts the document inspection sample entry point.
/// </summary>
internal static class Program
{
    /// <summary>
    /// Runs the document inspection sample.
    /// </summary>
    /// <param name="args">Optional command-line arguments: [input.psd].</param>
    private static void Main(string[] args)
    {
        string? inputPath = SamplePathHelper.ResolveInputPath(args);
        if (inputPath == null)
        {
            PrintUsage();
            return;
        }

        RunDocumentInspection(inputPath);
    }

    /// <summary>
    /// Loads a PSD or PSB file and prints document-level inspection metadata.
    /// </summary>
    /// <param name="inputPath">The input PSD or PSB file path.</param>
    private static void RunDocumentInspection(string inputPath)
    {
        using var image = (PsdImage)Image.Load(inputPath);

        PrintSampleDescription(inputPath);
        PrintHeaderInfo(image);
        PrintColorModeData(image);
        PrintImageResources(image);
        PrintMergedImageData(image);
    }

    /// <summary>
    /// Writes a short description of the sample and its current input.
    /// </summary>
    /// <param name="inputPath">The input PSD or PSB file path.</param>
    private static void PrintSampleDescription(string inputPath)
    {
        Console.WriteLine("Aspose.PSD.FOSS Basic Sample");
        Console.WriteLine("Description: Loads a PSD/PSB file and prints document-level metadata without rendering pixel data.");
        Console.WriteLine($"Input: {inputPath}");
        Console.WriteLine();
    }

    /// <summary>
    /// Prints fixed header information and basic document flags.
    /// </summary>
    /// <param name="image">The loaded PSD or PSB image.</param>
    private static void PrintHeaderInfo(PsdImage image)
    {
        Console.WriteLine("Header");
        Console.WriteLine($"  Width: {image.Width}");
        Console.WriteLine($"  Height: {image.Height}");
        Console.WriteLine($"  Channels: {image.Channels}");
        Console.WriteLine($"  BitsPerChannel: {image.BitsPerChannel}");
        Console.WriteLine($"  ColorMode: {image.ColorMode}");
        Console.WriteLine($"  Version: {image.Version}");
        Console.WriteLine($"  IsLargeDocument: {image.IsLargeDocument}");
        Console.WriteLine($"  IsPsb: {image.IsPsb}");
        Console.WriteLine($"  Header.Width: {image.Header.Width}");
        Console.WriteLine($"  Header.Height: {image.Header.Height}");
        Console.WriteLine($"  Header.Channels: {image.Header.Channels}");
        Console.WriteLine($"  Header.BitDepth: {image.Header.BitDepth}");
        Console.WriteLine($"  Header.ColorMode: {image.Header.ColorMode}");
        Console.WriteLine($"  Header.Version: {image.Header.Version}");
        Console.WriteLine($"  LayerCount: {image.LayerCount}");
        Console.WriteLine($"  HasLayers: {image.HasLayers}");
        Console.WriteLine();
    }

    /// <summary>
    /// Prints the interpreted Color Mode Data section.
    /// </summary>
    /// <param name="image">The loaded PSD or PSB image.</param>
    private static void PrintColorModeData(PsdImage image)
    {
        Console.WriteLine("Color Mode Data");
        Console.WriteLine($"  HasColorModeData: {image.HasColorModeData}");
        Console.WriteLine($"  Kind: {image.ColorDataInfo.Kind}");
        Console.WriteLine($"  RawDataLength: {image.ColorDataInfo.RawDataLength}");
        Console.WriteLine($"  IndexedPaletteEntries: {GetIndexedPaletteEntryCount(image)}");
        Console.WriteLine();
    }

    /// <summary>
    /// Prints parsed image resource summaries in the current unknown-only mode.
    /// </summary>
    /// <param name="image">The loaded PSD or PSB image.</param>
    private static void PrintImageResources(PsdImage image)
    {
        Console.WriteLine("Image Resources");
        Console.WriteLine($"  HasImageResources: {image.HasImageResources}");
        Console.WriteLine($"  ResourceCount: {image.ResourceCount}");
        Console.WriteLine("  Mode: Unknown-only resource summaries without ID-specific semantic reconstruction");

        for (int i = 0; i < image.Resources.Count; i++)
        {
            PsdResourceInfo resource = image.Resources[i];
            Console.WriteLine($"  [{i}] ResourceId={resource.ResourceId}; Name={resource.Name}; Kind={resource.Kind}; DataLength={resource.DataLength}");
        }

        Console.WriteLine();
    }

    /// <summary>
    /// Prints merged image data structure details.
    /// </summary>
    /// <param name="image">The loaded PSD or PSB image.</param>
    private static void PrintMergedImageData(PsdImage image)
    {
        Console.WriteLine("Merged Image Data");
        Console.WriteLine($"  HasMergedImageData: {image.HasMergedImageData}");
        Console.WriteLine($"  Compression: {image.Compression}");
        Console.WriteLine($"  ImageDataKind: {image.ImageDataKind}");
        Console.WriteLine($"  UsesPrediction: {image.UsesPrediction}");
        Console.WriteLine($"  RowLengthFieldSize: {image.ImageDataInfo.RowLengthFieldSize}");
        Console.WriteLine($"  RleRowCount: {image.ImageDataInfo.RowByteCounts.Count}");
        Console.WriteLine($"  CompressedPayloadLength: {image.ImageDataInfo.CompressedPayloadLength}");
    }

    /// <summary>
    /// Gets the indexed palette entry count when an indexed palette is present.
    /// </summary>
    /// <param name="image">The loaded PSD or PSB image.</param>
    /// <returns>The entry count, or zero when no indexed palette was parsed.</returns>
    private static int GetIndexedPaletteEntryCount(PsdImage image)
    {
        return image.IndexedPalette?.Entries.Count ?? 0;
    }

    /// <summary>
    /// Prints usage instructions for the sample.
    /// </summary>
    private static void PrintUsage()
    {
        Console.WriteLine("Usage: dotnet run --project samples/Aspose.PSD.FOSS.Samples.Basic -- [input.psd]");
        Console.WriteLine("Description: Prints document-level metadata, resource summaries, color data details, and merged image data structure.");
        Console.WriteLine("If no input path is provided, the sample tries to use the repository test fixture.");
    }
}
