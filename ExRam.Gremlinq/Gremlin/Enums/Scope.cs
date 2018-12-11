namespace ExRam.Gremlinq
{
    public sealed class Scope : GremlinEnum<Scope>
    {
        public static readonly Scope Local = new Scope("local");
        public static readonly Scope Global = new Scope("global");

        private Scope(string name) : base(name)
        {
        }
    }
}
