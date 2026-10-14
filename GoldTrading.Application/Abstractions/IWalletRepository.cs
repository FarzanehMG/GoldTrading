using GoldTrading.Domain.Aggregate.Entities;

namespace GoldTrading.Application.Abstractions;

public interface IWalletRepository
{
    Task<GoldWallet?> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task AddAsync(GoldWallet wallet, CancellationToken ct = default);
    Task SaveAsync(GoldWallet wallet, CancellationToken ct = default);

    /// Gets the user's wallet (creating it if missing) and applies the change atomically.
    Task<T> UpdateAsync<T>(Guid userId, Func<GoldWallet, T> update, CancellationToken ct = default);
}