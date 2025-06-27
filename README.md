# PriceLoaderWebApp

> Приложение для автоматической загрузки прайс-листов от поставщиков через IMAP, парсинга CSV и сохранения в PostgreSQL

## Описание

Это ASP.NET Core Web API приложение, реализующее автоматическую загрузку прайс-листов от поставщика "ООО Доставим в срок":
- Получает письма с вложениями `.csv` через IMAP
- Парсит файлы с учётом переменного формата колонок
- Обрабатывает данные перед сохранением
- Сохраняет результат в PostgreSQL

Идеально подходит для интеграции в системы управления запасами, ERP или ETL процесс.

---

## Технологии

| Технология | Использование |
|------------|---------------|
| C# / .NET 8 | Backend |
| ASP.NET Core | Web API |
| MailKit | Работа с IMAP |
| CsvHelper | Парсинг CSV |
| Entity Framework Core | ORM для работы с БД |
| PostgreSQL | Хранение данных |
| AutoMapper | Маппинг DTO ↔ Entities |
| Serilog (опционально) | Логгирование |

---

## Структура проекта

```
/PriceLoaderWebApp
├── API/                   # Контроллеры
│   ├── Controllers/
├── Application/            # Бизнес-логика
│   ├── Services/
│   └── DTOs/
├── Domain/                 # Сущности и исключения
│   ├── Entities/
│   └── Exceptions/
├── Infrastructure/         # IMAP, БД, конфигурация
│   ├── Mail/
│   ├── Persistence/
│   └── Configuration/
├── Mappings/               # AutoMapper профили
├── appsettings.json
└── PriceLoaderWebApp.csproj
```

---

## Установка и запуск

### 1. Клонируй репозиторий:

```bash
git clone https://github.com/yourname/PriceLoaderWebApp.git
cd PriceLoaderWebApp
```

### 2. Настрой `appsettings.json`

Создай или обнови файл `appsettings.json`:

```json
{
  "ImapSettings": {
    "Host": "imap.yandex.ru",
    "Port": 993,
    "Username": "TaTami375@yandex.ru",
    "Password": "ваш_пароль_приложения"
  },
  "Supplier": {
    "Name": "ООО Доставим в срок",
    "ColumnMapping": {
      "Vendor": "Бренд",
      "Number": "Артикул",
      "Description": "Описание",
      "Price": "Цена, руб.",
      "Count": "Наличие"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=priceloaderdb;Username=postgres;Password=yourpassword"
  }
}
```

### 3. Восстанови зависимости:

```bash
dotnet restore
```

### 4. Собери и запусти:

```bash
dotnet build
dotnet run
```

---

## Эндпоинты API

### Загрузить прайс-лист

**POST** `/api/Price/load`  
Тело запроса: `"ООО Доставим в срок"`

Пример:
```bash
curl -X POST 'http://localhost:5139/api/Price/load' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '"ООО Доставим в срок"'
```

Возвращает список товаров в формате:

```json
[
  {
    "vendor": "Bosch",
    "number": "123456",
    "description": "Фильтр масляный...",
    "price": 890.50,
    "count": 10
  }
]
```

---

## База данных

Приложение использует PostgreSQL. Создай таблицу:

```sql
CREATE TABLE "PriceItems" (
    "Id" SERIAL PRIMARY KEY,
    "Vendor" VARCHAR(64),
    "Number" VARCHAR(64),
    "SearchVendor" VARCHAR(64),
    "SearchNumber" VARCHAR(64),
    "Description" VARCHAR(512),
    "Price" NUMERIC(18,2),
    "Count" INTEGER
);
```

---

## Что можно улучшить

- [ ] Поддержка нескольких поставщиков
- [ ] Фоновая служба (BackgroundService)
- [ ] Логгирование через Serilog
- [ ] Проверка дубликатов при импорте

---


## Автор

Куприянов М.А. 

---

## Связь

Если у тебя есть вопросы, ты можешь:
- Написать мне в Telegram: @MaksimKupriyanov_official
- Или отправить письмо: makskupriyanoff@Yandex.ru
```
