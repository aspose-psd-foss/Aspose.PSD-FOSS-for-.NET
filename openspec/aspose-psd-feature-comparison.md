# Сравнение публичного API Aspose.PSD и Aspose.PSD.FOSS

Дата актуальности: 18 августа 2026 года.

## Назначение документа

Этот документ отвечает на два вопроса:

1. насколько публичное API `Aspose.PSD.FOSS` похоже на публичное API `Aspose.PSD for .NET`;
2. какие безопасные расширения public API можно добавить в FOSS-проект без ухода в рендеринг, пиксельное редактирование и графический стек общего назначения.

Документ ориентирован на разработчика, который рассматривает `Aspose.PSD.FOSS` как migration target для metadata-oriented сценариев работы с PSD/PSB.

## Легенда статусов

- `Поддерживается` — функция или API уже реализованы и входят в текущий публичный контракт.
- `Частично` — сценарий поддержан, но surface сильно уже или покрывает только subset.
- `Не поддерживается` — публичного API для сценария нет.
- `Кандидат на расширение` — сценарий хорошо ложится в текущую архитектуру FOSS и может быть добавлен без рендеринга.

## Короткий вывод

`Aspose.PSD.FOSS` сейчас не является open-source аналогом полного публичного API `Aspose.PSD for .NET`.

Это отдельная небольшая библиотека для безопасного structural load/save PSD/PSB с акцентом на:

- загрузку файла;
- чтение document metadata;
- чтение базовых layer metadata;
- ограниченное редактирование простых свойств;
- сохранение без рендеринга;
- raw-preserve неподдерживаемых секций;
- strict no-mutation round-trip в поддерживаемых сценариях.

Если смотреть на проект как на migration target именно для таких задач, текущая архитектура FOSS подходит. Основная проблема сейчас не в концепции, а в слишком маленьком публичном API: внутри уже есть больше структурной информации, чем наружу отдано пользователю.

## Что реально экспортируется наружу в FOSS

По фактическим `public` объявлениям типов текущий public API `Aspose.PSD.FOSS` мал:

- `PsdImage`
- `Layer`
- `PsdHeader`
- `PsdLoadException`
- `PsdSaveException`
- `ColorModes`
- `CompressionMethod`
- `BlendMode`

Важно: часть вспомогательных типов содержит `public` члены, но сами типы объявлены как `internal`, поэтому это не часть внешнего контракта NuGet-пакета. Это относится, например, к `BigEndianReader`, `BigEndianWriter`, `ResourceBlock`, `ImageData`, `ColorData`, `LayerMaskData`, `LayerBlendingRangesData`, `PsdWriter`.

## Сводная таблица по функциональности

