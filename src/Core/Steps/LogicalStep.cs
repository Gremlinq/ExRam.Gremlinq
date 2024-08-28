using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core.Steps
{
    public abstract class LogicalStep<TStep> : Step
        where TStep : LogicalStep<TStep>
    {
        [Obsolete("Will be removed in a future version. Use the overload taking an ImmutableArray<Traversal> instead.")]
        protected LogicalStep(string name, IEnumerable<Traversal> traversals) : this(name, traversals.ToImmutableArray())
        {
        }

        protected LogicalStep(string name, ImmutableArray<Traversal> traversals) : base(traversals.GetSideEffectSemanticsChange())
        {
            Name = name;
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

        //TODO: Seemingly unused.
        public string Name { get; }
        public ImmutableArray<Traversal> Traversals { get; }
    }
}
