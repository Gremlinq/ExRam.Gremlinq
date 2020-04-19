using System;
using System.Collections.Immutable;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public class GremlinqOption
    {
        public static GremlinqOption<IImmutableList<Step>> VertexProjectionSteps = new GremlinqOption<IImmutableList<Step>>(
            new Step[]
            {
                new ProjectStep("id", "label", "properties"),
                new ProjectStep.ByKeyStep(T.Id),
                new ProjectStep.ByKeyStep(T.Label),
                new ProjectStep.ByTraversalStep(new Step[]
                {
                    new PropertiesStep(Array.Empty<string>()),
                    GroupStep.Instance,
                    new GroupStep.ByKeyStep(T.Label),
                    new GroupStep.ByTraversalStep(new Step[]
                    {
                        new ProjectStep("id", "label", "value", "properties"),
                        new ProjectStep.ByKeyStep(T.Id),
                        new ProjectStep.ByKeyStep(T.Label),
                        new ProjectStep.ByKeyStep(T.Value),
                        new ProjectStep.ByTraversalStep(new ValueMapStep(Array.Empty<string>())),
                        FoldStep.Instance
                    })
                })
            }
            .ToImmutableList());

        public static GremlinqOption<IImmutableList<Step>> EdgeProjectionSteps = new GremlinqOption<IImmutableList<Step>>(
            new Step[]
            {
                new ProjectStep("id", "label", "properties"),
                new ProjectStep.ByKeyStep(T.Id),
                new ProjectStep.ByKeyStep(T.Label),
                new ProjectStep.ByTraversalStep(new ValueMapStep(Array.Empty<string>()))
            }
            .ToImmutableList());

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
