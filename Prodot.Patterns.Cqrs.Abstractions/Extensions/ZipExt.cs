// Reimplementation of NeverNull
// Taken from https://github.com/Bomret/NeverNull
// Licensed under MIT License by @bomret

namespace Prodot.Patterns.Cqrs;

/// <summary>
///     Provides extension methods to combine the values of several instances of Option.
/// </summary>
public static class ZipExt
{
    /// <summary>
    ///     Combines the values of the specified options using the specified select function.
    ///     Returns None if any of the arguments is None or executing select returns null.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     The select argument is null.
    /// </exception>
    public static Option<TC> Zip<TA, TB, TC>(
        this Option<TA> first,
        Option<TB> second,
        Func<TA, TB, TC> @select
    ) => ZipWith(first, second, (f, s) => Option.From(@select(f, s)));

    /// <summary>
    ///     Combines the values of the specified options using the specified select function.
    ///     Returns None if any of the arguments is None or executing select returns None.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     The select argument is null.
    /// </exception>
    public static Option<TC> ZipWith<TA, TB, TC>(
        this Option<TA> first,
        Option<TB> second,
        Func<TA, TB, Option<TC>> @select
    )
    {
        @select.ThrowIfNull(nameof(@select));

        return first.Match(
            none: () => Option<TC>.None,
            some: a => second.Match(none: () => Option<TC>.None, some: b => @select(a, b))
        );
    }
}
