// Reimplementation of NeverNull
// Taken from https://github.com/Bomret/NeverNull
// Licensed under MIT License by @bomret

namespace Prodot.Patterns.Cqrs.Tests.Options;

public class NormalizeExtTests
{
    [Fact]
    public void Normalizing_options_of_nullables_should_return_the_correct_value_representation() =>
        Prop.ForAll<int?>(x => Option.From<int?>(x).Normalize().Equals(Option.From(x)))
            .QuickCheckThrowOnFailure();
}
