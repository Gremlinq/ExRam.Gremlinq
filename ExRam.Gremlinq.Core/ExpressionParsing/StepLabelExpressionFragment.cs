using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    internal class StepLabelExpressionFragment : ConstantExpressionFragment
    {
        public StepLabelExpressionFragment(StepLabel stepLabel, Expression? expression = default) : base(stepLabel, expression)
        {
        }
    }
}