| Функциональность | Aspose.PSD for .NET | Aspose.PSD.FOSS | Статус FOSS | Комментарий |
|---|---|---|---|---|
| Загрузка PSD из файла | Да | Да | Поддерживается | `PsdImage.Load(string)` |
| Загрузка PSD из потока | Да | Да | Поддерживается | `PsdImage.Load(Stream)` |
| Загрузка PSB | Да | Ограниченно | Частично | Поддерживается только текущий subset PSB |
| Сохранение PSD | Да | Да | Поддерживается | Без рендеринга |
| Сохранение PSB | Да | Ограниченно | Частично | Только для поддерживаемого subset |
| Чтение размеров документа | Да | Да | Поддерживается | `Width`, `Height` |
| Чтение числа каналов | Да | Да | Поддерживается | `Channels` |
| Чтение глубины цвета | Да | Да | Поддерживается | `BitsPerChannel` |
| Чтение color mode | Да | Да | Поддерживается | `ColorMode` |
| Чтение версии PSD/PSB | Да | Да | Поддерживается | `Version` |
| Чтение списка слоёв | Да | Да | Поддерживается | `Layers` |
| Чтение имени слоя | Да | Да | Поддерживается | `Layer.Name` |
| Чтение bounds слоя | Да | Да | Поддерживается | `Layer.Bounds` |
| Чтение видимости слоя | Да | Да | Поддерживается | `Layer.IsVisible` |
| Чтение opacity слоя | Да | Да | Поддерживается | `Layer.Opacity` |
| Чтение blend mode слоя | Да | Да | Поддерживается | Поддерживается базовый набор режимов |
| Изменение имени слоя | Да | Да | Поддерживается | Сохраняется при `Save` |
| Изменение видимости слоя | Да | Да | Поддерживается | Сохраняется при `Save` |
| Изменение opacity слоя | Да | Да | Поддерживается | Сохраняется при `Save` |
| Создание нового PSD/PSB с нуля | Да | Нет | Не поддерживается | Публичного API для создания документа нет |
| Рендеринг PSD | Да | Нет | Не поддерживается | Явно вне scope |
| Растеризация слоёв | Да | Нет | Не поддерживается | Явно вне scope |
| Экспорт в PNG | Да | Нет | Не поддерживается | |
| Экспорт в JPEG | Да | Нет | Не поддерживается | |
| Экспорт в TIFF/GIF/BMP/PDF и др. | Да | Нет | Не поддерживается | |
| Пиксельное редактирование | Да | Нет | Не поддерживается | |
| Graphics/drawing operations | Да | Нет | Не поддерживается | |
| Crop/resize/rotate | Да | Нет | Не поддерживается | |
| Text layers | Да | Нет | Не поддерживается | |
| Fill layers | Да | Нет | Не поддерживается | |
| Adjustment layers | Да | Нет | Не поддерживается | |
| Layer effects | Да | Нет | Не поддерживается | |
| Smart objects | Да | Нет | Не поддерживается | |
| Smart filters | Да | Нет | Не поддерживается | |
| Vector rendering/editing | Да | Нет | Не поддерживается | |
| Mask rendering/editing | Да | Нет | Не поддерживается | Возможен raw-preserve без семантического редактирования |
| Полноценное редактирование image resources | Да | Нет | Не поддерживается | |
| Разбор известных image resources | Да | Ограниченно | Частично | Внутренний semantic subset есть, но почти не вынесен в public API |
| Разбор color mode data | Да | Ограниченно | Частично | Indexed palette и mode-aware parsing есть внутри, но не public |
| Полноценное редактирование tagged blocks | Да | Нет | Не поддерживается | |
| Сохранение неизвестных секций как raw | Не ключевой advertised contract | Да | Поддерживается | Один из основных принципов FOSS |
| Byte-for-byte round-trip без мутаций | Не основной advertised contract | Да | Поддерживается | Ключевой контракт FOSS |
| Загрузка из non-seekable stream | Да | Да | Поддерживается | Покрыто тестами |
| Reject malformed length fields | Да | Да | Поддерживается | Покрыто negative tests |

## Migration matrix по публичному API

### Document-level API

| Aspose.PSD for .NET | FOSS аналог | Статус | Комментарий |
|---|---|---|---|
| `Aspose.PSD.FileFormats.Psd.PsdImage` | `Aspose.PSD.FOSS.PsdImage` | Частичный аналог | Совпадает роль типа, но не модель наследования и не breadth API |
| `PsdImage(Stream)` constructor | Нет | Не поддерживается | В FOSS только `Load(Stream)` |
| `PsdImage(string)` constructor | Нет | Не поддерживается | В FOSS только `Load(string)` |
| `PsdImage(int, int)` | Нет | Не поддерживается | Создание пустого документа отсутствует |
| `PsdImage(... palette/colorMode/channels/compression ...)` | Нет | Не поддерживается | Нет API создания документа |
| `Save(...)` | `Save(string)`, `Save(Stream)` | Частичный аналог | Есть только structural save |

### Document properties

