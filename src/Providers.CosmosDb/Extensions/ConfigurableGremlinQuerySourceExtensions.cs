﻿using System.Linq.Expressions;
using System.Reflection;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Core.Serialization;
using ExRam.Gremlinq.Core.Steps;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Providers.Core;

using Gremlin.Net.Driver.Messages;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Providers.CosmosDb
{
    public static class ConfigurableGremlinQuerySourceExtensions
    {
        private sealed class CosmosDbConfigurator<TVertexBase> : ICosmosDbConfigurator<TVertexBase>
        {
            public static readonly CosmosDbConfigurator<TVertexBase> Default = new(null, null, null, null, _ => _, _ => _);

            private readonly string? _authKey;
            private readonly string? _graphName;
            private readonly string? _databaseName;
            private readonly Expression<Func<TVertexBase, object>>? _partitionKeyExpression;
            private readonly Func<IGremlinQuerySource, IGremlinQuerySource> _querySourceTransformation;
            private readonly Func<IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>> _clientFactoryTransformation;

            private CosmosDbConfigurator(string? databaseName, string? graphName, Expression<Func<TVertexBase, object>>? partitionKeyExpression, string? authKey, Func<IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>> clientFactoryTransformation, Func<IGremlinQuerySource, IGremlinQuerySource> querySourceTransformation)
            {
                _authKey = authKey;
                _graphName = graphName;
                _databaseName = databaseName;
                _partitionKeyExpression = partitionKeyExpression;
                _querySourceTransformation = querySourceTransformation;
                _clientFactoryTransformation = clientFactoryTransformation;
            }

            public ICosmosDbConfigurator<TVertexBase> OnDatabase(string databaseName) => new CosmosDbConfigurator<TVertexBase>(
                databaseName,
                _graphName,
                _partitionKeyExpression,
                _authKey,
                _clientFactoryTransformation,
                _querySourceTransformation);

            public ICosmosDbConfigurator<TVertexBase> OnGraph(string graphName) => new CosmosDbConfigurator<TVertexBase>(
                _databaseName,
                graphName,
                _partitionKeyExpression,
                _authKey,
                _clientFactoryTransformation,
                _querySourceTransformation);

            public ICosmosDbConfigurator<TVertexBase> AuthenticateBy(string authKey) => new CosmosDbConfigurator<TVertexBase>(
                _databaseName,
                _graphName,
                _partitionKeyExpression,
                authKey,
                _clientFactoryTransformation,
                _querySourceTransformation);

            public ICosmosDbConfigurator<TVertexBase> ConfigureClientFactory(Func<IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>, IPoolGremlinqClientFactory<IWebSocketGremlinqClientFactory>> transformation) => new CosmosDbConfigurator<TVertexBase>(
                _databaseName,
                _graphName,
                _partitionKeyExpression,
                _authKey,
                _ => transformation(_clientFactoryTransformation(_)),
                _querySourceTransformation);

            public ICosmosDbConfigurator<TVertexBase> ConfigureQuerySource(Func<IGremlinQuerySource, IGremlinQuerySource> transformation) => new CosmosDbConfigurator<TVertexBase>(
                _databaseName,
                _graphName,
                _partitionKeyExpression,
                _authKey,
                _clientFactoryTransformation,
                _ => transformation(_querySourceTransformation(_)));

            public ICosmosDbConfigurator<TVertexBase> WithPartitionKey(Expression<Func<TVertexBase, object>> partitionKeyExpression) => new CosmosDbConfigurator<TVertexBase>(
                _databaseName,
                _graphName,
                partitionKeyExpression,
                _authKey,
                _clientFactoryTransformation,
                _querySourceTransformation);

            public IGremlinQuerySource Transform(IGremlinQuerySource source)
            {
                if (_databaseName is { Length: > 0 } databaseName)
                {
                    if (_graphName is { Length: > 0 } graphName)
                    {
                        if (_partitionKeyExpression is { } partitionKeyExpression)
                        {
                            if (_authKey is { Length: > 0 } authKey)
                            {
                                return _querySourceTransformation
                                    .Invoke(source
                                        .ConfigureEnvironment(environment => environment
                                            .ConfigureModel(model =>
                                            {
                                                var cosmosDbSystemPropertyNames = typeof(SystemProperty)
                                                    .GetFields(BindingFlags.Public | BindingFlags.Static)
                                                    .Select(field => field
                                                        .GetValue(null))
                                                    .OfType<SystemProperty>()
                                                    .Select(property => property.Name)
                                                    .ToArray();

                                                return model
                                                    .ConfigureElements(model => model
                                                        .ConfigureMetadata((memberInfo, metadata) => cosmosDbSystemPropertyNames.Contains(memberInfo.Name, StringComparer.OrdinalIgnoreCase)
                                                            ? new MemberMetadata(metadata.Key, metadata.SerializationBehaviour | SerializationBehaviour.IgnoreAlways)
                                                            : metadata))
                                                    .ConfigureVertices(model => model
                                                        .ConfigureElement<TVertexBase>(conf => conf
                                                            .IgnoreOnUpdate(partitionKeyExpression)));
                                            })
                                            .UseExecutor(_clientFactoryTransformation
                                                .Invoke(WebSocketGremlinqClientFactory.LocalHost
                                                    .WithBinaryMessage<GraphSon2BinaryMessage>()
                                                    .WithPlainCredentials($"/dbs/{databaseName}/colls/{graphName}", authKey)
                                                    .Pool())
                                                .Log()
                                                .ToExecutor())));
                            }

                            throw new InvalidOperationException($"A valid authentication key must be configured. Use {nameof(AuthenticateBy)} on {nameof(ICosmosDbConfigurator<TVertexBase>)} to configure the CosmosDb authentication key.");
                        }

                        throw new InvalidOperationException($"A valid partition key must be configured. Use {nameof(WithPartitionKey)} on {nameof(ICosmosDbConfigurator<TVertexBase>)} to configure a CosmosDb partition key.");
                    }

                    throw new InvalidOperationException($"A valid graph name must be configured. Use {nameof(OnGraph)} on {nameof(ICosmosDbConfigurator<TVertexBase>)} to configure the CosmosDb graph name.");
                }

                throw new InvalidOperationException($"A valid database name must be configured. Use {nameof(OnDatabase)} on {nameof(ICosmosDbConfigurator<TVertexBase>)} to configure the CosmosDb database name.");
            }
        }

        private class WorkaroundOrder : EnumWrapper, IComparator
        {
            public static readonly WorkaroundOrder Incr = new("incr");
            public static readonly WorkaroundOrder Decr = new("decr");

            private WorkaroundOrder(string enumValue)  : base("Order", enumValue)
            {
            }
        }

        private static readonly NotStep NoneWorkaround = new(IdentityStep.Instance);

        public static IGremlinQuerySource UseCosmosDb<TVertexBase, TEdgeBase>(this IGremlinQuerySource source, Func<ICosmosDbConfigurator<TVertexBase>, IGremlinQuerySourceTransformation> configuratorTransformation)
        {
            return configuratorTransformation
                .Invoke(CosmosDbConfigurator<TVertexBase>.Default)
                .Transform(source
                    .ConfigureEnvironment(environment => environment
                        .UseModel(GraphModel
                            .FromBaseTypes<TVertexBase, TEdgeBase>())
                        .ConfigureFeatureSet(featureSet => featureSet
                            .ConfigureGraphFeatures(_ => GraphFeatures.Transactions | GraphFeatures.Persistence | GraphFeatures.ConcurrentAccess)
                            .ConfigureVariableFeatures(_ => VariableFeatures.BooleanValues | VariableFeatures.IntegerValues | VariableFeatures.ByteValues | VariableFeatures.DoubleValues | VariableFeatures.FloatValues | VariableFeatures.IntegerValues | VariableFeatures.LongValues | VariableFeatures.StringValues)
                            .ConfigureVertexFeatures(_ => VertexFeatures.RemoveVertices | VertexFeatures.MetaProperties | VertexFeatures.AddVertices | VertexFeatures.MultiProperties | VertexFeatures.StringIds | VertexFeatures.UserSuppliedIds | VertexFeatures.AddProperty | VertexFeatures.RemoveProperty)
                            .ConfigureVertexPropertyFeatures(_ => VertexPropertyFeatures.StringIds | VertexPropertyFeatures.UserSuppliedIds | VertexPropertyFeatures.RemoveProperty | VertexPropertyFeatures.BooleanValues | VertexPropertyFeatures.ByteValues | VertexPropertyFeatures.DoubleValues | VertexPropertyFeatures.FloatValues | VertexPropertyFeatures.IntegerValues | VertexPropertyFeatures.LongValues | VertexPropertyFeatures.StringValues)
                            .ConfigureEdgeFeatures(_ => EdgeFeatures.AddEdges | EdgeFeatures.RemoveEdges | EdgeFeatures.StringIds | EdgeFeatures.UserSuppliedIds | EdgeFeatures.AddProperty | EdgeFeatures.RemoveProperty)
                            .ConfigureEdgePropertyFeatures(_ => EdgePropertyFeatures.Properties | EdgePropertyFeatures.BooleanValues | EdgePropertyFeatures.ByteValues | EdgePropertyFeatures.DoubleValues | EdgePropertyFeatures.FloatValues | EdgePropertyFeatures.IntegerValues | EdgePropertyFeatures.LongValues | EdgePropertyFeatures.StringValues))
                        .ConfigureOptions(options => options
                            .SetValue(GremlinqOption.WorkaroundRangeInconsistencies, true)
                            .SetValue(GremlinqOption.VertexProjectionSteps, Traversal.Empty)
                            .SetValue(GremlinqOption.EdgeProjectionSteps, Traversal.Empty)
                            .SetValue(GremlinqOption.VertexPropertyProjectionSteps, Traversal.Empty)
                            .ConfigureValue(GremlinqOption.DisabledTextPredicates, option => option | DisabledTextPredicates.Regex | DisabledTextPredicates.NotRegex))
                        .ConfigureNativeTypes(nativeTypes => nativeTypes
                            .Remove(typeof(byte[]))
                            .Remove(typeof(TimeSpan)))
                        .AddGraphSonBinarySupport()
                        .ConfigureSerializer(serializer => serializer
                             .Add(ConverterFactory
                                .Create<Bytecode, RequestMessage>((bytecode, env, _, recurse) => recurse.TryTransform(bytecode, env, out GroovyGremlinScript groovyQuery) && recurse.TryTransform(groovyQuery, env, out RequestMessage? message)
                                    ? message
                                    : null))
                            .Add(ConverterFactory
                                .Create<CosmosDbKey, string>((key, _, _, _) => key.Id))
                            .Add(ConverterFactory
                                .Create<CosmosDbKey, string[]>((key, _, _, _) => key.PartitionKey is { } partitionKey
                                    ? new[] { partitionKey, key.Id }
                                    : default))
                            .Add(ConverterFactory
                                .Create<FilterStep.ByTraversalStep, WhereTraversalStep>(static (step, _, _, _) => new WhereTraversalStep(
                                    step.Traversal.Count > 0 && step.Traversal[0] is AsStep
                                        ? new MapStep(step.Traversal)
                                        : step.Traversal)))
                            .Add(ConverterFactory
                                .Create<HasKeyStep, WhereTraversalStep>((step, _, _, _) => step.Argument is P p && (!p.OperatorName.Equals("eq", StringComparison.OrdinalIgnoreCase))
                                    ? new WhereTraversalStep(Traversal.Empty.Push(
                                        KeyStep.Instance,
                                        new IsStep(p)))
                                    : default))
                            .Add(ConverterFactory
                                .Create<NoneStep, NotStep>((_, _, _, _) => NoneWorkaround))
                            .Add(ConverterFactory
                                .Create<SkipStep, RangeStep>((step, _, _, _) => new RangeStep(step.Count, -1, step.Scope)))
                            .Guard<LimitStep>(step =>
                            {
                                if (step.Count > int.MaxValue)
                                    throw new ArgumentOutOfRangeException(nameof(step), "CosmosDb doesn't currently support values for 'Limit' outside the range of a 32-bit-integer.");
                            })
                            .Guard<TailStep>(step =>
                            {
                                if (step.Count > int.MaxValue)
                                    throw new ArgumentOutOfRangeException(nameof(step), "CosmosDb doesn't currently support values for 'Tail' outside the range of a 32-bit-integer.");
                            })
                            .Guard<RangeStep>(step =>
                            {
                                if (step.Lower > int.MaxValue || step.Upper > int.MaxValue)
                                    throw new ArgumentOutOfRangeException(nameof(step), "CosmosDb doesn't currently support values for 'Range' outside the range of a 32-bit-integer.");
                            })
                            .Add(ConverterFactory
                                .Create<Order, WorkaroundOrder>((order, _, _, _) => order.Equals(Order.Asc)
                                    ? WorkaroundOrder.Incr
                                    : order.Equals(Order.Desc)
                                        ? WorkaroundOrder.Decr
                                        : default)))
                        .ConfigureDeserializer(deserializer => deserializer
                            .AsIncomplete())));
        }
    }
}
