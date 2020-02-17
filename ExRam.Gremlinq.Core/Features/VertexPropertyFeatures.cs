namespace ExRam.Gremlinq.Core
{
    public enum VertexPropertyFeatures
    {
        None = 0,

        AnyIds             = 0b1,
        UuidIds            = 0b10,
        UserSuppliedIds    = 0b100,
        CustomIds          = 0b1000,
        NumericIds         = 0b10000,
        RemoveProperty     = 0b100000,
        StringIds          = 0b1000000,
        Properties         = 0b10000000,
        SerializableValues = 0b100000000,
        UniformListValues  = 0b1000000000,
        BooleanArrayValues = 0b10000000000,
        DoubleArrayValues  = 0b100000000000,
        IntegerArrayValues = 0b1000000000000,
        StringArrayValues  = 0b10000000000000,
        FloatValues        = 0b100000000000000,
        LongValues         = 0b1000000000000000,
        MixedListValues    = 0b10000000000000000,
        StringValues       = 0b100000000000000000,
        LongArrayValues    = 0b1000000000000000000,
        MapValues          = 0b10000000000000000000,
        ByteArrayValues    = 0b100000000000000000000,
        FloatArrayValues   = 0b1000000000000000000000,
        BooleanValues      = 0b10000000000000000000000,
        ByteValues         = 0b100000000000000000000000,
        DoubleValues       = 0b1000000000000000000000000,
        IntegerValues      = 0b10000000000000000000000000,

        All = 0xFF
    }
}
