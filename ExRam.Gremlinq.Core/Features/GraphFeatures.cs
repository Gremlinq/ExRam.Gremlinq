using System;

namespace ExRam.Gremlinq.Core
{
    [Flags]
    public enum GraphFeatures
    {
        None = 0,

        Transactions         = 0b1,
        Computer             = 0b10,
        IoWrite              = 0b100,
        IoRead               = 0b1000,
        ThreadedTransactions = 0b10000,
        Persistence          = 0b100000,
        ConcurrentAccess     = 0b1000000,

        All = 0xFF
    }
}
