using System.Linq.Expressions;
using ExRam.Gremlinq.Serialization;
using LanguageExt;

namespace ExRam.Gremlinq
{
    public sealed class HasStep : HasStepBase
    {
        public HasStep(IGraphModel model, Expression expression, Option<object> value = default) : base(model, expression, value)
        {
        }

        public override void Accept(IGremlinQueryElementVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
