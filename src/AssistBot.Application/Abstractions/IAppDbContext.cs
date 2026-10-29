using AssistBot.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AssistBot.Application.Abstractions;

public interface IAppDbContext
{
    DbSet<User> Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
