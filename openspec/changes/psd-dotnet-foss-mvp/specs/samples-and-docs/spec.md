## ADDED Requirements

### Requirement: Provide runnable sample projects in the repository
The repository SHALL include runnable sample projects in a top-level `samples/` folder as a first-class part of the product repository.

#### Scenario: Discover repository samples
- **WHEN** a developer opens the repository
- **THEN** a top-level `samples/` folder is present

#### Scenario: Run a basic metadata sample
- **WHEN** a developer runs the basic sample project
- **THEN** the sample demonstrates loading a PSD/PSB file and printing document properties

#### Scenario: Run a layer editing sample
- **WHEN** a developer runs the layer sample project
- **THEN** the sample demonstrates reading layer metadata and saving a file after changing supported layer properties

### Requirement: Provide repository documentation as markdown files
The repository SHALL include end-user and developer-facing documentation as markdown files.

#### Scenario: Discover documentation
- **WHEN** a developer opens the repository
- **THEN** a top-level documentation area is present
- **AND** the root README links to it

#### Scenario: Read getting started guidance
- **WHEN** a new user opens the getting started document
- **THEN** the document explains installation, the basic API shape, and a first working example

#### Scenario: Read supported-scope guidance
- **WHEN** a user opens the limitations or support-scope document
- **THEN** the document explains what the current product scope supports and what remains out of scope

## MODIFIED Requirements

## REMOVED Requirements

## RENAMED Requirements
