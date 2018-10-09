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

        IImmutableList<GremlinStep> Steps { get; }
        IImmutableDictionary<StepLabel, string> StepLabelMappings { get; }
    }

    public interface IGremlinQuery<TElement> : IGremlinQuery
    {
        IGremlinQuery<TElement> And(params Func<IGremlinQuery<TElement>, IGremlinQuery>[] andTraversals);
        IGremlinQuery<TTarget> As<TTarget>(Func<IGremlinQuery<TElement>, StepLabel<TElement>, IGremlinQuery<TTarget>> continuation);
        IGremlinQuery<TElement> As(StepLabel<TElement> stepLabel);
        IGremlinQuery<TElement> Barrier();
        //IGremlinQuery<TTarget> BranchOnIdentity<TTarget>(params Func<IGremlinQuery<TElement>, IGremlinQuery<TTarget>>[] options);
        //IGremlinQuery<TTarget> Branch<TBranch, TTarget>(Func<IGremlinQuery<TElement>, IGremlinQuery<TBranch>> branchSelector, params Func<IGremlinQuery<TElement>, IGremlinQuery<TTarget>>[] options);
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
        IGremlinQuery<TElement> Identity();
        IGremlinQuery<TElement> Inject(params TElement[] elements);
        IGremlinQuery<TElement> Limit(long limit);
        IGremlinQuery<TTarget> Local<TTarget>(Func<IGremlinQuery<TElement>, IGremlinQuery<TTarget>> localTraversal);
        IGremlinQuery<TTarget> Map<TTarget>(Func<IGremlinQuery<TElement>, IGremlinQuery<TTarget>> mapping);
        IGremlinQuery<TElement> Match(params Func<IGremlinQuery<TElement>, IGremlinQuery<TElement>>[] matchTraversals);
        IGremlinQuery<TElement> Not(Func<IGremlinQuery<TElement>, IGremlinQuery> notTraversal);
        IGremlinQuery<TElement> Or(params Func<IGremlinQuery<TElement>, IGremlinQuery>[] orTraversals);
        IGremlinQuery<TElement> Order();
        IGremlinQuery<string> Profile();
        IGremlinQuery<TElement> Property<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression, TProperty property);
        IGremlinQuery<TElement> Property(string key, object value);
        IGremlinQuery<TElement> Range(long low, long high);
        IGremlinQuery<TElement> SideEffect(Func<IGremlinQuery<TElement>, IGremlinQuery> sideEffectTraversal);
        IGremlinQuery<TElement> Skip(long skip);
        IGremlinQuery<TElement> Sum(Scope scope);
        IGremlinQuery<TElement> Times(int count);
        IGremlinQuery<TElement> Tail(long limit);
        IGremlinQuery<TElement> To<TStepLabel>(StepLabel<TStepLabel> stepLabel);
        IGremlinQuery<TElement> Unfold(IGremlinQuery<IEnumerable<TElement>> query);
        IGremlinQuery<TElement> Until(Func<IGremlinQuery<TElement>, IGremlinQuery> untilTraversal);
        IGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TElement, TTarget>>[] projections);
        IGremlinQuery<TElement> Where(Func<IGremlinQuery<TElement>, IGremlinQuery> filterTraversal);
    }

    public interface IVGremlinQuery<TVertex> : IGremlinQuery<TVertex>
    {
        new IEGremlinQuery<TEdge, TVertex> AddE<TEdge>(TEdge edge);
        new IEGremlinQuery<TEdge, TVertex> AddE<TEdge>() where TEdge : new();

        IVGremlinQuery<TVertex> And(params Func<IVGremlinQuery<TVertex>, IGremlinQuery>[] andTraversals);
        IVGremlinQuery<Vertex> Both<TEdge>();
        IEGremlinQuery<TEdge> BothE<TEdge>();
        new IVGremlinQuery<TOtherVertex> Cast<TOtherVertex>();
        IVGremlinQuery<Vertex> In<TEdge>();
        IEGremlinQuery<TEdge> InE<TEdge>();
        IVGremlinQuery<TVertex> Not(Func<IVGremlinQuery<TVertex>, IGremlinQuery> notTraversal);
        new IVGremlinQuery<TTarget> OfType<TTarget>();
        IVGremlinQuery<Vertex> Out<TEdge>();
        IGremlinQuery<TOther> Optional<TOther>(Func<IVGremlinQuery<TVertex>, IGremlinQuery<TOther>> optionalTraversal);
        IEGremlinQuery<TEdge> OutE<TEdge>();
        IVGremlinQuery<TVertex> Repeat(Func<IVGremlinQuery<TVertex>, IVGremlinQuery<TVertex>> repeatTraversal);
        IGremlinQuery<TTarget> Union<TTarget>(params Func<IVGremlinQuery<TVertex>, IGremlinQuery<TTarget>>[] unionTraversals);

        //IGremlinQuery<TTarget> Branch<TBranch, TTarget>(Func<IVGremlinQuery<TVertex>, IGremlinQuery<TBranch>> branchSelector, params Func<IVGremlinQuery<TVertex>, IGremlinQuery<TTarget>>[] options);
    }

    public interface IEGremlinQuery<TEdge> : IGremlinQuery<TEdge>
    {
        IVGremlinQuery<Vertex> BothV();
        IVGremlinQuery<Vertex> InV();
        IVGremlinQuery<Vertex> OutV();
        IVGremlinQuery<Vertex> OtherV();

        new IEGremlinQuery<TOtherEdge> Cast<TOtherEdge>();
        new IEGremlinQuery<TTarget> OfType<TTarget>();
    }

    public interface IEGremlinQuery<TEdge, TAdjacentVertex> : IEGremlinQuery<TEdge>
    {
        new IEGremlinQuery<TOtherEdge, TAdjacentVertex> Cast<TOtherEdge>();
        new IEGremlinQuery<TTarget, TAdjacentVertex> OfType<TTarget>();

        IEGremlinQuery<TEdge, TAdjacentVertex, TTargetVertex> To<TTargetVertex>(Func<IGremlinQuery<TEdge>, IGremlinQuery<TTargetVertex>> toVertexTraversal);
        IEGremlinQuery<TEdge, TTargetVertex, TAdjacentVertex> From<TTargetVertex>(Func<IGremlinQuery<TEdge>, IGremlinQuery<TTargetVertex>> fromVertexTraversal);
        
        //new IEGremlinQuery<TAdjacentVertex, TTarget> Cast<TTarget>();
        //IEGremlinQuery<TTarget, TEdge> CastAdjacentVertex<TTarget>();
        //new IEGremlinQuery<TAdjacentVertex, TElement> InsertStep<TElement>(int index, GremlinStep step);
    }

    public interface IEGremlinQuery<TEdge, TOutVertex, TInVertex> : IEGremlinQuery<TEdge>
    {
        new IEGremlinQuery<TOtherEdge, TOutVertex, TInVertex> Cast<TOtherEdge>();
        new IEGremlinQuery<TTarget, TOutVertex, TInVertex> OfType<TTarget>();

        new IVGremlinQuery<TInVertex> InV();
        new IVGremlinQuery<TOutVertex> OutV();

        //new IEGremlinQuery<TOutVertex, TInVertex, TTarget> Cast<TTarget>();
        //IEGremlinQuery<TTarget, TInVertex, TEdge> CastOutVertex<TTarget>();
        //IEGremlinQuery<TOutVertex, TTarget, TEdge> CastInVertex<TTarget>();
        //new IEGremlinQuery<TOutVertex, TInVertex, TElement> InsertStep<TElement>(int index, GremlinStep step);
    }
}