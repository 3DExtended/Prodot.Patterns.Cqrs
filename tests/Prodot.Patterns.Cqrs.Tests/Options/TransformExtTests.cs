// Reimplementation of NeverNull
// Taken from https://github.com/Bomret/NeverNull
// Licensed under MIT License by @bomret

using FsCheck.Fluent;

namespace Prodot.Patterns.Cqrs.Tests.Options;

public class TransformExtTests
{
    [Fact]
    public void Only_the_appropiate_handler_should_be_called_and_return_a_value_for_an_option_that_is_transformed() =>
        Prop.ForAll<string>(x =>
            {
                return Option
                    .From(x)
                    .Transform(some: v => 1, none: () => -1)
                    .Equals(x == null ? -1 : 1);
            })
            .QuickCheckThrowOnFailure();
}
