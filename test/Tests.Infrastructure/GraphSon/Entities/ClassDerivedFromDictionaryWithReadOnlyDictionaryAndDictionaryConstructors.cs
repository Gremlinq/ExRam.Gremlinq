namespace ExRam.Gremlinq.Tests.Infrastructure.GraphSon.Entities
{
    // A dictionary that cannot be made empty, with a constructor that takes an
    // IReadOnlyDictionary<,> and, declared after it, one that takes an IDictionary<,>. Each adds an
    // entry that names it, so it shows which of them built the dictionary.
    public class ClassDerivedFromDictionaryWithReadOnlyDictionaryAndDictionaryConstructors : Dictionary<string, int>
    {
        public ClassDerivedFromDictionaryWithReadOnlyDictionaryAndDictionaryConstructors(IReadOnlyDictionary<string, int> dictionary) : base(dictionary)
        {
            Add("fromReadOnlyDictionary", 0);
        }

        public ClassDerivedFromDictionaryWithReadOnlyDictionaryAndDictionaryConstructors(IDictionary<string, int> dictionary) : base(dictionary)
        {
            Add("fromDictionary", 0);
        }
    }
}
