namespace Prodot.Patterns.Cqrs;

/// <summary>
/// Base class for commands (mostly for convenience).
/// </summary>
/// <typeparam name="TSelf">The command type itself, e.g. <code>public class ACommand : Command&lt;ACommand&gt;</code></typeparam>
public abstract class Command<TSelf> : IQuery<Unit, TSelf>
    where TSelf : IQuery<Unit, TSelf> { }
