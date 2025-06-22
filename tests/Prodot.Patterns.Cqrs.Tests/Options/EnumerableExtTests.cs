// Reimplementation of NeverNull
// Taken from https://github.com/Bomret/NeverNull
// Licensed under MIT License by @bomret

using FsCheck.Fluent;

namespace Prodot.Patterns.Cqrs.Tests.Options;

public class EnumerableExtTests
{
    [Fact]
    public void Aggregating_the_values_from_an_enumerable_of_options_of_nullables_should_yield_None_for_an_empty_one_or_if_all_values_are_null() =>
        Prop.ForAll<int?[]>(xs =>
            {
                var check = xs.Where(x => x.HasValue)
                    .Aggregate(default(int?), (a, c) => a.HasValue ? a.Value + c!.Value : c!.Value);
                var o = xs.AggregateOptionalNullable((a, c) => a + c);

                return o.Equals(Option.From(check));
            })
            .QuickCheckThrowOnFailure();

    [Fact]
    public void Aggregating_the_values_from_an_enumerable_of_options_should_yield_None_for_an_empty_one_or_if_all_values_are_null() =>
        Prop.ForAll<string[]>(xs =>
            {
                var check = xs.Where(x => x != null).Aggregate(default(string), (a, c) => a + c);
                var o = xs.AggregateOptional((a, c) => a + c);

                return o.Equals(Option.From(check));
            })
            .QuickCheckThrowOnFailure();

    [Fact]
    public void Exchanged_options_of_arrays_of_nullables_should_yield_empty_arrays_for_None_or_the_arrays_for_Some() =>
        Prop.ForAll<int?[]>(xs =>
                Option
                    .From(xs)
                    .Exchange()
                    .SequenceEqual(xs?.Select(Option.From) ?? Enumerable.Empty<Option<int>>())
            )
            .QuickCheckThrowOnFailure();

    [Fact]
    public void Exchanged_options_of_arrays_should_yield_empty_arrays_for_None_or_the_arrays_for_Some() =>
        Prop.ForAll<string[]>(xs =>
                Option
                    .From(xs)
                    .Exchange()
                    .SequenceEqual(xs?.Select(Option.From) ?? Enumerable.Empty<Option<string>>())
            )
            .QuickCheckThrowOnFailure();

    [Fact]
    public void Exchanged_options_of_enumerables_of_nullables_should_yield_empty_enumerables_for_None_or_the_enumerables_for_Some() =>
        Prop.ForAll<int?[]>(xs =>
                Option
                    .From(xs.AsEnumerable())
                    .Exchange()
                    .SequenceEqual(xs?.Select(Option.From) ?? Enumerable.Empty<Option<int>>())
            )
            .QuickCheckThrowOnFailure();

    [Fact]
    public void Exchanged_options_of_enumerables_should_yield_empty_enumerables_for_None_or_the_enumerables_for_Some() =>
        Prop.ForAll<string[]>(xs =>
                Option
                    .From(xs.AsEnumerable())
                    .Exchange()
                    .SequenceEqual(xs?.Select(Option.From) ?? Enumerable.Empty<Option<string>>())
            )
            .QuickCheckThrowOnFailure();

    [Fact]
    public void Selecting_a_single_value_from_an_enumerable_of_nullables_should_yield_None_for_an_empty_enumerable_and_throw_if_the_enumerable_contains_more_than_one_element() =>
        Prop.ForAll<int?[]>(xs =>
            {
                Option<int> val = Option.None;
                Exception? err = null;
                try
                {
                    val = xs.SingleOptional();
                }
                catch (Exception e)
                {
                    err = e;
                }

                return err != null || val.Equals(Option.From(xs.SingleOrDefault()));
            })
            .QuickCheckThrowOnFailure();

    [Fact]
    public void Selecting_a_single_value_from_an_enumerable_of_nullables_should_yield_None_for_an_empty_enumerable_or_if_the_predicate_does_not_hold_and_throw_if_the_enumerable_contains_more_than_one_element() =>
        Prop.ForAll<int?[]>(xs =>
            {
                Option<int> val = Option.None;
                Exception? err = null;
                try
                {
                    val = xs.SingleOptionalNullable(x => x > 5);
                }
                catch (Exception e)
                {
                    err = e;
                }

                return err != null
                    || val.Equals(Option.From(xs.SingleOrDefault(x => x.HasValue && x.Value > 5)));
            })
            .QuickCheckThrowOnFailure();

    [Fact]
    public void Selecting_a_single_value_from_an_enumerable_should_yield_None_for_an_empty_enumerable_and_throw_if_the_enumerable_contains_more_than_one_element() =>
        Prop.ForAll<string[]>(xs =>
            {
                Option<string> val = Option.None;
                Exception? err = null;
                try
                {
                    val = xs.SingleOptional();
                }
                catch (Exception e)
                {
                    err = e;
                }

                return err != null || val.Equals(xs.SingleOrDefault()!);
            })
            .QuickCheckThrowOnFailure();

    [Fact]
    public void Selecting_a_single_value_from_an_enumerable_should_yield_None_for_an_empty_enumerable_or_if_the_predicate_does_not_hold_and_throw_if_the_enumerable_contains_more_than_one_element() =>
        Prop.ForAll<string[]>(xs =>
            {
                Option<string> val = Option.None;
                Exception? err = null;
                try
                {
                    val = xs.SingleOptional(x => x.Contains('a'));
                }
                catch (Exception e)
                {
                    err = e;
                }

                return err != null
                    || val.Equals(xs.SingleOrDefault(x => x != null && x.Contains('a'))!);
            })
            .QuickCheckThrowOnFailure();

    [Fact]
    public void Selecting_the_first_value_from_an_enumerable_of_nullables_should_yield_None_for_an_empty_enumerable() =>
        Prop.ForAll<int?[]>(xs => xs.FirstOptional().Equals(Option.From(xs.FirstOrDefault())))
            .QuickCheckThrowOnFailure();

    [Fact]
    public void Selecting_the_first_value_from_an_enumerable_should_yield_None_for_an_empty_enumerable() =>
        Prop.ForAll<string[]>(xs => xs.FirstOptional().Equals(Option.From(xs.FirstOrDefault())))
            .QuickCheckThrowOnFailure();

    [Fact]
    public void Selecting_the_last_value_from_an_enumerable_of_nullables_should_yield_None_for_an_empty_enumerable() =>
        Prop.ForAll<int?[]>(xs => xs.LastOptional().Equals(Option.From(xs.LastOrDefault())))
            .QuickCheckThrowOnFailure();

    [Fact]
    public void Selecting_the_last_value_from_an_enumerable_should_yield_None_for_an_empty_enumerable() =>
        Prop.ForAll<string[]>(xs => xs.LastOptional().Equals(xs.LastOrDefault()!))
            .QuickCheckThrowOnFailure();

    [Fact]
    public void Selecting_the_values_from_an_enumerable_of_options_of_nullables_should_only_yield_the_values() =>
        Prop.ForAll<int?[]>(xs =>
                xs.Select(x => Option.From<int?>(x))
                    .SelectValues()
                    .SequenceEqual(xs.Where(x => x.HasValue).Select(x => x!.Value))
            )
            .QuickCheckThrowOnFailure();

    [Fact]
    public void Selecting_the_values_from_an_enumerable_of_options_should_only_yield_the_values() =>
        Prop.ForAll<string[]>(xs =>
                xs.Select(Option.From).SelectValues().SequenceEqual(xs.Where(x => x != null))
            )
            .QuickCheckThrowOnFailure();
}
