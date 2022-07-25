// Reimplementation of NeverNull
// Taken from https://github.com/Bomret/NeverNull
// Licensed under MIT License by @bomret

namespace Prodot.Patterns.Cqrs;

/// <summary>
///     Provides extension methods to join the values of multiple instances of Option.
/// </summary>
public static class JoinExt
{
    /// <summary>
    ///     Joins the specified options if they have values, based
    ///     on the equality of selected keys into a new Option.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     The firstKeySelector argument, secondKeySelector or resultSelector is null.
    /// </exception>
    public static Option<TC> Join<TA, TB, TC, TKey>(this Option<TA> firstOption, Option<TB> secondOption,
        Func<TA, TKey> firstKeySelector, Func<TB, TKey> secondKeySelector,
        Func<TA, TB, TC> resultSelector,
        IEqualityComparer<TKey>? comparer = null)
    {
        firstKeySelector.ThrowIfNull(nameof(firstKeySelector));
        secondKeySelector.ThrowIfNull(nameof(secondKeySelector));
        resultSelector.ThrowIfNull(nameof(resultSelector));

        return firstOption.Match(
            none: () => Option<TC>.None,
            some: a => secondOption.Match(
                none: () => Option<TC>.None,
                some: b => Option.From(firstKeySelector(a)).Match(
                    none: () => Option<TC>.None,
                    some: keyA => Option.From(secondKeySelector(b)).Match(
                        none: () => Option<TC>.None,
                        some: keyB =>
                        {
                            var equalityComparer = comparer ?? EqualityComparer<TKey>.Default;

                            return equalityComparer.Equals(keyA, keyB)
                                ? resultSelector(a, b)
                                : Option<TC>.None;
                        }))));
    }
}
