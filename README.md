# Aspose.PSD.FOSS

Free Open Source .NET library for basic PSD file operations (read/write without rendering).

## Features

- Load PSD files from file path or stream
- Read document properties (Width, Height, BitsPerChannel, ColorMode, Version)
- Read and edit layer properties (Name, IsVisible, Opacity, BlendMode)
- Save PSD files to file path or stream
- Support for PSD v1-6 and PSB (big documents)
- Preserve unknown sections as raw bytes

## Requirements

- .NET 10.0 or later

## Installation

### NuGet

```bash
dotnet add package Aspose.PSD.FOSS
```

## Usage

### Loading a PSD file

```csharp
using Aspose.PSD.FOSS;

var image = PsdImage.Load("input.psd");
Console.WriteLine($"Dimensions: {image.Width}x{image.Height}");
Console.WriteLine($"Color mode: {image.ColorMode}");
```

### Reading layer properties

```csharp
foreach (var layer in image.Layers)
{
    Console.WriteLine($"Layer: {layer.Name}");
    Console.WriteLine($"Visible: {layer.IsVisible}");
    Console.WriteLine($"Opacity: {layer.Opacity}");
    Console.WriteLine($"Blend mode: {layer.BlendMode}");
}
```

### Editing layer properties

```csharp
image.Layers[0].Name = "New Layer Name";
image.Layers[0].IsVisible = false;
image.Layers[0].Opacity = 128;

image.Save("output.psd");
```

### Loading from stream

```csharp
using (var stream = File.OpenRead("input.psd"))
{
    var image = PsdImage.Load(stream);
    // Work with image
}
```

## Supported Color Modes

- RGB
- CMYK
- Grayscale
- Indexed
- Duotone
- Lab

## Supported Compression Methods

- Raw
- RLE
- ZIP
- RZ

## Limitations

- No rendering or pixel manipulation
- No support for layer effects (blending options, layer styles)
- No support for vector paths
- No support for text layers (basic reading only)
- No support for adjustment layers

## API Reference

See the XML documentation comments in the source code for detailed API reference.

## License

MIT

## Contributing

Contributions are welcome! Please open an issue or submit a pull request.
