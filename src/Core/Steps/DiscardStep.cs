namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>
    /// Represents the Gremlin <c>discard()</c> step that filters out all traversers. TinkerPop 3.8 renamed <c>none()</c> to
    /// <c>discard()</c>, so Gremlinq uses this step in place of <see cref="NoneStep"/> once the lowest version of
    /// <see cref="IGremlinQueryEnvironment.ServerCompatibilityRange"/> is 3.8 or higher.
    /// </summary>
    public sealed class DiscardStep : Step, IFilterStep
    {
        /// <summary>Gets the singleton instance of <see cref="DiscardStep"/>.</summary>
        public static readonly DiscardStep Instance = new();
    }
}
