namespace Prodot.Patterns.Cqrs.EfCore.Tests;

public class CountQueryHandlerTests : EfCoreTestBase
{
    [Fact]
    public async Task RunQueryAsync_ReturnsCorrectCount()
    {
        // Arrange
        var entity1 = new TestEntity { Id = 0, StringProperty = "Bla1" };
        var entity2 = new TestEntity { Id = 0, StringProperty = "Bla2" };
        var entity3 = new TestEntity { Id = 0, StringProperty = "Bla3" };

        Context.Entities.Add(entity1);
        Context.Entities.Add(entity2);
        Context.Entities.Add(entity3);
        Context.SaveChanges();

        var query = new TestModelCountQuery();
        var subjectUnderTest = new TestModelCountQueryHandler(ContextFactory);

        // Act
        var result = await subjectUnderTest.RunQueryAsync(query, default);

        // Assert
        result.IsSome.Should().BeTrue("because the query should be successful");
        result.Get().Should().Be(3);
    }
}
