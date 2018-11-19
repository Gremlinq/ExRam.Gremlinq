using System.Linq.Expressions;

namespace ExRam.Gremlinq
{
    public sealed class HasNotStep : HasStepBase
    {
        public HasNotStep(Expression expression) : base("hasNot", expression)
        {
        }
    }
}
