using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Reflection;
using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Core.Transformation;
using Microsoft.Extensions.Logging;

namespace ExRam.Gremlinq.Core
{
    internal sealed class CachingGremlinQueryEnvironment : ICachingGremlinQueryEnvironment
    {
        private readonly ConcurrentDictionary<MemberInfo, MemberMetadata> _members;
        private readonly ConcurrentDictionary<Type, (PropertyInfo propertyInfo, MemberMetadata metadata)[]> _typeProperties;

        public CachingGremlinQueryEnvironment(IGremlinQueryEnvironment environment) : this(
            environment,
            new(),
            new(),
            environment != GremlinQueryEnvironment.Invalid
                ? [.. environment.Model.VerticesModel.ElementTypes.Concat(environment.Model.EdgesModel.ElementTypes)]
                : [])
        {
        }

        private CachingGremlinQueryEnvironment(IGremlinQueryEnvironment environment, ConcurrentDictionary<MemberInfo, MemberMetadata> members, ConcurrentDictionary<Type, (PropertyInfo propertyInfo, MemberMetadata metadata)[]> typeProperties, HashSet<Type> modelTypes)
        {
            _members = members;
            ModelTypes = modelTypes;
            InnerEnvironment = environment;
            _typeProperties = typeProperties;
        }

        public IGremlinQueryEnvironment InnerEnvironment { get; }

        public IGremlinQueryEnvironment ConfigureLogger(Func<ILogger, ILogger> loggerTransformation) => new CachingGremlinQueryEnvironment(InnerEnvironment.ConfigureLogger(loggerTransformation), _members, _typeProperties, ModelTypes);
        public IGremlinQueryEnvironment ConfigureModel(Func<IGraphModel, IGraphModel> modelTransformation) => InnerEnvironment.ConfigureModel(modelTransformation);
        public IGremlinQueryEnvironment ConfigureFeatureSet(Func<IFeatureSet, IFeatureSet> featureSetTransformation) => new CachingGremlinQueryEnvironment(InnerEnvironment.ConfigureFeatureSet(featureSetTransformation), _members, _typeProperties, ModelTypes);
        public IGremlinQueryEnvironment ConfigureSerializer(Func<ITransformer, ITransformer> serializerTransformation) => new CachingGremlinQueryEnvironment(InnerEnvironment.ConfigureSerializer(serializerTransformation), _members, _typeProperties, ModelTypes);
        public IGremlinQueryEnvironment ConfigureOptions(Func<IGremlinqOptions, IGremlinqOptions> optionsTransformation) => new CachingGremlinQueryEnvironment(InnerEnvironment.ConfigureOptions(optionsTransformation), _members, _typeProperties, ModelTypes);
        public IGremlinQueryEnvironment ConfigureDeserializer(Func<ITransformer, ITransformer> deserializerTransformation) => new CachingGremlinQueryEnvironment(InnerEnvironment.ConfigureDeserializer(deserializerTransformation), _members, _typeProperties, ModelTypes);
        public IGremlinQueryEnvironment ConfigureNativeTypes(Func<IImmutableSet<Type>, IImmutableSet<Type>> transformation) => new CachingGremlinQueryEnvironment(InnerEnvironment.ConfigureNativeTypes(transformation), _members, _typeProperties, ModelTypes);
        public IGremlinQueryEnvironment ConfigureDebugger(Func<IGremlinQueryDebugger, IGremlinQueryDebugger> debuggerTransformation) => new CachingGremlinQueryEnvironment(InnerEnvironment.ConfigureDebugger(debuggerTransformation), _members, _typeProperties, ModelTypes);
        public IGremlinQueryEnvironment ConfigureExecutor(Func<IGremlinQueryExecutor, IGremlinQueryExecutor> executorTransformation) => new CachingGremlinQueryEnvironment(InnerEnvironment.ConfigureExecutor(executorTransformation), _members, _typeProperties, ModelTypes);

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

        public HashSet<Type> ModelTypes { get; }
    }
}
