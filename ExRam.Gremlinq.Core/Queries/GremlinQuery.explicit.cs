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
    partial class GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> :
        IGremlinQueryAdmin,
        IArrayGremlinQuery<TElement, TFoldedQuery>,
        IValueGremlinQuery<TElement>,
        IVertexGremlinQuery<TElement>,
        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>,
        IVertexPropertyGremlinQuery<TElement, TPropertyValue>,
        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>,
        IPropertyGremlinQuery<TElement> where TMeta : class
    {
        IEdgeGremlinQuery<TEdge> IGremlinQueryBase.AddE<TEdge>(TEdge edge) => AddE(edge);

        IEdgeGremlinQuery<TEdge, TElement> IVertexGremlinQuery<TElement>.AddE<TEdge>(TEdge edge) => AddE(edge);

        IEdgeGremlinQuery<TEdge, TElement> IVertexGremlinQuery<TElement>.AddE<TEdge>() => AddE(new TEdge());
        
        IVertexGremlinQuery<TVertex> IGremlinQueryBase.AddV<TVertex>(TVertex vertex) => AddV(vertex);

        IGremlinQueryAdmin IGremlinQuery.AsAdmin() => this;

        IVertexGremlinQuery<object> IVertexGremlinQuery.Both() => AddStepWithObjectTypes<object>(BothStep.NoLabels, QuerySemantics.Vertex);

        IVertexGremlinQuery<object> IVertexGremlinQuery.Both<TEdge>() => AddStepWithObjectTypes<object>(Environment.Model.VerticesModel.GetFilterStepOrNone(typeof(TEdge), Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity), labels => new BothStep(labels)), QuerySemantics.Vertex);

        IEdgeGremlinQuery<object> IVertexGremlinQuery.BothE() => AddStepWithObjectTypes<object>(BothEStep.NoLabels, QuerySemantics.Edge);

        IEdgeGremlinQuery<TEdge> IVertexGremlinQuery.BothE<TEdge>() => AddStepWithObjectTypes<TEdge>(Environment.Model.EdgesModel.GetFilterStepOrNone(typeof(TEdge), Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity), labels => new BothEStep(labels)), QuerySemantics.Edge);

        IVertexGremlinQuery<object> IEdgeGremlinQuery.BothV() => BothV<object>();

        IVertexGremlinQuery<TVertex> IEdgeGremlinQuery.BothV<TVertex>() => BothV<object>().OfType<TVertex>(Environment.Model.VerticesModel);

        IGremlinQuery<object> IGremlinQueryAdmin.ConfigureSteps(Func<IImmutableList<Step>, IImmutableList<Step>> configurator) => ConfigureSteps<object>(configurator);

        TTargetQuery IGremlinQueryAdmin.ChangeQueryType<TTargetQuery>() => ChangeQueryType<TTargetQuery>();

        TTargetQuery IValueGremlinQuery<TElement>.Choose<TTargetQuery>(Expression<Func<TElement, bool>> predicate, Func<IValueGremlinQuery<TElement>, TTargetQuery> trueChoice, Func<IValueGremlinQuery<TElement>, TTargetQuery> falseChoice) => Choose(predicate, trueChoice, falseChoice);

        TTargetQuery IValueGremlinQuery<TElement>.Choose<TTargetQuery>(Expression<Func<TElement, bool>> predicate, Func<IValueGremlinQuery<TElement>, TTargetQuery> trueChoice) => Choose(predicate, trueChoice);

        IValueGremlinQuery<TValue> IGremlinQuery.Constant<TValue>(TValue constant) => AddStepWithObjectTypes<TValue>(new ConstantStep(constant), QuerySemantics.None);

        IValueGremlinQuery<long> IGremlinQuery.Count() => AddStepWithObjectTypes<long>(CountStep.Global, QuerySemantics.None);

        IValueGremlinQuery<long> IGremlinQuery.CountLocal() => AddStepWithObjectTypes<long>(CountStep.Local, QuerySemantics.None);

        IEdgeGremlinQuery<object> IGremlinQueryBase.E(params object[] ids) => AddStepWithObjectTypes<object>(new EStep(ids), QuerySemantics.Edge);

        IGremlinQuery<string> IGremlinQuery.Explain() => AddStepWithObjectTypes<string>(ExplainStep.Instance, QuerySemantics.None);

        IOutEdgeGremlinQuery<TElement, TNewOutVertex> IEdgeGremlinQuery<TElement>.From<TNewOutVertex>(StepLabel<TNewOutVertex> stepLabel) => AddStep<TElement, TNewOutVertex, object, object, object, object>(new FromLabelStep(stepLabel), QuerySemantics.Edge);

        IEdgeGremlinQuery<TElement, TTargetVertex, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.From<TTargetVertex>(StepLabel<TTargetVertex> stepLabel) => AddStep<TElement, TTargetVertex, TOutVertex, object, object, object>(new FromLabelStep(stepLabel), QuerySemantics.Edge);

        IEdgeGremlinQuery<TElement, TTargetVertex, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.From<TTargetVertex>(Func<IVertexGremlinQuery<TOutVertex>, IVertexGremlinQuery<TTargetVertex>> fromVertexTraversal) => AddStep<TElement, TTargetVertex, TOutVertex, object, object, object>(new FromTraversalStep(fromVertexTraversal(Anonymize<TOutVertex, object, object, object, object, object>())), QuerySemantics.Edge);

        IOutEdgeGremlinQuery<TElement, TNewOutVertex> IEdgeGremlinQuery<TElement>.From<TNewOutVertex>(Func<IVertexGremlinQuery, IVertexGremlinQuery<TNewOutVertex>> fromVertexTraversal) => AddStep<TElement, TNewOutVertex, object, object, object, object>(new FromTraversalStep(fromVertexTraversal(Anonymize())), QuerySemantics.Edge);

        IEdgeGremlinQuery<TElement, TNewOutVertex, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.From<TNewOutVertex>(Func<IVertexGremlinQuery<TInVertex>, IGremlinQuery<TNewOutVertex>> fromVertexTraversal) => AddStep<TElement, TNewOutVertex, TInVertex, object, object, object>(new FromTraversalStep(fromVertexTraversal(Anonymize<TInVertex, object, object, object, object, object>())), QuerySemantics.Edge);

        TaskAwaiter IGremlinQuery.GetAwaiter() => ((Task)((IGremlinQuery<TElement>)this).ToAsyncEnumerable().LastOrDefaultAsync().AsTask()).GetAwaiter();
       
        GremlinQueryAwaiter<TElement> IGremlinQuery<TElement>.GetAwaiter() => new GremlinQueryAwaiter<TElement>(this.ToArrayAsync().AsTask().GetAwaiter());

        IValueGremlinQuery<object> IElementGremlinQuery.Id() => Id();

        IVertexGremlinQuery<object> IVertexGremlinQuery.In() => AddStepWithObjectTypes<object>(InStep.NoLabels, QuerySemantics.Vertex);

        IVertexGremlinQuery<object> IVertexGremlinQuery.In<TEdge>() => AddStepWithObjectTypes<object>(Environment.Model.EdgesModel.GetFilterStepOrNone(typeof(TEdge), Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity), labels => new InStep(labels)), QuerySemantics.Vertex);

        IEdgeGremlinQuery<object> IVertexGremlinQuery.InE() => AddStepWithObjectTypes<object>(InEStep.NoLabels, QuerySemantics.Edge);

        IEdgeGremlinQuery<TEdge> IVertexGremlinQuery.InE<TEdge>() => AddStepWithObjectTypes<TEdge>(Environment.Model.EdgesModel.GetFilterStepOrNone(typeof(TEdge), Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity), labels => new InEStep(labels)), QuerySemantics.Edge);

        IInEdgeGremlinQuery<TEdge, TElement> IVertexGremlinQuery<TElement>.InE<TEdge>() => AddStep<TEdge, object, TElement, object, object, object>(Environment.Model.EdgesModel.GetFilterStepOrNone(typeof(TEdge), Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity), labels => new InEStep(labels)), QuerySemantics.Edge);

        IGremlinQuery<TNewElement> IGremlinQueryBase.Inject<TNewElement>(params TNewElement[] elements) => Inject(elements);

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Inject(params TElement[] elements) => Inject(elements);

        IAsyncEnumerable<TElement> IGremlinQuery<TElement>.ToAsyncEnumerable() => Environment.Pipeline.Execute(this);

        IValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.Inject(params TElement[] elements) => Inject(elements);

        IVertexGremlinQuery<object> IEdgeGremlinQuery.InV() => InV<object>();

        IVertexGremlinQuery<TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.InV() => InV<TInVertex>();

        IVertexGremlinQuery<TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.InV() => InV<TInVertex>();

        IVertexGremlinQuery<TVertex> IEdgeGremlinQuery.InV<TVertex>() => InV<object>().OfType<TVertex>(Environment.Model.VerticesModel);

        IValueGremlinQuery<string> IPropertyGremlinQuery<TElement>.Key() => Key();

        IValueGremlinQuery<string> IElementGremlinQuery.Label() => Label();

        IVertexPropertyGremlinQuery<VertexProperty<TPropertyValue, TNewMeta>, TPropertyValue, TNewMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Meta<TNewMeta>() where TNewMeta : class => Cast<VertexProperty<TPropertyValue, TNewMeta>, object, object, TPropertyValue, TNewMeta, object>();

        IVertexGremlinQuery<object> IEdgeGremlinQuery.OtherV() => OtherV<object>();

        IVertexGremlinQuery<TVertex> IEdgeGremlinQuery.OtherV<TVertex>() => OtherV<object>().OfType<TVertex>(Environment.Model.VerticesModel);

        IVertexGremlinQuery<object> IVertexGremlinQuery.Out() => AddStepWithObjectTypes<object>(OutStep.NoLabels, QuerySemantics.Vertex);

        IVertexGremlinQuery<object> IVertexGremlinQuery.Out<TEdge>() => AddStepWithObjectTypes<object>(Environment.Model.EdgesModel.GetFilterStepOrNone(typeof(TEdge), Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity), labels => new OutStep(labels)), QuerySemantics.Vertex);

        IEdgeGremlinQuery<TEdge> IVertexGremlinQuery.OutE<TEdge>() => AddStepWithObjectTypes<TEdge>(Environment.Model.EdgesModel.GetFilterStepOrNone(typeof(TEdge), Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity), labels => new OutEStep(labels)), QuerySemantics.Edge);

        IEdgeGremlinQuery<object> IVertexGremlinQuery.OutE() => AddStepWithObjectTypes<object>(OutEStep.NoLabels, QuerySemantics.Edge);

        IOutEdgeGremlinQuery<TEdge, TElement> IVertexGremlinQuery<TElement>.OutE<TEdge>() => AddStep<TEdge, TElement, object, object, object, object>(Environment.Model.EdgesModel.GetFilterStepOrNone(typeof(TEdge), Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity), labels => new OutEStep(labels)), QuerySemantics.Edge);

        IVertexGremlinQuery<object> IEdgeGremlinQuery.OutV() => OutV<object>();

        IVertexGremlinQuery<TVertex> IEdgeGremlinQuery.OutV<TVertex>() => OutV<object>().OfType<TVertex>(Environment.Model.VerticesModel);

        IVertexGremlinQuery<TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.OutV() => AddStepWithObjectTypes<TOutVertex>(OutVStep.Instance, QuerySemantics.Vertex);

        IVertexGremlinQuery<TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.OutV() => AddStepWithObjectTypes<TOutVertex>(OutVStep.Instance, QuerySemantics.Vertex);

        IGremlinQuery<string> IGremlinQuery.Profile() => AddStepWithObjectTypes<string>(ProfileStep.Instance, QuerySemantics.None);

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQuery<TElement>.Properties<TValue>(params string[] keys) => Properties<VertexProperty<TValue>, TValue, object>(keys, QuerySemantics.VertexProperty);

        IVertexPropertyGremlinQuery<VertexProperty<object>, object> IVertexGremlinQuery<TElement>.Properties(params string[] keys) => Properties<VertexProperty<object>, object, object>(keys, QuerySemantics.VertexProperty);

        IVertexPropertyGremlinQuery<VertexProperty<object>, object> IVertexGremlinQuery<TElement>.Properties() => Properties<VertexProperty<object>, object, object>(Array.Empty<string>(), QuerySemantics.VertexProperty);

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQuery<TElement>.Properties<TValue>(params Expression<Func<TElement, TValue>>[] projections) => VertexProperties<TValue>(projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQuery<TElement>.Properties<TValue>(params Expression<Func<TElement, TValue[]>>[] projections) => VertexProperties<TValue>(projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQuery<TElement>.Properties<TValue>(params Expression<Func<TElement, VertexProperty<TValue>>>[] projections) => VertexProperties<TValue>(projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue, TNewMeta>, TValue, TNewMeta> IVertexGremlinQuery<TElement>.Properties<TValue, TNewMeta>(params Expression<Func<TElement, VertexProperty<TValue, TNewMeta>>>[] projections) => Properties<VertexProperty<TValue, TNewMeta>, TValue, TNewMeta>(QuerySemantics.VertexProperty, projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQuery<TElement>.Properties<TValue>(params Expression<Func<TElement, VertexProperty<TValue>[]>>[] projections) => VertexProperties<TValue>(projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue, TNewMeta>, TValue, TNewMeta> IVertexGremlinQuery<TElement>.Properties<TValue, TNewMeta>(params Expression<Func<TElement, VertexProperty<TValue, TNewMeta>[]>>[] projections) => Properties<VertexProperty<TValue, TNewMeta>, TValue, TNewMeta>(QuerySemantics.VertexProperty, projections);

        IPropertyGremlinQuery<Property<TValue>> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Properties<TValue>(params Expression<Func<TMeta, TValue>>[] projections) => Properties<Property<TValue>, TValue, object>(QuerySemantics.Property, projections);

        IPropertyGremlinQuery<Property<object>> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Properties(params string[] keys) => Properties<Property<object>, object, object>(keys, QuerySemantics.Property);

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQuery<TElement>.Properties<TValue>() => Properties<VertexProperty<TValue>, TValue, object>(Array.Empty<string>(), QuerySemantics.VertexProperty);

        IPropertyGremlinQuery<Property<TValue>> IEdgeGremlinQuery<TElement>.Properties<TValue>(params Expression<Func<TElement, TValue>>[] projections) => Properties<Property<TValue>, TValue, object>(QuerySemantics.Property, projections);

        IPropertyGremlinQuery<Property<TValue>> IEdgeGremlinQuery<TElement>.Properties<TValue>(params Expression<Func<TElement, Property<TValue>>>[] projections) => Properties<Property<TValue>, TValue, object>(QuerySemantics.Property, projections);

        IPropertyGremlinQuery<Property<TValue>> IEdgeGremlinQuery<TElement>.Properties<TValue>(params Expression<Func<TElement, TValue[]>>[] projections) => Properties<Property<TValue>, TValue, object>(QuerySemantics.Property, projections);

        IPropertyGremlinQuery<Property<TValue>> IEdgeGremlinQuery<TElement>.Properties<TValue>(params Expression<Func<TElement, Property<TValue>[]>>[] projections) => Properties<Property<TValue>, TValue, object>(QuerySemantics.Property, projections);

        IPropertyGremlinQuery<Property<TValue>> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Properties<TValue>(params string[] keys) => Properties<Property<TValue>, object, object>(keys, QuerySemantics.Property);

        IVertexPropertyGremlinQuery<VertexProperty<object>, object> IVertexGremlinQuery<TElement>.Properties(params Expression<Func<TElement, VertexProperty<object>>>[] projections) => VertexProperties<object>(projections);

        IPropertyGremlinQuery<Property<object>> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Properties(params string[] keys) => Properties<Property<object>, object, object>(keys, QuerySemantics.Property);

        IPropertyGremlinQuery<Property<object>> IEdgeGremlinQuery<TElement>.Properties(params Expression<Func<TElement, Property<object>>>[] projections) => Properties<Property<object>, object, object>(QuerySemantics.Property, projections);

        IPropertyGremlinQuery<Property<object>> IEdgeGremlinQuery<TElement>.Properties() => Properties<Property<object>, object, object>(Array.Empty<string>(), QuerySemantics.Property);

        IPropertyGremlinQuery<Property<TValue>> IEdgeGremlinQuery<TElement>.Properties<TValue>() => Properties<Property<TValue>, object, object>(Array.Empty<string>(), QuerySemantics.Property);

        IPropertyGremlinQuery<Property<TValue>> IEdgeGremlinQuery<TElement>.Properties<TValue>(params string[] keys) => Properties<Property<TValue>, object, object>(keys, QuerySemantics.Property);

        IPropertyGremlinQuery<Property<object>> IEdgeGremlinQuery<TElement>.Properties(params string[] keys) => Properties<Property<object>, object, object>(keys, QuerySemantics.Property);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Property<TValue>(Expression<Func<TMeta, TValue>> projection, TValue value) => Property(projection, value);

        TQuery IGremlinQuery.Select<TQuery, TStepElement>(StepLabel<TQuery, TStepElement> label)
        {
            if (Steps.LastOrDefault() is AsStep asStep && asStep.StepLabels.Contains(label))
                return this.ChangeQueryType<TQuery>();

            return this
                .Select(label)
                .ChangeQueryType<TQuery>();
        }

        IGremlinQuery<TLabelledElement> IGremlinQuery.Select<TLabelledElement>(StepLabel<TLabelledElement> label) => Select(label);

        IImmutableList<Step> IGremlinQueryAdmin.Steps => Steps;

        IGremlinQueryEnvironment IGremlinQueryAdmin.Environment => Environment;

        IValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.SumGlobal() => SumGlobal();

        IValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.SumLocal() => SumLocal();

        IInEdgeGremlinQuery<TElement, TNewInVertex> IEdgeGremlinQuery<TElement>.To<TNewInVertex>(StepLabel<TNewInVertex> stepLabel) => To<TElement, object, TNewInVertex>(stepLabel);

        IEdgeGremlinQuery<TElement, TOutVertex, TTargetVertex> IEdgeGremlinQuery<TElement, TOutVertex>.To<TTargetVertex>(StepLabel<TTargetVertex> stepLabel) => To<TElement, TOutVertex, TTargetVertex>(stepLabel);

        IEdgeGremlinQuery<TElement, TOutVertex, TTargetVertex> IEdgeGremlinQuery<TElement, TOutVertex>.To<TTargetVertex>(Func<IVertexGremlinQuery<TOutVertex>, IVertexGremlinQuery<TTargetVertex>> toVertexTraversal) => AddStep<TElement, TOutVertex, TTargetVertex, object, object, object>(new AddEStep.ToTraversalStep(toVertexTraversal(Anonymize<TOutVertex, object, object, object, object, object>())), QuerySemantics.Edge);

        IInEdgeGremlinQuery<TElement, TNewInVertex> IEdgeGremlinQuery<TElement>.To<TNewInVertex>(Func<IVertexGremlinQuery, IVertexGremlinQuery<TNewInVertex>> toVertexTraversal) => AddStep<TElement, object, TNewInVertex, object, object, object>(new AddEStep.ToTraversalStep(toVertexTraversal(Anonymize())), QuerySemantics.Edge);

        IEdgeGremlinQuery<TElement, TOutVertex, TNewInVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.To<TNewInVertex>(Func<IVertexGremlinQuery<TOutVertex>, IGremlinQuery<TNewInVertex>> toVertexTraversal) => AddStep<TElement, TOutVertex, TNewInVertex, object, object, object>(new AddEStep.ToTraversalStep(toVertexTraversal(Anonymize<TOutVertex, object, object, object, object, object>())), QuerySemantics.Edge);

        TFoldedQuery IArrayGremlinQuery<TElement, TFoldedQuery>.Unfold() => Unfold<TFoldedQuery>();

        IVertexGremlinQuery<object> IGremlinQueryBase.V(params object[] ids) => AddStepWithObjectTypes<object>(new VStep(ids), QuerySemantics.Vertex);

        IValueGremlinQuery<TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Value() => Value<TPropertyValue>();

        IValueGremlinQuery<TValue> IPropertyGremlinQuery<TElement>.Value<TValue>() => Value<TValue>();

        IValueGremlinQuery<TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Value() => Value<TPropertyValue>();

        IValueGremlinQuery<object> IPropertyGremlinQuery<TElement>.Value() => Value<object>();

        IGremlinQuery<TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.ValueMap() => ValueMap<TMeta>(Array.Empty<string>());

        IValueGremlinQuery<IDictionary<string, TTarget>> IElementGremlinQuery.ValueMap<TTarget>(params string[] keys) => ValueMap<IDictionary<string, TTarget>>(keys);

        IValueGremlinQuery<IDictionary<string, object>> IElementGremlinQuery.ValueMap(params string[] keys) => ValueMap<IDictionary<string, object>>(keys);

        IValueGremlinQuery<TValue> IVertexGremlinQuery<TElement>.Values<TValue, TNewMeta>(params Expression<Func<TElement, VertexProperty<TValue, TNewMeta>>>[] projections) where TNewMeta : class => ValuesForProjections<TValue>(projections);

        IValueGremlinQuery<TValue> IVertexGremlinQuery<TElement>.Values<TValue>(params Expression<Func<TElement, VertexProperty<TValue>>>[] projections) => ValuesForProjections<TValue>(projections);

        IValueGremlinQuery<TValue> IEdgeGremlinQuery<TElement>.Values<TValue>(params Expression<Func<TElement, Property<TValue>>>[] projections) => ValuesForProjections<TValue>(projections);

        IValueGremlinQuery<TTarget> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Values<TTarget>(params Expression<Func<TMeta, TTarget>>[] projections) => ValuesForProjections<TTarget>(projections);

        IValueGremlinQuery<TTarget> IVertexGremlinQuery<TElement>.Values<TTarget>(params Expression<Func<TElement, VertexProperty<TTarget>[]>>[] projections) => ValuesForProjections<TTarget>(projections);

        IValueGremlinQuery<TTarget> IVertexGremlinQuery<TElement>.Values<TTarget, TTargetMeta>(params Expression<Func<TElement, VertexProperty<TTarget, TTargetMeta>[]>>[] projections) where TTargetMeta : class => ValuesForProjections<TTarget>(projections);

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
