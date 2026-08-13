using System;
using System.IO;

namespace Aspose.PSD.FOSS.Tests;

class Program
{
    static int Main(string[] args)
    {
        try
        {
            string testFile = args.Length > 0 ? args[0] : "test.psd";
            
            if (!File.Exists(testFile))
            {
                Console.WriteLine($"Test file not found: {testFile}");
                return 1;
            }

            Console.WriteLine($"Loading {testFile}...");
            using var image = PsdImage.Load(testFile);

            Console.WriteLine($"Loaded successfully!");
            Console.WriteLine($"  Width: {image.Width}");
            Console.WriteLine($"  Height: {image.Height}");
            Console.WriteLine($"  Channels: {image.Channels}");
            Console.WriteLine($"  BitsPerChannel: {image.BitsPerChannel}");
            Console.WriteLine($"  ColorMode: {image.ColorMode}");
            Console.WriteLine($"  Version: {image.Version}");
            Console.WriteLine($"  HasLayers: {image.HasLayers}");
            Console.WriteLine($"  Layer count: {image.Layers.Length}");

            if (image.Layers.Length > 0)
            {
                Console.WriteLine("\nLayers:");
                for (int i = 0; i < Math.Min(5, image.Layers.Length); i++)
                {
                    var layer = image.Layers[i];
                    Console.WriteLine($"  Layer {i}: {layer.Name}");
                    Console.WriteLine($"    Bounds: {layer.Bounds}");
                    Console.WriteLine($"    Visible: {layer.IsVisible}");
                    Console.WriteLine($"    Opacity: {layer.Opacity}");
                    Console.WriteLine($"    BlendMode: {layer.BlendMode}");
                }
            }

            Console.WriteLine("\nSaving to output.psd...");
            image.Save("output.psd");

            var originalSize = new FileInfo(testFile).Length;
            var outputSize = new FileInfo("output.psd").Length;

            Console.WriteLine($"Original size: {originalSize} bytes");
            Console.WriteLine($"Output size: {outputSize} bytes");

            if (originalSize == outputSize)
            {
                Console.WriteLine("✓ File sizes match (structurally identical)");
            }
            else
            {
                Console.WriteLine($"⚠ File sizes differ ({outputSize - originalSize} bytes)");
            }

            Console.WriteLine("\nLoading saved file...");
            using var loadedAgain = PsdImage.Load("output.psd");

            bool matches = image.Width == loadedAgain.Width &&
                          image.Height == loadedAgain.Height &&
                          image.Channels == loadedAgain.Channels &&
                          image.BitsPerChannel == loadedAgain.BitsPerChannel &&
                          image.ColorMode == loadedAgain.ColorMode &&
                          image.Version == loadedAgain.Version;

            if (image.Layers.Length == loadedAgain.Layers.Length)
            {
                for (int i = 0; i < image.Layers.Length; i++)
                {
                    if (image.Layers[i].Name != loadedAgain.Layers[i].Name ||
                        image.Layers[i].Bounds != loadedAgain.Layers[i].Bounds ||
                        image.Layers[i].IsVisible != loadedAgain.Layers[i].IsVisible ||
                        image.Layers[i].Opacity != loadedAgain.Layers[i].Opacity ||
                        image.Layers[i].BlendMode != loadedAgain.Layers[i].BlendMode)
                    {
                        matches = false;
                        break;
                    }
                }
            }
            else
            {
                matches = false;
            }

            if (matches)
            {
                Console.WriteLine("✓ Round-trip successful! All properties match.");
                return 0;
            }
            else
            {
                Console.WriteLine("✗ Round-trip failed! Properties don't match.");
                return 1;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
            return 1;
        }
    }
}
