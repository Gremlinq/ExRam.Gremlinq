using System.Reflection;
using ExRam.Gremlinq.Serialization;

namespace ExRam.Gremlinq
{
    public sealed class PropertiesStep : Step
    {
        public PropertiesStep(MemberInfo[] members)
        {
            Members = members;
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public MemberInfo[] Members { get; }
    }
}
