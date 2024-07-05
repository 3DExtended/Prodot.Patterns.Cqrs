// Reimplementation of NeverNull
// Taken from https://github.com/Bomret/NeverNull
// Licensed under MIT License by @bomret

namespace Prodot.Patterns.Cqrs;

public struct Option
{
    public static None None => default(None);

    /// <summary>
    ///     Creates a new option from the specified value.
    ///     If 'null' is provided None is returned, in every other case an option containing
    ///     the value.
    /// </summary>
    public static Option<T> From<T>(T? value) =>
        ReferenceEquals(value, null) ? Option<T>.None : new Option<T>(value);

    /// <summary>
    ///     Creates a new option from the specified Nullable.
    ///     If 'null' is provided None is returned, in every other case an option containing
    ///     the value of the Nullable.
    /// </summary>
    public static Option<T> From<T>(T? nullable)
        where T : struct => nullable.HasValue ? new Option<T>(nullable.Value) : Option<T>.None;
}
