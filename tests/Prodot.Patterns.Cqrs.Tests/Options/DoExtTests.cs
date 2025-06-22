// Reimplementation of NeverNull
// Taken from https://github.com/Bomret/NeverNull
// Licensed under MIT License by @bomret

using FsCheck.Fluent;

namespace Prodot.Patterns.Cqrs.Tests.Options;

public class DoExtTests
{
    [Fact]
    public void Side_effects_should_only_be_executed_for_options_that_contain_values() =>
        Prop.ForAll<string>(x =>
            {
                var modfiedVal = "-1";
                var option = Option.From(x);
                var returnedOption = option.Do(v => modfiedVal = v + "1");

                return option.Equals(returnedOption)
                    && modfiedVal.Equals(x == null ? "-1" : x + "1");
            })
            .QuickCheckThrowOnFailure();
}
