namespace ExRam.Gremlinq.Tests.Infrastructure.GraphSon.Entities
{
    // A read-only dictionary whose one constructor takes its pairs.
    public class ClassImplementingIReadOnlyDictionaryWithPairsConstructor<TKey, TValue> : AbstractClassImplementingIReadOnlyDictionary<TKey, TValue>
        where TKey : notnull
    {
        public ClassImplementingIReadOnlyDictionaryWithPairsConstructor(IEnumerable<KeyValuePair<TKey, TValue>> pairs) : base(pairs)
        {
        }
    }
}
