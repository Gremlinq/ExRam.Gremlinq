using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace ExRam.Gremlinq.Core
{
    public class Tree<K, V> : ITree, IDictionary<K, V> where V : ITree where K : notnull
    {
        private readonly IDictionary<K, V> _inner;

        public Tree(IDictionary<K, V> inner)
        {
            _inner = inner;
        }

        public V this[K key] { get => _inner[key]; set => _inner[key] = value; }

        public ICollection<K> Keys => _inner.Keys;

        public ICollection<V> Values => _inner.Values;

        public int Count => _inner.Count;

        public bool IsReadOnly => _inner.IsReadOnly;

        public void Add(K key, V value)
        {
            _inner.Add(key, value);
        }

        public void Add(KeyValuePair<K, V> item)
        {
            _inner.Add(item);
        }

        public void Clear()
        {
            _inner.Clear();
        }

        public bool Contains(KeyValuePair<K, V> item)
        {
            return _inner.Contains(item);
        }

        public bool ContainsKey(K key)
        {
            return _inner.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
        {
            _inner.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            return _inner.GetEnumerator();
        }

        public bool Remove(K key)
        {
            return _inner.Remove(key);
        }

        public bool Remove(KeyValuePair<K, V> item)
        {
            return _inner.Remove(item);
        }

        public bool TryGetValue(K key, [MaybeNullWhen(false)] out V value)
        {
            return _inner.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_inner).GetEnumerator();
        }
    }

    public class Tree<K> : Tree<K, Tree<object>> where K : notnull
    {
        public Tree(IDictionary<K, Tree<object>> inner) : base(inner)
        {
        }
    }
}
