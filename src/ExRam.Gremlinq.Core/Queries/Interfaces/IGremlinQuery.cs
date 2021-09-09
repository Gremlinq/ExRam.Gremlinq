using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using ExRam.Gremlinq.Core.GraphElements;
using ExRam.Gremlinq.Core.Serialization;

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

        string Debug(GroovyFormatting groovyFormatting = GroovyFormatting.Inline, bool indented = false);

        IValueGremlinQuery<object> Drop();

        IValueGremlinQuery<string> Explain();

        IGremlinQuery<object> Lower();

        IValueGremlinQuery<Path> Path();

        IValueGremlinQuery<string> Profile();

        IValueGremlinQuery<TStepElement> Select<TStepElement>(StepLabel<TStepElement> label);
        IValueTupleGremlinQuery<(T1, T2)> Select<T1, T2>(StepLabel<T1> label1, StepLabel<T2> label2);
        IValueTupleGremlinQuery<(T1, T2, T3)> Select<T1, T2, T3>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3);
        IValueTupleGremlinQuery<(T1, T2, T3, T4)> Select<T1, T2, T3, T4>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4);
        IValueTupleGremlinQuery<(T1, T2, T3, T4, T5)> Select<T1, T2, T3, T4, T5>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5);
        IValueTupleGremlinQuery<(T1, T2, T3, T4, T5, T6)> Select<T1, T2, T3, T4, T5, T6>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6);
        IValueTupleGremlinQuery<(T1, T2, T3, T4, T5, T6, T7)> Select<T1, T2, T3, T4, T5, T6, T7>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7);
        IValueTupleGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8)> Select<T1, T2, T3, T4, T5, T6, T7, T8>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8);
        IValueTupleGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9);
        IValueTupleGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10);
        IValueTupleGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11);
        IValueTupleGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11, StepLabel<T12> label12);
        IValueTupleGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11, StepLabel<T12> label12, StepLabel<T13> label13);
        IValueTupleGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11, StepLabel<T12> label12, StepLabel<T13> label13, StepLabel<T14> label14);
        IValueTupleGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11, StepLabel<T12> label12, StepLabel<T13> label13, StepLabel<T14> label14, StepLabel<T15> label15);
        IValueTupleGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11, StepLabel<T12> label12, StepLabel<T13> label13, StepLabel<T14> label14, StepLabel<T15> label15, StepLabel<T16> label16);

        TQuery Select<TQuery, TElement>(StepLabel<TQuery, TElement> label) where TQuery : IGremlinQueryBase;

        IArrayGremlinQuery<TElement, TArrayItem, TOriginalQuery> Cap<TElement, TArrayItem, TOriginalQuery>(StepLabel<IArrayGremlinQuery<TElement, TArrayItem, TOriginalQuery>, TElement> label) where TOriginalQuery : IGremlinQueryBase;
    }

    public interface IGremlinQueryBase<TElement> : IGremlinQueryBase
    {
        new GremlinQueryAwaiter<TElement> GetAwaiter();

        IGremlinQuery<TElement> ForceBase();
        IEdgeGremlinQuery<TElement> ForceEdge();
        IValueGremlinQuery<TElement> ForceValue();
        IVertexGremlinQuery<TElement> ForceVertex();
        IElementGremlinQuery<TElement> ForceElement();
        IPropertyGremlinQuery<TElement> ForceProperty();
        IValueTupleGremlinQuery<TElement> ForceValueTuple();
        IInEdgeGremlinQuery<TElement, TInVertex> ForceInEdge<TInVertex>();
        IOutEdgeGremlinQuery<TElement, TOutVertex> ForceOutEdge<TOutVertex>();
        IVertexPropertyGremlinQuery<TElement, TValue> ForceVertexProperty<TValue>();
        IArrayGremlinQuery<TElement[], TElement, IGremlinQueryBase<TElement>> ForceArray();
        IVertexPropertyGremlinQuery<TElement, TValue, TMeta> ForceVertexProperty<TValue, TMeta>();
        IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex> ForceBothEdge<TOutVertex, TInVertex>();

        new IGremlinQuery<TElement> Lower();

        IAsyncEnumerable<TElement> ToAsyncEnumerable();
    }

    public interface IGremlinQueryBaseRec<TSelf> : IGremlinQueryBase
        where TSelf : IGremlinQueryBaseRec<TSelf>
    {
        TSelf And(params Func<TSelf, IGremlinQueryBase>[] andTraversals);

        TTargetQuery As<TTargetQuery>(Func<TSelf, StepLabel<TSelf, object>, TTargetQuery> continuation) where TTargetQuery : IGremlinQueryBase;

        TSelf Coin(double probability);

        TSelf Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<TSelf, IGremlinQueryBase> traversalPredicate, Func<TSelf, TTargetQuery> trueChoice, Func<TSelf, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQueryBase;
        TSelf Choose(Func<TSelf, IGremlinQueryBase> traversalPredicate, Func<TSelf, TSelf> trueChoice);
        IValueGremlinQuery<object> Choose(Func<TSelf, IGremlinQueryBase> traversalPredicate, Func<TSelf, IGremlinQueryBase> trueChoice);

        TTargetQuery Choose<TTargetQuery>(Func<IChooseBuilder<TSelf>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) where TTargetQuery : IGremlinQueryBase;

        TTargetQuery Coalesce<TTargetQuery>(params Func<TSelf, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQueryBase;
        IValueGremlinQuery<object> Coalesce(params Func<TSelf, IGremlinQueryBase>[] traversals);

        TSelf CyclicPath();

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

        TSelf Order(Func<IOrderBuilder<TSelf>, IOrderBuilderWithBy<TSelf>> projection);
        TSelf OrderLocal(Func<IOrderBuilder<TSelf>, IOrderBuilderWithBy<TSelf>> projection);

        TSelf Range(long low, long high);
        TSelf RangeLocal(long low, long high);

        TSelf Repeat(Func<TSelf, TSelf> repeatTraversal);
        TSelf RepeatUntil(Func<TSelf, TSelf> repeatTraversal, Func<TSelf, IGremlinQueryBase> untilTraversal);
        TSelf UntilRepeat(Func<TSelf, TSelf> repeatTraversal, Func<TSelf, IGremlinQueryBase> untilTraversal);

        TSelf SideEffect(Func<TSelf, IGremlinQueryBase> sideEffectTraversal);

        TSelf SimplePath();

        TSelf Skip(long count);
        TSelf SkipLocal(long count);

        TSelf Tail(long count);
        TSelf TailLocal(long count);

        TSelf Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<TSelf, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQueryBase;
        IValueGremlinQuery<object> Union(params Func<TSelf, IGremlinQueryBase>[] traversals);

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

        TTargetQuery Choose<TTargetQuery>(Expression<Func<TElement, bool>> predicate, Func<TSelf, TTargetQuery> trueChoice, Func<TSelf, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQueryBase;
        TSelf Choose(Expression<Func<TElement, bool>> predicate, Func<TSelf, TSelf> trueChoice);
        IValueGremlinQuery<object> Choose(Expression<Func<TElement, bool>> predicate, Func<TSelf, IGremlinQueryBase> trueChoice);

        IArrayGremlinQuery<TElement[], TElement, TSelf> Fold();

        new IArrayGremlinQuery<TElement[], TElement, TSelf> ForceArray();

        TSelf Inject(params TElement[] elements);

        IValueTupleGremlinQuery<dynamic> Project(Func<IProjectBuilder<TSelf, TElement>, IProjectResult> continuation);
        IValueTupleGremlinQuery<TResult> Project<TResult>(Func<IProjectBuilder<TSelf, TElement>, IProjectResult<TResult>> continuation);

        TSelf Order(Func<IOrderBuilder<TElement, TSelf>, IOrderBuilderWithBy<TElement, TSelf>> projection);
        TSelf OrderLocal(Func<IOrderBuilder<TElement, TSelf>, IOrderBuilderWithBy<TElement, TSelf>> projection);

        TSelf Where(Expression<Func<TElement, bool>> predicate);
    }

    public interface IGremlinQuery<TElement> : IGremlinQueryBaseRec<TElement, IGremlinQuery<TElement>>
    {
    }
}
