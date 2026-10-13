namespace ExRam.Gremlinq.Tests.Infrastructure.GraphSon.Entities
{
    // A dictionary that cannot be made empty, with a constructor that takes an IDictionary<,> and,
    // declared after it, one that takes its pairs. Each adds an entry that names it, so it shows
    // which of them built the dictionary.
    public class ClassDerivedFromDictionaryWithDictionaryAndPairsConstructors : Dictionary<string, int>
    {
        public ClassDerivedFromDictionaryWithDictionaryAndPairsConstructors(IDictionary<string, int> dictionary) : base(dictionary)
        {
            Add("fromDictionary", 0);
        }

        public ClassDerivedFromDictionaryWithDictionaryAndPairsConstructors(IEnumerable<KeyValuePair<string, int>> pairs) : base(pairs)
        {
            Add("fromPairs", 0);
        }
    }
}
