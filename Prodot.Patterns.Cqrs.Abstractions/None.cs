// Reimplementation of NeverNull
// Taken from https://github.com/Bomret/NeverNull
// Licensed under MIT License by @bomret

namespace Prodot.Patterns.Cqrs;

/// <summary>
///     Represents the absence of a value.
/// </summary>
public struct None
{
    /// <summary>
    ///     Returns the string representation of this type.
    /// </summary>
    public override string ToString() => "None";
}
