namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>none()</c> step that filters out all traversers.</summary>
    public sealed class NoneStep : Step, IFilterStep
    {
        public static readonly NoneStep Instance = new();
    }
}
