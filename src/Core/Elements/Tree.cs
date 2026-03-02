using System.Collections;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Represents a graph tree structure where each root node maps to a subtree.
    /// </summary>
    /// <typeparam name="TRoot">The type of the root-level nodes.</typeparam>
    /// <typeparam name="TSubTree">The type of the subtree, which itself must be a tree.</typeparam>
    public class Tree<TRoot, TSubTree> : ITree, IReadOnlyDictionary<TRoot, TSubTree>
        where TSubTree : ITree
        where TRoot : notnull
    {
        public static readonly Tree<TRoot, TSubTree> Empty = new (ImmutableDictionary<TRoot, TSubTree>.Empty);

        private readonly IReadOnlyDictionary<TRoot, TSubTree> _inner;

        public Tree(IReadOnlyDictionary<TRoot, TSubTree> inner)
        {
            ArgumentNullException.ThrowIfNull(inner);

            _inner = inner;
        }

        public TSubTree this[TRoot key] => _inner[key];

        public IEnumerable<TRoot> Keys => _inner.Keys;

        public IEnumerable<TSubTree> Values => _inner.Values;

        public int Count => _inner.Count;

        public bool ContainsKey(TRoot key)
        {
            ArgumentNullException.ThrowIfNull(key);

            return _inner.ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<TRoot, TSubTree>> GetEnumerator() => _inner.GetEnumerator();

#pragma warning disable CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
        public bool TryGetValue(TRoot key, [MaybeNullWhen(false)] out TSubTree value)
        {
            ArgumentNullException.ThrowIfNull(key);

            return _inner.TryGetValue(key, out value);
        }
#pragma warning restore CS8767

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_inner).GetEnumerator();
    }

    /// <summary>
    /// Represents a recursive graph tree where each node maps to another tree of the same node type.
    /// </summary>
    /// <typeparam name="TNode">The type of the tree nodes.</typeparam>
    public class Tree<TNode> : Tree<TNode, Tree<TNode>>
        where TNode : notnull
    {
        public static new readonly Tree<TNode> Empty = new (ImmutableDictionary<TNode, Tree<TNode>>.Empty);

        public Tree(IReadOnlyDictionary<TNode, Tree<TNode>> inner) : base(inner)
        {
            ArgumentNullException.ThrowIfNull(inner);

        }
    }
}
