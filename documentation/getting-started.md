# Getting Started

Aspose.PSD.FOSS is a lightweight open-source .NET library for loading, inspecting, editing a small subset of layer metadata, and saving PSD/PSB files without rendering.

## Requirements

- .NET 10.0 SDK or later

## Install from NuGet

```bash
dotnet add package Aspose.PSD.FOSS
```

## First Example

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
```

## Save After a Simple Change

```csharp
using Aspose.PSD.FOSS;

using PsdImage image = PsdImage.Load("input.psd");
image.Layers[0].Name = "Updated layer";
image.Layers[0].IsVisible = false;
image.Layers[0].Opacity = 128;
image.Save("output.psd");
```

## Next Steps

- See the top-level README for a quick overview.
- See the developer guide for the supported PSD subset.
- Run the sample projects in the `samples/` folder for end-to-end examples.
