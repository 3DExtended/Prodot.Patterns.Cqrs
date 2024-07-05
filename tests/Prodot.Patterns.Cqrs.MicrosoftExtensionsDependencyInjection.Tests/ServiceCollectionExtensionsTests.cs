using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Prodot.Patterns.Cqrs.MicrosoftExtensionsDependencyInjection.Tests.TestHelpers;
using Xunit;

namespace Prodot.Patterns.Cqrs.MicrosoftExtensionsDependencyInjection.Tests;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void QueryProcessor_CanBeResolvedAndUsedToRunGenericQueryHandlerForGenericQueryIfExplicitlyRegistered()
    {
        // arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddProdotPatternsCqrs(o =>
            o.WithQueryHandlersFrom(typeof(SimpleHasBeenCalledQueryHandler).Assembly)
                .WithQueryHandlerPipelineConfiguration(c =>
                    c.AddProfiles(typeof(ServiceCollectionExtensionsTests).Assembly)
                )
        );

        var provider = serviceCollection.BuildServiceProvider();

        var subjectUnderTest = provider.GetRequiredService<IQueryProcessor>();

        // act
        var result = new GenericQuery<string>().RunAsyncWithDefaultExceptionIfNone(
            subjectUnderTest,
            default
        );

        // assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void QueryProcessor_CanBeResolvedAndUsedToRunGenericQueryHandlerForUnitQueryIfExplicitlyRegistered()
    {
        // arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddProdotPatternsCqrs(o =>
            o.WithQueryHandlersFrom(typeof(SimpleHasBeenCalledQueryHandler).Assembly)
                .WithQueryHandlerPipelineConfiguration(c =>
                    c.AddProfiles(typeof(ServiceCollectionExtensionsTests).Assembly)
                )
        );

        var provider = serviceCollection.BuildServiceProvider();

        var subjectUnderTest = provider.GetRequiredService<IQueryProcessor>();

        // act
        var result = new UnitQuery().RunAsyncWithDefaultExceptionIfNone(subjectUnderTest, default);

        // assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void QueryProcessor_CanBeResolvedAndUsedToRunQuery()
    {
        // arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddProdotPatternsCqrs(o =>
            o.WithQueryHandlersFrom(typeof(SimpleHasBeenCalledQueryHandler).Assembly)
                .WithQueryHandlerPipelineConfiguration(c => c.WithPipelineAutoRegistration())
        );

        var provider = serviceCollection.BuildServiceProvider();

        var subjectUnderTest = provider.GetRequiredService<IQueryProcessor>();

        // act
        var result = new UnitQuery().RunAsyncWithDefaultExceptionIfNone(subjectUnderTest, default);

        // assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void RegisteredGenericQueryHandlers_CanBeResolvedByConcreteGenericImplementationTypeFromRegisteredFactory()
    {
        // arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddProdotPatternsCqrs(o =>
            o.WithQueryHandlersFrom(typeof(SimpleHasBeenCalledQueryHandler).Assembly)
                .WithQueryHandlerPipelineConfiguration(c => c.WithPipelineAutoRegistration())
        );

        var provider = serviceCollection.BuildServiceProvider();

        var handler = provider.GetRequiredService<GenericQueryHandler<UnitQuery, Unit>>();

        var subjectUnderTest = provider.GetRequiredService<IQueryHandlerFactory>();

        // act
        var handlerFromFactory = subjectUnderTest.CreateQueryHandler<
            GenericQueryHandler<UnitQuery, Unit>,
            UnitQuery,
            Unit
        >();

        // assert
        ReferenceEquals(handler, handlerFromFactory)
            .Should()
            .BeFalse("because query handlers are registered as transient");
    }

    [Fact]
    public void RegisteredGenericQueryHandlers_CanNotBeResolvedByInterfaceTypeFromRegisteredFactory()
    {
        // arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddProdotPatternsCqrs(o =>
            o.WithQueryHandlersFrom(typeof(SimpleHasBeenCalledQueryHandler).Assembly)
                .WithQueryHandlerPipelineConfiguration(c => c.WithPipelineAutoRegistration())
        );

        var provider = serviceCollection.BuildServiceProvider();

        var subjectUnderTest = provider.GetRequiredService<IQueryHandlerFactory>();

        // act
        var action = () =>
            subjectUnderTest.CreateQueryHandler<
                IQueryHandler<GenericQuery<string>, string>,
                GenericQuery<string>,
                string
            >();

        // assert
        action
            .Should()
            .Throw<InvalidOperationException>(
                "because open generic implementations like GenericQueryHandler can not be resolved by interface"
            );
    }

    [Fact]
    public void RegisteredQueryHandlers_CanBeResolvedByTheirConcreteTypeFromRegisteredFactory()
    {
        // arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddProdotPatternsCqrs(o =>
            o.WithQueryHandlersFrom(typeof(SimpleHasBeenCalledQueryHandler).Assembly)
                .WithQueryHandlerPipelineConfiguration(c => c.WithPipelineAutoRegistration())
        );

        var provider = serviceCollection.BuildServiceProvider();

        var handler = provider.GetRequiredService<SimpleHasBeenCalledQueryHandler>();

        var subjectUnderTest = provider.GetRequiredService<IQueryHandlerFactory>();

        // act
        var handlerFromFactory = subjectUnderTest.CreateQueryHandler<
            SimpleHasBeenCalledQueryHandler,
            UnitQuery,
            Unit
        >();

        // assert
        ReferenceEquals(handler, handlerFromFactory)
            .Should()
            .BeFalse("because query handlers are registered as transient");
    }

    [Fact]
    public void RegisteredQueryHandlers_CanBeResolvedByTheirInterfaceFromRegisteredFactory()
    {
        // arrange
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddProdotPatternsCqrs(o =>
            o.WithQueryHandlersFrom(typeof(SimpleHasBeenCalledQueryHandler).Assembly)
                .WithQueryHandlerPipelineConfiguration(c => c.WithPipelineAutoRegistration())
        );

        var provider = serviceCollection.BuildServiceProvider();

        var handler = provider.GetRequiredService<IQueryHandler<UnitQuery, Unit>>();

        var subjectUnderTest = provider.GetRequiredService<IQueryHandlerFactory>();

        // act
        var handlerFromFactory = subjectUnderTest.CreateQueryHandler<
            IQueryHandler<UnitQuery, Unit>,
            UnitQuery,
            Unit
        >();

        // assert
        ReferenceEquals(handler, handlerFromFactory)
            .Should()
            .BeFalse("because query handlers are registered as transient");
    }
}
