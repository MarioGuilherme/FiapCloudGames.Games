using FiapCloudGames.Games.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiapCloudGames.Games.Infrastructure.Persistence.Configurations;

public class GameEventConfiguration : IEntityTypeConfiguration<GameEvent>
{
    public void Configure(EntityTypeBuilder<GameEvent> builder)
    {
        builder.HasKey(ou => ou.GameEventId);

        builder.Property(g => g.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(g => g.Description)
            .IsRequired(false)
            .HasMaxLength(1000);

        builder.Property(g => g.Price).HasPrecision(18, 2);

        builder.Property(ou => ou.EventAt)
            .HasDefaultValue(DateTime.Now);

        builder.HasOne(ge => ge.Game)
               .WithMany(g => g.GameEvents)
               .HasForeignKey(ge => ge.GameId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("GameEvents");
    }
}
