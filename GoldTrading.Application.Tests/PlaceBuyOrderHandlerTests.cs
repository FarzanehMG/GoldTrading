using FluentAssertions;
using FluentValidation;
using GoldTrading.Application.Abstractions;
using GoldTrading.Application.Features.Orders.Commands;
using GoldTrading.Application.Features.Orders.Handlers;
using GoldTrading.Application.Features.Orders.Validators;
using GoldTrading.Infrastructure.Persistence;

namespace GoldTrading.Application.Tests;

public class PlaceBuyOrderHandlerTests
{
    private sealed class FixedClock : IClock
    {
        public DateTime UtcNow { get; } = new(2026, 10, 14, 8, 0, 0, DateTimeKind.Utc);
    }

    private readonly InMemoryOrderRepository _orders = new();
    private readonly InMemoryWalletRepository _wallets = new();
    private readonly PlaceBuyOrderHandler _handler;

    public PlaceBuyOrderHandlerTests() =>
        _handler = new PlaceBuyOrderHandler(_orders, _wallets, new FixedClock(), new PlaceBuyOrderCommandValidator());

    private static PlaceBuyOrderCommand Command(Guid userId, decimal grams = 2.5m) =>
        new(userId, grams, 3_500_000m, 50_000m, ["پس‌انداز_بلندمدت"], "یادداشت");

    [Fact]
    public async Task Handle_CreatesSettledOrder_AndCreditsWallet()
    {
        var userId = Guid.NewGuid();

        var result = await _handler.HandleAsync(Command(userId));

        result.Status.Should().Be("Settled");
        result.TotalAmount.Should().Be(8_800_000m);
        result.WalletBalanceInGrams.Should().Be(2.5m);
        (await _orders.GetByIdAsync(result.OrderId)).Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_SecondPurchase_AccumulatesBalance()
    {
        var userId = Guid.NewGuid();

        await _handler.HandleAsync(Command(userId, 1m));
        var result = await _handler.HandleAsync(Command(userId, 1.5m));

        result.WalletBalanceInGrams.Should().Be(2.5m);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(1.2345)]
    public async Task Handle_WithInvalidWeight_ThrowsValidationException_AndLeavesWalletUntouched(double grams)
    {
        var userId = Guid.NewGuid();
        await _handler.HandleAsync(Command(userId, 1m));

        var act = () => _handler.HandleAsync(Command(userId, (decimal)grams));

        await act.Should().ThrowAsync<ValidationException>();
        var result = await _handler.HandleAsync(Command(userId, 1m));
        result.WalletBalanceInGrams.Should().Be(2m); 
    }

    [Fact]
    public async Task Handle_ConcurrentPurchases_DoNotLoseUpdates()
    {
        var userId = Guid.NewGuid();

        await Task.WhenAll(Enumerable.Range(0, 100)
            .Select(_ => Task.Run(() => _handler.HandleAsync(Command(userId, 1m)))));

        var result = await _handler.HandleAsync(Command(userId, 1m));
        result.WalletBalanceInGrams.Should().Be(101m);
    }
}
