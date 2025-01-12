using System.Collections;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace ExRam.Gremlinq.Core
{
    public class Tree<TRoot, TSubTree> : ITree, IReadOnlyDictionary<TRoot, TSubTree>
        where TSubTree : ITree
        where TRoot : notnull
    {
        public static readonly Tree<TRoot, TSubTree> Empty = new (ImmutableDictionary<TRoot, TSubTree>.Empty);

        private readonly IReadOnlyDictionary<TRoot, TSubTree> _inner;

        public Tree(IReadOnlyDictionary<TRoot, TSubTree> inner)
        {
            _inner = inner;
        }

        public TSubTree this[TRoot key] => _inner[key];

        public IEnumerable<TRoot> Keys => _inner.Keys;

        public IEnumerable<TSubTree> Values => _inner.Values;

        public int Count => _inner.Count;

        public bool ContainsKey(TRoot key) => _inner.ContainsKey(key);

        public IEnumerator<KeyValuePair<TRoot, TSubTree>> GetEnumerator() => _inner.GetEnumerator();

        public bool TryGetValue(TRoot key, [MaybeNullWhen(false)] out TSubTree value) => _inner.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_inner).GetEnumerator();
    }

    public class Tree<TRoot> : Tree<TRoot, Tree<object>>
        where TRoot : notnull
    {
        public static new readonly Tree<TRoot> Empty = new (ImmutableDictionary<TRoot, Tree<object>>.Empty);

        public Tree(IReadOnlyDictionary<TRoot, Tree<object>> inner) : base(inner)
        {
        }
    }
}
