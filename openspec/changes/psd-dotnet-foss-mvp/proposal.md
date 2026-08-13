## Почему

Существующая библиотека Aspose.PSD предоставляет полнофункциональную поддержку формата PSD, но она является коммерческой и закрытой. Создание лёгкой Free Open Source библиотеки .NET с минимальным функционалом PSD позволит разработчикам:

- Работать с PSD-файлами в open-source проектах без лицензионных ограничений
- Читать и изменять основные свойства PSD без ресурсоемкого рендеринга
- Интегрировать базовую поддержку PSD в инструменты CI/CD, конвертеры, валидаторы

**What problem does this solve?** Нет лёгкой, полностью open-source .NET библиотеки для базовой работы с PSD-файлами. Решение для создания FOSS-версии Aspose.PSD было бы избыточным и юридически сложным. Наш подход: написать минимальную реализацию только с теми возможностями, которые нужны для MVP.

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

**Изменения API:**
- Новый public API (см. design.md)
- Backward compatibility не требуется (новая библиотека)

**Зависимости:**
- Минимальные зависимости (System.IO, System.Collections.Generic, System.Drawing.Common для Rectangle/Color)
- Без внешних image processing библиотек (рендеринг исключён из MVP)

**Системы:**
- Standalone библиотека, не требует интеграции с существующими системами
- Может быть использована в CI/CD, конвертерах, инспекторах PSD
