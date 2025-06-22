// Reimplementation of NeverNull
// Taken from https://github.com/Bomret/NeverNull
// Licensed under MIT License by @bomret

using FsCheck.Fluent;

namespace Prodot.Patterns.Cqrs.Tests.Options;

public class ToOptionExtTests
{
    [Fact]
    public void Converting_nullable_values_to_options_should_yield_None_for_null_otherwise_a_Some_containing_the_value()
    {
        Prop.ForAll<int?>(x => x.ToOption().Equals(Option.From(x))).QuickCheckThrowOnFailure();
    }

    [Fact]
    public void Converting_nullable_values_to_options_with_custom_mapping_and_magic_null_value_should_yield_None_for_null_otherwise_a_Some_containing_the_value()
    {
        Prop.ForAll<int>(x =>
                x.ToOptionMappedOrNoneIf(1, CustomType.Create)
                    .Equals(x == 1 ? Option.None : x.ToOption().Select(CustomType.Create))
            )
            .QuickCheckThrowOnFailure();
    }

    [Fact]
    public void Converting_nullable_values_to_options_with_custom_mapping_should_yield_None_for_null_otherwise_a_Some_containing_the_value()
    {
        Prop.ForAll<int?>(x =>
                x.ToOptionMapped(CustomType.Create).Equals(x.ToOption().Select(CustomType.Create))
            )
            .QuickCheckThrowOnFailure();
    }

    [Fact]
    public void Converting_reference_values_to_options_should_yield_None_for_null_otherwise_a_Some_containing_the_value()
    {
        Prop.ForAll<string>(x => x.ToOption().Equals(Option.From(x))).QuickCheckThrowOnFailure();
    }

    [Fact]
    public void Converting_values_to_options_should_yield_None_for_null_otherwise_a_Some_containing_the_value()
    {
        Prop.ForAll<int>(x => x.ToOption().Equals(x)).QuickCheckThrowOnFailure();
    }

    private class CustomType
    {
        public int Value { get; private set; }

        public static CustomType Create(int i)
        {
            return new CustomType { Value = i };
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((CustomType)obj);
        }

        public override int GetHashCode()
        {
            return Value;
        }

        protected bool Equals(CustomType other)
        {
            return Value == other.Value;
        }
    }
}
