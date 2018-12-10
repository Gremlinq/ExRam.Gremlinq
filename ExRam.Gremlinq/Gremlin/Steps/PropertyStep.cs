using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ExRam.Gremlinq
{
    public sealed class PropertyStep : NonTerminalStep
    {
        private readonly object _value;
        private readonly PropertyInfo _property;
        private readonly MemberExpression _memberExpression;

        public PropertyStep(PropertyInfo property, object value)
        {
            _value = value;
            _property = property;
        }

        public PropertyStep(MemberExpression memberExpression, object value)
        {
            _value = value;
            _memberExpression = memberExpression;
        }

        public override IEnumerable<Step> Resolve(IGraphModel model)
        {
            if (_value != null)
            {
                var type = _property?.PropertyType ?? _memberExpression.Type;
                var name = model.GetIdentifier(_property?.Name ?? _memberExpression.Member.Name);

                if (!type.IsArray || type == typeof(byte[]))
                {
                    yield return Resolve(Cardinality.Single, name, _value);
                }
                else
                {
                    if (type.GetElementType().IsInstanceOfType(_value))
                        yield return Resolve(Cardinality.List, name, _value);
                    else
                    {
                        foreach (var item in (IEnumerable)_value)
                        {
                            yield return Resolve(Cardinality.List, name, item);
                        }
                    }
                }
            }
        }

        private static MethodStep Resolve(Cardinality cardinality, object name, object value)
        {
            if (value is IMeta meta)
            {
                var metaProperties = meta.Properties
                    .SelectMany(kvp => new[] {kvp.Key, kvp.Value})
                    .Prepend(meta.Value)
                    .Prepend(name)
                    .Prepend(cardinality);

                return MethodStep.Create("property", metaProperties);
            }

            return MethodStep.Create("property", cardinality, name, value);
        }
    }
}
