## 1. Инфраструктура проекта

- [x] 1.1 Создать solution `Aspose.PSD-FOSS-for-.NET.sln`
- [x] 1.2 Создать project `src/Aspose.PSD.FOSS/Aspose.PSD.FOSS.csproj`
- [x] 1.3 Создать namespace `Aspose.PSD.FOSS`
- [x] 1.4 Настроить target framework `net10.0`
- [x] 1.5 Подключить `Aspose.PSD.FOSS.Test` к solution
- [x] 1.6 Организовать базовую структуру проекта для standalone PSD reader/writer

## 2. Базовые типы и enum'ы

- [x] 2.1 Реализовать enum `ColorModes` для базовых режимов PSD
- [x] 2.2 Реализовать enum `CompressionMethod`
- [x] 2.3 Реализовать class `PsdHeader`
- [x] 2.4 Выделить отдельный тип `PsdVersion` вместо хранения только числового значения версии
- [x] 2.5 Реализовать enum `BlendMode` (`Normal`, `Multiply`, `Screen`, `Overlay` и базовый поддерживаемый набор)
- [x] 2.6 **ВАЖНО: Зафиксировать понимание PSD file structure до реализации**
  - Header: 26 bytes
  - Color Mode Data: length field + raw payload
  - Image Resources: length field + resource blocks
  - Layer and Mask Information: PSD/PSB length field + layer info + global mask / tail data
  - Image Data: compression (2 bytes) + raw pixel payload

## 3. Загрузка файла — заголовок PSD/PSB

- [x] 3.1 Реализовать валидацию сигнатуры файла `8BPS` и различать PSD/PSB по version field (`1` для PSD, `2` для PSB)
- [x] 3.2 Реализовать метод `PsdHeader.Load` (парсинг 26-байтного заголовка)
- [x] 3.3 Реализовать чтение ширины и высоты (big-endian целые числа)
- [x] 3.4 Реализовать чтение `channels`, `bitsDepth`, `colorMode`
- [x] 3.5 Реализовать определение версии документа (PSD = 1, PSB = 2)
- [x] 3.6 Добавить обработку ошибок для некорректной сигнатуры и неподдерживаемой версии

## 4. Загрузка файла — цветовые данные

- [x] 4.1 Реализовать отдельный класс `ColorData` (или эквивалентный выделенный тип для секции color mode data)
- [x] 4.2 Реализовать выделенный метод `ColorData.Load`
- [x] 4.3 Добавить явную обработку RGB color data beyond raw-preserve
- [x] 4.4 Добавить явную обработку indexed palette, если она присутствует
- [x] 4.5 Добавить явную обработку CMYK-related color data, если она присутствует

## 5. Загрузка файла — информация о слоях

- [x] 5.1 Реализовать класс `Layer` с базовыми свойствами (`Name`, `Bounds`, `IsVisible`, `Opacity`, `BlendMode`)
- [x] 5.2 Реализовать отдельный класс `LayerMaskData`
- [x] 5.3 Реализовать отдельный класс `LayerBlendingRangesData`
- [x] 5.4 Реализовать парсинг layer record header и фиксированной части layer record
- [x] 5.5 Реализовать чтение имени слоя (Pascal string с PSD padding)
- [x] 5.6 Реализовать чтение позиции слоя (`top`, `left`, `bottom`, `right`)
- [x] 5.7 Реализовать парсинг layer flags из корректного байта flags и вычисление `Visible`
- [x] 5.8 Реализовать чтение `opacity` и `clipping` в правильном порядке после blend mode key
- [x] 5.9 Реализовать парсинг ключа режима смешивания (4 bytes → `BlendMode`)
- [x] 5.10 Реализовать парсинг channel information (`channel ID` + `data length` для каждого канала, с учётом PSD/PSB-размеров)
- [x] 5.11 Обработать `layer mask` и `blending ranges` как выделенные raw subsections с сохранением границ
- [x] 5.12 Сохранять layer channel data для round-trip
- [x] 5.13 Сохранять extra data для round-trip

## 6. Загрузка файла — image resources

- [x] 6.1 Реализовать отдельный класс `ResourceBlock` для заголовков и данных ресурсов
- [x] 6.2 Реализовать выделенный метод `ResourceBlock.Load`
- [x] 6.3 **ВАЖНО: верифицировать `8BIM` signature при чтении resource blocks**
- [x] 6.4 Реализовать парсинг секции image resources с raw-preserve для неизвестных ресурсов
- [x] 6.5 Добавить обработку известных ресурсов только там, где это действительно требуется текущему scope продукта
- [x] 6.6 **ВАЖНО: сохранять resource data целиком для round-trip**

## 7. Загрузка файла — данные изображения

