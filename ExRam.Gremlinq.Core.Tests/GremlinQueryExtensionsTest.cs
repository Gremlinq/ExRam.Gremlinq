using System;
using ExRam.Gremlinq.Core.GraphElements;
using ExRam.Gremlinq.Tests.Entities;
using FluentAssertions;
using Xunit;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public class GremlinQueryExtensionsTest
    {
        [Fact]
        public void CreateContinuation()
        {
            var query1 = g
                .UseModel(GraphModel.FromBaseTypes<IVertex, IEdge>())
                .UseExecutionPipeline(b => b
                    .EchoGremlinQueryAsString())
                .V()
                .OfType<Person>();

            var query2 = query1
                .Where(x => x.Age == 36);

            var cont = ((IGremlinQuery)query1).CreateContinuationFrom(query2);

            cont(GremlinQuery
                    .Anonymous(query1.AsAdmin().Environment))
                .Should()
                .SerializeToGroovy("__.has(_a, _b)");
        }

        [Fact]
        public void CreateContinuation_incomparable()
        {
            var query1 = g
                .UseModel(GraphModel.FromBaseTypes<IVertex, IEdge>())
                .UseExecutionPipeline(b => b
                    .EchoGremlinQueryAsString())
                .V()
                .OfType<Person>();

            var query2 = g
                .V()
                .OfType<Company>();

            query1
                .Invoking(q => ((IGremlinQuery)q).CreateContinuationFrom(query2))
                .Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public void CreateContinuation_equal()
        {
            var query1 = g
                .UseModel(GraphModel.FromBaseTypes<IVertex, IEdge>())
                .UseExecutionPipeline(b => b
                    .EchoGremlinQueryAsString())
                .V()
                .OfType<Person>();

            var query2 = query1;

            var cont = ((IGremlinQuery)query1).CreateContinuationFrom(query2);

            cont(GremlinQuery
                    .Anonymous(query1.AsAdmin().Environment))
                .Should()
                .SerializeToGroovy("__.identity()");
        }
    }
}
