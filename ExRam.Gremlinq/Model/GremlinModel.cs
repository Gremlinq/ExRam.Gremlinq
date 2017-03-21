using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace ExRam.Gremlinq
{
    public static class GremlinModel
    {
        private sealed class AssemblyGremlinModel : IGremlinModel
        {
            public AssemblyGremlinModel(Assembly assembly, Type vertexBaseType, Type edgeBaseType)
            {
                this.VertexTypes = assembly.DefinedTypes
                    .Select(typeInfo => typeInfo.AsType())
                    .Where(vertexBaseType.IsAssignableFrom)
                    .ToImmutableList();

                this.EdgeTypes = assembly.DefinedTypes
                    .Select(typeInfo => typeInfo.AsType())
                    .Where(edgeBaseType.IsAssignableFrom)
                    .ToImmutableList();
            }

            public IImmutableList<Type> EdgeTypes { get; }
            public IImmutableList<Type> VertexTypes { get; }
        }

        private sealed class EmptyGremlinModel : IGremlinModel
        {
            public IImmutableList<Type> EdgeTypes => ImmutableList<Type>.Empty;
            public IImmutableList<Type> VertexTypes => ImmutableList<Type>.Empty;
        }

        public static readonly IGremlinModel Empty = new EmptyGremlinModel();

        public static IGremlinModel FromAssembly(Assembly assembly, Type vertexBaseType, Type edgeBaseType)
        {
            return new AssemblyGremlinModel(assembly, vertexBaseType, edgeBaseType);
        }

        public static IGremlinModel AddVertexType<T>(this IGremlinModel model)
        {
            throw new NotImplementedException();
        }

        public static IGremlinModel AddEdgeType<T>(this IGremlinModel model)
        {
            throw new NotImplementedException();
        }
    }
}