| Aspose.PSD for .NET | FOSS аналог | Статус | Комментарий |
|---|---|---|---|
| `Width` | `Width` | Поддерживается | |
| `Height` | `Height` | Поддерживается | |
| `BitsPerChannel` | `BitsPerChannel` | Поддерживается | |
| `ColorMode` | `ColorMode` | Поддерживается | |
| `Version` | `Version` | Поддерживается | |
| `ChannelsCount` | `Channels` | Частично | Смысл совпадает, имя отличается |
| `Layers` | `Layers` | Частично | В FOSS только read-only массив |
| `ActiveLayer` | Нет | Не поддерживается | |
| `Compression` | Нет public свойства | Не поддерживается | Compression разобран внутренне |
| `Bounds` | Нет | Не поддерживается | |
| `ImageResources` | Нет public свойства | Не поддерживается | Resources разобраны частично, но не экспортированы |
| `XmpData` | Нет | Не поддерживается | |
| `GlobalAngle` | Нет public свойства | Не поддерживается | Внутри known resource parsing уже есть |
| `SmartObjectProvider` | Нет | Не поддерживается | |

### Layer API

| Aspose.PSD for .NET | FOSS аналог | Статус | Комментарий |
|---|---|---|---|
| `Layer` | `Layer` | Частичный аналог | Только metadata subset |
| `TextLayer` | Нет | Не поддерживается | |
| `ShapeLayer` | Нет | Не поддерживается | |
| `LayerGroup` | Нет | Не поддерживается | |
| `ArtboardLayer` | Нет | Не поддерживается | |
| `SectionDividerLayer` | Нет | Не поддерживается | |
| `AddLayer` / `AddTextLayer` / `AddShapeLayer` / `AddLayerGroup` | Нет | Не поддерживается | |

### Layer properties

| Aspose.PSD for .NET `Layer` | FOSS `Layer` | Статус | Комментарий |
|---|---|---|---|
| `Name` | `Name` | Поддерживается | И чтение, и редактирование |
| `Bounds` | `Bounds` | Поддерживается | Пока read-only |
| `IsVisible` | `IsVisible` | Поддерживается | И чтение, и редактирование |
| `Opacity` | `Opacity` | Поддерживается | И чтение, и редактирование |
| `Clipping` | `Clipping` | Частично | Только чтение |
| Blend mode | `BlendMode` | Частично | Только чтение, mapping в enum |
| `DisplayName` | Нет | Не поддерживается | |
| `FillOpacity` | Нет | Не поддерживается | |
| `Flags` | Нет | Не поддерживается | Исходные layer flags сейчас не сохраняются как public state |
| `LayerLock` | Нет | Не поддерживается | |
| `LayerCreationDateTime` | Нет | Не поддерживается | |
| `LayerMaskData` | Нет public свойства | Не поддерживается | Raw subsection уже сохраняется внутри |
| `LayerBlendingRangesData` | Нет public свойства | Не поддерживается | Raw subsection уже сохраняется внутри |
| `Resources` | Нет public свойства | Не поддерживается | Additional layer data пока opaque |
| `ChannelInformation` | Нет public свойства | Не поддерживается | Channel info внутри уже разобран |

## Что FOSS реально умеет сейчас

- загружать PSD и ограниченный subset PSB;
- читать свойства документа из header;
- читать базовые свойства слоёв из Layer and Mask Information;
- изменять только `Name`, `IsVisible`, `Opacity`;
- сохранять PSD/PSB без рендеринга;
- сохранять неизвестные и неподдерживаемые данные как raw там, где это безопасно;
- гарантировать strict no-mutation round-trip для поддерживаемых сценариев.

## Где public API сейчас явно слишком маленький

Если смотреть именно на задачу безопасного metadata-oriented чтения и редактирования, проект уже хранит больше информации, чем отдает наружу.

Основные зазоры:

