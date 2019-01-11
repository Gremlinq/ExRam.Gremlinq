using System;
using System.Linq.Expressions;
using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Core
{
    public interface IVGremlinQuery : IElementGremlinQuery
    {
        IVGremlinQuery<IVertex> Both<TEdge>();
        IEGremlinQuery<TEdge> BothE<TEdge>();

        new IVGremlinQuery<TOtherVertex> Cast<TOtherVertex>();

        IVGremlinQuery<IVertex> In<TEdge>();

        IVGremlinQuery<TTarget> OfType<TTarget>();
        IVGremlinQuery<IVertex> Out<TEdge>();
    }

    public interface IVGremlinQuery<TVertex> : IGremlinQuery<TVertex>, IVGremlinQuery
    {
        new IEGremlinQuery<TEdge, TVertex> AddE<TEdge>(TEdge edge);
        IEGremlinQuery<TEdge, TVertex> AddE<TEdge>() where TEdge : new();
        IVGremlinQuery<TVertex> And(params Func<IVGremlinQuery<TVertex>, IGremlinQuery>[] andTraversals);
        TTargetQuery Aggregate<TTargetQuery>(Func<IVGremlinQuery<TVertex>, VStepLabel<TVertex[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IVGremlinQuery<TVertex>, VStepLabel<TVertex>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        new IVGremlinQuery<TVertex> As(StepLabel stepLabel);

        TTargetQuery Coalesce<TTargetQuery>(params Func<IVGremlinQuery<TVertex>, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IVGremlinQuery<TVertex> Dedup();

        new IVGremlinQuery<TVertex> Emit();
        new IVGremlinQuery<TVertex> Identity();

        IInEGremlinQuery<TEdge, TVertex> InE<TEdge>();

        new IVGremlinQuery<TVertex> Limit(long limit);
        TTargetQuery Local<TTargetQuery>(Func<IVGremlinQuery<TVertex>, TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IVGremlinQuery<TVertex>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;
        IVGremlinQuery<TVertex> Not(Func<IVGremlinQuery<TVertex>, IGremlinQuery> notTraversal);

        IVGremlinQuery<TVertex> Or(params Func<IVGremlinQuery<TVertex>, IGremlinQuery>[] orTraversals);
        new IOrderedVGremlinQuery<TVertex> OrderBy(Expression<Func<TVertex, object>> projection);
        IOrderedVGremlinQuery<TVertex> OrderBy(Func<IVGremlinQuery<TVertex>, IGremlinQuery> traversal);
        new IOrderedVGremlinQuery<TVertex> OrderBy(string lambda);
        new IOrderedVGremlinQuery<TVertex> OrderByDescending(Expression<Func<TVertex, object>> projection);
        IOrderedVGremlinQuery<TVertex> OrderByDescending(Func<IVGremlinQuery<TVertex>, IGremlinQuery> traversal);
        IVGremlinQuery<TVertex> Optional(Func<IVGremlinQuery<TVertex>, IVGremlinQuery<TVertex>> optionalTraversal);
        IOutEGremlinQuery<TEdge, TVertex> OutE<TEdge>();

        IVPropertiesGremlinQuery<VertexProperty<TTarget>, TTarget> Properties<TTarget>(params Expression<Func<TVertex, TTarget>>[] projections);
        IVPropertiesGremlinQuery<VertexProperty<TTarget>, TTarget> Properties<TTarget>(params Expression<Func<TVertex, VertexProperty<TTarget>>>[] projections);
        IVPropertiesGremlinQuery<VertexProperty<TTarget, TMeta>, TTarget, TMeta> Properties<TTarget, TMeta>(params Expression<Func<TVertex, VertexProperty<TTarget, TMeta>>>[] projections);

        IVPropertiesGremlinQuery<VertexProperty<TTarget>, TTarget> Properties<TTarget>(params Expression<Func<TVertex, TTarget[]>>[] projections);
        IVPropertiesGremlinQuery<VertexProperty<TTarget>, TTarget> Properties<TTarget>(params Expression<Func<TVertex, VertexProperty<TTarget>[]>>[] projections);
        IVPropertiesGremlinQuery<VertexProperty<TTarget, TMeta>, TTarget, TMeta> Properties<TTarget, TMeta>(params Expression<Func<TVertex, VertexProperty<TTarget, TMeta>[]>>[] projections);

        IVGremlinQuery<TVertex> Property<TValue>(Expression<Func<TVertex, TValue>> projection, TValue value);
        IVGremlinQuery<TVertex> Property<TValue>(Expression<Func<TVertex, TValue[]>> projection, TValue value);

        new IVGremlinQuery<TVertex> Range(long low, long high);
        IVGremlinQuery<TVertex> Repeat(Func<IVGremlinQuery<TVertex>, IVGremlinQuery<TVertex>> repeatTraversal);
        IVGremlinQuery<TVertex> RepeatUntil(Func<IVGremlinQuery<TVertex>, IVGremlinQuery<TVertex>> repeatTraversal, Func<IVGremlinQuery<TVertex>, IGremlinQuery> untilTraversal);

        IVGremlinQuery<TVertex> SideEffect(Func<IVGremlinQuery<TVertex>, IGremlinQuery> sideEffectTraversal);
        new IVGremlinQuery<TVertex> Skip(long skip);

        new IVGremlinQuery<TVertex> Tail(long limit);

        TTargetQuery Union<TTargetQuery>(params Func<IVGremlinQuery<TVertex>, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;

        IGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TVertex, TTarget>>[] projections);
        IGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TVertex, VertexProperty<TTarget>>>[] projections);
        IGremlinQuery<TTarget> Values<TTarget, TMeta>(params Expression<Func<TVertex, VertexProperty<TTarget, TMeta>>>[] projections);

        new IVGremlinQuery<TVertex> Where(Expression<Func<TVertex, bool>> predicate);
        new IVGremlinQuery<TVertex> Where<TProjection>(Expression<Func<TVertex, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal);
        IVGremlinQuery<TVertex> Where(Func<IVGremlinQuery<TVertex>, IGremlinQuery> filterTraversal);
    }
}
