using FluentAssertions;

namespace ExRam.Gremlinq.Providers.CosmosDb.Tests
{
    public class CosmosDbKeyTests
    {
        [Fact]
        public void Equality_same_id()
        {
            var value1 = "id1";
            var value2 = "id1234".Substring(0, 3);

            ReferenceEquals(value1, value2).Should().BeFalse();

            var key1 = new CosmosDbKey(value1);
            var key2 = new CosmosDbKey(value2);

            key1.Equals(key2).Should().BeTrue();
            (key1 == key2).Should().BeTrue();
        }

        [Fact]
        public void Equality_same_partition_and_id()
        {
            var key1 = new CosmosDbKey("pk", "id");
            var key2 = new CosmosDbKey("pk", "id");

            key1.Equals(key2).Should().BeTrue();
            (key1 == key2).Should().BeTrue();
        }

        [Fact]
        public void Inequality_different_id()
        {
            var key1 = new CosmosDbKey("id1");
            var key2 = new CosmosDbKey("id2");

            (key1 != key2).Should().BeTrue();
            (key1 == key2).Should().BeFalse();
        }

        [Fact]
        public void Inequality_different_partition()
        {
            var key1 = new CosmosDbKey("pk1", "id");
            var key2 = new CosmosDbKey("pk2", "id");

            (key1 != key2).Should().BeTrue();
            key1.Equals(key2).Should().BeFalse();
        }

        [Fact]
        public void Inequality_with_and_without_partition()
        {
            var key1 = new CosmosDbKey("id");
            var key2 = new CosmosDbKey("pk", "id");

            key1.Equals(key2).Should().BeFalse();
        }

        [Fact]
        public void GetHashCode_equal_for_equal_keys()
        {
            var key1 = new CosmosDbKey("pk", "id");
            var key2 = new CosmosDbKey("pk", "id");

            key1.GetHashCode().Should().Be(key2.GetHashCode());
        }

        [Fact]
        public void GetHashCode_different_for_different_keys()
        {
            var key1 = new CosmosDbKey("pk1", "id1");
            var key2 = new CosmosDbKey("pk2", "id2");

            key1.GetHashCode().Should().NotBe(key2.GetHashCode());
        }

        [Fact]
        public void Equals_object_returns_false_for_non_CosmosDbKey()
        {
            var key = new CosmosDbKey("id");

            key.Equals("id").Should().BeFalse();
            key.Equals(null).Should().BeFalse();
        }

        [Fact]
        public void Default_struct_Id_throws()
        {
            var key = default(CosmosDbKey);

            key
                .Invoking(k => _ = k.Id)
                .Should()
                .Throw<InvalidOperationException>();
        }

        [Fact]
        public void Constructor_with_null_id_throws()
        {
            FluentActions.Invoking(() => new CosmosDbKey(null!))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_with_null_partitionKey_throws()
        {
            FluentActions.Invoking(() => new CosmosDbKey(null!, "id"))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_with_null_id_and_partition_throws()
        {
            FluentActions.Invoking(() => new CosmosDbKey("pk", null!))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void Id_returns_value()
        {
            new CosmosDbKey("myId").Id.Should().Be("myId");
        }

        [Fact]
        public void PartitionKey_returns_value()
        {
            new CosmosDbKey("pk", "id").PartitionKey.Should().Be("pk");
        }

        [Fact]
        public void PartitionKey_is_null_without_partition()
        {
            new CosmosDbKey("id").PartitionKey.Should().BeNull();
        }
    }
}
