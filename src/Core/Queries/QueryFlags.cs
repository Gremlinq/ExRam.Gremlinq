namespace ExRam.Gremlinq.Core
{
    [Flags]
    internal enum QueryFlags
    {
        None = 0,

        IsAnonymous = 2,
        IsMuted = 4
    }
}
