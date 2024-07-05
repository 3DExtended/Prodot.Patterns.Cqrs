namespace Prodot.Patterns.Cqrs.EfCore.Tests.TestHelpers;

public class TestModel : TestModelBase<TestModelId>
{
    public string StringProperty { get; set; } = string.Empty;
}