1. документные свойства ограничены почти только header-полями;
2. слой отдает только 6 свойств, хотя внутри уже есть channel info, raw mask/blending subsections и часть record metadata;
3. image resources частично разбираются, но почти полностью скрыты;
4. color mode data и image data имеют внутреннюю структурную модель, но не отражены в public API;
5. часть простых mutable metadata-полей можно добавить без рендеринга и без перехода к сложной объектной модели.

## Кандидаты на расширение public API

Ниже перечислены свойства и типы, которые выглядят реалистичными для FOSS-проекта, если цель — расширить API для чтения и редактирования PSD/PSB, но остаться в structural/non-rendering области.

### Группа A. Низкий риск, высокая польза

Это лучший первый приоритет. Большая часть этих данных уже либо загружена, либо выводится из уже загруженного состояния.

| Кандидат | Тип | Чтение / запись | Почему реалистично |
|---|---|---|---|
| `PsdImage.IsLargeDocument` или `PsdImage.IsPsb` | `bool` | Чтение | Уже есть `PsdHeader.IsLargeDocument` |
| `PsdImage.Header` | `PsdHeader` | Чтение | Header уже хранится как отдельный тип |
| `PsdImage.LayerCount` | `int` | Чтение | Просто derived property от `Layers.Length` |
| `PsdImage.HasImageResources` | `bool` | Чтение | Ресурсы уже читаются |
| `PsdImage.ResourceCount` | `int` | Чтение | Внутренний массив `_resources` уже есть |
| `PsdImage.HasColorModeData` | `bool` | Чтение | `_colorData` уже есть |
| `PsdImage.HasMergedImageData` | `bool` | Чтение | `_imageData` уже есть |
| `PsdImage.Compression` | `CompressionMethod` | Чтение | Уже разобрано в `ImageData` |
| `PsdImage.ImageDataKind` | `enum` | Чтение | Уже выводится из `ImageDataStructure.Kind` |
| `PsdImage.UsesPrediction` | `bool` | Чтение | Уже выводится из `ImageDataStructure.UsesPrediction` |
| `Layer.Width` | `int` | Чтение | Вычисляется из `Bounds` |
| `Layer.Height` | `int` | Чтение | Вычисляется из `Bounds` |
| `Layer.Top` / `Left` / `Bottom` / `Right` | `int` | Чтение | Уже есть в `Bounds` |
| `Layer.ChannelCount` | `int` | Чтение | `ChannelInfo.Length` уже есть |
| `Layer.HasMaskData` | `bool` | Чтение | Можно выводить из raw mask subsection length |
| `Layer.HasBlendingRangesData` | `bool` | Чтение | Можно выводить из raw blending subsection length |
| `Layer.HasAdditionalLayerData` | `bool` | Чтение | `_additionalLayerData` уже есть |
| `Layer.BlendModeKey` | `string` или `enum` | Чтение | Сейчас key теряется при маппинге в enum, но легко хранить |

### Группа B. Низкий-средний риск, очень полезно для metadata editing

Эти свойства по-прежнему не требуют рендеринга, но уже затрагивают сохранение дополнительных простых полей.

| Кандидат | Тип | Чтение / запись | Комментарий |
|---|---|---|---|
| `Layer.BlendMode` setter | `enum` | Чтение + запись | Внутренний write-path уже умеет кодировать enum обратно в 4-byte key |
| `Layer.Clipping` setter | `byte` или маленький enum | Чтение + запись | Значение уже пишется при save |
| `Layer.Bounds` setter | `Rectangle` | Чтение + запись | Технически слой уже сериализует bounds обратно |
| `Layer.Top` / `Left` / `Bottom` / `Right` setters | `int` | Чтение + запись | Удобнее для API, чем прямой setter `Bounds` |
| `Layer.VisibleInDocument` / raw layer flags model | `bool` / `flags enum` | Чтение | Потребует сохранить исходный byte flags, а не только derived visibility |
| `Layer.NameEncodingLossy` | `bool` | Чтение | Полезно, если имя хранится Pascal ASCII и при save может быть деградация |
| `PsdImage.GlobalAngle` | `int?` | Чтение | Внутренний known resource parsing уже умеет это |
| `PsdImage.IsIccProfileUntagged` | `bool?` | Чтение | Уже умеет `ResourceBlock` |
| `PsdImage.HasIccProfile` | `bool` | Чтение | Следует из parsed resources |
| `PsdImage.IndexedPalette` | read-only palette type | Чтение | Indexed palette уже парсится для indexed documents |

