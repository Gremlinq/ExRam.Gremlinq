namespace ExRam.Gremlinq.Tests.Infrastructure.GraphSon.Entities
{
    // A read-only dictionary whose one constructor takes a dictionary - but a read-only one,
    // which an IDictionary<,> is not.
    public class ClassImplementingIReadOnlyDictionaryWithReadOnlyDictionaryConstructor<TKey, TValue> : AbstractClassImplementingIReadOnlyDictionary<TKey, TValue>
        where TKey : notnull
    {
        public ClassImplementingIReadOnlyDictionaryWithReadOnlyDictionaryConstructor(IReadOnlyDictionary<TKey, TValue> dictionary) : base(dictionary)
        {
        }
    }
}
