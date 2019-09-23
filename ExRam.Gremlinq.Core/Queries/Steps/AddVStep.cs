namespace ExRam.Gremlinq.Core
{
    public sealed class AddVStep : AddElementStep
    {
        public AddVStep(IGraphModel model, object value) : base(model.VerticesModel, value)
        {
        }
    }
}
