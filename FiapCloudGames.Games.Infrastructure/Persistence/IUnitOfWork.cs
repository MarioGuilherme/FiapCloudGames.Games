using FiapCloudGames.Games.Domain.Repositories;

namespace FiapCloudGames.Games.Infrastructure.Persistence;

public interface IUnitOfWork : IDisposable
{
    IGameRepository Games { get; }
    IGameGenreRepository GameGenres { get; }
    IOrderRepository Orders { get; }
    Task<int> CompleteAsync();
    Task BeginTransactionAsync();
    Task CommitAsync();
}
