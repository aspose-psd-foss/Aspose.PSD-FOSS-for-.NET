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

## Product Scope

- Загрузка PSD
- Базовая загрузка PSB
- Чтение document properties из header
- Чтение layer metadata из Layer and Mask Information
- Изменение `Name`, `IsVisible`, `Opacity`
- Сохранение без рендеринга
- Byte-for-byte round-trip без мутаций
- Raw-preserve для неподдерживаемых/неизвестных данных, где это возможно

## Out of scope

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
- public surface библиотеки документируется через XML summary
- public поля и константы тоже обязаны иметь XML summary
- в репозитории должна быть папка `samples/` с runnable sample projects
- в репозитории должна быть markdown-документация для нового пользователя и разработчика

## Текущее покрытие acceptance tests

- Load document properties from PSD
- Load layer properties from PSD
- Load document without layers
- Load from seekable stream without changing original position
- Reject invalid signature
- Reject null stream
- Save PSD without mutation byte-for-byte
- Save PSD after changing `Name`
- Save PSD after changing `IsVisible`
- Save PSD after changing `Opacity`
- Save minimal PSB without mutation byte-for-byte
