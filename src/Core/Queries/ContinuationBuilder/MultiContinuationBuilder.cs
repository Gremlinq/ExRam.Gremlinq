namespace ExRam.Gremlinq.Core
{
    internal readonly ref struct MultiContinuationBuilder<TAnonymousQuery, TProjectedQuery>
        where TAnonymousQuery : GremlinQueryBase, IGremlinQueryBase
        where TProjectedQuery : IGremlinQueryBase
    {
        private readonly GremlinQueryBase _outer;
        private readonly ContinuationFlags _flags;
        private readonly TAnonymousQuery _anonymous;
        private readonly ReadOnlySpan<Func<TAnonymousQuery, TProjectedQuery>> _continuations;

        public MultiContinuationBuilder(GremlinQueryBase outer, TAnonymousQuery anonymous, ReadOnlySpan<Func<TAnonymousQuery, TProjectedQuery>> continuations, ContinuationFlags flags)
        {
            _outer = outer;
            _flags = flags;
            _anonymous = anonymous;
            _continuations = continuations;
        }

        public ArrayPoolMultiContinuationBuilder<TAnonymousQuery, TProjectedQuery> UseArrayPool() => ArrayPoolMultiContinuationBuilder<TAnonymousQuery, TProjectedQuery>.Create(_outer, _anonymous, _continuations, _flags);

        public TResult Build<TResult>(FinalContinuationBuilderTransformation<TResult> builderTransformation)
        {
            var traversals = new Traversal[_continuations.Length];
            var finalBuilder = FinalContinuationBuilder.Create(_outer);

            for (var i = 0; i < _continuations.Length; i++)
            {
                finalBuilder = finalBuilder
                    .Apply(_continuations[i], _anonymous, _flags, out traversals[i]);
            }

            return builderTransformation(finalBuilder, traversals);
        }

        public static MultiContinuationBuilder<TAnonymousQuery, TProjectedQuery> Create(GremlinQueryBase outer, TAnonymousQuery anonymous, ReadOnlySpan<Func<TAnonymousQuery, TProjectedQuery>> continuations, ContinuationFlags flags) => new(outer, anonymous, continuations, flags);
    }
}
