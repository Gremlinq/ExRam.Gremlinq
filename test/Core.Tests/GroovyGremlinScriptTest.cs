using System.Collections.Immutable;

using ExRam.Gremlinq.Core.Serialization;

using FluentAssertions;

namespace ExRam.Gremlinq.Core.Tests
{
    public class GroovyGremlinScriptTest
    {
        [Fact]
        public void From_creates_script_with_empty_bindings()
        {
            var script = GroovyGremlinScript.From("g.V()");

            script.Script
                .Should()
                .Be("g.V()");

            script.Bindings
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void From_creates_script_with_bindings()
        {
            var bindings = ImmutableDictionary<string, object?>.Empty
                .Add("x", 42);

            var script = GroovyGremlinScript.From("g.V(x)", bindings);

            script.Script
                .Should()
                .Be("g.V(x)");

            script.Bindings
                .Should()
                .ContainKey("x")
                .WhoseValue
                .Should()
                .Be(42);
        }

        [Fact]
        public void Bind_adds_binding()
        {
            var script = GroovyGremlinScript.From("g.V(x)")
                .Bind("x", 42);

            script.Bindings
                .Should()
                .ContainKey("x")
                .WhoseValue
                .Should()
                .Be(42);
        }

        [Fact]
        public void ToString_returns_script()
        {
            var script = GroovyGremlinScript.From("g.V()");

            script.ToString()
                .Should()
                .Be("g.V()");
        }

        [Fact]
        public void Uninitialized_Script_throws()
        {
            var script = default(GroovyGremlinScript);

            script.Invoking(s => s.Script)
                .Should()
                .Throw<InvalidOperationException>();
        }

        [Fact]
        public void Uninitialized_Bindings_throws()
        {
            var script = default(GroovyGremlinScript);

            script.Invoking(s => s.Bindings)
                .Should()
                .Throw<InvalidOperationException>();
        }

        [Fact]
        public void From_throws_on_null_script()
        {
            var act = () => GroovyGremlinScript.From(null!);

            act.Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void Bind_throws_on_null_variable()
        {
            var script = GroovyGremlinScript.From("g.V()");

            script.Invoking(s => s.Bind(null!, 1))
                .Should()
                .Throw<ArgumentNullException>();
        }
    }
}
