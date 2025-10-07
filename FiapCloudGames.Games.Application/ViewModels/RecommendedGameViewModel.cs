using FiapCloudGames.Games.Infrastructure.ElasticSearch.Models;

namespace FiapCloudGames.Games.Application.ViewModels;

public record RecommendedGameViewModel(int GameId, string Title, string? Description, decimal Price, double MetricToYourUser, IEnumerable<string> Genres)
{
    public static RecommendedGameViewModel FromElasticSearchModel(GameElasticSearchModel game, double score) => new(
        game.Id,
        game.Title,
        game.Description,
        game.Price,
        score,
        game.Genres
    );
}
