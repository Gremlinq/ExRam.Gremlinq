// ReSharper disable ArrangeThisQualifier
// ReSharper disable CoVariantArrayConversion

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Core
{
    partial class GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> :
        IGremlinQueryAdmin,

        IGremlinQuerySource,
        IGremlinQuery<TElement>,

        IArrayGremlinQuery<TElement, TFoldedQuery>,

        IElementGremlinQuery<TElement>,

        IValueGremlinQuery<TElement>,
        IEdgeOrVertexGremlinQuery<TElement>,
        IVertexGremlinQuery<TElement>,
        IEdgeGremlinQuery<TElement>,
        IInOrOutEdgeGremlinQuery<TElement, TOutVertex>,
        IBothEdgeGremlinQuery<TElement, TOutVertex, TInVertex>,

        IInEdgeGremlinQuery<TElement, TInVertex>,
        IOutEdgeGremlinQuery<TElement, TOutVertex>,

        IVertexPropertyGremlinQuery<TElement, TScalar>,
        IVertexPropertyGremlinQuery<TElement, TScalar, TMeta>,
        IPropertyGremlinQuery<TElement> where TMeta : class
    {
        TFoldedQuery IArrayGremlinQueryBase<TElement, TFoldedQuery>.MeanLocal() => MeanLocal().ChangeQueryType<TFoldedQuery>();

        TFoldedQuery IArrayGremlinQueryBase<TElement, TFoldedQuery>.Unfold() => Unfold<TFoldedQuery>();

        IValueGremlinQuery<object[]> IArrayGremlinQueryBase.Lower() => Cast<object[]>();

        IValueGremlinQuery<TElement> IArrayGremlinQueryBase<TElement, TFoldedQuery>.Lower() => this;

        TTargetQuery IValueGremlinQueryBase<TElement>.Choose<TTargetQuery>(Expression<Func<TElement, bool>> predicate, Func<IValueGremlinQuery<TElement>, TTargetQuery> trueChoice, Func<IValueGremlinQuery<TElement>, TTargetQuery> falseChoice) => Choose(predicate, trueChoice, falseChoice);

        TTargetQuery IValueGremlinQueryBase<TElement>.Choose<TTargetQuery>(Expression<Func<TElement, bool>> predicate, Func<IValueGremlinQuery<TElement>, TTargetQuery> trueChoice) => Choose(predicate, trueChoice);

        TFoldedQuery IArrayGremlinQueryBase<TElement, TFoldedQuery>.SumLocal() => SumLocal().ChangeQueryType<TFoldedQuery>();

        TFoldedQuery IArrayGremlinQueryBase<TElement, TFoldedQuery>.MinLocal() => MinLocal().ChangeQueryType<TFoldedQuery>();

        TFoldedQuery IArrayGremlinQueryBase<TElement, TFoldedQuery>.MaxLocal() => MaxLocal().ChangeQueryType<TFoldedQuery>();

        IValueGremlinQuery<TElement> IValueGremlinQueryBase<TElement>.Sum() => SumGlobal();

        IValueGremlinQuery<object> IValueGremlinQueryBase<TElement>.SumLocal() => SumLocal();

        IValueGremlinQuery<TElement> IValueGremlinQueryBase<TElement>.Min() => MinGlobal();

        IValueGremlinQuery<object> IValueGremlinQueryBase<TElement>.MinLocal() => MinLocal();

        IValueGremlinQuery<TElement> IValueGremlinQueryBase<TElement>.Max() => MaxGlobal();

        IValueGremlinQuery<object> IValueGremlinQueryBase<TElement>.MaxLocal() => MaxLocal();

        IValueGremlinQuery<TElement> IValueGremlinQueryBase<TElement>.Mean() => MeanGlobal();

        IValueGremlinQuery<object> IValueGremlinQueryBase<TElement>.MeanLocal() => MeanLocal();

        IValueGremlinQuery<TElement> IValueGremlinQueryBase<TElement>.Where(Expression<Func<TElement, bool>> predicate) => Where(predicate);

        IArrayGremlinQuery<TElement, TFoldedQuery> IValueGremlinQueryBaseRec<TElement, IArrayGremlinQuery<TElement, TFoldedQuery>>.Order(Func<IOrderBuilder<IArrayGremlinQuery<TElement, TFoldedQuery>>, IOrderBuilderWithBy<IArrayGremlinQuery<TElement, TFoldedQuery>>> projection) => OrderGlobal(projection);

        IArrayGremlinQuery<TElement, TFoldedQuery> IValueGremlinQueryBaseRec<TElement, IArrayGremlinQuery<TElement, TFoldedQuery>>.OrderLocal(Func<IOrderBuilder<IArrayGremlinQuery<TElement, TFoldedQuery>>, IOrderBuilderWithBy<IArrayGremlinQuery<TElement, TFoldedQuery>>> projection) => OrderLocal(projection);

        IBothEdgeGremlinQuery<TElement, TNewOutVertex, TInVertex> IInEdgeGremlinQueryBase<TElement, TInVertex>.From<TNewOutVertex>(Func<IVertexGremlinQuery<TInVertex>, IGremlinQuery<TNewOutVertex>> fromVertexTraversal) => AddStep<TElement, TNewOutVertex, TInVertex, object, object, object>(new AddEStep.FromTraversalStep(Cast<TInVertex>().Continue(fromVertexTraversal).ToTraversal()), QuerySemantics.Edge);

        IVertexGremlinQuery<TInVertex> IInEdgeGremlinQueryBase<TElement, TInVertex>.InV() => InV<TInVertex>();

        IEdgeGremlinQuery<TElement> IInEdgeGremlinQueryBase<TElement, TInVertex>.Lower() => this;

        IEdgeGremlinQuery<TElement> IOutEdgeGremlinQueryBase<TElement, TOutVertex>.Lower() => this;

        IVertexGremlinQuery<TOutVertex> IOutEdgeGremlinQueryBase<TElement, TOutVertex>.OutV() => AddStepWithObjectTypes<TOutVertex>(OutVStep.Instance, QuerySemantics.Vertex);

        IBothEdgeGremlinQuery<TElement, TOutVertex, TNewInVertex> IOutEdgeGremlinQueryBase<TElement, TOutVertex>.To<TNewInVertex>(Func<IVertexGremlinQuery<TOutVertex>, IGremlinQuery<TNewInVertex>> toVertexTraversal) => AddStep<TElement, TOutVertex, TNewInVertex, object, object, object>(new AddEStep.ToTraversalStep(Cast<TOutVertex>().Continue(toVertexTraversal).ToTraversal()), QuerySemantics.Edge);

        IVertexGremlinQuery<object> IEdgeGremlinQueryBase.BothV() => BothV<object>();

        IVertexGremlinQuery<TVertex> IEdgeGremlinQueryBase.BothV<TVertex>() => BothV<object>().OfType<TVertex>(Environment.Model.VerticesModel);

        IOutEdgeGremlinQuery<TElement, TNewOutVertex> IEdgeGremlinQueryBase<TElement>.From<TNewOutVertex>(StepLabel<TNewOutVertex> stepLabel) => AddStep<TElement, TNewOutVertex, object, object, object, object>(new AddEStep.FromLabelStep(stepLabel), QuerySemantics.Edge);

        IEdgeOrVertexGremlinQuery<TElement> IEdgeGremlinQueryBase<TElement>.Lower() => this;

        IOutEdgeGremlinQuery<TElement, TNewOutVertex> IEdgeGremlinQueryBase<TElement>.From<TNewOutVertex>(Func<IVertexGremlinQueryBase, IVertexGremlinQuery<TNewOutVertex>> fromVertexTraversal) => AddStep<TElement, TNewOutVertex, object, object, object, object>(new AddEStep.FromTraversalStep(Continue(fromVertexTraversal).ToTraversal()), QuerySemantics.Edge);

        IVertexGremlinQuery<object> IEdgeGremlinQueryBase.InV() => InV<object>();

        IVertexGremlinQuery<TVertex> IEdgeGremlinQueryBase.InV<TVertex>() => InV<object>().OfType<TVertex>(Environment.Model.VerticesModel);

        IEdgeGremlinQuery<object> IInEdgeGremlinQueryBase.Lower() => Cast<object>();

        IEdgeGremlinQuery<object> IOutEdgeGremlinQueryBase.Lower() => Cast<object>();

        IEdgeOrVertexGremlinQuery<object> IEdgeGremlinQueryBase.Lower() => Cast<object>();

        IVertexGremlinQuery<object> IEdgeGremlinQueryBase.OtherV() => OtherV<object>();

        IVertexGremlinQuery<TVertex> IEdgeGremlinQueryBase.OtherV<TVertex>() => OtherV<object>().OfType<TVertex>(Environment.Model.VerticesModel);

        IVertexGremlinQuery<object> IEdgeGremlinQueryBase.OutV() => OutV<object>();

        IVertexGremlinQuery<TVertex> IEdgeGremlinQueryBase.OutV<TVertex>() => OutV<object>().OfType<TVertex>(Environment.Model.VerticesModel);

        IPropertyGremlinQuery<Property<TValue>> IEdgeGremlinQueryBase<TElement>.Properties<TValue>(params Expression<Func<TElement, TValue>>[] projections) => Properties<Property<TValue>, TValue, object>(QuerySemantics.Property, projections);

        IPropertyGremlinQuery<Property<TValue>> IEdgeGremlinQueryBase<TElement>.Properties<TValue>(params Expression<Func<TElement, Property<TValue>>>[] projections) => Properties<Property<TValue>, TValue, object>(QuerySemantics.Property, projections);

        IPropertyGremlinQuery<Property<object>> IEdgeGremlinQueryBase<TElement>.Properties(params Expression<Func<TElement, Property<object>>>[] projections) => Properties<Property<object>, object, object>(QuerySemantics.Property, projections);

        IPropertyGremlinQuery<Property<object>> IEdgeGremlinQueryBase.Properties() => Properties<Property<object>, object, object>(Array.Empty<string>(), QuerySemantics.Property);

        IPropertyGremlinQuery<Property<TValue>> IEdgeGremlinQueryBase.Properties<TValue>() => Properties<Property<TValue>, object, object>(Array.Empty<string>(), QuerySemantics.Property);

        IInEdgeGremlinQuery<TElement, TNewInVertex> IEdgeGremlinQueryBase<TElement>.To<TNewInVertex>(StepLabel<TNewInVertex> stepLabel) => To<TElement, object, TNewInVertex>(stepLabel);

        IInEdgeGremlinQuery<TElement, TNewInVertex> IEdgeGremlinQueryBase<TElement>.To<TNewInVertex>(Func<IVertexGremlinQueryBase, IVertexGremlinQuery<TNewInVertex>> toVertexTraversal) => AddStep<TElement, object, TNewInVertex, object, object, object>(new AddEStep.ToTraversalStep(Continue(toVertexTraversal).ToTraversal()), QuerySemantics.Edge);

        IValueGremlinQuery<TValue> IEdgeGremlinQueryBase<TElement>.Values<TValue>(params Expression<Func<TElement, Property<TValue>>>[] projections) => ValuesForProjections<TValue>(projections);

        IValueGremlinQuery<object> IEdgeGremlinQueryBase<TElement>.Values(params Expression<Func<TElement, Property<object>>>[] projections) => ValuesForProjections<object>(projections);

        IEdgeGremlinQuery<TElement> IEdgeGremlinQueryBase<TElement>.Update(TElement element) => AddOrUpdate(element, false, false);

        IValueGremlinQuery<TTarget> IEdgeGremlinQueryBase<TElement>.Values<TTarget>(params Expression<Func<TElement, TTarget>>[] projections) => ValuesForProjections<TTarget>(projections);

        IValueGremlinQuery<object> IElementGremlinQueryBase.Id() => Id();

        IValueGremlinQuery<string> IElementGremlinQueryBase.Label() => Label();

        IValueGremlinQuery<IDictionary<string, TTarget>> IElementGremlinQueryBase.ValueMap<TTarget>() => ValueMap<IDictionary<string, TTarget>>(ImmutableArray<string>.Empty);

        IValueGremlinQuery<IDictionary<string, object>> IElementGremlinQueryBase.ValueMap() => ValueMap<IDictionary<string, object>>(ImmutableArray<string>.Empty);

        IValueGremlinQuery<TValue> IElementGremlinQueryBase.Values<TValue>() => ValuesForKeys<TValue>(Array.Empty<string>());

        IValueGremlinQuery<object> IElementGremlinQueryBase.Values() => ValuesForKeys<object>(Array.Empty<string>());

        IElementGremlinQuery<TElement> IElementGremlinQueryBase<TElement>.Update(TElement element) => AddOrUpdate(element, false, false);

        IValueGremlinQuery<TTarget> IElementGremlinQueryBase<TElement>.Values<TTarget>(params Expression<Func<TElement, TTarget>>[] projections) => ValuesForProjections<TTarget>(projections);

        IValueGremlinQuery<TTarget> IElementGremlinQueryBase<TElement>.Values<TTarget>(params Expression<Func<TElement, TTarget[]>>[] projections) => ValuesForProjections<TTarget>(projections);

        IValueGremlinQuery<IDictionary<string, TTarget>> IElementGremlinQueryBase<TElement>.ValueMap<TTarget>(params Expression<Func<TElement, TTarget>>[] keys) => ValueMap<IDictionary<string, TTarget>>(keys);

        IEdgeGremlinQuery<TEdge> IStartGremlinQuery.AddE<TEdge>(TEdge edge) => AddE(edge);

        IVertexGremlinQuery<TVertex> IStartGremlinQuery.AddV<TVertex>(TVertex vertex) => AddV(vertex);

        IGremlinQueryAdmin IGremlinQueryBase.AsAdmin() => this;

        IValueGremlinQuery<TValue> IGremlinQueryBase.Constant<TValue>(TValue constant) => AddStepWithObjectTypes<TValue>(new ConstantStep(constant), QuerySemantics.None);

        IValueGremlinQuery<long> IGremlinQueryBase.Count() => AddStepWithObjectTypes<long>(CountStep.Global, QuerySemantics.None);

        IValueGremlinQuery<long> IGremlinQueryBase.CountLocal() => AddStepWithObjectTypes<long>(CountStep.Local, QuerySemantics.None);

        IValueGremlinQuery<string> IGremlinQueryBase.Explain() => AddStepWithObjectTypes<string>(ExplainStep.Instance, QuerySemantics.None);

        IElementGremlinQuery<TElement> IVertexPropertyGremlinQueryBase<TElement, TScalar>.Lower() => this;

        IElementGremlinQuery<object> IVertexPropertyGremlinQueryBase.Lower() => Cast<object>();

        IEdgeOrVertexGremlinQuery<TElement> IVertexGremlinQueryBase<TElement>.Lower() => this;

        IEdgeOrVertexGremlinQuery<object> IVertexGremlinQueryBase.Lower() => Cast<object>();

        IElementGremlinQuery<TElement> IEdgeOrVertexGremlinQueryBase<TElement>.Lower() => this;

        IElementGremlinQuery<object> IEdgeOrVertexGremlinQueryBase.Lower() => Cast<object>();

        TaskAwaiter IGremlinQueryBase.GetAwaiter() => ((Task)((IGremlinQuery<TElement>)this).ToAsyncEnumerable().LastOrDefaultAsync().AsTask()).GetAwaiter();

        GremlinQueryAwaiter<TElement> IGremlinQueryBase<TElement>.GetAwaiter() => new GremlinQueryAwaiter<TElement>((this).ToArrayAsync().AsTask().GetAwaiter());

        IGremlinQuery<TNewElement> IStartGremlinQuery.Inject<TNewElement>(params TNewElement[] elements) => Inject(elements);

        IAsyncEnumerable<TElement> IGremlinQueryBase<TElement>.ToAsyncEnumerable() => Environment.Execute(this);

        IValueGremlinQuery<string> IGremlinQueryBase.Profile() => AddStepWithObjectTypes<string>(ProfileStep.Instance, QuerySemantics.None);

        TQuery IGremlinQueryBase.Select<TQuery, TStepElement>(StepLabel<TQuery, TStepElement> label)
        {
            if (!Steps.IsEmpty && Steps.Peek() is AsStep asStep && ReferenceEquals(asStep.StepLabel, label))
                return ChangeQueryType<TQuery>();

            return this
                .Select(label)
                .ChangeQueryType<TQuery>();
        }

        IArrayGremlinQuery<TNewElement, TQuery> IGremlinQueryBase.Cap<TQuery, TNewElement>(StepLabel<IArrayGremlinQuery<TNewElement, TQuery>, TNewElement> label) => Cap(label);

        IValueGremlinQuery<TLabelledElement> IGremlinQueryBase.Select<TLabelledElement>(StepLabel<TLabelledElement> label) => Select(label);

        IVertexGremlinQuery<object> IStartGremlinQuery.V(params object[] ids) => AddStepWithObjectTypes<object>(new VStep(ids), QuerySemantics.Vertex);

        IVertexGremlinQuery<TNewVertex> IStartGremlinQuery.ReplaceV<TNewVertex>(TNewVertex vertex)
        {
            return ((IStartGremlinQuery)this)
                .V<TNewVertex>(vertex.GetId(Environment.Model.PropertiesModel))
                .Update(vertex);
        }

        IGremlinQuery<TElement> IGremlinQueryBase<TElement>.Lower() => this;

        IGremlinQuery<object> IGremlinQueryBase.Lower() => Cast<object>();

        IValueGremlinQuery<object> IGremlinQueryBase.Drop() => Drop();

        IEdgeGremlinQuery<TEdge> IStartGremlinQuery.AddE<TEdge>() => AddE(new TEdge());

        IVertexGremlinQuery<TVertex> IStartGremlinQuery.AddV<TVertex>() => AddV(new TVertex());

        IVertexGremlinQuery<TVertex> IStartGremlinQuery.V<TVertex>(params object[] ids) => ((IStartGremlinQuery)this).V(ids).OfType<TVertex>();

        IGremlinQuery<object> IGremlinQueryAdmin.ConfigureSteps(Func<IImmutableStack<Step>, IImmutableStack<Step>> configurator) => ConfigureSteps<object>(configurator);

        TTargetQuery IGremlinQueryAdmin.ChangeQueryType<TTargetQuery>() => ChangeQueryType<TTargetQuery>();

        IImmutableStack<Step> IGremlinQueryAdmin.Steps => Steps;

        IGremlinQueryEnvironment IGremlinQuerySource.Environment => Environment;

        IGremlinQueryEnvironment IGremlinQueryAdmin.Environment => Environment;

        Traversal IGremlinQueryAdmin.ToTraversal() => ToTraversalImpl().ToImmutableArray();

        private IEnumerable<Step> ToTraversalImpl()
        {
            var steps = Steps;

            if (steps.IsEmpty)
            {
                yield return IdentityStep.Instance;
            }
            else
            {
                var stepsArray = steps.ToArray();

                for (var i = stepsArray.Length - 1; i >= 0; i--)
                {
                    yield return stepsArray[i];
                }
            }

            if ((Flags & QueryFlags.SurfaceVisible) == QueryFlags.SurfaceVisible)
            {
                switch (Semantics)
                {
                    case QuerySemantics.Vertex:
                    {
                        foreach (var step in Environment.Options.GetValue(GremlinqOption.VertexProjectionSteps))
                        {
                            yield return step;
                        }

                        break;
                    }
                    case QuerySemantics.Edge:
                    {
                        foreach (var step in Environment.Options.GetValue(GremlinqOption.EdgeProjectionSteps))
                        {
                            yield return step;
                        }

                        break;
                    }
                }
            }
        }

        IBothEdgeGremlinQuery<TElement, TTargetVertex, TOutVertex> IInOrOutEdgeGremlinQueryBase<TElement, TOutVertex>.From<TTargetVertex>(StepLabel<TTargetVertex> stepLabel) => AddStep<TElement, TTargetVertex, TOutVertex, object, object, object>(new AddEStep.FromLabelStep(stepLabel), QuerySemantics.Edge);

        IBothEdgeGremlinQuery<TElement, TTargetVertex, TOutVertex> IInOrOutEdgeGremlinQueryBase<TElement, TOutVertex>.From<TTargetVertex>(Func<IVertexGremlinQuery<TOutVertex>, IVertexGremlinQuery<TTargetVertex>> fromVertexTraversal) => AddStep<TElement, TTargetVertex, TOutVertex, object, object, object>(new AddEStep.FromTraversalStep(Cast<TOutVertex>().Continue(fromVertexTraversal).ToTraversal()), QuerySemantics.Edge);

        IBothEdgeGremlinQuery<TElement, TOutVertex, TTargetVertex> IInOrOutEdgeGremlinQueryBase<TElement, TOutVertex>.To<TTargetVertex>(StepLabel<TTargetVertex> stepLabel) => To<TElement, TOutVertex, TTargetVertex>(stepLabel);

        IBothEdgeGremlinQuery<TElement, TOutVertex, TTargetVertex> IInOrOutEdgeGremlinQueryBase<TElement, TOutVertex>.To<TTargetVertex>(Func<IVertexGremlinQuery<TOutVertex>, IVertexGremlinQuery<TTargetVertex>> toVertexTraversal) => AddStep<TElement, TOutVertex, TTargetVertex, object, object, object>(new AddEStep.ToTraversalStep(Cast<TOutVertex>().Continue(toVertexTraversal).ToTraversal()), QuerySemantics.Edge);

        IValueGremlinQuery<string> IPropertyGremlinQueryBase<TElement>.Key() => Key();

        IValueGremlinQuery<TValue> IPropertyGremlinQueryBase<TElement>.Value<TValue>() => Value<TValue>();

        IValueGremlinQuery<object> IPropertyGremlinQueryBase<TElement>.Value() => Value<object>();

        IPropertyGremlinQuery<TElement> IPropertyGremlinQueryBase<TElement>.Where(Expression<Func<TElement, bool>> predicate) => Where(predicate);

        IValueGremlinQuery<TElement> IValueGremlinQueryBaseRec<TElement, IValueGremlinQuery<TElement>>.Order(Func<IOrderBuilder<IValueGremlinQuery<TElement>>, IOrderBuilderWithBy<IValueGremlinQuery<TElement>>> projection) => OrderGlobal(projection);

        IValueGremlinQuery<TElement> IValueGremlinQueryBaseRec<TElement, IValueGremlinQuery<TElement>>.OrderLocal(Func<IOrderBuilder<IValueGremlinQuery<TElement>>, IOrderBuilderWithBy<IValueGremlinQuery<TElement>>> projection) => OrderLocal(projection);

        IInOrOutEdgeGremlinQuery<TEdge, TElement> IVertexGremlinQueryBase<TElement>.AddE<TEdge>(TEdge edge) => AddE(edge);

        IInOrOutEdgeGremlinQuery<TEdge, TElement> IVertexGremlinQueryBase<TElement>.AddE<TEdge>() => AddE(new TEdge());

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.Both() => AddStepWithObjectTypes<object>(BothStep.NoLabels, QuerySemantics.Vertex);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.Both<TEdge>() => AddStepWithObjectTypes<object>(new BothStep(Environment.Model.EdgesModel.GetFilterLabelsOrDefault(typeof(TEdge), Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity))), QuerySemantics.Vertex);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.BothE() => AddStepWithObjectTypes<object>(BothEStep.NoLabels, QuerySemantics.Edge);

        IEdgeGremlinQuery<TEdge> IVertexGremlinQueryBase.BothE<TEdge>() => AddStepWithObjectTypes<TEdge>(new BothEStep(Environment.Model.EdgesModel.GetFilterLabelsOrDefault(typeof(TEdge), Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity))), QuerySemantics.Edge);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.In() => AddStepWithObjectTypes<object>(InStep.NoLabels, QuerySemantics.Vertex);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.In<TEdge>() => AddStepWithObjectTypes<object>(new InStep(Environment.Model.EdgesModel.GetFilterLabelsOrDefault(typeof(TEdge), Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity))), QuerySemantics.Vertex);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.InE() => AddStepWithObjectTypes<object>(InEStep.NoLabels, QuerySemantics.Edge);

        IEdgeGremlinQuery<TEdge> IVertexGremlinQueryBase.InE<TEdge>() => AddStepWithObjectTypes<TEdge>(new InEStep(Environment.Model.EdgesModel.GetFilterLabelsOrDefault(typeof(TEdge), Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity))), QuerySemantics.Edge);

        IInEdgeGremlinQuery<TEdge, TElement> IVertexGremlinQueryBase<TElement>.InE<TEdge>() => AddStep<TEdge, object, TElement, object, object, object>(new InEStep(Environment.Model.EdgesModel.GetFilterLabelsOrDefault(typeof(TEdge), Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity))), QuerySemantics.Edge);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.Out() => AddStepWithObjectTypes<object>(OutStep.NoLabels, QuerySemantics.Vertex);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.Out<TEdge>() => AddStepWithObjectTypes<object>(new OutStep(Environment.Model.EdgesModel.GetFilterLabelsOrDefault(typeof(TEdge), Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity))), QuerySemantics.Vertex);

        IEdgeGremlinQuery<TEdge> IVertexGremlinQueryBase.OutE<TEdge>() => AddStepWithObjectTypes<TEdge>(new OutEStep(Environment.Model.EdgesModel.GetFilterLabelsOrDefault(typeof(TEdge), Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity))), QuerySemantics.Edge);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.OutE() => AddStepWithObjectTypes<object>(OutEStep.NoLabels, QuerySemantics.Edge);

        IOutEdgeGremlinQuery<TEdge, TElement> IVertexGremlinQueryBase<TElement>.OutE<TEdge>() => AddStep<TEdge, TElement, object, object, object, object>(new OutEStep(Environment.Model.EdgesModel.GetFilterLabelsOrDefault(typeof(TEdge), Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity))), QuerySemantics.Edge);

        IVertexPropertyGremlinQuery<VertexProperty<object>, object> IVertexGremlinQueryBase<TElement>.Properties() => Properties<VertexProperty<object>, object, object>(Array.Empty<string>(), QuerySemantics.VertexProperty);

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQueryBase<TElement>.Properties<TValue>(params Expression<Func<TElement, TValue>>[] projections) => VertexProperties<TValue>(projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQueryBase<TElement>.Properties<TValue>(params Expression<Func<TElement, TValue[]>>[] projections) => VertexProperties<TValue>(projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQueryBase<TElement>.Properties<TValue>(params Expression<Func<TElement, VertexProperty<TValue>>>[] projections) => VertexProperties<TValue>(projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue, TNewMeta>, TValue, TNewMeta> IVertexGremlinQueryBase<TElement>.Properties<TValue, TNewMeta>(params Expression<Func<TElement, VertexProperty<TValue, TNewMeta>>>[] projections) => Properties<VertexProperty<TValue, TNewMeta>, TValue, TNewMeta>(QuerySemantics.VertexProperty, projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQueryBase<TElement>.Properties<TValue>(params Expression<Func<TElement, VertexProperty<TValue>[]>>[] projections) => VertexProperties<TValue>(projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue, TNewMeta>, TValue, TNewMeta> IVertexGremlinQueryBase<TElement>.Properties<TValue, TNewMeta>(params Expression<Func<TElement, VertexProperty<TValue, TNewMeta>[]>>[] projections) => Properties<VertexProperty<TValue, TNewMeta>, TValue, TNewMeta>(QuerySemantics.VertexProperty, projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQueryBase<TElement>.Properties<TValue>() => Properties<VertexProperty<TValue>, TValue, object>(Array.Empty<string>(), QuerySemantics.VertexProperty);

        IVertexPropertyGremlinQuery<VertexProperty<object>, object> IVertexGremlinQueryBase<TElement>.Properties(params Expression<Func<TElement, VertexProperty<object>>>[] projections) => VertexProperties<object>(projections);

        IValueGremlinQuery<TValue> IVertexGremlinQueryBase<TElement>.Values<TValue, TNewMeta>(params Expression<Func<TElement, VertexProperty<TValue, TNewMeta>>>[] projections) where TNewMeta : class => ValuesForProjections<TValue>(projections);

        IValueGremlinQuery<TValue> IVertexGremlinQueryBase<TElement>.Values<TValue>(params Expression<Func<TElement, VertexProperty<TValue>>>[] projections) => ValuesForProjections<TValue>(projections);

        IValueGremlinQuery<TTarget> IVertexGremlinQueryBase<TElement>.Values<TTarget>(params Expression<Func<TElement, VertexProperty<TTarget>[]>>[] projections) => ValuesForProjections<TTarget>(projections);

        IValueGremlinQuery<TTarget> IVertexGremlinQueryBase<TElement>.Values<TTarget, TTargetMeta>(params Expression<Func<TElement, VertexProperty<TTarget, TTargetMeta>[]>>[] projections) where TTargetMeta : class => ValuesForProjections<TTarget>(projections);

        IValueGremlinQuery<object> IVertexGremlinQueryBase<TElement>.Values(params Expression<Func<TElement, VertexProperty<object>>>[] projections) => ValuesForProjections<object>(projections);

        IVertexGremlinQuery<TElement> IVertexGremlinQueryBase<TElement>.Update(TElement element) => AddOrUpdate(element, false, true);

        IVertexGremlinQuery<TElement> IVertexGremlinQueryBaseRec<TElement, IVertexGremlinQuery<TElement>>.Property<TProjectedValue>(Expression<Func<TElement, TProjectedValue[]>> projection, TProjectedValue value) => VertexProperty(projection, value);

        IValueGremlinQuery<TTarget> IVertexGremlinQueryBase<TElement>.Values<TTarget>(params Expression<Func<TElement, TTarget>>[] projections) => ValuesForProjections<TTarget>(projections);

        IValueGremlinQuery<TTarget> IVertexGremlinQueryBase<TElement>.Values<TTarget>(params Expression<Func<TElement, TTarget[]>>[] projections) => ValuesForProjections<TTarget>(projections);

        IPropertyGremlinQuery<Property<TValue>> IVertexPropertyGremlinQueryBase<TElement, TScalar, TMeta>.Properties<TValue>(params Expression<Func<TMeta, TValue>>[] projections) => Properties<Property<TValue>, TValue, object>(QuerySemantics.Property, projections);

        IVertexPropertyGremlinQuery<TElement, TScalar, TMeta> IVertexPropertyGremlinQueryBase<TElement, TScalar, TMeta>.Property<TValue>(Expression<Func<TMeta, TValue>> projection, TValue value) => Property(projection, value);

        IValueGremlinQuery<TScalar> IVertexPropertyGremlinQueryBase<TElement, TScalar, TMeta>.Value() => Value<TScalar>();

        IValueGremlinQuery<TMeta> IVertexPropertyGremlinQueryBase<TElement, TScalar, TMeta>.ValueMap() => ValueMap<TMeta>(ImmutableArray<string>.Empty);

        IValueGremlinQuery<TTarget> IVertexPropertyGremlinQueryBase<TElement, TScalar, TMeta>.Values<TTarget>(params Expression<Func<TMeta, TTarget>>[] projections) => ValuesForProjections<TTarget>(projections);

        IVertexPropertyGremlinQuery<TElement, TScalar, TMeta> IVertexPropertyGremlinQueryBase<TElement, TScalar, TMeta>.Where(Expression<Func<VertexProperty<TScalar, TMeta>, bool>> predicate) => Where(predicate);

        IValueGremlinQuery<IDictionary<string, TTarget>> IVertexPropertyGremlinQueryBase.ValueMap<TTarget>() => ValueMap<IDictionary<string, TTarget>>(ImmutableArray<string>.Empty);

        IValueGremlinQuery<IDictionary<string, TTarget>> IVertexPropertyGremlinQueryBase.ValueMap<TTarget>(params string[] keys) => ValueMap<IDictionary<string, TTarget>>(keys.ToImmutableArray());

        IValueGremlinQuery<IDictionary<string, object>> IVertexPropertyGremlinQueryBase.ValueMap(params string[] keys) => ValueMap<IDictionary<string, object>>(keys.ToImmutableArray());

        IValueGremlinQuery<TValue> IVertexPropertyGremlinQueryBase.Values<TValue>() => ValuesForKeys<TValue>(Array.Empty<string>());

        IValueGremlinQuery<TValue> IVertexPropertyGremlinQueryBase.Values<TValue>(params string[] keys) => ValuesForKeys<TValue>(keys);

        IValueGremlinQuery<object> IVertexPropertyGremlinQueryBase.Values(params string[] keys) => ValuesForKeys<object>(keys);

        IPropertyGremlinQuery<Property<object>> IVertexPropertyGremlinQueryBase.Properties(params string[] keys) => Properties<Property<object>, object, object>(keys, QuerySemantics.Property);

        IVertexPropertyGremlinQuery<VertexProperty<TScalar, TNewMeta>, TScalar, TNewMeta> IVertexPropertyGremlinQueryBase<TElement, TScalar>.Meta<TNewMeta>() where TNewMeta : class => Cast<VertexProperty<TScalar, TNewMeta>, object, object, TScalar, TNewMeta, object>();

        IPropertyGremlinQuery<Property<TValue>> IVertexPropertyGremlinQueryBase<TElement, TScalar>.Properties<TValue>(params string[] keys) => Properties<Property<TValue>, object, object>(keys, QuerySemantics.Property);

        IValueGremlinQuery<TScalar> IVertexPropertyGremlinQueryBase<TElement, TScalar>.Value() => Value<TScalar>();

        IEdgeGremlinQuery<object> IGremlinQuerySource.E(params object[] ids) => AddStepWithObjectTypes<object>(new EStep(ids), QuerySemantics.Edge);

        IEdgeGremlinQuery<TEdge> IGremlinQuerySource.E<TEdge>(params object[] ids) => ((IGremlinQuerySource)this).E(ids).OfType<TEdge>();

        IEdgeGremlinQuery<TNewEdge> IGremlinQuerySource.ReplaceE<TNewEdge>(TNewEdge edge) => ((IGremlinQuerySource)this).E<TNewEdge>(edge.GetId(Environment.Model.PropertiesModel)).Update(edge);

        IGremlinQuerySource IConfigurableGremlinQuerySource.ConfigureEnvironment(Func<IGremlinQueryEnvironment, IGremlinQueryEnvironment> environmentTransformation) => new GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>(Steps, environmentTransformation(Environment), Semantics, StepLabelSemantics, Flags);

        IGremlinQuerySource IGremlinQuerySource.RemoveStrategies(params Type[] strategyTypes)
        {
            return (!Steps.IsEmpty && Steps.Peek() is WithoutStrategiesStep withoutStrategies)
                ? ConfigureSteps<TElement>(steps => steps.Pop().Push(new WithoutStrategiesStep(withoutStrategies.StrategyTypes.Concat(strategyTypes).Distinct().ToImmutableArray())))
                : AddStep(new WithoutStrategiesStep(strategyTypes.ToImmutableArray()));
        }
    }
}
