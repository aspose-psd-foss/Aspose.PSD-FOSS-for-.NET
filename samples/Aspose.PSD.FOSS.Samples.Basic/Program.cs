// Description:
// This sample loads a PSD/PSB document and prints document-level metadata
// using the supported Aspose.PSD-compatible public API subset.

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
        PrintDocumentState(image);
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
        Console.WriteLine($"  ChannelsCount: {image.ChannelsCount}");
        Console.WriteLine($"  BitsPerChannel: {image.BitsPerChannel}");
        Console.WriteLine($"  ColorMode: {image.ColorMode}");
        Console.WriteLine($"  Version: {image.Version}");
        Console.WriteLine($"  IsLargeDocument: {image.IsLargeDocument}");
        Console.WriteLine($"  IsPsb: {image.IsPsb}");
        Console.WriteLine($"  LayerCount: {image.LayerCount}");
        Console.WriteLine($"  HasLayers: {image.HasLayers}");
        Console.WriteLine();
    }

    /// <summary>
    /// Prints supported document state flags.
    /// </summary>
    /// <param name="image">The loaded PSD or PSB image.</param>
    private static void PrintDocumentState(PsdImage image)
    {
        Console.WriteLine("Document State");
        Console.WriteLine($"  HasColorModeData: {image.HasColorModeData}");
        Console.WriteLine($"  HasImageResources: {image.HasImageResources}");
        Console.WriteLine($"  ResourceCount: {image.ResourceCount}");
        Console.WriteLine($"  HasMergedImageData: {image.HasMergedImageData}");
        Console.WriteLine($"  Compression: {image.Compression}");
        Console.WriteLine($"  UsesPrediction: {image.UsesPrediction}");
    }

    /// <summary>
    /// Prints usage instructions for the sample.
    /// </summary>
    private static void PrintUsage()
    {
        Console.WriteLine("Usage: dotnet run --project samples/Aspose.PSD.FOSS.Samples.Basic -- [input.psd]");
        Console.WriteLine("Description: Prints document-level metadata exposed by the compatible public API subset.");
        Console.WriteLine("If no input path is provided, the sample tries to use the repository test fixture.");
    }
}
