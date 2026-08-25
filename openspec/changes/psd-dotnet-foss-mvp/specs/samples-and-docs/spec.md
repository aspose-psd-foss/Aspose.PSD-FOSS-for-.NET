## ADDED Requirements

### Requirement: Предоставление runnable sample projects в репозитории
Система SHALL предоставлять runnable sample projects в репозитории.

#### Scenario: Обнаружение samples в репозитории
- **WHEN** разработчик открывает репозиторий
- **THEN** присутствует верхнеуровневая папка `samples/`

#### Scenario: Запуск базового metadata sample
- **WHEN** разработчик запускает базовый sample project
- **THEN** sample демонстрирует загрузку PSD/PSB файла и вывод document properties

#### Scenario: Запуск sample по layer editing
- **WHEN** разработчик запускает sample project для layer editing
- **THEN** sample демонстрирует чтение layer metadata и сохранение файла после изменения поддерживаемых layer properties

#### Scenario: Наличие парных samples для FOSS и официального Aspose.PSD
- **WHEN** разработчик открывает `samples/`
- **THEN** для каждого поддерживаемого сценария есть FOSS sample project и аналогичный NuGet sample project на официальном пакете `Aspose.PSD`

#### Scenario: Идентичность sample code между FOSS и NuGet samples
- **WHEN** разработчик сравнивает соответствующие `Program.cs` в FOSS и NuGet sample projects
- **THEN** файлы совпадают по содержимому
- **AND** различие между проектами ограничено project-level references/dependencies

### Requirement: Предоставление documentation репозитория как markdown files
Система SHALL предоставлять documentation репозитория в виде markdown files.

#### Scenario: Обнаружение documentation
- **WHEN** разработчик открывает репозиторий
- **THEN** присутствует верхнеуровневая область с documentation
- **AND** корневой README ссылается на неё

#### Scenario: Чтение getting started guidance
- **WHEN** новый пользователь открывает getting started document
- **THEN** document объясняет installation, базовую форму API и первый рабочий пример

#### Scenario: Чтение guidance по supported scope
- **WHEN** пользователь открывает документ с limitations или support scope
- **THEN** document объясняет, что поддерживает текущий scope продукта и что остаётся вне scope

#### Scenario: Документация sample parity
- **WHEN** пользователь читает документацию по samples
- **THEN** документация объясняет, что FOSS samples и NuGet samples должны оставаться полностью аналогичными и использовать идентичный sample code

## MODIFIED Requirements

## REMOVED Requirements

## RENAMED Requirements
