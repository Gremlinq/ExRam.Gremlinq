namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>times()</c> step modulator that limits the number of loop iterations.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#repeat-step">Reference Documentation - Repeat Step</seealso>
    public sealed class TimesStep : Step
    {
        /// <summary>Initializes a new instance of <see cref="TimesStep"/> with the specified iteration count.</summary>
        /// <param name="count">The maximum number of loop iterations.</param>
        public TimesStep(int count)
        {
            Count = count;
        }

        /// <summary>Gets the maximum number of loop iterations.</summary>
        public int Count { get; }
    }
}
