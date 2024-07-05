namespace Prodot.Patterns.Cqrs.EfCore.Tests;

public class DeleteCommandHandlerTests : EfCoreTestBase
{
    [Fact]
    public async Task RunQueryAsync_DeletesEntityCorrectly()
    {
        // Arrange
        var entity1 = new TestEntity { Id = 0, StringProperty = "Bla1" };
        var entity2 = new TestEntity { Id = 0, StringProperty = "Bla2" };
        var entity3 = new TestEntity { Id = 0, StringProperty = "Bla3" };

        Context.Entities.Add(entity1);
        Context.Entities.Add(entity2);
        Context.Entities.Add(entity3);
        Context.SaveChanges();

        var query = new TestModelDeleteCommand { Id = TestModelId.From(entity2.Id) };
        var subjectUnderTest = new TestModelDeleteCommandHandler(ContextFactory);

        // Act
        var result = await subjectUnderTest.RunQueryAsync(query, default);

        // Assert
        result.IsSome.Should().BeTrue("because the deletion should be successful");
        var entityCount = Context.Entities.Count();
        entityCount.Should().Be(2);
    }
}
