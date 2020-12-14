using System;
using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Reflection;

namespace ExRam.Gremlinq.Core
{
    public interface IMemberMetadataConfigurator<TElement> : IImmutableDictionary<MemberInfo, MemberMetadata>
    {
        IMemberMetadataConfigurator<TElement> IgnoreOnAdd<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression);

        IMemberMetadataConfigurator<TElement> IgnoreOnUpdate<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression);

        IMemberMetadataConfigurator<TElement> IgnoreAlways<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression);

        IMemberMetadataConfigurator<TElement> ConfigureName<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression, string name);
    }
}
