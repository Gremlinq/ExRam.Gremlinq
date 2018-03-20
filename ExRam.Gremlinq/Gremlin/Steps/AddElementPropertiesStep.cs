using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ExRam.Gremlinq
{
    public sealed class AddElementPropertiesStep : NonTerminalGremlinStep
    {
        private static readonly ConcurrentDictionary<Type, PropertyInfo[]> TypeProperties = new ConcurrentDictionary<Type, PropertyInfo[]>();

        public AddElementPropertiesStep(object element)
        {
            this.Element = element;
        }

        public override IEnumerable<TerminalGremlinStep> Resolve(IGraphModel model)
        {
            return TypeProperties.GetOrAdd(
                this.Element.GetType(),
                type => type             
                    .GetProperties()
                    .Where(property => IsNativeType(property.PropertyType))
                    .ToArray())
                .SelectMany(property =>
                {
                    var value = property.GetValue(this.Element);
                    var propertyName = model.GetIdentifier(property.Name);

                    if (value != null)
                    {
                        if (property.PropertyType.IsArray)  //TODO: Other types?
                        {
                            return ((IEnumerable)value)
                                .Cast<object>()
                                .Select(item => new MethodGremlinStep("property", propertyName, item));
                        }

                        return new[] { new MethodGremlinStep("property", propertyName, value) };
                    }

                    return Array.Empty<MethodGremlinStep>();
                });
        }

        private static bool IsNativeType(Type type)   //TODO: Native types are a matter of...what?
        {
            return type.GetTypeInfo().IsValueType || type == typeof(string) || type.IsArray && IsNativeType(type.GetElementType());
        }
        
        public object Element { get; }
    }
}