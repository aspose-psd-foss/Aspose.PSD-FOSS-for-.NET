# Supported Features and Limitations

## Supported in the Current Product Scope

- Load PSD files
- Load the currently supported PSB subset
- Read document properties from the PSD/PSB header
- Read layer name, bounds, visibility, opacity, and blend mode
- Change layer name, visibility, and opacity
- Save without rendering
- Preserve unsupported sections as raw bytes where possible

## Not Supported

- Rendering
- Rasterization
- Export to PNG, JPEG, or other raster formats
- Pixel editing
- Full image resource editing
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
