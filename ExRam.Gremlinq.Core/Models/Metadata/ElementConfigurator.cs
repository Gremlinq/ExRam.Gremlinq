using System;
using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Reflection;
using LanguageExt;

namespace ExRam.Gremlinq.Core
{
    internal class ElementConfigurator<TElement> : IElementConfigurator<TElement>
    {
        public ElementConfigurator(IImmutableDictionary<MemberInfo, PropertyMetadata> metadata)
        {
            Metadata = metadata;
        }

        public IElementConfigurator<TElement> IgnoreOnUpdate<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression)
        {
            return Set(propertyExpression, SerializationDirective.IgnoreOnUpdate);
        }

        public IElementConfigurator<TElement> IgnoreAlways<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression)
        {
            return Set(propertyExpression, SerializationDirective.IgnoreAlways);
        }

        public IElementConfigurator<TElement> Set<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression, SerializationDirective newDirective)
        {
            var property = propertyExpression.GetPropertyAccess();

            Metadata = Metadata.SetItem(
                property,
                Metadata
                    .TryGetValue(property)
                    .Map(metaData => new PropertyMetadata(metaData.IdentifierOverride, newDirective))
                    .IfNone(new PropertyMetadata(default, newDirective)));

            return this;
        }

        public IImmutableDictionary<MemberInfo, PropertyMetadata> Metadata;
    }
}
