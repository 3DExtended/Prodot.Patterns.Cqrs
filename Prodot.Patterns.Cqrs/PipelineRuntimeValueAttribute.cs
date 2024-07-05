namespace Prodot.Patterns.Cqrs;

/// <summary>
/// Marks a property that is only used by the chained query handlers during pipeline execution and must therefore be unset when starting the query execution.
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class PipelineRuntimeValueAttribute : Attribute { }
