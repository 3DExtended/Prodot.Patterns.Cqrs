using Microsoft.Extensions.DependencyInjection;

namespace Prodot.Patterns.Cqrs.Tests;

public class AutoRegistrationTests
{
    [Fact]
    public async Task AutoRegisteredPipeline_GetsBuildAndCalledCorrectly()
    {
        // Arrange
        var registry = QueryHandlerRegistry.Builder().WithPipelineAutoRegistration().Build();
        var factory = new DependencyInjectionQueryHandlerFactory();
        factory.Services.AddSingleton<
            IQueryHandler<UnitQuery, Unit>,
            SimpleHasBeenCalledQueryHandler
        >();

        var subjectUnderTest = new QueryProcessor(registry, factory);

        // Act
        var result = await new UnitQuery().RunAsync(subjectUnderTest, default);

        // Assert
        var handler = (SimpleHasBeenCalledQueryHandler)
            factory.CreateQueryHandler<IQueryHandler<UnitQuery, Unit>, UnitQuery, Unit>();
        handler.HasBeenCalled.Should().BeTrue();
    }
}
