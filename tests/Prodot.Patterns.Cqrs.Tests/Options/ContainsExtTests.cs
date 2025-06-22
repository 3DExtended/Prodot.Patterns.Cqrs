// Reimplementation of NeverNull
// Taken from https://github.com/Bomret/NeverNull
// Licensed under MIT License by @bomret

using FsCheck.Fluent;

namespace Prodot.Patterns.Cqrs.Tests.Options;

public class ContainsExtTests
{
    [Fact]
    public void Options_containing_a_value_should_return_true_for_Contains() =>
        Prop.ForAll<string>(x => Option.From(x).Contains(x) == (x != null))
            .QuickCheckThrowOnFailure();

    [Fact]
    public void Options_containing_a_value_should_return_true_for_Contains_if_the_compare_func_returns_true() =>
        Prop.ForAll<int?, int>(
                (x, y) => Option.From(x).Contains(y, (l, r) => l == r).Equals(x == y)
            )
            .QuickCheckThrowOnFailure();

    [Fact]
    public void Options_containing_a_value_should_return_true_for_Contains_if_the_EqualityComparer_returns_true() =>
        Prop.ForAll<int?, int>(
                (x, y) => Option.From(x).Contains(y, EqualityComparer<int>.Default).Equals(x == y)
            )
            .QuickCheckThrowOnFailure();
}
