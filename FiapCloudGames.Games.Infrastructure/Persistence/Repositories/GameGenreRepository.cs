using FiapCloudGames.Games.Domain.Entities;
using FiapCloudGames.Games.Domain.Repositories;

namespace FiapCloudGames.Games.Infrastructure.Persistence.Repositories;

public class GameGenreRepository(FiapCloudGamesGamesDbContext dbContext) : IGameGenreRepository
{
    private readonly FiapCloudGamesGamesDbContext _dbContext = dbContext;

    public ValueTask<GameGenre?> GetByIdAsync(int gameGenreId) => _dbContext.GameGenres.FindAsync(gameGenreId);
}
