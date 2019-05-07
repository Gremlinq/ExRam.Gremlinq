using System;
using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Reflection;

namespace ExRam.Gremlinq.Core
{
    public interface IPropertyMetadataBuilder<TElement> : IImmutableDictionary<MemberInfo, PropertyMetadata>
    {
        IPropertyMetadataBuilder<TElement> IgnoreOnUpdate<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression);

        IPropertyMetadataBuilder<TElement> IgnoreAlways<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression);
    }
}
