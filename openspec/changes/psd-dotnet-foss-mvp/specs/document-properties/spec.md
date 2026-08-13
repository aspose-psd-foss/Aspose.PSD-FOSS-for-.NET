## ADDED Requirements

### Requirement: Чтение ширины изображения
Система SHALL читать и экспонировать ширину изображения из заголовка PSD.

#### Scenario: Чтение ширины из корректного PSD
- **WHEN** пользователь загружает PSD файл с шириной 1920
- **THEN** image.Width возвращает 1920

### Requirement: Чтение высоты изображения
Система SHALL читать и экспонировать высоту изображения из заголовка PSD.

#### Scenario: Чтение высоты из корректного PSD
- **WHEN** пользователь загружает PSD файл с высотой 1080
- **THEN** image.Height возвращает 1080

### Requirement: Чтение бит на канал
Система SHALL читать и экспонировать бит на канал из заголовка PSD.

#### Scenario: Чтение 8-бит на канал
- **WHEN** пользователь загружает 8-битный PSD файл
- **THEN** image.BitsPerChannel возвращает 8

#### Scenario: Чтение 16-бит на канал
- **WHEN** пользователь загружает 16-битный PSD файл
- **THEN** image.BitsPerChannel возвращает 16

#### Scenario: Чтение 32-бит на канал
- **WHEN** пользователь загружает 32-битный PSD файл
- **THEN** image.BitsPerChannel возвращает 32

### Requirement: Чтение цветового режима
Система SHALL читать и экспонировать цветовой режим из заголовка PSD.

#### Scenario: Чтение режима RGB
- **WHEN** пользователь загружает PSD файл в режиме RGB
- **THEN** image.ColorMode возвращает ColorModes.Rgb

#### Scenario: Чтение режима CMYK
- **WHEN** пользователь загружает PSD файл в режиме CMYK
- **THEN** image.ColorMode возвращает ColorModes.Cmyk

#### Scenario: Чтение режима Grayscale
- **WHEN** пользователь загружает PSD файл в режиме Grayscale
- **THEN** image.ColorMode возвращает ColorModes.Grayscale

### Requirement: Чтение версии PSD
Система SHALL читать и экспонировать версию PSD из заголовка файла.

#### Scenario: Чтение версии 6
- **WHEN** пользователь загружает PSD файл версии 6
- **THEN** image.Version возвращает 6

#### Scenario: Чтение версии 1
- **WHEN** пользователь загружает PSD файл версии 1
- **THEN** image.Version возвращает 1

## MODIFIED Requirements

## REMOVED Requirements

## RENAMED Requirements
