## ADDED Requirements

### Requirement: Предоставление распарсенных слоёв
Система SHALL предоставлять распарсенные layer records через `image.Layers`.

#### Scenario: Чтение коллекции слоёв
- **WHEN** пользователь загружает файл со слоями
- **THEN** `image.Layers` возвращает массив `Layer`

#### Scenario: Чтение файла без слоёв
- **WHEN** пользователь загружает файл без layer records
- **THEN** `image.Layers` возвращает пустой массив

### Requirement: Чтение поддерживаемых layer metadata
Система SHALL предоставлять поддерживаемый subset layer metadata.

#### Scenario: Чтение имени и bounds
- **WHEN** пользователь обращается к распарсенному слою
- **THEN** `layer.Name` возвращает Pascal layer name
- **AND** `layer.Bounds` возвращает прямоугольник в координатах документа

#### Scenario: Чтение visibility и opacity
- **WHEN** пользователь обращается к распарсенному слою
- **THEN** `layer.IsVisible` возвращает флаг видимости
- **AND** `layer.Opacity` возвращает сохранённую opacity

#### Scenario: Чтение blend mode
- **WHEN** пользователь обращается к распарсенному слою
- **THEN** `layer.BlendMode` возвращает распарсенный blend mode key, сопоставленный с `BlendMode`

### Requirement: Предоставление дополнительных простых layer metadata
Система SHALL предоставлять дополнительные простые layer properties, derived from layer records, без требования rendering.

#### Scenario: Чтение geometry helper properties слоя
- **WHEN** пользователь обращается к распарсенному слою
- **THEN** `layer.Width` возвращает `layer.Bounds.Width`
- **AND** `layer.Height` возвращает `layer.Bounds.Height`
- **AND** `layer.Top`, `layer.Left`, `layer.Bottom` и `layer.Right` отражают сохранённый прямоугольник слоя

#### Scenario: Чтение количества layer channels
- **WHEN** пользователь обращается к распарсенному слою
- **THEN** `layer.ChannelCount` возвращает количество распарсенных layer channel records

#### Scenario: Чтение флагов наличия layer subsections
- **WHEN** пользователь обращается к распарсенному слою
- **THEN** `layer.HasMaskData` показывает, присутствует ли непустая layer mask subsection
- **AND** `layer.HasBlendingRangesData` показывает, присутствует ли непустая blending ranges subsection
- **AND** `layer.HasAdditionalLayerData` показывает, присутствуют ли opaque trailing данные из layer extra data

#### Scenario: Чтение исходного blend mode key
- **WHEN** пользователь обращается к распарсенному слою
- **THEN** `layer.BlendModeKey` возвращает исходный 4-byte PSD blend mode key как строковое значение

## MODIFIED Requirements

## REMOVED Requirements

## RENAMED Requirements
