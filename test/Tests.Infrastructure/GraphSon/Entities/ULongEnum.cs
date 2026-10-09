namespace ExRam.Gremlinq.Tests.Infrastructure.GraphSon.Entities
{
    // An enum whose underlying type is wider than long: 18446744073709551615 is a value of it,
    // -1 is none.
    public enum ULongEnum : ulong
    {
        Zero,
        One,
        Two
    }
}
