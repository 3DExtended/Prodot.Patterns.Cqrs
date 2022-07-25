using Microsoft.EntityFrameworkCore;

namespace Prodot.Patterns.Cqrs.EfCore.Tests;

public class UpdateCommandHandlerTests : EfCoreTestBase
{
    [Fact]
    public async Task RunQueryAsync_UpdatesEntityCorrectly()
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

        var query = new TestModelUpdateCommand
        {
            UpdatedModel = new()
            {
                Id = TestModelId.From(entity2.Id),
                StringProperty = "Foo"
            }
        };
        var subjectUnderTest = new TestModelUpdateCommandHandler(Mapper, ContextFactory);

        // Act
        var result = await subjectUnderTest.RunQueryAsync(query, default);

        // Assert
        result.IsSome.Should().BeTrue("because the update should be successful");
        var entityCount = Context.Entities.Count();
        entityCount.Should().Be(3);

        var updatedEntity = Context.Entities.AsNoTracking().First(x => x.Id == entity2.Id);
        updatedEntity!.StringProperty.Should().Be("Foo");
    }
}
