namespace Prodot.Patterns.Cqrs.Examples.WebApiWithCqrs.Models;

public record Employee(int Id, string FirstName, string LastName, DateTimeOffset BirthDate);
