using System;

namespace ExRam.Gremlinq.Core
{
    public sealed class AndStep : LogicalStep
    {
        public static readonly AndStep Infix = new AndStep(Array.Empty<IGremlinQueryBase>());

        public AndStep(IGremlinQueryBase[] traversals) : base("and", traversals)
        {
        }
    }
}
