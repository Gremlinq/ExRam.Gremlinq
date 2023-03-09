using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class TraverserConverterFactory : IConverterFactory
    {
        private sealed class TraverserConverter<TTargetArray, TTargetItem> : IConverter<JArray, TTargetArray>
        {
            public bool TryConvert(JArray serialized, IGremlinQueryEnvironment environment, ITransformer recurse, [NotNullWhen(true)] out TTargetArray? value)
            {
                if (!environment.GetCache().FastNativeTypes.ContainsKey(typeof(TTargetArray)))
                {
                    var array = default(List<TTargetItem>);

                    for (var i = 0; i < serialized.Count; i++)
                    {
                        if (serialized[i] is JObject traverserObject && traverserObject.TryExpandTraverser<TTargetItem>(environment, recurse) is { } enumerable)
                        {
                            array ??= new List<TTargetItem>(serialized.Count);

                            foreach (var item1 in enumerable)
                                array.Add(item1);
                        }
                        else if (recurse.TryTransform<JToken, TTargetItem>(serialized[i], environment, out var item2))
                        {
                            array ??= new List<TTargetItem>(serialized.Count);

                            array.Add(item2);
                        }
                    }

                    value = (TTargetArray)(object)(array?.ToArray() ?? Array.Empty<TTargetItem>());
                    return true;
                }

                value = default;
                return false;
            }
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>()
        {
            return typeof(TSource) == typeof(JArray) && typeof(TTarget).IsArray
                ? (IConverter<TSource, TTarget>?)Activator.CreateInstance(typeof(TraverserConverter<,>).MakeGenericType(typeof(TTarget), typeof(TTarget).GetElementType()!))
                : default;
        }
    }
}
