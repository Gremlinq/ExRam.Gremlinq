using System;
using NullGuard;
using System.Linq.Expressions;
using System.Collections.Generic;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    partial class GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>
    {
        IValueGremlinQuery<(T1, T2)> IGremlinQueryBase.Select<T1, T2>(StepLabel<T1> label1, StepLabel<T2> label2) => Project<TElement, (T1, T2)>(p => p.ToTuple().By(_ => _.Select(label1)).By(_ => _.Select(label2)));
        IValueGremlinQuery<(T1, T2, T3)> IGremlinQueryBase.Select<T1, T2, T3>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3) => Project<TElement, (T1, T2, T3)>(p => p.ToTuple().By(_ => _.Select(label1)).By(_ => _.Select(label2)).By(_ => _.Select(label3)));
        IValueGremlinQuery<(T1, T2, T3, T4)> IGremlinQueryBase.Select<T1, T2, T3, T4>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4) => Project<TElement, (T1, T2, T3, T4)>(p => p.ToTuple().By(_ => _.Select(label1)).By(_ => _.Select(label2)).By(_ => _.Select(label3)).By(_ => _.Select(label4)));
        IValueGremlinQuery<(T1, T2, T3, T4, T5)> IGremlinQueryBase.Select<T1, T2, T3, T4, T5>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5) => Project<TElement, (T1, T2, T3, T4, T5)>(p => p.ToTuple().By(_ => _.Select(label1)).By(_ => _.Select(label2)).By(_ => _.Select(label3)).By(_ => _.Select(label4)).By(_ => _.Select(label5)));
        IValueGremlinQuery<(T1, T2, T3, T4, T5, T6)> IGremlinQueryBase.Select<T1, T2, T3, T4, T5, T6>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6) => Project<TElement, (T1, T2, T3, T4, T5, T6)>(p => p.ToTuple().By(_ => _.Select(label1)).By(_ => _.Select(label2)).By(_ => _.Select(label3)).By(_ => _.Select(label4)).By(_ => _.Select(label5)).By(_ => _.Select(label6)));
        IValueGremlinQuery<(T1, T2, T3, T4, T5, T6, T7)> IGremlinQueryBase.Select<T1, T2, T3, T4, T5, T6, T7>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7) => Project<TElement, (T1, T2, T3, T4, T5, T6, T7)>(p => p.ToTuple().By(_ => _.Select(label1)).By(_ => _.Select(label2)).By(_ => _.Select(label3)).By(_ => _.Select(label4)).By(_ => _.Select(label5)).By(_ => _.Select(label6)).By(_ => _.Select(label7)));
        IValueGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8)> IGremlinQueryBase.Select<T1, T2, T3, T4, T5, T6, T7, T8>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8) => Project<TElement, (T1, T2, T3, T4, T5, T6, T7, T8)>(p => p.ToTuple().By(_ => _.Select(label1)).By(_ => _.Select(label2)).By(_ => _.Select(label3)).By(_ => _.Select(label4)).By(_ => _.Select(label5)).By(_ => _.Select(label6)).By(_ => _.Select(label7)).By(_ => _.Select(label8)));
        IValueGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> IGremlinQueryBase.Select<T1, T2, T3, T4, T5, T6, T7, T8, T9>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9) => Project<TElement, (T1, T2, T3, T4, T5, T6, T7, T8, T9)>(p => p.ToTuple().By(_ => _.Select(label1)).By(_ => _.Select(label2)).By(_ => _.Select(label3)).By(_ => _.Select(label4)).By(_ => _.Select(label5)).By(_ => _.Select(label6)).By(_ => _.Select(label7)).By(_ => _.Select(label8)).By(_ => _.Select(label9)));
        IValueGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> IGremlinQueryBase.Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10) => Project<TElement, (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)>(p => p.ToTuple().By(_ => _.Select(label1)).By(_ => _.Select(label2)).By(_ => _.Select(label3)).By(_ => _.Select(label4)).By(_ => _.Select(label5)).By(_ => _.Select(label6)).By(_ => _.Select(label7)).By(_ => _.Select(label8)).By(_ => _.Select(label9)).By(_ => _.Select(label10)));
        IValueGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> IGremlinQueryBase.Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11) => Project<TElement, (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)>(p => p.ToTuple().By(_ => _.Select(label1)).By(_ => _.Select(label2)).By(_ => _.Select(label3)).By(_ => _.Select(label4)).By(_ => _.Select(label5)).By(_ => _.Select(label6)).By(_ => _.Select(label7)).By(_ => _.Select(label8)).By(_ => _.Select(label9)).By(_ => _.Select(label10)).By(_ => _.Select(label11)));
        IValueGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> IGremlinQueryBase.Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11, StepLabel<T12> label12) => Project<TElement, (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)>(p => p.ToTuple().By(_ => _.Select(label1)).By(_ => _.Select(label2)).By(_ => _.Select(label3)).By(_ => _.Select(label4)).By(_ => _.Select(label5)).By(_ => _.Select(label6)).By(_ => _.Select(label7)).By(_ => _.Select(label8)).By(_ => _.Select(label9)).By(_ => _.Select(label10)).By(_ => _.Select(label11)).By(_ => _.Select(label12)));
        IValueGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)> IGremlinQueryBase.Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11, StepLabel<T12> label12, StepLabel<T13> label13) => Project<TElement, (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)>(p => p.ToTuple().By(_ => _.Select(label1)).By(_ => _.Select(label2)).By(_ => _.Select(label3)).By(_ => _.Select(label4)).By(_ => _.Select(label5)).By(_ => _.Select(label6)).By(_ => _.Select(label7)).By(_ => _.Select(label8)).By(_ => _.Select(label9)).By(_ => _.Select(label10)).By(_ => _.Select(label11)).By(_ => _.Select(label12)).By(_ => _.Select(label13)));
        IValueGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)> IGremlinQueryBase.Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11, StepLabel<T12> label12, StepLabel<T13> label13, StepLabel<T14> label14) => Project<TElement, (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)>(p => p.ToTuple().By(_ => _.Select(label1)).By(_ => _.Select(label2)).By(_ => _.Select(label3)).By(_ => _.Select(label4)).By(_ => _.Select(label5)).By(_ => _.Select(label6)).By(_ => _.Select(label7)).By(_ => _.Select(label8)).By(_ => _.Select(label9)).By(_ => _.Select(label10)).By(_ => _.Select(label11)).By(_ => _.Select(label12)).By(_ => _.Select(label13)).By(_ => _.Select(label14)));
        IValueGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)> IGremlinQueryBase.Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11, StepLabel<T12> label12, StepLabel<T13> label13, StepLabel<T14> label14, StepLabel<T15> label15) => Project<TElement, (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)>(p => p.ToTuple().By(_ => _.Select(label1)).By(_ => _.Select(label2)).By(_ => _.Select(label3)).By(_ => _.Select(label4)).By(_ => _.Select(label5)).By(_ => _.Select(label6)).By(_ => _.Select(label7)).By(_ => _.Select(label8)).By(_ => _.Select(label9)).By(_ => _.Select(label10)).By(_ => _.Select(label11)).By(_ => _.Select(label12)).By(_ => _.Select(label13)).By(_ => _.Select(label14)).By(_ => _.Select(label15)));
        IValueGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16)> IGremlinQueryBase.Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11, StepLabel<T12> label12, StepLabel<T13> label13, StepLabel<T14> label14, StepLabel<T15> label15, StepLabel<T16> label16) => Project<TElement, (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16)>(p => p.ToTuple().By(_ => _.Select(label1)).By(_ => _.Select(label2)).By(_ => _.Select(label3)).By(_ => _.Select(label4)).By(_ => _.Select(label5)).By(_ => _.Select(label6)).By(_ => _.Select(label7)).By(_ => _.Select(label8)).By(_ => _.Select(label9)).By(_ => _.Select(label10)).By(_ => _.Select(label11)).By(_ => _.Select(label12)).By(_ => _.Select(label13)).By(_ => _.Select(label14)).By(_ => _.Select(label15)).By(_ => _.Select(label16)));

        IGremlinQuery<TResult> IGremlinQueryBase.Cast<TResult>() => Cast<TResult>();
        IValueGremlinQuery<TResult> IValueGremlinQueryBase.Cast<TResult>() => Cast<TResult>();
        IElementGremlinQuery<TResult> IElementGremlinQueryBase.Cast<TResult>() => Cast<TResult>();
        IEdgeOrVertexGremlinQuery<TResult> IEdgeOrVertexGremlinQueryBase.Cast<TResult>() => Cast<TResult>();
        IVertexGremlinQuery<TResult> IVertexGremlinQueryBase.Cast<TResult>() => Cast<TResult>();
        IEdgeGremlinQuery<TResult> IEdgeGremlinQueryBase.Cast<TResult>() => Cast<TResult>();
        IPropertyGremlinQuery<TResult> IPropertyGremlinQueryBase.Cast<TResult>() => Cast<TResult>();

        TTargetQuery IGremlinQueryBaseRec<TElement, IGremlinQuery<TElement>>.Aggregate<TTargetQuery>(Func<IGremlinQuery<TElement>, StepLabel<IGremlinQuery<TElement>, TElement[]>, TTargetQuery> continuation) => Aggregate(Scope.Local, new StepLabel<IGremlinQuery<TElement>, TElement[]>(), continuation);
        TTargetQuery IGremlinQueryBaseRec<TElement, IGremlinQuery<TElement>>.AggregateGlobal<TTargetQuery>(Func<IGremlinQuery<TElement>, StepLabel<IGremlinQuery<TElement>, TElement[]>, TTargetQuery> continuation) => Aggregate(Scope.Global, new StepLabel<IGremlinQuery<TElement>, TElement[]>(), continuation);

        TTargetQuery IGremlinQueryBaseRec<TElement, IGremlinQuery<TElement>>.As<TTargetQuery>(Func<IGremlinQuery<TElement>, StepLabel<IGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => As(new StepLabel<IGremlinQuery<TElement>, TElement>(), continuation);
        IGremlinQuery<TElement> IGremlinQueryBaseRec<TElement, IGremlinQuery<TElement>>.As<TTargetQuery>(StepLabel<IGremlinQuery<TElement>, TElement> stepLabel) => As(stepLabel);
        IGremlinQuery<TElement> IGremlinQueryBase<TElement>.As(StepLabel<TElement> stepLabel) => As(stepLabel);

        IGremlinQuery<TElement> IGremlinQueryBaseRec<IGremlinQuery<TElement>>.And(params Func<IGremlinQuery<TElement>, IGremlinQueryBase>[] andTraversals) => And(andTraversals);

        IGremlinQuery<TElement> IGremlinQueryBaseRec<IGremlinQuery<TElement>>.Barrier() => Barrier();

        TTargetQuery IGremlinQueryBaseRec<IGremlinQuery<TElement>>.Choose<TTargetQuery>(Func<IGremlinQuery<TElement>, IGremlinQueryBase> traversalPredicate, Func<IGremlinQuery<TElement>, TTargetQuery> trueChoice, Func<IGremlinQuery<TElement>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);
        TTargetQuery IGremlinQueryBaseRec<IGremlinQuery<TElement>>.Choose<TTargetQuery>(Func<IGremlinQuery<TElement>, IGremlinQueryBase> traversalPredicate, Func<IGremlinQuery<TElement>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);
        TTargetQuery IGremlinQueryBaseRec<IGremlinQuery<TElement>>.Choose<TTargetQuery>(Func<IChooseBuilder<IGremlinQuery<TElement>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);

        TTargetQuery IGremlinQueryBaseRec<IGremlinQuery<TElement>>.Coalesce<TTargetQuery>(params Func<IGremlinQuery<TElement>, TTargetQuery>[] traversals) => Coalesce(traversals);

        IGremlinQuery<TElement> IGremlinQueryBaseRec<IGremlinQuery<TElement>>.Coin(double probability) => Coin(probability);

        IGremlinQuery<TElement> IGremlinQueryBaseRec<IGremlinQuery<TElement>>.Dedup() => Dedup();

        IGremlinQuery<TElement> IGremlinQueryBaseRec<IGremlinQuery<TElement>>.Emit() => Emit();

        TTargetQuery IGremlinQueryBaseRec<IGremlinQuery<TElement>>.FlatMap<TTargetQuery>(Func<IGremlinQuery<TElement>, TTargetQuery> mapping) => FlatMap(mapping);

        IArrayGremlinQuery<TElement[], IGremlinQuery<TElement>> IGremlinQueryBaseRec<TElement, IGremlinQuery<TElement>>.Fold() => Fold<IGremlinQuery<TElement>>();

        IValueGremlinQuery<IDictionary<TNewKey, TNewValue>> IGremlinQueryBaseRec<IGremlinQuery<TElement>>.Group<TNewKey, TNewValue>(Func<IGroupBuilder<IGremlinQuery<TElement>>, IGroupBuilderWithKeyAndValue<IGremlinQuery<TElement>, TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);
        IValueGremlinQuery<IDictionary<TNewKey, object>> IGremlinQueryBaseRec<IGremlinQuery<TElement>>.Group<TNewKey>(Func<IGroupBuilder<IGremlinQuery<TElement>>, IGroupBuilderWithKey<IGremlinQuery<TElement>, TNewKey>> groupBuilder) => Group(groupBuilder);

        IGremlinQuery<TElement> IGremlinQueryBaseRec<IGremlinQuery<TElement>>.Identity() => Identity();

        IGremlinQuery<TElement> IGremlinQueryBaseRec<TElement, IGremlinQuery<TElement>>.Inject(params TElement[] elements) => Inject(elements);
        
        IGremlinQuery<TElement> IGremlinQueryBaseRec<IGremlinQuery<TElement>>.Limit(long count) => Limit(count);

        IGremlinQuery<TElement> IGremlinQueryBaseRec<IGremlinQuery<TElement>>.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IGremlinQueryBaseRec<IGremlinQuery<TElement>>.Local<TTargetQuery>(Func<IGremlinQuery<TElement> , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IGremlinQueryBaseRec<IGremlinQuery<TElement>>.Map<TTargetQuery>(Func<IGremlinQuery<TElement>, TTargetQuery> mapping) => Map(mapping);
        
        IGremlinQuery<TElement> IGremlinQueryBaseRec<IGremlinQuery<TElement>>.Not(Func<IGremlinQuery<TElement>, IGremlinQueryBase> notTraversal) => Not(notTraversal);

        IGremlinQuery<TElement> IGremlinQueryBaseRec<IGremlinQuery<TElement>>.None() => None();

        IGremlinQuery<TElement> IGremlinQueryBaseRec<IGremlinQuery<TElement>>.Optional(Func<IGremlinQuery<TElement>, IGremlinQuery<TElement>> optionalTraversal) => Optional(optionalTraversal);

        IGremlinQuery<TElement> IGremlinQueryBaseRec<IGremlinQuery<TElement>>.Or(params Func<IGremlinQuery<TElement>, IGremlinQueryBase>[] orTraversals) => Or(orTraversals);

        IValueGremlinQuery<dynamic> IGremlinQueryBaseRec<TElement, IGremlinQuery<TElement>>.Project(Func<IProjectBuilder<IGremlinQuery<TElement>, TElement>, IProjectResult> continuation) => Project<TElement, dynamic>(continuation);
        IValueGremlinQuery<TResult> IGremlinQueryBaseRec<TElement, IGremlinQuery<TElement>>.Project<TResult>(Func<IProjectBuilder<IGremlinQuery<TElement>, TElement>, IProjectResult<TResult>> continuation) => Project<TElement, TResult>(continuation);

        IGremlinQuery<TElement> IGremlinQueryBaseRec<IGremlinQuery<TElement>>.Range(long low, long high) => Range(low, high);

        IGremlinQuery<TElement> IGremlinQueryBaseRec<IGremlinQuery<TElement>>.Repeat(Func<IGremlinQuery<TElement>, IGremlinQuery<TElement>> repeatTraversal) => Repeat(repeatTraversal);

        IGremlinQuery<TElement> IGremlinQueryBaseRec<IGremlinQuery<TElement>>.RepeatUntil(Func<IGremlinQuery<TElement>, IGremlinQuery<TElement>> repeatTraversal, Func<IGremlinQuery<TElement>, IGremlinQueryBase> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        IGremlinQuery<TElement> IGremlinQueryBaseRec<IGremlinQuery<TElement>>.UntilRepeat(Func<IGremlinQuery<TElement>, IGremlinQuery<TElement>> repeatTraversal, Func<IGremlinQuery<TElement>, IGremlinQueryBase> untilTraversal) => UntilRepeat(repeatTraversal, untilTraversal);

        IGremlinQuery<TElement> IGremlinQueryBaseRec<IGremlinQuery<TElement>>.SideEffect(Func<IGremlinQuery<TElement>, IGremlinQueryBase> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IGremlinQuery<TElement> IGremlinQueryBaseRec<IGremlinQuery<TElement>>.Skip(long count) => Skip(count);

        IGremlinQuery<TElement> IGremlinQueryBaseRec<IGremlinQuery<TElement>>.Tail(long count) => Tail(count);
        IGremlinQuery<TElement> IGremlinQueryBaseRec<IGremlinQuery<TElement>>.TailLocal(int count) => TailLocal(count);

        IGremlinQuery<TElement> IGremlinQueryBaseRec<IGremlinQuery<TElement>>.Times(int count) => Times(count);

        TTargetQuery IGremlinQueryBaseRec<IGremlinQuery<TElement>>.Union<TTargetQuery>(params Func<IGremlinQuery<TElement>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        IGremlinQuery<TElement> IGremlinQueryBaseRec<IGremlinQuery<TElement>>.Where(ILambda lambda) => Where(lambda);
        IGremlinQuery<TElement> IGremlinQueryBaseRec<IGremlinQuery<TElement>>.Where(Func<IGremlinQuery<TElement>, IGremlinQueryBase> filterTraversal) => Where(filterTraversal);

        TTargetQuery IGremlinQueryBaseRec<TElement, IValueGremlinQuery<TElement>>.Aggregate<TTargetQuery>(Func<IValueGremlinQuery<TElement>, StepLabel<IValueGremlinQuery<TElement>, TElement[]>, TTargetQuery> continuation) => Aggregate(Scope.Local, new StepLabel<IValueGremlinQuery<TElement>, TElement[]>(), continuation);
        TTargetQuery IGremlinQueryBaseRec<TElement, IValueGremlinQuery<TElement>>.AggregateGlobal<TTargetQuery>(Func<IValueGremlinQuery<TElement>, StepLabel<IValueGremlinQuery<TElement>, TElement[]>, TTargetQuery> continuation) => Aggregate(Scope.Global, new StepLabel<IValueGremlinQuery<TElement>, TElement[]>(), continuation);

        TTargetQuery IGremlinQueryBaseRec<TElement, IValueGremlinQuery<TElement>>.As<TTargetQuery>(Func<IValueGremlinQuery<TElement>, StepLabel<IValueGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => As(new StepLabel<IValueGremlinQuery<TElement>, TElement>(), continuation);
        IValueGremlinQuery<TElement> IGremlinQueryBaseRec<TElement, IValueGremlinQuery<TElement>>.As<TTargetQuery>(StepLabel<IValueGremlinQuery<TElement>, TElement> stepLabel) => As(stepLabel);
        IValueGremlinQuery<TElement> IValueGremlinQueryBase<TElement>.As(StepLabel<TElement> stepLabel) => As(stepLabel);

        IValueGremlinQuery<TElement> IGremlinQueryBaseRec<IValueGremlinQuery<TElement>>.And(params Func<IValueGremlinQuery<TElement>, IGremlinQueryBase>[] andTraversals) => And(andTraversals);

        IValueGremlinQuery<TElement> IGremlinQueryBaseRec<IValueGremlinQuery<TElement>>.Barrier() => Barrier();

        TTargetQuery IGremlinQueryBaseRec<IValueGremlinQuery<TElement>>.Choose<TTargetQuery>(Func<IValueGremlinQuery<TElement>, IGremlinQueryBase> traversalPredicate, Func<IValueGremlinQuery<TElement>, TTargetQuery> trueChoice, Func<IValueGremlinQuery<TElement>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);
        TTargetQuery IGremlinQueryBaseRec<IValueGremlinQuery<TElement>>.Choose<TTargetQuery>(Func<IValueGremlinQuery<TElement>, IGremlinQueryBase> traversalPredicate, Func<IValueGremlinQuery<TElement>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);
        TTargetQuery IGremlinQueryBaseRec<IValueGremlinQuery<TElement>>.Choose<TTargetQuery>(Func<IChooseBuilder<IValueGremlinQuery<TElement>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);

        TTargetQuery IGremlinQueryBaseRec<IValueGremlinQuery<TElement>>.Coalesce<TTargetQuery>(params Func<IValueGremlinQuery<TElement>, TTargetQuery>[] traversals) => Coalesce(traversals);

        IValueGremlinQuery<TElement> IGremlinQueryBaseRec<IValueGremlinQuery<TElement>>.Coin(double probability) => Coin(probability);

        IValueGremlinQuery<TElement> IGremlinQueryBaseRec<IValueGremlinQuery<TElement>>.Dedup() => Dedup();

        IValueGremlinQuery<TElement> IGremlinQueryBaseRec<IValueGremlinQuery<TElement>>.Emit() => Emit();

        TTargetQuery IGremlinQueryBaseRec<IValueGremlinQuery<TElement>>.FlatMap<TTargetQuery>(Func<IValueGremlinQuery<TElement>, TTargetQuery> mapping) => FlatMap(mapping);

        IArrayGremlinQuery<TElement[], IValueGremlinQuery<TElement>> IGremlinQueryBaseRec<TElement, IValueGremlinQuery<TElement>>.Fold() => Fold<IValueGremlinQuery<TElement>>();

        IValueGremlinQuery<IDictionary<TNewKey, TNewValue>> IGremlinQueryBaseRec<IValueGremlinQuery<TElement>>.Group<TNewKey, TNewValue>(Func<IGroupBuilder<IValueGremlinQuery<TElement>>, IGroupBuilderWithKeyAndValue<IValueGremlinQuery<TElement>, TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);
        IValueGremlinQuery<IDictionary<TNewKey, object>> IGremlinQueryBaseRec<IValueGremlinQuery<TElement>>.Group<TNewKey>(Func<IGroupBuilder<IValueGremlinQuery<TElement>>, IGroupBuilderWithKey<IValueGremlinQuery<TElement>, TNewKey>> groupBuilder) => Group(groupBuilder);

        IValueGremlinQuery<TElement> IGremlinQueryBaseRec<IValueGremlinQuery<TElement>>.Identity() => Identity();

        IValueGremlinQuery<TElement> IGremlinQueryBaseRec<TElement, IValueGremlinQuery<TElement>>.Inject(params TElement[] elements) => Inject(elements);
        
        IValueGremlinQuery<TElement> IGremlinQueryBaseRec<IValueGremlinQuery<TElement>>.Limit(long count) => Limit(count);

        IValueGremlinQuery<TElement> IGremlinQueryBaseRec<IValueGremlinQuery<TElement>>.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IGremlinQueryBaseRec<IValueGremlinQuery<TElement>>.Local<TTargetQuery>(Func<IValueGremlinQuery<TElement> , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IGremlinQueryBaseRec<IValueGremlinQuery<TElement>>.Map<TTargetQuery>(Func<IValueGremlinQuery<TElement>, TTargetQuery> mapping) => Map(mapping);
        
        IValueGremlinQuery<TElement> IGremlinQueryBaseRec<IValueGremlinQuery<TElement>>.Not(Func<IValueGremlinQuery<TElement>, IGremlinQueryBase> notTraversal) => Not(notTraversal);

        IValueGremlinQuery<TElement> IGremlinQueryBaseRec<IValueGremlinQuery<TElement>>.None() => None();

        IValueGremlinQuery<TElement> IGremlinQueryBaseRec<IValueGremlinQuery<TElement>>.Optional(Func<IValueGremlinQuery<TElement>, IValueGremlinQuery<TElement>> optionalTraversal) => Optional(optionalTraversal);

        IValueGremlinQuery<TElement> IGremlinQueryBaseRec<IValueGremlinQuery<TElement>>.Or(params Func<IValueGremlinQuery<TElement>, IGremlinQueryBase>[] orTraversals) => Or(orTraversals);

        IValueGremlinQuery<dynamic> IGremlinQueryBaseRec<TElement, IValueGremlinQuery<TElement>>.Project(Func<IProjectBuilder<IValueGremlinQuery<TElement>, TElement>, IProjectResult> continuation) => Project<TElement, dynamic>(continuation);
        IValueGremlinQuery<TResult> IGremlinQueryBaseRec<TElement, IValueGremlinQuery<TElement>>.Project<TResult>(Func<IProjectBuilder<IValueGremlinQuery<TElement>, TElement>, IProjectResult<TResult>> continuation) => Project<TElement, TResult>(continuation);

        IValueGremlinQuery<TElement> IGremlinQueryBaseRec<IValueGremlinQuery<TElement>>.Range(long low, long high) => Range(low, high);

        IValueGremlinQuery<TElement> IGremlinQueryBaseRec<IValueGremlinQuery<TElement>>.Repeat(Func<IValueGremlinQuery<TElement>, IValueGremlinQuery<TElement>> repeatTraversal) => Repeat(repeatTraversal);

        IValueGremlinQuery<TElement> IGremlinQueryBaseRec<IValueGremlinQuery<TElement>>.RepeatUntil(Func<IValueGremlinQuery<TElement>, IValueGremlinQuery<TElement>> repeatTraversal, Func<IValueGremlinQuery<TElement>, IGremlinQueryBase> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        IValueGremlinQuery<TElement> IGremlinQueryBaseRec<IValueGremlinQuery<TElement>>.UntilRepeat(Func<IValueGremlinQuery<TElement>, IValueGremlinQuery<TElement>> repeatTraversal, Func<IValueGremlinQuery<TElement>, IGremlinQueryBase> untilTraversal) => UntilRepeat(repeatTraversal, untilTraversal);

        IValueGremlinQuery<TElement> IGremlinQueryBaseRec<IValueGremlinQuery<TElement>>.SideEffect(Func<IValueGremlinQuery<TElement>, IGremlinQueryBase> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IValueGremlinQuery<TElement> IGremlinQueryBaseRec<IValueGremlinQuery<TElement>>.Skip(long count) => Skip(count);

        IValueGremlinQuery<TElement> IGremlinQueryBaseRec<IValueGremlinQuery<TElement>>.Tail(long count) => Tail(count);
        IValueGremlinQuery<TElement> IGremlinQueryBaseRec<IValueGremlinQuery<TElement>>.TailLocal(int count) => TailLocal(count);

        IValueGremlinQuery<TElement> IGremlinQueryBaseRec<IValueGremlinQuery<TElement>>.Times(int count) => Times(count);

        TTargetQuery IGremlinQueryBaseRec<IValueGremlinQuery<TElement>>.Union<TTargetQuery>(params Func<IValueGremlinQuery<TElement>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        IValueGremlinQuery<TElement> IGremlinQueryBaseRec<IValueGremlinQuery<TElement>>.Where(ILambda lambda) => Where(lambda);
        IValueGremlinQuery<TElement> IGremlinQueryBaseRec<IValueGremlinQuery<TElement>>.Where(Func<IValueGremlinQuery<TElement>, IGremlinQueryBase> filterTraversal) => Where(filterTraversal);

        TTargetQuery IGremlinQueryBaseRec<TElement, IArrayGremlinQuery<TElement, TFoldedQuery>>.Aggregate<TTargetQuery>(Func<IArrayGremlinQuery<TElement, TFoldedQuery>, StepLabel<IArrayGremlinQuery<TElement, TFoldedQuery>, TElement[]>, TTargetQuery> continuation) => Aggregate(Scope.Local, new StepLabel<IArrayGremlinQuery<TElement, TFoldedQuery>, TElement[]>(), continuation);
        TTargetQuery IGremlinQueryBaseRec<TElement, IArrayGremlinQuery<TElement, TFoldedQuery>>.AggregateGlobal<TTargetQuery>(Func<IArrayGremlinQuery<TElement, TFoldedQuery>, StepLabel<IArrayGremlinQuery<TElement, TFoldedQuery>, TElement[]>, TTargetQuery> continuation) => Aggregate(Scope.Global, new StepLabel<IArrayGremlinQuery<TElement, TFoldedQuery>, TElement[]>(), continuation);

        TTargetQuery IGremlinQueryBaseRec<TElement, IArrayGremlinQuery<TElement, TFoldedQuery>>.As<TTargetQuery>(Func<IArrayGremlinQuery<TElement, TFoldedQuery>, StepLabel<IArrayGremlinQuery<TElement, TFoldedQuery>, TElement>, TTargetQuery> continuation) => As(new StepLabel<IArrayGremlinQuery<TElement, TFoldedQuery>, TElement>(), continuation);
        IArrayGremlinQuery<TElement, TFoldedQuery> IGremlinQueryBaseRec<TElement, IArrayGremlinQuery<TElement, TFoldedQuery>>.As<TTargetQuery>(StepLabel<IArrayGremlinQuery<TElement, TFoldedQuery>, TElement> stepLabel) => As(stepLabel);
        IArrayGremlinQuery<TElement, TFoldedQuery> IArrayGremlinQueryBase<TElement, TFoldedQuery>.As(StepLabel<TElement> stepLabel) => As(stepLabel);

        IArrayGremlinQuery<TElement, TFoldedQuery> IGremlinQueryBaseRec<IArrayGremlinQuery<TElement, TFoldedQuery>>.And(params Func<IArrayGremlinQuery<TElement, TFoldedQuery>, IGremlinQueryBase>[] andTraversals) => And(andTraversals);

        IArrayGremlinQuery<TElement, TFoldedQuery> IGremlinQueryBaseRec<IArrayGremlinQuery<TElement, TFoldedQuery>>.Barrier() => Barrier();

        TTargetQuery IGremlinQueryBaseRec<IArrayGremlinQuery<TElement, TFoldedQuery>>.Choose<TTargetQuery>(Func<IArrayGremlinQuery<TElement, TFoldedQuery>, IGremlinQueryBase> traversalPredicate, Func<IArrayGremlinQuery<TElement, TFoldedQuery>, TTargetQuery> trueChoice, Func<IArrayGremlinQuery<TElement, TFoldedQuery>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);
        TTargetQuery IGremlinQueryBaseRec<IArrayGremlinQuery<TElement, TFoldedQuery>>.Choose<TTargetQuery>(Func<IArrayGremlinQuery<TElement, TFoldedQuery>, IGremlinQueryBase> traversalPredicate, Func<IArrayGremlinQuery<TElement, TFoldedQuery>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);
        TTargetQuery IGremlinQueryBaseRec<IArrayGremlinQuery<TElement, TFoldedQuery>>.Choose<TTargetQuery>(Func<IChooseBuilder<IArrayGremlinQuery<TElement, TFoldedQuery>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);

        TTargetQuery IGremlinQueryBaseRec<IArrayGremlinQuery<TElement, TFoldedQuery>>.Coalesce<TTargetQuery>(params Func<IArrayGremlinQuery<TElement, TFoldedQuery>, TTargetQuery>[] traversals) => Coalesce(traversals);

        IArrayGremlinQuery<TElement, TFoldedQuery> IGremlinQueryBaseRec<IArrayGremlinQuery<TElement, TFoldedQuery>>.Coin(double probability) => Coin(probability);

        IArrayGremlinQuery<TElement, TFoldedQuery> IGremlinQueryBaseRec<IArrayGremlinQuery<TElement, TFoldedQuery>>.Dedup() => Dedup();

        IArrayGremlinQuery<TElement, TFoldedQuery> IGremlinQueryBaseRec<IArrayGremlinQuery<TElement, TFoldedQuery>>.Emit() => Emit();

        TTargetQuery IGremlinQueryBaseRec<IArrayGremlinQuery<TElement, TFoldedQuery>>.FlatMap<TTargetQuery>(Func<IArrayGremlinQuery<TElement, TFoldedQuery>, TTargetQuery> mapping) => FlatMap(mapping);

        IArrayGremlinQuery<TElement[], IArrayGremlinQuery<TElement, TFoldedQuery>> IGremlinQueryBaseRec<TElement, IArrayGremlinQuery<TElement, TFoldedQuery>>.Fold() => Fold<IArrayGremlinQuery<TElement, TFoldedQuery>>();

        IValueGremlinQuery<IDictionary<TNewKey, TNewValue>> IGremlinQueryBaseRec<IArrayGremlinQuery<TElement, TFoldedQuery>>.Group<TNewKey, TNewValue>(Func<IGroupBuilder<IArrayGremlinQuery<TElement, TFoldedQuery>>, IGroupBuilderWithKeyAndValue<IArrayGremlinQuery<TElement, TFoldedQuery>, TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);
        IValueGremlinQuery<IDictionary<TNewKey, object>> IGremlinQueryBaseRec<IArrayGremlinQuery<TElement, TFoldedQuery>>.Group<TNewKey>(Func<IGroupBuilder<IArrayGremlinQuery<TElement, TFoldedQuery>>, IGroupBuilderWithKey<IArrayGremlinQuery<TElement, TFoldedQuery>, TNewKey>> groupBuilder) => Group(groupBuilder);

        IArrayGremlinQuery<TElement, TFoldedQuery> IGremlinQueryBaseRec<IArrayGremlinQuery<TElement, TFoldedQuery>>.Identity() => Identity();

        IArrayGremlinQuery<TElement, TFoldedQuery> IGremlinQueryBaseRec<TElement, IArrayGremlinQuery<TElement, TFoldedQuery>>.Inject(params TElement[] elements) => Inject(elements);
        
        IArrayGremlinQuery<TElement, TFoldedQuery> IGremlinQueryBaseRec<IArrayGremlinQuery<TElement, TFoldedQuery>>.Limit(long count) => Limit(count);

        IArrayGremlinQuery<TElement, TFoldedQuery> IGremlinQueryBaseRec<IArrayGremlinQuery<TElement, TFoldedQuery>>.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IGremlinQueryBaseRec<IArrayGremlinQuery<TElement, TFoldedQuery>>.Local<TTargetQuery>(Func<IArrayGremlinQuery<TElement, TFoldedQuery> , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IGremlinQueryBaseRec<IArrayGremlinQuery<TElement, TFoldedQuery>>.Map<TTargetQuery>(Func<IArrayGremlinQuery<TElement, TFoldedQuery>, TTargetQuery> mapping) => Map(mapping);
        
        IArrayGremlinQuery<TElement, TFoldedQuery> IGremlinQueryBaseRec<IArrayGremlinQuery<TElement, TFoldedQuery>>.Not(Func<IArrayGremlinQuery<TElement, TFoldedQuery>, IGremlinQueryBase> notTraversal) => Not(notTraversal);

        IArrayGremlinQuery<TElement, TFoldedQuery> IGremlinQueryBaseRec<IArrayGremlinQuery<TElement, TFoldedQuery>>.None() => None();

        IArrayGremlinQuery<TElement, TFoldedQuery> IGremlinQueryBaseRec<IArrayGremlinQuery<TElement, TFoldedQuery>>.Optional(Func<IArrayGremlinQuery<TElement, TFoldedQuery>, IArrayGremlinQuery<TElement, TFoldedQuery>> optionalTraversal) => Optional(optionalTraversal);

        IArrayGremlinQuery<TElement, TFoldedQuery> IGremlinQueryBaseRec<IArrayGremlinQuery<TElement, TFoldedQuery>>.Or(params Func<IArrayGremlinQuery<TElement, TFoldedQuery>, IGremlinQueryBase>[] orTraversals) => Or(orTraversals);

        IValueGremlinQuery<dynamic> IGremlinQueryBaseRec<TElement, IArrayGremlinQuery<TElement, TFoldedQuery>>.Project(Func<IProjectBuilder<IArrayGremlinQuery<TElement, TFoldedQuery>, TElement>, IProjectResult> continuation) => Project<TElement, dynamic>(continuation);
        IValueGremlinQuery<TResult> IGremlinQueryBaseRec<TElement, IArrayGremlinQuery<TElement, TFoldedQuery>>.Project<TResult>(Func<IProjectBuilder<IArrayGremlinQuery<TElement, TFoldedQuery>, TElement>, IProjectResult<TResult>> continuation) => Project<TElement, TResult>(continuation);

        IArrayGremlinQuery<TElement, TFoldedQuery> IGremlinQueryBaseRec<IArrayGremlinQuery<TElement, TFoldedQuery>>.Range(long low, long high) => Range(low, high);

        IArrayGremlinQuery<TElement, TFoldedQuery> IGremlinQueryBaseRec<IArrayGremlinQuery<TElement, TFoldedQuery>>.Repeat(Func<IArrayGremlinQuery<TElement, TFoldedQuery>, IArrayGremlinQuery<TElement, TFoldedQuery>> repeatTraversal) => Repeat(repeatTraversal);

        IArrayGremlinQuery<TElement, TFoldedQuery> IGremlinQueryBaseRec<IArrayGremlinQuery<TElement, TFoldedQuery>>.RepeatUntil(Func<IArrayGremlinQuery<TElement, TFoldedQuery>, IArrayGremlinQuery<TElement, TFoldedQuery>> repeatTraversal, Func<IArrayGremlinQuery<TElement, TFoldedQuery>, IGremlinQueryBase> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        IArrayGremlinQuery<TElement, TFoldedQuery> IGremlinQueryBaseRec<IArrayGremlinQuery<TElement, TFoldedQuery>>.UntilRepeat(Func<IArrayGremlinQuery<TElement, TFoldedQuery>, IArrayGremlinQuery<TElement, TFoldedQuery>> repeatTraversal, Func<IArrayGremlinQuery<TElement, TFoldedQuery>, IGremlinQueryBase> untilTraversal) => UntilRepeat(repeatTraversal, untilTraversal);

        IArrayGremlinQuery<TElement, TFoldedQuery> IGremlinQueryBaseRec<IArrayGremlinQuery<TElement, TFoldedQuery>>.SideEffect(Func<IArrayGremlinQuery<TElement, TFoldedQuery>, IGremlinQueryBase> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IArrayGremlinQuery<TElement, TFoldedQuery> IGremlinQueryBaseRec<IArrayGremlinQuery<TElement, TFoldedQuery>>.Skip(long count) => Skip(count);

        IArrayGremlinQuery<TElement, TFoldedQuery> IGremlinQueryBaseRec<IArrayGremlinQuery<TElement, TFoldedQuery>>.Tail(long count) => Tail(count);
        IArrayGremlinQuery<TElement, TFoldedQuery> IGremlinQueryBaseRec<IArrayGremlinQuery<TElement, TFoldedQuery>>.TailLocal(int count) => TailLocal(count);

        IArrayGremlinQuery<TElement, TFoldedQuery> IGremlinQueryBaseRec<IArrayGremlinQuery<TElement, TFoldedQuery>>.Times(int count) => Times(count);

        TTargetQuery IGremlinQueryBaseRec<IArrayGremlinQuery<TElement, TFoldedQuery>>.Union<TTargetQuery>(params Func<IArrayGremlinQuery<TElement, TFoldedQuery>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        IArrayGremlinQuery<TElement, TFoldedQuery> IGremlinQueryBaseRec<IArrayGremlinQuery<TElement, TFoldedQuery>>.Where(ILambda lambda) => Where(lambda);
        IArrayGremlinQuery<TElement, TFoldedQuery> IGremlinQueryBaseRec<IArrayGremlinQuery<TElement, TFoldedQuery>>.Where(Func<IArrayGremlinQuery<TElement, TFoldedQuery>, IGremlinQueryBase> filterTraversal) => Where(filterTraversal);

        TTargetQuery IGremlinQueryBaseRec<TElement, IElementGremlinQuery<TElement>>.Aggregate<TTargetQuery>(Func<IElementGremlinQuery<TElement>, StepLabel<IElementGremlinQuery<TElement>, TElement[]>, TTargetQuery> continuation) => Aggregate(Scope.Local, new StepLabel<IElementGremlinQuery<TElement>, TElement[]>(), continuation);
        TTargetQuery IGremlinQueryBaseRec<TElement, IElementGremlinQuery<TElement>>.AggregateGlobal<TTargetQuery>(Func<IElementGremlinQuery<TElement>, StepLabel<IElementGremlinQuery<TElement>, TElement[]>, TTargetQuery> continuation) => Aggregate(Scope.Global, new StepLabel<IElementGremlinQuery<TElement>, TElement[]>(), continuation);

        TTargetQuery IGremlinQueryBaseRec<TElement, IElementGremlinQuery<TElement>>.As<TTargetQuery>(Func<IElementGremlinQuery<TElement>, StepLabel<IElementGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => As(new StepLabel<IElementGremlinQuery<TElement>, TElement>(), continuation);
        IElementGremlinQuery<TElement> IGremlinQueryBaseRec<TElement, IElementGremlinQuery<TElement>>.As<TTargetQuery>(StepLabel<IElementGremlinQuery<TElement>, TElement> stepLabel) => As(stepLabel);
        IElementGremlinQuery<TElement> IElementGremlinQueryBase<TElement>.As(StepLabel<TElement> stepLabel) => As(stepLabel);

        IElementGremlinQuery<TElement> IGremlinQueryBaseRec<IElementGremlinQuery<TElement>>.And(params Func<IElementGremlinQuery<TElement>, IGremlinQueryBase>[] andTraversals) => And(andTraversals);

        IElementGremlinQuery<TElement> IGremlinQueryBaseRec<IElementGremlinQuery<TElement>>.Barrier() => Barrier();

        TTargetQuery IGremlinQueryBaseRec<IElementGremlinQuery<TElement>>.Choose<TTargetQuery>(Func<IElementGremlinQuery<TElement>, IGremlinQueryBase> traversalPredicate, Func<IElementGremlinQuery<TElement>, TTargetQuery> trueChoice, Func<IElementGremlinQuery<TElement>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);
        TTargetQuery IGremlinQueryBaseRec<IElementGremlinQuery<TElement>>.Choose<TTargetQuery>(Func<IElementGremlinQuery<TElement>, IGremlinQueryBase> traversalPredicate, Func<IElementGremlinQuery<TElement>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);
        TTargetQuery IGremlinQueryBaseRec<IElementGremlinQuery<TElement>>.Choose<TTargetQuery>(Func<IChooseBuilder<IElementGremlinQuery<TElement>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);

        TTargetQuery IGremlinQueryBaseRec<IElementGremlinQuery<TElement>>.Coalesce<TTargetQuery>(params Func<IElementGremlinQuery<TElement>, TTargetQuery>[] traversals) => Coalesce(traversals);

        IElementGremlinQuery<TElement> IGremlinQueryBaseRec<IElementGremlinQuery<TElement>>.Coin(double probability) => Coin(probability);

        IElementGremlinQuery<TElement> IGremlinQueryBaseRec<IElementGremlinQuery<TElement>>.Dedup() => Dedup();

        IElementGremlinQuery<TElement> IGremlinQueryBaseRec<IElementGremlinQuery<TElement>>.Emit() => Emit();

        TTargetQuery IGremlinQueryBaseRec<IElementGremlinQuery<TElement>>.FlatMap<TTargetQuery>(Func<IElementGremlinQuery<TElement>, TTargetQuery> mapping) => FlatMap(mapping);

        IArrayGremlinQuery<TElement[], IElementGremlinQuery<TElement>> IGremlinQueryBaseRec<TElement, IElementGremlinQuery<TElement>>.Fold() => Fold<IElementGremlinQuery<TElement>>();

        IValueGremlinQuery<IDictionary<TNewKey, TNewValue>> IGremlinQueryBaseRec<IElementGremlinQuery<TElement>>.Group<TNewKey, TNewValue>(Func<IGroupBuilder<IElementGremlinQuery<TElement>>, IGroupBuilderWithKeyAndValue<IElementGremlinQuery<TElement>, TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);
        IValueGremlinQuery<IDictionary<TNewKey, object>> IGremlinQueryBaseRec<IElementGremlinQuery<TElement>>.Group<TNewKey>(Func<IGroupBuilder<IElementGremlinQuery<TElement>>, IGroupBuilderWithKey<IElementGremlinQuery<TElement>, TNewKey>> groupBuilder) => Group(groupBuilder);

        IElementGremlinQuery<TElement> IGremlinQueryBaseRec<IElementGremlinQuery<TElement>>.Identity() => Identity();

        IElementGremlinQuery<TElement> IGremlinQueryBaseRec<TElement, IElementGremlinQuery<TElement>>.Inject(params TElement[] elements) => Inject(elements);
        
        IElementGremlinQuery<TElement> IGremlinQueryBaseRec<IElementGremlinQuery<TElement>>.Limit(long count) => Limit(count);

        IElementGremlinQuery<TElement> IGremlinQueryBaseRec<IElementGremlinQuery<TElement>>.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IGremlinQueryBaseRec<IElementGremlinQuery<TElement>>.Local<TTargetQuery>(Func<IElementGremlinQuery<TElement> , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IGremlinQueryBaseRec<IElementGremlinQuery<TElement>>.Map<TTargetQuery>(Func<IElementGremlinQuery<TElement>, TTargetQuery> mapping) => Map(mapping);
        
        IElementGremlinQuery<TElement> IGremlinQueryBaseRec<IElementGremlinQuery<TElement>>.Not(Func<IElementGremlinQuery<TElement>, IGremlinQueryBase> notTraversal) => Not(notTraversal);

        IElementGremlinQuery<TElement> IGremlinQueryBaseRec<IElementGremlinQuery<TElement>>.None() => None();

        IElementGremlinQuery<TElement> IGremlinQueryBaseRec<IElementGremlinQuery<TElement>>.Optional(Func<IElementGremlinQuery<TElement>, IElementGremlinQuery<TElement>> optionalTraversal) => Optional(optionalTraversal);

        IElementGremlinQuery<TElement> IGremlinQueryBaseRec<IElementGremlinQuery<TElement>>.Or(params Func<IElementGremlinQuery<TElement>, IGremlinQueryBase>[] orTraversals) => Or(orTraversals);

        IValueGremlinQuery<dynamic> IGremlinQueryBaseRec<TElement, IElementGremlinQuery<TElement>>.Project(Func<IProjectBuilder<IElementGremlinQuery<TElement>, TElement>, IProjectResult> continuation) => Project<TElement, dynamic>(continuation);
        IValueGremlinQuery<TResult> IGremlinQueryBaseRec<TElement, IElementGremlinQuery<TElement>>.Project<TResult>(Func<IProjectBuilder<IElementGremlinQuery<TElement>, TElement>, IProjectResult<TResult>> continuation) => Project<TElement, TResult>(continuation);

        IElementGremlinQuery<TElement> IGremlinQueryBaseRec<IElementGremlinQuery<TElement>>.Range(long low, long high) => Range(low, high);

        IElementGremlinQuery<TElement> IGremlinQueryBaseRec<IElementGremlinQuery<TElement>>.Repeat(Func<IElementGremlinQuery<TElement>, IElementGremlinQuery<TElement>> repeatTraversal) => Repeat(repeatTraversal);

        IElementGremlinQuery<TElement> IGremlinQueryBaseRec<IElementGremlinQuery<TElement>>.RepeatUntil(Func<IElementGremlinQuery<TElement>, IElementGremlinQuery<TElement>> repeatTraversal, Func<IElementGremlinQuery<TElement>, IGremlinQueryBase> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        IElementGremlinQuery<TElement> IGremlinQueryBaseRec<IElementGremlinQuery<TElement>>.UntilRepeat(Func<IElementGremlinQuery<TElement>, IElementGremlinQuery<TElement>> repeatTraversal, Func<IElementGremlinQuery<TElement>, IGremlinQueryBase> untilTraversal) => UntilRepeat(repeatTraversal, untilTraversal);

        IElementGremlinQuery<TElement> IGremlinQueryBaseRec<IElementGremlinQuery<TElement>>.SideEffect(Func<IElementGremlinQuery<TElement>, IGremlinQueryBase> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IElementGremlinQuery<TElement> IGremlinQueryBaseRec<IElementGremlinQuery<TElement>>.Skip(long count) => Skip(count);

        IElementGremlinQuery<TElement> IGremlinQueryBaseRec<IElementGremlinQuery<TElement>>.Tail(long count) => Tail(count);
        IElementGremlinQuery<TElement> IGremlinQueryBaseRec<IElementGremlinQuery<TElement>>.TailLocal(int count) => TailLocal(count);

        IElementGremlinQuery<TElement> IGremlinQueryBaseRec<IElementGremlinQuery<TElement>>.Times(int count) => Times(count);

        TTargetQuery IGremlinQueryBaseRec<IElementGremlinQuery<TElement>>.Union<TTargetQuery>(params Func<IElementGremlinQuery<TElement>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        IElementGremlinQuery<TElement> IGremlinQueryBaseRec<IElementGremlinQuery<TElement>>.Where(ILambda lambda) => Where(lambda);
        IElementGremlinQuery<TElement> IGremlinQueryBaseRec<IElementGremlinQuery<TElement>>.Where(Func<IElementGremlinQuery<TElement>, IGremlinQueryBase> filterTraversal) => Where(filterTraversal);

        TTargetQuery IGremlinQueryBaseRec<TElement, IEdgeOrVertexGremlinQuery<TElement>>.Aggregate<TTargetQuery>(Func<IEdgeOrVertexGremlinQuery<TElement>, StepLabel<IEdgeOrVertexGremlinQuery<TElement>, TElement[]>, TTargetQuery> continuation) => Aggregate(Scope.Local, new StepLabel<IEdgeOrVertexGremlinQuery<TElement>, TElement[]>(), continuation);
        TTargetQuery IGremlinQueryBaseRec<TElement, IEdgeOrVertexGremlinQuery<TElement>>.AggregateGlobal<TTargetQuery>(Func<IEdgeOrVertexGremlinQuery<TElement>, StepLabel<IEdgeOrVertexGremlinQuery<TElement>, TElement[]>, TTargetQuery> continuation) => Aggregate(Scope.Global, new StepLabel<IEdgeOrVertexGremlinQuery<TElement>, TElement[]>(), continuation);

        TTargetQuery IGremlinQueryBaseRec<TElement, IEdgeOrVertexGremlinQuery<TElement>>.As<TTargetQuery>(Func<IEdgeOrVertexGremlinQuery<TElement>, StepLabel<IEdgeOrVertexGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => As(new StepLabel<IEdgeOrVertexGremlinQuery<TElement>, TElement>(), continuation);
        IEdgeOrVertexGremlinQuery<TElement> IGremlinQueryBaseRec<TElement, IEdgeOrVertexGremlinQuery<TElement>>.As<TTargetQuery>(StepLabel<IEdgeOrVertexGremlinQuery<TElement>, TElement> stepLabel) => As(stepLabel);
        IEdgeOrVertexGremlinQuery<TElement> IEdgeOrVertexGremlinQueryBase<TElement>.As(StepLabel<TElement> stepLabel) => As(stepLabel);

        IEdgeOrVertexGremlinQuery<TElement> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<TElement>>.And(params Func<IEdgeOrVertexGremlinQuery<TElement>, IGremlinQueryBase>[] andTraversals) => And(andTraversals);

        IEdgeOrVertexGremlinQuery<TElement> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<TElement>>.Barrier() => Barrier();

        TTargetQuery IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<TElement>>.Choose<TTargetQuery>(Func<IEdgeOrVertexGremlinQuery<TElement>, IGremlinQueryBase> traversalPredicate, Func<IEdgeOrVertexGremlinQuery<TElement>, TTargetQuery> trueChoice, Func<IEdgeOrVertexGremlinQuery<TElement>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);
        TTargetQuery IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<TElement>>.Choose<TTargetQuery>(Func<IEdgeOrVertexGremlinQuery<TElement>, IGremlinQueryBase> traversalPredicate, Func<IEdgeOrVertexGremlinQuery<TElement>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);
        TTargetQuery IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<TElement>>.Choose<TTargetQuery>(Func<IChooseBuilder<IEdgeOrVertexGremlinQuery<TElement>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);

        TTargetQuery IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<TElement>>.Coalesce<TTargetQuery>(params Func<IEdgeOrVertexGremlinQuery<TElement>, TTargetQuery>[] traversals) => Coalesce(traversals);

        IEdgeOrVertexGremlinQuery<TElement> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<TElement>>.Coin(double probability) => Coin(probability);

        IEdgeOrVertexGremlinQuery<TElement> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<TElement>>.Dedup() => Dedup();

        IEdgeOrVertexGremlinQuery<TElement> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<TElement>>.Emit() => Emit();

        TTargetQuery IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<TElement>>.FlatMap<TTargetQuery>(Func<IEdgeOrVertexGremlinQuery<TElement>, TTargetQuery> mapping) => FlatMap(mapping);

        IArrayGremlinQuery<TElement[], IEdgeOrVertexGremlinQuery<TElement>> IGremlinQueryBaseRec<TElement, IEdgeOrVertexGremlinQuery<TElement>>.Fold() => Fold<IEdgeOrVertexGremlinQuery<TElement>>();

        IValueGremlinQuery<IDictionary<TNewKey, TNewValue>> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<TElement>>.Group<TNewKey, TNewValue>(Func<IGroupBuilder<IEdgeOrVertexGremlinQuery<TElement>>, IGroupBuilderWithKeyAndValue<IEdgeOrVertexGremlinQuery<TElement>, TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);
        IValueGremlinQuery<IDictionary<TNewKey, object>> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<TElement>>.Group<TNewKey>(Func<IGroupBuilder<IEdgeOrVertexGremlinQuery<TElement>>, IGroupBuilderWithKey<IEdgeOrVertexGremlinQuery<TElement>, TNewKey>> groupBuilder) => Group(groupBuilder);

        IEdgeOrVertexGremlinQuery<TElement> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<TElement>>.Identity() => Identity();

        IEdgeOrVertexGremlinQuery<TElement> IGremlinQueryBaseRec<TElement, IEdgeOrVertexGremlinQuery<TElement>>.Inject(params TElement[] elements) => Inject(elements);
        
        IEdgeOrVertexGremlinQuery<TElement> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<TElement>>.Limit(long count) => Limit(count);

        IEdgeOrVertexGremlinQuery<TElement> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<TElement>>.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<TElement>>.Local<TTargetQuery>(Func<IEdgeOrVertexGremlinQuery<TElement> , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<TElement>>.Map<TTargetQuery>(Func<IEdgeOrVertexGremlinQuery<TElement>, TTargetQuery> mapping) => Map(mapping);
        
        IEdgeOrVertexGremlinQuery<TElement> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<TElement>>.Not(Func<IEdgeOrVertexGremlinQuery<TElement>, IGremlinQueryBase> notTraversal) => Not(notTraversal);

        IEdgeOrVertexGremlinQuery<TElement> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<TElement>>.None() => None();

        IEdgeOrVertexGremlinQuery<TElement> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<TElement>>.Optional(Func<IEdgeOrVertexGremlinQuery<TElement>, IEdgeOrVertexGremlinQuery<TElement>> optionalTraversal) => Optional(optionalTraversal);

        IEdgeOrVertexGremlinQuery<TElement> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<TElement>>.Or(params Func<IEdgeOrVertexGremlinQuery<TElement>, IGremlinQueryBase>[] orTraversals) => Or(orTraversals);

        IValueGremlinQuery<dynamic> IGremlinQueryBaseRec<TElement, IEdgeOrVertexGremlinQuery<TElement>>.Project(Func<IProjectBuilder<IEdgeOrVertexGremlinQuery<TElement>, TElement>, IProjectResult> continuation) => Project<TElement, dynamic>(continuation);
        IValueGremlinQuery<TResult> IGremlinQueryBaseRec<TElement, IEdgeOrVertexGremlinQuery<TElement>>.Project<TResult>(Func<IProjectBuilder<IEdgeOrVertexGremlinQuery<TElement>, TElement>, IProjectResult<TResult>> continuation) => Project<TElement, TResult>(continuation);

        IEdgeOrVertexGremlinQuery<TElement> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<TElement>>.Range(long low, long high) => Range(low, high);

        IEdgeOrVertexGremlinQuery<TElement> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<TElement>>.Repeat(Func<IEdgeOrVertexGremlinQuery<TElement>, IEdgeOrVertexGremlinQuery<TElement>> repeatTraversal) => Repeat(repeatTraversal);

        IEdgeOrVertexGremlinQuery<TElement> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<TElement>>.RepeatUntil(Func<IEdgeOrVertexGremlinQuery<TElement>, IEdgeOrVertexGremlinQuery<TElement>> repeatTraversal, Func<IEdgeOrVertexGremlinQuery<TElement>, IGremlinQueryBase> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        IEdgeOrVertexGremlinQuery<TElement> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<TElement>>.UntilRepeat(Func<IEdgeOrVertexGremlinQuery<TElement>, IEdgeOrVertexGremlinQuery<TElement>> repeatTraversal, Func<IEdgeOrVertexGremlinQuery<TElement>, IGremlinQueryBase> untilTraversal) => UntilRepeat(repeatTraversal, untilTraversal);

        IEdgeOrVertexGremlinQuery<TElement> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<TElement>>.SideEffect(Func<IEdgeOrVertexGremlinQuery<TElement>, IGremlinQueryBase> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IEdgeOrVertexGremlinQuery<TElement> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<TElement>>.Skip(long count) => Skip(count);

        IEdgeOrVertexGremlinQuery<TElement> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<TElement>>.Tail(long count) => Tail(count);
        IEdgeOrVertexGremlinQuery<TElement> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<TElement>>.TailLocal(int count) => TailLocal(count);

        IEdgeOrVertexGremlinQuery<TElement> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<TElement>>.Times(int count) => Times(count);

        TTargetQuery IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<TElement>>.Union<TTargetQuery>(params Func<IEdgeOrVertexGremlinQuery<TElement>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        IEdgeOrVertexGremlinQuery<TElement> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<TElement>>.Where(ILambda lambda) => Where(lambda);
        IEdgeOrVertexGremlinQuery<TElement> IGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<TElement>>.Where(Func<IEdgeOrVertexGremlinQuery<TElement>, IGremlinQueryBase> filterTraversal) => Where(filterTraversal);

        TTargetQuery IGremlinQueryBaseRec<TElement, IVertexGremlinQuery<TElement>>.Aggregate<TTargetQuery>(Func<IVertexGremlinQuery<TElement>, StepLabel<IVertexGremlinQuery<TElement>, TElement[]>, TTargetQuery> continuation) => Aggregate(Scope.Local, new StepLabel<IVertexGremlinQuery<TElement>, TElement[]>(), continuation);
        TTargetQuery IGremlinQueryBaseRec<TElement, IVertexGremlinQuery<TElement>>.AggregateGlobal<TTargetQuery>(Func<IVertexGremlinQuery<TElement>, StepLabel<IVertexGremlinQuery<TElement>, TElement[]>, TTargetQuery> continuation) => Aggregate(Scope.Global, new StepLabel<IVertexGremlinQuery<TElement>, TElement[]>(), continuation);

        TTargetQuery IGremlinQueryBaseRec<TElement, IVertexGremlinQuery<TElement>>.As<TTargetQuery>(Func<IVertexGremlinQuery<TElement>, StepLabel<IVertexGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => As(new StepLabel<IVertexGremlinQuery<TElement>, TElement>(), continuation);
        IVertexGremlinQuery<TElement> IGremlinQueryBaseRec<TElement, IVertexGremlinQuery<TElement>>.As<TTargetQuery>(StepLabel<IVertexGremlinQuery<TElement>, TElement> stepLabel) => As(stepLabel);
        IVertexGremlinQuery<TElement> IVertexGremlinQueryBase<TElement>.As(StepLabel<TElement> stepLabel) => As(stepLabel);

        IVertexGremlinQuery<TElement> IGremlinQueryBaseRec<IVertexGremlinQuery<TElement>>.And(params Func<IVertexGremlinQuery<TElement>, IGremlinQueryBase>[] andTraversals) => And(andTraversals);

        IVertexGremlinQuery<TElement> IGremlinQueryBaseRec<IVertexGremlinQuery<TElement>>.Barrier() => Barrier();

        TTargetQuery IGremlinQueryBaseRec<IVertexGremlinQuery<TElement>>.Choose<TTargetQuery>(Func<IVertexGremlinQuery<TElement>, IGremlinQueryBase> traversalPredicate, Func<IVertexGremlinQuery<TElement>, TTargetQuery> trueChoice, Func<IVertexGremlinQuery<TElement>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);
        TTargetQuery IGremlinQueryBaseRec<IVertexGremlinQuery<TElement>>.Choose<TTargetQuery>(Func<IVertexGremlinQuery<TElement>, IGremlinQueryBase> traversalPredicate, Func<IVertexGremlinQuery<TElement>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);
        TTargetQuery IGremlinQueryBaseRec<IVertexGremlinQuery<TElement>>.Choose<TTargetQuery>(Func<IChooseBuilder<IVertexGremlinQuery<TElement>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);

        TTargetQuery IGremlinQueryBaseRec<IVertexGremlinQuery<TElement>>.Coalesce<TTargetQuery>(params Func<IVertexGremlinQuery<TElement>, TTargetQuery>[] traversals) => Coalesce(traversals);

        IVertexGremlinQuery<TElement> IGremlinQueryBaseRec<IVertexGremlinQuery<TElement>>.Coin(double probability) => Coin(probability);

        IVertexGremlinQuery<TElement> IGremlinQueryBaseRec<IVertexGremlinQuery<TElement>>.Dedup() => Dedup();

        IVertexGremlinQuery<TElement> IGremlinQueryBaseRec<IVertexGremlinQuery<TElement>>.Emit() => Emit();

        TTargetQuery IGremlinQueryBaseRec<IVertexGremlinQuery<TElement>>.FlatMap<TTargetQuery>(Func<IVertexGremlinQuery<TElement>, TTargetQuery> mapping) => FlatMap(mapping);

        IArrayGremlinQuery<TElement[], IVertexGremlinQuery<TElement>> IGremlinQueryBaseRec<TElement, IVertexGremlinQuery<TElement>>.Fold() => Fold<IVertexGremlinQuery<TElement>>();

        IValueGremlinQuery<IDictionary<TNewKey, TNewValue>> IGremlinQueryBaseRec<IVertexGremlinQuery<TElement>>.Group<TNewKey, TNewValue>(Func<IGroupBuilder<IVertexGremlinQuery<TElement>>, IGroupBuilderWithKeyAndValue<IVertexGremlinQuery<TElement>, TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);
        IValueGremlinQuery<IDictionary<TNewKey, object>> IGremlinQueryBaseRec<IVertexGremlinQuery<TElement>>.Group<TNewKey>(Func<IGroupBuilder<IVertexGremlinQuery<TElement>>, IGroupBuilderWithKey<IVertexGremlinQuery<TElement>, TNewKey>> groupBuilder) => Group(groupBuilder);

        IVertexGremlinQuery<TElement> IGremlinQueryBaseRec<IVertexGremlinQuery<TElement>>.Identity() => Identity();

        IVertexGremlinQuery<TElement> IGremlinQueryBaseRec<TElement, IVertexGremlinQuery<TElement>>.Inject(params TElement[] elements) => Inject(elements);
        
        IVertexGremlinQuery<TElement> IGremlinQueryBaseRec<IVertexGremlinQuery<TElement>>.Limit(long count) => Limit(count);

        IVertexGremlinQuery<TElement> IGremlinQueryBaseRec<IVertexGremlinQuery<TElement>>.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IGremlinQueryBaseRec<IVertexGremlinQuery<TElement>>.Local<TTargetQuery>(Func<IVertexGremlinQuery<TElement> , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IGremlinQueryBaseRec<IVertexGremlinQuery<TElement>>.Map<TTargetQuery>(Func<IVertexGremlinQuery<TElement>, TTargetQuery> mapping) => Map(mapping);
        
        IVertexGremlinQuery<TElement> IGremlinQueryBaseRec<IVertexGremlinQuery<TElement>>.Not(Func<IVertexGremlinQuery<TElement>, IGremlinQueryBase> notTraversal) => Not(notTraversal);

        IVertexGremlinQuery<TElement> IGremlinQueryBaseRec<IVertexGremlinQuery<TElement>>.None() => None();

        IVertexGremlinQuery<TElement> IGremlinQueryBaseRec<IVertexGremlinQuery<TElement>>.Optional(Func<IVertexGremlinQuery<TElement>, IVertexGremlinQuery<TElement>> optionalTraversal) => Optional(optionalTraversal);

        IVertexGremlinQuery<TElement> IGremlinQueryBaseRec<IVertexGremlinQuery<TElement>>.Or(params Func<IVertexGremlinQuery<TElement>, IGremlinQueryBase>[] orTraversals) => Or(orTraversals);

        IValueGremlinQuery<dynamic> IGremlinQueryBaseRec<TElement, IVertexGremlinQuery<TElement>>.Project(Func<IProjectBuilder<IVertexGremlinQuery<TElement>, TElement>, IProjectResult> continuation) => Project<TElement, dynamic>(continuation);
        IValueGremlinQuery<TResult> IGremlinQueryBaseRec<TElement, IVertexGremlinQuery<TElement>>.Project<TResult>(Func<IProjectBuilder<IVertexGremlinQuery<TElement>, TElement>, IProjectResult<TResult>> continuation) => Project<TElement, TResult>(continuation);

        IVertexGremlinQuery<TElement> IGremlinQueryBaseRec<IVertexGremlinQuery<TElement>>.Range(long low, long high) => Range(low, high);

        IVertexGremlinQuery<TElement> IGremlinQueryBaseRec<IVertexGremlinQuery<TElement>>.Repeat(Func<IVertexGremlinQuery<TElement>, IVertexGremlinQuery<TElement>> repeatTraversal) => Repeat(repeatTraversal);

        IVertexGremlinQuery<TElement> IGremlinQueryBaseRec<IVertexGremlinQuery<TElement>>.RepeatUntil(Func<IVertexGremlinQuery<TElement>, IVertexGremlinQuery<TElement>> repeatTraversal, Func<IVertexGremlinQuery<TElement>, IGremlinQueryBase> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        IVertexGremlinQuery<TElement> IGremlinQueryBaseRec<IVertexGremlinQuery<TElement>>.UntilRepeat(Func<IVertexGremlinQuery<TElement>, IVertexGremlinQuery<TElement>> repeatTraversal, Func<IVertexGremlinQuery<TElement>, IGremlinQueryBase> untilTraversal) => UntilRepeat(repeatTraversal, untilTraversal);

        IVertexGremlinQuery<TElement> IGremlinQueryBaseRec<IVertexGremlinQuery<TElement>>.SideEffect(Func<IVertexGremlinQuery<TElement>, IGremlinQueryBase> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IVertexGremlinQuery<TElement> IGremlinQueryBaseRec<IVertexGremlinQuery<TElement>>.Skip(long count) => Skip(count);

        IVertexGremlinQuery<TElement> IGremlinQueryBaseRec<IVertexGremlinQuery<TElement>>.Tail(long count) => Tail(count);
        IVertexGremlinQuery<TElement> IGremlinQueryBaseRec<IVertexGremlinQuery<TElement>>.TailLocal(int count) => TailLocal(count);

        IVertexGremlinQuery<TElement> IGremlinQueryBaseRec<IVertexGremlinQuery<TElement>>.Times(int count) => Times(count);

        TTargetQuery IGremlinQueryBaseRec<IVertexGremlinQuery<TElement>>.Union<TTargetQuery>(params Func<IVertexGremlinQuery<TElement>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        IVertexGremlinQuery<TElement> IGremlinQueryBaseRec<IVertexGremlinQuery<TElement>>.Where(ILambda lambda) => Where(lambda);
        IVertexGremlinQuery<TElement> IGremlinQueryBaseRec<IVertexGremlinQuery<TElement>>.Where(Func<IVertexGremlinQuery<TElement>, IGremlinQueryBase> filterTraversal) => Where(filterTraversal);

        TTargetQuery IGremlinQueryBaseRec<TElement, IEdgeGremlinQuery<TElement>>.Aggregate<TTargetQuery>(Func<IEdgeGremlinQuery<TElement>, StepLabel<IEdgeGremlinQuery<TElement>, TElement[]>, TTargetQuery> continuation) => Aggregate(Scope.Local, new StepLabel<IEdgeGremlinQuery<TElement>, TElement[]>(), continuation);
        TTargetQuery IGremlinQueryBaseRec<TElement, IEdgeGremlinQuery<TElement>>.AggregateGlobal<TTargetQuery>(Func<IEdgeGremlinQuery<TElement>, StepLabel<IEdgeGremlinQuery<TElement>, TElement[]>, TTargetQuery> continuation) => Aggregate(Scope.Global, new StepLabel<IEdgeGremlinQuery<TElement>, TElement[]>(), continuation);

        TTargetQuery IGremlinQueryBaseRec<TElement, IEdgeGremlinQuery<TElement>>.As<TTargetQuery>(Func<IEdgeGremlinQuery<TElement>, StepLabel<IEdgeGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => As(new StepLabel<IEdgeGremlinQuery<TElement>, TElement>(), continuation);
        IEdgeGremlinQuery<TElement> IGremlinQueryBaseRec<TElement, IEdgeGremlinQuery<TElement>>.As<TTargetQuery>(StepLabel<IEdgeGremlinQuery<TElement>, TElement> stepLabel) => As(stepLabel);
        IEdgeGremlinQuery<TElement> IEdgeGremlinQueryBase<TElement>.As(StepLabel<TElement> stepLabel) => As(stepLabel);

        IEdgeGremlinQuery<TElement> IGremlinQueryBaseRec<IEdgeGremlinQuery<TElement>>.And(params Func<IEdgeGremlinQuery<TElement>, IGremlinQueryBase>[] andTraversals) => And(andTraversals);

        IEdgeGremlinQuery<TElement> IGremlinQueryBaseRec<IEdgeGremlinQuery<TElement>>.Barrier() => Barrier();

        TTargetQuery IGremlinQueryBaseRec<IEdgeGremlinQuery<TElement>>.Choose<TTargetQuery>(Func<IEdgeGremlinQuery<TElement>, IGremlinQueryBase> traversalPredicate, Func<IEdgeGremlinQuery<TElement>, TTargetQuery> trueChoice, Func<IEdgeGremlinQuery<TElement>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);
        TTargetQuery IGremlinQueryBaseRec<IEdgeGremlinQuery<TElement>>.Choose<TTargetQuery>(Func<IEdgeGremlinQuery<TElement>, IGremlinQueryBase> traversalPredicate, Func<IEdgeGremlinQuery<TElement>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);
        TTargetQuery IGremlinQueryBaseRec<IEdgeGremlinQuery<TElement>>.Choose<TTargetQuery>(Func<IChooseBuilder<IEdgeGremlinQuery<TElement>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);

        TTargetQuery IGremlinQueryBaseRec<IEdgeGremlinQuery<TElement>>.Coalesce<TTargetQuery>(params Func<IEdgeGremlinQuery<TElement>, TTargetQuery>[] traversals) => Coalesce(traversals);

        IEdgeGremlinQuery<TElement> IGremlinQueryBaseRec<IEdgeGremlinQuery<TElement>>.Coin(double probability) => Coin(probability);

        IEdgeGremlinQuery<TElement> IGremlinQueryBaseRec<IEdgeGremlinQuery<TElement>>.Dedup() => Dedup();

        IEdgeGremlinQuery<TElement> IGremlinQueryBaseRec<IEdgeGremlinQuery<TElement>>.Emit() => Emit();

        TTargetQuery IGremlinQueryBaseRec<IEdgeGremlinQuery<TElement>>.FlatMap<TTargetQuery>(Func<IEdgeGremlinQuery<TElement>, TTargetQuery> mapping) => FlatMap(mapping);

        IArrayGremlinQuery<TElement[], IEdgeGremlinQuery<TElement>> IGremlinQueryBaseRec<TElement, IEdgeGremlinQuery<TElement>>.Fold() => Fold<IEdgeGremlinQuery<TElement>>();

        IValueGremlinQuery<IDictionary<TNewKey, TNewValue>> IGremlinQueryBaseRec<IEdgeGremlinQuery<TElement>>.Group<TNewKey, TNewValue>(Func<IGroupBuilder<IEdgeGremlinQuery<TElement>>, IGroupBuilderWithKeyAndValue<IEdgeGremlinQuery<TElement>, TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);
        IValueGremlinQuery<IDictionary<TNewKey, object>> IGremlinQueryBaseRec<IEdgeGremlinQuery<TElement>>.Group<TNewKey>(Func<IGroupBuilder<IEdgeGremlinQuery<TElement>>, IGroupBuilderWithKey<IEdgeGremlinQuery<TElement>, TNewKey>> groupBuilder) => Group(groupBuilder);

        IEdgeGremlinQuery<TElement> IGremlinQueryBaseRec<IEdgeGremlinQuery<TElement>>.Identity() => Identity();

        IEdgeGremlinQuery<TElement> IGremlinQueryBaseRec<TElement, IEdgeGremlinQuery<TElement>>.Inject(params TElement[] elements) => Inject(elements);
        
        IEdgeGremlinQuery<TElement> IGremlinQueryBaseRec<IEdgeGremlinQuery<TElement>>.Limit(long count) => Limit(count);

        IEdgeGremlinQuery<TElement> IGremlinQueryBaseRec<IEdgeGremlinQuery<TElement>>.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IGremlinQueryBaseRec<IEdgeGremlinQuery<TElement>>.Local<TTargetQuery>(Func<IEdgeGremlinQuery<TElement> , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IGremlinQueryBaseRec<IEdgeGremlinQuery<TElement>>.Map<TTargetQuery>(Func<IEdgeGremlinQuery<TElement>, TTargetQuery> mapping) => Map(mapping);
        
        IEdgeGremlinQuery<TElement> IGremlinQueryBaseRec<IEdgeGremlinQuery<TElement>>.Not(Func<IEdgeGremlinQuery<TElement>, IGremlinQueryBase> notTraversal) => Not(notTraversal);

        IEdgeGremlinQuery<TElement> IGremlinQueryBaseRec<IEdgeGremlinQuery<TElement>>.None() => None();

        IEdgeGremlinQuery<TElement> IGremlinQueryBaseRec<IEdgeGremlinQuery<TElement>>.Optional(Func<IEdgeGremlinQuery<TElement>, IEdgeGremlinQuery<TElement>> optionalTraversal) => Optional(optionalTraversal);

        IEdgeGremlinQuery<TElement> IGremlinQueryBaseRec<IEdgeGremlinQuery<TElement>>.Or(params Func<IEdgeGremlinQuery<TElement>, IGremlinQueryBase>[] orTraversals) => Or(orTraversals);

        IValueGremlinQuery<dynamic> IGremlinQueryBaseRec<TElement, IEdgeGremlinQuery<TElement>>.Project(Func<IProjectBuilder<IEdgeGremlinQuery<TElement>, TElement>, IProjectResult> continuation) => Project<TElement, dynamic>(continuation);
        IValueGremlinQuery<TResult> IGremlinQueryBaseRec<TElement, IEdgeGremlinQuery<TElement>>.Project<TResult>(Func<IProjectBuilder<IEdgeGremlinQuery<TElement>, TElement>, IProjectResult<TResult>> continuation) => Project<TElement, TResult>(continuation);

        IEdgeGremlinQuery<TElement> IGremlinQueryBaseRec<IEdgeGremlinQuery<TElement>>.Range(long low, long high) => Range(low, high);

        IEdgeGremlinQuery<TElement> IGremlinQueryBaseRec<IEdgeGremlinQuery<TElement>>.Repeat(Func<IEdgeGremlinQuery<TElement>, IEdgeGremlinQuery<TElement>> repeatTraversal) => Repeat(repeatTraversal);

        IEdgeGremlinQuery<TElement> IGremlinQueryBaseRec<IEdgeGremlinQuery<TElement>>.RepeatUntil(Func<IEdgeGremlinQuery<TElement>, IEdgeGremlinQuery<TElement>> repeatTraversal, Func<IEdgeGremlinQuery<TElement>, IGremlinQueryBase> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        IEdgeGremlinQuery<TElement> IGremlinQueryBaseRec<IEdgeGremlinQuery<TElement>>.UntilRepeat(Func<IEdgeGremlinQuery<TElement>, IEdgeGremlinQuery<TElement>> repeatTraversal, Func<IEdgeGremlinQuery<TElement>, IGremlinQueryBase> untilTraversal) => UntilRepeat(repeatTraversal, untilTraversal);

        IEdgeGremlinQuery<TElement> IGremlinQueryBaseRec<IEdgeGremlinQuery<TElement>>.SideEffect(Func<IEdgeGremlinQuery<TElement>, IGremlinQueryBase> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IEdgeGremlinQuery<TElement> IGremlinQueryBaseRec<IEdgeGremlinQuery<TElement>>.Skip(long count) => Skip(count);

        IEdgeGremlinQuery<TElement> IGremlinQueryBaseRec<IEdgeGremlinQuery<TElement>>.Tail(long count) => Tail(count);
        IEdgeGremlinQuery<TElement> IGremlinQueryBaseRec<IEdgeGremlinQuery<TElement>>.TailLocal(int count) => TailLocal(count);

        IEdgeGremlinQuery<TElement> IGremlinQueryBaseRec<IEdgeGremlinQuery<TElement>>.Times(int count) => Times(count);

        TTargetQuery IGremlinQueryBaseRec<IEdgeGremlinQuery<TElement>>.Union<TTargetQuery>(params Func<IEdgeGremlinQuery<TElement>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        IEdgeGremlinQuery<TElement> IGremlinQueryBaseRec<IEdgeGremlinQuery<TElement>>.Where(ILambda lambda) => Where(lambda);
        IEdgeGremlinQuery<TElement> IGremlinQueryBaseRec<IEdgeGremlinQuery<TElement>>.Where(Func<IEdgeGremlinQuery<TElement>, IGremlinQueryBase> filterTraversal) => Where(filterTraversal);

        TTargetQuery IGremlinQueryBaseRec<TElement, IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>.Aggregate<TTargetQuery>(Func<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>, StepLabel<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>, TElement[]>, TTargetQuery> continuation) => Aggregate(Scope.Local, new StepLabel<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>, TElement[]>(), continuation);
        TTargetQuery IGremlinQueryBaseRec<TElement, IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>.AggregateGlobal<TTargetQuery>(Func<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>, StepLabel<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>, TElement[]>, TTargetQuery> continuation) => Aggregate(Scope.Global, new StepLabel<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>, TElement[]>(), continuation);

        TTargetQuery IGremlinQueryBaseRec<TElement, IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>.As<TTargetQuery>(Func<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>, StepLabel<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>, TElement>, TTargetQuery> continuation) => As(new StepLabel<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>, TElement>(), continuation);
        IInOrOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<TElement, IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>.As<TTargetQuery>(StepLabel<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>, TElement> stepLabel) => As(stepLabel);
        IInOrOutEdgeGremlinQuery<TElement, TOutVertex> IInOrOutEdgeGremlinQueryBase<TElement, TOutVertex>.As(StepLabel<TElement> stepLabel) => As(stepLabel);

        IInOrOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>.And(params Func<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQueryBase>[] andTraversals) => And(andTraversals);

        IInOrOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>.Barrier() => Barrier();

        TTargetQuery IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>.Choose<TTargetQuery>(Func<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQueryBase> traversalPredicate, Func<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery> trueChoice, Func<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);
        TTargetQuery IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>.Choose<TTargetQuery>(Func<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQueryBase> traversalPredicate, Func<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);
        TTargetQuery IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>.Choose<TTargetQuery>(Func<IChooseBuilder<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);

        TTargetQuery IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>.Coalesce<TTargetQuery>(params Func<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery>[] traversals) => Coalesce(traversals);

        IInOrOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>.Coin(double probability) => Coin(probability);

        IInOrOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>.Dedup() => Dedup();

        IInOrOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>.Emit() => Emit();

        TTargetQuery IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>.FlatMap<TTargetQuery>(Func<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery> mapping) => FlatMap(mapping);

        IArrayGremlinQuery<TElement[], IInOrOutEdgeGremlinQuery<TElement, TOutVertex>> IGremlinQueryBaseRec<TElement, IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>.Fold() => Fold<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>();

        IValueGremlinQuery<IDictionary<TNewKey, TNewValue>> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>.Group<TNewKey, TNewValue>(Func<IGroupBuilder<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>, IGroupBuilderWithKeyAndValue<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>, TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);
        IValueGremlinQuery<IDictionary<TNewKey, object>> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>.Group<TNewKey>(Func<IGroupBuilder<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>, IGroupBuilderWithKey<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>, TNewKey>> groupBuilder) => Group(groupBuilder);

        IInOrOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>.Identity() => Identity();

        IInOrOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<TElement, IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>.Inject(params TElement[] elements) => Inject(elements);
        
        IInOrOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>.Limit(long count) => Limit(count);

        IInOrOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>.Local<TTargetQuery>(Func<IInOrOutEdgeGremlinQuery<TElement, TOutVertex> , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>.Map<TTargetQuery>(Func<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery> mapping) => Map(mapping);
        
        IInOrOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>.Not(Func<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQueryBase> notTraversal) => Not(notTraversal);

        IInOrOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>.None() => None();

        IInOrOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>.Optional(Func<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>, IInOrOutEdgeGremlinQuery<TElement, TOutVertex>> optionalTraversal) => Optional(optionalTraversal);

        IInOrOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>.Or(params Func<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQueryBase>[] orTraversals) => Or(orTraversals);

        IValueGremlinQuery<dynamic> IGremlinQueryBaseRec<TElement, IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>.Project(Func<IProjectBuilder<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>, TElement>, IProjectResult> continuation) => Project<TElement, dynamic>(continuation);
        IValueGremlinQuery<TResult> IGremlinQueryBaseRec<TElement, IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>.Project<TResult>(Func<IProjectBuilder<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>, TElement>, IProjectResult<TResult>> continuation) => Project<TElement, TResult>(continuation);

        IInOrOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>.Range(long low, long high) => Range(low, high);

        IInOrOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>.Repeat(Func<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>, IInOrOutEdgeGremlinQuery<TElement, TOutVertex>> repeatTraversal) => Repeat(repeatTraversal);

        IInOrOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>.RepeatUntil(Func<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>, IInOrOutEdgeGremlinQuery<TElement, TOutVertex>> repeatTraversal, Func<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQueryBase> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        IInOrOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>.UntilRepeat(Func<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>, IInOrOutEdgeGremlinQuery<TElement, TOutVertex>> repeatTraversal, Func<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQueryBase> untilTraversal) => UntilRepeat(repeatTraversal, untilTraversal);

        IInOrOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>.SideEffect(Func<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQueryBase> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IInOrOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>.Skip(long count) => Skip(count);

        IInOrOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>.Tail(long count) => Tail(count);
        IInOrOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>.TailLocal(int count) => TailLocal(count);

        IInOrOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>.Times(int count) => Times(count);

        TTargetQuery IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>.Union<TTargetQuery>(params Func<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        IInOrOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>.Where(ILambda lambda) => Where(lambda);
        IInOrOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>.Where(Func<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQueryBase> filterTraversal) => Where(filterTraversal);

        TTargetQuery IGremlinQueryBaseRec<TElement, IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>.Aggregate<TTargetQuery>(Func<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, StepLabel<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TElement[]>, TTargetQuery> continuation) => Aggregate(Scope.Local, new StepLabel<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TElement[]>(), continuation);
        TTargetQuery IGremlinQueryBaseRec<TElement, IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>.AggregateGlobal<TTargetQuery>(Func<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, StepLabel<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TElement[]>, TTargetQuery> continuation) => Aggregate(Scope.Global, new StepLabel<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TElement[]>(), continuation);

        TTargetQuery IGremlinQueryBaseRec<TElement, IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>.As<TTargetQuery>(Func<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, StepLabel<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TElement>, TTargetQuery> continuation) => As(new StepLabel<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TElement>(), continuation);
        IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IGremlinQueryBaseRec<TElement, IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>.As<TTargetQuery>(StepLabel<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TElement> stepLabel) => As(stepLabel);
        IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IBothEdgeGremlinQueryBase<TElement, TOutVertex, TInVertex>.As(StepLabel<TElement> stepLabel) => As(stepLabel);

        IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IGremlinQueryBaseRec<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>.And(params Func<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQueryBase>[] andTraversals) => And(andTraversals);

        IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IGremlinQueryBaseRec<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>.Barrier() => Barrier();

        TTargetQuery IGremlinQueryBaseRec<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>.Choose<TTargetQuery>(Func<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQueryBase> traversalPredicate, Func<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TTargetQuery> trueChoice, Func<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);
        TTargetQuery IGremlinQueryBaseRec<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>.Choose<TTargetQuery>(Func<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQueryBase> traversalPredicate, Func<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);
        TTargetQuery IGremlinQueryBaseRec<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>.Choose<TTargetQuery>(Func<IChooseBuilder<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);

        TTargetQuery IGremlinQueryBaseRec<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>.Coalesce<TTargetQuery>(params Func<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TTargetQuery>[] traversals) => Coalesce(traversals);

        IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IGremlinQueryBaseRec<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>.Coin(double probability) => Coin(probability);

        IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IGremlinQueryBaseRec<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>.Dedup() => Dedup();

        IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IGremlinQueryBaseRec<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>.Emit() => Emit();

        TTargetQuery IGremlinQueryBaseRec<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>.FlatMap<TTargetQuery>(Func<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TTargetQuery> mapping) => FlatMap(mapping);

        IArrayGremlinQuery<TElement[], IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>> IGremlinQueryBaseRec<TElement, IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>.Fold() => Fold<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>();

        IValueGremlinQuery<IDictionary<TNewKey, TNewValue>> IGremlinQueryBaseRec<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>.Group<TNewKey, TNewValue>(Func<IGroupBuilder<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>, IGroupBuilderWithKeyAndValue<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);
        IValueGremlinQuery<IDictionary<TNewKey, object>> IGremlinQueryBaseRec<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>.Group<TNewKey>(Func<IGroupBuilder<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>, IGroupBuilderWithKey<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TNewKey>> groupBuilder) => Group(groupBuilder);

        IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IGremlinQueryBaseRec<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>.Identity() => Identity();

        IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IGremlinQueryBaseRec<TElement, IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>.Inject(params TElement[] elements) => Inject(elements);
        
        IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IGremlinQueryBaseRec<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>.Limit(long count) => Limit(count);

        IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IGremlinQueryBaseRec<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IGremlinQueryBaseRec<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>.Local<TTargetQuery>(Func<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex> , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IGremlinQueryBaseRec<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>.Map<TTargetQuery>(Func<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TTargetQuery> mapping) => Map(mapping);
        
        IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IGremlinQueryBaseRec<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>.Not(Func<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQueryBase> notTraversal) => Not(notTraversal);

        IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IGremlinQueryBaseRec<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>.None() => None();

        IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IGremlinQueryBaseRec<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>.Optional(Func<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>> optionalTraversal) => Optional(optionalTraversal);

        IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IGremlinQueryBaseRec<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>.Or(params Func<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQueryBase>[] orTraversals) => Or(orTraversals);

        IValueGremlinQuery<dynamic> IGremlinQueryBaseRec<TElement, IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>.Project(Func<IProjectBuilder<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TElement>, IProjectResult> continuation) => Project<TElement, dynamic>(continuation);
        IValueGremlinQuery<TResult> IGremlinQueryBaseRec<TElement, IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>.Project<TResult>(Func<IProjectBuilder<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TElement>, IProjectResult<TResult>> continuation) => Project<TElement, TResult>(continuation);

        IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IGremlinQueryBaseRec<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>.Range(long low, long high) => Range(low, high);

        IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IGremlinQueryBaseRec<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>.Repeat(Func<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>> repeatTraversal) => Repeat(repeatTraversal);

        IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IGremlinQueryBaseRec<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>.RepeatUntil(Func<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>> repeatTraversal, Func<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQueryBase> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IGremlinQueryBaseRec<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>.UntilRepeat(Func<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>> repeatTraversal, Func<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQueryBase> untilTraversal) => UntilRepeat(repeatTraversal, untilTraversal);

        IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IGremlinQueryBaseRec<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>.SideEffect(Func<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQueryBase> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IGremlinQueryBaseRec<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>.Skip(long count) => Skip(count);

        IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IGremlinQueryBaseRec<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>.Tail(long count) => Tail(count);
        IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IGremlinQueryBaseRec<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>.TailLocal(int count) => TailLocal(count);

        IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IGremlinQueryBaseRec<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>.Times(int count) => Times(count);

        TTargetQuery IGremlinQueryBaseRec<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>.Union<TTargetQuery>(params Func<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IGremlinQueryBaseRec<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>.Where(ILambda lambda) => Where(lambda);
        IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IGremlinQueryBaseRec<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>.Where(Func<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQueryBase> filterTraversal) => Where(filterTraversal);

        TTargetQuery IGremlinQueryBaseRec<TElement, IInEdgeGremlinQuery<TElement, TInVertex>>.Aggregate<TTargetQuery>(Func<IInEdgeGremlinQuery<TElement, TInVertex>, StepLabel<IInEdgeGremlinQuery<TElement, TInVertex>, TElement[]>, TTargetQuery> continuation) => Aggregate(Scope.Local, new StepLabel<IInEdgeGremlinQuery<TElement, TInVertex>, TElement[]>(), continuation);
        TTargetQuery IGremlinQueryBaseRec<TElement, IInEdgeGremlinQuery<TElement, TInVertex>>.AggregateGlobal<TTargetQuery>(Func<IInEdgeGremlinQuery<TElement, TInVertex>, StepLabel<IInEdgeGremlinQuery<TElement, TInVertex>, TElement[]>, TTargetQuery> continuation) => Aggregate(Scope.Global, new StepLabel<IInEdgeGremlinQuery<TElement, TInVertex>, TElement[]>(), continuation);

        TTargetQuery IGremlinQueryBaseRec<TElement, IInEdgeGremlinQuery<TElement, TInVertex>>.As<TTargetQuery>(Func<IInEdgeGremlinQuery<TElement, TInVertex>, StepLabel<IInEdgeGremlinQuery<TElement, TInVertex>, TElement>, TTargetQuery> continuation) => As(new StepLabel<IInEdgeGremlinQuery<TElement, TInVertex>, TElement>(), continuation);
        IInEdgeGremlinQuery<TElement, TInVertex> IGremlinQueryBaseRec<TElement, IInEdgeGremlinQuery<TElement, TInVertex>>.As<TTargetQuery>(StepLabel<IInEdgeGremlinQuery<TElement, TInVertex>, TElement> stepLabel) => As(stepLabel);
        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQueryBase<TElement, TInVertex>.As(StepLabel<TElement> stepLabel) => As(stepLabel);

        IInEdgeGremlinQuery<TElement, TInVertex> IGremlinQueryBaseRec<IInEdgeGremlinQuery<TElement, TInVertex>>.And(params Func<IInEdgeGremlinQuery<TElement, TInVertex>, IGremlinQueryBase>[] andTraversals) => And(andTraversals);

        IInEdgeGremlinQuery<TElement, TInVertex> IGremlinQueryBaseRec<IInEdgeGremlinQuery<TElement, TInVertex>>.Barrier() => Barrier();

        TTargetQuery IGremlinQueryBaseRec<IInEdgeGremlinQuery<TElement, TInVertex>>.Choose<TTargetQuery>(Func<IInEdgeGremlinQuery<TElement, TInVertex>, IGremlinQueryBase> traversalPredicate, Func<IInEdgeGremlinQuery<TElement, TInVertex>, TTargetQuery> trueChoice, Func<IInEdgeGremlinQuery<TElement, TInVertex>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);
        TTargetQuery IGremlinQueryBaseRec<IInEdgeGremlinQuery<TElement, TInVertex>>.Choose<TTargetQuery>(Func<IInEdgeGremlinQuery<TElement, TInVertex>, IGremlinQueryBase> traversalPredicate, Func<IInEdgeGremlinQuery<TElement, TInVertex>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);
        TTargetQuery IGremlinQueryBaseRec<IInEdgeGremlinQuery<TElement, TInVertex>>.Choose<TTargetQuery>(Func<IChooseBuilder<IInEdgeGremlinQuery<TElement, TInVertex>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);

        TTargetQuery IGremlinQueryBaseRec<IInEdgeGremlinQuery<TElement, TInVertex>>.Coalesce<TTargetQuery>(params Func<IInEdgeGremlinQuery<TElement, TInVertex>, TTargetQuery>[] traversals) => Coalesce(traversals);

        IInEdgeGremlinQuery<TElement, TInVertex> IGremlinQueryBaseRec<IInEdgeGremlinQuery<TElement, TInVertex>>.Coin(double probability) => Coin(probability);

        IInEdgeGremlinQuery<TElement, TInVertex> IGremlinQueryBaseRec<IInEdgeGremlinQuery<TElement, TInVertex>>.Dedup() => Dedup();

        IInEdgeGremlinQuery<TElement, TInVertex> IGremlinQueryBaseRec<IInEdgeGremlinQuery<TElement, TInVertex>>.Emit() => Emit();

        TTargetQuery IGremlinQueryBaseRec<IInEdgeGremlinQuery<TElement, TInVertex>>.FlatMap<TTargetQuery>(Func<IInEdgeGremlinQuery<TElement, TInVertex>, TTargetQuery> mapping) => FlatMap(mapping);

        IArrayGremlinQuery<TElement[], IInEdgeGremlinQuery<TElement, TInVertex>> IGremlinQueryBaseRec<TElement, IInEdgeGremlinQuery<TElement, TInVertex>>.Fold() => Fold<IInEdgeGremlinQuery<TElement, TInVertex>>();

        IValueGremlinQuery<IDictionary<TNewKey, TNewValue>> IGremlinQueryBaseRec<IInEdgeGremlinQuery<TElement, TInVertex>>.Group<TNewKey, TNewValue>(Func<IGroupBuilder<IInEdgeGremlinQuery<TElement, TInVertex>>, IGroupBuilderWithKeyAndValue<IInEdgeGremlinQuery<TElement, TInVertex>, TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);
        IValueGremlinQuery<IDictionary<TNewKey, object>> IGremlinQueryBaseRec<IInEdgeGremlinQuery<TElement, TInVertex>>.Group<TNewKey>(Func<IGroupBuilder<IInEdgeGremlinQuery<TElement, TInVertex>>, IGroupBuilderWithKey<IInEdgeGremlinQuery<TElement, TInVertex>, TNewKey>> groupBuilder) => Group(groupBuilder);

        IInEdgeGremlinQuery<TElement, TInVertex> IGremlinQueryBaseRec<IInEdgeGremlinQuery<TElement, TInVertex>>.Identity() => Identity();

        IInEdgeGremlinQuery<TElement, TInVertex> IGremlinQueryBaseRec<TElement, IInEdgeGremlinQuery<TElement, TInVertex>>.Inject(params TElement[] elements) => Inject(elements);
        
        IInEdgeGremlinQuery<TElement, TInVertex> IGremlinQueryBaseRec<IInEdgeGremlinQuery<TElement, TInVertex>>.Limit(long count) => Limit(count);

        IInEdgeGremlinQuery<TElement, TInVertex> IGremlinQueryBaseRec<IInEdgeGremlinQuery<TElement, TInVertex>>.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IGremlinQueryBaseRec<IInEdgeGremlinQuery<TElement, TInVertex>>.Local<TTargetQuery>(Func<IInEdgeGremlinQuery<TElement, TInVertex> , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IGremlinQueryBaseRec<IInEdgeGremlinQuery<TElement, TInVertex>>.Map<TTargetQuery>(Func<IInEdgeGremlinQuery<TElement, TInVertex>, TTargetQuery> mapping) => Map(mapping);
        
        IInEdgeGremlinQuery<TElement, TInVertex> IGremlinQueryBaseRec<IInEdgeGremlinQuery<TElement, TInVertex>>.Not(Func<IInEdgeGremlinQuery<TElement, TInVertex>, IGremlinQueryBase> notTraversal) => Not(notTraversal);

        IInEdgeGremlinQuery<TElement, TInVertex> IGremlinQueryBaseRec<IInEdgeGremlinQuery<TElement, TInVertex>>.None() => None();

        IInEdgeGremlinQuery<TElement, TInVertex> IGremlinQueryBaseRec<IInEdgeGremlinQuery<TElement, TInVertex>>.Optional(Func<IInEdgeGremlinQuery<TElement, TInVertex>, IInEdgeGremlinQuery<TElement, TInVertex>> optionalTraversal) => Optional(optionalTraversal);

        IInEdgeGremlinQuery<TElement, TInVertex> IGremlinQueryBaseRec<IInEdgeGremlinQuery<TElement, TInVertex>>.Or(params Func<IInEdgeGremlinQuery<TElement, TInVertex>, IGremlinQueryBase>[] orTraversals) => Or(orTraversals);

        IValueGremlinQuery<dynamic> IGremlinQueryBaseRec<TElement, IInEdgeGremlinQuery<TElement, TInVertex>>.Project(Func<IProjectBuilder<IInEdgeGremlinQuery<TElement, TInVertex>, TElement>, IProjectResult> continuation) => Project<TElement, dynamic>(continuation);
        IValueGremlinQuery<TResult> IGremlinQueryBaseRec<TElement, IInEdgeGremlinQuery<TElement, TInVertex>>.Project<TResult>(Func<IProjectBuilder<IInEdgeGremlinQuery<TElement, TInVertex>, TElement>, IProjectResult<TResult>> continuation) => Project<TElement, TResult>(continuation);

        IInEdgeGremlinQuery<TElement, TInVertex> IGremlinQueryBaseRec<IInEdgeGremlinQuery<TElement, TInVertex>>.Range(long low, long high) => Range(low, high);

        IInEdgeGremlinQuery<TElement, TInVertex> IGremlinQueryBaseRec<IInEdgeGremlinQuery<TElement, TInVertex>>.Repeat(Func<IInEdgeGremlinQuery<TElement, TInVertex>, IInEdgeGremlinQuery<TElement, TInVertex>> repeatTraversal) => Repeat(repeatTraversal);

        IInEdgeGremlinQuery<TElement, TInVertex> IGremlinQueryBaseRec<IInEdgeGremlinQuery<TElement, TInVertex>>.RepeatUntil(Func<IInEdgeGremlinQuery<TElement, TInVertex>, IInEdgeGremlinQuery<TElement, TInVertex>> repeatTraversal, Func<IInEdgeGremlinQuery<TElement, TInVertex>, IGremlinQueryBase> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        IInEdgeGremlinQuery<TElement, TInVertex> IGremlinQueryBaseRec<IInEdgeGremlinQuery<TElement, TInVertex>>.UntilRepeat(Func<IInEdgeGremlinQuery<TElement, TInVertex>, IInEdgeGremlinQuery<TElement, TInVertex>> repeatTraversal, Func<IInEdgeGremlinQuery<TElement, TInVertex>, IGremlinQueryBase> untilTraversal) => UntilRepeat(repeatTraversal, untilTraversal);

        IInEdgeGremlinQuery<TElement, TInVertex> IGremlinQueryBaseRec<IInEdgeGremlinQuery<TElement, TInVertex>>.SideEffect(Func<IInEdgeGremlinQuery<TElement, TInVertex>, IGremlinQueryBase> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IInEdgeGremlinQuery<TElement, TInVertex> IGremlinQueryBaseRec<IInEdgeGremlinQuery<TElement, TInVertex>>.Skip(long count) => Skip(count);

        IInEdgeGremlinQuery<TElement, TInVertex> IGremlinQueryBaseRec<IInEdgeGremlinQuery<TElement, TInVertex>>.Tail(long count) => Tail(count);
        IInEdgeGremlinQuery<TElement, TInVertex> IGremlinQueryBaseRec<IInEdgeGremlinQuery<TElement, TInVertex>>.TailLocal(int count) => TailLocal(count);

        IInEdgeGremlinQuery<TElement, TInVertex> IGremlinQueryBaseRec<IInEdgeGremlinQuery<TElement, TInVertex>>.Times(int count) => Times(count);

        TTargetQuery IGremlinQueryBaseRec<IInEdgeGremlinQuery<TElement, TInVertex>>.Union<TTargetQuery>(params Func<IInEdgeGremlinQuery<TElement, TInVertex>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        IInEdgeGremlinQuery<TElement, TInVertex> IGremlinQueryBaseRec<IInEdgeGremlinQuery<TElement, TInVertex>>.Where(ILambda lambda) => Where(lambda);
        IInEdgeGremlinQuery<TElement, TInVertex> IGremlinQueryBaseRec<IInEdgeGremlinQuery<TElement, TInVertex>>.Where(Func<IInEdgeGremlinQuery<TElement, TInVertex>, IGremlinQueryBase> filterTraversal) => Where(filterTraversal);

        TTargetQuery IGremlinQueryBaseRec<TElement, IOutEdgeGremlinQuery<TElement, TOutVertex>>.Aggregate<TTargetQuery>(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, StepLabel<IOutEdgeGremlinQuery<TElement, TOutVertex>, TElement[]>, TTargetQuery> continuation) => Aggregate(Scope.Local, new StepLabel<IOutEdgeGremlinQuery<TElement, TOutVertex>, TElement[]>(), continuation);
        TTargetQuery IGremlinQueryBaseRec<TElement, IOutEdgeGremlinQuery<TElement, TOutVertex>>.AggregateGlobal<TTargetQuery>(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, StepLabel<IOutEdgeGremlinQuery<TElement, TOutVertex>, TElement[]>, TTargetQuery> continuation) => Aggregate(Scope.Global, new StepLabel<IOutEdgeGremlinQuery<TElement, TOutVertex>, TElement[]>(), continuation);

        TTargetQuery IGremlinQueryBaseRec<TElement, IOutEdgeGremlinQuery<TElement, TOutVertex>>.As<TTargetQuery>(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, StepLabel<IOutEdgeGremlinQuery<TElement, TOutVertex>, TElement>, TTargetQuery> continuation) => As(new StepLabel<IOutEdgeGremlinQuery<TElement, TOutVertex>, TElement>(), continuation);
        IOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<TElement, IOutEdgeGremlinQuery<TElement, TOutVertex>>.As<TTargetQuery>(StepLabel<IOutEdgeGremlinQuery<TElement, TOutVertex>, TElement> stepLabel) => As(stepLabel);
        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQueryBase<TElement, TOutVertex>.As(StepLabel<TElement> stepLabel) => As(stepLabel);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<TElement, TOutVertex>>.And(params Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQueryBase>[] andTraversals) => And(andTraversals);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<TElement, TOutVertex>>.Barrier() => Barrier();

        TTargetQuery IGremlinQueryBaseRec<IOutEdgeGremlinQuery<TElement, TOutVertex>>.Choose<TTargetQuery>(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQueryBase> traversalPredicate, Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery> trueChoice, Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);
        TTargetQuery IGremlinQueryBaseRec<IOutEdgeGremlinQuery<TElement, TOutVertex>>.Choose<TTargetQuery>(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQueryBase> traversalPredicate, Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);
        TTargetQuery IGremlinQueryBaseRec<IOutEdgeGremlinQuery<TElement, TOutVertex>>.Choose<TTargetQuery>(Func<IChooseBuilder<IOutEdgeGremlinQuery<TElement, TOutVertex>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);

        TTargetQuery IGremlinQueryBaseRec<IOutEdgeGremlinQuery<TElement, TOutVertex>>.Coalesce<TTargetQuery>(params Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery>[] traversals) => Coalesce(traversals);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<TElement, TOutVertex>>.Coin(double probability) => Coin(probability);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<TElement, TOutVertex>>.Dedup() => Dedup();

        IOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<TElement, TOutVertex>>.Emit() => Emit();

        TTargetQuery IGremlinQueryBaseRec<IOutEdgeGremlinQuery<TElement, TOutVertex>>.FlatMap<TTargetQuery>(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery> mapping) => FlatMap(mapping);

        IArrayGremlinQuery<TElement[], IOutEdgeGremlinQuery<TElement, TOutVertex>> IGremlinQueryBaseRec<TElement, IOutEdgeGremlinQuery<TElement, TOutVertex>>.Fold() => Fold<IOutEdgeGremlinQuery<TElement, TOutVertex>>();

        IValueGremlinQuery<IDictionary<TNewKey, TNewValue>> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<TElement, TOutVertex>>.Group<TNewKey, TNewValue>(Func<IGroupBuilder<IOutEdgeGremlinQuery<TElement, TOutVertex>>, IGroupBuilderWithKeyAndValue<IOutEdgeGremlinQuery<TElement, TOutVertex>, TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);
        IValueGremlinQuery<IDictionary<TNewKey, object>> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<TElement, TOutVertex>>.Group<TNewKey>(Func<IGroupBuilder<IOutEdgeGremlinQuery<TElement, TOutVertex>>, IGroupBuilderWithKey<IOutEdgeGremlinQuery<TElement, TOutVertex>, TNewKey>> groupBuilder) => Group(groupBuilder);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<TElement, TOutVertex>>.Identity() => Identity();

        IOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<TElement, IOutEdgeGremlinQuery<TElement, TOutVertex>>.Inject(params TElement[] elements) => Inject(elements);
        
        IOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<TElement, TOutVertex>>.Limit(long count) => Limit(count);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<TElement, TOutVertex>>.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IGremlinQueryBaseRec<IOutEdgeGremlinQuery<TElement, TOutVertex>>.Local<TTargetQuery>(Func<IOutEdgeGremlinQuery<TElement, TOutVertex> , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IGremlinQueryBaseRec<IOutEdgeGremlinQuery<TElement, TOutVertex>>.Map<TTargetQuery>(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery> mapping) => Map(mapping);
        
        IOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<TElement, TOutVertex>>.Not(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQueryBase> notTraversal) => Not(notTraversal);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<TElement, TOutVertex>>.None() => None();

        IOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<TElement, TOutVertex>>.Optional(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, IOutEdgeGremlinQuery<TElement, TOutVertex>> optionalTraversal) => Optional(optionalTraversal);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<TElement, TOutVertex>>.Or(params Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQueryBase>[] orTraversals) => Or(orTraversals);

        IValueGremlinQuery<dynamic> IGremlinQueryBaseRec<TElement, IOutEdgeGremlinQuery<TElement, TOutVertex>>.Project(Func<IProjectBuilder<IOutEdgeGremlinQuery<TElement, TOutVertex>, TElement>, IProjectResult> continuation) => Project<TElement, dynamic>(continuation);
        IValueGremlinQuery<TResult> IGremlinQueryBaseRec<TElement, IOutEdgeGremlinQuery<TElement, TOutVertex>>.Project<TResult>(Func<IProjectBuilder<IOutEdgeGremlinQuery<TElement, TOutVertex>, TElement>, IProjectResult<TResult>> continuation) => Project<TElement, TResult>(continuation);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<TElement, TOutVertex>>.Range(long low, long high) => Range(low, high);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<TElement, TOutVertex>>.Repeat(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, IOutEdgeGremlinQuery<TElement, TOutVertex>> repeatTraversal) => Repeat(repeatTraversal);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<TElement, TOutVertex>>.RepeatUntil(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, IOutEdgeGremlinQuery<TElement, TOutVertex>> repeatTraversal, Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQueryBase> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<TElement, TOutVertex>>.UntilRepeat(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, IOutEdgeGremlinQuery<TElement, TOutVertex>> repeatTraversal, Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQueryBase> untilTraversal) => UntilRepeat(repeatTraversal, untilTraversal);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<TElement, TOutVertex>>.SideEffect(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQueryBase> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<TElement, TOutVertex>>.Skip(long count) => Skip(count);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<TElement, TOutVertex>>.Tail(long count) => Tail(count);
        IOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<TElement, TOutVertex>>.TailLocal(int count) => TailLocal(count);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<TElement, TOutVertex>>.Times(int count) => Times(count);

        TTargetQuery IGremlinQueryBaseRec<IOutEdgeGremlinQuery<TElement, TOutVertex>>.Union<TTargetQuery>(params Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<TElement, TOutVertex>>.Where(ILambda lambda) => Where(lambda);
        IOutEdgeGremlinQuery<TElement, TOutVertex> IGremlinQueryBaseRec<IOutEdgeGremlinQuery<TElement, TOutVertex>>.Where(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQueryBase> filterTraversal) => Where(filterTraversal);

        TTargetQuery IGremlinQueryBaseRec<TElement, IVertexPropertyGremlinQuery<TElement, TPropertyValue>>.Aggregate<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, StepLabel<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, TElement[]>, TTargetQuery> continuation) => Aggregate(Scope.Local, new StepLabel<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, TElement[]>(), continuation);
        TTargetQuery IGremlinQueryBaseRec<TElement, IVertexPropertyGremlinQuery<TElement, TPropertyValue>>.AggregateGlobal<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, StepLabel<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, TElement[]>, TTargetQuery> continuation) => Aggregate(Scope.Global, new StepLabel<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, TElement[]>(), continuation);

        TTargetQuery IGremlinQueryBaseRec<TElement, IVertexPropertyGremlinQuery<TElement, TPropertyValue>>.As<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, StepLabel<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, TElement>, TTargetQuery> continuation) => As(new StepLabel<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, TElement>(), continuation);
        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IGremlinQueryBaseRec<TElement, IVertexPropertyGremlinQuery<TElement, TPropertyValue>>.As<TTargetQuery>(StepLabel<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, TElement> stepLabel) => As(stepLabel);
        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQueryBase<TElement, TPropertyValue>.As(StepLabel<TElement> stepLabel) => As(stepLabel);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue>>.And(params Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, IGremlinQueryBase>[] andTraversals) => And(andTraversals);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue>>.Barrier() => Barrier();

        TTargetQuery IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue>>.Choose<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, IGremlinQueryBase> traversalPredicate, Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, TTargetQuery> trueChoice, Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);
        TTargetQuery IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue>>.Choose<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, IGremlinQueryBase> traversalPredicate, Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);
        TTargetQuery IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue>>.Choose<TTargetQuery>(Func<IChooseBuilder<IVertexPropertyGremlinQuery<TElement, TPropertyValue>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);

        TTargetQuery IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue>>.Coalesce<TTargetQuery>(params Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, TTargetQuery>[] traversals) => Coalesce(traversals);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue>>.Coin(double probability) => Coin(probability);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue>>.Dedup() => Dedup();

        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue>>.Emit() => Emit();

        TTargetQuery IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue>>.FlatMap<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, TTargetQuery> mapping) => FlatMap(mapping);

        IArrayGremlinQuery<TElement[], IVertexPropertyGremlinQuery<TElement, TPropertyValue>> IGremlinQueryBaseRec<TElement, IVertexPropertyGremlinQuery<TElement, TPropertyValue>>.Fold() => Fold<IVertexPropertyGremlinQuery<TElement, TPropertyValue>>();

        IValueGremlinQuery<IDictionary<TNewKey, TNewValue>> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue>>.Group<TNewKey, TNewValue>(Func<IGroupBuilder<IVertexPropertyGremlinQuery<TElement, TPropertyValue>>, IGroupBuilderWithKeyAndValue<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);
        IValueGremlinQuery<IDictionary<TNewKey, object>> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue>>.Group<TNewKey>(Func<IGroupBuilder<IVertexPropertyGremlinQuery<TElement, TPropertyValue>>, IGroupBuilderWithKey<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, TNewKey>> groupBuilder) => Group(groupBuilder);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue>>.Identity() => Identity();

        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IGremlinQueryBaseRec<TElement, IVertexPropertyGremlinQuery<TElement, TPropertyValue>>.Inject(params TElement[] elements) => Inject(elements);
        
        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue>>.Limit(long count) => Limit(count);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue>>.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue>>.Local<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue> , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue>>.Map<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, TTargetQuery> mapping) => Map(mapping);
        
        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue>>.Not(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, IGremlinQueryBase> notTraversal) => Not(notTraversal);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue>>.None() => None();

        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue>>.Optional(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, IVertexPropertyGremlinQuery<TElement, TPropertyValue>> optionalTraversal) => Optional(optionalTraversal);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue>>.Or(params Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, IGremlinQueryBase>[] orTraversals) => Or(orTraversals);

        IValueGremlinQuery<dynamic> IGremlinQueryBaseRec<TElement, IVertexPropertyGremlinQuery<TElement, TPropertyValue>>.Project(Func<IProjectBuilder<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, TElement>, IProjectResult> continuation) => Project<TElement, dynamic>(continuation);
        IValueGremlinQuery<TResult> IGremlinQueryBaseRec<TElement, IVertexPropertyGremlinQuery<TElement, TPropertyValue>>.Project<TResult>(Func<IProjectBuilder<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, TElement>, IProjectResult<TResult>> continuation) => Project<TElement, TResult>(continuation);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue>>.Range(long low, long high) => Range(low, high);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue>>.Repeat(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, IVertexPropertyGremlinQuery<TElement, TPropertyValue>> repeatTraversal) => Repeat(repeatTraversal);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue>>.RepeatUntil(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, IVertexPropertyGremlinQuery<TElement, TPropertyValue>> repeatTraversal, Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, IGremlinQueryBase> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue>>.UntilRepeat(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, IVertexPropertyGremlinQuery<TElement, TPropertyValue>> repeatTraversal, Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, IGremlinQueryBase> untilTraversal) => UntilRepeat(repeatTraversal, untilTraversal);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue>>.SideEffect(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, IGremlinQueryBase> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue>>.Skip(long count) => Skip(count);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue>>.Tail(long count) => Tail(count);
        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue>>.TailLocal(int count) => TailLocal(count);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue>>.Times(int count) => Times(count);

        TTargetQuery IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue>>.Union<TTargetQuery>(params Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue>>.Where(ILambda lambda) => Where(lambda);
        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue>>.Where(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, IGremlinQueryBase> filterTraversal) => Where(filterTraversal);

        TTargetQuery IGremlinQueryBaseRec<TElement, IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>.Aggregate<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, StepLabel<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, TElement[]>, TTargetQuery> continuation) => Aggregate(Scope.Local, new StepLabel<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, TElement[]>(), continuation);
        TTargetQuery IGremlinQueryBaseRec<TElement, IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>.AggregateGlobal<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, StepLabel<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, TElement[]>, TTargetQuery> continuation) => Aggregate(Scope.Global, new StepLabel<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, TElement[]>(), continuation);

        TTargetQuery IGremlinQueryBaseRec<TElement, IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>.As<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, StepLabel<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, TElement>, TTargetQuery> continuation) => As(new StepLabel<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, TElement>(), continuation);
        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IGremlinQueryBaseRec<TElement, IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>.As<TTargetQuery>(StepLabel<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, TElement> stepLabel) => As(stepLabel);
        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQueryBase<TElement, TPropertyValue, TMeta>.As(StepLabel<TElement> stepLabel) => As(stepLabel);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>.And(params Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, IGremlinQueryBase>[] andTraversals) => And(andTraversals);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>.Barrier() => Barrier();

        TTargetQuery IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>.Choose<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, IGremlinQueryBase> traversalPredicate, Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, TTargetQuery> trueChoice, Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);
        TTargetQuery IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>.Choose<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, IGremlinQueryBase> traversalPredicate, Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);
        TTargetQuery IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>.Choose<TTargetQuery>(Func<IChooseBuilder<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);

        TTargetQuery IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>.Coalesce<TTargetQuery>(params Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, TTargetQuery>[] traversals) => Coalesce(traversals);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>.Coin(double probability) => Coin(probability);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>.Dedup() => Dedup();

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>.Emit() => Emit();

        TTargetQuery IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>.FlatMap<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, TTargetQuery> mapping) => FlatMap(mapping);

        IArrayGremlinQuery<TElement[], IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>> IGremlinQueryBaseRec<TElement, IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>.Fold() => Fold<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>();

        IValueGremlinQuery<IDictionary<TNewKey, TNewValue>> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>.Group<TNewKey, TNewValue>(Func<IGroupBuilder<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>, IGroupBuilderWithKeyAndValue<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);
        IValueGremlinQuery<IDictionary<TNewKey, object>> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>.Group<TNewKey>(Func<IGroupBuilder<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>, IGroupBuilderWithKey<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, TNewKey>> groupBuilder) => Group(groupBuilder);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>.Identity() => Identity();

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IGremlinQueryBaseRec<TElement, IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>.Inject(params TElement[] elements) => Inject(elements);
        
        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>.Limit(long count) => Limit(count);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>.Local<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>.Map<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, TTargetQuery> mapping) => Map(mapping);
        
        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>.Not(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, IGremlinQueryBase> notTraversal) => Not(notTraversal);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>.None() => None();

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>.Optional(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>> optionalTraversal) => Optional(optionalTraversal);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>.Or(params Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, IGremlinQueryBase>[] orTraversals) => Or(orTraversals);

        IValueGremlinQuery<dynamic> IGremlinQueryBaseRec<TElement, IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>.Project(Func<IProjectBuilder<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, TElement>, IProjectResult> continuation) => Project<TElement, dynamic>(continuation);
        IValueGremlinQuery<TResult> IGremlinQueryBaseRec<TElement, IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>.Project<TResult>(Func<IProjectBuilder<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, TElement>, IProjectResult<TResult>> continuation) => Project<TElement, TResult>(continuation);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>.Range(long low, long high) => Range(low, high);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>.Repeat(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>> repeatTraversal) => Repeat(repeatTraversal);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>.RepeatUntil(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>> repeatTraversal, Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, IGremlinQueryBase> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>.UntilRepeat(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>> repeatTraversal, Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, IGremlinQueryBase> untilTraversal) => UntilRepeat(repeatTraversal, untilTraversal);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>.SideEffect(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, IGremlinQueryBase> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>.Skip(long count) => Skip(count);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>.Tail(long count) => Tail(count);
        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>.TailLocal(int count) => TailLocal(count);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>.Times(int count) => Times(count);

        TTargetQuery IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>.Union<TTargetQuery>(params Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>.Where(ILambda lambda) => Where(lambda);
        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>.Where(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, IGremlinQueryBase> filterTraversal) => Where(filterTraversal);

        TTargetQuery IGremlinQueryBaseRec<TElement, IPropertyGremlinQuery<TElement>>.Aggregate<TTargetQuery>(Func<IPropertyGremlinQuery<TElement>, StepLabel<IPropertyGremlinQuery<TElement>, TElement[]>, TTargetQuery> continuation) => Aggregate(Scope.Local, new StepLabel<IPropertyGremlinQuery<TElement>, TElement[]>(), continuation);
        TTargetQuery IGremlinQueryBaseRec<TElement, IPropertyGremlinQuery<TElement>>.AggregateGlobal<TTargetQuery>(Func<IPropertyGremlinQuery<TElement>, StepLabel<IPropertyGremlinQuery<TElement>, TElement[]>, TTargetQuery> continuation) => Aggregate(Scope.Global, new StepLabel<IPropertyGremlinQuery<TElement>, TElement[]>(), continuation);

        TTargetQuery IGremlinQueryBaseRec<TElement, IPropertyGremlinQuery<TElement>>.As<TTargetQuery>(Func<IPropertyGremlinQuery<TElement>, StepLabel<IPropertyGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => As(new StepLabel<IPropertyGremlinQuery<TElement>, TElement>(), continuation);
        IPropertyGremlinQuery<TElement> IGremlinQueryBaseRec<TElement, IPropertyGremlinQuery<TElement>>.As<TTargetQuery>(StepLabel<IPropertyGremlinQuery<TElement>, TElement> stepLabel) => As(stepLabel);
        IPropertyGremlinQuery<TElement> IPropertyGremlinQueryBase<TElement>.As(StepLabel<TElement> stepLabel) => As(stepLabel);

        IPropertyGremlinQuery<TElement> IGremlinQueryBaseRec<IPropertyGremlinQuery<TElement>>.And(params Func<IPropertyGremlinQuery<TElement>, IGremlinQueryBase>[] andTraversals) => And(andTraversals);

        IPropertyGremlinQuery<TElement> IGremlinQueryBaseRec<IPropertyGremlinQuery<TElement>>.Barrier() => Barrier();

        TTargetQuery IGremlinQueryBaseRec<IPropertyGremlinQuery<TElement>>.Choose<TTargetQuery>(Func<IPropertyGremlinQuery<TElement>, IGremlinQueryBase> traversalPredicate, Func<IPropertyGremlinQuery<TElement>, TTargetQuery> trueChoice, Func<IPropertyGremlinQuery<TElement>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);
        TTargetQuery IGremlinQueryBaseRec<IPropertyGremlinQuery<TElement>>.Choose<TTargetQuery>(Func<IPropertyGremlinQuery<TElement>, IGremlinQueryBase> traversalPredicate, Func<IPropertyGremlinQuery<TElement>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);
        TTargetQuery IGremlinQueryBaseRec<IPropertyGremlinQuery<TElement>>.Choose<TTargetQuery>(Func<IChooseBuilder<IPropertyGremlinQuery<TElement>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);

        TTargetQuery IGremlinQueryBaseRec<IPropertyGremlinQuery<TElement>>.Coalesce<TTargetQuery>(params Func<IPropertyGremlinQuery<TElement>, TTargetQuery>[] traversals) => Coalesce(traversals);

        IPropertyGremlinQuery<TElement> IGremlinQueryBaseRec<IPropertyGremlinQuery<TElement>>.Coin(double probability) => Coin(probability);

        IPropertyGremlinQuery<TElement> IGremlinQueryBaseRec<IPropertyGremlinQuery<TElement>>.Dedup() => Dedup();

        IPropertyGremlinQuery<TElement> IGremlinQueryBaseRec<IPropertyGremlinQuery<TElement>>.Emit() => Emit();

        TTargetQuery IGremlinQueryBaseRec<IPropertyGremlinQuery<TElement>>.FlatMap<TTargetQuery>(Func<IPropertyGremlinQuery<TElement>, TTargetQuery> mapping) => FlatMap(mapping);

        IArrayGremlinQuery<TElement[], IPropertyGremlinQuery<TElement>> IGremlinQueryBaseRec<TElement, IPropertyGremlinQuery<TElement>>.Fold() => Fold<IPropertyGremlinQuery<TElement>>();

        IValueGremlinQuery<IDictionary<TNewKey, TNewValue>> IGremlinQueryBaseRec<IPropertyGremlinQuery<TElement>>.Group<TNewKey, TNewValue>(Func<IGroupBuilder<IPropertyGremlinQuery<TElement>>, IGroupBuilderWithKeyAndValue<IPropertyGremlinQuery<TElement>, TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);
        IValueGremlinQuery<IDictionary<TNewKey, object>> IGremlinQueryBaseRec<IPropertyGremlinQuery<TElement>>.Group<TNewKey>(Func<IGroupBuilder<IPropertyGremlinQuery<TElement>>, IGroupBuilderWithKey<IPropertyGremlinQuery<TElement>, TNewKey>> groupBuilder) => Group(groupBuilder);

        IPropertyGremlinQuery<TElement> IGremlinQueryBaseRec<IPropertyGremlinQuery<TElement>>.Identity() => Identity();

        IPropertyGremlinQuery<TElement> IGremlinQueryBaseRec<TElement, IPropertyGremlinQuery<TElement>>.Inject(params TElement[] elements) => Inject(elements);
        
        IPropertyGremlinQuery<TElement> IGremlinQueryBaseRec<IPropertyGremlinQuery<TElement>>.Limit(long count) => Limit(count);

        IPropertyGremlinQuery<TElement> IGremlinQueryBaseRec<IPropertyGremlinQuery<TElement>>.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IGremlinQueryBaseRec<IPropertyGremlinQuery<TElement>>.Local<TTargetQuery>(Func<IPropertyGremlinQuery<TElement> , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IGremlinQueryBaseRec<IPropertyGremlinQuery<TElement>>.Map<TTargetQuery>(Func<IPropertyGremlinQuery<TElement>, TTargetQuery> mapping) => Map(mapping);
        
        IPropertyGremlinQuery<TElement> IGremlinQueryBaseRec<IPropertyGremlinQuery<TElement>>.Not(Func<IPropertyGremlinQuery<TElement>, IGremlinQueryBase> notTraversal) => Not(notTraversal);

        IPropertyGremlinQuery<TElement> IGremlinQueryBaseRec<IPropertyGremlinQuery<TElement>>.None() => None();

        IPropertyGremlinQuery<TElement> IGremlinQueryBaseRec<IPropertyGremlinQuery<TElement>>.Optional(Func<IPropertyGremlinQuery<TElement>, IPropertyGremlinQuery<TElement>> optionalTraversal) => Optional(optionalTraversal);

        IPropertyGremlinQuery<TElement> IGremlinQueryBaseRec<IPropertyGremlinQuery<TElement>>.Or(params Func<IPropertyGremlinQuery<TElement>, IGremlinQueryBase>[] orTraversals) => Or(orTraversals);

        IValueGremlinQuery<dynamic> IGremlinQueryBaseRec<TElement, IPropertyGremlinQuery<TElement>>.Project(Func<IProjectBuilder<IPropertyGremlinQuery<TElement>, TElement>, IProjectResult> continuation) => Project<TElement, dynamic>(continuation);
        IValueGremlinQuery<TResult> IGremlinQueryBaseRec<TElement, IPropertyGremlinQuery<TElement>>.Project<TResult>(Func<IProjectBuilder<IPropertyGremlinQuery<TElement>, TElement>, IProjectResult<TResult>> continuation) => Project<TElement, TResult>(continuation);

        IPropertyGremlinQuery<TElement> IGremlinQueryBaseRec<IPropertyGremlinQuery<TElement>>.Range(long low, long high) => Range(low, high);

        IPropertyGremlinQuery<TElement> IGremlinQueryBaseRec<IPropertyGremlinQuery<TElement>>.Repeat(Func<IPropertyGremlinQuery<TElement>, IPropertyGremlinQuery<TElement>> repeatTraversal) => Repeat(repeatTraversal);

        IPropertyGremlinQuery<TElement> IGremlinQueryBaseRec<IPropertyGremlinQuery<TElement>>.RepeatUntil(Func<IPropertyGremlinQuery<TElement>, IPropertyGremlinQuery<TElement>> repeatTraversal, Func<IPropertyGremlinQuery<TElement>, IGremlinQueryBase> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        IPropertyGremlinQuery<TElement> IGremlinQueryBaseRec<IPropertyGremlinQuery<TElement>>.UntilRepeat(Func<IPropertyGremlinQuery<TElement>, IPropertyGremlinQuery<TElement>> repeatTraversal, Func<IPropertyGremlinQuery<TElement>, IGremlinQueryBase> untilTraversal) => UntilRepeat(repeatTraversal, untilTraversal);

        IPropertyGremlinQuery<TElement> IGremlinQueryBaseRec<IPropertyGremlinQuery<TElement>>.SideEffect(Func<IPropertyGremlinQuery<TElement>, IGremlinQueryBase> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IPropertyGremlinQuery<TElement> IGremlinQueryBaseRec<IPropertyGremlinQuery<TElement>>.Skip(long count) => Skip(count);

        IPropertyGremlinQuery<TElement> IGremlinQueryBaseRec<IPropertyGremlinQuery<TElement>>.Tail(long count) => Tail(count);
        IPropertyGremlinQuery<TElement> IGremlinQueryBaseRec<IPropertyGremlinQuery<TElement>>.TailLocal(int count) => TailLocal(count);

        IPropertyGremlinQuery<TElement> IGremlinQueryBaseRec<IPropertyGremlinQuery<TElement>>.Times(int count) => Times(count);

        TTargetQuery IGremlinQueryBaseRec<IPropertyGremlinQuery<TElement>>.Union<TTargetQuery>(params Func<IPropertyGremlinQuery<TElement>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        IPropertyGremlinQuery<TElement> IGremlinQueryBaseRec<IPropertyGremlinQuery<TElement>>.Where(ILambda lambda) => Where(lambda);
        IPropertyGremlinQuery<TElement> IGremlinQueryBaseRec<IPropertyGremlinQuery<TElement>>.Where(Func<IPropertyGremlinQuery<TElement>, IGremlinQueryBase> filterTraversal) => Where(filterTraversal);



        IVertexGremlinQuery<TTarget> IVertexGremlinQueryBase.OfType<TTarget>() => OfType<TTarget>(Environment.Model.VerticesModel);
        IEdgeGremlinQuery<TTarget> IEdgeGremlinQueryBase.OfType<TTarget>() => OfType<TTarget>(Environment.Model.EdgesModel);


        IElementGremlinQuery<TElement> IElementGremlinQueryBaseRec<TElement, IElementGremlinQuery<TElement>>.Order(Func<IOrderBuilder<TElement, IElementGremlinQuery<TElement>>, IOrderBuilderWithBy<TElement, IElementGremlinQuery<TElement>>> projection) => OrderGlobal(projection);

        IElementGremlinQuery<TElement> IElementGremlinQueryBaseRec<IElementGremlinQuery<TElement>>.Property(string key, [AllowNull] object value) => Property(key, value);
        IElementGremlinQuery<TElement> IElementGremlinQueryBaseRec<TElement, IElementGremlinQuery<TElement>>.Property<TProjectedValue>(Expression<Func<TElement, TProjectedValue>> projection, [AllowNull] TProjectedValue value) => Property(projection, value);

        IElementGremlinQuery<TElement> IElementGremlinQueryBaseRec<TElement, IElementGremlinQuery<TElement>>.Where(Expression<Func<TElement, bool>> predicate) => Where(predicate);
        IElementGremlinQuery<TElement> IElementGremlinQueryBaseRec<TElement, IElementGremlinQuery<TElement>>.Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQueryBase<TProjection>, IGremlinQueryBase> propertyTraversal) => Where(projection, propertyTraversal);
        IEdgeOrVertexGremlinQuery<TElement> IElementGremlinQueryBaseRec<TElement, IEdgeOrVertexGremlinQuery<TElement>>.Order(Func<IOrderBuilder<TElement, IEdgeOrVertexGremlinQuery<TElement>>, IOrderBuilderWithBy<TElement, IEdgeOrVertexGremlinQuery<TElement>>> projection) => OrderGlobal(projection);

        IEdgeOrVertexGremlinQuery<TElement> IElementGremlinQueryBaseRec<IEdgeOrVertexGremlinQuery<TElement>>.Property(string key, [AllowNull] object value) => Property(key, value);
        IEdgeOrVertexGremlinQuery<TElement> IElementGremlinQueryBaseRec<TElement, IEdgeOrVertexGremlinQuery<TElement>>.Property<TProjectedValue>(Expression<Func<TElement, TProjectedValue>> projection, [AllowNull] TProjectedValue value) => Property(projection, value);

        IEdgeOrVertexGremlinQuery<TElement> IElementGremlinQueryBaseRec<TElement, IEdgeOrVertexGremlinQuery<TElement>>.Where(Expression<Func<TElement, bool>> predicate) => Where(predicate);
        IEdgeOrVertexGremlinQuery<TElement> IElementGremlinQueryBaseRec<TElement, IEdgeOrVertexGremlinQuery<TElement>>.Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQueryBase<TProjection>, IGremlinQueryBase> propertyTraversal) => Where(projection, propertyTraversal);
        IVertexGremlinQuery<TElement> IElementGremlinQueryBaseRec<TElement, IVertexGremlinQuery<TElement>>.Order(Func<IOrderBuilder<TElement, IVertexGremlinQuery<TElement>>, IOrderBuilderWithBy<TElement, IVertexGremlinQuery<TElement>>> projection) => OrderGlobal(projection);

        IVertexGremlinQuery<TElement> IElementGremlinQueryBaseRec<IVertexGremlinQuery<TElement>>.Property(string key, [AllowNull] object value) => Property(key, value);
        IVertexGremlinQuery<TElement> IElementGremlinQueryBaseRec<TElement, IVertexGremlinQuery<TElement>>.Property<TProjectedValue>(Expression<Func<TElement, TProjectedValue>> projection, [AllowNull] TProjectedValue value) => VertexProperty(projection, value);

        IVertexGremlinQuery<TElement> IElementGremlinQueryBaseRec<TElement, IVertexGremlinQuery<TElement>>.Where(Expression<Func<TElement, bool>> predicate) => Where(predicate);
        IVertexGremlinQuery<TElement> IElementGremlinQueryBaseRec<TElement, IVertexGremlinQuery<TElement>>.Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQueryBase<TProjection>, IGremlinQueryBase> propertyTraversal) => Where(projection, propertyTraversal);
        IEdgeGremlinQuery<TElement> IElementGremlinQueryBaseRec<TElement, IEdgeGremlinQuery<TElement>>.Order(Func<IOrderBuilder<TElement, IEdgeGremlinQuery<TElement>>, IOrderBuilderWithBy<TElement, IEdgeGremlinQuery<TElement>>> projection) => OrderGlobal(projection);

        IEdgeGremlinQuery<TElement> IElementGremlinQueryBaseRec<IEdgeGremlinQuery<TElement>>.Property(string key, [AllowNull] object value) => Property(key, value);
        IEdgeGremlinQuery<TElement> IElementGremlinQueryBaseRec<TElement, IEdgeGremlinQuery<TElement>>.Property<TProjectedValue>(Expression<Func<TElement, TProjectedValue>> projection, [AllowNull] TProjectedValue value) => Property(projection, value);

        IEdgeGremlinQuery<TElement> IElementGremlinQueryBaseRec<TElement, IEdgeGremlinQuery<TElement>>.Where(Expression<Func<TElement, bool>> predicate) => Where(predicate);
        IEdgeGremlinQuery<TElement> IElementGremlinQueryBaseRec<TElement, IEdgeGremlinQuery<TElement>>.Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQueryBase<TProjection>, IGremlinQueryBase> propertyTraversal) => Where(projection, propertyTraversal);
        IInOrOutEdgeGremlinQuery<TElement, TOutVertex> IElementGremlinQueryBaseRec<TElement, IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>.Order(Func<IOrderBuilder<TElement, IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>, IOrderBuilderWithBy<TElement, IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>> projection) => OrderGlobal(projection);

        IInOrOutEdgeGremlinQuery<TElement, TOutVertex> IElementGremlinQueryBaseRec<IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>.Property(string key, [AllowNull] object value) => Property(key, value);
        IInOrOutEdgeGremlinQuery<TElement, TOutVertex> IElementGremlinQueryBaseRec<TElement, IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>.Property<TProjectedValue>(Expression<Func<TElement, TProjectedValue>> projection, [AllowNull] TProjectedValue value) => Property(projection, value);

        IInOrOutEdgeGremlinQuery<TElement, TOutVertex> IElementGremlinQueryBaseRec<TElement, IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>.Where(Expression<Func<TElement, bool>> predicate) => Where(predicate);
        IInOrOutEdgeGremlinQuery<TElement, TOutVertex> IElementGremlinQueryBaseRec<TElement, IInOrOutEdgeGremlinQuery<TElement, TOutVertex>>.Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQueryBase<TProjection>, IGremlinQueryBase> propertyTraversal) => Where(projection, propertyTraversal);
        IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IElementGremlinQueryBaseRec<TElement, IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>.Order(Func<IOrderBuilder<TElement, IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>, IOrderBuilderWithBy<TElement, IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>> projection) => OrderGlobal(projection);

        IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IElementGremlinQueryBaseRec<IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>.Property(string key, [AllowNull] object value) => Property(key, value);
        IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IElementGremlinQueryBaseRec<TElement, IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>.Property<TProjectedValue>(Expression<Func<TElement, TProjectedValue>> projection, [AllowNull] TProjectedValue value) => Property(projection, value);

        IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IElementGremlinQueryBaseRec<TElement, IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>.Where(Expression<Func<TElement, bool>> predicate) => Where(predicate);
        IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IElementGremlinQueryBaseRec<TElement, IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>.Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQueryBase<TProjection>, IGremlinQueryBase> propertyTraversal) => Where(projection, propertyTraversal);
        IInEdgeGremlinQuery<TElement, TInVertex> IElementGremlinQueryBaseRec<TElement, IInEdgeGremlinQuery<TElement, TInVertex>>.Order(Func<IOrderBuilder<TElement, IInEdgeGremlinQuery<TElement, TInVertex>>, IOrderBuilderWithBy<TElement, IInEdgeGremlinQuery<TElement, TInVertex>>> projection) => OrderGlobal(projection);

        IInEdgeGremlinQuery<TElement, TInVertex> IElementGremlinQueryBaseRec<IInEdgeGremlinQuery<TElement, TInVertex>>.Property(string key, [AllowNull] object value) => Property(key, value);
        IInEdgeGremlinQuery<TElement, TInVertex> IElementGremlinQueryBaseRec<TElement, IInEdgeGremlinQuery<TElement, TInVertex>>.Property<TProjectedValue>(Expression<Func<TElement, TProjectedValue>> projection, [AllowNull] TProjectedValue value) => Property(projection, value);

        IInEdgeGremlinQuery<TElement, TInVertex> IElementGremlinQueryBaseRec<TElement, IInEdgeGremlinQuery<TElement, TInVertex>>.Where(Expression<Func<TElement, bool>> predicate) => Where(predicate);
        IInEdgeGremlinQuery<TElement, TInVertex> IElementGremlinQueryBaseRec<TElement, IInEdgeGremlinQuery<TElement, TInVertex>>.Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQueryBase<TProjection>, IGremlinQueryBase> propertyTraversal) => Where(projection, propertyTraversal);
        IOutEdgeGremlinQuery<TElement, TOutVertex> IElementGremlinQueryBaseRec<TElement, IOutEdgeGremlinQuery<TElement, TOutVertex>>.Order(Func<IOrderBuilder<TElement, IOutEdgeGremlinQuery<TElement, TOutVertex>>, IOrderBuilderWithBy<TElement, IOutEdgeGremlinQuery<TElement, TOutVertex>>> projection) => OrderGlobal(projection);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IElementGremlinQueryBaseRec<IOutEdgeGremlinQuery<TElement, TOutVertex>>.Property(string key, [AllowNull] object value) => Property(key, value);
        IOutEdgeGremlinQuery<TElement, TOutVertex> IElementGremlinQueryBaseRec<TElement, IOutEdgeGremlinQuery<TElement, TOutVertex>>.Property<TProjectedValue>(Expression<Func<TElement, TProjectedValue>> projection, [AllowNull] TProjectedValue value) => Property(projection, value);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IElementGremlinQueryBaseRec<TElement, IOutEdgeGremlinQuery<TElement, TOutVertex>>.Where(Expression<Func<TElement, bool>> predicate) => Where(predicate);
        IOutEdgeGremlinQuery<TElement, TOutVertex> IElementGremlinQueryBaseRec<TElement, IOutEdgeGremlinQuery<TElement, TOutVertex>>.Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQueryBase<TProjection>, IGremlinQueryBase> propertyTraversal) => Where(projection, propertyTraversal);
        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IElementGremlinQueryBaseRec<TElement, IVertexPropertyGremlinQuery<TElement, TPropertyValue>>.Order(Func<IOrderBuilder<TElement, IVertexPropertyGremlinQuery<TElement, TPropertyValue>>, IOrderBuilderWithBy<TElement, IVertexPropertyGremlinQuery<TElement, TPropertyValue>>> projection) => OrderGlobal(projection);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IElementGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue>>.Property(string key, [AllowNull] object value) => Property(key, value);
        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IElementGremlinQueryBaseRec<TElement, IVertexPropertyGremlinQuery<TElement, TPropertyValue>>.Property<TProjectedValue>(Expression<Func<TElement, TProjectedValue>> projection, [AllowNull] TProjectedValue value) => Property(projection, value);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IElementGremlinQueryBaseRec<TElement, IVertexPropertyGremlinQuery<TElement, TPropertyValue>>.Where(Expression<Func<TElement, bool>> predicate) => Where(predicate);
        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IElementGremlinQueryBaseRec<TElement, IVertexPropertyGremlinQuery<TElement, TPropertyValue>>.Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQueryBase<TProjection>, IGremlinQueryBase> propertyTraversal) => Where(projection, propertyTraversal);
        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IElementGremlinQueryBaseRec<TElement, IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>.Order(Func<IOrderBuilder<TElement, IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>, IOrderBuilderWithBy<TElement, IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>> projection) => OrderGlobal(projection);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IElementGremlinQueryBaseRec<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>.Property(string key, [AllowNull] object value) => Property(key, value);
        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IElementGremlinQueryBaseRec<TElement, IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>.Property<TProjectedValue>(Expression<Func<TElement, TProjectedValue>> projection, [AllowNull] TProjectedValue value) => Property(projection, value);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IElementGremlinQueryBaseRec<TElement, IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>.Where(Expression<Func<TElement, bool>> predicate) => Where(predicate);
        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IElementGremlinQueryBaseRec<TElement, IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>.Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQueryBase<TProjection>, IGremlinQueryBase> propertyTraversal) => Where(projection, propertyTraversal);
   }
}

