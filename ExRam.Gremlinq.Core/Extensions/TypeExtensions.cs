using System;
using System.Collections.Generic;

namespace ExRam.Gremlinq.Core
{
    internal static class TypeExtensions
    {
        public static IEnumerable<Type> GetTypeHierarchy(this Type type)
        {
            while (type != null)
            {
                yield return type;
                type = type.BaseType;
            }
        }

        public static QuerySemantics GetQuerySemantics(this Type type)
        {
            var semantics = QuerySemantics.None;
            var containsVertex = type.Name.Contains("Vertex");
            var containsProperty = type.Name.Contains("Property");

            if (containsVertex || containsProperty)
            {
                if (containsVertex && containsProperty)
                    semantics = QuerySemantics.VertexProperty;
                else if (containsVertex)
                    semantics = QuerySemantics.Vertex;
                else
                    semantics = QuerySemantics.Property;
            }
            if (type.Name.Contains("Edge"))
                semantics = QuerySemantics.Edge;

            return semantics;
        }
    }
}
