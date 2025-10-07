using FiapCloudGames.Games.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiapCloudGames.Games.Infrastructure.Persistence.Configurations;

public class UserGameConfiguration : IEntityTypeConfiguration<GameUser>
{
    public void Configure(EntityTypeBuilder<GameUser> builder)
    {
        builder.HasKey(gu => new { gu.UserId, gu.GameId });
        builder.HasOne(gu => gu.Game)
               .WithMany(g => g.Users);
    }
}
