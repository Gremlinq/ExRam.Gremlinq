using ExRam.Gremlinq.Tests.Entities;

namespace ExRam.Gremlinq.Tests.Infrastructure.GraphSon.Entities
{
    public sealed class PersonLanguageTuple
    {
        public Person? Key { get; set; }
        public Language? Value { get; set; }
    }
}
