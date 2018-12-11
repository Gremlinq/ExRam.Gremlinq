using System.Reflection;

namespace ExRam.Gremlinq
{
    public sealed class PropertiesStep : Step
    {
        public PropertiesStep(MemberInfo[] members)
        {
            Members = members;
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public MemberInfo[] Members { get; }
    }
}
