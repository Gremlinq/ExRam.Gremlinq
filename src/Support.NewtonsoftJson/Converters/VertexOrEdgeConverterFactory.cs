using Newtonsoft.Json.Linq;
using ExRam.Gremlinq.Core.GraphElements;
using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class VertexOrEdgeConverterFactory : IConverterFactory
    {
        private sealed class VertexOrEdgeConverter<TTarget> : IConverter<JObject, TTarget>
        {
            private static readonly JObject EmptyJObject = new();
            private readonly IGremlinQueryEnvironment _environment;

            public VertexOrEdgeConverter(IGremlinQueryEnvironment environment)
            {
                _environment = environment;
            }

            public bool TryConvert(JObject jObject, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                if (jObject.LooksLikeElement(out var idToken, out var label, out var maybePropertiesObject))
                {
                    if (recurse.TryTransform(maybePropertiesObject ?? EmptyJObject, _environment, out value))
                    {
                        value.SetIdAndLabel(idToken, label, _environment, recurse);
                        return true;
                    }
                }

                value = default;
                return false;
            }
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment) => (typeof(TSource) == typeof(JObject) && !typeof(TTarget).IsAssignableFrom(typeof(TSource)) && !typeof(TTarget).IsArray && typeof(TTarget) != typeof(object) && !typeof(TTarget).IsInterface && !typeof(Property).IsAssignableFrom(typeof(TTarget)))
            ? (IConverter<TSource, TTarget>)(object)new VertexOrEdgeConverter<TTarget>(environment)
            : default;
    }
}
