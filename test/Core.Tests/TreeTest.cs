using System.Collections.Immutable;

using FluentAssertions;

namespace ExRam.Gremlinq.Core.Tests
{
    public class TreeTest
    {
        [Fact]
        public void Empty_tree_has_count_zero()
        {
            var tree = Tree<string, Tree<string>>.Empty;

            tree.Count
                .Should()
                .Be(0);
        }

        [Fact]
        public void Empty_tree_keys_are_empty()
        {
            var tree = Tree<string, Tree<string>>.Empty;

            tree.Keys
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void Empty_tree_values_are_empty()
        {
            var tree = Tree<string, Tree<string>>.Empty;

            tree.Values
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void ContainsKey_returns_false_for_missing_key()
        {
            var tree = Tree<string, Tree<string>>.Empty;

            tree.ContainsKey("missing")
                .Should()
                .BeFalse();
        }

        [Fact]
        public void TryGetValue_returns_false_for_missing_key()
        {
            var tree = Tree<string, Tree<string>>.Empty;

            tree.TryGetValue("missing", out _)
                .Should()
                .BeFalse();
        }

        [Fact]
        public void Tree_with_entries()
        {
            var subtree = Tree<string>.Empty;
            var dict = new Dictionary<string, Tree<string>> { ["root"] = subtree }.ToImmutableDictionary();
            var tree = new Tree<string, Tree<string>>(dict);

            tree.Count
                .Should()
                .Be(1);

            tree["root"]
                .Should()
                .BeSameAs(subtree);

            tree.ContainsKey("root")
                .Should()
                .BeTrue();

            tree.TryGetValue("root", out var value)
                .Should()
                .BeTrue();

            value
                .Should()
                .BeSameAs(subtree);
        }

        [Fact]
        public void GetEnumerator_enumerates_entries()
        {
            var subtree = Tree<string>.Empty;
            var dict = new Dictionary<string, Tree<string>> { ["a"] = subtree, ["b"] = subtree }.ToImmutableDictionary();
            var tree = new Tree<string, Tree<string>>(dict);

            tree
                .Should()
                .HaveCount(2);
        }

        [Fact]
        public void Constructor_throws_on_null()
        {
            var act = () => new Tree<string, Tree<string>>(null!);

            act.Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void Recursive_tree_empty()
        {
            var tree = Tree<int>.Empty;

            tree.Count
                .Should()
                .Be(0);
        }

        [Fact]
        public void Recursive_tree_with_entries()
        {
            var inner = Tree<int>.Empty;
            var dict = new Dictionary<int, Tree<int>> { [1] = inner, [2] = inner }.ToImmutableDictionary();
            var tree = new Tree<int>(dict);

            tree.Count
                .Should()
                .Be(2);

            tree[1]
                .Should()
                .BeSameAs(inner);
        }

        [Fact]
        public void Recursive_tree_constructor_throws_on_null()
        {
            var act = () => new Tree<int>(null!);

            act.Should()
                .Throw<ArgumentNullException>();
        }
    }
}
