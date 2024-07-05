namespace Prodot.Patterns.Cqrs.EfCore.Tests.TestHelpers.Queries;

public class TestModelCreateQuery
    : CreateQuery<TestModel, TestModelId, int, TestModelCreateQuery> { }
