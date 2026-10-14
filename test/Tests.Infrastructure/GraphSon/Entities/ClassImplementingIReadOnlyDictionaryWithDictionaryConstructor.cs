namespace ExRam.Gremlinq.Tests.Infrastructure.GraphSon.Entities
{
    // A read-only dictionary whose one constructor takes an IDictionary<,>.
    public class ClassImplementingIReadOnlyDictionaryWithDictionaryConstructor<TKey, TValue> : AbstractClassImplementingIReadOnlyDictionary<TKey, TValue>
        where TKey : notnull
    {
        public ClassImplementingIReadOnlyDictionaryWithDictionaryConstructor(IDictionary<TKey, TValue> dictionary) : base(dictionary)
        {
        }
    }
}
