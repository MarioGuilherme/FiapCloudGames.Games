using FiapCloudGames.Games.Domain.Entities;
using FiapCloudGames.Games.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FiapCloudGames.Games.Infrastructure.Persistence.Repositories;

public class OrderRepository(FiapCloudGamesGamesDbContext dbContext) : IOrderRepository
{
    private readonly FiapCloudGamesGamesDbContext _dbContext = dbContext;

    public async Task CreateAsync(Order order)
    {
        await _dbContext.Orders.AddAsync(order);
    }

    public Task<List<Order>> GetAllByUserIdAsync(int userId) => _dbContext.Orders
        .AsNoTracking()
        .Include(p => p.Games)
        .ThenInclude(g => g.Genres)
        .Where(p => p.UserId == userId)
        .ToListAsync();

    public Task<Order?> GetByIdTrackingAsync(int orderId) => _dbContext.Orders
        .Include(o => o.Games)
        .Include(o => o.OrderEvents)
        .FirstOrDefaultAsync(p => p.OrderId == orderId);

    public List<Game> GetOrderedGamesByUserId(int userId) => _dbContext.Orders
        .AsNoTracking()
        .Where(p => p.UserId == userId)
        .Include(p => p.Games)
        .ThenInclude(g => g.Genres)
        .SelectMany(p => p.Games)
        .AsEnumerable()
        .DistinctBy(g => g.GameId)
        .ToList();
}
