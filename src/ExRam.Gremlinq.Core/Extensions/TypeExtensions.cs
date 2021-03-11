using System;
using System.Collections.Generic;

namespace ExRam.Gremlinq.Core
{
    public static class TypeExtensions
    {
        internal static IEnumerable<Type> GetTypeHierarchy(this Type type)
        {
            var currentType = (Type?)type;

            while (currentType != null)
            {
                yield return currentType;
                currentType = currentType.BaseType;
            }
        }

        public static QuerySemantics? TryGetQuerySemantics(this Type type)
        {
            var typeName = type.Name;
            var semantics = default(QuerySemantics?);

            if (typeName.Contains("Query"))
            {
                if (typeName.Contains("Value"))
                    semantics = QuerySemantics.Value;
                else if (typeName.Contains("Element"))
                    semantics = QuerySemantics.Element;
                else
                {
                    var containsVertex = typeName.Contains("Vertex");
                    var containsProperty = typeName.Contains("Property");

                    if (containsProperty)
                    {
                        semantics = containsVertex
                            ? QuerySemantics.VertexProperty
                            : QuerySemantics.Property;
                    }
                    else
                    {
                        var containsEdge = typeName.Contains("Edge");

                        if (containsVertex)
                        {
                            semantics = containsEdge
                                ? QuerySemantics.EdgeOrVertex
                                : QuerySemantics.Vertex;
                        }
                        else if (containsEdge)
                            semantics = QuerySemantics.Edge;
                    }
                }
            }

            return semantics;
        }
    }
}
