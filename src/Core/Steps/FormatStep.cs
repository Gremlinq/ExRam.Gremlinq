using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class FormatStep : Step
    {
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
