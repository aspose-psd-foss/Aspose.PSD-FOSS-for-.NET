# Aspose.PSD.FOSS for .NET

Aspose.PSD.FOSS is a free open-source .NET library for loading, inspecting, editing a small subset of PSD/PSB metadata, and saving the file back without rendering.

## Highlights

- Aspose.PSD-style API for common PSD metadata scenarios
- Load PSD/PSB from file paths and streams
- Read document properties: `Width`, `Height`, `Channels`, `BitsPerChannel`, `ColorMode`, `Version`
- Read additional document metadata: `IsLargeDocument`, `IsPsb`, `LayerCount`, resource and merged-image summaries
- Read layer metadata: `Name`, `Bounds`, `Width`, `Height`, `Top`, `Left`, `Bottom`, `Right`, `IsVisible`, `Opacity`, `Clipping`, `BlendMode`, `BlendModeKey`
- Inspect parsed resources, color mode data, image data structure, layer channels, mask presence, and blending-range presence
- Change `Name`, `IsVisible`, `Opacity`, `BlendMode`, `Clipping`, and layer geometry
- Save without rendering
- Preserve unsupported sections as raw bytes where possible

## Requirements

- .NET 10.0 SDK or later

## Installation

The package metadata is defined in this repository, but `Aspose.PSD.FOSS` is not published to NuGet yet.

Build the package locally:

```bash
dotnet pack src/Aspose.PSD.FOSS/Aspose.PSD.FOSS.csproj -c Release
```

The package will be created under `src/Aspose.PSD.FOSS/bin/Release/`.

To install it through a local NuGet source:

```bash
dotnet nuget add source src/Aspose.PSD.FOSS/bin/Release --name AsposePsdFossLocal
dotnet add package Aspose.PSD.FOSS --source AsposePsdFossLocal
```

## Quick Start

```csharp
using Aspose.PSD.FOSS;

using PsdImage image = PsdImage.Load("input.psd");

Console.WriteLine(image.Width);
Console.WriteLine(image.Height);
Console.WriteLine(image.BitsPerChannel);
Console.WriteLine(image.ColorMode);
Console.WriteLine(image.IsPsb);
Console.WriteLine(image.Compression);
Console.WriteLine(image.ResourceCount);

foreach (Layer layer in image.Layers)
{
    Console.WriteLine(layer.Name);
    Console.WriteLine(layer.Bounds);
    Console.WriteLine(layer.BlendModeKey);
    Console.WriteLine(layer.ChannelCount);
    Console.WriteLine(layer.IsVisible);
    Console.WriteLine(layer.Opacity);
    Console.WriteLine(layer.BlendMode);
}

image.Layers[0].Name = "Updated layer";
image.Layers[0].IsVisible = false;
image.Layers[0].Opacity = 128;
image.Layers[0].BlendMode = BlendMode.Multiply;
image.Save("output.psd");
```

## Samples

Runnable sample projects are available in the repository `samples/` folder:

- `Aspose.PSD.FOSS.Samples.Basic`
- `Aspose.PSD.FOSS.Samples.Layers`
- `Aspose.PSD.FOSS.Samples.StructuralEditing`
- `Aspose.PSD.FOSS.Samples.Streams`

Example commands:

```bash
dotnet run --project samples/Aspose.PSD.FOSS.Samples.Basic
dotnet run --project samples/Aspose.PSD.FOSS.Samples.Layers
dotnet run --project samples/Aspose.PSD.FOSS.Samples.StructuralEditing
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
- Read structural metadata from Color Mode Data, Image Resources, Layer and Mask Information, and merged image data
- Change `Name`, `IsVisible`, `Opacity`, `BlendMode`, `Clipping`, and layer geometry
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
