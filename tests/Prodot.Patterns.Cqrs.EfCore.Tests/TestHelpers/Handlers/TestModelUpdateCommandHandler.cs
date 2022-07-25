using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace Prodot.Patterns.Cqrs.EfCore.Tests.TestHelpers.Handlers;

public class TestModelUpdateCommandHandler : UpdateCommandHandlerBase<TestModelUpdateCommand, TestModel, TestModelId, int, TestDbContext, TestEntity>
{
    public TestModelUpdateCommandHandler(IMapper mapper, IDbContextFactory<TestDbContext> contextFactory)
        : base(mapper, contextFactory)
    {
    }
}
