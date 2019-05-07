using System;

namespace ExRam.Gremlinq.Core
{
    [Flags]
    public enum SerializationDirective
    {
        Default = 0,

        IgnoreOnAdd = 1,
        IgnoreOnUpdate = 2,

        IgnoreAlways = 3
    }
}