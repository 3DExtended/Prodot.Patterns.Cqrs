// Reimplementation of NeverNull
// Taken from https://github.com/Bomret/NeverNull
// Licensed under MIT License by @bomret

namespace Prodot.Patterns.Cqrs.Tests.Options;

public class FlattenExtTests
{
    [Fact]
    public void Nested_options_should_be_flattened_correctly() =>
        Prop.ForAll<string>(x =>
            {
                var nestedOption = Option.From(x);

                return Option.From(nestedOption).Flatten().Equals(nestedOption);
            })
            .QuickCheckThrowOnFailure();
}
