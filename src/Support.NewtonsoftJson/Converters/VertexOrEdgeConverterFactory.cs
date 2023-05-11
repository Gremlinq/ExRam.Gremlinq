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
            private readonly IGremlinQueryEnvironment _environment;

            public VertexOrEdgeConverter(IGremlinQueryEnvironment environment)
            {
                _environment = environment;
            }

            public bool TryConvert(JObject jObject, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                if (jObject.TryGetValue("id", StringComparison.OrdinalIgnoreCase, out var idToken) && jObject.TryGetValue("label", StringComparison.OrdinalIgnoreCase, out var labelToken) && labelToken.Type == JTokenType.String && jObject.TryGetValue("properties", out var propertiesToken) && propertiesToken is JObject propertiesObject)
                {
                    if (recurse.TryTransform(propertiesObject, _environment, out value))
                    {
                        value.SetIdAndLabel(idToken, labelToken, _environment, recurse);
                        return true;
                    }
                }

                value = default;
                return false;
            }
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment)
        {
            if (typeof(TSource) == typeof(JObject) && !typeof(TTarget).IsAssignableFrom(typeof(TSource)) && !typeof(TTarget).IsArray && typeof(TTarget) != typeof(object) && !typeof(TTarget).IsInterface && !typeof(Property).IsAssignableFrom(typeof(TTarget)))
            {
                if (environment.Model.VerticesModel.Metadata.Keys.Concat(environment.Model.EdgesModel.Metadata.Keys).Any(type => type.IsAssignableFrom(typeof(TTarget))))
                    return (IConverter<TSource, TTarget>)(object)new VertexOrEdgeConverter<TTarget>(environment);
            }

            return default;
        }
    }
}
