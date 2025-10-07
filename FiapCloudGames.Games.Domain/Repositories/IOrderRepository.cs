using FiapCloudGames.Games.Domain.Entities;

namespace FiapCloudGames.Games.Domain.Repositories;

public interface IOrderRepository
{
    Task CreateAsync(Order order);
    Task<List<Order>> GetAllByUserIdAsync(int userId);
    Task<Order?> GetByIdTrackingAsync(int orderId);
    List<Game> GetOrderedGamesByUserId(int userId);
}
