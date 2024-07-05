namespace Prodot.Patterns.Cqrs.Examples.WebApiWithCqrs.Models.Queries;

public class EmployeeQuery : IQuery<Employee, EmployeeQuery>
{
    public int Id { get; init; }
}
