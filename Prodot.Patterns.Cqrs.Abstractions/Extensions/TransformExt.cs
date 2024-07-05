// Reimplementation of NeverNull
// Taken from https://github.com/Bomret/NeverNull
// Licensed under MIT License by @bomret

namespace Prodot.Patterns.Cqrs;

/// <summary>
///     Provides extension methods that allow to transform instances of option depending on their type.
/// </summary>
public static class TransformExt
{
    /// <summary>
    ///     Executes the appropriate callback depending on the type of the specified option.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     One of the side effect functions is null.
    /// </exception>
    public static Option<TB> Transform<TA, TB>(
        this Option<TA> option,
        Func<TA, TB> some,
        Func<TB> none
    )
    {
        some.ThrowIfNull(nameof(some));
        none.ThrowIfNull(nameof(none));

        return option.Match(none: none, some: some);
    }
}
