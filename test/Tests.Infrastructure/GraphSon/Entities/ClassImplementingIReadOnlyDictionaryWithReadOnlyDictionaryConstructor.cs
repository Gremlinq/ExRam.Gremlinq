namespace ExRam.Gremlinq.Tests.Infrastructure.GraphSon.Entities
{
    // A read-only dictionary whose one constructor takes a dictionary - a read-only one, which an
    // IDictionary<,> is not, but which a Dictionary<,> can be passed as.
    public class ClassImplementingIReadOnlyDictionaryWithReadOnlyDictionaryConstructor<TKey, TValue> : AbstractClassImplementingIReadOnlyDictionary<TKey, TValue>
        where TKey : notnull
    {
        public ClassImplementingIReadOnlyDictionaryWithReadOnlyDictionaryConstructor(IReadOnlyDictionary<TKey, TValue> dictionary) : base(dictionary)
        {
        }
    }
}
