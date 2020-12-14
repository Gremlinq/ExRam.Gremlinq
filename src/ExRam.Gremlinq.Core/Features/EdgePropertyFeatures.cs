using System;

namespace ExRam.Gremlinq.Core
{
    [Flags]
    public enum EdgePropertyFeatures
    {
        None               =                     0,

        Properties         =                   0b1,
        SerializableValues =                  0b10,
        UniformListValues  =                 0b100,
        BooleanArrayValues =                0b1000,
        DoubleArrayValues  =               0b10000,
        IntegerArrayValues =              0b100000,
        StringArrayValues  =             0b1000000,
        FloatValues        =            0b10000000,
        LongValues         =           0b100000000,
        MixedListValues    =          0b1000000000,
        StringValues       =         0b10000000000,
        LongArrayValues    =        0b100000000000,
        MapValues          =       0b1000000000000,
        ByteArrayValues    =      0b10000000000000,
        FloatArrayValues   =     0b100000000000000,
        BooleanValues      =    0b1000000000000000,
        ByteValues         =   0b10000000000000000,
        DoubleValues       =  0b100000000000000000,
        IntegerValues      = 0b1000000000000000000,

        All                = 0b1111111111111111111
    }
}
