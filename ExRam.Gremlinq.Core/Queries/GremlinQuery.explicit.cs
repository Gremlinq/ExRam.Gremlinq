// ReSharper disable ArrangeThisQualifier
// ReSharper disable CoVariantArrayConversion

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using ExRam.Gremlinq.Core.GraphElements;
using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    public struct GremlinQueryAwaiter : ICriticalNotifyCompletion, INotifyCompletion
    {
        private readonly ValueTaskAwaiter<Unit> _valueTaskAwaiter;

        internal GremlinQueryAwaiter(ValueTaskAwaiter<Unit> valueTaskAwaiter)
        {
            _valueTaskAwaiter = valueTaskAwaiter;
        }

        public void GetResult()
        {
            _valueTaskAwaiter.GetResult();
        }

        public void OnCompleted(Action continuation)
        {
            _valueTaskAwaiter.OnCompleted(continuation);
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            _valueTaskAwaiter.UnsafeOnCompleted(continuation);
        }

        public bool IsCompleted { get => _valueTaskAwaiter.IsCompleted; }
    }

    public struct GremlinQueryAwaiter<TElement> : ICriticalNotifyCompletion, INotifyCompletion
    {
        private readonly ValueTaskAwaiter<TElement[]> _valueTaskAwaiter;

        internal GremlinQueryAwaiter(ValueTaskAwaiter<TElement[]> valueTaskAwaiter)
        {
            _valueTaskAwaiter = valueTaskAwaiter;
        }

        public TElement[] GetResult()
        {
            return _valueTaskAwaiter.GetResult();
        }

        public void OnCompleted(Action continuation)
        {
            _valueTaskAwaiter.OnCompleted(continuation);
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            _valueTaskAwaiter.UnsafeOnCompleted(continuation);
        }

        public bool IsCompleted { get => _valueTaskAwaiter.IsCompleted; }
    }

    partial class GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> :
        IGremlinQueryAdmin,
        IGremlinQuery,
        IElementGremlinQuery,
        IArrayGremlinQuery<TElement, TFoldedQuery>,
        IGremlinQuery<TElement>,
        IElementGremlinQuery<TElement>,
        IValueGremlinQuery<TElement>,
        IVertexGremlinQuery,
        IVertexGremlinQuery<TElement>,
        IEdgeGremlinQuery,
        IEdgeGremlinQuery<TElement>,
        IEdgeGremlinQuery<TElement, TOutVertex>,
        IInEdgeGremlinQuery<TElement, TInVertex>,
        IOutEdgeGremlinQuery<TElement, TOutVertex>,
        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>,
        IVertexPropertyGremlinQuery<TElement, TPropertyValue>,
        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>,
        IPropertyGremlinQuery<TElement>
    {
        IEdgeGremlinQuery<TEdge> IGremlinQueryBase.AddE<TEdge>(TEdge edge) => AddE(edge);

        IEdgeGremlinQuery<TEdge, TElement> IVertexGremlinQuery<TElement>.AddE<TEdge>(TEdge edge) => AddE(edge);

        IEdgeGremlinQuery<TEdge, TElement> IVertexGremlinQuery<TElement>.AddE<TEdge>() => AddE(new TEdge());
        
        IVertexGremlinQuery<TVertex> IGremlinQueryBase.AddV<TVertex>(TVertex vertex) => AddV(vertex);

        IGremlinQueryAdmin IGremlinQuery.AsAdmin() => this;

        IVertexGremlinQuery<IVertex> IVertexGremlinQuery.Both() => AddStepWithUnitTypes<IVertex>(BothStep.NoLabels, QuerySemantics.Vertex);

        IVertexGremlinQuery<IVertex> IVertexGremlinQuery.Both<TEdge>() => AddStepWithUnitTypes<IVertex>(Environment.Model.VerticesModel.GetFilterStepOrNone(typeof(TEdge), Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity), labels => new BothStep(labels)), QuerySemantics.Vertex);

        IEdgeGremlinQuery<IEdge> IVertexGremlinQuery.BothE() => AddStepWithUnitTypes<IEdge>(BothEStep.NoLabels, QuerySemantics.Edge);

        IEdgeGremlinQuery<TEdge> IVertexGremlinQuery.BothE<TEdge>() => AddStepWithUnitTypes<TEdge>(Environment.Model.EdgesModel.GetFilterStepOrNone(typeof(TEdge), Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity), labels => new BothEStep(labels)), QuerySemantics.Edge);

        IVertexGremlinQuery<IVertex> IEdgeGremlinQuery.BothV() => BothV<IVertex>();

        IVertexGremlinQuery<TVertex> IEdgeGremlinQuery.BothV<TVertex>() => BothV<Unit>().OfType<TVertex>(Environment.Model.VerticesModel);

        IGremlinQuery<object> IGremlinQueryAdmin.ConfigureSteps(Func<IImmutableList<Step>, IImmutableList<Step>> configurator) => ConfigureSteps<object>(configurator);

        TTargetQuery IGremlinQueryAdmin.ChangeQueryType<TTargetQuery>() => ChangeQueryType<TTargetQuery>();

        TTargetQuery IValueGremlinQuery<TElement>.Choose<TTargetQuery>(Expression<Func<TElement, bool>> predicate, Func<IValueGremlinQuery<TElement>, TTargetQuery> trueChoice, Func<IValueGremlinQuery<TElement>, TTargetQuery> falseChoice) => Choose(predicate, trueChoice, falseChoice);

        TTargetQuery IValueGremlinQuery<TElement>.Choose<TTargetQuery>(Expression<Func<TElement, bool>> predicate, Func<IValueGremlinQuery<TElement>, TTargetQuery> trueChoice) => Choose(predicate, trueChoice);

        IValueGremlinQuery<TValue> IGremlinQuery.Constant<TValue>(TValue constant) => AddStepWithUnitTypes<TValue>(new ConstantStep(constant), QuerySemantics.None);

        IValueGremlinQuery<long> IGremlinQuery.Count() => AddStepWithUnitTypes<long>(CountStep.Global, QuerySemantics.None);

        IValueGremlinQuery<long> IGremlinQuery.CountLocal() => AddStepWithUnitTypes<long>(CountStep.Local, QuerySemantics.None);

        IEdgeGremlinQuery<IEdge> IGremlinQueryBase.E(params object[] ids) => AddStepWithUnitTypes<IEdge>(new EStep(ids), QuerySemantics.Edge);

        IGremlinQuery<string> IGremlinQuery.Explain() => AddStepWithUnitTypes<string>(ExplainStep.Instance, QuerySemantics.None);

        IOutEdgeGremlinQuery<TElement, TNewOutVertex> IEdgeGremlinQuery<TElement>.From<TNewOutVertex>(StepLabel<TNewOutVertex> stepLabel) => AddStep<TElement, TNewOutVertex, Unit, Unit, Unit, Unit>(new FromLabelStep(stepLabel), QuerySemantics.Edge);

        IEdgeGremlinQuery<TElement, TTargetVertex, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.From<TTargetVertex>(StepLabel<TTargetVertex> stepLabel) => AddStep<TElement, TTargetVertex, TOutVertex, Unit, Unit, Unit>(new FromLabelStep(stepLabel), QuerySemantics.Edge);

        IEdgeGremlinQuery<TElement, TTargetVertex, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.From<TTargetVertex>(Func<IVertexGremlinQuery<TOutVertex>, IVertexGremlinQuery<TTargetVertex>> fromVertexTraversal) => AddStep<TElement, TTargetVertex, TOutVertex, Unit, Unit, Unit>(new FromTraversalStep(fromVertexTraversal(Anonymize<TOutVertex, Unit, Unit, Unit, Unit, Unit>())), QuerySemantics.Edge);

        IOutEdgeGremlinQuery<TElement, TNewOutVertex> IEdgeGremlinQuery<TElement>.From<TNewOutVertex>(Func<IVertexGremlinQuery, IVertexGremlinQuery<TNewOutVertex>> fromVertexTraversal) => AddStep<TElement, TNewOutVertex, Unit, Unit, Unit, Unit>(new FromTraversalStep(fromVertexTraversal(Anonymize())), QuerySemantics.Edge);

        IEdgeGremlinQuery<TElement, TNewOutVertex, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.From<TNewOutVertex>(Func<IVertexGremlinQuery<TInVertex>, IGremlinQuery<TNewOutVertex>> fromVertexTraversal) => AddStep<TElement, TNewOutVertex, TInVertex, Unit, Unit, Unit>(new FromTraversalStep(fromVertexTraversal(Anonymize<TInVertex, Unit, Unit, Unit, Unit, Unit>())), QuerySemantics.Edge);

        GremlinQueryAwaiter IGremlinQuery.GetAwaiter() => new GremlinQueryAwaiter(((IGremlinQuery<TElement>)this).ToAsyncEnumerable().Select(_ => Unit.Default).LastAsync().GetAwaiter());

        GremlinQueryAwaiter<TElement> IGremlinQuery<TElement>.GetAwaiter() => new GremlinQueryAwaiter<TElement>(this.ToArrayAsync().GetAwaiter());

        IValueGremlinQuery<object> IElementGremlinQuery.Id() => Id();

        IVertexGremlinQuery<IVertex> IVertexGremlinQuery.In() => AddStepWithUnitTypes<IVertex>(InStep.NoLabels, QuerySemantics.Vertex);

        IVertexGremlinQuery<IVertex> IVertexGremlinQuery.In<TEdge>() => AddStepWithUnitTypes<IVertex>(Environment.Model.EdgesModel.GetFilterStepOrNone(typeof(TEdge), Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity), labels => new InStep(labels)), QuerySemantics.Vertex);

        IEdgeGremlinQuery<IEdge> IVertexGremlinQuery.InE() => AddStepWithUnitTypes<IEdge>(InEStep.NoLabels, QuerySemantics.Edge);

        IEdgeGremlinQuery<TEdge> IVertexGremlinQuery.InE<TEdge>() => AddStepWithUnitTypes<TEdge>(Environment.Model.EdgesModel.GetFilterStepOrNone(typeof(TEdge), Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity), labels => new InEStep(labels)), QuerySemantics.Edge);

        IInEdgeGremlinQuery<TEdge, TElement> IVertexGremlinQuery<TElement>.InE<TEdge>() => AddStep<TEdge, Unit, TElement, Unit, Unit, Unit>(Environment.Model.EdgesModel.GetFilterStepOrNone(typeof(TEdge), Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity), labels => new InEStep(labels)), QuerySemantics.Edge);

        IGremlinQuery<TNewElement> IGremlinQueryBase.Inject<TNewElement>(params TNewElement[] elements) => Inject(elements);

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Inject(params TElement[] elements) => Inject(elements);

        IAsyncEnumerable<TElement> IGremlinQuery<TElement>.ToAsyncEnumerable() => Environment.Pipeline.Execute(this);

        IValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.Inject(params TElement[] elements) => Inject(elements);

        IVertexGremlinQuery<IVertex> IEdgeGremlinQuery.InV() => InV<IVertex>();

        IVertexGremlinQuery<TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.InV() => InV<TInVertex>();

        IVertexGremlinQuery<TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.InV() => InV<TInVertex>();

        IVertexGremlinQuery<TVertex> IEdgeGremlinQuery.InV<TVertex>() => InV<Unit>().OfType<TVertex>(Environment.Model.VerticesModel);

        IValueGremlinQuery<string> IPropertyGremlinQuery<TElement>.Key() => Key();

        IValueGremlinQuery<string> IElementGremlinQuery.Label() => Label();

        IVertexPropertyGremlinQuery<VertexProperty<TPropertyValue, TNewMeta>, TPropertyValue, TNewMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Meta<TNewMeta>() => Cast<VertexProperty<TPropertyValue, TNewMeta>, Unit, Unit, TPropertyValue, TNewMeta, Unit>();

        IVertexGremlinQuery<IVertex> IEdgeGremlinQuery.OtherV() => OtherV<IVertex>();

        IVertexGremlinQuery<TVertex> IEdgeGremlinQuery.OtherV<TVertex>() => OtherV<Unit>().OfType<TVertex>(Environment.Model.VerticesModel);

        IVertexGremlinQuery<IVertex> IVertexGremlinQuery.Out() => AddStepWithUnitTypes<IVertex>(OutStep.NoLabels, QuerySemantics.Vertex);

        IVertexGremlinQuery<IVertex> IVertexGremlinQuery.Out<TEdge>() => AddStepWithUnitTypes<IVertex>(Environment.Model.EdgesModel.GetFilterStepOrNone(typeof(TEdge), Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity), labels => new OutStep(labels)), QuerySemantics.Vertex);

        IEdgeGremlinQuery<TEdge> IVertexGremlinQuery.OutE<TEdge>() => AddStepWithUnitTypes<TEdge>(Environment.Model.EdgesModel.GetFilterStepOrNone(typeof(TEdge), Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity), labels => new OutEStep(labels)), QuerySemantics.Edge);

        IEdgeGremlinQuery<IEdge> IVertexGremlinQuery.OutE() => AddStepWithUnitTypes<IEdge>(OutEStep.NoLabels, QuerySemantics.Edge);

        IOutEdgeGremlinQuery<TEdge, TElement> IVertexGremlinQuery<TElement>.OutE<TEdge>() => AddStep<TEdge, TElement, Unit, Unit, Unit, Unit>(Environment.Model.EdgesModel.GetFilterStepOrNone(typeof(TEdge), Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity), labels => new OutEStep(labels)), QuerySemantics.Edge);

        IVertexGremlinQuery<IVertex> IEdgeGremlinQuery.OutV() => OutV<IVertex>();

        IVertexGremlinQuery<TVertex> IEdgeGremlinQuery.OutV<TVertex>() => OutV<Unit>().OfType<TVertex>(Environment.Model.VerticesModel);

        IVertexGremlinQuery<TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.OutV() => AddStepWithUnitTypes<TOutVertex>(OutVStep.Instance, QuerySemantics.Vertex);

        IVertexGremlinQuery<TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.OutV() => AddStepWithUnitTypes<TOutVertex>(OutVStep.Instance, QuerySemantics.Vertex);

        IGremlinQuery<string> IGremlinQuery.Profile() => AddStepWithUnitTypes<string>(ProfileStep.Instance, QuerySemantics.None);

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQuery<TElement>.Properties<TValue>(params string[] keys) => Properties<VertexProperty<TValue>, TValue, Unit>(keys, QuerySemantics.VertexProperty);

        IVertexPropertyGremlinQuery<VertexProperty<object>, object> IVertexGremlinQuery<TElement>.Properties(params string[] keys) => Properties<VertexProperty<object>, object, Unit>(keys, QuerySemantics.VertexProperty);

        IVertexPropertyGremlinQuery<VertexProperty<object>, object> IVertexGremlinQuery<TElement>.Properties() => Properties<VertexProperty<object>, object, Unit>(Array.Empty<string>(), QuerySemantics.VertexProperty);

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQuery<TElement>.Properties<TValue>(params Expression<Func<TElement, TValue>>[] projections) => VertexProperties<TValue>(projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQuery<TElement>.Properties<TValue>(params Expression<Func<TElement, TValue[]>>[] projections) => VertexProperties<TValue>(projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQuery<TElement>.Properties<TValue>(params Expression<Func<TElement, VertexProperty<TValue>>>[] projections) => VertexProperties<TValue>(projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue, TNewMeta>, TValue, TNewMeta> IVertexGremlinQuery<TElement>.Properties<TValue, TNewMeta>(params Expression<Func<TElement, VertexProperty<TValue, TNewMeta>>>[] projections) => Properties<VertexProperty<TValue, TNewMeta>, TValue, TNewMeta>(QuerySemantics.VertexProperty, projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQuery<TElement>.Properties<TValue>(params Expression<Func<TElement, VertexProperty<TValue>[]>>[] projections) => VertexProperties<TValue>(projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue, TNewMeta>, TValue, TNewMeta> IVertexGremlinQuery<TElement>.Properties<TValue, TNewMeta>(params Expression<Func<TElement, VertexProperty<TValue, TNewMeta>[]>>[] projections) => Properties<VertexProperty<TValue, TNewMeta>, TValue, TNewMeta>(QuerySemantics.VertexProperty, projections);

        IPropertyGremlinQuery<Property<TValue>> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Properties<TValue>(params Expression<Func<TMeta, TValue>>[] projections) => Properties<Property<TValue>, TValue, Unit>(QuerySemantics.Property, projections);

        IPropertyGremlinQuery<Property<object>> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Properties(params string[] keys) => Properties<Property<object>, object, Unit>(keys, QuerySemantics.Property);

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQuery<TElement>.Properties<TValue>() => Properties<VertexProperty<TValue>, TValue, Unit>(Array.Empty<string>(), QuerySemantics.VertexProperty);

        IPropertyGremlinQuery<Property<TValue>> IEdgeGremlinQuery<TElement>.Properties<TValue>(params Expression<Func<TElement, TValue>>[] projections) => Properties<Property<TValue>, TValue, Unit>(QuerySemantics.Property, projections);

        IPropertyGremlinQuery<Property<TValue>> IEdgeGremlinQuery<TElement>.Properties<TValue>(params Expression<Func<TElement, Property<TValue>>>[] projections) => Properties<Property<TValue>, TValue, Unit>(QuerySemantics.Property, projections);

        IPropertyGremlinQuery<Property<TValue>> IEdgeGremlinQuery<TElement>.Properties<TValue>(params Expression<Func<TElement, TValue[]>>[] projections) => Properties<Property<TValue>, TValue, Unit>(QuerySemantics.Property, projections);

        IPropertyGremlinQuery<Property<TValue>> IEdgeGremlinQuery<TElement>.Properties<TValue>(params Expression<Func<TElement, Property<TValue>[]>>[] projections) => Properties<Property<TValue>, TValue, Unit>(QuerySemantics.Property, projections);

        IPropertyGremlinQuery<Property<TValue>> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Properties<TValue>(params string[] keys) => Properties<Property<TValue>, Unit, Unit>(keys, QuerySemantics.Property);

        IVertexPropertyGremlinQuery<VertexProperty<object>, object> IVertexGremlinQuery<TElement>.Properties(params Expression<Func<TElement, VertexProperty<object>>>[] projections) => VertexProperties<object>(projections);

        IPropertyGremlinQuery<Property<object>> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Properties(params string[] keys) => Properties<Property<object>, Unit, Unit>(keys, QuerySemantics.Property);

        IPropertyGremlinQuery<Property<object>> IEdgeGremlinQuery<TElement>.Properties(params Expression<Func<TElement, Property<object>>>[] projections) => Properties<Property<object>, object, Unit>(QuerySemantics.Property, projections);

        IPropertyGremlinQuery<Property<object>> IEdgeGremlinQuery<TElement>.Properties() => Properties<Property<object>, Unit, Unit>(Array.Empty<string>(), QuerySemantics.Property);

        IPropertyGremlinQuery<Property<TValue>> IEdgeGremlinQuery<TElement>.Properties<TValue>() => Properties<Property<TValue>, Unit, Unit>(Array.Empty<string>(), QuerySemantics.Property);

        IPropertyGremlinQuery<Property<TValue>> IEdgeGremlinQuery<TElement>.Properties<TValue>(params string[] keys) => Properties<Property<TValue>, Unit, Unit>(keys, QuerySemantics.Property);

        IPropertyGremlinQuery<Property<object>> IEdgeGremlinQuery<TElement>.Properties(params string[] keys) => Properties<Property<object>, Unit, Unit>(keys, QuerySemantics.Property);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Property<TValue>(Expression<Func<TMeta, TValue>> projection, TValue value) => Property(projection, value);

        TQuery IGremlinQuery.Select<TQuery, TStepElement>(StepLabel<TQuery, TStepElement> label)
        {
            if (Steps.LastOrDefault() is AsStep asStep && asStep.StepLabels.Contains(label))
                return this.ChangeQueryType<TQuery>();

            return this
                .Select(label)
                .ChangeQueryType<TQuery>();
        }

        IGremlinQuery<TStep> IGremlinQuery.Select<TStep>(StepLabel<TStep> label) => Select<TStep>(label);

        IImmutableList<Step> IGremlinQueryAdmin.Steps => Steps;

        IGremlinQueryEnvironment IGremlinQueryAdmin.Environment => Environment;

        IValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.SumGlobal() => SumGlobal();

        IValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.SumLocal() => SumLocal();

        IInEdgeGremlinQuery<TElement, TNewInVertex> IEdgeGremlinQuery<TElement>.To<TNewInVertex>(StepLabel<TNewInVertex> stepLabel) => To<TElement, Unit, TNewInVertex>(stepLabel);

        IEdgeGremlinQuery<TElement, TOutVertex, TTargetVertex> IEdgeGremlinQuery<TElement, TOutVertex>.To<TTargetVertex>(StepLabel<TTargetVertex> stepLabel) => To<TElement, TOutVertex, TTargetVertex>(stepLabel);

        IEdgeGremlinQuery<TElement, TOutVertex, TTargetVertex> IEdgeGremlinQuery<TElement, TOutVertex>.To<TTargetVertex>(Func<IVertexGremlinQuery<TOutVertex>, IVertexGremlinQuery<TTargetVertex>> toVertexTraversal) => AddStep<TElement, TOutVertex, TTargetVertex, Unit, Unit, Unit>(new AddEStep.ToTraversalStep(toVertexTraversal(Anonymize<TOutVertex, Unit, Unit, Unit, Unit, Unit>())), QuerySemantics.Edge);

        IInEdgeGremlinQuery<TElement, TNewInVertex> IEdgeGremlinQuery<TElement>.To<TNewInVertex>(Func<IVertexGremlinQuery, IVertexGremlinQuery<TNewInVertex>> toVertexTraversal) => AddStep<TElement, Unit, TNewInVertex, Unit, Unit, Unit>(new AddEStep.ToTraversalStep(toVertexTraversal(Anonymize())), QuerySemantics.Edge);

        IEdgeGremlinQuery<TElement, TOutVertex, TNewInVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.To<TNewInVertex>(Func<IVertexGremlinQuery<TOutVertex>, IGremlinQuery<TNewInVertex>> toVertexTraversal) => AddStep<TElement, TOutVertex, TNewInVertex, Unit, Unit, Unit>(new AddEStep.ToTraversalStep(toVertexTraversal(Anonymize<TOutVertex, Unit, Unit, Unit, Unit, Unit>())), QuerySemantics.Edge);

        TFoldedQuery IArrayGremlinQuery<TElement, TFoldedQuery>.Unfold() => Unfold<TFoldedQuery>();

        IVertexGremlinQuery<IVertex> IGremlinQueryBase.V(params object[] ids) => AddStepWithUnitTypes<IVertex>(new VStep(ids), QuerySemantics.Vertex);

        IValueGremlinQuery<TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Value() => Value<TPropertyValue>();

        IValueGremlinQuery<TValue> IPropertyGremlinQuery<TElement>.Value<TValue>() => Value<TValue>();

        IValueGremlinQuery<TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Value() => Value<TPropertyValue>();

        IValueGremlinQuery<object> IPropertyGremlinQuery<TElement>.Value() => Value<object>();

        IGremlinQuery<TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.ValueMap() => ValueMap<TMeta>(Array.Empty<string>());

        IValueGremlinQuery<IDictionary<string, TTarget>> IElementGremlinQuery.ValueMap<TTarget>(params string[] keys) => ValueMap<IDictionary<string, TTarget>>(keys);

        IValueGremlinQuery<IDictionary<string, object>> IElementGremlinQuery.ValueMap(params string[] keys) => ValueMap<IDictionary<string, object>>(keys);

        IValueGremlinQuery<TValue> IVertexGremlinQuery<TElement>.Values<TValue, TNewMeta>(params Expression<Func<TElement, VertexProperty<TValue, TNewMeta>>>[] projections) => ValuesForProjections<TValue>(projections);

        IValueGremlinQuery<TValue> IVertexGremlinQuery<TElement>.Values<TValue>(params Expression<Func<TElement, VertexProperty<TValue>>>[] projections) => ValuesForProjections<TValue>(projections);

        IValueGremlinQuery<TValue> IEdgeGremlinQuery<TElement>.Values<TValue>(params Expression<Func<TElement, Property<TValue>>>[] projections) => ValuesForProjections<TValue>(projections);

        IValueGremlinQuery<TTarget> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Values<TTarget>(params Expression<Func<TMeta, TTarget>>[] projections) => ValuesForProjections<TTarget>(projections);

        IValueGremlinQuery<TTarget> IVertexGremlinQuery<TElement>.Values<TTarget>(params Expression<Func<TElement, VertexProperty<TTarget>[]>>[] projections) => ValuesForProjections<TTarget>(projections);

        IValueGremlinQuery<TTarget> IVertexGremlinQuery<TElement>.Values<TTarget, TTargetMeta>(params Expression<Func<TElement, VertexProperty<TTarget, TTargetMeta>[]>>[] projections) => ValuesForProjections<TTarget>(projections);

        IValueGremlinQuery<TTarget> IEdgeGremlinQuery<TElement>.Values<TTarget>(params Expression<Func<TElement, Property<TTarget>[]>>[] projections) => ValuesForProjections<TTarget>(projections);

        IValueGremlinQuery<TValue> IElementGremlinQuery.Values<TValue>(params string[] keys) => ValuesForKeys<TValue>(keys);

        IValueGremlinQuery<object> IElementGremlinQuery.Values(params string[] keys) => ValuesForKeys<object>(keys);

        IValueGremlinQuery<object> IVertexGremlinQuery<TElement>.Values(params Expression<Func<TElement, VertexProperty<object>>>[] projections) => ValuesForProjections<object>(projections);

        IValueGremlinQuery<object> IEdgeGremlinQuery<TElement>.Values(params Expression<Func<TElement, Property<object>>>[] projections) => ValuesForProjections<object>(projections);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Where(Expression<Func<VertexProperty<TPropertyValue, TMeta>, bool>> predicate) => Where(predicate);

        IValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.Where(Expression<Func<TElement, bool>> predicate) => Where(predicate);

        IPropertyGremlinQuery<TElement> IPropertyGremlinQuery<TElement>.Where(Expression<Func<TElement, bool>> predicate) => Where(predicate);

        IVertexGremlinQuery<TNewVertex> IGremlinQueryBase.ReplaceV<TNewVertex>(TNewVertex vertex)
        {
            return this
                .V<TNewVertex>(vertex.GetId(Environment.Model.PropertiesModel))
                .Update(vertex);
        }

        IEdgeGremlinQuery<TNewEdge> IGremlinQueryBase.ReplaceE<TNewEdge>(TNewEdge edge)
        {
            return this
                .E<TNewEdge>(edge.GetId(Environment.Model.PropertiesModel))
                .Update(edge);
        }
    }
}
