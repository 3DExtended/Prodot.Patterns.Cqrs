using Prodot.Patterns.Cqrs.Examples.WebApiWithCqrs.Models.Queries;

namespace Prodot.Patterns.Cqrs.Examples.WebApiWithCqrs.Business;

public class BusinessLayerPipelineProfile : IPipelineProfile
{
    public void RegisterPipelines(Action<Pipeline> registerFunction)
    {
        // since this pipeline uses only one query handler, which is also the only implementation of IQueryHandler<EmployeeAgeQuery, TimeSpan>
        // we could get rid of this explicit registration and use auto pipeline registration
        // however, we have it already and they coexist just fine
        registerFunction(
            new PipelineBuilder<EmployeeAgeQuery, TimeSpan>()
                .With<EmployeeAgeQueryHandler>()
                .Build()
        );
    }
}
