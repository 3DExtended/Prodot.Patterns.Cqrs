namespace Prodot.Patterns.Cqrs.Examples.WebApiWithCqrs.Models.Queries;

public class EmployeeUpdateCommand : Command<EmployeeUpdateCommand>
{
    public Employee UpdatedEmployee { get; init; } = default!;
}
