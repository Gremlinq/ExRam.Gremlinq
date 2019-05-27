using System;
using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Reflection;

namespace ExRam.Gremlinq.Core
{
    public interface IPropertyMetadataConfigurator<TElement> : IImmutableDictionary<MemberInfo, PropertyMetadata>
    {
        IPropertyMetadataConfigurator<TElement> IgnoreOnUpdate<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression);

        IPropertyMetadataConfigurator<TElement> IgnoreAlways<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression);
    }
}
