using ExRam.Gremlinq.Core.Serialization;

namespace ExRam.Gremlinq.Core
{
    public sealed class CountStep : Step
    {
        public static readonly CountStep Global = new CountStep(Scope.Global);
        public static readonly CountStep Local = new CountStep(Scope.Local);

        public CountStep(Scope scope)
        {
            Scope = scope;
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public Scope Scope { get; }
    }
}
