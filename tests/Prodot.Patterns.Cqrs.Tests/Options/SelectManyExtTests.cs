// Reimplementation of NeverNull
// Taken from https://github.com/Bomret/NeverNull
// Licensed under MIT License by @bomret

using FsCheck.Fluent;

namespace Prodot.Patterns.Cqrs.Tests.Options;

public class SelectManyExtTests
{
    [Fact]
    public void Only_the_values_of_options_inside_a_Some_should_be_selected() =>
        Prop.ForAll<string>(x =>
                Option
                    .From(x)
                    .SelectMany(v => Option.From(v + "1"))
                    .Equals(x == null ? Option<string>.None : x + "1")
            )
            .QuickCheckThrowOnFailure();
}
