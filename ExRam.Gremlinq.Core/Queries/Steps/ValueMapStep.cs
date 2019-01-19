using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core
{
    public sealed class ValueMapStep : Step
    {
        public ValueMapStep(string[] keys)
        {
            Keys = keys;
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public string[] Keys { get; }
    }
}
