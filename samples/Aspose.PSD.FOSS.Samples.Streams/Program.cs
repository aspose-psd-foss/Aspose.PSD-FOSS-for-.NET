using Aspose.PSD.FOSS;

string? inputPath = ResolveInputPath(args);
if (inputPath == null)
{
    PrintUsage();
    return;
}

string outputPath = args.Length > 1
    ? Path.GetFullPath(args[1])
    : Path.Combine(Environment.CurrentDirectory, "streams-sample-output.psd");

using FileStream inputStream = File.OpenRead(inputPath);
using PsdImage image = PsdImage.Load(inputStream);

Console.WriteLine("Aspose.PSD.FOSS Stream Sample");
Console.WriteLine($"Loaded from stream: {inputPath}");
Console.WriteLine($"Original stream position after load: {inputStream.Position}");

using FileStream outputStream = File.Create(outputPath);
image.Save(outputStream);

Console.WriteLine($"Saved through stream: {outputPath}");

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
    Console.WriteLine("Usage: dotnet run --project samples/Aspose.PSD.FOSS.Samples.Streams -- [input.psd] [output.psd]");
    Console.WriteLine("If no input path is provided, the sample tries to use the repository test fixture.");
}
