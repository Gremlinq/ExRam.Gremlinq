namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Feature flags describing edge-related capabilities of a graph database.
    /// </summary>
    [Flags]
    public enum EdgeFeatures
    {
        /// <summary>No edge features supported.</summary>
        None            =             0,

        /// <summary>Supports adding edges.</summary>
        AddEdges        =           0b1,
        /// <summary>Supports upserting edges.</summary>
        Upsert          =          0b10,
        /// <summary>Supports removing edges.</summary>
        RemoveEdges     =         0b100,
        /// <summary>Supports any type of edge identifier.</summary>
        AnyIds          =        0b1000,
        /// <summary>Supports UUID edge identifiers.</summary>
        UuidIds         =       0b10000,
        /// <summary>Supports user-supplied edge identifiers.</summary>
        UserSuppliedIds =      0b100000,
        /// <summary>Supports custom edge identifier types.</summary>
        CustomIds       =     0b1000000,
        /// <summary>Supports numeric edge identifiers.</summary>
        NumericIds      =    0b10000000,
        /// <summary>Supports removing edge properties.</summary>
        RemoveProperty  =   0b100000000,
        /// <summary>Supports adding edge properties.</summary>
        AddProperty     =  0b1000000000,
        /// <summary>Supports string edge identifiers.</summary>
        StringIds       = 0b10000000000,

        /// <summary>All edge features supported.</summary>
        All             = 0b11111111111
    }
}
