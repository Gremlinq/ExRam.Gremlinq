using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core
{
    public sealed class ChooseOptionTraversalStep : SingleTraversalArgumentStep
    {
        public ChooseOptionTraversalStep(IGremlinQuery chooseTraversal) : base(chooseTraversal)
        {
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}