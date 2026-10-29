# AssistBot

Telegram-бот для ассистентов семинаров (НИУ ВШЭ): подбор времени консультаций, позже — сдача
ДЗ в Google Drive и учёт «выходил к доске».

## Что нужно на Mac

| Инструмент | Как поставить |
|---|---|
| .NET 10 SDK | `brew install --cask dotnet-sdk` или dotnet.microsoft.com/download |
| Docker Desktop | docker.com |
| Rider 2025.3+ | открывает `AssistBot.slnx` (новый формат solution) |
| dotnet-ef | `dotnet tool install --global dotnet-ef` |

Проверка: `dotnet --version` → `10.0.x`.

## Запуск локально (бот в Rider, Postgres в Docker)

```bash
# 1. Postgres
docker compose up -d db

# 2. Токен для запуска из Rider — в user-secrets (хранятся вне репозитория, в ~/.microsoft/usersecrets)
dotnet user-secrets set BOT_TOKEN "123456:ABC..." --project src/AssistBot.Bot

# 3. Первая миграция (один раз; потом — на каждое изменение модели)
dotnet ef migrations add InitialCreate \
  --project src/AssistBot.Infrastructure \
  --startup-project src/AssistBot.Bot \
  --output-dir Persistence/Migrations

# 4. Запуск: в Rider — конфигурация «AssistBot.Bot», или:
dotnet run --project src/AssistBot.Bot
```

Миграции применяются **автоматически при старте** бота, `dotnet ef database update` не нужен.
