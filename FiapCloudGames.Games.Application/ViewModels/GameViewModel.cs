using FiapCloudGames.Games.Domain.Entities;
using FiapCloudGames.Games.Domain.Enums;
using FiapCloudGames.Games.Infrastructure.ElasticSearch.Models;

namespace FiapCloudGames.Games.Application.ViewModels;

public record GameViewModel(int GameId, string Title, string? Description, decimal Price, IEnumerable<string> Genres, long TotalSales)
{
    public static GameViewModel FromDomain(Game game) => new(
        game.GameId,
        game.Title,
        game.Description,
        game.Price,
        game.Genres.Select(gg => Enum.GetName(typeof(InitialGameGenreType), gg.GameGenreId)!).ToList(),
        game.Orders.Count
    );

    public static GameViewModel FromElasticSearchModel(GameElasticSearchModel game) => new(
        game.Id,
        game.Title,
        game.Description,
        game.Price,
        game.Genres,
        game.TotalSales
    );
};
