using System.Collections.Immutable;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public class GremlinqOption
    {
        public static GremlinqOption<IImmutableList<Instruction>> VertexProjectionSteps = new GremlinqOption<IImmutableList<Instruction>>(
            new[]
            {
                new Instruction("project", "id", "label", "properties"),
                new Instruction("by", T.Id),
                new Instruction("by", T.Label),
                new Instruction(
                    "by",
                    new Bytecode
                    {
                        StepInstructions =
                        {
                            new Instruction("properties"),
                            new Instruction("group"),
                            new Instruction("by", new Bytecode
                            {
                                StepInstructions =
                                {
                                    new Instruction("label")
                                }
                            }),
                            new Instruction("by", new Bytecode
                            {
                                StepInstructions =
                                {
                                    new Instruction("project", "id", "label", "value", "properties"),
                                    new Instruction("by", T.Id),
                                    new Instruction("by", new Bytecode
                                    {
                                        StepInstructions =
                                        {
                                            new Instruction("label")
                                        }
                                    }),
                                    new Instruction("by", new Bytecode
                                    {
                                        StepInstructions =
                                        {
                                            new Instruction("value")
                                        }
                                    }),
                                    new Instruction("by", new Bytecode
                                    {
                                        StepInstructions =
                                        {
                                            new Instruction("valueMap")
                                        }
                                    }),
                                    new Instruction("fold")
                                }
                            })
                        }
                    })
            }
            .ToImmutableList());

        public static GremlinqOption<IImmutableList<Instruction>> EdgeProjectionSteps = new GremlinqOption<IImmutableList<Instruction>>(
            new[]
            {
                new Instruction("project", "id", "label", "properties"),
                new Instruction("by", T.Id),
                new Instruction("by", T.Label),
                new Instruction("by", new Bytecode
                {
                    StepInstructions =
                    {
                        new Instruction("valueMap")
                    }
                })
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
