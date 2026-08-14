using Aspose.PSD.FOSS;

string? inputPath = ResolveInputPath(args);
if (inputPath == null)
{
    PrintUsage();
    return;
}

using PsdImage image = PsdImage.Load(inputPath);

Console.WriteLine("Aspose.PSD.FOSS Basic Sample");
Console.WriteLine($"Input: {inputPath}");
Console.WriteLine($"Width: {image.Width}");
Console.WriteLine($"Height: {image.Height}");
Console.WriteLine($"Channels: {image.Channels}");
Console.WriteLine($"BitsPerChannel: {image.BitsPerChannel}");
Console.WriteLine($"ColorMode: {image.ColorMode}");
Console.WriteLine($"Version: {image.Version}");
Console.WriteLine($"Layers: {image.Layers.Length}");

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
    Console.WriteLine("Usage: dotnet run --project samples/Aspose.PSD.FOSS.Samples.Basic -- [input.psd]");
    Console.WriteLine("If no input path is provided, the sample tries to use the repository test fixture.");
}
