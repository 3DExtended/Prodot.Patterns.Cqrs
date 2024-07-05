// Reimplementation of NeverNull
// Taken from https://github.com/Bomret/NeverNull
// Licensed under MIT License by @bomret

namespace Prodot.Patterns.Cqrs;

/// <summary>
///   Provides extension methods to transform instances of option into their Nullable representation.
/// </summary>
public static class ToNullableExt
{
    /// <summary>
    ///   Returns a Nullable containing the value of the specified option or an empty Nullable otherwise.
    /// </summary>
    public static T? ToNullable<T>(this Option<T> option)
        where T : struct
    {
        return option.Match(none: () => default(T?), some: x => x);
    }

    public static TR? ToNullable<T, TR>(this Option<T> item, Func<T, TR> selectFn)
        where TR : struct
    {
        return item.Select(selectFn).ToNullable();
    }
}
