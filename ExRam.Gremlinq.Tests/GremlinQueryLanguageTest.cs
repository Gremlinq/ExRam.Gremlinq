using System;
using System.Reflection;
using FluentAssertions;
using Moq;
using Xunit;

namespace ExRam.Gremlinq.Tests
{
    public class GremlinQueryLanguageTest
    {
        private readonly IGremlinQueryProvider _queryProvider;

        public GremlinQueryLanguageTest()
        {
            this._queryProvider = Mock
                .Of<IGremlinQueryProvider>()
                .WithModel(GremlinModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge)))
                .WithNamingStrategy(GraphElementNamingStrategy.Simple);
        }

        [Fact]
        public void AddV()
        {
            var query = GremlinQuery
                .ForGraph("g", this._queryProvider)
                .AddV(new Description { Id = "id", Value = "A description of something." })
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.addV('Description').property('Id', 'id').property('Value', 'A description of something.')");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType()
        {
            var query = GremlinQuery
                .ForGraph("g", this._queryProvider)
                .V<SomeDerivedEntity>()
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('SomeDerivedEntity')");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType_has_int_property()
        {
            var query = GremlinQuery
                .ForGraph("g", this._queryProvider)
                .V<SomeDerivedEntity>()
                    .Has(t => t.SomeIntProperty == 36)
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('SomeDerivedEntity').has('SomeIntProperty', 36)");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType_has_string_property()
        {
            var query = GremlinQuery
                .ForGraph("g", this._queryProvider)
                .V<Language>()
                .Has(t => t.Id == "languageId")
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('Language').has('Id', 'languageId')");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType_has_with_local_string()
        {
            var local = "languageId";

            var query = GremlinQuery
                .ForGraph("g", this._queryProvider)
                .V<Language>()
                .Has(t => t.Id == local)
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('Language').has('Id', 'languageId')");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType_has_with_local_anonymous_type()
        {
            var local = new { Value = "languageId" };

            var query = GremlinQuery
                .ForGraph("g", this._queryProvider)
                .V<Language>()
                .Has(t => t.Id == local.Value)
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('Language').has('Id', 'languageId')");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_of_type_has_with_expression_parameter_on_both_sides()
        {
            GremlinQuery
                .ForGraph("g", this._queryProvider)
                .V<Language>()
                .Invoking(query => query.Has(t => t.Id == t.IetfLanguageTag))
                .ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void AddE_to_traversal()
        {
            var query = GremlinQuery
                .ForGraph("g", this._queryProvider)
                .AddE(new Describes())
                .To(__ => __
                    .V<SomeDerivedEntity>()
                    .Has(t => t.Id == "entityId"))
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.addE('Describes').to(__.V().hasLabel('SomeDerivedEntity').has('Id', 'entityId'))");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void And()
        {
            var query = GremlinQuery
                .ForGraph("g", this._queryProvider)
                .V<SomeDerivedEntity>()
                .And(
                    __ => __
                        .InE<Describes>(),
                    __ => __
                        .OutE<IsLocalizedIn>())
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('SomeDerivedEntity').and(__.inE('Describes'), __.outE('IsLocalizedIn'))");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void Drop()
        {
            var query = GremlinQuery
                .ForGraph("g", this._queryProvider)
                .V<SomeDerivedEntity>()
                .Drop()
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('SomeDerivedEntity').drop()");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void FilterWithLambda()
        {
            var query = GremlinQuery
                .ForGraph("g", this._queryProvider)
                .V<SomeDerivedEntity>()
                .FilterWithLambda("it.property('str').value().length() == 2")
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('SomeDerivedEntity').filter({it.property('str').value().length() == 2})");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType_order_ByMember()
        {
            var query = GremlinQuery
                .ForGraph("g", this._queryProvider)
                .V<SomeDerivedEntity>()
                .Order()
                .ByMember(x => x.Name)
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('SomeDerivedEntity').order().by('Name', incr)");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType_order_ByTraversal()
        {
            var query = GremlinQuery
                .ForGraph("g", this._queryProvider)
                .V<SomeDerivedEntity>()
                .Order()
                .ByTraversal(__ => __.Values(x => x.Name))
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('SomeDerivedEntity').order().by(__.values('Name'), incr)");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType_order_ByLambda()
        {
            var query = GremlinQuery
                .ForGraph("g", this._queryProvider)
                .V<SomeDerivedEntity>()
                .Order()
                .ByLambda("it.property('str').value().length()")
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('SomeDerivedEntity').order().by({it.property('str').value().length()})");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType_values_of_one_property()
        {
            var query = GremlinQuery
                .ForGraph("g", this._queryProvider)
                .V<SomeDerivedEntity>()
                .Values(x => x.Name)
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('SomeDerivedEntity').values('Name')");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType_values_of_two_properties()
        {
            var query = GremlinQuery
                .ForGraph("g", this._queryProvider)
                .V<SomeDerivedEntity>()
                .Values(x => x.Name, x => x.Id)
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('SomeDerivedEntity').values('Name', 'Id')");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_without_type()
        {
            var query = GremlinQuery
                .ForGraph("g", this._queryProvider)
                .V()
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V()");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_OfType_with_inheritance()
        {           
            var query = GremlinQuery
                .ForGraph("g", this._queryProvider)
                .V()
                .OfType<SomeBaseEntity>()
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('SomeBaseEntity', 'SomeDerivedEntity')");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType_Repeat_out_traversal()
        {
            var query = GremlinQuery
                .ForGraph("g", this._queryProvider)
                .V<SomeDerivedEntity>()
                .Repeat(__ => __.Out<Describes>())
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('SomeDerivedEntity').repeat(__.out('Describes'))");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType_Union_two_out_traversals()
        {
            var query = GremlinQuery
                .ForGraph("g", this._queryProvider)
                .V<SomeDerivedEntity>()
                .Union(
                    __ => __.Out<Describes>(),
                    __ => __.Out<IsLocalizedIn>())
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('SomeDerivedEntity').union(__.out('Describes'), __.out('IsLocalizedIn'))");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType_Optional_one_out_traversal()
        {
            var query = GremlinQuery
                .ForGraph("g", this._queryProvider)
                .V()
                .Optional(
                    __ => __.Out<Describes>())
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().optional(__.out('Describes'))");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType_Not_one_out_traversal()
        {
            var query = GremlinQuery
                .ForGraph("g", this._queryProvider)
                .V()
                .Not(__ => __.Out<Describes>())
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().not(__.out('Describes'))");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType_Optional_one_out_traversal_1()
        {
            var query = GremlinQuery
                .ForGraph("g", this._queryProvider)
                .V()
                .Not(__ => __.OfType<Language>())
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().not(__.hasLabel('Language'))");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType_Optional_one_out_traversal_2()
        {
            var query = GremlinQuery
                .ForGraph("g", this._queryProvider)
                .V()
                .Not(__ => __.OfType<SomeBaseEntity>())
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().not(__.hasLabel('SomeBaseEntity', 'SomeDerivedEntity'))");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_as()
        {
            var query = GremlinQuery
                .ForGraph("g", this._queryProvider)
                .V<SomeDerivedEntity>()
                .As(new StepLabel<SomeDerivedEntity>("a"))
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('SomeDerivedEntity').as('a')");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_as_not_inlined()
        {
            var query = GremlinQuery
                .ForGraph("g", this._queryProvider)
                .V<SomeDerivedEntity>()
                .As(new StepLabel<SomeDerivedEntity>("a"))
                .Serialize(false);

            query.queryString
                .Should()
                .Be("g.V().hasLabel(P1).as(P2)");

            query.parameters
                .Should()
                .Contain("P1", "SomeDerivedEntity")
                .And
                .Contain("P2", "a");
        }

        [Fact]
        public void V_as_select()
        {
            var stepLabel = new StepLabel<SomeDerivedEntity>("a");

            var query = GremlinQuery
                .ForGraph("g", this._queryProvider)
                .V<SomeDerivedEntity>()
                .As(stepLabel)
                .Select(stepLabel)
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('SomeDerivedEntity').as('a').select('a')");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_as_select_not_inlined()
        {
            var stepLabel = new StepLabel<SomeDerivedEntity>("a");

            var query = GremlinQuery
                .ForGraph("g", this._queryProvider)
                .V<SomeDerivedEntity>()
                .As(stepLabel)
                .Select(stepLabel)
                .Serialize(false);

            query.queryString
                .Should()
                .Be("g.V().hasLabel(P1).as(P2).select(P3)");

            query.parameters
                .Should()
                .Contain("P1", "SomeDerivedEntity")
                .And
                .Contain("P2", "a")
                .And
                .Contain("P3", "a"); ;
        }
    }
}
