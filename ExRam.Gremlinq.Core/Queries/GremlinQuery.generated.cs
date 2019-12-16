#pragma warning disable CS0109 // Member does not hide an inherited member; new keyword is not required
using System;
using NullGuard;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using LanguageExt;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    partial class GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>
    {
        IGremlinQuery<(T1, T2)> IGremlinQuery.Select<T1, T2>(StepLabel<T1> label1, StepLabel<T2> label2) => Project<TElement, (T1, T2)>(p => p.ToTuple().By(_ => _.Select(label1)).By(_ => _.Select(label2)));
        IGremlinQuery<(T1, T2, T3)> IGremlinQuery.Select<T1, T2, T3>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3) => Project<TElement, (T1, T2, T3)>(p => p.ToTuple().By(_ => _.Select(label1)).By(_ => _.Select(label2)).By(_ => _.Select(label3)));
        IGremlinQuery<(T1, T2, T3, T4)> IGremlinQuery.Select<T1, T2, T3, T4>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4) => Project<TElement, (T1, T2, T3, T4)>(p => p.ToTuple().By(_ => _.Select(label1)).By(_ => _.Select(label2)).By(_ => _.Select(label3)).By(_ => _.Select(label4)));
        IGremlinQuery<(T1, T2, T3, T4, T5)> IGremlinQuery.Select<T1, T2, T3, T4, T5>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5) => Project<TElement, (T1, T2, T3, T4, T5)>(p => p.ToTuple().By(_ => _.Select(label1)).By(_ => _.Select(label2)).By(_ => _.Select(label3)).By(_ => _.Select(label4)).By(_ => _.Select(label5)));
        IGremlinQuery<(T1, T2, T3, T4, T5, T6)> IGremlinQuery.Select<T1, T2, T3, T4, T5, T6>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6) => Project<TElement, (T1, T2, T3, T4, T5, T6)>(p => p.ToTuple().By(_ => _.Select(label1)).By(_ => _.Select(label2)).By(_ => _.Select(label3)).By(_ => _.Select(label4)).By(_ => _.Select(label5)).By(_ => _.Select(label6)));
        IGremlinQuery<(T1, T2, T3, T4, T5, T6, T7)> IGremlinQuery.Select<T1, T2, T3, T4, T5, T6, T7>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7) => Project<TElement, (T1, T2, T3, T4, T5, T6, T7)>(p => p.ToTuple().By(_ => _.Select(label1)).By(_ => _.Select(label2)).By(_ => _.Select(label3)).By(_ => _.Select(label4)).By(_ => _.Select(label5)).By(_ => _.Select(label6)).By(_ => _.Select(label7)));
        IGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8)> IGremlinQuery.Select<T1, T2, T3, T4, T5, T6, T7, T8>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8) => Project<TElement, (T1, T2, T3, T4, T5, T6, T7, T8)>(p => p.ToTuple().By(_ => _.Select(label1)).By(_ => _.Select(label2)).By(_ => _.Select(label3)).By(_ => _.Select(label4)).By(_ => _.Select(label5)).By(_ => _.Select(label6)).By(_ => _.Select(label7)).By(_ => _.Select(label8)));
        IGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> IGremlinQuery.Select<T1, T2, T3, T4, T5, T6, T7, T8, T9>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9) => Project<TElement, (T1, T2, T3, T4, T5, T6, T7, T8, T9)>(p => p.ToTuple().By(_ => _.Select(label1)).By(_ => _.Select(label2)).By(_ => _.Select(label3)).By(_ => _.Select(label4)).By(_ => _.Select(label5)).By(_ => _.Select(label6)).By(_ => _.Select(label7)).By(_ => _.Select(label8)).By(_ => _.Select(label9)));
        IGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> IGremlinQuery.Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10) => Project<TElement, (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)>(p => p.ToTuple().By(_ => _.Select(label1)).By(_ => _.Select(label2)).By(_ => _.Select(label3)).By(_ => _.Select(label4)).By(_ => _.Select(label5)).By(_ => _.Select(label6)).By(_ => _.Select(label7)).By(_ => _.Select(label8)).By(_ => _.Select(label9)).By(_ => _.Select(label10)));
        IGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> IGremlinQuery.Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11) => Project<TElement, (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)>(p => p.ToTuple().By(_ => _.Select(label1)).By(_ => _.Select(label2)).By(_ => _.Select(label3)).By(_ => _.Select(label4)).By(_ => _.Select(label5)).By(_ => _.Select(label6)).By(_ => _.Select(label7)).By(_ => _.Select(label8)).By(_ => _.Select(label9)).By(_ => _.Select(label10)).By(_ => _.Select(label11)));
        IGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> IGremlinQuery.Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11, StepLabel<T12> label12) => Project<TElement, (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)>(p => p.ToTuple().By(_ => _.Select(label1)).By(_ => _.Select(label2)).By(_ => _.Select(label3)).By(_ => _.Select(label4)).By(_ => _.Select(label5)).By(_ => _.Select(label6)).By(_ => _.Select(label7)).By(_ => _.Select(label8)).By(_ => _.Select(label9)).By(_ => _.Select(label10)).By(_ => _.Select(label11)).By(_ => _.Select(label12)));
        IGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)> IGremlinQuery.Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11, StepLabel<T12> label12, StepLabel<T13> label13) => Project<TElement, (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)>(p => p.ToTuple().By(_ => _.Select(label1)).By(_ => _.Select(label2)).By(_ => _.Select(label3)).By(_ => _.Select(label4)).By(_ => _.Select(label5)).By(_ => _.Select(label6)).By(_ => _.Select(label7)).By(_ => _.Select(label8)).By(_ => _.Select(label9)).By(_ => _.Select(label10)).By(_ => _.Select(label11)).By(_ => _.Select(label12)).By(_ => _.Select(label13)));
        IGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)> IGremlinQuery.Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11, StepLabel<T12> label12, StepLabel<T13> label13, StepLabel<T14> label14) => Project<TElement, (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)>(p => p.ToTuple().By(_ => _.Select(label1)).By(_ => _.Select(label2)).By(_ => _.Select(label3)).By(_ => _.Select(label4)).By(_ => _.Select(label5)).By(_ => _.Select(label6)).By(_ => _.Select(label7)).By(_ => _.Select(label8)).By(_ => _.Select(label9)).By(_ => _.Select(label10)).By(_ => _.Select(label11)).By(_ => _.Select(label12)).By(_ => _.Select(label13)).By(_ => _.Select(label14)));
        IGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)> IGremlinQuery.Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11, StepLabel<T12> label12, StepLabel<T13> label13, StepLabel<T14> label14, StepLabel<T15> label15) => Project<TElement, (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)>(p => p.ToTuple().By(_ => _.Select(label1)).By(_ => _.Select(label2)).By(_ => _.Select(label3)).By(_ => _.Select(label4)).By(_ => _.Select(label5)).By(_ => _.Select(label6)).By(_ => _.Select(label7)).By(_ => _.Select(label8)).By(_ => _.Select(label9)).By(_ => _.Select(label10)).By(_ => _.Select(label11)).By(_ => _.Select(label12)).By(_ => _.Select(label13)).By(_ => _.Select(label14)).By(_ => _.Select(label15)));
        IGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16)> IGremlinQuery.Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11, StepLabel<T12> label12, StepLabel<T13> label13, StepLabel<T14> label14, StepLabel<T15> label15, StepLabel<T16> label16) => Project<TElement, (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16)>(p => p.ToTuple().By(_ => _.Select(label1)).By(_ => _.Select(label2)).By(_ => _.Select(label3)).By(_ => _.Select(label4)).By(_ => _.Select(label5)).By(_ => _.Select(label6)).By(_ => _.Select(label7)).By(_ => _.Select(label8)).By(_ => _.Select(label9)).By(_ => _.Select(label10)).By(_ => _.Select(label11)).By(_ => _.Select(label12)).By(_ => _.Select(label13)).By(_ => _.Select(label14)).By(_ => _.Select(label15)).By(_ => _.Select(label16)));


        IGremlinQuery IGremlinQuery.And(params Func<IGremlinQuery, IGremlinQuery>[] andTraversals) => And(andTraversals);
        IGremlinQuery IGremlinQuery.As(params StepLabel[] stepLabels) => As(stepLabels);

        IGremlinQuery IGremlinQuery.Barrier() => Barrier();

        TTargetQuery IGremlinQuery.Choose<TTargetQuery>(Func<IGremlinQuery, IGremlinQuery> traversalPredicate, Func<IGremlinQuery, TTargetQuery> trueChoice, Func<IGremlinQuery, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);
        TTargetQuery IGremlinQuery.Choose<TTargetQuery>(Func<IGremlinQuery, IGremlinQuery> traversalPredicate, Func<IGremlinQuery, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);
        TTargetQuery IGremlinQuery.Choose<TTargetQuery>(Func<IChooseBuilder<IGremlinQuery>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);

        TTargetQuery IGremlinQuery.Coalesce<TTargetQuery>(params Func<IGremlinQuery, TTargetQuery>[] traversals) => Coalesce(traversals);

        IGremlinQuery IGremlinQuery.Dedup() => Dedup();
        IGremlinQuery IGremlinQuery.Drop() => Drop();

        IGremlinQuery IGremlinQuery.Emit() => Emit();

        IGremlinQuery IGremlinQuery.Where(ILambda lambda) => Where(lambda);

        TTargetQuery IGremlinQuery.FlatMap<TTargetQuery>(Func<IGremlinQuery, TTargetQuery> mapping) => FlatMap(mapping);

        IGremlinQuery<IDictionary<TNewKey, TNewValue>> IGremlinQuery.Group<TNewKey, TNewValue>(Func<IGroupBuilder<IGremlinQuery>, IGroupBuilderWithKeyAndValue<IGremlinQuery, TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);
        IGremlinQuery<IDictionary<TNewKey, object>> IGremlinQuery.Group<TNewKey>(Func<IGroupBuilder<IGremlinQuery>, IGroupBuilderWithKey<IGremlinQuery, TNewKey>> groupBuilder) => Group(groupBuilder);

        IGremlinQuery IGremlinQuery.Identity() => Identity();

        IGremlinQuery IGremlinQuery.Limit(long count) => Limit(count);
        IGremlinQuery IGremlinQuery.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IGremlinQuery.Local<TTargetQuery>(Func<IGremlinQuery , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IGremlinQuery.Map<TTargetQuery>(Func<IGremlinQuery, TTargetQuery> mapping) => Map(mapping);
        
        IGremlinQuery IGremlinQuery.Not(Func<IGremlinQuery, IGremlinQuery> notTraversal) => Not(notTraversal);
        IGremlinQuery IGremlinQuery.None() => None();

        IGremlinQuery IGremlinQuery.Optional(Func<IGremlinQuery, IGremlinQuery> optionalTraversal) => Optional(optionalTraversal);
        IGremlinQuery IGremlinQuery.Or(params Func<IGremlinQuery, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IGremlinQuery IGremlinQuery.Range(long low, long high) => Range(low, high);

        IGremlinQuery IGremlinQuery.Repeat(Func<IGremlinQuery, IGremlinQuery> repeatTraversal) => Repeat(repeatTraversal);
        IGremlinQuery IGremlinQuery.RepeatUntil(Func<IGremlinQuery, IGremlinQuery> repeatTraversal, Func<IGremlinQuery, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);
        IGremlinQuery IGremlinQuery.UntilRepeat(Func<IGremlinQuery, IGremlinQuery> repeatTraversal, Func<IGremlinQuery, IGremlinQuery> untilTraversal) => UntilRepeat(repeatTraversal, untilTraversal);

        IGremlinQuery IGremlinQuery.SideEffect(Func<IGremlinQuery, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);
        IGremlinQuery IGremlinQuery.Skip(long count) => Skip(count);

        IGremlinQuery IGremlinQuery.Tail(long count) => Tail(count);
        IGremlinQuery IGremlinQuery.TailLocal(int count) => TailLocal(count);

        IGremlinQuery IGremlinQuery.Times(int count) => Times(count);

        TTargetQuery IGremlinQuery.Union<TTargetQuery>(params Func<IGremlinQuery, TTargetQuery>[] unionTraversals) => Union(unionTraversals);
        IElementGremlinQuery IElementGremlinQuery.And(params Func<IElementGremlinQuery, IGremlinQuery>[] andTraversals) => And(andTraversals);
        IElementGremlinQuery IElementGremlinQuery.As(params StepLabel[] stepLabels) => As(stepLabels);

        IElementGremlinQuery IElementGremlinQuery.Barrier() => Barrier();

        TTargetQuery IElementGremlinQuery.Choose<TTargetQuery>(Func<IElementGremlinQuery, IGremlinQuery> traversalPredicate, Func<IElementGremlinQuery, TTargetQuery> trueChoice, Func<IElementGremlinQuery, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);
        TTargetQuery IElementGremlinQuery.Choose<TTargetQuery>(Func<IElementGremlinQuery, IGremlinQuery> traversalPredicate, Func<IElementGremlinQuery, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);
        TTargetQuery IElementGremlinQuery.Choose<TTargetQuery>(Func<IChooseBuilder<IElementGremlinQuery>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);

        TTargetQuery IElementGremlinQuery.Coalesce<TTargetQuery>(params Func<IElementGremlinQuery, TTargetQuery>[] traversals) => Coalesce(traversals);

        IElementGremlinQuery IElementGremlinQuery.Dedup() => Dedup();
        IElementGremlinQuery IElementGremlinQuery.Drop() => Drop();

        IElementGremlinQuery IElementGremlinQuery.Emit() => Emit();

        IElementGremlinQuery IElementGremlinQuery.Where(ILambda lambda) => Where(lambda);

        TTargetQuery IElementGremlinQuery.FlatMap<TTargetQuery>(Func<IElementGremlinQuery, TTargetQuery> mapping) => FlatMap(mapping);

        IGremlinQuery<IDictionary<TNewKey, TNewValue>> IElementGremlinQuery.Group<TNewKey, TNewValue>(Func<IGroupBuilder<IElementGremlinQuery>, IGroupBuilderWithKeyAndValue<IElementGremlinQuery, TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);
        IGremlinQuery<IDictionary<TNewKey, object>> IElementGremlinQuery.Group<TNewKey>(Func<IGroupBuilder<IElementGremlinQuery>, IGroupBuilderWithKey<IElementGremlinQuery, TNewKey>> groupBuilder) => Group(groupBuilder);

        IElementGremlinQuery IElementGremlinQuery.Identity() => Identity();

        IElementGremlinQuery IElementGremlinQuery.Limit(long count) => Limit(count);
        IElementGremlinQuery IElementGremlinQuery.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IElementGremlinQuery.Local<TTargetQuery>(Func<IElementGremlinQuery , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IElementGremlinQuery.Map<TTargetQuery>(Func<IElementGremlinQuery, TTargetQuery> mapping) => Map(mapping);
        
        IElementGremlinQuery IElementGremlinQuery.Not(Func<IElementGremlinQuery, IGremlinQuery> notTraversal) => Not(notTraversal);
        IElementGremlinQuery IElementGremlinQuery.None() => None();

        IElementGremlinQuery IElementGremlinQuery.Optional(Func<IElementGremlinQuery, IElementGremlinQuery> optionalTraversal) => Optional(optionalTraversal);
        IElementGremlinQuery IElementGremlinQuery.Or(params Func<IElementGremlinQuery, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IElementGremlinQuery IElementGremlinQuery.Range(long low, long high) => Range(low, high);

        IElementGremlinQuery IElementGremlinQuery.Repeat(Func<IElementGremlinQuery, IElementGremlinQuery> repeatTraversal) => Repeat(repeatTraversal);
        IElementGremlinQuery IElementGremlinQuery.RepeatUntil(Func<IElementGremlinQuery, IElementGremlinQuery> repeatTraversal, Func<IElementGremlinQuery, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);
        IElementGremlinQuery IElementGremlinQuery.UntilRepeat(Func<IElementGremlinQuery, IElementGremlinQuery> repeatTraversal, Func<IElementGremlinQuery, IGremlinQuery> untilTraversal) => UntilRepeat(repeatTraversal, untilTraversal);

        IElementGremlinQuery IElementGremlinQuery.SideEffect(Func<IElementGremlinQuery, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);
        IElementGremlinQuery IElementGremlinQuery.Skip(long count) => Skip(count);

        IElementGremlinQuery IElementGremlinQuery.Tail(long count) => Tail(count);
        IElementGremlinQuery IElementGremlinQuery.TailLocal(int count) => TailLocal(count);

        IElementGremlinQuery IElementGremlinQuery.Times(int count) => Times(count);

        TTargetQuery IElementGremlinQuery.Union<TTargetQuery>(params Func<IElementGremlinQuery, TTargetQuery>[] unionTraversals) => Union(unionTraversals);
        IVertexGremlinQuery IVertexGremlinQuery.And(params Func<IVertexGremlinQuery, IGremlinQuery>[] andTraversals) => And(andTraversals);
        IVertexGremlinQuery IVertexGremlinQuery.As(params StepLabel[] stepLabels) => As(stepLabels);

        IVertexGremlinQuery IVertexGremlinQuery.Barrier() => Barrier();

        TTargetQuery IVertexGremlinQuery.Choose<TTargetQuery>(Func<IVertexGremlinQuery, IGremlinQuery> traversalPredicate, Func<IVertexGremlinQuery, TTargetQuery> trueChoice, Func<IVertexGremlinQuery, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);
        TTargetQuery IVertexGremlinQuery.Choose<TTargetQuery>(Func<IVertexGremlinQuery, IGremlinQuery> traversalPredicate, Func<IVertexGremlinQuery, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);
        TTargetQuery IVertexGremlinQuery.Choose<TTargetQuery>(Func<IChooseBuilder<IVertexGremlinQuery>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);

        TTargetQuery IVertexGremlinQuery.Coalesce<TTargetQuery>(params Func<IVertexGremlinQuery, TTargetQuery>[] traversals) => Coalesce(traversals);

        IVertexGremlinQuery IVertexGremlinQuery.Dedup() => Dedup();
        IVertexGremlinQuery IVertexGremlinQuery.Drop() => Drop();

        IVertexGremlinQuery IVertexGremlinQuery.Emit() => Emit();

        IVertexGremlinQuery IVertexGremlinQuery.Where(ILambda lambda) => Where(lambda);

        TTargetQuery IVertexGremlinQuery.FlatMap<TTargetQuery>(Func<IVertexGremlinQuery, TTargetQuery> mapping) => FlatMap(mapping);

        IGremlinQuery<IDictionary<TNewKey, TNewValue>> IVertexGremlinQuery.Group<TNewKey, TNewValue>(Func<IGroupBuilder<IVertexGremlinQuery>, IGroupBuilderWithKeyAndValue<IVertexGremlinQuery, TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);
        IGremlinQuery<IDictionary<TNewKey, object>> IVertexGremlinQuery.Group<TNewKey>(Func<IGroupBuilder<IVertexGremlinQuery>, IGroupBuilderWithKey<IVertexGremlinQuery, TNewKey>> groupBuilder) => Group(groupBuilder);

        IVertexGremlinQuery IVertexGremlinQuery.Identity() => Identity();

        IVertexGremlinQuery IVertexGremlinQuery.Limit(long count) => Limit(count);
        IVertexGremlinQuery IVertexGremlinQuery.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IVertexGremlinQuery.Local<TTargetQuery>(Func<IVertexGremlinQuery , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IVertexGremlinQuery.Map<TTargetQuery>(Func<IVertexGremlinQuery, TTargetQuery> mapping) => Map(mapping);
        
        IVertexGremlinQuery IVertexGremlinQuery.Not(Func<IVertexGremlinQuery, IGremlinQuery> notTraversal) => Not(notTraversal);
        IVertexGremlinQuery IVertexGremlinQuery.None() => None();

        IVertexGremlinQuery IVertexGremlinQuery.Optional(Func<IVertexGremlinQuery, IVertexGremlinQuery> optionalTraversal) => Optional(optionalTraversal);
        IVertexGremlinQuery IVertexGremlinQuery.Or(params Func<IVertexGremlinQuery, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IVertexGremlinQuery IVertexGremlinQuery.Range(long low, long high) => Range(low, high);

        IVertexGremlinQuery IVertexGremlinQuery.Repeat(Func<IVertexGremlinQuery, IVertexGremlinQuery> repeatTraversal) => Repeat(repeatTraversal);
        IVertexGremlinQuery IVertexGremlinQuery.RepeatUntil(Func<IVertexGremlinQuery, IVertexGremlinQuery> repeatTraversal, Func<IVertexGremlinQuery, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);
        IVertexGremlinQuery IVertexGremlinQuery.UntilRepeat(Func<IVertexGremlinQuery, IVertexGremlinQuery> repeatTraversal, Func<IVertexGremlinQuery, IGremlinQuery> untilTraversal) => UntilRepeat(repeatTraversal, untilTraversal);

        IVertexGremlinQuery IVertexGremlinQuery.SideEffect(Func<IVertexGremlinQuery, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);
        IVertexGremlinQuery IVertexGremlinQuery.Skip(long count) => Skip(count);

        IVertexGremlinQuery IVertexGremlinQuery.Tail(long count) => Tail(count);
        IVertexGremlinQuery IVertexGremlinQuery.TailLocal(int count) => TailLocal(count);

        IVertexGremlinQuery IVertexGremlinQuery.Times(int count) => Times(count);

        TTargetQuery IVertexGremlinQuery.Union<TTargetQuery>(params Func<IVertexGremlinQuery, TTargetQuery>[] unionTraversals) => Union(unionTraversals);
        IEdgeGremlinQuery IEdgeGremlinQuery.And(params Func<IEdgeGremlinQuery, IGremlinQuery>[] andTraversals) => And(andTraversals);
        IEdgeGremlinQuery IEdgeGremlinQuery.As(params StepLabel[] stepLabels) => As(stepLabels);

        IEdgeGremlinQuery IEdgeGremlinQuery.Barrier() => Barrier();

        TTargetQuery IEdgeGremlinQuery.Choose<TTargetQuery>(Func<IEdgeGremlinQuery, IGremlinQuery> traversalPredicate, Func<IEdgeGremlinQuery, TTargetQuery> trueChoice, Func<IEdgeGremlinQuery, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);
        TTargetQuery IEdgeGremlinQuery.Choose<TTargetQuery>(Func<IEdgeGremlinQuery, IGremlinQuery> traversalPredicate, Func<IEdgeGremlinQuery, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);
        TTargetQuery IEdgeGremlinQuery.Choose<TTargetQuery>(Func<IChooseBuilder<IEdgeGremlinQuery>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);

        TTargetQuery IEdgeGremlinQuery.Coalesce<TTargetQuery>(params Func<IEdgeGremlinQuery, TTargetQuery>[] traversals) => Coalesce(traversals);

        IEdgeGremlinQuery IEdgeGremlinQuery.Dedup() => Dedup();
        IEdgeGremlinQuery IEdgeGremlinQuery.Drop() => Drop();

        IEdgeGremlinQuery IEdgeGremlinQuery.Emit() => Emit();

        IEdgeGremlinQuery IEdgeGremlinQuery.Where(ILambda lambda) => Where(lambda);

        TTargetQuery IEdgeGremlinQuery.FlatMap<TTargetQuery>(Func<IEdgeGremlinQuery, TTargetQuery> mapping) => FlatMap(mapping);

        IGremlinQuery<IDictionary<TNewKey, TNewValue>> IEdgeGremlinQuery.Group<TNewKey, TNewValue>(Func<IGroupBuilder<IEdgeGremlinQuery>, IGroupBuilderWithKeyAndValue<IEdgeGremlinQuery, TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);
        IGremlinQuery<IDictionary<TNewKey, object>> IEdgeGremlinQuery.Group<TNewKey>(Func<IGroupBuilder<IEdgeGremlinQuery>, IGroupBuilderWithKey<IEdgeGremlinQuery, TNewKey>> groupBuilder) => Group(groupBuilder);

        IEdgeGremlinQuery IEdgeGremlinQuery.Identity() => Identity();

        IEdgeGremlinQuery IEdgeGremlinQuery.Limit(long count) => Limit(count);
        IEdgeGremlinQuery IEdgeGremlinQuery.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IEdgeGremlinQuery.Local<TTargetQuery>(Func<IEdgeGremlinQuery , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IEdgeGremlinQuery.Map<TTargetQuery>(Func<IEdgeGremlinQuery, TTargetQuery> mapping) => Map(mapping);
        
        IEdgeGremlinQuery IEdgeGremlinQuery.Not(Func<IEdgeGremlinQuery, IGremlinQuery> notTraversal) => Not(notTraversal);
        IEdgeGremlinQuery IEdgeGremlinQuery.None() => None();

        IEdgeGremlinQuery IEdgeGremlinQuery.Optional(Func<IEdgeGremlinQuery, IEdgeGremlinQuery> optionalTraversal) => Optional(optionalTraversal);
        IEdgeGremlinQuery IEdgeGremlinQuery.Or(params Func<IEdgeGremlinQuery, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IEdgeGremlinQuery IEdgeGremlinQuery.Range(long low, long high) => Range(low, high);

        IEdgeGremlinQuery IEdgeGremlinQuery.Repeat(Func<IEdgeGremlinQuery, IEdgeGremlinQuery> repeatTraversal) => Repeat(repeatTraversal);
        IEdgeGremlinQuery IEdgeGremlinQuery.RepeatUntil(Func<IEdgeGremlinQuery, IEdgeGremlinQuery> repeatTraversal, Func<IEdgeGremlinQuery, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);
        IEdgeGremlinQuery IEdgeGremlinQuery.UntilRepeat(Func<IEdgeGremlinQuery, IEdgeGremlinQuery> repeatTraversal, Func<IEdgeGremlinQuery, IGremlinQuery> untilTraversal) => UntilRepeat(repeatTraversal, untilTraversal);

        IEdgeGremlinQuery IEdgeGremlinQuery.SideEffect(Func<IEdgeGremlinQuery, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);
        IEdgeGremlinQuery IEdgeGremlinQuery.Skip(long count) => Skip(count);

        IEdgeGremlinQuery IEdgeGremlinQuery.Tail(long count) => Tail(count);
        IEdgeGremlinQuery IEdgeGremlinQuery.TailLocal(int count) => TailLocal(count);

        IEdgeGremlinQuery IEdgeGremlinQuery.Times(int count) => Times(count);

        TTargetQuery IEdgeGremlinQuery.Union<TTargetQuery>(params Func<IEdgeGremlinQuery, TTargetQuery>[] unionTraversals) => Union(unionTraversals);
        IGremlinQuery<TElement> IGremlinQuery<TElement>.And(params Func<IGremlinQuery<TElement>, IGremlinQuery>[] andTraversals) => And(andTraversals);
        IGremlinQuery<TElement> IGremlinQuery<TElement>.As(params StepLabel[] stepLabels) => As(stepLabels);

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Barrier() => Barrier();

        TTargetQuery IGremlinQuery<TElement>.Choose<TTargetQuery>(Func<IGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IGremlinQuery<TElement>, TTargetQuery> trueChoice, Func<IGremlinQuery<TElement>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);
        TTargetQuery IGremlinQuery<TElement>.Choose<TTargetQuery>(Func<IGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IGremlinQuery<TElement>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);
        TTargetQuery IGremlinQuery<TElement>.Choose<TTargetQuery>(Func<IChooseBuilder<IGremlinQuery<TElement>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);

        TTargetQuery IGremlinQuery<TElement>.Coalesce<TTargetQuery>(params Func<IGremlinQuery<TElement>, TTargetQuery>[] traversals) => Coalesce(traversals);

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Dedup() => Dedup();
        IGremlinQuery<TElement> IGremlinQuery<TElement>.Drop() => Drop();

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Emit() => Emit();

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Where(ILambda lambda) => Where(lambda);

        TTargetQuery IGremlinQuery<TElement>.FlatMap<TTargetQuery>(Func<IGremlinQuery<TElement>, TTargetQuery> mapping) => FlatMap(mapping);

        IGremlinQuery<IDictionary<TNewKey, TNewValue>> IGremlinQuery<TElement>.Group<TNewKey, TNewValue>(Func<IGroupBuilder<IGremlinQuery<TElement>>, IGroupBuilderWithKeyAndValue<IGremlinQuery<TElement>, TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);
        IGremlinQuery<IDictionary<TNewKey, object>> IGremlinQuery<TElement>.Group<TNewKey>(Func<IGroupBuilder<IGremlinQuery<TElement>>, IGroupBuilderWithKey<IGremlinQuery<TElement>, TNewKey>> groupBuilder) => Group(groupBuilder);

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Identity() => Identity();

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Limit(long count) => Limit(count);
        IGremlinQuery<TElement> IGremlinQuery<TElement>.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IGremlinQuery<TElement>.Local<TTargetQuery>(Func<IGremlinQuery<TElement> , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IGremlinQuery<TElement>.Map<TTargetQuery>(Func<IGremlinQuery<TElement>, TTargetQuery> mapping) => Map(mapping);
        
        IGremlinQuery<TElement> IGremlinQuery<TElement>.Not(Func<IGremlinQuery<TElement>, IGremlinQuery> notTraversal) => Not(notTraversal);
        IGremlinQuery<TElement> IGremlinQuery<TElement>.None() => None();

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Optional(Func<IGremlinQuery<TElement>, IGremlinQuery<TElement>> optionalTraversal) => Optional(optionalTraversal);
        IGremlinQuery<TElement> IGremlinQuery<TElement>.Or(params Func<IGremlinQuery<TElement>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Range(long low, long high) => Range(low, high);

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Repeat(Func<IGremlinQuery<TElement>, IGremlinQuery<TElement>> repeatTraversal) => Repeat(repeatTraversal);
        IGremlinQuery<TElement> IGremlinQuery<TElement>.RepeatUntil(Func<IGremlinQuery<TElement>, IGremlinQuery<TElement>> repeatTraversal, Func<IGremlinQuery<TElement>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);
        IGremlinQuery<TElement> IGremlinQuery<TElement>.UntilRepeat(Func<IGremlinQuery<TElement>, IGremlinQuery<TElement>> repeatTraversal, Func<IGremlinQuery<TElement>, IGremlinQuery> untilTraversal) => UntilRepeat(repeatTraversal, untilTraversal);

        IGremlinQuery<TElement> IGremlinQuery<TElement>.SideEffect(Func<IGremlinQuery<TElement>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);
        IGremlinQuery<TElement> IGremlinQuery<TElement>.Skip(long count) => Skip(count);

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Tail(long count) => Tail(count);
        IGremlinQuery<TElement> IGremlinQuery<TElement>.TailLocal(int count) => TailLocal(count);

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Times(int count) => Times(count);

        TTargetQuery IGremlinQuery<TElement>.Union<TTargetQuery>(params Func<IGremlinQuery<TElement>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);
        IValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.And(params Func<IValueGremlinQuery<TElement>, IGremlinQuery>[] andTraversals) => And(andTraversals);
        IValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.As(params StepLabel[] stepLabels) => As(stepLabels);

        IValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.Barrier() => Barrier();

        TTargetQuery IValueGremlinQuery<TElement>.Choose<TTargetQuery>(Func<IValueGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IValueGremlinQuery<TElement>, TTargetQuery> trueChoice, Func<IValueGremlinQuery<TElement>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);
        TTargetQuery IValueGremlinQuery<TElement>.Choose<TTargetQuery>(Func<IValueGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IValueGremlinQuery<TElement>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);
        TTargetQuery IValueGremlinQuery<TElement>.Choose<TTargetQuery>(Func<IChooseBuilder<IValueGremlinQuery<TElement>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);

        TTargetQuery IValueGremlinQuery<TElement>.Coalesce<TTargetQuery>(params Func<IValueGremlinQuery<TElement>, TTargetQuery>[] traversals) => Coalesce(traversals);

        IValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.Dedup() => Dedup();
        IValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.Drop() => Drop();

        IValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.Emit() => Emit();

        IValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.Where(ILambda lambda) => Where(lambda);

        TTargetQuery IValueGremlinQuery<TElement>.FlatMap<TTargetQuery>(Func<IValueGremlinQuery<TElement>, TTargetQuery> mapping) => FlatMap(mapping);

        IGremlinQuery<IDictionary<TNewKey, TNewValue>> IValueGremlinQuery<TElement>.Group<TNewKey, TNewValue>(Func<IGroupBuilder<IValueGremlinQuery<TElement>>, IGroupBuilderWithKeyAndValue<IValueGremlinQuery<TElement>, TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);
        IGremlinQuery<IDictionary<TNewKey, object>> IValueGremlinQuery<TElement>.Group<TNewKey>(Func<IGroupBuilder<IValueGremlinQuery<TElement>>, IGroupBuilderWithKey<IValueGremlinQuery<TElement>, TNewKey>> groupBuilder) => Group(groupBuilder);

        IValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.Identity() => Identity();

        IValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.Limit(long count) => Limit(count);
        IValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IValueGremlinQuery<TElement>.Local<TTargetQuery>(Func<IValueGremlinQuery<TElement> , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IValueGremlinQuery<TElement>.Map<TTargetQuery>(Func<IValueGremlinQuery<TElement>, TTargetQuery> mapping) => Map(mapping);
        
        IValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.Not(Func<IValueGremlinQuery<TElement>, IGremlinQuery> notTraversal) => Not(notTraversal);
        IValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.None() => None();

        IValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.Optional(Func<IValueGremlinQuery<TElement>, IValueGremlinQuery<TElement>> optionalTraversal) => Optional(optionalTraversal);
        IValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.Or(params Func<IValueGremlinQuery<TElement>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.Range(long low, long high) => Range(low, high);

        IValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.Repeat(Func<IValueGremlinQuery<TElement>, IValueGremlinQuery<TElement>> repeatTraversal) => Repeat(repeatTraversal);
        IValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.RepeatUntil(Func<IValueGremlinQuery<TElement>, IValueGremlinQuery<TElement>> repeatTraversal, Func<IValueGremlinQuery<TElement>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);
        IValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.UntilRepeat(Func<IValueGremlinQuery<TElement>, IValueGremlinQuery<TElement>> repeatTraversal, Func<IValueGremlinQuery<TElement>, IGremlinQuery> untilTraversal) => UntilRepeat(repeatTraversal, untilTraversal);

        IValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.SideEffect(Func<IValueGremlinQuery<TElement>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);
        IValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.Skip(long count) => Skip(count);

        IValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.Tail(long count) => Tail(count);
        IValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.TailLocal(int count) => TailLocal(count);

        IValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.Times(int count) => Times(count);

        TTargetQuery IValueGremlinQuery<TElement>.Union<TTargetQuery>(params Func<IValueGremlinQuery<TElement>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);
        IArrayGremlinQuery<TElement, TFoldedQuery> IArrayGremlinQuery<TElement, TFoldedQuery>.And(params Func<IArrayGremlinQuery<TElement, TFoldedQuery>, IGremlinQuery>[] andTraversals) => And(andTraversals);
        IArrayGremlinQuery<TElement, TFoldedQuery> IArrayGremlinQuery<TElement, TFoldedQuery>.As(params StepLabel[] stepLabels) => As(stepLabels);

        IArrayGremlinQuery<TElement, TFoldedQuery> IArrayGremlinQuery<TElement, TFoldedQuery>.Barrier() => Barrier();

        TTargetQuery IArrayGremlinQuery<TElement, TFoldedQuery>.Choose<TTargetQuery>(Func<IArrayGremlinQuery<TElement, TFoldedQuery>, IGremlinQuery> traversalPredicate, Func<IArrayGremlinQuery<TElement, TFoldedQuery>, TTargetQuery> trueChoice, Func<IArrayGremlinQuery<TElement, TFoldedQuery>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);
        TTargetQuery IArrayGremlinQuery<TElement, TFoldedQuery>.Choose<TTargetQuery>(Func<IArrayGremlinQuery<TElement, TFoldedQuery>, IGremlinQuery> traversalPredicate, Func<IArrayGremlinQuery<TElement, TFoldedQuery>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);
        TTargetQuery IArrayGremlinQuery<TElement, TFoldedQuery>.Choose<TTargetQuery>(Func<IChooseBuilder<IArrayGremlinQuery<TElement, TFoldedQuery>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);

        TTargetQuery IArrayGremlinQuery<TElement, TFoldedQuery>.Coalesce<TTargetQuery>(params Func<IArrayGremlinQuery<TElement, TFoldedQuery>, TTargetQuery>[] traversals) => Coalesce(traversals);

        IArrayGremlinQuery<TElement, TFoldedQuery> IArrayGremlinQuery<TElement, TFoldedQuery>.Dedup() => Dedup();
        IArrayGremlinQuery<TElement, TFoldedQuery> IArrayGremlinQuery<TElement, TFoldedQuery>.Drop() => Drop();

        IArrayGremlinQuery<TElement, TFoldedQuery> IArrayGremlinQuery<TElement, TFoldedQuery>.Emit() => Emit();

        IArrayGremlinQuery<TElement, TFoldedQuery> IArrayGremlinQuery<TElement, TFoldedQuery>.Where(ILambda lambda) => Where(lambda);

        TTargetQuery IArrayGremlinQuery<TElement, TFoldedQuery>.FlatMap<TTargetQuery>(Func<IArrayGremlinQuery<TElement, TFoldedQuery>, TTargetQuery> mapping) => FlatMap(mapping);

        IGremlinQuery<IDictionary<TNewKey, TNewValue>> IArrayGremlinQuery<TElement, TFoldedQuery>.Group<TNewKey, TNewValue>(Func<IGroupBuilder<IArrayGremlinQuery<TElement, TFoldedQuery>>, IGroupBuilderWithKeyAndValue<IArrayGremlinQuery<TElement, TFoldedQuery>, TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);
        IGremlinQuery<IDictionary<TNewKey, object>> IArrayGremlinQuery<TElement, TFoldedQuery>.Group<TNewKey>(Func<IGroupBuilder<IArrayGremlinQuery<TElement, TFoldedQuery>>, IGroupBuilderWithKey<IArrayGremlinQuery<TElement, TFoldedQuery>, TNewKey>> groupBuilder) => Group(groupBuilder);

        IArrayGremlinQuery<TElement, TFoldedQuery> IArrayGremlinQuery<TElement, TFoldedQuery>.Identity() => Identity();

        IArrayGremlinQuery<TElement, TFoldedQuery> IArrayGremlinQuery<TElement, TFoldedQuery>.Limit(long count) => Limit(count);
        IArrayGremlinQuery<TElement, TFoldedQuery> IArrayGremlinQuery<TElement, TFoldedQuery>.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IArrayGremlinQuery<TElement, TFoldedQuery>.Local<TTargetQuery>(Func<IArrayGremlinQuery<TElement, TFoldedQuery> , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IArrayGremlinQuery<TElement, TFoldedQuery>.Map<TTargetQuery>(Func<IArrayGremlinQuery<TElement, TFoldedQuery>, TTargetQuery> mapping) => Map(mapping);
        
        IArrayGremlinQuery<TElement, TFoldedQuery> IArrayGremlinQuery<TElement, TFoldedQuery>.Not(Func<IArrayGremlinQuery<TElement, TFoldedQuery>, IGremlinQuery> notTraversal) => Not(notTraversal);
        IArrayGremlinQuery<TElement, TFoldedQuery> IArrayGremlinQuery<TElement, TFoldedQuery>.None() => None();

        IArrayGremlinQuery<TElement, TFoldedQuery> IArrayGremlinQuery<TElement, TFoldedQuery>.Optional(Func<IArrayGremlinQuery<TElement, TFoldedQuery>, IArrayGremlinQuery<TElement, TFoldedQuery>> optionalTraversal) => Optional(optionalTraversal);
        IArrayGremlinQuery<TElement, TFoldedQuery> IArrayGremlinQuery<TElement, TFoldedQuery>.Or(params Func<IArrayGremlinQuery<TElement, TFoldedQuery>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IArrayGremlinQuery<TElement, TFoldedQuery> IArrayGremlinQuery<TElement, TFoldedQuery>.Range(long low, long high) => Range(low, high);

        IArrayGremlinQuery<TElement, TFoldedQuery> IArrayGremlinQuery<TElement, TFoldedQuery>.Repeat(Func<IArrayGremlinQuery<TElement, TFoldedQuery>, IArrayGremlinQuery<TElement, TFoldedQuery>> repeatTraversal) => Repeat(repeatTraversal);
        IArrayGremlinQuery<TElement, TFoldedQuery> IArrayGremlinQuery<TElement, TFoldedQuery>.RepeatUntil(Func<IArrayGremlinQuery<TElement, TFoldedQuery>, IArrayGremlinQuery<TElement, TFoldedQuery>> repeatTraversal, Func<IArrayGremlinQuery<TElement, TFoldedQuery>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);
        IArrayGremlinQuery<TElement, TFoldedQuery> IArrayGremlinQuery<TElement, TFoldedQuery>.UntilRepeat(Func<IArrayGremlinQuery<TElement, TFoldedQuery>, IArrayGremlinQuery<TElement, TFoldedQuery>> repeatTraversal, Func<IArrayGremlinQuery<TElement, TFoldedQuery>, IGremlinQuery> untilTraversal) => UntilRepeat(repeatTraversal, untilTraversal);

        IArrayGremlinQuery<TElement, TFoldedQuery> IArrayGremlinQuery<TElement, TFoldedQuery>.SideEffect(Func<IArrayGremlinQuery<TElement, TFoldedQuery>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);
        IArrayGremlinQuery<TElement, TFoldedQuery> IArrayGremlinQuery<TElement, TFoldedQuery>.Skip(long count) => Skip(count);

        IArrayGremlinQuery<TElement, TFoldedQuery> IArrayGremlinQuery<TElement, TFoldedQuery>.Tail(long count) => Tail(count);
        IArrayGremlinQuery<TElement, TFoldedQuery> IArrayGremlinQuery<TElement, TFoldedQuery>.TailLocal(int count) => TailLocal(count);

        IArrayGremlinQuery<TElement, TFoldedQuery> IArrayGremlinQuery<TElement, TFoldedQuery>.Times(int count) => Times(count);

        TTargetQuery IArrayGremlinQuery<TElement, TFoldedQuery>.Union<TTargetQuery>(params Func<IArrayGremlinQuery<TElement, TFoldedQuery>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);
        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.And(params Func<IElementGremlinQuery<TElement>, IGremlinQuery>[] andTraversals) => And(andTraversals);
        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.As(params StepLabel[] stepLabels) => As(stepLabels);

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Barrier() => Barrier();

        TTargetQuery IElementGremlinQuery<TElement>.Choose<TTargetQuery>(Func<IElementGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IElementGremlinQuery<TElement>, TTargetQuery> trueChoice, Func<IElementGremlinQuery<TElement>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);
        TTargetQuery IElementGremlinQuery<TElement>.Choose<TTargetQuery>(Func<IElementGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IElementGremlinQuery<TElement>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);
        TTargetQuery IElementGremlinQuery<TElement>.Choose<TTargetQuery>(Func<IChooseBuilder<IElementGremlinQuery<TElement>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);

        TTargetQuery IElementGremlinQuery<TElement>.Coalesce<TTargetQuery>(params Func<IElementGremlinQuery<TElement>, TTargetQuery>[] traversals) => Coalesce(traversals);

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Dedup() => Dedup();
        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Drop() => Drop();

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Emit() => Emit();

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Where(ILambda lambda) => Where(lambda);

        TTargetQuery IElementGremlinQuery<TElement>.FlatMap<TTargetQuery>(Func<IElementGremlinQuery<TElement>, TTargetQuery> mapping) => FlatMap(mapping);

        IGremlinQuery<IDictionary<TNewKey, TNewValue>> IElementGremlinQuery<TElement>.Group<TNewKey, TNewValue>(Func<IGroupBuilder<IElementGremlinQuery<TElement>>, IGroupBuilderWithKeyAndValue<IElementGremlinQuery<TElement>, TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);
        IGremlinQuery<IDictionary<TNewKey, object>> IElementGremlinQuery<TElement>.Group<TNewKey>(Func<IGroupBuilder<IElementGremlinQuery<TElement>>, IGroupBuilderWithKey<IElementGremlinQuery<TElement>, TNewKey>> groupBuilder) => Group(groupBuilder);

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Identity() => Identity();

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Limit(long count) => Limit(count);
        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IElementGremlinQuery<TElement>.Local<TTargetQuery>(Func<IElementGremlinQuery<TElement> , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IElementGremlinQuery<TElement>.Map<TTargetQuery>(Func<IElementGremlinQuery<TElement>, TTargetQuery> mapping) => Map(mapping);
        
        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Not(Func<IElementGremlinQuery<TElement>, IGremlinQuery> notTraversal) => Not(notTraversal);
        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.None() => None();

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Optional(Func<IElementGremlinQuery<TElement>, IElementGremlinQuery<TElement>> optionalTraversal) => Optional(optionalTraversal);
        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Or(params Func<IElementGremlinQuery<TElement>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Range(long low, long high) => Range(low, high);

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Repeat(Func<IElementGremlinQuery<TElement>, IElementGremlinQuery<TElement>> repeatTraversal) => Repeat(repeatTraversal);
        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.RepeatUntil(Func<IElementGremlinQuery<TElement>, IElementGremlinQuery<TElement>> repeatTraversal, Func<IElementGremlinQuery<TElement>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);
        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.UntilRepeat(Func<IElementGremlinQuery<TElement>, IElementGremlinQuery<TElement>> repeatTraversal, Func<IElementGremlinQuery<TElement>, IGremlinQuery> untilTraversal) => UntilRepeat(repeatTraversal, untilTraversal);

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.SideEffect(Func<IElementGremlinQuery<TElement>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);
        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Skip(long count) => Skip(count);

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Tail(long count) => Tail(count);
        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.TailLocal(int count) => TailLocal(count);

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Times(int count) => Times(count);

        TTargetQuery IElementGremlinQuery<TElement>.Union<TTargetQuery>(params Func<IElementGremlinQuery<TElement>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);
        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.And(params Func<IVertexGremlinQuery<TElement>, IGremlinQuery>[] andTraversals) => And(andTraversals);
        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.As(params StepLabel[] stepLabels) => As(stepLabels);

        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Barrier() => Barrier();

        TTargetQuery IVertexGremlinQuery<TElement>.Choose<TTargetQuery>(Func<IVertexGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IVertexGremlinQuery<TElement>, TTargetQuery> trueChoice, Func<IVertexGremlinQuery<TElement>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);
        TTargetQuery IVertexGremlinQuery<TElement>.Choose<TTargetQuery>(Func<IVertexGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IVertexGremlinQuery<TElement>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);
        TTargetQuery IVertexGremlinQuery<TElement>.Choose<TTargetQuery>(Func<IChooseBuilder<IVertexGremlinQuery<TElement>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);

        TTargetQuery IVertexGremlinQuery<TElement>.Coalesce<TTargetQuery>(params Func<IVertexGremlinQuery<TElement>, TTargetQuery>[] traversals) => Coalesce(traversals);

        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Dedup() => Dedup();
        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Drop() => Drop();

        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Emit() => Emit();

        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Where(ILambda lambda) => Where(lambda);

        TTargetQuery IVertexGremlinQuery<TElement>.FlatMap<TTargetQuery>(Func<IVertexGremlinQuery<TElement>, TTargetQuery> mapping) => FlatMap(mapping);

        IGremlinQuery<IDictionary<TNewKey, TNewValue>> IVertexGremlinQuery<TElement>.Group<TNewKey, TNewValue>(Func<IGroupBuilder<IVertexGremlinQuery<TElement>>, IGroupBuilderWithKeyAndValue<IVertexGremlinQuery<TElement>, TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);
        IGremlinQuery<IDictionary<TNewKey, object>> IVertexGremlinQuery<TElement>.Group<TNewKey>(Func<IGroupBuilder<IVertexGremlinQuery<TElement>>, IGroupBuilderWithKey<IVertexGremlinQuery<TElement>, TNewKey>> groupBuilder) => Group(groupBuilder);

        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Identity() => Identity();

        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Limit(long count) => Limit(count);
        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IVertexGremlinQuery<TElement>.Local<TTargetQuery>(Func<IVertexGremlinQuery<TElement> , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IVertexGremlinQuery<TElement>.Map<TTargetQuery>(Func<IVertexGremlinQuery<TElement>, TTargetQuery> mapping) => Map(mapping);
        
        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Not(Func<IVertexGremlinQuery<TElement>, IGremlinQuery> notTraversal) => Not(notTraversal);
        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.None() => None();

        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Optional(Func<IVertexGremlinQuery<TElement>, IVertexGremlinQuery<TElement>> optionalTraversal) => Optional(optionalTraversal);
        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Or(params Func<IVertexGremlinQuery<TElement>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Range(long low, long high) => Range(low, high);

        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Repeat(Func<IVertexGremlinQuery<TElement>, IVertexGremlinQuery<TElement>> repeatTraversal) => Repeat(repeatTraversal);
        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.RepeatUntil(Func<IVertexGremlinQuery<TElement>, IVertexGremlinQuery<TElement>> repeatTraversal, Func<IVertexGremlinQuery<TElement>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);
        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.UntilRepeat(Func<IVertexGremlinQuery<TElement>, IVertexGremlinQuery<TElement>> repeatTraversal, Func<IVertexGremlinQuery<TElement>, IGremlinQuery> untilTraversal) => UntilRepeat(repeatTraversal, untilTraversal);

        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.SideEffect(Func<IVertexGremlinQuery<TElement>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);
        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Skip(long count) => Skip(count);

        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Tail(long count) => Tail(count);
        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.TailLocal(int count) => TailLocal(count);

        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Times(int count) => Times(count);

        TTargetQuery IVertexGremlinQuery<TElement>.Union<TTargetQuery>(params Func<IVertexGremlinQuery<TElement>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);
        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.And(params Func<IEdgeGremlinQuery<TElement>, IGremlinQuery>[] andTraversals) => And(andTraversals);
        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.As(params StepLabel[] stepLabels) => As(stepLabels);

        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Barrier() => Barrier();

        TTargetQuery IEdgeGremlinQuery<TElement>.Choose<TTargetQuery>(Func<IEdgeGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IEdgeGremlinQuery<TElement>, TTargetQuery> trueChoice, Func<IEdgeGremlinQuery<TElement>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);
        TTargetQuery IEdgeGremlinQuery<TElement>.Choose<TTargetQuery>(Func<IEdgeGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IEdgeGremlinQuery<TElement>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);
        TTargetQuery IEdgeGremlinQuery<TElement>.Choose<TTargetQuery>(Func<IChooseBuilder<IEdgeGremlinQuery<TElement>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);

        TTargetQuery IEdgeGremlinQuery<TElement>.Coalesce<TTargetQuery>(params Func<IEdgeGremlinQuery<TElement>, TTargetQuery>[] traversals) => Coalesce(traversals);

        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Dedup() => Dedup();
        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Drop() => Drop();

        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Emit() => Emit();

        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Where(ILambda lambda) => Where(lambda);

        TTargetQuery IEdgeGremlinQuery<TElement>.FlatMap<TTargetQuery>(Func<IEdgeGremlinQuery<TElement>, TTargetQuery> mapping) => FlatMap(mapping);

        IGremlinQuery<IDictionary<TNewKey, TNewValue>> IEdgeGremlinQuery<TElement>.Group<TNewKey, TNewValue>(Func<IGroupBuilder<IEdgeGremlinQuery<TElement>>, IGroupBuilderWithKeyAndValue<IEdgeGremlinQuery<TElement>, TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);
        IGremlinQuery<IDictionary<TNewKey, object>> IEdgeGremlinQuery<TElement>.Group<TNewKey>(Func<IGroupBuilder<IEdgeGremlinQuery<TElement>>, IGroupBuilderWithKey<IEdgeGremlinQuery<TElement>, TNewKey>> groupBuilder) => Group(groupBuilder);

        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Identity() => Identity();

        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Limit(long count) => Limit(count);
        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IEdgeGremlinQuery<TElement>.Local<TTargetQuery>(Func<IEdgeGremlinQuery<TElement> , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IEdgeGremlinQuery<TElement>.Map<TTargetQuery>(Func<IEdgeGremlinQuery<TElement>, TTargetQuery> mapping) => Map(mapping);
        
        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Not(Func<IEdgeGremlinQuery<TElement>, IGremlinQuery> notTraversal) => Not(notTraversal);
        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.None() => None();

        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Optional(Func<IEdgeGremlinQuery<TElement>, IEdgeGremlinQuery<TElement>> optionalTraversal) => Optional(optionalTraversal);
        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Or(params Func<IEdgeGremlinQuery<TElement>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Range(long low, long high) => Range(low, high);

        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Repeat(Func<IEdgeGremlinQuery<TElement>, IEdgeGremlinQuery<TElement>> repeatTraversal) => Repeat(repeatTraversal);
        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.RepeatUntil(Func<IEdgeGremlinQuery<TElement>, IEdgeGremlinQuery<TElement>> repeatTraversal, Func<IEdgeGremlinQuery<TElement>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);
        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.UntilRepeat(Func<IEdgeGremlinQuery<TElement>, IEdgeGremlinQuery<TElement>> repeatTraversal, Func<IEdgeGremlinQuery<TElement>, IGremlinQuery> untilTraversal) => UntilRepeat(repeatTraversal, untilTraversal);

        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.SideEffect(Func<IEdgeGremlinQuery<TElement>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);
        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Skip(long count) => Skip(count);

        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Tail(long count) => Tail(count);
        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.TailLocal(int count) => TailLocal(count);

        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Times(int count) => Times(count);

        TTargetQuery IEdgeGremlinQuery<TElement>.Union<TTargetQuery>(params Func<IEdgeGremlinQuery<TElement>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);
        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.And(params Func<IEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery>[] andTraversals) => And(andTraversals);
        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.As(params StepLabel[] stepLabels) => As(stepLabels);

        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Barrier() => Barrier();

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex>.Choose<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> traversalPredicate, Func<IEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery> trueChoice, Func<IEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);
        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex>.Choose<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> traversalPredicate, Func<IEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);
        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex>.Choose<TTargetQuery>(Func<IChooseBuilder<IEdgeGremlinQuery<TElement, TOutVertex>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex>.Coalesce<TTargetQuery>(params Func<IEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery>[] traversals) => Coalesce(traversals);

        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Dedup() => Dedup();
        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Drop() => Drop();

        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Emit() => Emit();

        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Where(ILambda lambda) => Where(lambda);

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex>.FlatMap<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery> mapping) => FlatMap(mapping);

        IGremlinQuery<IDictionary<TNewKey, TNewValue>> IEdgeGremlinQuery<TElement, TOutVertex>.Group<TNewKey, TNewValue>(Func<IGroupBuilder<IEdgeGremlinQuery<TElement, TOutVertex>>, IGroupBuilderWithKeyAndValue<IEdgeGremlinQuery<TElement, TOutVertex>, TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);
        IGremlinQuery<IDictionary<TNewKey, object>> IEdgeGremlinQuery<TElement, TOutVertex>.Group<TNewKey>(Func<IGroupBuilder<IEdgeGremlinQuery<TElement, TOutVertex>>, IGroupBuilderWithKey<IEdgeGremlinQuery<TElement, TOutVertex>, TNewKey>> groupBuilder) => Group(groupBuilder);

        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Identity() => Identity();

        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Limit(long count) => Limit(count);
        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex>.Local<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex> , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex>.Map<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery> mapping) => Map(mapping);
        
        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Not(Func<IEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> notTraversal) => Not(notTraversal);
        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.None() => None();

        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Optional(Func<IEdgeGremlinQuery<TElement, TOutVertex>, IEdgeGremlinQuery<TElement, TOutVertex>> optionalTraversal) => Optional(optionalTraversal);
        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Or(params Func<IEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Range(long low, long high) => Range(low, high);

        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Repeat(Func<IEdgeGremlinQuery<TElement, TOutVertex>, IEdgeGremlinQuery<TElement, TOutVertex>> repeatTraversal) => Repeat(repeatTraversal);
        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.RepeatUntil(Func<IEdgeGremlinQuery<TElement, TOutVertex>, IEdgeGremlinQuery<TElement, TOutVertex>> repeatTraversal, Func<IEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);
        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.UntilRepeat(Func<IEdgeGremlinQuery<TElement, TOutVertex>, IEdgeGremlinQuery<TElement, TOutVertex>> repeatTraversal, Func<IEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> untilTraversal) => UntilRepeat(repeatTraversal, untilTraversal);

        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.SideEffect(Func<IEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);
        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Skip(long count) => Skip(count);

        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Tail(long count) => Tail(count);
        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.TailLocal(int count) => TailLocal(count);

        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Times(int count) => Times(count);

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex>.Union<TTargetQuery>(params Func<IEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);
        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.And(params Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery>[] andTraversals) => And(andTraversals);
        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.As(params StepLabel[] stepLabels) => As(stepLabels);

        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Barrier() => Barrier();

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Choose<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery> traversalPredicate, Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TTargetQuery> trueChoice, Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);
        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Choose<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery> traversalPredicate, Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);
        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Choose<TTargetQuery>(Func<IChooseBuilder<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Coalesce<TTargetQuery>(params Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TTargetQuery>[] traversals) => Coalesce(traversals);

        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Dedup() => Dedup();
        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Drop() => Drop();

        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Emit() => Emit();

        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Where(ILambda lambda) => Where(lambda);

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.FlatMap<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TTargetQuery> mapping) => FlatMap(mapping);

        IGremlinQuery<IDictionary<TNewKey, TNewValue>> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Group<TNewKey, TNewValue>(Func<IGroupBuilder<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>, IGroupBuilderWithKeyAndValue<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);
        IGremlinQuery<IDictionary<TNewKey, object>> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Group<TNewKey>(Func<IGroupBuilder<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>, IGroupBuilderWithKey<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TNewKey>> groupBuilder) => Group(groupBuilder);

        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Identity() => Identity();

        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Limit(long count) => Limit(count);
        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Local<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Map<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TTargetQuery> mapping) => Map(mapping);
        
        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Not(Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery> notTraversal) => Not(notTraversal);
        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.None() => None();

        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Optional(Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>> optionalTraversal) => Optional(optionalTraversal);
        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Or(params Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Range(long low, long high) => Range(low, high);

        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Repeat(Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>> repeatTraversal) => Repeat(repeatTraversal);
        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.RepeatUntil(Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>> repeatTraversal, Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);
        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.UntilRepeat(Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>> repeatTraversal, Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery> untilTraversal) => UntilRepeat(repeatTraversal, untilTraversal);

        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.SideEffect(Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);
        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Skip(long count) => Skip(count);

        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Tail(long count) => Tail(count);
        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.TailLocal(int count) => TailLocal(count);

        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Times(int count) => Times(count);

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Union<TTargetQuery>(params Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);
        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.And(params Func<IInEdgeGremlinQuery<TElement, TInVertex>, IGremlinQuery>[] andTraversals) => And(andTraversals);
        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.As(params StepLabel[] stepLabels) => As(stepLabels);

        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Barrier() => Barrier();

        TTargetQuery IInEdgeGremlinQuery<TElement, TInVertex>.Choose<TTargetQuery>(Func<IInEdgeGremlinQuery<TElement, TInVertex>, IGremlinQuery> traversalPredicate, Func<IInEdgeGremlinQuery<TElement, TInVertex>, TTargetQuery> trueChoice, Func<IInEdgeGremlinQuery<TElement, TInVertex>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);
        TTargetQuery IInEdgeGremlinQuery<TElement, TInVertex>.Choose<TTargetQuery>(Func<IInEdgeGremlinQuery<TElement, TInVertex>, IGremlinQuery> traversalPredicate, Func<IInEdgeGremlinQuery<TElement, TInVertex>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);
        TTargetQuery IInEdgeGremlinQuery<TElement, TInVertex>.Choose<TTargetQuery>(Func<IChooseBuilder<IInEdgeGremlinQuery<TElement, TInVertex>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);

        TTargetQuery IInEdgeGremlinQuery<TElement, TInVertex>.Coalesce<TTargetQuery>(params Func<IInEdgeGremlinQuery<TElement, TInVertex>, TTargetQuery>[] traversals) => Coalesce(traversals);

        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Dedup() => Dedup();
        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Drop() => Drop();

        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Emit() => Emit();

        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Where(ILambda lambda) => Where(lambda);

        TTargetQuery IInEdgeGremlinQuery<TElement, TInVertex>.FlatMap<TTargetQuery>(Func<IInEdgeGremlinQuery<TElement, TInVertex>, TTargetQuery> mapping) => FlatMap(mapping);

        IGremlinQuery<IDictionary<TNewKey, TNewValue>> IInEdgeGremlinQuery<TElement, TInVertex>.Group<TNewKey, TNewValue>(Func<IGroupBuilder<IInEdgeGremlinQuery<TElement, TInVertex>>, IGroupBuilderWithKeyAndValue<IInEdgeGremlinQuery<TElement, TInVertex>, TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);
        IGremlinQuery<IDictionary<TNewKey, object>> IInEdgeGremlinQuery<TElement, TInVertex>.Group<TNewKey>(Func<IGroupBuilder<IInEdgeGremlinQuery<TElement, TInVertex>>, IGroupBuilderWithKey<IInEdgeGremlinQuery<TElement, TInVertex>, TNewKey>> groupBuilder) => Group(groupBuilder);

        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Identity() => Identity();

        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Limit(long count) => Limit(count);
        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IInEdgeGremlinQuery<TElement, TInVertex>.Local<TTargetQuery>(Func<IInEdgeGremlinQuery<TElement, TInVertex> , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IInEdgeGremlinQuery<TElement, TInVertex>.Map<TTargetQuery>(Func<IInEdgeGremlinQuery<TElement, TInVertex>, TTargetQuery> mapping) => Map(mapping);
        
        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Not(Func<IInEdgeGremlinQuery<TElement, TInVertex>, IGremlinQuery> notTraversal) => Not(notTraversal);
        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.None() => None();

        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Optional(Func<IInEdgeGremlinQuery<TElement, TInVertex>, IInEdgeGremlinQuery<TElement, TInVertex>> optionalTraversal) => Optional(optionalTraversal);
        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Or(params Func<IInEdgeGremlinQuery<TElement, TInVertex>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Range(long low, long high) => Range(low, high);

        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Repeat(Func<IInEdgeGremlinQuery<TElement, TInVertex>, IInEdgeGremlinQuery<TElement, TInVertex>> repeatTraversal) => Repeat(repeatTraversal);
        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.RepeatUntil(Func<IInEdgeGremlinQuery<TElement, TInVertex>, IInEdgeGremlinQuery<TElement, TInVertex>> repeatTraversal, Func<IInEdgeGremlinQuery<TElement, TInVertex>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);
        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.UntilRepeat(Func<IInEdgeGremlinQuery<TElement, TInVertex>, IInEdgeGremlinQuery<TElement, TInVertex>> repeatTraversal, Func<IInEdgeGremlinQuery<TElement, TInVertex>, IGremlinQuery> untilTraversal) => UntilRepeat(repeatTraversal, untilTraversal);

        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.SideEffect(Func<IInEdgeGremlinQuery<TElement, TInVertex>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);
        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Skip(long count) => Skip(count);

        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Tail(long count) => Tail(count);
        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.TailLocal(int count) => TailLocal(count);

        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Times(int count) => Times(count);

        TTargetQuery IInEdgeGremlinQuery<TElement, TInVertex>.Union<TTargetQuery>(params Func<IInEdgeGremlinQuery<TElement, TInVertex>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);
        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.And(params Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery>[] andTraversals) => And(andTraversals);
        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.As(params StepLabel[] stepLabels) => As(stepLabels);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Barrier() => Barrier();

        TTargetQuery IOutEdgeGremlinQuery<TElement, TOutVertex>.Choose<TTargetQuery>(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> traversalPredicate, Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery> trueChoice, Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);
        TTargetQuery IOutEdgeGremlinQuery<TElement, TOutVertex>.Choose<TTargetQuery>(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> traversalPredicate, Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);
        TTargetQuery IOutEdgeGremlinQuery<TElement, TOutVertex>.Choose<TTargetQuery>(Func<IChooseBuilder<IOutEdgeGremlinQuery<TElement, TOutVertex>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);

        TTargetQuery IOutEdgeGremlinQuery<TElement, TOutVertex>.Coalesce<TTargetQuery>(params Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery>[] traversals) => Coalesce(traversals);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Dedup() => Dedup();
        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Drop() => Drop();

        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Emit() => Emit();

        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Where(ILambda lambda) => Where(lambda);

        TTargetQuery IOutEdgeGremlinQuery<TElement, TOutVertex>.FlatMap<TTargetQuery>(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery> mapping) => FlatMap(mapping);

        IGremlinQuery<IDictionary<TNewKey, TNewValue>> IOutEdgeGremlinQuery<TElement, TOutVertex>.Group<TNewKey, TNewValue>(Func<IGroupBuilder<IOutEdgeGremlinQuery<TElement, TOutVertex>>, IGroupBuilderWithKeyAndValue<IOutEdgeGremlinQuery<TElement, TOutVertex>, TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);
        IGremlinQuery<IDictionary<TNewKey, object>> IOutEdgeGremlinQuery<TElement, TOutVertex>.Group<TNewKey>(Func<IGroupBuilder<IOutEdgeGremlinQuery<TElement, TOutVertex>>, IGroupBuilderWithKey<IOutEdgeGremlinQuery<TElement, TOutVertex>, TNewKey>> groupBuilder) => Group(groupBuilder);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Identity() => Identity();

        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Limit(long count) => Limit(count);
        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IOutEdgeGremlinQuery<TElement, TOutVertex>.Local<TTargetQuery>(Func<IOutEdgeGremlinQuery<TElement, TOutVertex> , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IOutEdgeGremlinQuery<TElement, TOutVertex>.Map<TTargetQuery>(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery> mapping) => Map(mapping);
        
        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Not(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> notTraversal) => Not(notTraversal);
        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.None() => None();

        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Optional(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, IOutEdgeGremlinQuery<TElement, TOutVertex>> optionalTraversal) => Optional(optionalTraversal);
        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Or(params Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Range(long low, long high) => Range(low, high);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Repeat(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, IOutEdgeGremlinQuery<TElement, TOutVertex>> repeatTraversal) => Repeat(repeatTraversal);
        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.RepeatUntil(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, IOutEdgeGremlinQuery<TElement, TOutVertex>> repeatTraversal, Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);
        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.UntilRepeat(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, IOutEdgeGremlinQuery<TElement, TOutVertex>> repeatTraversal, Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> untilTraversal) => UntilRepeat(repeatTraversal, untilTraversal);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.SideEffect(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);
        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Skip(long count) => Skip(count);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Tail(long count) => Tail(count);
        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.TailLocal(int count) => TailLocal(count);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Times(int count) => Times(count);

        TTargetQuery IOutEdgeGremlinQuery<TElement, TOutVertex>.Union<TTargetQuery>(params Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);
        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.And(params Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, IGremlinQuery>[] andTraversals) => And(andTraversals);
        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.As(params StepLabel[] stepLabels) => As(stepLabels);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Barrier() => Barrier();

        TTargetQuery IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Choose<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, IGremlinQuery> traversalPredicate, Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, TTargetQuery> trueChoice, Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);
        TTargetQuery IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Choose<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, IGremlinQuery> traversalPredicate, Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);
        TTargetQuery IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Choose<TTargetQuery>(Func<IChooseBuilder<IVertexPropertyGremlinQuery<TElement, TPropertyValue>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);

        TTargetQuery IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Coalesce<TTargetQuery>(params Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, TTargetQuery>[] traversals) => Coalesce(traversals);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Dedup() => Dedup();
        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Drop() => Drop();

        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Emit() => Emit();

        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Where(ILambda lambda) => Where(lambda);

        TTargetQuery IVertexPropertyGremlinQuery<TElement, TPropertyValue>.FlatMap<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, TTargetQuery> mapping) => FlatMap(mapping);

        IGremlinQuery<IDictionary<TNewKey, TNewValue>> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Group<TNewKey, TNewValue>(Func<IGroupBuilder<IVertexPropertyGremlinQuery<TElement, TPropertyValue>>, IGroupBuilderWithKeyAndValue<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);
        IGremlinQuery<IDictionary<TNewKey, object>> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Group<TNewKey>(Func<IGroupBuilder<IVertexPropertyGremlinQuery<TElement, TPropertyValue>>, IGroupBuilderWithKey<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, TNewKey>> groupBuilder) => Group(groupBuilder);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Identity() => Identity();

        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Limit(long count) => Limit(count);
        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Local<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue> , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Map<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, TTargetQuery> mapping) => Map(mapping);
        
        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Not(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, IGremlinQuery> notTraversal) => Not(notTraversal);
        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.None() => None();

        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Optional(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, IVertexPropertyGremlinQuery<TElement, TPropertyValue>> optionalTraversal) => Optional(optionalTraversal);
        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Or(params Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Range(long low, long high) => Range(low, high);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Repeat(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, IVertexPropertyGremlinQuery<TElement, TPropertyValue>> repeatTraversal) => Repeat(repeatTraversal);
        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.RepeatUntil(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, IVertexPropertyGremlinQuery<TElement, TPropertyValue>> repeatTraversal, Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);
        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.UntilRepeat(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, IVertexPropertyGremlinQuery<TElement, TPropertyValue>> repeatTraversal, Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, IGremlinQuery> untilTraversal) => UntilRepeat(repeatTraversal, untilTraversal);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.SideEffect(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);
        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Skip(long count) => Skip(count);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Tail(long count) => Tail(count);
        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.TailLocal(int count) => TailLocal(count);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Times(int count) => Times(count);

        TTargetQuery IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Union<TTargetQuery>(params Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);
        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.And(params Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, IGremlinQuery>[] andTraversals) => And(andTraversals);
        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.As(params StepLabel[] stepLabels) => As(stepLabels);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Barrier() => Barrier();

        TTargetQuery IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Choose<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, IGremlinQuery> traversalPredicate, Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, TTargetQuery> trueChoice, Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);
        TTargetQuery IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Choose<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, IGremlinQuery> traversalPredicate, Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);
        TTargetQuery IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Choose<TTargetQuery>(Func<IChooseBuilder<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);

        TTargetQuery IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Coalesce<TTargetQuery>(params Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, TTargetQuery>[] traversals) => Coalesce(traversals);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Dedup() => Dedup();
        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Drop() => Drop();

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Emit() => Emit();

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Where(ILambda lambda) => Where(lambda);

        TTargetQuery IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.FlatMap<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, TTargetQuery> mapping) => FlatMap(mapping);

        IGremlinQuery<IDictionary<TNewKey, TNewValue>> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Group<TNewKey, TNewValue>(Func<IGroupBuilder<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>, IGroupBuilderWithKeyAndValue<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);
        IGremlinQuery<IDictionary<TNewKey, object>> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Group<TNewKey>(Func<IGroupBuilder<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>, IGroupBuilderWithKey<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, TNewKey>> groupBuilder) => Group(groupBuilder);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Identity() => Identity();

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Limit(long count) => Limit(count);
        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Local<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Map<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, TTargetQuery> mapping) => Map(mapping);
        
        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Not(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, IGremlinQuery> notTraversal) => Not(notTraversal);
        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.None() => None();

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Optional(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>> optionalTraversal) => Optional(optionalTraversal);
        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Or(params Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Range(long low, long high) => Range(low, high);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Repeat(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>> repeatTraversal) => Repeat(repeatTraversal);
        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.RepeatUntil(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>> repeatTraversal, Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);
        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.UntilRepeat(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>> repeatTraversal, Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, IGremlinQuery> untilTraversal) => UntilRepeat(repeatTraversal, untilTraversal);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.SideEffect(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);
        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Skip(long count) => Skip(count);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Tail(long count) => Tail(count);
        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.TailLocal(int count) => TailLocal(count);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Times(int count) => Times(count);

        TTargetQuery IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Union<TTargetQuery>(params Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);
        IPropertyGremlinQuery<TElement> IPropertyGremlinQuery<TElement>.And(params Func<IPropertyGremlinQuery<TElement>, IGremlinQuery>[] andTraversals) => And(andTraversals);
        IPropertyGremlinQuery<TElement> IPropertyGremlinQuery<TElement>.As(params StepLabel[] stepLabels) => As(stepLabels);

        IPropertyGremlinQuery<TElement> IPropertyGremlinQuery<TElement>.Barrier() => Barrier();

        TTargetQuery IPropertyGremlinQuery<TElement>.Choose<TTargetQuery>(Func<IPropertyGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IPropertyGremlinQuery<TElement>, TTargetQuery> trueChoice, Func<IPropertyGremlinQuery<TElement>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);
        TTargetQuery IPropertyGremlinQuery<TElement>.Choose<TTargetQuery>(Func<IPropertyGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IPropertyGremlinQuery<TElement>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);
        TTargetQuery IPropertyGremlinQuery<TElement>.Choose<TTargetQuery>(Func<IChooseBuilder<IPropertyGremlinQuery<TElement>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) => Choose<TTargetQuery>(continuation);

        TTargetQuery IPropertyGremlinQuery<TElement>.Coalesce<TTargetQuery>(params Func<IPropertyGremlinQuery<TElement>, TTargetQuery>[] traversals) => Coalesce(traversals);

        IPropertyGremlinQuery<TElement> IPropertyGremlinQuery<TElement>.Dedup() => Dedup();
        IPropertyGremlinQuery<TElement> IPropertyGremlinQuery<TElement>.Drop() => Drop();

        IPropertyGremlinQuery<TElement> IPropertyGremlinQuery<TElement>.Emit() => Emit();

        IPropertyGremlinQuery<TElement> IPropertyGremlinQuery<TElement>.Where(ILambda lambda) => Where(lambda);

        TTargetQuery IPropertyGremlinQuery<TElement>.FlatMap<TTargetQuery>(Func<IPropertyGremlinQuery<TElement>, TTargetQuery> mapping) => FlatMap(mapping);

        IGremlinQuery<IDictionary<TNewKey, TNewValue>> IPropertyGremlinQuery<TElement>.Group<TNewKey, TNewValue>(Func<IGroupBuilder<IPropertyGremlinQuery<TElement>>, IGroupBuilderWithKeyAndValue<IPropertyGremlinQuery<TElement>, TNewKey, TNewValue>> groupBuilder) => Group(groupBuilder);
        IGremlinQuery<IDictionary<TNewKey, object>> IPropertyGremlinQuery<TElement>.Group<TNewKey>(Func<IGroupBuilder<IPropertyGremlinQuery<TElement>>, IGroupBuilderWithKey<IPropertyGremlinQuery<TElement>, TNewKey>> groupBuilder) => Group(groupBuilder);

        IPropertyGremlinQuery<TElement> IPropertyGremlinQuery<TElement>.Identity() => Identity();

        IPropertyGremlinQuery<TElement> IPropertyGremlinQuery<TElement>.Limit(long count) => Limit(count);
        IPropertyGremlinQuery<TElement> IPropertyGremlinQuery<TElement>.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IPropertyGremlinQuery<TElement>.Local<TTargetQuery>(Func<IPropertyGremlinQuery<TElement> , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IPropertyGremlinQuery<TElement>.Map<TTargetQuery>(Func<IPropertyGremlinQuery<TElement>, TTargetQuery> mapping) => Map(mapping);
        
        IPropertyGremlinQuery<TElement> IPropertyGremlinQuery<TElement>.Not(Func<IPropertyGremlinQuery<TElement>, IGremlinQuery> notTraversal) => Not(notTraversal);
        IPropertyGremlinQuery<TElement> IPropertyGremlinQuery<TElement>.None() => None();

        IPropertyGremlinQuery<TElement> IPropertyGremlinQuery<TElement>.Optional(Func<IPropertyGremlinQuery<TElement>, IPropertyGremlinQuery<TElement>> optionalTraversal) => Optional(optionalTraversal);
        IPropertyGremlinQuery<TElement> IPropertyGremlinQuery<TElement>.Or(params Func<IPropertyGremlinQuery<TElement>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IPropertyGremlinQuery<TElement> IPropertyGremlinQuery<TElement>.Range(long low, long high) => Range(low, high);

        IPropertyGremlinQuery<TElement> IPropertyGremlinQuery<TElement>.Repeat(Func<IPropertyGremlinQuery<TElement>, IPropertyGremlinQuery<TElement>> repeatTraversal) => Repeat(repeatTraversal);
        IPropertyGremlinQuery<TElement> IPropertyGremlinQuery<TElement>.RepeatUntil(Func<IPropertyGremlinQuery<TElement>, IPropertyGremlinQuery<TElement>> repeatTraversal, Func<IPropertyGremlinQuery<TElement>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);
        IPropertyGremlinQuery<TElement> IPropertyGremlinQuery<TElement>.UntilRepeat(Func<IPropertyGremlinQuery<TElement>, IPropertyGremlinQuery<TElement>> repeatTraversal, Func<IPropertyGremlinQuery<TElement>, IGremlinQuery> untilTraversal) => UntilRepeat(repeatTraversal, untilTraversal);

        IPropertyGremlinQuery<TElement> IPropertyGremlinQuery<TElement>.SideEffect(Func<IPropertyGremlinQuery<TElement>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);
        IPropertyGremlinQuery<TElement> IPropertyGremlinQuery<TElement>.Skip(long count) => Skip(count);

        IPropertyGremlinQuery<TElement> IPropertyGremlinQuery<TElement>.Tail(long count) => Tail(count);
        IPropertyGremlinQuery<TElement> IPropertyGremlinQuery<TElement>.TailLocal(int count) => TailLocal(count);

        IPropertyGremlinQuery<TElement> IPropertyGremlinQuery<TElement>.Times(int count) => Times(count);

        TTargetQuery IPropertyGremlinQuery<TElement>.Union<TTargetQuery>(params Func<IPropertyGremlinQuery<TElement>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);


        IGremlinQuery<TResult> IGremlinQuery.Cast<TResult>() => Cast<TResult>();
        IGremlinQuery<dynamic> IGremlinQuery.Project(Func<IProjectBuilder<IGremlinQuery<object>, object>, IProjectResult> continuation) => Project<object, dynamic>(continuation);
        IGremlinQuery<TResult> IGremlinQuery.Project<TResult>(Func<IProjectBuilder<IGremlinQuery<object>, object>, IProjectResult<TResult>> continuation) => Project<object, TResult>(continuation);
        IElementGremlinQuery<TResult> IElementGremlinQuery.Cast<TResult>() => Cast<TResult>();
        IGremlinQuery<dynamic> IElementGremlinQuery.Project(Func<IProjectBuilder<IElementGremlinQuery<object>, object>, IProjectResult> continuation) => Project<object, dynamic>(continuation);
        IGremlinQuery<TResult> IElementGremlinQuery.Project<TResult>(Func<IProjectBuilder<IElementGremlinQuery<object>, object>, IProjectResult<TResult>> continuation) => Project<object, TResult>(continuation);
        IVertexGremlinQuery<TResult> IVertexGremlinQuery.Cast<TResult>() => Cast<TResult>();
        IGremlinQuery<dynamic> IVertexGremlinQuery.Project(Func<IProjectBuilder<IVertexGremlinQuery<object>, object>, IProjectResult> continuation) => Project<object, dynamic>(continuation);
        IGremlinQuery<TResult> IVertexGremlinQuery.Project<TResult>(Func<IProjectBuilder<IVertexGremlinQuery<object>, object>, IProjectResult<TResult>> continuation) => Project<object, TResult>(continuation);
        IEdgeGremlinQuery<TResult> IEdgeGremlinQuery.Cast<TResult>() => Cast<TResult>();
        IGremlinQuery<dynamic> IEdgeGremlinQuery.Project(Func<IProjectBuilder<IEdgeGremlinQuery<object>, object>, IProjectResult> continuation) => Project<object, dynamic>(continuation);
        IGremlinQuery<TResult> IEdgeGremlinQuery.Project<TResult>(Func<IProjectBuilder<IEdgeGremlinQuery<object>, object>, IProjectResult<TResult>> continuation) => Project<object, TResult>(continuation);


        IGremlinQuery<dynamic> IGremlinQuery<TElement>.Project(Func<IProjectBuilder<IGremlinQuery<TElement>, TElement>, IProjectResult> continuation) => Project<TElement, dynamic>(continuation);
        IGremlinQuery<TResult> IGremlinQuery<TElement>.Project<TResult>(Func<IProjectBuilder<IGremlinQuery<TElement>, TElement>, IProjectResult<TResult>> continuation) => Project<TElement, TResult>(continuation);
        IGremlinQuery<dynamic> IValueGremlinQuery<TElement>.Project(Func<IProjectBuilder<IValueGremlinQuery<TElement>, TElement>, IProjectResult> continuation) => Project<TElement, dynamic>(continuation);
        IGremlinQuery<TResult> IValueGremlinQuery<TElement>.Project<TResult>(Func<IProjectBuilder<IValueGremlinQuery<TElement>, TElement>, IProjectResult<TResult>> continuation) => Project<TElement, TResult>(continuation);
        IGremlinQuery<dynamic> IArrayGremlinQuery<TElement, TFoldedQuery>.Project(Func<IProjectBuilder<IArrayGremlinQuery<TElement, TFoldedQuery>, TElement>, IProjectResult> continuation) => Project<TElement, dynamic>(continuation);
        IGremlinQuery<TResult> IArrayGremlinQuery<TElement, TFoldedQuery>.Project<TResult>(Func<IProjectBuilder<IArrayGremlinQuery<TElement, TFoldedQuery>, TElement>, IProjectResult<TResult>> continuation) => Project<TElement, TResult>(continuation);
        IGremlinQuery<dynamic> IElementGremlinQuery<TElement>.Project(Func<IProjectBuilder<IElementGremlinQuery<TElement>, TElement>, IProjectResult> continuation) => Project<TElement, dynamic>(continuation);
        IGremlinQuery<TResult> IElementGremlinQuery<TElement>.Project<TResult>(Func<IProjectBuilder<IElementGremlinQuery<TElement>, TElement>, IProjectResult<TResult>> continuation) => Project<TElement, TResult>(continuation);
        IGremlinQuery<dynamic> IVertexGremlinQuery<TElement>.Project(Func<IProjectBuilder<IVertexGremlinQuery<TElement>, TElement>, IProjectResult> continuation) => Project<TElement, dynamic>(continuation);
        IGremlinQuery<TResult> IVertexGremlinQuery<TElement>.Project<TResult>(Func<IProjectBuilder<IVertexGremlinQuery<TElement>, TElement>, IProjectResult<TResult>> continuation) => Project<TElement, TResult>(continuation);
        IGremlinQuery<dynamic> IEdgeGremlinQuery<TElement>.Project(Func<IProjectBuilder<IEdgeGremlinQuery<TElement>, TElement>, IProjectResult> continuation) => Project<TElement, dynamic>(continuation);
        IGremlinQuery<TResult> IEdgeGremlinQuery<TElement>.Project<TResult>(Func<IProjectBuilder<IEdgeGremlinQuery<TElement>, TElement>, IProjectResult<TResult>> continuation) => Project<TElement, TResult>(continuation);
        IGremlinQuery<dynamic> IEdgeGremlinQuery<TElement, TOutVertex>.Project(Func<IProjectBuilder<IEdgeGremlinQuery<TElement, TOutVertex>, TElement>, IProjectResult> continuation) => Project<TElement, dynamic>(continuation);
        IGremlinQuery<TResult> IEdgeGremlinQuery<TElement, TOutVertex>.Project<TResult>(Func<IProjectBuilder<IEdgeGremlinQuery<TElement, TOutVertex>, TElement>, IProjectResult<TResult>> continuation) => Project<TElement, TResult>(continuation);
        IGremlinQuery<dynamic> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Project(Func<IProjectBuilder<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TElement>, IProjectResult> continuation) => Project<TElement, dynamic>(continuation);
        IGremlinQuery<TResult> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Project<TResult>(Func<IProjectBuilder<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TElement>, IProjectResult<TResult>> continuation) => Project<TElement, TResult>(continuation);
        IGremlinQuery<dynamic> IInEdgeGremlinQuery<TElement, TInVertex>.Project(Func<IProjectBuilder<IInEdgeGremlinQuery<TElement, TInVertex>, TElement>, IProjectResult> continuation) => Project<TElement, dynamic>(continuation);
        IGremlinQuery<TResult> IInEdgeGremlinQuery<TElement, TInVertex>.Project<TResult>(Func<IProjectBuilder<IInEdgeGremlinQuery<TElement, TInVertex>, TElement>, IProjectResult<TResult>> continuation) => Project<TElement, TResult>(continuation);
        IGremlinQuery<dynamic> IOutEdgeGremlinQuery<TElement, TOutVertex>.Project(Func<IProjectBuilder<IOutEdgeGremlinQuery<TElement, TOutVertex>, TElement>, IProjectResult> continuation) => Project<TElement, dynamic>(continuation);
        IGremlinQuery<TResult> IOutEdgeGremlinQuery<TElement, TOutVertex>.Project<TResult>(Func<IProjectBuilder<IOutEdgeGremlinQuery<TElement, TOutVertex>, TElement>, IProjectResult<TResult>> continuation) => Project<TElement, TResult>(continuation);
        IGremlinQuery<dynamic> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Project(Func<IProjectBuilder<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, TElement>, IProjectResult> continuation) => Project<TElement, dynamic>(continuation);
        IGremlinQuery<TResult> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Project<TResult>(Func<IProjectBuilder<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, TElement>, IProjectResult<TResult>> continuation) => Project<TElement, TResult>(continuation);
        IGremlinQuery<dynamic> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Project(Func<IProjectBuilder<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, TElement>, IProjectResult> continuation) => Project<TElement, dynamic>(continuation);
        IGremlinQuery<TResult> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Project<TResult>(Func<IProjectBuilder<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, TElement>, IProjectResult<TResult>> continuation) => Project<TElement, TResult>(continuation);
        IGremlinQuery<dynamic> IPropertyGremlinQuery<TElement>.Project(Func<IProjectBuilder<IPropertyGremlinQuery<TElement>, TElement>, IProjectResult> continuation) => Project<TElement, dynamic>(continuation);
        IGremlinQuery<TResult> IPropertyGremlinQuery<TElement>.Project<TResult>(Func<IProjectBuilder<IPropertyGremlinQuery<TElement>, TElement>, IProjectResult<TResult>> continuation) => Project<TElement, TResult>(continuation);




        IValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.Order(Func<IOrderBuilder<TElement, IValueGremlinQuery<TElement>>, IOrderBuilderWithBy<TElement, IValueGremlinQuery<TElement>>> projection) => Order(projection);



        IArrayGremlinQuery<TElement, TFoldedQuery> IArrayGremlinQuery<TElement, TFoldedQuery>.Order(Func<IOrderBuilder<TElement, IArrayGremlinQuery<TElement, TFoldedQuery>>, IOrderBuilderWithBy<TElement, IArrayGremlinQuery<TElement, TFoldedQuery>>> projection) => Order(projection);



        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Order(Func<IOrderBuilder<TElement, IVertexGremlinQuery<TElement>>, IOrderBuilderWithBy<TElement, IVertexGremlinQuery<TElement>>> projection) => Order(projection);



        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Order(Func<IOrderBuilder<TElement, IEdgeGremlinQuery<TElement>>, IOrderBuilderWithBy<TElement, IEdgeGremlinQuery<TElement>>> projection) => Order(projection);



        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Order(Func<IOrderBuilder<TElement, IEdgeGremlinQuery<TElement, TOutVertex>>, IOrderBuilderWithBy<TElement, IEdgeGremlinQuery<TElement, TOutVertex>>> projection) => Order(projection);



        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Order(Func<IOrderBuilder<TElement, IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>, IOrderBuilderWithBy<TElement, IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>> projection) => Order(projection);



        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Order(Func<IOrderBuilder<TElement, IInEdgeGremlinQuery<TElement, TInVertex>>, IOrderBuilderWithBy<TElement, IInEdgeGremlinQuery<TElement, TInVertex>>> projection) => Order(projection);



        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Order(Func<IOrderBuilder<TElement, IOutEdgeGremlinQuery<TElement, TOutVertex>>, IOrderBuilderWithBy<TElement, IOutEdgeGremlinQuery<TElement, TOutVertex>>> projection) => Order(projection);



        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Order(Func<IOrderBuilder<TElement, IVertexPropertyGremlinQuery<TElement, TPropertyValue>>, IOrderBuilderWithBy<TElement, IVertexPropertyGremlinQuery<TElement, TPropertyValue>>> projection) => Order(projection);



        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Order(Func<IOrderBuilder<TElement, IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>, IOrderBuilderWithBy<TElement, IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>> projection) => Order(projection);



        IPropertyGremlinQuery<TElement> IPropertyGremlinQuery<TElement>.Order(Func<IOrderBuilder<TElement, IPropertyGremlinQuery<TElement>>, IOrderBuilderWithBy<TElement, IPropertyGremlinQuery<TElement>>> projection) => Order(projection);




        IVertexGremlinQuery IVertexGremlinQuery.Order(Func<IOrderBuilder<IVertexGremlinQuery>, IOrderBuilderWithBy<IVertexGremlinQuery>> projection) => Order(projection);


        IEdgeGremlinQuery IEdgeGremlinQuery.Order(Func<IOrderBuilder<IEdgeGremlinQuery>, IOrderBuilderWithBy<IEdgeGremlinQuery>> projection) => Order(projection);



        IValueGremlinQuery<IDictionary<string, TTarget>> IElementGremlinQuery<TElement>.ValueMap<TTarget>(params Expression<Func<TElement, TTarget>>[] keys) => ValueMap<IDictionary<string, TTarget>>(keys);

        IValueGremlinQuery<TTarget> IElementGremlinQuery<TElement>.Values<TTarget>() => ValuesForProjections<TTarget>(Enumerable.Empty<LambdaExpression>()); 
        IValueGremlinQuery<TTarget> IElementGremlinQuery<TElement>.Values<TTarget>(params Expression<Func<TElement, TTarget>>[] projections) => ValuesForProjections<TTarget>(projections);
        IValueGremlinQuery<TTarget> IElementGremlinQuery<TElement>.Values<TTarget>(params Expression<Func<TElement, TTarget[]>>[] projections) => ValuesForProjections<TTarget>(projections);
        IValueGremlinQuery<IDictionary<string, TTarget>> IVertexGremlinQuery<TElement>.ValueMap<TTarget>(params Expression<Func<TElement, TTarget>>[] keys) => ValueMap<IDictionary<string, TTarget>>(keys);

        IValueGremlinQuery<TTarget> IVertexGremlinQuery<TElement>.Values<TTarget>() => ValuesForProjections<TTarget>(Enumerable.Empty<LambdaExpression>()); 
        IValueGremlinQuery<TTarget> IVertexGremlinQuery<TElement>.Values<TTarget>(params Expression<Func<TElement, TTarget>>[] projections) => ValuesForProjections<TTarget>(projections);
        IValueGremlinQuery<TTarget> IVertexGremlinQuery<TElement>.Values<TTarget>(params Expression<Func<TElement, TTarget[]>>[] projections) => ValuesForProjections<TTarget>(projections);
        IValueGremlinQuery<IDictionary<string, TTarget>> IEdgeGremlinQuery<TElement>.ValueMap<TTarget>(params Expression<Func<TElement, TTarget>>[] keys) => ValueMap<IDictionary<string, TTarget>>(keys);

        IValueGremlinQuery<TTarget> IEdgeGremlinQuery<TElement>.Values<TTarget>() => ValuesForProjections<TTarget>(Enumerable.Empty<LambdaExpression>()); 
        IValueGremlinQuery<TTarget> IEdgeGremlinQuery<TElement>.Values<TTarget>(params Expression<Func<TElement, TTarget>>[] projections) => ValuesForProjections<TTarget>(projections);
        IValueGremlinQuery<TTarget> IEdgeGremlinQuery<TElement>.Values<TTarget>(params Expression<Func<TElement, TTarget[]>>[] projections) => ValuesForProjections<TTarget>(projections);

        IValueGremlinQuery<TTarget> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Values<TTarget>() => ValuesForProjections<TTarget>(Enumerable.Empty<LambdaExpression>()); 
        IValueGremlinQuery<TTarget> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Values<TTarget>(params Expression<Func<TElement, TTarget>>[] projections) => ValuesForProjections<TTarget>(projections);
        IValueGremlinQuery<TTarget> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Values<TTarget>(params Expression<Func<TElement, TTarget[]>>[] projections) => ValuesForProjections<TTarget>(projections);


        IGremlinQuery<TTarget> IGremlinQuery.OfType<TTarget>() => OfType<TTarget>(Environment.Model.EdgesModel);
        IElementGremlinQuery<TTarget> IElementGremlinQuery.OfType<TTarget>() => OfType<TTarget>(Environment.Model.EdgesModel);
        IVertexGremlinQuery<TTarget> IVertexGremlinQuery.OfType<TTarget>() => OfType<TTarget>(Environment.Model.VerticesModel);
        IEdgeGremlinQuery<TTarget> IEdgeGremlinQuery.OfType<TTarget>() => OfType<TTarget>(Environment.Model.EdgesModel);
    


        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Update(TElement element) => AddOrUpdate(element, false, true);

        IVertexGremlinQuery<TTarget> IVertexGremlinQuery<TElement>.OfType<TTarget>() => OfType<TTarget>(Environment.Model.VerticesModel);

        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Property<TProjectedValue>(Expression<Func<TElement, TProjectedValue>> projection, [AllowNull] TProjectedValue value) => VertexProperty(projection, value);

        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Property<TProjectedValue>(Expression<Func<TElement, TProjectedValue[]>> projection, [AllowNull] TProjectedValue value) => VertexProperty(projection, value);


        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Property(string key, [AllowNull] object value) => Property(key, value);

        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Where(Expression<Func<TElement, bool>> predicate) => Where(predicate);
        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal) => Where(projection, propertyTraversal);


        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Update(TElement element) => AddOrUpdate(element, false, false);

        IEdgeGremlinQuery<TTarget> IEdgeGremlinQuery<TElement>.OfType<TTarget>() => OfType<TTarget>(Environment.Model.EdgesModel);

        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Property<TProjectedValue>(Expression<Func<TElement, TProjectedValue>> projection, [AllowNull] TProjectedValue value) => Property(projection, value);



        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Property(string key, [AllowNull] object value) => Property(key, value);

        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Where(Expression<Func<TElement, bool>> predicate) => Where(predicate);
        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal) => Where(projection, propertyTraversal);


        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Update(TElement element) => AddOrUpdate(element, false, false);

        IEdgeGremlinQuery<TTarget, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.OfType<TTarget>() => OfType<TTarget>(Environment.Model.EdgesModel);

        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Property<TProjectedValue>(Expression<Func<TElement, TProjectedValue>> projection, [AllowNull] TProjectedValue value) => Property(projection, value);



        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Property(string key, [AllowNull] object value) => Property(key, value);

        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Where(Expression<Func<TElement, bool>> predicate) => Where(predicate);
        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal) => Where(projection, propertyTraversal);


        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Update(TElement element) => AddOrUpdate(element, false, false);

        IEdgeGremlinQuery<TTarget, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.OfType<TTarget>() => OfType<TTarget>(Environment.Model.EdgesModel);

        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Property<TProjectedValue>(Expression<Func<TElement, TProjectedValue>> projection, [AllowNull] TProjectedValue value) => Property(projection, value);



        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Property(string key, [AllowNull] object value) => Property(key, value);

        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Where(Expression<Func<TElement, bool>> predicate) => Where(predicate);
        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal) => Where(projection, propertyTraversal);


        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Update(TElement element) => AddOrUpdate(element, false, false);

        IInEdgeGremlinQuery<TTarget, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.OfType<TTarget>() => OfType<TTarget>(Environment.Model.EdgesModel);

        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Property<TProjectedValue>(Expression<Func<TElement, TProjectedValue>> projection, [AllowNull] TProjectedValue value) => Property(projection, value);



        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Property(string key, [AllowNull] object value) => Property(key, value);

        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Where(Expression<Func<TElement, bool>> predicate) => Where(predicate);
        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal) => Where(projection, propertyTraversal);


        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Update(TElement element) => AddOrUpdate(element, false, false);

        IOutEdgeGremlinQuery<TTarget, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.OfType<TTarget>() => OfType<TTarget>(Environment.Model.EdgesModel);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Property<TProjectedValue>(Expression<Func<TElement, TProjectedValue>> projection, [AllowNull] TProjectedValue value) => Property(projection, value);



        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Property(string key, [AllowNull] object value) => Property(key, value);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Where(Expression<Func<TElement, bool>> predicate) => Where(predicate);
        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal) => Where(projection, propertyTraversal);


        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Property(string key, [AllowNull] object value) => Property(key, value);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Where(Expression<Func<TElement, bool>> predicate) => Where(predicate);
        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal) => Where(projection, propertyTraversal);


        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Property(string key, [AllowNull] object value) => Property(key, value);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Where(Expression<Func<TElement, bool>> predicate) => Where(predicate);
        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal) => Where(projection, propertyTraversal);



        IGremlinQuery<TResult> IGremlinQuery<TElement>.Cast<TResult>() => Cast<TResult>();
        IGremlinQuery<TElement> IGremlinQuery<TElement>.Coin(double probability) => Coin(probability);

        TTargetQuery IGremlinQuery<TElement>.Aggregate<TTargetQuery>(Func<IGremlinQuery<TElement>, StepLabel<IGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => Aggregate(new StepLabel<IGremlinQuery<TElement>, TElement>(Semantics), continuation);
        TTargetQuery IGremlinQuery<TElement>.As<TTargetQuery>(Func<IGremlinQuery<TElement>, StepLabel<IGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => As(new StepLabel<IGremlinQuery<TElement>, TElement>(Semantics), continuation);

        IArrayGremlinQuery<TElement[], IGremlinQuery<TElement>> IGremlinQuery<TElement>.Fold() => Fold<IGremlinQuery<TElement>>();
        IValueGremlinQuery<TResult> IValueGremlinQuery<TElement>.Cast<TResult>() => Cast<TResult>();
        IValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.Coin(double probability) => Coin(probability);

        TTargetQuery IValueGremlinQuery<TElement>.Aggregate<TTargetQuery>(Func<IValueGremlinQuery<TElement>, StepLabel<IValueGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => Aggregate(new StepLabel<IValueGremlinQuery<TElement>, TElement>(Semantics), continuation);
        TTargetQuery IValueGremlinQuery<TElement>.As<TTargetQuery>(Func<IValueGremlinQuery<TElement>, StepLabel<IValueGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => As(new StepLabel<IValueGremlinQuery<TElement>, TElement>(Semantics), continuation);

        IArrayGremlinQuery<TElement[], IValueGremlinQuery<TElement>> IValueGremlinQuery<TElement>.Fold() => Fold<IValueGremlinQuery<TElement>>();
        IArrayGremlinQuery<TResult, TFoldedQuery> IArrayGremlinQuery<TElement, TFoldedQuery>.Cast<TResult>() => Cast<TResult>();
        IArrayGremlinQuery<TElement, TFoldedQuery> IArrayGremlinQuery<TElement, TFoldedQuery>.Coin(double probability) => Coin(probability);

        TTargetQuery IArrayGremlinQuery<TElement, TFoldedQuery>.Aggregate<TTargetQuery>(Func<IArrayGremlinQuery<TElement, TFoldedQuery>, StepLabel<IArrayGremlinQuery<TElement, TFoldedQuery>, TElement>, TTargetQuery> continuation) => Aggregate(new StepLabel<IArrayGremlinQuery<TElement, TFoldedQuery>, TElement>(Semantics), continuation);
        TTargetQuery IArrayGremlinQuery<TElement, TFoldedQuery>.As<TTargetQuery>(Func<IArrayGremlinQuery<TElement, TFoldedQuery>, StepLabel<IArrayGremlinQuery<TElement, TFoldedQuery>, TElement>, TTargetQuery> continuation) => As(new StepLabel<IArrayGremlinQuery<TElement, TFoldedQuery>, TElement>(Semantics), continuation);

        IArrayGremlinQuery<TElement[], IArrayGremlinQuery<TElement, TFoldedQuery>> IArrayGremlinQuery<TElement, TFoldedQuery>.Fold() => Fold<IArrayGremlinQuery<TElement, TFoldedQuery>>();
        IElementGremlinQuery<TResult> IElementGremlinQuery<TElement>.Cast<TResult>() => Cast<TResult>();
        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Coin(double probability) => Coin(probability);

        TTargetQuery IElementGremlinQuery<TElement>.Aggregate<TTargetQuery>(Func<IElementGremlinQuery<TElement>, StepLabel<IElementGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => Aggregate(new StepLabel<IElementGremlinQuery<TElement>, TElement>(Semantics), continuation);
        TTargetQuery IElementGremlinQuery<TElement>.As<TTargetQuery>(Func<IElementGremlinQuery<TElement>, StepLabel<IElementGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => As(new StepLabel<IElementGremlinQuery<TElement>, TElement>(Semantics), continuation);

        IArrayGremlinQuery<TElement[], IElementGremlinQuery<TElement>> IElementGremlinQuery<TElement>.Fold() => Fold<IElementGremlinQuery<TElement>>();
        IVertexGremlinQuery<TResult> IVertexGremlinQuery<TElement>.Cast<TResult>() => Cast<TResult>();
        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Coin(double probability) => Coin(probability);

        TTargetQuery IVertexGremlinQuery<TElement>.Aggregate<TTargetQuery>(Func<IVertexGremlinQuery<TElement>, StepLabel<IVertexGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => Aggregate(new StepLabel<IVertexGremlinQuery<TElement>, TElement>(Semantics), continuation);
        TTargetQuery IVertexGremlinQuery<TElement>.As<TTargetQuery>(Func<IVertexGremlinQuery<TElement>, StepLabel<IVertexGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => As(new StepLabel<IVertexGremlinQuery<TElement>, TElement>(Semantics), continuation);

        IArrayGremlinQuery<TElement[], IVertexGremlinQuery<TElement>> IVertexGremlinQuery<TElement>.Fold() => Fold<IVertexGremlinQuery<TElement>>();
        IEdgeGremlinQuery<TResult> IEdgeGremlinQuery<TElement>.Cast<TResult>() => Cast<TResult>();
        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Coin(double probability) => Coin(probability);

        TTargetQuery IEdgeGremlinQuery<TElement>.Aggregate<TTargetQuery>(Func<IEdgeGremlinQuery<TElement>, StepLabel<IEdgeGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => Aggregate(new StepLabel<IEdgeGremlinQuery<TElement>, TElement>(Semantics), continuation);
        TTargetQuery IEdgeGremlinQuery<TElement>.As<TTargetQuery>(Func<IEdgeGremlinQuery<TElement>, StepLabel<IEdgeGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => As(new StepLabel<IEdgeGremlinQuery<TElement>, TElement>(Semantics), continuation);

        IArrayGremlinQuery<TElement[], IEdgeGremlinQuery<TElement>> IEdgeGremlinQuery<TElement>.Fold() => Fold<IEdgeGremlinQuery<TElement>>();
        IEdgeGremlinQuery<TResult, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Cast<TResult>() => Cast<TResult>();
        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Coin(double probability) => Coin(probability);

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex>.Aggregate<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex>, StepLabel<IEdgeGremlinQuery<TElement, TOutVertex>, TElement>, TTargetQuery> continuation) => Aggregate(new StepLabel<IEdgeGremlinQuery<TElement, TOutVertex>, TElement>(Semantics), continuation);
        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex>.As<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex>, StepLabel<IEdgeGremlinQuery<TElement, TOutVertex>, TElement>, TTargetQuery> continuation) => As(new StepLabel<IEdgeGremlinQuery<TElement, TOutVertex>, TElement>(Semantics), continuation);

        IArrayGremlinQuery<TElement[], IEdgeGremlinQuery<TElement, TOutVertex>> IEdgeGremlinQuery<TElement, TOutVertex>.Fold() => Fold<IEdgeGremlinQuery<TElement, TOutVertex>>();
        IEdgeGremlinQuery<TResult, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Cast<TResult>() => Cast<TResult>();
        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Coin(double probability) => Coin(probability);

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Aggregate<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, StepLabel<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TElement>, TTargetQuery> continuation) => Aggregate(new StepLabel<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TElement>(Semantics), continuation);
        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.As<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, StepLabel<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TElement>, TTargetQuery> continuation) => As(new StepLabel<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TElement>(Semantics), continuation);

        IArrayGremlinQuery<TElement[], IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Fold() => Fold<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>();
        IInEdgeGremlinQuery<TResult, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Cast<TResult>() => Cast<TResult>();
        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Coin(double probability) => Coin(probability);

        TTargetQuery IInEdgeGremlinQuery<TElement, TInVertex>.Aggregate<TTargetQuery>(Func<IInEdgeGremlinQuery<TElement, TInVertex>, StepLabel<IInEdgeGremlinQuery<TElement, TInVertex>, TElement>, TTargetQuery> continuation) => Aggregate(new StepLabel<IInEdgeGremlinQuery<TElement, TInVertex>, TElement>(Semantics), continuation);
        TTargetQuery IInEdgeGremlinQuery<TElement, TInVertex>.As<TTargetQuery>(Func<IInEdgeGremlinQuery<TElement, TInVertex>, StepLabel<IInEdgeGremlinQuery<TElement, TInVertex>, TElement>, TTargetQuery> continuation) => As(new StepLabel<IInEdgeGremlinQuery<TElement, TInVertex>, TElement>(Semantics), continuation);

        IArrayGremlinQuery<TElement[], IInEdgeGremlinQuery<TElement, TInVertex>> IInEdgeGremlinQuery<TElement, TInVertex>.Fold() => Fold<IInEdgeGremlinQuery<TElement, TInVertex>>();
        IOutEdgeGremlinQuery<TResult, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Cast<TResult>() => Cast<TResult>();
        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Coin(double probability) => Coin(probability);

        TTargetQuery IOutEdgeGremlinQuery<TElement, TOutVertex>.Aggregate<TTargetQuery>(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, StepLabel<IOutEdgeGremlinQuery<TElement, TOutVertex>, TElement>, TTargetQuery> continuation) => Aggregate(new StepLabel<IOutEdgeGremlinQuery<TElement, TOutVertex>, TElement>(Semantics), continuation);
        TTargetQuery IOutEdgeGremlinQuery<TElement, TOutVertex>.As<TTargetQuery>(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, StepLabel<IOutEdgeGremlinQuery<TElement, TOutVertex>, TElement>, TTargetQuery> continuation) => As(new StepLabel<IOutEdgeGremlinQuery<TElement, TOutVertex>, TElement>(Semantics), continuation);

        IArrayGremlinQuery<TElement[], IOutEdgeGremlinQuery<TElement, TOutVertex>> IOutEdgeGremlinQuery<TElement, TOutVertex>.Fold() => Fold<IOutEdgeGremlinQuery<TElement, TOutVertex>>();
        IVertexPropertyGremlinQuery<TResult, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Cast<TResult>() => Cast<TResult>();
        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Coin(double probability) => Coin(probability);

        TTargetQuery IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Aggregate<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, StepLabel<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, TElement>, TTargetQuery> continuation) => Aggregate(new StepLabel<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, TElement>(Semantics), continuation);
        TTargetQuery IVertexPropertyGremlinQuery<TElement, TPropertyValue>.As<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, StepLabel<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, TElement>, TTargetQuery> continuation) => As(new StepLabel<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, TElement>(Semantics), continuation);

        IArrayGremlinQuery<TElement[], IVertexPropertyGremlinQuery<TElement, TPropertyValue>> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Fold() => Fold<IVertexPropertyGremlinQuery<TElement, TPropertyValue>>();
        IVertexPropertyGremlinQuery<TResult, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Cast<TResult>() => Cast<TResult>();
        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Coin(double probability) => Coin(probability);

        TTargetQuery IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Aggregate<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, StepLabel<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, TElement>, TTargetQuery> continuation) => Aggregate(new StepLabel<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, TElement>(Semantics), continuation);
        TTargetQuery IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.As<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, StepLabel<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, TElement>, TTargetQuery> continuation) => As(new StepLabel<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, TElement>(Semantics), continuation);

        IArrayGremlinQuery<TElement[], IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Fold() => Fold<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>();
        IPropertyGremlinQuery<TResult> IPropertyGremlinQuery<TElement>.Cast<TResult>() => Cast<TResult>();
        IPropertyGremlinQuery<TElement> IPropertyGremlinQuery<TElement>.Coin(double probability) => Coin(probability);

        TTargetQuery IPropertyGremlinQuery<TElement>.Aggregate<TTargetQuery>(Func<IPropertyGremlinQuery<TElement>, StepLabel<IPropertyGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => Aggregate(new StepLabel<IPropertyGremlinQuery<TElement>, TElement>(Semantics), continuation);
        TTargetQuery IPropertyGremlinQuery<TElement>.As<TTargetQuery>(Func<IPropertyGremlinQuery<TElement>, StepLabel<IPropertyGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => As(new StepLabel<IPropertyGremlinQuery<TElement>, TElement>(Semantics), continuation);

        IArrayGremlinQuery<TElement[], IPropertyGremlinQuery<TElement>> IPropertyGremlinQuery<TElement>.Fold() => Fold<IPropertyGremlinQuery<TElement>>();

        IGremlinQuery IGremlinQuery.Where(Func<IGremlinQuery, IGremlinQuery> filterTraversal) => Where(filterTraversal);
        IElementGremlinQuery IElementGremlinQuery.Where(Func<IElementGremlinQuery, IGremlinQuery> filterTraversal) => Where(filterTraversal);
        IVertexGremlinQuery IVertexGremlinQuery.Where(Func<IVertexGremlinQuery, IGremlinQuery> filterTraversal) => Where(filterTraversal);
        IEdgeGremlinQuery IEdgeGremlinQuery.Where(Func<IEdgeGremlinQuery, IGremlinQuery> filterTraversal) => Where(filterTraversal);
        IGremlinQuery<TElement> IGremlinQuery<TElement>.Where(Func<IGremlinQuery<TElement>, IGremlinQuery> filterTraversal) => Where(filterTraversal);
        IValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.Where(Func<IValueGremlinQuery<TElement>, IGremlinQuery> filterTraversal) => Where(filterTraversal);
        IArrayGremlinQuery<TElement, TFoldedQuery> IArrayGremlinQuery<TElement, TFoldedQuery>.Where(Func<IArrayGremlinQuery<TElement, TFoldedQuery>, IGremlinQuery> filterTraversal) => Where(filterTraversal);
        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Where(Func<IElementGremlinQuery<TElement>, IGremlinQuery> filterTraversal) => Where(filterTraversal);
        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Where(Func<IVertexGremlinQuery<TElement>, IGremlinQuery> filterTraversal) => Where(filterTraversal);
        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Where(Func<IEdgeGremlinQuery<TElement>, IGremlinQuery> filterTraversal) => Where(filterTraversal);
        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Where(Func<IEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> filterTraversal) => Where(filterTraversal);
        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Where(Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery> filterTraversal) => Where(filterTraversal);
        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Where(Func<IInEdgeGremlinQuery<TElement, TInVertex>, IGremlinQuery> filterTraversal) => Where(filterTraversal);
        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Where(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> filterTraversal) => Where(filterTraversal);
        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Where(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, IGremlinQuery> filterTraversal) => Where(filterTraversal);
        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Where(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, IGremlinQuery> filterTraversal) => Where(filterTraversal);
        IPropertyGremlinQuery<TElement> IPropertyGremlinQuery<TElement>.Where(Func<IPropertyGremlinQuery<TElement>, IGremlinQuery> filterTraversal) => Where(filterTraversal);
   }
}
#pragma warning restore CS0109 // Member does not hide an inherited member; new keyword is not required

