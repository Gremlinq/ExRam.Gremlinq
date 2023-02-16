using System.Collections.Concurrent;
using System.Reflection;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Deserialization;
using Gremlin.Net.Process.Traversal;
using Newtonsoft.Json.Linq;

namespace System
{
    internal static class ObjectExtensions
    {
        private static class Info<TElement>
        {
            private static readonly ConcurrentDictionary<IGremlinQueryEnvironment, Action<TElement, JToken, IGremlinQueryFragmentDeserializer>?> IdSetters = new();
            private static readonly ConcurrentDictionary<IGremlinQueryEnvironment, Action<TElement, JToken, IGremlinQueryFragmentDeserializer>?> LabelSetters = new();

            public static void SetId(TElement element, JToken idToken, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse)
            {
                if (TryGetSetter(IdSetters, T.Id, environment) is { } idSetter)
                    idSetter(element, idToken, recurse);
            }

            public static void SetLabel(TElement element, JToken idToken, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse)
            {
                if (TryGetSetter(LabelSetters, T.Label, environment) is { } labelSetter)
                    labelSetter(element, idToken, recurse);
            }

            private static Action<TElement, JToken, IGremlinQueryFragmentDeserializer>? TryGetSetter(ConcurrentDictionary<IGremlinQueryEnvironment, Action<TElement, JToken, IGremlinQueryFragmentDeserializer>?> dict, T relevantT, IGremlinQueryEnvironment environment)
            {
                return dict
                    .GetOrAdd(
                        environment,
                        static (environment, relevantT) =>
                        {
                            var serializationData = environment
                                .GetCache()
                                .GetSerializationData(typeof(TElement));

                            for (var i = 0; i < serializationData.Length; i++)
                            {
                                var info = serializationData[i];

                                if (info.key.RawKey is T t && relevantT.Equals(t) && info.propertyInfo is { } propertyInfo)
                                {
                                    return (Action<TElement, JToken, IGremlinQueryFragmentDeserializer>)typeof(Info<TElement>)
                                        .GetMethod(nameof(CreateSetter), BindingFlags.NonPublic | BindingFlags.Static)!
                                        .MakeGenericMethod(propertyInfo.PropertyType)
                                        .Invoke(null, new object[] { propertyInfo, environment })!;
                                }
                            }

                            return null;
                        },
                        relevantT);
            }

            private static Action<TElement, JToken, IGremlinQueryFragmentDeserializer>? CreateSetter<TProperty>(PropertyInfo propertyInfo, IGremlinQueryEnvironment environment)
            {
                return (element, token, recurse) =>
                {
                    if (recurse.TryDeserialize<JToken, TProperty>(token, environment, out var value))
                        propertyInfo.SetValue(element, value);
                };
            }
        }


        public static TElement SetIdAndLabel<TElement>(this TElement element, JToken idToken, JToken labelToken, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse)
        {
            Info<TElement>.SetId(element, idToken, environment, recurse);
            Info<TElement>.SetLabel(element, labelToken, environment, recurse);

            return element;
        }
    }
}
