using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class MetaPropertiesStep : Step
    {
        public MetaPropertiesStep(string[] keys)
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
