namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Feature flags describing edge property capabilities of a graph database.
    /// </summary>
    [Flags]
    public enum EdgePropertyFeatures
    {
        /// <summary>No edge property features supported.</summary>
        None               =                     0,

        /// <summary>Supports properties on edges.</summary>
        Properties         =                   0b1,
        /// <summary>Supports serializable property values.</summary>
        SerializableValues =                  0b10,
        /// <summary>Supports uniform list property values.</summary>
        UniformListValues  =                 0b100,
        /// <summary>Supports boolean array property values.</summary>
        BooleanArrayValues =                0b1000,
        /// <summary>Supports double array property values.</summary>
        DoubleArrayValues  =               0b10000,
        /// <summary>Supports integer array property values.</summary>
        IntegerArrayValues =              0b100000,
        /// <summary>Supports string array property values.</summary>
        StringArrayValues  =             0b1000000,
        /// <summary>Supports float property values.</summary>
        FloatValues        =            0b10000000,
        /// <summary>Supports long property values.</summary>
        LongValues         =           0b100000000,
        /// <summary>Supports mixed list property values.</summary>
        MixedListValues    =          0b1000000000,
        /// <summary>Supports string property values.</summary>
        StringValues       =         0b10000000000,
        /// <summary>Supports long array property values.</summary>
        LongArrayValues    =        0b100000000000,
        /// <summary>Supports map property values.</summary>
        MapValues          =       0b1000000000000,
        /// <summary>Supports byte array property values.</summary>
        ByteArrayValues    =      0b10000000000000,
        /// <summary>Supports float array property values.</summary>
        FloatArrayValues   =     0b100000000000000,
        /// <summary>Supports boolean property values.</summary>
        BooleanValues      =    0b1000000000000000,
        /// <summary>Supports byte property values.</summary>
        ByteValues         =   0b10000000000000000,
        /// <summary>Supports double property values.</summary>
        DoubleValues       =  0b100000000000000000,
        /// <summary>Supports integer property values.</summary>
        IntegerValues      = 0b1000000000000000000,

        /// <summary>All edge property features supported.</summary>
        All                = 0b1111111111111111111
    }
}
