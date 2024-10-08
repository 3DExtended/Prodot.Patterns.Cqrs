﻿// Reimplementation of NeverNull
// Taken from https://github.com/Bomret/NeverNull
// Licensed under MIT License by @bomret

namespace Prodot.Patterns.Cqrs;

/// <summary>
///   Provides extension methods to wrap values into instances of Option.
/// </summary>
public static class ToOptionExt
{
    /// <summary>
    ///   Wraps this value in a Option.
    /// </summary>
    public static Option<T> ToOption<T>(this T value)
    {
        return Option.From(value);
    }

    /// <summary>
    ///   Wraps the value of this Nullable in a option or returns None.
    /// </summary>
    public static Option<T> ToOption<T>(this T? nullable)
        where T : struct
    {
        return Option.From(nullable);
    }

    public static Option<TR> ToOptionMapped<T, TR>(this T? item, Func<T, TR> mapFn)
        where T : struct
    {
        return item.ToOption().Select(mapFn);
    }

    public static Option<TR> ToOptionMappedOrNoneIf<T, TR>(
        this T item,
        T nullValue,
        Func<T, TR> mapFn
    )
        where T : struct
    {
        return item.Equals(nullValue) ? Option<TR>.None : mapFn(item);
    }
}
