// Reimplementation of NeverNull
// Taken from https://github.com/Bomret/NeverNull
// Licensed under MIT License by @bomret

namespace Prodot.Patterns.Cqrs;

/// <summary>
///     Provides extension methods for working with instances of option in conjunction with instances of IEnumerable.
/// </summary>
public static class EnumerableExt
{
    /// <summary>
    ///     Aggregates the values of the specified enumerable using the given fold function.
    ///     If the enumerable is empty or all values are null, None is returned.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     The enumerable argument or fold is null.
    /// </exception>
    public static Option<T> AggregateOptional<T>(this IEnumerable<T> enumerable, Func<T, T, T> fold)
    {
        enumerable.ThrowIfNull(nameof(enumerable));
        fold.ThrowIfNull(nameof(fold));

        return enumerable.Aggregate(
            Option<T>.None,
            (accu, current) =>
            {
                var currentOption = Option.From(current);

                return accu.Match(
                    none: () => currentOption,
                    some: previousValue =>
                        currentOption.Match(
                            none: () => previousValue,
                            some: currentValue => fold(previousValue, currentValue)
                        )
                );
            }
        );
    }

    /// <summary>
    ///    Aggregates the values of the specified enumerable using the given fold function.
    ///     If the enumerable is empty or all values are null, None is returned.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     The enumerable argument or fold is null.
    /// </exception>
    public static Option<T> AggregateOptionalNullable<T>(
        this IEnumerable<T?> enumerable,
        Func<T, T, T> fold
    )
        where T : struct
    {
        enumerable.ThrowIfNull(nameof(enumerable));
        fold.ThrowIfNull(nameof(fold));

        return enumerable.Aggregate(
            Option<T>.None,
            (accu, current) =>
            {
                var currentOption = Option.From(current);

                return accu.Match(
                    none: () => currentOption,
                    some: previousValue =>
                        currentOption.Match(
                            none: () => previousValue,
                            some: currentValue => fold(previousValue, currentValue)
                        )
                );
            }
        );
    }

    /// <summary>
    ///     Returns an option containing an IEnumerable with all values or None, if any of
    ///     the options in the specified enumerable does not contain a value or the enumerable is empty.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     The enumerable argument is null.
    /// </exception>
    public static Option<IEnumerable<T>> AllOrNone<T>(this IEnumerable<Option<T>> enumerable)
    {
        enumerable.ThrowIfNull(nameof(enumerable));

        var results = new List<T>();
        foreach (var option in enumerable)
        {
            if (option.IsNone)
            {
                return Option.None;
            }

            option.IfSome(x => results.Add(x));
        }

        return results.Count == 0 ? Option.None : Option.From(results.AsEnumerable());
    }

    /// <summary>
    ///     Returns an option containing an IEnumerable with all values or None, if any of
    ///     the options in the specified enumerable does not contain a value or the enumerable is empty.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     The enumerable argument is null.
    /// </exception>
    public static Option<IEnumerable<T>> AllOrNone<T>(this IEnumerable<Option<T?>> enumerable)
        where T : struct
    {
        enumerable.ThrowIfNull(nameof(enumerable));

        var results = new List<T>();
        foreach (var option in enumerable)
        {
            if (option.IsNone)
            {
                return Option.None;
            }

            option.IfSome(x =>
            {
                if (x.HasValue)
                {
                    results.Add(x.Value);
                }
            });
        }

        return results.Count == 0 ? Option.None : Option.From(results.AsEnumerable());
    }

    /// <summary>
    ///     Returns all values in the specified option of IEnumerable as options, if it contains an enumerable.
    ///     If the contained enumerable is empty or None is provided, an empty enumerable is returned.
    /// </summary>
    public static IEnumerable<Option<T>> Exchange<T>(
        this Option<IEnumerable<T>> optionalEnumerable
    ) =>
        optionalEnumerable.Match(
            none: Enumerable.Empty<Option<T>>,
            some: xs => xs.Select(Option.From)
        );

    /// <summary>
    ///     Returns all values in the specified option of IEnumerable as options, if it contains an enumerable.
    ///     If the contained IEnumerable is empty or None is provided, an empty enumerable is returned.
    /// </summary>
    public static IEnumerable<Option<T>> Exchange<T>(
        this Option<IEnumerable<T?>> optionalEnumerable
    )
        where T : struct =>
        optionalEnumerable.Match(
            none: Enumerable.Empty<Option<T>>,
            some: xs => xs.Select(Option.From)
        );

    /// <summary>
    ///     Returns all values in the specified option of Array as options, if it contains an array.
    ///     If the contained array is empty or None is provided, an empty array is returned.
    /// </summary>
    public static Option<T>[] Exchange<T>(this Option<T[]> optionalArray) =>
        optionalArray.Match(
            none: () => new Option<T>[0],
            some: xs => xs.Select(Option.From).ToArray()
        );

    /// <summary>
    ///     Returns all values in the specified option of Array as options, if it contains an array.
    ///     If the contained array is empty or None is provided, an empty array is returned.
    /// </summary>
    public static Option<T>[] Exchange<T>(this Option<T?[]> optionalArray)
        where T : struct =>
        optionalArray.Match(
            none: () => new Option<T>[0],
            some: xs => xs.Select(Option.From).ToArray()
        );

    /// <summary>
    ///     Returns the first value of the specified enumerable wrapped in an option.
    ///     If the enumerable is empty or the first value is null, None is returned.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     The enumerable argument is null.
    /// </exception>
    public static Option<T> FirstOptional<T>(this IEnumerable<T> enumerable)
    {
        enumerable.ThrowIfNull(nameof(enumerable));

        foreach (var value in enumerable)
        {
            return Option.From(value);
        }

        return Option.None;
    }

