using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core
{
    public sealed class AddEStep : AddElementStep
    {
        public AddEStep(IGraphModel model, object value) : base(model.EdgesModel, value)
        {
        }
    }
}
