using System.Dynamic;

using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Tests.Entities;

using FluentAssertions;

using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Support.NewtonsoftJson.Tests
{
    public class DynamicDictionaryTests
    {
        private readonly IGremlinQueryEnvironment _environment;

        public DynamicDictionaryTests()
        {
            _environment = GremlinQueryEnvironment.Invalid
                .UseModel(GraphModel.FromBaseTypes<Vertex, Edge>())
                .UseNewtonsoftJson();
        }

        private dynamic GetDynamic(string json = "{ \"name\": \"test\", \"age\": 42 }")
        {
            var jObject = JObject.Parse(json);

            return _environment
                .Deserializer
                .TryTransformTo<object>().From(jObject, _environment)!;
        }

        [Fact]
        public void Is_DynamicObject()
        {
            var result = GetDynamic();

            ((object)result).Should().BeAssignableTo<DynamicObject>();
        }

        [Fact]
        public void Is_IReadOnlyDictionary()
        {
            var result = GetDynamic();

            ((object)result).Should().BeAssignableTo<IReadOnlyDictionary<string, object?>>();
        }

        [Fact]
        public void Is_IDictionary()
        {
            var result = GetDynamic();

            ((object)result).Should().BeAssignableTo<IDictionary<string, object?>>();
        }

        [Fact]
        public void ReadOnly_Keys()
        {
            IReadOnlyDictionary<string, object?> dict = GetDynamic();

            dict.Keys.Should().Contain("name").And.Contain("age");
        }

        [Fact]
        public void ReadOnly_Values()
        {
            IReadOnlyDictionary<string, object?> dict = GetDynamic();

            dict.Values.Should().HaveCount(2);
        }

        [Fact]
        public void ReadOnly_Count()
        {
            IReadOnlyDictionary<string, object?> dict = GetDynamic();

            dict.Count.Should().Be(2);
        }

        [Fact]
        public void ReadOnly_Indexer()
        {
            IReadOnlyDictionary<string, object?> dict = GetDynamic();

            dict["name"].Should().Be("test");
        }

        [Fact]
        public void ReadOnly_ContainsKey()
        {
            IReadOnlyDictionary<string, object?> dict = GetDynamic();

            dict.ContainsKey("name").Should().BeTrue();
            dict.ContainsKey("nonexistent").Should().BeFalse();
        }

        [Fact]
        public void ReadOnly_TryGetValue()
        {
            IReadOnlyDictionary<string, object?> dict = GetDynamic();

            dict.TryGetValue("name", out var value).Should().BeTrue();
            value.Should().Be("test");

            dict.TryGetValue("nonexistent", out _).Should().BeFalse();
        }

        [Fact]
        public void ReadOnly_GetEnumerator()
        {
            IReadOnlyDictionary<string, object?> dict = GetDynamic();

            var items = dict.ToList();
            items.Should().HaveCount(2);
        }

        [Fact]
        public void Mutable_Keys()
        {
            IDictionary<string, object?> dict = GetDynamic();

            dict.Keys.Should().Contain("name").And.Contain("age");
        }

        [Fact]
        public void Mutable_Values()
        {
            IDictionary<string, object?> dict = GetDynamic();

            dict.Values.Should().HaveCount(2);
        }

        [Fact]
        public void Mutable_Count()
        {
            ICollection<KeyValuePair<string, object?>> collection = GetDynamic();

            collection.Count.Should().Be(2);
        }

        [Fact]
        public void Mutable_IsReadOnly()
        {
            ICollection<KeyValuePair<string, object?>> collection = GetDynamic();

            collection.IsReadOnly.Should().BeFalse();
        }

        [Fact]
        public void Mutable_Indexer_get()
        {
            IDictionary<string, object?> dict = GetDynamic();

            dict["name"].Should().Be("test");
        }

        [Fact]
        public void Mutable_Indexer_set()
        {
            IDictionary<string, object?> dict = GetDynamic();

            dict["name"] = "updated";
            dict["name"].Should().Be("updated");
        }

        [Fact]
        public void Mutable_Add()
        {
            IDictionary<string, object?> dict = GetDynamic();

            dict.Add("newKey", "newValue");
            dict["newKey"].Should().Be("newValue");
        }

        [Fact]
        public void Mutable_ContainsKey()
        {
            IDictionary<string, object?> dict = GetDynamic();

            dict.ContainsKey("name").Should().BeTrue();
            dict.ContainsKey("nonexistent").Should().BeFalse();
        }

        [Fact]
        public void Mutable_Remove()
        {
            IDictionary<string, object?> dict = GetDynamic();

            dict.Remove("name").Should().BeTrue();
            dict.ContainsKey("name").Should().BeFalse();
        }

        [Fact]
        public void Mutable_TryGetValue()
        {
            IDictionary<string, object?> dict = GetDynamic();

            dict.TryGetValue("name", out var value).Should().BeTrue();
            value.Should().Be("test");

            dict.TryGetValue("nonexistent", out _).Should().BeFalse();
        }

        [Fact]
        public void Collection_Add_pair()
        {
            ICollection<KeyValuePair<string, object?>> collection = GetDynamic();

            collection.Add(new KeyValuePair<string, object?>("extra", "val"));
            collection.Count.Should().Be(3);
        }

        [Fact]
        public void Collection_Clear()
        {
            ICollection<KeyValuePair<string, object?>> collection = GetDynamic();

            collection.Clear();
            collection.Count.Should().Be(0);
        }

        [Fact]
        public void Collection_Contains()
        {
            IDictionary<string, object?> dict = GetDynamic();
            ICollection<KeyValuePair<string, object?>> collection = (ICollection<KeyValuePair<string, object?>>)dict;

            collection.Contains(new KeyValuePair<string, object?>("name", "test")).Should().BeTrue();
        }

        [Fact]
        public void Collection_CopyTo()
        {
            ICollection<KeyValuePair<string, object?>> collection = GetDynamic();

            var array = new KeyValuePair<string, object?>[2];
            collection.CopyTo(array, 0);

            array.Should().HaveCount(2);
        }

        [Fact]
        public void Collection_Remove_pair()
        {
            IDictionary<string, object?> dict = GetDynamic();
            ICollection<KeyValuePair<string, object?>> collection = (ICollection<KeyValuePair<string, object?>>)dict;

            collection.Remove(new KeyValuePair<string, object?>("name", "test")).Should().BeTrue();
        }

        [Fact]
        public void NonGeneric_GetEnumerator()
        {
            var result = GetDynamic();
            var enumerable = (System.Collections.IEnumerable)result;

            var enumerator = enumerable.GetEnumerator();
            enumerator.MoveNext().Should().BeTrue();
        }

        [Fact]
        public void Dynamic_TrySetMember()
        {
            dynamic result = GetDynamic();

            result.newProp = "hello";

            IDictionary<string, object?> dict = result;
            dict["newProp"].Should().Be("hello");
        }

        [Fact]
        public void Dynamic_TryGetMember()
        {
            dynamic result = GetDynamic();

            string name = result.name;
            name.Should().Be("test");
        }

        [Fact]
        public void Dynamic_TryGetMember_nonexistent()
        {
            dynamic result = GetDynamic();

            FluentActions.Invoking(() =>
            {
                object? _ = result.nonexistent;
            })
            .Should()
            .Throw<Microsoft.CSharp.RuntimeBinder.RuntimeBinderException>();
        }
    }
}