    /// <summary>
    ///     Returns the first value of the specified enumerable wrapped in an option.
    ///     If the enumerable is empty or the first value is null, None is returned.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     The enumerable argument is null.
    /// </exception>
    public static Option<T> FirstOptional<T>(this IEnumerable<T?> enumerable)
        where T : struct
    {
        enumerable.ThrowIfNull(nameof(enumerable));

        foreach (var value in enumerable)
        {
            return Option.From(value);
        }

        return Option.None;
    }

    /// <summary>
    ///     Returns the last value of the specified enumerable wrapped in an option.
    ///     If the enumerable is empty or the last value is null, None is returned.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     The enumerable argument is null.
    /// </exception>
    public static Option<T> LastOptional<T>(this IEnumerable<T> enumerable)
    {
        enumerable.ThrowIfNull(nameof(enumerable));

        var xs = enumerable.ToList();
        return xs.Count == 0 ? Option.None : Option.From(xs[xs.Count - 1]);
    }

    /// <summary>
    ///     Returns the last value of the specified enumerable wrapped in an option.
    ///     If the enumerable is empty or the last value is null, None is returned.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     The enumerable argument is null.
    /// </exception>
    public static Option<T> LastOptional<T>(this IEnumerable<T?> enumerable)
        where T : struct
    {
        enumerable.ThrowIfNull(nameof(enumerable));

        var xs = enumerable.ToList();
        return xs.Count == 0 ? Option.None : Option.From(xs[xs.Count - 1]);
    }

    /// <summary>
    ///     Selects all values from the options inside the specified enumerable that contain values.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     The enumerable argument is null.
    /// </exception>
    public static IEnumerable<T> SelectValues<T>(this IEnumerable<Option<T>> enumerable)
    {
        enumerable.ThrowIfNull(nameof(enumerable));

        return enumerable
            .Select(o =>
                o.Match(
                    none: () => new { hasVal = false, val = default(T) },
                    some: x => new { hasVal = true, val = (T?)x }
                )
            )
            .Where(o => o.hasVal)
            .Select(o => o.val!);
    }

    /// <summary>
    ///     Selects all values from the options inside the specified enumerable that contain values.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     The enumerable argument is null.
    /// </exception>
    public static IEnumerable<T> SelectValues<T>(this IEnumerable<Option<T?>> enumerable)
        where T : struct
    {
        enumerable.ThrowIfNull(nameof(enumerable));

        return enumerable
            .Select(o =>
                o.Match(
                    none: () => new { hasVal = false, val = default(T?) },
                    some: x => new { hasVal = x.HasValue, val = x }
                )
            )
            .Where(o => o.hasVal)
            .Select(o => o.val!.Value);
    }

    /// <summary>
    ///     Returns the only element in the specified enumerable wrapped in an option.
    ///     If the enumerable is empty or the single element is null, None is returned.
    ///     Throws an exception if the enumerable contains more than one element.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     The enumerable argument is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     The enumerable contains more than one element.
    /// </exception>
    public static Option<T> SingleOptional<T>(this IEnumerable<T> enumerable)
    {
        enumerable.ThrowIfNull(nameof(enumerable));

        var xs = enumerable.ToList();
        return xs.Count == 0 ? Option.None : Option.From(xs.SingleOrDefault()!);
    }

    /// <summary>
    ///     Returns the only element in the specified enumerable wrapped in an option.
    ///     If the enumerable is empty or the single element is null, None is returned.
    ///     Throws an exception if the enumerable contains more than one element.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     The enumerable argument is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     The enumerable contains more than one element.
    /// </exception>
    public static Option<T> SingleOptional<T>(this IEnumerable<T?> enumerable)
        where T : struct
    {
        enumerable.ThrowIfNull(nameof(enumerable));

        var xs = enumerable.ToList();
        return xs.Count == 0 ? Option.None : Option.From(xs.SingleOrDefault());
    }

    /// <summary>
    ///     Returns the only element in the specified enumerable that matches a predicate wrapped in an option.
    ///     If the enumerable is empty or the single element is null or the predicate does not match any element, None is returned.
    ///     Throws an exception if the enumerable contains more than one element.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     The enumerable or predicate is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     The enumerable contains more than one element.
    /// </exception>
    public static Option<T> SingleOptional<T>(
        this IEnumerable<T> enumerable,
        Func<T, bool> predicate
    )
    {
        enumerable.ThrowIfNull(nameof(enumerable));
        predicate.ThrowIfNull(nameof(predicate));

        var xs = enumerable.ToList();
        return xs.Count == 0 ? Option.None : Option.From(xs.SingleOrDefault(predicate)!);
    }

    /// <summary>
    ///     Returns the only element in the specified enumerable that matches a predicate wrapped in an Option.
    ///     If enumerable is empty or the single element is null or the predicate does not match any element, None is returned.
    ///     Throws an exception if enumerable contains more than one element.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     The enumerable argument or predicate is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     The enumerable contains more than one element.
    /// </exception>
    public static Option<T> SingleOptionalNullable<T>(
        this IEnumerable<T?> enumerable,
        Func<T, bool> predicate
    )
        where T : struct
    {
        enumerable.ThrowIfNull(nameof(enumerable));
        predicate.ThrowIfNull(nameof(predicate));

        var xs = enumerable.ToList();
        return xs.Count == 0
            ? Option.None
            : Option.From(xs.SingleOrDefault(x => x.HasValue && predicate(x.Value)));
    }
}
