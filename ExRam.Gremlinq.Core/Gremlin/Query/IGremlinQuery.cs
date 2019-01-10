using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ExRam.Gremlinq.Core.GraphElements;
using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQuery : IGremlinQuerySource
    {
        IGremlinQueryAdmin AsAdmin();
        IGremlinQuery<TElement> Cast<TElement>();
        IGremlinQuery<long> Count();
        IGremlinQuery<Unit> Drop();
        IGremlinQuery<string> Explain();

        TTargetQuery Map<TTargetQuery>(Func<IGremlinQuery, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;

        IGremlinQuery<string> Profile();

        IGremlinQuery<TStepElement> Select<TStepElement>(StepLabel<TStepElement> label);
        IVGremlinQuery<TVertex> Select<TVertex>(VStepLabel<TVertex> label);
        IEGremlinQuery<TEdge> Select<TEdge>(EStepLabel<TEdge> label);
        IEGremlinQuery<TEdge, TAdjacentVertex> Select<TEdge, TAdjacentVertex>(EStepLabel<TEdge, TAdjacentVertex> label);
        IOutEGremlinQuery<TEdge, TAdjacentVertex> Select<TEdge, TAdjacentVertex>(OutEStepLabel<TEdge, TAdjacentVertex> label);
        IInEGremlinQuery<TEdge, TAdjacentVertex> Select<TEdge, TAdjacentVertex>(InEStepLabel<TEdge, TAdjacentVertex> label);

        IGremlinQuery<(T1, T2)> Select<T1, T2>(StepLabel<T1> label1, StepLabel<T2> label2);
        IGremlinQuery<(T1, T2, T3)> Select<T1, T2, T3>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3);
        IGremlinQuery<(T1, T2, T3, T4)> Select<T1, T2, T3, T4>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4);
    }

    public interface IOrderedGremlinQuery<TElement> : IGremlinQuery<TElement>
    {
        IOrderedGremlinQuery<TElement> ThenBy(Expression<Func<TElement, object>> projection);
        IOrderedGremlinQuery<TElement> ThenBy(Func<IGremlinQuery<TElement>, IGremlinQuery> traversal);
        IOrderedGremlinQuery<TElement> ThenBy(string lambda);
        IOrderedGremlinQuery<TElement> ThenByDescending(Expression<Func<TElement, object>> projection);
        IOrderedGremlinQuery<TElement> ThenByDescending(Func<IGremlinQuery<TElement>, IGremlinQuery> traversal);
    }

    public interface IGremlinQuery<TElement> : IGremlinQuery, IAsyncEnumerable<TElement>
    {
        IGremlinQuery<TElement> And(params Func<IGremlinQuery<TElement>, IGremlinQuery>[] andTraversals);
        TTargetQuery Aggregate<TTargetQuery>(Func<IGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IGremlinQuery<TElement>, StepLabel<TElement>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        IGremlinQuery<TElement> As(StepLabel stepLabel);
        IGremlinQuery<TElement> Barrier();
        TTargetQuery Coalesce<TTargetQuery>(params Func<IGremlinQuery<TElement>, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;
        IGremlinQuery<TResult> Choose<TResult>(Func<IGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IGremlinQuery<TElement>, IGremlinQuery<TResult>> trueChoice, Func<IGremlinQuery<TElement>, IGremlinQuery<TResult>> falseChoice);
        IGremlinQuery<TResult> Choose<TResult>(Func<IGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IGremlinQuery<TElement>, IGremlinQuery<TResult>> trueChoice);
        IGremlinQuery<TElement> Dedup();
        IGremlinQuery<TElement> Emit();
        IGremlinQuery<TElement> Filter(string lambda);
        IGremlinQuery<TElement[]> Fold();
        IGremlinQuery<TElement> Identity();
        IGremlinQuery<TElement> Inject(params TElement[] elements);
        IGremlinQuery<TElement> Limit(long limit);
        TTargetQuery Local<TTargetQuery>(Func<IGremlinQuery<TElement>, TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;
        TTargetQuery Map<TTargetQuery>(Func<IGremlinQuery<TElement>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;
        IGremlinQuery<TElement> Match(params Func<IGremlinQuery<TElement>, IGremlinQuery<TElement>>[] matchTraversals);
        IGremlinQuery<TElement> Not(Func<IGremlinQuery<TElement>, IGremlinQuery> notTraversal);
        IGremlinQuery<TElement> Optional(Func<IGremlinQuery<TElement>, IGremlinQuery<TElement>> optionalTraversal);
        IGremlinQuery<TElement> Or(params Func<IGremlinQuery<TElement>, IGremlinQuery>[] orTraversals);
        IOrderedGremlinQuery<TElement> OrderBy(Expression<Func<TElement, object>> projection);
        IOrderedGremlinQuery<TElement> OrderBy(Func<IGremlinQuery<TElement>, IGremlinQuery> traversal);
        IOrderedGremlinQuery<TElement> OrderBy(string lambda);
        IOrderedGremlinQuery<TElement> OrderByDescending(Expression<Func<TElement, object>> projection);
        IOrderedGremlinQuery<TElement> OrderByDescending(Func<IGremlinQuery<TElement>, IGremlinQuery> traversal);
        IGremlinQuery<TElement> Range(long low, long high);

        IGremlinQuery<TElement> Repeat(Func<IGremlinQuery<TElement>, IGremlinQuery<TElement>> repeatTraversal);
        IGremlinQuery<TElement> RepeatUntil(Func<IGremlinQuery<TElement>, IGremlinQuery<TElement>> repeatTraversal, Func<IGremlinQuery<TElement>, IGremlinQuery> untilTraversal);

        IGremlinQuery<TElement> SideEffect(Func<IGremlinQuery<TElement>, IGremlinQuery> sideEffectTraversal);
        IGremlinQuery<TElement> Skip(long skip);
        IGremlinQuery<TElement> SumLocal();
        IGremlinQuery<TElement> SumGlobal();
        IGremlinQuery<TElement> Times(int count);
        IGremlinQuery<TElement> Tail(long limit);
        IGremlinQuery<TItem> Unfold<TItem>();

        IGremlinQuery<TElement> Where(Expression<Func<TElement, bool>> predicate);
        IGremlinQuery<TElement> Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal);
        IGremlinQuery<TElement> Where(Func<IGremlinQuery<TElement>, IGremlinQuery> filterTraversal);
    }

    public interface IOrderedVGremlinQuery<TVertex> : IVGremlinQuery<TVertex>
    {
        IOrderedVGremlinQuery<TVertex> ThenBy(Expression<Func<TVertex, object>> projection);
        IOrderedVGremlinQuery<TVertex> ThenBy(Func<IGremlinQuery<TVertex>, IGremlinQuery> traversal);
        IOrderedVGremlinQuery<TVertex> ThenBy(string lambda);
        IOrderedVGremlinQuery<TVertex> ThenByDescending(Expression<Func<TVertex, object>> projection);
        IOrderedVGremlinQuery<TVertex> ThenByDescending(Func<IGremlinQuery<TVertex>, IGremlinQuery> traversal);
    }

    public interface IVGremlinQuery : IGremlinQuery
    {
        IVGremlinQuery<IVertex> Both<TEdge>();
        IEGremlinQuery<TEdge> BothE<TEdge>();

        new IVGremlinQuery<TOtherVertex> Cast<TOtherVertex>();

        IGremlinQuery<object> Id();
        IVGremlinQuery<IVertex> In<TEdge>();

        IVGremlinQuery<TTarget> OfType<TTarget>();
        IVGremlinQuery<IVertex> Out<TEdge>();
    }

    public interface IVGremlinQuery<TVertex> : IGremlinQuery<TVertex>, IVGremlinQuery
    {
        new IEGremlinQuery<TEdge, TVertex> AddE<TEdge>(TEdge edge);
        new IEGremlinQuery<TEdge, TVertex> AddE<TEdge>() where TEdge : new();
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
        IVPropertiesGremlinQuery<VertexProperty<TTarget>, TTarget> Properties<TTarget>(params Expression<Func<TVertex, TTarget[]>>[] projections);

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

    public interface IVPropertiesGremlinQuery<TProperty, TPropertyValue> : IGremlinQuery<TProperty>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IVPropertiesGremlinQuery<TProperty, TPropertyValue>, StepLabel<TPropertyValue[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        IGremlinQuery<object> Id();

        IVPropertiesGremlinQuery<VertexProperty<TPropertyValue, TMeta>, TPropertyValue, TMeta> Meta<TMeta>();

        IGremlinQuery<Property<object>> Properties(params string[] keys);
        IVPropertiesGremlinQuery<TProperty, TPropertyValue> Property(string key, object value);

        IVPropertiesGremlinQuery<TProperty, TPropertyValue> SideEffect(Func<IVPropertiesGremlinQuery<TProperty, TPropertyValue>, IGremlinQuery> sideEffectTraversal);

        IGremlinQuery<object> Values(params string[] keys);
        IGremlinQuery<IDictionary<string, object>> ValueMap();
    }

    public interface IVPropertiesGremlinQuery<TProperty, TPropertyValue, TMeta> : IVPropertiesGremlinQuery<TProperty, TPropertyValue>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IVPropertiesGremlinQuery<TProperty, TPropertyValue, TMeta>, StepLabel<TPropertyValue[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        IGremlinQuery<Property<TTarget>> Properties<TTarget>(params Expression<Func<TMeta, TTarget>>[] projections);
        IVPropertiesGremlinQuery<TProperty, TPropertyValue, TMeta> Property<TValue>(Expression<Func<TMeta, TValue>> projection, TValue value);

        IVPropertiesGremlinQuery<TProperty, TPropertyValue, TMeta> SideEffect(Func<IVPropertiesGremlinQuery<TProperty, TPropertyValue, TMeta>, IGremlinQuery> sideEffectTraversal);

        IGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TMeta, TTarget>>[] projections);
        new IGremlinQuery<TMeta> ValueMap();
    }
    
    public interface IEPropertiesGremlinQuery<TProperty, TPropertyValue> : IGremlinQuery<TProperty>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IEPropertiesGremlinQuery<TProperty, TPropertyValue>, StepLabel<TPropertyValue[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        IEPropertiesGremlinQuery<TProperty, TPropertyValue> SideEffect(Func<IEPropertiesGremlinQuery<TProperty, TPropertyValue>, IGremlinQuery> sideEffectTraversal);
    }

    public interface IOrderedEGremlinQuery<TEdge> : IEGremlinQuery<TEdge>
    {
        IOrderedEGremlinQuery<TEdge> ThenBy(Expression<Func<TEdge, object>> projection);
        IOrderedEGremlinQuery<TEdge> ThenBy(Func<IGremlinQuery<TEdge>, IGremlinQuery> traversal);
        IOrderedEGremlinQuery<TEdge> ThenBy(string lambda);
        IOrderedEGremlinQuery<TEdge> ThenByDescending(Expression<Func<TEdge, object>> projection);
        IOrderedEGremlinQuery<TEdge> ThenByDescending(Func<IGremlinQuery<TEdge>, IGremlinQuery> traversal);
    }

    public interface IEGremlinQuery<TEdge> : IGremlinQuery<TEdge>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IEGremlinQuery<TEdge>, EStepLabel<TEdge[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        TTargetQuery As<TTargetQuery>(Func<IEGremlinQuery<TEdge>, EStepLabel<TEdge>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        new IEGremlinQuery<TEdge> As(StepLabel stepLabel);

        IVGremlinQuery<IVertex> BothV();
        new IEGremlinQuery<TOtherEdge> Cast<TOtherEdge>();

        new IEGremlinQuery<TEdge> Dedup();

        new IEGremlinQuery<TEdge> Emit();

        IOutEGremlinQuery<TEdge, TOutVertex> From<TOutVertex>(Func<IGremlinQuery, IGremlinQuery<TOutVertex>> fromVertexTraversal);
        IOutEGremlinQuery<TEdge, TOutVertex> From<TOutVertex>(StepLabel<TOutVertex> stepLabel);

        new IEGremlinQuery<TEdge> Identity();
        IGremlinQuery<object> Id();
        IVGremlinQuery<IVertex> InV();
        
        new IEGremlinQuery<TEdge> Limit(long limit);
        TTargetQuery Local<TTargetQuery>(Func<IEGremlinQuery<TEdge>, TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IEGremlinQuery<TEdge>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;

        IEGremlinQuery<TTarget> OfType<TTarget>();
        new IOrderedEGremlinQuery<TEdge> OrderBy(Expression<Func<TEdge, object>> projection);
        IOrderedEGremlinQuery<TEdge> OrderBy(Func<IEGremlinQuery<TEdge>, IGremlinQuery> traversal);
        new IOrderedEGremlinQuery<TEdge> OrderBy(string lambda);
        new IOrderedEGremlinQuery<TEdge> OrderByDescending(Expression<Func<TEdge, object>> projection);
        IOrderedEGremlinQuery<TEdge> OrderByDescending(Func<IEGremlinQuery<TEdge>, IGremlinQuery> traversal);
        IVGremlinQuery<IVertex> OtherV();
        IVGremlinQuery<IVertex> OutV();

        IEPropertiesGremlinQuery<Property<TTarget>, TTarget> Properties<TTarget>(params Expression<Func<TEdge, TTarget>>[] projections);
        IEPropertiesGremlinQuery<Property<TTarget>, TTarget> Properties<TTarget>(params Expression<Func<TEdge, TTarget[]>>[] projections);

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

    public interface IOrderedOutEGremlinQuery<TEdge, TAdjacentVertex> : IOutEGremlinQuery<TEdge, TAdjacentVertex>
    {
        IOrderedOutEGremlinQuery<TEdge, TAdjacentVertex> ThenBy(Expression<Func<TEdge, object>> projection);
        IOrderedOutEGremlinQuery<TEdge, TAdjacentVertex> ThenBy(Func<IGremlinQuery<TEdge>, IGremlinQuery> traversal);
        IOrderedOutEGremlinQuery<TEdge, TAdjacentVertex> ThenBy(string lambda);
        IOrderedOutEGremlinQuery<TEdge, TAdjacentVertex> ThenByDescending(Expression<Func<TEdge, object>> projection);
        IOrderedOutEGremlinQuery<TEdge, TAdjacentVertex> ThenByDescending(Func<IGremlinQuery<TEdge>, IGremlinQuery> traversal);
    }

    public interface IOutEGremlinQuery<TEdge, TAdjacentVertex> : IEGremlinQuery<TEdge>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IOutEGremlinQuery<TEdge, TAdjacentVertex>, EStepLabel<TEdge[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IOutEGremlinQuery<TEdge, TAdjacentVertex>, EStepLabel<TEdge, TAdjacentVertex>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
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

    public interface IOrderedInEGremlinQuery<TEdge, TAdjacentVertex> : IInEGremlinQuery<TEdge, TAdjacentVertex>
    {
        IOrderedInEGremlinQuery<TEdge, TAdjacentVertex> ThenBy(Expression<Func<TEdge, object>> projection);
        IOrderedInEGremlinQuery<TEdge, TAdjacentVertex> ThenBy(Func<IGremlinQuery<TEdge>, IGremlinQuery> traversal);
        IOrderedInEGremlinQuery<TEdge, TAdjacentVertex> ThenBy(string lambda);
        IOrderedInEGremlinQuery<TEdge, TAdjacentVertex> ThenByDescending(Expression<Func<TEdge, object>> projection);
        IOrderedInEGremlinQuery<TEdge, TAdjacentVertex> ThenByDescending(Func<IGremlinQuery<TEdge>, IGremlinQuery> traversal);
    }

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

    public interface IOrderedEGremlinQuery<TEdge, TOutVertex, TInVertex> : IEGremlinQuery<TEdge, TOutVertex, TInVertex>
    {
        IOrderedEGremlinQuery<TEdge, TOutVertex, TInVertex> ThenBy(Expression<Func<TEdge, object>> projection);
        IOrderedEGremlinQuery<TEdge, TOutVertex, TInVertex> ThenBy(Func<IGremlinQuery<TEdge>, IGremlinQuery> traversal);
        IOrderedEGremlinQuery<TEdge, TOutVertex, TInVertex> ThenBy(string lambda);
        IOrderedEGremlinQuery<TEdge, TOutVertex, TInVertex> ThenByDescending(Expression<Func<TEdge, object>> projection);
        IOrderedEGremlinQuery<TEdge, TOutVertex, TInVertex> ThenByDescending(Func<IGremlinQuery<TEdge>, IGremlinQuery> traversal);
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
