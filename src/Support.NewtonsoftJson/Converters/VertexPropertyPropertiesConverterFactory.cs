using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class VertexPropertyPropertiesConverterFactory : IConverterFactory
    {
        private sealed class VertexPropertyPropertiesConverter<TSource, TOption> : IConverter<TSource, VertexPropertyPropertiesWrapper<TOption>>
            where TSource : JToken
        {
            private readonly IGremlinQueryEnvironment _environment;

            public VertexPropertyPropertiesConverter(IGremlinQueryEnvironment environment)
            {
                _environment = environment;
            }

            public bool TryConvert(TSource source, ITransformer recurse, [NotNullWhen(true)] out VertexPropertyPropertiesWrapper<TOption> value)
            {
                if (source is JObject { Count: 0 })
                {
                    value = VertexPropertyPropertiesWrapper<TOption>.None;
                    return true;
                }

                if (recurse.TryTransform(source, _environment, out TOption? option))
                {
                    value = VertexPropertyPropertiesWrapper<TOption>.From(option);
                    return true;
                }

                value = default;
                return false;
            }
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment)
        {
            return typeof(JToken).IsAssignableFrom(typeof(TSource)) && typeof(TTarget).IsGenericType && typeof(TTarget).GetGenericTypeDefinition() == typeof(VertexPropertyPropertiesWrapper<>)
                ? (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(VertexPropertyPropertiesConverter<,>).MakeGenericType(typeof(TSource), typeof(TTarget).GenericTypeArguments[0]), environment)
                : null;
        }
    }
}
