// Description:
// This sample loads a PSD/PSB document and prints supported layer metadata,
// channel summaries, and raw mask/blending-range subsection presence flags.

using Aspose.PSD;
using Aspose.PSD.FileFormats.Core.Blending;
using Aspose.PSD.FileFormats.Psd;
using Aspose.PSD.FileFormats.Psd.Layers;
using Aspose.PSD.FOSS.Samples.Common;

namespace Aspose.PSD.FOSS.Samples.Layers;

/// <summary>
/// Hosts the layer inspection sample entry point.
/// </summary>
internal static class Program
{
    /// <summary>
    /// Runs the layer inspection sample.
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

        RunLayerInspection(inputPath);
    }

    /// <summary>
    /// Loads a PSD or PSB file and prints all supported layer-level metadata.
    /// </summary>
    /// <param name="inputPath">The input PSD or PSB file path.</param>
    private static void RunLayerInspection(string inputPath)
    {
        using var image = (PsdImage)Image.Load(inputPath);

        PrintSampleDescription(inputPath, image.LayerCount);

        for (int i = 0; i < image.Layers.Length; i++)
        {
            PrintLayer(image.Layers[i], i);
        }
    }

    /// <summary>
    /// Writes a short description of the sample and the current input document.
    /// </summary>
    /// <param name="inputPath">The input PSD or PSB file path.</param>
    /// <param name="layerCount">The parsed layer count.</param>
    private static void PrintSampleDescription(string inputPath, int layerCount)
    {
        Console.WriteLine("Aspose.PSD.FOSS Layer Sample");
        Console.WriteLine("Description: Loads a PSD/PSB file and prints supported layer metadata, channel summaries, and raw subsection presence flags.");
        Console.WriteLine($"Input: {inputPath}");
        Console.WriteLine($"LayerCount: {layerCount}");
        Console.WriteLine();
    }

    /// <summary>
    /// Prints one layer and its related inspection data.
    /// </summary>
    /// <param name="layer">The layer to print.</param>
    /// <param name="index">The zero-based layer index.</param>
    private static void PrintLayer(Layer layer, int index)
    {
        Console.WriteLine($"Layer [{index}]");
        Console.WriteLine($"  Name: {layer.Name}");
        Console.WriteLine($"  Bounds: {layer.Bounds}");
        Console.WriteLine($"  Width: {layer.Width}");
        Console.WriteLine($"  Height: {layer.Height}");
        Console.WriteLine($"  Top: {layer.Top}");
        Console.WriteLine($"  Left: {layer.Left}");
        Console.WriteLine($"  Bottom: {layer.Bottom}");
        Console.WriteLine($"  Right: {layer.Right}");
        Console.WriteLine($"  IsVisible: {layer.IsVisible}");
        Console.WriteLine($"  Opacity: {layer.Opacity}");
        Console.WriteLine($"  Clipping: {layer.Clipping}");
        Console.WriteLine($"  BlendMode: {layer.BlendMode}");
        Console.WriteLine($"  BlendModeKey: {layer.BlendModeKey}");
        Console.WriteLine($"  ChannelCount: {layer.ChannelCount}");
        Console.WriteLine($"  HasMaskData: {layer.HasMaskData}");
        Console.WriteLine($"  HasBlendingRangesData: {layer.HasBlendingRangesData}");
        Console.WriteLine($"  HasAdditionalLayerData: {layer.HasAdditionalLayerData}");
        Console.WriteLine($"  MaskInfo.IsPresent: {layer.MaskInfo.IsPresent}");
        Console.WriteLine($"  MaskInfo.RawDataLength: {layer.MaskInfo.RawDataLength}");
        Console.WriteLine($"  BlendingRangesInfo.IsPresent: {layer.BlendingRangesInfo.IsPresent}");
        Console.WriteLine($"  BlendingRangesInfo.RawDataLength: {layer.BlendingRangesInfo.RawDataLength}");

        for (int i = 0; i < layer.Channels.Count; i++)
        {
            PsdLayerChannelInfo channel = layer.Channels[i];
            Console.WriteLine($"  Channel [{i}] Id={channel.ChannelId}; DataLength={channel.DataLength}");
        }

        Console.WriteLine();
    }

    /// <summary>
    /// Prints usage instructions for the sample.
    /// </summary>
    private static void PrintUsage()
    {
        Console.WriteLine("Usage: dotnet run --project samples/Aspose.PSD.FOSS.Samples.Layers -- [input.psd]");
        Console.WriteLine("Description: Prints supported layer metadata, channel records, and raw mask/blending-range section summaries.");
        Console.WriteLine("If no input path is provided, the sample tries to use the repository test fixture.");
    }
}