### Группа C. Нужны новые public DTO, но всё ещё в рамках scope

Это уже не просто дополнительные свойства. Здесь нужно вынести наружу отдельные read-only типы. По scope это всё ещё безопасно.

| Кандидат | Предлагаемый тип | Чтение / запись | Почему это разумно |
|---|---|---|---|
| Image resources | `PsdResourceInfo` / `ResourceBlockInfo[]` | Чтение | Parsing known resource ids уже есть |
| Layer channels | `LayerChannelInfo[]` public read-only DTO | Чтение | Channel id и declared data length уже читаются |
| Image data structure | `PsdImageDataInfo` | Чтение | Compression kind, row length field size, row byte counts, ZIP prediction |
| Color mode data info | `PsdColorDataInfo` | Чтение | Kind + raw length + indexed palette summary |
| Layer mask info | `LayerMaskInfo` minimal | Чтение | Даже без semantic parse уже полезно иметь raw length / presence |
| Layer blending ranges info | `LayerBlendingRangesInfo` minimal | Чтение | Аналогично |

### Группа D. Возможны, но уже ближе к границе текущего дизайна

Эти кандидаты могут быть полезны, но несут больше риска для контракта или быстро тащат проект к low-level editor API.

| Кандидат | Риск | Почему стоит отложить |
|---|---|---|
| Public raw bytes для любых секций | Средний | Пользователь начнет зависеть от binary layout, API станет трудно стабилизировать |
| Public mutable resources collection | Средний-высокий | Быстро приводит к partial semantic editing без достаточной валидации |
| Public mutable additional layer data | Высокий | Очень легко сломать round-trip guarantees |
| Полное API tagged blocks / layer resources | Высокий | Это уже отдельный большой пласт PSD object model |

## Что можно реализовать почти без изменения архитектуры

Ниже — наиболее прагматичный shortlist.

### Приоритет 1: добавить сразу

Это свойства, которые дают много пользы и почти не расширяют модель риска:

- `PsdImage.IsLargeDocument` или `PsdImage.IsPsb`
- `PsdImage.LayerCount`
- `PsdImage.ResourceCount`
- `PsdImage.HasImageResources`
- `PsdImage.HasColorModeData`
- `PsdImage.Compression`
- `PsdImage.ImageDataKind`
- `PsdImage.UsesPrediction`
- `Layer.Width`
- `Layer.Height`
- `Layer.Top`
- `Layer.Left`
- `Layer.Bottom`
- `Layer.Right`
- `Layer.ChannelCount`
- `Layer.HasMaskData`
- `Layer.HasBlendingRangesData`
- `Layer.HasAdditionalLayerData`

Это в основном `int`, `bool`, `enum` и derived properties. Они хорошо соответствуют вашей цели расширить simple public API.

### Приоритет 2: следующий полезный шаг

- `PsdImage.GlobalAngle`
- `PsdImage.HasIccProfile`
- `PsdImage.IsIccProfileUntagged`
- `PsdImage.IndexedPalette`
- `Layer.BlendMode` setter
- `Layer.Clipping` setter
- `Layer.Bounds` setter или набор `Top/Left/Bottom/Right` setters

Здесь уже появляется реальное расширение editing API, но оно всё ещё structural и не требует рендеринга.

### Приоритет 3: read-only DTO для углубленного чтения

- public read-only resources list
- public read-only channel info list
- public read-only image data info
- public read-only color mode data info

Это делает FOSS заметно полезнее как library для inspection и migration сценариев.

