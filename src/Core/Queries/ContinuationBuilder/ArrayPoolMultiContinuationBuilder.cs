using System.Buffers;

namespace ExRam.Gremlinq.Core
{
    internal readonly ref struct ArrayPoolMultiContinuationBuilder<TAnonymousQuery, TProjectedQuery>
        where TAnonymousQuery : GremlinQueryBase, IGremlinQueryBase
        where TProjectedQuery : IGremlinQueryBase
    {
        private readonly GremlinQueryBase _outer;
        private readonly ContinuationFlags _flags;
        private readonly TAnonymousQuery _anonymous;
        private readonly ReadOnlySpan<Func<TAnonymousQuery, TProjectedQuery>> _continuations;

        public ArrayPoolMultiContinuationBuilder(GremlinQueryBase outer, TAnonymousQuery anonymous, ReadOnlySpan<Func<TAnonymousQuery, TProjectedQuery>> continuations, ContinuationFlags flags)
        {
            _outer = outer;
            _flags = flags;
            _anonymous = anonymous;
            _continuations = continuations;
        }

        public TResult Build<TResult>(SpanFinalContinuationBuilderTransformation<TResult> builderTransformation)
        {
            var traversals = ArrayPool<Traversal>.Shared
                .Rent(_continuations.Length);

            try
            {
                var finalBuilder = FinalContinuationBuilder.Create(_outer);

                for (var i = 0; i < _continuations.Length; i++)
                {
                    finalBuilder = finalBuilder
                        .Apply(_continuations[i], _anonymous, _flags, out traversals[i]);
                }

                return builderTransformation(finalBuilder, traversals.AsSpan()[.._continuations.Length]);
            }
            finally
            {
                ArrayPool<Traversal>.Shared.Return(traversals);
            }
        }

        public static ArrayPoolMultiContinuationBuilder<TAnonymousQuery, TProjectedQuery> Create(GremlinQueryBase outer, TAnonymousQuery anonymous, ReadOnlySpan<Func<TAnonymousQuery, TProjectedQuery>> continuations, ContinuationFlags flags) => new(outer, anonymous, continuations, flags);
    }
}
