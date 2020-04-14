using System.Collections.Generic;

namespace ExRam.Gremlinq.Core
{
    public sealed class ProjectStep : Step
    {
        public sealed class ByTraversalStep : SingleTraversalArgumentStep
        {
            public ByTraversalStep(IGremlinQueryBase traversal) : base(traversal)
            {
            }
        }

        public sealed class ByStepsStep : Step
        {
            public ByStepsStep(IEnumerable<Step> steps)
            {
                Steps = steps;
            }

            public IEnumerable<Step> Steps { get; }
        }

        public sealed class ByKeyStep : Step
        {
            public ByKeyStep(object key)
            {
                Key = key;
            }

            public object Key { get; }
        }

        public ProjectStep(params string[] projections)
        {
            Projections = projections;
        }

        public string[] Projections { get; }
    }
}
