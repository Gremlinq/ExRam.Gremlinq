using System;
using ExRam.Gremlinq.Core.GraphElements;
using ExRam.Gremlinq.Tests.Entities;
using FluentAssertions;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public class GremlinQueryExtensionsTest : VerifyBase
    {
        public GremlinQueryExtensionsTest(ITestOutputHelper output) : base(output)
        {

        }

        [Fact]
        public void CreateContinuation()
        {
            var query1 = g
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel.FromBaseTypes<IVertex, IEdge>(lookup => lookup
                        .IncludeAssembliesOfBaseTypes()))
                    .EchoGroovy())
                .V()
                .OfType<Person>();

            var query2 = query1
                .Where(x => x.Age == 36);

            var cont = ((IGremlinQueryBase)query1).CreateContinuationFrom(query2);

            cont(GremlinQuery
                    .Anonymous(query1.AsAdmin().Environment))
                .VerifyQuery(this);
        }

        [Fact]
        public void CreateContinuation_incomparable()
        {
            var query1 = g
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel.FromBaseTypes<IVertex, IEdge>(lookup => lookup
                        .IncludeAssembliesOfBaseTypes()))
                    .EchoGraphson())
                .V()
                .OfType<Person>();

            var query2 = g
                .ConfigureEnvironment(_ => _)
                .V()
                .OfType<Company>();

            query1
                .Invoking(q => ((IGremlinQueryBase)q).CreateContinuationFrom(query2))
                .Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public void CreateContinuation_equal()
        {
            var query1 = g
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel.FromBaseTypes<IVertex, IEdge>(lookup => lookup
                        .IncludeAssembliesOfBaseTypes()))
                    .EchoGroovy())
                .V()
                .OfType<Person>();

            var query2 = query1;

            var cont = ((IGremlinQueryBase)query1).CreateContinuationFrom(query2);

            cont(GremlinQuery
                    .Anonymous(query1.AsAdmin().Environment))
                .VerifyQuery(this);
        }
    }
}
