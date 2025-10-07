using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Search;
using Elastic.Clients.Elasticsearch.QueryDsl;
using FiapCloudGames.Games.Application.InputModels;
using FiapCloudGames.Games.Application.Interfaces;
using FiapCloudGames.Games.Application.ViewModels;
using FiapCloudGames.Games.Domain.Entities;
using FiapCloudGames.Games.Domain.Enums;
using FiapCloudGames.Games.Domain.Exceptions;
using FiapCloudGames.Games.Infrastructure.ElasticSearch;
using FiapCloudGames.Games.Infrastructure.ElasticSearch.Models;
using FiapCloudGames.Games.Infrastructure.Persistence;
using Serilog;

namespace FiapCloudGames.Games.Application.Services;

public class GameService(IUnitOfWork unitOfWork, ElasticsearchClient elasticsearchClient) : IGameService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IElasticClient<GameElasticSearchModel> _elasticClient = new ElasticClient<GameElasticSearchModel>(elasticsearchClient);

    public async Task<RestResponse<GameViewModel>> CreateGameAsync(CreateGameInputModel inputModel)
    {
        Log.Information("Iniciando criação do jogo {title}", inputModel.Title);
        Game game = inputModel.ToDomain();

        foreach (int gameGenreId in inputModel.GameGenreIds)
        {
            GameGenre? gameGenre = await _unitOfWork.GameGenres.GetByIdAsync(gameGenreId);
            if (gameGenre is null)
            {
                Log.Warning("Gênero de jogo {gameGenreId} não encontrado", gameGenreId);
                throw new GameGenreNotFoundException();
            }
            game.Genres.Add(gameGenre);
        }

        await _unitOfWork.Games.CreateAsync(game);
        await _unitOfWork.CompleteAsync();
        Log.Information("Jogo {gameId} criado com sucesso", game.GameId);

        Log.Information("Indexando jogo {gameId} no ElasticSearch", game.GameId);
        await _elasticClient.UpinsertAsync(GameElasticSearchModel.FromDomain(game));

        Log.Information("Jogo {gameId} indexado com sucesso no ElasticSearch", game.GameId);
        return RestResponse<GameViewModel>.Success(GameViewModel.FromDomain(game));
    }

    public async Task<RestResponse> DeleteByIdAsync(int gameId)
    {
        Log.Information("Iniciando exclusão do jogo {gameId}", gameId);
        Game? game = await _unitOfWork.Games.GetByIdTrackingAsync(gameId);
        if (game is null)
        {
            Log.Warning("Jogo {gameId} não encontrado", gameId);
            throw new GameNotFoundException();
        }

        _unitOfWork.Games.Delete(game);
        await _unitOfWork.CompleteAsync();
        await _elasticClient.DeleteByIdAsync(gameId);
        Log.Information("Jogo {gameId} excluído com sucesso", gameId);

        return RestResponse.Success();
    }

    public async Task<RestResponse<GameViewModel>> GetByIdAsync(int gameId)
    {
        Log.Information("Buscando jogo {gameId}", gameId);
        GameElasticSearchModel? gameElasticSearchModel = await _elasticClient.GetByIdAsync(gameId);
        if (gameElasticSearchModel is null)
        {
            Log.Warning("Jogo {gameId} não encontrado", gameId);
            throw new GameNotFoundException();
        }

        Log.Information("Jogo {gameId} encontrado", gameId);
        return RestResponse<GameViewModel>.Success(GameViewModel.FromElasticSearchModel(gameElasticSearchModel));
    }

    public async Task<RestResponse<IEnumerable<RecommendedGameViewModel>>> GetBasedOnUserHistoryAsync(int userId, int page, int size)
    {
        Log.Information("Buscando jogos recomendados para o usuário {userId}", userId);
        List<Game> purchasedGames = _unitOfWork.Orders.GetOrderedGamesByUserId(userId);
        IEnumerable<int> userPurchasedGameIds = purchasedGames.Select(g => g.GameId).Distinct();

        List<string> allGenres = [];
        string? mostPurchasedGenre = purchasedGames
            .SelectMany(g => g.Genres)
            .Select(gg => Enum.GetName(typeof(InitialGameGenreType), gg.GameGenreId))
            .GroupBy(g => g)
            .OrderByDescending(g => g.Count())
            .ThenBy(g => g.Key).Select(g => g.Key)
            .FirstOrDefault();

        Log.Information("Buscando os jogos recomendados para o usuário {userId} baseado no gênero mais comprado {mostPurchasedGenre}", userId, mostPurchasedGenre);
        IReadOnlyCollection<Hit<GameElasticSearchModel>> games = await _elasticClient.SearchWithMostMatchQueryAsync("genres.keyword", mostPurchasedGenre!, page, size);

        Log.Information("{totalGames} jogos recomendados encontrados para o usuário {userId}", games.Count, userId);
        return RestResponse<IEnumerable<RecommendedGameViewModel>>.Success(games.Select(h => RecommendedGameViewModel.FromElasticSearchModel(h.Source!, h.Score!.Value)));
    }

    public async Task<RestResponse<IEnumerable<GameViewModel>>> GetMostPopularGamesWithPaginationAsync(int page, int size)
    {
        Log.Information("Buscando jogos mais populares");
        IReadOnlyCollection<GameElasticSearchModel> games = await _elasticClient.GetWithComplexFiltersSearchAsync(page, size, orderBy: g => g.TotalSales, sortOrder: SortOrder.Desc);

        Log.Information("{totalGames} jogos mais populares encontrados", games.Count);
        return RestResponse<IEnumerable<GameViewModel>>.Success(games.Select(GameViewModel.FromElasticSearchModel));
    }

    public async Task<RestResponse<IEnumerable<GameViewModel>>> GetGamesByUserIdAsync(int userId)
    {
        Log.Information("Buscando jogos do usuário {userId}", userId);
        List<Game> games = await _unitOfWork.Games.GetAllByUserId(userId);

        Log.Information("{totalGames} jogos encontrados para o usuário {userId}", games.Count, userId);
        return RestResponse<IEnumerable<GameViewModel>>.Success(games.Select(GameViewModel.FromDomain));
    }

    public async Task<RestResponse<IEnumerable<GameViewModel>>> GetWithPaginationAsync(int page, int size, string? title = default, string? description = default)
    {
        Log.Information("Buscando jogos com paginação complexa - Página: {page}, Tamanho: {size}, Título: {title}, Descrição: {description}", page, size, title, description);
        IReadOnlyCollection<GameElasticSearchModel> games = await _elasticClient.GetWithComplexFiltersSearchAsync(page, size, queries: !string.IsNullOrWhiteSpace(title) || !string.IsNullOrWhiteSpace(description)
            ? mq =>
            {
                ICollection<Query> queries = [];

                if (!string.IsNullOrWhiteSpace(title))
                    queries.Add(mq.Match(m => m.Field(f => f.Title).Query(title)));
                if (!string.IsNullOrWhiteSpace(description))
                    queries.Add(mq.Match(m => m.Field(f => f.Description).Query(description)));

                return queries;
            } : null);

        Log.Information("{totalGames} jogos encontrados", games.Count);
        return RestResponse<IEnumerable<GameViewModel>>.Success(games.Select(GameViewModel.FromElasticSearchModel));
    }

    public async Task<RestResponse<GameViewModel>> UpdateGameAsync(int gameId, UpdateGameInputModel inputModel)
    {
        Log.Information("Iniciando atualização do jogo {gameId}", gameId);
        Game? game = await _unitOfWork.Games.GetByIdTrackingAsync(gameId);
        if (game is null)
        {
            Log.Warning("Jogo {gameId} não encontrado", gameId);
            throw new GameNotFoundException();
        }

        game.Genres.Clear();
        game.Update(inputModel.Title, inputModel.Description, inputModel.Price);
        foreach (int gameGenreId in inputModel.GameGenreIds)
        {
            GameGenre? gameGenre = await _unitOfWork.GameGenres.GetByIdAsync(gameGenreId);
            if (gameGenre is null)
            {
                Log.Warning("Gênero de jogo {gameGenreId} não encontrado", gameGenreId);
                throw new GameGenreNotFoundException();
            }
            game.Genres.Add(gameGenre);
        }
        await _unitOfWork.CompleteAsync();
        Log.Information("Jogo {gameId} atualizado com sucesso no banco de dados", gameId);

        await _elasticClient.UpinsertAsync(GameElasticSearchModel.FromDomain(game));
        Log.Information("Jogo {gameId} atualizado com sucesso no ElasticSearch", gameId);

        return RestResponse<GameViewModel>.Success(GameViewModel.FromDomain(game));
    }
}