- [x] 7.1 Реализовать отдельный класс `ImageData`
- [x] 7.2 Реализовать выделенный метод `ImageData.Load`
- [x] 7.3 Добавить структурное понимание `raw`, `RLE`, `ZIP` compression без рендеринга
- [x] 7.4 Хранить image data как raw payload с сохранением compression field для save
- [x] 7.5 **ВАЖНО: не читать за пределы image data section**

## 8. Сохранение файла

- [x] 8.1 Реализовать отдельный класс `PsdWriter`
- [x] 8.2 Реализовать метод `PsdHeader.Save`
- [x] 8.3 Реализовать метод `ColorData.Save`
- [x] 8.4 Реализовать запись layer record с обновлением заголовка при изменении поддерживаемых свойств
- [x] 8.5 Реализовать корректный порядок секций PSD (`header → color → resources → layers → image data`)
- [x] 8.6 Реализовать запись секции resources с сохранением неизвестных ресурсов и корректной сигнатурой `8BIM`
- [x] 8.7 **ВАЖНО: сохранять layer channel data и extra data для round-trip**
- [x] 8.8 Реализовать запись секции image data с сохранением raw пиксельных данных

## 9. Главный класс `PsdImage`

- [x] 9.1 Реализовать `PsdImage.Load(string filePath)` — `public static`
- [x] 9.2 Реализовать `PsdImage.Load(Stream stream)` — `public static`
- [x] 9.3 Реализовать `PsdImage.Save(string filePath)` — `instance method`
- [x] 9.4 Реализовать `PsdImage.Save(Stream stream)` — `instance method`
- [x] 9.5 Реализовать свойства документа (`Width`, `Height`, `Channels`, `BitsPerChannel`, `ColorMode`, `Version`)
- [x] 9.6 Реализовать свойство `Layers` (`Layer[]`)
- [x] 9.7 Реализовать обработку исключений (`PsdLoadException`, `PsdSaveException`)
- [x] 9.8 Реализовать `IDisposable`

## 10. Тестирование

- [x] 10.1 Создать test project `Aspose.PSD.FOSS.Test` как реальный NUnit test project
- [x] 10.2 Тест загрузки из file path
- [x] 10.3 Тест round-trip (`load → save → load`) с проверкой идентичности байтов для no-mutation сценария
- [x] 10.4 Тест чтения свойств документа (`Width`, `Height`, `Channels`, `BitsPerChannel`, `ColorMode`, `Version`)
- [x] 10.5 Тест чтения свойств слоёв (`Name`, `Bounds`, `IsVisible`, `Opacity`, `BlendMode`)
- [x] 10.6 Тест изменения свойств слоя (`Name`, `Visible`, `Opacity`) и сохранения
- [x] 10.7 Тест с PSD файлом без слоёв
- [x] 10.8 Тест доступа к слоям по индексу
- [x] 10.9 Тест некорректных аргументов (`null stream`, отсутствующий file path)
- [x] 10.10 Тест структурной корректности PSD/PSB файлов через reload и strict round-trip сценарии

## 11. Документация

- [x] 11.1 Написать API reference документацию
- [x] 11.2 Написать примеры использования (`README`)
- [x] 11.3 Документировать поддерживаемые функции PSD/PSB
- [x] 11.4 Документировать неподдерживаемые функции и ограничения
- [x] 11.5 Документировать типы исключений и условия их выброса
- [x] 11.6 Добавить markdown-документацию в отдельную папку `documentation/`
- [x] 11.7 Добавить index-документ для навигации по markdown-документации
- [x] 11.8 Добавить отдельный getting started guide
- [x] 11.9 Добавить developer guide для базовых PSD операций

## 12. Тестовая инфраструктура и обязательные требования

### 12.1 Расположение тестовых данных

- [x] 12.1.1 Все PSD файлы для тестов должны находиться в `src/Aspose.PSD.FOSS.Test/testdata/`
- [x] 12.1.2 Тесты должны ссылаться на файлы относительно `TestContext.CurrentContext.TestDirectory`
- [x] 12.1.3 Test project должен содержать `test.psd` в `testdata/`

### 12.2 Тесты строгого round-trip без мутации

- [x] 12.2.1 Создать test strict no-mutation round-trip с byte-for-byte сравнением
- [x] 12.2.2 Тест должен загрузить файл, сохранить в другой файл и сравнить байты
- [x] 12.2.3 Запретить ослабление contract до просто "валидный файл"
- [x] 12.2.4 Сохранение без мутации должно производить идентичный файл byte-for-byte

### 12.3 Поведение save path для no-mutation и minimal mutation

- [x] 12.3.1 Реализовать save path, который при отсутствии мутаций сохраняет raw sections byte-for-byte
- [x] 12.3.2 Реализовать минимальную пересборку layer records только после изменения `Name`, `Visible`, `Opacity`
- [x] 12.3.3 Не переписывать Image Resources при отсутствии прямых изменений ресурсов
- [x] 12.3.4 Не переписывать Layer and Mask Information целиком, если можно сохранить raw для no-mutation сценария

