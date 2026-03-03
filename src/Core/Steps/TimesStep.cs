namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>times()</c> step modulator that limits the number of loop iterations.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#repeat-step">Reference Documentation - Repeat Step</seealso>
    public sealed class TimesStep : Step
    {
        public TimesStep(int count)
        {
            Count = count;
        }

        public int Count { get; }
    }
}
