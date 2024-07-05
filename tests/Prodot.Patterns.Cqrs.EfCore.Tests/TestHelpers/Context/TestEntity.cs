namespace Prodot.Patterns.Cqrs.EfCore.Tests.TestHelpers.Context;

public class TestEntity : IIdentifiableEntity<int>
{
    public int Id { get; set; }

    public string StringProperty { get; set; } = string.Empty;
}
