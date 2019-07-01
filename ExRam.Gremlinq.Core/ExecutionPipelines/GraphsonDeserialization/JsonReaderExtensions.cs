using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ExRam.Gremlinq.Core
{
    internal static class JsonReaderExtensions
    {
        private sealed class EnumerableJsonReader : JsonReader
        {
            private readonly IEnumerator<(JsonToken tokenType, object tokenValue)> _enumerator;

            public EnumerableJsonReader(IEnumerator<(JsonToken tokenType, object tokenValue)> enumerator)
            {
                _enumerator = enumerator;
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                    _enumerator.Dispose();

                base.Dispose(disposing);
            }

            public override bool Read()
            {
                if (_enumerator.MoveNext())
                {
                    SetToken(_enumerator.Current.tokenType, _enumerator.Current.tokenValue);

                    return true;
                }

                return false;
            }
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

        public static IEnumerable<(JsonToken tokenType, object tokenValue)> Apply(this IEnumerable<(JsonToken tokenType, object tokenValue)> source, IJsonTransform transform)
        {
            using (var e = source.GetEnumerator())
            {
                e.MoveNext();
                using (var sub = transform.Transform(e))
                {
                    while (sub.MoveNext())
                        yield return sub.Current;
                }
            }
        }
    }
}
