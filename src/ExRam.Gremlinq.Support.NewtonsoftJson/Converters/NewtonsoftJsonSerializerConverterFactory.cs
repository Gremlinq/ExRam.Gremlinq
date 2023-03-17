using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class NewtonsoftJsonSerializerConverterFactory : IConverterFactory
    {
        private sealed class NewtonsoftJsonSerializerConverter<TSource, TTarget> : IConverter<TSource, TTarget>
            where TSource : JToken
        {
            private readonly IGremlinQueryEnvironment _environment;

            public NewtonsoftJsonSerializerConverter(IGremlinQueryEnvironment environment)
            {
                _environment = environment;
            }

            public bool TryConvert(TSource source, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                if (source is TTarget alreadyRequestedValue)
                {
                    value = alreadyRequestedValue;
                    return true;
                }

                if (source.ToObject<TTarget>(_environment.GetJsonSerializer(recurse)) is { } requestedValue)
                {
                    value = requestedValue;
                    return true;
                }

                value = default;
                return false;
            }
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment)
        {
            return typeof(JToken).IsAssignableFrom(typeof(TSource))
                ? (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(NewtonsoftJsonSerializerConverter<,>).MakeGenericType(typeof(TSource), typeof(TTarget)), environment)
                : null;
        }
    }
}
