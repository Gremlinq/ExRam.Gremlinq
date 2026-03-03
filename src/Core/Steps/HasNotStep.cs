namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>hasNot()</c> step that filters elements lacking a given property.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#has-step">Reference Documentation - Has Step</seealso>
    public sealed class HasNotStep : Step, IFilterStep
    {
        public HasNotStep(Key key)
        {
            Key = key;
        }

        public Key Key { get; }
    }
}
