namespace Prodot.Patterns.Cqrs.Examples.WebApiWithCqrs.Models.Queries;

public class EmployeeAgeQuery : IQuery<TimeSpan, EmployeeAgeQuery>
{
    public Employee Employee { get; init; } = default!;
}
