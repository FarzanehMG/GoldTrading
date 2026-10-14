using GoldTrading.Application.Abstractions;
using GoldTrading.Domain.Aggregate.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldTrading.Infrastructure.Persistence;

public sealed class InMemoryOrderRepository : IOrderRepository
{
    private readonly ConcurrentDictionary<Guid, GoldOrder> _store = new();

    public Task AddAsync(GoldOrder order, CancellationToken ct = default)
    {
        if (!_store.TryAdd(order.Id, order))
            throw new InvalidOperationException($"Order {order.Id} already exists.");

        return Task.CompletedTask;
    }

    public Task<GoldOrder?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        Task.FromResult(_store.GetValueOrDefault(id));
}
