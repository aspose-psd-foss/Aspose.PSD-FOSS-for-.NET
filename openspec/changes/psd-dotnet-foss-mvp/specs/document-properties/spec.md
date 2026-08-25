## ADDED Requirements

### Requirement: Чтение размера документа из file header
Система SHALL предоставлять ширину и высоту документа из PSD/PSB header.

#### Scenario: Чтение ширины и высоты
- **WHEN** пользователь загружает PSD или PSB файл
- **THEN** `image.Width` возвращает сохранённую ширину
- **AND** `image.Height` возвращает сохранённую высоту

### Requirement: Чтение metadata каналов и bit depth из file header
Система SHALL предоставлять число каналов и bits per channel из PSD/PSB header.

#### Scenario: Чтение каналов и bit depth
- **WHEN** пользователь загружает PSD или PSB файл
- **THEN** `image.ChannelsCount` возвращает сохранённое число каналов
- **AND** `image.BitsPerChannel` возвращает сохранённый bit depth

### Requirement: Чтение color mode и версии формата из file header
Система SHALL предоставлять color mode и версию формата из PSD/PSB header.

#### Scenario: Чтение color mode и версии
- **WHEN** пользователь загружает PSD или PSB файл
- **THEN** `image.ColorMode` возвращает сохранённый color mode
- **AND** `image.Version` возвращает `1` для PSD или `2` для PSB

### Requirement: Предоставление совместимой document metadata для metadata-oriented сценариев
Система SHALL предоставлять дополнительные document properties, не требующие rendering или pixel decoding и совпадающие с поддерживаемым Aspose.PSD compatibility surface.

#### Scenario: Чтение размера через compatibility properties
- **WHEN** пользователь загружает PSD или PSB файл
- **THEN** `image.Size` возвращает ширину и высоту документа
- **AND** `image.Bounds` возвращает прямоугольник документа от `(0, 0)` до размеров изображения

#### Scenario: Чтение активного слоя и flatten state
- **WHEN** пользователь загружает файл
- **THEN** `image.ActiveLayer` возвращает первый распарсенный слой или `null`
- **AND** `image.IsFlatten` показывает отсутствие распарсенных слоёв

#### Scenario: Ограничения setters для сложных document properties
- **WHEN** пользователь пытается менять `image.ActiveLayer` или `image.HasTransparencyData`
- **THEN** система выбрасывает `NotSupportedException`
- **AND** не обещает полноценное semantic editing этих областей в текущем FOSS scope

## MODIFIED Requirements

## REMOVED Requirements

## RENAMED Requirements
