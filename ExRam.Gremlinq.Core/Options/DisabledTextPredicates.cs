using System;

namespace ExRam.Gremlinq.Core
{
    [Flags]
    public enum DisabledTextPredicates
    {
        None = 0,
        Containing = 1,
        EndingWith = 2,
        NotContaining = 4,
        NotEndingWith = 8,
        NotStartingWith = 16,
        StartingWith = 32
    }
}