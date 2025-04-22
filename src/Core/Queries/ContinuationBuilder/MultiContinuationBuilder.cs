namespace ExRam.Gremlinq.Core
{
    internal readonly ref struct MultiContinuationBuilder<TAnonymousQuery>
        where TAnonymousQuery : GremlinQueryBase, IGremlinQueryBase
    {
        private readonly Memory<Traversal> _continuations;
        private readonly FinalContinuationBuilder _finalBuilder;

        private MultiContinuationBuilder(FinalContinuationBuilder finalBuilder, Memory<Traversal> continuations)
        {
            _finalBuilder = finalBuilder;
            _continuations = continuations;
        }

        public TResult Build<TResult>(FinalContinuationBuilderTransformation<TResult> builderTransformation) => builderTransformation(_finalBuilder, _continuations);

        public static MultiContinuationBuilder<TAnonymousQuery> Create<TProjectedQuery>(GremlinQueryBase outer, TAnonymousQuery anonymous, ReadOnlySpan<Func<TAnonymousQuery, TProjectedQuery>> continuations, ContinuationFlags flags)
            where TProjectedQuery : IGremlinQueryBase
        {
            var traversals = new Traversal[continuations.Length];
            var finalBuilder = FinalContinuationBuilder.Create(outer);

            for (var i = 0; i < continuations.Length; i++)
            {
                finalBuilder = finalBuilder.Apply(continuations[i], anonymous, flags, out traversals[i]);
            }

            return new MultiContinuationBuilder<TAnonymousQuery>(finalBuilder, traversals);
        }

        public static MultiContinuationBuilder<TAnonymousQuery> Create<TProjectedQuery, TState>(GremlinQueryBase outer, TAnonymousQuery anonymous, ReadOnlySpan<Func<TAnonymousQuery, TState, TProjectedQuery>> continuations, ContinuationFlags flags, TState state)
            where TProjectedQuery : IGremlinQueryBase
        {
            var traversals = new Traversal[continuations.Length];
            var finalBuilder = FinalContinuationBuilder.Create(outer);

            for (var i = 0; i < continuations.Length; i++)
            {
                finalBuilder = finalBuilder.Apply(continuations[i], anonymous, flags, out traversals[i], state);
            }

            return new MultiContinuationBuilder<TAnonymousQuery>(finalBuilder, traversals);
        }
    }
}
