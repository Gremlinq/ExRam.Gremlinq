using ExRam.Gremlinq.Core.GraphElements;

using FluentAssertions;

namespace ExRam.Gremlinq.Core.Tests
{
    public class PropertyTests
    {
        [Fact]
        public void Constructor_sets_value()
        {
            var prop = new Property<string>("hello");

            prop.Value.Should().Be("hello");
        }

        [Fact]
        public void Constructor_null_value_throws()
        {
            FluentActions.Invoking(() => new Property<string>(null!))
                .Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Value_setter_null_throws()
        {
            var prop = new Property<string>("hello");

            prop.Invoking(p => p.Value = null!)
                .Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Value_setter_updates_value()
        {
            var prop = new Property<string>("hello");

            prop.Value = "world";

            prop.Value.Should().Be("world");
        }

        [Fact]
        public void ToString_format()
        {
            var prop = new Property<int>(42);

            prop.ToString().Should().Contain("42");
            prop.ToString().Should().StartWith("p[");
        }

        [Fact]
        public void Implicit_conversion_from_value()
        {
            Property<string> prop = "test";

            prop.Value.Should().Be("test");
        }

        [Fact]
        public void Implicit_conversion_from_array_throws()
        {
            FluentActions.Invoking(() =>
            {
                Property<string> _ = new[] { "a", "b" };
            })
            .Should().Throw<NotSupportedException>();
        }

        [Fact]
        public void Implicit_conversion_from_property_array_throws()
        {
            FluentActions.Invoking(() =>
            {
                Property<string> _ = new Property<string>[] { new("a"), new("b") };
            })
            .Should().Throw<NotSupportedException>();
        }
    }

    public class VertexPropertyTests
    {
        [Fact]
        public void Constructor_sets_value()
        {
            var vp = new VertexProperty<string>("hello");

            vp.Value.Should().Be("hello");
        }

        [Fact]
        public void Constructor_initializes_dictionary()
        {
            var vp = new VertexProperty<string>("hello");

            vp.Properties.Should().NotBeNull();
            vp.Properties.Should().BeEmpty();
        }

        [Fact]
        public void ToString_format()
        {
            var vp = new VertexProperty<int>(42);

            vp.ToString().Should().Contain("42");
            vp.ToString().Should().StartWith("vp[");
        }

        [Fact]
        public void Implicit_conversion_from_value()
        {
            VertexProperty<string> vp = "test";

            vp.Value.Should().Be("test");
        }

        [Fact]
        public void Implicit_conversion_from_array_throws()
        {
            FluentActions.Invoking(() =>
            {
                VertexProperty<string> _ = new[] { "a", "b" };
            })
            .Should().Throw<NotSupportedException>();
        }

        [Fact]
        public void Implicit_conversion_from_vp_array_throws()
        {
            FluentActions.Invoking(() =>
            {
                VertexProperty<string> _ = new VertexProperty<string>[] { new("a") };
            })
            .Should().Throw<NotSupportedException>();
        }
    }

    public class VertexPropertyWithMetaTests
    {
        private sealed class Meta
        {
            public string? Description { get; set; }
        }

        [Fact]
        public void Constructor_sets_value()
        {
            var vp = new VertexProperty<string, Meta>("hello");

            vp.Value.Should().Be("hello");
        }

        [Fact]
        public void ToString_format()
        {
            var vp = new VertexProperty<int, Meta>(42);

            vp.ToString().Should().Contain("42");
            vp.ToString().Should().StartWith("vp[");
        }

        [Fact]
        public void Implicit_conversion_from_value()
        {
            VertexProperty<string, Meta> vp = "test";

            vp.Value.Should().Be("test");
        }

        [Fact]
        public void Implicit_conversion_from_array_throws()
        {
            FluentActions.Invoking(() =>
            {
                VertexProperty<string, Meta> _ = new[] { "a", "b" };
            })
            .Should().Throw<NotSupportedException>();
        }

        [Fact]
        public void Implicit_conversion_from_vp_array_throws()
        {
            FluentActions.Invoking(() =>
            {
                VertexProperty<string, Meta> _ = new VertexProperty<string, Meta>[] { new("a") };
            })
            .Should().Throw<NotSupportedException>();
        }

        [Fact]
        public void Properties_can_be_set()
        {
            var vp = new VertexProperty<string, Meta>("hello");
            var meta = new Meta { Description = "desc" };

            vp.Properties = meta;

            vp.Properties.Should().BeSameAs(meta);
        }
    }

    public class PathTests
    {
        [Fact]
        public void Default_Labels_is_empty()
        {
            var path = new GraphElements.Path();

            path.Labels.Should().BeEmpty();
        }

        [Fact]
        public void Default_Objects_is_empty()
        {
            var path = new GraphElements.Path();

            path.Objects.Should().BeEmpty();
        }

        [Fact]
        public void Labels_can_be_set()
        {
            var path = new GraphElements.Path
            {
                Labels = [["a", "b"], ["c"]]
            };

            path.Labels.Should().HaveCount(2);
            path.Labels[0].Should().ContainInOrder("a", "b");
        }

        [Fact]
        public void Objects_can_be_set()
        {
            var path = new GraphElements.Path
            {
                Objects = [1, "two", 3.0]
            };

            path.Objects.Should().HaveCount(3);
        }
    }
}
