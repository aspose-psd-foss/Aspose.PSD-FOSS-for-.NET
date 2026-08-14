## ADDED Requirements

### Requirement: Load from file path
The system SHALL load a PSD or PSB file from a file path.

#### Scenario: Load an existing file
- **WHEN** the user calls `PsdImage.Load(path)` with an existing PSD or PSB file
- **THEN** the system returns a loaded `PsdImage`

#### Scenario: Reject a missing file path
- **WHEN** the user calls `PsdImage.Load(path)` for a missing file
- **THEN** the system throws `FileNotFoundException`

### Requirement: Load from stream
The system SHALL load a PSD or PSB file from a stream.

#### Scenario: Load from a seekable stream
- **WHEN** the user calls `PsdImage.Load(stream)` with a seekable stream
- **THEN** the system loads the file from the current stream position
- **AND** restores the original stream position after loading

#### Scenario: Reject null stream input
- **WHEN** the user calls `PsdImage.Load(null)`
- **THEN** the system throws `ArgumentNullException`

#### Scenario: Reject an invalid file signature
- **WHEN** the user loads a stream that does not start with a valid PSD/PSB signature
- **THEN** the system throws `PsdLoadException`

#### Scenario: Reject malformed section lengths
- **WHEN** the user loads a PSD or PSB stream whose declared section length exceeds the available bytes
- **THEN** the system throws `PsdLoadException`

### Requirement: Save to file path or stream
The system SHALL save the current document to a file path or stream.

#### Scenario: Save to file path
- **WHEN** the user calls `image.Save(path)`
- **THEN** the system writes a loadable PSD or PSB file

#### Scenario: Save to stream
- **WHEN** the user calls `image.Save(stream)`
- **THEN** the system writes the current document bytes to the provided stream

### Requirement: Preserve unknown sections without mutation
The system SHALL preserve unknown or unsupported file sections as raw bytes when that is enough to keep the file stable.

#### Scenario: No-mutation round-trip
- **WHEN** the user loads a file and saves it without any supported mutations
- **THEN** the saved bytes are byte-for-byte identical to the original file

### Requirement: Preserve raw image resources
The system SHALL preserve the Image Resources section without rewriting it when direct resource editing is out of scope.

#### Scenario: Save a file with existing image resources
- **WHEN** the user loads and saves a file without resource edits
- **THEN** the raw Image Resources section is written back unchanged

### Requirement: Support the current PSD/PSB length fields required by the supported subset
The system SHALL use the correct field sizes for the supported PSD/PSB load/save subset.

#### Scenario: Save PSD lengths
- **WHEN** the current document format version is PSD (`Version == 1`)
- **THEN** Layer and Mask length fields use the PSD-sized integers required by the format

#### Scenario: Save PSB lengths
- **WHEN** the current document format version is PSB (`Version == 2`)
- **THEN** Layer and Mask length fields and per-channel layer data lengths use the PSB-sized integers required by the format

## MODIFIED Requirements

## REMOVED Requirements

## RENAMED Requirements
