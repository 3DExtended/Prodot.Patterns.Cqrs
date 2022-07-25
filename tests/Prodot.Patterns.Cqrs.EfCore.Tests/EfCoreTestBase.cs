using AutoMapper;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Prodot.Patterns.Cqrs.EfCore.Tests;

public abstract class EfCoreTestBase : IDisposable
{
    private bool _isDisposed;
    private string _runIdentifier;

    protected EfCoreTestBase()
    {
        _runIdentifier = Guid.NewGuid().ToString();

        ServiceProvider = new ServiceCollection()
            .AddDbContextFactory<TestDbContext>(o => o.UseSqlite($"DataSource=file:memdb{_runIdentifier}?mode=memory&cache=shared"))
            .BuildServiceProvider();

        Mapper = new MapperConfiguration(o =>
            {
                o.CreateMap<TestModel, TestEntity>().ReverseMap();
                o.CreateMap<TestEntity, TestEntity>();
                o.CreateMap<TestModelId, int>()
                    .ConvertUsing((tmid, _) => tmid.Value);
                o.CreateMap<int, TestModelId>()
                    .ConvertUsing((id, _) => TestModelId.From(id));
            })
            .CreateMapper();

        // initialize test database
        ContextFactory = ServiceProvider.GetRequiredService<IDbContextFactory<TestDbContext>>();
        Context = ContextFactory.CreateDbContext();
        Context.Database.OpenConnection();
        Context.Database.EnsureCreated();
    }

    protected TestDbContext Context { get; }

    protected IDbContextFactory<TestDbContext> ContextFactory { get; }

    protected IMapper Mapper { get; }

    protected IServiceProvider ServiceProvider { get; }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed)
        {
            return;
        }

        if (disposing)
        {
            // free managed resources
            Context.Database.CloseConnection();
            Context.Dispose();
        }

        _isDisposed = true;
    }
}
