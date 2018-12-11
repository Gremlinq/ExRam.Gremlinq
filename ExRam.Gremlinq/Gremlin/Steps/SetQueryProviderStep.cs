namespace ExRam.Gremlinq
{
    public sealed class SetQueryProviderStep : Step
    {
        public SetQueryProviderStep(IGremlinQueryProvider gremlinQueryProvider)
        {
            GremlinQueryProvider = gremlinQueryProvider;
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
        }

        public IGremlinQueryProvider GremlinQueryProvider { get; }
    }
}
