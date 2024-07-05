using Prodot.Patterns.Cqrs.Examples.WebApiWithCqrs.Models.Queries;

namespace Prodot.Patterns.Cqrs.Examples.WebApiWithCqrs.Business;

public class EmployeeAgeQueryHandler : IQueryHandler<EmployeeAgeQuery, TimeSpan>
{
    public IQueryHandler<EmployeeAgeQuery, TimeSpan> Successor { get; set; } = default!;

    public Task<Option<TimeSpan>> RunQueryAsync(
        EmployeeAgeQuery query,
        CancellationToken cancellationToken
    )
    {
        // here we have some business logic
        return Task.FromResult(Option.From(DateTimeOffset.Now - query.Employee.BirthDate));
    }
}
