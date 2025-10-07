using FiapCloudGames.Games.Application.ViewModels;

namespace FiapCloudGames.Games.Application.Interfaces;

public interface IOrderService
{
    Task CancelByIdAsync(int orderId);
    Task<RestResponse> CreateForGameIdsAsync(int userId, IEnumerable<int> gamesIds);
    Task<RestResponse<IEnumerable<OrderViewModel>>> GetOrdersByUserIdAsync(int userId);
    Task UnlockGamesFromOrderToUserAsync(int orderId);
    Task UpdatePaymentIdAsync(int orderId, int paymentId);
}
