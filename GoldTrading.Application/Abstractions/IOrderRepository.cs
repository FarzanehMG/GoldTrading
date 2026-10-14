using GoldTrading.Domain.Aggregate.Entities;

namespace GoldTrading.Application.Abstractions;

public interface IOrderRepository
{
    Task AddAsync(GoldOrder order, CancellationToken ct = default);
    Task<GoldOrder?> GetByIdAsync(Guid id, CancellationToken ct = default);
}
