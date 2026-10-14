using GoldTrading.Application.Abstractions;
using GoldTrading.Domain.Aggregate.Entities;
using System.Collections.Concurrent;

namespace GoldTrading.Infrastructure.Persistence;

public sealed class InMemoryOrderRepository : IOrderRepository
{
    private readonly ConcurrentDictionary<Guid, GoldOrder> _store = new();

    public Task AddAsync(GoldOrder order, CancellationToken ct = default)
    {
        return !_store.TryAdd(order.Id, order) ? 
            throw new InvalidOperationException($"Order {order.Id} already exists.") : 
            Task.CompletedTask;
    }

    public Task<GoldOrder?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        Task.FromResult(_store.GetValueOrDefault(id));
}
