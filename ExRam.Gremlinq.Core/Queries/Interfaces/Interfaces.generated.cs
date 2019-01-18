#pragma warning disable CS0109 // Member does not hide an inherited member; new keyword is not required
using System;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    public partial interface IOrderedVertexGremlinQuery : IVertexGremlinQuery
    {
    }
    public partial interface IOrderedEdgeGremlinQuery : IEdgeGremlinQuery
    {
    }
    public partial interface IOrderedValueGremlinQuery<TElement> : IValueGremlinQuery<TElement>
    {
    }
    public partial interface IOrderedArrayGremlinQuery<TArray, TQuery> : IArrayGremlinQuery<TArray, TQuery>
    {
    }
    public partial interface IOrderedVertexGremlinQuery<TVertex> : IVertexGremlinQuery<TVertex>
    {
    }
    public partial interface IOrderedEdgeGremlinQuery<TEdge> : IEdgeGremlinQuery<TEdge>
    {
    }
    public partial interface IOrderedEdgeGremlinQuery<TEdge, TAdjacentVertex> : IEdgeGremlinQuery<TEdge, TAdjacentVertex>
    {
    }
    public partial interface IOrderedEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> : IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>
    {
    }
    public partial interface IOrderedInEdgeGremlinQuery<TEdge, TInVertex> : IInEdgeGremlinQuery<TEdge, TInVertex>
    {
    }
    public partial interface IOrderedOutEdgeGremlinQuery<TEdge, TOutVertex> : IOutEdgeGremlinQuery<TEdge, TOutVertex>
    {
    }
    public partial interface IOrderedVertexPropertyGremlinQuery<TProperty, TValue> : IVertexPropertyGremlinQuery<TProperty, TValue>
    {
    }
    public partial interface IOrderedVertexPropertyGremlinQuery<TProperty, TValue, TMeta> : IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>
    {
    }
    public partial interface IOrderedEdgePropertyGremlinQuery<TElement, TValue> : IEdgePropertyGremlinQuery<TElement, TValue>
    {
    }
    public partial interface IGremlinQuery
    {
        IGremlinQuery And(params Func<IGremlinQuery, IGremlinQuery>[] andTraversals);

        new IGremlinQuery As(params StepLabel[] stepLabels);

        new IGremlinQuery Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IGremlinQuery, IGremlinQuery> traversalPredicate, Func<IGremlinQuery, TTargetQuery> trueChoice, Func<IGremlinQuery, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IGremlinQuery, IGremlinQuery> traversalPredicate, Func<IGremlinQuery, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;

        TTargetQuery Coalesce<TTargetQuery>(params Func<IGremlinQuery, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IGremlinQuery Dedup();

        new IGremlinQuery Emit();

        new IGremlinQuery Where(string lambda);
        TTargetQuery FlatMap<TTargetQuery>(Func<IGremlinQuery, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;

        new IGremlinQuery Identity();

        new IGremlinQuery Limit(long count);
        new IGremlinQuery LimitLocal(long count);
        TTargetQuery Local<TTargetQuery>(Func<IGremlinQuery , TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IGremlinQuery, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;
        
        IGremlinQuery Not(Func<IGremlinQuery, IGremlinQuery> notTraversal);

        TTargetQuery Optional<TTargetQuery>(Func<IGremlinQuery, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery;
        IGremlinQuery Or(params Func<IGremlinQuery, IGremlinQuery>[] orTraversals);

        new IGremlinQuery Range(long low, long high);

        TTargetQuery Repeat<TTargetQuery>(Func<IGremlinQuery, TTargetQuery> repeatTraversal) where TTargetQuery : IGremlinQuery;
        TTargetQuery RepeatUntil<TTargetQuery>(Func<IGremlinQuery, TTargetQuery> repeatTraversal, Func<IGremlinQuery, IGremlinQuery> untilTraversal) where TTargetQuery : IGremlinQuery;

        IGremlinQuery SideEffect(Func<IGremlinQuery, IGremlinQuery> sideEffectTraversal);
        new IGremlinQuery Skip(long count);

        new IGremlinQuery Tail(long count);
        new IGremlinQuery TailLocal(int count);

        new IGremlinQuery Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IGremlinQuery, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;
    }

    public partial interface IElementGremlinQuery
    {
        IElementGremlinQuery And(params Func<IElementGremlinQuery, IGremlinQuery>[] andTraversals);

        new IElementGremlinQuery As(params StepLabel[] stepLabels);

        new IElementGremlinQuery Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IElementGremlinQuery, IGremlinQuery> traversalPredicate, Func<IElementGremlinQuery, TTargetQuery> trueChoice, Func<IElementGremlinQuery, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IElementGremlinQuery, IGremlinQuery> traversalPredicate, Func<IElementGremlinQuery, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;

        TTargetQuery Coalesce<TTargetQuery>(params Func<IElementGremlinQuery, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IElementGremlinQuery Dedup();

        new IElementGremlinQuery Emit();

        new IElementGremlinQuery Where(string lambda);
        TTargetQuery FlatMap<TTargetQuery>(Func<IElementGremlinQuery, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;

        new IElementGremlinQuery Identity();

        new IElementGremlinQuery Limit(long count);
        new IElementGremlinQuery LimitLocal(long count);
        TTargetQuery Local<TTargetQuery>(Func<IElementGremlinQuery , TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IElementGremlinQuery, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;
        
        IElementGremlinQuery Not(Func<IElementGremlinQuery, IGremlinQuery> notTraversal);

        TTargetQuery Optional<TTargetQuery>(Func<IElementGremlinQuery, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery;
        IElementGremlinQuery Or(params Func<IElementGremlinQuery, IGremlinQuery>[] orTraversals);

        new IElementGremlinQuery Range(long low, long high);

        TTargetQuery Repeat<TTargetQuery>(Func<IElementGremlinQuery, TTargetQuery> repeatTraversal) where TTargetQuery : IElementGremlinQuery;
        TTargetQuery RepeatUntil<TTargetQuery>(Func<IElementGremlinQuery, TTargetQuery> repeatTraversal, Func<IElementGremlinQuery, IGremlinQuery> untilTraversal) where TTargetQuery : IElementGremlinQuery;

        IElementGremlinQuery SideEffect(Func<IElementGremlinQuery, IGremlinQuery> sideEffectTraversal);
        new IElementGremlinQuery Skip(long count);

        new IElementGremlinQuery Tail(long count);
        new IElementGremlinQuery TailLocal(int count);

        new IElementGremlinQuery Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IElementGremlinQuery, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;
    }

    public partial interface IVertexGremlinQuery
    {
        IVertexGremlinQuery And(params Func<IVertexGremlinQuery, IGremlinQuery>[] andTraversals);

        new IVertexGremlinQuery As(params StepLabel[] stepLabels);

        new IVertexGremlinQuery Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IVertexGremlinQuery, IGremlinQuery> traversalPredicate, Func<IVertexGremlinQuery, TTargetQuery> trueChoice, Func<IVertexGremlinQuery, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IVertexGremlinQuery, IGremlinQuery> traversalPredicate, Func<IVertexGremlinQuery, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;

        TTargetQuery Coalesce<TTargetQuery>(params Func<IVertexGremlinQuery, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IVertexGremlinQuery Dedup();

        new IVertexGremlinQuery Emit();

        new IVertexGremlinQuery Where(string lambda);
        TTargetQuery FlatMap<TTargetQuery>(Func<IVertexGremlinQuery, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;

        new IVertexGremlinQuery Identity();

        new IVertexGremlinQuery Limit(long count);
        new IVertexGremlinQuery LimitLocal(long count);
        TTargetQuery Local<TTargetQuery>(Func<IVertexGremlinQuery , TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IVertexGremlinQuery, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;
        
        IVertexGremlinQuery Not(Func<IVertexGremlinQuery, IGremlinQuery> notTraversal);

        TTargetQuery Optional<TTargetQuery>(Func<IVertexGremlinQuery, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery;
        IVertexGremlinQuery Or(params Func<IVertexGremlinQuery, IGremlinQuery>[] orTraversals);

        new IVertexGremlinQuery Range(long low, long high);

        TTargetQuery Repeat<TTargetQuery>(Func<IVertexGremlinQuery, TTargetQuery> repeatTraversal) where TTargetQuery : IVertexGremlinQuery;
        TTargetQuery RepeatUntil<TTargetQuery>(Func<IVertexGremlinQuery, TTargetQuery> repeatTraversal, Func<IVertexGremlinQuery, IGremlinQuery> untilTraversal) where TTargetQuery : IVertexGremlinQuery;

        IVertexGremlinQuery SideEffect(Func<IVertexGremlinQuery, IGremlinQuery> sideEffectTraversal);
        new IVertexGremlinQuery Skip(long count);

        new IVertexGremlinQuery Tail(long count);
        new IVertexGremlinQuery TailLocal(int count);

        new IVertexGremlinQuery Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IVertexGremlinQuery, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;
    }

    public partial interface IEdgeGremlinQuery
    {
        IEdgeGremlinQuery And(params Func<IEdgeGremlinQuery, IGremlinQuery>[] andTraversals);

        new IEdgeGremlinQuery As(params StepLabel[] stepLabels);

        new IEdgeGremlinQuery Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IEdgeGremlinQuery, IGremlinQuery> traversalPredicate, Func<IEdgeGremlinQuery, TTargetQuery> trueChoice, Func<IEdgeGremlinQuery, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IEdgeGremlinQuery, IGremlinQuery> traversalPredicate, Func<IEdgeGremlinQuery, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;

        TTargetQuery Coalesce<TTargetQuery>(params Func<IEdgeGremlinQuery, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IEdgeGremlinQuery Dedup();

        new IEdgeGremlinQuery Emit();

        new IEdgeGremlinQuery Where(string lambda);
        TTargetQuery FlatMap<TTargetQuery>(Func<IEdgeGremlinQuery, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;

        new IEdgeGremlinQuery Identity();

        new IEdgeGremlinQuery Limit(long count);
        new IEdgeGremlinQuery LimitLocal(long count);
        TTargetQuery Local<TTargetQuery>(Func<IEdgeGremlinQuery , TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IEdgeGremlinQuery, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;
        
        IEdgeGremlinQuery Not(Func<IEdgeGremlinQuery, IGremlinQuery> notTraversal);

        TTargetQuery Optional<TTargetQuery>(Func<IEdgeGremlinQuery, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery;
        IEdgeGremlinQuery Or(params Func<IEdgeGremlinQuery, IGremlinQuery>[] orTraversals);

        new IEdgeGremlinQuery Range(long low, long high);

        TTargetQuery Repeat<TTargetQuery>(Func<IEdgeGremlinQuery, TTargetQuery> repeatTraversal) where TTargetQuery : IEdgeGremlinQuery;
        TTargetQuery RepeatUntil<TTargetQuery>(Func<IEdgeGremlinQuery, TTargetQuery> repeatTraversal, Func<IEdgeGremlinQuery, IGremlinQuery> untilTraversal) where TTargetQuery : IEdgeGremlinQuery;

        IEdgeGremlinQuery SideEffect(Func<IEdgeGremlinQuery, IGremlinQuery> sideEffectTraversal);
        new IEdgeGremlinQuery Skip(long count);

        new IEdgeGremlinQuery Tail(long count);
        new IEdgeGremlinQuery TailLocal(int count);

        new IEdgeGremlinQuery Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IEdgeGremlinQuery, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;
    }

    public partial interface IGremlinQuery<TElement>
    {
        IGremlinQuery<TElement> And(params Func<IGremlinQuery<TElement>, IGremlinQuery>[] andTraversals);

        new IGremlinQuery<TElement> As(params StepLabel[] stepLabels);

        new IGremlinQuery<TElement> Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IGremlinQuery<TElement>, TTargetQuery> trueChoice, Func<IGremlinQuery<TElement>, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IGremlinQuery<TElement>, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;

        TTargetQuery Coalesce<TTargetQuery>(params Func<IGremlinQuery<TElement>, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IGremlinQuery<TElement> Dedup();

        new IGremlinQuery<TElement> Emit();

        new IGremlinQuery<TElement> Where(string lambda);
        TTargetQuery FlatMap<TTargetQuery>(Func<IGremlinQuery<TElement>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;

        new IGremlinQuery<TElement> Identity();

        new IGremlinQuery<TElement> Limit(long count);
        new IGremlinQuery<TElement> LimitLocal(long count);
        TTargetQuery Local<TTargetQuery>(Func<IGremlinQuery<TElement> , TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IGremlinQuery<TElement>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;
        
        IGremlinQuery<TElement> Not(Func<IGremlinQuery<TElement>, IGremlinQuery> notTraversal);

        TTargetQuery Optional<TTargetQuery>(Func<IGremlinQuery<TElement>, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery;
        IGremlinQuery<TElement> Or(params Func<IGremlinQuery<TElement>, IGremlinQuery>[] orTraversals);

        new IGremlinQuery<TElement> Range(long low, long high);

        TTargetQuery Repeat<TTargetQuery>(Func<IGremlinQuery<TElement>, TTargetQuery> repeatTraversal) where TTargetQuery : IGremlinQuery<TElement>;
        TTargetQuery RepeatUntil<TTargetQuery>(Func<IGremlinQuery<TElement>, TTargetQuery> repeatTraversal, Func<IGremlinQuery<TElement>, IGremlinQuery> untilTraversal) where TTargetQuery : IGremlinQuery<TElement>;

        IGremlinQuery<TElement> SideEffect(Func<IGremlinQuery<TElement>, IGremlinQuery> sideEffectTraversal);
        new IGremlinQuery<TElement> Skip(long count);

        new IGremlinQuery<TElement> Tail(long count);
        new IGremlinQuery<TElement> TailLocal(int count);

        new IGremlinQuery<TElement> Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IGremlinQuery<TElement>, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;
    }

    public partial interface IValueGremlinQuery<TElement>
    {
        IValueGremlinQuery<TElement> And(params Func<IValueGremlinQuery<TElement>, IGremlinQuery>[] andTraversals);

        new IValueGremlinQuery<TElement> As(params StepLabel[] stepLabels);

        new IValueGremlinQuery<TElement> Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IValueGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IValueGremlinQuery<TElement>, TTargetQuery> trueChoice, Func<IValueGremlinQuery<TElement>, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IValueGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IValueGremlinQuery<TElement>, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;

        TTargetQuery Coalesce<TTargetQuery>(params Func<IValueGremlinQuery<TElement>, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IValueGremlinQuery<TElement> Dedup();

        new IValueGremlinQuery<TElement> Emit();

        new IValueGremlinQuery<TElement> Where(string lambda);
        TTargetQuery FlatMap<TTargetQuery>(Func<IValueGremlinQuery<TElement>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;

        new IValueGremlinQuery<TElement> Identity();

        new IValueGremlinQuery<TElement> Limit(long count);
        new IValueGremlinQuery<TElement> LimitLocal(long count);
        TTargetQuery Local<TTargetQuery>(Func<IValueGremlinQuery<TElement> , TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IValueGremlinQuery<TElement>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;
        
        IValueGremlinQuery<TElement> Not(Func<IValueGremlinQuery<TElement>, IGremlinQuery> notTraversal);

        TTargetQuery Optional<TTargetQuery>(Func<IValueGremlinQuery<TElement>, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery;
        IValueGremlinQuery<TElement> Or(params Func<IValueGremlinQuery<TElement>, IGremlinQuery>[] orTraversals);

        new IValueGremlinQuery<TElement> Range(long low, long high);

        TTargetQuery Repeat<TTargetQuery>(Func<IValueGremlinQuery<TElement>, TTargetQuery> repeatTraversal) where TTargetQuery : IValueGremlinQuery<TElement>;
        TTargetQuery RepeatUntil<TTargetQuery>(Func<IValueGremlinQuery<TElement>, TTargetQuery> repeatTraversal, Func<IValueGremlinQuery<TElement>, IGremlinQuery> untilTraversal) where TTargetQuery : IValueGremlinQuery<TElement>;

        IValueGremlinQuery<TElement> SideEffect(Func<IValueGremlinQuery<TElement>, IGremlinQuery> sideEffectTraversal);
        new IValueGremlinQuery<TElement> Skip(long count);

        new IValueGremlinQuery<TElement> Tail(long count);
        new IValueGremlinQuery<TElement> TailLocal(int count);

        new IValueGremlinQuery<TElement> Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IValueGremlinQuery<TElement>, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;
    }

    public partial interface IArrayGremlinQuery<TArray, TQuery>
    {
        IArrayGremlinQuery<TArray, TQuery> And(params Func<IArrayGremlinQuery<TArray, TQuery>, IGremlinQuery>[] andTraversals);

        new IArrayGremlinQuery<TArray, TQuery> As(params StepLabel[] stepLabels);

        new IArrayGremlinQuery<TArray, TQuery> Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IArrayGremlinQuery<TArray, TQuery>, IGremlinQuery> traversalPredicate, Func<IArrayGremlinQuery<TArray, TQuery>, TTargetQuery> trueChoice, Func<IArrayGremlinQuery<TArray, TQuery>, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IArrayGremlinQuery<TArray, TQuery>, IGremlinQuery> traversalPredicate, Func<IArrayGremlinQuery<TArray, TQuery>, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;

        TTargetQuery Coalesce<TTargetQuery>(params Func<IArrayGremlinQuery<TArray, TQuery>, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IArrayGremlinQuery<TArray, TQuery> Dedup();

        new IArrayGremlinQuery<TArray, TQuery> Emit();

        new IArrayGremlinQuery<TArray, TQuery> Where(string lambda);
        TTargetQuery FlatMap<TTargetQuery>(Func<IArrayGremlinQuery<TArray, TQuery>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;

        new IArrayGremlinQuery<TArray, TQuery> Identity();

        new IArrayGremlinQuery<TArray, TQuery> Limit(long count);
        new IArrayGremlinQuery<TArray, TQuery> LimitLocal(long count);
        TTargetQuery Local<TTargetQuery>(Func<IArrayGremlinQuery<TArray, TQuery> , TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IArrayGremlinQuery<TArray, TQuery>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;
        
        IArrayGremlinQuery<TArray, TQuery> Not(Func<IArrayGremlinQuery<TArray, TQuery>, IGremlinQuery> notTraversal);

        TTargetQuery Optional<TTargetQuery>(Func<IArrayGremlinQuery<TArray, TQuery>, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery;
        IArrayGremlinQuery<TArray, TQuery> Or(params Func<IArrayGremlinQuery<TArray, TQuery>, IGremlinQuery>[] orTraversals);

        new IArrayGremlinQuery<TArray, TQuery> Range(long low, long high);

        TTargetQuery Repeat<TTargetQuery>(Func<IArrayGremlinQuery<TArray, TQuery>, TTargetQuery> repeatTraversal) where TTargetQuery : IArrayGremlinQuery<TArray, TQuery>;
        TTargetQuery RepeatUntil<TTargetQuery>(Func<IArrayGremlinQuery<TArray, TQuery>, TTargetQuery> repeatTraversal, Func<IArrayGremlinQuery<TArray, TQuery>, IGremlinQuery> untilTraversal) where TTargetQuery : IArrayGremlinQuery<TArray, TQuery>;

        IArrayGremlinQuery<TArray, TQuery> SideEffect(Func<IArrayGremlinQuery<TArray, TQuery>, IGremlinQuery> sideEffectTraversal);
        new IArrayGremlinQuery<TArray, TQuery> Skip(long count);

        new IArrayGremlinQuery<TArray, TQuery> Tail(long count);
        new IArrayGremlinQuery<TArray, TQuery> TailLocal(int count);

        new IArrayGremlinQuery<TArray, TQuery> Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IArrayGremlinQuery<TArray, TQuery>, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;
    }

    public partial interface IElementGremlinQuery<TElement>
    {
        IElementGremlinQuery<TElement> And(params Func<IElementGremlinQuery<TElement>, IGremlinQuery>[] andTraversals);

        new IElementGremlinQuery<TElement> As(params StepLabel[] stepLabels);

        new IElementGremlinQuery<TElement> Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IElementGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IElementGremlinQuery<TElement>, TTargetQuery> trueChoice, Func<IElementGremlinQuery<TElement>, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IElementGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IElementGremlinQuery<TElement>, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;

        TTargetQuery Coalesce<TTargetQuery>(params Func<IElementGremlinQuery<TElement>, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IElementGremlinQuery<TElement> Dedup();

        new IElementGremlinQuery<TElement> Emit();

        new IElementGremlinQuery<TElement> Where(string lambda);
        TTargetQuery FlatMap<TTargetQuery>(Func<IElementGremlinQuery<TElement>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;

        new IElementGremlinQuery<TElement> Identity();

        new IElementGremlinQuery<TElement> Limit(long count);
        new IElementGremlinQuery<TElement> LimitLocal(long count);
        TTargetQuery Local<TTargetQuery>(Func<IElementGremlinQuery<TElement> , TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IElementGremlinQuery<TElement>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;
        
        IElementGremlinQuery<TElement> Not(Func<IElementGremlinQuery<TElement>, IGremlinQuery> notTraversal);

        TTargetQuery Optional<TTargetQuery>(Func<IElementGremlinQuery<TElement>, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery;
        IElementGremlinQuery<TElement> Or(params Func<IElementGremlinQuery<TElement>, IGremlinQuery>[] orTraversals);

        new IElementGremlinQuery<TElement> Range(long low, long high);

        TTargetQuery Repeat<TTargetQuery>(Func<IElementGremlinQuery<TElement>, TTargetQuery> repeatTraversal) where TTargetQuery : IElementGremlinQuery<TElement>;
        TTargetQuery RepeatUntil<TTargetQuery>(Func<IElementGremlinQuery<TElement>, TTargetQuery> repeatTraversal, Func<IElementGremlinQuery<TElement>, IGremlinQuery> untilTraversal) where TTargetQuery : IElementGremlinQuery<TElement>;

        IElementGremlinQuery<TElement> SideEffect(Func<IElementGremlinQuery<TElement>, IGremlinQuery> sideEffectTraversal);
        new IElementGremlinQuery<TElement> Skip(long count);

        new IElementGremlinQuery<TElement> Tail(long count);
        new IElementGremlinQuery<TElement> TailLocal(int count);

        new IElementGremlinQuery<TElement> Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IElementGremlinQuery<TElement>, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;
    }

    public partial interface IVertexGremlinQuery<TVertex>
    {
        IVertexGremlinQuery<TVertex> And(params Func<IVertexGremlinQuery<TVertex>, IGremlinQuery>[] andTraversals);

        new IVertexGremlinQuery<TVertex> As(params StepLabel[] stepLabels);

        new IVertexGremlinQuery<TVertex> Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IVertexGremlinQuery<TVertex>, IGremlinQuery> traversalPredicate, Func<IVertexGremlinQuery<TVertex>, TTargetQuery> trueChoice, Func<IVertexGremlinQuery<TVertex>, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IVertexGremlinQuery<TVertex>, IGremlinQuery> traversalPredicate, Func<IVertexGremlinQuery<TVertex>, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;

        TTargetQuery Coalesce<TTargetQuery>(params Func<IVertexGremlinQuery<TVertex>, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IVertexGremlinQuery<TVertex> Dedup();

        new IVertexGremlinQuery<TVertex> Emit();

        new IVertexGremlinQuery<TVertex> Where(string lambda);
        TTargetQuery FlatMap<TTargetQuery>(Func<IVertexGremlinQuery<TVertex>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;

        new IVertexGremlinQuery<TVertex> Identity();

        new IVertexGremlinQuery<TVertex> Limit(long count);
        new IVertexGremlinQuery<TVertex> LimitLocal(long count);
        TTargetQuery Local<TTargetQuery>(Func<IVertexGremlinQuery<TVertex> , TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IVertexGremlinQuery<TVertex>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;
        
        IVertexGremlinQuery<TVertex> Not(Func<IVertexGremlinQuery<TVertex>, IGremlinQuery> notTraversal);

        TTargetQuery Optional<TTargetQuery>(Func<IVertexGremlinQuery<TVertex>, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery;
        IVertexGremlinQuery<TVertex> Or(params Func<IVertexGremlinQuery<TVertex>, IGremlinQuery>[] orTraversals);

        new IVertexGremlinQuery<TVertex> Range(long low, long high);

        TTargetQuery Repeat<TTargetQuery>(Func<IVertexGremlinQuery<TVertex>, TTargetQuery> repeatTraversal) where TTargetQuery : IVertexGremlinQuery<TVertex>;
        TTargetQuery RepeatUntil<TTargetQuery>(Func<IVertexGremlinQuery<TVertex>, TTargetQuery> repeatTraversal, Func<IVertexGremlinQuery<TVertex>, IGremlinQuery> untilTraversal) where TTargetQuery : IVertexGremlinQuery<TVertex>;

        IVertexGremlinQuery<TVertex> SideEffect(Func<IVertexGremlinQuery<TVertex>, IGremlinQuery> sideEffectTraversal);
        new IVertexGremlinQuery<TVertex> Skip(long count);

        new IVertexGremlinQuery<TVertex> Tail(long count);
        new IVertexGremlinQuery<TVertex> TailLocal(int count);

        new IVertexGremlinQuery<TVertex> Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IVertexGremlinQuery<TVertex>, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;
    }

    public partial interface IEdgeGremlinQuery<TEdge>
    {
        IEdgeGremlinQuery<TEdge> And(params Func<IEdgeGremlinQuery<TEdge>, IGremlinQuery>[] andTraversals);

        new IEdgeGremlinQuery<TEdge> As(params StepLabel[] stepLabels);

        new IEdgeGremlinQuery<TEdge> Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge>, IGremlinQuery> traversalPredicate, Func<IEdgeGremlinQuery<TEdge>, TTargetQuery> trueChoice, Func<IEdgeGremlinQuery<TEdge>, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge>, IGremlinQuery> traversalPredicate, Func<IEdgeGremlinQuery<TEdge>, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;

        TTargetQuery Coalesce<TTargetQuery>(params Func<IEdgeGremlinQuery<TEdge>, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IEdgeGremlinQuery<TEdge> Dedup();

        new IEdgeGremlinQuery<TEdge> Emit();

        new IEdgeGremlinQuery<TEdge> Where(string lambda);
        TTargetQuery FlatMap<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;

        new IEdgeGremlinQuery<TEdge> Identity();

        new IEdgeGremlinQuery<TEdge> Limit(long count);
        new IEdgeGremlinQuery<TEdge> LimitLocal(long count);
        TTargetQuery Local<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge> , TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;
        
        IEdgeGremlinQuery<TEdge> Not(Func<IEdgeGremlinQuery<TEdge>, IGremlinQuery> notTraversal);

        TTargetQuery Optional<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge>, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery;
        IEdgeGremlinQuery<TEdge> Or(params Func<IEdgeGremlinQuery<TEdge>, IGremlinQuery>[] orTraversals);

        new IEdgeGremlinQuery<TEdge> Range(long low, long high);

        TTargetQuery Repeat<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge>, TTargetQuery> repeatTraversal) where TTargetQuery : IEdgeGremlinQuery<TEdge>;
        TTargetQuery RepeatUntil<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge>, TTargetQuery> repeatTraversal, Func<IEdgeGremlinQuery<TEdge>, IGremlinQuery> untilTraversal) where TTargetQuery : IEdgeGremlinQuery<TEdge>;

        IEdgeGremlinQuery<TEdge> SideEffect(Func<IEdgeGremlinQuery<TEdge>, IGremlinQuery> sideEffectTraversal);
        new IEdgeGremlinQuery<TEdge> Skip(long count);

        new IEdgeGremlinQuery<TEdge> Tail(long count);
        new IEdgeGremlinQuery<TEdge> TailLocal(int count);

        new IEdgeGremlinQuery<TEdge> Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IEdgeGremlinQuery<TEdge>, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;
    }

    public partial interface IEdgeGremlinQuery<TEdge, TAdjacentVertex>
    {
        IEdgeGremlinQuery<TEdge, TAdjacentVertex> And(params Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery>[] andTraversals);

        new IEdgeGremlinQuery<TEdge, TAdjacentVertex> As(params StepLabel[] stepLabels);

        new IEdgeGremlinQuery<TEdge, TAdjacentVertex> Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> traversalPredicate, Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery> trueChoice, Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> traversalPredicate, Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;

        TTargetQuery Coalesce<TTargetQuery>(params Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IEdgeGremlinQuery<TEdge, TAdjacentVertex> Dedup();

        new IEdgeGremlinQuery<TEdge, TAdjacentVertex> Emit();

        new IEdgeGremlinQuery<TEdge, TAdjacentVertex> Where(string lambda);
        TTargetQuery FlatMap<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;

        new IEdgeGremlinQuery<TEdge, TAdjacentVertex> Identity();

        new IEdgeGremlinQuery<TEdge, TAdjacentVertex> Limit(long count);
        new IEdgeGremlinQuery<TEdge, TAdjacentVertex> LimitLocal(long count);
        TTargetQuery Local<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex> , TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;
        
        IEdgeGremlinQuery<TEdge, TAdjacentVertex> Not(Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> notTraversal);

        TTargetQuery Optional<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery;
        IEdgeGremlinQuery<TEdge, TAdjacentVertex> Or(params Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery>[] orTraversals);

        new IEdgeGremlinQuery<TEdge, TAdjacentVertex> Range(long low, long high);

        TTargetQuery Repeat<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery> repeatTraversal) where TTargetQuery : IEdgeGremlinQuery<TEdge, TAdjacentVertex>;
        TTargetQuery RepeatUntil<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery> repeatTraversal, Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> untilTraversal) where TTargetQuery : IEdgeGremlinQuery<TEdge, TAdjacentVertex>;

        IEdgeGremlinQuery<TEdge, TAdjacentVertex> SideEffect(Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> sideEffectTraversal);
        new IEdgeGremlinQuery<TEdge, TAdjacentVertex> Skip(long count);

        new IEdgeGremlinQuery<TEdge, TAdjacentVertex> Tail(long count);
        new IEdgeGremlinQuery<TEdge, TAdjacentVertex> TailLocal(int count);

        new IEdgeGremlinQuery<TEdge, TAdjacentVertex> Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;
    }

    public partial interface IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>
    {
        IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> And(params Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, IGremlinQuery>[] andTraversals);

        new IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> As(params StepLabel[] stepLabels);

        new IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, IGremlinQuery> traversalPredicate, Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, TTargetQuery> trueChoice, Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, IGremlinQuery> traversalPredicate, Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;

        TTargetQuery Coalesce<TTargetQuery>(params Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> Dedup();

        new IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> Emit();

        new IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> Where(string lambda);
        TTargetQuery FlatMap<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;

        new IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> Identity();

        new IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> Limit(long count);
        new IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> LimitLocal(long count);
        TTargetQuery Local<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> , TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;
        
        IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> Not(Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, IGremlinQuery> notTraversal);

        TTargetQuery Optional<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery;
        IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> Or(params Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, IGremlinQuery>[] orTraversals);

        new IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> Range(long low, long high);

        TTargetQuery Repeat<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, TTargetQuery> repeatTraversal) where TTargetQuery : IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>;
        TTargetQuery RepeatUntil<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, TTargetQuery> repeatTraversal, Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, IGremlinQuery> untilTraversal) where TTargetQuery : IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>;

        IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> SideEffect(Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, IGremlinQuery> sideEffectTraversal);
        new IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> Skip(long count);

        new IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> Tail(long count);
        new IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> TailLocal(int count);

        new IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;
    }

    public partial interface IInEdgeGremlinQuery<TEdge, TInVertex>
    {
        IInEdgeGremlinQuery<TEdge, TInVertex> And(params Func<IInEdgeGremlinQuery<TEdge, TInVertex>, IGremlinQuery>[] andTraversals);

        new IInEdgeGremlinQuery<TEdge, TInVertex> As(params StepLabel[] stepLabels);

        new IInEdgeGremlinQuery<TEdge, TInVertex> Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IInEdgeGremlinQuery<TEdge, TInVertex>, IGremlinQuery> traversalPredicate, Func<IInEdgeGremlinQuery<TEdge, TInVertex>, TTargetQuery> trueChoice, Func<IInEdgeGremlinQuery<TEdge, TInVertex>, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IInEdgeGremlinQuery<TEdge, TInVertex>, IGremlinQuery> traversalPredicate, Func<IInEdgeGremlinQuery<TEdge, TInVertex>, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;

        TTargetQuery Coalesce<TTargetQuery>(params Func<IInEdgeGremlinQuery<TEdge, TInVertex>, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IInEdgeGremlinQuery<TEdge, TInVertex> Dedup();

        new IInEdgeGremlinQuery<TEdge, TInVertex> Emit();

        new IInEdgeGremlinQuery<TEdge, TInVertex> Where(string lambda);
        TTargetQuery FlatMap<TTargetQuery>(Func<IInEdgeGremlinQuery<TEdge, TInVertex>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;

        new IInEdgeGremlinQuery<TEdge, TInVertex> Identity();

        new IInEdgeGremlinQuery<TEdge, TInVertex> Limit(long count);
        new IInEdgeGremlinQuery<TEdge, TInVertex> LimitLocal(long count);
        TTargetQuery Local<TTargetQuery>(Func<IInEdgeGremlinQuery<TEdge, TInVertex> , TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IInEdgeGremlinQuery<TEdge, TInVertex>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;
        
        IInEdgeGremlinQuery<TEdge, TInVertex> Not(Func<IInEdgeGremlinQuery<TEdge, TInVertex>, IGremlinQuery> notTraversal);

        TTargetQuery Optional<TTargetQuery>(Func<IInEdgeGremlinQuery<TEdge, TInVertex>, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery;
        IInEdgeGremlinQuery<TEdge, TInVertex> Or(params Func<IInEdgeGremlinQuery<TEdge, TInVertex>, IGremlinQuery>[] orTraversals);

        new IInEdgeGremlinQuery<TEdge, TInVertex> Range(long low, long high);

        TTargetQuery Repeat<TTargetQuery>(Func<IInEdgeGremlinQuery<TEdge, TInVertex>, TTargetQuery> repeatTraversal) where TTargetQuery : IInEdgeGremlinQuery<TEdge, TInVertex>;
        TTargetQuery RepeatUntil<TTargetQuery>(Func<IInEdgeGremlinQuery<TEdge, TInVertex>, TTargetQuery> repeatTraversal, Func<IInEdgeGremlinQuery<TEdge, TInVertex>, IGremlinQuery> untilTraversal) where TTargetQuery : IInEdgeGremlinQuery<TEdge, TInVertex>;

        IInEdgeGremlinQuery<TEdge, TInVertex> SideEffect(Func<IInEdgeGremlinQuery<TEdge, TInVertex>, IGremlinQuery> sideEffectTraversal);
        new IInEdgeGremlinQuery<TEdge, TInVertex> Skip(long count);

        new IInEdgeGremlinQuery<TEdge, TInVertex> Tail(long count);
        new IInEdgeGremlinQuery<TEdge, TInVertex> TailLocal(int count);

        new IInEdgeGremlinQuery<TEdge, TInVertex> Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IInEdgeGremlinQuery<TEdge, TInVertex>, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;
    }

    public partial interface IOutEdgeGremlinQuery<TEdge, TOutVertex>
    {
        IOutEdgeGremlinQuery<TEdge, TOutVertex> And(params Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, IGremlinQuery>[] andTraversals);

        new IOutEdgeGremlinQuery<TEdge, TOutVertex> As(params StepLabel[] stepLabels);

        new IOutEdgeGremlinQuery<TEdge, TOutVertex> Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, IGremlinQuery> traversalPredicate, Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, TTargetQuery> trueChoice, Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, IGremlinQuery> traversalPredicate, Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;

        TTargetQuery Coalesce<TTargetQuery>(params Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IOutEdgeGremlinQuery<TEdge, TOutVertex> Dedup();

        new IOutEdgeGremlinQuery<TEdge, TOutVertex> Emit();

        new IOutEdgeGremlinQuery<TEdge, TOutVertex> Where(string lambda);
        TTargetQuery FlatMap<TTargetQuery>(Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;

        new IOutEdgeGremlinQuery<TEdge, TOutVertex> Identity();

        new IOutEdgeGremlinQuery<TEdge, TOutVertex> Limit(long count);
        new IOutEdgeGremlinQuery<TEdge, TOutVertex> LimitLocal(long count);
        TTargetQuery Local<TTargetQuery>(Func<IOutEdgeGremlinQuery<TEdge, TOutVertex> , TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;
        
        IOutEdgeGremlinQuery<TEdge, TOutVertex> Not(Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, IGremlinQuery> notTraversal);

        TTargetQuery Optional<TTargetQuery>(Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery;
        IOutEdgeGremlinQuery<TEdge, TOutVertex> Or(params Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, IGremlinQuery>[] orTraversals);

        new IOutEdgeGremlinQuery<TEdge, TOutVertex> Range(long low, long high);

        TTargetQuery Repeat<TTargetQuery>(Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, TTargetQuery> repeatTraversal) where TTargetQuery : IOutEdgeGremlinQuery<TEdge, TOutVertex>;
        TTargetQuery RepeatUntil<TTargetQuery>(Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, TTargetQuery> repeatTraversal, Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, IGremlinQuery> untilTraversal) where TTargetQuery : IOutEdgeGremlinQuery<TEdge, TOutVertex>;

        IOutEdgeGremlinQuery<TEdge, TOutVertex> SideEffect(Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, IGremlinQuery> sideEffectTraversal);
        new IOutEdgeGremlinQuery<TEdge, TOutVertex> Skip(long count);

        new IOutEdgeGremlinQuery<TEdge, TOutVertex> Tail(long count);
        new IOutEdgeGremlinQuery<TEdge, TOutVertex> TailLocal(int count);

        new IOutEdgeGremlinQuery<TEdge, TOutVertex> Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;
    }

    public partial interface IVertexPropertyGremlinQuery<TProperty, TValue>
    {
        IVertexPropertyGremlinQuery<TProperty, TValue> And(params Func<IVertexPropertyGremlinQuery<TProperty, TValue>, IGremlinQuery>[] andTraversals);

        new IVertexPropertyGremlinQuery<TProperty, TValue> As(params StepLabel[] stepLabels);

        new IVertexPropertyGremlinQuery<TProperty, TValue> Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TProperty, TValue>, IGremlinQuery> traversalPredicate, Func<IVertexPropertyGremlinQuery<TProperty, TValue>, TTargetQuery> trueChoice, Func<IVertexPropertyGremlinQuery<TProperty, TValue>, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TProperty, TValue>, IGremlinQuery> traversalPredicate, Func<IVertexPropertyGremlinQuery<TProperty, TValue>, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;

        TTargetQuery Coalesce<TTargetQuery>(params Func<IVertexPropertyGremlinQuery<TProperty, TValue>, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IVertexPropertyGremlinQuery<TProperty, TValue> Dedup();

        new IVertexPropertyGremlinQuery<TProperty, TValue> Emit();

        new IVertexPropertyGremlinQuery<TProperty, TValue> Where(string lambda);
        TTargetQuery FlatMap<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TProperty, TValue>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;

        new IVertexPropertyGremlinQuery<TProperty, TValue> Identity();

        new IVertexPropertyGremlinQuery<TProperty, TValue> Limit(long count);
        new IVertexPropertyGremlinQuery<TProperty, TValue> LimitLocal(long count);
        TTargetQuery Local<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TProperty, TValue> , TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TProperty, TValue>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;
        
        IVertexPropertyGremlinQuery<TProperty, TValue> Not(Func<IVertexPropertyGremlinQuery<TProperty, TValue>, IGremlinQuery> notTraversal);

        TTargetQuery Optional<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TProperty, TValue>, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery;
        IVertexPropertyGremlinQuery<TProperty, TValue> Or(params Func<IVertexPropertyGremlinQuery<TProperty, TValue>, IGremlinQuery>[] orTraversals);

        new IVertexPropertyGremlinQuery<TProperty, TValue> Range(long low, long high);

        TTargetQuery Repeat<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TProperty, TValue>, TTargetQuery> repeatTraversal) where TTargetQuery : IVertexPropertyGremlinQuery<TProperty, TValue>;
        TTargetQuery RepeatUntil<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TProperty, TValue>, TTargetQuery> repeatTraversal, Func<IVertexPropertyGremlinQuery<TProperty, TValue>, IGremlinQuery> untilTraversal) where TTargetQuery : IVertexPropertyGremlinQuery<TProperty, TValue>;

        IVertexPropertyGremlinQuery<TProperty, TValue> SideEffect(Func<IVertexPropertyGremlinQuery<TProperty, TValue>, IGremlinQuery> sideEffectTraversal);
        new IVertexPropertyGremlinQuery<TProperty, TValue> Skip(long count);

        new IVertexPropertyGremlinQuery<TProperty, TValue> Tail(long count);
        new IVertexPropertyGremlinQuery<TProperty, TValue> TailLocal(int count);

        new IVertexPropertyGremlinQuery<TProperty, TValue> Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IVertexPropertyGremlinQuery<TProperty, TValue>, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;
    }

    public partial interface IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>
    {
        IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> And(params Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, IGremlinQuery>[] andTraversals);

        new IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> As(params StepLabel[] stepLabels);

        new IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, IGremlinQuery> traversalPredicate, Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, TTargetQuery> trueChoice, Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, IGremlinQuery> traversalPredicate, Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;

        TTargetQuery Coalesce<TTargetQuery>(params Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Dedup();

        new IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Emit();

        new IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Where(string lambda);
        TTargetQuery FlatMap<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;

        new IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Identity();

        new IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Limit(long count);
        new IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> LimitLocal(long count);
        TTargetQuery Local<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> , TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;
        
        IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Not(Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, IGremlinQuery> notTraversal);

        TTargetQuery Optional<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery;
        IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Or(params Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, IGremlinQuery>[] orTraversals);

        new IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Range(long low, long high);

        TTargetQuery Repeat<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, TTargetQuery> repeatTraversal) where TTargetQuery : IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>;
        TTargetQuery RepeatUntil<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, TTargetQuery> repeatTraversal, Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, IGremlinQuery> untilTraversal) where TTargetQuery : IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>;

        IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> SideEffect(Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, IGremlinQuery> sideEffectTraversal);
        new IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Skip(long count);

        new IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Tail(long count);
        new IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> TailLocal(int count);

        new IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;
    }

    public partial interface IEdgePropertyGremlinQuery<TElement, TValue>
    {
        IEdgePropertyGremlinQuery<TElement, TValue> And(params Func<IEdgePropertyGremlinQuery<TElement, TValue>, IGremlinQuery>[] andTraversals);

        new IEdgePropertyGremlinQuery<TElement, TValue> As(params StepLabel[] stepLabels);

        new IEdgePropertyGremlinQuery<TElement, TValue> Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IEdgePropertyGremlinQuery<TElement, TValue>, IGremlinQuery> traversalPredicate, Func<IEdgePropertyGremlinQuery<TElement, TValue>, TTargetQuery> trueChoice, Func<IEdgePropertyGremlinQuery<TElement, TValue>, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IEdgePropertyGremlinQuery<TElement, TValue>, IGremlinQuery> traversalPredicate, Func<IEdgePropertyGremlinQuery<TElement, TValue>, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;

        TTargetQuery Coalesce<TTargetQuery>(params Func<IEdgePropertyGremlinQuery<TElement, TValue>, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IEdgePropertyGremlinQuery<TElement, TValue> Dedup();

        new IEdgePropertyGremlinQuery<TElement, TValue> Emit();

        new IEdgePropertyGremlinQuery<TElement, TValue> Where(string lambda);
        TTargetQuery FlatMap<TTargetQuery>(Func<IEdgePropertyGremlinQuery<TElement, TValue>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;

        new IEdgePropertyGremlinQuery<TElement, TValue> Identity();

        new IEdgePropertyGremlinQuery<TElement, TValue> Limit(long count);
        new IEdgePropertyGremlinQuery<TElement, TValue> LimitLocal(long count);
        TTargetQuery Local<TTargetQuery>(Func<IEdgePropertyGremlinQuery<TElement, TValue> , TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IEdgePropertyGremlinQuery<TElement, TValue>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;
        
        IEdgePropertyGremlinQuery<TElement, TValue> Not(Func<IEdgePropertyGremlinQuery<TElement, TValue>, IGremlinQuery> notTraversal);

        TTargetQuery Optional<TTargetQuery>(Func<IEdgePropertyGremlinQuery<TElement, TValue>, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery;
        IEdgePropertyGremlinQuery<TElement, TValue> Or(params Func<IEdgePropertyGremlinQuery<TElement, TValue>, IGremlinQuery>[] orTraversals);

        new IEdgePropertyGremlinQuery<TElement, TValue> Range(long low, long high);

        TTargetQuery Repeat<TTargetQuery>(Func<IEdgePropertyGremlinQuery<TElement, TValue>, TTargetQuery> repeatTraversal) where TTargetQuery : IEdgePropertyGremlinQuery<TElement, TValue>;
        TTargetQuery RepeatUntil<TTargetQuery>(Func<IEdgePropertyGremlinQuery<TElement, TValue>, TTargetQuery> repeatTraversal, Func<IEdgePropertyGremlinQuery<TElement, TValue>, IGremlinQuery> untilTraversal) where TTargetQuery : IEdgePropertyGremlinQuery<TElement, TValue>;

        IEdgePropertyGremlinQuery<TElement, TValue> SideEffect(Func<IEdgePropertyGremlinQuery<TElement, TValue>, IGremlinQuery> sideEffectTraversal);
        new IEdgePropertyGremlinQuery<TElement, TValue> Skip(long count);

        new IEdgePropertyGremlinQuery<TElement, TValue> Tail(long count);
        new IEdgePropertyGremlinQuery<TElement, TValue> TailLocal(int count);

        new IEdgePropertyGremlinQuery<TElement, TValue> Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IEdgePropertyGremlinQuery<TElement, TValue>, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;
    }

    public partial interface IGremlinQuery
    {
        new IGremlinQuery<TResult> Cast<TResult>();
    }

    public partial interface IElementGremlinQuery
    {
        new IElementGremlinQuery<TResult> Cast<TResult>();
    }

    public partial interface IVertexGremlinQuery
    {
        new IVertexGremlinQuery<TResult> Cast<TResult>();
    }

    public partial interface IEdgeGremlinQuery
    {
        new IEdgeGremlinQuery<TResult> Cast<TResult>();
    }

    public partial interface IValueGremlinQuery<TElement>
    {
        new IOrderedValueGremlinQuery<TElement> OrderBy(Expression<Func<TElement, object>> projection);
        new IOrderedValueGremlinQuery<TElement> OrderByDescending(Expression<Func<TElement, object>> projection);
    }
    public partial interface IArrayGremlinQuery<TArray, TQuery>
    {
        new IOrderedArrayGremlinQuery<TArray, TQuery> OrderBy(Expression<Func<TArray, object>> projection);
        new IOrderedArrayGremlinQuery<TArray, TQuery> OrderByDescending(Expression<Func<TArray, object>> projection);
    }
    public partial interface IVertexGremlinQuery<TVertex>
    {
        new IOrderedVertexGremlinQuery<TVertex> OrderBy(Expression<Func<TVertex, object>> projection);
        new IOrderedVertexGremlinQuery<TVertex> OrderByDescending(Expression<Func<TVertex, object>> projection);
    }
    public partial interface IEdgeGremlinQuery<TEdge>
    {
        new IOrderedEdgeGremlinQuery<TEdge> OrderBy(Expression<Func<TEdge, object>> projection);
        new IOrderedEdgeGremlinQuery<TEdge> OrderByDescending(Expression<Func<TEdge, object>> projection);
    }
    public partial interface IEdgeGremlinQuery<TEdge, TAdjacentVertex>
    {
        new IOrderedEdgeGremlinQuery<TEdge, TAdjacentVertex> OrderBy(Expression<Func<TEdge, object>> projection);
        new IOrderedEdgeGremlinQuery<TEdge, TAdjacentVertex> OrderByDescending(Expression<Func<TEdge, object>> projection);
    }
    public partial interface IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>
    {
        new IOrderedEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> OrderBy(Expression<Func<TEdge, object>> projection);
        new IOrderedEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> OrderByDescending(Expression<Func<TEdge, object>> projection);
    }
    public partial interface IInEdgeGremlinQuery<TEdge, TInVertex>
    {
        new IOrderedInEdgeGremlinQuery<TEdge, TInVertex> OrderBy(Expression<Func<TEdge, object>> projection);
        new IOrderedInEdgeGremlinQuery<TEdge, TInVertex> OrderByDescending(Expression<Func<TEdge, object>> projection);
    }
    public partial interface IOutEdgeGremlinQuery<TEdge, TOutVertex>
    {
        new IOrderedOutEdgeGremlinQuery<TEdge, TOutVertex> OrderBy(Expression<Func<TEdge, object>> projection);
        new IOrderedOutEdgeGremlinQuery<TEdge, TOutVertex> OrderByDescending(Expression<Func<TEdge, object>> projection);
    }
    public partial interface IVertexPropertyGremlinQuery<TProperty, TValue>
    {
        new IOrderedVertexPropertyGremlinQuery<TProperty, TValue> OrderBy(Expression<Func<TProperty, object>> projection);
        new IOrderedVertexPropertyGremlinQuery<TProperty, TValue> OrderByDescending(Expression<Func<TProperty, object>> projection);
    }
    public partial interface IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>
    {
        new IOrderedVertexPropertyGremlinQuery<TProperty, TValue, TMeta> OrderBy(Expression<Func<TProperty, object>> projection);
        new IOrderedVertexPropertyGremlinQuery<TProperty, TValue, TMeta> OrderByDescending(Expression<Func<TProperty, object>> projection);
    }
    public partial interface IEdgePropertyGremlinQuery<TElement, TValue>
    {
        new IOrderedEdgePropertyGremlinQuery<TElement, TValue> OrderBy(Expression<Func<TElement, object>> projection);
        new IOrderedEdgePropertyGremlinQuery<TElement, TValue> OrderByDescending(Expression<Func<TElement, object>> projection);
    }
    public partial interface IVertexGremlinQuery
    {
        new IOrderedVertexGremlinQuery OrderBy(Func<IVertexGremlinQuery, IGremlinQuery> traversal);
        new IOrderedVertexGremlinQuery OrderBy(string lambda);
        new IOrderedVertexGremlinQuery OrderByDescending(Func<IVertexGremlinQuery, IGremlinQuery> traversal);
    }
    public partial interface IEdgeGremlinQuery
    {
        new IOrderedEdgeGremlinQuery OrderBy(Func<IEdgeGremlinQuery, IGremlinQuery> traversal);
        new IOrderedEdgeGremlinQuery OrderBy(string lambda);
        new IOrderedEdgeGremlinQuery OrderByDescending(Func<IEdgeGremlinQuery, IGremlinQuery> traversal);
    }
    public partial interface IValueGremlinQuery<TElement>
    {
        new IOrderedValueGremlinQuery<TElement> OrderBy(Func<IValueGremlinQuery<TElement>, IGremlinQuery> traversal);
        new IOrderedValueGremlinQuery<TElement> OrderBy(string lambda);
        new IOrderedValueGremlinQuery<TElement> OrderByDescending(Func<IValueGremlinQuery<TElement>, IGremlinQuery> traversal);
    }
    public partial interface IArrayGremlinQuery<TArray, TQuery>
    {
        new IOrderedArrayGremlinQuery<TArray, TQuery> OrderBy(Func<IArrayGremlinQuery<TArray, TQuery>, IGremlinQuery> traversal);
        new IOrderedArrayGremlinQuery<TArray, TQuery> OrderBy(string lambda);
        new IOrderedArrayGremlinQuery<TArray, TQuery> OrderByDescending(Func<IArrayGremlinQuery<TArray, TQuery>, IGremlinQuery> traversal);
    }
    public partial interface IVertexGremlinQuery<TVertex>
    {
        new IOrderedVertexGremlinQuery<TVertex> OrderBy(Func<IVertexGremlinQuery<TVertex>, IGremlinQuery> traversal);
        new IOrderedVertexGremlinQuery<TVertex> OrderBy(string lambda);
        new IOrderedVertexGremlinQuery<TVertex> OrderByDescending(Func<IVertexGremlinQuery<TVertex>, IGremlinQuery> traversal);
    }
    public partial interface IEdgeGremlinQuery<TEdge>
    {
        new IOrderedEdgeGremlinQuery<TEdge> OrderBy(Func<IEdgeGremlinQuery<TEdge>, IGremlinQuery> traversal);
        new IOrderedEdgeGremlinQuery<TEdge> OrderBy(string lambda);
        new IOrderedEdgeGremlinQuery<TEdge> OrderByDescending(Func<IEdgeGremlinQuery<TEdge>, IGremlinQuery> traversal);
    }
    public partial interface IEdgeGremlinQuery<TEdge, TAdjacentVertex>
    {
        new IOrderedEdgeGremlinQuery<TEdge, TAdjacentVertex> OrderBy(Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> traversal);
        new IOrderedEdgeGremlinQuery<TEdge, TAdjacentVertex> OrderBy(string lambda);
        new IOrderedEdgeGremlinQuery<TEdge, TAdjacentVertex> OrderByDescending(Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> traversal);
    }
    public partial interface IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>
    {
        new IOrderedEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> OrderBy(Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, IGremlinQuery> traversal);
        new IOrderedEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> OrderBy(string lambda);
        new IOrderedEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> OrderByDescending(Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, IGremlinQuery> traversal);
    }
    public partial interface IInEdgeGremlinQuery<TEdge, TInVertex>
    {
        new IOrderedInEdgeGremlinQuery<TEdge, TInVertex> OrderBy(Func<IInEdgeGremlinQuery<TEdge, TInVertex>, IGremlinQuery> traversal);
        new IOrderedInEdgeGremlinQuery<TEdge, TInVertex> OrderBy(string lambda);
        new IOrderedInEdgeGremlinQuery<TEdge, TInVertex> OrderByDescending(Func<IInEdgeGremlinQuery<TEdge, TInVertex>, IGremlinQuery> traversal);
    }
    public partial interface IOutEdgeGremlinQuery<TEdge, TOutVertex>
    {
        new IOrderedOutEdgeGremlinQuery<TEdge, TOutVertex> OrderBy(Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, IGremlinQuery> traversal);
        new IOrderedOutEdgeGremlinQuery<TEdge, TOutVertex> OrderBy(string lambda);
        new IOrderedOutEdgeGremlinQuery<TEdge, TOutVertex> OrderByDescending(Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, IGremlinQuery> traversal);
    }
    public partial interface IVertexPropertyGremlinQuery<TProperty, TValue>
    {
        new IOrderedVertexPropertyGremlinQuery<TProperty, TValue> OrderBy(Func<IVertexPropertyGremlinQuery<TProperty, TValue>, IGremlinQuery> traversal);
        new IOrderedVertexPropertyGremlinQuery<TProperty, TValue> OrderBy(string lambda);
        new IOrderedVertexPropertyGremlinQuery<TProperty, TValue> OrderByDescending(Func<IVertexPropertyGremlinQuery<TProperty, TValue>, IGremlinQuery> traversal);
    }
    public partial interface IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>
    {
        new IOrderedVertexPropertyGremlinQuery<TProperty, TValue, TMeta> OrderBy(Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, IGremlinQuery> traversal);
        new IOrderedVertexPropertyGremlinQuery<TProperty, TValue, TMeta> OrderBy(string lambda);
        new IOrderedVertexPropertyGremlinQuery<TProperty, TValue, TMeta> OrderByDescending(Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, IGremlinQuery> traversal);
    }
    public partial interface IEdgePropertyGremlinQuery<TElement, TValue>
    {
        new IOrderedEdgePropertyGremlinQuery<TElement, TValue> OrderBy(Func<IEdgePropertyGremlinQuery<TElement, TValue>, IGremlinQuery> traversal);
        new IOrderedEdgePropertyGremlinQuery<TElement, TValue> OrderBy(string lambda);
        new IOrderedEdgePropertyGremlinQuery<TElement, TValue> OrderByDescending(Func<IEdgePropertyGremlinQuery<TElement, TValue>, IGremlinQuery> traversal);
    }
    public partial interface IOrderedVertexGremlinQuery
    {
        IOrderedVertexGremlinQuery ThenBy(Func<IVertexGremlinQuery, IGremlinQuery> traversal);
        IOrderedVertexGremlinQuery ThenByDescending(Func<IVertexGremlinQuery, IGremlinQuery> traversal);
        new IOrderedVertexGremlinQuery ThenBy(string lambda);
    }
    public partial interface IOrderedEdgeGremlinQuery
    {
        IOrderedEdgeGremlinQuery ThenBy(Func<IEdgeGremlinQuery, IGremlinQuery> traversal);
        IOrderedEdgeGremlinQuery ThenByDescending(Func<IEdgeGremlinQuery, IGremlinQuery> traversal);
        new IOrderedEdgeGremlinQuery ThenBy(string lambda);
    }
    public partial interface IOrderedValueGremlinQuery<TElement>
    {
        IOrderedValueGremlinQuery<TElement> ThenBy(Func<IValueGremlinQuery<TElement>, IGremlinQuery> traversal);
        IOrderedValueGremlinQuery<TElement> ThenByDescending(Func<IValueGremlinQuery<TElement>, IGremlinQuery> traversal);
        new IOrderedValueGremlinQuery<TElement> ThenBy(string lambda);
    }
    public partial interface IOrderedArrayGremlinQuery<TArray, TQuery>
    {
        IOrderedArrayGremlinQuery<TArray, TQuery> ThenBy(Func<IArrayGremlinQuery<TArray, TQuery>, IGremlinQuery> traversal);
        IOrderedArrayGremlinQuery<TArray, TQuery> ThenByDescending(Func<IArrayGremlinQuery<TArray, TQuery>, IGremlinQuery> traversal);
        new IOrderedArrayGremlinQuery<TArray, TQuery> ThenBy(string lambda);
    }
    public partial interface IOrderedVertexGremlinQuery<TVertex>
    {
        IOrderedVertexGremlinQuery<TVertex> ThenBy(Func<IVertexGremlinQuery<TVertex>, IGremlinQuery> traversal);
        IOrderedVertexGremlinQuery<TVertex> ThenByDescending(Func<IVertexGremlinQuery<TVertex>, IGremlinQuery> traversal);
        new IOrderedVertexGremlinQuery<TVertex> ThenBy(string lambda);
    }
    public partial interface IOrderedEdgeGremlinQuery<TEdge>
    {
        IOrderedEdgeGremlinQuery<TEdge> ThenBy(Func<IEdgeGremlinQuery<TEdge>, IGremlinQuery> traversal);
        IOrderedEdgeGremlinQuery<TEdge> ThenByDescending(Func<IEdgeGremlinQuery<TEdge>, IGremlinQuery> traversal);
        new IOrderedEdgeGremlinQuery<TEdge> ThenBy(string lambda);
    }
    public partial interface IOrderedEdgeGremlinQuery<TEdge, TAdjacentVertex>
    {
        IOrderedEdgeGremlinQuery<TEdge, TAdjacentVertex> ThenBy(Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> traversal);
        IOrderedEdgeGremlinQuery<TEdge, TAdjacentVertex> ThenByDescending(Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> traversal);
        new IOrderedEdgeGremlinQuery<TEdge, TAdjacentVertex> ThenBy(string lambda);
    }
    public partial interface IOrderedEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>
    {
        IOrderedEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> ThenBy(Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, IGremlinQuery> traversal);
        IOrderedEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> ThenByDescending(Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, IGremlinQuery> traversal);
        new IOrderedEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> ThenBy(string lambda);
    }
    public partial interface IOrderedInEdgeGremlinQuery<TEdge, TInVertex>
    {
        IOrderedInEdgeGremlinQuery<TEdge, TInVertex> ThenBy(Func<IInEdgeGremlinQuery<TEdge, TInVertex>, IGremlinQuery> traversal);
        IOrderedInEdgeGremlinQuery<TEdge, TInVertex> ThenByDescending(Func<IInEdgeGremlinQuery<TEdge, TInVertex>, IGremlinQuery> traversal);
        new IOrderedInEdgeGremlinQuery<TEdge, TInVertex> ThenBy(string lambda);
    }
    public partial interface IOrderedOutEdgeGremlinQuery<TEdge, TOutVertex>
    {
        IOrderedOutEdgeGremlinQuery<TEdge, TOutVertex> ThenBy(Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, IGremlinQuery> traversal);
        IOrderedOutEdgeGremlinQuery<TEdge, TOutVertex> ThenByDescending(Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, IGremlinQuery> traversal);
        new IOrderedOutEdgeGremlinQuery<TEdge, TOutVertex> ThenBy(string lambda);
    }
    public partial interface IOrderedVertexPropertyGremlinQuery<TProperty, TValue>
    {
        IOrderedVertexPropertyGremlinQuery<TProperty, TValue> ThenBy(Func<IVertexPropertyGremlinQuery<TProperty, TValue>, IGremlinQuery> traversal);
        IOrderedVertexPropertyGremlinQuery<TProperty, TValue> ThenByDescending(Func<IVertexPropertyGremlinQuery<TProperty, TValue>, IGremlinQuery> traversal);
        new IOrderedVertexPropertyGremlinQuery<TProperty, TValue> ThenBy(string lambda);
    }
    public partial interface IOrderedVertexPropertyGremlinQuery<TProperty, TValue, TMeta>
    {
        IOrderedVertexPropertyGremlinQuery<TProperty, TValue, TMeta> ThenBy(Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, IGremlinQuery> traversal);
        IOrderedVertexPropertyGremlinQuery<TProperty, TValue, TMeta> ThenByDescending(Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, IGremlinQuery> traversal);
        new IOrderedVertexPropertyGremlinQuery<TProperty, TValue, TMeta> ThenBy(string lambda);
    }
    public partial interface IOrderedEdgePropertyGremlinQuery<TElement, TValue>
    {
        IOrderedEdgePropertyGremlinQuery<TElement, TValue> ThenBy(Func<IEdgePropertyGremlinQuery<TElement, TValue>, IGremlinQuery> traversal);
        IOrderedEdgePropertyGremlinQuery<TElement, TValue> ThenByDescending(Func<IEdgePropertyGremlinQuery<TElement, TValue>, IGremlinQuery> traversal);
        new IOrderedEdgePropertyGremlinQuery<TElement, TValue> ThenBy(string lambda);
    }
    public partial interface IOrderedValueGremlinQuery<TElement>
    {
        new IOrderedValueGremlinQuery<TElement> ThenBy(Expression<Func<TElement, object>> projection);
        new IOrderedValueGremlinQuery<TElement> ThenByDescending(Expression<Func<TElement, object>> projection);
    }
    public partial interface IOrderedArrayGremlinQuery<TArray, TQuery>
    {
        new IOrderedArrayGremlinQuery<TArray, TQuery> ThenBy(Expression<Func<TArray, object>> projection);
        new IOrderedArrayGremlinQuery<TArray, TQuery> ThenByDescending(Expression<Func<TArray, object>> projection);
    }
    public partial interface IOrderedVertexGremlinQuery<TVertex>
    {
        new IOrderedVertexGremlinQuery<TVertex> ThenBy(Expression<Func<TVertex, object>> projection);
        new IOrderedVertexGremlinQuery<TVertex> ThenByDescending(Expression<Func<TVertex, object>> projection);
    }
    public partial interface IOrderedEdgeGremlinQuery<TEdge>
    {
        new IOrderedEdgeGremlinQuery<TEdge> ThenBy(Expression<Func<TEdge, object>> projection);
        new IOrderedEdgeGremlinQuery<TEdge> ThenByDescending(Expression<Func<TEdge, object>> projection);
    }
    public partial interface IOrderedEdgeGremlinQuery<TEdge, TAdjacentVertex>
    {
        new IOrderedEdgeGremlinQuery<TEdge, TAdjacentVertex> ThenBy(Expression<Func<TEdge, object>> projection);
        new IOrderedEdgeGremlinQuery<TEdge, TAdjacentVertex> ThenByDescending(Expression<Func<TEdge, object>> projection);
    }
    public partial interface IOrderedEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>
    {
        new IOrderedEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> ThenBy(Expression<Func<TEdge, object>> projection);
        new IOrderedEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> ThenByDescending(Expression<Func<TEdge, object>> projection);
    }
    public partial interface IOrderedInEdgeGremlinQuery<TEdge, TInVertex>
    {
        new IOrderedInEdgeGremlinQuery<TEdge, TInVertex> ThenBy(Expression<Func<TEdge, object>> projection);
        new IOrderedInEdgeGremlinQuery<TEdge, TInVertex> ThenByDescending(Expression<Func<TEdge, object>> projection);
    }
    public partial interface IOrderedOutEdgeGremlinQuery<TEdge, TOutVertex>
    {
        new IOrderedOutEdgeGremlinQuery<TEdge, TOutVertex> ThenBy(Expression<Func<TEdge, object>> projection);
        new IOrderedOutEdgeGremlinQuery<TEdge, TOutVertex> ThenByDescending(Expression<Func<TEdge, object>> projection);
    }
    public partial interface IOrderedVertexPropertyGremlinQuery<TProperty, TValue>
    {
        new IOrderedVertexPropertyGremlinQuery<TProperty, TValue> ThenBy(Expression<Func<TProperty, object>> projection);
        new IOrderedVertexPropertyGremlinQuery<TProperty, TValue> ThenByDescending(Expression<Func<TProperty, object>> projection);
    }
    public partial interface IOrderedVertexPropertyGremlinQuery<TProperty, TValue, TMeta>
    {
        new IOrderedVertexPropertyGremlinQuery<TProperty, TValue, TMeta> ThenBy(Expression<Func<TProperty, object>> projection);
        new IOrderedVertexPropertyGremlinQuery<TProperty, TValue, TMeta> ThenByDescending(Expression<Func<TProperty, object>> projection);
    }
    public partial interface IOrderedEdgePropertyGremlinQuery<TElement, TValue>
    {
        new IOrderedEdgePropertyGremlinQuery<TElement, TValue> ThenBy(Expression<Func<TElement, object>> projection);
        new IOrderedEdgePropertyGremlinQuery<TElement, TValue> ThenByDescending(Expression<Func<TElement, object>> projection);
    }
    public partial interface IVertexGremlinQuery<TVertex>
    {
        new IValueGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TVertex, TTarget>>[] projections);
        new IValueGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TVertex, TTarget[]>>[] projections);
    }

    public partial interface IEdgeGremlinQuery<TEdge>
    {
        new IValueGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TEdge, TTarget>>[] projections);
        new IValueGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TEdge, TTarget[]>>[] projections);
    }

    public partial interface IVertexGremlinQuery
    {
        IVertexGremlinQuery<TTarget> OfType<TTarget>();
    }

    public partial interface IEdgeGremlinQuery
    {
        IEdgeGremlinQuery<TTarget> OfType<TTarget>();
    }

    public partial interface IVertexGremlinQuery<TVertex>
    {
        new IVertexGremlinQuery<TTarget> OfType<TTarget>();

        new IVertexGremlinQuery<TVertex> Property<TProjectedValue>(Expression<Func<TVertex, TProjectedValue>> projection, TProjectedValue value);
        new IVertexGremlinQuery<TVertex> Property<TProjectedValue>(Expression<Func<TVertex, TProjectedValue[]>> projection, TProjectedValue value);

        new IVertexGremlinQuery<TVertex> Where(Expression<Func<TVertex, bool>> predicate);
        new IVertexGremlinQuery<TVertex> Where<TProjection>(Expression<Func<TVertex, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal);
    }

    public partial interface IEdgeGremlinQuery<TEdge>
    {
        new IEdgeGremlinQuery<TTarget> OfType<TTarget>();

        new IEdgeGremlinQuery<TEdge> Property<TProjectedValue>(Expression<Func<TEdge, TProjectedValue>> projection, TProjectedValue value);
        new IEdgeGremlinQuery<TEdge> Property<TProjectedValue>(Expression<Func<TEdge, TProjectedValue[]>> projection, TProjectedValue value);

        new IEdgeGremlinQuery<TEdge> Where(Expression<Func<TEdge, bool>> predicate);
        new IEdgeGremlinQuery<TEdge> Where<TProjection>(Expression<Func<TEdge, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal);
    }

    public partial interface IEdgeGremlinQuery<TEdge, TAdjacentVertex>
    {
        new IEdgeGremlinQuery<TTarget, TAdjacentVertex> OfType<TTarget>();

        new IEdgeGremlinQuery<TEdge, TAdjacentVertex> Property<TProjectedValue>(Expression<Func<TEdge, TProjectedValue>> projection, TProjectedValue value);
        new IEdgeGremlinQuery<TEdge, TAdjacentVertex> Property<TProjectedValue>(Expression<Func<TEdge, TProjectedValue[]>> projection, TProjectedValue value);

        new IEdgeGremlinQuery<TEdge, TAdjacentVertex> Where(Expression<Func<TEdge, bool>> predicate);
        new IEdgeGremlinQuery<TEdge, TAdjacentVertex> Where<TProjection>(Expression<Func<TEdge, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal);
    }

    public partial interface IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>
    {
        new IEdgeGremlinQuery<TTarget, TOutVertex, TInVertex> OfType<TTarget>();

        new IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> Property<TProjectedValue>(Expression<Func<TEdge, TProjectedValue>> projection, TProjectedValue value);
        new IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> Property<TProjectedValue>(Expression<Func<TEdge, TProjectedValue[]>> projection, TProjectedValue value);

        new IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> Where(Expression<Func<TEdge, bool>> predicate);
        new IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> Where<TProjection>(Expression<Func<TEdge, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal);
    }

    public partial interface IInEdgeGremlinQuery<TEdge, TInVertex>
    {
        new IInEdgeGremlinQuery<TTarget, TInVertex> OfType<TTarget>();

        new IInEdgeGremlinQuery<TEdge, TInVertex> Property<TProjectedValue>(Expression<Func<TEdge, TProjectedValue>> projection, TProjectedValue value);
        new IInEdgeGremlinQuery<TEdge, TInVertex> Property<TProjectedValue>(Expression<Func<TEdge, TProjectedValue[]>> projection, TProjectedValue value);

        new IInEdgeGremlinQuery<TEdge, TInVertex> Where(Expression<Func<TEdge, bool>> predicate);
        new IInEdgeGremlinQuery<TEdge, TInVertex> Where<TProjection>(Expression<Func<TEdge, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal);
    }

    public partial interface IOutEdgeGremlinQuery<TEdge, TOutVertex>
    {
        new IOutEdgeGremlinQuery<TTarget, TOutVertex> OfType<TTarget>();

        new IOutEdgeGremlinQuery<TEdge, TOutVertex> Property<TProjectedValue>(Expression<Func<TEdge, TProjectedValue>> projection, TProjectedValue value);
        new IOutEdgeGremlinQuery<TEdge, TOutVertex> Property<TProjectedValue>(Expression<Func<TEdge, TProjectedValue[]>> projection, TProjectedValue value);

        new IOutEdgeGremlinQuery<TEdge, TOutVertex> Where(Expression<Func<TEdge, bool>> predicate);
        new IOutEdgeGremlinQuery<TEdge, TOutVertex> Where<TProjection>(Expression<Func<TEdge, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal);
    }

    public partial interface IGremlinQuery<TElement>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IGremlinQuery<TElement>, StepLabel<IGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IGremlinQuery<TResult> Cast<TResult>();

        new IArrayGremlinQuery<TElement[], IGremlinQuery<TElement>> Fold();

        new IGremlinQuery<TElement> Where(Func<IGremlinQuery<TElement>, IGremlinQuery> filterTraversal);
    }

    public partial interface IValueGremlinQuery<TElement>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IValueGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IValueGremlinQuery<TElement>, StepLabel<IValueGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IValueGremlinQuery<TResult> Cast<TResult>();

        new IArrayGremlinQuery<TElement[], IValueGremlinQuery<TElement>> Fold();

        new IValueGremlinQuery<TElement> Where(Func<IValueGremlinQuery<TElement>, IGremlinQuery> filterTraversal);
    }

    public partial interface IArrayGremlinQuery<TArray, TQuery>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IArrayGremlinQuery<TArray, TQuery>, StepLabel<TArray[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IArrayGremlinQuery<TArray, TQuery>, StepLabel<IArrayGremlinQuery<TArray, TQuery>, TArray>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IArrayGremlinQuery<TResult, TQuery> Cast<TResult>();

        new IArrayGremlinQuery<TArray[], IArrayGremlinQuery<TArray, TQuery>> Fold();

        new IArrayGremlinQuery<TArray, TQuery> Where(Func<IArrayGremlinQuery<TArray, TQuery>, IGremlinQuery> filterTraversal);
    }

    public partial interface IElementGremlinQuery<TElement>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IElementGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IElementGremlinQuery<TElement>, StepLabel<IElementGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IElementGremlinQuery<TResult> Cast<TResult>();

        new IArrayGremlinQuery<TElement[], IElementGremlinQuery<TElement>> Fold();

        new IElementGremlinQuery<TElement> Where(Func<IElementGremlinQuery<TElement>, IGremlinQuery> filterTraversal);
    }

    public partial interface IVertexGremlinQuery<TVertex>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IVertexGremlinQuery<TVertex>, StepLabel<TVertex[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IVertexGremlinQuery<TVertex>, StepLabel<IVertexGremlinQuery<TVertex>, TVertex>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IVertexGremlinQuery<TResult> Cast<TResult>();

        new IArrayGremlinQuery<TVertex[], IVertexGremlinQuery<TVertex>> Fold();

        new IVertexGremlinQuery<TVertex> Where(Func<IVertexGremlinQuery<TVertex>, IGremlinQuery> filterTraversal);
    }

    public partial interface IEdgeGremlinQuery<TEdge>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge>, StepLabel<TEdge[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge>, StepLabel<IEdgeGremlinQuery<TEdge>, TEdge>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IEdgeGremlinQuery<TResult> Cast<TResult>();

        new IArrayGremlinQuery<TEdge[], IEdgeGremlinQuery<TEdge>> Fold();

        new IEdgeGremlinQuery<TEdge> Where(Func<IEdgeGremlinQuery<TEdge>, IGremlinQuery> filterTraversal);
    }

    public partial interface IEdgeGremlinQuery<TEdge, TAdjacentVertex>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, StepLabel<TEdge[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, StepLabel<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, TEdge>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IEdgeGremlinQuery<TResult, TAdjacentVertex> Cast<TResult>();

        new IArrayGremlinQuery<TEdge[], IEdgeGremlinQuery<TEdge, TAdjacentVertex>> Fold();

        new IEdgeGremlinQuery<TEdge, TAdjacentVertex> Where(Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> filterTraversal);
    }

    public partial interface IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, StepLabel<TEdge[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, StepLabel<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, TEdge>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IEdgeGremlinQuery<TResult, TOutVertex, TInVertex> Cast<TResult>();

        new IArrayGremlinQuery<TEdge[], IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>> Fold();

        new IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> Where(Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, IGremlinQuery> filterTraversal);
    }

    public partial interface IInEdgeGremlinQuery<TEdge, TInVertex>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IInEdgeGremlinQuery<TEdge, TInVertex>, StepLabel<TEdge[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IInEdgeGremlinQuery<TEdge, TInVertex>, StepLabel<IInEdgeGremlinQuery<TEdge, TInVertex>, TEdge>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IInEdgeGremlinQuery<TResult, TInVertex> Cast<TResult>();

        new IArrayGremlinQuery<TEdge[], IInEdgeGremlinQuery<TEdge, TInVertex>> Fold();

        new IInEdgeGremlinQuery<TEdge, TInVertex> Where(Func<IInEdgeGremlinQuery<TEdge, TInVertex>, IGremlinQuery> filterTraversal);
    }

    public partial interface IOutEdgeGremlinQuery<TEdge, TOutVertex>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, StepLabel<TEdge[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, StepLabel<IOutEdgeGremlinQuery<TEdge, TOutVertex>, TEdge>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IOutEdgeGremlinQuery<TResult, TOutVertex> Cast<TResult>();

        new IArrayGremlinQuery<TEdge[], IOutEdgeGremlinQuery<TEdge, TOutVertex>> Fold();

        new IOutEdgeGremlinQuery<TEdge, TOutVertex> Where(Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, IGremlinQuery> filterTraversal);
    }

    public partial interface IVertexPropertyGremlinQuery<TProperty, TValue>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TProperty, TValue>, StepLabel<TProperty[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TProperty, TValue>, StepLabel<IVertexPropertyGremlinQuery<TProperty, TValue>, TProperty>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IVertexPropertyGremlinQuery<TResult, TValue> Cast<TResult>();

        new IArrayGremlinQuery<TProperty[], IVertexPropertyGremlinQuery<TProperty, TValue>> Fold();

        new IVertexPropertyGremlinQuery<TProperty, TValue> Where(Func<IVertexPropertyGremlinQuery<TProperty, TValue>, IGremlinQuery> filterTraversal);
    }

    public partial interface IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, StepLabel<TProperty[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, StepLabel<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, TProperty>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IVertexPropertyGremlinQuery<TResult, TValue, TMeta> Cast<TResult>();

        new IArrayGremlinQuery<TProperty[], IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>> Fold();

        new IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Where(Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, IGremlinQuery> filterTraversal);
    }

    public partial interface IEdgePropertyGremlinQuery<TElement, TValue>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IEdgePropertyGremlinQuery<TElement, TValue>, StepLabel<TElement[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IEdgePropertyGremlinQuery<TElement, TValue>, StepLabel<IEdgePropertyGremlinQuery<TElement, TValue>, TElement>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IEdgePropertyGremlinQuery<TResult, TValue> Cast<TResult>();

        new IArrayGremlinQuery<TElement[], IEdgePropertyGremlinQuery<TElement, TValue>> Fold();

        new IEdgePropertyGremlinQuery<TElement, TValue> Where(Func<IEdgePropertyGremlinQuery<TElement, TValue>, IGremlinQuery> filterTraversal);
    }

    public partial interface IOrderedValueGremlinQuery<TElement>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IOrderedValueGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IOrderedValueGremlinQuery<TElement>, StepLabel<IOrderedValueGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IOrderedValueGremlinQuery<TResult> Cast<TResult>();

        new IArrayGremlinQuery<TElement[], IValueGremlinQuery<TElement>> Fold();

        new IValueGremlinQuery<TElement> Where(Func<IOrderedValueGremlinQuery<TElement>, IGremlinQuery> filterTraversal);
    }

    public partial interface IOrderedArrayGremlinQuery<TArray, TQuery>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IOrderedArrayGremlinQuery<TArray, TQuery>, StepLabel<TArray[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IOrderedArrayGremlinQuery<TArray, TQuery>, StepLabel<IOrderedArrayGremlinQuery<TArray, TQuery>, TArray>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IOrderedArrayGremlinQuery<TResult, TQuery> Cast<TResult>();

        new IArrayGremlinQuery<TArray[], IArrayGremlinQuery<TArray, TQuery>> Fold();

        new IArrayGremlinQuery<TArray, TQuery> Where(Func<IOrderedArrayGremlinQuery<TArray, TQuery>, IGremlinQuery> filterTraversal);
    }

    public partial interface IOrderedVertexGremlinQuery<TVertex>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IOrderedVertexGremlinQuery<TVertex>, StepLabel<TVertex[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IOrderedVertexGremlinQuery<TVertex>, StepLabel<IOrderedVertexGremlinQuery<TVertex>, TVertex>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IOrderedVertexGremlinQuery<TResult> Cast<TResult>();

        new IArrayGremlinQuery<TVertex[], IVertexGremlinQuery<TVertex>> Fold();

        new IVertexGremlinQuery<TVertex> Where(Func<IOrderedVertexGremlinQuery<TVertex>, IGremlinQuery> filterTraversal);
    }

    public partial interface IOrderedEdgeGremlinQuery<TEdge>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IOrderedEdgeGremlinQuery<TEdge>, StepLabel<TEdge[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IOrderedEdgeGremlinQuery<TEdge>, StepLabel<IOrderedEdgeGremlinQuery<TEdge>, TEdge>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IOrderedEdgeGremlinQuery<TResult> Cast<TResult>();

        new IArrayGremlinQuery<TEdge[], IEdgeGremlinQuery<TEdge>> Fold();

        new IEdgeGremlinQuery<TEdge> Where(Func<IOrderedEdgeGremlinQuery<TEdge>, IGremlinQuery> filterTraversal);
    }

    public partial interface IOrderedEdgeGremlinQuery<TEdge, TAdjacentVertex>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IOrderedEdgeGremlinQuery<TEdge, TAdjacentVertex>, StepLabel<TEdge[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IOrderedEdgeGremlinQuery<TEdge, TAdjacentVertex>, StepLabel<IOrderedEdgeGremlinQuery<TEdge, TAdjacentVertex>, TEdge>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IOrderedEdgeGremlinQuery<TResult, TAdjacentVertex> Cast<TResult>();

        new IArrayGremlinQuery<TEdge[], IEdgeGremlinQuery<TEdge, TAdjacentVertex>> Fold();

        new IEdgeGremlinQuery<TEdge, TAdjacentVertex> Where(Func<IOrderedEdgeGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> filterTraversal);
    }

    public partial interface IOrderedEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IOrderedEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, StepLabel<TEdge[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IOrderedEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, StepLabel<IOrderedEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, TEdge>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IOrderedEdgeGremlinQuery<TResult, TOutVertex, TInVertex> Cast<TResult>();

        new IArrayGremlinQuery<TEdge[], IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>> Fold();

        new IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> Where(Func<IOrderedEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, IGremlinQuery> filterTraversal);
    }

    public partial interface IOrderedInEdgeGremlinQuery<TEdge, TInVertex>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IOrderedInEdgeGremlinQuery<TEdge, TInVertex>, StepLabel<TEdge[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IOrderedInEdgeGremlinQuery<TEdge, TInVertex>, StepLabel<IOrderedInEdgeGremlinQuery<TEdge, TInVertex>, TEdge>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IOrderedInEdgeGremlinQuery<TResult, TInVertex> Cast<TResult>();

        new IArrayGremlinQuery<TEdge[], IInEdgeGremlinQuery<TEdge, TInVertex>> Fold();

        new IInEdgeGremlinQuery<TEdge, TInVertex> Where(Func<IOrderedInEdgeGremlinQuery<TEdge, TInVertex>, IGremlinQuery> filterTraversal);
    }

    public partial interface IOrderedOutEdgeGremlinQuery<TEdge, TOutVertex>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IOrderedOutEdgeGremlinQuery<TEdge, TOutVertex>, StepLabel<TEdge[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IOrderedOutEdgeGremlinQuery<TEdge, TOutVertex>, StepLabel<IOrderedOutEdgeGremlinQuery<TEdge, TOutVertex>, TEdge>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IOrderedOutEdgeGremlinQuery<TResult, TOutVertex> Cast<TResult>();

        new IArrayGremlinQuery<TEdge[], IOutEdgeGremlinQuery<TEdge, TOutVertex>> Fold();

        new IOutEdgeGremlinQuery<TEdge, TOutVertex> Where(Func<IOrderedOutEdgeGremlinQuery<TEdge, TOutVertex>, IGremlinQuery> filterTraversal);
    }

    public partial interface IOrderedVertexPropertyGremlinQuery<TProperty, TValue>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IOrderedVertexPropertyGremlinQuery<TProperty, TValue>, StepLabel<TProperty[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IOrderedVertexPropertyGremlinQuery<TProperty, TValue>, StepLabel<IOrderedVertexPropertyGremlinQuery<TProperty, TValue>, TProperty>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IOrderedVertexPropertyGremlinQuery<TResult, TValue> Cast<TResult>();

        new IArrayGremlinQuery<TProperty[], IVertexPropertyGremlinQuery<TProperty, TValue>> Fold();

        new IVertexPropertyGremlinQuery<TProperty, TValue> Where(Func<IOrderedVertexPropertyGremlinQuery<TProperty, TValue>, IGremlinQuery> filterTraversal);
    }

    public partial interface IOrderedVertexPropertyGremlinQuery<TProperty, TValue, TMeta>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IOrderedVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, StepLabel<TProperty[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IOrderedVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, StepLabel<IOrderedVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, TProperty>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IOrderedVertexPropertyGremlinQuery<TResult, TValue, TMeta> Cast<TResult>();

        new IArrayGremlinQuery<TProperty[], IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>> Fold();

        new IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Where(Func<IOrderedVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, IGremlinQuery> filterTraversal);
    }

    public partial interface IOrderedEdgePropertyGremlinQuery<TElement, TValue>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IOrderedEdgePropertyGremlinQuery<TElement, TValue>, StepLabel<TElement[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IOrderedEdgePropertyGremlinQuery<TElement, TValue>, StepLabel<IOrderedEdgePropertyGremlinQuery<TElement, TValue>, TElement>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IOrderedEdgePropertyGremlinQuery<TResult, TValue> Cast<TResult>();

        new IArrayGremlinQuery<TElement[], IEdgePropertyGremlinQuery<TElement, TValue>> Fold();

        new IEdgePropertyGremlinQuery<TElement, TValue> Where(Func<IOrderedEdgePropertyGremlinQuery<TElement, TValue>, IGremlinQuery> filterTraversal);
    }

    public partial interface IGremlinQuery
    {
        new IGremlinQuery Coin(double probability);
    }
    public partial interface IElementGremlinQuery
    {
        new IElementGremlinQuery Coin(double probability);
    }
    public partial interface IVertexGremlinQuery
    {
        new IVertexGremlinQuery Coin(double probability);
    }
    public partial interface IEdgeGremlinQuery
    {
        new IEdgeGremlinQuery Coin(double probability);
    }
    public partial interface IGremlinQuery<TElement>
    {
        new IGremlinQuery<TElement> Coin(double probability);
    }
    public partial interface IValueGremlinQuery<TElement>
    {
        new IValueGremlinQuery<TElement> Coin(double probability);
    }
    public partial interface IArrayGremlinQuery<TArray, TQuery>
    {
        new IArrayGremlinQuery<TArray, TQuery> Coin(double probability);
    }
    public partial interface IElementGremlinQuery<TElement>
    {
        new IElementGremlinQuery<TElement> Coin(double probability);
    }
    public partial interface IVertexGremlinQuery<TVertex>
    {
        new IVertexGremlinQuery<TVertex> Coin(double probability);
    }
    public partial interface IEdgeGremlinQuery<TEdge>
    {
        new IEdgeGremlinQuery<TEdge> Coin(double probability);
    }
    public partial interface IEdgeGremlinQuery<TEdge, TAdjacentVertex>
    {
        new IEdgeGremlinQuery<TEdge, TAdjacentVertex> Coin(double probability);
    }
    public partial interface IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>
    {
        new IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> Coin(double probability);
    }
    public partial interface IInEdgeGremlinQuery<TEdge, TInVertex>
    {
        new IInEdgeGremlinQuery<TEdge, TInVertex> Coin(double probability);
    }
    public partial interface IOutEdgeGremlinQuery<TEdge, TOutVertex>
    {
        new IOutEdgeGremlinQuery<TEdge, TOutVertex> Coin(double probability);
    }
    public partial interface IVertexPropertyGremlinQuery<TProperty, TValue>
    {
        new IVertexPropertyGremlinQuery<TProperty, TValue> Coin(double probability);
    }
    public partial interface IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>
    {
        new IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Coin(double probability);
    }
    public partial interface IEdgePropertyGremlinQuery<TElement, TValue>
    {
        new IEdgePropertyGremlinQuery<TElement, TValue> Coin(double probability);
    }
    public partial interface IOrderedGremlinQuery
    {
        new IOrderedGremlinQuery Coin(double probability);
    }
    public partial interface IOrderedElementGremlinQuery
    {
        new IOrderedElementGremlinQuery Coin(double probability);
    }
    public partial interface IOrderedVertexGremlinQuery
    {
        new IOrderedVertexGremlinQuery Coin(double probability);
    }
    public partial interface IOrderedEdgeGremlinQuery
    {
        new IOrderedEdgeGremlinQuery Coin(double probability);
    }
    public partial interface IOrderedGremlinQuery<TElement>
    {
        new IOrderedGremlinQuery<TElement> Coin(double probability);
    }
    public partial interface IOrderedValueGremlinQuery<TElement>
    {
        new IOrderedValueGremlinQuery<TElement> Coin(double probability);
    }
    public partial interface IOrderedArrayGremlinQuery<TArray, TQuery>
    {
        new IOrderedArrayGremlinQuery<TArray, TQuery> Coin(double probability);
    }
    public partial interface IOrderedElementGremlinQuery<TElement>
    {
        new IOrderedElementGremlinQuery<TElement> Coin(double probability);
    }
    public partial interface IOrderedVertexGremlinQuery<TVertex>
    {
        new IOrderedVertexGremlinQuery<TVertex> Coin(double probability);
    }
    public partial interface IOrderedEdgeGremlinQuery<TEdge>
    {
        new IOrderedEdgeGremlinQuery<TEdge> Coin(double probability);
    }
    public partial interface IOrderedEdgeGremlinQuery<TEdge, TAdjacentVertex>
    {
        new IOrderedEdgeGremlinQuery<TEdge, TAdjacentVertex> Coin(double probability);
    }
    public partial interface IOrderedEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>
    {
        new IOrderedEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> Coin(double probability);
    }
    public partial interface IOrderedInEdgeGremlinQuery<TEdge, TInVertex>
    {
        new IOrderedInEdgeGremlinQuery<TEdge, TInVertex> Coin(double probability);
    }
    public partial interface IOrderedOutEdgeGremlinQuery<TEdge, TOutVertex>
    {
        new IOrderedOutEdgeGremlinQuery<TEdge, TOutVertex> Coin(double probability);
    }
    public partial interface IOrderedVertexPropertyGremlinQuery<TProperty, TValue>
    {
        new IOrderedVertexPropertyGremlinQuery<TProperty, TValue> Coin(double probability);
    }
    public partial interface IOrderedVertexPropertyGremlinQuery<TProperty, TValue, TMeta>
    {
        new IOrderedVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Coin(double probability);
    }
    public partial interface IOrderedEdgePropertyGremlinQuery<TElement, TValue>
    {
        new IOrderedEdgePropertyGremlinQuery<TElement, TValue> Coin(double probability);
    }
}
#pragma warning restore CS0109 // Member does not hide an inherited member; new keyword is not required
