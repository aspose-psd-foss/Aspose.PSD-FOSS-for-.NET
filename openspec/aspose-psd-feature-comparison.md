# Сравнение публичного API Aspose.PSD и Aspose.PSD.FOSS

Дата актуальности: 26 августа 2026 года.

## Назначение документа

Этот документ фиксирует текущее состояние совместимости `Aspose.PSD.FOSS` с официальным `Aspose.PSD for .NET` для metadata-oriented PSD/PSB сценариев.

Документ не описывает полный parity с коммерческой библиотекой. FOSS-проект остаётся lightweight structural reader/writer без рендеринга, pixel editing, export pipeline и полной PSD object model.

## Короткий вывод

`Aspose.PSD.FOSS` сейчас ближе к официальному API в тех сценариях, которые покрывают samples: загрузка PSD/PSB, чтение document/layer metadata, изменение простых layer properties и structural save без рендеринга.

Ключевая совместимость для samples достигнута так: FOSS и NuGet sample projects имеют парные `Program.cs`, которые должны оставаться идентичными, а различаться должны только project references/dependencies.

## Что поддерживается в текущем public API

Основная публичная поверхность:

- `Image.Load(string)` и `Image.Load(Stream)`;
- `PsdImage.Load(string)` и `PsdImage.Load(Stream)`;
- `PsdImage.Save(string)` и `PsdImage.Save(Stream)`;
- document properties: `Width`, `Height`, `Bounds`, `Size`, `ChannelsCount`, `BitsPerChannel`, `ColorMode`, `Version`, `Compression`, `Layers`, `ActiveLayer`, `ImageResources`, `GlobalLayerResources`, `GlobalLayerMaskInfo`, `IsFlatten`, `HasTransparencyData`, `GlobalAngle`;
- layer properties: `Name`, read-only `Bounds`, `Width`, `Height`, `Top`, `Left`, `Bottom`, `Right`, `IsVisible`, `Opacity`, `Clipping`, `BlendModeKey`, `ChannelsCount`, `ChannelInformation`, `LayerMaskData`, `LayerBlendingRangesData`;
- compatibility DTO/base types: `ResourceBlock`, `PreservedResourceBlock`, `LayerResource`, `GlobalLayerMaskInfo`, `ChannelInformation`, `LayerMaskData`, `LayerMaskDataShort`, `LayerBlendingRangesData`, `BlendRange`, `StreamContainer`;
- shared primitives/enums/exceptions: `Rectangle`, `RectangleF`, `Point`, `Size`, `ColorModes`, `CompressionMethod`, `BlendMode`, `PsdVersion`, `PsdLoadException`, `PsdSaveException`.

Часть internal inspection state остаётся непубличной намеренно: raw blend mode key string, raw additional layer data, internal resource/color/image-data summaries и PSB/parser diagnostics.

## Важные решения по совместимости

- `Layer.BlendMode` не является public API, потому что такого свойства нет в NuGet `Aspose.PSD` `Layer`.
- `Layer.BlendModeKey` имеет тип `BlendMode`, как в официальной библиотеке, а не raw string key.
- `Layer.Bounds` read-only; geometry editing делается через `Top`, `Left`, `Bottom`, `Right`.
- `Layer.HasAdditionalLayerData` не является public API; opaque additional layer data сохраняется внутри для round-trip.
- `LayerMaskData`, `LayerBlendingRangesData`, `ChannelInformation`, `ImageResources` предоставлены как compatibility surface, но редактирование этих сложных структур в FOSS ограничено или явно выбрасывает `NotSupportedException`.

## Функциональная матрица

