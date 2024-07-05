// Reimplementation of NeverNull
// Taken from https://github.com/Bomret/NeverNull
// Licensed under MIT License by @bomret

namespace Prodot.Patterns.Cqrs.Tests.Options;

public class RejectExtTests
{
    [Fact]
    public void Rejected_options_and_None_should_result_in_None()
    {
        Prop.ForAll<string>(x =>
            {
                var option = Option.From(x);
                var result = option.Reject(v => v.Length < 10);

                return result.Equals(x?.Length < 10 ? Option.None : option);
            })
            .QuickCheckThrowOnFailure();
    }
}
