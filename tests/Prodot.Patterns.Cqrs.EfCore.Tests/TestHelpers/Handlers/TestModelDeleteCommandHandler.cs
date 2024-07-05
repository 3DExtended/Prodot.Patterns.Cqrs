using Microsoft.EntityFrameworkCore;

namespace Prodot.Patterns.Cqrs.EfCore.Tests.TestHelpers.Handlers;

public class TestModelDeleteCommandHandler
    : DeleteCommandHandlerBase<
        TestModelDeleteCommand,
        TestModel,
        TestModelId,
        int,
        TestDbContext,
        TestEntity
    >
{
    public TestModelDeleteCommandHandler(IDbContextFactory<TestDbContext> contextFactory)
        : base(contextFactory) { }
}
