using System;

namespace ExRam.Gremlinq.Providers.WebSocket
{
    [Flags]
    public enum QueryLogVerbosity
    {
        QueryOnly = 0,
        IncludeParameters = 1
    }
}
