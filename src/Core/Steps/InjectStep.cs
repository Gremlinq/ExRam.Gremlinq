using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>inject()</c> step that injects additional elements into the traversal stream.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#inject-step">Reference Documentation - Inject Step</seealso>
    public sealed class InjectStep : Step
    {
        /// <summary>Initializes a new instance of <see cref="InjectStep"/> with the specified elements.</summary>
        /// <param name="elements">The elements to inject into the traversal stream.</param>
        public InjectStep(ImmutableArray<object> elements)
        {
            Elements = elements;
        }

        /// <summary>Gets the elements to inject.</summary>
        public ImmutableArray<object> Elements { get; }
    }
}
