namespace AssistBot.Application.Abstractions;

public interface IClock
{
    DateTimeOffset UtcNow { get; }
}
