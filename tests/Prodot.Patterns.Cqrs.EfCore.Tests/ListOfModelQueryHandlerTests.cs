namespace Prodot.Patterns.Cqrs.EfCore.Tests;

public class ListOfModelQueryHandlerTests : EfCoreTestBase
{
    [Fact]
    public async Task RunQueryAsync_RetrievesAllEntitiesCorrectly()
    {
        // Arrange
        var entity1 = new TestEntity
        {
            Id = 0,
            StringProperty = "Bla1"
        };
        var entity2 = new TestEntity
        {
            Id = 0,
            StringProperty = "Bla2"
        };
        var entity3 = new TestEntity
        {
            Id = 0,
            StringProperty = "Bla3"
        };

        Context.Entities.Add(entity1);
        Context.Entities.Add(entity2);
        Context.Entities.Add(entity3);
        Context.SaveChanges();

        var query = new TestModelsQuery
        {
            Ids = Option.None
        };
        var subjectUnderTest = new TestModelsQueryHandler(Mapper, ContextFactory);

        // Act
        var result = await subjectUnderTest.RunQueryAsync(query, default);

        // Assert
        result.IsSome.Should().BeTrue("because the retrieval should be successful");
        result.Get().Should().HaveCount(3);
    }

    [Fact]
    public async Task RunQueryAsync_RetrievesSelectedEntitiesCorrectly()
    {
        // Arrange
        var entity1 = new TestEntity
        {
            Id = 0,
            StringProperty = "Bla1"
        };
        var entity2 = new TestEntity
        {
            Id = 0,
            StringProperty = "Bla2"
        };
        var entity3 = new TestEntity
        {
            Id = 0,
            StringProperty = "Bla3"
        };

        Context.Entities.Add(entity1);
        Context.Entities.Add(entity2);
        Context.Entities.Add(entity3);
        Context.SaveChanges();

        var query = new TestModelsQuery
        {
            Ids = new List<TestModelId> { TestModelId.From(entity2.Id), TestModelId.From(entity3.Id) }
        };
        var subjectUnderTest = new TestModelsQueryHandler(Mapper, ContextFactory);

        // Act
        var result = await subjectUnderTest.RunQueryAsync(query, default);

        // Assert
        result.IsSome.Should().BeTrue("because the retrieval should be successful");
        result.Get().Should().HaveCount(2);

        result.Get().First().Id.Should().Be(TestModelId.From(entity2.Id));
        result.Get().First().StringProperty.Should().Be("Bla2");

        result.Get().Last().Id.Should().Be(TestModelId.From(entity3.Id));
        result.Get().Last().StringProperty.Should().Be("Bla3");
    }
}
