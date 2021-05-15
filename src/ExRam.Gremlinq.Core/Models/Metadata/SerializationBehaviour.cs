using System;

namespace ExRam.Gremlinq.Core.Models
{
    [Flags]
    public enum SerializationBehaviour
    {
        Default = 0,

        IgnoreOnAdd = 1,
        IgnoreOnUpdate = 2,

        IgnoreAlways = 3
    }
}
