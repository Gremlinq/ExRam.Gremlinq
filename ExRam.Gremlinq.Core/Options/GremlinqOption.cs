using System.Collections.Generic;
using System.Collections.Immutable;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public class GremlinqOption
    {
        public static GremlinqOption<IImmutableList<Step>> VertexProjectionSteps = new GremlinqOption<IImmutableList<Step>>(
            new Step[]
            {
                new ProjectStep(ImmutableArray.Create("id", "label", "properties")),
                new ProjectStep.ByKeyStep(T.Id),
                new ProjectStep.ByKeyStep(T.Label),
                new ProjectStep.ByTraversalStep(new Step[]
                {
                    new PropertiesStep(ImmutableArray<string>.Empty),
                    GroupStep.Instance,
                    new GroupStep.ByKeyStep(T.Label),
                    new GroupStep.ByTraversalStep(new Step[]
                    {
                        new ProjectStep(ImmutableArray.Create("id", "label", "value", "properties")),
                        new ProjectStep.ByKeyStep(T.Id),
                        new ProjectStep.ByKeyStep(T.Label),
                        new ProjectStep.ByKeyStep(T.Value),
                        new ProjectStep.ByTraversalStep(new ValueMapStep(ImmutableArray<string>.Empty)),
                        FoldStep.Instance
                    })
                })
            }
            .ToImmutableList());

        public static GremlinqOption<IImmutableList<Step>> VertexProjectionWithoutMetaPropertiesSteps = new GremlinqOption<IImmutableList<Step>>(
            new Step[]
            {
                new ProjectStep(ImmutableArray.Create("id", "label", "properties")),
                new ProjectStep.ByKeyStep(T.Id),
                new ProjectStep.ByKeyStep(T.Label),
                new ProjectStep.ByTraversalStep(new Step[]
                {
                    new PropertiesStep(ImmutableArray<string>.Empty),
                    GroupStep.Instance,
                    new GroupStep.ByKeyStep(T.Label),
                    new GroupStep.ByTraversalStep(new Step[]
                    {
                        new ProjectStep(ImmutableArray.Create("id", "label", "value")),
                        new ProjectStep.ByKeyStep(T.Id),
                        new ProjectStep.ByKeyStep(T.Label),
                        new ProjectStep.ByKeyStep(T.Value),
                        FoldStep.Instance
                    })
                })
            }
            .ToImmutableList());

        public static GremlinqOption<IImmutableList<Step>> EdgeProjectionSteps = new GremlinqOption<IImmutableList<Step>>(
            new Step[]
            {
                new ProjectStep(ImmutableArray.Create("id", "label", "properties")),
                new ProjectStep.ByKeyStep(T.Id),
                new ProjectStep.ByKeyStep(T.Label),
                new ProjectStep.ByTraversalStep(new ValueMapStep(ImmutableArray<string>.Empty))
            }
            .ToImmutableList());

        public static GremlinqOption<IImmutableDictionary<T, SerializationBehaviour>> TSerializationBehaviourOverrides = new GremlinqOption<IImmutableDictionary<T, SerializationBehaviour>>(
            new Dictionary<T, SerializationBehaviour>
            {
                { T.Key, SerializationBehaviour.IgnoreOnUpdate },
                { T.Id, SerializationBehaviour.IgnoreOnUpdate },
                { T.Label, SerializationBehaviour.IgnoreAlways },
                { T.Value, SerializationBehaviour.Default }
            }
            .ToImmutableDictionary());

        public static GremlinqOption<FilterLabelsVerbosity> FilterLabelsVerbosity = new GremlinqOption<FilterLabelsVerbosity>(Core.FilterLabelsVerbosity.Maximum);
        public static GremlinqOption<DisabledTextPredicates> DisabledTextPredicates = new GremlinqOption<DisabledTextPredicates>(Core.DisabledTextPredicates.None);
    }

    public class GremlinqOption<TValue> : GremlinqOption
    {
        public GremlinqOption(TValue defaultValue)
        {
            DefaultValue = defaultValue;
        }

        public TValue DefaultValue { get; }
    }
}
