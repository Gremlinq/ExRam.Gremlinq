using System.Collections.Concurrent;
using System.Reflection;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Transformation;
using Gremlin.Net.Process.Traversal;
using Newtonsoft.Json.Linq;

namespace System
{
    internal static class ObjectExtensions
    {
        private static class Info<TElement>
        {
            private static readonly ConcurrentDictionary<IGremlinQueryEnvironment, Action<TElement, JToken, ITransformer>?> IdSetters = new();
            private static readonly ConcurrentDictionary<IGremlinQueryEnvironment, Action<TElement, JToken, ITransformer>?> LabelSetters = new();

            public static void SetId(TElement element, JToken idToken, IGremlinQueryEnvironment environment, ITransformer recurse)
            {
                if (TryGetSetter(IdSetters, T.Id, environment) is { } idSetter)
                    idSetter(element, idToken, recurse);
            }

            public static void SetLabel(TElement element, JToken labelToken, IGremlinQueryEnvironment environment, ITransformer recurse)
            {
                if (TryGetSetter(LabelSetters, T.Label, environment) is { } labelSetter)
                    labelSetter(element, labelToken, recurse);
            }

            private static Action<TElement, JToken, ITransformer>? TryGetSetter(ConcurrentDictionary<IGremlinQueryEnvironment, Action<TElement, JToken, ITransformer>?> dict, T relevantT, IGremlinQueryEnvironment environment)
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

                                if (info.metadata.Key.RawKey is T t && relevantT.Equals(t) && info.propertyInfo is { } propertyInfo)
                                {
                                    return (Action<TElement, JToken, ITransformer>)typeof(Info<TElement>)
                                        .GetMethod(nameof(CreateSetter), BindingFlags.NonPublic | BindingFlags.Static)!
                                        .MakeGenericMethod(propertyInfo.PropertyType)
                                        .Invoke(null, new object[] { propertyInfo, environment })!;
                                }
                            }

                            return null;
                        },
                        relevantT);
            }

            private static Action<TElement, JToken, ITransformer> CreateSetter<TProperty>(PropertyInfo propertyInfo, IGremlinQueryEnvironment environment)
            {
                return (element, token, recurse) =>
                {
                    if (recurse.TryTransform<JToken, TProperty>(token, environment, out var value))
                        propertyInfo.SetValue(element, value);
                };
            }
        }


        public static TElement SetIdAndLabel<TElement>(this TElement element, JToken idToken, JToken labelToken, IGremlinQueryEnvironment environment, ITransformer recurse)
        {
            Info<TElement>.SetId(element, idToken, environment, recurse);
            Info<TElement>.SetLabel(element, labelToken, environment, recurse);

            return element;
        }
    }
}
