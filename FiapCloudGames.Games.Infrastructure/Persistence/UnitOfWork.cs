using FiapCloudGames.Games.Domain.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace FiapCloudGames.Games.Infrastructure.Persistence;

public class UnitOfWork(FiapCloudGamesGamesDbContext dbContext, IGameRepository games, IGameGenreRepository gameGenres, IOrderRepository orders) : IUnitOfWork
{
    private readonly FiapCloudGamesGamesDbContext _dbContext = dbContext;
    private IDbContextTransaction? _transaction;

    public IGameRepository Games => games;
    public IGameGenreRepository GameGenres => gameGenres;
    public IOrderRepository Orders => orders;

    public Task<int> CompleteAsync() => _dbContext.SaveChangesAsync();

    public async Task BeginTransactionAsync() => _transaction = await _dbContext.Database.BeginTransactionAsync();

    public async Task CommitAsync()
    {
        if (_transaction is null) return;

        try
        {
            await CompleteAsync();
            await _transaction!.CommitAsync();
        }
        catch
        {
            await _transaction!.RollbackAsync();
            throw;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
            _dbContext.Dispose();
    }
}
