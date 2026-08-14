# Aspose.PSD.FOSS for .NET

Aspose.PSD.FOSS is a free open-source .NET library for loading, inspecting, editing a small subset of PSD/PSB metadata, and saving the file back without rendering.

## Highlights

- Aspose.PSD-style API for common PSD metadata scenarios
- Load PSD/PSB from file paths and streams
- Read document properties: `Width`, `Height`, `Channels`, `BitsPerChannel`, `ColorMode`, `Version`
- Read layer metadata: `Name`, `Bounds`, `IsVisible`, `Opacity`, `BlendMode`
- Change `Name`, `IsVisible`, and `Opacity`
- Save without rendering
- Preserve unsupported sections as raw bytes where possible

## Requirements

- .NET 10.0 SDK or later

## Installation

```bash
dotnet add package Aspose.PSD.FOSS
```

## Quick Start

```csharp
using Aspose.PSD.FOSS;

using PsdImage image = PsdImage.Load("input.psd");

Console.WriteLine(image.Width);
Console.WriteLine(image.Height);
Console.WriteLine(image.BitsPerChannel);
Console.WriteLine(image.ColorMode);

foreach (Layer layer in image.Layers)
{
    Console.WriteLine(layer.Name);
    Console.WriteLine(layer.Bounds);
    Console.WriteLine(layer.IsVisible);
    Console.WriteLine(layer.Opacity);
    Console.WriteLine(layer.BlendMode);
}

image.Layers[0].Name = "Updated layer";
image.Layers[0].IsVisible = false;
image.Layers[0].Opacity = 128;
image.Save("output.psd");
```

## Samples

Runnable sample projects are available in the repository `samples/` folder:

- `Aspose.PSD.FOSS.Samples.Basic`
- `Aspose.PSD.FOSS.Samples.Layers`
- `Aspose.PSD.FOSS.Samples.Streams`

Example commands:

```bash
dotnet run --project samples/Aspose.PSD.FOSS.Samples.Basic
dotnet run --project samples/Aspose.PSD.FOSS.Samples.Layers
dotnet run --project samples/Aspose.PSD.FOSS.Samples.Streams
```

If you do not pass an input file, the samples try to use the repository PSD test fixture.

## Documentation

Markdown documentation is available in the repository:

- `documentation/README.md`
- `documentation/getting-started.md`
- `documentation/developer-guide/basic-psd-operations.md`
- `documentation/limitations.md`

## Supported Scope

- Load PSD files
- Load the currently supported PSB subset
- Read document properties from the file header
- Read layer metadata from Layer and Mask Information
- Change `Name`, `IsVisible`, and `Opacity`
- Save PSD/PSB without rendering

## Out of Scope

- Rendering
- Pixel editing
- Export to PNG, JPEG, or other raster formats
- Full image resource editing
- Full tagged block editing
- Text, vector, effects, smart filters, and adjustment rendering

## License

MIT
