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
        private readonly ConcurrentDictionary<MemberInfo, MemberMetadata> _members;
        private readonly ConcurrentDictionary<Type, (PropertyInfo propertyInfo, MemberMetadata metadata)[]> _typeProperties;

        private readonly HashSet<Type> _modelTypes;

        public CachingGremlinQueryEnvironmentImpl(IGremlinQueryEnvironment environment)
        {
            InnerEnvironment = environment;
            _members = new();
            _typeProperties = new();

            _modelTypes = environment == GremlinQueryEnvironment.Invalid
                ? []
                : [.. environment.Model.VerticesModel.ElementTypes.Concat(environment.Model.EdgesModel.ElementTypes)];
        }

        private CachingGremlinQueryEnvironmentImpl(
            IGremlinQueryEnvironment environment,
            ConcurrentDictionary<MemberInfo, MemberMetadata> members,
            ConcurrentDictionary<Type, (PropertyInfo propertyInfo, MemberMetadata metadata)[]> typeProperties,
            HashSet<Type> modelTypes)
        {
            InnerEnvironment = environment;
            _members = members;
            _typeProperties = typeProperties;
            _modelTypes = modelTypes;
        }

        public IGremlinQueryEnvironment InnerEnvironment { get; }

        public IGremlinQueryEnvironment ConfigureLogger(Func<ILogger, ILogger> loggerTransformation) => new CachingGremlinQueryEnvironmentImpl(InnerEnvironment.ConfigureLogger(loggerTransformation), _members, _typeProperties, _modelTypes);
        public IGremlinQueryEnvironment ConfigureModel(Func<IGraphModel, IGraphModel> modelTransformation) => InnerEnvironment.ConfigureModel(modelTransformation);
        public IGremlinQueryEnvironment ConfigureFeatureSet(Func<IFeatureSet, IFeatureSet> featureSetTransformation) => new CachingGremlinQueryEnvironmentImpl(InnerEnvironment.ConfigureFeatureSet(featureSetTransformation), _members, _typeProperties, _modelTypes);
        public IGremlinQueryEnvironment ConfigureSerializer(Func<ITransformer, ITransformer> serializerTransformation) => new CachingGremlinQueryEnvironmentImpl(InnerEnvironment.ConfigureSerializer(serializerTransformation), _members, _typeProperties, _modelTypes);
        public IGremlinQueryEnvironment ConfigureOptions(Func<IGremlinqOptions, IGremlinqOptions> optionsTransformation) => new CachingGremlinQueryEnvironmentImpl(InnerEnvironment.ConfigureOptions(optionsTransformation), _members, _typeProperties, _modelTypes);
        public IGremlinQueryEnvironment ConfigureDeserializer(Func<ITransformer, ITransformer> deserializerTransformation) => new CachingGremlinQueryEnvironmentImpl(InnerEnvironment.ConfigureDeserializer(deserializerTransformation), _members, _typeProperties, _modelTypes);
        public IGremlinQueryEnvironment ConfigureNativeTypes(Func<IImmutableSet<Type>, IImmutableSet<Type>> transformation) => new CachingGremlinQueryEnvironmentImpl(InnerEnvironment.ConfigureNativeTypes(transformation), _members, _typeProperties, _modelTypes);
        public IGremlinQueryEnvironment ConfigureDebugger(Func<IGremlinQueryDebugger, IGremlinQueryDebugger> debuggerTransformation) => new CachingGremlinQueryEnvironmentImpl(InnerEnvironment.ConfigureDebugger(debuggerTransformation), _members, _typeProperties, _modelTypes);
        public IGremlinQueryEnvironment ConfigureExecutor(Func<IGremlinQueryExecutor, IGremlinQueryExecutor> executorTransformation) => new CachingGremlinQueryEnvironmentImpl(InnerEnvironment.ConfigureExecutor(executorTransformation), _members, _typeProperties, _modelTypes);

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

        public HashSet<Type> ModelTypes => _modelTypes;
    }
}
