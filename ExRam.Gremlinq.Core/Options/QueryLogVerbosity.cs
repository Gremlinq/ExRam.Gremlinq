using System;

namespace ExRam.Gremlinq.Core
{
    [Flags]
    public enum QueryLogVerbosity
    {
        QueryOnly = 0,
        IncludeBindings = 1
    }
}
