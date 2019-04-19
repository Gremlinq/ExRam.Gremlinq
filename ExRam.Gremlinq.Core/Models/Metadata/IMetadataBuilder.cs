using System;
using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    public interface IMetadataBuilder<TElement>
    {
        IMetadataBuilder<TElement> ReadOnly<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression);

        IMetadataBuilder<TElement> Ignored<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression);
    }
}
