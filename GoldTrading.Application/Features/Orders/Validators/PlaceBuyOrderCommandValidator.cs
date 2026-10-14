using FluentValidation;
using GoldTrading.Application.Features.Orders.Commands;
using GoldTrading.Domain.Aggregate.Entities;
using GoldTrading.Domain.Aggregate.ValueObjects;

namespace GoldTrading.Application.Features.Orders.Validators;

public sealed class PlaceBuyOrderCommandValidator : AbstractValidator<PlaceBuyOrderCommand>
{
    public PlaceBuyOrderCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();

        RuleFor(x => x.WeightInGrams)
            .GreaterThan(0)
            .PrecisionScale(18, GoldWeight.MaxDecimalPlaces, ignoreTrailingZeros: true)
            .WithMessage($"Weight must have at most {GoldWeight.MaxDecimalPlaces} decimal places.");

        RuleFor(x => x.UnitPrice).GreaterThan(0);
        RuleFor(x => x.Fee).GreaterThanOrEqualTo(0);

        RuleFor(x => x.Tags)
            .Must(t => t is null || t.Count <= GoldOrder.MaxTagsPerOrder)
            .WithMessage($"At most {GoldOrder.MaxTagsPerOrder} tags are allowed.");

        RuleForEach(x => x.Tags)
            .NotEmpty()
            .MaximumLength(Tag.MaxLength);

        RuleFor(x => x.Note).MaximumLength(GoldOrder.MaxNoteLength);
    }
}