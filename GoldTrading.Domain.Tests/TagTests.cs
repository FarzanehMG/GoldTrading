using FluentAssertions;
using GoldTrading.Domain.Aggregate.ValueObjects;
using GoldTrading.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldTrading.Domain.Tests;

public class TagTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithEmptyValue_Throws(string? value)
    {
        var act = () => Tag.Create(value);

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Create_WithTooLongValue_Throws()
    {
        var act = () => Tag.Create(new string('a', Tag.MaxLength + 1));

        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Create_TrimsValue_AndEqualTagsAreEqual()
    {
        Tag.Create("  نوسان‌گیری_روزانه ").Should().Be(Tag.Create("نوسان‌گیری_روزانه"));
    }
}
