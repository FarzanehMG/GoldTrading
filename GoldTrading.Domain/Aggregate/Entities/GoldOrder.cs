using GoldTrading.Domain.Aggregate.Enums;
using GoldTrading.Domain.Aggregate.Events;
using GoldTrading.Domain.Aggregate.ValueObjects;
using GoldTrading.Domain.Common;

namespace GoldTrading.Domain.Aggregate.Entities;

public sealed class GoldOrder
{
    #region Properties

    #region Identity

    public Guid Id { get; }
    public Guid UserId { get; }
    public OrderType Type { get; }

    #endregion

    #region Order Details

    public GoldWeight Weight { get; }
    public Money UnitPrice { get; }
    public Money Fee { get; }
    public Money TotalAmount { get; }

    #endregion

    #region Status and Lifecycle

    public OrderStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; }

    #endregion

    #region Notes

    public const int MaxNoteLength = 500;
    public string? UserNote { get; private set; }

    #endregion

    #region Tags

    public const int MaxTagsPerOrder = 10;

    private readonly HashSet<Tag> _tags = new();

    public IReadOnlyCollection<Tag> Tags => _tags;

    #endregion

    #endregion

    #region Domain Events

    private readonly List<IDomainEvent> _domainEvents = new();

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents;

    public void ClearDomainEvents() => _domainEvents.Clear();

    #endregion

    #region Constructor

    private GoldOrder(
        Guid id, 
        Guid userId, 
        GoldWeight weight,
        Money unitPrice, 
        Money fee, 
        Money totalAmount,
        DateTime createdAtUtc)
    {
        Id = id;
        UserId = userId;
        Type = OrderType.Buy;
        Weight = weight;
        UnitPrice = unitPrice;
        Fee = fee;
        TotalAmount = totalAmount;
        Status = OrderStatus.Pending;
        CreatedAtUtc = createdAtUtc;
    }

    #endregion

    #region Methods

    public static GoldOrder PlaceBuy(
        Guid userId, GoldWeight weight, Money unitPrice, Money fee, DateTime nowUtc)
    {
        if (userId == Guid.Empty)
            throw new DomainException("UserId is required.");

        if (!weight.IsPositive)
            throw new DomainException("Order weight must be greater than zero.");

        if (!unitPrice.IsPositive)
            throw new DomainException("Unit price must be greater than zero.");

        // TotalAmount = (Weight × UnitPrice) + Fee
        var total = unitPrice.Multiply(weight).Add(fee);

        return new GoldOrder(Guid.NewGuid(), userId, weight, unitPrice, fee, total, nowUtc);
    }

    public void Settle(DateTime nowUtc)
    {
        if (Status != OrderStatus.Pending)
            throw new DomainException($"Only pending orders can be settled. Current status: {Status}.");

        Status = OrderStatus.Settled;

        _domainEvents.Add(new GoldPurchasedDomainEvent(Id, UserId, Weight, TotalAmount, nowUtc));
    }

    public void ReplaceTags(IEnumerable<Tag> tags)
    {
        ArgumentNullException.ThrowIfNull(tags);

        var newTags = tags.ToHashSet();

        if (newTags.Count > MaxTagsPerOrder)
            throw new DomainException($"An order cannot have more than {MaxTagsPerOrder} tags.");

        _tags.Clear();
        foreach (var tag in newTags)
            _tags.Add(tag);
    }

    public void UpdateNote(string? note)
    {
        var trimmed = string.IsNullOrWhiteSpace(note) ? null : note.Trim();

        if (trimmed is { Length: > MaxNoteLength })
            throw new DomainException($"Note cannot exceed {MaxNoteLength} characters.");

        UserNote = trimmed;
    }

    #endregion
}
