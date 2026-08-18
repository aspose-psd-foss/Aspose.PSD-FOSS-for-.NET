# Getting Started

Aspose.PSD.FOSS is a lightweight open-source .NET library for loading, inspecting, editing a small subset of layer metadata, and saving PSD/PSB files without rendering.

## What the Library Is For

Use Aspose.PSD.FOSS when you need to inspect PSD/PSB structure, read supported metadata, make small structural edits, and save the file back without rendering.

This library is intentionally narrow in scope. It does not render Photoshop documents, edit pixels, or expose the full commercial Aspose.PSD feature set.

## Requirements

- .NET 10.0 SDK or later

## Build the Package Locally

`Aspose.PSD.FOSS` is not published to NuGet yet. Build the package from this repository:

```bash
dotnet pack src/Aspose.PSD.FOSS/Aspose.PSD.FOSS.csproj -c Release
```

The generated `.nupkg` file will be placed under `src/Aspose.PSD.FOSS/bin/Release/`.

If you want to install it through a local NuGet source:

```bash
dotnet nuget add source src/Aspose.PSD.FOSS/bin/Release --name AsposePsdFossLocal
dotnet add package Aspose.PSD.FOSS --source AsposePsdFossLocal
```

## First 5 Minutes

The fastest way to understand the project is:

1. Load a PSD/PSB file.
2. Print document and layer metadata.
3. Change one supported layer property.
4. Save the result back without rendering.

## First Example

```csharp
using Aspose.PSD.FOSS;

using PsdImage image = PsdImage.Load("input.psd");

Console.WriteLine(image.Width);
Console.WriteLine(image.Height);
Console.WriteLine(image.BitsPerChannel);
Console.WriteLine(image.ColorMode);
Console.WriteLine(image.IsPsb);
Console.WriteLine(image.Compression);
Console.WriteLine(image.LayerCount);

foreach (Layer layer in image.Layers)
{
    Console.WriteLine(layer.Name);
    Console.WriteLine(layer.Bounds);
    Console.WriteLine(layer.BlendModeKey);
    Console.WriteLine(layer.IsVisible);
    Console.WriteLine(layer.Opacity);
    Console.WriteLine(layer.BlendMode);
}
```

## Save After a Simple Change

```csharp
using Aspose.PSD.FOSS;

using PsdImage image = PsdImage.Load("input.psd");
image.Layers[0].Name = "Updated layer";
image.Layers[0].IsVisible = false;
image.Layers[0].Opacity = 128;
image.Layers[0].BlendMode = BlendMode.Multiply;
image.Layers[0].Clipping = 1;
image.Save("output.psd");
```

## What to Read Next

- Read the developer guide to see the full supported workflow surface.
- Read the limitations page before assuming support for broader Photoshop features.
- Run the sample projects for end-to-end examples of inspection, editing, and stream-based usage.

## Next Steps

- See the top-level README for a quick overview.
- See the developer guide for the supported PSD subset.
- Run the sample projects in the `samples/` folder for end-to-end examples.
