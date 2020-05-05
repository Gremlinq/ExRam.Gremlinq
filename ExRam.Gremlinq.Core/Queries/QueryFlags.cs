using System;

namespace ExRam.Gremlinq.Core
{
    [Flags]
    internal enum QueryFlags
    {
        None = 0,
        SurfaceVisible = 1,
        IsAnonymous = 2
    }
}
