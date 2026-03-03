using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>format()</c> step that formats element properties into a string.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#format-step">Reference Documentation - Format Step</seealso>
    public sealed class FormatStep : Step
    {
        /// <summary>Represents a <c>by()</c> modulator with a traversal argument applied to a <c>format()</c> step.</summary>
        public sealed class By : Step
        {
            public By(Traversal traversal)
            {
                Traversal = traversal;
            }

            public Traversal Traversal { get; }
        }

        public FormatStep(string format, ImmutableArray<object?> arguments)
        {
            ArgumentNullException.ThrowIfNull(format);

            Format = format;
            Arguments = arguments;
        }

        public string Format { get; }
        public ImmutableArray<object?> Arguments { get; }
    }
}
