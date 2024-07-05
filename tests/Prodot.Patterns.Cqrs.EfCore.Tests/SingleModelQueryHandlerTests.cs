namespace Prodot.Patterns.Cqrs.EfCore.Tests;

public class SingleModelQueryHandlerTests : EfCoreTestBase
{
    [Fact]
    public async Task RunQueryAsync_RetrievesEntityCorrectly()
    {
        // Arrange
        var entity1 = new TestEntity { Id = 0, StringProperty = "Bla1" };
        var entity2 = new TestEntity { Id = 0, StringProperty = "Bla2" };
        var entity3 = new TestEntity { Id = 0, StringProperty = "Bla3" };

        Context.Entities.Add(entity1);
        Context.Entities.Add(entity2);
        Context.Entities.Add(entity3);
        Context.SaveChanges();

        var query = new TestModelQuery { ModelId = TestModelId.From(entity2.Id) };
        var subjectUnderTest = new TestModelQueryHandler(Mapper, ContextFactory);

        // Act
        var result = await subjectUnderTest.RunQueryAsync(query, default);

        // Assert
        result.IsSome.Should().BeTrue("because the retrieval should be successful");
        result.Get().Id.Should().Be(TestModelId.From(entity2.Id));
        result.Get().StringProperty.Should().Be("Bla2");
    }
}
