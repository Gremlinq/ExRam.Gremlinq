using System;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    public interface IInEGremlinQuery<TEdge, TAdjacentVertex> : IEGremlinQuery<TEdge>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IInEGremlinQuery<TEdge, TAdjacentVertex>, EStepLabel<TEdge[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IInEGremlinQuery<TEdge, TAdjacentVertex>, EStepLabel<TEdge, TAdjacentVertex>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        new IInEGremlinQuery<TEdge, TAdjacentVertex> As(StepLabel stepLabel);

        new IInEGremlinQuery<TOtherEdge, TAdjacentVertex> Cast<TOtherEdge>();

        new IEGremlinQuery<TEdge, TOutVertex, TAdjacentVertex> From<TOutVertex>(Func<IGremlinQuery, IGremlinQuery<TOutVertex>> fromVertexTraversal);

        new IInEGremlinQuery<TEdge, TAdjacentVertex> Identity();
        new IVGremlinQuery<TAdjacentVertex> InV();
        
        TTargetQuery Map<TTargetQuery>(Func<IInEGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;

        new IInEGremlinQuery<TTarget, TAdjacentVertex> OfType<TTarget>();
        new IOrderedInEGremlinQuery<TEdge, TAdjacentVertex> OrderBy(Expression<Func<TEdge, object>> projection);
        IOrderedInEGremlinQuery<TEdge, TAdjacentVertex> OrderBy(Func<IInEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> traversal);
        new IOrderedInEGremlinQuery<TEdge, TAdjacentVertex> OrderBy(string lambda);
        new IOrderedInEGremlinQuery<TEdge, TAdjacentVertex> OrderByDescending(Expression<Func<TEdge, object>> projection);
        IOrderedInEGremlinQuery<TEdge, TAdjacentVertex> OrderByDescending(Func<IInEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> traversal);

        new IInEGremlinQuery<TEdge, TAdjacentVertex> Where(Expression<Func<TEdge, bool>> predicate);
        IInEGremlinQuery<TEdge, TAdjacentVertex> Where(Func<IInEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> filterTraversal);
    }
}