## Что сейчас мешает расширять API ещё сильнее

Есть несколько архитектурных ограничений, которые видны по текущему коду:

1. часть исходных полей layer record при чтении нормализуется слишком рано;
2. некоторые данные парсятся только в derived form, а исходное значение не сохраняется как отдельное public-ready state;
3. resources и color mode data уже разбираются, но не имеют стабильных public DTO;
4. current `Layer` model intentionally small, поэтому любое расширение нужно делать так, чтобы не превратить его в partially implemented clone коммерческого `Layer`.

Особенно заметный пример — layer flags. Сейчас библиотека хранит только derived `IsVisible`, но не сохраняет полный исходный flags byte как самостоятельную модель. Если вы захотите расширять simple layer booleans, сначала лучше сохранить raw flags как часть внутреннего состояния.

## Рекомендация по направлению развития public API

Если цель — не строить рендеринг и не копировать весь коммерческий продукт, но сделать FOSS полезнее для PSD/PSB metadata editing, то лучший путь такой:

1. расширять API сначала простыми `int`, `bool`, `string`, `enum` свойствами;
2. затем добавить несколько read-only DTO для inspection;
3. mutable API добавлять только там, где save-path уже прозрачен и не требует semantic rewriting сложных структур;
4. не выносить сразу raw byte-oriented low-level editor API в public surface.

Иными словами:

- сначала больше простых свойств;
- потом больше read-only структур;
- и только потом selective metadata editing beyond `Name` / `IsVisible` / `Opacity`.

## Итог

Если смотреть на `Aspose.PSD.FOSS` как на migration target для metadata-oriented PSD/PSB работы, проект уже находится в правильной области, но public API действительно слишком маленький.

Лучшие кандидаты на расширение — это не text layers, не export и не effects. Лучшие кандидаты — это дополнительные простые свойства документа и слоя, а также read-only доступ к уже разобранным структурным данным:

- compression;
- resource presence и resource summary;
- indexed palette / color mode data summary;
- image data structure summary;
- channel metadata;
- simple geometric and boolean layer properties;
- ещё несколько безопасных mutable metadata-полей.

Это даст FOSS заметно больше прикладной ценности, не ломая текущую идею библиотеки как structural non-rendering PSD/PSB editor.

## Источники

- Официальная документация Aspose.PSD for .NET: https://docs.aspose.com/psd/net/
- Getting Started Aspose.PSD for .NET: https://docs.aspose.com/psd/net/getting-started/
- API reference root Aspose.PSD for .NET: https://reference.aspose.com/psd/net/
- API reference `Aspose.PSD.FileFormats.Psd`: https://reference.aspose.com/psd/net/aspose.psd.fileformats.psd/
- API reference `PsdImage`: https://reference.aspose.com/psd/net/aspose.psd.fileformats.psd/psdimage/
- API reference `Layers`: https://reference.aspose.com/psd/net/aspose.psd.fileformats.psd.layers/
- Текущий scope FOSS-библиотеки: [README.md](/Users/sid/Source/Aspose.PSD-FOSS-for-.NET/README.md)
- Ограничения FOSS-библиотеки: [limitations.md](/Users/sid/Source/Aspose.PSD-FOSS-for-.NET/documentation/limitations.md)
- Текущий public API FOSS: [PsdImage.cs](/Users/sid/Source/Aspose.PSD-FOSS-for-.NET/src/Aspose.PSD.FOSS/PsdImage.cs), [Layer.cs](/Users/sid/Source/Aspose.PSD-FOSS-for-.NET/src/Aspose.PSD.FOSS/Layer.cs), [PsdHeader.cs](/Users/sid/Source/Aspose.PSD-FOSS-for-.NET/src/Aspose.PSD.FOSS/PsdHeader.cs), [Exceptions.cs](/Users/sid/Source/Aspose.PSD-FOSS-for-.NET/src/Aspose.PSD.FOSS/Exceptions.cs)
