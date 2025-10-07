using FiapCloudGames.Games.Domain.Entities;

namespace FiapCloudGames.Games.Domain.Repositories;

public interface IGameRepository
{
    Task CreateAsync(Game game);
    void Delete(Game game);
    Task<Game?> GetByIdTrackingAsync(int gameId);
    Task<List<Game>> GetAllAsync();
    Task<List<Game>> GetAllByUserId(int userId);
    Task UnlockGamesFromOrderToUserAsync(int userId, IEnumerable<Game> games);
}
