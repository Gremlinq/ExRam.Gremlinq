using System;
using System.Collections.Immutable;
using System.Reflection;
using ExRam.Gremlinq.Core.GraphElements;
using Microsoft.Extensions.Logging;

namespace ExRam.Gremlinq.Core
{
    public static class GraphModel
    {
        private sealed class GraphModelImpl : IGraphModel
        {
            private static readonly IImmutableSet<Type> DefaultNativeTypes = new[]
            {
                typeof(bool),
                typeof(byte),
                typeof(byte[]),
                typeof(sbyte),
                typeof(short),
                typeof(ushort),
                typeof(int),
                typeof(uint),
                typeof(long),
                typeof(ulong),
                typeof(float),
                typeof(double),
                typeof(string),
                typeof(TimeSpan),
                typeof(DateTime),
                typeof(DateTimeOffset)
            }.ToImmutableHashSet();

            public GraphModelImpl(
                IGraphElementModel verticesModel,
                IGraphElementModel edgesModel,
                IGraphElementPropertyModel propertiesModel) : this(verticesModel, edgesModel, propertiesModel, DefaultNativeTypes)
            {
            }

            private GraphModelImpl(IGraphElementModel verticesModel, IGraphElementModel edgesModel, IGraphElementPropertyModel propertiesModel, IImmutableSet<Type> nativeTypes)
            {
                VerticesModel = verticesModel;
                EdgesModel = edgesModel;
                PropertiesModel = propertiesModel;
                NativeTypes = nativeTypes;
            }

            public IGraphModel ConfigureNativeTypes(Func<IImmutableSet<Type>, IImmutableSet<Type>> transformation)
            {
                return new GraphModelImpl(
                    VerticesModel,
                    EdgesModel,
                    PropertiesModel,
                    transformation(NativeTypes));
            }

            public IGraphModel ConfigureVertices(Func<IGraphElementModel, IGraphElementModel> transformation)
            {
                return new GraphModelImpl(
                    transformation(VerticesModel),
                    EdgesModel,
                    PropertiesModel,
                    NativeTypes);
            }

            public IGraphModel ConfigureEdges(Func<IGraphElementModel, IGraphElementModel> transformation)
            {
                return new GraphModelImpl(
                    VerticesModel,
                    transformation(EdgesModel),
                    PropertiesModel,
                    NativeTypes);
            }

            public IGraphModel ConfigureProperties(Func<IGraphElementPropertyModel, IGraphElementPropertyModel> transformation)
            {
                return new GraphModelImpl(
                    VerticesModel,
                    EdgesModel,
                    transformation(PropertiesModel),
                    NativeTypes);
            }

            public IGraphElementModel VerticesModel { get; }
            public IGraphElementModel EdgesModel { get; }
            public IGraphElementPropertyModel PropertiesModel { get; }
            public IImmutableSet<Type> NativeTypes { get; }
        }

        public static readonly IGraphModel Empty = new GraphModelImpl(
            GraphElementModel.Empty,
            GraphElementModel.Empty,
            GraphElementPropertyModel.Empty);

        public static IGraphModel Default(Func<IAssemblyLookupBuilder, IAssemblyLookupSet> assemblyLookupTransformation)
        {
            return FromBaseTypes<IVertex, IEdge>(assemblyLookupTransformation);
        }

        public static IGraphModel FromBaseTypes<TVertex, TEdge>(Func<IAssemblyLookupBuilder, IAssemblyLookupSet> assemblyLookupTransformation)
        {
            return FromBaseTypes(typeof(TVertex), typeof(TEdge), assemblyLookupTransformation);
        }

        public static IGraphModel FromBaseTypes(Type vertexBaseType, Type edgeBaseType, Func<IAssemblyLookupBuilder, IAssemblyLookupSet> assemblyLookupTransformation)
        {
            if (vertexBaseType.IsAssignableFrom(edgeBaseType))
                throw new ArgumentException($"{vertexBaseType} may not be in the inheritance hierarchy of {edgeBaseType}.");

            if (edgeBaseType.IsAssignableFrom(vertexBaseType))
                throw new ArgumentException($"{edgeBaseType} may not be in the inheritance hierarchy of {vertexBaseType}.");

            var assemblies = assemblyLookupTransformation(new AssemblyLookupSet(new[] { vertexBaseType, edgeBaseType }, ImmutableHashSet<Assembly>.Empty))
                .Assemblies;

            var verticesModel = GraphElementModel.FromBaseType(vertexBaseType, assemblies);
            var edgesModel = GraphElementModel.FromBaseType(edgeBaseType, assemblies);

            return new GraphModelImpl(
                verticesModel,
                edgesModel,
                GraphElementPropertyModel.FromGraphElementModels(verticesModel, edgesModel));
        }

        public static IGraphModel FromTypes(Type[] vertexTypes, Type[] edgeTypes)
        {
            var verticesModel = GraphElementModel.FromTypes(vertexTypes);
            var edgesModel = GraphElementModel.FromTypes(edgeTypes);

            return new GraphModelImpl(
                verticesModel,
                edgesModel,
                GraphElementPropertyModel.FromGraphElementModels(verticesModel, edgesModel));
        }

        public static IGraphModel ConfigureElements(this IGraphModel model, Func<IGraphElementModel, IGraphElementModel> transformation)
        {
            return model
                .ConfigureVertices(transformation)
                .ConfigureEdges(transformation);
        }
    }
}
