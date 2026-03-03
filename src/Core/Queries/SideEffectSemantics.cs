namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Describes whether a traversal reads or writes data.</summary>
    public enum SideEffectSemantics
    {
        /// <summary>The traversal only reads data.</summary>
        Read = 0,
        /// <summary>The traversal writes (mutates) data.</summary>
        Write = 1
    }
}
