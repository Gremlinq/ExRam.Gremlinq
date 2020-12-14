using System;

namespace ExRam.Gremlinq.Core
{
    [Flags]
    public enum EdgeFeatures
    {
        None            =             0,

        AddEdges        =           0b1,
        Upsert          =          0b10,
        RemoveEdges     =         0b100,
        AnyIds          =        0b1000,
        UuidIds         =       0b10000,
        UserSuppliedIds =      0b100000,
        CustomIds       =     0b1000000,
        NumericIds      =    0b10000000,
        RemoveProperty  =   0b100000000,
        AddProperty     =  0b1000000000,
        StringIds       = 0b10000000000,

        All             = 0b11111111111
    }
}
