using System;

namespace ExRam.Gremlinq.Core
{
    [Flags]
    public enum QueryFlags
    {
        None = 0,
        SurfaceVisible = 1,
        IsAnonymous = 2,
        IsMuted = 4
    }
}
