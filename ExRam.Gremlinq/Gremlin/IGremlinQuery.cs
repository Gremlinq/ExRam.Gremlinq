using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq.Expressions;
using LanguageExt;

namespace ExRam.Gremlinq
{
    public interface IGremlinQuery : IGroovySerializable
    {
        IEGremlinQuery<TEdge> AddE<TEdge>(TEdge edge);
        IEGremlinQuery<TEdge> AddE<TEdge>() where TEdge : new();
        IVGremlinQuery<TVertex> AddV<TVertex>(TVertex vertex);
        IVGremlinQuery<TVertex> AddV<TVertex>() where TVertex : new();

        IEGremlinQuery<Edge> E(params object[] ids);
        IVGremlinQuery<Vertex> V(params object[] ids);

        IGremlinQuery<TElement> Cast<TElement>();
        IGremlinQuery<Unit> Drop();

        IGremlinQuery<object> Id();
        
        IGremlinQuery<TElement> InsertStep<TElement>(int index, GremlinStep step);
        
        IGremlinQuery<TStep> Select<TStep>(StepLabel<TStep> label);
        IGremlinQuery<(T1, T2)> Select<T1, T2>(StepLabel<T1> label1, StepLabel<T2> label2);
        IGremlinQuery<(T1, T2, T3)> Select<T1, T2, T3>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3);
        IGremlinQuery<(T1, T2, T3, T4)> Select<T1, T2, T3, T4>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4);
        IGremlinQuery<TTarget> OfType<TTarget>();

        IGremlinQuery<string> Explain();
        IGremlinQuery<string> Profile();

        IImmutableList<GremlinStep> Steps { get; }
        IImmutableDictionary<StepLabel, string> StepLabelMappings { get; }
    }

    public interface IGremlinQuery<TElement> : IGremlinQuery
    {
        IGremlinQuery<TElement> And(params Func<IGremlinQuery<TElement>, IGremlinQuery>[] andTraversals);
        TTargetQuery As<TTargetQuery>(Func<IGremlinQuery<TElement>, StepLabel<TElement>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        IGremlinQuery<TElement> As(StepLabel<TElement> stepLabel);
        IGremlinQuery<TElement> Barrier();
        TTargetQuery Coalesce<TTargetQuery>(params Func<IGremlinQuery<TElement>, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;
        IGremlinQuery<TResult> Choose<TResult>(Func<IGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IGremlinQuery<TElement>, IGremlinQuery<TResult>> trueChoice, Func<IGremlinQuery<TElement>, IGremlinQuery<TResult>> falseChoice);
        IGremlinQuery<TResult> Choose<TResult>(Func<IGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IGremlinQuery<TElement>, IGremlinQuery<TResult>> trueChoice);
        IGremlinQuery<TElement> Dedup();
        IGremlinQuery<TElement> Emit();
        IGremlinQuery<TElement> FilterWithLambda(string lambda);
        IGremlinQuery<TElement[]> Fold();
        IGremlinQuery<TElement> Identity();
        IGremlinQuery<TElement> Inject(params TElement[] elements);
        IGremlinQuery<TElement> Limit(long limit);
        IGremlinQuery<TTarget> Local<TTarget>(Func<IGremlinQuery<TElement>, IGremlinQuery<TTarget>> localTraversal);
        IGremlinQuery<TTarget> Map<TTarget>(Func<IGremlinQuery<TElement>, IGremlinQuery<TTarget>> mapping);
        IGremlinQuery<TElement> Match(params Func<IGremlinQuery<TElement>, IGremlinQuery<TElement>>[] matchTraversals);
        IGremlinQuery<TElement> Not(Func<IGremlinQuery<TElement>, IGremlinQuery> notTraversal);
        IGremlinQuery<TElement> Or(params Func<IGremlinQuery<TElement>, IGremlinQuery>[] orTraversals);
        IGremlinQuery<TElement> OrderBy(Expression<Func<TElement, object>> projection);
        IGremlinQuery<TElement> OrderBy(Func<IGremlinQuery<TElement>, IGremlinQuery> traversal);
        IGremlinQuery<TElement> OrderBy(string lambda);
        IGremlinQuery<TElement> OrderByDescending(Expression<Func<TElement, object>> projection);
        IGremlinQuery<TElement> OrderByDescending(Func<IGremlinQuery<TElement>, IGremlinQuery> traversal);
        IGremlinQuery<TElement> Property(string key, object value);
        IGremlinQuery<TElement> Range(long low, long high);
        IGremlinQuery<TElement> SideEffect(Func<IGremlinQuery<TElement>, IGremlinQuery> sideEffectTraversal);
        IGremlinQuery<TElement> Skip(long skip);
        IGremlinQuery<TElement> Sum(Scope scope);
        IGremlinQuery<TElement> Times(int count);
        IGremlinQuery<TElement> Tail(long limit);
        IGremlinQuery<TElement> Unfold(IGremlinQuery<IEnumerable<TElement>> query);
        IGremlinQuery<TElement> Until(Func<IGremlinQuery<TElement>, IGremlinQuery> untilTraversal);
        IGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TElement, TTarget>>[] projections);

        IGremlinQuery<TElement> Where(Expression<Func<TElement, bool>> predicate);
        IGremlinQuery<TElement> Where(Func<IGremlinQuery<TElement>, IGremlinQuery> filterTraversal);
    }

    public interface IVGremlinQuery<TVertex> : IGremlinQuery<TVertex>
    {
        new IEGremlinQuery<TEdge, TVertex> AddE<TEdge>(TEdge edge);
        new IEGremlinQuery<TEdge, TVertex> AddE<TEdge>() where TEdge : new();
        IVGremlinQuery<TVertex> And(params Func<IVGremlinQuery<TVertex>, IGremlinQuery>[] andTraversals);
        TTargetQuery As<TTargetQuery>(Func<IVGremlinQuery<TVertex>, StepLabel<TVertex>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        new IVGremlinQuery<TVertex> As(StepLabel<TVertex> stepLabel);

        IVGremlinQuery<Vertex> Both<TEdge>();
        IEGremlinQuery<TEdge> BothE<TEdge>();

        new IVGremlinQuery<TOtherVertex> Cast<TOtherVertex>();
        TTargetQuery Coalesce<TTargetQuery>(params Func<IVGremlinQuery<TVertex>, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IVGremlinQuery<TVertex> Dedup();

        new IVGremlinQuery<TVertex> Emit();
        IVGremlinQuery<Vertex> In<TEdge>();
        IInEGremlinQuery<TEdge, TVertex> InE<TEdge>();

        new IVGremlinQuery<TVertex> Limit(long limit);
        IGremlinQuery<TTarget> Local<TTarget>(Func<IVGremlinQuery<TVertex>, IGremlinQuery<TTarget>> localTraversal);

        IVGremlinQuery<TVertex> Not(Func<IVGremlinQuery<TVertex>, IGremlinQuery> notTraversal);

        new IVGremlinQuery<TTarget> OfType<TTarget>();
        IVGremlinQuery<TVertex> Or(params Func<IVGremlinQuery<TVertex>, IGremlinQuery>[] orTraversals);
        new IVGremlinQuery<TVertex> OrderBy(Expression<Func<TVertex, object>> projection);
        IVGremlinQuery<TVertex> OrderBy(Func<IVGremlinQuery<TVertex>, IGremlinQuery> traversal);
        new IVGremlinQuery<TVertex> OrderBy(string lambda);
        new IVGremlinQuery<TVertex> OrderByDescending(Expression<Func<TVertex, object>> projection);
        IVGremlinQuery<TVertex> OrderByDescending(Func<IVGremlinQuery<TVertex>, IGremlinQuery> traversal);
        IVGremlinQuery<Vertex> Out<TEdge>();
        IGremlinQuery<TOther> Optional<TOther>(Func<IVGremlinQuery<TVertex>, IGremlinQuery<TOther>> optionalTraversal);
        IOutEGremlinQuery<TEdge, TVertex> OutE<TEdge>();

        new IVGremlinQuery<TVertex> Range(long low, long high);
        IVGremlinQuery<TVertex> Repeat(Func<IVGremlinQuery<TVertex>, IVGremlinQuery<TVertex>> repeatTraversal);

        IVGremlinQuery<TVertex> SideEffect(Func<IVGremlinQuery<TVertex>, IGremlinQuery> sideEffectTraversal);
        new IVGremlinQuery<TVertex> Skip(long skip);

        new IVGremlinQuery<TVertex> Tail(long limit);
        
        TTargetQuery Union<TTargetQuery>(params Func<IVGremlinQuery<TVertex>, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;

        new IVGremlinQuery<TVertex> Where(Expression<Func<TVertex, bool>> predicate);
        IVGremlinQuery<TVertex> Where(Func<IVGremlinQuery<TVertex>, IGremlinQuery> filterTraversal);
    }

    public interface IEGremlinQuery<TEdge> : IGremlinQuery<TEdge>
    {
        IVGremlinQuery<Vertex> BothV();
        new IEGremlinQuery<TOtherEdge> Cast<TOtherEdge>();

        new IEGremlinQuery<TEdge> Dedup();

        new IEGremlinQuery<TEdge> Emit();

        IOutEGremlinQuery<TEdge, TOutVertex> From<TOutVertex>(Func<IGremlinQuery<TEdge>, IGremlinQuery<TOutVertex>> fromVertexTraversal);
        IOutEGremlinQuery<TEdge, TOutVertex> From<TOutVertex>(StepLabel<TOutVertex> stepLabel);

        IVGremlinQuery<Vertex> InV();

        new IEGremlinQuery<TEdge> Limit(long limit);
        IGremlinQuery<TTarget> Local<TTarget>(Func<IEGremlinQuery<TEdge>, IGremlinQuery<TTarget>> localTraversal);

        new IEGremlinQuery<TTarget> OfType<TTarget>();
        new IEGremlinQuery<TEdge> OrderBy(Expression<Func<TEdge, object>> projection);
        IEGremlinQuery<TEdge> OrderBy(Func<IEGremlinQuery<TEdge>, IGremlinQuery> traversal);
        new IEGremlinQuery<TEdge> OrderBy(string lambda);
        new IEGremlinQuery<TEdge> OrderByDescending(Expression<Func<TEdge, object>> projection);
        IEGremlinQuery<TEdge> OrderByDescending(Func<IEGremlinQuery<TEdge>, IGremlinQuery> traversal);
        IVGremlinQuery<Vertex> OtherV();
        IVGremlinQuery<Vertex> OutV();

        new IEGremlinQuery<TEdge> Range(long low, long high);

        IEGremlinQuery<TEdge> SideEffect(Func<IEGremlinQuery<TEdge>, IGremlinQuery> sideEffectTraversal);
        new IEGremlinQuery<TEdge> Skip(long skip);

        new IEGremlinQuery<TEdge> Tail(long limit);

        IInEGremlinQuery<TEdge, TInVertex> To<TInVertex>(Func<IGremlinQuery<TEdge>, IGremlinQuery<TInVertex>> toVertexTraversal);
        IInEGremlinQuery<TEdge, TInVertex> To<TInVertex>(StepLabel<TInVertex> stepLabel);

        new IEGremlinQuery<TEdge> Where(Expression<Func<TEdge, bool>> predicate);
        IEGremlinQuery<TEdge> Where(Func<IEGremlinQuery<TEdge>, IGremlinQuery> filterTraversal);
    }

    public interface IOutEGremlinQuery<TEdge, TAdjacentVertex> : IEGremlinQuery<TEdge>
    {
        new IOutEGremlinQuery<TOtherEdge, TAdjacentVertex> Cast<TOtherEdge>();

        new IOutEGremlinQuery<TTarget, TAdjacentVertex> OfType<TTarget>();
        new IVGremlinQuery<TAdjacentVertex> OutV();

        new IOutEGremlinQuery<TEdge, TAdjacentVertex> Where(Expression<Func<TEdge, bool>> predicate);
        IOutEGremlinQuery<TEdge, TAdjacentVertex> Where(Func<IOutEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> filterTraversal);
    }

    public interface IInEGremlinQuery<TEdge, TAdjacentVertex> : IEGremlinQuery<TEdge>
    {
        new IInEGremlinQuery<TOtherEdge, TAdjacentVertex> Cast<TOtherEdge>();

        new IInEGremlinQuery<TTarget, TAdjacentVertex> OfType<TTarget>();
        new IVGremlinQuery<TAdjacentVertex> InV();

        new IInEGremlinQuery<TEdge, TAdjacentVertex> Where(Expression<Func<TEdge, bool>> predicate);
        IInEGremlinQuery<TEdge, TAdjacentVertex> Where(Func<IInEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> filterTraversal);
    }

    public interface IEGremlinQuery<TEdge, TAdjacentVertex> : IEGremlinQuery<TEdge>
    {
        new IEGremlinQuery<TOtherEdge, TAdjacentVertex> Cast<TOtherEdge>();

        new IEGremlinQuery<TEdge, TTargetVertex, TAdjacentVertex> From<TTargetVertex>(Func<IGremlinQuery<TEdge>, IGremlinQuery<TTargetVertex>> fromVertexTraversal);
        new IEGremlinQuery<TEdge, TTargetVertex, TAdjacentVertex> From<TTargetVertex>(StepLabel<TTargetVertex> stepLabel);

        new IEGremlinQuery<TTarget, TAdjacentVertex> OfType<TTarget>();

        new IEGremlinQuery<TEdge, TAdjacentVertex, TTargetVertex> To<TTargetVertex>(Func<IGremlinQuery<TEdge>, IGremlinQuery<TTargetVertex>> toVertexTraversal);
        new IEGremlinQuery<TEdge, TAdjacentVertex, TTargetVertex> To<TTargetVertex>(StepLabel<TTargetVertex> stepLabel);

        new IEGremlinQuery<TEdge, TAdjacentVertex> Where(Expression<Func<TEdge, bool>> predicate);
        IEGremlinQuery<TEdge, TAdjacentVertex> Where(Func<IEGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> filterTraversal);
    }

    public interface IEGremlinQuery<TEdge, TOutVertex, TInVertex> : IEGremlinQuery<TEdge, TOutVertex>, IOutEGremlinQuery<TEdge, TOutVertex>, IInEGremlinQuery<TEdge, TInVertex>
    {
        new IEGremlinQuery<TOtherEdge, TOutVertex, TInVertex> Cast<TOtherEdge>();

        new IVGremlinQuery<TInVertex> InV();

        new IEGremlinQuery<TTarget, TOutVertex, TInVertex> OfType<TTarget>();
        new IVGremlinQuery<TOutVertex> OutV();

        new IEGremlinQuery<TEdge, TOutVertex, TInVertex> Where(Expression<Func<TEdge, bool>> predicate);
        IEGremlinQuery<TEdge, TOutVertex, TInVertex> Where(Func<IEGremlinQuery<TEdge, TOutVertex, TInVertex>, IGremlinQuery> filterTraversal);
    }
}