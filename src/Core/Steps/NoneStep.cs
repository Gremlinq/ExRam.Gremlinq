namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>
    /// Represents the Gremlin <c>none()</c> step that filters out all traversers. Gremlinq uses it as long as the lowest version of
    /// <see cref="IGremlinQueryEnvironment.ServerCompatibilityRange"/> is below TinkerPop 3.8, and <see cref="DiscardStep"/> from then on.
    /// </summary>
    public sealed class NoneStep : Step, IFilterStep
    {
        /// <summary>Gets the singleton instance of <see cref="NoneStep"/>.</summary>
        public static readonly NoneStep Instance = new();
    }
}
