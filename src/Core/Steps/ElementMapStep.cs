namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>elementMap()</c> step that maps elements to a dictionary of property keys and values, including element tokens</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#valuemap-step">Reference Documentation - ValueMap Step</seealso>
    public sealed class ElementMapStep : Step
    {
        internal static readonly ElementMapStep Instance = new();

        /// <summary>
        /// 
        /// </summary>
        private ElementMapStep() { }
    }
}
