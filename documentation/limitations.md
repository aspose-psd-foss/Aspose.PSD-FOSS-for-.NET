# Supported Features and Limitations

This page is the contract for what a new developer should assume about the project. If a feature is not listed as supported here, treat it as unsupported.

## Supported in the Current Product Scope

- Load PSD files
- Load the currently supported PSB subset
- Read document properties from the PSD/PSB header
- Read additional structural metadata from image resources, color mode data, merged image data, and layer subsections
- Read layer name, bounds, derived geometry, visibility, opacity, clipping, blend mode key, and channel summaries
- Change layer name, visibility, opacity, clipping, blend mode key, and layer geometry through coordinate properties
- Save without rendering
- Preserve unsupported sections as raw bytes where possible

## Not Supported

- Rendering
- Rasterization
- Export to PNG, JPEG, or other raster formats
- Pixel editing
- Full image resource editing
- Semantic recognition of specific image resource kinds
- Full tagged block editing
- Text rendering
- Vector rendering
- Effects rendering
- Smart filters
- Adjustment rendering

## Important Scope Notes

- The library is not an open-source clone of the full commercial Aspose.PSD product.
- The implementation intentionally focuses on a small safe subset of PSD/PSB behavior.
- Unknown sections are preserved where possible, but not all PSD/PSB structures are interpreted semantically.
- The safest assumption is that the library supports structural inspection and limited metadata edits, not general Photoshop document processing.
