// Reimplementation of NeverNull
// Taken from https://github.com/Bomret/NeverNull
// Licensed under MIT License by @bomret

namespace Prodot.Patterns.Cqrs.Tests.Options;

public class SwitchExtTests
{
    [Fact]
    public void A_collection_of_options_should_switch_to_the_first_Some_otherwise_to_None() =>
        Prop.ForAll<string, string, string>(
                (a, b, c) =>
                    Option
                        .From(a)
                        .Switch(Option.From(b), Option.From(c))
                        .Equals(a ?? b ?? c ?? Option<string>.None)
            )
            .QuickCheckThrowOnFailure();

    [Fact]
    public void An_enumerable_of_options_should_switch_to_the_first_Some_otherwise_to_None() =>
        Prop.ForAll<string, string[]>(
                (a, xs) =>
                    Option
                        .From(a)
                        .Switch(xs.Select(Option.From))
                        .Equals(a ?? xs.FirstOrDefault(x => x != null) ?? Option<string>.None)
            )
            .QuickCheckThrowOnFailure();
}
