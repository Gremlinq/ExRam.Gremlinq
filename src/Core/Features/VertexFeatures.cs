namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Feature flags describing vertex-related capabilities of a graph database.
    /// </summary>
    [Flags]
    public enum VertexFeatures
    {
        /// <summary>No vertex features supported.</summary>
        None = 0,

        /// <summary>Supports meta-properties on vertex properties.</summary>
        MetaProperties           =              0b1,
        /// <summary>Supports upserting vertices.</summary>
        Upsert                   =             0b10,
        /// <summary>Supports duplicate multi-properties on vertices.</summary>
        DuplicateMultiProperties =            0b100,
        /// <summary>Supports adding vertices.</summary>
        AddVertices              =           0b1000,
        /// <summary>Supports multi-properties on vertices.</summary>
        MultiProperties          =          0b10000,
        /// <summary>Supports removing vertices.</summary>
        RemoveVertices           =         0b100000,
        /// <summary>Supports any type of vertex identifier.</summary>
        AnyIds                   =        0b1000000,
        /// <summary>Supports UUID vertex identifiers.</summary>
        UuidIds                  =       0b10000000,
        /// <summary>Supports user-supplied vertex identifiers.</summary>
        UserSuppliedIds          =      0b100000000,
        /// <summary>Supports custom vertex identifier types.</summary>
        CustomIds                =     0b1000000000,
        /// <summary>Supports numeric vertex identifiers.</summary>
        NumericIds               =    0b10000000000,
        /// <summary>Supports removing vertex properties.</summary>
        RemoveProperty           =   0b100000000000,
        /// <summary>Supports adding vertex properties.</summary>
        AddProperty              =  0b1000000000000,
        /// <summary>Supports string vertex identifiers.</summary>
        StringIds                = 0b10000000000000,

        /// <summary>All vertex features supported.</summary>
        All                      = 0b11111111111111
    }
}
