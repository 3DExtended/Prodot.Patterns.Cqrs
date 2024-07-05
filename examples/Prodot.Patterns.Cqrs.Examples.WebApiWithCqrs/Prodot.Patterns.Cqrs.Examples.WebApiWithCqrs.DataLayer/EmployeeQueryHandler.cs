using Prodot.Patterns.Cqrs.Examples.WebApiWithCqrs.Models;
using Prodot.Patterns.Cqrs.Examples.WebApiWithCqrs.Models.Queries;

namespace Prodot.Patterns.Cqrs.Examples.WebApiWithCqrs.DataLayer;

public class EmployeeQueryHandler : IQueryHandler<EmployeeQuery, Employee>
{
    public IQueryHandler<EmployeeQuery, Employee> Successor { get; set; } = default!;

    public async Task<Option<Employee>> RunQueryAsync(
        EmployeeQuery query,
        CancellationToken cancellationToken
    )
    {
        // normally we would load it from a database...
        await Task.Yield();
        return new Employee(query.Id, "Test", "Tester", new(2022, 02, 01, 0, 0, 0, TimeSpan.Zero));
    }
}
