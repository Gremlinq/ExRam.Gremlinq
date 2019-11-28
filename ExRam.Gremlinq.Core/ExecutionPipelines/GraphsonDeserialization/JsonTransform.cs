using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace ExRam.Gremlinq.Core
{
    internal static class JsonTransform
    {
        private sealed class JsonTransformImpl : IJsonTransform
        {
            private readonly Func<IPebbleEnumerator<(JsonToken tokenType, object tokenValue)>, IJsonTransform, IEnumerator<(JsonToken tokenType, object tokenValue)>> _factory;

            public JsonTransformImpl(Func<IPebbleEnumerator<(JsonToken tokenType, object tokenValue)>, IJsonTransform, IEnumerator<(JsonToken tokenType, object tokenValue)>> factory)
            {
                _factory = factory;
            }

            public IEnumerator<(JsonToken tokenType, object tokenValue)> Transform(IPebbleEnumerator<(JsonToken tokenType, object tokenValue)> enumerator, IJsonTransform recurse)
            {
                return _factory(enumerator, recurse);
            }
        }

        private sealed class CombinedJsonTransform : IJsonTransform
        {
            private readonly IJsonTransform _first;
            private readonly IJsonTransform _second;

            public CombinedJsonTransform(IJsonTransform first, IJsonTransform second)
            {
                _first = first;
                _second = second;
            }

            public IEnumerator<(JsonToken tokenType, object tokenValue)> Transform(IPebbleEnumerator<(JsonToken tokenType, object tokenValue)> source, IJsonTransform recurse)
            {
                source.DropPebble();

                var yielded = false;
                var secondTransform = _second.Transform(source, recurse);

                while (secondTransform.MoveNext())
                {
                    if (!yielded)
                    {
                        source.LiftPebble();
                        yielded = true;
                    }

                    yield return secondTransform.Current;
                }

                if (!yielded)
                {
                    source.Return();
                    var firstTransform = _first.Transform(source, recurse);

                    while (firstTransform.MoveNext())
                    {
                        if (!yielded)
                        {
                            source.LiftPebble();
                            yielded = true;
                        }

                        yield return firstTransform.Current;
                    }

                    if (!yielded)
                        source.LiftPebble();
                }
            }
        }

        private static readonly IEnumerator<(JsonToken tokenType, object tokenValue)> EmptyEnumerator = Enumerable.Empty<(JsonToken tokenType, object tokenValue)>().GetEnumerator();

        private static readonly IJsonTransform Empty = Create((source, recurse) => EmptyEnumerator);

        public static IJsonTransform Identity()
        {
            return Scalars(Empty)
                .Properties()
                .Objects()
                .Arrays();
        }

        public static IJsonTransform Objects(this IJsonTransform transform)
        {
            return transform.Combine(Create((source, recurse) => source.Objects(recurse)));
        }

        public static IJsonTransform Properties(this IJsonTransform transform)
        {
            return transform.Combine(Create((source, recurse) => source.Properties(recurse)));
        }

        public static IJsonTransform Arrays(this IJsonTransform transform)
        {
            return transform.Combine(Create((source, recurse) => source.Arrays(recurse)));
        }

        public static IJsonTransform Scalars(this IJsonTransform transform)
        {
            return transform.Combine(Create((source, recurse) => source.Scalars()));
        }

        public static IJsonTransform NestedValues(this IJsonTransform transform)
        {
            return transform.Combine(Create((source, recurse) => source.NestedValues(recurse)));
        }

        public static IJsonTransform GraphElements(this IJsonTransform transform)
        {
            return transform.Combine(Create((source, recurse) => source.GraphElements(recurse)));
        }

        public static IJsonTransform Traversers(this IJsonTransform transform)
        {
            return transform.Combine(Create((source, recurse) => source.Traversers(recurse)));
        }

        public static IJsonTransform Create(Func<IEnumerator<(JsonToken tokenType, object tokenValue)>, IJsonTransform, IEnumerator<(JsonToken tokenType, object tokenValue)>> factory)
        {
            return new JsonTransformImpl(factory);
        }

        public static IJsonTransform Combine(this IJsonTransform first, IJsonTransform second)
        {
            return new CombinedJsonTransform(first, second);
        }

        public static IEnumerator<(JsonToken tokenType, object tokenValue)> Transform(this IJsonTransform transform, IEnumerator<(JsonToken tokenType, object tokenValue)> source)
        {
            var transformed = transform.Transform(source.WithPebbles(), transform);

            while (transformed.MoveNext())
                yield return transformed.Current;
        }

        private static IEnumerator<(JsonToken tokenType, object tokenValue)> Objects(this IEnumerator<(JsonToken tokenType, object tokenValue)> source, IJsonTransform recurse)
        {
            if (source.Current.tokenType == JsonToken.StartObject)
            {
                yield return source.Current;

                while (source.MoveNext() && source.Current.tokenType != JsonToken.EndObject)
                {
                    var valueToken = recurse.Transform(source);

                    while (valueToken.MoveNext())
                        yield return valueToken.Current;
                }

                yield return (JsonToken.EndObject, null);
            }
        }

        private static IEnumerator<(JsonToken tokenType, object tokenValue)> Properties(this IEnumerator<(JsonToken tokenType, object tokenValue)> source, IJsonTransform recurse)
        {
            if (source.Current.tokenType == JsonToken.PropertyName)
            {
                yield return source.Current;

                if (source.MoveNext())
                {
                    var valueToken = recurse.Transform(source);

                    while (valueToken.MoveNext())
                        yield return valueToken.Current;
                }
            }
        }

        private static IEnumerator<(JsonToken tokenType, object tokenValue)> Arrays(this IEnumerator<(JsonToken tokenType, object tokenValue)> source, IJsonTransform recurse)
        {
            if (source.Current.tokenType == JsonToken.StartArray)
            {
                yield return source.Current;

                while (source.MoveNext() && source.Current.tokenType != JsonToken.EndArray)
                {
                    var valueToken = recurse.Transform(source);

                    while (valueToken.MoveNext())
                        yield return valueToken.Current;
                }

                yield return (JsonToken.EndArray, null);
            }
        }

        private static IEnumerator<(JsonToken tokenType, object tokenValue)> Scalars(this IEnumerator<(JsonToken tokenType, object tokenValue)> source)
        {
            var tokenType = source.Current.tokenType;

            if (tokenType == JsonToken.Boolean || tokenType == JsonToken.String || tokenType == JsonToken.Bytes || tokenType == JsonToken.Date || tokenType == JsonToken.Float || tokenType == JsonToken.Integer)
                yield return source.Current;
        }

        private static IEnumerator<(JsonToken tokenType, object tokenValue)> GraphElements(this IEnumerator<(JsonToken tokenType, object tokenValue)> source, IJsonTransform recurse)
        {
            if (source.Current.tokenType == JsonToken.StartObject)
            {
                var pebbled = source.WithPebbles();
                pebbled.DropPebble();

                string elementType = default;

                while (pebbled.MoveNext() && pebbled.Current.tokenType != JsonToken.EndObject)
                {
                    if (pebbled.Current.tokenType != JsonToken.PropertyName)
                        throw new InvalidOperationException();

                    var propertyName = pebbled.Current.tokenValue.ToString();

                    if (propertyName.Equals("properties", StringComparison.OrdinalIgnoreCase) && elementType != null)
                    {
                        if (pebbled.MoveNext())
                        {
                            if (pebbled.Current.tokenType == JsonToken.StartObject)
                            {
                                while (pebbled.MoveNext() && pebbled.Current.tokenType != JsonToken.EndObject)
                                {
                                    var valueToken = recurse.Transform(pebbled);

                                    while (valueToken.MoveNext())
                                        yield return valueToken.Current;
                                }
                            }
                            else
                                yield break;
                        }
                    }
                    else
                    {
                        if (!pebbled.HasPebble)
                            yield return pebbled.Current;

                        if (pebbled.MoveNext())
                        {
                            if (propertyName.Equals("type", StringComparison.OrdinalIgnoreCase) && pebbled.Current.tokenType == JsonToken.String)
                            {
                                var name = pebbled.Current.tokenValue.ToString();

                                if ("vertex".Equals(name, StringComparison.OrdinalIgnoreCase) || "edge".Equals(name, StringComparison.OrdinalIgnoreCase))
                                {
                                    elementType = name;

                                    if (pebbled.HasPebble)
                                    {
                                        var replay = pebbled.Replay();

                                        while (replay.MoveNext())
                                            yield return replay.Current;

                                        pebbled.LiftPebble();
                                    }

                                    yield return pebbled.Current;
                                }
                                else
                                    yield break;
                            }
                            else
                            {
                                var valueToken = recurse.Transform(pebbled);

                                while (valueToken.MoveNext())
                                {
                                    if (!pebbled.HasPebble)
                                        yield return valueToken.Current;
                                }
                            }
                        }
                    }
                }

                if (elementType != null)
                    yield return (JsonToken.EndObject, null);
            }
        }

        private static IEnumerator<(JsonToken tokenType, object tokenValue)> Traversers(this IEnumerator<(JsonToken tokenType, object tokenValue)> source, IJsonTransform recurse)
        {
            if (source.Current.tokenType == JsonToken.StartObject)
            {
                var bulk = (long)-1;

                while (source.MoveNext() && source.Current.tokenType != JsonToken.EndObject)
                {
                    if ("bulk".Equals(source.Current.tokenValue))
                    {
                        bulk = Convert.ToInt64(recurse.Transform(source).Last().tokenValue);
                    }
                    else if ("value".Equals(source.Current.tokenValue) && bulk > 0)
                    {
                        if (source.MoveNext())
                        {
                            var valueToken = recurse.Transform(source);

                            if (bulk > 1)
                            {
                                var pebbled = valueToken.WithPebbles();
                                pebbled.DropPebble();

                                for (var i = 0; i < bulk; i++)
                                {
                                    while (pebbled.MoveNext())
                                        yield return pebbled.Current;

                                    pebbled.Return();
                                }
                            }
                            else
                            {
                                while (valueToken.MoveNext())
                                    yield return valueToken.Current;
                            }
                        }
                    }
                    else
                    {
                        recurse.Transform(source).Iterate();
                    }
                }
            }
        }

        private static IEnumerator<(JsonToken tokenType, object tokenValue)> NestedValues(this IEnumerator<(JsonToken tokenType, object tokenValue)> source, IJsonTransform recurse)
        {
            if (source.Current.tokenType == JsonToken.StartObject && source.MoveNext() &&
                source.Current.tokenType == JsonToken.PropertyName && source.Current.tokenValue.Equals("@type") &&
                source.MoveNext() && source.Current.tokenType == JsonToken.String)
            {
                var type = source.Current.tokenValue.ToString();

                if (source.MoveNext() && source.Current.tokenType == JsonToken.PropertyName &&
                    source.Current.tokenValue.Equals("@value") &&
                    source.MoveNext())
                {
                    if ("g:Map".Equals(type, StringComparison.OrdinalIgnoreCase))
                    {
                        if (source.Current.tokenType == JsonToken.StartArray)
                        {
                            yield return (JsonToken.StartObject, null);

                            while (source.MoveNext() && source.Current.tokenType != JsonToken.EndArray)
                            {
                                var key = source.Current.tokenValue.ToString();

                                if (source.MoveNext())
                                {
                                    yield return (JsonToken.PropertyName, key);

                                    var value = recurse.Transform(source);

                                    while (value.MoveNext())
                                        yield return value.Current;
                                }
                            }

                            yield return (JsonToken.EndObject, null);
                        }
                    }
                    else if ("g:Vertex".Equals(type, StringComparison.OrdinalIgnoreCase))
                    {
                        source = source.InjectProperty("type", "vertex");
                        source.MoveNext();
                    }
                    else if ("g:Edge".Equals(type, StringComparison.OrdinalIgnoreCase))
                    {
                        source = source.InjectProperty("type", "edge");
                        source.MoveNext();
                    }
                    else if ("g:Traverser".Equals(type, StringComparison.OrdinalIgnoreCase))
                    {
                        source = source.InjectProperty("type", "traverser");
                        source.MoveNext();
                    }

                    var valueToken = recurse.Transform(source);

                    while (valueToken.MoveNext())
                        yield return valueToken.Current;

                    source.MoveNext();
                }
            }
        }

        private static IEnumerator<(JsonToken tokenType, object tokenValue)> InjectProperty(this IEnumerator<(JsonToken tokenType, object tokenValue)> source, string name, string value)
        {
            yield return source.Current;

            if (source.Current.tokenType == JsonToken.StartObject)
            {
                yield return (JsonToken.PropertyName, name);
                yield return (JsonToken.String, value);
            }

            while (source.MoveNext())
                yield return source.Current;
        }
    }
}
