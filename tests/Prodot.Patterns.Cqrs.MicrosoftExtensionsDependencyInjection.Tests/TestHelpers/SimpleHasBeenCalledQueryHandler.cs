namespace Prodot.Patterns.Cqrs.MicrosoftExtensionsDependencyInjection.Tests.TestHelpers;

public class SimpleHasBeenCalledQueryHandler : QueryHandler<UnitQuery, Unit>
{
    public bool HasBeenCalled { get; private set; }

    public override Task<Option<Unit>> RunQueryAsync(
        UnitQuery query,
        CancellationToken cancellationToken
    )
    {
        HasBeenCalled = true;
        return Task.FromResult(Option.From(Unit.Value));
    }
}
