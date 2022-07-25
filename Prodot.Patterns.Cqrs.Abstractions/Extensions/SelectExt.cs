// Reimplementation of NeverNull
// Taken from https://github.com/Bomret/NeverNull
// Licensed under MIT License by @bomret

namespace Prodot.Patterns.Cqrs;

/// <summary>
///     Provides extension methods to transform the values of option into new forms.
/// </summary>
public static class SelectExt
{
    /// <summary>
    ///     Applies the specified <paramref name="selector" /> function to the value of the specified <see cref="Option{T}"/> if it contains one and wraps the result in a new <see cref="Option{T}"/>. Otherwise None is returned.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="selector" /> is null.
    /// </exception>
    public static Option<TB> Select<TA, TB>(this Option<TA> option, Func<TA, TB> selector)
    {
        selector.ThrowIfNull(nameof(selector));

        return option.Match(
            none: () => Option<TB>.None,
            some: x => selector(x));
    }
}
