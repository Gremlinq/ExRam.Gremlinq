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
            /// <summary>Initializes a new instance of <see cref="By"/>.</summary>
            /// <param name="traversal">The by-modulator traversal.</param>
            public By(Traversal traversal)
            {
                Traversal = traversal;
            }

            /// <summary>Gets the by-modulator traversal.</summary>
            public Traversal Traversal { get; }
        }

        /// <summary>Initializes a new instance of <see cref="FormatStep"/>.</summary>
        /// <param name="format">The format string.</param>
        /// <param name="arguments">The format arguments.</param>
        public FormatStep(string format, ImmutableArray<object?> arguments)
        {
            ArgumentNullException.ThrowIfNull(format);

            Format = format;
            Arguments = arguments;
        }

        /// <summary>Gets the format string.</summary>
        public string Format { get; }
        /// <summary>Gets the format arguments.</summary>
        public ImmutableArray<object?> Arguments { get; }
    }
}
