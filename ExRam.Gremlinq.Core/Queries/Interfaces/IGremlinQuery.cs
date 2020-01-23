using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public partial interface IGremlinQueryBase : IStartGremlinQuery
    {
        TaskAwaiter GetAwaiter();

        IGremlinQueryAdmin AsAdmin();
        IValueGremlinQuery<long> Count();
        IValueGremlinQuery<long> CountLocal();
        IValueGremlinQuery<TValue> Constant<TValue>(TValue constant);

        IValueGremlinQuery<object> Drop();

        IValueGremlinQuery<string> Explain();

        IGremlinQuery<object> Lower();

        IValueGremlinQuery<string> Profile();

        IValueGremlinQuery<TStepElement> Select<TStepElement>(StepLabel<TStepElement> label);
        TQuery Select<TQuery, TElement>(StepLabel<TQuery, TElement> label) where TQuery : IGremlinQueryBase;
    }

    public partial interface IGremlinQueryBase<TElement> : IGremlinQueryBase
    {
        new GremlinQueryAwaiter<TElement> GetAwaiter();

        new IGremlinQuery<TElement> Lower();

        IAsyncEnumerable<TElement> ToAsyncEnumerable();
    }

    public interface IGremlinQueryBaseRec<TSelf> : IGremlinQueryBase
        where TSelf : IGremlinQueryBaseRec<TSelf>
    {
        TSelf And(params Func<TSelf, IGremlinQueryBase>[] andTraversals);

        TSelf Coin(double probability);

        TSelf Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<TSelf, IGremlinQueryBase> traversalPredicate, Func<TSelf, TTargetQuery> trueChoice, Func<TSelf, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQueryBase;
        TTargetQuery Choose<TTargetQuery>(Func<TSelf, IGremlinQueryBase> traversalPredicate, Func<TSelf, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQueryBase;

        TTargetQuery Choose<TTargetQuery>(Func<IChooseBuilder<TSelf>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) where TTargetQuery : IGremlinQueryBase;

        TTargetQuery Coalesce<TTargetQuery>(params Func<TSelf, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQueryBase;

        TSelf Dedup();

        TSelf Emit();

        TTargetQuery FlatMap<TTargetQuery>(Func<TSelf, TTargetQuery> mapping) where TTargetQuery : IGremlinQueryBase;

        IValueGremlinQuery<IDictionary<TNewKey, TNewValue>> Group<TNewKey, TNewValue>(Func<IGroupBuilder<TSelf>, IGroupBuilderWithKeyAndValue<TSelf, TNewKey, TNewValue>> groupBuilder);
        IValueGremlinQuery<IDictionary<TNewKey, object>> Group<TNewKey>(Func<IGroupBuilder<TSelf>, IGroupBuilderWithKey<TSelf, TNewKey>> groupBuilder);

        TSelf Identity();

        TSelf Limit(long count);
        TSelf LimitLocal(long count);
        TTargetQuery Local<TTargetQuery>(Func<TSelf, TTargetQuery> localTraversal) where TTargetQuery : IGremlinQueryBase;

        TTargetQuery Map<TTargetQuery>(Func<TSelf, TTargetQuery> mapping) where TTargetQuery : IGremlinQueryBase;

        TSelf Not(Func<TSelf, IGremlinQueryBase> notTraversal);
        TSelf None();

        TSelf Optional(Func<TSelf, TSelf> optionalTraversal);
        TSelf Or(params Func<TSelf, IGremlinQueryBase>[] orTraversals);

        TSelf Range(long low, long high);

        TSelf Repeat(Func<TSelf, TSelf> repeatTraversal);
        TSelf RepeatUntil(Func<TSelf, TSelf> repeatTraversal, Func<TSelf, IGremlinQueryBase> untilTraversal);
        TSelf UntilRepeat(Func<TSelf, TSelf> repeatTraversal, Func<TSelf, IGremlinQueryBase> untilTraversal);

        TSelf SideEffect(Func<TSelf, IGremlinQueryBase> sideEffectTraversal);
        TSelf Skip(long count);

        TSelf Tail(long count);
        TSelf TailLocal(int count);

        TSelf Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<TSelf, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQueryBase;

        TSelf Where(ILambda lambda);
        TSelf Where(Func<TSelf, IGremlinQueryBase> filterTraversal);
    }

    public interface IGremlinQueryBaseRec<TElement, TSelf> :
        IGremlinQueryBaseRec<TSelf>,
        IGremlinQueryBase<TElement>
        where TSelf : IGremlinQueryBaseRec<TElement, TSelf>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<TSelf, StepLabel<TSelf, TElement>, TTargetQuery> continuation) where TTargetQuery : IGremlinQueryBase;
        TSelf As<TTargetQuery>(StepLabel<TSelf, TElement> stepLabel);
        TTargetQuery As<TTargetQuery>(Func<TSelf, StepLabel<TSelf, TElement>, TTargetQuery> continuation) where TTargetQuery : IGremlinQueryBase;

        IArrayGremlinQuery<TElement[], TSelf> Fold();

        TSelf Inject(params TElement[] elements);

        IValueGremlinQuery<dynamic> Project(Func<IProjectBuilder<TSelf, TElement>, IProjectResult> continuation);
        IValueGremlinQuery<TResult> Project<TResult>(Func<IProjectBuilder<TSelf, TElement>, IProjectResult<TResult>> continuation);
    }

    public interface IGremlinQuery<TElement> : IGremlinQueryBaseRec<TElement, IGremlinQuery<TElement>>
    {
    }
}
