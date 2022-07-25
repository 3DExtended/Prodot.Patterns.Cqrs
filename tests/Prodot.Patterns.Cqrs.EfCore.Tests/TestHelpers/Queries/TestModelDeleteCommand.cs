namespace Prodot.Patterns.Cqrs.EfCore.Tests.TestHelpers.Queries;

public class TestModelDeleteCommand : DeleteCommand<TestModel, TestModelId, int, TestModelDeleteCommand>
{
}
