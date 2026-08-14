using Aspose.PSD.FOSS;

string? inputPath = ResolveInputPath(args);
if (inputPath == null)
{
    PrintUsage();
    return;
}

string outputPath = args.Length > 1
    ? Path.GetFullPath(args[1])
    : Path.Combine(Environment.CurrentDirectory, "layers-sample-output.psd");

using PsdImage image = PsdImage.Load(inputPath);

Console.WriteLine("Aspose.PSD.FOSS Layer Sample");
Console.WriteLine($"Input: {inputPath}");

for (int i = 0; i < image.Layers.Length; i++)
{
    Layer layer = image.Layers[i];
    Console.WriteLine($"[{i}] Name={layer.Name}; Bounds={layer.Bounds}; Visible={layer.IsVisible}; Opacity={layer.Opacity}; BlendMode={layer.BlendMode}");
}

if (image.Layers.Length > 0)
{
    image.Layers[0].Name = $"{image.Layers[0].Name} (updated)";
    image.Layers[0].IsVisible = false;
    image.Layers[0].Opacity = 128;
    image.Save(outputPath);

    Console.WriteLine($"Updated first layer and saved: {outputPath}");
}
else
{
    Console.WriteLine("The document has no layers, so no mutations were applied.");
}

static string? ResolveInputPath(string[] args)
{
    if (args.Length > 0 && File.Exists(args[0]))
    {
        return Path.GetFullPath(args[0]);
    }

    string repositoryFixture = Path.GetFullPath(
        Path.Combine(AppContext.BaseDirectory, "../../../../../src/Aspose.PSD.FOSS.Test/testdata/test.psd"));

    return File.Exists(repositoryFixture) ? repositoryFixture : null;
}

static void PrintUsage()
{
    Console.WriteLine("Usage: dotnet run --project samples/Aspose.PSD.FOSS.Samples.Layers -- [input.psd] [output.psd]");
    Console.WriteLine("If no input path is provided, the sample tries to use the repository test fixture.");
}
