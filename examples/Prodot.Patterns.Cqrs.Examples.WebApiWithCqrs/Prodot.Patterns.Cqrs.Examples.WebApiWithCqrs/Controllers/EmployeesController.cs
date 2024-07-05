using Microsoft.AspNetCore.Mvc;
using Prodot.Patterns.Cqrs.Examples.WebApiWithCqrs.Models;
using Prodot.Patterns.Cqrs.Examples.WebApiWithCqrs.Models.Queries;

namespace Prodot.Patterns.Cqrs.Examples.WebApiWithCqrs.Controllers;

[ApiController]
[Route("employees")]
public class EmployeesController : ControllerBase
{
    private readonly IQueryProcessor _queryProcessor;

    public EmployeesController(IQueryProcessor queryProcessor)
    {
        _queryProcessor = queryProcessor;
    }

    [HttpGet("{id}/age")]
    public async Task<IActionResult> GetEmployeeAgeByIdAsync(
        [FromRoute] int id,
        CancellationToken cancellationToken
    )
    {
        var employee = await new EmployeeQuery { Id = id, }
            .RunAsync<EmployeeQuery, Employee>(_queryProcessor, cancellationToken)
            .WithExceptionIfNone(() => new Exception($"Could not find employee with ID {id}"))
            .ConfigureAwait(false);

        var employeeAge = await new EmployeeAgeQuery { Employee = employee, }
            .RunAsync<EmployeeAgeQuery, TimeSpan>(_queryProcessor, cancellationToken)
            .WithExceptionIfNone(
                () => new Exception($"Could not calculate age for employee with ID {id}")
            )
            .ConfigureAwait(false);

        return Ok(employeeAge);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployeeByIdAsync(
        [FromRoute] int id,
        CancellationToken cancellationToken
    )
    {
        var employee = await new EmployeeQuery { Id = id, }
            .RunAsync<EmployeeQuery, Employee>(_queryProcessor, cancellationToken)
            .WithExceptionIfNone(() => new Exception($"Could not find employee with ID {id}"))
            .ConfigureAwait(false);

        return Ok(employee);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployeeAsync(
        [FromRoute] int id,
        [FromBody] Employee employee,
        CancellationToken cancellationToken
    )
    {
        await new EmployeeUpdateCommand { UpdatedEmployee = employee, }
            .RunAsync<EmployeeUpdateCommand, Unit>(_queryProcessor, cancellationToken)
            .WithExceptionIfNone(() => new Exception($"Could not find employee with ID {id}"))
            .ConfigureAwait(false);

        return Ok(employee);
    }
}
