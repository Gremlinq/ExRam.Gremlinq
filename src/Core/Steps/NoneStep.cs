namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>none()</c> step that filters out all traversers.</summary>
    public sealed class NoneStep : Step, IFilterStep
    {
        /// <summary>Gets the singleton instance of <see cref="NoneStep"/>.</summary>
        public static readonly NoneStep Instance = new();
    }
}
