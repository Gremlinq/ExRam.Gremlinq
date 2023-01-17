using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Deserialization;
using Gremlin.Net.Process.Traversal;
using Newtonsoft.Json.Linq;

namespace System
{
    internal static class ObjectExtensions
    {
        public static object SetIdAndLabel(this object element, JToken idToken, JToken labelToken, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse)
        {
            var serializationData = environment
                .GetCache()
                .GetSerializationData(element.GetType());

            for (var i = 0; i < serializationData.Length; i++)
            {
                var info = serializationData[i];

                if (info.key.RawKey is T t && info.propertyInfo is { } propertyInfo)
                {
                    var maybeRelevantToken = T.Id.Equals(t)
                        ? idToken
                        : T.Label.Equals(t)
                            ? labelToken
                            : default;

                    if (maybeRelevantToken is { } token)
                        propertyInfo.SetValue(element, recurse.TryDeserialize(token, propertyInfo.PropertyType, environment));
                }
            }

            return element;
        }
    }
}
