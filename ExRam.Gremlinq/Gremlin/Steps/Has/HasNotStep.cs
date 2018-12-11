using System.Linq.Expressions;

namespace ExRam.Gremlinq
{
    public sealed class HasNotStep : HasStepBase
    {
        public HasNotStep(Expression expression) : base(expression)
        {
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
