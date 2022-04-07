using System;

namespace ExRam.Gremlinq.Core
{
    internal static class ContinuationBuilderExtensions
    {
        public static TProjectedQuery Apply<TAnonymousQuery, TProjectedQuery>(this Func<TAnonymousQuery, TProjectedQuery> continuation, TAnonymousQuery anonymous)
            where TAnonymousQuery : IGremlinQueryBase
            where TProjectedQuery : IGremlinQueryBase
        {
            var continuedQuery = continuation(anonymous);

            if (continuedQuery is GremlinQueryBase queryBase && (queryBase.Flags & QueryFlags.IsAnonymous) == QueryFlags.None)
                throw new InvalidOperationException("A query continuation must originate from the query that was passed to the continuation function. Did you accidentally use 'g' in the continuation?");

            return continuedQuery;
        }

        public static TNewQuery Build<TOuterQuery, TAnonymousQuery, TNewQuery>(this ContinuationBuilder<TOuterQuery, TAnonymousQuery> continuationBuilder, Func<FinalContinuationBuilder<TOuterQuery>, TNewQuery> builderTransformation)
            where TOuterQuery : GremlinQueryBase, IGremlinQueryBase
            where TAnonymousQuery : GremlinQueryBase, IGremlinQueryBase
        {
            return continuationBuilder.Build(static (builder, state) => state(builder), builderTransformation);
        }


        public static TNewQuery Build<TOuterQuery, TAnonymousQuery, TNewQuery>(this SingleContinuationBuilder<TOuterQuery, TAnonymousQuery> continuationBuilder, Func<FinalContinuationBuilder<TOuterQuery>, Traversal, TNewQuery> builderTransformation)
            where TOuterQuery : GremlinQueryBase, IGremlinQueryBase
            where TAnonymousQuery : GremlinQueryBase, IGremlinQueryBase
        {
            return continuationBuilder.Build(static (builder, continuation, state) => state(builder, continuation), builderTransformation);
        }

        public static TNewQuery Build<TOuterQuery, TAnonymousQuery, TNewQuery>(this MultiContinuationBuilder<TOuterQuery, TAnonymousQuery> continuationBuilder, Func<FinalContinuationBuilder<TOuterQuery>, Traversal[], TNewQuery> builderTransformation)
            where TOuterQuery : GremlinQueryBase, IGremlinQueryBase
            where TAnonymousQuery : GremlinQueryBase, IGremlinQueryBase
        {
            return continuationBuilder.Build(static (builder, continuations, state) => state(builder, continuations), builderTransformation);
        }
    }
}
