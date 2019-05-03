using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace ExRam.Gremlinq.Core
{
    internal class ElementConfigurator<TElement> : IElementConfigurator<TElement>
    {
        public IElementConfigurator<TElement> IgnoreOnUpdate<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression)
        {
            UpdateSemantics[propertyExpression.GetPropertyAccess()] = IgnoreDirective.OnUpdate;

            return this;
        }

        public IElementConfigurator<TElement> IgnoreAlways<TProperty>(Expression<Func<TElement, TProperty>> propertyExpression)
        {
            UpdateSemantics[propertyExpression.GetPropertyAccess()] = IgnoreDirective.Always;

            return this;
        }

        public Dictionary<PropertyInfo, IgnoreDirective> UpdateSemantics { get; } = new Dictionary<PropertyInfo, IgnoreDirective>();
    }
}
