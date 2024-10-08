﻿// Reimplementation of NeverNull
// Taken from https://github.com/Bomret/NeverNull
// Licensed under MIT License by @bomret

namespace Prodot.Patterns.Cqrs.Tests.Options;

public class FromTests
{
    [Fact]
    public void Creating_an_option_from_nullable_types_should_yield_None_for_null_and_a_Some_containing_the_value_otherwise() =>
        Prop.ForAll<int?>(x =>
                Option
                    .From(x)
                    .Match(
                        none: () => Option.From(x).Equals(Option.None) && !Option.From(x).IsSome,
                        some: val => Option.From(x).IsSome && val.Equals(x!.Value)
                    )
            )
            .QuickCheckThrowOnFailure();

    [Fact]
    public void Creating_an_option_from_reference_types_should_yield_None_for_null_and_a_Some_containing_the_value_otherwise() =>
        Prop.ForAll<string>(x =>
                Option
                    .From(x)
                    .Match(
                        none: () => Option.From(x).Equals(Option.None) && !Option.From(x).IsSome,
                        some: val => Option.From(x).IsSome && val.Equals(x)
                    )
            )
            .QuickCheckThrowOnFailure();

    [Fact]
    public void Creating_an_option_from_value_types_should_always_yield_a_Some_containing_the_value() =>
        Prop.ForAll<double>(x =>
                Option
                    .From(x)
                    .Match(
                        none: () => throw new Exception("Must never happen"),
                        some: val => Option.From(x).IsSome && val.Equals(x)
                    )
            )
            .QuickCheckThrowOnFailure();

    [Fact]
    public void Only_the_appropiate_handler_should_be_called_and_return_a_value_for_an_option_that_is_matched() =>
        Prop.ForAll<string>(x =>
                Option.From(x).Match(none: () => -1, some: v => 1).Equals(x == null ? -1 : 1)
            )
            .QuickCheckThrowOnFailure();
}
