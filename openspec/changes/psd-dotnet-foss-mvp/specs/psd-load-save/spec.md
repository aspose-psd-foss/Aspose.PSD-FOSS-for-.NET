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

### Requirement: Читаемость и структурная корректность PSD
Система SHALL читать и писать PSD файлы в соответствии со структурой формата, определенной в официальной спецификации PSD.

**Приоритет:** Структурная корректность важнее "зеленой сборки". Если реализация компилируется, но записывает структурно некорректные PSD файлы - это недопустимо.

#### Scenario: Правильный порядок полей в заголовке
- **WHEN** загружается PSD файл
- **THEN** парсер читает поля в правильном порядке: signature → version → reserved → channels → height → width → bitsPerChannel → colorMode

#### Scenario: Правильная структура layer record
- **WHEN** загружается layer record
- **THEN** парсер читает: top/left/bottom/right → channel count → channel info (длина каждого канала) → blend mode signature (4B) → blend mode key (4B) → opacity (1B) → clipping (1B) → flags (1B) → filler (1B) → extra data length (4B) + extra data internals (layer mask data length, blending ranges data length, Pascal name, tagged blocks) → channel image data блок после всех layer records

#### Scenario: Структурно корректное сохранение
- **WHEN** сохраняется PSD файл
- **THEN** сохранённый файл имеет правильную структуру: Header → Color Data → Resources → Layer and Mask Information section (outer length, layer records, channel image data block, global layer mask info, raw tail) → Image Data

#### Scenario: Round-trip сохранения
- **WHEN** пользователь загружает PSD файл и сохраняет без изменений
- **THEN** сохранённый файл идентичен оригиналу по байтам (если не изменялись свойства)

## MODIFIED Requirements

## REMOVED Requirements

## RENAMED Requirements
