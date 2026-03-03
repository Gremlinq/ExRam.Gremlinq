namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Feature flags describing graph-level capabilities of a graph database.
    /// </summary>
    [Flags]
    public enum GraphFeatures
    {
        /// <summary>No graph features supported.</summary>
        None                 =         0,

        /// <summary>Supports transactions.</summary>
        Transactions         =       0b1,
        /// <summary>Supports graph computer (OLAP).</summary>
        Computer             =      0b10,
        /// <summary>Supports writing graph data to an I/O format.</summary>
        IoWrite              =     0b100,
        /// <summary>Supports reading graph data from an I/O format.</summary>
        IoRead               =    0b1000,
        /// <summary>Supports threaded transactions.</summary>
        ThreadedTransactions =   0b10000,
        /// <summary>Supports graph persistence.</summary>
        Persistence          =  0b100000,
        /// <summary>Supports concurrent access to the graph.</summary>
        ConcurrentAccess     = 0b1000000,

        /// <summary>All graph features supported.</summary>
        All                  = 0b1111111
    }
}
