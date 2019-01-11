using System;
using System.Linq.Expressions;
using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Core
{
    public interface IEGremlinQuery : IElementGremlinQuery
    {
        IVGremlinQuery<IVertex> BothV();
        new IEGremlinQuery<TOtherEdge> Cast<TOtherEdge>();

        IVGremlinQuery<IVertex> InV();

        IEGremlinQuery<TTarget> OfType<TTarget>();
        IVGremlinQuery<IVertex> OtherV();
        IVGremlinQuery<IVertex> OutV();
    }

    public interface IEGremlinQuery<TEdge> : IGremlinQuery<TEdge>, IEGremlinQuery
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IEGremlinQuery<TEdge>, EStepLabel<TEdge[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        TTargetQuery As<TTargetQuery>(Func<IEGremlinQuery<TEdge>, EStepLabel<TEdge>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        new IEGremlinQuery<TEdge> As(StepLabel stepLabel);

        new IEGremlinQuery<TEdge> Dedup();

        new IEGremlinQuery<TEdge> Emit();

        IOutEGremlinQuery<TEdge, TOutVertex> From<TOutVertex>(Func<IGremlinQuery, IGremlinQuery<TOutVertex>> fromVertexTraversal);
        IOutEGremlinQuery<TEdge, TOutVertex> From<TOutVertex>(StepLabel<TOutVertex> stepLabel);

        new IEGremlinQuery<TEdge> Identity();

        new IEGremlinQuery<TEdge> Limit(long limit);
        TTargetQuery Local<TTargetQuery>(Func<IEGremlinQuery<TEdge>, TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IEGremlinQuery<TEdge>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;

        new IOrderedEGremlinQuery<TEdge> OrderBy(Expression<Func<TEdge, object>> projection);
        IOrderedEGremlinQuery<TEdge> OrderBy(Func<IEGremlinQuery<TEdge>, IGremlinQuery> traversal);
        new IOrderedEGremlinQuery<TEdge> OrderBy(string lambda);
        new IOrderedEGremlinQuery<TEdge> OrderByDescending(Expression<Func<TEdge, object>> projection);
        IOrderedEGremlinQuery<TEdge> OrderByDescending(Func<IEGremlinQuery<TEdge>, IGremlinQuery> traversal);

        IEPropertiesGremlinQuery<Property<TTarget>, TTarget> Properties<TTarget>(params Expression<Func<TEdge, TTarget>>[] projections);
        IEPropertiesGremlinQuery<Property<TTarget>, TTarget> Properties<TTarget>(params Expression<Func<TEdge, Property<TTarget>>>[] projections);
        IEPropertiesGremlinQuery<Property<TTarget>, TTarget> Properties<TTarget>(params Expression<Func<TEdge, TTarget[]>>[] projections);
        IEPropertiesGremlinQuery<Property<TTarget>, TTarget> Properties<TTarget>(params Expression<Func<TEdge, Property<TTarget>[]>>[] projections);

        IEGremlinQuery<TEdge> Property<TValue>(Expression<Func<TEdge, TValue>> projection, TValue value);
        IEGremlinQuery<TEdge> Property<TValue>(Expression<Func<TEdge, TValue[]>> projection, TValue value);

        new IEGremlinQuery<TEdge> Range(long low, long high);
        IEGremlinQuery<TEdge> Repeat(Func<IEGremlinQuery<TEdge>, IEGremlinQuery<TEdge>> repeatTraversal);
        IEGremlinQuery<TEdge> RepeatUntil(Func<IEGremlinQuery<TEdge>, IEGremlinQuery<TEdge>> repeatTraversal, Func<IEGremlinQuery<TEdge>, IGremlinQuery> untilTraversal);

        IEGremlinQuery<TEdge> SideEffect(Func<IEGremlinQuery<TEdge>, IGremlinQuery> sideEffectTraversal);
        new IEGremlinQuery<TEdge> Skip(long skip);

        new IEGremlinQuery<TEdge> Tail(long limit);

        IInEGremlinQuery<TEdge, TInVertex> To<TInVertex>(Func<IGremlinQuery, IGremlinQuery<TInVertex>> toVertexTraversal);
        IInEGremlinQuery<TEdge, TInVertex> To<TInVertex>(StepLabel<TInVertex> stepLabel);

        IGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TEdge, TTarget>>[] projections);
        IGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TEdge, Property<TTarget>>>[] projections);

        new IEGremlinQuery<TEdge> Where(Expression<Func<TEdge, bool>> predicate);
        new IEGremlinQuery<TEdge> Where<TProjection>(Expression<Func<TEdge, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal);
        IEGremlinQuery<TEdge> Where(Func<IEGremlinQuery<TEdge>, IGremlinQuery> filterTraversal);
    }

    public interface IEGremlinQuery<TEdge, TAdjacentVertex> : IEGremlinQuery<TEdge>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IEGremlinQuery<TEdge, TAdjacentVertex>, EStepLabel<TEdge[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IEGremlinQuery<TEdge, TAdjacentVertex>, EStepLabel<TEdge, TAdjacentVertex>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        new IEGremlinQuery<TEdge, TAdjacentVertex> As(StepLabel stepLabel);

        new IEGremlinQuery<TOtherEdge, TAdjacentVertex> Cast<TOtherEdge>();

        IEGremlinQuery<TEdge, TTargetVertex, TAdjacentVertex> From<TTargetVertex>(Func<IVGremlinQuery<TAdjacentVertex>, IGremlinQuery<TTargetVertex>> fromVertexTraversal);
        new IEGremlinQuery<TEdge, TTargetVertex, TAdjacentVertex> From<TTargetVertex>(StepLabel<TTargetVertex> stepLabel);

        new IEGremlinQuery<TEdge, TAdjacentVertex> Identity();

        TTargetQuery Map<TTargetQuery>(Func<IEGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;

        new IEGremlinQuery<TTarget, TAdjacentVertex> OfType<TTarget>();

        IEGremlinQuery<TEdge, TAdjacentVertex, TTargetVertex> To<TTargetVertex>(Func<IVGremlinQuery<TAdjacentVertex>, IGremlinQuery<TTargetVertex>> toVertexTraversal);
        new IEGremlinQuery<TEdge, TAdjacentVertex, TTargetVertex> To<TTargetVertex>(StepLabel<TTargetVertex> stepLabel);

        new IEGremlinQuery<TEdge, TAdjacentVertex> Where(Expression<Func<TEdge, bool>> predicate);
        IEGremlinQuery<TEdge, TAdjacentVertex> Where(Func<IEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> filterTraversal);
    }

    public interface IEGremlinQuery<TEdge, TOutVertex, TInVertex> : IEGremlinQuery<TEdge, TOutVertex>, IOutEGremlinQuery<TEdge, TOutVertex>, IInEGremlinQuery<TEdge, TInVertex>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IEGremlinQuery<TEdge, TOutVertex, TInVertex>, EStepLabel<TEdge[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        new IEGremlinQuery<TEdge, TOutVertex, TInVertex> As(StepLabel stepLabel);

        new IEGremlinQuery<TOtherEdge, TOutVertex, TInVertex> Cast<TOtherEdge>();

        new IEGremlinQuery<TEdge, TOutVertex, TInVertex> Identity();
        new IVGremlinQuery<TInVertex> InV();

        TTargetQuery Map<TTargetQuery>(Func<IEGremlinQuery<TEdge, TOutVertex, TInVertex>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;

        new IEGremlinQuery<TTarget, TOutVertex, TInVertex> OfType<TTarget>();

        new IOrderedEGremlinQuery<TEdge, TOutVertex, TInVertex> OrderBy(Expression<Func<TEdge, object>> projection);
        IOrderedEGremlinQuery<TEdge, TOutVertex, TInVertex> OrderBy(Func<IEGremlinQuery<TEdge, TOutVertex, TInVertex>, IGremlinQuery> traversal);
        new IOrderedEGremlinQuery<TEdge, TOutVertex, TInVertex> OrderBy(string lambda);
        new IOrderedEGremlinQuery<TEdge, TOutVertex, TInVertex> OrderByDescending(Expression<Func<TEdge, object>> projection);
        IOrderedEGremlinQuery<TEdge, TOutVertex, TInVertex> OrderByDescending(Func<IEGremlinQuery<TEdge, TOutVertex, TInVertex>, IGremlinQuery> traversal);

        new IVGremlinQuery<TOutVertex> OutV();

        new IEGremlinQuery<TEdge, TOutVertex, TInVertex> Where(Expression<Func<TEdge, bool>> predicate);
        IEGremlinQuery<TEdge, TOutVertex, TInVertex> Where(Func<IEGremlinQuery<TEdge, TOutVertex, TInVertex>, IGremlinQuery> filterTraversal);
    }
}
