using System;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    public static class ProjectBuilderExtensions
    {
        public static IProjectDynamicBuilder<TSourceQuery, TElement> By<TSourceQuery, TElement>(this IProjectDynamicBuilder<TSourceQuery, TElement> projectBuilder, Expression<Func<TElement, object>> projection)
            where TSourceQuery : IElementGremlinQuery<TElement>
        {
            if (projection.Body.StripConvert() is MemberExpression memberExpression)
                return projectBuilder.By(memberExpression.Member.Name, projection);

            throw new ExpressionNotSupportedException(projection);
        }

        public static IProjectDynamicBuilder<TSourceQuery, TElement> By<TSourceQuery, TElement>(this IProjectDynamicBuilder<TSourceQuery, TElement> projectBuilder, string name, Expression<Func<TElement, object>> projection)
            where TSourceQuery : IElementGremlinQuery<TElement>
        {
            return projectBuilder.By(name, _ => _.Values(projection));
        }
    }
}
