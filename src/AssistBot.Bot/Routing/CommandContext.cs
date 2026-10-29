namespace AssistBot.Bot.Routing;

public record CommandContext(long ChatId, long UserId, string FirstName, string? LastName, string Args);
