// Reimplementation of NeverNull
// Taken from https://github.com/Bomret/NeverNull
// Licensed under MIT License by @bomret

namespace Prodot.Patterns.Cqrs;

/// <summary>
///     Provides extension methods to get the value from an option or react to None.
/// </summary>
public static class GetExt
{
    /// <summary>
    ///     Returns the value of the specified option if it has one or throws an InvalidOperationException.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    ///     The option contains no value.
    /// </exception>
    public static T Get<T>(this Option<T> option) =>
        option.Match(
            none: () =>
            {
                throw new InvalidOperationException("None does not contain a value.");
            },
            some: x => x
        );

    /// <summary>
    ///     Returns the value of the specified option if it has one or the given fallback.
    /// </summary>
    public static T? GetOrElse<T>(this Option<T> option, T? fallback) =>
        GetOrElse(option, () => fallback);

    /// <summary>
    ///     Returns the value of the specified option if it has one or executes the given fallback func and returns the produced value.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     The fallback argument is null.
    /// </exception>
    public static T? GetOrElse<T>(this Option<T> option, Func<T?> fallback)
    {
        fallback.ThrowIfNull(nameof(fallback));

        return option.Match(none: fallback, some: x => x);
    }
}
