## ADDED Requirements

### Requirement: Change supported layer fields in memory
The system SHALL allow changing only the MVP layer fields `Name`, `IsVisible`, and `Opacity`.

#### Scenario: Change layer name
- **WHEN** the user sets `layer.Name`
- **THEN** a subsequent read returns the new value

#### Scenario: Change layer visibility
- **WHEN** the user sets `layer.IsVisible`
- **THEN** a subsequent read returns the new value

#### Scenario: Change layer opacity
- **WHEN** the user sets `layer.Opacity`
- **THEN** a subsequent read returns the new value

### Requirement: Persist supported layer mutations on save
The system SHALL persist supported layer mutations when saving the file.

#### Scenario: Save after supported mutations
- **WHEN** the user changes `Name`, `IsVisible`, or `Opacity`
- **AND** saves the file
- **THEN** reloading the file shows the changed values

### Requirement: Preserve unsupported layer data where possible
The system SHALL preserve unsupported layer data where it is not being edited directly.

#### Scenario: Save after changing one supported field
- **WHEN** the user changes a supported layer field and saves
- **THEN** unsupported layer payload data is preserved where the implementation can keep it raw

## MODIFIED Requirements

## REMOVED Requirements

## RENAMED Requirements
