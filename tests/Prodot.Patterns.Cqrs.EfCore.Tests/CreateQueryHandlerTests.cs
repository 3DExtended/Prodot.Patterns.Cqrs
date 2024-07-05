namespace Prodot.Patterns.Cqrs.EfCore.Tests;

public class CreateQueryHandlerTests : EfCoreTestBase
{
    [Fact]
    public async Task RunQueryAsync_CreatesModelSuccessfully()
    {
        // Arrange
        var model = new TestModel { Id = TestModelId.From(0), StringProperty = "Bla" };
        var query = new TestModelCreateQuery { ModelToCreate = model, };
        var subjectUnderTest = new TestModelCreateQueryHandler(Mapper, ContextFactory);

        // Act
        var result = await subjectUnderTest.RunQueryAsync(query, default);

        // Assert
        result.IsSome.Should().BeTrue("because the creation should be successful");
        var entities = Context.Entities.ToList();
        entities.Should().HaveCount(1);
        entities[0].StringProperty.Should().Be("Bla");
    }
}
