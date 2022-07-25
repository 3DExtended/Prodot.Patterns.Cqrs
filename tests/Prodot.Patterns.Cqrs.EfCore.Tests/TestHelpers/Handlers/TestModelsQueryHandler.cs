using AutoMapper;

using Microsoft.EntityFrameworkCore;

namespace Prodot.Patterns.Cqrs.EfCore.Tests.TestHelpers.Handlers;

public class TestModelsQueryHandler : ListOfModelQueryHandlerBase<TestModelsQuery, TestModel, TestModelId, int, TestDbContext, TestEntity>
{
    public TestModelsQueryHandler(IMapper mapper, IDbContextFactory<TestDbContext> contextFactory)
        : base(mapper, contextFactory)
    {
    }
}
