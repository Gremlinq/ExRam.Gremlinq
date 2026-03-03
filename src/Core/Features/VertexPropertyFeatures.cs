namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Feature flags describing vertex property capabilities of a graph database.
    /// </summary>
    [Flags]
    public enum VertexPropertyFeatures
    {
        /// <summary>No vertex property features supported.</summary>
        None               =                            0,

        /// <summary>Supports any type of vertex property identifier.</summary>
        AnyIds             =                          0b1,
        /// <summary>Supports UUID vertex property identifiers.</summary>
        UuidIds            =                         0b10,
        /// <summary>Supports user-supplied vertex property identifiers.</summary>
        UserSuppliedIds    =                        0b100,
        /// <summary>Supports custom vertex property identifier types.</summary>
        CustomIds          =                       0b1000,
        /// <summary>Supports numeric vertex property identifiers.</summary>
        NumericIds         =                      0b10000,
        /// <summary>Supports removing vertex properties.</summary>
        RemoveProperty     =                     0b100000,
        /// <summary>Supports string vertex property identifiers.</summary>
        StringIds          =                    0b1000000,
        /// <summary>Supports properties on vertex properties (meta-properties).</summary>
        Properties         =                   0b10000000,
        /// <summary>Supports serializable property values.</summary>
        SerializableValues =                  0b100000000,
        /// <summary>Supports uniform list property values.</summary>
        UniformListValues  =                 0b1000000000,
        /// <summary>Supports boolean array property values.</summary>
        BooleanArrayValues =                0b10000000000,
        /// <summary>Supports double array property values.</summary>
        DoubleArrayValues  =               0b100000000000,
        /// <summary>Supports integer array property values.</summary>
        IntegerArrayValues =              0b1000000000000,
        /// <summary>Supports string array property values.</summary>
        StringArrayValues  =             0b10000000000000,
        /// <summary>Supports float property values.</summary>
        FloatValues        =            0b100000000000000,
        /// <summary>Supports long property values.</summary>
        LongValues         =           0b1000000000000000,
        /// <summary>Supports mixed list property values.</summary>
        MixedListValues    =          0b10000000000000000,
        /// <summary>Supports string property values.</summary>
        StringValues       =         0b100000000000000000,
        /// <summary>Supports long array property values.</summary>
        LongArrayValues    =        0b1000000000000000000,
        /// <summary>Supports map property values.</summary>
        MapValues          =       0b10000000000000000000,
        /// <summary>Supports byte array property values.</summary>
        ByteArrayValues    =      0b100000000000000000000,
        /// <summary>Supports float array property values.</summary>
        FloatArrayValues   =     0b1000000000000000000000,
        /// <summary>Supports boolean property values.</summary>
        BooleanValues      =    0b10000000000000000000000,
        /// <summary>Supports byte property values.</summary>
        ByteValues         =   0b100000000000000000000000,
        /// <summary>Supports double property values.</summary>
        DoubleValues       =  0b1000000000000000000000000,
        /// <summary>Supports integer property values.</summary>
        IntegerValues      = 0b10000000000000000000000000,

        /// <summary>All vertex property features supported.</summary>
        All                = 0b11111111111111111111111111
    }
}
