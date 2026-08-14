## 1. Настройка проекта

- [x] 1.1 Создать новый проект .NET class library
- [x] 1.2 Настроить структуру проекта (namespaces, folder layout)
- [x] 1.3 Настроить метаданные NuGet пакета (имя, версия, автор)
- [x] 1.4 Добавить README.md с базовыми примерами использования
- [x] 1.5 Настроить CI/CD pipeline для тестирования и упаковки

### 1.6 Переделка тестового project (Aspose.PSD.FOSS.Test)

- [x] 1.6.1 Изменить OutputType с Exe на Library
- [x] 1.6.2 Добавить пакеты NUnit (NUnit, NUnit3TestAdapter, Microsoft.NET.Test.Sdk)
- [x] 1.6.3 Удалить Program.cs (demo-oriented entry point)
- [x] 1.6.4 Добавить PsdImageTests.cs с MVP тестами
- [x] 1.6.5 Сохранить test.psd как test resource в project
- [x] 1.6.6 Подключить project к solution как test project

## 2. Базовые структуры данных

## 2. Базовые структуры данных

## 2. Базовые структуры данных

- [x] 2.2 Реализовать enum ColorModes (Rgb, CMYK, Grayscale, Indexed, Duotone, Lab)
- [x] 2.3 Реализовать enum CompressionMethod (Raw, RLE, ZIP, RZ)
- [x] 2.4 Реализовать класс PsdVersion (internal)
- [ ] 2.5 Реализовать enum BlendMode (Normal, Multiply, Screen, Overlay и т.д.)
- [ ] 2.6 **ВАЖНО: Понимание PSD file structure przed реализацией**
  - Header: 26 bytes
  - Color Mode Data: length field (4 bytes) + data
  - Image Resources: length field (4 bytes) + resource blocks (each: 8BIM + id + name + data)
  - Layer and Mask Information: length field (4 bytes) + layer records + extra data
  - Image Data: compression (2 bytes) + channel data

## 3. Загрузка файла — заголовок PSD

- [ ] 3.1 Реализовать валидацию сигнатуры файла (8BPS для PSD, 8BIM для PSB)
- [ ] 3.2 Реализовать метод PsdHeader.Load (парсинг 26-байтного заголовка)
- [ ] 3.3 Реализовать чтение ширины и высоты (big-endian целые числа)
- [ ] 3.4 Реализовать чтение channels, bitsDepth, colorMode
- [ ] 3.5 Реализовать определение версии (PSD v1-6, PSB >6)
- [ ] 3.6 Добавить обработку ошибок для некорректных сигнатур

## 4. Загрузка файла — цветовые данные

- [ ] 4.1 Реализовать класс ColorData (информация о палитре)
- [ ] 4.2 Реализовать метод ColorData.Load (чтение секции данных цветового режима)
- [ ] 4.3 Обработка RGB цветового профиля
- [ ] 4.4 Обработка индексированной цветовой палитры (если присутствует)
- [ ] 4.5 Обработка CMYK цветового профиля (если присутствует)

## 5. Загрузка файла — информация о слоях

- [ ] 5.1 Реализовать класс Layer с базовыми свойствами (Name, Bounds, IsVisible, Opacity, BlendMode)
- [ ] 5.2 Реализовать класс LayerMaskData (layer mask)
- [ ] 5.3 Реализовать класс LayerBlendingRangesData (blending ranges)
- [ ] 5.4 Реализовать парсинг заголовка слоя (34 байта для layer record)
- [ ] 5.5 Реализовать чтение имени слоя (Pascal string с padding)
- [ ] 5.6 Реализовать чтение позиции слоя (top, left, bottom, right)
- [ ] 5.7 Реализовать парсинг флагов слоя (видимость, protected transparency и т.д.) **ВАЖНО: флаги - это 4 байта, не 1**
- [ ] 5.8 Реализовать чтение opacity и clipping слоя **ВАЖНО: правильный порядок полей после blend mode key**
- [ ] 5.9 Реализовать парсинг ключа режима смешивания (4 байта)
- [ ] 5.10 Реализовать парсинг channel information (offsets и длины данных каналов) **ВАЖНО: читать channel ID и data length для каждого канала**
- [ ] 5.11 Обработка layer mask и blending ranges (если присутствуют)
- [ ] 5.12 Сохранение channel data для round-trip
- [ ] 5.13 Сохранение extra data для round-trip

## 6. Загрузка файла — image resources

