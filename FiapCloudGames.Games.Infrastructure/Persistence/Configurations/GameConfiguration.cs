using FiapCloudGames.Games.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiapCloudGames.Games.Infrastructure.Persistence.Configurations;

public class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.HasKey(g => g.GameId);

        builder.Property(g => g.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(g => g.Description)
            .IsRequired(false)
            .HasMaxLength(1000);

        builder.Property(g => g.Price).HasPrecision(18, 2);

        builder.HasMany(g => g.Users)
               .WithOne(u => u.Game);

        builder.HasMany(g => g.Orders)
               .WithMany(p => p.Games)
               .UsingEntity<Dictionary<string, object>>(
                   "GamesOrders",
                   j => j.HasOne<Order>()
                         .WithMany()
                         .HasForeignKey(nameof(Order.OrderId))
                         .HasPrincipalKey(nameof(Order.OrderId)),
                   j => j.HasOne<Game>()
                         .WithMany()
                         .HasForeignKey(nameof(Game.GameId))
                         .HasPrincipalKey(nameof(Game.GameId)),
                   j => j.HasKey(nameof(Game.GameId), nameof(Order.OrderId))
               );

        builder.HasMany(g => g.Genres)
               .WithMany(gr => gr.Games);

        builder.HasMany(g => g.GameEvents)
               .WithOne(ge => ge.Game)
               .HasForeignKey(ge => ge.GameId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
