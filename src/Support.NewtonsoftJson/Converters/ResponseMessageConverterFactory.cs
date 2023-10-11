using System.Diagnostics.CodeAnalysis;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Transformation;

using Gremlin.Net.Driver.Messages;

using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class ResponseMessageConverterFactory : IConverterFactory
    {
        private sealed class ResponseMessageConverter<T> : IConverter<byte[], ResponseMessage<T>>
        {
            private readonly IGremlinQueryEnvironment _environment;

            public ResponseMessageConverter(IGremlinQueryEnvironment environment)
            {
                _environment = environment;
            }

            public bool TryConvert(byte[] source, ITransformer recurse, [NotNullWhen(true)] out ResponseMessage<T>? value)
            {
                var token = recurse
                    .TransformTo<JToken>()
                    .From(source, _environment);

                return recurse.TryTransform(token, _environment, out value);
            }
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment)
        {
            return typeof(TSource) == typeof(byte[]) && typeof(TTarget).IsGenericType && typeof(TTarget).GetGenericTypeDefinition() == typeof(ResponseMessage<>) && typeof(TTarget).GetGenericArguments() is [var dataType]
                ? (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(ResponseMessageConverter<>).MakeGenericType(dataType), environment)
                : default;
        }
    }
}
