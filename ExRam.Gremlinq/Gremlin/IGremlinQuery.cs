using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq.Expressions;
using LanguageExt;

namespace ExRam.Gremlinq
{
    public interface IGremlinQuery : IGroovySerializable
    {
        IGremlinQuery<TEdge> AddE<TEdge>(TEdge edge);
        IGremlinQuery<TEdge> AddE<TEdge>() where TEdge : new();
        IGremlinQuery<TVertex> AddV<TVertex>(TVertex vertex);
        IGremlinQuery<TVertex> AddV<TVertex>() where TVertex : new();

        IGremlinQuery<TElement> Cast<TElement>();
        IGremlinQuery<Unit> Drop();
        IGremlinQuery<Edge> E(params object[] ids);
        IGremlinQuery<object> Id();
        IGremlinQuery<Vertex> In<TEdge>();
        IGremlinQuery<TElement> InsertStep<TElement>(int index, GremlinStep step);
        IGremlinQuery<TVertex> InV<TVertex>();
        IGremlinQuery<TEdge> InE<TEdge>();
        IGremlinQuery<Vertex> OtherV();
        IGremlinQuery<TEdge> OutE<TEdge>();
        IGremlinQuery<TVertex> OutV<TVertex>();
        IGremlinQuery<Vertex> Out<TEdge>();
        IGremlinQuery<TStep> Select<TStep>(StepLabel<TStep> label);
        IGremlinQuery<(T1, T2)> Select<T1, T2>(StepLabel<T1> label1, StepLabel<T2> label2);
        IGremlinQuery<(T1, T2, T3)> Select<T1, T2, T3>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3);
        IGremlinQuery<(T1, T2, T3, T4)> Select<T1, T2, T3, T4>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4);
        IGremlinQuery<TTarget> OfType<TTarget>();
        IGremlinQuery<Vertex> V(params object[] ids);

        IImmutableList<GremlinStep> Steps { get; }
        IImmutableDictionary<StepLabel, string> StepLabelMappings { get; }
    }

    public interface IGremlinQuery<TElement> : IGremlinQuery
    {
        IGremlinQuery<TElement> And(params Func<IGremlinQuery<TElement>, IGremlinQuery>[] andTraversals);
        IGremlinQuery<TTarget> As<TTarget>(Func<IGremlinQuery<TElement>, StepLabel<TElement>, IGremlinQuery<TTarget>> continuation);
        IGremlinQuery<TElement> As(StepLabel<TElement> stepLabel);
        IGremlinQuery<TElement> Barrier();
        IGremlinQuery<TTarget> BranchOnIdentity<TTarget>(params Func<IGremlinQuery<TElement>, IGremlinQuery<TTarget>>[] options);
        IGremlinQuery<TTarget> Branch<TBranch, TTarget>(Func<IGremlinQuery<TElement>, IGremlinQuery<TBranch>> branchSelector, params Func<IGremlinQuery<TBranch>, IGremlinQuery<TTarget>>[] options);
        IGremlinQuery<TElement> ByTraversal(Func<IGremlinQuery<TElement>, IGremlinQuery> traversal, Order sortOrder);
        IGremlinQuery<TElement> ByMember(Expression<Func<TElement, object>> projection, Order sortOrder);
        IGremlinQuery<TElement> ByLambda(string lambdaString);
        IGremlinQuery<TTarget> Coalesce<TTarget>(params Func<IGremlinQuery<TElement>, IGremlinQuery<TTarget>>[] traversals);
        IGremlinQuery<TResult> Choose<TResult>(Func<IGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IGremlinQuery<TElement>, IGremlinQuery<TResult>> trueChoice, Func<IGremlinQuery<TElement>, IGremlinQuery<TResult>> falseChoice);
        IGremlinQuery<TResult> Choose<TResult>(Func<IGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IGremlinQuery<TElement>, IGremlinQuery<TResult>> trueChoice);
        IGremlinQuery<TElement> Dedup();
        IGremlinQuery<TElement> Emit();
        IGremlinQuery<TElement> Explain();  //TODO: Wrong signature
        IGremlinQuery<TElement> FilterWithLambda(string lambda);
        IGremlinQuery<TElement[]> Fold();
        IGremlinQuery<TElement> From<TStepLabel>(StepLabel<TStepLabel> stepLabel);
        IGremlinQuery<TElement> From(Func<IGremlinQuery<TElement>, IGremlinQuery> fromVertex);
        IGremlinQuery<TElement> Identity();
        IGremlinQuery<TElement> Inject(params TElement[] elements);
        IGremlinQuery<TElement> Limit(long limit);
        IGremlinQuery<TTarget> Local<TTarget>(Func<IGremlinQuery<TElement>, IGremlinQuery<TTarget>> localTraversal);
        IGremlinQuery<TTarget> Map<TTarget>(Func<IGremlinQuery<TElement>, IGremlinQuery<TTarget>> mapping);
        IGremlinQuery<TElement> Match(params Func<IGremlinQuery<TElement>, IGremlinQuery<TElement>>[] matchTraversals);
        IGremlinQuery<TElement> Not(Func<IGremlinQuery<TElement>, IGremlinQuery> notTraversal);
        IGremlinQuery<TOther> Optional<TOther>(Func<IGremlinQuery<TElement>, IGremlinQuery<TOther>> optionalTraversal);
        IGremlinQuery<TElement> Or(params Func<IGremlinQuery<TElement>, IGremlinQuery>[] orTraversals);
        IGremlinQuery<TElement> Order();
        IGremlinQuery<string> Profile();
        IGremlinQuery<TElement> Property<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression, TProperty property);
        IGremlinQuery<TElement> Property(string key, object value);
        IGremlinQuery<TElement> Range(long low, long high);
        IGremlinQuery<TElement> Repeat(Func<IGremlinQuery<TElement>, IGremlinQuery<TElement>> repeatTraversal);
        IGremlinQuery<TElement> SideEffect(Func<IGremlinQuery<TElement>, IGremlinQuery> sideEffectTraversal);
        IGremlinQuery<TElement> Skip(long skip);
        IGremlinQuery<TElement> Sum(Scope scope);
        IGremlinQuery<TElement> Times(int count);
        IGremlinQuery<TElement> Tail(long limit);
        IGremlinQuery<TElement> To<TStepLabel>(StepLabel<TStepLabel> stepLabel);
        IGremlinQuery<TElement> To(Func<IGremlinQuery<TElement>, IGremlinQuery> toVertex);
        IGremlinQuery<TElement> Unfold(IGremlinQuery<IEnumerable<TElement>> query);
        IGremlinQuery<TTarget> Union<TTarget>(params Func<IGremlinQuery<TElement>, IGremlinQuery<TTarget>>[] unionTraversals);
        IGremlinQuery<TElement> Until(Func<IGremlinQuery<TElement>, IGremlinQuery> untilTraversal);
        IGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TElement, TTarget>>[] projections);
        IGremlinQuery<TElement> Where(Func<IGremlinQuery<TElement>, IGremlinQuery> filterTraversal);
    }

    public interface IVGremlinQuery<TVertex> : IGremlinQuery<TVertex>
    {
        IVGremlinQuery<Vertex> Both<TEdge>();
        IEGremlinQuery<TEdge> BothE<TEdge>();
    }

    public interface IEGremlinQuery<TEdge> : IGremlinQuery<TEdge>
    {
        IVGremlinQuery<Vertex> BothV();
    }

    public interface IEGremlinQuery<TAdjacentVertex, TEdge> : IGremlinQuery<TEdge>
    {
        //new IEGremlinQuery<TAdjacentVertex, TTarget> Cast<TTarget>();
        //IEGremlinQuery<TTarget, TEdge> CastAdjacentVertex<TTarget>();
        //new IEGremlinQuery<TAdjacentVertex, TElement> InsertStep<TElement>(int index, GremlinStep step);
    }

    public interface IEGremlinQuery<TOutVertex, TInVertex, TEdge> : IGremlinQuery<TEdge>
    {
        //new IEGremlinQuery<TOutVertex, TInVertex, TTarget> Cast<TTarget>();
        //IEGremlinQuery<TTarget, TInVertex, TEdge> CastOutVertex<TTarget>();
        //IEGremlinQuery<TOutVertex, TTarget, TEdge> CastInVertex<TTarget>();
        //new IEGremlinQuery<TOutVertex, TInVertex, TElement> InsertStep<TElement>(int index, GremlinStep step);
    }
}