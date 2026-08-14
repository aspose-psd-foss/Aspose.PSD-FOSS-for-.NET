## ADDED Requirements

### Requirement: Read document size from the file header
The system SHALL expose document width and height from the PSD/PSB header.

#### Scenario: Read width and height
- **WHEN** the user loads a PSD or PSB file
- **THEN** `image.Width` returns the stored width
- **AND** `image.Height` returns the stored height

### Requirement: Read channel and bit-depth metadata from the file header
The system SHALL expose channel count and bits per channel from the PSD/PSB header.

#### Scenario: Read channels and bit depth
- **WHEN** the user loads a PSD or PSB file
- **THEN** `image.Channels` returns the stored channel count
- **AND** `image.BitsPerChannel` returns the stored bit depth

### Requirement: Read color mode and format version from the file header
The system SHALL expose the color mode and format version from the PSD/PSB header.

#### Scenario: Read color mode and version
- **WHEN** the user loads a PSD or PSB file
- **THEN** `image.ColorMode` returns the stored color mode
- **AND** `image.Version` returns `1` for PSD or `2` for PSB

## MODIFIED Requirements

## REMOVED Requirements

## RENAMED Requirements
