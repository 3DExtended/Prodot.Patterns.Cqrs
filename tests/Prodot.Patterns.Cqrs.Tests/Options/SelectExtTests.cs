// Reimplementation of NeverNull
// Taken from https://github.com/Bomret/NeverNull
// Licensed under MIT License by @bomret

using FsCheck.Fluent;

namespace Prodot.Patterns.Cqrs.Tests.Options;

public class SelectExtTests
{
    [Fact]
    public void Only_values_inside_a_Some_should_be_selected() =>
        Prop.ForAll<string>(x =>
                Option.From(x).Select(v => v.Length).Equals(x?.Length ?? Option<int>.None)
            )
            .QuickCheckThrowOnFailure();
}
