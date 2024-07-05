namespace Prodot.Patterns.Cqrs.EfCore.Tests.TestHelpers.Queries;

public class TestModelUpdateCommand
    : UpdateCommand<TestModel, TestModelId, int, TestModelUpdateCommand> { }
