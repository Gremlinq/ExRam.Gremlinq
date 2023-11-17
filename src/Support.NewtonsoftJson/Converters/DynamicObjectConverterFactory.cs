using Newtonsoft.Json.Linq;
using System.Dynamic;
using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Transformation;
using ExRam.Gremlinq.Core;
using System.Collections;

namespace ExRam.Gremlinq.Support.NewtonsoftJson
{
    internal sealed class DynamicObjectConverterFactory : IConverterFactory
    {
        private sealed class DynamicObjectConverter<TTarget> : IConverter<JObject, TTarget>
        {
            #region DynamicDictionary
            private sealed class DynamicDictionary : DynamicObject, IReadOnlyDictionary<string, object?>, IDictionary<string, object?>
            {
                private readonly IDictionary<string, object?> _dictionary;

                public DynamicDictionary(IDictionary<string, object?> dictionary)
                {
                    _dictionary = dictionary;
                }

                public override bool TrySetMember(SetMemberBinder binder, object? value)
                {
                    _dictionary[binder.Name] = value;
                    return true;
                }

                public override bool TryGetMember(GetMemberBinder binder, out object? result) => _dictionary.TryGetValue(binder.Name, out result);

                object? IReadOnlyDictionary<string, object?>.this[string key] => _dictionary[key];

                IEnumerable<string> IReadOnlyDictionary<string, object?>.Keys => _dictionary.Keys;

                IEnumerable<object?> IReadOnlyDictionary<string, object?>.Values => _dictionary.Values;

                int IReadOnlyCollection<KeyValuePair<string, object?>>.Count => _dictionary.Count;

                ICollection<string> IDictionary<string, object?>.Keys => _dictionary.Keys;

                ICollection<object?> IDictionary<string, object?>.Values => _dictionary.Values;

                int ICollection<KeyValuePair<string, object?>>.Count => _dictionary.Count;

                bool ICollection<KeyValuePair<string, object?>>.IsReadOnly => _dictionary.IsReadOnly;

                object? IDictionary<string, object?>.this[string key] { get => _dictionary[key]; set => _dictionary[key] = value; }

                bool IReadOnlyDictionary<string, object?>.ContainsKey(string key) => _dictionary.ContainsKey(key);

                IEnumerator<KeyValuePair<string, object?>> IEnumerable<KeyValuePair<string, object?>>.GetEnumerator() => _dictionary.GetEnumerator();

                bool IReadOnlyDictionary<string, object?>.TryGetValue(string key, out object? value) => _dictionary.TryGetValue(key, out value);

                IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_dictionary).GetEnumerator();

                void IDictionary<string, object?>.Add(string key, object? value) => _dictionary.Add(key, value);

                bool IDictionary<string, object?>.ContainsKey(string key) => _dictionary.ContainsKey(key);

                bool IDictionary<string, object?>.Remove(string key) => _dictionary.Remove(key);

                bool IDictionary<string, object?>.TryGetValue(string key, out object? value) => _dictionary.TryGetValue(key, out value);

                void ICollection<KeyValuePair<string, object?>>.Add(KeyValuePair<string, object?> item) => _dictionary.Add(item);

                void ICollection<KeyValuePair<string, object?>>.Clear() => _dictionary.Clear();

                bool ICollection<KeyValuePair<string, object?>>.Contains(KeyValuePair<string, object?> item) => _dictionary.Contains(item);

                void ICollection<KeyValuePair<string, object?>>.CopyTo(KeyValuePair<string, object?>[] array, int arrayIndex) => _dictionary.CopyTo(array, arrayIndex);

                bool ICollection<KeyValuePair<string, object?>>.Remove(KeyValuePair<string, object?> item) => _dictionary.Remove(item);
            }
            #endregion

            private readonly IGremlinQueryEnvironment _environment;

            public DynamicObjectConverter(IGremlinQueryEnvironment environment)
            {
                _environment = environment;
            }

            public bool TryConvert(JObject serialized, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value)
            {
                if (recurse.TryTransform(serialized, _environment, out IDictionary<string, object?>? dictionary))
                {
                    value = (TTarget)(object)new DynamicDictionary(dictionary);
                    return true;
                }

                value = default;
                return false;
            }
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>(IGremlinQueryEnvironment environment)
        {
            return typeof(TSource) == typeof(JObject) && typeof(TTarget) == typeof(object)
                ? (IConverter<TSource, TTarget>)(object)new DynamicObjectConverter<TTarget>(environment)
                : default;
        }
    }
}
