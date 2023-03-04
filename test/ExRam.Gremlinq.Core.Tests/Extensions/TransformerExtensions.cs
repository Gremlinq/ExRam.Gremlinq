using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Transformation;
using Gremlin.Net.Structure.IO.GraphSON;

namespace ExRam.Gremlinq.Core.Tests
{
    public static class TransformerExtensions
    {
        private sealed class SelectSerializer : ITransformer
        {
            private readonly Func<object, object> _projection;
            private readonly ITransformer _baseSerializer;

            public SelectSerializer(ITransformer baseSerializer, Func<object, object> projection)
            {
                _projection = projection;
                _baseSerializer = baseSerializer;
            }

            public ITransformer Add(IConverterFactory converterFactory) => new SelectSerializer(_baseSerializer.Add(converterFactory), _projection);

            public bool TryTransform<TSource, TTarget>(TSource source, IGremlinQueryEnvironment environment, [NotNullWhen(true)] out TTarget? value)
            {
                if (_baseSerializer.TryTransform<TSource, TTarget>(source, environment, out var serialized))
                {
                    if (_projection(serialized) is TTarget target)
                    {
                        value = target;
                        return true;
                    }
                }

                value = default;
                return false;
            }
        }

        public static ITransformer Select(this ITransformer serializer, Func<object, object> projection)
        {
            return new SelectSerializer(serializer, projection);
        }

        public static ITransformer ToGraphsonString(this ITransformer transformer)
        {
            return transformer
                .Add<object, string>(static (data, env, recurse) => new GraphSON2Writer().WriteObject(data));
        }
    }
}
