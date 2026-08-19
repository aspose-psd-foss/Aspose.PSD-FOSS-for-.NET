## ADDED Requirements

### Requirement: Предоставление inspection metadata по image resources
Система SHALL предоставлять read-only metadata по image resources для metadata-oriented сценариев без введения полной модели редактирования resources.

#### Scenario: Чтение summary по resources
- **WHEN** пользователь загружает файл, содержащий image resources
- **THEN** `image.Resources` возвращает read-only collection объектов summary по resources
- **AND** каждый summary-объект предоставляет identifier ресурса, name, kind и длину payload
- **AND** в текущем lightweight FOSS scope каждый успешно считанный resource summary имеет `Kind == Unknown`

#### Scenario: Unknown-only parse resource blocks
- **WHEN** пользователь загружает файл, содержащий arbitrary image resource blocks
- **THEN** система выполняет только общий PSD-level parse block envelope
- **AND** каждый корректно считанный block материализуется как unknown resource без ID-specific semantic recognition
- **AND** система не требует registry специализированных resource loaders для текущего FOSS scope

#### Scenario: Чтение счётчиков и флагов присутствия resources
- **WHEN** пользователь загружает файл
- **THEN** `image.HasImageResources` показывает, присутствуют ли какие-либо resource blocks
- **AND** `image.ResourceCount` возвращает количество распарсенных resource blocks

#### Scenario: Derived known-resource properties недоступны в unknown-only режиме
- **WHEN** пользователь обращается к convenience properties, зависящим от semantic recognition specific resource IDs
- **THEN** lightweight FOSS implementation не выводит derived values из image resources
- **AND** такие свойства остаются unset или equivalent default values до появления отдельной специализированной поддержки

### Requirement: Предоставление inspection metadata по color mode data
Система SHALL предоставлять read-only metadata по color mode data без вывода mutable raw section bytes как основного public contract.

#### Scenario: Чтение summary по color mode data
- **WHEN** пользователь загружает файл
- **THEN** `image.HasColorModeData` показывает, непуста ли секция Color Mode Data
- **AND** `image.ColorDataInfo` предоставляет kind интерпретации и длину raw payload

#### Scenario: Чтение summary по indexed palette
- **WHEN** загруженный файл использует indexed color и содержит стандартную indexed palette
- **THEN** `image.ColorDataInfo` предоставляет summary по indexed palette
- **AND** `image.IndexedPalette` возвращает read-only palette view или эквивалентный read-only summary

### Requirement: Предоставление inspection metadata по структуре image data
Система SHALL предоставлять read-only metadata по структуре image data без pixel decoding.

#### Scenario: Чтение summary по image data
- **WHEN** пользователь загружает файл
- **THEN** `image.HasMergedImageData` показывает, присутствуют ли merged image data
- **AND** `image.Compression` возвращает сохранённый compression method
- **AND** `image.ImageDataInfo` предоставляет структурную информацию о секции merged image data

#### Scenario: Чтение деталей структуры compression
- **WHEN** merged image data использует structured compression mode, например RLE или ZIP prediction
- **THEN** `image.ImageDataKind` отражает распарсенный structural kind
- **AND** `image.UsesPrediction` показывает, используется ли ZIP-with-prediction
- **AND** `image.ImageDataInfo` предоставляет распарсенные row-length metadata, когда это применимо

### Requirement: Предоставление read-only DTO для layer structural metadata
Система SHALL предоставлять read-only DTO для уже распарсенных layer structural metadata.

#### Scenario: Чтение summary по layer channels
- **WHEN** пользователь обращается к распарсенному слою
- **THEN** `layer.Channels` возвращает read-only collection объектов summary по channels
- **AND** каждый summary-объект channel предоставляет channel identifier и объявленную длину payload

#### Scenario: Чтение summary по layer mask и blending ranges
- **WHEN** пользователь обращается к распарсенному слою
- **THEN** `layer.MaskInfo` возвращает read-only summary наличия и длины распарсенной layer mask subsection
- **AND** `layer.BlendingRangesInfo` возвращает read-only summary наличия и длины распарсенной blending ranges subsection

## MODIFIED Requirements

## REMOVED Requirements

## RENAMED Requirements
