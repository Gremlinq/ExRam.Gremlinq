using System;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;

namespace ExRam.Gremlinq
{
    public static class JsonExtensions
    {
        private sealed class EnumerableJsonReader : JsonReader
        {
            private readonly IEnumerator<(JsonToken tokenType, object tokenValue)> _enumerator;

            public EnumerableJsonReader(IEnumerator<(JsonToken tokenType, object tokenValue)> enumerator)
            {
                this._enumerator = enumerator;
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                    this._enumerator.Dispose();

                base.Dispose(disposing);
            }

            public override bool Read()
            {
                return this._enumerator.MoveNext();
            }

            public override JsonToken TokenType => this._enumerator.Current.tokenType;

            public override object Value => this._enumerator.Current.tokenValue;
        }

        public static IEnumerable<(JsonToken tokenType, object tokenValue)> ToTokenEnumerable(this JsonReader jsonReader)
        {
            while (jsonReader.Read())
                yield return (jsonReader.TokenType, jsonReader.Value);
        }

        public static JsonReader ToJsonReader(this IEnumerable<(JsonToken tokenType, object tokenValue)> enumerable)
        {
            return new EnumerableJsonReader(enumerable.GetEnumerator());
        }

        public static IEnumerable<(JsonToken tokenType, object tokenValue)> Apply(this IEnumerable<(JsonToken tokenType, object tokenValue)> source, Func<IEnumerator<(JsonToken tokenType, object tokenValue)>, IEnumerator<(JsonToken tokenType, object tokenValue)>> transformation)
        {
            using (var e = transformation(source.GetEnumerator()))
            {
                while (e.MoveNext())
                    yield return e.Current;
            }
        }

        public static IEnumerator<(JsonToken tokenType, object tokenValue)> UnwrapObject(this IEnumerator<(JsonToken tokenType, object tokenValue)> enumerator, string property, Func<IEnumerator<(JsonToken tokenType, object tokenValue)>, IEnumerator<(JsonToken tokenType, object tokenValue)>> innerTransformation)
        {
            while (enumerator.MoveNext())
            {
                var current = enumerator.Current;

                if (current.tokenType == JsonToken.PropertyName)
                {
                    if (property.Equals(current.tokenValue as string, StringComparison.Ordinal))
                    {
                        if (enumerator.MoveNext())
                        {
                            if (enumerator.Current.tokenType == JsonToken.StartObject)
                            {
                                using (var inner = innerTransformation(enumerator.UntilEndObject()))
                                {
                                    while (inner.MoveNext())
                                    {
                                        yield return inner.Current;
                                    }
                                }

                                continue;
                            }

                            yield return (JsonToken.PropertyName, property);
                            yield return (enumerator.Current.tokenType, enumerator.Current.tokenValue);

                            continue;
                        }

                        yield break;
                    }
                }

                yield return current;
            }
        }

        public static IEnumerator<(JsonToken tokenType, object tokenValue)> ReadValue(this IEnumerator<(JsonToken tokenType, object tokenValue)> source)
        {
            var openArrays = 0;
            var openObjects = 0;

            while (source.MoveNext())
            {
                var current = source.Current;

                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (current.tokenType)
                {
                    case JsonToken.StartObject:
                        openObjects++;
                        break;
                    case JsonToken.StartArray:
                        openArrays++;
                        break;
                    case JsonToken.EndObject:
                        openObjects--;
                        break;
                    case JsonToken.EndArray:
                        openArrays--;
                        break;
                }

                yield return current;

                if (openArrays == 0 && openObjects == 0)
                    break;
            }
        }

        public static IEnumerator<(JsonToken tokenType, object tokenValue)> UntilEndObject(this IEnumerator<(JsonToken tokenType, object tokenValue)> source)
        {
            var openObjects = 0;

            while (source.MoveNext())
            {
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (source.Current.tokenType)
                {
                    case JsonToken.StartObject:
                    { 
                        openObjects++;
                        break;
                    }
                    case JsonToken.EndObject:
                    { 
                        if (openObjects == 0)
                            yield break;

                        openObjects--;
                        break;
                    }
                }

                yield return source.Current;
            }
        }
        
        public static IEnumerator<(JsonToken tokenType, object tokenValue)> Log(this IEnumerator<(JsonToken tokenType, object tokenValue)> source)
        {
            while (source.MoveNext())
            {
                Debug.WriteLine(source.Current);
                yield return source.Current;
            }
        }

        public static IEnumerator<(JsonToken tokenType, object tokenValue)> ExtractProperty(this IEnumerator<(JsonToken tokenType, object tokenValue)> source, string property)
        {
            while (source.MoveNext())
            {
                if (source.Current.tokenType == JsonToken.PropertyName && property.Equals(source.Current.tokenValue as string))
                {
                    using (var e = source.ReadValue())
                    {
                        while (e.MoveNext())
                            yield return e.Current;
                    }
                }
            }
        }

        public static IEnumerator<(JsonToken tokenType, object tokenValue)> SelectPropertyValue(this IEnumerator<(JsonToken tokenType, object tokenValue)> source, Func<IEnumerator<(JsonToken tokenType, object tokenValue)>, IEnumerator<(JsonToken tokenType, object tokenValue)>> projection)
        {
            while (source.MoveNext())
            {
                if (source.Current.tokenType == JsonToken.PropertyName)
                {
                    yield return source.Current;

                    using (var e = projection(source.ReadValue()))
                    {
                        while (e.MoveNext())
                            yield return e.Current;
                    }
                }
            }
        }

        public static IEnumerator<(JsonToken tokenType, object tokenValue)> SelectArray(this IEnumerator<(JsonToken tokenType, object tokenValue)> source, Func<IEnumerator<(JsonToken tokenType, object tokenValue)>, IEnumerator<(JsonToken tokenType, object tokenValue)>> projection)
        {
            while (source.MoveNext())
            {
                if (source.Current.tokenType == JsonToken.StartArray || source.Current.tokenType == JsonToken.EndArray)
                {
                    yield return source.Current;

                    if (source.Current.tokenType == JsonToken.EndArray)
                        yield break;
                }
                else
                    source = source.Pushback();

                using (var e = projection(source.ReadValue()))
                {
                    while (e.MoveNext())
                        yield return e.Current;
                }
            }
        }

        public static IEnumerator<(JsonToken tokenType, object tokenValue)> Pushback(this IEnumerator<(JsonToken tokenType, object tokenValue)> source)
        {
            yield return source.Current;

            using (source)
            {
                while (source.MoveNext())
                    yield return source.Current;
            }
        }

        public static IEnumerator<(JsonToken tokenType, object tokenValue)> SelectPropertyValue(this IEnumerator<(JsonToken tokenType, object tokenValue)> source, string propertyName, Func<IEnumerator<(JsonToken tokenType, object tokenValue)>, IEnumerator<(JsonToken tokenType, object tokenValue)>> projection)
        {
            while (source.MoveNext())
            {
                yield return source.Current;

                if (source.Current.tokenType == JsonToken.PropertyName && propertyName.Equals(source.Current.tokenValue as string))
                {
                    using (var e = projection(source.ReadValue()))
                    {
                        while (e.MoveNext())
                            yield return e.Current;
                    }
                }
            }
        }

        public static IEnumerator<(JsonToken tokenType, object tokenValue)> SelectToken(this IEnumerator<(JsonToken tokenType, object tokenValue)> source, Func<(JsonToken tokenType, object tokenValue), (JsonToken tokenType, object tokenValue)> projection)
        {
            while (source.MoveNext())
            {
                yield return projection(source.Current);
            }
        }
    }
}