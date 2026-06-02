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
        public static readonly ICachingGremlinQueryEnvironment Invalid = new CachingGremlinQueryEnvironment(GremlinQueryEnvironment.Invalid, new(), new(), []);

        private readonly IGremlinQueryEnvironment _environment;
        private readonly ConcurrentDictionary<MemberInfo, MemberMetadata> _members;
        private readonly ConcurrentDictionary<Type, (PropertyInfo propertyInfo, MemberMetadata metadata)[]> _typeProperties;

        public CachingGremlinQueryEnvironment(IGremlinQueryEnvironment environment) : this(environment, new(), new(), [.. environment.Model.VerticesModel.ElementTypes.Concat(environment.Model.EdgesModel.ElementTypes)])
        {
        }

        private CachingGremlinQueryEnvironment(IGremlinQueryEnvironment environment, ConcurrentDictionary<MemberInfo, MemberMetadata> members, ConcurrentDictionary<Type, (PropertyInfo propertyInfo, MemberMetadata metadata)[]> typeProperties, HashSet<Type> modelTypes)
        {
            _environment = environment;
            _members = members;
            ModelTypes = modelTypes;
            _typeProperties = typeProperties;
        }

        public IGremlinQueryEnvironment ConfigureLogger(Func<ILogger, ILogger> loggerTransformation) => new CachingGremlinQueryEnvironment(_environment.ConfigureLogger(loggerTransformation), _members, _typeProperties, ModelTypes);
        public IGremlinQueryEnvironment ConfigureModel(Func<IGraphModel, IGraphModel> modelTransformation) => _environment.ConfigureModel(modelTransformation);
        public IGremlinQueryEnvironment ConfigureFeatureSet(Func<IFeatureSet, IFeatureSet> featureSetTransformation) => new CachingGremlinQueryEnvironment(_environment.ConfigureFeatureSet(featureSetTransformation), _members, _typeProperties, ModelTypes);
        public IGremlinQueryEnvironment ConfigureSerializer(Func<ITransformer, ITransformer> serializerTransformation) => new CachingGremlinQueryEnvironment(_environment.ConfigureSerializer(serializerTransformation), _members, _typeProperties, ModelTypes);
        public IGremlinQueryEnvironment ConfigureOptions(Func<IGremlinqOptions, IGremlinqOptions> optionsTransformation) => new CachingGremlinQueryEnvironment(_environment.ConfigureOptions(optionsTransformation), _members, _typeProperties, ModelTypes);
        public IGremlinQueryEnvironment ConfigureDeserializer(Func<ITransformer, ITransformer> deserializerTransformation) => new CachingGremlinQueryEnvironment(_environment.ConfigureDeserializer(deserializerTransformation), _members, _typeProperties, ModelTypes);
        public IGremlinQueryEnvironment ConfigureNativeTypes(Func<IImmutableSet<Type>, IImmutableSet<Type>> transformation) => new CachingGremlinQueryEnvironment(_environment.ConfigureNativeTypes(transformation), _members, _typeProperties, ModelTypes);
        public IGremlinQueryEnvironment ConfigureDebugger(Func<IGremlinQueryDebugger, IGremlinQueryDebugger> debuggerTransformation) => new CachingGremlinQueryEnvironment(_environment.ConfigureDebugger(debuggerTransformation), _members, _typeProperties, ModelTypes);
        public IGremlinQueryEnvironment ConfigureExecutor(Func<IGremlinQueryExecutor, IGremlinQueryExecutor> executorTransformation) => new CachingGremlinQueryEnvironment(_environment.ConfigureExecutor(executorTransformation), _members, _typeProperties, ModelTypes);

        public ILogger Logger => _environment.Logger;
        public IGraphModel Model => _environment.Model;
        public IFeatureSet FeatureSet => _environment.FeatureSet;
        public ITransformer Serializer => _environment.Serializer;
        public IGremlinqOptions Options => _environment.Options;
        public ITransformer Deserializer => _environment.Deserializer;
        public IGremlinQueryDebugger Debugger => _environment.Debugger;
        public IGremlinQueryExecutor Executor => _environment.Executor;
        public IImmutableSet<Type> NativeTypes => _environment.NativeTypes;

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
            _environment.Model);

        public HashSet<Type> ModelTypes { get; }
    }
}
