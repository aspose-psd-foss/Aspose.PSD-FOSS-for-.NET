## Почему

Нужна лёгкая FOSS `.NET` библиотека для базовой работы с PSD/PSB без рендеринга: загрузить файл, прочитать ключевые свойства документа и слоёв, изменить небольшой безопасный набор полей и сохранить файл без потери остального содержимого.

## Что меняется

Создаётся библиотека `Aspose.PSD.FOSS` с минимальным public API в стиле Aspose.PSD:

- `PsdImage.Load(string)`
- `PsdImage.Load(Stream)`
- `image.Width`
- `image.Height`
- `image.Channels`
- `image.BitsPerChannel`
- `image.ColorMode`
- `image.Version`
- `image.Layers`
- `layer.Name`
- `layer.Bounds`
- `layer.IsVisible`
- `layer.Opacity`
- `layer.BlendMode`
- `image.Save(string)`
- `image.Save(Stream)`

Public API расширяется в пределах non-rendering scope за счёт дополнительных возможностей для metadata-oriented сценариев:

- дополнительные простые document properties (`bool`, `int`, `enum`)
- дополнительные простые layer properties (`bool`, `int`, `string`, `enum`)
- read-only inspection DTO для image resources, image data, color mode data, layer channels, mask/blending metadata
- безопасное расширение layer metadata editing для `BlendMode`, `Clipping` и layer geometry

## Scope продукта

- Загрузка PSD
- Базовая загрузка PSB
- Чтение document properties из header
- Чтение layer metadata из Layer and Mask Information
- Чтение дополнительных structural metadata без рендеринга
- Изменение `Name`, `IsVisible`, `Opacity`
- Изменение `BlendMode`, `Clipping` и layer geometry в рамках structural save path
- Сохранение без рендеринга
- Byte-for-byte round-trip без мутаций
- Raw-preserve для неподдерживаемых/неизвестных данных, где это возможно

## Вне scope

- Rendering
- Pixel editing
- Export to PNG/JPEG/etc.
- Полноценное редактирование image resources
- Полноценное редактирование tagged blocks
- Effects, text, vector, smart filters, adjustments

## Тестирование

`Aspose.PSD.FOSS.Test` — это реальный NUnit test project.

- PSD fixtures хранятся в `src/Aspose.PSD.FOSS.Test/testdata/`
- acceptance tests проверяют observable behavior
- отдельные tests проверяют strict no-mutation round-trip

## Дополнительные engineering requirements

- `Aspose.PSD.FOSS.Test` остаётся единственной точкой для acceptance tests этого FOSS-продукта
- testdata хранится внутри test project
- public API библиотеки документируется через XML summary
- public поля и константы тоже обязаны иметь XML summary
- в репозитории должна быть папка `samples/` с runnable sample projects
- в репозитории должна быть markdown-документация для нового пользователя и разработчика

## Текущее покрытие acceptance tests

- Чтение document properties из PSD
- Чтение layer properties из PSD
- Загрузка документа без слоёв
- Загрузка из seekable stream без изменения исходной позиции
- Отклонение некорректной signature
- Отклонение `null` stream
- Сохранение PSD без мутаций byte-for-byte
- Сохранение PSD после изменения `Name`
- Сохранение PSD после изменения `IsVisible`
- Сохранение PSD после изменения `Opacity`
- Сохранение минимального PSB без мутаций byte-for-byte
