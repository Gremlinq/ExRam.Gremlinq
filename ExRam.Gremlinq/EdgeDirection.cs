using System;

namespace ExRam.Gremlinq
{
    [Flags]
    public enum EdgeDirection
    {
        None = 0,
        Out = 1,
        In = 2,
        Both = 3
    }
}