| Функциональность | Aspose.PSD for .NET | Aspose.PSD.FOSS | Статус FOSS | Комментарий |
|---|---|---|---|---|
| Загрузка PSD из файла | Да | Да | Поддерживается | `Image.Load`, `PsdImage.Load` |
| Загрузка PSD из потока | Да | Да | Поддерживается | Позиция seekable stream восстанавливается после load |
| Загрузка PSB | Да | Ограниченно | Частично | Поддерживается structural subset |
| Сохранение PSD | Да | Да | Поддерживается | Structural save без rendering |
| Сохранение PSB | Да | Ограниченно | Частично | Для покрытого parser/writer subset |
| Strict no-mutation round-trip | Не основной advertised contract | Да | Поддерживается | Ключевой контракт FOSS |
| Чтение document metadata | Да | Да | Поддерживается | Header, size, channels, version, compression |
| Чтение layer metadata | Да | Да | Поддерживается | Базовый metadata subset |
| Изменение `Layer.Name` | Да | Да | Поддерживается | Сохраняется при `Save` |
| Изменение `Layer.IsVisible` | Да | Да | Поддерживается | Сохраняется при `Save` |
| Изменение `Layer.Opacity` | Да | Да | Поддерживается | Сохраняется при `Save` |
| Изменение `Layer.BlendModeKey` | Да | Да | Поддерживается | Enum-based compatibility API |
| Изменение `Layer.Clipping` | Да | Да | Поддерживается | Сохраняется при `Save` |
| Изменение layer geometry | Да | Да | Поддерживается | Через coordinate setters |
| Создание нового PSD/PSB с нуля | Да | Нет | Не поддерживается | Вне текущего scope |
| Рендеринг PSD и export в raster formats | Да | Нет | Не поддерживается | Явно вне scope |
| Pixel editing | Да | Нет | Не поддерживается | Явно вне scope |
| Text/vector/effects/smart-object model | Да | Нет | Не поддерживается | Raw-preserve там, где возможно |
| Полноценное редактирование image resources/tagged blocks | Да | Нет | Не поддерживается | Public API ограничен compatibility summaries |

## Migration matrix по samples

| Sample group | FOSS project | NuGet project | Contract |
|---|---|---|---|
| Basic | `samples/Aspose.PSD.FOSS.Samples.Basic` | `samples/Aspose.PSD.NuGet.Samples.Basic` | `Program.cs` идентичен |
| Layers | `samples/Aspose.PSD.FOSS.Samples.Layers` | `samples/Aspose.PSD.NuGet.Samples.Layers` | `Program.cs` идентичен |
| Streams | `samples/Aspose.PSD.FOSS.Samples.Streams` | `samples/Aspose.PSD.NuGet.Samples.Streams` | `Program.cs` идентичен |
| StructuralEditing | `samples/Aspose.PSD.FOSS.Samples.StructuralEditing` | `samples/Aspose.PSD.NuGet.Samples.StructuralEditing` | `Program.cs` идентичен |

NuGet sample projects reference the official `Aspose.PSD` package. FOSS sample projects reference the local FOSS project. Sample code parity is the practical compatibility guard for the supported migration scenarios.

## Границы текущего FOSS scope

FOSS поддерживает structural PSD/PSB load/save и metadata editing только там, где writer может сохранить неподдерживаемые данные без semantic rewrite сложных структур.

Не следует трактовать наличие compatibility types как обещание полной Aspose.PSD object model. Например, `ImageResources`, `LayerMaskData`, `LayerBlendingRangesData`, `GlobalLayerResources` и `HasTransparencyData` существуют для совместимости public surface и sample portability, но полноценное редактирование этих областей не входит в текущий FOSS scope.

## Источники в репозитории

- [README.md](/Users/sid/Source/Aspose.PSD-FOSS-for-.NET/README.md)
- [limitations.md](/Users/sid/Source/Aspose.PSD-FOSS-for-.NET/documentation/limitations.md)
- [PsdImage.cs](/Users/sid/Source/Aspose.PSD-FOSS-for-.NET/src/Aspose.PSD.FOSS/PsdImage.cs)
- [Layer.cs](/Users/sid/Source/Aspose.PSD-FOSS-for-.NET/src/Aspose.PSD.FOSS/Layer.cs)
- [samples](/Users/sid/Source/Aspose.PSD-FOSS-for-.NET/samples)
