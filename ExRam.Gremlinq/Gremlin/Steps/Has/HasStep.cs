using System.Linq.Expressions;
using LanguageExt;

namespace ExRam.Gremlinq
{
    public sealed class HasStep : HasStepBase
    {
        public HasStep(Expression expression, Option<object> value = default) : base("has", expression, value)
        {
        }
    }
}
