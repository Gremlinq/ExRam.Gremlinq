using System.Collections.Generic;
using System.Collections.Immutable;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinqOption
    {
        public static readonly GremlinqOption<Traversal> VertexProjectionSteps = new(
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
            });

        public static readonly GremlinqOption<Traversal> VertexProjectionWithoutMetaPropertiesSteps = new(
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
            });

        public static readonly GremlinqOption<Traversal> EdgeProjectionSteps = new(
            new Step[]
            {
                new ProjectStep(ImmutableArray.Create("id", "label", "properties")),
                new ProjectStep.ByKeyStep(T.Id),
                new ProjectStep.ByKeyStep(T.Label),
                new ProjectStep.ByTraversalStep(new ValueMapStep(ImmutableArray<string>.Empty))
            });

        public static readonly GremlinqOption<IImmutableDictionary<T, SerializationBehaviour>> TSerializationBehaviourOverrides = new(
            new Dictionary<T, SerializationBehaviour>
            {
                { T.Key, SerializationBehaviour.IgnoreOnUpdate },
                { T.Id, SerializationBehaviour.IgnoreOnUpdate },
                { T.Label, SerializationBehaviour.IgnoreAlways },
                { T.Value, SerializationBehaviour.Default }
            }
            .ToImmutableDictionary());

        public static readonly GremlinqOption<FilterLabelsVerbosity> FilterLabelsVerbosity = new(Core.FilterLabelsVerbosity.Maximum);
        public static readonly GremlinqOption<DisabledTextPredicates> DisabledTextPredicates = new(Core.DisabledTextPredicates.None);
        public static readonly GremlinqOption<StringComparisonTranslationStrictness> StringComparisonTranslationStrictness = new(Core.StringComparisonTranslationStrictness.Strict);
    }

    public class GremlinqOption<TValue> : IGremlinqOption
    {
        public GremlinqOption(TValue defaultValue)
        {
            DefaultValue = defaultValue;
        }

        public TValue DefaultValue { get; }
    }
}
