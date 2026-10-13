namespace ExRam.Gremlinq.Tests.Infrastructure.GraphSon.Entities
{
    // A dictionary that cannot be made empty, whose one constructor takes a dictionary - but a
    // read-only one, which an IDictionary<,> is not.
    public class ClassDerivedFromDictionaryWithReadOnlyDictionaryConstructor : Dictionary<string, int>
    {
        public ClassDerivedFromDictionaryWithReadOnlyDictionaryConstructor(IReadOnlyDictionary<string, int> dictionary) : base(dictionary)
        {
        }
    }
}
