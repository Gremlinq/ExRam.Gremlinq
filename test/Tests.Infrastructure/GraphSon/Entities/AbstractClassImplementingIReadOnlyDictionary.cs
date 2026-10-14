using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace ExRam.Gremlinq.Tests.Infrastructure.GraphSon.Entities
{
    // A read-only dictionary and nothing more: it implements IReadOnlyDictionary<,>, but no
    // IDictionary<,> and no IDictionary, over a Dictionary<,> that only its constructor fills.
    public abstract class AbstractClassImplementingIReadOnlyDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
        where TKey : notnull
    {
        private readonly Dictionary<TKey, TValue> _dictionary;

        protected AbstractClassImplementingIReadOnlyDictionary(IEnumerable<KeyValuePair<TKey, TValue>> pairs)
        {
            _dictionary = new(pairs);
        }

        TValue IReadOnlyDictionary<TKey, TValue>.this[TKey key] => _dictionary[key];

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => _dictionary.Keys;

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => _dictionary.Values;

        int IReadOnlyCollection<KeyValuePair<TKey, TValue>>.Count => _dictionary.Count;

        bool IReadOnlyDictionary<TKey, TValue>.ContainsKey(TKey key) => _dictionary.ContainsKey(key);

        bool IReadOnlyDictionary<TKey, TValue>.TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) => _dictionary.TryGetValue(key, out value);

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() => _dictionary.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _dictionary.GetEnumerator();
    }
}
