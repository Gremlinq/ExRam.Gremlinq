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
            var semantics = default(QuerySemantics?);

            if (type.Name.Contains("Query"))
            {
                if (type.Name.Contains("Value"))
                    semantics = QuerySemantics.Value;
                else if (type.Name.Contains("Element"))
                    semantics = QuerySemantics.Element;
                else
                {
                    var containsVertex = type.Name.Contains("Vertex");
                    var containsProperty = type.Name.Contains("Property");

                    if (containsProperty)
                    {
                        semantics = containsVertex
                            ? QuerySemantics.VertexProperty
                            : QuerySemantics.Property;
                    }
                    else
                    {
                        var containsEdge = type.Name.Contains("Edge");

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
