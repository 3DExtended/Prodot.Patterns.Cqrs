using System.Linq.Expressions;
using System.Reflection;

using Prodot.Patterns.Cqrs.EfCore.Abstractions;

namespace Prodot.Patterns.Cqrs.EfCore;

public record Identifier<TIdentifierValue, TSelf> : IIdentifier<TIdentifierValue>
    where TSelf : Identifier<TIdentifierValue, TSelf>, new()
{
    private static readonly Func<TSelf> _valueFactory;

    static Identifier()
    {
        var ctor = typeof(TSelf)
            .GetTypeInfo()
            .DeclaredConstructors
            .First(c => c.GetParameters().Length == 0);

        var argsExp = new Expression[0];
        var newExp = Expression.New(ctor, argsExp);
        var lambda = Expression.Lambda(typeof(Func<TSelf>), newExp);

        _valueFactory = (Func<TSelf>)lambda.Compile();
    }

    public TIdentifierValue Value { get; set; } = default!;

    public static TSelf From(TIdentifierValue item)
    {
        var x = _valueFactory();
        x.Value = item;

        return x;
    }
}
