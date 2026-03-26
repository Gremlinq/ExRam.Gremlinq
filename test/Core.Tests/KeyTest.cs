using Gremlin.Net.Process.Traversal;

using FluentAssertions;

namespace ExRam.Gremlinq.Core.Tests
{
    public class KeyTest
    {
        [Fact]
        public void From_string()
        {
            Key key = "name";

            key.RawKey
                .Should()
                .Be("name");
        }

        [Fact]
        public void From_T()
        {
            Key key = T.Id;

            key.RawKey
                .Should()
                .Be(T.Id);
        }

        [Fact]
        public void Equality_same_string()
        {
            Key key1 = "name";
            Key key2 = "name";

            key1.Equals(key2)
                .Should()
                .BeTrue();

            (key1 == key2)
                .Should()
                .BeTrue();
        }

        [Fact]
        public void Inequality_different_strings()
        {
            Key key1 = "name";
            Key key2 = "age";

            (key1 != key2)
                .Should()
                .BeTrue();
        }

        [Fact]
        public void Equality_same_T()
        {
            Key key1 = T.Id;
            Key key2 = T.Id;

            key1.Equals(key2)
                .Should()
                .BeTrue();
        }

        [Fact]
        public void Equals_object_with_key()
        {
            Key key1 = "name";
            object key2 = (Key)"name";

            key1.Equals(key2)
                .Should()
                .BeTrue();
        }

        [Fact]
        public void Equals_object_non_key()
        {
            Key key1 = "name";

            key1.Equals((object)42)
                .Should()
                .BeFalse();
        }

        [Fact]
        public void GetHashCode_same_for_equal_keys()
        {
            Key key1 = "name";
            Key key2 = "name";

            key1.GetHashCode()
                .Should()
                .Be(key2.GetHashCode());
        }

        [Fact]
        public void CompareTo_T_vs_T()
        {
            Key key1 = T.Id;
            Key key2 = T.Label;

            key1.CompareTo(key2)
                .Should()
                .NotBe(0);
        }

        [Fact]
        public void CompareTo_string_vs_string()
        {
            Key key1 = "alpha";
            Key key2 = "beta";

            key1.CompareTo(key2)
                .Should()
                .BeNegative();
        }

        [Fact]
        public void CompareTo_T_before_string()
        {
            Key key1 = T.Id;
            Key key2 = "name";

            key1.CompareTo(key2)
                .Should()
                .BeNegative();
        }

        [Fact]
        public void CompareTo_string_after_T()
        {
            Key key1 = "name";
            Key key2 = T.Id;

            key1.CompareTo(key2)
                .Should()
                .BePositive();
        }

        [Fact]
        public void Uninitialized_RawKey_throws()
        {
            var key = default(Key);

            key.Invoking(k => k.RawKey)
                .Should()
                .Throw<InvalidOperationException>();
        }

        [Fact]
        public void CompareTo_null_vs_null()
        {
            var key1 = default(Key);
            var key2 = default(Key);

            key1.CompareTo(key2)
                .Should()
                .Be(0);
        }

        [Fact]
        public void CompareTo_null_before_value()
        {
            var key1 = default(Key);
            Key key2 = "name";

            key1.CompareTo(key2)
                .Should()
                .BeNegative();
        }
    }
}
