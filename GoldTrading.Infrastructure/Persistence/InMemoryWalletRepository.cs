using GoldTrading.Application.Abstractions;
using GoldTrading.Domain.Aggregate.Entities;
using System.Collections.Concurrent;

namespace GoldTrading.Infrastructure.Persistence;

public sealed class InMemoryWalletRepository : IWalletRepository
{
    private readonly ConcurrentDictionary<Guid, GoldWallet> _wallets = new();
    private readonly ConcurrentDictionary<Guid, object> _locks = new();

    public Task AddAsync(GoldWallet wallet, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<GoldWallet?> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task SaveAsync(GoldWallet wallet, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<T> UpdateAsync<T>(Guid userId, Func<GoldWallet, T> update, CancellationToken ct = default)
    {
        var gate = _locks.GetOrAdd(userId, _ => new object());

        lock (gate)
        {
            var wallet = _wallets.GetOrAdd(userId, id => GoldWallet.Open(id));
            return Task.FromResult(update(wallet));
        }
    }
}