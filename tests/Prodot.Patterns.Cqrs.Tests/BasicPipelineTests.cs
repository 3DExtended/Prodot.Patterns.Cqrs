namespace Prodot.Patterns.Cqrs.Tests;

public class BasicPipelineTests
{
    [Fact]
    public async Task SinglePartPipeline_GetsBuildAndCalledCorrectly()
    {
        // Arrange
        var callCounter = 0;
        var preCall = 0;
        var postCall = 0;

        var pipeline = new PipelineBuilder<UnitQuery, Unit>()
            .With<CallbackQueryHandler<UnitQuery, Unit>>()
            .Build();
        var registry = QueryHandlerRegistry.Builder()
            .AddRegisterCallback(r =>
            {
                r(pipeline);
            })
            .Build();
        var factory = new CallbackQueryHandlerFactory(() => preCall = Interlocked.Increment(ref callCounter),
            () => postCall = Interlocked.Increment(ref callCounter));

        var subjectUnderTest = new QueryProcessor(registry, factory);

        // Act
        var result = await new UnitQuery().RunAsync(subjectUnderTest, default);

        // Assert
        callCounter.Should().Be(2);
        preCall.Should().BeLessThan(postCall);
    }

    [Fact]
    public async Task SinglePartPipelineWithConfiguration_GetsBuildAndCalledCorrectly()
    {
        // Arrange
        var callCounter = 0;
        var preCall = 0;
        var postCall = 0;

        var pipeline = new PipelineBuilder<UnitQuery, Unit>()
            .With<ConfigurableCallbackQueryHandler<UnitQuery, Unit>, bool>(true)
            .Build();
        var registry = QueryHandlerRegistry.Builder()
            .AddRegisterCallback(r =>
            {
                r(pipeline);
            })
            .Build();
        var factory = new CallbackQueryHandlerFactory(config =>
            {
                config.Should().Be(true);
                preCall = Interlocked.Increment(ref callCounter);
            },
            config =>
            {
                config.Should().Be(true);
                postCall = Interlocked.Increment(ref callCounter);
            });

        var subjectUnderTest = new QueryProcessor(registry, factory);

        // Act
        var result = await subjectUnderTest.RunQueryAsync(new UnitQuery(), default);

        // Assert
        callCounter.Should().Be(2);
        preCall.Should().BeLessThan(postCall);
    }
}
