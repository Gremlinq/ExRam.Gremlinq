using FluentAssertions;
using Xunit;
using LanguageExt;

namespace ExRam.Gremlinq.Core.Tests
{
    public class GraphModelTest
    {
        private sealed class Vertex
        {
            public object Id { get; set; }
        }

        [Fact]
        public void FromAssembly_includes_abstract_types()
        {
            var model = GraphModel.Dynamic();

            model.VerticesModel.TryGetFilterLabels(typeof(Authority))
                .IfNone(new string[0])
                .Should()
                .NotBeEmpty();
        }

        [Fact]
        public void Relax1()
        {
            GraphModel.Dynamic().Relax()
                .VerticesModel
                .TryGetConstructiveLabel(typeof(Vertex))
                .Should()
                .BeSome("Vertex");
        }

        [Fact]
        public void Relax2()
        {
            GraphModel.Empty.Relax()
                .VerticesModel
                .TryGetConstructiveLabel(typeof(Vertex))
                .Should()
                .BeSome("Vertex");
        }
    }
}
