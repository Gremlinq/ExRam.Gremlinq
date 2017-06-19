namespace ExRam.Gremlinq.Dse
{
    public sealed class DseGraphModel
    {
        public DseGraphModel(IGraphModel model)
        {
            this.Model = model;
        }

        public IGraphModel Model { get; }
    }
}