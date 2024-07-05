// Reimplementation of NeverNull
// Taken from https://github.com/Bomret/NeverNull
// Licensed under MIT License by @bomret

namespace Prodot.Patterns.Cqrs.Tests.Options;

public class OrElseExtTests
{
    [Fact]
    public void The_fallback_option_should_only_be_used_for_None() =>
        Prop.ForAll<string, string>(
                (a, b) => Option.From(a).OrElseWith(Option.From(b)) == Option.From(a ?? b)
            )
            .QuickCheckThrowOnFailure();

    [Fact]
    public void The_fallback_value_should_only_be_used_for_None() =>
        Prop.ForAll<string, string>((a, b) => Option.From(a).OrElse(b) == Option.From(a ?? b))
            .QuickCheckThrowOnFailure();

    [Fact]
    public void The_option_produced_by_the_fallback_func_should_only_be_used_for_None() =>
        Prop.ForAll<string, string>(
                (a, b) => Option.From(a).OrElseWith(() => Option.From(b)) == Option.From(a ?? b)
            )
            .QuickCheckThrowOnFailure();

    [Fact]
    public void The_value_produced_by_the_fallback_func_should_only_be_used_for_None() =>
        Prop.ForAll<string, string>((a, b) => Option.From(a).OrElse(() => b) == Option.From(a ?? b))
            .QuickCheckThrowOnFailure();
}
