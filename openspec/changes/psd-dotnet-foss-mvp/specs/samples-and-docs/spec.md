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

## MODIFIED Requirements

## REMOVED Requirements

## RENAMED Requirements
