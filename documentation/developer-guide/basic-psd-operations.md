# Basic PSD Operations

This guide covers the currently supported operations of Aspose.PSD.FOSS.

## Load a PSD or PSB Document

Use `PsdImage.Load(string)` or `PsdImage.Load(Stream)` to open a document.

Supported document metadata:

- `Width`
- `Height`
- `Channels`
- `BitsPerChannel`
- `ColorMode`
- `Version`
- `Header`
- `IsLargeDocument`
- `IsPsb`
- `LayerCount`
- `HasImageResources`
- `ResourceCount`
- `HasColorModeData`
- `HasMergedImageData`
- `Compression`
- `ImageDataKind`
- `UsesPrediction`

Additional read-only inspection API:

- `Resources`
- `GlobalAngle`
- `HasIccProfile`
- `IsIccProfileUntagged`
- `ColorDataInfo`
- `IndexedPalette`
- `ImageDataInfo`

## Inspect Layers

Use `image.Layers` to enumerate parsed layer records.

Supported layer metadata:

- `Name`
- `Bounds`
- `Width`
- `Height`
- `Top`
- `Left`
- `Bottom`
- `Right`
- `IsVisible`
- `Opacity`
- `Clipping`
- `BlendMode`
- `BlendModeKey`
- `ChannelCount`
- `HasMaskData`
- `HasBlendingRangesData`
- `HasAdditionalLayerData`
- `Channels`
- `MaskInfo`
- `BlendingRangesInfo`

## Modify Layers

The current product scope supports changing:

- `Name`
- `IsVisible`
- `Opacity`
- `BlendMode`
- `Clipping`
- `Bounds`
- `Top`
- `Left`
- `Bottom`
- `Right`

These changes are saved back into the PSD/PSB structure without rendering.

## Save Behavior

The library follows a minimal parse and raw-preserve approach:

- supported structures are parsed and rewritten when needed;
- unsupported sections are preserved as raw bytes where possible;
- a no-mutation save keeps the original bytes unchanged for the currently supported scenarios.

## Stream Behavior

When loading from a seekable stream, the library restores the original stream position after loading. This makes it safer to use inside larger workflows that share a stream instance.
