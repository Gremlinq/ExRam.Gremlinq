namespace ExRam.Gremlinq.Tests.Infrastructure.GraphSon.Entities
{
    // A dictionary that cannot be made empty, and none of whose constructors takes a dictionary.
    public class ClassDerivedFromDictionaryWithCapacityConstructors : Dictionary<string, int>
    {
        public ClassDerivedFromDictionaryWithCapacityConstructors(int capacity) : base(capacity)
        {
        }

        public ClassDerivedFromDictionaryWithCapacityConstructors(int capacity, IEqualityComparer<string>? comparer) : base(capacity, comparer)
        {
        }
    }
}
