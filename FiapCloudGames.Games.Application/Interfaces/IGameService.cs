using FiapCloudGames.Games.Application.InputModels;
using FiapCloudGames.Games.Application.ViewModels;

namespace FiapCloudGames.Games.Application.Interfaces;

public interface IGameService
{
    Task<RestResponse<GameViewModel>> CreateGameAsync(CreateGameInputModel inputModel);
    Task<RestResponse> DeleteByIdAsync(int gameId);
    Task<RestResponse<GameViewModel>> GetByIdAsync(int gameId);
    Task<RestResponse<IEnumerable<GameViewModel>>> GetMostPopularGamesWithPaginationAsync(int page, int size);
    Task<RestResponse<IEnumerable<GameViewModel>>> GetGamesByUserIdAsync(int userId);
    Task<RestResponse<IEnumerable<GameViewModel>>> GetWithPaginationAsync(int page, int size, string? name = default, string? description = default);
    Task<RestResponse<IEnumerable<RecommendedGameViewModel>>> GetBasedOnUserHistoryAsync(int userId, int page, int size);
    Task<RestResponse<GameViewModel>> UpdateGameAsync(int gameId, UpdateGameInputModel inputModel);
}
