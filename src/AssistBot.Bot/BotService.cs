using AssistBot.Application.Users;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace AssistBot.Bot;

public class BotService(
    ITelegramBotClient bot,
    IServiceScopeFactory scopeFactory,
    ILogger<BotService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        logger.LogInformation("Бот запущен");

        var offset = 0;

        while (!ct.IsCancellationRequested)
        {
            Update[] updates;

            try
            {
                updates = await bot.GetUpdates(
                    offset: offset,
                    timeout: 30,
                    allowedUpdates: [UpdateType.Message],
                    cancellationToken: ct);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Не удалось получить сообщения, пробую через 5 секунд");
                await Task.Delay(5000, ct);
                continue;
            }

            foreach (var update in updates)
            {
                offset = update.Id + 1;

                try
                {
                    await HandleAsync(update, ct);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Ошибка при обработке сообщения {UpdateId}", update.Id);
                }
            }
        }
    }
    private async Task HandleAsync(Update update, CancellationToken ct)
    {
        var message = update.Message;
        if (message?.Text is null || message.From is null) return;

        if (message.Text.StartsWith("/start"))
        {
            using var scope = scopeFactory.CreateScope();
            var users = scope.ServiceProvider.GetRequiredService<UserService>();

            var from = message.From;

            var user = await users.RegisterAsync(from.Id, from.FirstName, from.LastName ?? "", ct);

            var inviteCode = message.Text["/start".Length..].Trim();
            await bot.SendMessage(
                message.Chat.Id,
                $"Привет, {user.FullName}! Я — HSE Assist бот!\nКоманды: /help",
                cancellationToken: ct);
        }
        else if (message.Text.StartsWith("/help"))
        {
            await bot.SendMessage(
                message.Chat.Id,
                "Команды:\n/start — регистрация\n/help — справка",
                cancellationToken: ct);
        }
    }
}
