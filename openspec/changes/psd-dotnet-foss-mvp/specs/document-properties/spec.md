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
- **THEN** `image.Channels` возвращает сохранённое число каналов
- **AND** `image.BitsPerChannel` возвращает сохранённый bit depth

### Requirement: Чтение color mode и версии формата из file header
Система SHALL предоставлять color mode и версию формата из PSD/PSB header.

#### Scenario: Чтение color mode и версии
- **WHEN** пользователь загружает PSD или PSB файл
- **THEN** `image.ColorMode` возвращает сохранённый color mode
- **AND** `image.Version` возвращает `1` для PSD или `2` для PSB

### Requirement: Предоставление derived document metadata для metadata-oriented сценариев
Система SHALL предоставлять дополнительные простые document properties, не требующие rendering или pixel decoding.

#### Scenario: Чтение состояния PSD против PSB
- **WHEN** пользователь загружает PSD файл
- **THEN** `image.IsLargeDocument` возвращает `false`
- **AND** `image.IsPsb` возвращает `false`

#### Scenario: Чтение состояния PSB
- **WHEN** пользователь загружает PSB файл
- **THEN** `image.IsLargeDocument` возвращает `true`
- **AND** `image.IsPsb` возвращает `true`

#### Scenario: Чтение количества слоёв
- **WHEN** пользователь загружает файл
- **THEN** `image.LayerCount` возвращает количество распарсенных слоёв
- **AND** `image.LayerCount` совпадает с `image.Layers.Length`

#### Scenario: Чтение объекта header
- **WHEN** пользователь обращается к `image.Header`
- **THEN** система возвращает распарсенный `PsdHeader`
- **AND** его значения совпадают с document-level properties, которые предоставляет `PsdImage`

## MODIFIED Requirements

## REMOVED Requirements

## RENAMED Requirements
