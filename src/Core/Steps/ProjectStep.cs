using System.Collections.Immutable;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>project()</c> step that projects elements into a keyed map.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#project-step">Reference Documentation - Project Step</seealso>
    public sealed class ProjectStep : Step
    {
        /// <summary>Base class for <c>by()</c> modulators applied to the <c>project()</c> step.</summary>
        public abstract class ByStep : Step
        {
            protected ByStep(SideEffectSemanticsChange sideEffectSemanticsChange = SideEffectSemanticsChange.None) : base(sideEffectSemanticsChange)
            {
            }

            public abstract ByTraversalStep ToByTraversalStep();
        }

        /// <summary>Represents a <c>by()</c> modulator with a traversal argument applied to a <c>project()</c> step.</summary>
        public sealed class ByTraversalStep : ByStep
        {
            public ByTraversalStep(Traversal traversal) : base(traversal.GetSideEffectSemanticsChange())
            {
                Traversal = traversal;
            }

            public override ByTraversalStep ToByTraversalStep() => this;

            public Traversal Traversal { get; }
        }

        /// <summary>Represents a <c>by()</c> modulator with a key argument applied to a <c>project()</c> step.</summary>
        public sealed class ByKeyStep : ByStep
        {
            public ByKeyStep(Key key)
            {
                Key = key;
            }

            public override ByTraversalStep ToByTraversalStep() => new (Key.RawKey switch
            {
                T t => t.TryToStep() ?? throw ConversionFailed(),
                string key => new ValuesStep(ImmutableArray.Create(key)),
                _ => throw ConversionFailed(),
            });

            public Key Key { get; }

            private NotSupportedException ConversionFailed() => new($"Failed to convert {nameof(ByKeyStep)}.{nameof(Key)} to a {nameof(ByTraversalStep)}.");

        }

        public ProjectStep(ImmutableArray<string> projections)
        {
            Projections = projections;
        }

        public ImmutableArray<string> Projections { get; }
    }
}
