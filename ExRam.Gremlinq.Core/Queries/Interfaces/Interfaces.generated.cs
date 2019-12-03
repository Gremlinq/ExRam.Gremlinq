#pragma warning disable CS0109 // Member does not hide an inherited member; new keyword is not required
using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    partial interface IGremlinQuery
    {
        IGremlinQuery<(T1, T2)> Select<T1, T2>(StepLabel<T1> label1, StepLabel<T2> label2);
        IGremlinQuery<(T1, T2, T3)> Select<T1, T2, T3>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3);
        IGremlinQuery<(T1, T2, T3, T4)> Select<T1, T2, T3, T4>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4);
        IGremlinQuery<(T1, T2, T3, T4, T5)> Select<T1, T2, T3, T4, T5>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5);
        IGremlinQuery<(T1, T2, T3, T4, T5, T6)> Select<T1, T2, T3, T4, T5, T6>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6);
        IGremlinQuery<(T1, T2, T3, T4, T5, T6, T7)> Select<T1, T2, T3, T4, T5, T6, T7>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7);
        IGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8)> Select<T1, T2, T3, T4, T5, T6, T7, T8>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8);
        IGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9);
        IGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10);
        IGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11);
        IGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11, StepLabel<T12> label12);
        IGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11, StepLabel<T12> label12, StepLabel<T13> label13);
        IGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11, StepLabel<T12> label12, StepLabel<T13> label13, StepLabel<T14> label14);
        IGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11, StepLabel<T12> label12, StepLabel<T13> label13, StepLabel<T14> label14, StepLabel<T15> label15);
        IGremlinQuery<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16)> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4, StepLabel<T5> label5, StepLabel<T6> label6, StepLabel<T7> label7, StepLabel<T8> label8, StepLabel<T9> label9, StepLabel<T10> label10, StepLabel<T11> label11, StepLabel<T12> label12, StepLabel<T13> label13, StepLabel<T14> label14, StepLabel<T15> label15, StepLabel<T16> label16);
    }


    public partial interface IOrderedGremlinQuery : IGremlinQuery { }

    public partial interface IGremlinQuery
    {
        IGremlinQuery And(params Func<IGremlinQuery, IGremlinQuery>[] andTraversals);

        new IGremlinQuery As(params StepLabel[] stepLabels);

        new IGremlinQuery Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IGremlinQuery, IGremlinQuery> traversalPredicate, Func<IGremlinQuery, TTargetQuery> trueChoice, Func<IGremlinQuery, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IGremlinQuery, IGremlinQuery> traversalPredicate, Func<IGremlinQuery, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;

        new TTargetQuery Choose<TTargetQuery>(Func<IChooseBuilder<IGremlinQuery>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) where TTargetQuery : IGremlinQuery;

        TTargetQuery Coalesce<TTargetQuery>(params Func<IGremlinQuery, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IGremlinQuery Dedup();
        new IGremlinQuery Drop();

        new IGremlinQuery Emit();

        TTargetQuery FlatMap<TTargetQuery>(Func<IGremlinQuery, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;

        new IGremlinQuery Identity();

        new IGremlinQuery Limit(long count);
        new IGremlinQuery LimitLocal(long count);
        TTargetQuery Local<TTargetQuery>(Func<IGremlinQuery , TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IGremlinQuery, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;
        
        IGremlinQuery Not(Func<IGremlinQuery, IGremlinQuery> notTraversal);
        new IGremlinQuery None();

        TTargetQuery Optional<TTargetQuery>(Func<IGremlinQuery, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery;
        IGremlinQuery Or(params Func<IGremlinQuery, IGremlinQuery>[] orTraversals);

        new IGremlinQuery Range(long low, long high);

        IGremlinQuery Repeat(Func<IGremlinQuery, IGremlinQuery> repeatTraversal);
        IGremlinQuery RepeatUntil(Func<IGremlinQuery, IGremlinQuery> repeatTraversal, Func<IGremlinQuery, IGremlinQuery> untilTraversal);
        IGremlinQuery UntilRepeat(Func<IGremlinQuery, IGremlinQuery> repeatTraversal, Func<IGremlinQuery, IGremlinQuery> untilTraversal);

        IGremlinQuery SideEffect(Func<IGremlinQuery, IGremlinQuery> sideEffectTraversal);
        new IGremlinQuery Skip(long count);

        new IGremlinQuery Tail(long count);
        new IGremlinQuery TailLocal(int count);

        new IGremlinQuery Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IGremlinQuery, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;

        new IGremlinQuery Where(ILambda lambda);
    }

    public partial interface IOrderedElementGremlinQuery : IElementGremlinQuery { }

    public partial interface IElementGremlinQuery
    {
        IElementGremlinQuery And(params Func<IElementGremlinQuery, IGremlinQuery>[] andTraversals);

        new IElementGremlinQuery As(params StepLabel[] stepLabels);

        new IElementGremlinQuery Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IElementGremlinQuery, IGremlinQuery> traversalPredicate, Func<IElementGremlinQuery, TTargetQuery> trueChoice, Func<IElementGremlinQuery, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IElementGremlinQuery, IGremlinQuery> traversalPredicate, Func<IElementGremlinQuery, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;

        new TTargetQuery Choose<TTargetQuery>(Func<IChooseBuilder<IElementGremlinQuery>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) where TTargetQuery : IGremlinQuery;

        TTargetQuery Coalesce<TTargetQuery>(params Func<IElementGremlinQuery, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IElementGremlinQuery Dedup();
        new IElementGremlinQuery Drop();

        new IElementGremlinQuery Emit();

        TTargetQuery FlatMap<TTargetQuery>(Func<IElementGremlinQuery, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;

        new IElementGremlinQuery Identity();

        new IElementGremlinQuery Limit(long count);
        new IElementGremlinQuery LimitLocal(long count);
        TTargetQuery Local<TTargetQuery>(Func<IElementGremlinQuery , TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IElementGremlinQuery, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;
        
        IElementGremlinQuery Not(Func<IElementGremlinQuery, IGremlinQuery> notTraversal);
        new IElementGremlinQuery None();

        TTargetQuery Optional<TTargetQuery>(Func<IElementGremlinQuery, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery;
        IElementGremlinQuery Or(params Func<IElementGremlinQuery, IGremlinQuery>[] orTraversals);

        new IElementGremlinQuery Range(long low, long high);

        IElementGremlinQuery Repeat(Func<IElementGremlinQuery, IElementGremlinQuery> repeatTraversal);
        IElementGremlinQuery RepeatUntil(Func<IElementGremlinQuery, IElementGremlinQuery> repeatTraversal, Func<IElementGremlinQuery, IGremlinQuery> untilTraversal);
        IElementGremlinQuery UntilRepeat(Func<IElementGremlinQuery, IElementGremlinQuery> repeatTraversal, Func<IElementGremlinQuery, IGremlinQuery> untilTraversal);

        IElementGremlinQuery SideEffect(Func<IElementGremlinQuery, IGremlinQuery> sideEffectTraversal);
        new IElementGremlinQuery Skip(long count);

        new IElementGremlinQuery Tail(long count);
        new IElementGremlinQuery TailLocal(int count);

        new IElementGremlinQuery Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IElementGremlinQuery, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;

        new IElementGremlinQuery Where(ILambda lambda);
    }

    public partial interface IOrderedVertexGremlinQuery : IVertexGremlinQuery { }

    public partial interface IVertexGremlinQuery
    {
        IVertexGremlinQuery And(params Func<IVertexGremlinQuery, IGremlinQuery>[] andTraversals);

        new IVertexGremlinQuery As(params StepLabel[] stepLabels);

        new IVertexGremlinQuery Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IVertexGremlinQuery, IGremlinQuery> traversalPredicate, Func<IVertexGremlinQuery, TTargetQuery> trueChoice, Func<IVertexGremlinQuery, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IVertexGremlinQuery, IGremlinQuery> traversalPredicate, Func<IVertexGremlinQuery, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;

        new TTargetQuery Choose<TTargetQuery>(Func<IChooseBuilder<IVertexGremlinQuery>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) where TTargetQuery : IGremlinQuery;

        TTargetQuery Coalesce<TTargetQuery>(params Func<IVertexGremlinQuery, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IVertexGremlinQuery Dedup();
        new IVertexGremlinQuery Drop();

        new IVertexGremlinQuery Emit();

        TTargetQuery FlatMap<TTargetQuery>(Func<IVertexGremlinQuery, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;

        new IVertexGremlinQuery Identity();

        new IVertexGremlinQuery Limit(long count);
        new IVertexGremlinQuery LimitLocal(long count);
        TTargetQuery Local<TTargetQuery>(Func<IVertexGremlinQuery , TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IVertexGremlinQuery, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;
        
        IVertexGremlinQuery Not(Func<IVertexGremlinQuery, IGremlinQuery> notTraversal);
        new IVertexGremlinQuery None();

        TTargetQuery Optional<TTargetQuery>(Func<IVertexGremlinQuery, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery;
        IVertexGremlinQuery Or(params Func<IVertexGremlinQuery, IGremlinQuery>[] orTraversals);

        new IVertexGremlinQuery Range(long low, long high);

        IVertexGremlinQuery Repeat(Func<IVertexGremlinQuery, IVertexGremlinQuery> repeatTraversal);
        IVertexGremlinQuery RepeatUntil(Func<IVertexGremlinQuery, IVertexGremlinQuery> repeatTraversal, Func<IVertexGremlinQuery, IGremlinQuery> untilTraversal);
        IVertexGremlinQuery UntilRepeat(Func<IVertexGremlinQuery, IVertexGremlinQuery> repeatTraversal, Func<IVertexGremlinQuery, IGremlinQuery> untilTraversal);

        IVertexGremlinQuery SideEffect(Func<IVertexGremlinQuery, IGremlinQuery> sideEffectTraversal);
        new IVertexGremlinQuery Skip(long count);

        new IVertexGremlinQuery Tail(long count);
        new IVertexGremlinQuery TailLocal(int count);

        new IVertexGremlinQuery Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IVertexGremlinQuery, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;

        new IVertexGremlinQuery Where(ILambda lambda);
    }

    public partial interface IOrderedEdgeGremlinQuery : IEdgeGremlinQuery { }

    public partial interface IEdgeGremlinQuery
    {
        IEdgeGremlinQuery And(params Func<IEdgeGremlinQuery, IGremlinQuery>[] andTraversals);

        new IEdgeGremlinQuery As(params StepLabel[] stepLabels);

        new IEdgeGremlinQuery Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IEdgeGremlinQuery, IGremlinQuery> traversalPredicate, Func<IEdgeGremlinQuery, TTargetQuery> trueChoice, Func<IEdgeGremlinQuery, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IEdgeGremlinQuery, IGremlinQuery> traversalPredicate, Func<IEdgeGremlinQuery, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;

        new TTargetQuery Choose<TTargetQuery>(Func<IChooseBuilder<IEdgeGremlinQuery>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) where TTargetQuery : IGremlinQuery;

        TTargetQuery Coalesce<TTargetQuery>(params Func<IEdgeGremlinQuery, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IEdgeGremlinQuery Dedup();
        new IEdgeGremlinQuery Drop();

        new IEdgeGremlinQuery Emit();

        TTargetQuery FlatMap<TTargetQuery>(Func<IEdgeGremlinQuery, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;

        new IEdgeGremlinQuery Identity();

        new IEdgeGremlinQuery Limit(long count);
        new IEdgeGremlinQuery LimitLocal(long count);
        TTargetQuery Local<TTargetQuery>(Func<IEdgeGremlinQuery , TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IEdgeGremlinQuery, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;
        
        IEdgeGremlinQuery Not(Func<IEdgeGremlinQuery, IGremlinQuery> notTraversal);
        new IEdgeGremlinQuery None();

        TTargetQuery Optional<TTargetQuery>(Func<IEdgeGremlinQuery, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery;
        IEdgeGremlinQuery Or(params Func<IEdgeGremlinQuery, IGremlinQuery>[] orTraversals);

        new IEdgeGremlinQuery Range(long low, long high);

        IEdgeGremlinQuery Repeat(Func<IEdgeGremlinQuery, IEdgeGremlinQuery> repeatTraversal);
        IEdgeGremlinQuery RepeatUntil(Func<IEdgeGremlinQuery, IEdgeGremlinQuery> repeatTraversal, Func<IEdgeGremlinQuery, IGremlinQuery> untilTraversal);
        IEdgeGremlinQuery UntilRepeat(Func<IEdgeGremlinQuery, IEdgeGremlinQuery> repeatTraversal, Func<IEdgeGremlinQuery, IGremlinQuery> untilTraversal);

        IEdgeGremlinQuery SideEffect(Func<IEdgeGremlinQuery, IGremlinQuery> sideEffectTraversal);
        new IEdgeGremlinQuery Skip(long count);

        new IEdgeGremlinQuery Tail(long count);
        new IEdgeGremlinQuery TailLocal(int count);

        new IEdgeGremlinQuery Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IEdgeGremlinQuery, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;

        new IEdgeGremlinQuery Where(ILambda lambda);
    }

    public partial interface IOrderedGremlinQuery<TElement> : IGremlinQuery<TElement> { }

    public partial interface IGremlinQuery<TElement>
    {
        IGremlinQuery<TElement> And(params Func<IGremlinQuery<TElement>, IGremlinQuery>[] andTraversals);

        new IGremlinQuery<TElement> As(params StepLabel[] stepLabels);

        new IGremlinQuery<TElement> Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IGremlinQuery<TElement>, TTargetQuery> trueChoice, Func<IGremlinQuery<TElement>, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IGremlinQuery<TElement>, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;

        new TTargetQuery Choose<TTargetQuery>(Func<IChooseBuilder<IGremlinQuery<TElement>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) where TTargetQuery : IGremlinQuery;

        TTargetQuery Coalesce<TTargetQuery>(params Func<IGremlinQuery<TElement>, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IGremlinQuery<TElement> Dedup();
        new IGremlinQuery<TElement> Drop();

        new IGremlinQuery<TElement> Emit();

        TTargetQuery FlatMap<TTargetQuery>(Func<IGremlinQuery<TElement>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;

        new IGremlinQuery<TElement> Identity();

        new IGremlinQuery<TElement> Limit(long count);
        new IGremlinQuery<TElement> LimitLocal(long count);
        TTargetQuery Local<TTargetQuery>(Func<IGremlinQuery<TElement> , TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IGremlinQuery<TElement>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;
        
        IGremlinQuery<TElement> Not(Func<IGremlinQuery<TElement>, IGremlinQuery> notTraversal);
        new IGremlinQuery<TElement> None();

        TTargetQuery Optional<TTargetQuery>(Func<IGremlinQuery<TElement>, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery;
        IGremlinQuery<TElement> Or(params Func<IGremlinQuery<TElement>, IGremlinQuery>[] orTraversals);

        new IGremlinQuery<TElement> Range(long low, long high);

        IGremlinQuery<TElement> Repeat(Func<IGremlinQuery<TElement>, IGremlinQuery<TElement>> repeatTraversal);
        IGremlinQuery<TElement> RepeatUntil(Func<IGremlinQuery<TElement>, IGremlinQuery<TElement>> repeatTraversal, Func<IGremlinQuery<TElement>, IGremlinQuery> untilTraversal);
        IGremlinQuery<TElement> UntilRepeat(Func<IGremlinQuery<TElement>, IGremlinQuery<TElement>> repeatTraversal, Func<IGremlinQuery<TElement>, IGremlinQuery> untilTraversal);

        IGremlinQuery<TElement> SideEffect(Func<IGremlinQuery<TElement>, IGremlinQuery> sideEffectTraversal);
        new IGremlinQuery<TElement> Skip(long count);

        new IGremlinQuery<TElement> Tail(long count);
        new IGremlinQuery<TElement> TailLocal(int count);

        new IGremlinQuery<TElement> Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IGremlinQuery<TElement>, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;

        new IGremlinQuery<TElement> Where(ILambda lambda);
    }

    public partial interface IOrderedValueGremlinQuery<TElement> : IValueGremlinQuery<TElement> { }

    public partial interface IValueGremlinQuery<TElement>
    {
        IValueGremlinQuery<TElement> And(params Func<IValueGremlinQuery<TElement>, IGremlinQuery>[] andTraversals);

        new IValueGremlinQuery<TElement> As(params StepLabel[] stepLabels);

        new IValueGremlinQuery<TElement> Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IValueGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IValueGremlinQuery<TElement>, TTargetQuery> trueChoice, Func<IValueGremlinQuery<TElement>, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IValueGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IValueGremlinQuery<TElement>, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;

        new TTargetQuery Choose<TTargetQuery>(Func<IChooseBuilder<IValueGremlinQuery<TElement>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) where TTargetQuery : IGremlinQuery;

        TTargetQuery Coalesce<TTargetQuery>(params Func<IValueGremlinQuery<TElement>, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IValueGremlinQuery<TElement> Dedup();
        new IValueGremlinQuery<TElement> Drop();

        new IValueGremlinQuery<TElement> Emit();

        TTargetQuery FlatMap<TTargetQuery>(Func<IValueGremlinQuery<TElement>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;

        new IValueGremlinQuery<TElement> Identity();

        new IValueGremlinQuery<TElement> Limit(long count);
        new IValueGremlinQuery<TElement> LimitLocal(long count);
        TTargetQuery Local<TTargetQuery>(Func<IValueGremlinQuery<TElement> , TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IValueGremlinQuery<TElement>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;
        
        IValueGremlinQuery<TElement> Not(Func<IValueGremlinQuery<TElement>, IGremlinQuery> notTraversal);
        new IValueGremlinQuery<TElement> None();

        TTargetQuery Optional<TTargetQuery>(Func<IValueGremlinQuery<TElement>, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery;
        IValueGremlinQuery<TElement> Or(params Func<IValueGremlinQuery<TElement>, IGremlinQuery>[] orTraversals);

        new IValueGremlinQuery<TElement> Range(long low, long high);

        IValueGremlinQuery<TElement> Repeat(Func<IValueGremlinQuery<TElement>, IValueGremlinQuery<TElement>> repeatTraversal);
        IValueGremlinQuery<TElement> RepeatUntil(Func<IValueGremlinQuery<TElement>, IValueGremlinQuery<TElement>> repeatTraversal, Func<IValueGremlinQuery<TElement>, IGremlinQuery> untilTraversal);
        IValueGremlinQuery<TElement> UntilRepeat(Func<IValueGremlinQuery<TElement>, IValueGremlinQuery<TElement>> repeatTraversal, Func<IValueGremlinQuery<TElement>, IGremlinQuery> untilTraversal);

        IValueGremlinQuery<TElement> SideEffect(Func<IValueGremlinQuery<TElement>, IGremlinQuery> sideEffectTraversal);
        new IValueGremlinQuery<TElement> Skip(long count);

        new IValueGremlinQuery<TElement> Tail(long count);
        new IValueGremlinQuery<TElement> TailLocal(int count);

        new IValueGremlinQuery<TElement> Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IValueGremlinQuery<TElement>, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;

        new IValueGremlinQuery<TElement> Where(ILambda lambda);
    }

    public partial interface IOrderedArrayGremlinQuery<TArray, TQuery> : IArrayGremlinQuery<TArray, TQuery> { }

    public partial interface IArrayGremlinQuery<TArray, TQuery>
    {
        IArrayGremlinQuery<TArray, TQuery> And(params Func<IArrayGremlinQuery<TArray, TQuery>, IGremlinQuery>[] andTraversals);

        new IArrayGremlinQuery<TArray, TQuery> As(params StepLabel[] stepLabels);

        new IArrayGremlinQuery<TArray, TQuery> Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IArrayGremlinQuery<TArray, TQuery>, IGremlinQuery> traversalPredicate, Func<IArrayGremlinQuery<TArray, TQuery>, TTargetQuery> trueChoice, Func<IArrayGremlinQuery<TArray, TQuery>, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IArrayGremlinQuery<TArray, TQuery>, IGremlinQuery> traversalPredicate, Func<IArrayGremlinQuery<TArray, TQuery>, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;

        new TTargetQuery Choose<TTargetQuery>(Func<IChooseBuilder<IArrayGremlinQuery<TArray, TQuery>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) where TTargetQuery : IGremlinQuery;

        TTargetQuery Coalesce<TTargetQuery>(params Func<IArrayGremlinQuery<TArray, TQuery>, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IArrayGremlinQuery<TArray, TQuery> Dedup();
        new IArrayGremlinQuery<TArray, TQuery> Drop();

        new IArrayGremlinQuery<TArray, TQuery> Emit();

        TTargetQuery FlatMap<TTargetQuery>(Func<IArrayGremlinQuery<TArray, TQuery>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;

        new IArrayGremlinQuery<TArray, TQuery> Identity();

        new IArrayGremlinQuery<TArray, TQuery> Limit(long count);
        new IArrayGremlinQuery<TArray, TQuery> LimitLocal(long count);
        TTargetQuery Local<TTargetQuery>(Func<IArrayGremlinQuery<TArray, TQuery> , TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IArrayGremlinQuery<TArray, TQuery>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;
        
        IArrayGremlinQuery<TArray, TQuery> Not(Func<IArrayGremlinQuery<TArray, TQuery>, IGremlinQuery> notTraversal);
        new IArrayGremlinQuery<TArray, TQuery> None();

        TTargetQuery Optional<TTargetQuery>(Func<IArrayGremlinQuery<TArray, TQuery>, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery;
        IArrayGremlinQuery<TArray, TQuery> Or(params Func<IArrayGremlinQuery<TArray, TQuery>, IGremlinQuery>[] orTraversals);

        new IArrayGremlinQuery<TArray, TQuery> Range(long low, long high);

        IArrayGremlinQuery<TArray, TQuery> Repeat(Func<IArrayGremlinQuery<TArray, TQuery>, IArrayGremlinQuery<TArray, TQuery>> repeatTraversal);
        IArrayGremlinQuery<TArray, TQuery> RepeatUntil(Func<IArrayGremlinQuery<TArray, TQuery>, IArrayGremlinQuery<TArray, TQuery>> repeatTraversal, Func<IArrayGremlinQuery<TArray, TQuery>, IGremlinQuery> untilTraversal);
        IArrayGremlinQuery<TArray, TQuery> UntilRepeat(Func<IArrayGremlinQuery<TArray, TQuery>, IArrayGremlinQuery<TArray, TQuery>> repeatTraversal, Func<IArrayGremlinQuery<TArray, TQuery>, IGremlinQuery> untilTraversal);

        IArrayGremlinQuery<TArray, TQuery> SideEffect(Func<IArrayGremlinQuery<TArray, TQuery>, IGremlinQuery> sideEffectTraversal);
        new IArrayGremlinQuery<TArray, TQuery> Skip(long count);

        new IArrayGremlinQuery<TArray, TQuery> Tail(long count);
        new IArrayGremlinQuery<TArray, TQuery> TailLocal(int count);

        new IArrayGremlinQuery<TArray, TQuery> Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IArrayGremlinQuery<TArray, TQuery>, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;

        new IArrayGremlinQuery<TArray, TQuery> Where(ILambda lambda);
    }

    public partial interface IOrderedElementGremlinQuery<TElement> : IElementGremlinQuery<TElement> { }

    public partial interface IElementGremlinQuery<TElement>
    {
        IElementGremlinQuery<TElement> And(params Func<IElementGremlinQuery<TElement>, IGremlinQuery>[] andTraversals);

        new IElementGremlinQuery<TElement> As(params StepLabel[] stepLabels);

        new IElementGremlinQuery<TElement> Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IElementGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IElementGremlinQuery<TElement>, TTargetQuery> trueChoice, Func<IElementGremlinQuery<TElement>, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IElementGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IElementGremlinQuery<TElement>, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;

        new TTargetQuery Choose<TTargetQuery>(Func<IChooseBuilder<IElementGremlinQuery<TElement>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) where TTargetQuery : IGremlinQuery;

        TTargetQuery Coalesce<TTargetQuery>(params Func<IElementGremlinQuery<TElement>, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IElementGremlinQuery<TElement> Dedup();
        new IElementGremlinQuery<TElement> Drop();

        new IElementGremlinQuery<TElement> Emit();

        TTargetQuery FlatMap<TTargetQuery>(Func<IElementGremlinQuery<TElement>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;

        new IElementGremlinQuery<TElement> Identity();

        new IElementGremlinQuery<TElement> Limit(long count);
        new IElementGremlinQuery<TElement> LimitLocal(long count);
        TTargetQuery Local<TTargetQuery>(Func<IElementGremlinQuery<TElement> , TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IElementGremlinQuery<TElement>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;
        
        IElementGremlinQuery<TElement> Not(Func<IElementGremlinQuery<TElement>, IGremlinQuery> notTraversal);
        new IElementGremlinQuery<TElement> None();

        TTargetQuery Optional<TTargetQuery>(Func<IElementGremlinQuery<TElement>, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery;
        IElementGremlinQuery<TElement> Or(params Func<IElementGremlinQuery<TElement>, IGremlinQuery>[] orTraversals);

        new IElementGremlinQuery<TElement> Range(long low, long high);

        IElementGremlinQuery<TElement> Repeat(Func<IElementGremlinQuery<TElement>, IElementGremlinQuery<TElement>> repeatTraversal);
        IElementGremlinQuery<TElement> RepeatUntil(Func<IElementGremlinQuery<TElement>, IElementGremlinQuery<TElement>> repeatTraversal, Func<IElementGremlinQuery<TElement>, IGremlinQuery> untilTraversal);
        IElementGremlinQuery<TElement> UntilRepeat(Func<IElementGremlinQuery<TElement>, IElementGremlinQuery<TElement>> repeatTraversal, Func<IElementGremlinQuery<TElement>, IGremlinQuery> untilTraversal);

        IElementGremlinQuery<TElement> SideEffect(Func<IElementGremlinQuery<TElement>, IGremlinQuery> sideEffectTraversal);
        new IElementGremlinQuery<TElement> Skip(long count);

        new IElementGremlinQuery<TElement> Tail(long count);
        new IElementGremlinQuery<TElement> TailLocal(int count);

        new IElementGremlinQuery<TElement> Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IElementGremlinQuery<TElement>, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;

        new IElementGremlinQuery<TElement> Where(ILambda lambda);
    }

    public partial interface IOrderedVertexGremlinQuery<TVertex> : IVertexGremlinQuery<TVertex> { }

    public partial interface IVertexGremlinQuery<TVertex>
    {
        IVertexGremlinQuery<TVertex> And(params Func<IVertexGremlinQuery<TVertex>, IGremlinQuery>[] andTraversals);

        new IVertexGremlinQuery<TVertex> As(params StepLabel[] stepLabels);

        new IVertexGremlinQuery<TVertex> Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IVertexGremlinQuery<TVertex>, IGremlinQuery> traversalPredicate, Func<IVertexGremlinQuery<TVertex>, TTargetQuery> trueChoice, Func<IVertexGremlinQuery<TVertex>, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IVertexGremlinQuery<TVertex>, IGremlinQuery> traversalPredicate, Func<IVertexGremlinQuery<TVertex>, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;

        new TTargetQuery Choose<TTargetQuery>(Func<IChooseBuilder<IVertexGremlinQuery<TVertex>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) where TTargetQuery : IGremlinQuery;

        TTargetQuery Coalesce<TTargetQuery>(params Func<IVertexGremlinQuery<TVertex>, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IVertexGremlinQuery<TVertex> Dedup();
        new IVertexGremlinQuery<TVertex> Drop();

        new IVertexGremlinQuery<TVertex> Emit();

        TTargetQuery FlatMap<TTargetQuery>(Func<IVertexGremlinQuery<TVertex>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;

        new IVertexGremlinQuery<TVertex> Identity();

        new IVertexGremlinQuery<TVertex> Limit(long count);
        new IVertexGremlinQuery<TVertex> LimitLocal(long count);
        TTargetQuery Local<TTargetQuery>(Func<IVertexGremlinQuery<TVertex> , TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IVertexGremlinQuery<TVertex>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;
        
        IVertexGremlinQuery<TVertex> Not(Func<IVertexGremlinQuery<TVertex>, IGremlinQuery> notTraversal);
        new IVertexGremlinQuery<TVertex> None();

        TTargetQuery Optional<TTargetQuery>(Func<IVertexGremlinQuery<TVertex>, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery;
        IVertexGremlinQuery<TVertex> Or(params Func<IVertexGremlinQuery<TVertex>, IGremlinQuery>[] orTraversals);

        new IVertexGremlinQuery<TVertex> Range(long low, long high);

        IVertexGremlinQuery<TVertex> Repeat(Func<IVertexGremlinQuery<TVertex>, IVertexGremlinQuery<TVertex>> repeatTraversal);
        IVertexGremlinQuery<TVertex> RepeatUntil(Func<IVertexGremlinQuery<TVertex>, IVertexGremlinQuery<TVertex>> repeatTraversal, Func<IVertexGremlinQuery<TVertex>, IGremlinQuery> untilTraversal);
        IVertexGremlinQuery<TVertex> UntilRepeat(Func<IVertexGremlinQuery<TVertex>, IVertexGremlinQuery<TVertex>> repeatTraversal, Func<IVertexGremlinQuery<TVertex>, IGremlinQuery> untilTraversal);

        IVertexGremlinQuery<TVertex> SideEffect(Func<IVertexGremlinQuery<TVertex>, IGremlinQuery> sideEffectTraversal);
        new IVertexGremlinQuery<TVertex> Skip(long count);

        new IVertexGremlinQuery<TVertex> Tail(long count);
        new IVertexGremlinQuery<TVertex> TailLocal(int count);

        new IVertexGremlinQuery<TVertex> Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IVertexGremlinQuery<TVertex>, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;

        new IVertexGremlinQuery<TVertex> Where(ILambda lambda);
    }

    public partial interface IOrderedEdgeGremlinQuery<TEdge> : IEdgeGremlinQuery<TEdge> { }

    public partial interface IEdgeGremlinQuery<TEdge>
    {
        IEdgeGremlinQuery<TEdge> And(params Func<IEdgeGremlinQuery<TEdge>, IGremlinQuery>[] andTraversals);

        new IEdgeGremlinQuery<TEdge> As(params StepLabel[] stepLabels);

        new IEdgeGremlinQuery<TEdge> Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge>, IGremlinQuery> traversalPredicate, Func<IEdgeGremlinQuery<TEdge>, TTargetQuery> trueChoice, Func<IEdgeGremlinQuery<TEdge>, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge>, IGremlinQuery> traversalPredicate, Func<IEdgeGremlinQuery<TEdge>, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;

        new TTargetQuery Choose<TTargetQuery>(Func<IChooseBuilder<IEdgeGremlinQuery<TEdge>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) where TTargetQuery : IGremlinQuery;

        TTargetQuery Coalesce<TTargetQuery>(params Func<IEdgeGremlinQuery<TEdge>, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IEdgeGremlinQuery<TEdge> Dedup();
        new IEdgeGremlinQuery<TEdge> Drop();

        new IEdgeGremlinQuery<TEdge> Emit();

        TTargetQuery FlatMap<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;

        new IEdgeGremlinQuery<TEdge> Identity();

        new IEdgeGremlinQuery<TEdge> Limit(long count);
        new IEdgeGremlinQuery<TEdge> LimitLocal(long count);
        TTargetQuery Local<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge> , TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;
        
        IEdgeGremlinQuery<TEdge> Not(Func<IEdgeGremlinQuery<TEdge>, IGremlinQuery> notTraversal);
        new IEdgeGremlinQuery<TEdge> None();

        TTargetQuery Optional<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge>, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery;
        IEdgeGremlinQuery<TEdge> Or(params Func<IEdgeGremlinQuery<TEdge>, IGremlinQuery>[] orTraversals);

        new IEdgeGremlinQuery<TEdge> Range(long low, long high);

        IEdgeGremlinQuery<TEdge> Repeat(Func<IEdgeGremlinQuery<TEdge>, IEdgeGremlinQuery<TEdge>> repeatTraversal);
        IEdgeGremlinQuery<TEdge> RepeatUntil(Func<IEdgeGremlinQuery<TEdge>, IEdgeGremlinQuery<TEdge>> repeatTraversal, Func<IEdgeGremlinQuery<TEdge>, IGremlinQuery> untilTraversal);
        IEdgeGremlinQuery<TEdge> UntilRepeat(Func<IEdgeGremlinQuery<TEdge>, IEdgeGremlinQuery<TEdge>> repeatTraversal, Func<IEdgeGremlinQuery<TEdge>, IGremlinQuery> untilTraversal);

        IEdgeGremlinQuery<TEdge> SideEffect(Func<IEdgeGremlinQuery<TEdge>, IGremlinQuery> sideEffectTraversal);
        new IEdgeGremlinQuery<TEdge> Skip(long count);

        new IEdgeGremlinQuery<TEdge> Tail(long count);
        new IEdgeGremlinQuery<TEdge> TailLocal(int count);

        new IEdgeGremlinQuery<TEdge> Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IEdgeGremlinQuery<TEdge>, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;

        new IEdgeGremlinQuery<TEdge> Where(ILambda lambda);
    }

    public partial interface IOrderedEdgeGremlinQuery<TEdge, TAdjacentVertex> : IEdgeGremlinQuery<TEdge, TAdjacentVertex> { }

    public partial interface IEdgeGremlinQuery<TEdge, TAdjacentVertex>
    {
        IEdgeGremlinQuery<TEdge, TAdjacentVertex> And(params Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery>[] andTraversals);

        new IEdgeGremlinQuery<TEdge, TAdjacentVertex> As(params StepLabel[] stepLabels);

        new IEdgeGremlinQuery<TEdge, TAdjacentVertex> Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> traversalPredicate, Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery> trueChoice, Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> traversalPredicate, Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;

        new TTargetQuery Choose<TTargetQuery>(Func<IChooseBuilder<IEdgeGremlinQuery<TEdge, TAdjacentVertex>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) where TTargetQuery : IGremlinQuery;

        TTargetQuery Coalesce<TTargetQuery>(params Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IEdgeGremlinQuery<TEdge, TAdjacentVertex> Dedup();
        new IEdgeGremlinQuery<TEdge, TAdjacentVertex> Drop();

        new IEdgeGremlinQuery<TEdge, TAdjacentVertex> Emit();

        TTargetQuery FlatMap<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;

        new IEdgeGremlinQuery<TEdge, TAdjacentVertex> Identity();

        new IEdgeGremlinQuery<TEdge, TAdjacentVertex> Limit(long count);
        new IEdgeGremlinQuery<TEdge, TAdjacentVertex> LimitLocal(long count);
        TTargetQuery Local<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex> , TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;
        
        IEdgeGremlinQuery<TEdge, TAdjacentVertex> Not(Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> notTraversal);
        new IEdgeGremlinQuery<TEdge, TAdjacentVertex> None();

        TTargetQuery Optional<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery;
        IEdgeGremlinQuery<TEdge, TAdjacentVertex> Or(params Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery>[] orTraversals);

        new IEdgeGremlinQuery<TEdge, TAdjacentVertex> Range(long low, long high);

        IEdgeGremlinQuery<TEdge, TAdjacentVertex> Repeat(Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, IEdgeGremlinQuery<TEdge, TAdjacentVertex>> repeatTraversal);
        IEdgeGremlinQuery<TEdge, TAdjacentVertex> RepeatUntil(Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, IEdgeGremlinQuery<TEdge, TAdjacentVertex>> repeatTraversal, Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> untilTraversal);
        IEdgeGremlinQuery<TEdge, TAdjacentVertex> UntilRepeat(Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, IEdgeGremlinQuery<TEdge, TAdjacentVertex>> repeatTraversal, Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> untilTraversal);

        IEdgeGremlinQuery<TEdge, TAdjacentVertex> SideEffect(Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> sideEffectTraversal);
        new IEdgeGremlinQuery<TEdge, TAdjacentVertex> Skip(long count);

        new IEdgeGremlinQuery<TEdge, TAdjacentVertex> Tail(long count);
        new IEdgeGremlinQuery<TEdge, TAdjacentVertex> TailLocal(int count);

        new IEdgeGremlinQuery<TEdge, TAdjacentVertex> Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;

        new IEdgeGremlinQuery<TEdge, TAdjacentVertex> Where(ILambda lambda);
    }

    public partial interface IOrderedEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> : IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> { }

    public partial interface IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>
    {
        IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> And(params Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, IGremlinQuery>[] andTraversals);

        new IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> As(params StepLabel[] stepLabels);

        new IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, IGremlinQuery> traversalPredicate, Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, TTargetQuery> trueChoice, Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, IGremlinQuery> traversalPredicate, Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;

        new TTargetQuery Choose<TTargetQuery>(Func<IChooseBuilder<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) where TTargetQuery : IGremlinQuery;

        TTargetQuery Coalesce<TTargetQuery>(params Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> Dedup();
        new IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> Drop();

        new IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> Emit();

        TTargetQuery FlatMap<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;

        new IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> Identity();

        new IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> Limit(long count);
        new IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> LimitLocal(long count);
        TTargetQuery Local<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> , TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;
        
        IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> Not(Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, IGremlinQuery> notTraversal);
        new IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> None();

        TTargetQuery Optional<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery;
        IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> Or(params Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, IGremlinQuery>[] orTraversals);

        new IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> Range(long low, long high);

        IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> Repeat(Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>> repeatTraversal);
        IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> RepeatUntil(Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>> repeatTraversal, Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, IGremlinQuery> untilTraversal);
        IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> UntilRepeat(Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>> repeatTraversal, Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, IGremlinQuery> untilTraversal);

        IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> SideEffect(Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, IGremlinQuery> sideEffectTraversal);
        new IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> Skip(long count);

        new IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> Tail(long count);
        new IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> TailLocal(int count);

        new IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;

        new IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> Where(ILambda lambda);
    }

    public partial interface IOrderedInEdgeGremlinQuery<TEdge, TInVertex> : IInEdgeGremlinQuery<TEdge, TInVertex> { }

    public partial interface IInEdgeGremlinQuery<TEdge, TInVertex>
    {
        IInEdgeGremlinQuery<TEdge, TInVertex> And(params Func<IInEdgeGremlinQuery<TEdge, TInVertex>, IGremlinQuery>[] andTraversals);

        new IInEdgeGremlinQuery<TEdge, TInVertex> As(params StepLabel[] stepLabels);

        new IInEdgeGremlinQuery<TEdge, TInVertex> Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IInEdgeGremlinQuery<TEdge, TInVertex>, IGremlinQuery> traversalPredicate, Func<IInEdgeGremlinQuery<TEdge, TInVertex>, TTargetQuery> trueChoice, Func<IInEdgeGremlinQuery<TEdge, TInVertex>, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IInEdgeGremlinQuery<TEdge, TInVertex>, IGremlinQuery> traversalPredicate, Func<IInEdgeGremlinQuery<TEdge, TInVertex>, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;

        new TTargetQuery Choose<TTargetQuery>(Func<IChooseBuilder<IInEdgeGremlinQuery<TEdge, TInVertex>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) where TTargetQuery : IGremlinQuery;

        TTargetQuery Coalesce<TTargetQuery>(params Func<IInEdgeGremlinQuery<TEdge, TInVertex>, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IInEdgeGremlinQuery<TEdge, TInVertex> Dedup();
        new IInEdgeGremlinQuery<TEdge, TInVertex> Drop();

        new IInEdgeGremlinQuery<TEdge, TInVertex> Emit();

        TTargetQuery FlatMap<TTargetQuery>(Func<IInEdgeGremlinQuery<TEdge, TInVertex>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;

        new IInEdgeGremlinQuery<TEdge, TInVertex> Identity();

        new IInEdgeGremlinQuery<TEdge, TInVertex> Limit(long count);
        new IInEdgeGremlinQuery<TEdge, TInVertex> LimitLocal(long count);
        TTargetQuery Local<TTargetQuery>(Func<IInEdgeGremlinQuery<TEdge, TInVertex> , TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IInEdgeGremlinQuery<TEdge, TInVertex>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;
        
        IInEdgeGremlinQuery<TEdge, TInVertex> Not(Func<IInEdgeGremlinQuery<TEdge, TInVertex>, IGremlinQuery> notTraversal);
        new IInEdgeGremlinQuery<TEdge, TInVertex> None();

        TTargetQuery Optional<TTargetQuery>(Func<IInEdgeGremlinQuery<TEdge, TInVertex>, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery;
        IInEdgeGremlinQuery<TEdge, TInVertex> Or(params Func<IInEdgeGremlinQuery<TEdge, TInVertex>, IGremlinQuery>[] orTraversals);

        new IInEdgeGremlinQuery<TEdge, TInVertex> Range(long low, long high);

        IInEdgeGremlinQuery<TEdge, TInVertex> Repeat(Func<IInEdgeGremlinQuery<TEdge, TInVertex>, IInEdgeGremlinQuery<TEdge, TInVertex>> repeatTraversal);
        IInEdgeGremlinQuery<TEdge, TInVertex> RepeatUntil(Func<IInEdgeGremlinQuery<TEdge, TInVertex>, IInEdgeGremlinQuery<TEdge, TInVertex>> repeatTraversal, Func<IInEdgeGremlinQuery<TEdge, TInVertex>, IGremlinQuery> untilTraversal);
        IInEdgeGremlinQuery<TEdge, TInVertex> UntilRepeat(Func<IInEdgeGremlinQuery<TEdge, TInVertex>, IInEdgeGremlinQuery<TEdge, TInVertex>> repeatTraversal, Func<IInEdgeGremlinQuery<TEdge, TInVertex>, IGremlinQuery> untilTraversal);

        IInEdgeGremlinQuery<TEdge, TInVertex> SideEffect(Func<IInEdgeGremlinQuery<TEdge, TInVertex>, IGremlinQuery> sideEffectTraversal);
        new IInEdgeGremlinQuery<TEdge, TInVertex> Skip(long count);

        new IInEdgeGremlinQuery<TEdge, TInVertex> Tail(long count);
        new IInEdgeGremlinQuery<TEdge, TInVertex> TailLocal(int count);

        new IInEdgeGremlinQuery<TEdge, TInVertex> Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IInEdgeGremlinQuery<TEdge, TInVertex>, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;

        new IInEdgeGremlinQuery<TEdge, TInVertex> Where(ILambda lambda);
    }

    public partial interface IOrderedOutEdgeGremlinQuery<TEdge, TOutVertex> : IOutEdgeGremlinQuery<TEdge, TOutVertex> { }

    public partial interface IOutEdgeGremlinQuery<TEdge, TOutVertex>
    {
        IOutEdgeGremlinQuery<TEdge, TOutVertex> And(params Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, IGremlinQuery>[] andTraversals);

        new IOutEdgeGremlinQuery<TEdge, TOutVertex> As(params StepLabel[] stepLabels);

        new IOutEdgeGremlinQuery<TEdge, TOutVertex> Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, IGremlinQuery> traversalPredicate, Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, TTargetQuery> trueChoice, Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, IGremlinQuery> traversalPredicate, Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;

        new TTargetQuery Choose<TTargetQuery>(Func<IChooseBuilder<IOutEdgeGremlinQuery<TEdge, TOutVertex>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) where TTargetQuery : IGremlinQuery;

        TTargetQuery Coalesce<TTargetQuery>(params Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IOutEdgeGremlinQuery<TEdge, TOutVertex> Dedup();
        new IOutEdgeGremlinQuery<TEdge, TOutVertex> Drop();

        new IOutEdgeGremlinQuery<TEdge, TOutVertex> Emit();

        TTargetQuery FlatMap<TTargetQuery>(Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;

        new IOutEdgeGremlinQuery<TEdge, TOutVertex> Identity();

        new IOutEdgeGremlinQuery<TEdge, TOutVertex> Limit(long count);
        new IOutEdgeGremlinQuery<TEdge, TOutVertex> LimitLocal(long count);
        TTargetQuery Local<TTargetQuery>(Func<IOutEdgeGremlinQuery<TEdge, TOutVertex> , TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;
        
        IOutEdgeGremlinQuery<TEdge, TOutVertex> Not(Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, IGremlinQuery> notTraversal);
        new IOutEdgeGremlinQuery<TEdge, TOutVertex> None();

        TTargetQuery Optional<TTargetQuery>(Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery;
        IOutEdgeGremlinQuery<TEdge, TOutVertex> Or(params Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, IGremlinQuery>[] orTraversals);

        new IOutEdgeGremlinQuery<TEdge, TOutVertex> Range(long low, long high);

        IOutEdgeGremlinQuery<TEdge, TOutVertex> Repeat(Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, IOutEdgeGremlinQuery<TEdge, TOutVertex>> repeatTraversal);
        IOutEdgeGremlinQuery<TEdge, TOutVertex> RepeatUntil(Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, IOutEdgeGremlinQuery<TEdge, TOutVertex>> repeatTraversal, Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, IGremlinQuery> untilTraversal);
        IOutEdgeGremlinQuery<TEdge, TOutVertex> UntilRepeat(Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, IOutEdgeGremlinQuery<TEdge, TOutVertex>> repeatTraversal, Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, IGremlinQuery> untilTraversal);

        IOutEdgeGremlinQuery<TEdge, TOutVertex> SideEffect(Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, IGremlinQuery> sideEffectTraversal);
        new IOutEdgeGremlinQuery<TEdge, TOutVertex> Skip(long count);

        new IOutEdgeGremlinQuery<TEdge, TOutVertex> Tail(long count);
        new IOutEdgeGremlinQuery<TEdge, TOutVertex> TailLocal(int count);

        new IOutEdgeGremlinQuery<TEdge, TOutVertex> Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;

        new IOutEdgeGremlinQuery<TEdge, TOutVertex> Where(ILambda lambda);
    }

    public partial interface IOrderedVertexPropertyGremlinQuery<TProperty, TValue> : IVertexPropertyGremlinQuery<TProperty, TValue> { }

    public partial interface IVertexPropertyGremlinQuery<TProperty, TValue>
    {
        IVertexPropertyGremlinQuery<TProperty, TValue> And(params Func<IVertexPropertyGremlinQuery<TProperty, TValue>, IGremlinQuery>[] andTraversals);

        new IVertexPropertyGremlinQuery<TProperty, TValue> As(params StepLabel[] stepLabels);

        new IVertexPropertyGremlinQuery<TProperty, TValue> Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TProperty, TValue>, IGremlinQuery> traversalPredicate, Func<IVertexPropertyGremlinQuery<TProperty, TValue>, TTargetQuery> trueChoice, Func<IVertexPropertyGremlinQuery<TProperty, TValue>, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TProperty, TValue>, IGremlinQuery> traversalPredicate, Func<IVertexPropertyGremlinQuery<TProperty, TValue>, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;

        new TTargetQuery Choose<TTargetQuery>(Func<IChooseBuilder<IVertexPropertyGremlinQuery<TProperty, TValue>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) where TTargetQuery : IGremlinQuery;

        TTargetQuery Coalesce<TTargetQuery>(params Func<IVertexPropertyGremlinQuery<TProperty, TValue>, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IVertexPropertyGremlinQuery<TProperty, TValue> Dedup();
        new IVertexPropertyGremlinQuery<TProperty, TValue> Drop();

        new IVertexPropertyGremlinQuery<TProperty, TValue> Emit();

        TTargetQuery FlatMap<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TProperty, TValue>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;

        new IVertexPropertyGremlinQuery<TProperty, TValue> Identity();

        new IVertexPropertyGremlinQuery<TProperty, TValue> Limit(long count);
        new IVertexPropertyGremlinQuery<TProperty, TValue> LimitLocal(long count);
        TTargetQuery Local<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TProperty, TValue> , TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TProperty, TValue>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;
        
        IVertexPropertyGremlinQuery<TProperty, TValue> Not(Func<IVertexPropertyGremlinQuery<TProperty, TValue>, IGremlinQuery> notTraversal);
        new IVertexPropertyGremlinQuery<TProperty, TValue> None();

        TTargetQuery Optional<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TProperty, TValue>, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery;
        IVertexPropertyGremlinQuery<TProperty, TValue> Or(params Func<IVertexPropertyGremlinQuery<TProperty, TValue>, IGremlinQuery>[] orTraversals);

        new IVertexPropertyGremlinQuery<TProperty, TValue> Range(long low, long high);

        IVertexPropertyGremlinQuery<TProperty, TValue> Repeat(Func<IVertexPropertyGremlinQuery<TProperty, TValue>, IVertexPropertyGremlinQuery<TProperty, TValue>> repeatTraversal);
        IVertexPropertyGremlinQuery<TProperty, TValue> RepeatUntil(Func<IVertexPropertyGremlinQuery<TProperty, TValue>, IVertexPropertyGremlinQuery<TProperty, TValue>> repeatTraversal, Func<IVertexPropertyGremlinQuery<TProperty, TValue>, IGremlinQuery> untilTraversal);
        IVertexPropertyGremlinQuery<TProperty, TValue> UntilRepeat(Func<IVertexPropertyGremlinQuery<TProperty, TValue>, IVertexPropertyGremlinQuery<TProperty, TValue>> repeatTraversal, Func<IVertexPropertyGremlinQuery<TProperty, TValue>, IGremlinQuery> untilTraversal);

        IVertexPropertyGremlinQuery<TProperty, TValue> SideEffect(Func<IVertexPropertyGremlinQuery<TProperty, TValue>, IGremlinQuery> sideEffectTraversal);
        new IVertexPropertyGremlinQuery<TProperty, TValue> Skip(long count);

        new IVertexPropertyGremlinQuery<TProperty, TValue> Tail(long count);
        new IVertexPropertyGremlinQuery<TProperty, TValue> TailLocal(int count);

        new IVertexPropertyGremlinQuery<TProperty, TValue> Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IVertexPropertyGremlinQuery<TProperty, TValue>, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;

        new IVertexPropertyGremlinQuery<TProperty, TValue> Where(ILambda lambda);
    }

    public partial interface IOrderedVertexPropertyGremlinQuery<TProperty, TValue, TMeta> : IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> { }

    public partial interface IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>
    {
        IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> And(params Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, IGremlinQuery>[] andTraversals);

        new IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> As(params StepLabel[] stepLabels);

        new IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, IGremlinQuery> traversalPredicate, Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, TTargetQuery> trueChoice, Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, IGremlinQuery> traversalPredicate, Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;

        new TTargetQuery Choose<TTargetQuery>(Func<IChooseBuilder<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) where TTargetQuery : IGremlinQuery;

        TTargetQuery Coalesce<TTargetQuery>(params Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Dedup();
        new IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Drop();

        new IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Emit();

        TTargetQuery FlatMap<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;

        new IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Identity();

        new IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Limit(long count);
        new IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> LimitLocal(long count);
        TTargetQuery Local<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> , TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;
        
        IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Not(Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, IGremlinQuery> notTraversal);
        new IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> None();

        TTargetQuery Optional<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery;
        IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Or(params Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, IGremlinQuery>[] orTraversals);

        new IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Range(long low, long high);

        IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Repeat(Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>> repeatTraversal);
        IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> RepeatUntil(Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>> repeatTraversal, Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, IGremlinQuery> untilTraversal);
        IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> UntilRepeat(Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>> repeatTraversal, Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, IGremlinQuery> untilTraversal);

        IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> SideEffect(Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, IGremlinQuery> sideEffectTraversal);
        new IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Skip(long count);

        new IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Tail(long count);
        new IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> TailLocal(int count);

        new IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;

        new IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Where(ILambda lambda);
    }

    public partial interface IOrderedPropertyGremlinQuery<TElement> : IPropertyGremlinQuery<TElement> { }

    public partial interface IPropertyGremlinQuery<TElement>
    {
        IPropertyGremlinQuery<TElement> And(params Func<IPropertyGremlinQuery<TElement>, IGremlinQuery>[] andTraversals);

        new IPropertyGremlinQuery<TElement> As(params StepLabel[] stepLabels);

        new IPropertyGremlinQuery<TElement> Barrier();

        TTargetQuery Choose<TTargetQuery>(Func<IPropertyGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IPropertyGremlinQuery<TElement>, TTargetQuery> trueChoice, Func<IPropertyGremlinQuery<TElement>, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery;
        TTargetQuery Choose<TTargetQuery>(Func<IPropertyGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IPropertyGremlinQuery<TElement>, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery;

        new TTargetQuery Choose<TTargetQuery>(Func<IChooseBuilder<IPropertyGremlinQuery<TElement>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation) where TTargetQuery : IGremlinQuery;

        TTargetQuery Coalesce<TTargetQuery>(params Func<IPropertyGremlinQuery<TElement>, TTargetQuery>[] traversals) where TTargetQuery : IGremlinQuery;

        new IPropertyGremlinQuery<TElement> Dedup();
        new IPropertyGremlinQuery<TElement> Drop();

        new IPropertyGremlinQuery<TElement> Emit();

        TTargetQuery FlatMap<TTargetQuery>(Func<IPropertyGremlinQuery<TElement>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;

        new IPropertyGremlinQuery<TElement> Identity();

        new IPropertyGremlinQuery<TElement> Limit(long count);
        new IPropertyGremlinQuery<TElement> LimitLocal(long count);
        TTargetQuery Local<TTargetQuery>(Func<IPropertyGremlinQuery<TElement> , TTargetQuery> localTraversal) where TTargetQuery : IGremlinQuery;

        TTargetQuery Map<TTargetQuery>(Func<IPropertyGremlinQuery<TElement>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery;
        
        IPropertyGremlinQuery<TElement> Not(Func<IPropertyGremlinQuery<TElement>, IGremlinQuery> notTraversal);
        new IPropertyGremlinQuery<TElement> None();

        TTargetQuery Optional<TTargetQuery>(Func<IPropertyGremlinQuery<TElement>, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery;
        IPropertyGremlinQuery<TElement> Or(params Func<IPropertyGremlinQuery<TElement>, IGremlinQuery>[] orTraversals);

        new IPropertyGremlinQuery<TElement> Range(long low, long high);

        IPropertyGremlinQuery<TElement> Repeat(Func<IPropertyGremlinQuery<TElement>, IPropertyGremlinQuery<TElement>> repeatTraversal);
        IPropertyGremlinQuery<TElement> RepeatUntil(Func<IPropertyGremlinQuery<TElement>, IPropertyGremlinQuery<TElement>> repeatTraversal, Func<IPropertyGremlinQuery<TElement>, IGremlinQuery> untilTraversal);
        IPropertyGremlinQuery<TElement> UntilRepeat(Func<IPropertyGremlinQuery<TElement>, IPropertyGremlinQuery<TElement>> repeatTraversal, Func<IPropertyGremlinQuery<TElement>, IGremlinQuery> untilTraversal);

        IPropertyGremlinQuery<TElement> SideEffect(Func<IPropertyGremlinQuery<TElement>, IGremlinQuery> sideEffectTraversal);
        new IPropertyGremlinQuery<TElement> Skip(long count);

        new IPropertyGremlinQuery<TElement> Tail(long count);
        new IPropertyGremlinQuery<TElement> TailLocal(int count);

        new IPropertyGremlinQuery<TElement> Times(int count);

        TTargetQuery Union<TTargetQuery>(params Func<IPropertyGremlinQuery<TElement>, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery;

        new IPropertyGremlinQuery<TElement> Where(ILambda lambda);
    }


    public partial interface IGremlinQuery
    {
        new IGremlinQuery<TResult> Cast<TResult>();
        new IGremlinQuery<dynamic> Project(Func<IProjectBuilder<IGremlinQuery<object>, object>, IProjectResult> continuation);
        new IGremlinQuery<TResult> Project<TResult>(Func<IProjectBuilder<IGremlinQuery<object>, object>, IProjectResult<TResult>> continuation);
    }
    public partial interface IElementGremlinQuery
    {
        new IElementGremlinQuery<TResult> Cast<TResult>();
        new IGremlinQuery<dynamic> Project(Func<IProjectBuilder<IElementGremlinQuery<object>, object>, IProjectResult> continuation);
        new IGremlinQuery<TResult> Project<TResult>(Func<IProjectBuilder<IElementGremlinQuery<object>, object>, IProjectResult<TResult>> continuation);
    }
    public partial interface IVertexGremlinQuery
    {
        new IVertexGremlinQuery<TResult> Cast<TResult>();
        new IGremlinQuery<dynamic> Project(Func<IProjectBuilder<IVertexGremlinQuery<object>, object>, IProjectResult> continuation);
        new IGremlinQuery<TResult> Project<TResult>(Func<IProjectBuilder<IVertexGremlinQuery<object>, object>, IProjectResult<TResult>> continuation);
    }
    public partial interface IEdgeGremlinQuery
    {
        new IEdgeGremlinQuery<TResult> Cast<TResult>();
        new IGremlinQuery<dynamic> Project(Func<IProjectBuilder<IEdgeGremlinQuery<object>, object>, IProjectResult> continuation);
        new IGremlinQuery<TResult> Project<TResult>(Func<IProjectBuilder<IEdgeGremlinQuery<object>, object>, IProjectResult<TResult>> continuation);
    }


    public partial interface IGremlinQuery<TElement>
    {
        new IGremlinQuery<dynamic> Project(Func<IProjectBuilder<IGremlinQuery<TElement>, TElement>, IProjectResult> continuation);
        new IGremlinQuery<TResult> Project<TResult>(Func<IProjectBuilder<IGremlinQuery<TElement>, TElement>, IProjectResult<TResult>> continuation);
    }
    public partial interface IValueGremlinQuery<TElement>
    {
        new IGremlinQuery<dynamic> Project(Func<IProjectBuilder<IValueGremlinQuery<TElement>, TElement>, IProjectResult> continuation);
        new IGremlinQuery<TResult> Project<TResult>(Func<IProjectBuilder<IValueGremlinQuery<TElement>, TElement>, IProjectResult<TResult>> continuation);
    }
    public partial interface IArrayGremlinQuery<TArray, TQuery>
    {
        new IGremlinQuery<dynamic> Project(Func<IProjectBuilder<IArrayGremlinQuery<TArray, TQuery>, TArray>, IProjectResult> continuation);
        new IGremlinQuery<TResult> Project<TResult>(Func<IProjectBuilder<IArrayGremlinQuery<TArray, TQuery>, TArray>, IProjectResult<TResult>> continuation);
    }
    public partial interface IElementGremlinQuery<TElement>
    {
        new IGremlinQuery<dynamic> Project(Func<IProjectBuilder<IElementGremlinQuery<TElement>, TElement>, IProjectResult> continuation);
        new IGremlinQuery<TResult> Project<TResult>(Func<IProjectBuilder<IElementGremlinQuery<TElement>, TElement>, IProjectResult<TResult>> continuation);
    }
    public partial interface IVertexGremlinQuery<TVertex>
    {
        new IGremlinQuery<dynamic> Project(Func<IProjectBuilder<IVertexGremlinQuery<TVertex>, TVertex>, IProjectResult> continuation);
        new IGremlinQuery<TResult> Project<TResult>(Func<IProjectBuilder<IVertexGremlinQuery<TVertex>, TVertex>, IProjectResult<TResult>> continuation);
    }
    public partial interface IEdgeGremlinQuery<TEdge>
    {
        new IGremlinQuery<dynamic> Project(Func<IProjectBuilder<IEdgeGremlinQuery<TEdge>, TEdge>, IProjectResult> continuation);
        new IGremlinQuery<TResult> Project<TResult>(Func<IProjectBuilder<IEdgeGremlinQuery<TEdge>, TEdge>, IProjectResult<TResult>> continuation);
    }
    public partial interface IEdgeGremlinQuery<TEdge, TAdjacentVertex>
    {
        new IGremlinQuery<dynamic> Project(Func<IProjectBuilder<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, TEdge>, IProjectResult> continuation);
        new IGremlinQuery<TResult> Project<TResult>(Func<IProjectBuilder<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, TEdge>, IProjectResult<TResult>> continuation);
    }
    public partial interface IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>
    {
        new IGremlinQuery<dynamic> Project(Func<IProjectBuilder<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, TEdge>, IProjectResult> continuation);
        new IGremlinQuery<TResult> Project<TResult>(Func<IProjectBuilder<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, TEdge>, IProjectResult<TResult>> continuation);
    }
    public partial interface IInEdgeGremlinQuery<TEdge, TInVertex>
    {
        new IGremlinQuery<dynamic> Project(Func<IProjectBuilder<IInEdgeGremlinQuery<TEdge, TInVertex>, TEdge>, IProjectResult> continuation);
        new IGremlinQuery<TResult> Project<TResult>(Func<IProjectBuilder<IInEdgeGremlinQuery<TEdge, TInVertex>, TEdge>, IProjectResult<TResult>> continuation);
    }
    public partial interface IOutEdgeGremlinQuery<TEdge, TOutVertex>
    {
        new IGremlinQuery<dynamic> Project(Func<IProjectBuilder<IOutEdgeGremlinQuery<TEdge, TOutVertex>, TEdge>, IProjectResult> continuation);
        new IGremlinQuery<TResult> Project<TResult>(Func<IProjectBuilder<IOutEdgeGremlinQuery<TEdge, TOutVertex>, TEdge>, IProjectResult<TResult>> continuation);
    }
    public partial interface IVertexPropertyGremlinQuery<TProperty, TValue>
    {
        new IGremlinQuery<dynamic> Project(Func<IProjectBuilder<IVertexPropertyGremlinQuery<TProperty, TValue>, TProperty>, IProjectResult> continuation);
        new IGremlinQuery<TResult> Project<TResult>(Func<IProjectBuilder<IVertexPropertyGremlinQuery<TProperty, TValue>, TProperty>, IProjectResult<TResult>> continuation);
    }
    public partial interface IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>
    {
        new IGremlinQuery<dynamic> Project(Func<IProjectBuilder<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, TProperty>, IProjectResult> continuation);
        new IGremlinQuery<TResult> Project<TResult>(Func<IProjectBuilder<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, TProperty>, IProjectResult<TResult>> continuation);
    }
    public partial interface IPropertyGremlinQuery<TElement>
    {
        new IGremlinQuery<dynamic> Project(Func<IProjectBuilder<IPropertyGremlinQuery<TElement>, TElement>, IProjectResult> continuation);
        new IGremlinQuery<TResult> Project<TResult>(Func<IProjectBuilder<IPropertyGremlinQuery<TElement>, TElement>, IProjectResult<TResult>> continuation);
    }


    public partial interface IValueGremlinQuery<TElement>
    {
        new IOrderedValueGremlinQuery<TElement> OrderBy(Expression<Func<TElement, object>> projection);
        new IOrderedValueGremlinQuery<TElement> OrderByDescending(Expression<Func<TElement, object>> projection);
    }
    public partial interface IArrayGremlinQuery<TArray, TQuery>
    {
        new IOrderedArrayGremlinQuery<TArray, TQuery> OrderBy(Expression<Func<TArray, object>> projection);
        new IOrderedArrayGremlinQuery<TArray, TQuery> OrderByDescending(Expression<Func<TArray, object>> projection);
    }
    public partial interface IVertexGremlinQuery<TVertex>
    {
        new IOrderedVertexGremlinQuery<TVertex> OrderBy(Expression<Func<TVertex, object>> projection);
        new IOrderedVertexGremlinQuery<TVertex> OrderByDescending(Expression<Func<TVertex, object>> projection);
    }
    public partial interface IEdgeGremlinQuery<TEdge>
    {
        new IOrderedEdgeGremlinQuery<TEdge> OrderBy(Expression<Func<TEdge, object>> projection);
        new IOrderedEdgeGremlinQuery<TEdge> OrderByDescending(Expression<Func<TEdge, object>> projection);
    }
    public partial interface IEdgeGremlinQuery<TEdge, TAdjacentVertex>
    {
        new IOrderedEdgeGremlinQuery<TEdge, TAdjacentVertex> OrderBy(Expression<Func<TEdge, object>> projection);
        new IOrderedEdgeGremlinQuery<TEdge, TAdjacentVertex> OrderByDescending(Expression<Func<TEdge, object>> projection);
    }
    public partial interface IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>
    {
        new IOrderedEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> OrderBy(Expression<Func<TEdge, object>> projection);
        new IOrderedEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> OrderByDescending(Expression<Func<TEdge, object>> projection);
    }
    public partial interface IInEdgeGremlinQuery<TEdge, TInVertex>
    {
        new IOrderedInEdgeGremlinQuery<TEdge, TInVertex> OrderBy(Expression<Func<TEdge, object>> projection);
        new IOrderedInEdgeGremlinQuery<TEdge, TInVertex> OrderByDescending(Expression<Func<TEdge, object>> projection);
    }
    public partial interface IOutEdgeGremlinQuery<TEdge, TOutVertex>
    {
        new IOrderedOutEdgeGremlinQuery<TEdge, TOutVertex> OrderBy(Expression<Func<TEdge, object>> projection);
        new IOrderedOutEdgeGremlinQuery<TEdge, TOutVertex> OrderByDescending(Expression<Func<TEdge, object>> projection);
    }
    public partial interface IVertexPropertyGremlinQuery<TProperty, TValue>
    {
        new IOrderedVertexPropertyGremlinQuery<TProperty, TValue> OrderBy(Expression<Func<TProperty, object>> projection);
        new IOrderedVertexPropertyGremlinQuery<TProperty, TValue> OrderByDescending(Expression<Func<TProperty, object>> projection);
    }
    public partial interface IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>
    {
        new IOrderedVertexPropertyGremlinQuery<TProperty, TValue, TMeta> OrderBy(Expression<Func<TProperty, object>> projection);
        new IOrderedVertexPropertyGremlinQuery<TProperty, TValue, TMeta> OrderByDescending(Expression<Func<TProperty, object>> projection);
    }
    public partial interface IPropertyGremlinQuery<TElement>
    {
        new IOrderedPropertyGremlinQuery<TElement> OrderBy(Expression<Func<TElement, object>> projection);
        new IOrderedPropertyGremlinQuery<TElement> OrderByDescending(Expression<Func<TElement, object>> projection);
    }


    public partial interface IVertexGremlinQuery
    {
        new IOrderedVertexGremlinQuery OrderBy(Func<IVertexGremlinQuery, IGremlinQuery> traversal);
        new IOrderedVertexGremlinQuery OrderBy(ILambda lambda);
        new IOrderedVertexGremlinQuery OrderByDescending(Func<IVertexGremlinQuery, IGremlinQuery> traversal);
    }
    public partial interface IEdgeGremlinQuery
    {
        new IOrderedEdgeGremlinQuery OrderBy(Func<IEdgeGremlinQuery, IGremlinQuery> traversal);
        new IOrderedEdgeGremlinQuery OrderBy(ILambda lambda);
        new IOrderedEdgeGremlinQuery OrderByDescending(Func<IEdgeGremlinQuery, IGremlinQuery> traversal);
    }
    public partial interface IValueGremlinQuery<TElement>
    {
        new IOrderedValueGremlinQuery<TElement> OrderBy(Func<IValueGremlinQuery<TElement>, IGremlinQuery> traversal);
        new IOrderedValueGremlinQuery<TElement> OrderBy(ILambda lambda);
        new IOrderedValueGremlinQuery<TElement> OrderByDescending(Func<IValueGremlinQuery<TElement>, IGremlinQuery> traversal);
    }
    public partial interface IArrayGremlinQuery<TArray, TQuery>
    {
        new IOrderedArrayGremlinQuery<TArray, TQuery> OrderBy(Func<IArrayGremlinQuery<TArray, TQuery>, IGremlinQuery> traversal);
        new IOrderedArrayGremlinQuery<TArray, TQuery> OrderBy(ILambda lambda);
        new IOrderedArrayGremlinQuery<TArray, TQuery> OrderByDescending(Func<IArrayGremlinQuery<TArray, TQuery>, IGremlinQuery> traversal);
    }
    public partial interface IVertexGremlinQuery<TVertex>
    {
        new IOrderedVertexGremlinQuery<TVertex> OrderBy(Func<IVertexGremlinQuery<TVertex>, IGremlinQuery> traversal);
        new IOrderedVertexGremlinQuery<TVertex> OrderBy(ILambda lambda);
        new IOrderedVertexGremlinQuery<TVertex> OrderByDescending(Func<IVertexGremlinQuery<TVertex>, IGremlinQuery> traversal);
    }
    public partial interface IEdgeGremlinQuery<TEdge>
    {
        new IOrderedEdgeGremlinQuery<TEdge> OrderBy(Func<IEdgeGremlinQuery<TEdge>, IGremlinQuery> traversal);
        new IOrderedEdgeGremlinQuery<TEdge> OrderBy(ILambda lambda);
        new IOrderedEdgeGremlinQuery<TEdge> OrderByDescending(Func<IEdgeGremlinQuery<TEdge>, IGremlinQuery> traversal);
    }
    public partial interface IEdgeGremlinQuery<TEdge, TAdjacentVertex>
    {
        new IOrderedEdgeGremlinQuery<TEdge, TAdjacentVertex> OrderBy(Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> traversal);
        new IOrderedEdgeGremlinQuery<TEdge, TAdjacentVertex> OrderBy(ILambda lambda);
        new IOrderedEdgeGremlinQuery<TEdge, TAdjacentVertex> OrderByDescending(Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> traversal);
    }
    public partial interface IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>
    {
        new IOrderedEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> OrderBy(Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, IGremlinQuery> traversal);
        new IOrderedEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> OrderBy(ILambda lambda);
        new IOrderedEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> OrderByDescending(Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, IGremlinQuery> traversal);
    }
    public partial interface IInEdgeGremlinQuery<TEdge, TInVertex>
    {
        new IOrderedInEdgeGremlinQuery<TEdge, TInVertex> OrderBy(Func<IInEdgeGremlinQuery<TEdge, TInVertex>, IGremlinQuery> traversal);
        new IOrderedInEdgeGremlinQuery<TEdge, TInVertex> OrderBy(ILambda lambda);
        new IOrderedInEdgeGremlinQuery<TEdge, TInVertex> OrderByDescending(Func<IInEdgeGremlinQuery<TEdge, TInVertex>, IGremlinQuery> traversal);
    }
    public partial interface IOutEdgeGremlinQuery<TEdge, TOutVertex>
    {
        new IOrderedOutEdgeGremlinQuery<TEdge, TOutVertex> OrderBy(Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, IGremlinQuery> traversal);
        new IOrderedOutEdgeGremlinQuery<TEdge, TOutVertex> OrderBy(ILambda lambda);
        new IOrderedOutEdgeGremlinQuery<TEdge, TOutVertex> OrderByDescending(Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, IGremlinQuery> traversal);
    }
    public partial interface IVertexPropertyGremlinQuery<TProperty, TValue>
    {
        new IOrderedVertexPropertyGremlinQuery<TProperty, TValue> OrderBy(Func<IVertexPropertyGremlinQuery<TProperty, TValue>, IGremlinQuery> traversal);
        new IOrderedVertexPropertyGremlinQuery<TProperty, TValue> OrderBy(ILambda lambda);
        new IOrderedVertexPropertyGremlinQuery<TProperty, TValue> OrderByDescending(Func<IVertexPropertyGremlinQuery<TProperty, TValue>, IGremlinQuery> traversal);
    }
    public partial interface IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>
    {
        new IOrderedVertexPropertyGremlinQuery<TProperty, TValue, TMeta> OrderBy(Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, IGremlinQuery> traversal);
        new IOrderedVertexPropertyGremlinQuery<TProperty, TValue, TMeta> OrderBy(ILambda lambda);
        new IOrderedVertexPropertyGremlinQuery<TProperty, TValue, TMeta> OrderByDescending(Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, IGremlinQuery> traversal);
    }
    public partial interface IPropertyGremlinQuery<TElement>
    {
        new IOrderedPropertyGremlinQuery<TElement> OrderBy(Func<IPropertyGremlinQuery<TElement>, IGremlinQuery> traversal);
        new IOrderedPropertyGremlinQuery<TElement> OrderBy(ILambda lambda);
        new IOrderedPropertyGremlinQuery<TElement> OrderByDescending(Func<IPropertyGremlinQuery<TElement>, IGremlinQuery> traversal);
    }


    public partial interface IOrderedVertexGremlinQuery
    {
        IOrderedVertexGremlinQuery ThenBy(Func<IVertexGremlinQuery, IGremlinQuery> traversal);
        IOrderedVertexGremlinQuery ThenByDescending(Func<IVertexGremlinQuery, IGremlinQuery> traversal);
        new IOrderedVertexGremlinQuery ThenBy(ILambda lambda);
    }
    public partial interface IOrderedEdgeGremlinQuery
    {
        IOrderedEdgeGremlinQuery ThenBy(Func<IEdgeGremlinQuery, IGremlinQuery> traversal);
        IOrderedEdgeGremlinQuery ThenByDescending(Func<IEdgeGremlinQuery, IGremlinQuery> traversal);
        new IOrderedEdgeGremlinQuery ThenBy(ILambda lambda);
    }
    public partial interface IOrderedValueGremlinQuery<TElement>
    {
        IOrderedValueGremlinQuery<TElement> ThenBy(Func<IValueGremlinQuery<TElement>, IGremlinQuery> traversal);
        IOrderedValueGremlinQuery<TElement> ThenByDescending(Func<IValueGremlinQuery<TElement>, IGremlinQuery> traversal);
        new IOrderedValueGremlinQuery<TElement> ThenBy(ILambda lambda);
    }
    public partial interface IOrderedArrayGremlinQuery<TArray, TQuery>
    {
        IOrderedArrayGremlinQuery<TArray, TQuery> ThenBy(Func<IArrayGremlinQuery<TArray, TQuery>, IGremlinQuery> traversal);
        IOrderedArrayGremlinQuery<TArray, TQuery> ThenByDescending(Func<IArrayGremlinQuery<TArray, TQuery>, IGremlinQuery> traversal);
        new IOrderedArrayGremlinQuery<TArray, TQuery> ThenBy(ILambda lambda);
    }
    public partial interface IOrderedVertexGremlinQuery<TVertex>
    {
        IOrderedVertexGremlinQuery<TVertex> ThenBy(Func<IVertexGremlinQuery<TVertex>, IGremlinQuery> traversal);
        IOrderedVertexGremlinQuery<TVertex> ThenByDescending(Func<IVertexGremlinQuery<TVertex>, IGremlinQuery> traversal);
        new IOrderedVertexGremlinQuery<TVertex> ThenBy(ILambda lambda);
    }
    public partial interface IOrderedEdgeGremlinQuery<TEdge>
    {
        IOrderedEdgeGremlinQuery<TEdge> ThenBy(Func<IEdgeGremlinQuery<TEdge>, IGremlinQuery> traversal);
        IOrderedEdgeGremlinQuery<TEdge> ThenByDescending(Func<IEdgeGremlinQuery<TEdge>, IGremlinQuery> traversal);
        new IOrderedEdgeGremlinQuery<TEdge> ThenBy(ILambda lambda);
    }
    public partial interface IOrderedEdgeGremlinQuery<TEdge, TAdjacentVertex>
    {
        IOrderedEdgeGremlinQuery<TEdge, TAdjacentVertex> ThenBy(Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> traversal);
        IOrderedEdgeGremlinQuery<TEdge, TAdjacentVertex> ThenByDescending(Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> traversal);
        new IOrderedEdgeGremlinQuery<TEdge, TAdjacentVertex> ThenBy(ILambda lambda);
    }
    public partial interface IOrderedEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>
    {
        IOrderedEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> ThenBy(Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, IGremlinQuery> traversal);
        IOrderedEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> ThenByDescending(Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, IGremlinQuery> traversal);
        new IOrderedEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> ThenBy(ILambda lambda);
    }
    public partial interface IOrderedInEdgeGremlinQuery<TEdge, TInVertex>
    {
        IOrderedInEdgeGremlinQuery<TEdge, TInVertex> ThenBy(Func<IInEdgeGremlinQuery<TEdge, TInVertex>, IGremlinQuery> traversal);
        IOrderedInEdgeGremlinQuery<TEdge, TInVertex> ThenByDescending(Func<IInEdgeGremlinQuery<TEdge, TInVertex>, IGremlinQuery> traversal);
        new IOrderedInEdgeGremlinQuery<TEdge, TInVertex> ThenBy(ILambda lambda);
    }
    public partial interface IOrderedOutEdgeGremlinQuery<TEdge, TOutVertex>
    {
        IOrderedOutEdgeGremlinQuery<TEdge, TOutVertex> ThenBy(Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, IGremlinQuery> traversal);
        IOrderedOutEdgeGremlinQuery<TEdge, TOutVertex> ThenByDescending(Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, IGremlinQuery> traversal);
        new IOrderedOutEdgeGremlinQuery<TEdge, TOutVertex> ThenBy(ILambda lambda);
    }
    public partial interface IOrderedVertexPropertyGremlinQuery<TProperty, TValue>
    {
        IOrderedVertexPropertyGremlinQuery<TProperty, TValue> ThenBy(Func<IVertexPropertyGremlinQuery<TProperty, TValue>, IGremlinQuery> traversal);
        IOrderedVertexPropertyGremlinQuery<TProperty, TValue> ThenByDescending(Func<IVertexPropertyGremlinQuery<TProperty, TValue>, IGremlinQuery> traversal);
        new IOrderedVertexPropertyGremlinQuery<TProperty, TValue> ThenBy(ILambda lambda);
    }
    public partial interface IOrderedVertexPropertyGremlinQuery<TProperty, TValue, TMeta>
    {
        IOrderedVertexPropertyGremlinQuery<TProperty, TValue, TMeta> ThenBy(Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, IGremlinQuery> traversal);
        IOrderedVertexPropertyGremlinQuery<TProperty, TValue, TMeta> ThenByDescending(Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, IGremlinQuery> traversal);
        new IOrderedVertexPropertyGremlinQuery<TProperty, TValue, TMeta> ThenBy(ILambda lambda);
    }
    public partial interface IOrderedPropertyGremlinQuery<TElement>
    {
        IOrderedPropertyGremlinQuery<TElement> ThenBy(Func<IPropertyGremlinQuery<TElement>, IGremlinQuery> traversal);
        IOrderedPropertyGremlinQuery<TElement> ThenByDescending(Func<IPropertyGremlinQuery<TElement>, IGremlinQuery> traversal);
        new IOrderedPropertyGremlinQuery<TElement> ThenBy(ILambda lambda);
    }
    
        
    public partial interface IOrderedValueGremlinQuery<TElement>
    {
        new IOrderedValueGremlinQuery<TElement> ThenBy(Expression<Func<TElement, object>> projection);
        new IOrderedValueGremlinQuery<TElement> ThenByDescending(Expression<Func<TElement, object>> projection);
    }

    public partial interface IOrderedArrayGremlinQuery<TArray, TQuery>
    {
        new IOrderedArrayGremlinQuery<TArray, TQuery> ThenBy(Expression<Func<TArray, object>> projection);
        new IOrderedArrayGremlinQuery<TArray, TQuery> ThenByDescending(Expression<Func<TArray, object>> projection);
    }

    public partial interface IOrderedVertexGremlinQuery<TVertex>
    {
        new IOrderedVertexGremlinQuery<TVertex> ThenBy(Expression<Func<TVertex, object>> projection);
        new IOrderedVertexGremlinQuery<TVertex> ThenByDescending(Expression<Func<TVertex, object>> projection);
    }

    public partial interface IOrderedEdgeGremlinQuery<TEdge>
    {
        new IOrderedEdgeGremlinQuery<TEdge> ThenBy(Expression<Func<TEdge, object>> projection);
        new IOrderedEdgeGremlinQuery<TEdge> ThenByDescending(Expression<Func<TEdge, object>> projection);
    }

    public partial interface IOrderedEdgeGremlinQuery<TEdge, TAdjacentVertex>
    {
        new IOrderedEdgeGremlinQuery<TEdge, TAdjacentVertex> ThenBy(Expression<Func<TEdge, object>> projection);
        new IOrderedEdgeGremlinQuery<TEdge, TAdjacentVertex> ThenByDescending(Expression<Func<TEdge, object>> projection);
    }

    public partial interface IOrderedEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>
    {
        new IOrderedEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> ThenBy(Expression<Func<TEdge, object>> projection);
        new IOrderedEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> ThenByDescending(Expression<Func<TEdge, object>> projection);
    }

    public partial interface IOrderedInEdgeGremlinQuery<TEdge, TInVertex>
    {
        new IOrderedInEdgeGremlinQuery<TEdge, TInVertex> ThenBy(Expression<Func<TEdge, object>> projection);
        new IOrderedInEdgeGremlinQuery<TEdge, TInVertex> ThenByDescending(Expression<Func<TEdge, object>> projection);
    }

    public partial interface IOrderedOutEdgeGremlinQuery<TEdge, TOutVertex>
    {
        new IOrderedOutEdgeGremlinQuery<TEdge, TOutVertex> ThenBy(Expression<Func<TEdge, object>> projection);
        new IOrderedOutEdgeGremlinQuery<TEdge, TOutVertex> ThenByDescending(Expression<Func<TEdge, object>> projection);
    }

    public partial interface IOrderedVertexPropertyGremlinQuery<TProperty, TValue>
    {
        new IOrderedVertexPropertyGremlinQuery<TProperty, TValue> ThenBy(Expression<Func<TProperty, object>> projection);
        new IOrderedVertexPropertyGremlinQuery<TProperty, TValue> ThenByDescending(Expression<Func<TProperty, object>> projection);
    }

    public partial interface IOrderedVertexPropertyGremlinQuery<TProperty, TValue, TMeta>
    {
        new IOrderedVertexPropertyGremlinQuery<TProperty, TValue, TMeta> ThenBy(Expression<Func<TProperty, object>> projection);
        new IOrderedVertexPropertyGremlinQuery<TProperty, TValue, TMeta> ThenByDescending(Expression<Func<TProperty, object>> projection);
    }

    public partial interface IOrderedPropertyGremlinQuery<TElement>
    {
        new IOrderedPropertyGremlinQuery<TElement> ThenBy(Expression<Func<TElement, object>> projection);
        new IOrderedPropertyGremlinQuery<TElement> ThenByDescending(Expression<Func<TElement, object>> projection);
    }



    public partial interface IElementGremlinQuery<TElement>
    {
        new IValueGremlinQuery<IDictionary<string, TTarget>> ValueMap<TTarget>(params Expression<Func<TElement, TTarget>>[] keys);

        new IValueGremlinQuery<TTarget> Values<TTarget>(); 
        new IValueGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TElement, TTarget>>[] projections);
        new IValueGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TElement, TTarget[]>>[] projections);
    }
    public partial interface IVertexGremlinQuery<TVertex>
    {
        new IValueGremlinQuery<IDictionary<string, TTarget>> ValueMap<TTarget>(params Expression<Func<TVertex, TTarget>>[] keys);

        new IValueGremlinQuery<TTarget> Values<TTarget>(); 
        new IValueGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TVertex, TTarget>>[] projections);
        new IValueGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TVertex, TTarget[]>>[] projections);
    }
    public partial interface IEdgeGremlinQuery<TEdge>
    {
        new IValueGremlinQuery<IDictionary<string, TTarget>> ValueMap<TTarget>(params Expression<Func<TEdge, TTarget>>[] keys);

        new IValueGremlinQuery<TTarget> Values<TTarget>(); 
        new IValueGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TEdge, TTarget>>[] projections);
        new IValueGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TEdge, TTarget[]>>[] projections);
    }
    public partial interface IVertexPropertyGremlinQuery<TProperty, TValue>
    {

        new IValueGremlinQuery<TTarget> Values<TTarget>(); 
        new IValueGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TProperty, TTarget>>[] projections);
        new IValueGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TProperty, TTarget[]>>[] projections);
    }


    public partial interface IGremlinQuery
    {
        new IGremlinQuery<TTarget> OfType<TTarget>();
    }
    public partial interface IElementGremlinQuery
    {
        new IElementGremlinQuery<TTarget> OfType<TTarget>();
    }
    public partial interface IVertexGremlinQuery
    {
        new IVertexGremlinQuery<TTarget> OfType<TTarget>();
    }
    public partial interface IEdgeGremlinQuery
    {
        new IEdgeGremlinQuery<TTarget> OfType<TTarget>();
    }
    

    public partial interface IVertexGremlinQuery<TVertex>
    {
        new IVertexGremlinQuery<TVertex> Update(TVertex element);
        new IVertexGremlinQuery<TTarget> OfType<TTarget>();
        new IVertexGremlinQuery<TVertex> Property<TProjectedValue>(Expression<Func<TVertex, TProjectedValue>> projection, TProjectedValue value);

        new IVertexGremlinQuery<TVertex> Property<TProjectedValue>(Expression<Func<TVertex, TProjectedValue[]>> projection, TProjectedValue value);


new IVertexGremlinQuery<TVertex> Property(string key, object value);
new IVertexGremlinQuery<TVertex> Where(Expression<Func<TVertex, bool>> predicate);
new IVertexGremlinQuery<TVertex> Where<TProjection>(Expression<Func<TVertex, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal);
    }
    public partial interface IEdgeGremlinQuery<TEdge>
    {
        new IEdgeGremlinQuery<TEdge> Update(TEdge element);
        new IEdgeGremlinQuery<TTarget> OfType<TTarget>();
        new IEdgeGremlinQuery<TEdge> Property<TProjectedValue>(Expression<Func<TEdge, TProjectedValue>> projection, TProjectedValue value);



new IEdgeGremlinQuery<TEdge> Property(string key, object value);
new IEdgeGremlinQuery<TEdge> Where(Expression<Func<TEdge, bool>> predicate);
new IEdgeGremlinQuery<TEdge> Where<TProjection>(Expression<Func<TEdge, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal);
    }
    public partial interface IEdgeGremlinQuery<TEdge, TAdjacentVertex>
    {
        new IEdgeGremlinQuery<TEdge, TAdjacentVertex> Update(TEdge element);
        new IEdgeGremlinQuery<TTarget, TAdjacentVertex> OfType<TTarget>();
        new IEdgeGremlinQuery<TEdge, TAdjacentVertex> Property<TProjectedValue>(Expression<Func<TEdge, TProjectedValue>> projection, TProjectedValue value);



new IEdgeGremlinQuery<TEdge, TAdjacentVertex> Property(string key, object value);
new IEdgeGremlinQuery<TEdge, TAdjacentVertex> Where(Expression<Func<TEdge, bool>> predicate);
new IEdgeGremlinQuery<TEdge, TAdjacentVertex> Where<TProjection>(Expression<Func<TEdge, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal);
    }
    public partial interface IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>
    {
        new IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> Update(TEdge element);
        new IEdgeGremlinQuery<TTarget, TOutVertex, TInVertex> OfType<TTarget>();
        new IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> Property<TProjectedValue>(Expression<Func<TEdge, TProjectedValue>> projection, TProjectedValue value);



new IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> Property(string key, object value);
new IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> Where(Expression<Func<TEdge, bool>> predicate);
new IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> Where<TProjection>(Expression<Func<TEdge, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal);
    }
    public partial interface IInEdgeGremlinQuery<TEdge, TInVertex>
    {
        new IInEdgeGremlinQuery<TEdge, TInVertex> Update(TEdge element);
        new IInEdgeGremlinQuery<TTarget, TInVertex> OfType<TTarget>();
        new IInEdgeGremlinQuery<TEdge, TInVertex> Property<TProjectedValue>(Expression<Func<TEdge, TProjectedValue>> projection, TProjectedValue value);



new IInEdgeGremlinQuery<TEdge, TInVertex> Property(string key, object value);
new IInEdgeGremlinQuery<TEdge, TInVertex> Where(Expression<Func<TEdge, bool>> predicate);
new IInEdgeGremlinQuery<TEdge, TInVertex> Where<TProjection>(Expression<Func<TEdge, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal);
    }
    public partial interface IOutEdgeGremlinQuery<TEdge, TOutVertex>
    {
        new IOutEdgeGremlinQuery<TEdge, TOutVertex> Update(TEdge element);
        new IOutEdgeGremlinQuery<TTarget, TOutVertex> OfType<TTarget>();
        new IOutEdgeGremlinQuery<TEdge, TOutVertex> Property<TProjectedValue>(Expression<Func<TEdge, TProjectedValue>> projection, TProjectedValue value);



new IOutEdgeGremlinQuery<TEdge, TOutVertex> Property(string key, object value);
new IOutEdgeGremlinQuery<TEdge, TOutVertex> Where(Expression<Func<TEdge, bool>> predicate);
new IOutEdgeGremlinQuery<TEdge, TOutVertex> Where<TProjection>(Expression<Func<TEdge, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal);
    }
    public partial interface IVertexPropertyGremlinQuery<TProperty, TValue>
    {

new IVertexPropertyGremlinQuery<TProperty, TValue> Property(string key, object value);
new IVertexPropertyGremlinQuery<TProperty, TValue> Where(Expression<Func<TProperty, bool>> predicate);
new IVertexPropertyGremlinQuery<TProperty, TValue> Where<TProjection>(Expression<Func<TProperty, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal);
    }
    public partial interface IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>
    {

new IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Property(string key, object value);
new IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Where(Expression<Func<TProperty, bool>> predicate);
new IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Where<TProjection>(Expression<Func<TProperty, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal);
    }

    public partial interface IGremlinQuery<TElement>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IGremlinQuery<TElement>, StepLabel<IGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IGremlinQuery<TResult> Cast<TResult>();
        new IGremlinQuery<TElement> Coin(double probability);

        new IArrayGremlinQuery<TElement[], IGremlinQuery<TElement>> Fold();
    }
    public partial interface IValueGremlinQuery<TElement>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IValueGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IValueGremlinQuery<TElement>, StepLabel<IValueGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IValueGremlinQuery<TResult> Cast<TResult>();
        new IValueGremlinQuery<TElement> Coin(double probability);

        new IArrayGremlinQuery<TElement[], IValueGremlinQuery<TElement>> Fold();
    }
    public partial interface IArrayGremlinQuery<TArray, TQuery>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IArrayGremlinQuery<TArray, TQuery>, StepLabel<TArray[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IArrayGremlinQuery<TArray, TQuery>, StepLabel<IArrayGremlinQuery<TArray, TQuery>, TArray>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IArrayGremlinQuery<TResult, TQuery> Cast<TResult>();
        new IArrayGremlinQuery<TArray, TQuery> Coin(double probability);

        new IArrayGremlinQuery<TArray[], IArrayGremlinQuery<TArray, TQuery>> Fold();
    }
    public partial interface IElementGremlinQuery<TElement>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IElementGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IElementGremlinQuery<TElement>, StepLabel<IElementGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IElementGremlinQuery<TResult> Cast<TResult>();
        new IElementGremlinQuery<TElement> Coin(double probability);

        new IArrayGremlinQuery<TElement[], IElementGremlinQuery<TElement>> Fold();
    }
    public partial interface IVertexGremlinQuery<TVertex>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IVertexGremlinQuery<TVertex>, StepLabel<TVertex[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IVertexGremlinQuery<TVertex>, StepLabel<IVertexGremlinQuery<TVertex>, TVertex>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IVertexGremlinQuery<TResult> Cast<TResult>();
        new IVertexGremlinQuery<TVertex> Coin(double probability);

        new IArrayGremlinQuery<TVertex[], IVertexGremlinQuery<TVertex>> Fold();
    }
    public partial interface IEdgeGremlinQuery<TEdge>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge>, StepLabel<TEdge[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge>, StepLabel<IEdgeGremlinQuery<TEdge>, TEdge>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IEdgeGremlinQuery<TResult> Cast<TResult>();
        new IEdgeGremlinQuery<TEdge> Coin(double probability);

        new IArrayGremlinQuery<TEdge[], IEdgeGremlinQuery<TEdge>> Fold();
    }
    public partial interface IEdgeGremlinQuery<TEdge, TAdjacentVertex>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, StepLabel<TEdge[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, StepLabel<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, TEdge>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IEdgeGremlinQuery<TResult, TAdjacentVertex> Cast<TResult>();
        new IEdgeGremlinQuery<TEdge, TAdjacentVertex> Coin(double probability);

        new IArrayGremlinQuery<TEdge[], IEdgeGremlinQuery<TEdge, TAdjacentVertex>> Fold();
    }
    public partial interface IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, StepLabel<TEdge[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, StepLabel<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, TEdge>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IEdgeGremlinQuery<TResult, TOutVertex, TInVertex> Cast<TResult>();
        new IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> Coin(double probability);

        new IArrayGremlinQuery<TEdge[], IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>> Fold();
    }
    public partial interface IInEdgeGremlinQuery<TEdge, TInVertex>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IInEdgeGremlinQuery<TEdge, TInVertex>, StepLabel<TEdge[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IInEdgeGremlinQuery<TEdge, TInVertex>, StepLabel<IInEdgeGremlinQuery<TEdge, TInVertex>, TEdge>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IInEdgeGremlinQuery<TResult, TInVertex> Cast<TResult>();
        new IInEdgeGremlinQuery<TEdge, TInVertex> Coin(double probability);

        new IArrayGremlinQuery<TEdge[], IInEdgeGremlinQuery<TEdge, TInVertex>> Fold();
    }
    public partial interface IOutEdgeGremlinQuery<TEdge, TOutVertex>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, StepLabel<TEdge[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, StepLabel<IOutEdgeGremlinQuery<TEdge, TOutVertex>, TEdge>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IOutEdgeGremlinQuery<TResult, TOutVertex> Cast<TResult>();
        new IOutEdgeGremlinQuery<TEdge, TOutVertex> Coin(double probability);

        new IArrayGremlinQuery<TEdge[], IOutEdgeGremlinQuery<TEdge, TOutVertex>> Fold();
    }
    public partial interface IVertexPropertyGremlinQuery<TProperty, TValue>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TProperty, TValue>, StepLabel<TProperty[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TProperty, TValue>, StepLabel<IVertexPropertyGremlinQuery<TProperty, TValue>, TProperty>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IVertexPropertyGremlinQuery<TResult, TValue> Cast<TResult>();
        new IVertexPropertyGremlinQuery<TProperty, TValue> Coin(double probability);

        new IArrayGremlinQuery<TProperty[], IVertexPropertyGremlinQuery<TProperty, TValue>> Fold();
    }
    public partial interface IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, StepLabel<TProperty[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, StepLabel<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, TProperty>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IVertexPropertyGremlinQuery<TResult, TValue, TMeta> Cast<TResult>();
        new IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Coin(double probability);

        new IArrayGremlinQuery<TProperty[], IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>> Fold();
    }
    public partial interface IPropertyGremlinQuery<TElement>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IPropertyGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IPropertyGremlinQuery<TElement>, StepLabel<IPropertyGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IPropertyGremlinQuery<TResult> Cast<TResult>();
        new IPropertyGremlinQuery<TElement> Coin(double probability);

        new IArrayGremlinQuery<TElement[], IPropertyGremlinQuery<TElement>> Fold();
    }
    public partial interface IOrderedGremlinQuery<TElement>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IOrderedGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IOrderedGremlinQuery<TElement>, StepLabel<IOrderedGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IOrderedGremlinQuery<TResult> Cast<TResult>();
        new IOrderedGremlinQuery<TElement> Coin(double probability);

        new IArrayGremlinQuery<TElement[], IGremlinQuery<TElement>> Fold();
    }
    public partial interface IOrderedValueGremlinQuery<TElement>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IOrderedValueGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IOrderedValueGremlinQuery<TElement>, StepLabel<IOrderedValueGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IOrderedValueGremlinQuery<TResult> Cast<TResult>();
        new IOrderedValueGremlinQuery<TElement> Coin(double probability);

        new IArrayGremlinQuery<TElement[], IValueGremlinQuery<TElement>> Fold();
    }
    public partial interface IOrderedArrayGremlinQuery<TArray, TQuery>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IOrderedArrayGremlinQuery<TArray, TQuery>, StepLabel<TArray[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IOrderedArrayGremlinQuery<TArray, TQuery>, StepLabel<IOrderedArrayGremlinQuery<TArray, TQuery>, TArray>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IOrderedArrayGremlinQuery<TResult, TQuery> Cast<TResult>();
        new IOrderedArrayGremlinQuery<TArray, TQuery> Coin(double probability);

        new IArrayGremlinQuery<TArray[], IArrayGremlinQuery<TArray, TQuery>> Fold();
    }
    public partial interface IOrderedElementGremlinQuery<TElement>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IOrderedElementGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IOrderedElementGremlinQuery<TElement>, StepLabel<IOrderedElementGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IOrderedElementGremlinQuery<TResult> Cast<TResult>();
        new IOrderedElementGremlinQuery<TElement> Coin(double probability);

        new IArrayGremlinQuery<TElement[], IElementGremlinQuery<TElement>> Fold();
    }
    public partial interface IOrderedVertexGremlinQuery<TVertex>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IOrderedVertexGremlinQuery<TVertex>, StepLabel<TVertex[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IOrderedVertexGremlinQuery<TVertex>, StepLabel<IOrderedVertexGremlinQuery<TVertex>, TVertex>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IOrderedVertexGremlinQuery<TResult> Cast<TResult>();
        new IOrderedVertexGremlinQuery<TVertex> Coin(double probability);

        new IArrayGremlinQuery<TVertex[], IVertexGremlinQuery<TVertex>> Fold();
    }
    public partial interface IOrderedEdgeGremlinQuery<TEdge>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IOrderedEdgeGremlinQuery<TEdge>, StepLabel<TEdge[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IOrderedEdgeGremlinQuery<TEdge>, StepLabel<IOrderedEdgeGremlinQuery<TEdge>, TEdge>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IOrderedEdgeGremlinQuery<TResult> Cast<TResult>();
        new IOrderedEdgeGremlinQuery<TEdge> Coin(double probability);

        new IArrayGremlinQuery<TEdge[], IEdgeGremlinQuery<TEdge>> Fold();
    }
    public partial interface IOrderedEdgeGremlinQuery<TEdge, TAdjacentVertex>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IOrderedEdgeGremlinQuery<TEdge, TAdjacentVertex>, StepLabel<TEdge[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IOrderedEdgeGremlinQuery<TEdge, TAdjacentVertex>, StepLabel<IOrderedEdgeGremlinQuery<TEdge, TAdjacentVertex>, TEdge>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IOrderedEdgeGremlinQuery<TResult, TAdjacentVertex> Cast<TResult>();
        new IOrderedEdgeGremlinQuery<TEdge, TAdjacentVertex> Coin(double probability);

        new IArrayGremlinQuery<TEdge[], IEdgeGremlinQuery<TEdge, TAdjacentVertex>> Fold();
    }
    public partial interface IOrderedEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IOrderedEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, StepLabel<TEdge[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IOrderedEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, StepLabel<IOrderedEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, TEdge>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IOrderedEdgeGremlinQuery<TResult, TOutVertex, TInVertex> Cast<TResult>();
        new IOrderedEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> Coin(double probability);

        new IArrayGremlinQuery<TEdge[], IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>> Fold();
    }
    public partial interface IOrderedInEdgeGremlinQuery<TEdge, TInVertex>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IOrderedInEdgeGremlinQuery<TEdge, TInVertex>, StepLabel<TEdge[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IOrderedInEdgeGremlinQuery<TEdge, TInVertex>, StepLabel<IOrderedInEdgeGremlinQuery<TEdge, TInVertex>, TEdge>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IOrderedInEdgeGremlinQuery<TResult, TInVertex> Cast<TResult>();
        new IOrderedInEdgeGremlinQuery<TEdge, TInVertex> Coin(double probability);

        new IArrayGremlinQuery<TEdge[], IInEdgeGremlinQuery<TEdge, TInVertex>> Fold();
    }
    public partial interface IOrderedOutEdgeGremlinQuery<TEdge, TOutVertex>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IOrderedOutEdgeGremlinQuery<TEdge, TOutVertex>, StepLabel<TEdge[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IOrderedOutEdgeGremlinQuery<TEdge, TOutVertex>, StepLabel<IOrderedOutEdgeGremlinQuery<TEdge, TOutVertex>, TEdge>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IOrderedOutEdgeGremlinQuery<TResult, TOutVertex> Cast<TResult>();
        new IOrderedOutEdgeGremlinQuery<TEdge, TOutVertex> Coin(double probability);

        new IArrayGremlinQuery<TEdge[], IOutEdgeGremlinQuery<TEdge, TOutVertex>> Fold();
    }
    public partial interface IOrderedVertexPropertyGremlinQuery<TProperty, TValue>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IOrderedVertexPropertyGremlinQuery<TProperty, TValue>, StepLabel<TProperty[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IOrderedVertexPropertyGremlinQuery<TProperty, TValue>, StepLabel<IOrderedVertexPropertyGremlinQuery<TProperty, TValue>, TProperty>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IOrderedVertexPropertyGremlinQuery<TResult, TValue> Cast<TResult>();
        new IOrderedVertexPropertyGremlinQuery<TProperty, TValue> Coin(double probability);

        new IArrayGremlinQuery<TProperty[], IVertexPropertyGremlinQuery<TProperty, TValue>> Fold();
    }
    public partial interface IOrderedVertexPropertyGremlinQuery<TProperty, TValue, TMeta>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IOrderedVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, StepLabel<TProperty[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IOrderedVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, StepLabel<IOrderedVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, TProperty>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IOrderedVertexPropertyGremlinQuery<TResult, TValue, TMeta> Cast<TResult>();
        new IOrderedVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Coin(double probability);

        new IArrayGremlinQuery<TProperty[], IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>> Fold();
    }
    public partial interface IOrderedPropertyGremlinQuery<TElement>
    {
        TTargetQuery Aggregate<TTargetQuery>(Func<IOrderedPropertyGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;
        TTargetQuery As<TTargetQuery>(Func<IOrderedPropertyGremlinQuery<TElement>, StepLabel<IOrderedPropertyGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) where TTargetQuery : IGremlinQuery;

        new IOrderedPropertyGremlinQuery<TResult> Cast<TResult>();
        new IOrderedPropertyGremlinQuery<TElement> Coin(double probability);

        new IArrayGremlinQuery<TElement[], IPropertyGremlinQuery<TElement>> Fold();
    }

    public partial interface IGremlinQuery
    {
        new IGremlinQuery Where(Func<IGremlinQuery, IGremlinQuery> filterTraversal);
    }
    public partial interface IElementGremlinQuery
    {
        new IElementGremlinQuery Where(Func<IElementGremlinQuery, IGremlinQuery> filterTraversal);
    }
    public partial interface IVertexGremlinQuery
    {
        new IVertexGremlinQuery Where(Func<IVertexGremlinQuery, IGremlinQuery> filterTraversal);
    }
    public partial interface IEdgeGremlinQuery
    {
        new IEdgeGremlinQuery Where(Func<IEdgeGremlinQuery, IGremlinQuery> filterTraversal);
    }
    public partial interface IGremlinQuery<TElement>
    {
        new IGremlinQuery<TElement> Where(Func<IGremlinQuery<TElement>, IGremlinQuery> filterTraversal);
    }
    public partial interface IValueGremlinQuery<TElement>
    {
        new IValueGremlinQuery<TElement> Where(Func<IValueGremlinQuery<TElement>, IGremlinQuery> filterTraversal);
    }
    public partial interface IArrayGremlinQuery<TArray, TQuery>
    {
        new IArrayGremlinQuery<TArray, TQuery> Where(Func<IArrayGremlinQuery<TArray, TQuery>, IGremlinQuery> filterTraversal);
    }
    public partial interface IElementGremlinQuery<TElement>
    {
        new IElementGremlinQuery<TElement> Where(Func<IElementGremlinQuery<TElement>, IGremlinQuery> filterTraversal);
    }
    public partial interface IVertexGremlinQuery<TVertex>
    {
        new IVertexGremlinQuery<TVertex> Where(Func<IVertexGremlinQuery<TVertex>, IGremlinQuery> filterTraversal);
    }
    public partial interface IEdgeGremlinQuery<TEdge>
    {
        new IEdgeGremlinQuery<TEdge> Where(Func<IEdgeGremlinQuery<TEdge>, IGremlinQuery> filterTraversal);
    }
    public partial interface IEdgeGremlinQuery<TEdge, TAdjacentVertex>
    {
        new IEdgeGremlinQuery<TEdge, TAdjacentVertex> Where(Func<IEdgeGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> filterTraversal);
    }
    public partial interface IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>
    {
        new IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> Where(Func<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, IGremlinQuery> filterTraversal);
    }
    public partial interface IInEdgeGremlinQuery<TEdge, TInVertex>
    {
        new IInEdgeGremlinQuery<TEdge, TInVertex> Where(Func<IInEdgeGremlinQuery<TEdge, TInVertex>, IGremlinQuery> filterTraversal);
    }
    public partial interface IOutEdgeGremlinQuery<TEdge, TOutVertex>
    {
        new IOutEdgeGremlinQuery<TEdge, TOutVertex> Where(Func<IOutEdgeGremlinQuery<TEdge, TOutVertex>, IGremlinQuery> filterTraversal);
    }
    public partial interface IVertexPropertyGremlinQuery<TProperty, TValue>
    {
        new IVertexPropertyGremlinQuery<TProperty, TValue> Where(Func<IVertexPropertyGremlinQuery<TProperty, TValue>, IGremlinQuery> filterTraversal);
    }
    public partial interface IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>
    {
        new IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Where(Func<IVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, IGremlinQuery> filterTraversal);
    }
    public partial interface IPropertyGremlinQuery<TElement>
    {
        new IPropertyGremlinQuery<TElement> Where(Func<IPropertyGremlinQuery<TElement>, IGremlinQuery> filterTraversal);
    }
    public partial interface IOrderedGremlinQuery
    {
        new IGremlinQuery Where(Func<IOrderedGremlinQuery, IGremlinQuery> filterTraversal);
    }
    public partial interface IOrderedElementGremlinQuery
    {
        new IElementGremlinQuery Where(Func<IOrderedElementGremlinQuery, IGremlinQuery> filterTraversal);
    }
    public partial interface IOrderedVertexGremlinQuery
    {
        new IVertexGremlinQuery Where(Func<IOrderedVertexGremlinQuery, IGremlinQuery> filterTraversal);
    }
    public partial interface IOrderedEdgeGremlinQuery
    {
        new IEdgeGremlinQuery Where(Func<IOrderedEdgeGremlinQuery, IGremlinQuery> filterTraversal);
    }
    public partial interface IOrderedGremlinQuery<TElement>
    {
        new IGremlinQuery<TElement> Where(Func<IOrderedGremlinQuery<TElement>, IGremlinQuery> filterTraversal);
    }
    public partial interface IOrderedValueGremlinQuery<TElement>
    {
        new IValueGremlinQuery<TElement> Where(Func<IOrderedValueGremlinQuery<TElement>, IGremlinQuery> filterTraversal);
    }
    public partial interface IOrderedArrayGremlinQuery<TArray, TQuery>
    {
        new IArrayGremlinQuery<TArray, TQuery> Where(Func<IOrderedArrayGremlinQuery<TArray, TQuery>, IGremlinQuery> filterTraversal);
    }
    public partial interface IOrderedElementGremlinQuery<TElement>
    {
        new IElementGremlinQuery<TElement> Where(Func<IOrderedElementGremlinQuery<TElement>, IGremlinQuery> filterTraversal);
    }
    public partial interface IOrderedVertexGremlinQuery<TVertex>
    {
        new IVertexGremlinQuery<TVertex> Where(Func<IOrderedVertexGremlinQuery<TVertex>, IGremlinQuery> filterTraversal);
    }
    public partial interface IOrderedEdgeGremlinQuery<TEdge>
    {
        new IEdgeGremlinQuery<TEdge> Where(Func<IOrderedEdgeGremlinQuery<TEdge>, IGremlinQuery> filterTraversal);
    }
    public partial interface IOrderedEdgeGremlinQuery<TEdge, TAdjacentVertex>
    {
        new IEdgeGremlinQuery<TEdge, TAdjacentVertex> Where(Func<IOrderedEdgeGremlinQuery<TEdge, TAdjacentVertex>, IGremlinQuery> filterTraversal);
    }
    public partial interface IOrderedEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>
    {
        new IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> Where(Func<IOrderedEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>, IGremlinQuery> filterTraversal);
    }
    public partial interface IOrderedInEdgeGremlinQuery<TEdge, TInVertex>
    {
        new IInEdgeGremlinQuery<TEdge, TInVertex> Where(Func<IOrderedInEdgeGremlinQuery<TEdge, TInVertex>, IGremlinQuery> filterTraversal);
    }
    public partial interface IOrderedOutEdgeGremlinQuery<TEdge, TOutVertex>
    {
        new IOutEdgeGremlinQuery<TEdge, TOutVertex> Where(Func<IOrderedOutEdgeGremlinQuery<TEdge, TOutVertex>, IGremlinQuery> filterTraversal);
    }
    public partial interface IOrderedVertexPropertyGremlinQuery<TProperty, TValue>
    {
        new IVertexPropertyGremlinQuery<TProperty, TValue> Where(Func<IOrderedVertexPropertyGremlinQuery<TProperty, TValue>, IGremlinQuery> filterTraversal);
    }
    public partial interface IOrderedVertexPropertyGremlinQuery<TProperty, TValue, TMeta>
    {
        new IVertexPropertyGremlinQuery<TProperty, TValue, TMeta> Where(Func<IOrderedVertexPropertyGremlinQuery<TProperty, TValue, TMeta>, IGremlinQuery> filterTraversal);
    }
    public partial interface IOrderedPropertyGremlinQuery<TElement>
    {
        new IPropertyGremlinQuery<TElement> Where(Func<IOrderedPropertyGremlinQuery<TElement>, IGremlinQuery> filterTraversal);
    }

public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2> : IProjectResult<(TItem1, TItem2)>
        where TSourceQuery : IGremlinQuery
    {
            IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3> By<TItem3>(Func<TSourceQuery, IGremlinQuery<TItem3>> projection);
        }
public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3> : IProjectResult<(TItem1, TItem2, TItem3)>
        where TSourceQuery : IGremlinQuery
    {
            IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4> By<TItem4>(Func<TSourceQuery, IGremlinQuery<TItem4>> projection);
        }
public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4> : IProjectResult<(TItem1, TItem2, TItem3, TItem4)>
        where TSourceQuery : IGremlinQuery
    {
            IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5> By<TItem5>(Func<TSourceQuery, IGremlinQuery<TItem5>> projection);
        }
public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5> : IProjectResult<(TItem1, TItem2, TItem3, TItem4, TItem5)>
        where TSourceQuery : IGremlinQuery
    {
            IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6> By<TItem6>(Func<TSourceQuery, IGremlinQuery<TItem6>> projection);
        }
public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6> : IProjectResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6)>
        where TSourceQuery : IGremlinQuery
    {
            IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7> By<TItem7>(Func<TSourceQuery, IGremlinQuery<TItem7>> projection);
        }
public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7> : IProjectResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7)>
        where TSourceQuery : IGremlinQuery
    {
            IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8> By<TItem8>(Func<TSourceQuery, IGremlinQuery<TItem8>> projection);
        }
public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8> : IProjectResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8)>
        where TSourceQuery : IGremlinQuery
    {
            IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9> By<TItem9>(Func<TSourceQuery, IGremlinQuery<TItem9>> projection);
        }
public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9> : IProjectResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9)>
        where TSourceQuery : IGremlinQuery
    {
            IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10> By<TItem10>(Func<TSourceQuery, IGremlinQuery<TItem10>> projection);
        }
public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10> : IProjectResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10)>
        where TSourceQuery : IGremlinQuery
    {
            IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11> By<TItem11>(Func<TSourceQuery, IGremlinQuery<TItem11>> projection);
        }
public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11> : IProjectResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11)>
        where TSourceQuery : IGremlinQuery
    {
            IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12> By<TItem12>(Func<TSourceQuery, IGremlinQuery<TItem12>> projection);
        }
public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12> : IProjectResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12)>
        where TSourceQuery : IGremlinQuery
    {
            IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13> By<TItem13>(Func<TSourceQuery, IGremlinQuery<TItem13>> projection);
        }
public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13> : IProjectResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13)>
        where TSourceQuery : IGremlinQuery
    {
            IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14> By<TItem14>(Func<TSourceQuery, IGremlinQuery<TItem14>> projection);
        }
public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14> : IProjectResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14)>
        where TSourceQuery : IGremlinQuery
    {
            IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15> By<TItem15>(Func<TSourceQuery, IGremlinQuery<TItem15>> projection);
        }
public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15> : IProjectResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15)>
        where TSourceQuery : IGremlinQuery
    {
            IProjectTupleBuilder<TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16> By<TItem16>(Func<TSourceQuery, IGremlinQuery<TItem16>> projection);
        }
public interface IProjectTupleBuilder<out TSourceQuery, TElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16> : IProjectResult<(TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16)>
        where TSourceQuery : IGremlinQuery
    {
        }

}

#pragma warning restore CS0109 // Member does not hide an inherited member; new keyword is not required
