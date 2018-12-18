using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core
{
    public sealed class ValuesStep : Step
    {
        public object[] Keys { get; }

        public ValuesStep(object[] keys)
        {
            Keys = keys;
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
