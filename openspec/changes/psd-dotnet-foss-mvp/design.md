## Контекст

`Aspose.PSD.FOSS` — это standalone-библиотека для безопасного subset PSD/PSB load/save without rendering.

## Цели

- Чтение основных document properties
- Чтение базовых layer properties
- Изменение `Name`, `IsVisible`, `Opacity`
- Сохранение файла без потери неподдерживаемых данных там, где это возможно
- Byte-for-byte no-mutation round-trip

## Нецели

- Rendering
- Pixel manipulation
- Export to raster formats
- Полный editor для resources/tagged blocks/effects

## Архитектурные решения

### Repository-facing guidance
Структура репозитория должна следовать явному repository-facing принципу:

- основная библиотека в `src/`
- runnable sample projects в `samples/`
- отдельная markdown-документация в `documentation/`

Документация может быть реализована как `.md` файлы вместо отдельного site generator.

### Buffered load from stream
`Load(Stream)` копирует входной поток в internal memory buffer.

Почему:
- не держит caller stream как source of truth
- позволяет безопасно закрывать/освобождать объект
- позволяет восстановить позицию seekable stream после загрузки

### Raw-preserve sections
Следующие части сохраняются как raw bytes там, где это выгоднее и безопаснее:

- Color Mode Data
- Image Resources
- Layer and Mask Information при отсутствии мутаций
- Image Data

### Minimal layer rewrite
При изменении `Name`, `IsVisible`, `Opacity` пересобирается только минимальная часть layer records, а остальные данные сохраняются raw, где возможно.

### PSD/PSB version-aware lengths
Реализация различает PSD (`Version == 1`) и PSB (`Version == 2`) в тех длинах, которые нужны текущему subset:

- outer Layer and Mask section length
- inner layer info length
- per-channel layer data length

## Основные риски

- PSB support пока покрыт только minimal subset и minimal fixture
- malformed files с неправильными length fields требуют дополнительного негативного тестирования
- поддержка non-seekable stream требует отдельного явного тестового покрытия
