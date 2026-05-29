using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Reflection;
using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Core.Transformation;
using Microsoft.Extensions.Logging;

namespace ExRam.Gremlinq.Core
{
    internal sealed class CachingGremlinQueryEnvironmentImpl : ICachingGremlinQueryEnvironment
    {
        private readonly ConcurrentDictionary<MemberInfo, MemberMetadata> _members = new();
        private readonly ConcurrentDictionary<Type, (PropertyInfo propertyInfo, MemberMetadata metadata)[]> _typeProperties = new();

        private readonly Lazy<HashSet<Type>> _modelTypes;

        public CachingGremlinQueryEnvironmentImpl(IGremlinQueryEnvironment environment)
        {
            InnerEnvironment = environment;

            _modelTypes = new Lazy<HashSet<Type>>(() => new HashSet<Type>(environment.Model
                .VerticesModel.ElementTypes
                .Concat(environment.Model.EdgesModel.ElementTypes)));
        }

        public IGremlinQueryEnvironment InnerEnvironment { get; }

        public IGremlinQueryEnvironment ConfigureLogger(Func<ILogger, ILogger> loggerTransformation) => InnerEnvironment.ConfigureLogger(loggerTransformation);
        public IGremlinQueryEnvironment ConfigureModel(Func<IGraphModel, IGraphModel> modelTransformation) => InnerEnvironment.ConfigureModel(modelTransformation);
        public IGremlinQueryEnvironment ConfigureFeatureSet(Func<IFeatureSet, IFeatureSet> featureSetTransformation) => InnerEnvironment.ConfigureFeatureSet(featureSetTransformation);
        public IGremlinQueryEnvironment ConfigureSerializer(Func<ITransformer, ITransformer> serializerTransformation) => InnerEnvironment.ConfigureSerializer(serializerTransformation);
        public IGremlinQueryEnvironment ConfigureOptions(Func<IGremlinqOptions, IGremlinqOptions> optionsTransformation) => InnerEnvironment.ConfigureOptions(optionsTransformation);
        public IGremlinQueryEnvironment ConfigureDeserializer(Func<ITransformer, ITransformer> deserializerTransformation) => InnerEnvironment.ConfigureDeserializer(deserializerTransformation);
        public IGremlinQueryEnvironment ConfigureNativeTypes(Func<IImmutableSet<Type>, IImmutableSet<Type>> transformation) => InnerEnvironment.ConfigureNativeTypes(transformation);
        public IGremlinQueryEnvironment ConfigureDebugger(Func<IGremlinQueryDebugger, IGremlinQueryDebugger> debuggerTransformation) => InnerEnvironment.ConfigureDebugger(debuggerTransformation);
        public IGremlinQueryEnvironment ConfigureExecutor(Func<IGremlinQueryExecutor, IGremlinQueryExecutor> executorTransformation) => InnerEnvironment.ConfigureExecutor(executorTransformation);

        public ILogger Logger => InnerEnvironment.Logger;
        public IGraphModel Model => InnerEnvironment.Model;
        public IFeatureSet FeatureSet => InnerEnvironment.FeatureSet;
        public ITransformer Serializer => InnerEnvironment.Serializer;
        public IGremlinqOptions Options => InnerEnvironment.Options;
        public ITransformer Deserializer => InnerEnvironment.Deserializer;
        public IGremlinQueryDebugger Debugger => InnerEnvironment.Debugger;
        public IGremlinQueryExecutor Executor => InnerEnvironment.Executor;
        public IImmutableSet<Type> NativeTypes => InnerEnvironment.NativeTypes;

        public (PropertyInfo propertyInfo, MemberMetadata metadata)[] GetSerializationData(Type type) => _typeProperties
            .GetOrAdd(
                type,
                static (closureType, self) => closureType
                    .GetSerializableProperties()
                    .Select(p => (
                        property: p,
                        metadata: self.GetMetadata(p)))
                    .OrderBy(static x => x.metadata.Key)
                    .ToArray(),
                this);

        public MemberMetadata GetMetadata(MemberInfo member) => _members.GetOrAdd(
            member,
            static (closureMember, model) => model.VerticesModel.TryGetMetadata(closureMember) ?? model.EdgesModel.TryGetMetadata(closureMember) ?? MemberMetadata.Default(closureMember.Name),
            InnerEnvironment.Model);

        public HashSet<Type> ModelTypes => _modelTypes.Value;
    }
}
