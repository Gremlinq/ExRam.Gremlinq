using System;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    public interface IOutEGremlinQuery<TEdge, TAdjacentVertex> : IEGremlinQuery<TEdge>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IOutEGremlinQuery<TEdge, TAdjacentVertex>, StepLabel<TEdge[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IOutEGremlinQuery<TEdge, TAdjacentVertex>, StepLabel<IOutEGremlinQuery<TEdge, TAdjacentVertex>, TEdge>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        new IOutEGremlinQuery<TEdge, TAdjacentVertex> As(StepLabel stepLabel);

        new IOutEGremlinQuery<TOtherEdge, TAdjacentVertex> Cast<TOtherEdge>();

        TTargetQuery Map<TTargetQuery>(Func<IOutEGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;

        new IOutEGremlinQuery<TEdge, TAdjacentVertex> Identity();

        new IOutEGremlinQuery<TTarget, TAdjacentVertex> OfType<TTarget>();
        new IOrderedOutEGremlinQuery<TEdge, TAdjacentVertex> OrderBy(Expression<Func<TEdge, object>> projection);
        IOrderedOutEGremlinQuery<TEdge, TAdjacentVertex> OrderBy(Func<IOutEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> traversal);
        new IOrderedOutEGremlinQuery<TEdge, TAdjacentVertex> OrderBy(string lambda);
        new IOrderedOutEGremlinQuery<TEdge, TAdjacentVertex> OrderByDescending(Expression<Func<TEdge, object>> projection);
        IOrderedOutEGremlinQuery<TEdge, TAdjacentVertex> OrderByDescending(Func<IOutEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> traversal);
        new IVGremlinQuery<TAdjacentVertex> OutV();

        new IEGremlinQuery<TEdge, TAdjacentVertex, TInVertex> To<TInVertex>(Func<IGremlinQuery, IGremlinQuery<TInVertex>> toVertexTraversal);

        new IOutEGremlinQuery<TEdge, TAdjacentVertex> Where(Expression<Func<TEdge, bool>> predicate);
        IOutEGremlinQuery<TEdge, TAdjacentVertex> Where(Func<IOutEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> filterTraversal);
    }
}
