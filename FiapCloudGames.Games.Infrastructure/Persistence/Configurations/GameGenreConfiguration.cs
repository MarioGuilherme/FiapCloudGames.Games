using FiapCloudGames.Games.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiapCloudGames.Games.Infrastructure.Persistence.Configurations;

public class GameGenreConfiguration : IEntityTypeConfiguration<GameGenre>
{
    public void Configure(EntityTypeBuilder<GameGenre> builder)
    {
        builder.HasKey(gr => gr.GameGenreId);

        builder.HasIndex(gr => gr.Title).IsUnique();
        builder.Property(gr => gr.Title)
            .IsRequired()
            .HasMaxLength(30);

        builder.HasMany(gr => gr.Games)
               .WithMany(g => g.Genres);

        builder.HasData([
            new(1, "Action"),
            new(2, "Adventure"),
            new(3, "Simulation"),
            new(4, "Strategy"),
            new(5, "Sports"),
            new(6, "Racing"),
            new(7, "Fight"),
            new(8, "Shooter"),
            new(9, "Platformer"),
            new(10, "Puzzle"),
            new(11, "Horror"),
            new(12, "Stealth"),
            new(13, "Sandbox"),
            new(14, "MMORPG"),
            new(15, "BattleRoyale"),
            new(16, "MusicRhythm"),
            new(17, "Indie")
        ]);
    }
}
