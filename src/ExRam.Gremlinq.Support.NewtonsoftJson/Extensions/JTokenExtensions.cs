using ExRam.Gremlinq.Core.Deserialization;
using ExRam.Gremlinq.Core;

namespace Newtonsoft.Json.Linq
{
    internal static class JTokenExtensions
    {
        public static IEnumerable<object>? TryExpandTraverser(this JObject jObject, Type type, IGremlinQueryEnvironment env, IGremlinQueryFragmentDeserializer recurse)
        {
            //Traversers
            if (jObject.TryGetValue("@type", out var nestedType) && "g:Traverser".Equals(nestedType.Value<string>(), StringComparison.OrdinalIgnoreCase) && jObject.TryGetValue("@value", out var valueToken) && valueToken is JObject nestedTraverserObject)
            {
                var bulk = 1;

                if (nestedTraverserObject.TryGetValue("bulk", out var bulkToken) && recurse.TryDeserialize<int>().From(bulkToken, env) is { } bulkObject)
                    bulk = bulkObject;

                if (nestedTraverserObject.TryGetValue("value", out var traverserValue))
                {
                    return Core();

                    IEnumerable<object> Core()
                    {
                        if (recurse.TryDeserialize(type).From(traverserValue, env) is { } item)
                        {
                            for (var j = 0; j < bulk; j++)
                                yield return item;
                        }
                    }
                }
            }

            return null;
        }
    }
}
