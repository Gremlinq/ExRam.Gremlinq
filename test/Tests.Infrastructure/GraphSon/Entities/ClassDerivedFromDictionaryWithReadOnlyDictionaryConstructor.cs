namespace ExRam.Gremlinq.Tests.Infrastructure.GraphSon.Entities
{
    // A dictionary that cannot be made empty, whose one constructor takes a dictionary - a
    // read-only one, which an IDictionary<,> is not, but which a Dictionary<,> can be passed as.
    public class ClassDerivedFromDictionaryWithReadOnlyDictionaryConstructor : Dictionary<string, int>
    {
        public ClassDerivedFromDictionaryWithReadOnlyDictionaryConstructor(IReadOnlyDictionary<string, int> dictionary) : base(dictionary)
        {
        }
    }
}
