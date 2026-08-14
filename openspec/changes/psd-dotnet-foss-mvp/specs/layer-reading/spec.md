## ADDED Requirements

### Requirement: Expose parsed layers
The system SHALL expose parsed layer records through `image.Layers`.

#### Scenario: Read the layer collection
- **WHEN** the user loads a file with layers
- **THEN** `image.Layers` returns an array of `Layer`

#### Scenario: Read a file without layers
- **WHEN** the user loads a file with no layer records
- **THEN** `image.Layers` returns an empty array

### Requirement: Read supported layer metadata
The system SHALL expose the supported subset of layer metadata.

#### Scenario: Read name and bounds
- **WHEN** the user accesses a parsed layer
- **THEN** `layer.Name` returns the Pascal layer name
- **AND** `layer.Bounds` returns the document-space rectangle

#### Scenario: Read visibility and opacity
- **WHEN** the user accesses a parsed layer
- **THEN** `layer.IsVisible` returns the visibility flag
- **AND** `layer.Opacity` returns the stored opacity

#### Scenario: Read blend mode
- **WHEN** the user accesses a parsed layer
- **THEN** `layer.BlendMode` returns the parsed blend mode key mapped to `BlendMode`

## MODIFIED Requirements

## REMOVED Requirements

## RENAMED Requirements
