using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    internal static class ContinuationBuilderExtensions
    {
        private static readonly Traversal IdentityTraversal = IdentityStep.Instance;

        public static TProjectedQuery Apply<TAnonymousQuery, TProjectedQuery>(this Func<TAnonymousQuery, TProjectedQuery> continuation, TAnonymousQuery anonymous)
            where TAnonymousQuery : IGremlinQueryBase
            where TProjectedQuery : IGremlinQueryBase => Apply(static (anonymous, continuation) => continuation(anonymous), anonymous, continuation);

        public static TProjectedQuery Apply<TAnonymousQuery, TProjectedQuery, TState>(this Func<TAnonymousQuery, TState, TProjectedQuery> continuation, TAnonymousQuery anonymous, TState state)
            where TAnonymousQuery : IGremlinQueryBase
            where TProjectedQuery : IGremlinQueryBase
        {
            var continuedQuery = continuation(anonymous, state);
            var admin = continuedQuery.AsAdmin();

            return admin.Steps.Count == 0
                ? admin.ConfigureSteps<TProjectedQuery>(static traversal => IdentityTraversal.WithProjection(traversal.Projection))
                : continuedQuery;
        }
    }
}
