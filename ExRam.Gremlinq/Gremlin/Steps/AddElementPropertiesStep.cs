using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace ExRam.Gremlinq
{
    public sealed class AddElementPropertiesStep : NonTerminalStep
    {
        private static readonly ConcurrentDictionary<Type, PropertyInfo[]> TypeProperties = new ConcurrentDictionary<Type, PropertyInfo[]>();

        public AddElementPropertiesStep(object element)
        {
            Element = element;
        }

        public override IEnumerable<TerminalStep> Resolve(IGraphModel model)
        {
            return TypeProperties.GetOrAdd(
                Element.GetType(),
                type => type             
                    .GetProperties()
                    .Where(property => IsMetaType(property.PropertyType) ||  IsNativeType(property.PropertyType))
                    .ToArray())
                .SelectMany(property =>
                {
                    var value = property.GetValue(Element);
                    var propertyName = model.GetIdentifier(property.Name);

                    if (value != null)
                    {
                        if (property.PropertyType.IsArray)  //TODO: Other types?
                        {
                            return ((IEnumerable)value)
                                .Cast<object>()
                                .Select(item => new MethodStep("property", propertyName, item));
                        }

                        if (value is IMeta meta)
                        {
                            var metaProperties = meta.Properties
                                .SelectMany(kvp => new[] {kvp.Key, kvp.Value})
                                .Prepend(meta.Value)
                                .Prepend(propertyName)
                                .ToImmutableList();

                            return new[] { new MethodStep("property", metaProperties) };
                        }

                        return new[] { new MethodStep("property", propertyName, value) };
                    }

                    return Array.Empty<MethodStep>();
                });
        }

        private static bool IsNativeType(Type type)   //TODO: Native types are a matter of...what?
        {
            return type.GetTypeInfo().IsValueType || type == typeof(string) || type.IsArray && IsNativeType(type.GetElementType());
        }

        private static bool IsMetaType(Type type)
        {
            return typeof(IMeta).IsAssignableFrom(type);
        }

        public object Element { get; }
    }
}