- [ ] 6.1 Реализовать класс ResourceBlock для заголовков ресурсов
- [ ] 6.2 Реализовать метод ResourceBlock.Load
- [ ] 6.3 **ВАЖНО: верифицировать 8BIM signature при чтении resource blocks**
- [ ] 6.4 Парсинг секции image resources (пропуск неизвестных ресурсов как raw bytes)
- [ ] 6.5 Обработка известных ресурсов (Опционально: реализовать только необходимые)
- [ ] 6.6 **ВАЖНО: сохранять resource data целиком для round-trip**

## 7. Загрузка файла — данные изображения

- [ ] 7.1 Реализовать класс ImageData (секция пиксельных данных)
- [ ] 7.2 Реализовать метод ImageData.Load (чтение, но не парсинг пиксельных данных)
- [ ] 7.3 Поддержка raw, RLE, ZIP сжатия (парсинг структуры только)
- [ ] 7.4 Хранение offset и длины пиксельных данных для сохранения
- [ ] 7.5 **ВАЖНО: не читать за пределы image data section**

## 8. Сохранение файла

- [ ] 8.1 Реализовать класс PsdWriter
- [ ] 8.2 Реализовать метод PsdHeader.Save
- [ ] 8.3 Реализовать метод ColorData.Save
- [ ] 8.4 Реализовать метод Layer.Save (обновление заголовка при изменении свойств)
- [ ] 8.5 Реализовать порядок секций PSD (header → color → resources → layers → image data)
- [ ] 8.6 Реализовать запись секции resources (сохранение неизвестных ресурсов) **ВАЖНО: записывать 8BIM signature**
- [ ] 8.7 **ВАЖНО: сохранение layer с channel data и extra data для round-trip**
- [ ] 8.8 Реализовать запись секции image data (сохранение raw пиксельных данных)

## 9. Главный класс PsdImage

- [ ] 9.1 Реализовать PsdImage.Load(string filePath) — public static
- [ ] 9.2 Реализовать PsdImage.Load(Stream stream) — public static
- [ ] 9.3 Реализовать PsdImage.Save(string filePath) — instance method
- [ ] 9.4 Реализовать PsdImage.Save(Stream stream) — instance method
- [ ] 9.5 Реализовать свойства документа (Width, Height, BitsPerChannel, ColorMode, Version)
- [ ] 9.6 Реализовать свойство Layers (Layer[] массив)
- [ ] 9.7 Реализовать обработку исключений (PsdLoadException, PsdSaveException)
- [ ] 9.8 Реализовать IDisposable для корректной очистки ресурсов

## 10. Тестирование

- [x] 10.1 Создать тестовый project (Aspose.PSD.FOSS.Test) как real test project с NUnit
- [x] 10.2 Тест загрузки из file path
- [x] 10.3 Тест round-trip (load → save → load) с проверкой идентичности байтов
- [x] 10.4 Тест чтения свойств документа (Width, Height, Channels, BitsPerChannel, ColorMode, Version)
- [x] 10.5 Тест чтения свойств слоёв (Name, Bounds, IsVisible, Opacity, BlendMode)
- [x] 10.6 Тест изменения свойств слоя (Name, Visible, Opacity) и сохранения
- [ ] 10.7 Тест с PSD файлом без слоёв
- [ ] 10.8 Тест доступа к слоям по индексу
- [ ] 10.9 Тест некорректных аргументов (null stream, некорректный путь)
- [ ] 10.10 Тест структурной корректности PSD файлов

## 11. Документация

- [x] 11.1 Написать API reference документацию
- [x] 11.2 Написать примеры использования (README)
- [x] 11.3 Документировать поддерживаемые функции PSD
- [x] 11.4 Документировать неподдерживаемые функции и ограничения
- [x] 11.5 Документировать типы исключений и условия их выброса

## 12. Тестовая инфраструктура и обязательные требования

### 12.1 Расположение тестовых данных

- [x] 12.1.1 Все PSD файлы для тестов должны находиться в `src/Aspose.PSD.FOSS.Test/testdata/`
- [x] 12.1.2 Тесты должны ссылаться на файлы относительно TestContext.CurrentContext.TestDirectory
- [x] 12.1.3 Тест проект должен содержать файл test.psd в testdata/ подпапке

### 12.2 Тесты строгого round-trip без мутации

- [ ] 12.2.1 Создать тест strict no-mutation round-trip с byte-for-byte сравнением
- [ ] 12.2.2 Тест должен загрузить файл, сохранить в другой файл, и сравнить байты
- [ ] 12.2.3 Запретить ослабление contract до просто "валидный файл"
- [ ] 12.2.4 Сохранение без мутации должно производить идентичный файл byte-for-byte

### 12.3 XML документация

- [x] 12.3.1 Все публичные классы, методы, свойства и поля должны иметь XML summary
- [x] 12.3.2 Документация должна быть краткой, но понятной
- [x] 12.3.3 XML documentation включена в project файл
