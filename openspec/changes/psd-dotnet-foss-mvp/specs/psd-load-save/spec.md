## ADDED Requirements

### Requirement: Загрузка PSD из file path
Система SHALL загружать PSD файл из указанного file path.

#### Scenario: Успешная загрузка из существующего файла
- **WHEN** пользователь вызывает `PsdImage.Load("input.psd")` с корректным PSD файлом
- **THEN** система возвращает экземпляр PsdImage с загруженными данными
- **AND** исключение не выбрасывается

#### Scenario: Загрузка из несуществующего файла выбрасывает ошибку
- **WHEN** пользователь вызывает `PsdImage.Load("nonexistent.psd")` с файлом, который не существует
- **THEN** система выбрасывает FileNotFoundException

#### Scenario: Загрузка из повреждённого файла выбрасывает ошибку
- **WHEN** пользователь вызывает `PsdImage.Load("corrupted.psd")` с файлом, имеющим некорректный PSD формат
- **THEN** система выбрасывает PsdLoadException

### Requirement: Загрузка PSD из stream
Система SHALL загружать PSD файл из Stream.

#### Scenario: Успешная загрузка из Stream
- **WHEN** пользователь вызывает `PsdImage.Load(stream)` с корректным PSD stream
- **THEN** система возвращает экземпляр PsdImage с загруженными данными
- **AND** позиция stream не изменяется после загрузки

#### Scenario: Загрузка из null stream выбрасывает ошибку
- **WHEN** пользователь вызывает `PsdImage.Load(null)`
- **THEN** система выбрасывает ArgumentNullException

### Requirement: Сохранение PSD в file path
Система SHALL сохранять PSD файл в указанный file path.

#### Scenario: Успешное сохранение
- **WHEN** пользователь вызывает `image.Save("output.psd")`
- **THEN** система записывает PSD данные в указанный файл
- **AND** сохранённый файл может быть загружен успешно

### Requirement: Сохранение PSD в stream
Система SHALL сохранять PSD файл в Stream.

#### Scenario: Успешное сохранение в Stream
- **WHEN** пользователь вызывает `image.Save(stream)` с записываемым stream
- **THEN** система записывает PSD данные в stream
- **AND** позиция stream находится в конце после сохранения

### Requirement: Поддержка определения версии PSD
Система SHALL читать версию PSD из заголовка файла.

#### Scenario: Чтение версии PSD 6
- **WHEN** пользователь загружает PSD файл версии 6
- **THEN** image.Version возвращает 6

#### Scenario: Чтение версии PSB
- **WHEN** пользователь загружает PSB файл (версия > 6)
- **THEN** image.Version возвращает фактический номер версии

### Requirement: Сохранение неизвестных section
Система SHALL сохранять неизвестные PSD section как raw bytes при сохранении.

#### Scenario: Сохранение с неизвестными section
- **WHEN** пользователь загружает PSD файл с неизвестными section и сохраняет без модификаций
- **THEN** сохранённый файл содержит все оригинальные section в том же порядке
- **AND** данные section идентичны оригиналу по байтам

## MODIFIED Requirements

## REMOVED Requirements

## RENAMED Requirements
