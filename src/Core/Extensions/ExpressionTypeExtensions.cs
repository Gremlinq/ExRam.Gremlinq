using System.Linq.Expressions;
using ExRam.Gremlinq.Core.ExpressionParsing;
using System.Diagnostics.CodeAnalysis;

namespace ExRam.Gremlinq.Core
{
    internal static class ExpressionTypeExtensions
    {
        public static bool TryToSemantics(this ExpressionType type, [NotNullWhen(true)] out ExpressionSemantics? semantics)
        {
            semantics = type switch
            {
                ExpressionType.Equal => EqualsExpressionSemantics.Instance,
                ExpressionType.NotEqual => NotEqualsExpressionSemantics.Instance,
                ExpressionType.LessThan => LowerThanExpressionSemantics.Instance,
                ExpressionType.LessThanOrEqual => LowerThanOrEqualExpressionSemantics.Instance,
                ExpressionType.GreaterThanOrEqual => GreaterThanOrEqualExpressionSemantics.Instance,
                ExpressionType.GreaterThan => GreaterThanExpressionSemantics.Instance,
                _ => null
            };

            return semantics is not null;
        }
    }
}
