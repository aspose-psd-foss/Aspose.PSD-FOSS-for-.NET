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
- **THEN** `layer.BlendModeKey` возвращает распарсенный blend mode key, сопоставленный с `BlendMode`

### Requirement: Предоставление дополнительных простых layer metadata
Система SHALL предоставлять дополнительные простые layer properties, derived from layer records, без требования rendering.

#### Scenario: Чтение geometry helper properties слоя
- **WHEN** пользователь обращается к распарсенному слою
- **THEN** `layer.Width` возвращает `layer.Bounds.Width`
- **AND** `layer.Height` возвращает `layer.Bounds.Height`
- **AND** `layer.Top`, `layer.Left`, `layer.Bottom` и `layer.Right` отражают сохранённый прямоугольник слоя

#### Scenario: Чтение количества layer channels
- **WHEN** пользователь обращается к распарсенному слою
- **THEN** `layer.ChannelsCount` возвращает количество распарсенных layer channel records

#### Scenario: Чтение публичных layer subsection summaries
- **WHEN** пользователь обращается к распарсенному слою
- **THEN** `layer.LayerMaskData` возвращает `null`, если layer mask subsection отсутствует, или совместимый объект mask data, если subsection присутствует
- **AND** `layer.LayerBlendingRangesData` возвращает совместимый summary object для blending ranges subsection
- **AND** `layer.ChannelInformation` возвращает public summaries распарсенных layer channel records

#### Scenario: Raw-preserve opaque layer extra data
- **WHEN** слой содержит неподдерживаемые opaque trailing данные в layer extra data
- **THEN** система сохраняет эти данные внутри для round-trip
- **AND** не добавляет public `HasAdditionalLayerData`, потому что такого свойства нет в официальном Aspose.PSD surface

## MODIFIED Requirements

## REMOVED Requirements

## RENAMED Requirements
