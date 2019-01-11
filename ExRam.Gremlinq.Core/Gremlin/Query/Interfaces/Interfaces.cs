#pragma warning disable CS0109 // Member does not hide an inherited member; new keyword is not required
using System;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    public partial interface IOrderedGremlinQuery : IGremlinQuery
    {
    }

    public partial interface IOrderedElementGremlinQuery : IElementGremlinQuery
    {
    }

    public partial interface IOrderedVGremlinQuery : IVGremlinQuery
    {
    }

    public partial interface IOrderedEGremlinQuery : IEGremlinQuery
    {
    }

    public partial interface IOrderedGremlinQuery<TElement> : IGremlinQuery<TElement>
    {
    }

    public partial interface IOrderedElementGremlinQuery<TElement> : IElementGremlinQuery<TElement>
    {
    }

    public partial interface IOrderedVGremlinQuery<TVertex> : IVGremlinQuery<TVertex>
    {
    }

    public partial interface IOrderedEGremlinQuery<TEdge> : IEGremlinQuery<TEdge>
    {
    }

    public partial interface IOrderedEGremlinQuery<TEdge, TAdjacentVertex> : IEGremlinQuery<TEdge, TAdjacentVertex>
    {
    }

    public partial interface IOrderedEGremlinQuery<TEdge, TOutVertex, TInVertex> : IEGremlinQuery<TEdge, TOutVertex, TInVertex>
    {
    }

    public partial interface IOrderedInEGremlinQuery<TEdge, TAdjacentVertex> : IInEGremlinQuery<TEdge, TAdjacentVertex>
    {
    }

    public partial interface IOrderedOutEGremlinQuery<TEdge, TAdjacentVertex> : IOutEGremlinQuery<TEdge, TAdjacentVertex>
    {
    }

    public partial interface IOrderedVPropertiesGremlinQuery<TValue> : IVPropertiesGremlinQuery<TValue>
    {
    }

    public partial interface IOrderedVPropertiesGremlinQuery<TValue, TMeta> : IVPropertiesGremlinQuery<TValue, TMeta>
    {
    }

    public partial interface IOrderedEPropertiesGremlinQuery<TValue> : IEPropertiesGremlinQuery<TValue>
    {
    }

    public partial interface IGremlinQuery
    {
        new IGremlinQuery<TResult> Cast<TResult>();
    }

    public partial interface IElementGremlinQuery
    {
        new IElementGremlinQuery<TResult> Cast<TResult>();
    }

    public partial interface IVGremlinQuery
    {
        new IVGremlinQuery<TResult> Cast<TResult>();
    }

    public partial interface IEGremlinQuery
    {
        new IEGremlinQuery<TResult> Cast<TResult>();
    }

    public partial interface IGremlinQuery
    {
        IGremlinQuery And(params Func<IGremlinQuery, IGremlinQuery>[] andTraversals);

        new IGremlinQuery As(StepLabel stepLabel);

        new IGremlinQuery Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IGremlinQuery, IGremlinQuery> traversalPredicate, Func<IGremlinQuery, TTargetQuery> trueChoice, Func<IGremlinQuery, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IGremlinQuery, IGremlinQuery> traversalPredicate, Func<IGremlinQuery, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Coalesce<TTargetQuery>(params Func<IGremlinQuery, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IGremlinQuery Dedup();

        new IGremlinQuery Emit();

        new IGremlinQuery Filter(string lambda);

        new IGremlinQuery Identity();

        new IGremlinQuery Limit(long count);
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
        new IGremlinQuery Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IGremlinQuery, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;
    }

    public partial interface IElementGremlinQuery
    {
        IElementGremlinQuery And(params Func<IElementGremlinQuery, IGremlinQuery>[] andTraversals);

        new IElementGremlinQuery As(StepLabel stepLabel);

        new IElementGremlinQuery Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IElementGremlinQuery, IGremlinQuery> traversalPredicate, Func<IElementGremlinQuery, TTargetQuery> trueChoice, Func<IElementGremlinQuery, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IElementGremlinQuery, IGremlinQuery> traversalPredicate, Func<IElementGremlinQuery, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Coalesce<TTargetQuery>(params Func<IElementGremlinQuery, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IElementGremlinQuery Dedup();

        new IElementGremlinQuery Emit();

        new IElementGremlinQuery Filter(string lambda);

        new IElementGremlinQuery Identity();

        new IElementGremlinQuery Limit(long count);
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
        new IElementGremlinQuery Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IElementGremlinQuery, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;
    }

    public partial interface IVGremlinQuery
    {
        IVGremlinQuery And(params Func<IVGremlinQuery, IGremlinQuery>[] andTraversals);

        new IVGremlinQuery As(StepLabel stepLabel);

        new IVGremlinQuery Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IVGremlinQuery, IGremlinQuery> traversalPredicate, Func<IVGremlinQuery, TTargetQuery> trueChoice, Func<IVGremlinQuery, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IVGremlinQuery, IGremlinQuery> traversalPredicate, Func<IVGremlinQuery, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Coalesce<TTargetQuery>(params Func<IVGremlinQuery, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IVGremlinQuery Dedup();

        new IVGremlinQuery Emit();

        new IVGremlinQuery Filter(string lambda);

        new IVGremlinQuery Identity();

        new IVGremlinQuery Limit(long count);
        TTargetQuery Local<TTargetQuery>(Func<IVGremlinQuery , TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IVGremlinQuery, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;
        
        IVGremlinQuery Not(Func<IVGremlinQuery, IGremlinQuery> notTraversal);

        TTargetQuery Optional<TTargetQuery>(Func<IVGremlinQuery, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery;
        IVGremlinQuery Or(params Func<IVGremlinQuery, IGremlinQuery>[] orTraversals);

        new IVGremlinQuery Range(long low, long high);

        TTargetQuery Repeat<TTargetQuery>(Func<IVGremlinQuery, TTargetQuery> repeatTraversal) where TTargetQuery : IVGremlinQuery;
        TTargetQuery RepeatUntil<TTargetQuery>(Func<IVGremlinQuery, TTargetQuery> repeatTraversal, Func<IVGremlinQuery, IGremlinQuery> untilTraversal) where TTargetQuery : IVGremlinQuery;

        IVGremlinQuery SideEffect(Func<IVGremlinQuery, IGremlinQuery> sideEffectTraversal);
        new IVGremlinQuery Skip(long count);

        new IVGremlinQuery Tail(long count);
        new IVGremlinQuery Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IVGremlinQuery, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;
    }

    public partial interface IEGremlinQuery
    {
        IEGremlinQuery And(params Func<IEGremlinQuery, IGremlinQuery>[] andTraversals);

        new IEGremlinQuery As(StepLabel stepLabel);

        new IEGremlinQuery Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IEGremlinQuery, IGremlinQuery> traversalPredicate, Func<IEGremlinQuery, TTargetQuery> trueChoice, Func<IEGremlinQuery, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IEGremlinQuery, IGremlinQuery> traversalPredicate, Func<IEGremlinQuery, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Coalesce<TTargetQuery>(params Func<IEGremlinQuery, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IEGremlinQuery Dedup();

        new IEGremlinQuery Emit();

        new IEGremlinQuery Filter(string lambda);

        new IEGremlinQuery Identity();

        new IEGremlinQuery Limit(long count);
        TTargetQuery Local<TTargetQuery>(Func<IEGremlinQuery , TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IEGremlinQuery, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;
        
        IEGremlinQuery Not(Func<IEGremlinQuery, IGremlinQuery> notTraversal);

        TTargetQuery Optional<TTargetQuery>(Func<IEGremlinQuery, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery;
        IEGremlinQuery Or(params Func<IEGremlinQuery, IGremlinQuery>[] orTraversals);

        new IEGremlinQuery Range(long low, long high);

        TTargetQuery Repeat<TTargetQuery>(Func<IEGremlinQuery, TTargetQuery> repeatTraversal) where TTargetQuery : IEGremlinQuery;
        TTargetQuery RepeatUntil<TTargetQuery>(Func<IEGremlinQuery, TTargetQuery> repeatTraversal, Func<IEGremlinQuery, IGremlinQuery> untilTraversal) where TTargetQuery : IEGremlinQuery;

        IEGremlinQuery SideEffect(Func<IEGremlinQuery, IGremlinQuery> sideEffectTraversal);
        new IEGremlinQuery Skip(long count);

        new IEGremlinQuery Tail(long count);
        new IEGremlinQuery Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IEGremlinQuery, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;
    }

    public partial interface IGremlinQuery<TElement>
    {
        IGremlinQuery<TElement> And(params Func<IGremlinQuery<TElement>, IGremlinQuery>[] andTraversals);

        new IGremlinQuery<TElement> As(StepLabel stepLabel);

        new IGremlinQuery<TElement> Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IGremlinQuery<TElement>, TTargetQuery> trueChoice, Func<IGremlinQuery<TElement>, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IGremlinQuery<TElement>, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Coalesce<TTargetQuery>(params Func<IGremlinQuery<TElement>, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IGremlinQuery<TElement> Dedup();

        new IGremlinQuery<TElement> Emit();

        new IGremlinQuery<TElement> Filter(string lambda);

        new IGremlinQuery<TElement> Identity();

        new IGremlinQuery<TElement> Limit(long count);
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
        new IGremlinQuery<TElement> Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IGremlinQuery<TElement>, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;
    }

    public partial interface IElementGremlinQuery<TElement>
    {
        IElementGremlinQuery<TElement> And(params Func<IElementGremlinQuery<TElement>, IGremlinQuery>[] andTraversals);

        new IElementGremlinQuery<TElement> As(StepLabel stepLabel);

        new IElementGremlinQuery<TElement> Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IElementGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IElementGremlinQuery<TElement>, TTargetQuery> trueChoice, Func<IElementGremlinQuery<TElement>, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IElementGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IElementGremlinQuery<TElement>, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Coalesce<TTargetQuery>(params Func<IElementGremlinQuery<TElement>, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IElementGremlinQuery<TElement> Dedup();

        new IElementGremlinQuery<TElement> Emit();

        new IElementGremlinQuery<TElement> Filter(string lambda);

        new IElementGremlinQuery<TElement> Identity();

        new IElementGremlinQuery<TElement> Limit(long count);
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
        new IElementGremlinQuery<TElement> Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IElementGremlinQuery<TElement>, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;
    }

    public partial interface IVGremlinQuery<TVertex>
    {
        IVGremlinQuery<TVertex> And(params Func<IVGremlinQuery<TVertex>, IGremlinQuery>[] andTraversals);

        new IVGremlinQuery<TVertex> As(StepLabel stepLabel);

        new IVGremlinQuery<TVertex> Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IVGremlinQuery<TVertex>, IGremlinQuery> traversalPredicate, Func<IVGremlinQuery<TVertex>, TTargetQuery> trueChoice, Func<IVGremlinQuery<TVertex>, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IVGremlinQuery<TVertex>, IGremlinQuery> traversalPredicate, Func<IVGremlinQuery<TVertex>, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Coalesce<TTargetQuery>(params Func<IVGremlinQuery<TVertex>, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IVGremlinQuery<TVertex> Dedup();

        new IVGremlinQuery<TVertex> Emit();

        new IVGremlinQuery<TVertex> Filter(string lambda);

        new IVGremlinQuery<TVertex> Identity();

        new IVGremlinQuery<TVertex> Limit(long count);
        TTargetQuery Local<TTargetQuery>(Func<IVGremlinQuery<TVertex> , TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IVGremlinQuery<TVertex>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;
        
        IVGremlinQuery<TVertex> Not(Func<IVGremlinQuery<TVertex>, IGremlinQuery> notTraversal);

        TTargetQuery Optional<TTargetQuery>(Func<IVGremlinQuery<TVertex>, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery;
        IVGremlinQuery<TVertex> Or(params Func<IVGremlinQuery<TVertex>, IGremlinQuery>[] orTraversals);

        new IVGremlinQuery<TVertex> Range(long low, long high);

        TTargetQuery Repeat<TTargetQuery>(Func<IVGremlinQuery<TVertex>, TTargetQuery> repeatTraversal) where TTargetQuery : IVGremlinQuery<TVertex>;
        TTargetQuery RepeatUntil<TTargetQuery>(Func<IVGremlinQuery<TVertex>, TTargetQuery> repeatTraversal, Func<IVGremlinQuery<TVertex>, IGremlinQuery> untilTraversal) where TTargetQuery : IVGremlinQuery<TVertex>;

        IVGremlinQuery<TVertex> SideEffect(Func<IVGremlinQuery<TVertex>, IGremlinQuery> sideEffectTraversal);
        new IVGremlinQuery<TVertex> Skip(long count);

        new IVGremlinQuery<TVertex> Tail(long count);
        new IVGremlinQuery<TVertex> Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IVGremlinQuery<TVertex>, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;
    }

    public partial interface IEGremlinQuery<TEdge>
    {
        IEGremlinQuery<TEdge> And(params Func<IEGremlinQuery<TEdge>, IGremlinQuery>[] andTraversals);

        new IEGremlinQuery<TEdge> As(StepLabel stepLabel);

        new IEGremlinQuery<TEdge> Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IEGremlinQuery<TEdge>, IGremlinQuery> traversalPredicate, Func<IEGremlinQuery<TEdge>, TTargetQuery> trueChoice, Func<IEGremlinQuery<TEdge>, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IEGremlinQuery<TEdge>, IGremlinQuery> traversalPredicate, Func<IEGremlinQuery<TEdge>, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Coalesce<TTargetQuery>(params Func<IEGremlinQuery<TEdge>, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IEGremlinQuery<TEdge> Dedup();

        new IEGremlinQuery<TEdge> Emit();

        new IEGremlinQuery<TEdge> Filter(string lambda);

        new IEGremlinQuery<TEdge> Identity();

        new IEGremlinQuery<TEdge> Limit(long count);
        TTargetQuery Local<TTargetQuery>(Func<IEGremlinQuery<TEdge> , TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IEGremlinQuery<TEdge>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;
        
        IEGremlinQuery<TEdge> Not(Func<IEGremlinQuery<TEdge>, IGremlinQuery> notTraversal);

        TTargetQuery Optional<TTargetQuery>(Func<IEGremlinQuery<TEdge>, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery;
        IEGremlinQuery<TEdge> Or(params Func<IEGremlinQuery<TEdge>, IGremlinQuery>[] orTraversals);

        new IEGremlinQuery<TEdge> Range(long low, long high);

        TTargetQuery Repeat<TTargetQuery>(Func<IEGremlinQuery<TEdge>, TTargetQuery> repeatTraversal) where TTargetQuery : IEGremlinQuery<TEdge>;
        TTargetQuery RepeatUntil<TTargetQuery>(Func<IEGremlinQuery<TEdge>, TTargetQuery> repeatTraversal, Func<IEGremlinQuery<TEdge>, IGremlinQuery> untilTraversal) where TTargetQuery : IEGremlinQuery<TEdge>;

        IEGremlinQuery<TEdge> SideEffect(Func<IEGremlinQuery<TEdge>, IGremlinQuery> sideEffectTraversal);
        new IEGremlinQuery<TEdge> Skip(long count);

        new IEGremlinQuery<TEdge> Tail(long count);
        new IEGremlinQuery<TEdge> Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IEGremlinQuery<TEdge>, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;
    }

    public partial interface IEGremlinQuery<TEdge, TAdjacentVertex>
    {
        IEGremlinQuery<TEdge, TAdjacentVertex> And(params Func<IEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery>[] andTraversals);

        new IEGremlinQuery<TEdge, TAdjacentVertex> As(StepLabel stepLabel);

        new IEGremlinQuery<TEdge, TAdjacentVertex> Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> traversalPredicate, Func<IEGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery> trueChoice, Func<IEGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> traversalPredicate, Func<IEGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Coalesce<TTargetQuery>(params Func<IEGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IEGremlinQuery<TEdge, TAdjacentVertex> Dedup();

        new IEGremlinQuery<TEdge, TAdjacentVertex> Emit();

        new IEGremlinQuery<TEdge, TAdjacentVertex> Filter(string lambda);

        new IEGremlinQuery<TEdge, TAdjacentVertex> Identity();

        new IEGremlinQuery<TEdge, TAdjacentVertex> Limit(long count);
        TTargetQuery Local<TTargetQuery>(Func<IEGremlinQuery<TEdge, TAdjacentVertex> , TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IEGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;
        
        IEGremlinQuery<TEdge, TAdjacentVertex> Not(Func<IEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> notTraversal);

        TTargetQuery Optional<TTargetQuery>(Func<IEGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery;
        IEGremlinQuery<TEdge, TAdjacentVertex> Or(params Func<IEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery>[] orTraversals);

        new IEGremlinQuery<TEdge, TAdjacentVertex> Range(long low, long high);

        TTargetQuery Repeat<TTargetQuery>(Func<IEGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery> repeatTraversal) where TTargetQuery : IEGremlinQuery<TEdge, TAdjacentVertex>;
        TTargetQuery RepeatUntil<TTargetQuery>(Func<IEGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery> repeatTraversal, Func<IEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> untilTraversal) where TTargetQuery : IEGremlinQuery<TEdge, TAdjacentVertex>;

        IEGremlinQuery<TEdge, TAdjacentVertex> SideEffect(Func<IEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> sideEffectTraversal);
        new IEGremlinQuery<TEdge, TAdjacentVertex> Skip(long count);

        new IEGremlinQuery<TEdge, TAdjacentVertex> Tail(long count);
        new IEGremlinQuery<TEdge, TAdjacentVertex> Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IEGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;
    }

    public partial interface IEGremlinQuery<TEdge, TOutVertex, TInVertex>
    {
        IEGremlinQuery<TEdge, TOutVertex, TInVertex> And(params Func<IEGremlinQuery<TEdge, TOutVertex, TInVertex>, IGremlinQuery>[] andTraversals);

        new IEGremlinQuery<TEdge, TOutVertex, TInVertex> As(StepLabel stepLabel);

        new IEGremlinQuery<TEdge, TOutVertex, TInVertex> Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IEGremlinQuery<TEdge, TOutVertex, TInVertex>, IGremlinQuery> traversalPredicate, Func<IEGremlinQuery<TEdge, TOutVertex, TInVertex>, TTargetQuery> trueChoice, Func<IEGremlinQuery<TEdge, TOutVertex, TInVertex>, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IEGremlinQuery<TEdge, TOutVertex, TInVertex>, IGremlinQuery> traversalPredicate, Func<IEGremlinQuery<TEdge, TOutVertex, TInVertex>, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Coalesce<TTargetQuery>(params Func<IEGremlinQuery<TEdge, TOutVertex, TInVertex>, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IEGremlinQuery<TEdge, TOutVertex, TInVertex> Dedup();

        new IEGremlinQuery<TEdge, TOutVertex, TInVertex> Emit();

        new IEGremlinQuery<TEdge, TOutVertex, TInVertex> Filter(string lambda);

        new IEGremlinQuery<TEdge, TOutVertex, TInVertex> Identity();

        new IEGremlinQuery<TEdge, TOutVertex, TInVertex> Limit(long count);
        TTargetQuery Local<TTargetQuery>(Func<IEGremlinQuery<TEdge, TOutVertex, TInVertex> , TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IEGremlinQuery<TEdge, TOutVertex, TInVertex>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;
        
        IEGremlinQuery<TEdge, TOutVertex, TInVertex> Not(Func<IEGremlinQuery<TEdge, TOutVertex, TInVertex>, IGremlinQuery> notTraversal);

        TTargetQuery Optional<TTargetQuery>(Func<IEGremlinQuery<TEdge, TOutVertex, TInVertex>, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery;
        IEGremlinQuery<TEdge, TOutVertex, TInVertex> Or(params Func<IEGremlinQuery<TEdge, TOutVertex, TInVertex>, IGremlinQuery>[] orTraversals);

        new IEGremlinQuery<TEdge, TOutVertex, TInVertex> Range(long low, long high);

        TTargetQuery Repeat<TTargetQuery>(Func<IEGremlinQuery<TEdge, TOutVertex, TInVertex>, TTargetQuery> repeatTraversal) where TTargetQuery : IEGremlinQuery<TEdge, TOutVertex, TInVertex>;
        TTargetQuery RepeatUntil<TTargetQuery>(Func<IEGremlinQuery<TEdge, TOutVertex, TInVertex>, TTargetQuery> repeatTraversal, Func<IEGremlinQuery<TEdge, TOutVertex, TInVertex>, IGremlinQuery> untilTraversal) where TTargetQuery : IEGremlinQuery<TEdge, TOutVertex, TInVertex>;

        IEGremlinQuery<TEdge, TOutVertex, TInVertex> SideEffect(Func<IEGremlinQuery<TEdge, TOutVertex, TInVertex>, IGremlinQuery> sideEffectTraversal);
        new IEGremlinQuery<TEdge, TOutVertex, TInVertex> Skip(long count);

        new IEGremlinQuery<TEdge, TOutVertex, TInVertex> Tail(long count);
        new IEGremlinQuery<TEdge, TOutVertex, TInVertex> Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IEGremlinQuery<TEdge, TOutVertex, TInVertex>, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;
    }

    public partial interface IInEGremlinQuery<TEdge, TAdjacentVertex>
    {
        IInEGremlinQuery<TEdge, TAdjacentVertex> And(params Func<IInEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery>[] andTraversals);

        new IInEGremlinQuery<TEdge, TAdjacentVertex> As(StepLabel stepLabel);

        new IInEGremlinQuery<TEdge, TAdjacentVertex> Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IInEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> traversalPredicate, Func<IInEGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery> trueChoice, Func<IInEGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IInEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> traversalPredicate, Func<IInEGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Coalesce<TTargetQuery>(params Func<IInEGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IInEGremlinQuery<TEdge, TAdjacentVertex> Dedup();

        new IInEGremlinQuery<TEdge, TAdjacentVertex> Emit();

        new IInEGremlinQuery<TEdge, TAdjacentVertex> Filter(string lambda);

        new IInEGremlinQuery<TEdge, TAdjacentVertex> Identity();

        new IInEGremlinQuery<TEdge, TAdjacentVertex> Limit(long count);
        TTargetQuery Local<TTargetQuery>(Func<IInEGremlinQuery<TEdge, TAdjacentVertex> , TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IInEGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;
        
        IInEGremlinQuery<TEdge, TAdjacentVertex> Not(Func<IInEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> notTraversal);

        TTargetQuery Optional<TTargetQuery>(Func<IInEGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery;
        IInEGremlinQuery<TEdge, TAdjacentVertex> Or(params Func<IInEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery>[] orTraversals);

        new IInEGremlinQuery<TEdge, TAdjacentVertex> Range(long low, long high);

        TTargetQuery Repeat<TTargetQuery>(Func<IInEGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery> repeatTraversal) where TTargetQuery : IInEGremlinQuery<TEdge, TAdjacentVertex>;
        TTargetQuery RepeatUntil<TTargetQuery>(Func<IInEGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery> repeatTraversal, Func<IInEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> untilTraversal) where TTargetQuery : IInEGremlinQuery<TEdge, TAdjacentVertex>;

        IInEGremlinQuery<TEdge, TAdjacentVertex> SideEffect(Func<IInEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> sideEffectTraversal);
        new IInEGremlinQuery<TEdge, TAdjacentVertex> Skip(long count);

        new IInEGremlinQuery<TEdge, TAdjacentVertex> Tail(long count);
        new IInEGremlinQuery<TEdge, TAdjacentVertex> Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IInEGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;
    }

    public partial interface IOutEGremlinQuery<TEdge, TAdjacentVertex>
    {
        IOutEGremlinQuery<TEdge, TAdjacentVertex> And(params Func<IOutEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery>[] andTraversals);

        new IOutEGremlinQuery<TEdge, TAdjacentVertex> As(StepLabel stepLabel);

        new IOutEGremlinQuery<TEdge, TAdjacentVertex> Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IOutEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> traversalPredicate, Func<IOutEGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery> trueChoice, Func<IOutEGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IOutEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> traversalPredicate, Func<IOutEGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Coalesce<TTargetQuery>(params Func<IOutEGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IOutEGremlinQuery<TEdge, TAdjacentVertex> Dedup();

        new IOutEGremlinQuery<TEdge, TAdjacentVertex> Emit();

        new IOutEGremlinQuery<TEdge, TAdjacentVertex> Filter(string lambda);

        new IOutEGremlinQuery<TEdge, TAdjacentVertex> Identity();

        new IOutEGremlinQuery<TEdge, TAdjacentVertex> Limit(long count);
        TTargetQuery Local<TTargetQuery>(Func<IOutEGremlinQuery<TEdge, TAdjacentVertex> , TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IOutEGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;
        
        IOutEGremlinQuery<TEdge, TAdjacentVertex> Not(Func<IOutEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> notTraversal);

        TTargetQuery Optional<TTargetQuery>(Func<IOutEGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery;
        IOutEGremlinQuery<TEdge, TAdjacentVertex> Or(params Func<IOutEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery>[] orTraversals);

        new IOutEGremlinQuery<TEdge, TAdjacentVertex> Range(long low, long high);

        TTargetQuery Repeat<TTargetQuery>(Func<IOutEGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery> repeatTraversal) where TTargetQuery : IOutEGremlinQuery<TEdge, TAdjacentVertex>;
        TTargetQuery RepeatUntil<TTargetQuery>(Func<IOutEGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery> repeatTraversal, Func<IOutEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> untilTraversal) where TTargetQuery : IOutEGremlinQuery<TEdge, TAdjacentVertex>;

        IOutEGremlinQuery<TEdge, TAdjacentVertex> SideEffect(Func<IOutEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> sideEffectTraversal);
        new IOutEGremlinQuery<TEdge, TAdjacentVertex> Skip(long count);

        new IOutEGremlinQuery<TEdge, TAdjacentVertex> Tail(long count);
        new IOutEGremlinQuery<TEdge, TAdjacentVertex> Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IOutEGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;
    }

    public partial interface IVPropertiesGremlinQuery<TValue>
    {
        IVPropertiesGremlinQuery<TValue> And(params Func<IVPropertiesGremlinQuery<TValue>, IGremlinQuery>[] andTraversals);

        new IVPropertiesGremlinQuery<TValue> As(StepLabel stepLabel);

        new IVPropertiesGremlinQuery<TValue> Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IVPropertiesGremlinQuery<TValue>, IGremlinQuery> traversalPredicate, Func<IVPropertiesGremlinQuery<TValue>, TTargetQuery> trueChoice, Func<IVPropertiesGremlinQuery<TValue>, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IVPropertiesGremlinQuery<TValue>, IGremlinQuery> traversalPredicate, Func<IVPropertiesGremlinQuery<TValue>, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Coalesce<TTargetQuery>(params Func<IVPropertiesGremlinQuery<TValue>, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IVPropertiesGremlinQuery<TValue> Dedup();

        new IVPropertiesGremlinQuery<TValue> Emit();

        new IVPropertiesGremlinQuery<TValue> Filter(string lambda);

        new IVPropertiesGremlinQuery<TValue> Identity();

        new IVPropertiesGremlinQuery<TValue> Limit(long count);
        TTargetQuery Local<TTargetQuery>(Func<IVPropertiesGremlinQuery<TValue> , TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IVPropertiesGremlinQuery<TValue>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;
        
        IVPropertiesGremlinQuery<TValue> Not(Func<IVPropertiesGremlinQuery<TValue>, IGremlinQuery> notTraversal);

        TTargetQuery Optional<TTargetQuery>(Func<IVPropertiesGremlinQuery<TValue>, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery;
        IVPropertiesGremlinQuery<TValue> Or(params Func<IVPropertiesGremlinQuery<TValue>, IGremlinQuery>[] orTraversals);

        new IVPropertiesGremlinQuery<TValue> Range(long low, long high);

        TTargetQuery Repeat<TTargetQuery>(Func<IVPropertiesGremlinQuery<TValue>, TTargetQuery> repeatTraversal) where TTargetQuery : IVPropertiesGremlinQuery<TValue>;
        TTargetQuery RepeatUntil<TTargetQuery>(Func<IVPropertiesGremlinQuery<TValue>, TTargetQuery> repeatTraversal, Func<IVPropertiesGremlinQuery<TValue>, IGremlinQuery> untilTraversal) where TTargetQuery : IVPropertiesGremlinQuery<TValue>;

        IVPropertiesGremlinQuery<TValue> SideEffect(Func<IVPropertiesGremlinQuery<TValue>, IGremlinQuery> sideEffectTraversal);
        new IVPropertiesGremlinQuery<TValue> Skip(long count);

        new IVPropertiesGremlinQuery<TValue> Tail(long count);
        new IVPropertiesGremlinQuery<TValue> Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IVPropertiesGremlinQuery<TValue>, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;
    }

    public partial interface IVPropertiesGremlinQuery<TValue, TMeta>
    {
        IVPropertiesGremlinQuery<TValue, TMeta> And(params Func<IVPropertiesGremlinQuery<TValue, TMeta>, IGremlinQuery>[] andTraversals);

        new IVPropertiesGremlinQuery<TValue, TMeta> As(StepLabel stepLabel);

        new IVPropertiesGremlinQuery<TValue, TMeta> Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IVPropertiesGremlinQuery<TValue, TMeta>, IGremlinQuery> traversalPredicate, Func<IVPropertiesGremlinQuery<TValue, TMeta>, TTargetQuery> trueChoice, Func<IVPropertiesGremlinQuery<TValue, TMeta>, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IVPropertiesGremlinQuery<TValue, TMeta>, IGremlinQuery> traversalPredicate, Func<IVPropertiesGremlinQuery<TValue, TMeta>, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Coalesce<TTargetQuery>(params Func<IVPropertiesGremlinQuery<TValue, TMeta>, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IVPropertiesGremlinQuery<TValue, TMeta> Dedup();

        new IVPropertiesGremlinQuery<TValue, TMeta> Emit();

        new IVPropertiesGremlinQuery<TValue, TMeta> Filter(string lambda);

        new IVPropertiesGremlinQuery<TValue, TMeta> Identity();

        new IVPropertiesGremlinQuery<TValue, TMeta> Limit(long count);
        TTargetQuery Local<TTargetQuery>(Func<IVPropertiesGremlinQuery<TValue, TMeta> , TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IVPropertiesGremlinQuery<TValue, TMeta>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;
        
        IVPropertiesGremlinQuery<TValue, TMeta> Not(Func<IVPropertiesGremlinQuery<TValue, TMeta>, IGremlinQuery> notTraversal);

        TTargetQuery Optional<TTargetQuery>(Func<IVPropertiesGremlinQuery<TValue, TMeta>, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery;
        IVPropertiesGremlinQuery<TValue, TMeta> Or(params Func<IVPropertiesGremlinQuery<TValue, TMeta>, IGremlinQuery>[] orTraversals);

        new IVPropertiesGremlinQuery<TValue, TMeta> Range(long low, long high);

        TTargetQuery Repeat<TTargetQuery>(Func<IVPropertiesGremlinQuery<TValue, TMeta>, TTargetQuery> repeatTraversal) where TTargetQuery : IVPropertiesGremlinQuery<TValue, TMeta>;
        TTargetQuery RepeatUntil<TTargetQuery>(Func<IVPropertiesGremlinQuery<TValue, TMeta>, TTargetQuery> repeatTraversal, Func<IVPropertiesGremlinQuery<TValue, TMeta>, IGremlinQuery> untilTraversal) where TTargetQuery : IVPropertiesGremlinQuery<TValue, TMeta>;

        IVPropertiesGremlinQuery<TValue, TMeta> SideEffect(Func<IVPropertiesGremlinQuery<TValue, TMeta>, IGremlinQuery> sideEffectTraversal);
        new IVPropertiesGremlinQuery<TValue, TMeta> Skip(long count);

        new IVPropertiesGremlinQuery<TValue, TMeta> Tail(long count);
        new IVPropertiesGremlinQuery<TValue, TMeta> Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IVPropertiesGremlinQuery<TValue, TMeta>, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;
    }

    public partial interface IEPropertiesGremlinQuery<TValue>
    {
        IEPropertiesGremlinQuery<TValue> And(params Func<IEPropertiesGremlinQuery<TValue>, IGremlinQuery>[] andTraversals);

        new IEPropertiesGremlinQuery<TValue> As(StepLabel stepLabel);

        new IEPropertiesGremlinQuery<TValue> Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IEPropertiesGremlinQuery<TValue>, IGremlinQuery> traversalPredicate, Func<IEPropertiesGremlinQuery<TValue>, TTargetQuery> trueChoice, Func<IEPropertiesGremlinQuery<TValue>, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IEPropertiesGremlinQuery<TValue>, IGremlinQuery> traversalPredicate, Func<IEPropertiesGremlinQuery<TValue>, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Coalesce<TTargetQuery>(params Func<IEPropertiesGremlinQuery<TValue>, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IEPropertiesGremlinQuery<TValue> Dedup();

        new IEPropertiesGremlinQuery<TValue> Emit();

        new IEPropertiesGremlinQuery<TValue> Filter(string lambda);

        new IEPropertiesGremlinQuery<TValue> Identity();

        new IEPropertiesGremlinQuery<TValue> Limit(long count);
        TTargetQuery Local<TTargetQuery>(Func<IEPropertiesGremlinQuery<TValue> , TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IEPropertiesGremlinQuery<TValue>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;
        
        IEPropertiesGremlinQuery<TValue> Not(Func<IEPropertiesGremlinQuery<TValue>, IGremlinQuery> notTraversal);

        TTargetQuery Optional<TTargetQuery>(Func<IEPropertiesGremlinQuery<TValue>, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery;
        IEPropertiesGremlinQuery<TValue> Or(params Func<IEPropertiesGremlinQuery<TValue>, IGremlinQuery>[] orTraversals);

        new IEPropertiesGremlinQuery<TValue> Range(long low, long high);

        TTargetQuery Repeat<TTargetQuery>(Func<IEPropertiesGremlinQuery<TValue>, TTargetQuery> repeatTraversal) where TTargetQuery : IEPropertiesGremlinQuery<TValue>;
        TTargetQuery RepeatUntil<TTargetQuery>(Func<IEPropertiesGremlinQuery<TValue>, TTargetQuery> repeatTraversal, Func<IEPropertiesGremlinQuery<TValue>, IGremlinQuery> untilTraversal) where TTargetQuery : IEPropertiesGremlinQuery<TValue>;

        IEPropertiesGremlinQuery<TValue> SideEffect(Func<IEPropertiesGremlinQuery<TValue>, IGremlinQuery> sideEffectTraversal);
        new IEPropertiesGremlinQuery<TValue> Skip(long count);

        new IEPropertiesGremlinQuery<TValue> Tail(long count);
        new IEPropertiesGremlinQuery<TValue> Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IEPropertiesGremlinQuery<TValue>, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;
    }

    public partial interface IOrderedGremlinQuery<TElement>
    {
        IOrderedGremlinQuery<TElement> ThenBy(Expression<Func<TElement, object>> projection);
        IOrderedGremlinQuery<TElement> ThenBy(Func<IGremlinQuery<TElement>, IGremlinQuery> traversal);
        IOrderedGremlinQuery<TElement> ThenByDescending(Expression<Func<TElement, object>> projection);
        IOrderedGremlinQuery<TElement> ThenByDescending(Func<IGremlinQuery<TElement>, IGremlinQuery> traversal);
        IOrderedGremlinQuery<TElement> ThenBy(string lambda);
    }

    public partial interface IOrderedElementGremlinQuery<TElement>
    {
        IOrderedElementGremlinQuery<TElement> ThenBy(Expression<Func<TElement, object>> projection);
        IOrderedElementGremlinQuery<TElement> ThenBy(Func<IElementGremlinQuery<TElement>, IGremlinQuery> traversal);
        IOrderedElementGremlinQuery<TElement> ThenByDescending(Expression<Func<TElement, object>> projection);
        IOrderedElementGremlinQuery<TElement> ThenByDescending(Func<IElementGremlinQuery<TElement>, IGremlinQuery> traversal);
        IOrderedElementGremlinQuery<TElement> ThenBy(string lambda);
    }

    public partial interface IOrderedVGremlinQuery<TVertex>
    {
        IOrderedVGremlinQuery<TVertex> ThenBy(Expression<Func<TVertex, object>> projection);
        IOrderedVGremlinQuery<TVertex> ThenBy(Func<IVGremlinQuery<TVertex>, IGremlinQuery> traversal);
        IOrderedVGremlinQuery<TVertex> ThenByDescending(Expression<Func<TVertex, object>> projection);
        IOrderedVGremlinQuery<TVertex> ThenByDescending(Func<IVGremlinQuery<TVertex>, IGremlinQuery> traversal);
        IOrderedVGremlinQuery<TVertex> ThenBy(string lambda);
    }

    public partial interface IOrderedEGremlinQuery<TEdge>
    {
        IOrderedEGremlinQuery<TEdge> ThenBy(Expression<Func<TEdge, object>> projection);
        IOrderedEGremlinQuery<TEdge> ThenBy(Func<IEGremlinQuery<TEdge>, IGremlinQuery> traversal);
        IOrderedEGremlinQuery<TEdge> ThenByDescending(Expression<Func<TEdge, object>> projection);
        IOrderedEGremlinQuery<TEdge> ThenByDescending(Func<IEGremlinQuery<TEdge>, IGremlinQuery> traversal);
        IOrderedEGremlinQuery<TEdge> ThenBy(string lambda);
    }

    public partial interface IOrderedEGremlinQuery<TEdge, TAdjacentVertex>
    {
        IOrderedEGremlinQuery<TEdge, TAdjacentVertex> ThenBy(Expression<Func<TEdge, object>> projection);
        IOrderedEGremlinQuery<TEdge, TAdjacentVertex> ThenBy(Func<IEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> traversal);
        IOrderedEGremlinQuery<TEdge, TAdjacentVertex> ThenByDescending(Expression<Func<TEdge, object>> projection);
        IOrderedEGremlinQuery<TEdge, TAdjacentVertex> ThenByDescending(Func<IEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> traversal);
        IOrderedEGremlinQuery<TEdge, TAdjacentVertex> ThenBy(string lambda);
    }

    public partial interface IOrderedEGremlinQuery<TEdge, TOutVertex, TInVertex>
    {
        IOrderedEGremlinQuery<TEdge, TOutVertex, TInVertex> ThenBy(Expression<Func<TEdge, object>> projection);
        IOrderedEGremlinQuery<TEdge, TOutVertex, TInVertex> ThenBy(Func<IEGremlinQuery<TEdge, TOutVertex, TInVertex>, IGremlinQuery> traversal);
        IOrderedEGremlinQuery<TEdge, TOutVertex, TInVertex> ThenByDescending(Expression<Func<TEdge, object>> projection);
        IOrderedEGremlinQuery<TEdge, TOutVertex, TInVertex> ThenByDescending(Func<IEGremlinQuery<TEdge, TOutVertex, TInVertex>, IGremlinQuery> traversal);
        IOrderedEGremlinQuery<TEdge, TOutVertex, TInVertex> ThenBy(string lambda);
    }

    public partial interface IOrderedInEGremlinQuery<TEdge, TAdjacentVertex>
    {
        IOrderedInEGremlinQuery<TEdge, TAdjacentVertex> ThenBy(Expression<Func<TEdge, object>> projection);
        IOrderedInEGremlinQuery<TEdge, TAdjacentVertex> ThenBy(Func<IInEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> traversal);
        IOrderedInEGremlinQuery<TEdge, TAdjacentVertex> ThenByDescending(Expression<Func<TEdge, object>> projection);
        IOrderedInEGremlinQuery<TEdge, TAdjacentVertex> ThenByDescending(Func<IInEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> traversal);
        IOrderedInEGremlinQuery<TEdge, TAdjacentVertex> ThenBy(string lambda);
    }

    public partial interface IOrderedOutEGremlinQuery<TEdge, TAdjacentVertex>
    {
        IOrderedOutEGremlinQuery<TEdge, TAdjacentVertex> ThenBy(Expression<Func<TEdge, object>> projection);
        IOrderedOutEGremlinQuery<TEdge, TAdjacentVertex> ThenBy(Func<IOutEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> traversal);
        IOrderedOutEGremlinQuery<TEdge, TAdjacentVertex> ThenByDescending(Expression<Func<TEdge, object>> projection);
        IOrderedOutEGremlinQuery<TEdge, TAdjacentVertex> ThenByDescending(Func<IOutEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> traversal);
        IOrderedOutEGremlinQuery<TEdge, TAdjacentVertex> ThenBy(string lambda);
    }

    public partial interface IOrderedVPropertiesGremlinQuery<TValue>
    {
        IOrderedVPropertiesGremlinQuery<TValue> ThenBy(Expression<Func<TValue, object>> projection);
        IOrderedVPropertiesGremlinQuery<TValue> ThenBy(Func<IVPropertiesGremlinQuery<TValue>, IGremlinQuery> traversal);
        IOrderedVPropertiesGremlinQuery<TValue> ThenByDescending(Expression<Func<TValue, object>> projection);
        IOrderedVPropertiesGremlinQuery<TValue> ThenByDescending(Func<IVPropertiesGremlinQuery<TValue>, IGremlinQuery> traversal);
        IOrderedVPropertiesGremlinQuery<TValue> ThenBy(string lambda);
    }

    public partial interface IOrderedVPropertiesGremlinQuery<TValue, TMeta>
    {
        IOrderedVPropertiesGremlinQuery<TValue, TMeta> ThenBy(Expression<Func<TValue, object>> projection);
        IOrderedVPropertiesGremlinQuery<TValue, TMeta> ThenBy(Func<IVPropertiesGremlinQuery<TValue, TMeta>, IGremlinQuery> traversal);
        IOrderedVPropertiesGremlinQuery<TValue, TMeta> ThenByDescending(Expression<Func<TValue, object>> projection);
        IOrderedVPropertiesGremlinQuery<TValue, TMeta> ThenByDescending(Func<IVPropertiesGremlinQuery<TValue, TMeta>, IGremlinQuery> traversal);
        IOrderedVPropertiesGremlinQuery<TValue, TMeta> ThenBy(string lambda);
    }

    public partial interface IOrderedEPropertiesGremlinQuery<TValue>
    {
        IOrderedEPropertiesGremlinQuery<TValue> ThenBy(Expression<Func<TValue, object>> projection);
        IOrderedEPropertiesGremlinQuery<TValue> ThenBy(Func<IEPropertiesGremlinQuery<TValue>, IGremlinQuery> traversal);
        IOrderedEPropertiesGremlinQuery<TValue> ThenByDescending(Expression<Func<TValue, object>> projection);
        IOrderedEPropertiesGremlinQuery<TValue> ThenByDescending(Func<IEPropertiesGremlinQuery<TValue>, IGremlinQuery> traversal);
        IOrderedEPropertiesGremlinQuery<TValue> ThenBy(string lambda);
    }

    public partial interface IElementGremlinQuery<TElement>
    {

    }

    public partial interface IVGremlinQuery<TVertex>
    {

    }

    public partial interface IEGremlinQuery<TEdge>
    {

    }

    public partial interface IEGremlinQuery<TEdge, TAdjacentVertex>
    {

    }

    public partial interface IEGremlinQuery<TEdge, TOutVertex, TInVertex>
    {

    }

    public partial interface IInEGremlinQuery<TEdge, TAdjacentVertex>
    {

    }

    public partial interface IOutEGremlinQuery<TEdge, TAdjacentVertex>
    {

    }

    public partial interface IVPropertiesGremlinQuery<TValue>
    {

    }

    public partial interface IVPropertiesGremlinQuery<TValue, TMeta>
    {

    }

    public partial interface IVGremlinQuery<TVertex>
    {
        new IGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TVertex, TTarget>>[] projections);
        new IGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TVertex, TTarget[]>>[] projections);
    }

    public partial interface IEGremlinQuery<TEdge>
    {
        new IGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TEdge, TTarget>>[] projections);
        new IGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TEdge, TTarget[]>>[] projections);
    }

    public partial interface IVGremlinQuery
    {
        IVGremlinQuery<TTarget> OfType<TTarget>();
    }

    public partial interface IEGremlinQuery
    {
        IEGremlinQuery<TTarget> OfType<TTarget>();
    }

    public partial interface IVGremlinQuery<TVertex>
    {
        new IVGremlinQuery<TTarget> OfType<TTarget>();

        new IVGremlinQuery<TVertex> Property<TProjectedValue>(Expression<Func<TVertex, TProjectedValue>> projection, TProjectedValue value);
        new IVGremlinQuery<TVertex> Property<TProjectedValue>(Expression<Func<TVertex, TProjectedValue[]>> projection, TProjectedValue value);

        new IVGremlinQuery<TVertex> Where(Expression<Func<TVertex, bool>> predicate);
        new IVGremlinQuery<TVertex> Where<TProjection>(Expression<Func<TVertex, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal);
    }

    public partial interface IEGremlinQuery<TEdge>
    {
        new IEGremlinQuery<TTarget> OfType<TTarget>();

        new IEGremlinQuery<TEdge> Property<TProjectedValue>(Expression<Func<TEdge, TProjectedValue>> projection, TProjectedValue value);
        new IEGremlinQuery<TEdge> Property<TProjectedValue>(Expression<Func<TEdge, TProjectedValue[]>> projection, TProjectedValue value);

        new IEGremlinQuery<TEdge> Where(Expression<Func<TEdge, bool>> predicate);
        new IEGremlinQuery<TEdge> Where<TProjection>(Expression<Func<TEdge, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal);
    }

    public partial interface IEGremlinQuery<TEdge, TAdjacentVertex>
    {
        new IEGremlinQuery<TTarget, TAdjacentVertex> OfType<TTarget>();

        new IEGremlinQuery<TEdge, TAdjacentVertex> Property<TProjectedValue>(Expression<Func<TEdge, TProjectedValue>> projection, TProjectedValue value);
        new IEGremlinQuery<TEdge, TAdjacentVertex> Property<TProjectedValue>(Expression<Func<TEdge, TProjectedValue[]>> projection, TProjectedValue value);

        new IEGremlinQuery<TEdge, TAdjacentVertex> Where(Expression<Func<TEdge, bool>> predicate);
        new IEGremlinQuery<TEdge, TAdjacentVertex> Where<TProjection>(Expression<Func<TEdge, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal);
    }

    public partial interface IEGremlinQuery<TEdge, TOutVertex, TInVertex>
    {
        new IEGremlinQuery<TTarget, TOutVertex, TInVertex> OfType<TTarget>();

        new IEGremlinQuery<TEdge, TOutVertex, TInVertex> Property<TProjectedValue>(Expression<Func<TEdge, TProjectedValue>> projection, TProjectedValue value);
        new IEGremlinQuery<TEdge, TOutVertex, TInVertex> Property<TProjectedValue>(Expression<Func<TEdge, TProjectedValue[]>> projection, TProjectedValue value);

        new IEGremlinQuery<TEdge, TOutVertex, TInVertex> Where(Expression<Func<TEdge, bool>> predicate);
        new IEGremlinQuery<TEdge, TOutVertex, TInVertex> Where<TProjection>(Expression<Func<TEdge, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal);
    }

    public partial interface IInEGremlinQuery<TEdge, TAdjacentVertex>
    {
        new IInEGremlinQuery<TTarget, TAdjacentVertex> OfType<TTarget>();

        new IInEGremlinQuery<TEdge, TAdjacentVertex> Property<TProjectedValue>(Expression<Func<TEdge, TProjectedValue>> projection, TProjectedValue value);
        new IInEGremlinQuery<TEdge, TAdjacentVertex> Property<TProjectedValue>(Expression<Func<TEdge, TProjectedValue[]>> projection, TProjectedValue value);

        new IInEGremlinQuery<TEdge, TAdjacentVertex> Where(Expression<Func<TEdge, bool>> predicate);
        new IInEGremlinQuery<TEdge, TAdjacentVertex> Where<TProjection>(Expression<Func<TEdge, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal);
    }

    public partial interface IOutEGremlinQuery<TEdge, TAdjacentVertex>
    {
        new IOutEGremlinQuery<TTarget, TAdjacentVertex> OfType<TTarget>();

        new IOutEGremlinQuery<TEdge, TAdjacentVertex> Property<TProjectedValue>(Expression<Func<TEdge, TProjectedValue>> projection, TProjectedValue value);
        new IOutEGremlinQuery<TEdge, TAdjacentVertex> Property<TProjectedValue>(Expression<Func<TEdge, TProjectedValue[]>> projection, TProjectedValue value);

        new IOutEGremlinQuery<TEdge, TAdjacentVertex> Where(Expression<Func<TEdge, bool>> predicate);
        new IOutEGremlinQuery<TEdge, TAdjacentVertex> Where<TProjection>(Expression<Func<TEdge, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal);
    }

    public partial interface IGremlinQuery<TElement>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IGremlinQuery<TElement>, StepLabel<IGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IGremlinQuery<TResult> Cast<TResult>();
        
        new IGremlinQuery<TElement> Where(Func<IGremlinQuery<TElement>, IGremlinQuery> filterTraversal);
    }

    public partial interface IElementGremlinQuery<TElement>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IElementGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IElementGremlinQuery<TElement>, StepLabel<IElementGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IElementGremlinQuery<TResult> Cast<TResult>();
        
        new IElementGremlinQuery<TElement> Where(Func<IElementGremlinQuery<TElement>, IGremlinQuery> filterTraversal);
    }

    public partial interface IVGremlinQuery<TVertex>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IVGremlinQuery<TVertex>, StepLabel<TVertex[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IVGremlinQuery<TVertex>, StepLabel<IVGremlinQuery<TVertex>, TVertex>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IVGremlinQuery<TResult> Cast<TResult>();
        
        new IVGremlinQuery<TVertex> Where(Func<IVGremlinQuery<TVertex>, IGremlinQuery> filterTraversal);
    }

    public partial interface IEGremlinQuery<TEdge>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IEGremlinQuery<TEdge>, StepLabel<TEdge[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IEGremlinQuery<TEdge>, StepLabel<IEGremlinQuery<TEdge>, TEdge>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IEGremlinQuery<TResult> Cast<TResult>();
        
        new IEGremlinQuery<TEdge> Where(Func<IEGremlinQuery<TEdge>, IGremlinQuery> filterTraversal);
    }

    public partial interface IEGremlinQuery<TEdge, TAdjacentVertex>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IEGremlinQuery<TEdge, TAdjacentVertex>, StepLabel<TEdge[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IEGremlinQuery<TEdge, TAdjacentVertex>, StepLabel<IEGremlinQuery<TEdge, TAdjacentVertex>, TEdge>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IEGremlinQuery<TResult, TAdjacentVertex> Cast<TResult>();
        
        new IEGremlinQuery<TEdge, TAdjacentVertex> Where(Func<IEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> filterTraversal);
    }

    public partial interface IEGremlinQuery<TEdge, TOutVertex, TInVertex>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IEGremlinQuery<TEdge, TOutVertex, TInVertex>, StepLabel<TEdge[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IEGremlinQuery<TEdge, TOutVertex, TInVertex>, StepLabel<IEGremlinQuery<TEdge, TOutVertex, TInVertex>, TEdge>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IEGremlinQuery<TResult, TOutVertex, TInVertex> Cast<TResult>();
        
        new IEGremlinQuery<TEdge, TOutVertex, TInVertex> Where(Func<IEGremlinQuery<TEdge, TOutVertex, TInVertex>, IGremlinQuery> filterTraversal);
    }

    public partial interface IInEGremlinQuery<TEdge, TAdjacentVertex>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IInEGremlinQuery<TEdge, TAdjacentVertex>, StepLabel<TEdge[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IInEGremlinQuery<TEdge, TAdjacentVertex>, StepLabel<IInEGremlinQuery<TEdge, TAdjacentVertex>, TEdge>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IInEGremlinQuery<TResult, TAdjacentVertex> Cast<TResult>();
        
        new IInEGremlinQuery<TEdge, TAdjacentVertex> Where(Func<IInEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> filterTraversal);
    }

    public partial interface IOutEGremlinQuery<TEdge, TAdjacentVertex>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IOutEGremlinQuery<TEdge, TAdjacentVertex>, StepLabel<TEdge[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IOutEGremlinQuery<TEdge, TAdjacentVertex>, StepLabel<IOutEGremlinQuery<TEdge, TAdjacentVertex>, TEdge>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IOutEGremlinQuery<TResult, TAdjacentVertex> Cast<TResult>();
        
        new IOutEGremlinQuery<TEdge, TAdjacentVertex> Where(Func<IOutEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> filterTraversal);
    }

    public partial interface IVPropertiesGremlinQuery<TValue>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IVPropertiesGremlinQuery<TValue>, StepLabel<TValue[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IVPropertiesGremlinQuery<TValue>, StepLabel<IVPropertiesGremlinQuery<TValue>, TValue>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IVPropertiesGremlinQuery<TResult> Cast<TResult>();
        
        new IVPropertiesGremlinQuery<TValue> Where(Func<IVPropertiesGremlinQuery<TValue>, IGremlinQuery> filterTraversal);
    }

    public partial interface IVPropertiesGremlinQuery<TValue, TMeta>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IVPropertiesGremlinQuery<TValue, TMeta>, StepLabel<TValue[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IVPropertiesGremlinQuery<TValue, TMeta>, StepLabel<IVPropertiesGremlinQuery<TValue, TMeta>, TValue>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IVPropertiesGremlinQuery<TResult, TMeta> Cast<TResult>();
        
        new IVPropertiesGremlinQuery<TValue, TMeta> Where(Func<IVPropertiesGremlinQuery<TValue, TMeta>, IGremlinQuery> filterTraversal);
    }

    public partial interface IEPropertiesGremlinQuery<TValue>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IEPropertiesGremlinQuery<TValue>, StepLabel<TValue[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IEPropertiesGremlinQuery<TValue>, StepLabel<IEPropertiesGremlinQuery<TValue>, TValue>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IEPropertiesGremlinQuery<TResult> Cast<TResult>();
        
        new IEPropertiesGremlinQuery<TValue> Where(Func<IEPropertiesGremlinQuery<TValue>, IGremlinQuery> filterTraversal);
    }

    public partial interface IOrderedGremlinQuery<TElement>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IOrderedGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IOrderedGremlinQuery<TElement>, StepLabel<IOrderedGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IOrderedGremlinQuery<TResult> Cast<TResult>();
        
        new IGremlinQuery<TElement> Where(Func<IOrderedGremlinQuery<TElement>, IGremlinQuery> filterTraversal);
    }

    public partial interface IOrderedElementGremlinQuery<TElement>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IOrderedElementGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IOrderedElementGremlinQuery<TElement>, StepLabel<IOrderedElementGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IOrderedElementGremlinQuery<TResult> Cast<TResult>();
        
        new IElementGremlinQuery<TElement> Where(Func<IOrderedElementGremlinQuery<TElement>, IGremlinQuery> filterTraversal);
    }

    public partial interface IOrderedVGremlinQuery<TVertex>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IOrderedVGremlinQuery<TVertex>, StepLabel<TVertex[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IOrderedVGremlinQuery<TVertex>, StepLabel<IOrderedVGremlinQuery<TVertex>, TVertex>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IOrderedVGremlinQuery<TResult> Cast<TResult>();
        
        new IVGremlinQuery<TVertex> Where(Func<IOrderedVGremlinQuery<TVertex>, IGremlinQuery> filterTraversal);
    }

    public partial interface IOrderedEGremlinQuery<TEdge>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IOrderedEGremlinQuery<TEdge>, StepLabel<TEdge[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IOrderedEGremlinQuery<TEdge>, StepLabel<IOrderedEGremlinQuery<TEdge>, TEdge>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IOrderedEGremlinQuery<TResult> Cast<TResult>();
        
        new IEGremlinQuery<TEdge> Where(Func<IOrderedEGremlinQuery<TEdge>, IGremlinQuery> filterTraversal);
    }

    public partial interface IOrderedEGremlinQuery<TEdge, TAdjacentVertex>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IOrderedEGremlinQuery<TEdge, TAdjacentVertex>, StepLabel<TEdge[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IOrderedEGremlinQuery<TEdge, TAdjacentVertex>, StepLabel<IOrderedEGremlinQuery<TEdge, TAdjacentVertex>, TEdge>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IOrderedEGremlinQuery<TResult, TAdjacentVertex> Cast<TResult>();
        
        new IEGremlinQuery<TEdge, TAdjacentVertex> Where(Func<IOrderedEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> filterTraversal);
    }

    public partial interface IOrderedEGremlinQuery<TEdge, TOutVertex, TInVertex>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IOrderedEGremlinQuery<TEdge, TOutVertex, TInVertex>, StepLabel<TEdge[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IOrderedEGremlinQuery<TEdge, TOutVertex, TInVertex>, StepLabel<IOrderedEGremlinQuery<TEdge, TOutVertex, TInVertex>, TEdge>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IOrderedEGremlinQuery<TResult, TOutVertex, TInVertex> Cast<TResult>();
        
        new IEGremlinQuery<TEdge, TOutVertex, TInVertex> Where(Func<IOrderedEGremlinQuery<TEdge, TOutVertex, TInVertex>, IGremlinQuery> filterTraversal);
    }

    public partial interface IOrderedInEGremlinQuery<TEdge, TAdjacentVertex>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IOrderedInEGremlinQuery<TEdge, TAdjacentVertex>, StepLabel<TEdge[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IOrderedInEGremlinQuery<TEdge, TAdjacentVertex>, StepLabel<IOrderedInEGremlinQuery<TEdge, TAdjacentVertex>, TEdge>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IOrderedInEGremlinQuery<TResult, TAdjacentVertex> Cast<TResult>();
        
        new IInEGremlinQuery<TEdge, TAdjacentVertex> Where(Func<IOrderedInEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> filterTraversal);
    }

    public partial interface IOrderedOutEGremlinQuery<TEdge, TAdjacentVertex>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IOrderedOutEGremlinQuery<TEdge, TAdjacentVertex>, StepLabel<TEdge[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IOrderedOutEGremlinQuery<TEdge, TAdjacentVertex>, StepLabel<IOrderedOutEGremlinQuery<TEdge, TAdjacentVertex>, TEdge>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IOrderedOutEGremlinQuery<TResult, TAdjacentVertex> Cast<TResult>();
        
        new IOutEGremlinQuery<TEdge, TAdjacentVertex> Where(Func<IOrderedOutEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> filterTraversal);
    }

    public partial interface IOrderedVPropertiesGremlinQuery<TValue>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IOrderedVPropertiesGremlinQuery<TValue>, StepLabel<TValue[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IOrderedVPropertiesGremlinQuery<TValue>, StepLabel<IOrderedVPropertiesGremlinQuery<TValue>, TValue>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IOrderedVPropertiesGremlinQuery<TResult> Cast<TResult>();
        
        new IVPropertiesGremlinQuery<TValue> Where(Func<IOrderedVPropertiesGremlinQuery<TValue>, IGremlinQuery> filterTraversal);
    }

    public partial interface IOrderedVPropertiesGremlinQuery<TValue, TMeta>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IOrderedVPropertiesGremlinQuery<TValue, TMeta>, StepLabel<TValue[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IOrderedVPropertiesGremlinQuery<TValue, TMeta>, StepLabel<IOrderedVPropertiesGremlinQuery<TValue, TMeta>, TValue>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IOrderedVPropertiesGremlinQuery<TResult, TMeta> Cast<TResult>();
        
        new IVPropertiesGremlinQuery<TValue, TMeta> Where(Func<IOrderedVPropertiesGremlinQuery<TValue, TMeta>, IGremlinQuery> filterTraversal);
    }

    public partial interface IOrderedEPropertiesGremlinQuery<TValue>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IOrderedEPropertiesGremlinQuery<TValue>, StepLabel<TValue[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IOrderedEPropertiesGremlinQuery<TValue>, StepLabel<IOrderedEPropertiesGremlinQuery<TValue>, TValue>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IOrderedEPropertiesGremlinQuery<TResult> Cast<TResult>();
        
        new IEPropertiesGremlinQuery<TValue> Where(Func<IOrderedEPropertiesGremlinQuery<TValue>, IGremlinQuery> filterTraversal);
    }

    public partial interface IGremlinQuery<TElement>
    {
        new IOrderedGremlinQuery<TElement> OrderBy(Expression<Func<TElement, object>> projection);
        new IOrderedGremlinQuery<TElement> OrderBy(Func<IGremlinQuery<TElement>, IGremlinQuery> traversal);
        new IOrderedGremlinQuery<TElement> OrderBy(string lambda);
        new IOrderedGremlinQuery<TElement> OrderByDescending(Expression<Func<TElement, object>> projection);
        new IOrderedGremlinQuery<TElement> OrderByDescending(Func<IGremlinQuery<TElement>, IGremlinQuery> traversal);
    }
    
    public partial interface IElementGremlinQuery<TElement>
    {
        new IOrderedElementGremlinQuery<TElement> OrderBy(Expression<Func<TElement, object>> projection);
        new IOrderedElementGremlinQuery<TElement> OrderBy(Func<IElementGremlinQuery<TElement>, IGremlinQuery> traversal);
        new IOrderedElementGremlinQuery<TElement> OrderBy(string lambda);
        new IOrderedElementGremlinQuery<TElement> OrderByDescending(Expression<Func<TElement, object>> projection);
        new IOrderedElementGremlinQuery<TElement> OrderByDescending(Func<IElementGremlinQuery<TElement>, IGremlinQuery> traversal);
    }
    
    public partial interface IVGremlinQuery<TVertex>
    {
        new IOrderedVGremlinQuery<TVertex> OrderBy(Expression<Func<TVertex, object>> projection);
        new IOrderedVGremlinQuery<TVertex> OrderBy(Func<IVGremlinQuery<TVertex>, IGremlinQuery> traversal);
        new IOrderedVGremlinQuery<TVertex> OrderBy(string lambda);
        new IOrderedVGremlinQuery<TVertex> OrderByDescending(Expression<Func<TVertex, object>> projection);
        new IOrderedVGremlinQuery<TVertex> OrderByDescending(Func<IVGremlinQuery<TVertex>, IGremlinQuery> traversal);
    }
    
    public partial interface IEGremlinQuery<TEdge>
    {
        new IOrderedEGremlinQuery<TEdge> OrderBy(Expression<Func<TEdge, object>> projection);
        new IOrderedEGremlinQuery<TEdge> OrderBy(Func<IEGremlinQuery<TEdge>, IGremlinQuery> traversal);
        new IOrderedEGremlinQuery<TEdge> OrderBy(string lambda);
        new IOrderedEGremlinQuery<TEdge> OrderByDescending(Expression<Func<TEdge, object>> projection);
        new IOrderedEGremlinQuery<TEdge> OrderByDescending(Func<IEGremlinQuery<TEdge>, IGremlinQuery> traversal);
    }
    
    public partial interface IEGremlinQuery<TEdge, TAdjacentVertex>
    {
        new IOrderedEGremlinQuery<TEdge, TAdjacentVertex> OrderBy(Expression<Func<TEdge, object>> projection);
        new IOrderedEGremlinQuery<TEdge, TAdjacentVertex> OrderBy(Func<IEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> traversal);
        new IOrderedEGremlinQuery<TEdge, TAdjacentVertex> OrderBy(string lambda);
        new IOrderedEGremlinQuery<TEdge, TAdjacentVertex> OrderByDescending(Expression<Func<TEdge, object>> projection);
        new IOrderedEGremlinQuery<TEdge, TAdjacentVertex> OrderByDescending(Func<IEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> traversal);
    }
    
    public partial interface IEGremlinQuery<TEdge, TOutVertex, TInVertex>
    {
        new IOrderedEGremlinQuery<TEdge, TOutVertex, TInVertex> OrderBy(Expression<Func<TEdge, object>> projection);
        new IOrderedEGremlinQuery<TEdge, TOutVertex, TInVertex> OrderBy(Func<IEGremlinQuery<TEdge, TOutVertex, TInVertex>, IGremlinQuery> traversal);
        new IOrderedEGremlinQuery<TEdge, TOutVertex, TInVertex> OrderBy(string lambda);
        new IOrderedEGremlinQuery<TEdge, TOutVertex, TInVertex> OrderByDescending(Expression<Func<TEdge, object>> projection);
        new IOrderedEGremlinQuery<TEdge, TOutVertex, TInVertex> OrderByDescending(Func<IEGremlinQuery<TEdge, TOutVertex, TInVertex>, IGremlinQuery> traversal);
    }
    
    public partial interface IInEGremlinQuery<TEdge, TAdjacentVertex>
    {
        new IOrderedInEGremlinQuery<TEdge, TAdjacentVertex> OrderBy(Expression<Func<TEdge, object>> projection);
        new IOrderedInEGremlinQuery<TEdge, TAdjacentVertex> OrderBy(Func<IInEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> traversal);
        new IOrderedInEGremlinQuery<TEdge, TAdjacentVertex> OrderBy(string lambda);
        new IOrderedInEGremlinQuery<TEdge, TAdjacentVertex> OrderByDescending(Expression<Func<TEdge, object>> projection);
        new IOrderedInEGremlinQuery<TEdge, TAdjacentVertex> OrderByDescending(Func<IInEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> traversal);
    }
    
    public partial interface IOutEGremlinQuery<TEdge, TAdjacentVertex>
    {
        new IOrderedOutEGremlinQuery<TEdge, TAdjacentVertex> OrderBy(Expression<Func<TEdge, object>> projection);
        new IOrderedOutEGremlinQuery<TEdge, TAdjacentVertex> OrderBy(Func<IOutEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> traversal);
        new IOrderedOutEGremlinQuery<TEdge, TAdjacentVertex> OrderBy(string lambda);
        new IOrderedOutEGremlinQuery<TEdge, TAdjacentVertex> OrderByDescending(Expression<Func<TEdge, object>> projection);
        new IOrderedOutEGremlinQuery<TEdge, TAdjacentVertex> OrderByDescending(Func<IOutEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> traversal);
    }
    
    public partial interface IVPropertiesGremlinQuery<TValue>
    {
        new IOrderedVPropertiesGremlinQuery<TValue> OrderBy(Expression<Func<TValue, object>> projection);
        new IOrderedVPropertiesGremlinQuery<TValue> OrderBy(Func<IVPropertiesGremlinQuery<TValue>, IGremlinQuery> traversal);
        new IOrderedVPropertiesGremlinQuery<TValue> OrderBy(string lambda);
        new IOrderedVPropertiesGremlinQuery<TValue> OrderByDescending(Expression<Func<TValue, object>> projection);
        new IOrderedVPropertiesGremlinQuery<TValue> OrderByDescending(Func<IVPropertiesGremlinQuery<TValue>, IGremlinQuery> traversal);
    }
    
    public partial interface IVPropertiesGremlinQuery<TValue, TMeta>
    {
        new IOrderedVPropertiesGremlinQuery<TValue, TMeta> OrderBy(Expression<Func<TValue, object>> projection);
        new IOrderedVPropertiesGremlinQuery<TValue, TMeta> OrderBy(Func<IVPropertiesGremlinQuery<TValue, TMeta>, IGremlinQuery> traversal);
        new IOrderedVPropertiesGremlinQuery<TValue, TMeta> OrderBy(string lambda);
        new IOrderedVPropertiesGremlinQuery<TValue, TMeta> OrderByDescending(Expression<Func<TValue, object>> projection);
        new IOrderedVPropertiesGremlinQuery<TValue, TMeta> OrderByDescending(Func<IVPropertiesGremlinQuery<TValue, TMeta>, IGremlinQuery> traversal);
    }
    
    public partial interface IEPropertiesGremlinQuery<TValue>
    {
        new IOrderedEPropertiesGremlinQuery<TValue> OrderBy(Expression<Func<TValue, object>> projection);
        new IOrderedEPropertiesGremlinQuery<TValue> OrderBy(Func<IEPropertiesGremlinQuery<TValue>, IGremlinQuery> traversal);
        new IOrderedEPropertiesGremlinQuery<TValue> OrderBy(string lambda);
        new IOrderedEPropertiesGremlinQuery<TValue> OrderByDescending(Expression<Func<TValue, object>> projection);
        new IOrderedEPropertiesGremlinQuery<TValue> OrderByDescending(Func<IEPropertiesGremlinQuery<TValue>, IGremlinQuery> traversal);
    }
    
    public partial interface IGremlinQuery
    {

    }

    public partial interface IElementGremlinQuery
    {

    }

    public partial interface IVGremlinQuery
    {

    }

    public partial interface IEGremlinQuery
    {

    }

    public partial interface IGremlinQuery<TElement>
    {

    }

    public partial interface IElementGremlinQuery<TElement>
    {

    }

    public partial interface IVGremlinQuery<TVertex>
    {

    }

    public partial interface IEGremlinQuery<TEdge>
    {

    }

    public partial interface IEGremlinQuery<TEdge, TAdjacentVertex>
    {

    }

    public partial interface IEGremlinQuery<TEdge, TOutVertex, TInVertex>
    {

    }

    public partial interface IInEGremlinQuery<TEdge, TAdjacentVertex>
    {

    }

    public partial interface IOutEGremlinQuery<TEdge, TAdjacentVertex>
    {

    }

    public partial interface IVPropertiesGremlinQuery<TValue>
    {

    }

    public partial interface IVPropertiesGremlinQuery<TValue, TMeta>
    {

    }

    public partial interface IEPropertiesGremlinQuery<TValue>
    {

    }

    public partial interface IOrderedGremlinQuery
    {

    }

    public partial interface IOrderedElementGremlinQuery
    {

    }

    public partial interface IOrderedVGremlinQuery
    {

    }

    public partial interface IOrderedEGremlinQuery
    {

    }

    public partial interface IOrderedGremlinQuery<TElement>
    {

    }

    public partial interface IOrderedElementGremlinQuery<TElement>
    {

    }

    public partial interface IOrderedVGremlinQuery<TVertex>
    {

    }

    public partial interface IOrderedEGremlinQuery<TEdge>
    {

    }

    public partial interface IOrderedEGremlinQuery<TEdge, TAdjacentVertex>
    {

    }

    public partial interface IOrderedEGremlinQuery<TEdge, TOutVertex, TInVertex>
    {

    }

    public partial interface IOrderedInEGremlinQuery<TEdge, TAdjacentVertex>
    {

    }

    public partial interface IOrderedOutEGremlinQuery<TEdge, TAdjacentVertex>
    {

    }

    public partial interface IOrderedVPropertiesGremlinQuery<TValue>
    {

    }

    public partial interface IOrderedVPropertiesGremlinQuery<TValue, TMeta>
    {

    }

    public partial interface IOrderedEPropertiesGremlinQuery<TValue>
    {

    }

}
#pragma warning restore CS0109 // Member does not hide an inherited member; new keyword is not required




