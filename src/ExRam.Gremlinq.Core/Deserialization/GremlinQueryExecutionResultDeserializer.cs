﻿namespace ExRam.Gremlinq.Core.Deserialization
{
    public static class GremlinQueryExecutionResultDeserializer
    {
        private sealed class GremlinQueryExecutionResultDeserializerImpl : IGremlinQueryExecutionResultDeserializer
        {
            private readonly IGremlinQueryFragmentDeserializer _fragmentSerializer;

            public GremlinQueryExecutionResultDeserializerImpl(IGremlinQueryFragmentDeserializer fragmentSerializer)
            {
                _fragmentSerializer = fragmentSerializer;
            }

            public IAsyncEnumerable<TElement> Deserialize<TElement>(object executionResult, IGremlinQueryEnvironment environment)
            {
                return _fragmentSerializer
                    .TryDeserialize<TElement[]>()
                    .From(executionResult, environment)?.ToAsyncEnumerable() ?? AsyncEnumerable.Empty<TElement>();
            }

            public IGremlinQueryExecutionResultDeserializer ConfigureFragmentDeserializer(Func<IGremlinQueryFragmentDeserializer, IGremlinQueryFragmentDeserializer> transformation)
            {
                return new GremlinQueryExecutionResultDeserializerImpl(transformation(_fragmentSerializer));
            }
        }

        public static readonly IGremlinQueryExecutionResultDeserializer Identity = new GremlinQueryExecutionResultDeserializerImpl(GremlinQueryFragmentDeserializer.Identity);

        public static readonly IGremlinQueryExecutionResultDeserializer Default = Identity
            .ConfigureFragmentDeserializer(static _ => _
                .Override<object>(static (data, type, env, overridden, recurse) =>
                {
                    if (type.IsInstanceOfType(data))
                        return data;

                    if (type.IsArray)
                    {
                        var elementType = type.GetElementType()!;
                        var ret = Array.CreateInstance(elementType, 1);

                        ret
                            .SetValue(recurse.TryDeserialize(elementType).From(data, env), 0);

                        return ret;
                    }

                    return overridden(data, type, env, recurse);
                })
                .AddToStringFallback());
    }
}
