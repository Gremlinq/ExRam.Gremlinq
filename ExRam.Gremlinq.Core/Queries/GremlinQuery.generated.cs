#pragma warning disable CS0109 // Member does not hide an inherited member; new keyword is not required
using System;
using NullGuard;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace ExRam.Gremlinq.Core
{
    partial class GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>
    {
        IGremlinQuery IGremlinQuery.And(params Func<IGremlinQuery, IGremlinQuery>[] andTraversals) => And(andTraversals);
        IGremlinQuery IGremlinQuery.As(params StepLabel[] stepLabels) => As(stepLabels);

        IGremlinQuery IGremlinQuery.Barrier() => Barrier();

        TTargetQuery IGremlinQuery.Choose<TTargetQuery>(Func<IGremlinQuery, IGremlinQuery> traversalPredicate, Func<IGremlinQuery, TTargetQuery> trueChoice, Func<IGremlinQuery, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);
        TTargetQuery IGremlinQuery.Choose<TTargetQuery>(Func<IGremlinQuery, IGremlinQuery> traversalPredicate, Func<IGremlinQuery, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);

        TTargetQuery IGremlinQuery.Coalesce<TTargetQuery>(params Func<IGremlinQuery, TTargetQuery>[] traversals) => Coalesce(traversals);

        IGremlinQuery IGremlinQuery.Dedup() => Dedup();

        IGremlinQuery IGremlinQuery.Emit() => Emit();

        IGremlinQuery IGremlinQuery.Where(string lambda) => Where(lambda);

        TTargetQuery IGremlinQuery.FlatMap<TTargetQuery>(Func<IGremlinQuery, TTargetQuery> mapping) => FlatMap(mapping);

        IGremlinQuery IGremlinQuery.Identity() => Identity();

        IGremlinQuery IGremlinQuery.Limit(long count) => Limit(count);
        IGremlinQuery IGremlinQuery.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IGremlinQuery.Local<TTargetQuery>(Func<IGremlinQuery , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IGremlinQuery.Map<TTargetQuery>(Func<IGremlinQuery, TTargetQuery> mapping) => Map(mapping);
        
        IGremlinQuery IGremlinQuery.Not(Func<IGremlinQuery, IGremlinQuery> notTraversal) => Not(notTraversal);

        TTargetQuery IGremlinQuery.Optional<TTargetQuery>(Func<IGremlinQuery, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);
        IGremlinQuery IGremlinQuery.Or(params Func<IGremlinQuery, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IGremlinQuery IGremlinQuery.Range(long low, long high) => Range(low, high);

        TTargetQuery IGremlinQuery.Repeat<TTargetQuery>(Func<IGremlinQuery, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);
        TTargetQuery IGremlinQuery.RepeatUntil<TTargetQuery>(Func<IGremlinQuery, TTargetQuery> repeatTraversal, Func<IGremlinQuery, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

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

        TTargetQuery IElementGremlinQuery.Coalesce<TTargetQuery>(params Func<IElementGremlinQuery, TTargetQuery>[] traversals) => Coalesce(traversals);

        IElementGremlinQuery IElementGremlinQuery.Dedup() => Dedup();

        IElementGremlinQuery IElementGremlinQuery.Emit() => Emit();

        IElementGremlinQuery IElementGremlinQuery.Where(string lambda) => Where(lambda);

        TTargetQuery IElementGremlinQuery.FlatMap<TTargetQuery>(Func<IElementGremlinQuery, TTargetQuery> mapping) => FlatMap(mapping);

        IElementGremlinQuery IElementGremlinQuery.Identity() => Identity();

        IElementGremlinQuery IElementGremlinQuery.Limit(long count) => Limit(count);
        IElementGremlinQuery IElementGremlinQuery.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IElementGremlinQuery.Local<TTargetQuery>(Func<IElementGremlinQuery , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IElementGremlinQuery.Map<TTargetQuery>(Func<IElementGremlinQuery, TTargetQuery> mapping) => Map(mapping);
        
        IElementGremlinQuery IElementGremlinQuery.Not(Func<IElementGremlinQuery, IGremlinQuery> notTraversal) => Not(notTraversal);

        TTargetQuery IElementGremlinQuery.Optional<TTargetQuery>(Func<IElementGremlinQuery, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);
        IElementGremlinQuery IElementGremlinQuery.Or(params Func<IElementGremlinQuery, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IElementGremlinQuery IElementGremlinQuery.Range(long low, long high) => Range(low, high);

        TTargetQuery IElementGremlinQuery.Repeat<TTargetQuery>(Func<IElementGremlinQuery, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);
        TTargetQuery IElementGremlinQuery.RepeatUntil<TTargetQuery>(Func<IElementGremlinQuery, TTargetQuery> repeatTraversal, Func<IElementGremlinQuery, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

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

        TTargetQuery IVertexGremlinQuery.Coalesce<TTargetQuery>(params Func<IVertexGremlinQuery, TTargetQuery>[] traversals) => Coalesce(traversals);

        IVertexGremlinQuery IVertexGremlinQuery.Dedup() => Dedup();

        IVertexGremlinQuery IVertexGremlinQuery.Emit() => Emit();

        IVertexGremlinQuery IVertexGremlinQuery.Where(string lambda) => Where(lambda);

        TTargetQuery IVertexGremlinQuery.FlatMap<TTargetQuery>(Func<IVertexGremlinQuery, TTargetQuery> mapping) => FlatMap(mapping);

        IVertexGremlinQuery IVertexGremlinQuery.Identity() => Identity();

        IVertexGremlinQuery IVertexGremlinQuery.Limit(long count) => Limit(count);
        IVertexGremlinQuery IVertexGremlinQuery.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IVertexGremlinQuery.Local<TTargetQuery>(Func<IVertexGremlinQuery , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IVertexGremlinQuery.Map<TTargetQuery>(Func<IVertexGremlinQuery, TTargetQuery> mapping) => Map(mapping);
        
        IVertexGremlinQuery IVertexGremlinQuery.Not(Func<IVertexGremlinQuery, IGremlinQuery> notTraversal) => Not(notTraversal);

        TTargetQuery IVertexGremlinQuery.Optional<TTargetQuery>(Func<IVertexGremlinQuery, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);
        IVertexGremlinQuery IVertexGremlinQuery.Or(params Func<IVertexGremlinQuery, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IVertexGremlinQuery IVertexGremlinQuery.Range(long low, long high) => Range(low, high);

        TTargetQuery IVertexGremlinQuery.Repeat<TTargetQuery>(Func<IVertexGremlinQuery, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);
        TTargetQuery IVertexGremlinQuery.RepeatUntil<TTargetQuery>(Func<IVertexGremlinQuery, TTargetQuery> repeatTraversal, Func<IVertexGremlinQuery, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

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

        TTargetQuery IEdgeGremlinQuery.Coalesce<TTargetQuery>(params Func<IEdgeGremlinQuery, TTargetQuery>[] traversals) => Coalesce(traversals);

        IEdgeGremlinQuery IEdgeGremlinQuery.Dedup() => Dedup();

        IEdgeGremlinQuery IEdgeGremlinQuery.Emit() => Emit();

        IEdgeGremlinQuery IEdgeGremlinQuery.Where(string lambda) => Where(lambda);

        TTargetQuery IEdgeGremlinQuery.FlatMap<TTargetQuery>(Func<IEdgeGremlinQuery, TTargetQuery> mapping) => FlatMap(mapping);

        IEdgeGremlinQuery IEdgeGremlinQuery.Identity() => Identity();

        IEdgeGremlinQuery IEdgeGremlinQuery.Limit(long count) => Limit(count);
        IEdgeGremlinQuery IEdgeGremlinQuery.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IEdgeGremlinQuery.Local<TTargetQuery>(Func<IEdgeGremlinQuery , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IEdgeGremlinQuery.Map<TTargetQuery>(Func<IEdgeGremlinQuery, TTargetQuery> mapping) => Map(mapping);
        
        IEdgeGremlinQuery IEdgeGremlinQuery.Not(Func<IEdgeGremlinQuery, IGremlinQuery> notTraversal) => Not(notTraversal);

        TTargetQuery IEdgeGremlinQuery.Optional<TTargetQuery>(Func<IEdgeGremlinQuery, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);
        IEdgeGremlinQuery IEdgeGremlinQuery.Or(params Func<IEdgeGremlinQuery, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IEdgeGremlinQuery IEdgeGremlinQuery.Range(long low, long high) => Range(low, high);

        TTargetQuery IEdgeGremlinQuery.Repeat<TTargetQuery>(Func<IEdgeGremlinQuery, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);
        TTargetQuery IEdgeGremlinQuery.RepeatUntil<TTargetQuery>(Func<IEdgeGremlinQuery, TTargetQuery> repeatTraversal, Func<IEdgeGremlinQuery, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

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

        TTargetQuery IGremlinQuery<TElement>.Coalesce<TTargetQuery>(params Func<IGremlinQuery<TElement>, TTargetQuery>[] traversals) => Coalesce(traversals);

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Dedup() => Dedup();

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Emit() => Emit();

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Where(string lambda) => Where(lambda);

        TTargetQuery IGremlinQuery<TElement>.FlatMap<TTargetQuery>(Func<IGremlinQuery<TElement>, TTargetQuery> mapping) => FlatMap(mapping);

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Identity() => Identity();

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Limit(long count) => Limit(count);
        IGremlinQuery<TElement> IGremlinQuery<TElement>.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IGremlinQuery<TElement>.Local<TTargetQuery>(Func<IGremlinQuery<TElement> , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IGremlinQuery<TElement>.Map<TTargetQuery>(Func<IGremlinQuery<TElement>, TTargetQuery> mapping) => Map(mapping);
        
        IGremlinQuery<TElement> IGremlinQuery<TElement>.Not(Func<IGremlinQuery<TElement>, IGremlinQuery> notTraversal) => Not(notTraversal);

        TTargetQuery IGremlinQuery<TElement>.Optional<TTargetQuery>(Func<IGremlinQuery<TElement>, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);
        IGremlinQuery<TElement> IGremlinQuery<TElement>.Or(params Func<IGremlinQuery<TElement>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Range(long low, long high) => Range(low, high);

        TTargetQuery IGremlinQuery<TElement>.Repeat<TTargetQuery>(Func<IGremlinQuery<TElement>, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);
        TTargetQuery IGremlinQuery<TElement>.RepeatUntil<TTargetQuery>(Func<IGremlinQuery<TElement>, TTargetQuery> repeatTraversal, Func<IGremlinQuery<TElement>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

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

        TTargetQuery IValueGremlinQuery<TElement>.Coalesce<TTargetQuery>(params Func<IValueGremlinQuery<TElement>, TTargetQuery>[] traversals) => Coalesce(traversals);

        IValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.Dedup() => Dedup();

        IValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.Emit() => Emit();

        IValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.Where(string lambda) => Where(lambda);

        TTargetQuery IValueGremlinQuery<TElement>.FlatMap<TTargetQuery>(Func<IValueGremlinQuery<TElement>, TTargetQuery> mapping) => FlatMap(mapping);

        IValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.Identity() => Identity();

        IValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.Limit(long count) => Limit(count);
        IValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IValueGremlinQuery<TElement>.Local<TTargetQuery>(Func<IValueGremlinQuery<TElement> , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IValueGremlinQuery<TElement>.Map<TTargetQuery>(Func<IValueGremlinQuery<TElement>, TTargetQuery> mapping) => Map(mapping);
        
        IValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.Not(Func<IValueGremlinQuery<TElement>, IGremlinQuery> notTraversal) => Not(notTraversal);

        TTargetQuery IValueGremlinQuery<TElement>.Optional<TTargetQuery>(Func<IValueGremlinQuery<TElement>, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);
        IValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.Or(params Func<IValueGremlinQuery<TElement>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.Range(long low, long high) => Range(low, high);

        TTargetQuery IValueGremlinQuery<TElement>.Repeat<TTargetQuery>(Func<IValueGremlinQuery<TElement>, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);
        TTargetQuery IValueGremlinQuery<TElement>.RepeatUntil<TTargetQuery>(Func<IValueGremlinQuery<TElement>, TTargetQuery> repeatTraversal, Func<IValueGremlinQuery<TElement>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

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

        TTargetQuery IArrayGremlinQuery<TElement, TFoldedQuery>.Coalesce<TTargetQuery>(params Func<IArrayGremlinQuery<TElement, TFoldedQuery>, TTargetQuery>[] traversals) => Coalesce(traversals);

        IArrayGremlinQuery<TElement, TFoldedQuery> IArrayGremlinQuery<TElement, TFoldedQuery>.Dedup() => Dedup();

        IArrayGremlinQuery<TElement, TFoldedQuery> IArrayGremlinQuery<TElement, TFoldedQuery>.Emit() => Emit();

        IArrayGremlinQuery<TElement, TFoldedQuery> IArrayGremlinQuery<TElement, TFoldedQuery>.Where(string lambda) => Where(lambda);

        TTargetQuery IArrayGremlinQuery<TElement, TFoldedQuery>.FlatMap<TTargetQuery>(Func<IArrayGremlinQuery<TElement, TFoldedQuery>, TTargetQuery> mapping) => FlatMap(mapping);

        IArrayGremlinQuery<TElement, TFoldedQuery> IArrayGremlinQuery<TElement, TFoldedQuery>.Identity() => Identity();

        IArrayGremlinQuery<TElement, TFoldedQuery> IArrayGremlinQuery<TElement, TFoldedQuery>.Limit(long count) => Limit(count);
        IArrayGremlinQuery<TElement, TFoldedQuery> IArrayGremlinQuery<TElement, TFoldedQuery>.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IArrayGremlinQuery<TElement, TFoldedQuery>.Local<TTargetQuery>(Func<IArrayGremlinQuery<TElement, TFoldedQuery> , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IArrayGremlinQuery<TElement, TFoldedQuery>.Map<TTargetQuery>(Func<IArrayGremlinQuery<TElement, TFoldedQuery>, TTargetQuery> mapping) => Map(mapping);
        
        IArrayGremlinQuery<TElement, TFoldedQuery> IArrayGremlinQuery<TElement, TFoldedQuery>.Not(Func<IArrayGremlinQuery<TElement, TFoldedQuery>, IGremlinQuery> notTraversal) => Not(notTraversal);

        TTargetQuery IArrayGremlinQuery<TElement, TFoldedQuery>.Optional<TTargetQuery>(Func<IArrayGremlinQuery<TElement, TFoldedQuery>, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);
        IArrayGremlinQuery<TElement, TFoldedQuery> IArrayGremlinQuery<TElement, TFoldedQuery>.Or(params Func<IArrayGremlinQuery<TElement, TFoldedQuery>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IArrayGremlinQuery<TElement, TFoldedQuery> IArrayGremlinQuery<TElement, TFoldedQuery>.Range(long low, long high) => Range(low, high);

        TTargetQuery IArrayGremlinQuery<TElement, TFoldedQuery>.Repeat<TTargetQuery>(Func<IArrayGremlinQuery<TElement, TFoldedQuery>, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);
        TTargetQuery IArrayGremlinQuery<TElement, TFoldedQuery>.RepeatUntil<TTargetQuery>(Func<IArrayGremlinQuery<TElement, TFoldedQuery>, TTargetQuery> repeatTraversal, Func<IArrayGremlinQuery<TElement, TFoldedQuery>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

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

        TTargetQuery IElementGremlinQuery<TElement>.Coalesce<TTargetQuery>(params Func<IElementGremlinQuery<TElement>, TTargetQuery>[] traversals) => Coalesce(traversals);

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Dedup() => Dedup();

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Emit() => Emit();

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Where(string lambda) => Where(lambda);

        TTargetQuery IElementGremlinQuery<TElement>.FlatMap<TTargetQuery>(Func<IElementGremlinQuery<TElement>, TTargetQuery> mapping) => FlatMap(mapping);

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Identity() => Identity();

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Limit(long count) => Limit(count);
        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IElementGremlinQuery<TElement>.Local<TTargetQuery>(Func<IElementGremlinQuery<TElement> , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IElementGremlinQuery<TElement>.Map<TTargetQuery>(Func<IElementGremlinQuery<TElement>, TTargetQuery> mapping) => Map(mapping);
        
        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Not(Func<IElementGremlinQuery<TElement>, IGremlinQuery> notTraversal) => Not(notTraversal);

        TTargetQuery IElementGremlinQuery<TElement>.Optional<TTargetQuery>(Func<IElementGremlinQuery<TElement>, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);
        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Or(params Func<IElementGremlinQuery<TElement>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Range(long low, long high) => Range(low, high);

        TTargetQuery IElementGremlinQuery<TElement>.Repeat<TTargetQuery>(Func<IElementGremlinQuery<TElement>, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);
        TTargetQuery IElementGremlinQuery<TElement>.RepeatUntil<TTargetQuery>(Func<IElementGremlinQuery<TElement>, TTargetQuery> repeatTraversal, Func<IElementGremlinQuery<TElement>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

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

        TTargetQuery IVertexGremlinQuery<TElement>.Coalesce<TTargetQuery>(params Func<IVertexGremlinQuery<TElement>, TTargetQuery>[] traversals) => Coalesce(traversals);

        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Dedup() => Dedup();

        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Emit() => Emit();

        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Where(string lambda) => Where(lambda);

        TTargetQuery IVertexGremlinQuery<TElement>.FlatMap<TTargetQuery>(Func<IVertexGremlinQuery<TElement>, TTargetQuery> mapping) => FlatMap(mapping);

        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Identity() => Identity();

        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Limit(long count) => Limit(count);
        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IVertexGremlinQuery<TElement>.Local<TTargetQuery>(Func<IVertexGremlinQuery<TElement> , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IVertexGremlinQuery<TElement>.Map<TTargetQuery>(Func<IVertexGremlinQuery<TElement>, TTargetQuery> mapping) => Map(mapping);
        
        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Not(Func<IVertexGremlinQuery<TElement>, IGremlinQuery> notTraversal) => Not(notTraversal);

        TTargetQuery IVertexGremlinQuery<TElement>.Optional<TTargetQuery>(Func<IVertexGremlinQuery<TElement>, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);
        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Or(params Func<IVertexGremlinQuery<TElement>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Range(long low, long high) => Range(low, high);

        TTargetQuery IVertexGremlinQuery<TElement>.Repeat<TTargetQuery>(Func<IVertexGremlinQuery<TElement>, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);
        TTargetQuery IVertexGremlinQuery<TElement>.RepeatUntil<TTargetQuery>(Func<IVertexGremlinQuery<TElement>, TTargetQuery> repeatTraversal, Func<IVertexGremlinQuery<TElement>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

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

        TTargetQuery IEdgeGremlinQuery<TElement>.Coalesce<TTargetQuery>(params Func<IEdgeGremlinQuery<TElement>, TTargetQuery>[] traversals) => Coalesce(traversals);

        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Dedup() => Dedup();

        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Emit() => Emit();

        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Where(string lambda) => Where(lambda);

        TTargetQuery IEdgeGremlinQuery<TElement>.FlatMap<TTargetQuery>(Func<IEdgeGremlinQuery<TElement>, TTargetQuery> mapping) => FlatMap(mapping);

        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Identity() => Identity();

        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Limit(long count) => Limit(count);
        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IEdgeGremlinQuery<TElement>.Local<TTargetQuery>(Func<IEdgeGremlinQuery<TElement> , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IEdgeGremlinQuery<TElement>.Map<TTargetQuery>(Func<IEdgeGremlinQuery<TElement>, TTargetQuery> mapping) => Map(mapping);
        
        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Not(Func<IEdgeGremlinQuery<TElement>, IGremlinQuery> notTraversal) => Not(notTraversal);

        TTargetQuery IEdgeGremlinQuery<TElement>.Optional<TTargetQuery>(Func<IEdgeGremlinQuery<TElement>, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);
        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Or(params Func<IEdgeGremlinQuery<TElement>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Range(long low, long high) => Range(low, high);

        TTargetQuery IEdgeGremlinQuery<TElement>.Repeat<TTargetQuery>(Func<IEdgeGremlinQuery<TElement>, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);
        TTargetQuery IEdgeGremlinQuery<TElement>.RepeatUntil<TTargetQuery>(Func<IEdgeGremlinQuery<TElement>, TTargetQuery> repeatTraversal, Func<IEdgeGremlinQuery<TElement>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

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

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex>.Coalesce<TTargetQuery>(params Func<IEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery>[] traversals) => Coalesce(traversals);

        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Dedup() => Dedup();

        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Emit() => Emit();

        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Where(string lambda) => Where(lambda);

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex>.FlatMap<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery> mapping) => FlatMap(mapping);

        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Identity() => Identity();

        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Limit(long count) => Limit(count);
        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex>.Local<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex> , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex>.Map<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery> mapping) => Map(mapping);
        
        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Not(Func<IEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> notTraversal) => Not(notTraversal);

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex>.Optional<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);
        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Or(params Func<IEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Range(long low, long high) => Range(low, high);

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex>.Repeat<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);
        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex>.RepeatUntil<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery> repeatTraversal, Func<IEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

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

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Coalesce<TTargetQuery>(params Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TTargetQuery>[] traversals) => Coalesce(traversals);

        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Dedup() => Dedup();

        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Emit() => Emit();

        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Where(string lambda) => Where(lambda);

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.FlatMap<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TTargetQuery> mapping) => FlatMap(mapping);

        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Identity() => Identity();

        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Limit(long count) => Limit(count);
        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Local<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Map<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TTargetQuery> mapping) => Map(mapping);
        
        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Not(Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery> notTraversal) => Not(notTraversal);

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Optional<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);
        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Or(params Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Range(long low, long high) => Range(low, high);

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Repeat<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);
        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.RepeatUntil<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TTargetQuery> repeatTraversal, Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

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

        TTargetQuery IInEdgeGremlinQuery<TElement, TInVertex>.Coalesce<TTargetQuery>(params Func<IInEdgeGremlinQuery<TElement, TInVertex>, TTargetQuery>[] traversals) => Coalesce(traversals);

        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Dedup() => Dedup();

        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Emit() => Emit();

        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Where(string lambda) => Where(lambda);

        TTargetQuery IInEdgeGremlinQuery<TElement, TInVertex>.FlatMap<TTargetQuery>(Func<IInEdgeGremlinQuery<TElement, TInVertex>, TTargetQuery> mapping) => FlatMap(mapping);

        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Identity() => Identity();

        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Limit(long count) => Limit(count);
        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IInEdgeGremlinQuery<TElement, TInVertex>.Local<TTargetQuery>(Func<IInEdgeGremlinQuery<TElement, TInVertex> , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IInEdgeGremlinQuery<TElement, TInVertex>.Map<TTargetQuery>(Func<IInEdgeGremlinQuery<TElement, TInVertex>, TTargetQuery> mapping) => Map(mapping);
        
        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Not(Func<IInEdgeGremlinQuery<TElement, TInVertex>, IGremlinQuery> notTraversal) => Not(notTraversal);

        TTargetQuery IInEdgeGremlinQuery<TElement, TInVertex>.Optional<TTargetQuery>(Func<IInEdgeGremlinQuery<TElement, TInVertex>, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);
        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Or(params Func<IInEdgeGremlinQuery<TElement, TInVertex>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Range(long low, long high) => Range(low, high);

        TTargetQuery IInEdgeGremlinQuery<TElement, TInVertex>.Repeat<TTargetQuery>(Func<IInEdgeGremlinQuery<TElement, TInVertex>, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);
        TTargetQuery IInEdgeGremlinQuery<TElement, TInVertex>.RepeatUntil<TTargetQuery>(Func<IInEdgeGremlinQuery<TElement, TInVertex>, TTargetQuery> repeatTraversal, Func<IInEdgeGremlinQuery<TElement, TInVertex>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

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

        TTargetQuery IOutEdgeGremlinQuery<TElement, TOutVertex>.Coalesce<TTargetQuery>(params Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery>[] traversals) => Coalesce(traversals);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Dedup() => Dedup();

        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Emit() => Emit();

        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Where(string lambda) => Where(lambda);

        TTargetQuery IOutEdgeGremlinQuery<TElement, TOutVertex>.FlatMap<TTargetQuery>(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery> mapping) => FlatMap(mapping);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Identity() => Identity();

        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Limit(long count) => Limit(count);
        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IOutEdgeGremlinQuery<TElement, TOutVertex>.Local<TTargetQuery>(Func<IOutEdgeGremlinQuery<TElement, TOutVertex> , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IOutEdgeGremlinQuery<TElement, TOutVertex>.Map<TTargetQuery>(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery> mapping) => Map(mapping);
        
        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Not(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> notTraversal) => Not(notTraversal);

        TTargetQuery IOutEdgeGremlinQuery<TElement, TOutVertex>.Optional<TTargetQuery>(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);
        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Or(params Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Range(long low, long high) => Range(low, high);

        TTargetQuery IOutEdgeGremlinQuery<TElement, TOutVertex>.Repeat<TTargetQuery>(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);
        TTargetQuery IOutEdgeGremlinQuery<TElement, TOutVertex>.RepeatUntil<TTargetQuery>(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery> repeatTraversal, Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

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

        TTargetQuery IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Coalesce<TTargetQuery>(params Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, TTargetQuery>[] traversals) => Coalesce(traversals);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Dedup() => Dedup();

        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Emit() => Emit();

        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Where(string lambda) => Where(lambda);

        TTargetQuery IVertexPropertyGremlinQuery<TElement, TPropertyValue>.FlatMap<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, TTargetQuery> mapping) => FlatMap(mapping);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Identity() => Identity();

        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Limit(long count) => Limit(count);
        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Local<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue> , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Map<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, TTargetQuery> mapping) => Map(mapping);
        
        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Not(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, IGremlinQuery> notTraversal) => Not(notTraversal);

        TTargetQuery IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Optional<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);
        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Or(params Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Range(long low, long high) => Range(low, high);

        TTargetQuery IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Repeat<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);
        TTargetQuery IVertexPropertyGremlinQuery<TElement, TPropertyValue>.RepeatUntil<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, TTargetQuery> repeatTraversal, Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

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

        TTargetQuery IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Coalesce<TTargetQuery>(params Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, TTargetQuery>[] traversals) => Coalesce(traversals);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Dedup() => Dedup();

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Emit() => Emit();

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Where(string lambda) => Where(lambda);

        TTargetQuery IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.FlatMap<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, TTargetQuery> mapping) => FlatMap(mapping);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Identity() => Identity();

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Limit(long count) => Limit(count);
        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Local<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Map<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, TTargetQuery> mapping) => Map(mapping);
        
        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Not(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, IGremlinQuery> notTraversal) => Not(notTraversal);

        TTargetQuery IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Optional<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);
        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Or(params Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Range(long low, long high) => Range(low, high);

        TTargetQuery IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Repeat<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);
        TTargetQuery IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.RepeatUntil<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, TTargetQuery> repeatTraversal, Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.SideEffect(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);
        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Skip(long count) => Skip(count);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Tail(long count) => Tail(count);
        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.TailLocal(int count) => TailLocal(count);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Times(int count) => Times(count);

        TTargetQuery IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Union<TTargetQuery>(params Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        IEdgePropertyGremlinQuery<TElement, TPropertyValue> IEdgePropertyGremlinQuery<TElement, TPropertyValue>.And(params Func<IEdgePropertyGremlinQuery<TElement, TPropertyValue>, IGremlinQuery>[] andTraversals) => And(andTraversals);
        IEdgePropertyGremlinQuery<TElement, TPropertyValue> IEdgePropertyGremlinQuery<TElement, TPropertyValue>.As(params StepLabel[] stepLabels) => As(stepLabels);

        IEdgePropertyGremlinQuery<TElement, TPropertyValue> IEdgePropertyGremlinQuery<TElement, TPropertyValue>.Barrier() => Barrier();

        TTargetQuery IEdgePropertyGremlinQuery<TElement, TPropertyValue>.Choose<TTargetQuery>(Func<IEdgePropertyGremlinQuery<TElement, TPropertyValue>, IGremlinQuery> traversalPredicate, Func<IEdgePropertyGremlinQuery<TElement, TPropertyValue>, TTargetQuery> trueChoice, Func<IEdgePropertyGremlinQuery<TElement, TPropertyValue>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);
        TTargetQuery IEdgePropertyGremlinQuery<TElement, TPropertyValue>.Choose<TTargetQuery>(Func<IEdgePropertyGremlinQuery<TElement, TPropertyValue>, IGremlinQuery> traversalPredicate, Func<IEdgePropertyGremlinQuery<TElement, TPropertyValue>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);

        TTargetQuery IEdgePropertyGremlinQuery<TElement, TPropertyValue>.Coalesce<TTargetQuery>(params Func<IEdgePropertyGremlinQuery<TElement, TPropertyValue>, TTargetQuery>[] traversals) => Coalesce(traversals);

        IEdgePropertyGremlinQuery<TElement, TPropertyValue> IEdgePropertyGremlinQuery<TElement, TPropertyValue>.Dedup() => Dedup();

        IEdgePropertyGremlinQuery<TElement, TPropertyValue> IEdgePropertyGremlinQuery<TElement, TPropertyValue>.Emit() => Emit();

        IEdgePropertyGremlinQuery<TElement, TPropertyValue> IEdgePropertyGremlinQuery<TElement, TPropertyValue>.Where(string lambda) => Where(lambda);

        TTargetQuery IEdgePropertyGremlinQuery<TElement, TPropertyValue>.FlatMap<TTargetQuery>(Func<IEdgePropertyGremlinQuery<TElement, TPropertyValue>, TTargetQuery> mapping) => FlatMap(mapping);

        IEdgePropertyGremlinQuery<TElement, TPropertyValue> IEdgePropertyGremlinQuery<TElement, TPropertyValue>.Identity() => Identity();

        IEdgePropertyGremlinQuery<TElement, TPropertyValue> IEdgePropertyGremlinQuery<TElement, TPropertyValue>.Limit(long count) => Limit(count);
        IEdgePropertyGremlinQuery<TElement, TPropertyValue> IEdgePropertyGremlinQuery<TElement, TPropertyValue>.LimitLocal(long count) => LimitLocal(count);

        TTargetQuery IEdgePropertyGremlinQuery<TElement, TPropertyValue>.Local<TTargetQuery>(Func<IEdgePropertyGremlinQuery<TElement, TPropertyValue> , TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IEdgePropertyGremlinQuery<TElement, TPropertyValue>.Map<TTargetQuery>(Func<IEdgePropertyGremlinQuery<TElement, TPropertyValue>, TTargetQuery> mapping) => Map(mapping);
        
        IEdgePropertyGremlinQuery<TElement, TPropertyValue> IEdgePropertyGremlinQuery<TElement, TPropertyValue>.Not(Func<IEdgePropertyGremlinQuery<TElement, TPropertyValue>, IGremlinQuery> notTraversal) => Not(notTraversal);

        TTargetQuery IEdgePropertyGremlinQuery<TElement, TPropertyValue>.Optional<TTargetQuery>(Func<IEdgePropertyGremlinQuery<TElement, TPropertyValue>, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);
        IEdgePropertyGremlinQuery<TElement, TPropertyValue> IEdgePropertyGremlinQuery<TElement, TPropertyValue>.Or(params Func<IEdgePropertyGremlinQuery<TElement, TPropertyValue>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IEdgePropertyGremlinQuery<TElement, TPropertyValue> IEdgePropertyGremlinQuery<TElement, TPropertyValue>.Range(long low, long high) => Range(low, high);

        TTargetQuery IEdgePropertyGremlinQuery<TElement, TPropertyValue>.Repeat<TTargetQuery>(Func<IEdgePropertyGremlinQuery<TElement, TPropertyValue>, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);
        TTargetQuery IEdgePropertyGremlinQuery<TElement, TPropertyValue>.RepeatUntil<TTargetQuery>(Func<IEdgePropertyGremlinQuery<TElement, TPropertyValue>, TTargetQuery> repeatTraversal, Func<IEdgePropertyGremlinQuery<TElement, TPropertyValue>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        IEdgePropertyGremlinQuery<TElement, TPropertyValue> IEdgePropertyGremlinQuery<TElement, TPropertyValue>.SideEffect(Func<IEdgePropertyGremlinQuery<TElement, TPropertyValue>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);
        IEdgePropertyGremlinQuery<TElement, TPropertyValue> IEdgePropertyGremlinQuery<TElement, TPropertyValue>.Skip(long count) => Skip(count);

        IEdgePropertyGremlinQuery<TElement, TPropertyValue> IEdgePropertyGremlinQuery<TElement, TPropertyValue>.Tail(long count) => Tail(count);
        IEdgePropertyGremlinQuery<TElement, TPropertyValue> IEdgePropertyGremlinQuery<TElement, TPropertyValue>.TailLocal(int count) => TailLocal(count);

        IEdgePropertyGremlinQuery<TElement, TPropertyValue> IEdgePropertyGremlinQuery<TElement, TPropertyValue>.Times(int count) => Times(count);

        TTargetQuery IEdgePropertyGremlinQuery<TElement, TPropertyValue>.Union<TTargetQuery>(params Func<IEdgePropertyGremlinQuery<TElement, TPropertyValue>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        IGremlinQuery<TResult> IGremlinQuery.Cast<TResult>() => Cast<TResult>();

        IElementGremlinQuery<TResult> IElementGremlinQuery.Cast<TResult>() => Cast<TResult>();

        IVertexGremlinQuery<TResult> IVertexGremlinQuery.Cast<TResult>() => Cast<TResult>();

        IEdgeGremlinQuery<TResult> IEdgeGremlinQuery.Cast<TResult>() => Cast<TResult>();

        IOrderedValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.OrderBy(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Increasing);
        IOrderedValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.OrderByDescending(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Decreasing);

        IOrderedArrayGremlinQuery<TElement, TFoldedQuery> IArrayGremlinQuery<TElement, TFoldedQuery>.OrderBy(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Increasing);
        IOrderedArrayGremlinQuery<TElement, TFoldedQuery> IArrayGremlinQuery<TElement, TFoldedQuery>.OrderByDescending(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Decreasing);

        IOrderedVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.OrderBy(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Increasing);
        IOrderedVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.OrderByDescending(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Decreasing);

        IOrderedEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.OrderBy(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Increasing);
        IOrderedEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.OrderByDescending(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Decreasing);

        IOrderedEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.OrderBy(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Increasing);
        IOrderedEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.OrderByDescending(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Decreasing);

        IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.OrderBy(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Increasing);
        IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.OrderByDescending(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Decreasing);

        IOrderedInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.OrderBy(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Increasing);
        IOrderedInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.OrderByDescending(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Decreasing);

        IOrderedOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.OrderBy(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Increasing);
        IOrderedOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.OrderByDescending(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Decreasing);

        IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.OrderBy(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Increasing);
        IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.OrderByDescending(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Decreasing);

        IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.OrderBy(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Increasing);
        IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.OrderByDescending(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Decreasing);

        IOrderedEdgePropertyGremlinQuery<TElement, TPropertyValue> IEdgePropertyGremlinQuery<TElement, TPropertyValue>.OrderBy(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Increasing);
        IOrderedEdgePropertyGremlinQuery<TElement, TPropertyValue> IEdgePropertyGremlinQuery<TElement, TPropertyValue>.OrderByDescending(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Decreasing);

        IOrderedVertexGremlinQuery IVertexGremlinQuery.OrderBy(Func<IVertexGremlinQuery, IGremlinQuery> traversal) => OrderBy(traversal, Order.Increasing);
        IOrderedVertexGremlinQuery IVertexGremlinQuery.OrderBy(string lambda) => OrderBy(lambda);
        IOrderedVertexGremlinQuery IVertexGremlinQuery.OrderByDescending(Func<IVertexGremlinQuery, IGremlinQuery> traversal) => OrderBy(traversal, Order.Decreasing);

        IOrderedEdgeGremlinQuery IEdgeGremlinQuery.OrderBy(Func<IEdgeGremlinQuery, IGremlinQuery> traversal) => OrderBy(traversal, Order.Increasing);
        IOrderedEdgeGremlinQuery IEdgeGremlinQuery.OrderBy(string lambda) => OrderBy(lambda);
        IOrderedEdgeGremlinQuery IEdgeGremlinQuery.OrderByDescending(Func<IEdgeGremlinQuery, IGremlinQuery> traversal) => OrderBy(traversal, Order.Decreasing);

        IOrderedValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.OrderBy(Func<IValueGremlinQuery<TElement>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Increasing);
        IOrderedValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.OrderBy(string lambda) => OrderBy(lambda);
        IOrderedValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.OrderByDescending(Func<IValueGremlinQuery<TElement>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Decreasing);

        IOrderedArrayGremlinQuery<TElement, TFoldedQuery> IArrayGremlinQuery<TElement, TFoldedQuery>.OrderBy(Func<IArrayGremlinQuery<TElement, TFoldedQuery>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Increasing);
        IOrderedArrayGremlinQuery<TElement, TFoldedQuery> IArrayGremlinQuery<TElement, TFoldedQuery>.OrderBy(string lambda) => OrderBy(lambda);
        IOrderedArrayGremlinQuery<TElement, TFoldedQuery> IArrayGremlinQuery<TElement, TFoldedQuery>.OrderByDescending(Func<IArrayGremlinQuery<TElement, TFoldedQuery>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Decreasing);

        IOrderedVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.OrderBy(Func<IVertexGremlinQuery<TElement>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Increasing);
        IOrderedVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.OrderBy(string lambda) => OrderBy(lambda);
        IOrderedVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.OrderByDescending(Func<IVertexGremlinQuery<TElement>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Decreasing);

        IOrderedEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.OrderBy(Func<IEdgeGremlinQuery<TElement>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Increasing);
        IOrderedEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.OrderBy(string lambda) => OrderBy(lambda);
        IOrderedEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.OrderByDescending(Func<IEdgeGremlinQuery<TElement>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Decreasing);

        IOrderedEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.OrderBy(Func<IEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Increasing);
        IOrderedEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.OrderBy(string lambda) => OrderBy(lambda);
        IOrderedEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.OrderByDescending(Func<IEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Decreasing);

        IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.OrderBy(Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Increasing);
        IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.OrderBy(string lambda) => OrderBy(lambda);
        IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.OrderByDescending(Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Decreasing);

        IOrderedInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.OrderBy(Func<IInEdgeGremlinQuery<TElement, TInVertex>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Increasing);
        IOrderedInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.OrderBy(string lambda) => OrderBy(lambda);
        IOrderedInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.OrderByDescending(Func<IInEdgeGremlinQuery<TElement, TInVertex>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Decreasing);

        IOrderedOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.OrderBy(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Increasing);
        IOrderedOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.OrderBy(string lambda) => OrderBy(lambda);
        IOrderedOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.OrderByDescending(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Decreasing);

        IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.OrderBy(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Increasing);
        IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.OrderBy(string lambda) => OrderBy(lambda);
        IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.OrderByDescending(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Decreasing);

        IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.OrderBy(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Increasing);
        IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.OrderBy(string lambda) => OrderBy(lambda);
        IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.OrderByDescending(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Decreasing);

        IOrderedEdgePropertyGremlinQuery<TElement, TPropertyValue> IEdgePropertyGremlinQuery<TElement, TPropertyValue>.OrderBy(Func<IEdgePropertyGremlinQuery<TElement, TPropertyValue>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Increasing);
        IOrderedEdgePropertyGremlinQuery<TElement, TPropertyValue> IEdgePropertyGremlinQuery<TElement, TPropertyValue>.OrderBy(string lambda) => OrderBy(lambda);
        IOrderedEdgePropertyGremlinQuery<TElement, TPropertyValue> IEdgePropertyGremlinQuery<TElement, TPropertyValue>.OrderByDescending(Func<IEdgePropertyGremlinQuery<TElement, TPropertyValue>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Decreasing);

        IOrderedVertexGremlinQuery IOrderedVertexGremlinQuery.ThenBy(Func<IVertexGremlinQuery, IGremlinQuery> traversal) => By(traversal, Order.Increasing);
        IOrderedVertexGremlinQuery IOrderedVertexGremlinQuery.ThenByDescending(Func<IVertexGremlinQuery, IGremlinQuery> traversal) => By(traversal, Order.Decreasing);
        IOrderedVertexGremlinQuery IOrderedVertexGremlinQuery.ThenBy(string lambda) => By(lambda);

        IOrderedEdgeGremlinQuery IOrderedEdgeGremlinQuery.ThenBy(Func<IEdgeGremlinQuery, IGremlinQuery> traversal) => By(traversal, Order.Increasing);
        IOrderedEdgeGremlinQuery IOrderedEdgeGremlinQuery.ThenByDescending(Func<IEdgeGremlinQuery, IGremlinQuery> traversal) => By(traversal, Order.Decreasing);
        IOrderedEdgeGremlinQuery IOrderedEdgeGremlinQuery.ThenBy(string lambda) => By(lambda);

        IOrderedValueGremlinQuery<TElement> IOrderedValueGremlinQuery<TElement>.ThenBy(Func<IValueGremlinQuery<TElement>, IGremlinQuery> traversal) => By(traversal, Order.Increasing);
        IOrderedValueGremlinQuery<TElement> IOrderedValueGremlinQuery<TElement>.ThenByDescending(Func<IValueGremlinQuery<TElement>, IGremlinQuery> traversal) => By(traversal, Order.Decreasing);
        IOrderedValueGremlinQuery<TElement> IOrderedValueGremlinQuery<TElement>.ThenBy(string lambda) => By(lambda);

        IOrderedArrayGremlinQuery<TElement, TFoldedQuery> IOrderedArrayGremlinQuery<TElement, TFoldedQuery>.ThenBy(Func<IArrayGremlinQuery<TElement, TFoldedQuery>, IGremlinQuery> traversal) => By(traversal, Order.Increasing);
        IOrderedArrayGremlinQuery<TElement, TFoldedQuery> IOrderedArrayGremlinQuery<TElement, TFoldedQuery>.ThenByDescending(Func<IArrayGremlinQuery<TElement, TFoldedQuery>, IGremlinQuery> traversal) => By(traversal, Order.Decreasing);
        IOrderedArrayGremlinQuery<TElement, TFoldedQuery> IOrderedArrayGremlinQuery<TElement, TFoldedQuery>.ThenBy(string lambda) => By(lambda);

        IOrderedVertexGremlinQuery<TElement> IOrderedVertexGremlinQuery<TElement>.ThenBy(Func<IVertexGremlinQuery<TElement>, IGremlinQuery> traversal) => By(traversal, Order.Increasing);
        IOrderedVertexGremlinQuery<TElement> IOrderedVertexGremlinQuery<TElement>.ThenByDescending(Func<IVertexGremlinQuery<TElement>, IGremlinQuery> traversal) => By(traversal, Order.Decreasing);
        IOrderedVertexGremlinQuery<TElement> IOrderedVertexGremlinQuery<TElement>.ThenBy(string lambda) => By(lambda);

        IOrderedEdgeGremlinQuery<TElement> IOrderedEdgeGremlinQuery<TElement>.ThenBy(Func<IEdgeGremlinQuery<TElement>, IGremlinQuery> traversal) => By(traversal, Order.Increasing);
        IOrderedEdgeGremlinQuery<TElement> IOrderedEdgeGremlinQuery<TElement>.ThenByDescending(Func<IEdgeGremlinQuery<TElement>, IGremlinQuery> traversal) => By(traversal, Order.Decreasing);
        IOrderedEdgeGremlinQuery<TElement> IOrderedEdgeGremlinQuery<TElement>.ThenBy(string lambda) => By(lambda);

        IOrderedEdgeGremlinQuery<TElement, TOutVertex> IOrderedEdgeGremlinQuery<TElement, TOutVertex>.ThenBy(Func<IEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> traversal) => By(traversal, Order.Increasing);
        IOrderedEdgeGremlinQuery<TElement, TOutVertex> IOrderedEdgeGremlinQuery<TElement, TOutVertex>.ThenByDescending(Func<IEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> traversal) => By(traversal, Order.Decreasing);
        IOrderedEdgeGremlinQuery<TElement, TOutVertex> IOrderedEdgeGremlinQuery<TElement, TOutVertex>.ThenBy(string lambda) => By(lambda);

        IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.ThenBy(Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery> traversal) => By(traversal, Order.Increasing);
        IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.ThenByDescending(Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery> traversal) => By(traversal, Order.Decreasing);
        IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.ThenBy(string lambda) => By(lambda);

        IOrderedInEdgeGremlinQuery<TElement, TInVertex> IOrderedInEdgeGremlinQuery<TElement, TInVertex>.ThenBy(Func<IInEdgeGremlinQuery<TElement, TInVertex>, IGremlinQuery> traversal) => By(traversal, Order.Increasing);
        IOrderedInEdgeGremlinQuery<TElement, TInVertex> IOrderedInEdgeGremlinQuery<TElement, TInVertex>.ThenByDescending(Func<IInEdgeGremlinQuery<TElement, TInVertex>, IGremlinQuery> traversal) => By(traversal, Order.Decreasing);
        IOrderedInEdgeGremlinQuery<TElement, TInVertex> IOrderedInEdgeGremlinQuery<TElement, TInVertex>.ThenBy(string lambda) => By(lambda);

        IOrderedOutEdgeGremlinQuery<TElement, TOutVertex> IOrderedOutEdgeGremlinQuery<TElement, TOutVertex>.ThenBy(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> traversal) => By(traversal, Order.Increasing);
        IOrderedOutEdgeGremlinQuery<TElement, TOutVertex> IOrderedOutEdgeGremlinQuery<TElement, TOutVertex>.ThenByDescending(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> traversal) => By(traversal, Order.Decreasing);
        IOrderedOutEdgeGremlinQuery<TElement, TOutVertex> IOrderedOutEdgeGremlinQuery<TElement, TOutVertex>.ThenBy(string lambda) => By(lambda);

        IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue> IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue>.ThenBy(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, IGremlinQuery> traversal) => By(traversal, Order.Increasing);
        IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue> IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue>.ThenByDescending(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, IGremlinQuery> traversal) => By(traversal, Order.Decreasing);
        IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue> IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue>.ThenBy(string lambda) => By(lambda);

        IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.ThenBy(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, IGremlinQuery> traversal) => By(traversal, Order.Increasing);
        IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.ThenByDescending(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, IGremlinQuery> traversal) => By(traversal, Order.Decreasing);
        IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.ThenBy(string lambda) => By(lambda);

        IOrderedEdgePropertyGremlinQuery<TElement, TPropertyValue> IOrderedEdgePropertyGremlinQuery<TElement, TPropertyValue>.ThenBy(Func<IEdgePropertyGremlinQuery<TElement, TPropertyValue>, IGremlinQuery> traversal) => By(traversal, Order.Increasing);
        IOrderedEdgePropertyGremlinQuery<TElement, TPropertyValue> IOrderedEdgePropertyGremlinQuery<TElement, TPropertyValue>.ThenByDescending(Func<IEdgePropertyGremlinQuery<TElement, TPropertyValue>, IGremlinQuery> traversal) => By(traversal, Order.Decreasing);
        IOrderedEdgePropertyGremlinQuery<TElement, TPropertyValue> IOrderedEdgePropertyGremlinQuery<TElement, TPropertyValue>.ThenBy(string lambda) => By(lambda);

        IOrderedValueGremlinQuery<TElement> IOrderedValueGremlinQuery<TElement>.ThenBy(Expression<Func<TElement, object>> projection) => By(projection, Order.Increasing);
        IOrderedValueGremlinQuery<TElement> IOrderedValueGremlinQuery<TElement>.ThenByDescending(Expression<Func<TElement, object>> projection) => By(projection, Order.Decreasing);

        IOrderedArrayGremlinQuery<TElement, TFoldedQuery> IOrderedArrayGremlinQuery<TElement, TFoldedQuery>.ThenBy(Expression<Func<TElement, object>> projection) => By(projection, Order.Increasing);
        IOrderedArrayGremlinQuery<TElement, TFoldedQuery> IOrderedArrayGremlinQuery<TElement, TFoldedQuery>.ThenByDescending(Expression<Func<TElement, object>> projection) => By(projection, Order.Decreasing);

        IOrderedVertexGremlinQuery<TElement> IOrderedVertexGremlinQuery<TElement>.ThenBy(Expression<Func<TElement, object>> projection) => By(projection, Order.Increasing);
        IOrderedVertexGremlinQuery<TElement> IOrderedVertexGremlinQuery<TElement>.ThenByDescending(Expression<Func<TElement, object>> projection) => By(projection, Order.Decreasing);

        IOrderedEdgeGremlinQuery<TElement> IOrderedEdgeGremlinQuery<TElement>.ThenBy(Expression<Func<TElement, object>> projection) => By(projection, Order.Increasing);
        IOrderedEdgeGremlinQuery<TElement> IOrderedEdgeGremlinQuery<TElement>.ThenByDescending(Expression<Func<TElement, object>> projection) => By(projection, Order.Decreasing);

        IOrderedEdgeGremlinQuery<TElement, TOutVertex> IOrderedEdgeGremlinQuery<TElement, TOutVertex>.ThenBy(Expression<Func<TElement, object>> projection) => By(projection, Order.Increasing);
        IOrderedEdgeGremlinQuery<TElement, TOutVertex> IOrderedEdgeGremlinQuery<TElement, TOutVertex>.ThenByDescending(Expression<Func<TElement, object>> projection) => By(projection, Order.Decreasing);

        IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.ThenBy(Expression<Func<TElement, object>> projection) => By(projection, Order.Increasing);
        IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.ThenByDescending(Expression<Func<TElement, object>> projection) => By(projection, Order.Decreasing);

        IOrderedInEdgeGremlinQuery<TElement, TInVertex> IOrderedInEdgeGremlinQuery<TElement, TInVertex>.ThenBy(Expression<Func<TElement, object>> projection) => By(projection, Order.Increasing);
        IOrderedInEdgeGremlinQuery<TElement, TInVertex> IOrderedInEdgeGremlinQuery<TElement, TInVertex>.ThenByDescending(Expression<Func<TElement, object>> projection) => By(projection, Order.Decreasing);

        IOrderedOutEdgeGremlinQuery<TElement, TOutVertex> IOrderedOutEdgeGremlinQuery<TElement, TOutVertex>.ThenBy(Expression<Func<TElement, object>> projection) => By(projection, Order.Increasing);
        IOrderedOutEdgeGremlinQuery<TElement, TOutVertex> IOrderedOutEdgeGremlinQuery<TElement, TOutVertex>.ThenByDescending(Expression<Func<TElement, object>> projection) => By(projection, Order.Decreasing);

        IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue> IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue>.ThenBy(Expression<Func<TElement, object>> projection) => By(projection, Order.Increasing);
        IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue> IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue>.ThenByDescending(Expression<Func<TElement, object>> projection) => By(projection, Order.Decreasing);

        IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.ThenBy(Expression<Func<TElement, object>> projection) => By(projection, Order.Increasing);
        IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.ThenByDescending(Expression<Func<TElement, object>> projection) => By(projection, Order.Decreasing);

        IOrderedEdgePropertyGremlinQuery<TElement, TPropertyValue> IOrderedEdgePropertyGremlinQuery<TElement, TPropertyValue>.ThenBy(Expression<Func<TElement, object>> projection) => By(projection, Order.Increasing);
        IOrderedEdgePropertyGremlinQuery<TElement, TPropertyValue> IOrderedEdgePropertyGremlinQuery<TElement, TPropertyValue>.ThenByDescending(Expression<Func<TElement, object>> projection) => By(projection, Order.Decreasing);

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

        IVertexGremlinQuery<TTarget> IVertexGremlinQuery.OfType<TTarget>() => OfType<TTarget>(Model.VerticesModel);
        IEdgeGremlinQuery<TTarget> IEdgeGremlinQuery.OfType<TTarget>() => OfType<TTarget>(Model.EdgesModel);
                IVertexGremlinQuery<TTarget> IVertexGremlinQuery<TElement>.OfType<TTarget>() => OfType<TTarget>(Model.VerticesModel);
        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Property<TProjectedValue>(Expression<Func<TElement, TProjectedValue>> projection, [AllowNull] TProjectedValue value) => Property(projection, value);
        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Property<TProjectedValue>(Expression<Func<TElement, TProjectedValue[]>> projection, [AllowNull] TProjectedValue value) => Property(projection, value);
        
        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Where(Expression<Func<TElement, bool>> predicate) => Where(predicate);
        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal) => Where(projection, propertyTraversal);

                IEdgeGremlinQuery<TTarget> IEdgeGremlinQuery<TElement>.OfType<TTarget>() => OfType<TTarget>(Model.EdgesModel);
        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Property<TProjectedValue>(Expression<Func<TElement, TProjectedValue>> projection, [AllowNull] TProjectedValue value) => Property(projection, value);
        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Property<TProjectedValue>(Expression<Func<TElement, TProjectedValue[]>> projection, [AllowNull] TProjectedValue value) => Property(projection, value);
        
        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Where(Expression<Func<TElement, bool>> predicate) => Where(predicate);
        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal) => Where(projection, propertyTraversal);

                IEdgeGremlinQuery<TTarget, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.OfType<TTarget>() => OfType<TTarget>(Model.EdgesModel);
        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Property<TProjectedValue>(Expression<Func<TElement, TProjectedValue>> projection, [AllowNull] TProjectedValue value) => Property(projection, value);
        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Property<TProjectedValue>(Expression<Func<TElement, TProjectedValue[]>> projection, [AllowNull] TProjectedValue value) => Property(projection, value);
        
        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Where(Expression<Func<TElement, bool>> predicate) => Where(predicate);
        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal) => Where(projection, propertyTraversal);

                IEdgeGremlinQuery<TTarget, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.OfType<TTarget>() => OfType<TTarget>(Model.EdgesModel);
        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Property<TProjectedValue>(Expression<Func<TElement, TProjectedValue>> projection, [AllowNull] TProjectedValue value) => Property(projection, value);
        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Property<TProjectedValue>(Expression<Func<TElement, TProjectedValue[]>> projection, [AllowNull] TProjectedValue value) => Property(projection, value);
        
        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Where(Expression<Func<TElement, bool>> predicate) => Where(predicate);
        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal) => Where(projection, propertyTraversal);

                IInEdgeGremlinQuery<TTarget, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.OfType<TTarget>() => OfType<TTarget>(Model.EdgesModel);
        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Property<TProjectedValue>(Expression<Func<TElement, TProjectedValue>> projection, [AllowNull] TProjectedValue value) => Property(projection, value);
        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Property<TProjectedValue>(Expression<Func<TElement, TProjectedValue[]>> projection, [AllowNull] TProjectedValue value) => Property(projection, value);
        
        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Where(Expression<Func<TElement, bool>> predicate) => Where(predicate);
        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal) => Where(projection, propertyTraversal);

                IOutEdgeGremlinQuery<TTarget, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.OfType<TTarget>() => OfType<TTarget>(Model.EdgesModel);
        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Property<TProjectedValue>(Expression<Func<TElement, TProjectedValue>> projection, [AllowNull] TProjectedValue value) => Property(projection, value);
        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Property<TProjectedValue>(Expression<Func<TElement, TProjectedValue[]>> projection, [AllowNull] TProjectedValue value) => Property(projection, value);
        
        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Where(Expression<Func<TElement, bool>> predicate) => Where(predicate);
        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal) => Where(projection, propertyTraversal);

        
        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Where(Expression<Func<TElement, bool>> predicate) => Where(predicate);
        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal) => Where(projection, propertyTraversal);

        
        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Where(Expression<Func<TElement, bool>> predicate) => Where(predicate);
        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal) => Where(projection, propertyTraversal);

        IGremlinQuery<TResult> IGremlinQuery<TElement>.Cast<TResult>() => Cast<TResult>();
        IGremlinQuery<TElement> IGremlinQuery<TElement>.Coin(double probability) => Coin(probability);

        TTargetQuery IGremlinQuery<TElement>.Aggregate<TTargetQuery>(Func<IGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);
        TTargetQuery IGremlinQuery<TElement>.As<TTargetQuery>(Func<IGremlinQuery<TElement>, StepLabel<IGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => As(continuation);

        IArrayGremlinQuery<TElement[], IGremlinQuery<TElement>> IGremlinQuery<TElement>.Fold() => Fold<IGremlinQuery<TElement>>();

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Where(Func<IGremlinQuery<TElement>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IValueGremlinQuery<TResult> IValueGremlinQuery<TElement>.Cast<TResult>() => Cast<TResult>();
        IValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.Coin(double probability) => Coin(probability);

        TTargetQuery IValueGremlinQuery<TElement>.Aggregate<TTargetQuery>(Func<IValueGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);
        TTargetQuery IValueGremlinQuery<TElement>.As<TTargetQuery>(Func<IValueGremlinQuery<TElement>, StepLabel<IValueGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => As(continuation);

        IArrayGremlinQuery<TElement[], IValueGremlinQuery<TElement>> IValueGremlinQuery<TElement>.Fold() => Fold<IValueGremlinQuery<TElement>>();

        IValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.Where(Func<IValueGremlinQuery<TElement>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IArrayGremlinQuery<TResult, TFoldedQuery> IArrayGremlinQuery<TElement, TFoldedQuery>.Cast<TResult>() => Cast<TResult>();
        IArrayGremlinQuery<TElement, TFoldedQuery> IArrayGremlinQuery<TElement, TFoldedQuery>.Coin(double probability) => Coin(probability);

        TTargetQuery IArrayGremlinQuery<TElement, TFoldedQuery>.Aggregate<TTargetQuery>(Func<IArrayGremlinQuery<TElement, TFoldedQuery>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);
        TTargetQuery IArrayGremlinQuery<TElement, TFoldedQuery>.As<TTargetQuery>(Func<IArrayGremlinQuery<TElement, TFoldedQuery>, StepLabel<IArrayGremlinQuery<TElement, TFoldedQuery>, TElement>, TTargetQuery> continuation) => As(continuation);

        IArrayGremlinQuery<TElement[], IArrayGremlinQuery<TElement, TFoldedQuery>> IArrayGremlinQuery<TElement, TFoldedQuery>.Fold() => Fold<IArrayGremlinQuery<TElement, TFoldedQuery>>();

        IArrayGremlinQuery<TElement, TFoldedQuery> IArrayGremlinQuery<TElement, TFoldedQuery>.Where(Func<IArrayGremlinQuery<TElement, TFoldedQuery>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IElementGremlinQuery<TResult> IElementGremlinQuery<TElement>.Cast<TResult>() => Cast<TResult>();
        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Coin(double probability) => Coin(probability);

        TTargetQuery IElementGremlinQuery<TElement>.Aggregate<TTargetQuery>(Func<IElementGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);
        TTargetQuery IElementGremlinQuery<TElement>.As<TTargetQuery>(Func<IElementGremlinQuery<TElement>, StepLabel<IElementGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => As(continuation);

        IArrayGremlinQuery<TElement[], IElementGremlinQuery<TElement>> IElementGremlinQuery<TElement>.Fold() => Fold<IElementGremlinQuery<TElement>>();

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Where(Func<IElementGremlinQuery<TElement>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IVertexGremlinQuery<TResult> IVertexGremlinQuery<TElement>.Cast<TResult>() => Cast<TResult>();
        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Coin(double probability) => Coin(probability);

        TTargetQuery IVertexGremlinQuery<TElement>.Aggregate<TTargetQuery>(Func<IVertexGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);
        TTargetQuery IVertexGremlinQuery<TElement>.As<TTargetQuery>(Func<IVertexGremlinQuery<TElement>, StepLabel<IVertexGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => As(continuation);

        IArrayGremlinQuery<TElement[], IVertexGremlinQuery<TElement>> IVertexGremlinQuery<TElement>.Fold() => Fold<IVertexGremlinQuery<TElement>>();

        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Where(Func<IVertexGremlinQuery<TElement>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IEdgeGremlinQuery<TResult> IEdgeGremlinQuery<TElement>.Cast<TResult>() => Cast<TResult>();
        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Coin(double probability) => Coin(probability);

        TTargetQuery IEdgeGremlinQuery<TElement>.Aggregate<TTargetQuery>(Func<IEdgeGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);
        TTargetQuery IEdgeGremlinQuery<TElement>.As<TTargetQuery>(Func<IEdgeGremlinQuery<TElement>, StepLabel<IEdgeGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => As(continuation);

        IArrayGremlinQuery<TElement[], IEdgeGremlinQuery<TElement>> IEdgeGremlinQuery<TElement>.Fold() => Fold<IEdgeGremlinQuery<TElement>>();

        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Where(Func<IEdgeGremlinQuery<TElement>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IEdgeGremlinQuery<TResult, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Cast<TResult>() => Cast<TResult>();
        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Coin(double probability) => Coin(probability);

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex>.Aggregate<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);
        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex>.As<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex>, StepLabel<IEdgeGremlinQuery<TElement, TOutVertex>, TElement>, TTargetQuery> continuation) => As(continuation);

        IArrayGremlinQuery<TElement[], IEdgeGremlinQuery<TElement, TOutVertex>> IEdgeGremlinQuery<TElement, TOutVertex>.Fold() => Fold<IEdgeGremlinQuery<TElement, TOutVertex>>();

        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Where(Func<IEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IEdgeGremlinQuery<TResult, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Cast<TResult>() => Cast<TResult>();
        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Coin(double probability) => Coin(probability);

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Aggregate<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);
        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.As<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, StepLabel<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TElement>, TTargetQuery> continuation) => As(continuation);

        IArrayGremlinQuery<TElement[], IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Fold() => Fold<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>();

        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Where(Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IInEdgeGremlinQuery<TResult, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Cast<TResult>() => Cast<TResult>();
        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Coin(double probability) => Coin(probability);

        TTargetQuery IInEdgeGremlinQuery<TElement, TInVertex>.Aggregate<TTargetQuery>(Func<IInEdgeGremlinQuery<TElement, TInVertex>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);
        TTargetQuery IInEdgeGremlinQuery<TElement, TInVertex>.As<TTargetQuery>(Func<IInEdgeGremlinQuery<TElement, TInVertex>, StepLabel<IInEdgeGremlinQuery<TElement, TInVertex>, TElement>, TTargetQuery> continuation) => As(continuation);

        IArrayGremlinQuery<TElement[], IInEdgeGremlinQuery<TElement, TInVertex>> IInEdgeGremlinQuery<TElement, TInVertex>.Fold() => Fold<IInEdgeGremlinQuery<TElement, TInVertex>>();

        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Where(Func<IInEdgeGremlinQuery<TElement, TInVertex>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IOutEdgeGremlinQuery<TResult, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Cast<TResult>() => Cast<TResult>();
        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Coin(double probability) => Coin(probability);

        TTargetQuery IOutEdgeGremlinQuery<TElement, TOutVertex>.Aggregate<TTargetQuery>(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);
        TTargetQuery IOutEdgeGremlinQuery<TElement, TOutVertex>.As<TTargetQuery>(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, StepLabel<IOutEdgeGremlinQuery<TElement, TOutVertex>, TElement>, TTargetQuery> continuation) => As(continuation);

        IArrayGremlinQuery<TElement[], IOutEdgeGremlinQuery<TElement, TOutVertex>> IOutEdgeGremlinQuery<TElement, TOutVertex>.Fold() => Fold<IOutEdgeGremlinQuery<TElement, TOutVertex>>();

        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Where(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IVertexPropertyGremlinQuery<TResult, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Cast<TResult>() => Cast<TResult>();
        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Coin(double probability) => Coin(probability);

        TTargetQuery IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Aggregate<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);
        TTargetQuery IVertexPropertyGremlinQuery<TElement, TPropertyValue>.As<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, StepLabel<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, TElement>, TTargetQuery> continuation) => As(continuation);

        IArrayGremlinQuery<TElement[], IVertexPropertyGremlinQuery<TElement, TPropertyValue>> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Fold() => Fold<IVertexPropertyGremlinQuery<TElement, TPropertyValue>>();

        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Where(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IVertexPropertyGremlinQuery<TResult, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Cast<TResult>() => Cast<TResult>();
        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Coin(double probability) => Coin(probability);

        TTargetQuery IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Aggregate<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);
        TTargetQuery IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.As<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, StepLabel<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, TElement>, TTargetQuery> continuation) => As(continuation);

        IArrayGremlinQuery<TElement[], IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Fold() => Fold<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>();

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Where(Func<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IEdgePropertyGremlinQuery<TResult, TPropertyValue> IEdgePropertyGremlinQuery<TElement, TPropertyValue>.Cast<TResult>() => Cast<TResult>();
        IEdgePropertyGremlinQuery<TElement, TPropertyValue> IEdgePropertyGremlinQuery<TElement, TPropertyValue>.Coin(double probability) => Coin(probability);

        TTargetQuery IEdgePropertyGremlinQuery<TElement, TPropertyValue>.Aggregate<TTargetQuery>(Func<IEdgePropertyGremlinQuery<TElement, TPropertyValue>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);
        TTargetQuery IEdgePropertyGremlinQuery<TElement, TPropertyValue>.As<TTargetQuery>(Func<IEdgePropertyGremlinQuery<TElement, TPropertyValue>, StepLabel<IEdgePropertyGremlinQuery<TElement, TPropertyValue>, TElement>, TTargetQuery> continuation) => As(continuation);

        IArrayGremlinQuery<TElement[], IEdgePropertyGremlinQuery<TElement, TPropertyValue>> IEdgePropertyGremlinQuery<TElement, TPropertyValue>.Fold() => Fold<IEdgePropertyGremlinQuery<TElement, TPropertyValue>>();

        IEdgePropertyGremlinQuery<TElement, TPropertyValue> IEdgePropertyGremlinQuery<TElement, TPropertyValue>.Where(Func<IEdgePropertyGremlinQuery<TElement, TPropertyValue>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IOrderedGremlinQuery<TResult> IOrderedGremlinQuery<TElement>.Cast<TResult>() => Cast<TResult>();
        IOrderedGremlinQuery<TElement> IOrderedGremlinQuery<TElement>.Coin(double probability) => Coin(probability);

        TTargetQuery IOrderedGremlinQuery<TElement>.Aggregate<TTargetQuery>(Func<IOrderedGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);
        TTargetQuery IOrderedGremlinQuery<TElement>.As<TTargetQuery>(Func<IOrderedGremlinQuery<TElement>, StepLabel<IOrderedGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => As(continuation);

        IArrayGremlinQuery<TElement[], IGremlinQuery<TElement>> IOrderedGremlinQuery<TElement>.Fold() => Fold<IGremlinQuery<TElement>>();

        IGremlinQuery<TElement> IOrderedGremlinQuery<TElement>.Where(Func<IOrderedGremlinQuery<TElement>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IOrderedValueGremlinQuery<TResult> IOrderedValueGremlinQuery<TElement>.Cast<TResult>() => Cast<TResult>();
        IOrderedValueGremlinQuery<TElement> IOrderedValueGremlinQuery<TElement>.Coin(double probability) => Coin(probability);

        TTargetQuery IOrderedValueGremlinQuery<TElement>.Aggregate<TTargetQuery>(Func<IOrderedValueGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);
        TTargetQuery IOrderedValueGremlinQuery<TElement>.As<TTargetQuery>(Func<IOrderedValueGremlinQuery<TElement>, StepLabel<IOrderedValueGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => As(continuation);

        IArrayGremlinQuery<TElement[], IValueGremlinQuery<TElement>> IOrderedValueGremlinQuery<TElement>.Fold() => Fold<IValueGremlinQuery<TElement>>();

        IValueGremlinQuery<TElement> IOrderedValueGremlinQuery<TElement>.Where(Func<IOrderedValueGremlinQuery<TElement>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IOrderedArrayGremlinQuery<TResult, TFoldedQuery> IOrderedArrayGremlinQuery<TElement, TFoldedQuery>.Cast<TResult>() => Cast<TResult>();
        IOrderedArrayGremlinQuery<TElement, TFoldedQuery> IOrderedArrayGremlinQuery<TElement, TFoldedQuery>.Coin(double probability) => Coin(probability);

        TTargetQuery IOrderedArrayGremlinQuery<TElement, TFoldedQuery>.Aggregate<TTargetQuery>(Func<IOrderedArrayGremlinQuery<TElement, TFoldedQuery>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);
        TTargetQuery IOrderedArrayGremlinQuery<TElement, TFoldedQuery>.As<TTargetQuery>(Func<IOrderedArrayGremlinQuery<TElement, TFoldedQuery>, StepLabel<IOrderedArrayGremlinQuery<TElement, TFoldedQuery>, TElement>, TTargetQuery> continuation) => As(continuation);

        IArrayGremlinQuery<TElement[], IArrayGremlinQuery<TElement, TFoldedQuery>> IOrderedArrayGremlinQuery<TElement, TFoldedQuery>.Fold() => Fold<IArrayGremlinQuery<TElement, TFoldedQuery>>();

        IArrayGremlinQuery<TElement, TFoldedQuery> IOrderedArrayGremlinQuery<TElement, TFoldedQuery>.Where(Func<IOrderedArrayGremlinQuery<TElement, TFoldedQuery>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IOrderedElementGremlinQuery<TResult> IOrderedElementGremlinQuery<TElement>.Cast<TResult>() => Cast<TResult>();
        IOrderedElementGremlinQuery<TElement> IOrderedElementGremlinQuery<TElement>.Coin(double probability) => Coin(probability);

        TTargetQuery IOrderedElementGremlinQuery<TElement>.Aggregate<TTargetQuery>(Func<IOrderedElementGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);
        TTargetQuery IOrderedElementGremlinQuery<TElement>.As<TTargetQuery>(Func<IOrderedElementGremlinQuery<TElement>, StepLabel<IOrderedElementGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => As(continuation);

        IArrayGremlinQuery<TElement[], IElementGremlinQuery<TElement>> IOrderedElementGremlinQuery<TElement>.Fold() => Fold<IElementGremlinQuery<TElement>>();

        IElementGremlinQuery<TElement> IOrderedElementGremlinQuery<TElement>.Where(Func<IOrderedElementGremlinQuery<TElement>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IOrderedVertexGremlinQuery<TResult> IOrderedVertexGremlinQuery<TElement>.Cast<TResult>() => Cast<TResult>();
        IOrderedVertexGremlinQuery<TElement> IOrderedVertexGremlinQuery<TElement>.Coin(double probability) => Coin(probability);

        TTargetQuery IOrderedVertexGremlinQuery<TElement>.Aggregate<TTargetQuery>(Func<IOrderedVertexGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);
        TTargetQuery IOrderedVertexGremlinQuery<TElement>.As<TTargetQuery>(Func<IOrderedVertexGremlinQuery<TElement>, StepLabel<IOrderedVertexGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => As(continuation);

        IArrayGremlinQuery<TElement[], IVertexGremlinQuery<TElement>> IOrderedVertexGremlinQuery<TElement>.Fold() => Fold<IVertexGremlinQuery<TElement>>();

        IVertexGremlinQuery<TElement> IOrderedVertexGremlinQuery<TElement>.Where(Func<IOrderedVertexGremlinQuery<TElement>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IOrderedEdgeGremlinQuery<TResult> IOrderedEdgeGremlinQuery<TElement>.Cast<TResult>() => Cast<TResult>();
        IOrderedEdgeGremlinQuery<TElement> IOrderedEdgeGremlinQuery<TElement>.Coin(double probability) => Coin(probability);

        TTargetQuery IOrderedEdgeGremlinQuery<TElement>.Aggregate<TTargetQuery>(Func<IOrderedEdgeGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);
        TTargetQuery IOrderedEdgeGremlinQuery<TElement>.As<TTargetQuery>(Func<IOrderedEdgeGremlinQuery<TElement>, StepLabel<IOrderedEdgeGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => As(continuation);

        IArrayGremlinQuery<TElement[], IEdgeGremlinQuery<TElement>> IOrderedEdgeGremlinQuery<TElement>.Fold() => Fold<IEdgeGremlinQuery<TElement>>();

        IEdgeGremlinQuery<TElement> IOrderedEdgeGremlinQuery<TElement>.Where(Func<IOrderedEdgeGremlinQuery<TElement>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IOrderedEdgeGremlinQuery<TResult, TOutVertex> IOrderedEdgeGremlinQuery<TElement, TOutVertex>.Cast<TResult>() => Cast<TResult>();
        IOrderedEdgeGremlinQuery<TElement, TOutVertex> IOrderedEdgeGremlinQuery<TElement, TOutVertex>.Coin(double probability) => Coin(probability);

        TTargetQuery IOrderedEdgeGremlinQuery<TElement, TOutVertex>.Aggregate<TTargetQuery>(Func<IOrderedEdgeGremlinQuery<TElement, TOutVertex>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);
        TTargetQuery IOrderedEdgeGremlinQuery<TElement, TOutVertex>.As<TTargetQuery>(Func<IOrderedEdgeGremlinQuery<TElement, TOutVertex>, StepLabel<IOrderedEdgeGremlinQuery<TElement, TOutVertex>, TElement>, TTargetQuery> continuation) => As(continuation);

        IArrayGremlinQuery<TElement[], IEdgeGremlinQuery<TElement, TOutVertex>> IOrderedEdgeGremlinQuery<TElement, TOutVertex>.Fold() => Fold<IEdgeGremlinQuery<TElement, TOutVertex>>();

        IEdgeGremlinQuery<TElement, TOutVertex> IOrderedEdgeGremlinQuery<TElement, TOutVertex>.Where(Func<IOrderedEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IOrderedEdgeGremlinQuery<TResult, TOutVertex, TInVertex> IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Cast<TResult>() => Cast<TResult>();
        IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Coin(double probability) => Coin(probability);

        TTargetQuery IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Aggregate<TTargetQuery>(Func<IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);
        TTargetQuery IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.As<TTargetQuery>(Func<IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, StepLabel<IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TElement>, TTargetQuery> continuation) => As(continuation);

        IArrayGremlinQuery<TElement[], IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>> IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Fold() => Fold<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>>();

        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Where(Func<IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IOrderedInEdgeGremlinQuery<TResult, TInVertex> IOrderedInEdgeGremlinQuery<TElement, TInVertex>.Cast<TResult>() => Cast<TResult>();
        IOrderedInEdgeGremlinQuery<TElement, TInVertex> IOrderedInEdgeGremlinQuery<TElement, TInVertex>.Coin(double probability) => Coin(probability);

        TTargetQuery IOrderedInEdgeGremlinQuery<TElement, TInVertex>.Aggregate<TTargetQuery>(Func<IOrderedInEdgeGremlinQuery<TElement, TInVertex>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);
        TTargetQuery IOrderedInEdgeGremlinQuery<TElement, TInVertex>.As<TTargetQuery>(Func<IOrderedInEdgeGremlinQuery<TElement, TInVertex>, StepLabel<IOrderedInEdgeGremlinQuery<TElement, TInVertex>, TElement>, TTargetQuery> continuation) => As(continuation);

        IArrayGremlinQuery<TElement[], IInEdgeGremlinQuery<TElement, TInVertex>> IOrderedInEdgeGremlinQuery<TElement, TInVertex>.Fold() => Fold<IInEdgeGremlinQuery<TElement, TInVertex>>();

        IInEdgeGremlinQuery<TElement, TInVertex> IOrderedInEdgeGremlinQuery<TElement, TInVertex>.Where(Func<IOrderedInEdgeGremlinQuery<TElement, TInVertex>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IOrderedOutEdgeGremlinQuery<TResult, TOutVertex> IOrderedOutEdgeGremlinQuery<TElement, TOutVertex>.Cast<TResult>() => Cast<TResult>();
        IOrderedOutEdgeGremlinQuery<TElement, TOutVertex> IOrderedOutEdgeGremlinQuery<TElement, TOutVertex>.Coin(double probability) => Coin(probability);

        TTargetQuery IOrderedOutEdgeGremlinQuery<TElement, TOutVertex>.Aggregate<TTargetQuery>(Func<IOrderedOutEdgeGremlinQuery<TElement, TOutVertex>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);
        TTargetQuery IOrderedOutEdgeGremlinQuery<TElement, TOutVertex>.As<TTargetQuery>(Func<IOrderedOutEdgeGremlinQuery<TElement, TOutVertex>, StepLabel<IOrderedOutEdgeGremlinQuery<TElement, TOutVertex>, TElement>, TTargetQuery> continuation) => As(continuation);

        IArrayGremlinQuery<TElement[], IOutEdgeGremlinQuery<TElement, TOutVertex>> IOrderedOutEdgeGremlinQuery<TElement, TOutVertex>.Fold() => Fold<IOutEdgeGremlinQuery<TElement, TOutVertex>>();

        IOutEdgeGremlinQuery<TElement, TOutVertex> IOrderedOutEdgeGremlinQuery<TElement, TOutVertex>.Where(Func<IOrderedOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IOrderedVertexPropertyGremlinQuery<TResult, TPropertyValue> IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue>.Cast<TResult>() => Cast<TResult>();
        IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue> IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue>.Coin(double probability) => Coin(probability);

        TTargetQuery IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue>.Aggregate<TTargetQuery>(Func<IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);
        TTargetQuery IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue>.As<TTargetQuery>(Func<IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue>, StepLabel<IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue>, TElement>, TTargetQuery> continuation) => As(continuation);

        IArrayGremlinQuery<TElement[], IVertexPropertyGremlinQuery<TElement, TPropertyValue>> IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue>.Fold() => Fold<IVertexPropertyGremlinQuery<TElement, TPropertyValue>>();

        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue>.Where(Func<IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IOrderedVertexPropertyGremlinQuery<TResult, TPropertyValue, TMeta> IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Cast<TResult>() => Cast<TResult>();
        IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Coin(double probability) => Coin(probability);

        TTargetQuery IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Aggregate<TTargetQuery>(Func<IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);
        TTargetQuery IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.As<TTargetQuery>(Func<IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, StepLabel<IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, TElement>, TTargetQuery> continuation) => As(continuation);

        IArrayGremlinQuery<TElement[], IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>> IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Fold() => Fold<IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>>();

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Where(Func<IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IOrderedEdgePropertyGremlinQuery<TResult, TPropertyValue> IOrderedEdgePropertyGremlinQuery<TElement, TPropertyValue>.Cast<TResult>() => Cast<TResult>();
        IOrderedEdgePropertyGremlinQuery<TElement, TPropertyValue> IOrderedEdgePropertyGremlinQuery<TElement, TPropertyValue>.Coin(double probability) => Coin(probability);

        TTargetQuery IOrderedEdgePropertyGremlinQuery<TElement, TPropertyValue>.Aggregate<TTargetQuery>(Func<IOrderedEdgePropertyGremlinQuery<TElement, TPropertyValue>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);
        TTargetQuery IOrderedEdgePropertyGremlinQuery<TElement, TPropertyValue>.As<TTargetQuery>(Func<IOrderedEdgePropertyGremlinQuery<TElement, TPropertyValue>, StepLabel<IOrderedEdgePropertyGremlinQuery<TElement, TPropertyValue>, TElement>, TTargetQuery> continuation) => As(continuation);

        IArrayGremlinQuery<TElement[], IEdgePropertyGremlinQuery<TElement, TPropertyValue>> IOrderedEdgePropertyGremlinQuery<TElement, TPropertyValue>.Fold() => Fold<IEdgePropertyGremlinQuery<TElement, TPropertyValue>>();

        IEdgePropertyGremlinQuery<TElement, TPropertyValue> IOrderedEdgePropertyGremlinQuery<TElement, TPropertyValue>.Where(Func<IOrderedEdgePropertyGremlinQuery<TElement, TPropertyValue>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

    }
}
#pragma warning restore CS0109 // Member does not hide an inherited member; new keyword is not required

