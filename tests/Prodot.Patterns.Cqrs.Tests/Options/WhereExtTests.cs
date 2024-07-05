// Reimplementation of NeverNull
// Taken from https://github.com/Bomret/NeverNull
// Licensed under MIT License by @bomret

namespace Prodot.Patterns.Cqrs.Tests.Options;

public class WhereExtTests
{
    [Fact]
    public void Options_that_not_adhere_to_a_predicate_and_None_should_result_in_None()
    {
        Prop.ForAll<string>(x =>
            {
                var option = Option.From(x);

                return option
                    .Where(v => v.Length < 10)
                    .Equals(x?.Length < 10 ? option : Option.None);
            })
            .QuickCheckThrowOnFailure();
    }
}
