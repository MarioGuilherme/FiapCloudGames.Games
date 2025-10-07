using FiapCloudGames.Games.Domain.Entities;

namespace FiapCloudGames.Games.Application.InputModels;

public record CreateGameInputModel(string Title, string Description, decimal Price, HashSet<int> GameGenreIds)
{
    public Game ToDomain() => new(Title, Description, Price);
}
