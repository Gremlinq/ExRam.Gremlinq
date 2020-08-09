using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Gremlin.Net.Process.Traversal;

namespace ExRam.Gremlinq.Core
{
    internal static class ObjectExtensions
    {
        public static IEnumerable<(Key key, object value)> Serialize(
            this object? obj,
            IGremlinQueryEnvironment environment,
            SerializationBehaviour ignoreMask)
        {
            if (obj == null)
                yield break;

            foreach (var (propertyInfo, key, serializationBehaviour) in environment.GetCache().GetSerializationData(obj.GetType()))
            {
                var actualSerializationBehaviour = serializationBehaviour;

                if (key.RawKey is T t)
                {
                    actualSerializationBehaviour |= environment.Options
                        .GetValue(GremlinqOption.TSerializationBehaviourOverrides)
                        .GetValueOrDefault(t, SerializationBehaviour.Default);
                }

                if ((actualSerializationBehaviour & ignoreMask) == 0)
                {
                    var value = propertyInfo.GetValue(obj);

                    if (value != null)
                        yield return (key, value);
                }
            }
        }

        public static object GetId(this object element, IGremlinQueryEnvironment environment)
        {
            var (propertyInfo, _, _) = environment.GetCache().GetSerializationData(element.GetType())
                .FirstOrDefault(info => info.key.RawKey is T t && T.Id.Equals(t));

            return propertyInfo == null
                ? throw new InvalidOperationException($"Unable to determine Id for {element}")
                : propertyInfo.GetValue(element);
        }
    }
}
