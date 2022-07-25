// Reimplementation of NeverNull
// Taken from https://github.com/Bomret/NeverNull
// Licensed under MIT License by @bomret

using System.Collections;

namespace Prodot.Patterns.Cqrs;

public struct Option<T> : IEquatable<Option<T>>, IStructuralEquatable, IStructuralComparable, IComparable<Option<T>>, IComparable
{
    private readonly T _value;

    internal Option(T value)
            : this()
    {
        _value = value;
        IsSome = true;
    }

    /// <summary>
    ///     Returns an option that represents the absence of a value.
    /// </summary>
    public static Option<T> None => default;

    /// <summary>
    ///     Indicates if this option is empty, i.e. is None.
    ///     Is true if no value is present, false otherwise.
    /// </summary>
    public bool IsNone => !IsSome;

    /// <summary>
    ///     Indicates if this option contains a value, i.e. is Some of T.
    ///     Is true if a value is present, false otherwise.
    /// </summary>
    public bool IsSome { get; }

    /// <summary>
    ///     Implicitly converts the specified None into its generic representation.
    /// </summary>
    public static implicit operator Option<T>(None none) => None;

    /// <summary>
    ///     Implicitly converts the specified value into an Option.
    /// </summary>
    public static implicit operator Option<T>(T value)
        => Option.From(value);

    /// <summary>
    ///     Compares the specified Options for inequality.
    /// </summary>
    public static bool operator !=(Option<T> left, Option<T> right)
        => !((IStructuralEquatable)left).Equals(right, EqualityComparer<object>.Default);

    /// <summary>
    ///     Compares the specified Options for equality.
    /// </summary>
    public static bool operator ==(Option<T> left, Option<T> right)
        => ((IStructuralEquatable)left).Equals(right, EqualityComparer<object>.Default);

    int IStructuralComparable.CompareTo(object? other, IComparer comparer)
    {
        if (other is not Option<T>)
        {
            throw new ArgumentException("Provided object is not of type Option<T>", nameof(other));
        }

        var otherOption = (Option<T>)other;
        if (IsSome && !otherOption.IsSome)
        {
            return 1;
        }

        if (!IsSome && otherOption.IsSome)
        {
            return -1;
        }

        return comparer.Compare(_value, otherOption._value);
    }

    int IComparable<Option<T>>.CompareTo(Option<T> other)
        => ((IStructuralComparable)this).CompareTo(other, Comparer<object>.Default);

    int IComparable.CompareTo(object? obj)
        => ((IStructuralComparable)this).CompareTo(obj, Comparer<object>.Default);

    bool IStructuralEquatable.Equals(object? other, IEqualityComparer comparer)
    {
        if (other is not Option<T>)
        {
            return false;
        }

        var option = (Option<T>)other;
        return (IsSome && option.IsSome && comparer.Equals(_value, option._value))
               || (!IsSome && !option.IsSome);
    }

    /// <summary>
    ///     Compares the specified option with this one for equality.
    /// </summary>
    public bool Equals(Option<T> other)
        => ((IStructuralEquatable)this).Equals(other, EqualityComparer<object>.Default);

    /// <summary>
    ///     Indicates if this instance is equal to the specified object.
    /// </summary>
    public override bool Equals(object? obj)
        => ((IStructuralEquatable)this).Equals(obj, EqualityComparer<object>.Default);

    int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        => CombineHashCodes(comparer.GetHashCode(IsSome), comparer.GetHashCode(_value!));

    /// <summary>
    ///     Returns the calculated hash code for this Option.
    /// </summary>
    public override int GetHashCode()
        => ((IStructuralEquatable)this).GetHashCode(EqualityComparer<object>.Default);

    /// <summary>
    ///     Executes the specified side effect if this is None.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="sideEffect" /> is null.
    /// </exception>
    public void IfNone(Action sideEffect)
    {
        sideEffect.ThrowIfNull(nameof(sideEffect));

        if (IsNone)
        {
            sideEffect();
        }
    }

    /// <summary>
    ///     Executes the specified side effect on the value of this if it contains one.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="sideEffect" /> is null.
    /// </exception>
    public void IfSome(Action<T> sideEffect)
    {
        sideEffect.ThrowIfNull(nameof(sideEffect));

        if (IsSome)
        {
            sideEffect(_value);
        }
    }

    /// <summary>
    ///     Executes a given side effect if this option contains a value, otherwise a different side effect.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="some" /> or <paramref name="none" /> is null.
    /// </exception>
    // ReSharper disable once ParameterHidesMember
    public void Match(Action<T> some, Action none)
    {
        some.ThrowIfNull(nameof(some));
        none.ThrowIfNull(nameof(none));

        if (IsSome)
        {
            some(_value);
        }
        else
        {
            none();
        }
    }

    /// <summary>
    ///     Applies the first selector to the value of this option if it contains one, otherwise executes the second selector.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="some" /> or <paramref name="none" /> is null.
    /// </exception>
    // ReSharper disable once ParameterHidesMember
    public TNew Match<TNew>(Func<T, TNew> some, Func<TNew> none)
    {
        some.ThrowIfNull(nameof(some));
        none.ThrowIfNull(nameof(none));

        return IsSome
            ? some(_value)
            : none();
    }

    /// <summary>
    ///     Returns the string representation of this option.
    /// </summary>
    public override string ToString()
        => IsSome ? $"Some({_value})" : "None";

    private static int CombineHashCodes(int h1, int h2) => ((h1 << 5) + h1) ^ h2;
}
