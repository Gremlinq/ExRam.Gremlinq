using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core
{
    public sealed class AddEStep : AddElementStep
    {
        public AddEStep(IGraphModel model, object value) : base(model.TryGetConstructiveEdgeLabel(value.GetType()).IfNone(value.GetType().Name))
        {
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
