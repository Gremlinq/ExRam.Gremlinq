using System;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    public interface IElementConfigurator<TElement>
    {
        IElementConfigurator<TElement> IgnoreOnUpdate<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression);

        IElementConfigurator<TElement> IgnoreAlways<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression);
    }
}
