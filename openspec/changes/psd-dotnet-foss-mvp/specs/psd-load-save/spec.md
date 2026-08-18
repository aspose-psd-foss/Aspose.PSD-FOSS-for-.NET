## ADDED Requirements

### Requirement: Загрузка из file path
Система SHALL загружать PSD или PSB файл из file path.

#### Scenario: Загрузка существующего файла
- **WHEN** пользователь вызывает `PsdImage.Load(path)` с существующим PSD или PSB файлом
- **THEN** система возвращает загруженный `PsdImage`

#### Scenario: Отклонение отсутствующего file path
- **WHEN** пользователь вызывает `PsdImage.Load(path)` для отсутствующего файла
- **THEN** система выбрасывает `FileNotFoundException`

### Requirement: Загрузка из stream
Система SHALL загружать PSD или PSB файл из stream.

#### Scenario: Загрузка из seekable stream
- **WHEN** пользователь вызывает `PsdImage.Load(stream)` с seekable stream
- **THEN** система загружает файл из текущей позиции stream
- **AND** восстанавливает исходную позицию stream после загрузки

#### Scenario: Отклонение `null` stream
- **WHEN** пользователь вызывает `PsdImage.Load(null)`
- **THEN** система выбрасывает `ArgumentNullException`

#### Scenario: Отклонение некорректной file signature
- **WHEN** пользователь загружает stream, который не начинается с корректной PSD/PSB signature
- **THEN** система выбрасывает `PsdLoadException`

#### Scenario: Отклонение malformed section lengths
- **WHEN** пользователь загружает PSD или PSB stream, у которого объявленная длина секции превышает доступные bytes
- **THEN** система выбрасывает `PsdLoadException`

### Requirement: Сохранение в file path или stream
Система SHALL сохранять текущий документ в file path или stream.

#### Scenario: Сохранение в file path
- **WHEN** пользователь вызывает `image.Save(path)`
- **THEN** система записывает загружаемый PSD или PSB файл

#### Scenario: Сохранение в stream
- **WHEN** пользователь вызывает `image.Save(stream)`
- **THEN** система записывает текущие bytes документа в предоставленный stream

### Requirement: Сохранение неизвестных секций без мутаций
Система SHALL сохранять неизвестные или неподдерживаемые секции файла как raw bytes, когда этого достаточно для стабильности файла.

#### Scenario: No-mutation round-trip
- **WHEN** пользователь загружает файл и сохраняет его без каких-либо поддерживаемых мутаций
- **THEN** сохранённые bytes byte-for-byte идентичны исходному файлу

### Requirement: Сохранение raw image resources
Система SHALL сохранять секцию Image Resources без переписывания, когда прямое редактирование resources вне scope.

#### Scenario: Save файла с существующими image resources
- **WHEN** пользователь загружает и сохраняет файл без изменений resources
- **THEN** raw секция Image Resources записывается обратно без изменений

### Requirement: Поддержка текущих PSD/PSB length fields, нужных поддерживаемому subset
Система SHALL использовать корректные размеры полей для поддерживаемого PSD/PSB load/save subset.

#### Scenario: Сохранение PSD length fields
- **WHEN** текущая версия формата документа — PSD (`Version == 1`)
- **THEN** поля длины Layer and Mask используют PSD-sized integers, требуемые форматом

#### Scenario: Сохранение PSB length fields
- **WHEN** текущая версия формата документа — PSB (`Version == 2`)
- **THEN** поля длины Layer and Mask и длины per-channel layer data используют PSB-sized integers, требуемые форматом

## MODIFIED Requirements

## REMOVED Requirements

## RENAMED Requirements
