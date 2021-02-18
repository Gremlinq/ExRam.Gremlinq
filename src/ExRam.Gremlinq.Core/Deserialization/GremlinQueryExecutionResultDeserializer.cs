using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Gremlin.Net.Structure.IO.GraphSON;

namespace ExRam.Gremlinq.Core
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
                var result = _fragmentSerializer
                    .TryDeserialize(executionResult, typeof(TElement[]), environment);

                return result switch
                {
                    TElement[] elements => elements.ToAsyncEnumerable(),
                    IAsyncEnumerable<TElement> enumerable => enumerable,
                    TElement element => new[] { element }.ToAsyncEnumerable(),
                    IEnumerable enumerable => enumerable
                        .Cast<TElement>()
                        .Where(x => x is not null)
                        .Select(x => x!)
                        .ToAsyncEnumerable(),
                    { } obj => throw new InvalidCastException($"A result of type {obj.GetType()} can't be interpreted as {nameof(IAsyncEnumerable<TElement>)}."),
                    _ => AsyncEnumerable.Empty<TElement>()
                };
            }

            public IGremlinQueryExecutionResultDeserializer ConfigureFragmentDeserializer(Func<IGremlinQueryFragmentDeserializer, IGremlinQueryFragmentDeserializer> transformation)
            {
                return new GremlinQueryExecutionResultDeserializerImpl(transformation(_fragmentSerializer));
            }
        }

        private sealed class InvalidQueryExecutionResultDeserializer : IGremlinQueryExecutionResultDeserializer
        {
            public IAsyncEnumerable<TElement> Deserialize<TElement>(object result, IGremlinQueryEnvironment environment)
            {
                throw new InvalidOperationException($"{nameof(Deserialize)} must not be called on {nameof(GremlinQueryExecutionResultDeserializer)}.{nameof(Invalid)}. If you are getting this exception while executing a query, configure a proper {nameof(IGremlinQueryExecutionResultDeserializer)} on your {nameof(GremlinQuerySource)}.");
            }

            public IGremlinQueryExecutionResultDeserializer ConfigureFragmentDeserializer(Func<IGremlinQueryFragmentDeserializer, IGremlinQueryFragmentDeserializer> transformation)
            {
                throw new InvalidOperationException($"{nameof(ConfigureFragmentDeserializer)} cannot be called on {nameof(GremlinQueryExecutionResultDeserializer)}.{nameof(Invalid)}.");
            }
        }

        private sealed class ToStringGremlinQueryExecutionResultDeserializer : IGremlinQueryExecutionResultDeserializer
        {
            public IAsyncEnumerable<TElement> Deserialize<TElement>(object result, IGremlinQueryEnvironment environment)
            {
                return AsyncEnumerable.Create(Core);

                async IAsyncEnumerator<TElement> Core(CancellationToken ct)
                {
                    if (!typeof(TElement).IsAssignableFrom(typeof(string)))
                        throw new InvalidOperationException($"Can't deserialize a string to {typeof(TElement).Name}. Make sure you cast call Cast<string>() on the query before executing it.");

                    if (result.ToString() is { } str)
                        yield return (TElement)(object)str;
                }
            }

            public IGremlinQueryExecutionResultDeserializer ConfigureFragmentDeserializer(Func<IGremlinQueryFragmentDeserializer, IGremlinQueryFragmentDeserializer> transformation)
            {
                throw new InvalidOperationException($"{nameof(ConfigureFragmentDeserializer)} cannot be called on {nameof(GremlinQueryExecutionResultDeserializer)}.{nameof(GremlinQueryExecutionResultDeserializer.ToString)}.");
            }
        }

        private sealed class ToGraphsonGremlinQueryExecutionResultDeserializer : IGremlinQueryExecutionResultDeserializer
        {
            public IAsyncEnumerable<TElement> Deserialize<TElement>(object result, IGremlinQueryEnvironment environment)
            {
                return AsyncEnumerable.Create(Core);

                async IAsyncEnumerator<TElement> Core(CancellationToken ct)
                {
                    if (!typeof(TElement).IsAssignableFrom(typeof(string)))
                        throw new InvalidOperationException($"Can't deserialize a string to {typeof(TElement).Name}. Make sure you cast call Cast<string>() on the query before executing it.");

                    yield return (TElement)(object)new GraphSON2Writer().WriteObject(result);
                }
            }

            public IGremlinQueryExecutionResultDeserializer ConfigureFragmentDeserializer(Func<IGremlinQueryFragmentDeserializer, IGremlinQueryFragmentDeserializer> transformation)
            {
                throw new InvalidOperationException($"{nameof(ConfigureFragmentDeserializer)} cannot be called on {nameof(GremlinQueryExecutionResultDeserializer)}.{nameof(ToGraphsonString)}.");
            }
        }

        public static readonly IGremlinQueryExecutionResultDeserializer Identity = new GremlinQueryExecutionResultDeserializerImpl(GremlinQueryFragmentDeserializer.Identity);

        public static readonly IGremlinQueryExecutionResultDeserializer Invalid = new InvalidQueryExecutionResultDeserializer();

        public static readonly IGremlinQueryExecutionResultDeserializer ToGraphsonString = new ToGraphsonGremlinQueryExecutionResultDeserializer();

        public static new readonly IGremlinQueryExecutionResultDeserializer ToString = new ToStringGremlinQueryExecutionResultDeserializer();

        // ReSharper disable ConvertToLambdaExpression
        [Obsolete("Use GremlinQueryExecutionResultDeserializer.Identity.ConfigureFragmentDeserializer(_ => _.AddNewtonsoftJson()) instead.")]
        public static readonly IGremlinQueryExecutionResultDeserializer FromJToken = Identity
            .ConfigureFragmentDeserializer(_ => _
                .AddNewtonsoftJson());
    }
}
