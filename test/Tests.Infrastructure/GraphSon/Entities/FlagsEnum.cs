namespace ExRam.Gremlinq.Tests.Infrastructure.GraphSon.Entities
{
    [Flags]
    public enum FlagsEnum
    {
        None = 0,
        Read = 1,
        Write = 2,
        Execute = 4
    }
}
