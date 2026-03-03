namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>has(key)</c> step that filters elements possessing a given property.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#has-step">Reference Documentation - Has Step</seealso>
    public sealed class HasStep : Step, IFilterStep
    {
        public HasStep(Key key)
        {
            Key = key;
        }

        public Key Key { get; }
    }
}
