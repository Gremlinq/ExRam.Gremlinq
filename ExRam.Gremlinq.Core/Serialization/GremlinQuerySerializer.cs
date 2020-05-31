using System;
using System.Collections.Generic;
using System.Linq;

using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQuerySerializer
    {
        private sealed class GremlinQuerySerializerImpl : IGremlinQuerySerializer
        {
            [ThreadStatic]
            private static Dictionary<StepLabel, string>? _stepLabelNames;

            private static readonly string[] StepLabelNames;

            private readonly IGremlinQueryFragmentSerializer _fragmentSerializer;
            private readonly IGremlinQueryFragmentSerializer _originalfragmentSerializer;

            static GremlinQuerySerializerImpl()
            {
                StepLabelNames = Enumerable.Range(1, 100)
                    .Select(x => "l" + x)
                    .ToArray();
            }

            public GremlinQuerySerializerImpl(IGremlinQueryFragmentSerializer fragmentSerializer)
            {
                _originalfragmentSerializer = fragmentSerializer;

                _fragmentSerializer = fragmentSerializer
                    .Override<StepLabel>((stepLabel, env, @base, recurse) =>
                    {
                        if (!_stepLabelNames!.TryGetValue(stepLabel, out var stepLabelMapping))
                        {
                            stepLabelMapping = _stepLabelNames.Count < StepLabelNames.Length
                                ? StepLabelNames[_stepLabelNames.Count]
                                : "l" + (_stepLabelNames.Count + 1).ToString();

                            _stepLabelNames.Add(stepLabel, stepLabelMapping);
                        }

                        // ReSharper disable once TailRecursiveCall
                        return recurse.Serialize(stepLabelMapping!, env);
                    });
            }

            public object Serialize(IGremlinQueryBase query)
            {
                (_stepLabelNames ??= new Dictionary<StepLabel, string>()).Clear();

                return _fragmentSerializer
                    .Serialize(query, query.AsAdmin().Environment);
            }

            public IGremlinQuerySerializer ConfigureFragmentSerializer(Func<IGremlinQueryFragmentSerializer, IGremlinQueryFragmentSerializer> transformation)
            {
                return new GremlinQuerySerializerImpl(transformation(_originalfragmentSerializer));
            }
        }

        private sealed class InvalidGremlinQuerySerializer : IGremlinQuerySerializer
        {
            public object Serialize(IGremlinQueryBase query) => throw new InvalidOperationException($"{nameof(Serialize)} must not be called on {nameof(GremlinQuerySerializer)}.{nameof(Invalid)}. If you are getting this exception while executing a query, configure a proper {nameof(IGremlinQuerySerializer)} on your {nameof(GremlinQuerySource)}.");

            public IGremlinQuerySerializer ConfigureFragmentSerializer(Func<IGremlinQueryFragmentSerializer, IGremlinQueryFragmentSerializer> transformation)
            {
                throw new InvalidOperationException($"{nameof(ConfigureFragmentSerializer)} must not be called on {nameof(GremlinQuerySerializer)}.{nameof(Invalid)}. If you are getting this exception while executing a query, configure a proper {nameof(IGremlinQuerySerializer)} on your {nameof(GremlinQuerySource)}.");
            }
        }

        private sealed class SelectGremlinQuerySerializer : IGremlinQuerySerializer
        {
            private readonly Func<object, object> _projection;
            private readonly IGremlinQuerySerializer _baseSerializer;

            public SelectGremlinQuerySerializer(IGremlinQuerySerializer baseSerializer, Func<object, object> projection)
            {
                _baseSerializer = baseSerializer;
                _projection = projection;
            }

            public object Serialize(IGremlinQueryBase query) => _projection(_baseSerializer.Serialize(query));

            public IGremlinQuerySerializer ConfigureFragmentSerializer(Func<IGremlinQueryFragmentSerializer, IGremlinQueryFragmentSerializer> transformation) => new SelectGremlinQuerySerializer(_baseSerializer.ConfigureFragmentSerializer(transformation), _projection);
        }

        public static readonly IGremlinQuerySerializer Invalid = new InvalidGremlinQuerySerializer();

        public static readonly IGremlinQuerySerializer Identity = new GremlinQuerySerializerImpl(GremlinQueryFragmentSerializer.Identity);

        public static readonly IGremlinQuerySerializer Default = Identity.ConfigureFragmentSerializer(fragmentSerializer => fragmentSerializer.UseDefaultGremlinStepSerializationHandlers());

        static GremlinQuerySerializer()
        {
        }

        public static IGremlinQuerySerializer Select(this IGremlinQuerySerializer serializer, Func<object, object> projection)
        {
            return new SelectGremlinQuerySerializer(serializer, projection);
        }

        public static IGremlinQuerySerializer ToGroovy(this IGremlinQuerySerializer serializer)
        {
            return serializer
                .Select(serialized =>
                {
                    return serialized switch
                    {
                        GroovyGremlinQuery serializedQuery => serializedQuery,
                        Bytecode bytecode => bytecode.ToGroovy(),
                        _ => throw new NotSupportedException($"Can't convert serialized query of type {serialized.GetType()} to {nameof(GroovyGremlinQuery)}.")
                    };
                });
        }
    }
}
