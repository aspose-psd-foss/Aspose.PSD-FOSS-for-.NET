## Почему

Существующая библиотека Aspose.PSD предоставляет полнофункциональную поддержку формата PSD, но она является коммерческой и закрытой. Создание лёгкой Free Open Source библиотеки .NET с минимальным функционалом PSD позволит разработчикам:

- Работать с PSD-файлами в open-source проектах без лицензионных ограничений
- Читать и изменять основные свойства PSD без ресурсоемкого рендеринга
- Интегрировать базовую поддержку PSD в инструменты CI/CD, конвертеры, валидаторы

**What problem does this solve?** Нет лёгкой, полностью open-source .NET библиотеки для базовой работы с PSD-файлами. Наш подход: написать минимальную реализацию только с теми возможностями, которые нужны для MVP.

## Тестирование

**Тестовый project (Aspose.PSD.FOSS.Test):**
- Структура: real test project с NUnit framework
- Цель: acceptance/validation на основе тестов, а не demo app behavior
- Тип OutputType: Library (не Exe/Console Application)
- Тестовые файлы PSD находятся в `src/Aspose.PSD.FOSS.Test/testdata/`

## Тесты (Acceptance/Validation)

**Приоритет:** Приемочные тесты должны оцениваться на основе behavior, определенного в spec, а не demo app. Тестовый project (Aspose.PSD.FOSS.Test) является real test project с NUnit framework.

#### Scenario: Round-trip без mutation
- **WHEN** пользователь загружает PSD файл и сохраняет без изменений
- **THEN** сохранённый файл структурно корректен
- **AND** повторная загрузка возвращает эквивалентные свойства

#### Scenario: Round-trip с мутацией
- **WHEN** пользователь изменяет свойства слоя и сохраняет
- **THEN** сохранённый файл содержит обновленные свойства
- **AND** повторная загрузка показывает измененные свойства

## Что меняется

Создание новой библиотеки `Aspose.PSD.FOSS` с минимальным набором возможностей:

**Новые возможности:**
- Загрузка PSD-файлов из пути или stream
- Чтение основных свойств документа: Width, Height, BitsPerChannel, ColorMode, Version
- Чтение и изменение свойств слоев: Name, IsVisible, Opacity, BlendMode
- Сохранение изменённых PSD без рендеринга

**Breaking Changes:** Нет

## Возможности

### Новые возможности

- `psd-load-save`: Загрузка и сохранение PSD-файлов
  - Загрузка из file path или Stream
  - Сохранение в file path или Stream
  - Поддержка PSD и PSB форматов
  - Обработка неизвестных section как opaque bytes

- `document-properties`: Чтение основных свойств документа
  - Width и Height (размеры изображения)
  - BitsPerChannel (бит на канал: 8, 16, 32)
  - ColorMode (ColorModes: Rgb, CMYK, Grayscale, Indexed, Duotone, Lab)
  - Version (PSD версия: 1-6, PSB: >6)

- `layer-reading`: Чтение базовой информации о слоях
  - Name (имя слоя)
  - Bounds (Bounds Rectangle: Left, Top, Right, Bottom)
  - IsVisible (видимость)
  - Opacity (0-255)
  - BlendMode (BlendMode enum: Normal, Multiply, Screen, Overlay и т.д.)

- `layer-editing`: Изменение свойств слоёв
  - Name (изменение имени)
  - IsVisible (переключение видимости)
  - Opacity (изменение прозрачности)

### Изменённые возможности

Нет существующих capabilities, которые нуждаются в модификации.

## Влияние

**Затронутый код:**
- Новая библиотека: `Aspose.PSD.FOSS`
- Namespace: `Aspose.PSD.FOSS`
- Tests: `Aspose.PSD.FOSS.Test` (тестовый project, не demo/app project)

**Изменения API:**
- Новый public API (см. design.md)
- Backward compatibility не требуется (новая библиотека)

**Зависимости:**
- Минимальные зависимости (System.IO, System.Collections.Generic, System.Drawing.Common для Rectangle/Color)
- Без внешних image processing библиотек (рендеринг исключён из MVP)

**Системы:**
- Standalone библиотека, не требует интеграции с существующими системами
- Может быть использована в CI/CD, конвертерах, инспекторах PSD

## Тестирование

**Тестовый project (Aspose.PSD.FOSS.Test):**
- Структура: real test project с NUnit framework
- Цель: acceptance/validation на основе тестов, а не demo app behavior
- MVP_TESTS:
  - Load document properties (Width, Height, Channels, BitsPerChannel, ColorMode, Version)
  - Load layer properties (Name, Bounds, IsVisible, Opacity, BlendMode)
  - Save round-trip without mutation (structurally correct output)
  - Save after changing Name property
  - Save after changing Visible property
  - Save after changing Opacity property
