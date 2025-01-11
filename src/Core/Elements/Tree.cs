using System.Collections;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace ExRam.Gremlinq.Core
{
    public class Tree<TRoot, TSubTree> : ITree, IDictionary<TRoot, TSubTree>
        where TSubTree : ITree
        where TRoot : notnull
    {
        public static readonly Tree<TRoot, TSubTree> Empty = new (ImmutableDictionary<TRoot, TSubTree>.Empty);

        private readonly IDictionary<TRoot, TSubTree> _inner;

        public Tree(IDictionary<TRoot, TSubTree> inner)
        {
            _inner = inner;
        }

        public TSubTree this[TRoot key] { get => _inner[key]; set => _inner[key] = value; }

        public ICollection<TRoot> Keys => _inner.Keys;

        public ICollection<TSubTree> Values => _inner.Values;

        public int Count => _inner.Count;

        public bool IsReadOnly => _inner.IsReadOnly;

        public void Add(TRoot key, TSubTree value)
        {
            _inner.Add(key, value);
        }

        public void Add(KeyValuePair<TRoot, TSubTree> item)
        {
            _inner.Add(item);
        }

        public void Clear()
        {
            _inner.Clear();
        }

        public bool Contains(KeyValuePair<TRoot, TSubTree> item)
        {
            return _inner.Contains(item);
        }

        public bool ContainsKey(TRoot key)
        {
            return _inner.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<TRoot, TSubTree>[] array, int arrayIndex)
        {
            _inner.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TRoot, TSubTree>> GetEnumerator()
        {
            return _inner.GetEnumerator();
        }

        public bool Remove(TRoot key)
        {
            return _inner.Remove(key);
        }

        public bool Remove(KeyValuePair<TRoot, TSubTree> item)
        {
            return _inner.Remove(item);
        }

        public bool TryGetValue(TRoot key, [MaybeNullWhen(false)] out TSubTree value)
        {
            return _inner.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_inner).GetEnumerator();
        }
    }

    public class Tree<TRoot> : Tree<TRoot, Tree<object>>
        where TRoot : notnull
    {
        public static new readonly Tree<TRoot> Empty = new (ImmutableDictionary<TRoot, Tree<object>>.Empty);

        public Tree(IDictionary<TRoot, Tree<object>> inner) : base(inner)
        {
        }
    }
}
