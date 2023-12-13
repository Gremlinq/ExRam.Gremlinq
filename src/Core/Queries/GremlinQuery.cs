#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier
using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

using ExRam.Gremlinq.Core.ExpressionParsing;
using ExRam.Gremlinq.Core.GraphElements;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Core.Projections;
using ExRam.Gremlinq.Core.Steps;

using Gremlin.Net.Process.Traversal;

using Microsoft.Extensions.Logging;

using Path = ExRam.Gremlinq.Core.GraphElements.Path;

namespace ExRam.Gremlinq.Core
{
    internal sealed partial class GremlinQuery<T1, T2, T3, T4> : GremlinQueryBase
    {
        public GremlinQuery(
            IGremlinQueryEnvironment environment,
            Traversal steps,
            IImmutableDictionary<StepLabel, LabelProjections> labelProjections,
            IImmutableDictionary<object, object?> metadata) : base(environment, steps, labelProjections, metadata)
        {

        }

        private GremlinQuery<TEdge, T1, object, IGremlinQueryBase> AddE<TEdge>(TEdge newEdge) => this
            .Continue()
            .Build(
                static (builder, newEdge) => builder
                    .AddStep(new AddEStep(builder.OuterQuery.Environment.Model.EdgesModel.GetCache().GetLabel(newEdge!.GetType())))
                    .WithNewProjection(Projection.Edge)
                    .AutoBuild<TEdge, T1>(),
                newEdge)
            .AddOrUpdate(newEdge, true);

        private GremlinQuery<T1, T2, T3, T4> AddOrUpdate(T1 element, bool add)
        {
            var ret = this;
            var props = element
                .Serialize(
                    Environment,
                    add
                        ? SerializationBehaviour.IgnoreOnAdd
                        : SerializationBehaviour.IgnoreOnUpdate)
                .ToArray();

            var droppableKeys = new List<string>();
            var propertySteps = new List<PropertyStep>();

            foreach (var (key, maybeValue) in props)
            {
                if (!Environment.FeatureSet.Supports(VertexFeatures.UserSuppliedIds) && T.Id.Equals(key.RawKey))
                    Environment.Logger.LogWarning($"User supplied ids are not supported according to the environment's {nameof(Environment.FeatureSet)}.");
                else
                {
                    var localPropertySteps = maybeValue is { } value
                        ? this
                            .GetPropertySteps(key, value, Steps.Projection == Projection.Vertex)
                            .ToArray()
                        : Array.Empty<PropertyStep>();

                    if (!add && key.RawKey is string rawStringKey && localPropertySteps.All(static propertyStep => Cardinality.List.Equals(propertyStep.Cardinality)))
                        droppableKeys.Add(rawStringKey);

                    propertySteps.AddRange(localPropertySteps);
                }
            }

            if (droppableKeys.Count > 0)
            {
                ret = ret
                    .SideEffect(__ => __
                        .Properties<object, object, object>(
                            Projection.Empty,
                            droppableKeys)
                        .Drop());
            }

            return ret
                .Continue()
                .Build(
                    static (builder, propertySteps) => builder
                        .AddSteps(propertySteps)
                        .Build(),
                    propertySteps);
        }

        private GremlinQuery<TVertex, object, object, IGremlinQueryBase> AddV<TVertex>(TVertex vertex) => this
            .Continue()
            .Build(
                static (builder, vertex) => builder
                    .AddStep(new AddVStep(builder.OuterQuery.Environment.Model.VerticesModel.GetCache().GetLabel(vertex!.GetType())))
                    .WithNewProjection(Projection.Vertex)
                    .AutoBuild<TVertex>(),
                vertex)
            .AddOrUpdate(vertex, true);

        private TTargetQuery Aggregate<TStepLabel, TTargetQuery>(Scope scope, Func<GremlinQuery<T1, T2, T3, T4>, TStepLabel, TTargetQuery> continuation)
            where TStepLabel : StepLabel, new()
            where TTargetQuery : IGremlinQueryBase
        {
            var stepLabel = new TStepLabel();

            return this
                .Aggregate(scope, stepLabel)
                .Map(continuation, stepLabel);
        }

        private GremlinQuery<T1, T2, T3, T4> Aggregate<TStepLabel>(Scope scope, TStepLabel stepLabel)
            where TStepLabel : StepLabel => this
                .Continue()
                .Build(
                    static (builder, tuple) => builder
                        .AddStep(new AggregateStep(tuple.scope, tuple.stepLabel))
                        .WithNewLabelProjections(
                            static (existingProjections, tuple) => existingProjections.Set(
                                tuple.stepLabel,
                                tuple.projection,
                                static (projections, projection) => projections.WithSideEffectLabelProjection(projection)),
                            (tuple.stepLabel, projection: builder.OuterQuery.Steps.Projection.Fold()))
                        .Build(),
                    (scope, stepLabel));

        private GremlinQuery<T1, T2, T3, T4> And<TState>(Func<GremlinQuery<T1, T2, T3, T4>, TState, IGremlinQueryBase> continuation1, Func<GremlinQuery<T1, T2, T3, T4>, TState, IGremlinQueryBase> continuation2, TState state) => And(this
            .Continue(ContinuationFlags.Filter)
            .With(continuation1, state)
            .With(continuation2, state));

        private GremlinQuery<T1, T2, T3, T4> And(Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase>[] continuations) => And(this
            .Continue(ContinuationFlags.Filter)
            .With(continuations));

        private GremlinQuery<T1, T2, T3, T4> And(MultiContinuationBuilder<GremlinQuery<T1, T2, T3, T4>, GremlinQuery<T1, T2, T3, T4>> continuationBuilder) => continuationBuilder
            .Build(static (builder, traversals) =>
            {
                if (traversals.Length == 0)
                    throw new ArgumentException("Expected at least 1 sub-query.");

                var count = 0;
                var containsNoneStep = false;
                var containsWriteStep = false;

                for (var i = 0; i < traversals.Length; i++)
                {
                    var traversal = traversals[i];

                    if (traversal.IsNone())
                        containsNoneStep = true;

                    if (traversal.SideEffectSemantics == SideEffectSemantics.Write)
                        containsWriteStep = true;
                    else if (traversal.IsIdentity())
                        continue;

                    traversals[count++] = traversal;
                }

                if (containsNoneStep && !containsWriteStep)
                    return builder.OuterQuery.None();

                var fusedTraversals = traversals[..count]
                    .Fuse(static (p1, p2) => p1.And(p2));

                return fusedTraversals.Length switch
                {
                    0 => builder.OuterQuery,
                    1 => builder.OuterQuery
                        .Where(fusedTraversals[0]),
                    _ => builder
                        .AddStep(new AndStep(fusedTraversals.ToArray()))
                        .Build()
                };
            });

        private TTargetQuery As<TTargetQuery>(Func<GremlinQuery<T1, T2, T3, T4>, StepLabel<GremlinQuery<T1, T2, T3, T4>, T1>, TTargetQuery> continuation)
            where TTargetQuery : IGremlinQueryBase
        {
            return As<StepLabel<GremlinQuery<T1, T2, T3, T4>, T1>, TTargetQuery>(continuation);
        }

        private TTargetQuery As<TStepLabel, TTargetQuery>(Func<GremlinQuery<T1, T2, T3, T4>, TStepLabel, TTargetQuery> continuation)
            where TStepLabel : StepLabel, new()
            where TTargetQuery : IGremlinQueryBase
        {
            TStepLabel stepLabel;
            var toContinue = this;

            if (Steps.PeekOrDefault() is AsStep { StepLabel: TStepLabel existingStepLabel })
                stepLabel = existingStepLabel;
            else
            {
                stepLabel = new TStepLabel();
                toContinue = As(stepLabel);
            }

            return toContinue
                .Map(continuation, stepLabel);
        }