### 12.4 Требования к тестовому проекту

- [x] 12.4.1 `Aspose.PSD.FOSS.Test` должен быть именно тестовым project, а не demo/console project
- [x] 12.4.2 Test project должен использовать NUnit
- [x] 12.4.3 Новые acceptance tests должны создаваться в `Aspose.PSD.FOSS.Test`

### 12.5 Требования к XML documentation

- [x] 12.5.1 Все public классы должны иметь XML summary
- [x] 12.5.2 Все public методы должны иметь XML summary
- [x] 12.5.3 Все public свойства должны иметь XML summary
- [x] 12.5.4 Все public поля и константы должны иметь XML summary
- [x] 12.5.5 Все internal/private сущности тоже должны иметь summary там, где это требуется внутренним стандартом проекта

## 13. Что ещё остаётся нереализованным, но не должно теряться из артефактов

- [x] 13.1 Добавить fixture с PSB, содержащим реальные layer records, а не только minimal no-layer case
- [x] 13.2 Добавить негативные тесты для corrupted length fields
- [x] 13.3 Добавить тесты для non-seekable stream
- [x] 13.4 Явно определить, нужен ли выделенный тип для color mode data вместо raw byte preservation
- [x] 13.5 Явно определить, нужен ли выделенный тип для image data вместо raw byte preservation

## 14. Samples и repository UX

- [x] 14.1 Добавить папку `samples/` в корень репозитория
- [x] 14.2 Добавить runnable sample project для базовой загрузки и чтения свойств документа
- [x] 14.3 Добавить runnable sample project для чтения и изменения layer metadata
- [x] 14.4 Добавить runnable sample project для stream-based load/save
- [x] 14.5 Подключить sample projects к solution
- [x] 14.6 Обновить `README.md`, чтобы он ссылался на `samples/` и markdown documentation

## 15. Расширение public API без рендеринга

- [x] 15.1 Подготовить internal state preservation для расширенного public API: сохранять raw layer flags, original blend mode key и stable parsed summaries вместо потери исходного structural state при ранней нормализации
- [x] 15.2 Добавить document-level simple read-only properties `IsLargeDocument`, `IsPsb`, `Header`, `LayerCount`
- [x] 15.3 Добавить document-level simple read-only properties `HasImageResources`, `ResourceCount`, `HasColorModeData`, `HasMergedImageData`
- [x] 15.4 Добавить document-level simple read-only properties `Compression`, `ImageDataKind`, `UsesPrediction`
- [x] 15.5 Добавить layer-level simple read-only properties `Width`, `Height`, `Top`, `Left`, `Bottom`, `Right`
- [x] 15.6 Добавить layer-level simple read-only properties `ChannelCount`, `HasMaskData`, `HasBlendingRangesData`, `HasAdditionalLayerData`, `BlendModeKey`
- [x] 15.7 Добавить read-only DTO `PsdResourceInfo` и вынести наружу read-only collection document resources
- [x] 15.8 Добавить document-level inspection properties `GlobalAngle`, `HasIccProfile`, `IsIccProfileUntagged`
- [x] 15.9 Добавить read-only DTO для color mode data и summary по indexed palette
- [x] 15.10 Добавить document-level inspection properties `ColorDataInfo` и `IndexedPalette`
- [x] 15.11 Добавить read-only DTO для summary по структуре image data
- [x] 15.12 Добавить document-level inspection property `ImageDataInfo`
- [x] 15.13 Добавить public read-only layer channel DTO и property `Channels`
- [x] 15.14 Добавить public read-only DTO для summary по layer mask/blending ranges и properties `MaskInfo`, `BlendingRangesInfo`
- [x] 15.15 Расширить mutable layer metadata editing: добавить setter для `BlendMode`
- [x] 15.16 Расширить mutable layer metadata editing: добавить setter для `Clipping`
- [x] 15.17 Расширить mutable layer metadata editing: добавить setter для `Bounds`
- [x] 15.18 Расширить mutable layer metadata editing: добавить coordinate setters `Top`, `Left`, `Bottom`, `Right` с согласованным обновлением `Bounds`
- [x] 15.19 Добавить acceptance tests для internal-state-sensitive round-trip сценариев после расширения metadata API
- [x] 15.20 Добавить acceptance tests для новых document-level simple properties
- [x] 15.21 Добавить acceptance tests для новых layer-level simple properties
- [x] 15.22 Добавить acceptance tests для read-only DTO document inspection API
- [x] 15.23 Добавить acceptance tests для read-only DTO layer inspection API
- [x] 15.24 Добавить acceptance tests для новых mutable metadata fields `BlendMode`, `Clipping`, `Bounds` и coordinate properties
- [x] 15.25 Обновить README для расширенного inspection/editing subset в public API
- [x] 15.26 Обновить markdown documentation и samples для новых inspection/editing возможностей
