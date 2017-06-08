using System;

namespace ExRam.Gremlinq
{
    public class PropertySchemaInfo
    {
        public PropertySchemaInfo(string name, Type type)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; }
        public Type Type { get; }
    }
}