using System;
using System.Collections.Generic;

namespace ExRam.Gremlinq.Core
{
    internal static class TypeExtensions
    {
        public static IEnumerable<Type> GetTypeHierarchy(this Type type)
        {
            var currentType = (Type?)type;

            while (currentType != null)
            {
                yield return currentType;
                currentType = currentType.BaseType;
            }
        }  

        public static QuerySemantics? TryGetQuerySemanticsFromQueryType(this Type type)
        {
            if (typeof(IGremlinQueryBase).IsAssignableFrom(type))
            {
                if (typeof(IVertexPropertyGremlinQueryBase).IsAssignableFrom(type))
                    return QuerySemantics.VertexProperty;

                if (typeof(IPropertyGremlinQueryBase).IsAssignableFrom(type))
                    return QuerySemantics.Property;

                if (typeof(IEdgeGremlinQueryBase).IsAssignableFrom(type))
                    return QuerySemantics.Edge;

                if (typeof(IVertexGremlinQueryBase).IsAssignableFrom(type))
                    return QuerySemantics.Vertex;

                if (typeof(IEdgeOrVertexGremlinQueryBase).IsAssignableFrom(type))
                    return QuerySemantics.EdgeOrVertex;

                if (typeof(IElementGremlinQueryBase).IsAssignableFrom(type))
                    return QuerySemantics.Element;

                if (typeof(IValueTupleGremlinQueryBase).IsAssignableFrom(type) || typeof(IValueGremlinQueryBase).IsAssignableFrom(type))
                    return QuerySemantics.Value;
            }

            return null;
        }
    }
}
