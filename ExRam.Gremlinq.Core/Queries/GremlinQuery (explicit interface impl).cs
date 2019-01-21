// ReSharper disable ArrangeThisQualifier
// ReSharper disable CoVariantArrayConversion
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq.Expressions;
using ExRam.Gremlinq.Core.GraphElements;
using LanguageExt;
using NullGuard;

namespace ExRam.Gremlinq.Core
{
    partial class GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> :
        IGremlinQueryAdmin,

        IOrderedGremlinQuery,
        IOrderedElementGremlinQuery,

        IOrderedArrayGremlinQuery<TElement, TFoldedQuery>,

        IOrderedGremlinQuery<TElement>,
        IOrderedElementGremlinQuery<TElement>,

        IOrderedValueGremlinQuery<TElement>,

        IOrderedVertexGremlinQuery,
        IOrderedVertexGremlinQuery<TElement>,

        IOrderedEdgeGremlinQuery,
        IOrderedEdgeGremlinQuery<TElement>,
        IOrderedEdgeGremlinQuery<TElement, TOutVertex>,
        IOrderedInEdgeGremlinQuery<TElement, TInVertex>,
        IOrderedOutEdgeGremlinQuery<TElement, TOutVertex>,
        IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex>,

        IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue>,
        IOrderedVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>,

