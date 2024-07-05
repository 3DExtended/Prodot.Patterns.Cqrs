using Prodot.Patterns.Cqrs.Examples.WebApiWithCqrs.Models.Queries;

namespace Prodot.Patterns.Cqrs.Examples.WebApiWithCqrs.DataLayer;

public class EmployeeUpdateCommandHandler : IQueryHandler<EmployeeUpdateCommand, Unit>
{
    public IQueryHandler<EmployeeUpdateCommand, Unit> Successor { get; set; } = default!;

    public async Task<Option<Unit>> RunQueryAsync(
        EmployeeUpdateCommand query,
        CancellationToken cancellationToken
    )
    {
        // normally we would store it to a database...
        await Task.Yield();
        return Unit.Value;
    }
}
