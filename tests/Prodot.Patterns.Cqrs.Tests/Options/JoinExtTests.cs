// Reimplementation of NeverNull
// Taken from https://github.com/Bomret/NeverNull
// Licensed under MIT License by @bomret

using FsCheck.Fluent;

namespace Prodot.Patterns.Cqrs.Tests.Options;

public class JoinExtTests
{
    [Fact]
    public void Only_options_containing_values_should_be_joined_otherwise_None_is_expected() =>
        Prop.ForAll<string, string>(
                (a, b) =>
                {
                    var optionA = Option.From(a);
                    var optionB = Option.From(b);

                    var joined =
                        from va in optionA
                        join vb in optionB on true equals true
                        select va + vb;

                    return a == null || b == null
                        ? joined.Equals(Option.None)
                        : joined.Equals(Option.From(a + b));
                }
            )
            .QuickCheckThrowOnFailure();
}
