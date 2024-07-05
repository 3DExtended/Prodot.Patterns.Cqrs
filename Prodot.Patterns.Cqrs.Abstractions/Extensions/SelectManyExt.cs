// Reimplementation of NeverNull
// Taken from https://github.com/Bomret/NeverNull
// Licensed under MIT License by @bomret

namespace Prodot.Patterns.Cqrs;

/// <summary>
///     Provides extension methods to transform the value of option into new forms of
///     Option.    ///
/// </summary>
public static class SelectManyExt
{
    /// <summary>
    ///     Applies the specified <paramref name="select" /> function to the value of the specified <paramref name="option" />,
    ///     if it has one, and returns the produced Option. Otherwise None is returned.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="select" /> is null.
    /// </exception>
    public static Option<TB> SelectMany<TA, TB>(
        this Option<TA> option,
        Func<TA, Option<TB>> @select
    )
    {
        @select.ThrowIfNull(nameof(@select));

        return option.Match(none: () => Option<TB>.None, some: @select);
    }

    /// <summary>
    ///     Applies the specified <paramref name="optionSelector" /> and <paramref name="resultSelector" /> functions to the
    ///     value of the specified <paramref name="option" />, if it contains one. Otherwise None is returned.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="optionSelector" /> or <paramref name="resultSelector" /> is null.
    /// </exception>
    public static Option<TC> SelectMany<TA, TB, TC>(
        this Option<TA> option,
        Func<TA, Option<TB>> optionSelector,
        Func<TA, TB, TC> resultSelector
    )
    {
        optionSelector.ThrowIfNull(nameof(optionSelector));
        resultSelector.ThrowIfNull(nameof(resultSelector));

        return option.Match(
            none: () => Option<TC>.None,
            some: a =>
                optionSelector(a)
                    .Match(none: () => Option<TC>.None, some: b => resultSelector(a, b))
        );
    }
}
