﻿// Reimplementation of NeverNull
// Taken from https://github.com/Bomret/NeverNull
// Licensed under MIT License by @bomret

namespace Prodot.Patterns.Cqrs.Tests.Options;

public class GetExtTests
{
    [Fact]
    public void Getting_the_value_from_an_option_from_a_nullable_should_throw_for_None_and_return_the_value_if_the_option_contains_one() =>
        Prop.ForAll<int?>(x =>
            {
                var val = 0;
                Exception? err = null;
                try
                {
                    val = Option.From(x).Get();
                }
                catch (Exception e)
                {
                    err = e;
                }

                return x == null ? err != null && val == 0 : err == null && val == x;
            })
            .QuickCheckThrowOnFailure();

    [Fact]
    public void Getting_the_value_from_an_option_should_execute_the_fallback_func_for_None_and_return_the_value_if_the_option_contains_one() =>
        Prop.ForAll<string, string>((a, b) => Option.From(a).GetOrElse(() => b) == (a ?? b))
            .QuickCheckThrowOnFailure();

    [Fact]
    public void Getting_the_value_from_an_option_should_return_its_default_for_None_and_return_the_value_if_the_option_contains_one() =>
        Prop.ForAll<string>(x => Option.From(x).GetOrElse(default(string)) == x)
            .QuickCheckThrowOnFailure();

    [Fact]
    public void Getting_the_value_from_an_option_should_return_the_fallback_for_None_and_return_the_value_if_the_option_contains_one() =>
        Prop.ForAll<string, string>((a, b) => Option.From(a).GetOrElse(b) == (a ?? b))
            .QuickCheckThrowOnFailure();

    [Fact]
    public void Getting_the_value_from_an_option_should_throw_for_None_and_return_the_value_if_the_option_contains_one() =>
        Prop.ForAll<string>(x =>
            {
                string? val = null;
                Exception? err = null;
                try
                {
                    val = Option.From(x).Get();
                }
                catch (Exception e)
                {
                    err = e;
                }

                return x == null ? err != null && val == null : err == null && val == x;
            })
            .QuickCheckThrowOnFailure();
}
