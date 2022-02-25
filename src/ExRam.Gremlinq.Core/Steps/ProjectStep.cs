using System;
using System.Collections.Immutable;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class ProjectStep : Step
    {
        public abstract class ByStep : Step
        {
            protected ByStep(SideEffectSemanticsChange sideEffectSemanticsChange = SideEffectSemanticsChange.None) : base(sideEffectSemanticsChange)
            {
            }
        }

        public sealed class ByTraversalStep : ByStep
        {
            public ByTraversalStep(Traversal traversal) : base(traversal.GetSideEffectSemanticsChange())
            {
                Traversal = traversal;
            }

            public Traversal Traversal { get; }
        }

        public sealed class ByKeyStep : ByStep
        {
            public ByKeyStep(Key key)
            {
                Key = key;
            }

            public ByTraversalStep ToByTraversalStep() => new ByTraversalStep(Key.RawKey switch
            {
                T t => t.EnumValue switch
                {
                    "id" => (Step)IdStep.Instance,
                    "label" => LabelStep.Instance,
                    "key" => KeyStep.Instance,
                    "value" => ValueStep.Instance,
                    _ => throw new NotImplementedException(),
                },
                string key => new ValuesStep(ImmutableArray.Create(key)),
                _ => throw new NotImplementedException(),
            });

            public Key Key { get; }
        }

        public ProjectStep(ImmutableArray<string> projections) : base()
        {
            Projections = projections;
        }

        public ImmutableArray<string> Projections { get; }
    }
}
