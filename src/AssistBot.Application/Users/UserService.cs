using AssistBot.Application.Abstractions;
using AssistBot.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AssistBot.Application.Users;

public sealed class UserService(IAppDbContext db, IClock clock)
{
    public async Task<User> RegisterAsync(
        long telegramId, string firstName, string lastName, CancellationToken ct)
    {
        var user = await db.Users.SingleOrDefaultAsync(u => u.TelegramId == telegramId, ct);

        if (user is null)
        {
            user = new User(telegramId, firstName, lastName, clock.UtcNow);
            db.Users.Add(user);
            await db.SaveChangesAsync(ct);
            return user;
        }

        user.UpdateFullName(firstName, lastName);
        await db.SaveChangesAsync(ct);

        return user;
    }

    public async Task<User?> ChangeFullNameAsync(
        long telegramId, string firstName, string lastName, CancellationToken ct)
    {
        var user = await db.Users.SingleOrDefaultAsync(u => u.TelegramId == telegramId, ct);

        if (user is null)
        {
            return user;
        }

        user.UpdateFullName(firstName, lastName);
        await db.SaveChangesAsync(ct);
        return user;
    }
}
