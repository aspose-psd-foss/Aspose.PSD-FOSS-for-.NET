## Контекст

`Aspose.PSD.FOSS` — это standalone-библиотека для безопасного ограниченного PSD/PSB load/save без rendering.

## Цели

- Чтение основных document properties
- Чтение базовых layer properties
- Чтение расширенных structural metadata без rendering
- Изменение `Name`, `IsVisible`, `Opacity`
- Изменение `BlendMode`, `Clipping` и layer geometry в рамках structural save path
- Сохранение файла без потери неподдерживаемых данных там, где это возможно
- Byte-for-byte no-mutation round-trip

## Нецели

- Rendering
- Pixel manipulation
- Export to raster formats
- Полный editor для resources/tagged blocks/effects

## Архитектурные решения

### Правила для структуры репозитория
Структура репозитория должна следовать явному repository-facing принципу:

- основная библиотека в `src/`
- runnable sample projects в `samples/`
- отдельная markdown-документация в `documentation/`

Документация может быть реализована как `.md` файлы вместо отдельного site generator.

### Buffered load из stream
`Load(Stream)` копирует входной поток в internal memory buffer.

Почему:
- не держит caller stream как source of truth
- позволяет безопасно закрывать/освобождать объект
- позволяет восстановить позицию seekable stream после загрузки

### Raw-preserve секции
Следующие части сохраняются как raw bytes там, где это выгоднее и безопаснее:

- Color Mode Data
- Image Resources
- Layer and Mask Information при отсутствии мутаций
- Image Data

### Минимальная пересборка layer records
При изменении поддерживаемых metadata-полей пересобирается только минимальная часть layer records, а остальные данные сохраняются raw, где возможно.

На первом расширенном этапе поддерживаемый mutable subset включает:

- `Name`
- `IsVisible`
- `Opacity`
- `BlendMode`
- `Clipping`
- layer geometry (`Bounds` и/или coordinate properties)

### PSD/PSB version-aware lengths
Реализация различает PSD (`Version == 1`) и PSB (`Version == 2`) в тех длинах, которые нужны текущему поддерживаемому subset:

- outer Layer and Mask section length
- inner layer info length
- per-channel layer data length

## Расширение public API без рендеринга

### Принцип расширения
Public API расширяется в пределах structural PSD/PSB editing и inspection. Новые возможности не должны:

- вводить pixel-level editing;
- требовать rasterization или rendering;
- превращать библиотеку в partial clone общего graphics/image framework.

### Категории расширения

#### Группа A: simple public properties
Новые simple properties должны по возможности быть derived from existing parsed state или отражать уже загруженные значения:

- document-level `bool` / `int` / `enum` properties;
- layer-level `bool` / `int` / `string` / `enum` properties.

#### Группа B: selective metadata editing
Новые editable metadata fields допускаются только там, где save path уже может прозрачно сериализовать их без semantic rewrite сложных структур.

Примеры:

- `Layer.BlendMode`
- `Layer.Clipping`
- layer geometry

#### Группа C: read-only structural DTO
Для already-parsed structural data допускается отдельный read-only public API через небольшие explicit DTO, а не через raw mutable byte arrays.

Примеры:

- summary по image resources
- summary по color mode data
- summary по image data structure
- summary по layer channels
- summary по layer mask / blending ranges

### Сохранение исходного state до normalizации
Если дальнейшее расширение public API требует исходного raw state, реализация должна хранить этот state до нормализации.

Особенно это относится к:

- raw layer flags;
- original blend mode key;
- parsed resource metadata;
- parsed image data structure metadata;
- parsed color mode data metadata.

### Правила стабильности public API
Расширение API должно следовать этим правилам:

- простые properties предпочтительнее широких mutable containers;
- read-only DTO предпочтительнее raw public byte buffers;
- неподдерживаемые секции могут оставаться raw-preserved внутри, даже если наружу отдается только summary;
- XML documentation должна явно описывать ограничения для частично интерпретируемых metadata.

## Основные риски

- PSB support пока покрыт только минимальным subset и minimal fixture
- malformed files с неправильными length fields требуют дополнительного негативного тестирования
- поддержка non-seekable stream требует отдельного явного тестового покрытия
