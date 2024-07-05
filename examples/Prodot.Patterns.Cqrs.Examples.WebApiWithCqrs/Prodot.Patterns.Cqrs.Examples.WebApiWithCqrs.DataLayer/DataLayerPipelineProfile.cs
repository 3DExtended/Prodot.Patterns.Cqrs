using Prodot.Patterns.Cqrs.Examples.WebApiWithCqrs.Models;
using Prodot.Patterns.Cqrs.Examples.WebApiWithCqrs.Models.Queries;

namespace Prodot.Patterns.Cqrs.Examples.WebApiWithCqrs.DataLayer;

public class DataLayerPipelineProfile : IPipelineProfile
{
    public void RegisterPipelines(Action<Pipeline> registerFunction)
    {
        // since these pipelines use only one query handler, which is also the only respective implementation of IQueryHandler<TQuery, TResult>
        // we could get rid of these explicit registrations and use auto pipeline registration
        // however, we have it already and they coexist just fine
        registerFunction(
            new PipelineBuilder<EmployeeQuery, Employee>().With<EmployeeQueryHandler>().Build()
        );

        registerFunction(
            new PipelineBuilder<EmployeeUpdateCommand, Unit>()
                .With<EmployeeUpdateCommandHandler>()
                .Build()
        );
    }
}
