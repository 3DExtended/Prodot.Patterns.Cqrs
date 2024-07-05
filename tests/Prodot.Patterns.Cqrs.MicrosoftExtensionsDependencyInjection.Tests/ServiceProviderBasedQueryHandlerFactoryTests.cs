using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Prodot.Patterns.Cqrs.MicrosoftExtensionsDependencyInjection.Tests.TestHelpers;
using Xunit;

namespace Prodot.Patterns.Cqrs.MicrosoftExtensionsDependencyInjection.Tests;

public class ServiceProviderBasedQueryHandlerFactoryTests
{
    [Fact]
    public void RegisteredGenericQueryHandlers_CanBeResolvedByTheirType()
    {
        // arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton(typeof(GenericQueryHandler<,>));
        var provider = serviceCollection.BuildServiceProvider();

        var handler = provider.GetRequiredService<GenericQueryHandler<UnitQuery, Unit>>();

        var subjectUnderTest = new ServiceProviderBasedQueryHandlerFactory(provider);

        // act
        var handlerFromFactory = subjectUnderTest.CreateQueryHandler<
            GenericQueryHandler<UnitQuery, Unit>,
            UnitQuery,
            Unit
        >();

        // assert
        ReferenceEquals(handler, handlerFromFactory).Should().BeTrue();
    }

    [Fact]
    public void RegisteredOpenGenericQueryHandlers_CanBeResolvedByTheirInterface()
    {
        // arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton(typeof(IQueryHandler<,>), typeof(GenericQueryHandler<,>));
        var provider = serviceCollection.BuildServiceProvider();

        var handler = provider.GetRequiredService<IQueryHandler<UnitQuery, Unit>>();

        var subjectUnderTest = new ServiceProviderBasedQueryHandlerFactory(provider);

        // act
        var handlerFromFactory = subjectUnderTest.CreateQueryHandler<
            IQueryHandler<UnitQuery, Unit>,
            UnitQuery,
            Unit
        >();

        // assert
        ReferenceEquals(handler, handlerFromFactory).Should().BeTrue();
    }

    [Fact]
    public void RegisteredQueryHandlers_CanBeResolvedByTheirInterface()
    {
        // arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<
            IQueryHandler<UnitQuery, Unit>,
            SimpleHasBeenCalledQueryHandler
        >();
        var provider = serviceCollection.BuildServiceProvider();

        var handler = provider.GetRequiredService<IQueryHandler<UnitQuery, Unit>>();

        var subjectUnderTest = new ServiceProviderBasedQueryHandlerFactory(provider);

        // act
        var handlerFromFactory = subjectUnderTest.CreateQueryHandler<
            IQueryHandler<UnitQuery, Unit>,
            UnitQuery,
            Unit
        >();

        // assert
        ReferenceEquals(handler, handlerFromFactory).Should().BeTrue();
    }

    [Fact]
    public void RegisteredQueryHandlers_CanBeResolvedByTheirType()
    {
        // arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<SimpleHasBeenCalledQueryHandler>();
        var provider = serviceCollection.BuildServiceProvider();

        var handler = provider.GetRequiredService<SimpleHasBeenCalledQueryHandler>();

        var subjectUnderTest = new ServiceProviderBasedQueryHandlerFactory(provider);

        // act
        var handlerFromFactory = subjectUnderTest.CreateQueryHandler<
            SimpleHasBeenCalledQueryHandler,
            UnitQuery,
            Unit
        >();

        // assert
        ReferenceEquals(handler, handlerFromFactory).Should().BeTrue();
    }
}