        private GremlinQuery<T1, T2, T3, T4> As(StepLabel stepLabel) => this
            .Continue()
            .Build(
                static (builder, stepLabel) => builder
                    .AddStep(new AsStep(stepLabel))
                    .WithNewLabelProjections(
                        static (projection, tuple) => projection.Set(
                            tuple.stepLabel,
                            tuple.otherProjection,
                            static (existingProjections, otherProjection) => existingProjections.WithStepLabelProjection(otherProjection)),
                        (stepLabel, otherProjection: builder.OuterQuery.Steps.Projection))
                    .Build(),
                stepLabel);

        private GremlinQuery<T1, T2, T3, T4> Barrier() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(BarrierStep.Instance)
                .Build());

        private GremlinQuery<object, object, object, IGremlinQueryBase> Both() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(BothStep.NoLabels)
                .AutoBuild());

        private GremlinQuery<object, object, object, IGremlinQueryBase> Both<TEdge>() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(new BothStep(builder.OuterQuery.Environment.Model.EdgesModel.GetFilterLabelsOrDefault(typeof(TEdge), builder.OuterQuery.Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity))))
                .AutoBuild());

        private GremlinQuery<object, object, object, IGremlinQueryBase> BothE() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(BothEStep.NoLabels)
                .WithNewProjection(Projection.Edge)
                .AutoBuild());

        private GremlinQuery<TEdge, object, object, IGremlinQueryBase> BothE<TEdge>() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(new BothEStep(builder.OuterQuery.Environment.Model.EdgesModel.GetFilterLabelsOrDefault(typeof(TEdge), builder.OuterQuery.Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity))))
                .WithNewProjection(Projection.Edge)
                .AutoBuild<TEdge>());

        private GremlinQuery<TTarget, object, object, IGremlinQueryBase> BothV<TTarget>() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(BothVStep.Instance)
                .WithNewProjection(Projection.Vertex)
                .AutoBuild<TTarget>());

        private GremlinQuery<TSelectedElement, TArrayItem, object, TQuery> Cap<TSelectedElement, TArrayItem, TQuery>(StepLabel<IArrayGremlinQuery<TSelectedElement, TArrayItem, TQuery>, TSelectedElement> stepLabel) where TQuery : IGremlinQueryBase => this
            .Continue()
            .Build(
                static (builder, stepLabel) => builder
                    .AddStep(new CapStep(stepLabel))
                    .WithNewProjection(static projection => projection.Fold())
                    .AutoBuild<TSelectedElement, TArrayItem, object, TQuery>(),
                stepLabel);

        private TTargetQuery Choose<TTrueQuery, TFalseQuery, TTargetQuery>(Expression<Func<T1, bool>> predicate, Func<GremlinQuery<T1, T2, T3, T4>, TTrueQuery> trueChoice, Func<GremlinQuery<T1, T2, T3, T4>, TFalseQuery>? maybeFalseChoice = default)
            where TTrueQuery : IGremlinQueryBase
            where TFalseQuery : IGremlinQueryBase
            where TTargetQuery : IGremlinQueryBase => this
                .Choose<TTrueQuery, TFalseQuery, TTargetQuery>(
                    __ => __
                        .Where(predicate),
                    trueChoice,
                    maybeFalseChoice);

        private TTargetQuery Choose<TTrueQuery, TFalseQuery, TTargetQuery>(Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase> traversalPredicate, Func<GremlinQuery<T1, T2, T3, T4>, TTrueQuery> trueChoice, Func<GremlinQuery<T1, T2, T3, T4>, TFalseQuery>? maybeFalseChoice = default)
            where TTrueQuery : IGremlinQueryBase
            where TFalseQuery : IGremlinQueryBase
            where TTargetQuery : IGremlinQueryBase => this
                .Continue()
                .With(traversalPredicate)
                .Build(
                    static (builder, traversal, choiceTuple) => builder.OuterQuery.Choose<TTrueQuery, TFalseQuery, TTargetQuery>(traversal, choiceTuple.trueChoice, choiceTuple.maybeFalseChoice),
                    (trueChoice, maybeFalseChoice));

        private TTargetQuery Choose<TTrueQuery, TFalseQuery, TTargetQuery>(Traversal chooseTraversal, Func<GremlinQuery<T1, T2, T3, T4>, TTrueQuery> trueChoice, Func<GremlinQuery<T1, T2, T3, T4>, TFalseQuery>? maybeFalseChoice = default)
            where TTrueQuery : IGremlinQueryBase
            where TFalseQuery : IGremlinQueryBase
            where TTargetQuery : IGremlinQueryBase => this
                .Continue()
                .With(trueChoice)
                .Build(
                    static (builder, trueTraversal, state) =>
                    {
                        var (chooseTraversal, maybeFalseChoice) = state;

                        if (maybeFalseChoice is { } falseChoice)
                        {
                            return builder.OuterQuery
                                .Continue()
                                .With(falseChoice)
                                .Build(
                                    static (builder, falseTraversal, state) =>
                                    {
                                        var (chooseTraversal, trueTraversal) = state;

                                        return builder
                                            .AddStep(chooseTraversal is [IsStep isStep]
                                                ? new ChoosePredicateStep(
                                                    isStep.Predicate,
                                                    trueTraversal,
                                                    falseTraversal)
                                                : new ChooseTraversalStep(
                                                    chooseTraversal,
                                                    trueTraversal,
                                                    falseTraversal))
                                            .WithNewProjection(
                                                static (_, state) => state.falseTraversal.Projection.Lowest(state.trueTraversal.Projection),
                                                (falseTraversal, trueTraversal))
                                            .Build<TTargetQuery>();
                                    },
                                    (chooseTraversal, trueTraversal));
                        }

                        return builder
                            .AddStep(chooseTraversal is [IsStep isStep]
                                ? new ChoosePredicateStep(
                                    isStep.Predicate,
                                    trueTraversal)
                                : new ChooseTraversalStep(
                                    state.chooseTraversal,
                                    trueTraversal))
                            .WithNewProjection(
                                static (projection, otherProjection) => projection.Lowest(otherProjection),
                                trueTraversal.Projection)
                            .Build<TTargetQuery>();
                    },
                    (chooseTraversal, maybeFalseChoice));

        private TTargetQuery Choose<TTargetQuery>(Func<IChooseBuilder<GremlinQuery<T1, T2, T3, T4>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation)
            where TTargetQuery : IGremlinQueryBase
        {
            return continuation(new ChooseBuilder<GremlinQuery<T1, T2, T3, T4>, object>(this)).TargetQuery;
        }

        private TReturnQuery Coalesce<TTargetQuery, TReturnQuery>(params Func<GremlinQuery<T1, T2, T3, T4>, TTargetQuery>[] continuations)
            where TTargetQuery : IGremlinQueryBase
            where TReturnQuery : IGremlinQueryBase => this
                .Continue()
                .With(continuations)
                .Build(static (builder, traversals) =>
                {
                    if (traversals.Length == 0)
                        throw new ArgumentException("Coalesce must have at least one sub-query.");

                    if (traversals.All(static traversal => traversal.IsIdentity()))
                        return builder.Build<TReturnQuery>();

                    return builder
                        .AddStep(new CoalesceStep(traversals
                            .ToImmutableArray()))
                        .WithNewProjection(traversals
                            .LowestProjection())
                        .Build<TReturnQuery>();
                });

        private GremlinQuery<T1, T2, T3, T4> Coin(double probability) => this
            .Continue()
            .Build(
                static (builder, probability) => builder
                    .AddStep(new CoinStep(probability))
                    .Build(),
                probability);

        private GremlinQuery<TValue, object, object, IGremlinQueryBase> Constant<TValue>(TValue constant) => this
            .Continue()
            .Build(
                static (builder, constant) => builder
                    .AddStep(new ConstantStep(constant!))
                    .WithNewProjection(Projection.Value)
                    .AutoBuild<TValue>(),
                constant);

        private GremlinQuery<long, object, object, IGremlinQueryBase> Count(Scope scope) => this
            .Continue()
            .Build(
                static (builder, scope) => builder
                    .AddStep(Scope.Global.Equals(scope)
                        ? CountStep.Global
                        : CountStep.Local)
                    .WithNewProjection(Projection.Value)
                    .AutoBuild<long>(),
                scope);

        private GremlinQuery<long, object, object, IGremlinQueryBase> CountGlobal() => Count(Scope.Global);

        private GremlinQuery<long, object, object, IGremlinQueryBase> CountLocal() => Count(Scope.Local);

        private GremlinQuery<T1, T2, T3, T4> CyclicPath() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(CyclicPathStep.Instance)
                .Build());

        private string Debug()
        {
            var serialized = Environment.Serializer
                .TransformTo<Bytecode>()
                .From(this, Environment);

            return Environment.Debugger.Debug(serialized, Environment);
        }

        private GremlinQuery<T1, T2, T3, T4> DedupGlobal() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(DedupStep.Global)
                .Build());

        private GremlinQuery<T1, T2, T3, T4> DedupLocal() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(DedupStep.Local)
                .Build());

        private GremlinQuery<object, object, object, IGremlinQueryBase> Drop() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(DropStep.Instance)
                .WithNewProjection(Projection.Empty)
                .AutoBuild());

        private GremlinQuery<T1, T2, T3, T4> DropProperties(string key) => this
            .SideEffect(_ => _
                .Properties<object, object, object>(
                    Projection.Empty,
                    new[] { key })
                .Drop());

        private GremlinQuery<object, object, object, IGremlinQueryBase> E(ImmutableArray<object> ids) => this
            .Continue()
            .Build(
                static (builder, ids) => builder
                    .AddStep(new EStep(ids))
                    .WithNewProjection(Projection.Edge)
                    .AutoBuild(),
                ids);

        private GremlinQuery<string, object, object, IGremlinQueryBase> Explain() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(ExplainStep.Instance)
                .WithNewProjection(Projection.Value)
                .AutoBuild<string>());

        private GremlinQuery<object, object, object, IGremlinQueryBase> Fail(string? message = null) => this
            .Continue()
            .Build(
                static (builder, message) => builder
                    .AddStep(message is { } actualMessage
                        ? new FailStep(actualMessage)
                        : FailStep.NoMessage)
                    .WithNewProjection(Projection.Empty)
                    .AutoBuild(),
                message);

        private TTargetQuery FlatMap<TTargetQuery>(Func<GremlinQuery<T1, T2, T3, T4>, TTargetQuery> continuation) where TTargetQuery : IGremlinQueryBase => this
            .Continue()
            .With(continuation)
            .Build(static (builder, innerTraversal) => builder
                .AddStep(new FlatMapStep(innerTraversal))
                .WithNewProjection(innerTraversal.Projection)
                .Build<TTargetQuery>());

        private GremlinQuery<T1[], T1, object, TNewFoldedQuery> Fold<TNewFoldedQuery>() where TNewFoldedQuery : IGremlinQueryBase => this
            .Continue()
            .Build(static builder => builder
                .AddStep(FoldStep.Instance)
                .WithNewProjection(static projection => projection.Fold())
                .AutoBuild<T1[], T1, object, TNewFoldedQuery>());

        private GremlinQuery<T1, TNewOutVertex, TInVertex, IGremlinQueryBase> From<TNewOutVertex, TInVertex>(Func<GremlinQuery<TInVertex, T2, T3, T4>, IVertexGremlinQueryBase<TNewOutVertex>> fromVertexContinuation) => this
            .Continue<TInVertex, T2, T3, T4>()
            .With(fromVertexContinuation)
            .Build(static (builder, fromVertexTraversal) => builder
                .AddStep(new AddEStep.FromTraversalStep(fromVertexTraversal))
                .AutoBuild<T1, TNewOutVertex, TInVertex>());

        private GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, IGremlinQueryBase> From<TNewElement, TNewOutVertex, TNewInVertex>(StepLabel<TNewOutVertex> label) => this
           .Continue()
           .Build(
                static (builder, label) => builder
                   .AddStep(new AddEStep.FromLabelStep(label))
                   .AutoBuild<TNewElement, TNewOutVertex, TNewInVertex>(),
                label);

        private IMapGremlinQuery<IDictionary<TKey, TValue>> Group<TKey, TValue>(Func<IGroupBuilder<GremlinQuery<T1, T2, T3, T4>>, IGroupBuilderWithKeyAndValue<TKey, TValue>> projection) =>
            projection(new GroupBuilder<object, object>(Continue())).Build();

        private IMapGremlinQuery<IDictionary<TKey, T1[]>> Group<TKey>(Func<IGroupBuilder<GremlinQuery<T1, T2, T3, T4>>, IGroupBuilderWithKey<IGremlinQueryBase<T1>, TKey>> projection) => new GroupBuilder<object, object>(Continue())
            .Map(projection)
            .ByValue(__ => __
                .Cast<T1>()
                .Fold())
            .Build();

        private GremlinQuery<object, object, object, IGremlinQueryBase> Id() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(IdStep.Instance)
                .WithNewProjection(Projection.Value)
                .AutoBuild());

        private GremlinQuery<T1, T2, T3, T4> Identity() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(IdentityStep.Instance)
                .Build());

        private GremlinQuery<object, object, object, IGremlinQueryBase> In() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(InStep.NoLabels)
                .AutoBuild());

        private GremlinQuery<object, object, object, IGremlinQueryBase> In<TEdge>() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(new InStep(builder.OuterQuery.Environment.Model.EdgesModel.GetFilterLabelsOrDefault(typeof(TEdge), builder.OuterQuery.Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity))))
                .AutoBuild());

        private GremlinQuery<object, object, object, IGremlinQueryBase> InE() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(InEStep.NoLabels)
                .WithNewProjection(Projection.Edge)
                .AutoBuild());

        private GremlinQuery<TEdge, object, T1, IGremlinQueryBase> InE<TEdge>() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(new InEStep(builder.OuterQuery.Environment.Model.EdgesModel.GetFilterLabelsOrDefault(typeof(TEdge), builder.OuterQuery.Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity))))
                .WithNewProjection(Projection.Edge)
                .AutoBuild<TEdge, object, T1>());

        private GremlinQuery<TNewElement, T2, T3, T4> Inject<TNewElement>(IEnumerable<TNewElement> elements) => this
            .Continue()
            .Build(
                static (builder, elements) => builder
                    .AddStep(new InjectStep(
                        elements
                            .Where(static x => x is not null)
                            .Select(static x => (object)x!)
                            .ToImmutableArray()))
                    .WithNewProjection(Projection.Value)
                    .AutoBuild<TNewElement, T2, T3, T4>(),
                elements);

        private GremlinQuery<TNewElement, object, object, IGremlinQueryBase> InOutV<TNewElement>(Step step) => this
            .Continue()
            .Build(
                static (builder, step) => builder
                    .AddStep(step)
                    .WithNewProjection(Projection.Vertex)
                    .AutoBuild<TNewElement>(),
                step);

        private GremlinQuery<TNewElement, object, object, IGremlinQueryBase> InV<TNewElement>() => InOutV<TNewElement>(InVStep.Instance);

        private GremlinQuery<string, object, object, IGremlinQueryBase> Key() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(KeyStep.Instance)
                .WithNewProjection(Projection.Value)
                .AutoBuild<string>());

        private GremlinQuery<string, object, object, IGremlinQueryBase> Label() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(LabelStep.Instance)
                .WithNewProjection(Projection.Value)
                .AutoBuild<string>());

        private GremlinQuery<T1, T2, T3, T4> LimitGlobal(long count) => this
            .Continue()
            .Build(
                static (builder, count) => builder
                    .AddStep(count == 1
                        ? LimitStep.LimitGlobal1
                        : new LimitStep(count, Scope.Global))
                    .Build(),
                count);

        private GremlinQuery<T1, T2, T3, T4> LimitLocal(long count) => this
            .Continue()
            .Build(
                static (builder, count) => builder
                    .AddStep(count == 1
                        ? builder.OuterQuery.Environment.Options.GetValue(GremlinqOption.WorkaroundRangeInconsistencies)
                            ? LimitStep.LimitLocal1Workaround
                            : LimitStep.LimitLocal1
                        : new LimitStep(count, Scope.Local))
                    .Build(),
                count);

        private TTargetQuery Local<TTargetQuery>(Func<GremlinQuery<T1, T2, T3, T4>, TTargetQuery> localTraversal) where TTargetQuery : IGremlinQueryBase => this
            .Continue()
            .With(localTraversal)
            .Build(static (builder, continuationTraversal) =>
            {
                if (!continuationTraversal.IsIdentity())
                {
                    builder = builder
                        .AddStep(new LocalStep(continuationTraversal))
                        .WithNewProjection(continuationTraversal.Projection);
                }

                return builder
                    .Build<TTargetQuery>();
            });

        private TTargetQuery Loop<TTargetQuery>(Func<IStartLoopBuilder<TTargetQuery>, IFinalLoopBuilder<TTargetQuery>> loopBuilderTransformation)
            where TTargetQuery : IGremlinQueryBase => loopBuilderTransformation(new LoopBuilder<TTargetQuery>(this)).Build();

        private TTargetQuery Map<TTargetQuery>(Func<GremlinQuery<T1, T2, T3, T4>, TTargetQuery> continuation) where TTargetQuery : IGremlinQueryBase => this
            .Continue()
            .With(continuation)
            .Build(static (builder, innerTraversal) => innerTraversal.IsIdentity()
                ? builder.OuterQuery
                    .CloneAs<TTargetQuery>()
                : builder
                    .AddStep(new MapStep(innerTraversal))
                    .WithNewProjection(innerTraversal.Projection)
                    .Build<TTargetQuery>());

        private GremlinQuery<T1, T2, T3, T4> MaxGlobal() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(MaxStep.Global)
                .WithNewProjection(Projection.Value)
                .Build());

        private TNewQuery MaxLocal<TNewQuery>() where TNewQuery : IGremlinQueryBase => this
            .Continue()
            .Build(static builder => builder
                .AddStep(MaxStep.Local)
                .Build<TNewQuery>());

        private GremlinQuery<T1, T2, T3, T4> MeanGlobal() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(MeanStep.Global)
                .WithNewProjection(Projection.Value)
                .Build());

        private TNewQuery MeanLocal<TNewQuery>() where TNewQuery : IGremlinQueryBase => this
            .Continue()
            .Build(static builder => builder
                .AddStep(MeanStep.Local)
                .Build<TNewQuery>());

        private GremlinQuery<T1, T2, T3, T4> MinGlobal() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(MinStep.Global)
                .WithNewProjection(Projection.Value)
                .Build());

        private TNewQuery MinLocal<TNewQuery>() where TNewQuery : IGremlinQueryBase => this
            .Continue()
            .Build(static builder => builder
                .AddStep(MinStep.Local)
                .Build<TNewQuery>());

        private GremlinQuery<T1, T2, T3, T4> None() => this
            .Continue()
            .Build(static builder => builder.OuterQuery.Steps.IsIdentity()
                ? builder
                    .WithSteps(Traversal.Empty.Push(NoneStep.Instance))
                    .Build()
                : builder
                    .AddStep(NoneStep.Instance)
                    .Build());

        private GremlinQuery<T1, T2, T3, T4> Not<TState>(Func<GremlinQuery<T1, T2, T3, T4>, TState, IGremlinQueryBase> continuation, TState state) => this
            .Continue()
            .With(continuation, state)
            .Build(static (builder, innerTraversal) => innerTraversal.IsIdentity()
                ? builder.OuterQuery
                    .None()
                : innerTraversal.IsNone()
                    ? builder.OuterQuery
                    : builder
                        .AddStep(new NotStep(innerTraversal))
                        .Build());

        private TTargetQuery OfType<TNewElement, TTargetQuery>(IGraphElementModel model, bool force = false) where TTargetQuery : IStartGremlinQuery => this
            .Continue()
            .Build(
                static (builder, tuple) =>
                {
                    var (@this, model, force) = tuple;

                    if (!force && typeof(TNewElement).IsAssignableFrom(typeof(T1)))
                        return @this.CloneAs<TTargetQuery>();

                    var labels = model.TryGetFilterLabels(typeof(TNewElement), @this.Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity)) ?? ImmutableArray.Create(typeof(TNewElement).Name);

                    if (labels.Length > 0)
                        builder = builder.AddStep(new HasLabelStep(labels));

                    return builder
                        .Build<TTargetQuery>();
                },
                (@this: this, model, force));

        private TTargetQuery Optional<TTargetQuery>(Func<GremlinQuery<T1, T2, T3, T4>, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQueryBase => this
            .Continue()
            .With(optionalTraversal)
            .Build(static (builder, continuedTraversal) => builder
                .AddStep(new OptionalStep(continuedTraversal))
                .WithNewProjection(
                    static (projection, otherProjection) => projection.Lowest(otherProjection),
                    continuedTraversal.Projection)
                .Build<TTargetQuery>());

        private GremlinQuery<T1, T2, T3, T4> Or<TState>(Func<GremlinQuery<T1, T2, T3, T4>, TState, IGremlinQueryBase> continuation1, Func<GremlinQuery<T1, T2, T3, T4>, TState, IGremlinQueryBase> continuation2, TState state) => Or(this
            .Continue(ContinuationFlags.Filter)
            .With(continuation1, state)
            .With(continuation2, state));

        private GremlinQuery<T1, T2, T3, T4> Or(Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase>[] continuations) => Or(this
            .Continue(ContinuationFlags.Filter)
            .With(continuations));

        private GremlinQuery<T1, T2, T3, T4> Or(MultiContinuationBuilder<GremlinQuery<T1, T2, T3, T4>, GremlinQuery<T1, T2, T3, T4>> continuationBuilder) => continuationBuilder
            .Build(static (builder, traversals) =>
            {
                if (traversals.Length == 0)
                    throw new ArgumentException("Expected at least 1 sub-query.");

                var count = 0;
                var containsWriteStep = false;
                var containsIdentityStep = false;

                for (var i = 0; i < traversals.Length; i++)
                {
                    var traversal = traversals[i];

                    if (traversal.IsIdentity())
                        containsIdentityStep = true;
                    else if (traversal.SideEffectSemantics == SideEffectSemantics.Write)
                        containsWriteStep = true;
                    else if (traversal.IsNone())
                        continue;

                    traversals[count++] = traversal;
                }

                if (containsIdentityStep && !containsWriteStep)
                    return builder.OuterQuery;

                var fusedTraversals = traversals[..count]
                    .Fuse(static (p1, p2) => p1.Or(p2));

                return fusedTraversals.Length switch
                {
                    0 => builder.OuterQuery
                        .None(),
                    1 => builder.OuterQuery
                        .Where(fusedTraversals[0]),
                    _ => builder
                        .AddStep(new OrStep(fusedTraversals.ToArray()))
                        .Build()
                };
            });

        private TTargetQuery Order<TTargetQuery>(Func<OrderBuilder, IOrderBuilderWithBy<TTargetQuery>> projection) where TTargetQuery : IGremlinQueryBase<T1> => projection(new OrderBuilder(this)).Build();

        private TTargetQuery OrderGlobal<TTargetQuery>(Func<OrderBuilder, IOrderBuilderWithBy<TTargetQuery>> projection) where TTargetQuery : IGremlinQueryBase<T1> => this
            .Continue()
            .Build(
                static (builder, projection) => builder
                    .AddStep(OrderStep.Global)
                    .Build()
                    .Order(projection),
                projection);

        private TTargetQuery OrderLocal<TTargetQuery>(Func<OrderBuilder, IOrderBuilderWithBy<TTargetQuery>> projection) where TTargetQuery : IGremlinQueryBase<T1> => this
            .Continue()
            .Build(
                static (builder, projection) => builder
                    .AddStep(OrderStep.Local)
                    .Build()
                    .Order(projection),
                projection);

        private GremlinQuery<TTarget, object, object, IGremlinQueryBase> OtherV<TTarget>() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(OtherVStep.Instance)
                .WithNewProjection(Projection.Vertex)
                .AutoBuild<TTarget>());

        private GremlinQuery<object, object, object, IGremlinQueryBase> Out() => this
           .Continue()
           .Build(static builder => builder
               .AddStep(OutStep.NoLabels)
               .AutoBuild());

        private GremlinQuery<object, object, object, IGremlinQueryBase> Out<TEdge>() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(new OutStep(builder.OuterQuery.Environment.Model.EdgesModel.GetFilterLabelsOrDefault(typeof(TEdge), builder.OuterQuery.Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity))))
                .AutoBuild());

        private GremlinQuery<object, object, object, IGremlinQueryBase> OutE() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(OutEStep.NoLabels)
                .WithNewProjection(Projection.Edge)
                .AutoBuild());

        private GremlinQuery<TEdge, T1, object, IGremlinQueryBase> OutE<TEdge>() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(new OutEStep(builder.OuterQuery.Environment.Model.EdgesModel.GetFilterLabelsOrDefault(typeof(TEdge), builder.OuterQuery.Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity))))
                .WithNewProjection(Projection.Edge)
                .AutoBuild<TEdge, T1>());

        private GremlinQuery<TNewElement, object, object, IGremlinQueryBase> OutV<TNewElement>() => InOutV<TNewElement>(OutVStep.Instance);

        private GremlinQuery<Path, object, object, IGremlinQueryBase> Path() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(PathStep.Instance)
                .WithNewProjection(Projection.Value)
                .AutoBuild<Path>());

        private GremlinQuery<string, object, object, IGremlinQueryBase> Profile() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(ProfileStep.Instance)
                .WithNewProjection(Projection.Value)
                .AutoBuild<string>());

        private IMapGremlinQuery<TResult> Project<TResult>(Func<IProjectBuilder<GremlinQuery<T1, T2, T3, T4>, T1>, IProjectMapResult<TResult>> continuation)
        {
            return new ProjectBuilder(this)
                .Map(continuation)
                .Build();
        }

        private IGremlinQuery<dynamic> Project(Func<IProjectBuilder<GremlinQuery<T1, T2, T3, T4>, T1>, IProjectDynamicResult> continuation)
        {
            return new ProjectBuilder(this)
                .Map(continuation)
                .Build();
        }

        private IMapGremlinQuery<TResult> Project<TResult>(Func<IProjectBuilder<GremlinQuery<T1, T2, T3, T4>, T1>, IProjectTupleResult<TResult>> continuation)
            where TResult : ITuple
        {
            return new ProjectBuilder(this)
                .Map(continuation)
                .Build();
        }

        private GremlinQuery<TNewElement, TNewPropertyValue, TNewMeta, IGremlinQueryBase> Properties<TNewElement, TNewPropertyValue, TNewMeta>(Projection projection, params Expression[] projections) => Properties<TNewElement, TNewPropertyValue, TNewMeta>(
            projection,
            projections
                .Select(projection => GetKey(projection).RawKey)
                .OfType<string>());

        private GremlinQuery<TNewElement, TNewPropertyValue, TNewMeta, IGremlinQueryBase> Properties<TNewElement, TNewPropertyValue, TNewMeta>(Projection projection, IEnumerable<string> keys) => this
            .Continue()
            .Build(
                static (builder, tuple) => builder
                    .AddStep(new PropertiesStep(tuple.keys.ToImmutableArray()))
                    .WithNewProjection(tuple.projection)
                    .AutoBuild<TNewElement, TNewPropertyValue, TNewMeta>(),
                (keys, projection));

        private GremlinQuery<T1, T2, T3, T4> Property(LambdaExpression projection, object? value) => Property(GetKey(projection), value);

        private GremlinQuery<T1, T2, T3, T4> Property(Key key, object? value) => this
            .Continue()
            .Build(
                static (builder, tuple) =>
                {
                    if (tuple.value == null)
                    {
                        if (tuple.key.RawKey is string stringKey)
                            return builder.OuterQuery.DropProperties(stringKey);

                        throw new InvalidOperationException("Can't set a special property to null.");
                    }

                    foreach (var propertyStep in builder.OuterQuery.GetPropertySteps(tuple.key, tuple.value, builder.OuterQuery.Steps.Projection == Projection.Vertex))
                    {
                        builder = builder.AddStep(propertyStep);
                    }

                    return builder
                        .Build();
                },
                (key, value));

        private GremlinQuery<T1, T2, T3, T4> Property(Key key, Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase> valueContinuation) => this
            .Continue()
            .With(valueContinuation)
            .Build(
                static (builder, valueTraversal, key) => builder.OuterQuery.Property(key, valueTraversal),
                key);

        private GremlinQuery<T1, T2, T3, T4> Property(LambdaExpression projection, Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase> valueContinuation) => this
            .Continue()
            .With(valueContinuation)
            .Build(
                static (builder, valueTraversal, projection) => builder.OuterQuery.Property(projection, valueTraversal),
                projection);

        private GremlinQuery<T1, T2, T3, T4> Range(long low, long high, Scope scope) => this
            .Continue()
            .Build(
                static (builder, tuple) => builder
                    .AddStep(Scope.Local.Equals(tuple.scope) && tuple.high - tuple.low == 1 && builder.OuterQuery.Environment.Options.GetValue(GremlinqOption.WorkaroundRangeInconsistencies)
                        ? new MapStep(Traversal.Empty.Push(
                            UnfoldStep.Instance,
                            new RangeStep(tuple.low, tuple.high, Scope.Global),
                            FoldStep.Instance))
                        : new RangeStep(tuple.low, tuple.high, tuple.scope))
                    .Build(),
                (low, high, scope));

        private GremlinQuery<T1, T2, T3, T4> RangeGlobal(long low, long high) => Range(low, high, Scope.Global);

        private GremlinQuery<T1, T2, T3, T4> RangeLocal(long low, long high) => Range(low, high, Scope.Local);

        private IGremlinQuery<TSelectedElement> Select<TSelectedElement>(StepLabel<TSelectedElement> stepLabel) => Select<IGremlinQuery<TSelectedElement>>(stepLabel);

        private TNewQuery Select<TNewQuery>(StepLabel stepLabel) where TNewQuery : IGremlinQueryBase => this
            .Continue()
            .Build(
                static (builder, tuple) => builder
                    .AddStep(new SelectStepLabelStep(ImmutableArray.Create<StepLabel>(tuple.stepLabel)))
                    .WithNewProjection(tuple.stepLabelProjection)
                    .Build<TNewQuery>(),
                (stepLabel, stepLabelProjection: GetLabelProjection(stepLabel)));

        private TTargetQuery Select<TTargetQuery>(params Expression[] projections) where TTargetQuery : IGremlinQueryBase => this
            .Continue()
            .Build(
                static (builder, projections) =>
                {
                    var keys = projections
                        .Select(static expression =>
                        {
                            if (expression is LambdaExpression { Body: MethodCallExpression { Arguments: [{ } indexerArgumentExpression] } methodCallExpression } && methodCallExpression.TryGetWellKnownMember() == WellKnownMember.IndexerGet)
                            {
                                if (indexerArgumentExpression.GetValue() is string indexerArgument)
                                    return indexerArgument;
                            }

                            return (Key)expression.AssumePropertyOrFieldMemberExpression().Member.Name;
                        })
                        .ToImmutableArray();

                    return builder
                        .AddStep(new SelectKeysStep(keys))
                        .WithNewProjection(
                            static (projection, keys) => projection.If<TupleProjection>(tuple => tuple.Select(keys)),
                            keys)
                        .Build<TTargetQuery>();
                },
                projections);

        private GremlinQuery<T1, T2, T3, T4> SideEffect(Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase> sideEffectContinuation) => this
            .Continue()
            .With(sideEffectContinuation)
            .Build(static (builder, traversal) => builder
                .AddStep(new SideEffectStep(traversal))
                .Build());

        private GremlinQuery<T1, T2, T3, T4> SimplePath() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(SimplePathStep.Instance)
                .Build());

        private GremlinQuery<T1, T2, T3, T4> Skip(long count, Scope scope) => this
            .Continue()
            .Build(
                static (builder, tuple) => builder
                    .AddStep(new SkipStep(tuple.count, tuple.scope))
                    .Build(),
                (count, scope));

        private GremlinQuery<T1, T2, T3, T4> SumGlobal() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(new SumStep(Scope.Global))
                .WithNewProjection(Projection.Value)
                .Build());

        private TNewQuery SumLocal<TNewQuery>() where TNewQuery : IGremlinQueryBase => this
            .Continue()
            .Build(static builder => builder
                .AddStep(new SumStep(Scope.Local))
                .WithNewProjection(Projection.Value)
                .Build<TNewQuery>());

        private GremlinQuery<T1, T2, T3, T4> TailGlobal(long count) => this
            .Continue()
            .Build(
                static (builder, count) => builder
                    .AddStep(new TailStep(count, Scope.Global))
                    .Build(),
                count);

        private GremlinQuery<T1, T2, T3, T4> TailLocal(long count) => this
            .Continue()
            .Build(
                static (builder, count) => builder
                    .AddStep(count == 1
                        ? builder.OuterQuery.Environment.Options.GetValue(GremlinqOption.WorkaroundRangeInconsistencies)
                            ? TailStep.TailLocal1Workaround
                            : TailStep.TailLocal1
                        : new TailStep(count, Scope.Local))
                    .Build(),
                count);

        private GremlinQuery<T1, TOutVertex, TNewInVertex, IGremlinQueryBase> To<TOutVertex, TNewInVertex>(Func<GremlinQuery<TOutVertex, T2, T3, T4>, IVertexGremlinQueryBase<TNewInVertex>> toVertexContinuation) => this
            .Continue<TOutVertex, T2, T3, T4>()
            .With(toVertexContinuation)
            .Build(static (builder, toVertexTraversal) => builder
                .AddStep(new AddEStep.ToTraversalStep(toVertexTraversal))
                .AutoBuild<T1, TOutVertex, TNewInVertex>());

        private GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, IGremlinQueryBase> To<TNewElement, TNewOutVertex, TNewInVertex>(StepLabel stepLabel) => this
            .Continue()
            .Build(
                static (builder, stepLabel) => builder
                    .AddStep(new AddEStep.ToLabelStep(stepLabel))
                    .AutoBuild<TNewElement, TNewOutVertex, TNewInVertex>(),
                stepLabel);

        private GremlinQuery<T1, T2, T3, T4> Unfold() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(UnfoldStep.Instance)
                .WithNewProjection(static projection => projection.If<ArrayProjection>(static array => array.Unfold()))
                .Build());

        private TTargetQuery Unfold<TTargetQuery>() => Unfold().CloneAs<TTargetQuery>();

        private GremlinQuery<T1, T2, T3, T4> Union(params Func<GremlinQuery<T1, T2, T3, T4>, GremlinQuery<T1, T2, T3, T4>>[] unionTraversals)
        {
            return Union<GremlinQuery<T1, T2, T3, T4>>(unionTraversals);
        }

        private TTargetQuery Union<TTargetQuery>(params Func<GremlinQuery<T1, T2, T3, T4>, TTargetQuery>[] unionTraversals)
            where TTargetQuery : IGremlinQueryBase
        {
            return Union<TTargetQuery, TTargetQuery>(unionTraversals);
        }

        private TReturnQuery Union<TTargetQuery, TReturnQuery>(params Func<GremlinQuery<T1, T2, T3, T4>, TTargetQuery>[] unionContinuations)
            where TTargetQuery : IGremlinQueryBase
            where TReturnQuery : IGremlinQueryBase => this
                .Continue()
                .With(unionContinuations)
                .Build(static (builder, unionTraversals) => builder
                    .AddStep(new UnionStep(unionTraversals
                        .ToImmutableArray()))
                    .WithNewProjection(unionTraversals
                        .LowestProjection())
                    .Build<TReturnQuery>());

        private GremlinQuery<object, object, object, IGremlinQueryBase> V(ImmutableArray<object> ids) => this
            .Continue()
            .Build(
                static (builder, ids) => builder
                    .AddStep(new VStep(ids))
                    .WithNewProjection(Projection.Vertex)
                    .AutoBuild(),
                ids);

        private GremlinQuery<TNewPropertyValue, object, object, IGremlinQueryBase> Value<TNewPropertyValue>() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(ValueStep.Instance)
                .WithNewProjection(Projection.Value)
                .AutoBuild<TNewPropertyValue>());

        private GremlinQuery<TNewElement, object, object, IGremlinQueryBase> ValueMap<TNewElement>(ImmutableArray<string> keys) => this
            .Continue()
            .Build(
                static (builder, keys) => builder
                    .AddStep(new ValueMapStep(keys))
                    .WithNewProjection(Projection.Value)
                    .AutoBuild<TNewElement>(),
                keys);

        private GremlinQuery<TNewElement, object, object, IGremlinQueryBase> ValueMap<TNewElement>(IEnumerable<LambdaExpression> projections)
        {
            var projectionsArray = projections
                .ToArray<Expression>();

            var stringKeys = GetStringKeys(projectionsArray)
                .ToImmutableArray();

            return stringKeys.Length != projectionsArray.Length
                ? throw new ExpressionNotSupportedException($"One of the expressions in {nameof(ValueMap)} maps to a {nameof(T)}-token. Can't have special tokens in {nameof(ValueMap)}.")
                : this
                    .Continue()
                    .Build(
                        static (builder, stringKeys) => builder
                            .AddStep(new ValueMapStep(stringKeys))
                            .WithNewProjection(Projection.Value)
                            .AutoBuild<TNewElement>(),
                        stringKeys);
        }

        private GremlinQuery<TValue, object, object, IGremlinQueryBase> ValuesForKeys<TValue>(IEnumerable<Key> keys)
        {
            var stepsArray = GetStepsForKeys(keys)
                .ToArray();

            return stepsArray.Length switch
            {
                0 => throw new ExpressionNotSupportedException(),
                1 => this
                    .Continue()
                    .Build(
                        static (builder, step) => builder
                            .AddStep(step)
                            .WithNewProjection(Projection.Value)
                            .AutoBuild<TValue>(),
                        stepsArray[0]),
                _ => this
                    .Union(stepsArray
                        .Select(static step => new Func<GremlinQuery<T1, T2, T3, T4>, GremlinQuery<TValue, object, object, IGremlinQueryBase>>(__ => __
                            .Continue()
                            .Build(
                                static (builder, step) => builder
                                    .AddStep(step)
                                    .WithNewProjection(Projection.Value)
                                    .AutoBuild<TValue>(),
                                step)))
                        .ToArray())
                    .Continue()
                    .Build(static builder => builder
                        .WithNewProjection(Projection.Value)
                        .Build())
            };
        }

        private GremlinQuery<TValue, object, object, IGremlinQueryBase> ValuesForProjections<TValue>(IEnumerable<LambdaExpression> projections) => ValuesForKeys<TValue>(projections.Select(GetKey));

        private GremlinQuery<VertexProperty<TNewPropertyValue, TNewMeta>, TNewPropertyValue, TNewMeta, IGremlinQueryBase> VertexProperties<TNewPropertyValue, TNewMeta>(Expression[] projections) => Properties<VertexProperty<TNewPropertyValue, TNewMeta>, TNewPropertyValue, TNewMeta>(Projection.VertexProperty, projections);

        private GremlinQuery<VertexProperty<TNewPropertyValue>, TNewPropertyValue, object, IGremlinQueryBase> VertexProperties<TNewPropertyValue>(Expression[] projections) => Properties<VertexProperty<TNewPropertyValue>, TNewPropertyValue, object>(Projection.VertexProperty, projections);

        private GremlinQuery<T1, T2, T3, T4> Where(Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase> filterContinuation) => this
            .Continue(ContinuationFlags.Filter)
            .With(filterContinuation)
            .Build(static (builder, filterTraversal) => filterTraversal.IsIdentity()
                ? builder.OuterQuery
                : filterTraversal.IsNone() && filterTraversal.SideEffectSemantics == SideEffectSemantics.Read
                    ? builder.OuterQuery.None()
                    : builder.OuterQuery.Where(filterTraversal));

        private GremlinQuery<T1, T2, T3, T4> Where(Traversal traversal) => this
            .Continue()
            .Build(
                static (builder, traversal) => builder
                    .AddSteps(traversal.Count > 0 && traversal.Steps.All(static x => x is IIsOptimizableInWhere)
                        ? traversal
                        : new FilterStep.ByTraversalStep(traversal))
                    .Build(),
                traversal);

        private GremlinQuery<T1, T2, T3, T4> Where(Expression<Func<T1, bool>> expression) => Where((Expression)expression);

        private GremlinQuery<T1, T2, T3, T4> Where(Expression expression)
        {
            try
            {
                return expression switch
                {
                    ConstantExpression { Value: bool value } => value
                        ? this
                        : None(),

                    LambdaExpression lambdaExpression => Where(lambdaExpression.Body),

                    UnaryExpression { NodeType: ExpressionType.Not } unaryExpression => Not(
                        static (__, unaryExpression) => __.Where(unaryExpression.Operand),
                        unaryExpression),

                    BinaryExpression { NodeType: ExpressionType.OrElse } binary => Or(
                        static (__, state) => __.Where(state.left),
                        static (__, state) => __.Where(state.right),
                        (left: binary.Left, right: binary.Right)),

                    BinaryExpression { NodeType: ExpressionType.AndAlso } binary => And(
                        static (__, state) => __.Where(state.left),
                        static (__, state) => __.Where(state.right),
                        (left: binary.Left, right: binary.Right)),

                    _ when expression.TryToGremlinExpression(Environment) is { } gremlinExpression => gremlinExpression.Equals(GremlinExpression.True)
                        ? this
                        : gremlinExpression.Equals(GremlinExpression.False)
                            ? None()
                            : this
                                .Continue()
                                .Build(
                                    static (builder, gremlinExpression) => builder
                                        .WithSteps(
                                            static (steps, state) =>
                                            {
                                                var (outerQuery, gremlinExpression) = state;

                                                return outerQuery
                                                    .Where(steps, gremlinExpression);
                                            },
                                            (builder.OuterQuery, gremlinExpression))
                                        .Build(),
                                    gremlinExpression),

                    _ => throw new ExpressionNotSupportedException()
                };
            }
            catch (ExpressionNotSupportedException ex)
            {
                throw new ExpressionNotSupportedException(expression, ex);
            }
        }

        private GremlinQuery<T1, T2, T3, T4> Where<TProjection>(Expression<Func<T1, TProjection>> predicate, Func<IGremlinQueryBase<TProjection>, IGremlinQueryBase> propertyContinuation) => predicate.TryGetReferredParameter() is not null && predicate.Body is MemberExpression memberExpression
             ? this
                 .Continue()
                 .With(
                     static (__, propertyContinuation) => propertyContinuation(__.CloneAs<IGremlinQueryBase<TProjection>>()),
                     propertyContinuation)
                 .Build(
                     static (builder, propertyTraversal, key) => builder
                         .AddStep(new HasTraversalStep(key, propertyTraversal))
                         .Build(),
                     GetKey(memberExpression))
             : throw new ExpressionNotSupportedException(predicate);

        private Traversal Where(Traversal traversal, GremlinExpression gremlinExpression) => Where(
            traversal,
            gremlinExpression.Left,
            gremlinExpression.LeftWellKnownMember,
            gremlinExpression.Semantics,
            gremlinExpression.Right);

        private Traversal Where(Traversal traversal, ExpressionFragment left, WellKnownMember? leftWellKnownMember, ExpressionSemantics semantics, ExpressionFragment right)
        {
            if (right.Type == ExpressionFragmentType.Constant)
            {
                var rightValue = right.GetValue();

                var maybeEffectivePredicate = Environment.Options
                    .GetValue(PFactory.PFactoryOption)
                    .TryGetP(semantics, rightValue, Environment)
                    ?.WorkaroundLimitations(Environment);

                if (maybeEffectivePredicate is { } effectivePredicate)
                {
                    if (effectivePredicate.EqualsConstant(false))
                        return traversal.Push(NoneStep.Instance);

                    if (left.Type == ExpressionFragmentType.Parameter)
                    {
                        switch (left.Expression)
                        {
                            case MemberExpression leftMemberExpression:
                                {
                                    var leftMemberExpressionKey = GetKey(leftMemberExpression);
                                    var leftMemberExpressionExpression = leftMemberExpression.Expression?.StripConvert();

                                    if (leftMemberExpressionExpression is ParameterExpression parameterExpression)
                                    {
                                        if (leftWellKnownMember == WellKnownMember.ArrayLength)
                                        {
                                            if (Environment.GetCache().ModelTypes.Contains(parameterExpression.Type))
                                            {
                                                if (leftMemberExpressionKey.RawKey is string stringKey)
                                                {
                                                    if (!Environment.SupportsType(leftMemberExpression.Type))
                                                    {
                                                        return traversal
                                                            .Push(
                                                                new FilterStep.ByTraversalStep(Traversal
                                                                    .Create(
                                                                        3,
                                                                        (stringKey, effectivePredicate),
                                                                        static (steps, state) =>
                                                                        {
                                                                            var (stringKey, effectivePredicate) = state;

                                                                            steps[0] = new PropertiesStep(ImmutableArray.Create(stringKey));
                                                                            steps[1] = CountStep.Global;
                                                                            steps[2] = new IsStep(effectivePredicate);
                                                                        })));
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                return traversal
                                                    .Push(
                                                        new FilterStep.ByTraversalStep(Traversal
                                                            .Create(
                                                                3,
                                                                (leftMemberExpressionKey, effectivePredicate),
                                                                static (steps, state) =>
                                                                {
                                                                    var (leftMemberExpressionKey, effectivePredicate) = state;

                                                                    steps[0] = new SelectKeysStep(ImmutableArray.Create(leftMemberExpressionKey));
                                                                    steps[1] = CountStep.Local;
                                                                    steps[2] = new IsStep(effectivePredicate);
                                                                })));
                                            }

                                            break;
                                        }
                                    }
                                    else if (leftMemberExpressionExpression is MemberExpression leftLeftMemberExpression)
                                    {
                                        // x => x.Name.Value == P.xy(...)
                                        if (leftWellKnownMember == WellKnownMember.PropertyValue)
                                            leftMemberExpression = leftLeftMemberExpression;
                                    }
                                    else
                                        break;

                                    // x => x.Name == P.xy(...)
                                    if (rightValue is StepLabel)
                                    {
                                        if (right.Expression is MemberExpression memberExpression)
                                        {
                                            traversal = traversal
                                                .Push(new WherePredicateStep(effectivePredicate))
                                                .Push(new WherePredicateStep.ByMemberStep(leftMemberExpressionKey));

                                            if (memberExpression.Member != leftMemberExpression.Member)
                                                traversal = traversal.Push(new WherePredicateStep.ByMemberStep(GetKey(memberExpression)));

                                            return traversal;
                                        }

                                        return traversal
                                            .Push(
                                                new HasTraversalStep(
                                                    leftMemberExpressionKey,
                                                    new WherePredicateStep(effectivePredicate)));
                                    }

                                    return traversal
                                        .Push(
                                            effectivePredicate
                                                .GetFilterStep(leftMemberExpressionKey));
                                }
                            case ParameterExpression parameterExpression:
                                {
                                    switch (leftWellKnownMember)
                                    {
                                        // x => x.Value == P.xy(...)
                                        case WellKnownMember.PropertyValue when rightValue is not null and not StepLabel:
                                            {
                                                return traversal.Push(new HasValueStep(effectivePredicate));
                                            }
                                        case WellKnownMember.PropertyKey:
                                            {
                                                return traversal
                                                    .Push(
                                                        new FilterStep.ByTraversalStep(this
                                                            .Where(
                                                                KeyStep.Instance,
                                                                ExpressionFragment.Create(parameterExpression, Environment),
                                                                default,
                                                                semantics,
                                                                right)));
                                            }
                                        case WellKnownMember.VertexPropertyLabel when rightValue is StepLabel:
                                            {
                                                return traversal
                                                    .Push(
                                                        new FilterStep.ByTraversalStep(this
                                                            .Where(
                                                                LabelStep.Instance,
                                                                ExpressionFragment.Create(parameterExpression, Environment),
                                                                default,
                                                                semantics,
                                                                right)));
                                            }
                                        case WellKnownMember.VertexPropertyLabel:
                                            {
                                                return traversal.Push(new HasKeyStep(effectivePredicate));
                                            }
                                    }

                                    // x => x == P.xy(...)
                                    if (rightValue is StepLabel)
                                    {
                                        traversal = traversal.Push(new WherePredicateStep(effectivePredicate));

                                        if (right.Expression is MemberExpression memberExpression)
                                            traversal = traversal.Push(new WherePredicateStep.ByMemberStep(GetKey(memberExpression)));
                                    }
                                    else if (!effectivePredicate.EqualsConstant(true))
                                        traversal = traversal.Push(new IsStep(effectivePredicate));

                                    return traversal;
                                }
                            case MethodCallExpression methodCallExpression:
                                {
                                    var targetExpression = methodCallExpression.Object?.StripConvert();

                                    if (targetExpression != null && typeof(IDictionary<string, object>).IsAssignableFrom(targetExpression.Type) && methodCallExpression.Method.Name == "get_Item")
                                    {
                                        if (methodCallExpression.Arguments[0].StripConvert().GetValue() is string key)
                                            return traversal.Push(new HasPredicateStep(key, effectivePredicate));
                                    }

                                    break;
                                }
                        }
                    }
                    else if (left.Type == ExpressionFragmentType.Constant && left.GetValue() is StepLabel leftStepLabel && rightValue is StepLabel)
                    {
                        traversal = traversal.Push(new WhereStepLabelAndPredicateStep(leftStepLabel, effectivePredicate));

                        if (left.Expression is MemberExpression leftStepValueExpression)
                            traversal = traversal.Push(new WherePredicateStep.ByMemberStep(GetKey(leftStepValueExpression)));

                        if (right.Expression is MemberExpression rightStepValueExpression)
                            traversal = traversal.Push(new WherePredicateStep.ByMemberStep(GetKey(rightStepValueExpression)));

                        return traversal;
                    }
                }
            }
            else if (right.Type == ExpressionFragmentType.Parameter)
            {
                if (left.Type == ExpressionFragmentType.Parameter)
                {
                    if (left.Expression is MemberExpression && right.Expression is MemberExpression rightMember)
                    {
                        var newStepLabel = new StepLabel<T1>();

                        return Where(
                            traversal
                                .Push(new AsStep(newStepLabel)),
                            left,
                            default,
                            semantics,
                            ExpressionFragment.StepLabel(newStepLabel, rightMember));
                    }
                }
            }

            throw new ExpressionNotSupportedException();
        }

        private TQuery WithSideEffect<TSideEffect, TQuery>(TSideEffect value, Func<IGremlinQuerySource, StepLabel<TSideEffect>, TQuery> continuation)
        {
            var stepLabel = new StepLabel<TSideEffect>();

            return continuation(
                ((IGremlinQuerySource)this).WithSideEffect(stepLabel, value),
                stepLabel);
        }

        private GremlinQuery<object, object, object, IGremlinQueryBase> WithSideEffect<TSideEffect>(StepLabel<TSideEffect> label, TSideEffect value) => this
            .Continue()
            .Build(
                static (builder, tuple) => builder
                    .AddStep(new WithSideEffectStep(tuple.label, tuple.value!))
                    .WithNewLabelProjections(
                        static (projections, tuple) => projections.Set(
                            tuple.label,
                            tuple.projection,
                            static (projections, projection) => projections.WithSideEffectLabelProjection(projection)),
                        (tuple.label, projection: builder.OuterQuery.Steps.Projection))
                    .AutoBuild(),
                (label, value));

        private GremlinQuery<T1, T2, T3, T4> WithPartitionStrategy(string partitionKey) => this
            .Continue()
                .Build(
            static (builder, partitionKey) => builder
                    .AddStep(new PartitionStrategyStep(partitionKey))
                    .Build(),
                partitionKey);
    }
}
