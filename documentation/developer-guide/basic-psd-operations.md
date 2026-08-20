# Basic PSD Operations

This guide covers the currently supported operations of Aspose.PSD.FOSS.

## Supported Workflows

The library currently supports four main workflows:

1. Load a PSD/PSB file and inspect document metadata.
2. Enumerate layers and inspect supported layer metadata.
3. Change a limited set of layer properties.
4. Save the file back without rendering.

## Load a PSD or PSB Document

Use `Image.Load(string)` or `Image.Load(Stream)` and cast the result to `PsdImage` to open a PSD/PSB document in the Aspose.PSD-compatible style.

Supported document metadata:

- `Width`
- `Height`
- `ChannelsCount`
- `BitsPerChannel`
- `ColorMode`
- `Version`
- `IsLargeDocument`
- `IsPsb`
- `LayerCount`
- `HasImageResources`
- `ResourceCount`
- `HasColorModeData`
- `HasMergedImageData`
- `Compression`
- `UsesPrediction`

Internal diagnostics preserve parsed resource, color data, image data, channel, mask, and blending-range details for tests and implementation verification. These DTOs are intentionally not part of the public API because they do not exist in the commercial Aspose.PSD surface.

`GlobalAngle`, `HasIccProfile`, and `IsIccProfileUntagged` remain public for now, but they are not reconstructed from resource IDs and therefore stay at default values in the current lightweight implementation.

## Inspect Layers

Use `image.Layers` to enumerate parsed layer records.

Supported layer metadata:

- `Name`
- `Bounds` (`Rectangle` in PSD document coordinates)
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
- `ChannelsCount`
- `ChannelInformation`
- `LayerMaskData`
- `LayerBlendingRangesData`
- `HasAdditionalLayerData`

Layer channel, mask, and blending-range DTOs are internal diagnostics, not public API.

## Modify Layers

The current product scope supports changing:

- `Name`
- `IsVisible`
- `Opacity`
- `BlendMode`
- `Clipping`
- `Bounds` (`Rectangle`)
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

This means the library is optimized for safe structural edits, not for reconstructing or normalizing the whole Photoshop document model.

## Stream Behavior

When loading from a seekable stream, the library restores the original stream position after loading. This makes it safer to use inside larger workflows that share a stream instance.
