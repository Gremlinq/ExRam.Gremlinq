using System.Linq.Expressions;
using LanguageExt;

namespace ExRam.Gremlinq
{
    public sealed class HasNotStep : HasStepBase
    {
        public HasNotStep(IGraphModel model, Expression expression, Option<object> value = default) : base(model, expression, value)
        {
        }

        public override void Accept(IQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
