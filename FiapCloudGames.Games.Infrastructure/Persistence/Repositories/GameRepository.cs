using FiapCloudGames.Games.Domain.Entities;
using FiapCloudGames.Games.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FiapCloudGames.Games.Infrastructure.Persistence.Repositories;

public class GameRepository(FiapCloudGamesGamesDbContext dbContext) : IGameRepository
{
    private readonly FiapCloudGamesGamesDbContext _dbContext = dbContext;

    public async Task CreateAsync(Game game)
    {
        await _dbContext.Games.AddAsync(game);
    }

    public void Delete(Game game) => _dbContext.Games.Remove(game);

    public Task<Game?> GetByIdTrackingAsync(int gameId) => _dbContext.Games
        .Include(g => g.Genres)
        .Include(g => g.Orders)
        .FirstOrDefaultAsync(g => g.GameId == gameId);

    public Task<List<Game>> GetAllAsync() => _dbContext.Games.AsNoTracking().ToListAsync();

    public Task<List<Game>> GetAllByUserId(int userId) => _dbContext.GamesUsers
        .AsNoTracking()
        .Where(gu => gu.UserId == userId)
        .Include(gu => gu.Game)
        .ThenInclude(g => g.Genres)
        .Include(gu => gu.Game)
        .ThenInclude(g => g.Orders)
        .Select(gu => gu.Game)
        .ToListAsync();

    public async Task UnlockGamesFromOrderToUserAsync(int userId, IEnumerable<Game> games)
    {
        foreach (Game game in games)
            await _dbContext.GamesUsers.AddAsync(new(userId, game.GameId));
    }
}