using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ExRam.Gremlinq
{
    public sealed class AddElementPropertiesStep : NonTerminalGremlinStep
    {
        public AddElementPropertiesStep(object element)
        {
            this.Element = element;
        }

        public override IEnumerable<TerminalGremlinStep> Resolve(IGraphModel model)
        {
            return this.Element
                .GetType()
                .GetProperties()
                .Where(property => IsNativeType(property.PropertyType))
                .SelectMany(property =>
                {
                    var value = property.GetValue(this.Element);
                    var propertyName = property.Name == model.IdPropertyName
                        ? (object)new SpecialGremlinString("T.id")
                        : property.Name;

                    if (value != null)
                    {
                        if (property.PropertyType.IsArray)  //TODO: Other types?
                        {
                            return ((IEnumerable)value)
                                .Cast<object>()
                                .Select(item => new TerminalGremlinStep("property", propertyName, item));
                        }

                        return new[] { new TerminalGremlinStep("property", propertyName, value) };
                    }

                    return Array.Empty<TerminalGremlinStep>();
                });
        }

        private static bool IsNativeType(Type type)   //TODO: Native types are a matter of...what?
        {
            return type.GetTypeInfo().IsValueType || type == typeof(string) || type.IsArray && IsNativeType(type.GetElementType());
        }
        
        public object Element { get; }
    }
}