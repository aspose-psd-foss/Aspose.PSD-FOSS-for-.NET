## ADDED Requirements

### Requirement: Предоставление inspection metadata по image resources
Система SHALL предоставлять read-only metadata по image resources для metadata-oriented сценариев без введения полной модели редактирования resources.

#### Scenario: Чтение summary по resources
- **WHEN** пользователь загружает файл, содержащий image resources
- **THEN** `image.Resources` возвращает read-only collection объектов summary по resources
- **AND** каждый summary-объект предоставляет identifier ресурса, name, kind и длину payload

#### Scenario: Чтение derived properties известных resources
- **WHEN** присутствует known resource kind
- **THEN** соответствующий summary-объект ресурса предоставляет поддерживаемые derived scalar metadata, такие как global angle или ICC-untagged state

#### Scenario: Чтение счётчиков и флагов присутствия resources
- **WHEN** пользователь загружает файл
- **THEN** `image.HasImageResources` показывает, присутствуют ли какие-либо resource blocks
- **AND** `image.ResourceCount` возвращает количество распарсенных resource blocks
- **AND** `image.HasIccProfile` показывает, присутствует ли ICC profile resource
- **AND** `image.IsIccProfileUntagged` предоставляет распарсенный флаг untagged-profile, когда он доступен

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
