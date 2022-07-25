// Reimplementation of NeverNull
// Taken from https://github.com/Bomret/NeverNull
// Licensed under MIT License by @bomret

namespace Prodot.Patterns.Cqrs;

/// <summary>
///     Provides extension methods to select the first option in a collection that has a value.
/// </summary>
public static class SwitchExt
{
    /// <summary>
    ///     Returns the first option that contains a value or None, if no option contains one.
    /// </summary>
    public static Option<T> Switch<T>(this Option<T> option, params Option<T>[] options) =>
        Switch(option, options.AsEnumerable());

    /// <summary>
    ///     Returns the first option that contains a value or None, if no option contains one.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="options" /> is null.
    /// </exception>
    public static Option<T> Switch<T>(this Option<T> option, IEnumerable<Option<T>> options)
    {
        options.ThrowIfNull(nameof(options));

        return option.IsSome ? option : options.FirstOrDefault(o => o.IsSome);
    }
}
