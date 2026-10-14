namespace ExRam.Gremlinq.Tests.Infrastructure.GraphSon.Entities
{
    // A read-only dictionary that can be made empty, but not filled: its one constructor takes
    // nothing.
    public class ClassImplementingIReadOnlyDictionaryWithParameterlessConstructor<TKey, TValue> : AbstractClassImplementingIReadOnlyDictionary<TKey, TValue>
        where TKey : notnull
    {
        public ClassImplementingIReadOnlyDictionaryWithParameterlessConstructor() : base([])
        {
        }
    }
}
