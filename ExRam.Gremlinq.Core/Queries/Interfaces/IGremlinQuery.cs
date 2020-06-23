using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryBase : IStartGremlinQuery
    {
        TaskAwaiter GetAwaiter();

        IGremlinQueryAdmin AsAdmin();
        IGremlinQuery<TResult> Cast<TResult>();
        IValueGremlinQuery<long> Count();
        IValueGremlinQuery<long> CountLocal();
        IValueGremlinQuery<TValue> Constant<TValue>(TValue constant);

        IValueGremlinQuery<object> Drop();

        IValueGremlinQuery<string> Explain();

        IGremlinQuery<object> Lower();

        IValueGremlinQuery<string> Profile();

        IValueGremlinQuery<TStepElement> Select<TStepElement>(StepLabel<TStepElement> label);
        IValueGremlinQuery<(T1, T2)> Select<T1, T2>(StepLabel<T1> label1, StepLabel<T2> label2);
        IValueGremlinQuery<(T1, T2, T3)> Select<T1, T2, T3>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3);
        IValueGremlinQuery<(T1, T2, T3, T4)> Select<T1, T2, T3, T4>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4);
        IValueGremlinQuery<(T1, T2, T3, T4, T5)> Select<T1, T2, T3, T4, T5>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5);
        IValueGremlinQuery<(T1, T2, T3, T4, T5, T6)> Select<T1, T2, T3, T4, T5, T6>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6);
        IValueGremlinQuery<(T1, T2, T3, T4, T5, T6, T7)> Select<T1, T2, T3, T4, T5, T6, T7>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7);
        IValueGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8)> Select<T1, T2, T3, T4, T5, T6, T7, T8>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8);
        IValueGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9);
        IValueGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10);
        IValueGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11);
        IValueGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11, StepLabel<T12> label12);
        IValueGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11, StepLabel<T12> label12, StepLabel<T13> label13);
        IValueGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11, StepLabel<T12> label12, StepLabel<T13> label13, StepLabel<T14> label14);
        IValueGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11, StepLabel<T12> label12, StepLabel<T13> label13, StepLabel<T14> label14, StepLabel<T15> label15);
        IValueGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11, StepLabel<T12> label12, StepLabel<T13> label13, StepLabel<T14> label14, StepLabel<T15> label15, StepLabel<T16> label16);

        TQuery Select<TQuery, TElement>(StepLabel<TQuery, TElement> label) where TQuery : IGremlinQueryBase;

        IArrayGremlinQuery<TElement, TArrayItem, TOriginalQuery> Cap<TElement, TArrayItem, TOriginalQuery>(StepLabel<IArrayGremlinQuery<TElement, TArrayItem, TOriginalQuery>, TElement> label) where TOriginalQuery : IGremlinQueryBase;
    }

    public interface IGremlinQueryBase<TElement> : IGremlinQueryBase
    {
        new GremlinQueryAwaiter<TElement> GetAwaiter();

        IElementGremlinQuery<TElement> ForceElement();

        IVertexGremlinQuery<TElement> ForceVertex();
        IVertexPropertyGremlinQuery<TElement, TValue> ForceVertexProperty<TValue>();
        IVertexPropertyGremlinQuery<TElement, TValue, TMeta> ForceVertexProperty<TValue, TMeta>() where TMeta : class;

        IPropertyGremlinQuery<TElement> ForceProperty();

        IEdgeGremlinQuery<TElement> ForceEdge();
        IInEdgeGremlinQuery<TElement, TInVertex> ForceInEdge<TInVertex>();
        IOutEdgeGremlinQuery<TElement, TOutVertex> ForceOutEdge<TOutVertex>();
        IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex> ForceBothEdge<TOutVertex, TInVertex>();

        IValueGremlinQuery<TElement> ForceValue();

        IArrayGremlinQuery<TElement[], TElement, IGremlinQueryBase<TElement>> ForceArray();

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
        TSelf DedupLocal();

        TSelf Emit();

        TTargetQuery FlatMap<TTargetQuery>(Func<TSelf, TTargetQuery> mapping) where TTargetQuery : IGremlinQueryBase;

        IValueGremlinQuery<IDictionary<TNewKey, TNewValue>> Group<TNewKey, TNewValue>(Func<IGroupBuilder<TSelf>, IGroupBuilderWithKeyAndValue<TSelf, TNewKey, TNewValue>> groupBuilder);
        IValueGremlinQuery<IDictionary<TNewKey, object>> Group<TNewKey>(Func<IGroupBuilder<TSelf>, IGroupBuilderWithKey<TSelf, TNewKey>> groupBuilder);

        TSelf Identity();

        TSelf Limit(long count);
        TSelf LimitLocal(long count);
        TTargetQuery Local<TTargetQuery>(Func<TSelf, TTargetQuery> localTraversal) where TTargetQuery : IGremlinQueryBase;

        TTargetQuery Map<TTargetQuery>(Func<TSelf, TTargetQuery> mapping) where TTargetQuery : IGremlinQueryBase;

        TSelf Mute();

        TSelf Not(Func<TSelf, IGremlinQueryBase> notTraversal);
        TSelf None();

        TSelf Optional(Func<TSelf, TSelf> optionalTraversal);
        TSelf Or(params Func<TSelf, IGremlinQueryBase>[] orTraversals);

        TSelf Range(long low, long high);
        TSelf RangeLocal(long low, long high);

        TSelf Repeat(Func<TSelf, TSelf> repeatTraversal);
        TSelf RepeatUntil(Func<TSelf, TSelf> repeatTraversal, Func<TSelf, IGremlinQueryBase> untilTraversal);
        TSelf UntilRepeat(Func<TSelf, TSelf> repeatTraversal, Func<TSelf, IGremlinQueryBase> untilTraversal);

        TSelf SideEffect(Func<TSelf, IGremlinQueryBase> sideEffectTraversal);

        TSelf Skip(long count);
        TSelf SkipLocal(long count);

        TSelf Tail(long count);
        TSelf TailLocal(long count);

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
        TTargetQuery Aggregate<TTargetQuery>(Func<TSelf, StepLabel<IArrayGremlinQuery<TElement[], TElement, TSelf>, TElement[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQueryBase;
        TTargetQuery AggregateGlobal<TTargetQuery>(Func<TSelf, StepLabel<IArrayGremlinQuery<TElement[], TElement, TSelf>, TElement[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQueryBase;

        TSelf As(StepLabel<TElement> stepLabel);
        TTargetQuery As<TTargetQuery>(Func<TSelf, StepLabel<TSelf, TElement>, TTargetQuery> continuation) where TTargetQuery : IGremlinQueryBase;

        IArrayGremlinQuery<TElement[], TElement, TSelf> Fold();

        new IArrayGremlinQuery<TElement[], TElement, TSelf> ForceArray();

        TSelf Inject(params TElement[] elements);

        IValueGremlinQuery<dynamic> Project(Func<IProjectBuilder<TSelf, TElement>, IProjectResult> continuation);
        IValueGremlinQuery<TResult> Project<TResult>(Func<IProjectBuilder<TSelf, TElement>, IProjectResult<TResult>> continuation);
    }

    public interface IGremlinQuery<TElement> : IGremlinQueryBaseRec<TElement, IGremlinQuery<TElement>>
    {
    }
}
