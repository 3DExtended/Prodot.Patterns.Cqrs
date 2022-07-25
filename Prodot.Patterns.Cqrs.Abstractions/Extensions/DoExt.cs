// Reimplementation of NeverNull
// Taken from https://github.com/Bomret/NeverNull
// Licensed under MIT License by @bomret

namespace Prodot.Patterns.Cqrs;

/// <summary>
///     Provides extension methods to execute side effects on the value of an option.
/// </summary>
public static class DoExt
{
    /// <summary>
    ///     Executes the specified side effect on the value of the specified option, if it has one.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     The sideEffect argument is null.
    /// </exception>
    public static Option<T> Do<T>(this Option<T> option, Action<T> sideEffect)
    {
        sideEffect.ThrowIfNull(nameof(sideEffect));

        option.IfSome(sideEffect);

        return option;
    }
}
