using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Reactive;

namespace ExRam.Gremlinq
{
    public interface IGremlinQuery : IGremlinSerializable
    {
        string TraversalSourceName { get; }
        IImmutableList<GremlinStep> Steps { get; }
        IImmutableDictionary<StepLabel, string> StepLabelMappings { get; }
    }

    public interface IGremlinQuery<TElement> : IGremlinQuery
    {
        IGremlinQuery<TVertex> AddV<TVertex>(TVertex vertex);

        IGremlinQuery<TVertex> AddV<TVertex>()
            where TVertex : new();

        IGremlinQuery<TEdge> AddE<TEdge>()
            where TEdge : new();

        IGremlinQuery<TEdge> AddE<TEdge>(TEdge edge);
        IGremlinQuery<TElement> And(params Func<IGremlinQuery<TElement>, IGremlinQuery>[] andTraversals);
        IGremlinQuery<TElement> And(IEnumerable<IGremlinQuery> andTraversals);
        IGremlinQuery<TTarget> As<TTarget>(Func<IGremlinQuery<TElement>, StepLabel<TElement>, IGremlinQuery<TTarget>> continuation);
        IGremlinQuery<TElement> As(StepLabel<TElement> stepLabel);
        IGremlinQuery<TElement> Barrier();
        IGremlinQuery<TElement> Coalesce(params Func<IGremlinQuery<Unit>, IGremlinQuery<TElement>>[] traversals);
        IGremlinQuery<TTarget> Coalesce<TTarget>(params Func<IGremlinQuery<TElement>, IGremlinQuery<TTarget>>[] traversals);
        IGremlinQuery<TResult> Choose<TResult>(Func<IGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IGremlinQuery<TElement>, IGremlinQuery<TResult>> trueChoice, Func<IGremlinQuery<TElement>, IGremlinQuery<TResult>> falseChoice);
        IGremlinQuery<TResult> Choose<TResult>(Func<IGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IGremlinQuery<TElement>, IGremlinQuery<TResult>> trueChoice);
        IGremlinQuery<Vertex> Both();
        IGremlinQuery<TElement> BothE();
        IGremlinQuery<Vertex> BothV();
        IGremlinQuery<TEnd> BranchOnIdentity<TEnd>(params Func<IGremlinQuery<TElement>, IGremlinQuery<TEnd>>[] options);
        IGremlinQuery<TEnd> Branch<TBranch, TEnd>(Func<IGremlinQuery<TElement>, IGremlinQuery<TBranch>> branchSelector, params Func<IGremlinQuery<TBranch>, IGremlinQuery<TEnd>>[] options);
        IGremlinQuery<TElement> ByTraversal(Func<IGremlinQuery<TElement>, IGremlinQuery> traversal, GremlinSortOrder sortOrder = GremlinSortOrder.Increasing);
        IGremlinQuery<TElement> ByMember(Expression<Func<TElement, object>> projection, GremlinSortOrder sortOrder = GremlinSortOrder.Increasing);
        IGremlinQuery<TElement> ByLambda(string lambdaString);
        IGremlinQuery<TElement> Choose(IGremlinQuery traversalPredicate, IGremlinQuery<TElement> trueChoice, IGremlinQuery<TElement> falseChoice);
        IGremlinQuery<TElement> Choose(IGremlinQuery traversalPredicate, IGremlinQuery<TElement> trueChoice);
        IGremlinQuery<TElement> Dedup();
        IGremlinQuery<Unit> Drop();
        IGremlinQuery<Edge> E(params object[] ids);
        IGremlinQuery<TEdge> E<TEdge>(params object[] ids);
        IGremlinQuery<TElement> Emit();
        IGremlinQuery<TElement> Explain();
        IGremlinQuery<TElement> FilterWithLambda(string lambda);
        IGremlinQuery<TElement[]> Fold();
        IGremlinQuery<TElement> From<TStepLabel>(StepLabel<TStepLabel> stepLabel);
        IGremlinQuery<TElement> From(Func<IGremlinQuery<TElement>, IGremlinQuery> fromVertex);
        IGremlinQuery<object> Id();
        IGremlinQuery<TElement> Identity();
        IGremlinQuery<Vertex> In<TEdge>();
        IGremlinQuery<TEdge> InE<TEdge>();
        IGremlinQuery<TVertex> InV<TVertex>();
        IGremlinQuery<TElement> Inject(params TElement[] elements);
        IGremlinQuery<TElement> Limit(long limit);
        IGremlinQuery<TTarget> Local<TTarget>(Func<IGremlinQuery<TElement>, IGremlinQuery<TTarget>> localTraversal);
        IGremlinQuery<TTarget> Map<TTarget>(Func<IGremlinQuery<TElement>, IGremlinQuery<TTarget>> mapping);
        IGremlinQuery<TElement> Match(params Func<IGremlinQuery<TElement>, IGremlinQuery<TElement>>[] matchTraversals);
        IGremlinQuery<TElement> Not(Func<IGremlinQuery<TElement>, IGremlinQuery> notTraversal);
        IGremlinQuery<TTarget> OfType<TTarget>();
        IGremlinQuery<TElement> Optional(Func<IGremlinQuery<TElement>, IGremlinQuery<TElement>> optionalTraversal);
        IGremlinQuery<TElement> Or(params Func<IGremlinQuery<TElement>, IGremlinQuery>[] orTraversals);
        IGremlinQuery<TElement> Or(IEnumerable<IGremlinQuery> orTraversals);
        IGremlinQuery<TElement> Order();
        IGremlinQuery<TElement> OtherV();
        IGremlinQuery<TEdge> OutE<TEdge>();
        IGremlinQuery<TVertex> OutV<TVertex>();
        IGremlinQuery<Vertex> Out<TEdge>();
        IGremlinQuery<TElement> Profile();
        IGremlinQuery<TElement> Property<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression, TProperty property);
        IGremlinQuery<TElement> Property(string key, object value);
        IGremlinQuery<TElement> Range(long low, long high);
        IGremlinQuery<TElement> Repeat(Func<IGremlinQuery<TElement>, IGremlinQuery<TElement>> repeatTraversal);
        IGremlinQuery<TStep> Select<TStep>(StepLabel<TStep> label);
        IGremlinQuery<(T1, T2)> Select<T1, T2>(StepLabel<T1> label1, StepLabel<T2> label2);
        IGremlinQuery<(T1, T2, T3)> Select<T1, T2, T3>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3);
        IGremlinQuery<(T1, T2, T3, T4)> Select<T1, T2, T3, T4>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4);
        IGremlinQuery<TElement> SideEffect(Func<IGremlinQuery<TElement>, IGremlinQuery> sideEffectTraversal);
        IGremlinQuery<TElement> Skip(long skip);
        IGremlinQuery<TElement> Sum(bool local = false);
        IGremlinQuery<TElement> Times(int count);
        IGremlinQuery<TElement> Tail(long limit);
        IGremlinQuery<TElement> To<TStepLabel>(StepLabel<TStepLabel> stepLabel);
        IGremlinQuery<TElement> To(Func<IGremlinQuery<TElement>, IGremlinQuery> toVertex);
        IGremlinQuery<TElement> Unfold(IGremlinQuery<IEnumerable<TElement>> query);
        IGremlinQuery<TTarget> Union<TTarget>(params Func<IGremlinQuery<TElement>, IGremlinQuery<TTarget>>[] unionTraversals);
        IGremlinQuery<TElement> Until(Func<IGremlinQuery<TElement>, IGremlinQuery> untilTraversal);
        IGremlinQuery<Vertex> V(params object[] ids);
        IGremlinQuery<TVertex> V<TVertex>(params object[] ids);
        IGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TElement, TTarget>>[] projections);
        IGremlinQuery<TElement> Where(Func<IGremlinQuery<TElement>, IGremlinQuery> filterTraversal);
        IGremlinQuery<TElement> Where(Expression<Func<TElement, bool>> predicate);
    }

    // ReSharper disable once UnusedTypeParameter
    public interface IGremlinQuery<TEdge, TAdjacentVertex> : IGremlinQuery<TEdge>
    {

    }

    // ReSharper disable UnusedTypeParameter
    public interface IGremlinQuery<TOutVertex, TEdge, TInVertex> : IGremlinQuery<TEdge>
    // ReSharper restore UnusedTypeParameter
    {

    }
}