namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Describes how a step changes the side-effect semantics of a traversal.</summary>
    public enum SideEffectSemanticsChange
    {
        /// <summary>The step does not introduce a write.</summary>
        None = 0,
        /// <summary>The step introduces a write (mutation).</summary>
        Write = 1
    }
}
