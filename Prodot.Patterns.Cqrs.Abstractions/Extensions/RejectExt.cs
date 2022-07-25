// Reimplementation of NeverNull
// Taken from https://github.com/Bomret/NeverNull
// Licensed under MIT License by @bomret

namespace Prodot.Patterns.Cqrs;

/// <summary>
///     Provides extension methods to reject instances of Option.
/// </summary>
public static class RejectExt
{
    /// <summary>
    ///     Returns None if the given <paramref name="predicate" /> holds for this option, otherwise this option.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="predicate" /> is null.
    /// </exception>
    public static Option<T> Reject<T>(this Option<T> option, Func<T, bool> predicate)
    {
        predicate.ThrowIfNull(nameof(predicate));

        return option.Match(
            none: () => Option<T>.None,
            some: x => predicate(x) ? Option<T>.None : option);
    }
}
