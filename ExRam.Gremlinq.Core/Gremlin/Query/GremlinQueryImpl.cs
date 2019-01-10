// ReSharper disable ArrangeThisQualifier
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ExRam.Gremlinq.Core.GraphElements;
using LanguageExt;
using Microsoft.Extensions.Logging;
using NullGuard;

namespace ExRam.Gremlinq.Core
{
    internal sealed class GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta> :
        IGremlinQueryAdmin,
        IOrderedGremlinQuery<TElement>,
        IOrderedVGremlinQuery<TElement>,
        IVPropertiesGremlinQuery<TElement, TPropertyValue, TMeta>,
        IEPropertiesGremlinQuery<TElement, TPropertyValue>,
        IOrderedEGremlinQuery<TElement>,
        IOrderedInEGremlinQuery<TElement, TInVertex>,
        IOrderedOutEGremlinQuery<TElement, TOutVertex>,
        IOrderedEGremlinQuery<TElement, TOutVertex, TInVertex>
    {
        private readonly ILogger _logger;
        private readonly IGraphModel _model;
        private readonly IImmutableList<Step> _steps;
        private readonly IGremlinQueryExecutor _queryExecutor;
        private readonly IImmutableDictionary<StepLabel, string> _stepLabelMappings;

        public GremlinQueryImpl(IGraphModel model, IGremlinQueryExecutor queryExecutor, IImmutableList<Step> steps, IImmutableDictionary<StepLabel, string> stepLabelBindings, ILogger logger)
        {
            _model = model;
            _steps = steps;
            _logger = logger;
            _queryExecutor = queryExecutor;
            _stepLabelMappings = stepLabelBindings;
        }

        #region AddV
        IVGremlinQuery<TVertex> IGremlinQuerySource.AddV<TVertex>(TVertex vertex) => AddV(vertex);

        IVGremlinQuery<TVertex> IGremlinQuerySource.AddV<TVertex>() => AddV(new TVertex());
        
        private GremlinQueryImpl<TVertex, Unit, Unit, Unit, Unit> AddV<TVertex>(TVertex vertex)
        {
            return this
                .AddStep<TVertex, Unit, Unit, Unit, Unit>(new AddVStep(_model, vertex))
                .AddElementProperties(GraphElementType.Vertex, vertex);
        }
        #endregion

        #region AddE
        IEGremlinQuery<TEdge> IGremlinQuerySource.AddE<TEdge>() => AddE(new TEdge());

        IEGremlinQuery<TEdge> IGremlinQuerySource.AddE<TEdge>(TEdge edge) => AddE(edge);

        IEGremlinQuery<TEdge, TElement> IVGremlinQuery<TElement>.AddE<TEdge>(TEdge edge) => AddE(edge);

        IEGremlinQuery<TEdge, TElement> IVGremlinQuery<TElement>.AddE<TEdge>() => AddE(new TEdge());

        private GremlinQueryImpl<TEdge, TElement, Unit, Unit, Unit> AddE<TEdge>(TEdge newEdge)
        {
            return this
                .AddStep<TEdge, TElement, Unit, Unit, Unit>(new AddEStep(_model, newEdge))
                .AddElementProperties(GraphElementType.Edge, newEdge);
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

        TTargetQuery IVPropertiesGremlinQuery<TElement, TPropertyValue, TMeta>.Aggregate<TTargetQuery>(Func<IVPropertiesGremlinQuery<TElement, TPropertyValue, TMeta>, StepLabel<TPropertyValue[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IVPropertiesGremlinQuery<TElement, TPropertyValue>.Aggregate<TTargetQuery>(Func<IVPropertiesGremlinQuery<TElement, TPropertyValue>, StepLabel<TPropertyValue[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IEPropertiesGremlinQuery<TElement, TPropertyValue>.Aggregate<TTargetQuery>(Func<IEPropertiesGremlinQuery<TElement, TPropertyValue>, StepLabel<TPropertyValue[]>, TTargetQuery> continuation) => Aggregate(continuation);

        private TTargetQuery Aggregate<TStepLabel, TTargetQuery>(Func<GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta>, TStepLabel, TTargetQuery> continuation)
            where TStepLabel : StepLabel, new()
            where TTargetQuery : IGremlinQuery
        {
            var stepLabel = new TStepLabel();

            return continuation(
                AddStep(new AggregateStep(stepLabel)),
                stepLabel);
        }
        #endregion

        #region And
        // ReSharper disable once CoVariantArrayConversion
        IGremlinQuery<TElement> IGremlinQuery<TElement>.And(params Func<IGremlinQuery<TElement>, IGremlinQuery>[] andTraversals) => And(andTraversals);

        // ReSharper disable once CoVariantArrayConversion
        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.And(params Func<IVGremlinQuery<TElement>, IGremlinQuery>[] andTraversals) => And(andTraversals);

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta> And(params Func<GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta>, IGremlinQuery>[] andTraversals)
        {
            return AddStep(new AndStep(andTraversals
                .Select(andTraversal => andTraversal(Anonymize()))
                .ToArray()));
        }
        #endregion

        #region As (inline)
        TTargetQuery IVGremlinQuery<TElement>.As<TTargetQuery>(Func<IVGremlinQuery<TElement>, VStepLabel<TElement>, TTargetQuery> continuation) => As(continuation);
        
        TTargetQuery IGremlinQuery<TElement>.As<TTargetQuery>(Func<IGremlinQuery<TElement>, StepLabel<TElement>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IEGremlinQuery<TElement>.As<TTargetQuery>(Func<IEGremlinQuery<TElement>, EStepLabel<TElement>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IEGremlinQuery<TElement, TOutVertex>.As<TTargetQuery>(Func<IEGremlinQuery<TElement, TOutVertex>, EStepLabel<TElement, TOutVertex>, TTargetQuery> continuation) => As(continuation);
        
        TTargetQuery IOutEGremlinQuery<TElement, TOutVertex>.As<TTargetQuery>(Func<IOutEGremlinQuery<TElement, TOutVertex>, EStepLabel<TElement, TOutVertex>, TTargetQuery> continuation) => As(continuation);
        
        TTargetQuery IInEGremlinQuery<TElement, TInVertex>.As<TTargetQuery>(Func<IInEGremlinQuery<TElement, TInVertex>, EStepLabel<TElement, TInVertex>, TTargetQuery> continuation) => As(continuation);
        
        private TTargetQuery As<TStepLabel, TTargetQuery>(Func<GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta>, TStepLabel, TTargetQuery> continuation)
            where TStepLabel : StepLabel<TElement>, new()
            where TTargetQuery : IGremlinQuery
        {
            var stepLabel = new TStepLabel();

            return continuation(
                As(stepLabel),
                stepLabel);
        }
        #endregion

        #region As (explicit)
        IGremlinQuery<TElement> IGremlinQuery<TElement>.As(StepLabel stepLabel) => As(stepLabel);

        IInEGremlinQuery<TElement, TInVertex> IInEGremlinQuery<TElement, TInVertex>.As(StepLabel stepLabel) => As(stepLabel);

        IOutEGremlinQuery<TElement, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.As(StepLabel stepLabel) => As(stepLabel);

        IEGremlinQuery<TElement, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.As(StepLabel stepLabel) => As(stepLabel);

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.As(StepLabel stepLabel) => As(stepLabel);

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.As(StepLabel stepLabel) => As(stepLabel);

        IEGremlinQuery<TElement, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.As(StepLabel stepLabel) => As(stepLabel);

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta> As(StepLabel stepLabel) => AddStep(new AsStep(stepLabel));
        #endregion

        IGremlinQueryAdmin IGremlinQuery.AsAdmin() => this;

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Barrier() => AddStep(BarrierStep.Instance);

        #region Cast
        IGremlinQuery<TNewElement> IGremlinQuery.Cast<TNewElement>() => Cast<TNewElement>();

        IVGremlinQuery<TNewElement> IVGremlinQuery.Cast<TNewElement>() => Cast<TNewElement>();

        IEGremlinQuery<TNewElement, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.Cast<TNewElement>() => Cast<TNewElement>();

        IEGremlinQuery<TNewElement> IEGremlinQuery<TElement>.Cast<TNewElement>() => Cast<TNewElement>();

        IEGremlinQuery<TNewElement, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.Cast<TNewElement>() => Cast<TNewElement>();

        IOutEGremlinQuery<TNewElement, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.Cast<TNewElement>() => Cast<TNewElement>();

        IInEGremlinQuery<TNewElement, TInVertex> IInEGremlinQuery<TElement, TInVertex>.Cast<TNewElement>() => Cast<TNewElement>();

        private GremlinQueryImpl<TNewElement, TOutVertex, TInVertex, TPropertyValue, TMeta> Cast<TNewElement>() => Cast<TNewElement, TOutVertex, TInVertex, TPropertyValue, TMeta>();

        private GremlinQueryImpl<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta> Cast<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta>() => new GremlinQueryImpl<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta>(_model, _queryExecutor, _steps, _stepLabelMappings, _logger);
        #endregion

        #region Coalesce
        // ReSharper disable once CoVariantArrayConversion
        TTargetQuery IVGremlinQuery<TElement>.Coalesce<TTargetQuery>(params Func<IVGremlinQuery<TElement>, TTargetQuery>[] traversals) => Coalesce(traversals);

        // ReSharper disable once CoVariantArrayConversion
        TTargetQuery IGremlinQuery<TElement>.Coalesce<TTargetQuery>(params Func<IGremlinQuery<TElement>, TTargetQuery>[] traversals) => Coalesce(traversals);

        private TTargetQuery Coalesce<TTargetQuery>(params Func<GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta>, TTargetQuery>[] traversals)
            where TTargetQuery : IGremlinQuery
        {
            return this
                .AddStep(new CoalesceStep(traversals
                    .Select(traversal => (IGremlinQuery)traversal(Anonymize()))))
                .ChangeQueryType<TTargetQuery>();
        }
        #endregion

        IGremlinQuery<long> IGremlinQuery.Count() => AddStep<long, Unit, Unit, Unit, Unit>(CountStep.Instance);

        #region Choose
        IGremlinQuery<TResult> IGremlinQuery<TElement>.Choose<TResult>(Func<IGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IGremlinQuery<TElement>, IGremlinQuery<TResult>> trueChoice, Func<IGremlinQuery<TElement>, IGremlinQuery<TResult>> falseChoice)
        {
            return AddStep<TResult>(new ChooseStep(traversalPredicate(Anonymize()), trueChoice(Anonymize()), Option<IGremlinQuery>.Some(falseChoice(Anonymize()))));
        }

        IGremlinQuery<TResult> IGremlinQuery<TElement>.Choose<TResult>(Func<IGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IGremlinQuery<TElement>, IGremlinQuery<TResult>> trueChoice)
        {
            var anonymous = Anonymize();

            return AddStep<TResult>(new ChooseStep(traversalPredicate(anonymous), trueChoice(anonymous)));
        }
        #endregion

        #region BothX
        IVGremlinQuery<IVertex> IVGremlinQuery.Both<TEdge>() => AddStep<IVertex>(new BothStep(_model.VerticesModel.GetValidFilterLabels(typeof(TEdge))));

        IEGremlinQuery<TEdge> IVGremlinQuery.BothE<TEdge>() => AddStep<TEdge>(new BothEStep(_model.EdgesModel.GetValidFilterLabels(typeof(TEdge))));

        IVGremlinQuery<IVertex> IEGremlinQuery<TElement>.BothV() => AddStep<IVertex>(BothVStep.Instance);
        #endregion

        #region Instance
        IGremlinQuery<TElement> IGremlinQuery<TElement>.Dedup() => Dedup();

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Dedup() => Dedup();

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.Dedup() => Dedup();

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta> Dedup() => AddStep(DedupStep.Instance);
        #endregion

        #region Drop
        IGremlinQuery<Unit> IGremlinQuery.Drop() => Drop();

        private GremlinQueryImpl<Unit, Unit, Unit, Unit, Unit> Drop() => AddStep<Unit, Unit, Unit, Unit, Unit>(DropStep.Instance);
        #endregion

        #region E
        IEGremlinQuery<TEdge> IGremlinQuerySource.E<TEdge>(params object[] ids) => AddStep<Unit, Unit, Unit, Unit, Unit>(new EStep(ids)).OfType<TEdge>(_model.EdgesModel, true);

        IEGremlinQuery<IEdge> IGremlinQuerySource.E(params object[] ids) => AddStep<IEdge, Unit, Unit, Unit, Unit>(new EStep(ids));
        #endregion

        #region Emit
        IGremlinQuery<TElement> IGremlinQuery<TElement>.Emit() => Emit();

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Emit() => Emit();

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.Emit() => Emit();

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta> Emit() => AddStep(EmitStep.Instance);
        #endregion

        IGremlinQuery<string> IGremlinQuery.Explain() => AddStep<string, Unit, Unit, Unit, Unit>(ExplainStep.Instance);

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Filter(string lambda) => AddStep(new FilterStep(new Lambda(lambda)));

        IGremlinQuery<TElement[]> IGremlinQuery<TElement>.Fold() => AddStep<TElement[], Unit, Unit, Unit, Unit>(FoldStep.Instance);

        #region From (step label)
        IOutEGremlinQuery<TElement, TNewOutVertex> IEGremlinQuery<TElement>.From<TNewOutVertex>(StepLabel<TNewOutVertex> stepLabel) => AddStep<TElement, TNewOutVertex, Unit, Unit, Unit>(new FromLabelStep(stepLabel));

        IEGremlinQuery<TElement, TTargetVertex, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.From<TTargetVertex>(StepLabel<TTargetVertex> stepLabel) => AddStep<TElement, TTargetVertex, TOutVertex, Unit, Unit>(new FromLabelStep(stepLabel));
        #endregion

        #region From (traversal)
        IEGremlinQuery<TElement, TTargetVertex, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.From<TTargetVertex>(Func<IVGremlinQuery<TOutVertex>, IGremlinQuery<TTargetVertex>> fromVertexTraversal) => AddStep<TElement, TTargetVertex, TOutVertex, Unit, Unit>(new FromTraversalStep(fromVertexTraversal(Anonymize<TOutVertex, Unit, Unit, Unit, Unit>())));

        IOutEGremlinQuery<TElement, TNewOutVertex> IEGremlinQuery<TElement>.From<TNewOutVertex>(Func<IGremlinQuery, IGremlinQuery<TNewOutVertex>> fromVertexTraversal) => From<TElement, TNewOutVertex, Unit>(fromVertexTraversal);

        IEGremlinQuery<TElement, TNewOutVertex, TInVertex> IInEGremlinQuery<TElement, TInVertex>.From<TNewOutVertex>(Func<IGremlinQuery, IGremlinQuery<TNewOutVertex>> fromVertexTraversal) => From<TElement, TNewOutVertex, TInVertex>(fromVertexTraversal);

        private GremlinQueryImpl<TNewElement, TNewOutVertex, TNewInVertex, Unit, Unit> From<TNewElement, TNewOutVertex, TNewInVertex>(Func<GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta>, IGremlinQuery> fromVertexTraversal) => AddStep<TNewElement, TNewOutVertex, TNewInVertex, Unit, Unit>(new FromTraversalStep(fromVertexTraversal(Anonymize())));
        #endregion

        #region Id
        IGremlinQuery<object> IVGremlinQuery.Id() => Id();

        IGremlinQuery<object> IVPropertiesGremlinQuery<TElement, TPropertyValue>.Id() => Id();

        IGremlinQuery<object> IEGremlinQuery<TElement>.Id() => Id();

        private GremlinQueryImpl<object, Unit, Unit, Unit, Unit> Id() => AddStep<object, Unit, Unit, Unit, Unit>(IdStep.Instance);
        #endregion

        #region Identity
        IGremlinQuery<TElement> IGremlinQuery<TElement>.Identity() => Identity();

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Identity() => Identity();

        IOutEGremlinQuery<TElement, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.Identity() => Identity();

        IEGremlinQuery<TElement, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.Identity() => Identity();

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.Identity() => Identity();

        IEGremlinQuery<TElement, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.Identity() => Identity();

        IInEGremlinQuery<TElement, TInVertex> IInEGremlinQuery<TElement, TInVertex>.Identity() => Identity();

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta> Identity() => AddStep(IdentityStep.Instance);
        #endregion

        IVGremlinQuery<IVertex> IVGremlinQuery.In<TEdge>() => AddStep<IVertex>(new InStep(_model.VerticesModel.GetValidFilterLabels(typeof(TEdge))));
        
        IInEGremlinQuery<TEdge, TElement> IVGremlinQuery<TElement>.InE<TEdge>() => AddStep<TEdge, Unit, TElement, Unit, Unit>(new InEStep(_model.EdgesModel.GetValidFilterLabels(typeof(TEdge))));

        #region InV
        IVGremlinQuery<IVertex> IEGremlinQuery<TElement>.InV() => InV<IVertex>();

        IVGremlinQuery<TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.InV() => InV<TInVertex>();

        IVGremlinQuery<TInVertex> IInEGremlinQuery<TElement, TInVertex>.InV() => InV<TInVertex>();

        private GremlinQueryImpl<TNewElement, Unit, Unit, Unit, Unit> InV<TNewElement>() => AddStep<TNewElement, Unit, Unit, Unit, Unit>(InVStep.Instance);
        #endregion

        #region Inject
        IGremlinQuery<TNewElement> IGremlinQuerySource.Inject<TNewElement>(params TNewElement[] elements) => Inject(elements);

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Inject(params TElement[] elements) => Inject(elements);

        private GremlinQueryImpl<TNewElement, TOutVertex, TInVertex, TPropertyValue, TMeta> Inject<TNewElement>(TNewElement[] elements) => AddStep<TNewElement>(new InjectStep(elements.Cast<object>().ToArray()));
        #endregion

        IGremlinQuery IGremlinQueryAdmin.InsertStep(int index, Step step) => new GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta>(_model, _queryExecutor, _steps.Insert(index, step), _stepLabelMappings, _logger);

        #region Limit
        IGremlinQuery<TElement> IGremlinQuery<TElement>.Limit(long count) => Limit(count);

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Limit(long count) => Limit(count);

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.Limit(long count) => Limit(count);

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta> Limit(long count)
        {
            return AddStep(count == 1
                ? LimitStep.Limit1
                : new LimitStep(count));
        }
        #endregion

        #region Local
        TTargetQuery IEGremlinQuery<TElement>.Local<TTargetQuery>(Func<IEGremlinQuery<TElement>, TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IVGremlinQuery<TElement>.Local<TTargetQuery>(Func<IVGremlinQuery<TElement>, TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IGremlinQuery<TElement>.Local<TTargetQuery>(Func<IGremlinQuery<TElement>, TTargetQuery> localTraversal) => Local(localTraversal);

        private TTargetQuery Local<TTargetQuery>(Func<GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta>, TTargetQuery> localTraversal)
            where TTargetQuery : IGremlinQuery
        {
            return this.AddStep(new LocalStep(localTraversal(Anonymize())))
                .ChangeQueryType<TTargetQuery>();
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

        private TTargetQuery Map<TTargetQuery>(Func<GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery
        {
            return this.AddStep(new MapStep(mapping(Anonymize())))
                .ChangeQueryType<TTargetQuery>();
        }
        #endregion

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Match(params Func<IGremlinQuery<TElement>, IGremlinQuery<TElement>>[] matchTraversals) => AddStep(new MatchStep(matchTraversals.Select(traversal => traversal(Anonymize()))));

        IVPropertiesGremlinQuery<VertexProperty<TPropertyValue, TNewMeta>, TPropertyValue, TNewMeta> IVPropertiesGremlinQuery<TElement, TPropertyValue>.Meta<TNewMeta>() => Cast<VertexProperty<TPropertyValue, TNewMeta>, Unit, Unit, TPropertyValue, TNewMeta>();

        #region Not
        IGremlinQuery<TElement> IGremlinQuery<TElement>.Not(Func<IGremlinQuery<TElement>, IGremlinQuery> notTraversal) => Not(notTraversal);

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Not(Func<IVGremlinQuery<TElement>, IGremlinQuery> notTraversal) => Not(notTraversal);

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta> Not(Func<GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta>, IGremlinQuery> notTraversal) => AddStep(new NotStep(notTraversal(Anonymize())));
        #endregion

        #region OfType
        IVGremlinQuery<TTarget> IVGremlinQuery.OfType<TTarget>() => OfType<TTarget>(_model.VerticesModel);

        IEGremlinQuery<TTarget, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.OfType<TTarget>() => OfType<TTarget>(_model.EdgesModel);

        IEGremlinQuery<TTarget> IEGremlinQuery<TElement>.OfType<TTarget>() => OfType<TTarget>(_model.EdgesModel);

        IEGremlinQuery<TTarget, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.OfType<TTarget>() => OfType<TTarget>(_model.EdgesModel);

        IOutEGremlinQuery<TTarget, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.OfType<TTarget>() => OfType<TTarget>(_model.EdgesModel);

        IInEGremlinQuery<TTarget, TInVertex> IInEGremlinQuery<TElement, TInVertex>.OfType<TTarget>() => OfType<TTarget>(_model.EdgesModel);

        private GremlinQueryImpl<TTarget, TOutVertex, TInVertex, TPropertyValue, TMeta> OfType<TTarget>(IGraphElementModel model, bool disableTypeOptimization = false)
        {
            if (disableTypeOptimization || !typeof(TTarget).IsAssignableFrom(typeof(TElement)))
            {
                var labels = model.GetValidFilterLabels(typeof(TTarget));

                if (labels.Length > 0)
                    return AddStep<TTarget>(new HasLabelStep(labels));
            }

            return Cast<TTarget>();
        }
        #endregion

        #region Optional
        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Optional(Func<IVGremlinQuery<TElement>, IVGremlinQuery<TElement>> optionalTraversal) => Optional(optionalTraversal);

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Optional(Func<IGremlinQuery<TElement>, IGremlinQuery<TElement>> optionalTraversal) => Optional(optionalTraversal);

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta> Optional(Func<GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta>, IGremlinQuery> optionalTraversal) => AddStep(new OptionalStep(optionalTraversal(Anonymize())));
        #endregion

        #region Or
        // ReSharper disable once CoVariantArrayConversion
        IGremlinQuery<TElement> IGremlinQuery<TElement>.Or(params Func<IGremlinQuery<TElement>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        // ReSharper disable once CoVariantArrayConversion
        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Or(params Func<IVGremlinQuery<TElement>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta> Or(params Func<GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta>, IGremlinQuery>[] orTraversals)
        {
            return AddStep(new OrStep(orTraversals
                .Select(orTraversal => orTraversal(Anonymize()))
                .ToArray()));
        }
        #endregion

        #region OrderBy{Descending} projection
        IOrderedGremlinQuery<TElement> IGremlinQuery<TElement>.OrderBy(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Increasing);

        IOrderedVGremlinQuery<TElement> IVGremlinQuery<TElement>.OrderBy(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Increasing);

        IOrderedEGremlinQuery<TElement> IEGremlinQuery<TElement>.OrderBy(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Increasing);

        IOrderedOutEGremlinQuery<TElement, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.OrderBy(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Increasing);

        IOrderedInEGremlinQuery<TElement, TInVertex> IInEGremlinQuery<TElement, TInVertex>.OrderBy(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Increasing);

        IOrderedEGremlinQuery<TElement, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.OrderBy(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Increasing);

        IOrderedGremlinQuery<TElement> IGremlinQuery<TElement>.OrderByDescending(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Decreasing);

        IOrderedVGremlinQuery<TElement> IVGremlinQuery<TElement>.OrderByDescending(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Decreasing);

        IOrderedEGremlinQuery<TElement> IEGremlinQuery<TElement>.OrderByDescending(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Decreasing);

        IOrderedOutEGremlinQuery<TElement, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.OrderByDescending(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Decreasing);

        IOrderedInEGremlinQuery<TElement, TInVertex> IInEGremlinQuery<TElement, TInVertex>.OrderByDescending(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Decreasing);

        IOrderedEGremlinQuery<TElement, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.OrderByDescending(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Decreasing);

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta> OrderBy(Expression<Func<TElement, object>> projection, Order order)
        {
            return this
                .AddStep(OrderStep.Instance)
                .By(projection, order);
        }

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta> By(Expression<Func<TElement, object>> projection, Order order)
        {
            if (projection.Body.StripConvert() is MemberExpression memberExpression)
                return AddStep(new ByMemberStep(memberExpression.Member, order));

            throw new ExpressionNotSupportedException(projection);
        }
        #endregion

        #region OrderBy{Descending} traversal
        IOrderedGremlinQuery<TElement> IGremlinQuery<TElement>.OrderBy(Func<IGremlinQuery<TElement>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Increasing);

        IOrderedVGremlinQuery<TElement> IVGremlinQuery<TElement>.OrderBy(Func<IVGremlinQuery<TElement>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Increasing);

        IOrderedEGremlinQuery<TElement> IEGremlinQuery<TElement>.OrderBy(Func<IEGremlinQuery<TElement>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Increasing);

        IOrderedOutEGremlinQuery<TElement, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.OrderBy(Func<IOutEGremlinQuery<TElement, TOutVertex>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Increasing);

        IOrderedInEGremlinQuery<TElement, TInVertex> IInEGremlinQuery<TElement, TInVertex>.OrderBy(Func<IInEGremlinQuery<TElement, TInVertex>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Increasing);

        IOrderedEGremlinQuery<TElement, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.OrderBy(Func<IEGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Increasing);

        IOrderedGremlinQuery<TElement> IGremlinQuery<TElement>.OrderByDescending(Func<IGremlinQuery<TElement>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Decreasing);

        IOrderedVGremlinQuery<TElement> IVGremlinQuery<TElement>.OrderByDescending(Func<IVGremlinQuery<TElement>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Decreasing);

        IOrderedEGremlinQuery<TElement> IEGremlinQuery<TElement>.OrderByDescending(Func<IEGremlinQuery<TElement>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Decreasing);

        IOrderedOutEGremlinQuery<TElement, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.OrderByDescending(Func<IOutEGremlinQuery<TElement, TOutVertex>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Decreasing);

        IOrderedInEGremlinQuery<TElement, TInVertex> IInEGremlinQuery<TElement, TInVertex>.OrderByDescending(Func<IInEGremlinQuery<TElement, TInVertex>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Decreasing);

        IOrderedEGremlinQuery<TElement, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.OrderByDescending(Func<IEGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Decreasing);

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta> OrderBy(Func<GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta>, IGremlinQuery> traversal, Order order)
        {
            return this
                .AddStep(OrderStep.Instance)
                .By(traversal, order);
        }

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta> By(Func<GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta>, IGremlinQuery> traversal, Order order)
        {
            return this
                .AddStep(new ByTraversalStep(traversal(Anonymize()), order));
        }
        #endregion

        #region OrderBy{Descending} lambda
        IOrderedGremlinQuery<TElement> IGremlinQuery<TElement>.OrderBy(string lambda) => OrderBy(lambda);

        IOrderedVGremlinQuery<TElement> IVGremlinQuery<TElement>.OrderBy(string lambda) => OrderBy(lambda);

        IOrderedEGremlinQuery<TElement> IEGremlinQuery<TElement>.OrderBy(string lambda) => OrderBy(lambda);

        IOrderedOutEGremlinQuery<TElement, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.OrderBy(string lambda) => OrderBy(lambda);

        IOrderedInEGremlinQuery<TElement, TInVertex> IInEGremlinQuery<TElement, TInVertex>.OrderBy(string lambda) => OrderBy(lambda);

        IOrderedEGremlinQuery<TElement, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.OrderBy(string lambda) => OrderBy(lambda);
        
        private GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta> OrderBy(string lambda)
        {
            return this
                .AddStep(OrderStep.Instance)
                .By(lambda);
        }

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta> By(string lambda)
        {
            return this
                .AddStep(new ByLambdaStep(new Lambda(lambda)));
        }
        #endregion

        IVGremlinQuery<IVertex> IEGremlinQuery<TElement>.OtherV() => AddStep<IVertex>(OtherVStep.Instance);

        #region OutX
        IOutEGremlinQuery<TNewEdge, TElement> IVGremlinQuery<TElement>.OutE<TNewEdge>() => AddStep<TNewEdge, TElement, Unit, Unit, Unit>(new OutEStep(_model.EdgesModel.GetValidFilterLabels(typeof(TNewEdge))));

        IVGremlinQuery<IVertex> IEGremlinQuery<TElement>.OutV() => AddStep<IVertex, Unit, Unit, Unit, Unit>(OutVStep.Instance);
        
        IVGremlinQuery<TOutVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.OutV() => AddStep<TOutVertex, Unit, Unit, Unit, Unit>(OutVStep.Instance);

        IVGremlinQuery<TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.OutV() => AddStep<TOutVertex, Unit, Unit, Unit, Unit>(OutVStep.Instance);

        IVGremlinQuery<IVertex> IVGremlinQuery.Out<TNewEdge>() => AddStep<IVertex, Unit, Unit, Unit, Unit>(new OutStep(_model.EdgesModel.GetValidFilterLabels(typeof(TNewEdge))));
        #endregion
        
        IGremlinQuery<string> IGremlinQuery.Profile() => AddStep<string>(ProfileStep.Instance);

        #region Properties
        IVPropertiesGremlinQuery<VertexProperty<TTarget>, TTarget> IVGremlinQuery<TElement>.Properties<TTarget>(params Expression<Func<TElement, TTarget>>[] projections) => Properties<TElement, TTarget, VertexProperty<TTarget>, TTarget, Unit>(projections);

        IVPropertiesGremlinQuery<VertexProperty<TTarget>, TTarget> IVGremlinQuery<TElement>.Properties<TTarget>(params Expression<Func<TElement, TTarget[]>>[] projections) => Properties<TElement, TTarget[], VertexProperty<TTarget>, TTarget, Unit>(projections);

        IGremlinQuery<Property<TTarget>> IVPropertiesGremlinQuery<TElement, TPropertyValue, TMeta>.Properties<TTarget>(params Expression<Func<TMeta, TTarget>>[] projections) => Properties<TMeta, TTarget, Property<TTarget>, Unit, Unit>(projections);

        IEPropertiesGremlinQuery<Property<TTarget>, TTarget> IEGremlinQuery<TElement>.Properties<TTarget>(params Expression<Func<TElement, TTarget>>[] projections) => Properties<TElement, TTarget, Property<TTarget>, TTarget, Unit>(projections);

        IEPropertiesGremlinQuery<Property<TTarget>, TTarget> IEGremlinQuery<TElement>.Properties<TTarget>(params Expression<Func<TElement, TTarget[]>>[] projections) => Properties<TElement, TTarget[], Property<TTarget>, TTarget, Unit>(projections);

        private GremlinQueryImpl<TNewElement, Unit, Unit, TNewPropertyValue, TNewMeta> Properties<TSource, TTarget, TNewElement, TNewPropertyValue, TNewMeta>(params Expression<Func<TSource, TTarget>>[] projections)
        {
            return AddStep<TNewElement, Unit, Unit, TNewPropertyValue, TNewMeta>(new PropertiesStep(projections
                .Select(projection =>
                {
                    if (projection.Body.StripConvert() is MemberExpression memberExpression)
                    {
                        return memberExpression.Member;
                    }

                    throw new ExpressionNotSupportedException(projection);
                })
                .ToArray()));
        }

        IGremlinQuery<Property<object>> IVPropertiesGremlinQuery<TElement, TPropertyValue>.Properties(params string[] keys) => Properties(keys);

        private GremlinQueryImpl<Property<object>, Unit, Unit, Unit, Unit> Properties(params string[] keys) => AddStep<Property<object>, Unit, Unit, Unit, Unit>(new MetaPropertiesStep(keys));
        #endregion

        #region Property
        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Property<TValue>(Expression<Func<TElement, TValue>> projection, [AllowNull] TValue value) => Property(projection, GraphElementType.VertexProperty, value);
 
        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Property<TValue>(Expression<Func<TElement, TValue[]>> projection, [AllowNull] TValue value) => Property(projection, GraphElementType.VertexProperty, value);

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.Property<TValue>(Expression<Func<TElement, TValue>> projection, [AllowNull] TValue value) => Property(projection, GraphElementType.Edge, value);

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.Property<TValue>(Expression<Func<TElement, TValue[]>> projection, [AllowNull] TValue value) => Property(projection, GraphElementType.Edge, value);

        IVPropertiesGremlinQuery<TElement, TPropertyValue, TMeta> IVPropertiesGremlinQuery<TElement, TPropertyValue, TMeta>.Property<TValue>(Expression<Func<TMeta, TValue>> projection, TValue value) => Property(projection, GraphElementType.VertexProperty, value);

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta> Property<TSource, TValue>(Expression<Func<TSource, TValue>> projection, GraphElementType elementType, [AllowNull] object value)
        {
            if (value == null)
            {
                return SideEffect(_ => _
                    .Properties<TSource, TValue, Unit, Unit, Unit>(projection)
                    .Drop());
            }

            if (projection.Body.StripConvert() is MemberExpression memberExpression)
                return AddStep(new PropertyStep(memberExpression.Type, _model.GetIdentifier(elementType, memberExpression.Member.Name), value));

            throw new ExpressionNotSupportedException(projection);
        }

        IVPropertiesGremlinQuery<TElement, TPropertyValue> IVPropertiesGremlinQuery<TElement, TPropertyValue>.Property(string key, [AllowNull] object value)
        {
            if (value == null)
            {
                return SideEffect(_ => _
                    .Properties(key)
                    .Drop());
            }

            return AddStep(new MetaPropertyStep(key, value));
        }
        #endregion

        #region Range
        IGremlinQuery<TElement> IGremlinQuery<TElement>.Range(long low, long high) => Range(low, high);

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Range(long low, long high) => Range(low, high);

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.Range(long low, long high) => Range(low, high);

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta> Range(long low, long high) => AddStep(new RangeStep(low, high));

        #endregion

        #region Repeat
        IGremlinQuery<TElement> IGremlinQuery<TElement>.Repeat(Func<IGremlinQuery<TElement>, IGremlinQuery<TElement>> repeatTraversal) => Repeat(repeatTraversal);

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Repeat(Func<IVGremlinQuery<TElement>, IVGremlinQuery<TElement>> repeatTraversal) => Repeat(repeatTraversal);

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.Repeat(Func<IEGremlinQuery<TElement>, IEGremlinQuery<TElement>> repeatTraversal) => Repeat(repeatTraversal);
        
        private GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta> Repeat(Func<GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta>, IGremlinQuery> repeatTraversal) => AddStep(new RepeatStep(repeatTraversal(Anonymize())));
        #endregion

        #region RepeatUntil
        IGremlinQuery<TElement> IGremlinQuery<TElement>.RepeatUntil(Func<IGremlinQuery<TElement>, IGremlinQuery<TElement>> repeatTraversal, Func<IGremlinQuery<TElement>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.RepeatUntil(Func<IVGremlinQuery<TElement>, IVGremlinQuery<TElement>> repeatTraversal, Func<IVGremlinQuery<TElement>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.RepeatUntil(Func<IEGremlinQuery<TElement>, IEGremlinQuery<TElement>> repeatTraversal, Func<IEGremlinQuery<TElement>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta> RepeatUntil(Func<GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta>, IGremlinQuery> repeatTraversal, Func<GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta>, IGremlinQuery> untilTraversal)
        {
            return this
                .AddStep(new RepeatStep(repeatTraversal(Anonymize())))
                .AddStep(new UntilStep(untilTraversal(Anonymize())));
        }
        #endregion

        #region Select
        IGremlinQuery<TStep> IGremlinQuery.Select<TStep>(StepLabel<TStep> label) => Select<TStep, Unit, Unit>(label);

        IVGremlinQuery<TVertex> IGremlinQuery.Select<TVertex>(VStepLabel<TVertex> label) => Select<TVertex, Unit, Unit>(label);

        IEGremlinQuery<TEdge> IGremlinQuery.Select<TEdge>(EStepLabel<TEdge> label) => Select<TEdge, Unit, Unit>(label);

        IEGremlinQuery<TEdge, TAdjacentVertex> IGremlinQuery.Select<TEdge, TAdjacentVertex>(EStepLabel<TEdge, TAdjacentVertex> label) => Select<TEdge, TAdjacentVertex, Unit>(label);

        IOutEGremlinQuery<TEdge, TAdjacentVertex> IGremlinQuery.Select<TEdge, TAdjacentVertex>(OutEStepLabel<TEdge, TAdjacentVertex> label) => Select<TEdge, TAdjacentVertex, Unit>(label);

        IInEGremlinQuery<TEdge, TAdjacentVertex> IGremlinQuery.Select<TEdge, TAdjacentVertex>(InEStepLabel<TEdge, TAdjacentVertex> label) => Select<TEdge, Unit, TAdjacentVertex>(label);

        private GremlinQueryImpl<TSelectedElement, TSelectedOutVertex, TSelectedInVertex, Unit, Unit> Select<TSelectedElement, TSelectedOutVertex, TSelectedInVertex>(StepLabel stepLabel) => AddStep<TSelectedElement, TSelectedOutVertex, TSelectedInVertex, Unit, Unit>(new SelectStep(stepLabel));

        IGremlinQuery<(T1, T2)> IGremlinQuery.Select<T1, T2>(StepLabel<T1> label1, StepLabel<T2> label2)
        {
            return this.AddStep<(T1, T2), Unit, Unit, Unit, Unit>(new SelectStep(label1, label2))
                .AddStepLabelBinding(x => x.Item1, label1)
                .AddStepLabelBinding(x => x.Item2, label2);
        }

        IGremlinQuery<(T1, T2, T3)> IGremlinQuery.Select<T1, T2, T3>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3)
        {
            return this.AddStep<(T1, T2, T3), Unit, Unit, Unit, Unit>(new SelectStep(label1, label2, label3))
                .AddStepLabelBinding(x => x.Item1, label1)
                .AddStepLabelBinding(x => x.Item2, label2)
                .AddStepLabelBinding(x => x.Item3, label3);
        }

        IGremlinQuery<(T1, T2, T3, T4)> IGremlinQuery.Select<T1, T2, T3, T4>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4)
        {
            return this.AddStep<(T1, T2, T3, T4), Unit, Unit, Unit, Unit>(new SelectStep(label1, label2, label3, label4))
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

        IVPropertiesGremlinQuery<TElement, TPropertyValue, TMeta> IVPropertiesGremlinQuery<TElement, TPropertyValue, TMeta>.SideEffect(Func<IVPropertiesGremlinQuery<TElement, TPropertyValue, TMeta>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IVPropertiesGremlinQuery<TElement, TPropertyValue> IVPropertiesGremlinQuery<TElement, TPropertyValue>.SideEffect(Func<IVPropertiesGremlinQuery<TElement, TPropertyValue>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IEPropertiesGremlinQuery<TElement, TPropertyValue> IEPropertiesGremlinQuery<TElement, TPropertyValue>.SideEffect(Func<IEPropertiesGremlinQuery<TElement, TPropertyValue>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta> SideEffect(Func<GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta>, IGremlinQuery> sideEffectTraversal) => AddStep(new SideEffectStep(sideEffectTraversal(Anonymize())));
        #endregion

        #region Skip
        IGremlinQuery<TElement> IGremlinQuery<TElement>.Skip(long count) => Skip(count);

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Skip(long count) => Skip(count);

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.Skip(long count) => Skip( count);
        
        private GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta> Skip(long count) => AddStep(new SkipStep(count));
        #endregion

        #region Sum
        IGremlinQuery<TElement> IGremlinQuery<TElement>.SumLocal() => AddStep(SumStep.Local);

        IGremlinQuery<TElement> IGremlinQuery<TElement>.SumGlobal() => AddStep(SumStep.Global);
        #endregion

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Times(int count) => AddStep(new TimesStep(count));

        #region Tail
        IGremlinQuery<TElement> IGremlinQuery<TElement>.Tail(long count) => Tail(count);

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Tail(long count) => Tail(count);

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.Tail(long count) => Tail(count);

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta> Tail(long count) => AddStep(new TailStep(count));
        #endregion

        #region ThenBy
        IOrderedVGremlinQuery<TElement> IOrderedVGremlinQuery<TElement>.ThenBy(Expression<Func<TElement, object>> projection) => By(projection, Order.Increasing);

        IOrderedVGremlinQuery<TElement> IOrderedVGremlinQuery<TElement>.ThenBy(Func<IGremlinQuery<TElement>, IGremlinQuery> traversal) => By(traversal, Order.Increasing);

        IOrderedVGremlinQuery<TElement> IOrderedVGremlinQuery<TElement>.ThenBy(string lambda) => By(lambda);

        IOrderedVGremlinQuery<TElement> IOrderedVGremlinQuery<TElement>.ThenByDescending(Expression<Func<TElement, object>> projection) => By(projection, Order.Decreasing);

        IOrderedVGremlinQuery<TElement> IOrderedVGremlinQuery<TElement>.ThenByDescending(Func<IGremlinQuery<TElement>, IGremlinQuery> traversal) => By(traversal, Order.Decreasing);

        IOrderedEGremlinQuery<TElement, TOutVertex, TInVertex> IOrderedEGremlinQuery<TElement, TOutVertex, TInVertex>.ThenBy(Expression<Func<TElement, object>> projection) => By(projection, Order.Increasing);

        IOrderedEGremlinQuery<TElement, TOutVertex, TInVertex> IOrderedEGremlinQuery<TElement, TOutVertex, TInVertex>.ThenBy(Func<IGremlinQuery<TElement>, IGremlinQuery> traversal) => By(traversal, Order.Increasing);

        IOrderedEGremlinQuery<TElement, TOutVertex, TInVertex> IOrderedEGremlinQuery<TElement, TOutVertex, TInVertex>.ThenBy(string lambda) => By(lambda);

        IOrderedEGremlinQuery<TElement, TOutVertex, TInVertex> IOrderedEGremlinQuery<TElement, TOutVertex, TInVertex>.ThenByDescending(Expression<Func<TElement, object>> projection) => By(projection, Order.Decreasing);

        IOrderedEGremlinQuery<TElement, TOutVertex, TInVertex> IOrderedEGremlinQuery<TElement, TOutVertex, TInVertex>.ThenByDescending(Func<IGremlinQuery<TElement>, IGremlinQuery> traversal) => By(traversal, Order.Decreasing);

        IOrderedGremlinQuery<TElement> IOrderedGremlinQuery<TElement>.ThenBy(Expression<Func<TElement, object>> projection) => By(projection, Order.Increasing);

        IOrderedGremlinQuery<TElement> IOrderedGremlinQuery<TElement>.ThenBy(Func<IGremlinQuery<TElement>, IGremlinQuery> traversal) => By(traversal, Order.Increasing);

        IOrderedGremlinQuery<TElement> IOrderedGremlinQuery<TElement>.ThenBy(string lambda) => By(lambda);

        IOrderedGremlinQuery<TElement> IOrderedGremlinQuery<TElement>.ThenByDescending(Expression<Func<TElement, object>> projection) => By(projection, Order.Decreasing);

        IOrderedGremlinQuery<TElement> IOrderedGremlinQuery<TElement>.ThenByDescending(Func<IGremlinQuery<TElement>, IGremlinQuery> traversal) => By(traversal, Order.Decreasing);

        IOrderedInEGremlinQuery<TElement, TInVertex> IOrderedInEGremlinQuery<TElement, TInVertex>.ThenBy(Expression<Func<TElement, object>> projection) => By(projection, Order.Increasing);

        IOrderedInEGremlinQuery<TElement, TInVertex> IOrderedInEGremlinQuery<TElement, TInVertex>.ThenBy(Func<IGremlinQuery<TElement>, IGremlinQuery> traversal) => By(traversal, Order.Increasing);

        IOrderedInEGremlinQuery<TElement, TInVertex> IOrderedInEGremlinQuery<TElement, TInVertex>.ThenBy(string lambda) => By(lambda);

        IOrderedInEGremlinQuery<TElement, TInVertex> IOrderedInEGremlinQuery<TElement, TInVertex>.ThenByDescending(Expression<Func<TElement, object>> projection) => By(projection, Order.Decreasing);

        IOrderedInEGremlinQuery<TElement, TInVertex> IOrderedInEGremlinQuery<TElement, TInVertex>.ThenByDescending(Func<IGremlinQuery<TElement>, IGremlinQuery> traversal) => By(traversal, Order.Decreasing);

        IOrderedOutEGremlinQuery<TElement, TOutVertex> IOrderedOutEGremlinQuery<TElement, TOutVertex>.ThenBy(Expression<Func<TElement, object>> projection) => By(projection, Order.Increasing);

        IOrderedOutEGremlinQuery<TElement, TOutVertex> IOrderedOutEGremlinQuery<TElement, TOutVertex>.ThenBy(Func<IGremlinQuery<TElement>, IGremlinQuery> traversal) => By(traversal, Order.Increasing);

        IOrderedOutEGremlinQuery<TElement, TOutVertex> IOrderedOutEGremlinQuery<TElement, TOutVertex>.ThenBy(string lambda) => By(lambda);

        IOrderedOutEGremlinQuery<TElement, TOutVertex> IOrderedOutEGremlinQuery<TElement, TOutVertex>.ThenByDescending(Expression<Func<TElement, object>> projection) => By(projection, Order.Decreasing);

        IOrderedOutEGremlinQuery<TElement, TOutVertex> IOrderedOutEGremlinQuery<TElement, TOutVertex>.ThenByDescending(Func<IGremlinQuery<TElement>, IGremlinQuery> traversal) => By(traversal, Order.Decreasing);

        IOrderedEGremlinQuery<TElement> IOrderedEGremlinQuery<TElement>.ThenBy(Expression<Func<TElement, object>> projection) => By(projection, Order.Increasing);

        IOrderedEGremlinQuery<TElement> IOrderedEGremlinQuery<TElement>.ThenBy(Func<IGremlinQuery<TElement>, IGremlinQuery> traversal) => By(traversal, Order.Increasing);

        IOrderedEGremlinQuery<TElement> IOrderedEGremlinQuery<TElement>.ThenBy(string lambda) => By(lambda);

        IOrderedEGremlinQuery<TElement> IOrderedEGremlinQuery<TElement>.ThenByDescending(Expression<Func<TElement, object>> projection) => By(projection, Order.Decreasing);

        IOrderedEGremlinQuery<TElement> IOrderedEGremlinQuery<TElement>.ThenByDescending(Func<IGremlinQuery<TElement>, IGremlinQuery> traversal) => By(traversal, Order.Decreasing);
        #endregion

        #region To (step label)
        IInEGremlinQuery<TElement, TNewInVertex> IEGremlinQuery<TElement>.To<TNewInVertex>(StepLabel<TNewInVertex> stepLabel) => To<TElement, Unit, TNewInVertex>(stepLabel);

        IEGremlinQuery<TElement, TOutVertex, TTargetVertex> IEGremlinQuery<TElement, TOutVertex>.To<TTargetVertex>(StepLabel<TTargetVertex> stepLabel) => To<TElement, TOutVertex, TTargetVertex>(stepLabel);

        private GremlinQueryImpl<TNewElement, TNewOutVertex, TNewInVertex, Unit, Unit> To<TNewElement, TNewOutVertex, TNewInVertex>(StepLabel<TNewInVertex> stepLabel) => AddStep<TNewElement, TNewOutVertex, TNewInVertex, Unit, Unit>(new ToLabelStep(stepLabel));
        #endregion

        #region To (traversal)
        IEGremlinQuery<TElement, TOutVertex, TTargetVertex> IEGremlinQuery<TElement, TOutVertex>.To<TTargetVertex>(Func<IVGremlinQuery<TOutVertex>, IGremlinQuery<TTargetVertex>> toVertexTraversal) => AddStep<TElement, TOutVertex, TTargetVertex, Unit, Unit>(new ToTraversalStep(toVertexTraversal(Anonymize<TOutVertex, Unit, Unit, Unit, Unit>())));

        IInEGremlinQuery<TElement, TNewInVertex> IEGremlinQuery<TElement>.To<TNewInVertex>(Func<IGremlinQuery, IGremlinQuery<TNewInVertex>> toVertexTraversal) => To<TElement, Unit, TNewInVertex>(toVertexTraversal);

        IEGremlinQuery<TElement, TOutVertex, TNewInVertex> IOutEGremlinQuery<TElement, TOutVertex>.To<TNewInVertex>(Func<IGremlinQuery, IGremlinQuery<TNewInVertex>> toVertexTraversal) => To<TElement, TOutVertex, TNewInVertex>(toVertexTraversal);

        private GremlinQueryImpl<TNewElement, TNewOutVertex, TNewInVertex, Unit, Unit> To<TNewElement, TNewOutVertex, TNewInVertex>(Func<GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta>, IGremlinQuery> toVertexTraversal) => AddStep<TNewElement, TNewOutVertex, TNewInVertex, Unit, Unit>(new ToTraversalStep(toVertexTraversal(Anonymize())));
        #endregion

        IGremlinQuery<TItem> IGremlinQuery<TElement>.Unfold<TItem>()
        {
            return AddStep<TItem, Unit, Unit, Unit, Unit>(UnfoldStep.Instance);
        }
        
        TTargetQuery IVGremlinQuery<TElement>.Union<TTargetQuery>(params Func<IVGremlinQuery<TElement>, TTargetQuery>[] unionTraversals)
        {
            return this
                .AddStep(
                    new UnionStep(
                        unionTraversals
                            .Select(unionTraversal => (IGremlinQuery)unionTraversal(Anonymize()))))
                .ChangeQueryType<TTargetQuery>();
        }

        #region V
        IVGremlinQuery<TVertex> IGremlinQuerySource.V<TVertex>(params object[] ids) => AddStep<Unit, Unit, Unit, Unit, Unit>(new VStep(ids)).OfType<TVertex>(_model.VerticesModel, true);

        IVGremlinQuery<IVertex> IGremlinQuerySource.V(params object[] ids) => AddStep<IVertex, Unit, Unit, Unit, Unit>(new VStep(ids));
        #endregion

        #region Values
        IGremlinQuery<TValue> IVGremlinQuery<TElement>.Values<TValue>(params Expression<Func<TElement, TValue>>[] projections) => Values<TElement, TValue, TValue>(GraphElementType.Vertex, projections);

        IGremlinQuery<TValue> IVGremlinQuery<TElement>.Values<TValue, TNewMeta>(params Expression<Func<TElement, VertexProperty<TValue, TNewMeta>>>[] projections) => Values<TElement, VertexProperty<TValue, TNewMeta>, TValue>(GraphElementType.Vertex, projections);

        IGremlinQuery<TValue> IVGremlinQuery<TElement>.Values<TValue>(params Expression<Func<TElement, VertexProperty<TValue>>>[] projections) => Values<TElement, VertexProperty<TValue>, TValue>(GraphElementType.Vertex, projections);

        IGremlinQuery<TValue> IEGremlinQuery<TElement>.Values<TValue>(params Expression<Func<TElement, TValue>>[] projections) => Values<TElement, TValue, TValue>(GraphElementType.Edge, projections);

        IGremlinQuery<TValue> IEGremlinQuery<TElement>.Values<TValue>(params Expression<Func<TElement, Property<TValue>>>[] projections) => Values<TElement, Property<TValue>, TValue>(GraphElementType.Edge, projections);

        IGremlinQuery<object> IVPropertiesGremlinQuery<TElement, TPropertyValue>.Values(params string[] keys) => AddStep<object, Unit, Unit, Unit, Unit>(new ValuesStep(keys));

        IGremlinQuery<TTarget> IVPropertiesGremlinQuery<TElement, TPropertyValue, TMeta>.Values<TTarget>(params Expression<Func<TMeta, TTarget>>[] projections) => Values<TMeta, TTarget, TTarget>(GraphElementType.VertexProperty, projections);

        private GremlinQueryImpl<TNewElement, Unit, Unit, Unit, Unit> Values<TSource, TTarget, TNewElement>(GraphElementType elementType, Expression<Func<TSource, TTarget>>[] projections)
        {
            var keys = projections
                .Select(projection =>
                {
                    if (projection.Body.StripConvert() is MemberExpression memberExpression)
                        return _model.GetIdentifier(elementType, memberExpression.Member.Name);

                    throw new ExpressionNotSupportedException(projection);
                })
                .ToArray();

            return AddStep<TNewElement, Unit, Unit, Unit, Unit>(new ValuesStep(keys));
        }
        #endregion

        #region ValueMap
        IGremlinQuery<IDictionary<string, object>> IVPropertiesGremlinQuery<TElement, TPropertyValue>.ValueMap() => ValueMap<IDictionary<string, object>>();

        IGremlinQuery<TMeta> IVPropertiesGremlinQuery<TElement, TPropertyValue, TMeta>.ValueMap() => ValueMap<TMeta>();

        private GremlinQueryImpl<TNewElement, Unit, Unit, Unit, Unit> ValueMap<TNewElement>() => AddStep<TNewElement, Unit, Unit, Unit, Unit>(new ValueMapStep());
        #endregion

        #region Where (Traversal)
        IGremlinQuery<TElement> IGremlinQuery<TElement>.Where(Func<IGremlinQuery<TElement>, IGremlinQuery> filterTraversal) => AddStep(new WhereTraversalStep(filterTraversal(Anonymize())));

        IEGremlinQuery<TElement, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.Where(Func<IEGremlinQuery<TElement, TOutVertex>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.Where(Func<IEGremlinQuery<TElement>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IEGremlinQuery<TElement, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.Where(Func<IEGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Where(Func<IVGremlinQuery<TElement>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IOutEGremlinQuery<TElement, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.Where(Func<IOutEGremlinQuery<TElement, TOutVertex>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IInEGremlinQuery<TElement, TInVertex> IInEGremlinQuery<TElement, TInVertex>.Where(Func<IInEGremlinQuery<TElement, TInVertex>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta> Where(Func<GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta>, IGremlinQuery> filterTraversal) => AddStep(new WhereTraversalStep(filterTraversal(Anonymize())));
        #endregion

        #region Where (Predicate)
        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Where(Expression<Func<TElement, bool>> predicate) => Where(GraphElementType.Vertex, predicate);

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Where(Expression<Func<TElement, bool>> predicate) => Where(GraphElementType.None, predicate);

        IEGremlinQuery<TElement, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.Where(Expression<Func<TElement, bool>> predicate) => Where(GraphElementType.Edge, predicate);

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.Where(Expression<Func<TElement, bool>> predicate) => Where(GraphElementType.Edge, predicate);

        IEGremlinQuery<TElement, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.Where(Expression<Func<TElement, bool>> predicate) => Where(GraphElementType.Edge, predicate);

        IOutEGremlinQuery<TElement, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.Where(Expression<Func<TElement, bool>> predicate) => Where(GraphElementType.Edge, predicate);

        IInEGremlinQuery<TElement, TInVertex> IInEGremlinQuery<TElement, TInVertex>.Where(Expression<Func<TElement, bool>> predicate) => Where(GraphElementType.Edge, predicate);

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta> Where(GraphElementType elementType, Expression<Func<TElement, bool>> predicate)
        {
            var body = predicate.Body;

            try
            {
                switch (body)
                {
                    case UnaryExpression unaryExpression:
                    {
                        if (unaryExpression.NodeType == ExpressionType.Not)
                            return Not(_ => _.Where(elementType, Expression.Lambda<Func<TElement, bool>>(unaryExpression.Operand, predicate.Parameters)));

                        break;
                    }
                    case MemberExpression memberExpression:
                    {
                        if (memberExpression.Member is PropertyInfo property && property.PropertyType == typeof(bool))
                            return Where(elementType, predicate.Parameters[0], memberExpression, Expression.Constant(true), ExpressionType.Equal);

                        break;
                    }
                    case BinaryExpression binaryExpression:
                        return Where(elementType, predicate.Parameters[0], binaryExpression.Left.StripConvert(), binaryExpression.Right.StripConvert(), binaryExpression.NodeType);
                    case MethodCallExpression methodCallExpression:
                    {
                        var methodInfo = methodCallExpression.Method;

                        if (methodInfo.IsEnumerableAny())
                        {
                            if (methodCallExpression.Arguments[0] is MethodCallExpression previousExpression && previousExpression.Method.IsEnumerableIntersect())
                            {
                                if (previousExpression.Arguments[0] is MemberExpression sourceMember)
                                    return HasWithin(elementType, sourceMember, previousExpression.Arguments[1]);

                                if (previousExpression.Arguments[1] is MemberExpression argument && argument.Expression == predicate.Parameters[0])
                                    return HasWithin(elementType, argument, previousExpression.Arguments[0]);
                            }
                            else
                                return Where(elementType, predicate.Parameters[0], methodCallExpression.Arguments[0], Expression.Constant(null, methodCallExpression.Arguments[0].Type), ExpressionType.NotEqual);
                        }
                        else if (methodInfo.IsEnumerableContains())
                        {
                            if (methodCallExpression.Arguments[0] is MemberExpression sourceMember && sourceMember.Expression == predicate.Parameters[0])
                                return Has(elementType, sourceMember, new P.Eq(methodCallExpression.Arguments[1].GetValue()));

                            if (methodCallExpression.Arguments[1] is MemberExpression argument && argument.Expression == predicate.Parameters[0])
                                return HasWithin(elementType, argument, methodCallExpression.Arguments[0]);
                        }
                        else if (methodInfo.IsStringStartsWith())
                        {
                            if (methodCallExpression.Arguments[0] is MemberExpression argumentExpression && argumentExpression.Expression == predicate.Parameters[0])
                            {
                                if (methodCallExpression.Object.GetValue() is string stringValue)
                                {
                                    return HasWithin(
                                        elementType,
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
                                        return Has(elementType, memberExpression, P.True);

                                    if (lowerBound[lowerBound.Length - 1] == char.MaxValue)
                                        upperBound = lowerBound + char.MinValue;
                                    else
                                    {
                                        var upperBoundChars = lowerBound.ToCharArray();

                                        upperBoundChars[upperBoundChars.Length - 1]++;
                                        upperBound = new string(upperBoundChars);
                                    }

                                    return Has(elementType, memberExpression, new P.Between(lowerBound, upperBound));
                                }
                            }
                        }

                        break;
                    }
                }
            }
            catch (ExpressionNotSupportedException ex)
            {
                throw new ExpressionNotSupportedException(predicate, ex);
            }

            throw new ExpressionNotSupportedException(predicate);
        }

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta> Where(GraphElementType elementType, ParameterExpression parameter, Expression left, Expression right, ExpressionType nodeType)
        {
            if (nodeType == ExpressionType.OrElse || nodeType == ExpressionType.AndAlso)
            {
                var leftLambda = Expression.Lambda<Func<TElement, bool>>(left, parameter);
                var rightLambda = Expression.Lambda<Func<TElement, bool>>(right, parameter);

                return nodeType == ExpressionType.OrElse
                    ? Or(
                        _ => _.Where(elementType, leftLambda),
                        _ => _.Where(elementType, rightLambda))
                    : And(
                        _ => _.Where(elementType, leftLambda),
                        _ => _.Where(elementType, rightLambda));
            }

            return right is MemberExpression memberExpression && memberExpression.Expression == parameter
                ? Where(elementType, parameter, right, left.GetValue(), nodeType.Switch())
                : Where(elementType, parameter, left, right.GetValue(), nodeType);
        }

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta> Where(GraphElementType elementType, ParameterExpression parameter, Expression left, object rightConstant, ExpressionType nodeType)
        {
            if (rightConstant == null)
            {
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (nodeType)
                {
                    case ExpressionType.Equal:
                        return HasNot(elementType, left);
                    case ExpressionType.NotEqual:
                        return Has(elementType, left, P.True);
                }
            }
            else
            {
                var predicateArgument = nodeType.ToP(rightConstant);

                switch (left)
                {
                    case MemberExpression leftMemberExpression when parameter == leftMemberExpression.Expression:
                    {
                        if (typeof(PropertyBase).IsAssignableFrom(leftMemberExpression.Expression.Type) && leftMemberExpression.Member.Name == nameof(Property<object>.Value))
                            return AddStep(new HasValueStep(predicateArgument));

                        return rightConstant is StepLabel
                            ? Has(elementType, leftMemberExpression, Anonymize().AddStep(new WherePredicateStep(predicateArgument)))
                            : Has(elementType, leftMemberExpression, predicateArgument);
                    }
                    case ParameterExpression leftParameterExpression when parameter == leftParameterExpression:
                    {
                        return AddStep(
                            rightConstant is StepLabel
                                ? new WherePredicateStep(predicateArgument)
                                : (Step)new IsStep(predicateArgument));
                    }
                }
            }

            throw new ExpressionNotSupportedException();
        }
        #endregion

        #region Where (PropertyTraversal)
        IGremlinQuery<TElement> IGremlinQuery<TElement>.Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal) => Where(GraphElementType.None, projection, propertyTraversal);

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal) => Where(GraphElementType.Vertex, projection, propertyTraversal);

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal) => Where(GraphElementType.Edge, projection, propertyTraversal);

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta> Where<TProjection>(GraphElementType elementType, Expression<Func<TElement, TProjection>> predicate, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal) => Has(elementType, predicate.Body, propertyTraversal(Anonymize<TProjection, Unit, Unit, Unit, Unit>()));
        #endregion

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta> Has(GraphElementType elementType, Expression expression, P predicate) => AddStep(new HasStep(GetIdentifier(elementType, expression), predicate));

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta> Has(GraphElementType elementType, Expression expression, IGremlinQuery traversal) => AddStep(new HasStep(GetIdentifier(elementType, expression), traversal));

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta> HasNot(GraphElementType elementType, Expression expression) => AddStep(new HasNotStep(GetIdentifier(elementType, expression)));

        private object GetIdentifier(GraphElementType elementType, Expression expression)
        {
            string memberName;

            switch (expression)
            {
                case MemberExpression leftMemberExpression:
                {
                    memberName = leftMemberExpression.Member.Name;
                    break;
                }
                case ParameterExpression leftParameterExpression:
                {
                    memberName = leftParameterExpression.Name;
                    break;
                }
                default:
                    throw new ExpressionNotSupportedException(expression);
            }

            return _model.GetIdentifier(elementType, memberName);
        }

        public IAsyncEnumerator<TElement> GetEnumerator()
        {
            return _queryExecutor
                .Execute(this)
                .GetEnumerator();
        }

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta> HasWithin(GraphElementType elementType, Expression expression, Expression enumerableExpression)
        {
            if (enumerableExpression.GetValue() is IEnumerable enumerable)
            {
                return HasWithin(elementType, expression, enumerable);
            }

            throw new ExpressionNotSupportedException(enumerableExpression);
        }

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta> HasWithin(GraphElementType elementType, Expression expression, IEnumerable enumerable)
        {
            var objectArray = enumerable as object[] ?? enumerable.Cast<object>().ToArray();

            return Has(
                elementType,
                expression,
                new P.Within(objectArray));
        }

        #region AddStep
        private GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta> AddStep(Step step) => AddStep<TElement>(step);

        private GremlinQueryImpl<TNewElement, TOutVertex, TInVertex, TPropertyValue, TMeta> AddStep<TNewElement>(Step step) => AddStep<TNewElement, TOutVertex, TInVertex, TPropertyValue, TMeta>(step);

        private GremlinQueryImpl<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta> AddStep<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta>(Step step) => new GremlinQueryImpl<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta>(_model, _queryExecutor, _steps.Insert(_steps.Count, step), _stepLabelMappings, _logger);
        #endregion

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta> AddStepLabelBinding(Expression<Func<TElement, object>> memberExpression, StepLabel stepLabel)
        {
            var body = memberExpression.Body.StripConvert();

            if (!(body is MemberExpression memberExpressionBody))
                throw new ExpressionNotSupportedException(memberExpression);

            var name = memberExpressionBody.Member.Name;

            if (_stepLabelMappings.TryGetValue(stepLabel, out var existingName) && existingName != name)
                throw new InvalidOperationException($"A StepLabel was already bound to {name} by a previous Select operation. Try changing the position of the StepLabel in the Select operation or introduce a new StepLabel.");

            return new GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta>(_model, _queryExecutor, _steps, _stepLabelMappings.Add(stepLabel, name), _logger);
        }

        #region Anonymize
        private GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta> Anonymize() => Anonymize<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta>();

        private GremlinQueryImpl<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta> Anonymize<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta>() => new GremlinQueryImpl<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta>(_model, GremlinQueryExecutor.Invalid, ImmutableList<Step>.Empty, ImmutableDictionary<StepLabel, string>.Empty, _logger);
        #endregion

        #region ChangeQueryType
        TTargetQuery IGremlinQueryAdmin.ChangeQueryType<TTargetQuery>() => ChangeQueryType<TTargetQuery>();

        private TTargetQuery ChangeQueryType<TTargetQuery>() where TTargetQuery : IGremlinQuery
        {
            var metaType = typeof(Unit);
            var elementType = typeof(Unit);
            var inVertexType = typeof(Unit);
            var outVertexType = typeof(Unit);
            var propertyValueType = typeof(Unit);

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

                if (genericTypeDef == typeof(IVPropertiesGremlinQuery<,>))
                {
                    propertyValueType = typeof(TTargetQuery).GetGenericArguments()[1];
                    metaType = typeof(TTargetQuery).GetGenericArguments()[2];
                }
            }

            var type = typeof(GremlinQueryImpl<,,,,>).MakeGenericType(elementType, outVertexType, inVertexType, propertyValueType, metaType);
            return (TTargetQuery)Activator.CreateInstance(type, _model, _queryExecutor, _steps, _stepLabelMappings, _logger);
        }
        #endregion

        private GremlinQueryImpl<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta> AddElementProperties(GraphElementType elementType, object element)
        {
            var ret = this;

            foreach (var (propertyInfo, value) in element.Serialize())
            {
                ret = ret.AddStep(new PropertyStep(propertyInfo.PropertyType, _model.GetIdentifier(elementType, propertyInfo.Name), value));
            }

            return ret;
        }

        IGraphModel IGremlinQueryAdmin.Model => _model;
        IImmutableList<Step> IGremlinQueryAdmin.Steps => _steps;
        IImmutableDictionary<StepLabel, string> IGremlinQueryAdmin.StepLabelMappings => _stepLabelMappings;
    }
}
