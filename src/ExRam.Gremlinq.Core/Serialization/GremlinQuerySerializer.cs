using System;
using System.Collections.Generic;
using System.Linq;

namespace ExRam.Gremlinq.Core.Serialization
{
    public static class GremlinQuerySerializer
    {
        private sealed class GremlinQuerySerializerImpl : IGremlinQuerySerializer
        {
            [ThreadStatic]
            private static Dictionary<StepLabel, string>? _stepLabelNames;

            private static readonly string[] StepLabelNames;

            private readonly IGremlinQueryFragmentSerializer _fragmentSerializer;
            private readonly IGremlinQueryFragmentSerializer _originalFragmentSerializer;

            static GremlinQuerySerializerImpl()
            {
                StepLabelNames = Enumerable.Range(1, 100)
                    .Select(x => "l" + x)
                    .ToArray();
            }

            public GremlinQuerySerializerImpl(IGremlinQueryFragmentSerializer fragmentSerializer)
            {
                _originalFragmentSerializer = fragmentSerializer;

                _fragmentSerializer = fragmentSerializer
                    .Override<StepLabel>((stepLabel, env, _, recurse) =>
                    {
                        if (!_stepLabelNames!.TryGetValue(stepLabel, out var stepLabelMapping))
                        {
                            stepLabelMapping = stepLabel.Identity is string stringIdentity
                                ? stringIdentity
                                : _stepLabelNames.Count < StepLabelNames.Length
                                    ? StepLabelNames[_stepLabelNames.Count]
                                    : "l" + (_stepLabelNames.Count + 1);

                            _stepLabelNames.Add(stepLabel, stepLabelMapping);
                        }

                        // ReSharper disable once TailRecursiveCall
                        return recurse.Serialize(stepLabelMapping!, env);
                    });
            }

            public ISerializedQuery Serialize(IGremlinQueryBase query)
            {
                (_stepLabelNames ??= new Dictionary<StepLabel, string>()).Clear();

                var serialized = _fragmentSerializer
                    .Serialize(query, query.AsAdmin().Environment) ?? throw new ArgumentException($"{nameof(query)} did not serialize to a non-null value.");

                if (serialized is ISerializedQuery serializedQuery)
                    return serializedQuery;

                throw new InvalidOperationException();//TODO: Message
            }

            public IGremlinQuerySerializer ConfigureFragmentSerializer(Func<IGremlinQueryFragmentSerializer, IGremlinQueryFragmentSerializer> transformation)
            {
                return new GremlinQuerySerializerImpl(transformation(_originalFragmentSerializer));
            }
        }

        private sealed class InvalidGremlinQuerySerializer : IGremlinQuerySerializer
        {
            public ISerializedQuery Serialize(IGremlinQueryBase query) => throw new InvalidOperationException($"{nameof(Serialize)} must not be called on {nameof(GremlinQuerySerializer)}.{nameof(Invalid)}. If you are getting this exception while executing a query, configure a proper {nameof(IGremlinQuerySerializer)} on your {nameof(GremlinQuerySource)}.");

            public IGremlinQuerySerializer ConfigureFragmentSerializer(Func<IGremlinQueryFragmentSerializer, IGremlinQueryFragmentSerializer> transformation)
            {
                throw new InvalidOperationException($"{nameof(ConfigureFragmentSerializer)} must not be called on {nameof(GremlinQuerySerializer)}.{nameof(Invalid)}. If you are getting this exception while executing a query, configure a proper {nameof(IGremlinQuerySerializer)} on your {nameof(GremlinQuerySource)}.");
            }
        }

        private sealed class SelectGremlinQuerySerializer : IGremlinQuerySerializer
        {
            private readonly Func<ISerializedQuery, ISerializedQuery> _projection;
            private readonly IGremlinQuerySerializer _baseSerializer;

            public SelectGremlinQuerySerializer(IGremlinQuerySerializer baseSerializer, Func<ISerializedQuery, ISerializedQuery> projection)
            {
                _baseSerializer = baseSerializer;
                _projection = projection;
            }

            public ISerializedQuery Serialize(IGremlinQueryBase query) => _projection(_baseSerializer.Serialize(query));

            public IGremlinQuerySerializer ConfigureFragmentSerializer(Func<IGremlinQueryFragmentSerializer, IGremlinQueryFragmentSerializer> transformation) => new SelectGremlinQuerySerializer(_baseSerializer.ConfigureFragmentSerializer(transformation), _projection);
        }

        public static readonly IGremlinQuerySerializer Invalid = new InvalidGremlinQuerySerializer();

        public static readonly IGremlinQuerySerializer Identity = new GremlinQuerySerializerImpl(GremlinQueryFragmentSerializer.Identity);

        public static readonly IGremlinQuerySerializer Default = Identity.ConfigureFragmentSerializer(fragmentSerializer => fragmentSerializer.UseDefaultGremlinStepSerializationHandlers());

        static GremlinQuerySerializer()
        {
        }

        public static IGremlinQuerySerializer Select(this IGremlinQuerySerializer serializer, Func<ISerializedQuery, ISerializedQuery> projection)
        {
            return new SelectGremlinQuerySerializer(serializer, projection);
        }

        public static IGremlinQuerySerializer ToGroovy(this IGremlinQuerySerializer serializer, GroovyFormatting formatting = GroovyFormatting.WithBindings)
        {
            return serializer
                .Select(serialized => serialized switch
                {
                    GroovyGremlinQuery serializedQuery => formatting == GroovyFormatting.Inline
                        ? serializedQuery.Inline()
                        : serializedQuery,
                    BytecodeGremlinQuery byteCodeQuery => byteCodeQuery.ToGroovy(formatting),
                    _ => throw new NotSupportedException($"Can't convert serialized query of type {serialized.GetType()} to {nameof(GroovyGremlinQuery)}.")
                });
        }
    }
}
