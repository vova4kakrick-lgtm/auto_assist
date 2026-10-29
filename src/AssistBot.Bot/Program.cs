using AssistBot.Application;
using AssistBot.Bot;
using AssistBot.Infrastructure;
using AssistBot.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddUserSecrets(System.Reflection.Assembly.GetExecutingAssembly(), optional: true);

var botToken = builder.Configuration["BOT_TOKEN"]
    ?? throw new InvalidOperationException(
        "BOT_TOKEN не задан: dotnet user-secrets set BOT_TOKEN \"...\" --project src/AssistBot.Bot");

var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("ConnectionStrings:Default не задан");

builder.Services.AddApplication();
builder.Services.AddInfrastructure(connectionString);
builder.Services.AddSingleton<ITelegramBotClient>(_ => new TelegramBotClient(botToken));
builder.Services.AddHostedService<BotService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.Migrate();
}

app.Run();
