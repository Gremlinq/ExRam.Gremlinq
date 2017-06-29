using System;
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
                .Select(property => (name: property.Name, value: property.GetValue(this.Element)))
                .Where(tuple => tuple.value != null)
                .Select(tuple => new TerminalGremlinStep("property", tuple.name, tuple.value));
        }

        private static bool IsNativeType(Type type)   //TODO: Native types are a matter of...what?
        {
            return type.GetTypeInfo().IsValueType || type == typeof(string) || type.IsArray && IsNativeType(type.GetElementType());
        }
        
        public object Element { get; }
    }
}