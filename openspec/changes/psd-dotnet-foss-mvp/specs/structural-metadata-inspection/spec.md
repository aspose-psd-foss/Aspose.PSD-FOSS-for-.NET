## ADDED Requirements

### Requirement: Предоставление совместимого доступа к image resources
Система SHALL предоставлять совместимый read-only доступ к image resources для metadata-oriented сценариев без введения полной модели редактирования resources.

#### Scenario: Чтение image resources
- **WHEN** пользователь загружает файл, содержащий image resources
- **THEN** `image.ImageResources` возвращает массив `ResourceBlock`
- **AND** каждый неизвестный resource block представлен как `PreservedResourceBlock`

#### Scenario: Unknown-only parse resource blocks
- **WHEN** пользователь загружает файл, содержащий arbitrary image resource blocks
- **THEN** система выполняет только общий PSD-level parse block envelope
- **AND** каждый корректно считанный block материализуется как unknown resource без ID-specific semantic recognition
- **AND** система не требует registry специализированных resource loaders для текущего FOSS scope

#### Scenario: Internal verification state по resources
- **WHEN** пользователь загружает файл
- **THEN** internal inspection state может фиксировать presence и количество resource blocks для acceptance tests
- **AND** этот internal state не является public compatibility API

#### Scenario: Derived known-resource properties недоступны в unknown-only режиме
- **WHEN** пользователь обращается к convenience properties, зависящим от semantic recognition specific resource IDs
- **THEN** lightweight FOSS implementation не выводит derived values из image resources автоматически
- **AND** такие свойства остаются unset или equivalent default values до появления отдельной специализированной поддержки

### Requirement: Предоставление inspection metadata по color mode data
Система SHALL предоставлять read-only metadata по color mode data без вывода mutable raw section bytes как основного public contract.

#### Scenario: Internal summary по color mode data
- **WHEN** пользователь загружает файл
- **THEN** internal inspection state показывает, непуста ли секция Color Mode Data
- **AND** internal `ColorDataInfo` предоставляет kind интерпретации и длину raw payload для acceptance tests

#### Scenario: Internal summary по indexed palette
- **WHEN** загруженный файл использует indexed color и содержит стандартную indexed palette
- **THEN** internal `ColorDataInfo` предоставляет summary по indexed palette
- **AND** internal `IndexedPalette` возвращает read-only palette view или эквивалентный read-only summary

### Requirement: Предоставление inspection metadata по структуре image data
Система SHALL предоставлять read-only metadata по структуре image data без pixel decoding.

#### Scenario: Чтение summary по image data
- **WHEN** пользователь загружает файл
- **THEN** `image.Compression` возвращает сохранённый compression method
- **AND** internal inspection state показывает, присутствуют ли merged image data
- **AND** internal `ImageDataInfo` предоставляет структурную информацию о секции merged image data для acceptance tests

#### Scenario: Чтение деталей структуры compression
- **WHEN** merged image data использует structured compression mode, например RLE или ZIP prediction
- **THEN** internal `ImageDataKind` отражает распарсенный structural kind
- **AND** internal `UsesPrediction` показывает, используется ли ZIP-with-prediction
- **AND** internal `ImageDataInfo` предоставляет распарсенные row-length metadata, когда это применимо

### Requirement: Предоставление compatibility DTO для layer structural metadata
Система SHALL предоставлять compatibility DTO для уже распарсенных layer structural metadata.

#### Scenario: Чтение layer channel information
- **WHEN** пользователь обращается к распарсенному слою
- **THEN** `layer.ChannelInformation` возвращает массив `ChannelInformation`
- **AND** каждый объект channel предоставляет channel identifier, compression method и объявленную длину payload

#### Scenario: Чтение layer mask и blending ranges compatibility DTO
- **WHEN** пользователь обращается к распарсенному слою
- **THEN** `layer.LayerMaskData` возвращает `null` или совместимый `LayerMaskData` object
- **AND** `layer.LayerBlendingRangesData` возвращает совместимый `LayerBlendingRangesData` object
- **AND** setters этих сложных metadata objects выбрасывают `NotSupportedException` в текущем FOSS scope

## MODIFIED Requirements

## REMOVED Requirements

## RENAMED Requirements
