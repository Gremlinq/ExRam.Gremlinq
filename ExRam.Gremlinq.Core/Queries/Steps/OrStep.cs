using System;

namespace ExRam.Gremlinq.Core
{
    public sealed class OrStep : LogicalStep
    {
        public static readonly OrStep Infix = new OrStep(Array.Empty<IGremlinQuery>());

        public OrStep(IGremlinQuery[] traversals) : base("or", traversals)
        {
        }
    }
}
