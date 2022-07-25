// Reimplementation of NeverNull
// Taken from https://github.com/Bomret/NeverNull
// Licensed under MIT License by @bomret

namespace Prodot.Patterns.Cqrs.Tests.Options;

public class ZipExtTests
{
    [Fact]
    public void Options_should_only_be_zipped_when_both_options_contain_values() =>
        Prop.ForAll<string, string>((a, b) =>
            Option.From(a)
                .Zip(Option.From(b), (va, vb) => va + vb)
                .Equals(a == null || b == null ? Option<string>.None : a + b))
        .QuickCheckThrowOnFailure();
}
