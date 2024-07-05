namespace Prodot.Patterns.Cqrs.MicrosoftExtensionsDependencyInjection.Tests.TestHelpers;

public class PipelineProfile : IPipelineProfile
{
    public void RegisterPipelines(Action<Pipeline> registerFunction)
    {
        registerFunction(
            new PipelineBuilder<GenericQuery<string>, string>()
                .With<GenericQueryHandler<GenericQuery<string>, string>>()
                .Build()
        );

        registerFunction(
            new PipelineBuilder<UnitQuery, Unit>()
                .With<GenericQueryHandler<UnitQuery, Unit>>()
                .Build()
        );
    }
}
