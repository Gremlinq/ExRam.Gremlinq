namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Feature flags describing variable-related capabilities of a graph database.
    /// </summary>
    [Flags]
    public enum VariableFeatures
    {
        /// <summary>No variable features supported.</summary>
        None               =                     0,

        /// <summary>Supports graph variables.</summary>
        Variables          =                   0b1,
        /// <summary>Supports serializable variable values.</summary>
        SerializableValues =                  0b10,
        /// <summary>Supports uniform list variable values.</summary>
        UniformListValues  =                 0b100,
        /// <summary>Supports boolean array variable values.</summary>
        BooleanArrayValues =                0b1000,
        /// <summary>Supports double array variable values.</summary>
        DoubleArrayValues  =               0b10000,
        /// <summary>Supports integer array variable values.</summary>
        IntegerArrayValues =              0b100000,
        /// <summary>Supports string array variable values.</summary>
        StringArrayValues  =             0b1000000,
        /// <summary>Supports float variable values.</summary>
        FloatValues        =            0b10000000,
        /// <summary>Supports long variable values.</summary>
        LongValues         =           0b100000000,
        /// <summary>Supports mixed list variable values.</summary>
        MixedListValues    =          0b1000000000,
        /// <summary>Supports string variable values.</summary>
        StringValues       =         0b10000000000,
        /// <summary>Supports long array variable values.</summary>
        LongArrayValues    =        0b100000000000,
        /// <summary>Supports map variable values.</summary>
        MapValues          =       0b1000000000000,
        /// <summary>Supports byte array variable values.</summary>
        ByteArrayValues    =      0b10000000000000,
        /// <summary>Supports float array variable values.</summary>
        FloatArrayValues   =     0b100000000000000,
        /// <summary>Supports boolean variable values.</summary>
        BooleanValues      =    0b1000000000000000,
        /// <summary>Supports byte variable values.</summary>
        ByteValues         =   0b10000000000000000,
        /// <summary>Supports double variable values.</summary>
        DoubleValues       =  0b100000000000000000,
        /// <summary>Supports integer variable values.</summary>
        IntegerValues      = 0b1000000000000000000,

        /// <summary>All variable features supported.</summary>
        All                = 0b1111111111111111111
    }
}
