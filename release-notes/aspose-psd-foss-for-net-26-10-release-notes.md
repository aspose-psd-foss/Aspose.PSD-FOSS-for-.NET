---
id: "aspose-psd-foss-for-net-26-10-release-notes"
slug: "aspose-psd-foss-for-net-26-10-release-notes"
linktitle: "Aspose.PSD FOSS for .NET 26.10 - Release Notes"
title: "Aspose.PSD FOSS for .NET 26.10 - Release Notes"
description: "Aspose.PSD FOSS for .NET 26.10 - Release Notes – the latest updates and examples."
---

This page contains release notes for [Aspose.PSD FOSS for .NET 26.10](https://www.nuget.org/packages/Aspose.PSD.FOSS/)

# Initial release of Aspose.PSD FOSS for .NET

This is the initial release of Aspose.PSD FOSS for .NET.

This new product provides functionality for PSD/PSB file processing. It is a simple library that helps you get started working with PSD and PSB files quickly. The first version focuses primarily on metadata exploration.

Please note that Aspose also offers the enterprise-quality Aspose.PSD, which additionally supports the AI file format, the ability to modify layers, masks, groups, effects, smart objects, and much more, as well as rendering of changes. So if the functionality of the FOSS version is not sufficient for your needs, visit https://releases.aspose.com/psd/ (available for .NET, Java, and Python).


## **Public API Changes**

# **Added APIs:**  
T:Aspose.PSD.Image
M:Aspose.PSD.Image.#ctor
P:Aspose.PSD.Image.Width
P:Aspose.PSD.Image.Height
P:Aspose.PSD.Image.Bounds
M:Aspose.PSD.Image.Load(System.String)
M:Aspose.PSD.Image.Load(System.IO.Stream)
M:Aspose.PSD.Image.Save(System.String)
M:Aspose.PSD.Image.Save(System.IO.Stream)
M:Aspose.PSD.Image.Dispose
T:Aspose.PSD.ImageLoader
M:Aspose.PSD.ImageLoader.#ctor
M:Aspose.PSD.ImageLoader.Load(System.String)
M:Aspose.PSD.ImageLoader.Load(System.IO.Stream)
T:Aspose.PSD.Point
M:Aspose.PSD.Point.#ctor(System.Int32,System.Int32)
P:Aspose.PSD.Point.X
P:Aspose.PSD.Point.Y
P:Aspose.PSD.Point.IsEmpty
M:Aspose.PSD.Point.Equals(Aspose.PSD.Point)
M:Aspose.PSD.Point.Equals(System.Object)
M:Aspose.PSD.Point.GetHashCode
M:Aspose.PSD.Point.ToString
M:Aspose.PSD.Point.op_Equality(Aspose.PSD.Point,Aspose.PSD.Point)
M:Aspose.PSD.Point.op_Inequality(Aspose.PSD.Point,Aspose.PSD.Point)
F:Aspose.PSD.Point.Empty
T:Aspose.PSD.BuildVersionInfo
F:Aspose.PSD.BuildVersionInfo.AssemblyVersion
F:Aspose.PSD.BuildVersionInfo.FileVersion
F:Aspose.PSD.BuildVersionInfo.Product
F:Aspose.PSD.BuildVersionInfo.ProductMajor
F:Aspose.PSD.BuildVersionInfo.ProductMinor
F:Aspose.PSD.BuildVersionInfo.ReleaseDate
T:Aspose.PSD.Rectangle
M:Aspose.PSD.Rectangle.#ctor(Aspose.PSD.Point,Aspose.PSD.Size)
M:Aspose.PSD.Rectangle.#ctor(System.Int32,System.Int32,System.Int32,System.Int32)
P:Aspose.PSD.Rectangle.X
P:Aspose.PSD.Rectangle.Y
P:Aspose.PSD.Rectangle.Width
P:Aspose.PSD.Rectangle.Height
P:Aspose.PSD.Rectangle.Bottom
P:Aspose.PSD.Rectangle.IsEmpty
P:Aspose.PSD.Rectangle.Left
P:Aspose.PSD.Rectangle.Location
P:Aspose.PSD.Rectangle.Right
P:Aspose.PSD.Rectangle.Size
P:Aspose.PSD.Rectangle.Top
M:Aspose.PSD.Rectangle.Ceiling(Aspose.PSD.RectangleF)
M:Aspose.PSD.Rectangle.FromLeftTopRightBottom(System.Int32,System.Int32,System.Int32,System.Int32)
M:Aspose.PSD.Rectangle.FromPoints(Aspose.PSD.Point,Aspose.PSD.Point)
M:Aspose.PSD.Rectangle.Inflate(Aspose.PSD.Rectangle,System.Int32,System.Int32)
M:Aspose.PSD.Rectangle.Intersect(Aspose.PSD.Rectangle,Aspose.PSD.Rectangle)
M:Aspose.PSD.Rectangle.Round(Aspose.PSD.RectangleF)
M:Aspose.PSD.Rectangle.Truncate(Aspose.PSD.RectangleF)
M:Aspose.PSD.Rectangle.Union(Aspose.PSD.Rectangle,Aspose.PSD.Rectangle)
M:Aspose.PSD.Rectangle.Contains(Aspose.PSD.Point)
M:Aspose.PSD.Rectangle.Contains(Aspose.PSD.Rectangle)
M:Aspose.PSD.Rectangle.Contains(System.Int32,System.Int32)
M:Aspose.PSD.Rectangle.Equals(Aspose.PSD.Rectangle)
M:Aspose.PSD.Rectangle.Equals(System.Object)
M:Aspose.PSD.Rectangle.GetHashCode
M:Aspose.PSD.Rectangle.Inflate(Aspose.PSD.Size)
M:Aspose.PSD.Rectangle.Inflate(System.Int32,System.Int32)
M:Aspose.PSD.Rectangle.Intersect(Aspose.PSD.Rectangle)
M:Aspose.PSD.Rectangle.IntersectsWith(Aspose.PSD.Rectangle)
M:Aspose.PSD.Rectangle.Normalize
M:Aspose.PSD.Rectangle.Offset(Aspose.PSD.Point)
M:Aspose.PSD.Rectangle.Offset(System.Int32,System.Int32)
M:Aspose.PSD.Rectangle.ToString
M:Aspose.PSD.Rectangle.op_Equality(Aspose.PSD.Rectangle,Aspose.PSD.Rectangle)
M:Aspose.PSD.Rectangle.op_Inequality(Aspose.PSD.Rectangle,Aspose.PSD.Rectangle)
F:Aspose.PSD.Rectangle.Empty
T:Aspose.PSD.RectangleF
M:Aspose.PSD.RectangleF.#ctor(System.Single,System.Single,System.Single,System.Single)
P:Aspose.PSD.RectangleF.X
P:Aspose.PSD.RectangleF.Y
P:Aspose.PSD.RectangleF.Width
P:Aspose.PSD.RectangleF.Height
P:Aspose.PSD.RectangleF.Bottom
P:Aspose.PSD.RectangleF.IsEmpty
P:Aspose.PSD.RectangleF.Left
P:Aspose.PSD.RectangleF.Right
P:Aspose.PSD.RectangleF.Top
M:Aspose.PSD.RectangleF.Equals(Aspose.PSD.RectangleF)
M:Aspose.PSD.RectangleF.Equals(System.Object)
M:Aspose.PSD.RectangleF.GetHashCode
M:Aspose.PSD.RectangleF.ToString
M:Aspose.PSD.RectangleF.op_Equality(Aspose.PSD.RectangleF,Aspose.PSD.RectangleF)
M:Aspose.PSD.RectangleF.op_Inequality(Aspose.PSD.RectangleF,Aspose.PSD.RectangleF)
F:Aspose.PSD.RectangleF.Empty
T:Aspose.PSD.Size
M:Aspose.PSD.Size.#ctor(System.Int32,System.Int32)
P:Aspose.PSD.Size.Width
P:Aspose.PSD.Size.Height
P:Aspose.PSD.Size.IsEmpty
M:Aspose.PSD.Size.Equals(Aspose.PSD.Size)
M:Aspose.PSD.Size.Equals(System.Object)
M:Aspose.PSD.Size.GetHashCode
M:Aspose.PSD.Size.ToString
M:Aspose.PSD.Size.op_Equality(Aspose.PSD.Size,Aspose.PSD.Size)
M:Aspose.PSD.Size.op_Inequality(Aspose.PSD.Size,Aspose.PSD.Size)
F:Aspose.PSD.Size.Empty
T:Aspose.PSD.StreamContainer
M:Aspose.PSD.StreamContainer.#ctor(System.IO.Stream)
P:Aspose.PSD.StreamContainer.Stream
T:Aspose.PSD.FileFormats.Core.Blending.BlendMode
F:Aspose.PSD.FileFormats.Core.Blending.BlendMode.Absent
F:Aspose.PSD.FileFormats.Core.Blending.BlendMode.Color
F:Aspose.PSD.FileFormats.Core.Blending.BlendMode.Darken
F:Aspose.PSD.FileFormats.Core.Blending.BlendMode.Difference
F:Aspose.PSD.FileFormats.Core.Blending.BlendMode.Dissolve
F:Aspose.PSD.FileFormats.Core.Blending.BlendMode.ColorDodge
F:Aspose.PSD.FileFormats.Core.Blending.BlendMode.DarkerColor
F:Aspose.PSD.FileFormats.Core.Blending.BlendMode.Divide
F:Aspose.PSD.FileFormats.Core.Blending.BlendMode.Subtract
F:Aspose.PSD.FileFormats.Core.Blending.BlendMode.HardLight
F:Aspose.PSD.FileFormats.Core.Blending.BlendMode.HardMix
F:Aspose.PSD.FileFormats.Core.Blending.BlendMode.Hue
F:Aspose.PSD.FileFormats.Core.Blending.BlendMode.ColorBurn
F:Aspose.PSD.FileFormats.Core.Blending.BlendMode.LinearLight
F:Aspose.PSD.FileFormats.Core.Blending.BlendMode.LinearBurn
F:Aspose.PSD.FileFormats.Core.Blending.BlendMode.LinearDodge
F:Aspose.PSD.FileFormats.Core.Blending.BlendMode.LighterColor
F:Aspose.PSD.FileFormats.Core.Blending.BlendMode.Lighten
F:Aspose.PSD.FileFormats.Core.Blending.BlendMode.Luminosity
F:Aspose.PSD.FileFormats.Core.Blending.BlendMode.Multiply
F:Aspose.PSD.FileFormats.Core.Blending.BlendMode.Normal
F:Aspose.PSD.FileFormats.Core.Blending.BlendMode.Overlay
F:Aspose.PSD.FileFormats.Core.Blending.BlendMode.PinLight
F:Aspose.PSD.FileFormats.Core.Blending.BlendMode.PassThrough
F:Aspose.PSD.FileFormats.Core.Blending.BlendMode.SoftLight
F:Aspose.PSD.FileFormats.Core.Blending.BlendMode.Saturation
F:Aspose.PSD.FileFormats.Core.Blending.BlendMode.Screen
F:Aspose.PSD.FileFormats.Core.Blending.BlendMode.Exclusion
F:Aspose.PSD.FileFormats.Core.Blending.BlendMode.VividLight
T:Aspose.PSD.FileFormats.Psd.ColorModes
F:Aspose.PSD.FileFormats.Psd.ColorModes.Bitmap
F:Aspose.PSD.FileFormats.Psd.ColorModes.Grayscale
F:Aspose.PSD.FileFormats.Psd.ColorModes.Indexed
F:Aspose.PSD.FileFormats.Psd.ColorModes.Rgb
F:Aspose.PSD.FileFormats.Psd.ColorModes.Cmyk
F:Aspose.PSD.FileFormats.Psd.ColorModes.Multichannel
F:Aspose.PSD.FileFormats.Psd.ColorModes.Duotone
F:Aspose.PSD.FileFormats.Psd.ColorModes.Lab
T:Aspose.PSD.FileFormats.Psd.CompressionMethod
F:Aspose.PSD.FileFormats.Psd.CompressionMethod.Raw
F:Aspose.PSD.FileFormats.Psd.CompressionMethod.RLE
F:Aspose.PSD.FileFormats.Psd.CompressionMethod.ZipWithoutPrediction
F:Aspose.PSD.FileFormats.Psd.CompressionMethod.ZipWithPrediction
T:Aspose.PSD.FileFormats.Psd.PsdLoadException
M:Aspose.PSD.FileFormats.Psd.PsdLoadException.#ctor(System.String)
M:Aspose.PSD.FileFormats.Psd.PsdLoadException.#ctor(System.String,System.Exception)
T:Aspose.PSD.FileFormats.Psd.PsdSaveException
M:Aspose.PSD.FileFormats.Psd.PsdSaveException.#ctor(System.String)
M:Aspose.PSD.FileFormats.Psd.PsdSaveException.#ctor(System.String,System.Exception)
T:Aspose.PSD.FileFormats.Psd.PsdImage
P:Aspose.PSD.FileFormats.Psd.PsdImage.Width
P:Aspose.PSD.FileFormats.Psd.PsdImage.Height
P:Aspose.PSD.FileFormats.Psd.PsdImage.BitsPerChannel
P:Aspose.PSD.FileFormats.Psd.PsdImage.ColorMode
P:Aspose.PSD.FileFormats.Psd.PsdImage.Version
P:Aspose.PSD.FileFormats.Psd.PsdImage.Layers
P:Aspose.PSD.FileFormats.Psd.PsdImage.ChannelsCount
P:Aspose.PSD.FileFormats.Psd.PsdImage.Size
P:Aspose.PSD.FileFormats.Psd.PsdImage.ActiveLayer
P:Aspose.PSD.FileFormats.Psd.PsdImage.ImageResources
P:Aspose.PSD.FileFormats.Psd.PsdImage.GlobalLayerResources
P:Aspose.PSD.FileFormats.Psd.PsdImage.GlobalLayerMaskInfo
P:Aspose.PSD.FileFormats.Psd.PsdImage.IsFlatten
P:Aspose.PSD.FileFormats.Psd.PsdImage.HasTransparencyData
P:Aspose.PSD.FileFormats.Psd.PsdImage.Compression
P:Aspose.PSD.FileFormats.Psd.PsdImage.GlobalAngle
M:Aspose.PSD.FileFormats.Psd.PsdImage.Load(System.String)
M:Aspose.PSD.FileFormats.Psd.PsdImage.Load(System.IO.Stream)
M:Aspose.PSD.FileFormats.Psd.PsdImage.Save(System.String)
M:Aspose.PSD.FileFormats.Psd.PsdImage.Save(System.IO.Stream)
M:Aspose.PSD.FileFormats.Psd.PsdImage.Dispose
T:Aspose.PSD.FileFormats.Psd.PsdVersion
F:Aspose.PSD.FileFormats.Psd.PsdVersion.Psd
F:Aspose.PSD.FileFormats.Psd.PsdVersion.Psb
T:Aspose.PSD.FileFormats.Psd.ResourceBlock
M:Aspose.PSD.FileFormats.Psd.ResourceBlock.#ctor
P:Aspose.PSD.FileFormats.Psd.ResourceBlock.DataSize
P:Aspose.PSD.FileFormats.Psd.ResourceBlock.ID
P:Aspose.PSD.FileFormats.Psd.ResourceBlock.MinimalVersion
P:Aspose.PSD.FileFormats.Psd.ResourceBlock.Name
P:Aspose.PSD.FileFormats.Psd.ResourceBlock.Signature
P:Aspose.PSD.FileFormats.Psd.ResourceBlock.Size
M:Aspose.PSD.FileFormats.Psd.ResourceBlock.Save(Aspose.PSD.StreamContainer)
M:Aspose.PSD.FileFormats.Psd.ResourceBlock.ValidateValues
F:Aspose.PSD.FileFormats.Psd.ResourceBlock.ResouceBlockSignature
F:Aspose.PSD.FileFormats.Psd.ResourceBlock.ResouceBlockMeSaSignature
T:Aspose.PSD.FileFormats.Psd.Layers.BlendRange
M:Aspose.PSD.FileFormats.Psd.Layers.BlendRange.#ctor
P:Aspose.PSD.FileFormats.Psd.Layers.BlendRange.Destination
P:Aspose.PSD.FileFormats.Psd.Layers.BlendRange.Source
T:Aspose.PSD.FileFormats.Psd.Layers.ChannelInformation
M:Aspose.PSD.FileFormats.Psd.Layers.ChannelInformation.#ctor(Aspose.PSD.FileFormats.Psd.CompressionMethod,System.Int32,System.Int32)
P:Aspose.PSD.FileFormats.Psd.Layers.ChannelInformation.ChannelID
P:Aspose.PSD.FileFormats.Psd.Layers.ChannelInformation.CompressionMethod
P:Aspose.PSD.FileFormats.Psd.Layers.ChannelInformation.Length
T:Aspose.PSD.FileFormats.Psd.Layers.GlobalLayerMaskInfo
M:Aspose.PSD.FileFormats.Psd.Layers.GlobalLayerMaskInfo.#ctor
T:Aspose.PSD.FileFormats.Psd.Layers.Layer
M:Aspose.PSD.FileFormats.Psd.Layers.Layer.#ctor
P:Aspose.PSD.FileFormats.Psd.Layers.Layer.Name
P:Aspose.PSD.FileFormats.Psd.Layers.Layer.Bounds
P:Aspose.PSD.FileFormats.Psd.Layers.Layer.Width
P:Aspose.PSD.FileFormats.Psd.Layers.Layer.Height
P:Aspose.PSD.FileFormats.Psd.Layers.Layer.Top
P:Aspose.PSD.FileFormats.Psd.Layers.Layer.Left
P:Aspose.PSD.FileFormats.Psd.Layers.Layer.Bottom
P:Aspose.PSD.FileFormats.Psd.Layers.Layer.Right
P:Aspose.PSD.FileFormats.Psd.Layers.Layer.IsVisible
P:Aspose.PSD.FileFormats.Psd.Layers.Layer.Opacity
P:Aspose.PSD.FileFormats.Psd.Layers.Layer.Clipping
P:Aspose.PSD.FileFormats.Psd.Layers.Layer.BlendModeKey
P:Aspose.PSD.FileFormats.Psd.Layers.Layer.ChannelsCount
P:Aspose.PSD.FileFormats.Psd.Layers.Layer.ChannelInformation
P:Aspose.PSD.FileFormats.Psd.Layers.Layer.LayerMaskData
P:Aspose.PSD.FileFormats.Psd.Layers.Layer.LayerBlendingRangesData
F:Aspose.PSD.FileFormats.Psd.Layers.Layer.LayerTrailerSize
T:Aspose.PSD.FileFormats.Psd.Layers.LayerBlendingRangesData
M:Aspose.PSD.FileFormats.Psd.Layers.LayerBlendingRangesData.#ctor
P:Aspose.PSD.FileFormats.Psd.Layers.LayerBlendingRangesData.CompositeBlendRange
P:Aspose.PSD.FileFormats.Psd.Layers.LayerBlendingRangesData.ChannelBlendRanges
P:Aspose.PSD.FileFormats.Psd.Layers.LayerBlendingRangesData.Length
T:Aspose.PSD.FileFormats.Psd.Layers.LayerMaskData
M:Aspose.PSD.FileFormats.Psd.Layers.LayerMaskData.#ctor
P:Aspose.PSD.FileFormats.Psd.Layers.LayerMaskData.Bottom
P:Aspose.PSD.FileFormats.Psd.Layers.LayerMaskData.DataSize
P:Aspose.PSD.FileFormats.Psd.Layers.LayerMaskData.DefaultColor
P:Aspose.PSD.FileFormats.Psd.Layers.LayerMaskData.Flags
P:Aspose.PSD.FileFormats.Psd.Layers.LayerMaskData.ImageData
P:Aspose.PSD.FileFormats.Psd.Layers.LayerMaskData.Left
P:Aspose.PSD.FileFormats.Psd.Layers.LayerMaskData.MaskRectangle
P:Aspose.PSD.FileFormats.Psd.Layers.LayerMaskData.Right
P:Aspose.PSD.FileFormats.Psd.Layers.LayerMaskData.Top
T:Aspose.PSD.FileFormats.Psd.Layers.LayerMaskDataShort
M:Aspose.PSD.FileFormats.Psd.Layers.LayerMaskDataShort.#ctor
P:Aspose.PSD.FileFormats.Psd.Layers.LayerMaskDataShort.Padding
T:Aspose.PSD.FileFormats.Psd.Layers.LayerMaskFlags
F:Aspose.PSD.FileFormats.Psd.Layers.LayerMaskFlags.None
F:Aspose.PSD.FileFormats.Psd.Layers.LayerMaskFlags.RelativeToLayer
F:Aspose.PSD.FileFormats.Psd.Layers.LayerMaskFlags.Disabled
F:Aspose.PSD.FileFormats.Psd.Layers.LayerMaskFlags.InvertedWhenBlending
F:Aspose.PSD.FileFormats.Psd.Layers.LayerMaskFlags.UserMaskFromRenderingOtherData
F:Aspose.PSD.FileFormats.Psd.Layers.LayerMaskFlags.UserOrVectorMasksHaveParameters
T:Aspose.PSD.FileFormats.Psd.Layers.LayerResource
M:Aspose.PSD.FileFormats.Psd.Layers.LayerResource.#ctor
P:Aspose.PSD.FileFormats.Psd.Layers.LayerResource.Key
P:Aspose.PSD.FileFormats.Psd.Layers.LayerResource.Length
P:Aspose.PSD.FileFormats.Psd.Layers.LayerResource.PsdVersion
P:Aspose.PSD.FileFormats.Psd.Layers.LayerResource.Signature
M:Aspose.PSD.FileFormats.Psd.Layers.LayerResource.Save(Aspose.PSD.StreamContainer,System.Int32)
F:Aspose.PSD.FileFormats.Psd.Layers.LayerResource.ResourceSignature
F:Aspose.PSD.FileFormats.Psd.Layers.LayerResource.PsbResourceSignature
T:Aspose.PSD.FileFormats.Psd.ResourceBlock.ResourceBlockState
F:Aspose.PSD.FileFormats.Psd.ResourceBlock.ResourceBlockState.Ready
F:Aspose.PSD.FileFormats.Psd.ResourceBlock.ResourceBlockState.Disposed


## **Usage examples:**

** Basic document inspection sample – prints document level metadata **

{{< highlight csharp >}}
using Aspose.PSD.FileFormats.Psd;
using Aspose.PSD.FOSS.Samples.Common;

string inputPath = SamplePathHelper.ResolveInputPath(args);
if (inputPath == null) return;

using (var image = (PsdImage)Image.Load(inputPath))
{
    Console.WriteLine("Header");
    Console.WriteLine($"  Width: {image.Width}");
    Console.WriteLine($"  Height: {image.Height}");
    Console.WriteLine($"  Size: {image.Size}");
    Console.WriteLine($"  ChannelsCount: {image.ChannelsCount}");
    Console.WriteLine($"  BitsPerChannel: {image.BitsPerChannel}");
    Console.WriteLine($"  ColorMode: {image.ColorMode}");
    Console.WriteLine($"  Version: {image.Version}");
    Console.WriteLine();

    Console.WriteLine("Document State");
    Console.WriteLine($"  Layers: {image.Layers.Length}");
    Console.WriteLine($"  ImageResources: {image.ImageResources.Length}");
    Console.WriteLine($"  Compression: {image.Compression}");
    Console.WriteLine($"  IsFlatten: {image.IsFlatten}");
    Console.WriteLine($"  ActiveLayer: {image.ActiveLayer?.Name ?? "<none>"}");
}
{{< /highlight >}}

** Layer inspection sample – prints supported layer metadata for all layers **

{{< highlight csharp >}}
using Aspose.PSD;
using Aspose.PSD.FileFormats.Psd;
using Aspose.PSD.FileFormats.Psd.Layers;
using Aspose.PSD.FOSS.Samples.Common;

string inputPath = SamplePathHelper.ResolveInputPath(args);
if (inputPath == null) return;

using var image = (PsdImage)Image.Load(inputPath);

Console.WriteLine($"LayerCount: {image.Layers.Length}");
for (int i = 0; i < image.Layers.Length; i++)
{
    var layer = image.Layers[i];
    Console.WriteLine($"Layer [{i}]");
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
    Console.WriteLine($"  BlendModeKey: {layer.BlendModeKey}");
    Console.WriteLine($"  ChannelsCount: {layer.ChannelsCount}");
    Console.WriteLine($"  HasLayerMaskData: {layer.LayerMaskData is not null}");
    Console.WriteLine($"  BlendingRangesLength: {layer.LayerBlendingRangesData.Length}");
    Console.WriteLine();
}
{{< /highlight >}}

** Stream round trip sample – demonstrates Load(Stream) and Save(Stream) **

{{< highlight csharp >}}
using Aspose.PSD;
using Aspose.PSD.FileFormats.Psd;
using Aspose.PSD.FOSS.Samples.Common;

string inputPath = SamplePathHelper.ResolveInputPath(args);
if (inputPath == null) return;

string outputPath = SamplePathHelper.ResolveOutputPath(args, 1, "streams-sample-output.psd");

byte[] inputBytes = File.ReadAllBytes(inputPath);
using var inputStream = new MemoryStream(inputBytes);
long originalPos = inputStream.Position;

using var image = (PsdImage)Image.Load(inputStream);

Console.WriteLine($"OriginalStreamPosition: {originalPos}");
Console.WriteLine($"StreamPositionAfterLoad: {inputStream.Position}");

using var outputStream = new MemoryStream();
image.Save(outputStream);

File.WriteAllBytes(outputPath, outputStream.ToArray());
Console.WriteLine($"SavedThroughStream: {outputPath}");
Console.WriteLine($"OutputBytes: {outputStream.Length}");
SampleOutputHelper.OpenOutputFolder(outputPath);
{{< /highlight >}}

** Structural editing sample – applies non rendering edits to the first layer and saves **

{{< highlight csharp >}}
using Aspose.PSD;
using Aspose.PSD.FileFormats.Core.Blending;
using Aspose.PSD.FileFormats.Psd;
using Aspose.PSD.FileFormats.Psd.Layers;
using Aspose.PSD.FOSS.Samples.Common;

string inputPath = SamplePathHelper.ResolveInputPath(args);
if (inputPath == null) return;

string outputPath = SamplePathHelper.ResolveOutputPath(args, 1, "structural-editing-output.psd");

using var image = (PsdImage)Image.Load(inputPath);

if (image.Layers.Length == 0)
{
    Console.WriteLine("No layers to edit.");
    return;
}

Layer layer = image.Layers[0];

Console.WriteLine("Before edit:");
PrintEditableLayerState(layer);

layer.Name = $"{layer.Name} (edited)";
layer.IsVisible = !layer.IsVisible;
layer.Opacity = layer.Opacity == byte.MaxValue ? (byte)128 : byte.MaxValue;
layer.Clipping = layer.Clipping == 0 ? (byte)1 : (byte)0;
layer.BlendModeKey = layer.BlendModeKey == BlendMode.Normal ? BlendMode.Multiply : BlendMode.Normal;
layer.Left += 1;
layer.Top += 1;
layer.Bottom += 1;

Console.WriteLine("After edit:");
PrintEditableLayerState(layer);

image.Save(outputPath);
Console.WriteLine($"SavedEditedDocument: {outputPath}");
SampleOutputHelper.OpenOutputFolder(outputPath);

void PrintEditableLayerState(Layer l)
{
    Console.WriteLine($"  Name: {l.Name}");
    Console.WriteLine($"  IsVisible: {l.IsVisible}");
    Console.WriteLine($"  Opacity: {l.Opacity}");
    Console.WriteLine($"  Clipping: {l.Clipping}");
    Console.WriteLine($"  BlendModeKey: {l.BlendModeKey}");
    Console.WriteLine($"  Bounds: {l.Bounds}");
    Console.WriteLine($"  Top: {l.Top}");
    Console.WriteLine($"  Left: {l.Left}");
    Console.WriteLine($"  Bottom: {l.Bottom}");
    Console.WriteLine($"  Right: {l.Right}");
    Console.WriteLine();
}
{{< /highlight >}}
