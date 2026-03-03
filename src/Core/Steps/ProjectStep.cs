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
            /// <inheritdoc />
            protected ByStep(SideEffectSemanticsChange sideEffectSemanticsChange = SideEffectSemanticsChange.None) : base(sideEffectSemanticsChange)
            {
            }

            /// <summary>Converts this by-step to a <see cref="ByTraversalStep"/>.</summary>
            public abstract ByTraversalStep ToByTraversalStep();
        }

        /// <summary>Represents a <c>by()</c> modulator with a traversal argument applied to a <c>project()</c> step.</summary>
        public sealed class ByTraversalStep : ByStep
        {
            /// <summary>Initializes a new instance of <see cref="ByTraversalStep"/>.</summary>
            /// <param name="traversal">The by-modulator traversal.</param>
            public ByTraversalStep(Traversal traversal) : base(traversal.GetSideEffectSemanticsChange())
            {
                Traversal = traversal;
            }

            /// <inheritdoc />
            public override ByTraversalStep ToByTraversalStep() => this;

            /// <summary>Gets the by-modulator traversal.</summary>
            public Traversal Traversal { get; }
        }

        /// <summary>Represents a <c>by()</c> modulator with a key argument applied to a <c>project()</c> step.</summary>
        public sealed class ByKeyStep : ByStep
        {
            /// <summary>Initializes a new instance of <see cref="ByKeyStep"/>.</summary>
            /// <param name="key">The property key to project.</param>
            public ByKeyStep(Key key)
            {
                Key = key;
            }

            /// <inheritdoc />
            public override ByTraversalStep ToByTraversalStep() => new (Key.RawKey switch
            {
                T t => t.TryToStep() ?? throw ConversionFailed(),
                string key => new ValuesStep(ImmutableArray.Create(key)),
                _ => throw ConversionFailed(),
            });

            /// <summary>Gets the property key.</summary>
            public Key Key { get; }

            private NotSupportedException ConversionFailed() => new($"Failed to convert {nameof(ByKeyStep)}.{nameof(Key)} to a {nameof(ByTraversalStep)}.");

        }

        /// <summary>Initializes a new instance of <see cref="ProjectStep"/> with the specified projection keys.</summary>
        /// <param name="projections">The projection key names.</param>
        public ProjectStep(ImmutableArray<string> projections)
        {
            Projections = projections;
        }

        /// <summary>Gets the projection key names.</summary>
        public ImmutableArray<string> Projections { get; }
    }
}
