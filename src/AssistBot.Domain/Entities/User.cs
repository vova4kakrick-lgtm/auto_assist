namespace AssistBot.Domain.Entities;

public class User
{
    public static int MaxNameLength = 128;

    public long TelegramId { get; set; }

    public string FullName { get; set; } = "";

    public DateTimeOffset CreatedAt { get; set; }

    private User() {}

    public User(long telegramId, string firstName, string lastName, DateTimeOffset createdAtUtc)
    {
        TelegramId = telegramId;
        FullName = NormalName(firstName, lastName);
        CreatedAt = createdAtUtc;
    }

    public void UpdateFullName(string firstName, string lastName) => FullName = NormalName(firstName, lastName);

    private static string NormalName(string firstName, string lastName)
    {
        return string.IsNullOrEmpty(lastName) ? firstName : $"{lastName} {firstName}";
    }
}
