namespace Prodot.Patterns.Cqrs.Extensions;

public static class TestSupportExtensions
{
    /// <summary>
    /// Retrieves all used query handler types from this registry.
    /// </summary>
    /// <param name="registry">The query handler registry.</param>
    /// <returns>A list of all registered query handler types.</returns>
    public static IReadOnlyList<Type> GetAllRegisteredQueryHandlerTypes(this IQueryHandlerRegistry registry)
    {
        var reg = (QueryHandlerRegistry)registry;
        return reg.GetAllPipelines().SelectMany(p => p.Parts).Select(p => p.HandlerType).Distinct().ToList();
    }
}
