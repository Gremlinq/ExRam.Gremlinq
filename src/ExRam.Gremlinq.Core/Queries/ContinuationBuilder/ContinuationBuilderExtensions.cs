using System;
using System.Linq;

namespace ExRam.Gremlinq.Core
{
    internal static class ContinuationBuilderExtensions
    {
        public static TProjectedQuery Apply<TAnonymousQuery, TProjectedQuery, TState>(this Func<TAnonymousQuery, TState, TProjectedQuery> continuation, TAnonymousQuery anonymous, TState state)
            where TAnonymousQuery : IGremlinQueryBase
            where TProjectedQuery : IGremlinQueryBase
        {
            var continuedQuery = continuation(anonymous, state);

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

        public static SingleContinuationBuilder<TOuterQuery, TAnonymousQuery> With<TOuterQuery, TAnonymousQuery, TProjectedQuery>(this ContinuationBuilder<TOuterQuery, TAnonymousQuery> continuationBuilder, Func<TAnonymousQuery, TProjectedQuery> continuation)
            where TOuterQuery : GremlinQueryBase, IGremlinQueryBase
            where TAnonymousQuery : GremlinQueryBase, IGremlinQueryBase
            where TProjectedQuery : IGremlinQueryBase => continuationBuilder.With(
                static (anonymous, continuation) => continuation(anonymous),
                continuation);

        public static MultiContinuationBuilder<TOuterQuery, TAnonymousQuery> With<TOuterQuery, TAnonymousQuery, TProjectedQuery>(this ContinuationBuilder<TOuterQuery, TAnonymousQuery> continuationBuilder, Func<TAnonymousQuery, TProjectedQuery>[] continuations)
            where TOuterQuery : GremlinQueryBase, IGremlinQueryBase
            where TAnonymousQuery : GremlinQueryBase, IGremlinQueryBase
            where TProjectedQuery : IGremlinQueryBase
        {
            var multi = continuationBuilder.ToMulti();

            for (var i = 0; i < continuations.Length; i++)
            {
                multi = multi
                    .With(continuations[i]);
            }

            return multi;
        }

        public static MultiContinuationBuilder<TOuterQuery, TAnonymousQuery> With<TOuterQuery, TAnonymousQuery, TProjectedQuery>(this MultiContinuationBuilder<TOuterQuery, TAnonymousQuery> continuationBuilder, Func<TAnonymousQuery, TProjectedQuery> continuation)
            where TOuterQuery : GremlinQueryBase, IGremlinQueryBase
            where TAnonymousQuery : GremlinQueryBase, IGremlinQueryBase
            where TProjectedQuery : IGremlinQueryBase => continuationBuilder.With(
                static (anonymous, continuation) => continuation(anonymous),
                continuation);

        public static MultiContinuationBuilder<TOuterQuery, TAnonymousQuery> With<TOuterQuery, TAnonymousQuery, TProjectedQuery>(this SingleContinuationBuilder<TOuterQuery, TAnonymousQuery> continuationBuilder, Func<TAnonymousQuery, TProjectedQuery> continuation)
            where TOuterQuery : GremlinQueryBase, IGremlinQueryBase
            where TAnonymousQuery : GremlinQueryBase, IGremlinQueryBase
            where TProjectedQuery : IGremlinQueryBase => continuationBuilder.With(
                static (anonymous, continuation) => continuation(anonymous),
                continuation);
    }
}
