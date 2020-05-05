using System;

namespace ExRam.Gremlinq.Core
{
    [Flags]
    public enum VertexFeatures
    {
        None = 0,

        MetaProperties           =              0b1,
        Upsert                   =             0b10,
        DuplicateMultiProperties =            0b100,
        AddVertices              =           0b1000,
        MultiProperties          =          0b10000,
        RemoveVertices           =         0b100000,
        AnyIds                   =        0b1000000,
        UuidIds                  =       0b10000000,
        UserSuppliedIds          =      0b100000000,
        CustomIds                =     0b1000000000,
        NumericIds               =    0b10000000000,
        RemoveProperty           =   0b100000000000,
        AddProperty              =  0b1000000000000,
        StringIds                = 0b10000000000000,

        All                      = 0b11111111111111
    }
}
