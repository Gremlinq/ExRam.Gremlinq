using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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
}
