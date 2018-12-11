using System.Linq.Expressions;

namespace ExRam.Gremlinq
{
    public sealed class ValuesStep : Step
    {
        public LambdaExpression[] Projections { get; }

        public ValuesStep(LambdaExpression[] projections)
        {
            Projections = projections;
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
