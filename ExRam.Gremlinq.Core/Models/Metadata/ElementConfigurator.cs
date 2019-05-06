using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Reflection;

namespace ExRam.Gremlinq.Core
{
    internal class ElementConfigurator<TElement> : IElementConfigurator<TElement>
    {
        public ElementConfigurator(IImmutableDictionary<MemberInfo, MemberMetadata> metaData)
        {
            MetaData = metaData;
        }

        public IElementConfigurator<TElement> IgnoreOnUpdate<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression)
        {
            MetaData = MetaData.SetItem(propertyExpression.GetPropertyAccess(), new MemberMetadata(IgnoreDirective.OnUpdate));

            return this;
        }

        public IElementConfigurator<TElement> IgnoreAlways<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression)
        {
            MetaData = MetaData.SetItem(propertyExpression.GetPropertyAccess(), new MemberMetadata(IgnoreDirective.Always));

            return this;
        }

        public IImmutableDictionary<MemberInfo, MemberMetadata> MetaData;
    }
}
