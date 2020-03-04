using System;

namespace ExRam.Gremlinq.Core
{
    public sealed class AndStep : LogicalStep<AndStep>
    {
        public static readonly AndStep Infix = new AndStep(Array.Empty<IGremlinQueryBase>());

        public AndStep(IGremlinQueryBase[] traversals) : base("and", traversals)
        {
        }
    }
}
