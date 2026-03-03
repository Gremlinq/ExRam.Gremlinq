using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Base class for logical steps (<c>and()</c>, <c>or()</c>) that combine multiple sub-traversals.</summary>
    public abstract class LogicalStep<TStep> : Step
        where TStep : LogicalStep<TStep>
    {
        /// <summary>Initializes a new instance of <see cref="LogicalStep{TStep}"/> with the specified sub-traversals.</summary>
        /// <param name="traversals">The sub-traversals to combine.</param>
        protected LogicalStep(ImmutableArray<Traversal> traversals) : base(traversals.GetSideEffectSemanticsChange())
        {
            Traversals = traversals;
        }

        internal static ImmutableArray<Traversal> FlattenLogicalTraversals(ReadOnlySpan<Traversal> traversals)
        {
            var builder = ImmutableArray.CreateBuilder<Traversal>();

            FlattenLogicalTraversals(builder, traversals);

            return builder.ToImmutableArray();

            static void FlattenLogicalTraversals(ImmutableArray<Traversal>.Builder builder, ReadOnlySpan<Traversal> traversals)
            {
                for (var i = 0; i < traversals.Length; i++)
                {
                    var traversal = traversals[i];

                    if (traversal is [TStep otherStep])
                    {
                        FlattenLogicalTraversals(builder, otherStep.Traversals.AsSpan());
                    }
                    else
                        builder.Add(traversal);
                }
            }
        }

        /// <summary>Gets the sub-traversals.</summary>
        public ImmutableArray<Traversal> Traversals { get; }
    }
}
