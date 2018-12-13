using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class AddEStep : AddElementStep
    {
        public AddEStep(IGraphModel model, object value) : base(model, value)
        {
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
