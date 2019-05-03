using System;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    public interface IMetadataBuilder<TElement>
    {
        IMetadataBuilder<TElement> IgnoreOnUpdate<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression);

        IMetadataBuilder<TElement> IgnoreAlways<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression);
    }
}