        IOrderedEdgePropertyGremlinQuery<TElement>
    {
        IEdgeGremlinQuery<TEdge> IGremlinQuerySource.AddE<TEdge>(TEdge edge) => AddE(edge);

        IEdgeGremlinQuery<TEdge, TElement> IVertexGremlinQuery<TElement>.AddE<TEdge>(TEdge edge) => AddE(edge);

        IEdgeGremlinQuery<TEdge, TElement> IVertexGremlinQuery<TElement>.AddE<TEdge>() => AddE(new TEdge());

        IVertexGremlinQuery<TVertex> IGremlinQuerySource.AddV<TVertex>(TVertex vertex) => AddV(vertex);

        IGremlinQueryAdmin IGremlinQuery.AsAdmin() => this;

        IVertexGremlinQuery<IVertex> IVertexGremlinQuery.Both() => AddStep<IVertex>(BothStep.NoLabels);

        IVertexGremlinQuery<IVertex> IVertexGremlinQuery.Both<TEdge>() => AddStep<IVertex>(new BothStep(Model.VerticesModel.GetValidFilterLabels(typeof(TEdge))));

        IEdgeGremlinQuery<IEdge> IVertexGremlinQuery.BothE() => AddStep<IEdge>(BothEStep.NoLabels);

        IEdgeGremlinQuery<TEdge> IVertexGremlinQuery.BothE<TEdge>() => AddStep<TEdge>(new BothEStep(Model.EdgesModel.GetValidFilterLabels(typeof(TEdge))));

        IVertexGremlinQuery<IVertex> IEdgeGremlinQuery.BothV() => AddStep<IVertex>(BothVStep.Instance);


        TTargetQuery IGremlinQueryAdmin.ChangeQueryType<TTargetQuery>() => ChangeQueryType<TTargetQuery>();


        TTargetQuery IValueGremlinQuery<TElement>.Choose<TTargetQuery>(Expression<Func<TElement, bool>> predicate, Func<IValueGremlinQuery<TElement>, TTargetQuery> trueChoice, Func<IValueGremlinQuery<TElement>, TTargetQuery> falseChoice) => Choose(predicate, trueChoice, falseChoice);

        TTargetQuery IValueGremlinQuery<TElement>.Choose<TTargetQuery>(Expression<Func<TElement, bool>> predicate, Func<IValueGremlinQuery<TElement>, TTargetQuery> trueChoice) => Choose(predicate, trueChoice);


        IValueGremlinQuery<TValue> IGremlinQuery.Constant<TValue>(TValue constant) => AddStep<TValue>(new ConstantStep(constant));

        IValueGremlinQuery<long> IGremlinQuery.Count() => AddStep<long, Unit, Unit, Unit, Unit, Unit>(CountStep.Global);

        IValueGremlinQuery<long> IGremlinQuery.CountLocal() => AddStep<long, Unit, Unit, Unit, Unit, Unit>(CountStep.Local);


        IGremlinQuery<Unit> IGremlinQuery.Drop() => Drop();

        IEdgeGremlinQuery<TEdge> IGremlinQuerySource.E<TEdge>(params object[] ids) => AddStep<Unit, Unit, Unit, Unit, Unit, Unit>(new EStep(ids)).OfType<TEdge>(Model.EdgesModel, true);

        IEdgeGremlinQuery<IEdge> IGremlinQuerySource.E(params object[] ids) => AddStep<IEdge, Unit, Unit, Unit, Unit, Unit>(new EStep(ids));


        IGremlinQuery<string> IGremlinQuery.Explain() => AddStep<string, Unit, Unit, Unit, Unit, Unit>(ExplainStep.Instance);


        IOutEdgeGremlinQuery<TElement, TNewOutVertex> IEdgeGremlinQuery<TElement>.From<TNewOutVertex>(StepLabel<TNewOutVertex> stepLabel) => AddStep<TElement, TNewOutVertex, Unit, Unit, Unit, Unit>(new FromLabelStep(stepLabel));

        IEdgeGremlinQuery<TElement, TTargetVertex, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.From<TTargetVertex>(StepLabel<TTargetVertex> stepLabel) => AddStep<TElement, TTargetVertex, TOutVertex, Unit, Unit, Unit>(new FromLabelStep(stepLabel));

        IEdgeGremlinQuery<TElement, TTargetVertex, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.From<TTargetVertex>(Func<IVertexGremlinQuery<TOutVertex>, IGremlinQuery<TTargetVertex>> fromVertexTraversal) => AddStep<TElement, TTargetVertex, TOutVertex, Unit, Unit, Unit>(new FromTraversalStep(fromVertexTraversal(Anonymize<TOutVertex, Unit, Unit, Unit, Unit, Unit>())));

        IOutEdgeGremlinQuery<TElement, TNewOutVertex> IEdgeGremlinQuery<TElement>.From<TNewOutVertex>(Func<IGremlinQuery, IGremlinQuery<TNewOutVertex>> fromVertexTraversal) => From<TElement, TNewOutVertex, Unit>(fromVertexTraversal);

        IEdgeGremlinQuery<TElement, TNewOutVertex, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.From<TNewOutVertex>(Func<IGremlinQuery, IGremlinQuery<TNewOutVertex>> fromVertexTraversal) => From<TElement, TNewOutVertex, TInVertex>(fromVertexTraversal);

        IAsyncEnumerator<TElement> IAsyncEnumerable<TElement>.GetEnumerator() => GetEnumerator<TElement>();

        IValueGremlinQuery<object> IElementGremlinQuery.Id() => Id();


        IVertexGremlinQuery<IVertex> IVertexGremlinQuery.In() => AddStep<IVertex>(InStep.NoLabels);

        IVertexGremlinQuery<IVertex> IVertexGremlinQuery.In<TEdge>() => AddStep<IVertex>(new InStep(Model.EdgesModel.GetValidFilterLabels(typeof(TEdge))));

        IEdgeGremlinQuery<IEdge> IVertexGremlinQuery.InE() => AddStep<IEdge>(InEStep.NoLabels);

        IEdgeGremlinQuery<TEdge> IVertexGremlinQuery.InE<TEdge>() => AddStep<TEdge>(new InEStep(Model.EdgesModel.GetValidFilterLabels(typeof(TEdge))));

        IInEdgeGremlinQuery<TEdge, TElement> IVertexGremlinQuery<TElement>.InE<TEdge>() => AddStep<TEdge, Unit, TElement, Unit, Unit, Unit>(new InEStep(Model.EdgesModel.GetValidFilterLabels(typeof(TEdge))));

        IGremlinQuery<TNewElement> IGremlinQuerySource.Inject<TNewElement>(params TNewElement[] elements) => Inject(elements);

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Inject(params TElement[] elements) => Inject(elements);

        IGremlinQuery IGremlinQueryAdmin.InsertStep(int index, Step step) => new GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>(Model, QueryExecutor, Steps.Insert(index, step), StepLabelMappings, Logger);

        IVertexGremlinQuery<IVertex> IEdgeGremlinQuery.InV() => InV<IVertex>();

        IVertexGremlinQuery<TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.InV() => InV<TInVertex>();

        IVertexGremlinQuery<TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.InV() => InV<TInVertex>();

        IValueGremlinQuery<string> IElementGremlinQuery.Label() => AddStep<string>(LabelStep.Instance);


        IVertexPropertyGremlinQuery<VertexProperty<TPropertyValue, TNewMeta>, TPropertyValue, TNewMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Meta<TNewMeta>() => Cast<VertexProperty<TPropertyValue, TNewMeta>, Unit, Unit, TPropertyValue, TNewMeta, Unit>();

        IGraphModel IGremlinQueryAdmin.Model => Model;

        IVertexGremlinQuery<IVertex> IEdgeGremlinQuery.OtherV() => AddStep<IVertex>(OtherVStep.Instance);

        IVertexGremlinQuery<IVertex> IVertexGremlinQuery.Out() => AddStep<IVertex>(OutStep.NoLabels);

        IVertexGremlinQuery<IVertex> IVertexGremlinQuery.Out<TEdge>() => AddStep<IVertex, Unit, Unit, Unit, Unit, Unit>(new OutStep(Model.EdgesModel.GetValidFilterLabels(typeof(TEdge))));

        IEdgeGremlinQuery<TEdge> IVertexGremlinQuery.OutE<TEdge>() => AddStep<TEdge>(new OutEStep(Model.EdgesModel.GetValidFilterLabels(typeof(TEdge))));

        IEdgeGremlinQuery<IEdge> IVertexGremlinQuery.OutE() => AddStep<IEdge>(OutEStep.NoLabels);

        IOutEdgeGremlinQuery<TEdge, TElement> IVertexGremlinQuery<TElement>.OutE<TEdge>() => AddStep<TEdge, TElement, Unit, Unit, Unit, Unit>(new OutEStep(Model.EdgesModel.GetValidFilterLabels(typeof(TEdge))));

        IVertexGremlinQuery<IVertex> IEdgeGremlinQuery.OutV() => AddStep<IVertex, Unit, Unit, Unit, Unit, Unit>(OutVStep.Instance);

        IVertexGremlinQuery<TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.OutV() => AddStep<TOutVertex, Unit, Unit, Unit, Unit, Unit>(OutVStep.Instance);

        IVertexGremlinQuery<TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.OutV() => AddStep<TOutVertex, Unit, Unit, Unit, Unit, Unit>(OutVStep.Instance);

        IGremlinQuery<string> IGremlinQuery.Profile() => AddStep<string>(ProfileStep.Instance);

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQuery<TElement>.Properties<TValue>(params Expression<Func<TElement, TValue>>[] projections) => VertexProperties<TValue>(projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQuery<TElement>.Properties<TValue>(params Expression<Func<TElement, TValue[]>>[] projections) => VertexProperties<TValue>(projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQuery<TElement>.Properties<TValue>(params Expression<Func<TElement, VertexProperty<TValue>>>[] projections) => VertexProperties<TValue>(projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue, TNewMeta>, TValue, TNewMeta> IVertexGremlinQuery<TElement>.Properties<TValue, TNewMeta>(params Expression<Func<TElement, VertexProperty<TValue, TNewMeta>>>[] projections) => Properties<VertexProperty<TValue, TNewMeta>, TValue, TNewMeta>(projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQuery<TElement>.Properties<TValue>(params Expression<Func<TElement, VertexProperty<TValue>[]>>[] projections) => VertexProperties<TValue>(projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue, TNewMeta>, TValue, TNewMeta> IVertexGremlinQuery<TElement>.Properties<TValue, TNewMeta>(params Expression<Func<TElement, VertexProperty<TValue, TNewMeta>[]>>[] projections) => Properties<VertexProperty<TValue, TNewMeta>, TValue, TNewMeta>(projections);

        IGremlinQuery<Property<TValue>> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Properties<TValue>(params Expression<Func<TMeta, TValue>>[] projections) => Properties<Property<TValue>, TValue, Unit>(projections);

        IEdgePropertyGremlinQuery<Property<TValue>> IEdgeGremlinQuery<TElement>.Properties<TValue>(params Expression<Func<TElement, TValue>>[] projections) => Properties<Property<TValue>, TValue, Unit>(projections);

        IEdgePropertyGremlinQuery<Property<TValue>> IEdgeGremlinQuery<TElement>.Properties<TValue>(params Expression<Func<TElement, Property<TValue>>>[] projections) => Properties<Property<TValue>, TValue, Unit>(projections);

        IEdgePropertyGremlinQuery<Property<TValue>> IEdgeGremlinQuery<TElement>.Properties<TValue>(params Expression<Func<TElement, TValue[]>>[] projections) => Properties<Property<TValue>, TValue, Unit>(projections);

        IEdgePropertyGremlinQuery<Property<TValue>> IEdgeGremlinQuery<TElement>.Properties<TValue>(params Expression<Func<TElement, Property<TValue>[]>>[] projections) => Properties<Property<TValue>, TValue, Unit>(projections);

        IGremlinQuery<Property<TValue>> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Properties<TValue>(params string[] keys) => Properties<TValue>(keys);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Property<TValue>(Expression<Func<TMeta, TValue>> projection, TValue value) => Property(projection, value);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Property(string key, [AllowNull] object value)
        {
            if (value == null)
            {
                return SideEffect(_ => _
                    .Properties(key)
                    .Drop());
            }

            return AddStep(new MetaPropertyStep(key, value));
        }

        TQuery IGremlinQuery.Select<TQuery, TStepElement>(StepLabel<TQuery, TStepElement> label)
        {
            return this
                .Select<TStepElement>(label)
                .ChangeQueryType<TQuery>();
        }

        IGremlinQuery<TStep> IGremlinQuery.Select<TStep>(StepLabel<TStep> label) => Select<TStep>(label);

        IGremlinQuery<(T1, T2)> IGremlinQuery.Select<T1, T2>(StepLabel<T1> label1, StepLabel<T2> label2)
        {
            return this
                .AddStep<(T1, T2), Unit, Unit, Unit, Unit, Unit>(new SelectStep(label1, label2))
                .AddStepLabelBinding(label1, x => x.Item1)
                .AddStepLabelBinding(label2, x => x.Item2);
        }

        IGremlinQuery<(T1, T2, T3)> IGremlinQuery.Select<T1, T2, T3>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3)
        {
            return this
                .AddStep<(T1, T2, T3), Unit, Unit, Unit, Unit, Unit>(new SelectStep(label1, label2, label3))
                .AddStepLabelBinding(label1, x => x.Item1)
                .AddStepLabelBinding(label2, x => x.Item2)
                .AddStepLabelBinding(label3, x => x.Item3);
        }

        IGremlinQuery<(T1, T2, T3, T4)> IGremlinQuery.Select<T1, T2, T3, T4>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4)
        {
            return this
                .AddStep<(T1, T2, T3, T4), Unit, Unit, Unit, Unit, Unit>(new SelectStep(label1, label2, label3, label4))
                .AddStepLabelBinding(label1, x => x.Item1)
                .AddStepLabelBinding(label2, x => x.Item2)
                .AddStepLabelBinding(label3, x => x.Item3)
                .AddStepLabelBinding(label4, x => x.Item4);
        }

        IImmutableDictionary<StepLabel, string> IGremlinQueryAdmin.StepLabelMappings => StepLabelMappings;

        IImmutableList<Step> IGremlinQueryAdmin.Steps => Steps;

        IValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.SumGlobal() => SumGlobal();

        IValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.SumLocal() => SumLocal();

        IInEdgeGremlinQuery<TElement, TNewInVertex> IEdgeGremlinQuery<TElement>.To<TNewInVertex>(StepLabel<TNewInVertex> stepLabel) => To<TElement, Unit, TNewInVertex>(stepLabel);

        IEdgeGremlinQuery<TElement, TOutVertex, TTargetVertex> IEdgeGremlinQuery<TElement, TOutVertex>.To<TTargetVertex>(StepLabel<TTargetVertex> stepLabel) => To<TElement, TOutVertex, TTargetVertex>(stepLabel);

        IEdgeGremlinQuery<TElement, TOutVertex, TTargetVertex> IEdgeGremlinQuery<TElement, TOutVertex>.To<TTargetVertex>(Func<IVertexGremlinQuery<TOutVertex>, IGremlinQuery<TTargetVertex>> toVertexTraversal) => AddStep<TElement, TOutVertex, TTargetVertex, Unit, Unit, Unit>(new ToTraversalStep(toVertexTraversal(Anonymize<TOutVertex, Unit, Unit, Unit, Unit, Unit>())));

        IInEdgeGremlinQuery<TElement, TNewInVertex> IEdgeGremlinQuery<TElement>.To<TNewInVertex>(Func<IGremlinQuery, IGremlinQuery<TNewInVertex>> toVertexTraversal) => To<TElement, Unit, TNewInVertex>(toVertexTraversal);

        IEdgeGremlinQuery<TElement, TOutVertex, TNewInVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.To<TNewInVertex>(Func<IGremlinQuery, IGremlinQuery<TNewInVertex>> toVertexTraversal) => To<TElement, TOutVertex, TNewInVertex>(toVertexTraversal);

        TFoldedQuery IArrayGremlinQuery<TElement, TFoldedQuery>.Unfold() => Unfold<TFoldedQuery>();

        IVertexGremlinQuery<TVertex> IGremlinQuerySource.V<TVertex>(params object[] ids) => AddStep<Unit, Unit, Unit, Unit, Unit, Unit>(new VStep(ids)).OfType<TVertex>(Model.VerticesModel, true);

        IVertexGremlinQuery<IVertex> IGremlinQuerySource.V(params object[] ids) => AddStep<IVertex, Unit, Unit, Unit, Unit, Unit>(new VStep(ids));

        IValueGremlinQuery<TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Value() => Value();

        IValueGremlinQuery<TPropertyValue> IVertexPropertyGremlinQuery<TElement, TPropertyValue>.Value() => Value();

        IGremlinQuery<TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.ValueMap() => ValueMap<TMeta>(Array.Empty<string>());

        IValueGremlinQuery<IDictionary<string, TTarget>> IElementGremlinQuery.ValueMap<TTarget>(params string[] keys) => ValueMap<IDictionary<string, TTarget>>(keys);

        IValueGremlinQuery<TValue> IVertexGremlinQuery<TElement>.Values<TValue, TNewMeta>(params Expression<Func<TElement, VertexProperty<TValue, TNewMeta>>>[] projections) => ValuesForProjections<TValue>(projections);

        IValueGremlinQuery<TValue> IVertexGremlinQuery<TElement>.Values<TValue>(params Expression<Func<TElement, VertexProperty<TValue>>>[] projections) => ValuesForProjections<TValue>(projections);

        IValueGremlinQuery<TValue> IEdgeGremlinQuery<TElement>.Values<TValue>(params Expression<Func<TElement, Property<TValue>>>[] projections) => ValuesForProjections<TValue>(projections);

        IValueGremlinQuery<TTarget> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Values<TTarget>(params Expression<Func<TMeta, TTarget>>[] projections) => ValuesForProjections<TTarget>(projections);

        IValueGremlinQuery<TTarget> IVertexGremlinQuery<TElement>.Values<TTarget>(params Expression<Func<TElement, VertexProperty<TTarget>[]>>[] projections) => ValuesForProjections<TTarget>(projections);

        IValueGremlinQuery<TTarget> IVertexGremlinQuery<TElement>.Values<TTarget, TTargetMeta>(params Expression<Func<TElement, VertexProperty<TTarget, TTargetMeta>[]>>[] projections) => ValuesForProjections<TTarget>(projections);

        IValueGremlinQuery<TTarget> IEdgeGremlinQuery<TElement>.Values<TTarget>(params Expression<Func<TElement, Property<TTarget>[]>>[] projections) => ValuesForProjections<TTarget>(projections);

        IValueGremlinQuery<TValue> IElementGremlinQuery.Values<TValue>(params string[] keys) => ValuesForKeys<TValue>(keys);

        IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta> IVertexPropertyGremlinQuery<TElement, TPropertyValue, TMeta>.Where(Expression<Func<VertexProperty<TPropertyValue, TMeta>, bool>> predicate) => Where(predicate);

        IValueGremlinQuery<TElement> IValueGremlinQuery<TElement>.Where(Expression<Func<TElement, bool>> predicate) => Where(predicate);
    }
}
