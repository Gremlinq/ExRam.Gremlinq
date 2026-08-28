using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.GraphElements;
using System.Runtime.CompilerServices;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class PropertyHeuristicConverterFactory : IConverterFactory
    {
        private sealed class PropertyHeuristicConverter<TTarget> : IConverter<JObject, TTarget>
        {
            private readonly IGremlinQueryEnvironment _environment;

            public PropertyHeuristicConverter(IGremlinQueryEnvironment environment)
            {
                _environment = environment;
            }

            bool IConverter<JObject, TTarget>.TryConvert(JObject jObject, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                ArgumentNullException.ThrowIfNull(jObject);
                ArgumentNullException.ThrowIfNull(defer);
                ArgumentNullException.ThrowIfNull(recurse);

                if (jObject.LooksLikeVertexProperty())
                {
                    if (recurse.TryTransform(jObject, _environment, out VertexProperty<object>? vProp) && vProp is TTarget target)
                    {
                        value = target;
                        return true;
                    }
                }
                else if (jObject.LooksLikeProperty())
                {
                    if (recurse.TryTransform(jObject, _environment, out Property<object>? prop) && prop is TTarget target)
                    {
                        value = target;
                        return true;
                    }
                }

                value = default;
                return false;
            }
        }

        IConverter<TSource, TTarget>? IConverterFactory.TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment)
        {
            ArgumentNullException.ThrowIfNull(environment);

            return typeof(JObject) == typeof(TSource) && typeof(TTarget).IsAssignableFrom(typeof(VertexProperty<object>)) && !typeof(Property<object>).IsAssignableFrom(typeof(TTarget))
                ? Unsafe.As<IConverter<TSource, TTarget>>(new PropertyHeuristicConverter<TTarget>(environment))
                : null;
        }
    }
}
