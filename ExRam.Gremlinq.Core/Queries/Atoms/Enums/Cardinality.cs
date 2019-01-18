namespace ExRam.Gremlinq.Core
{
    public sealed class Cardinality : GremlinEnum<Cardinality>
    {
        public static readonly Cardinality Set = new Cardinality("set");
        public static readonly Cardinality List = new Cardinality("list");
        public static readonly Cardinality Single = new Cardinality("single");

        private Cardinality(string name) : base(name)
        {
        }
    }
}
