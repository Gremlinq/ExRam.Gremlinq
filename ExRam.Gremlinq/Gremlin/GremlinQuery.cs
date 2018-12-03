using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using LanguageExt;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
// ReSharper disable ArrangeThisQualifier

namespace ExRam.Gremlinq
{
    internal sealed class GremlinQueryImpl<TElement, TOutVertex, TInVertex> :
        IVGremlinQuery<TElement>,
        IVPropertiesGremlinQuery<TElement>,
        IEGremlinQuery<TElement, TOutVertex, TInVertex>
    {
        public static readonly GremlinQueryImpl<TElement, TOutVertex, TInVertex> Anonymous = Create("__");

        public GremlinQueryImpl(IImmutableList<Step> steps, IImmutableDictionary<StepLabel, string> stepLabelBindings)
        {
            Steps = steps;
            StepLabelMappings = stepLabelBindings;
        }

        public static GremlinQueryImpl<TElement, TOutVertex, TInVertex> Create(string graphName = "g")
        {
            return new GremlinQueryImpl<TElement, TOutVertex, TInVertex>(ImmutableList<Step>.Empty.Add(new IdentifierStep(graphName)), ImmutableDictionary<StepLabel, string>.Empty);
        }

        #region AddV
        IVGremlinQuery<TNewVertex> IGremlinQuery.AddV<TNewVertex>(TNewVertex vertex) => AddV(vertex);

        IVGremlinQuery<TNewVertex> IGremlinQuery.AddV<TNewVertex>() => AddV(new TNewVertex());
        
        private GremlinQueryImpl<TNewVertex, TOutVertex, TInVertex> AddV<TNewVertex>(TNewVertex vertex)
        {
            return this
                .AddStep<TNewVertex>(new AddVStep(vertex))
                .AddStep(new AddElementPropertiesStep(vertex));
        }
        #endregion

        #region AddE
        IEGremlinQuery<TNewEdge> IGremlinQuery.AddE<TNewEdge>() => AddE(new TNewEdge());

        IEGremlinQuery<TNewEdge> IGremlinQuery.AddE<TNewEdge>(TNewEdge edge) => AddE(edge);

        IEGremlinQuery<TEdge, TElement> IVGremlinQuery<TElement>.AddE<TEdge>(TEdge edge) => AddE(edge);

        IEGremlinQuery<TNewEdge, TElement> IVGremlinQuery<TElement>.AddE<TNewEdge>() => AddE(new TNewEdge());

        private GremlinQueryImpl<TNewEdge, TElement, Unit> AddE<TNewEdge>(TNewEdge newEdge)
        {
            return this
                .AddStep<TNewEdge, TElement, Unit>(new AddEStep(newEdge))
                .AddStep(new AddElementPropertiesStep(newEdge));
        }
        #endregion

        #region Aggregate
        TTargetQuery IVGremlinQuery<TElement>.Aggregate<TTargetQuery>(Func<IVGremlinQuery<TElement>, VStepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IGremlinQuery<TElement>.Aggregate<TTargetQuery>(Func<IGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IEGremlinQuery<TElement>.Aggregate<TTargetQuery>(Func<IEGremlinQuery<TElement>, EStepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IEGremlinQuery<TElement, TOutVertex>.Aggregate<TTargetQuery>(Func<IEGremlinQuery<TElement, TOutVertex>, EStepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IOutEGremlinQuery<TElement, TOutVertex>.Aggregate<TTargetQuery>(Func<IOutEGremlinQuery<TElement, TOutVertex>, EStepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IInEGremlinQuery<TElement, TInVertex>.Aggregate<TTargetQuery>(Func<IInEGremlinQuery<TElement, TInVertex>, EStepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IEGremlinQuery<TElement, TOutVertex, TInVertex>.Aggregate<TTargetQuery>(Func<IEGremlinQuery<TElement, TOutVertex, TInVertex>, EStepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IVPropertiesGremlinQuery<TElement>.Aggregate<TTargetQuery>(Func<IVPropertiesGremlinQuery<TElement>, EStepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        private TTargetQuery Aggregate<TStepLabel, TTargetQuery>(Func<GremlinQueryImpl<TElement, TOutVertex, TInVertex>, TStepLabel, TTargetQuery> continuation)
            where TStepLabel : StepLabel, new()
            where TTargetQuery : IGremlinQuery
        {
            var stepLabel = new TStepLabel();

            return continuation(
                AddStep<TElement>(new MethodStep("aggregate", stepLabel)),
                stepLabel);
        }
        #endregion

        #region And
        // ReSharper disable once CoVariantArrayConversion
        IGremlinQuery<TElement> IGremlinQuery<TElement>.And(params Func<IGremlinQuery<TElement>, IGremlinQuery>[] andTraversals) => And(andTraversals);

        // ReSharper disable once CoVariantArrayConversion
        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.And(params Func<IVGremlinQuery<TElement>, IGremlinQuery>[] andTraversals) => And(andTraversals);

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex> And(params Func<GremlinQueryImpl<TElement, TOutVertex, TInVertex>, IGremlinQuery>[] andTraversals)
        {
            return AddStep<TElement>(new AndStep(andTraversals
                .Select(andTraversal => andTraversal(Anonymous))
                .ToArray()));
        }
        #endregion
        
        #region As
        TTargetQuery IVGremlinQuery<TElement>.As<TTargetQuery>(Func<IVGremlinQuery<TElement>, VStepLabel<TElement>, TTargetQuery> continuation) => As(continuation);
        
        TTargetQuery IGremlinQuery<TElement>.As<TTargetQuery>(Func<IGremlinQuery<TElement>, StepLabel<TElement>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IEGremlinQuery<TElement>.As<TTargetQuery>(Func<IEGremlinQuery<TElement>, EStepLabel<TElement>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IEGremlinQuery<TElement, TOutVertex>.As<TTargetQuery>(Func<IEGremlinQuery<TElement, TOutVertex>, EStepLabel<TElement, TOutVertex>, TTargetQuery> continuation) => As(continuation);
        
        TTargetQuery IOutEGremlinQuery<TElement, TOutVertex>.As<TTargetQuery>(Func<IOutEGremlinQuery<TElement, TOutVertex>, EStepLabel<TElement, TOutVertex>, TTargetQuery> continuation) => As(continuation);
        
        TTargetQuery IInEGremlinQuery<TElement, TInVertex>.As<TTargetQuery>(Func<IInEGremlinQuery<TElement, TInVertex>, EStepLabel<TElement, TInVertex>, TTargetQuery> continuation) => As(continuation);
        
        private TTargetQuery As<TStepLabel, TTargetQuery>(Func<GremlinQueryImpl<TElement, TOutVertex, TInVertex>, TStepLabel, TTargetQuery> continuation)
            where TStepLabel : StepLabel<TElement>, new()
            where TTargetQuery : IGremlinQuery
        {
            var stepLabel = new TStepLabel();

            return continuation(
                As(stepLabel),
                stepLabel);
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.As(StepLabel<TElement> stepLabel) => As(stepLabel);

        IInEGremlinQuery<TElement, TInVertex> IInEGremlinQuery<TElement, TInVertex>.As(InEStepLabel<TElement, TInVertex> stepLabel) => As(stepLabel);

        IOutEGremlinQuery<TElement, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.As(OutEStepLabel<TElement, TOutVertex> stepLabel) => As(stepLabel);

        IEGremlinQuery<TElement, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.As(EStepLabel<TElement, TOutVertex> stepLabel) => As(stepLabel);

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.As(EStepLabel<TElement> stepLabel) => As(stepLabel);

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.As(VStepLabel<TElement> stepLabel) => As(stepLabel);

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex> As(StepLabel<TElement> stepLabel) => AddStep<TElement>(new MethodStep("as", stepLabel));
        #endregion

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Barrier() => AddStep<TElement>(new MethodStep("barrier"));

        #region Cast
        IGremlinQuery<TTarget> IGremlinQuery.Cast<TTarget>() => Cast<TTarget>();

        IVGremlinQuery<TOtherVertex> IVGremlinQuery<TElement>.Cast<TOtherVertex>() => Cast<TOtherVertex>();

        IEGremlinQuery<TOtherEdge, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.Cast<TOtherEdge>() => Cast<TOtherEdge>();

        IEGremlinQuery<TOtherEdge> IEGremlinQuery<TElement>.Cast<TOtherEdge>() => Cast<TOtherEdge>();

        IEGremlinQuery<TOtherEdge, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.Cast<TOtherEdge>() => Cast<TOtherEdge>();

        IOutEGremlinQuery<TOtherEdge, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.Cast<TOtherEdge>() => Cast<TOtherEdge>();

        IInEGremlinQuery<TOtherEdge, TInVertex> IInEGremlinQuery<TElement, TInVertex>.Cast<TOtherEdge>() => Cast<TOtherEdge>();

        private GremlinQueryImpl<TTarget, TOutVertex, TInVertex> Cast<TTarget>() => new GremlinQueryImpl<TTarget, TOutVertex, TInVertex>(Steps, StepLabelMappings);
        #endregion

        #region Coalesce
        // ReSharper disable once CoVariantArrayConversion
        TTargetQuery IVGremlinQuery<TElement>.Coalesce<TTargetQuery>(params Func<IVGremlinQuery<TElement>, TTargetQuery>[] traversals) => Coalesce(traversals);

        // ReSharper disable once CoVariantArrayConversion
        TTargetQuery IGremlinQuery<TElement>.Coalesce<TTargetQuery>(params Func<IGremlinQuery<TElement>, TTargetQuery>[] traversals) => Coalesce(traversals);

        private TTargetQuery Coalesce<TTargetQuery>(params Func<GremlinQueryImpl<TElement, TOutVertex, TInVertex>, TTargetQuery>[] traversals)
            where TTargetQuery : IGremlinQuery
        {
            return this.AddStep<TElement>(new MethodStep("coalesce", traversals
                    .Select(traversal => (object)traversal(Anonymous))
                    .ToArray()))
                .CastQuery<TTargetQuery>();
        }
        #endregion

        IGremlinQuery<long> IGremlinQuery.Count() => AddStep<long>(new MethodStep("count"));

        #region Choose
        IGremlinQuery<TResult> IGremlinQuery<TElement>.Choose<TResult>(Func<IGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IGremlinQuery<TElement>, IGremlinQuery<TResult>> trueChoice, Func<IGremlinQuery<TElement>, IGremlinQuery<TResult>> falseChoice)
        {
            var anonymous = Anonymous;

            return AddStep<TResult>(new MethodStep("choose", traversalPredicate(anonymous), trueChoice(anonymous), falseChoice(anonymous)));
        }

        IGremlinQuery<TResult> IGremlinQuery<TElement>.Choose<TResult>(Func<IGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IGremlinQuery<TElement>, IGremlinQuery<TResult>> trueChoice)
        {
            var anonymous = Anonymous;

            return AddStep<TResult>(new MethodStep("choose", traversalPredicate(anonymous), trueChoice(anonymous)));
        }
        #endregion

        #region BothX
        IVGremlinQuery<Vertex> IVGremlinQuery<TElement>.Both<TNewEdge>() => AddStep<Vertex>(new DerivedLabelNamesStep<TNewEdge>("both"));

        IEGremlinQuery<TNewEdge> IVGremlinQuery<TElement>.BothE<TNewEdge>() => AddStep<TNewEdge>(new DerivedLabelNamesStep<TNewEdge>("bothE"));

        IVGremlinQuery<Vertex> IEGremlinQuery<TElement>.BothV() => AddStep<Vertex>(new MethodStep("bothV"));
        #endregion

        #region Dedup
        IGremlinQuery<TElement> IGremlinQuery<TElement>.Dedup() => Dedup();

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Dedup() => Dedup();

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.Dedup() => Dedup();

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex> Dedup() => AddStep<TElement>(new MethodStep("dedup"));
        #endregion

        IGremlinQuery<Unit> IGremlinQuery.Drop() => AddStep<Unit>(new MethodStep("drop"));

        IEGremlinQuery<Edge> IGremlinQuery.E(params object[] ids) => AddStep<Edge>(new MethodStep("E", ids));

        #region Emit
        IGremlinQuery<TElement> IGremlinQuery<TElement>.Emit() => Emit();

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Emit() => Emit();

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.Emit() => Emit();

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex> Emit() => AddStep<TElement>(new MethodStep("emit"));
        #endregion

        IGremlinQuery<string> IGremlinQuery.Explain() => AddStep<string>(new MethodStep("explain"));

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Filter(string lambda) => AddStep<TElement>(new MethodStep("filter", new Lambda(lambda)));

        IGremlinQuery<TElement[]> IGremlinQuery<TElement>.Fold() => AddStep<TElement[]>(new MethodStep("fold"));

        #region From (step label)
        IOutEGremlinQuery<TElement, TNewOutVertex> IEGremlinQuery<TElement>.From<TNewOutVertex>(StepLabel<TNewOutVertex> stepLabel) => AddStep<TElement, TNewOutVertex, Unit>(new MethodStep("from", stepLabel));

        IEGremlinQuery<TElement, TTargetVertex, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.From<TTargetVertex>(StepLabel<TTargetVertex> stepLabel) => AddStep<TElement, TTargetVertex, TOutVertex>(new MethodStep("from", stepLabel));
        #endregion

        #region From (traversal)
        IEGremlinQuery<TElement, TTargetVertex, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.From<TTargetVertex>(Func<IVGremlinQuery<TOutVertex>, IGremlinQuery<TTargetVertex>> fromVertexTraversal) => AddStep<TElement, TTargetVertex, TOutVertex>(new MethodStep("from", fromVertexTraversal(GremlinQueryImpl<TOutVertex, Unit, Unit>.Anonymous)));

        IOutEGremlinQuery<TElement, TNewOutVertex> IEGremlinQuery<TElement>.From<TNewOutVertex>(Func<IGremlinQuery, IGremlinQuery<TNewOutVertex>> fromVertexTraversal) => From<TElement, TNewOutVertex, Unit>(fromVertexTraversal);

        IEGremlinQuery<TElement, TNewOutVertex, TInVertex> IInEGremlinQuery<TElement, TInVertex>.From<TNewOutVertex>(Func<IGremlinQuery, IGremlinQuery<TNewOutVertex>> fromVertexTraversal) => From<TElement, TNewOutVertex, TInVertex>(fromVertexTraversal);

        private GremlinQueryImpl<TNewElement, TNewOutVertex, TNewInVertex> From<TNewElement, TNewOutVertex, TNewInVertex>(Func<GremlinQueryImpl<TElement, TOutVertex, TInVertex>, IGremlinQuery> fromVertexTraversal) => AddStep<TNewElement, TNewOutVertex, TNewInVertex>(new MethodStep("from", fromVertexTraversal(Anonymous)));
        #endregion

        IGremlinQuery<object> IGremlinQuery.Id() => AddStep<object>(new MethodStep("id"));

        #region Identity
        IGremlinQuery<TElement> IGremlinQuery<TElement>.Identity() => Identity();

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Identity() => Identity();

        IOutEGremlinQuery<TElement, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.Identity() => Identity();

        IEGremlinQuery<TElement, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.Identity() => Identity();

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.Identity() => Identity();

        IEGremlinQuery<TElement, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.Identity() => Identity();

        IInEGremlinQuery<TElement, TInVertex> IInEGremlinQuery<TElement, TInVertex>.Identity() => Identity();

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex> Identity() => AddStep<TElement>(new MethodStep("identity"));
        #endregion

        IVGremlinQuery<Vertex> IVGremlinQuery<TElement>.In<TNewEdge>() => AddStep<Vertex>(new DerivedLabelNamesStep<TNewEdge>("in"));

        IGremlinQuery<TTarget> IGremlinQuery.InsertStep<TTarget>(int index, Step step) => new GremlinQueryImpl<TTarget, TOutVertex, TInVertex>(Steps.Insert(index, step), StepLabelMappings);

        IInEGremlinQuery<TNewEdge, TElement> IVGremlinQuery<TElement>.InE<TNewEdge>() => AddStep<TNewEdge, Unit, TElement>(new DerivedLabelNamesStep<TNewEdge>("inE"));

        #region InV
        IVGremlinQuery<Vertex> IEGremlinQuery<TElement>.InV() => AddStep<Vertex, Unit, Unit>(new MethodStep("inV"));

        IVGremlinQuery<TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.InV() => AddStep<TInVertex, Unit, Unit>(new MethodStep("inV"));

        IVGremlinQuery<TInVertex> IInEGremlinQuery<TElement, TInVertex>.InV() => AddStep<TInVertex, Unit, Unit>(new MethodStep("inV"));
        #endregion

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Inject(params TElement[] elements) => AddStep<TElement>(new MethodStep("inject", elements.Cast<object>().ToArray()));

        #region Limit
        IGremlinQuery<TElement> IGremlinQuery<TElement>.Limit(long limit) => Limit(limit);

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Limit(long limit) => Limit(limit);

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.Limit(long limit) => Limit(limit);

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex> Limit(long limit)
        {
            // This is the easier workaround for https://feedback.azure.com/forums/263030-azure-cosmos-db/suggestions/33998623-cosmosdb-s-implementation-of-the-tinkerpop-dsl-has
            // 4 billion should be enough for everyone (tm).
            if (limit > int.MaxValue || limit < 0)
                throw new ArgumentException("Parameter out of range.", nameof(limit));

            return AddStep(new LimitStep((int)limit));
        }

        #endregion

        #region Local
        TTargetQuery IEGremlinQuery<TElement>.Local<TTargetQuery>(Func<IEGremlinQuery<TElement>, TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IVGremlinQuery<TElement>.Local<TTargetQuery>(Func<IVGremlinQuery<TElement>, TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IGremlinQuery<TElement>.Local<TTargetQuery>(Func<IGremlinQuery<TElement>, TTargetQuery> localTraversal) => Local(localTraversal);

        private TTargetQuery Local<TTargetQuery>(Func<GremlinQueryImpl<TElement, TOutVertex, TInVertex>, TTargetQuery> localTraversal)
            where TTargetQuery : IGremlinQuery
        {
            return this.AddStep<TElement>(new MethodStep("local", localTraversal(Anonymous)))
                .CastQuery<TTargetQuery>();
        }
        #endregion

        #region Map
        TTargetQuery IGremlinQuery.Map<TTargetQuery>(Func<IGremlinQuery, TTargetQuery> mapping) => Map(mapping);

        TTargetQuery IEGremlinQuery<TElement, TOutVertex>.Map<TTargetQuery>(Func<IEGremlinQuery<TElement, TOutVertex>, TTargetQuery> mapping) => Map(mapping);

        TTargetQuery IOutEGremlinQuery<TElement, TOutVertex>.Map<TTargetQuery>(Func<IOutEGremlinQuery<TElement, TOutVertex>, TTargetQuery> mapping) => Map(mapping);

        TTargetQuery IEGremlinQuery<TElement, TOutVertex, TInVertex>.Map<TTargetQuery>(Func<IEGremlinQuery<TElement, TOutVertex, TInVertex>, TTargetQuery> mapping) => Map(mapping);

        TTargetQuery IInEGremlinQuery<TElement, TInVertex>.Map<TTargetQuery>(Func<IInEGremlinQuery<TElement, TInVertex>, TTargetQuery> mapping) => Map(mapping);

        TTargetQuery IGremlinQuery<TElement>.Map<TTargetQuery>(Func<IGremlinQuery<TElement>, TTargetQuery> mapping) => Map(mapping);

        TTargetQuery IVGremlinQuery<TElement>.Map<TTargetQuery>(Func<IVGremlinQuery<TElement>, TTargetQuery> mapping) => Map(mapping);

        TTargetQuery IEGremlinQuery<TElement>.Map<TTargetQuery>(Func<IEGremlinQuery<TElement>, TTargetQuery> mapping) => Map(mapping);

        private TTargetQuery Map<TTargetQuery>(Func<GremlinQueryImpl<TElement, TOutVertex, TInVertex>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery
        {
            return this.AddStep<TElement>(new MethodStep("map", mapping(Anonymous)))
                .CastQuery<TTargetQuery>();
        }
        #endregion

        // ReSharper disable once CoVariantArrayConversion
        IGremlinQuery<TElement> IGremlinQuery<TElement>.Match(params Func<IGremlinQuery<TElement>, IGremlinQuery<TElement>>[] matchTraversals) => AddStep<TElement>(new MethodStep("match", matchTraversals.Select(traversal => traversal(Anonymous)).ToArray<object>()));

        #region Not
        IGremlinQuery<TElement> IGremlinQuery<TElement>.Not(Func<IGremlinQuery<TElement>, IGremlinQuery> notTraversal) => Not(notTraversal);

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Not(Func<IVGremlinQuery<TElement>, IGremlinQuery> notTraversal) => Not(notTraversal);

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex> Not(Func<GremlinQueryImpl<TElement, TOutVertex, TInVertex>, IGremlinQuery> notTraversal) => AddStep(new NotStep(notTraversal(Anonymous)));
        #endregion

        #region OfType
        IGremlinQuery<TTarget> IGremlinQuery.OfType<TTarget>() => OfType<TTarget>();
        
        IVGremlinQuery<TTarget> IVGremlinQuery<TElement>.OfType<TTarget>() => OfType<TTarget>();

        IEGremlinQuery<TTarget, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.OfType<TTarget>() => OfType<TTarget>();

        IEGremlinQuery<TTarget> IEGremlinQuery<TElement>.OfType<TTarget>() => OfType<TTarget>();

        IEGremlinQuery<TTarget, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.OfType<TTarget>() => OfType<TTarget>();

        IOutEGremlinQuery<TTarget, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.OfType<TTarget>() => OfType<TTarget>();

        IInEGremlinQuery<TTarget, TInVertex> IInEGremlinQuery<TElement, TInVertex>.OfType<TTarget>() => OfType<TTarget>();

        private GremlinQueryImpl<TTarget, TOutVertex, TInVertex> OfType<TTarget>() => AddStep<TTarget>(new DerivedLabelNamesStep<TTarget>("hasLabel"));
        #endregion

        #region Optional
        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Optional(Func<IVGremlinQuery<TElement>, IVGremlinQuery<TElement>> optionalTraversal) => Optional(optionalTraversal);

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Optional(Func<IGremlinQuery<TElement>, IGremlinQuery<TElement>> optionalTraversal) => Optional(optionalTraversal);

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex> Optional(Func<GremlinQueryImpl<TElement, TOutVertex, TInVertex>, IGremlinQuery> optionalTraversal) => AddStep<TElement>(new MethodStep("optional", optionalTraversal(Anonymous)));
        #endregion

        #region Or
        // ReSharper disable once CoVariantArrayConversion
        IGremlinQuery<TElement> IGremlinQuery<TElement>.Or(params Func<IGremlinQuery<TElement>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        // ReSharper disable once CoVariantArrayConversion
        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Or(params Func<IVGremlinQuery<TElement>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex> Or(params Func<GremlinQueryImpl<TElement, TOutVertex, TInVertex>, IGremlinQuery>[] orTraversals)
        {
            return AddStep<TElement>(new OrStep(orTraversals
                .Select(orTraversal => orTraversal(Anonymous))
                .ToArray()));
        }
        #endregion

        #region OrderBy{Descending} projection
        IGremlinQuery<TElement> IGremlinQuery<TElement>.OrderBy(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Increasing);

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.OrderBy(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Increasing);

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.OrderBy(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Increasing);

        IOutEGremlinQuery<TElement, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.OrderBy(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Increasing);

        IInEGremlinQuery<TElement, TInVertex> IInEGremlinQuery<TElement, TInVertex>.OrderBy(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Increasing);

        IEGremlinQuery<TElement, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.OrderBy(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Increasing);

        IGremlinQuery<TElement> IGremlinQuery<TElement>.OrderByDescending(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Decreasing);

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.OrderByDescending(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Decreasing);

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.OrderByDescending(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Decreasing);

        IOutEGremlinQuery<TElement, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.OrderByDescending(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Decreasing);

        IInEGremlinQuery<TElement, TInVertex> IInEGremlinQuery<TElement, TInVertex>.OrderByDescending(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Decreasing);

        IEGremlinQuery<TElement, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.OrderByDescending(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Decreasing);

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex> OrderBy(Expression<Func<TElement, object>> projection, Order order)
        {
            if (projection.Body.StripConvert() is MemberExpression memberExpression)
            {
                return this.AddStep<TElement>(new MethodStep("order")).AddStep<TElement>(new MethodStep("by", memberExpression.Member.Name, order));
            }

            throw new NotSupportedException();
        }
        #endregion

        #region OrderBy{Descending} traversal
        IGremlinQuery<TElement> IGremlinQuery<TElement>.OrderBy(Func<IGremlinQuery<TElement>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Increasing);

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.OrderBy(Func<IVGremlinQuery<TElement>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Increasing);

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.OrderBy(Func<IEGremlinQuery<TElement>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Increasing);

        IOutEGremlinQuery<TElement, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.OrderBy(Func<IOutEGremlinQuery<TElement, TOutVertex>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Increasing);

        IInEGremlinQuery<TElement, TInVertex> IInEGremlinQuery<TElement, TInVertex>.OrderBy(Func<IInEGremlinQuery<TElement, TInVertex>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Increasing);

        IEGremlinQuery<TElement, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.OrderBy(Func<IEGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Increasing);

        IGremlinQuery<TElement> IGremlinQuery<TElement>.OrderByDescending(Func<IGremlinQuery<TElement>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Decreasing);

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.OrderByDescending(Func<IVGremlinQuery<TElement>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Decreasing);

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.OrderByDescending(Func<IEGremlinQuery<TElement>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Decreasing);
        
        IOutEGremlinQuery<TElement, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.OrderByDescending(Func<IOutEGremlinQuery<TElement, TOutVertex>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Decreasing);

        IInEGremlinQuery<TElement, TInVertex> IInEGremlinQuery<TElement, TInVertex>.OrderByDescending(Func<IInEGremlinQuery<TElement, TInVertex>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Decreasing);

        IEGremlinQuery<TElement, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.OrderByDescending(Func<IEGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Decreasing);

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex> OrderBy(Func<GremlinQueryImpl<TElement, TOutVertex, TInVertex>, IGremlinQuery> traversal, Order order)
        {
            return this.AddStep<TElement>(new MethodStep("order")).AddStep<TElement>(new MethodStep("by", traversal(Anonymous), order));
        }
        #endregion

        #region OrderBy{Descending} lambda
        IGremlinQuery<TElement> IGremlinQuery<TElement>.OrderBy(string lambda) => OrderBy(lambda);

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.OrderBy(string lambda) => OrderBy(lambda);

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.OrderBy(string lambda) => OrderBy(lambda);

        IOutEGremlinQuery<TElement, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.OrderBy(string lambda) => OrderBy(lambda);

        IInEGremlinQuery<TElement, TInVertex> IInEGremlinQuery<TElement, TInVertex>.OrderBy(string lambda) => OrderBy(lambda);

        IEGremlinQuery<TElement, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.OrderBy(string lambda) => OrderBy(lambda);
        
        private GremlinQueryImpl<TElement, TOutVertex, TInVertex> OrderBy(string lambda)
        {
            return this.AddStep<TElement>(new MethodStep("order")).AddStep<TElement>(new MethodStep("by", new Lambda(lambda)));
        }
        #endregion

        IVGremlinQuery<Vertex> IEGremlinQuery<TElement>.OtherV() => AddStep<Vertex>(new MethodStep("otherV"));

        #region OutX
        IOutEGremlinQuery<TNewEdge, TElement> IVGremlinQuery<TElement>.OutE<TNewEdge>() => AddStep<TNewEdge, TElement, Unit>(new DerivedLabelNamesStep<TNewEdge>("outE"));

        IVGremlinQuery<Vertex> IEGremlinQuery<TElement>.OutV() => AddStep<Vertex, Unit, Unit>(new MethodStep("outV"));
        
        IVGremlinQuery<TOutVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.OutV() => AddStep<TOutVertex, Unit, Unit>(new MethodStep("outV"));

        IVGremlinQuery<TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.OutV() => AddStep<TOutVertex, Unit, Unit>(new MethodStep("outV"));

        IVGremlinQuery<Vertex> IVGremlinQuery<TElement>.Out<TNewEdge>() => AddStep<Vertex>(new DerivedLabelNamesStep<TNewEdge>("out"));
        #endregion
        
        IGremlinQuery<string> IGremlinQuery.Profile() => AddStep<string>(new MethodStep("profile"));

        #region Properties
        IVPropertiesGremlinQuery<VertexProperty> IVGremlinQuery<TElement>.Properties(params Expression<Func<TElement, object>>[] projections) => Properties(projections);

        IGremlinQuery<Property> IVPropertiesGremlinQuery<TElement>.Properties(params string[] keys)
        {
            return AddStep<Property, Unit, Unit>(new MethodStep("properties", keys.ToArray<object>()));
        }

        private GremlinQueryImpl<VertexProperty, Unit, Unit> Properties(params Expression<Func<TElement, object>>[] projections)
        {
            return AddStep<VertexProperty, Unit, Unit>(new MethodStep("properties", projections
                .Select(projection =>
                {
                    if (projection.Body.StripConvert() is MemberExpression memberExpression)
                    {
                        return memberExpression.Member.Name;
                    }

                    throw new NotSupportedException();
                })
                .ToArray<object>()));
        }
        #endregion
        
        #region Property
        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Property<TValue>(Expression<Func<TElement, TValue>> projection, TValue value) => Property(projection, value);
 
        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Property<TValue>(Expression<Func<TElement, TValue[]>> projection, TValue value) => Property(projection, value);

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.Property<TValue>(Expression<Func<TElement, TValue>> projection, TValue value) => Property(projection, value);

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.Property<TValue>(Expression<Func<TElement, TValue[]>> projection, TValue value) => Property(projection, value);

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex> Property<TProperty>(Expression<Func<TElement, TProperty>> projection, object value)
        {
            if (projection.Body.StripConvert() is MemberExpression memberExpression)
            {
                return AddStep(new PropertyStep(memberExpression, value));
            }

            throw new NotSupportedException();
        }

        IVPropertiesGremlinQuery<TElement> IVPropertiesGremlinQuery<TElement>.Property(string key, object value)
        {
            return AddStep<TElement>(new MethodStep("property", key, value));
        }
        #endregion

        #region Range
        IGremlinQuery<TElement> IGremlinQuery<TElement>.Range(long low, long high) => Range(low, high);

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Range(long low, long high) => Range(low, high);

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.Range(long low, long high) => Range(low, high);

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex> Range(long low, long high)
        {
            // This is the easier workaround for https://feedback.azure.com/forums/263030-azure-cosmos-db/suggestions/33998623-cosmosdb-s-implementation-of-the-tinkerpop-dsl-has
            // 4 billion should be enough for everyone (tm).
            if (low > int.MaxValue || low < 0)
                throw new ArgumentException("Parameter out of range.", nameof(low));

            if (high > int.MaxValue || high < 0)
                throw new ArgumentException("Parameter out of range.", nameof(high));

            return AddStep<TElement>(new MethodStep("range", (int)low, (int)high));
        }
        #endregion

        #region Repeat
        IGremlinQuery<TElement> IGremlinQuery<TElement>.Repeat(Func<IGremlinQuery<TElement>, IGremlinQuery<TElement>> repeatTraversal) => Repeat(repeatTraversal);

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Repeat(Func<IVGremlinQuery<TElement>, IVGremlinQuery<TElement>> repeatTraversal) => Repeat(repeatTraversal);

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.Repeat(Func<IEGremlinQuery<TElement>, IEGremlinQuery<TElement>> repeatTraversal) => Repeat(repeatTraversal);
        
        private GremlinQueryImpl<TElement, TOutVertex, TInVertex> Repeat(Func<GremlinQueryImpl<TElement, TOutVertex, TInVertex>, IGremlinQuery> repeatTraversal) => AddStep<TElement>(new MethodStep("repeat", repeatTraversal(Anonymous)));
        #endregion

        #region RepeatUntil
        IGremlinQuery<TElement> IGremlinQuery<TElement>.RepeatUntil(Func<IGremlinQuery<TElement>, IGremlinQuery<TElement>> repeatTraversal, Func<IGremlinQuery<TElement>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.RepeatUntil(Func<IVGremlinQuery<TElement>, IVGremlinQuery<TElement>> repeatTraversal, Func<IVGremlinQuery<TElement>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.RepeatUntil(Func<IEGremlinQuery<TElement>, IEGremlinQuery<TElement>> repeatTraversal, Func<IEGremlinQuery<TElement>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex> RepeatUntil(Func<GremlinQueryImpl<TElement, TOutVertex, TInVertex>, IGremlinQuery> repeatTraversal, Func<GremlinQueryImpl<TElement, TOutVertex, TInVertex>, IGremlinQuery> untilTraversal)
        {
            return this.AddStep<TElement>(new MethodStep("repeat", repeatTraversal(Anonymous))).AddStep<TElement>(new MethodStep("until", untilTraversal(Anonymous)));
        }
        #endregion

        #region Select
        IGremlinQuery<TStep> IGremlinQuery.Select<TStep>(StepLabel<TStep> label) => Select<TStep, Unit, Unit>(label);

        IVGremlinQuery<TVertex> IGremlinQuery.Select<TVertex>(VStepLabel<TVertex> label) => Select<TVertex, Unit, Unit>(label);

        IEGremlinQuery<TEdge> IGremlinQuery.Select<TEdge>(EStepLabel<TEdge> label) => Select<TEdge, Unit, Unit>(label);

        IEGremlinQuery<TEdge, TAdjacentVertex> IGremlinQuery.Select<TEdge, TAdjacentVertex>(EStepLabel<TEdge, TAdjacentVertex> label) => Select<TEdge, TAdjacentVertex, Unit>(label);

        IOutEGremlinQuery<TEdge, TAdjacentVertex> IGremlinQuery.Select<TEdge, TAdjacentVertex>(OutEStepLabel<TEdge, TAdjacentVertex> label) => Select<TEdge, TAdjacentVertex, Unit>(label);

        IInEGremlinQuery<TEdge, TAdjacentVertex> IGremlinQuery.Select<TEdge, TAdjacentVertex>(InEStepLabel<TEdge, TAdjacentVertex> label) => Select<TEdge, Unit, TAdjacentVertex>(label);

        private GremlinQueryImpl<TSelectedElement, TSelectedOutVertex, TSelectedInVertex> Select<TSelectedElement, TSelectedOutVertex, TSelectedInVertex>(StepLabel<TSelectedElement> stepLabel) => AddStep<TSelectedElement, TSelectedOutVertex, TSelectedInVertex>(new MethodStep("select", stepLabel));

        IGremlinQuery<(T1, T2)> IGremlinQuery.Select<T1, T2>(StepLabel<T1> label1, StepLabel<T2> label2)
        {
            return this.AddStep<(T1, T2)>(new MethodStep("select", label1, label2))
                .AddStepLabelBinding(x => x.Item1, label1)
                .AddStepLabelBinding(x => x.Item2, label2);
        }

        IGremlinQuery<(T1, T2, T3)> IGremlinQuery.Select<T1, T2, T3>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3)
        {
            return this.AddStep<(T1, T2, T3)>(new MethodStep("select", label1, label2, label3))
                .AddStepLabelBinding(x => x.Item1, label1)
                .AddStepLabelBinding(x => x.Item2, label2)
                .AddStepLabelBinding(x => x.Item3, label3);
        }

        IGremlinQuery<(T1, T2, T3, T4)> IGremlinQuery.Select<T1, T2, T3, T4>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4)
        {
            return this.AddStep<(T1, T2, T3, T4)>(new MethodStep("select", label1, label2, label3, label4))
                .AddStepLabelBinding(x => x.Item1, label1)
                .AddStepLabelBinding(x => x.Item2, label2)
                .AddStepLabelBinding(x => x.Item3, label3)
                .AddStepLabelBinding(x => x.Item4, label4);
        }
        #endregion

        #region SideEffect
        IGremlinQuery<TElement> IGremlinQuery<TElement>.SideEffect(Func<IGremlinQuery<TElement>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.SideEffect(Func<IVGremlinQuery<TElement>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.SideEffect(Func<IEGremlinQuery<TElement>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IVPropertiesGremlinQuery<TElement> IVPropertiesGremlinQuery<TElement>.SideEffect(Func<IVPropertiesGremlinQuery<TElement>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex> SideEffect(Func<GremlinQueryImpl<TElement, TOutVertex, TInVertex>, IGremlinQuery> sideEffectTraversal) => AddStep<TElement>(new MethodStep("sideEffect", sideEffectTraversal(Anonymous)));
        #endregion

        #region Skip
        IGremlinQuery<TElement> IGremlinQuery<TElement>.Skip(long skip) => Skip(skip);

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Skip(long skip) => Skip(skip);

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.Skip(long skip) => Skip( skip);
        
        private GremlinQueryImpl<TElement, TOutVertex, TInVertex> Skip(long skip)
        {
            // This is the easier workaround for https://feedback.azure.com/forums/263030-azure-cosmos-db/suggestions/33998623-cosmosdb-s-implementation-of-the-tinkerpop-dsl-has
            // 4 billion should be enough for everyone (tm).
            if (skip > int.MaxValue || skip < 0)
                throw new ArgumentException("Parameter out of range.", nameof(skip));

            return AddStep<TElement>(new MethodStep("skip", (int)skip));
        }

        #endregion

        #region Sum
        IGremlinQuery<TElement> IGremlinQuery<TElement>.SumLocal() => AddStep<TElement>(new MethodStep("sum", Scope.Local));

        IGremlinQuery<TElement> IGremlinQuery<TElement>.SumGlobal() => AddStep<TElement>(new MethodStep("sum", Scope.Global));
        #endregion

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Times(int count) => AddStep<TElement>(new MethodStep("times", count));

        #region Tail
        IGremlinQuery<TElement> IGremlinQuery<TElement>.Tail(long limit) => Tail(limit);

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Tail(long limit) => Tail(limit);

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.Tail(long limit) => Tail(limit);

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex> Tail(long limit)
        {
            // This is the easier workaround for https://feedback.azure.com/forums/263030-azure-cosmos-db/suggestions/33998623-cosmosdb-s-implementation-of-the-tinkerpop-dsl-has
            // 4 billion should be enough for everyone (tm).
            if (limit > int.MaxValue || limit < 0)
                throw new ArgumentException("Parameter out of range.", nameof(limit));

            return AddStep<TElement>(new MethodStep("tail", (int)limit));
        }
        #endregion

        #region To (step label)
        IInEGremlinQuery<TElement, TNewInVertex> IEGremlinQuery<TElement>.To<TNewInVertex>(StepLabel<TNewInVertex> stepLabel) => AddStep<TElement, Unit, TNewInVertex>(new MethodStep("to", stepLabel));

        IEGremlinQuery<TElement, TOutVertex, TTargetVertex> IEGremlinQuery<TElement, TOutVertex>.To<TTargetVertex>(StepLabel<TTargetVertex> stepLabel) => AddStep<TElement, TOutVertex, TTargetVertex>(new MethodStep("to", stepLabel));
        #endregion

        #region To (traversal)
        IEGremlinQuery<TElement, TOutVertex, TTargetVertex> IEGremlinQuery<TElement, TOutVertex>.To<TTargetVertex>(Func<IVGremlinQuery<TOutVertex>, IGremlinQuery<TTargetVertex>> toVertexTraversal) => AddStep<TElement, TOutVertex, TTargetVertex>(new MethodStep("to", toVertexTraversal(GremlinQueryImpl<TOutVertex, Unit, Unit>.Anonymous)));

        IInEGremlinQuery<TElement, TNewInVertex> IEGremlinQuery<TElement>.To<TNewInVertex>(Func<IGremlinQuery, IGremlinQuery<TNewInVertex>> toVertexTraversal) => To<TElement, Unit, TNewInVertex>(toVertexTraversal);

        IEGremlinQuery<TElement, TOutVertex, TNewInVertex> IOutEGremlinQuery<TElement, TOutVertex>.To<TNewInVertex>(Func<IGremlinQuery, IGremlinQuery<TNewInVertex>> toVertexTraversal) => To<TElement, TOutVertex, TNewInVertex>(toVertexTraversal);

        private GremlinQueryImpl<TNewElement, TNewOutVertex, TNewInVertex> To<TNewElement, TNewOutVertex, TNewInVertex>(Func<GremlinQueryImpl<TElement, TOutVertex, TInVertex>, IGremlinQuery> toVertexTraversal) => AddStep<TNewElement, TNewOutVertex, TNewInVertex>(new MethodStep("to", toVertexTraversal(Anonymous)));
        #endregion

        IGremlinQuery<TItem> IGremlinQuery<TElement>.Unfold<TItem>()
        {
            return AddStep<TItem>(new MethodStep("unfold"));
        }
        
        TTargetQuery IVGremlinQuery<TElement>.Union<TTargetQuery>(params Func<IVGremlinQuery<TElement>, TTargetQuery>[] unionTraversals)
        {
            return this
                .AddStep<TElement>(
                    new MethodStep(
                        "union",
                        unionTraversals
                            .Select(unionTraversal => (object)unionTraversal(Anonymous))
                            .ToArray()))
                .CastQuery<TTargetQuery>();
        }

        IVGremlinQuery<Vertex> IGremlinQuery.V(params object[] ids) => AddStep<Vertex>(new MethodStep("V", ids));

        IGremlinQuery<TTarget> IGremlinQuery<TElement>.Values<TTarget>(params Expression<Func<TElement, TTarget>>[] projections) => AddStep<TTarget>(new ValuesStep<TElement, TTarget>(projections));

        #region Where (Traversal)
        IGremlinQuery<TElement> IGremlinQuery<TElement>.Where(Func<IGremlinQuery<TElement>, IGremlinQuery> filterTraversal) => AddStep<TElement>(new MethodStep("where", filterTraversal(Anonymous)));

        IEGremlinQuery<TElement, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.Where(Func<IEGremlinQuery<TElement, TOutVertex>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.Where(Func<IEGremlinQuery<TElement>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IEGremlinQuery<TElement, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.Where(Func<IEGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Where(Func<IVGremlinQuery<TElement>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IOutEGremlinQuery<TElement, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.Where(Func<IOutEGremlinQuery<TElement, TOutVertex>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IInEGremlinQuery<TElement, TInVertex> IInEGremlinQuery<TElement, TInVertex>.Where(Func<IInEGremlinQuery<TElement, TInVertex>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex> Where(Func<GremlinQueryImpl<TElement, TOutVertex, TInVertex>, IGremlinQuery> filterTraversal) => AddStep<TElement>(new MethodStep("where", filterTraversal(Anonymous)));
        #endregion

        #region Where (Predicate)
        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Where(Expression<Func<TElement, bool>> predicate) => Where(predicate);

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Where(Expression<Func<TElement, bool>> predicate) => Where(predicate);

        IEGremlinQuery<TElement, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.Where(Expression<Func<TElement, bool>> predicate) => Where(predicate);

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.Where(Expression<Func<TElement, bool>> predicate) => Where(predicate);

        IEGremlinQuery<TElement, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.Where(Expression<Func<TElement, bool>> predicate) => Where(predicate);

        IOutEGremlinQuery<TElement, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.Where(Expression<Func<TElement, bool>> predicate) => Where(predicate);

        IInEGremlinQuery<TElement, TInVertex> IInEGremlinQuery<TElement, TInVertex>.Where(Expression<Func<TElement, bool>> predicate) => Where(predicate);

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex> Where(Expression<Func<TElement, bool>> predicate)
        {
            var body = predicate.Body;

            switch (body)
            {
                case UnaryExpression unaryExpression:
                {
                    if (unaryExpression.NodeType == ExpressionType.Not)
                        return Not(_ => _.Where(Expression.Lambda<Func<TElement, bool>>(unaryExpression.Operand, predicate.Parameters)));

                    break;
                }
                case MemberExpression memberExpression:
                {
                    if (memberExpression.Member is PropertyInfo property)
                    {
                        if (property.PropertyType == typeof(bool))
                            return Where(predicate.Parameters[0], memberExpression, Expression.Constant(true), ExpressionType.Equal);
                    }

                    break;
                }
                case BinaryExpression binaryExpression:
                    return Where(predicate.Parameters[0], binaryExpression.Left.StripConvert(), binaryExpression.Right.StripConvert(), binaryExpression.NodeType);
                case MethodCallExpression methodCallExpression:
                {
                    var methodInfo = methodCallExpression.Method;

                    if (methodInfo.DeclaringType == typeof(Enumerable) || methodInfo.DeclaringType == typeof(EnumerableExtensions))
                    {
                        // ReSharper disable once SwitchStatementMissingSomeCases
                        switch (methodInfo.Name)
                        {
                            case nameof(EnumerableExtensions.Intersects) when methodInfo.GetParameters().Length == 2:
                            case nameof(Enumerable.Contains) when methodInfo.GetParameters().Length == 2:
                            {
                                if (methodCallExpression.Arguments[0] is MemberExpression sourceMember)
                                {
                                    if (methodInfo.Name == nameof(EnumerableExtensions.Intersects))
                                        return HasWithin(sourceMember, methodCallExpression.Arguments[1]);

                                    if (sourceMember.Expression == predicate.Parameters[0])
                                        return AddStep(new HasStep(sourceMember, new P.Eq(methodCallExpression.Arguments[1].GetValue())));
                                }

                                if (methodCallExpression.Arguments[1] is MemberExpression argument && argument.Expression == predicate.Parameters[0])
                                    return HasWithin(argument, methodCallExpression.Arguments[0]);

                                break;
                            }
                            case nameof(Enumerable.Any) when methodInfo.GetParameters().Length == 1:
                                return Where(predicate.Parameters[0], methodCallExpression.Arguments[0], Expression.Constant(null, methodCallExpression.Arguments[0].Type), ExpressionType.NotEqual);
                        }
                    }
                    else if (methodInfo.DeclaringType == typeof(string))
                    {
                        if (methodInfo.Name == nameof(string.StartsWith))
                        {
                            if (methodCallExpression.Arguments[0] is MemberExpression argumentExpression && argumentExpression.Expression == predicate.Parameters[0])
                            {
                                if (methodCallExpression.Object.GetValue() is string stringValue)
                                {
                                    return HasWithin(
                                        argumentExpression,
                                        Enumerable
                                            .Range(0, stringValue.Length + 1)
                                            .Select(i => stringValue.Substring(0, i)));
                                }
                            }
                            else if (methodCallExpression.Object is MemberExpression memberExpression && memberExpression.Expression == predicate.Parameters[0])
                            {
                                if (methodCallExpression.Arguments[0].GetValue() is string lowerBound)
                                {
                                    string upperBound;

                                    if (lowerBound.Length == 0)
                                        return AddStep(new HasStep(memberExpression));

                                    if (lowerBound[lowerBound.Length - 1] == char.MaxValue)
                                        upperBound = lowerBound + char.MinValue;
                                    else
                                    {
                                        var upperBoundChars = lowerBound.ToCharArray();

                                        upperBoundChars[upperBoundChars.Length - 1]++;
                                        upperBound = new string(upperBoundChars);
                                    }

                                    return AddStep(new HasStep(memberExpression, new P.Between(lowerBound, upperBound)));
                                }
                            }
                        }
                    }

                    break;
                }
            }

            throw new NotSupportedException();
        }

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex> Where(ParameterExpression parameter, Expression left, Expression right, ExpressionType nodeType)
        {
            if (nodeType == ExpressionType.OrElse || nodeType == ExpressionType.AndAlso)
            {
                var leftLambda = Expression.Lambda<Func<TElement, bool>>(left, parameter);
                var rightLambda = Expression.Lambda<Func<TElement, bool>>(right, parameter);

                return nodeType == ExpressionType.OrElse
                    ? Or(
                        _ => _.Where(leftLambda),
                        _ => _.Where(rightLambda))
                    : And(
                        _ => _.Where(leftLambda),
                        _ => _.Where(rightLambda));
            }

            if (right is MemberExpression memberExpression && memberExpression.Expression == parameter)
            {
                if (nodeType != ExpressionType.Equal)
                    throw new NotSupportedException("Please switch the expression operands!");

                var temp = right;
                right = left;
                left = temp;
            }

            return Where(parameter, left, right.GetValue(), nodeType);
        }

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex> Where(ParameterExpression parameter, Expression left, object rightConstant, ExpressionType nodeType)
        {
            if (rightConstant == null)
            {
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (nodeType)
                {
                    case ExpressionType.Equal:
                        return AddStep(new HasNotStep(left));
                    case ExpressionType.NotEqual:
                        return AddStep(new HasStep(left));
                }
            }
            else
            {
                var predicateArgument = P.ForExpressionType(nodeType, rightConstant);

                switch (left)
                {
                    case MemberExpression leftMemberExpression when parameter == leftMemberExpression.Expression:
                    {
                        if (leftMemberExpression.Expression.Type == typeof(VertexProperty) && leftMemberExpression.Member.Name == nameof(VertexProperty.Value))
                            return AddStep(new HasValueStep(predicateArgument));

                        return AddStep(new HasStep(
                            leftMemberExpression,
                            rightConstant is StepLabel
                                ? GremlinQuery
                                    .Anonymous
                                    .Call("where", predicateArgument)
                                : (object)predicateArgument));
                    }
                    case ParameterExpression leftParameterExpression when parameter == leftParameterExpression:
                    {
                        return AddStep(
                            rightConstant is StepLabel
                                ? new MethodStep("where", predicateArgument)
                                : (Step)new IsStep(predicateArgument));
                    }
                }
            }

            throw new NotSupportedException();
        }
        #endregion

        #region Where (PropertyTraversal)
        IGremlinQuery<TElement> IGremlinQuery<TElement>.Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal) => Where(projection, propertyTraversal);

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal) => Where(projection, propertyTraversal);

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal) => Where(projection, propertyTraversal);

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex> Where<TProjection>(Expression<Func<TElement, TProjection>> predicate, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal)
        {
            return AddStep(new HasStep(predicate.Body, (object)propertyTraversal(Anonymous.Cast<TProjection>())));
        }
        #endregion

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex> HasWithin(Expression expression, Expression enumerableExpression)
        {
            if (enumerableExpression.GetValue() is IEnumerable enumerable)
            {
                return HasWithin(expression, enumerable);
            }

            throw new NotSupportedException();
        }

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex> HasWithin(Expression expression, IEnumerable enumerable)
        {
            var objectArray = enumerable as object[] ?? enumerable.Cast<object>().ToArray();

            return AddStep(new HasStep(
                expression,
                objectArray.Length == 0
                    ? P.False
                    : new P.Within(objectArray)));
        }

        public GroovyExpressionState Serialize(StringBuilder stringBuilder, GroovyExpressionState state)
        {
            foreach (var step in Steps)
            {
                if (step is IGroovySerializable serializableStep)
                    state = serializableStep.Serialize(stringBuilder, state);
                else
                    throw new ArgumentException("Query contains non-serializable step. Please call Resolve on the query first.");
            }

            return state;
        }

        #region AddStep
        private GremlinQueryImpl<TElement, TOutVertex, TInVertex> AddStep(Step step) => AddStep<TElement>(step);

        private GremlinQueryImpl<TNewElement, TOutVertex, TInVertex> AddStep<TNewElement>(Step step) => AddStep<TNewElement, TOutVertex, TInVertex>(step);

        private GremlinQueryImpl<TNewElement, TNewOutVertex, TNewInVertex> AddStep<TNewElement, TNewOutVertex, TNewInVertex>(Step step) => new GremlinQueryImpl<TNewElement, TNewOutVertex, TNewInVertex>(Steps.Insert(Steps.Count, step), StepLabelMappings);
        #endregion

        private TTargetQuery CastQuery<TTargetQuery>() where TTargetQuery : IGremlinQuery
        {
            var elementType = typeof(Unit);
            var inVertexType = typeof(Unit);
            var outVertexType = typeof(Unit);

            if (typeof(TTargetQuery) != typeof(IGremlinQuery))
            {
                if (!typeof(TTargetQuery).IsGenericType)
                    throw new NotSupportedException();

                var genericTypeDef = typeof(TTargetQuery).GetGenericTypeDefinition();

                if (genericTypeDef != typeof(IGremlinQuery<>) && genericTypeDef != typeof(IVGremlinQuery<>) && genericTypeDef != typeof(IEGremlinQuery<>) && genericTypeDef != typeof(IEGremlinQuery<,>) && genericTypeDef != typeof(IEGremlinQuery<,,>))
                    throw new NotSupportedException();

                elementType = typeof(TTargetQuery).GetGenericArguments()[0];

                if (genericTypeDef == typeof(IEGremlinQuery<,>) || genericTypeDef == typeof(IEGremlinQuery<,,>))
                    outVertexType = typeof(TTargetQuery).GetGenericArguments()[1];

                if (genericTypeDef == typeof(IEGremlinQuery<,,>))
                    inVertexType = typeof(TTargetQuery).GetGenericArguments()[2];
            }

            var type = typeof(GremlinQueryImpl<,,>).MakeGenericType(elementType, outVertexType, inVertexType);
            return (TTargetQuery)Activator.CreateInstance(type, Steps, StepLabelMappings);
        }

        public IImmutableList<Step> Steps { get; }
        public IImmutableDictionary<StepLabel, string> StepLabelMappings { get; }
    }

    internal static class GremlinQuery<TElement>
    {
        public static IGremlinQuery<TElement> Create(string graphName = "g")
        {
            return new GremlinQueryImpl<TElement, Unit, Unit>(ImmutableList<Step>.Empty.Add(new IdentifierStep(graphName)), ImmutableDictionary<StepLabel, string>.Empty);
        }
    }

    public static class GremlinQuery
    {
        public static readonly IGremlinQuery<Unit> Anonymous = GremlinQueryImpl<Unit, Unit, Unit>.Anonymous;

        public static IGremlinQuery<Unit> Create(string graphName)
        {
            return GremlinQuery<Unit>.Create(graphName);
        }

        public static IGremlinQuery<TElement> Create<TElement>(string graphName)
        {
            return GremlinQuery<TElement>.Create(graphName);
        }

        public static IGremlinQuery<TElement> SetTypedGremlinQueryProvider<TElement>(this IGremlinQuery<TElement> query, IGremlinQueryProvider gremlinQueryProvider)
        {
            return query
                .AddStep(new SetQueryProviderStep(gremlinQueryProvider));
        }

        public static (string queryString, IDictionary<string, object> parameters) Serialize(this IGremlinQuery query)
        {
            var stringBuilder = new StringBuilder();

            var groovyBuilder = query
                .Serialize(stringBuilder, GroovyExpressionState.FromQuery(query));

            return (stringBuilder.ToString(), groovyBuilder.GetVariables());
        }

        public static Task<TElement> FirstAsync<TElement>(this IGremlinQuery<TElement> query, CancellationToken ct = default)
        {
            return query
                .Limit(1)
                .Execute()
                .First(ct);
        }

        public static async Task<Option<TElement>> FirstOrNoneAsync<TElement>(this IGremlinQuery<TElement> query, CancellationToken ct = default)
        {
            var array = await query
                .Limit(1)
                .Execute()
                .ToArray(ct)
                .ConfigureAwait(false);

            return array.Length > 0
                ? array[0]
                : Option<TElement>.None;
        }

        public static Task<TElement[]> ToArrayAsync<TElement>(this IGremlinQuery<TElement> query, CancellationToken ct = default)
        {
            return query
                .Execute()
                .ToArray(ct);
        }

        public static IGremlinQuery<TElement> Call<TElement>(this IGremlinQuery<TElement> query, string name, params object[] parameters)
        {
            return query.InsertStep<TElement>(query.Steps.Count, new MethodStep(name, parameters));
        }

        internal static IGremlinQuery<TElement> AddStep<TElement>(this IGremlinQuery<TElement> query, Step step)
        {
            return query.InsertStep<TElement>(query.Steps.Count, step);
        }
        
        public static IGremlinQuery<TNewEdge> E<TNewEdge>(this IGremlinQuery query, params object[] ids)
        {
            return query
                .E(ids)
                .OfType<TNewEdge>();
        }

        public static IGremlinQuery<TElement> ReplaceSteps<TElement>(this IGremlinQuery<TElement> query, IImmutableList<Step> steps)
        {
            return ReferenceEquals(steps, query.Steps)
                ? query 
                : new GremlinQueryImpl<TElement, Unit, Unit>(steps, query.StepLabelMappings);
        }

        public static IGremlinQuery<TElement> Resolve<TElement>(this IGremlinQuery<TElement> query, IGraphModel model)
        {
            return new GremlinQueryImpl<TElement, Unit, Unit>(ImmutableList.Create<Step>(new ResolutionStep(query.GetSteps().Resolve(model))), query.StepLabelMappings);
        }

        internal static IGremlinQuery Resolve(this IGremlinQuery query, IGraphModel model)
        {
            return new GremlinQueryImpl<Unit, Unit, Unit>(ImmutableList.Create<Step>(new ResolutionStep(query.GetSteps().Resolve(model))), query.StepLabelMappings);
        }

        private static IEnumerable<Step> GetSteps(this IGremlinQuery query)
        {
            var steps = query.Steps;
            if (steps.Count == 1 && steps[0] is IdentifierStep identifierStep && identifierStep.Identifier == "__")
                return new Step[] {identifierStep, new ResolvedMethodStep("identity", Enumerable.Empty<object>())};

            return steps;
        }

        public static IEnumerable<TerminalStep> Resolve(this IEnumerable<Step> steps, IGraphModel model)
        {
            foreach(var step in steps)
            {
                switch (step)
                {
                    case NonTerminalStep nonTerminal:
                    {
                        foreach (var resolvedStep in nonTerminal.Resolve(model).Resolve(model))
                        {
                            yield return resolvedStep;
                        }

                        break;
                    }
                    case TerminalStep terminalStep:
                    {
                        yield return terminalStep;

                        break;
                    }
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public static IVGremlinQuery<TNewVertex> V<TNewVertex>(this IGremlinQuery query, params object[] ids)
        {
            return query
                .V(ids)
                .OfType<TNewVertex>();
        }

        public static IGremlinQuery<TItem> Unfold<TItem>(this IGremlinQuery<TItem[]> query)
        {
            return query.Unfold<TItem>();
        }

        internal static IGremlinQuery<TElement> AddStepLabelBinding<TElement>(this IGremlinQuery<TElement> query, Expression<Func<TElement, object>> memberExpression, StepLabel stepLabel)
        {
            var body = memberExpression.Body.StripConvert();
            
            if (!(body is MemberExpression memberExpressionBody))
                throw new ArgumentException();

            return new GremlinQueryImpl<TElement, Unit, Unit>(query.Steps, query.StepLabelMappings.SetItem(stepLabel, memberExpressionBody.Member.Name));
        }

        internal static IGremlinQuery<TElement> ReplaceProvider<TElement>(this IGremlinQuery<TElement> query, IGremlinQueryProvider provider)
        {
            return new GremlinQueryImpl<TElement, Unit, Unit>(query.Steps, query.StepLabelMappings);
        }

        public static IGremlinQuery<Unit> WithSubgraphStrategy(this IGremlinQuery<Unit> query, Func<IGremlinQuery<Unit>, IGremlinQuery> vertexCriterion, Func<IGremlinQuery<Unit>, IGremlinQuery> edgeCriterion)
        {
            var vertexCriterionTraversal = vertexCriterion(Anonymous);
            var edgeCriterionTraversal = edgeCriterion(Anonymous);

            if (vertexCriterionTraversal.Steps.Count > 1 || edgeCriterionTraversal.Steps.Count > 1)
            {
                var strategy = Create("SubgraphStrategy")
                    .Call("build");

                if (vertexCriterionTraversal.Steps.Count > 0)
                    strategy = strategy.Call("vertices", vertexCriterionTraversal);

                if (edgeCriterionTraversal.Steps.Count > 0)
                    strategy = strategy.Call("edges", edgeCriterionTraversal);

                return query.AddStep(new MethodStep("withStrategies", strategy.Call("create")));
            }

            return query;
        }

        internal static Option<IGremlinQueryProvider> TryGetTypedGremlinQueryProvider(this IGremlinQuery query)
        {
            return query
                .Steps
                .OfType<SetQueryProviderStep>()
                .Select(x => Option<IGremlinQueryProvider>.Some(x.GremlinQueryProvider))
                .LastOrDefault();
        }
    }
}
