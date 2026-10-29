using AssistBot.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssistBot.Infrastructure.Persistence.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.TelegramId);
        builder.Property(u => u.TelegramId).ValueGeneratedNever();

        builder.Property(u => u.FullName).IsRequired().HasMaxLength(User.MaxNameLength);

        builder.Property(u => u.CreatedAt).IsRequired();
    }
}
