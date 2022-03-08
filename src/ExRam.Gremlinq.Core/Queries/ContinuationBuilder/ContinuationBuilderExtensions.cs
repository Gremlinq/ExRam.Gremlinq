using System;
using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    internal static class ContinuationBuilderExtensions
    {
        public static IGremlinQueryBase Apply<TAnonymousQuery, TProjectedQuery>(this Func<TAnonymousQuery, TProjectedQuery> continuation, TAnonymousQuery anonymous)
            where TAnonymousQuery : IGremlinQueryBase
            where TProjectedQuery : IGremlinQueryBase
        {
            var continuatedQuery = continuation(anonymous);

            if (continuatedQuery is GremlinQueryBase queryBase && (queryBase.Flags & QueryFlags.IsAnonymous) == QueryFlags.None)
                throw new InvalidOperationException("A query continuation must originate from the query that was passed to the continuation function. Did you accidentally use 'g' in the continuation?");

            return continuatedQuery;
        }

        public static TNewQuery Build<TOuterQuery, TAnonymousQuery, TNewQuery>(this ContinuationBuilder<TOuterQuery, TAnonymousQuery> continuationBuilder, Func<FinalContinuationBuilder<TOuterQuery>, TNewQuery> builderTransformation)
            where TOuterQuery : GremlinQueryBase
            where TAnonymousQuery : GremlinQueryBase, IGremlinQueryBase
        {
            return continuationBuilder.Build(static (builder, state) => state(builder), builderTransformation);
        }


        public static TNewQuery Build<TOuterQuery, TAnonymousQuery, TNewQuery>(this SingleContinuationBuilder<TOuterQuery, TAnonymousQuery> continuationBuilder, Func<FinalContinuationBuilder<TOuterQuery>, Traversal, TNewQuery> builderTransformation)
            where TOuterQuery : GremlinQueryBase
            where TAnonymousQuery : GremlinQueryBase, IGremlinQueryBase
        {
            return continuationBuilder.Build(static (builder, continuation, state) => state(builder, continuation), builderTransformation);
        }

        public static TNewQuery Build<TOuterQuery, TAnonymousQuery, TNewQuery>(this MultiContinuationBuilder<TOuterQuery, TAnonymousQuery> continuationBuilder, Func<FinalContinuationBuilder<TOuterQuery>, IImmutableList<Traversal>, TNewQuery> builderTransformation)
            where TOuterQuery : GremlinQueryBase
            where TAnonymousQuery : GremlinQueryBase, IGremlinQueryBase
        {
            return continuationBuilder.Build(static (builder, continuation, state) => state(builder, continuation), builderTransformation);
        }
    }
}
