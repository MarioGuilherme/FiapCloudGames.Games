using FiapCloudGames.Games.Domain.Entities;

namespace FiapCloudGames.Games.Domain.Repositories;

public interface IGameGenreRepository
{
    ValueTask<GameGenre?> GetByIdAsync(int gameGenreId);
}
