using System.Collections.Generic;

namespace ExRam.Gremlinq.Core
{
    public sealed class GroupStep : Step
    {
        public sealed class ByTraversalStep : SingleTraversalArgumentStep
        {
            public ByTraversalStep(IGremlinQueryBase traversal) : base(traversal)
            {
            }
        }

        public sealed class ByKeyStep : Step
        {
            public ByKeyStep(object key)
            {
                Key = key;
            }

            public object Key { get; }
        }

        public sealed class ByStepsStep : Step
        {
            public ByStepsStep(Step[] steps)
            {
                Steps = steps;
            }

            public Step[] Steps { get; }
        }

        private GroupStep()
        {

        }

        public static readonly GroupStep Instance = new GroupStep();
    }
}
