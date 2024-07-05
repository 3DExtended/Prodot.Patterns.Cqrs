namespace Prodot.Patterns.Cqrs.EfCore.Tests.TestHelpers;

public abstract class TestModelBase<TIdentifier> : ModelBase<TIdentifier, int>
    where TIdentifier : Identifier<int, TIdentifier>, new() { }
