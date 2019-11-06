using System;

namespace ExRam.Gremlinq.Core
{
    public sealed class AndStep : LogicalStep
    {
        public static readonly AndStep Infix = new AndStep(Array.Empty<IGremlinQuery>());

        public AndStep(IGremlinQuery[] traversals) : base("and", traversals)
        {
        }
    }
}
