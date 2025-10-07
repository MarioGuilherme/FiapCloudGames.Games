using FiapCloudGames.Games.Domain.Entities;
using FiapCloudGames.Games.Domain.Enums;

namespace FiapCloudGames.Games.Infrastructure.ElasticSearch.Models;

public class GameElasticSearchModel : ElasticSearchModel
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; } = null!;
    public decimal Price { get; set; }
    public IEnumerable<string> Genres { get; set; } = null!;
    public long TotalSales { get; set; }

    public static GameElasticSearchModel FromDomain(Game game) => new()
    {
        Id = game.GameId,
        Title = game.Title,
        Description = game.Description,
        Price = game.Price,
        Genres = game.Genres.Select(gg => Enum.GetName(typeof(InitialGameGenreType), gg.GameGenreId)!),
        TotalSales = game.Orders?.Count ?? 0
    };
}
