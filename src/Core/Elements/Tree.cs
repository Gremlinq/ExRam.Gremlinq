using System.Collections;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Represents a tree structure with strongly-typed root nodes and sub-trees.
    /// Trees in graph databases represent hierarchical structures discovered during traversals.
    /// </summary>
    /// <typeparam name="TRoot">The type of the root nodes in the tree.</typeparam>
    /// <typeparam name="TSubTree">The type of the sub-trees, which must also be trees.</typeparam>
    public class Tree<TRoot, TSubTree> : ITree, IReadOnlyDictionary<TRoot, TSubTree>
        where TSubTree : ITree
        where TRoot : notnull
    {
        /// <summary>
        /// Gets an empty tree instance.
        /// </summary>
        public static readonly Tree<TRoot, TSubTree> Empty = new (ImmutableDictionary<TRoot, TSubTree>.Empty);

        private readonly IReadOnlyDictionary<TRoot, TSubTree> _inner;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tree{TRoot, TSubTree}"/> class with the specified dictionary.
        /// </summary>
        /// <param name="inner">The dictionary containing the tree structure.</param>
        public Tree(IReadOnlyDictionary<TRoot, TSubTree> inner)
        {
            _inner = inner;
        }

        /// <summary>
        /// Gets the sub-tree associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the sub-tree to get.</param>
        /// <returns>The sub-tree associated with the specified key.</returns>
        public TSubTree this[TRoot key] => _inner[key];

        /// <summary>
        /// Gets the keys (root nodes) in this tree level.
        /// </summary>
        public IEnumerable<TRoot> Keys => _inner.Keys;

        /// <summary>
        /// Gets the sub-trees in this tree level.
        /// </summary>
        public IEnumerable<TSubTree> Values => _inner.Values;

        /// <summary>
        /// Gets the number of root nodes in this tree level.
        /// </summary>
        public int Count => _inner.Count;

        /// <summary>
        /// Determines whether the tree contains a root node with the specified key.
        /// </summary>
        /// <param name="key">The key to locate.</param>
        /// <returns>true if the tree contains a node with the key; otherwise, false.</returns>
        public bool ContainsKey(TRoot key) => _inner.ContainsKey(key);

        /// <summary>
        /// Returns an enumerator that iterates through the tree.
        /// </summary>
        /// <returns>An enumerator for the tree.</returns>
        public IEnumerator<KeyValuePair<TRoot, TSubTree>> GetEnumerator() => _inner.GetEnumerator();

#pragma warning disable CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
        /// <summary>
        /// Tries to get the sub-tree associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the sub-tree to get.</param>
        /// <param name="value">When this method returns, contains the sub-tree associated with the key, if found; otherwise, the default value.</param>
        /// <returns>true if the sub-tree was found; otherwise, false.</returns>
        public bool TryGetValue(TRoot key, [MaybeNullWhen(false)] out TSubTree value) => _inner.TryGetValue(key, out value);
#pragma warning restore CS8767

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_inner).GetEnumerator();
    }

    /// <summary>
    /// Represents a tree structure where all nodes are of the same type.
    /// </summary>
    /// <typeparam name="TNode">The type of nodes in the tree.</typeparam>
    public class Tree<TNode> : Tree<TNode, Tree<TNode>>
        where TNode : notnull
    {
        /// <summary>
        /// Gets an empty tree instance.
        /// </summary>
        public static new readonly Tree<TNode> Empty = new (ImmutableDictionary<TNode, Tree<TNode>>.Empty);

        /// <summary>
        /// Initializes a new instance of the <see cref="Tree{TNode}"/> class with the specified dictionary.
        /// </summary>
        /// <param name="inner">The dictionary containing the tree structure.</param>
        public Tree(IReadOnlyDictionary<TNode, Tree<TNode>> inner) : base(inner)
        {
        }
    }
}
