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
                .WithModel(GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple));
        }

        [Fact]
        public void AddV()
        {
            var query = GremlinQuery
                .Create("g", this._queryProvider)
                .AddV(new Language { Id = "id", IetfLanguageTag = "en" })
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.addV('Language').property('Id', 'id').property('IetfLanguageTag', 'en')");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void AddV_with_nulls()
        {
            var query = GremlinQuery
                .Create("g", this._queryProvider)
                .AddV(new Language {Id = "id"})
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.addV('Language').property('Id', 'id')");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType()
        {
            var query = GremlinQuery
                .Create("g", this._queryProvider)
                .V<User>()
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('User')");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType_does_not_include_abstract_types()
        {
            var query = GremlinQuery
                .Create("g", this._queryProvider)
                .V<Authority>()
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('Company', 'User')");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType_has_int_property()
        {
            var query = GremlinQuery
                .Create("g", this._queryProvider)
                .V<User>()
                .Where(t => t.Age == 36)
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('User').has('Age', eq(36))");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType_has_converted_int_property()
        {
            var query = GremlinQuery
                .Create("g", this._queryProvider)
                .V<User>()
                .Where(t => (object)t.Age == (object)36)
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('User').has('Age', eq(36))");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType_has_unequal_int_property()
        {
            var query = GremlinQuery
                .Create("g", this._queryProvider)
                .V<User>()
                .Where(t => t.Age != 36)
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('User').has('Age', neq(36))");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType_has_no_string_property()
        {
            var query = GremlinQuery
                .Create("g", this._queryProvider)
                .V<User>()
                .Where(t => t.Name == null)
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('User').not(__.has('Name'))");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType_string_property_exists()
        {
            var query = GremlinQuery
                .Create("g", this._queryProvider)
                .V<User>()
                .Where(t => t.Name != null)
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('User').has('Name')");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType_has_lower_int_property()
        {
            var query = GremlinQuery
                .Create("g", this._queryProvider)
                .V<User>()
                .Where(t => t.Age < 36)
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('User').has('Age', lt(36))");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType_has_lower_or_equal_int_property()
        {
            var query = GremlinQuery
                .Create("g", this._queryProvider)
                .V<User>()
                .Where(t => t.Age <= 36)
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('User').has('Age', lte(36))");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType_has_greater_int_property()
        {
            var query = GremlinQuery
                .Create("g", this._queryProvider)
                .V<User>()
                .Where(t => t.Age > 36)
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('User').has('Age', gt(36))");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType_has_greater_or_equal_int_property()
        {
            var query = GremlinQuery
                .Create("g", this._queryProvider)
                .V<User>()
                .Where(t => t.Age >= 36)
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('User').has('Age', gte(36))");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType_has_string_property()
        {
            var query = GremlinQuery
                .Create("g", this._queryProvider)
                .V<Language>()
                .Where(t => t.Id == "languageId")
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('Language').has('Id', eq('languageId'))");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType_where_with_local_string()
        {
            var local = "languageId";

            var query = GremlinQuery
                .Create("g", this._queryProvider)
                .V<Language>()
                .Where(t => t.Id == local)
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('Language').has('Id', eq('languageId'))");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType_where_with_local_anonymous_type()
        {
            var local = new { Value = "languageId" };

            var query = GremlinQuery
                .Create("g", this._queryProvider)
                .V<Language>()
                .Where(t => t.Id == local.Value)
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('Language').has('Id', eq('languageId'))");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_of_type_where_with_expression_parameter_on_both_sides()
        {
            GremlinQuery
                .Create("g", this._queryProvider)
                .V<Language>()
                .Invoking(query => query.Where(t => t.Id == t.IetfLanguageTag))
                .ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void V_of_type_where_with_stepLabel()
        {
            var l = new StepLabel<Language>("l");

            var query = GremlinQuery
                .Create("g", this._queryProvider)
                .V<Language>()
                .As(l)
                .V<Language>()
                .Where(l2 => l2 == l)
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('Language').as('l').V().hasLabel('Language').where(eq('l'))");
        }

        [Fact]
        public void V_of_type_where_with_stepLabel_not_inlined()
        {
            var l = new StepLabel<Language>("l");

            var query = GremlinQuery
                .Create("g", this._queryProvider)
                .V<Language>()
                .As(l)
                .V<Language>()
                .Where(l2 => l2 == l)
                .Serialize(false);

            query.queryString
                .Should()
                .Be("g.V().hasLabel(P1).as(P2).V().hasLabel(P1).where(eq(P2))");

            query
                .parameters
                .Should()
                .Contain("P1", "Language").And
                .Contain("P2", "l");
        }

        [Fact]
        public void AddE_to_traversal()
        {
            var query = GremlinQuery
                .Create("g", this._queryProvider)
                .AddV(new User { Name = "Bob" })
                .AddE(new LivesIn())
                .To(__ => __
                    .V<Country>()
                    .Where(t => t.CountryCallingCode == "+49"))
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.addV('User').property('Age', 0).property('Name', 'Bob').addE('LivesIn').to(__.V().hasLabel('Country').has('CountryCallingCode', eq('+49')))");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void AddE_to_StepLabel()
        {
            var query = GremlinQuery
                .Create("g", this._queryProvider)
                .AddV(new Language { IetfLanguageTag = "en" })
                .As((_, l) => _
                    .AddV(new Country { CountryCallingCode = "+49" })
                    .AddE(new IsDescribedIn { Text = "Germany" })
                    .To(l))
                .Serialize(false);

            query.queryString
                .Should()
                .Be("g.addV(P1).property(P2, P3).as(P4).addV(P5).property(P6, P7).addE(P8).property(P9, P10).to(P4)");

            query.parameters
                .Should()
                .NotBeEmpty();
        }

        [Fact]
        public void AddE_from_StepLabel()
        {
            var query = GremlinQuery
                .Create("g", this._queryProvider)
                .AddV(new Country { CountryCallingCode = "+49" })
                .As((_, c) => _
                    .AddV(new Language { IetfLanguageTag = "en" })
                    .AddE(new IsDescribedIn { Text = "Germany" })
                    .From(c))
                .Serialize(false);

            query.queryString
                .Should()
                .Be("g.addV(P1).property(P2, P3).as(P4).addV(P5).property(P6, P7).addE(P8).property(P9, P10).from(P4)");

            query.parameters
                .Should()
                .NotBeEmpty();
        }

        [Fact]
        public void And()
        {
            var query = GremlinQuery
                .Create("g", this._queryProvider)
                .V<User>()
                .And(
                    __ => __
                        .InE<Knows>(),
                    __ => __
                        .OutE<LivesIn>())
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('User').and(__.inE('Knows'), __.outE('LivesIn'))");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void Drop()
        {
            var query = GremlinQuery
                .Create("g", this._queryProvider)
                .V<User>()
                .Drop()
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('User').drop()");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void FilterWithLambda()
        {
            var query = GremlinQuery
                .Create("g", this._queryProvider)
                .V<User>()
                .FilterWithLambda("it.property('str').value().length() == 2")
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('User').filter({it.property('str').value().length() == 2})");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void Out()
        {
            var query = GremlinQuery
                .Create("g", this._queryProvider)
                .V<User>()
                .Out<Knows>()
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('User').out('Knows')");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void Out_does_not_include_abstract_edge()
        {
            var query = GremlinQuery
                .Create("g", this._queryProvider)
                .V<User>()
                .Out<Edge>()
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('User').out('IsDescribedIn', 'Knows', 'LivesIn', 'Speaks', 'WorksFor')");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType_order_ByMember()
        {
            var query = GremlinQuery
                .Create("g", this._queryProvider)
                .V<User>()
                .Order()
                .ByMember(x => x.Name)
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('User').order().by('Name', incr)");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType_order_ByTraversal()
        {
            var query = GremlinQuery
                .Create("g", this._queryProvider)
                .V<User>()
                .Order()
                .ByTraversal(__ => __.Values(x => x.Name))
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('User').order().by(__.values('Name'), incr)");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType_order_ByLambda()
        {
            var query = GremlinQuery
                .Create("g", this._queryProvider)
                .V<User>()
                .Order()
                .ByLambda("it.property('str').value().length()")
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('User').order().by({it.property('str').value().length()})");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType_values_of_one_property()
        {
            var query = GremlinQuery
                .Create("g", this._queryProvider)
                .V<User>()
                .Values(x => x.Name)
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('User').values('Name')");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType_values_of_two_properties()
        {
            var query = GremlinQuery
                .Create("g", this._queryProvider)
                .V<User>()
                .Values(x => x.Name, x => x.Id)
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('User').values('Name', 'Id')");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_without_type()
        {
            var query = GremlinQuery
                .Create("g", this._queryProvider)
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
                .Create("g", this._queryProvider)
                .V()
                .OfType<Authority>()
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('Company', 'User')");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType_Repeat_out_traversal()
        {
            var query = GremlinQuery
                .Create("g", this._queryProvider)
                .V<User>()
                .Repeat(__ => __.Out<Knows>())
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('User').repeat(__.out('Knows'))");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType_Union_two_out_traversals()
        {
            var query = GremlinQuery
                .Create("g", this._queryProvider)
                .V<User>()
                .Union(
                    __ => __.Out<Knows>(),
                    __ => __.Out<LivesIn>())
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('User').union(__.out('Knows'), __.out('LivesIn'))");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType_Optional_one_out_traversal()
        {
            var query = GremlinQuery
                .Create("g", this._queryProvider)
                .V()
                .Optional(
                    __ => __.Out<Knows>())
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().optional(__.out('Knows'))");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType_Not_one_out_traversal()
        {
            var query = GremlinQuery
                .Create("g", this._queryProvider)
                .V()
                .Not(__ => __.Out<Knows>())
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().not(__.out('Knows'))");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType_Optional_one_out_traversal_1()
        {
            var query = GremlinQuery
                .Create("g", this._queryProvider)
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
                .Create("g", this._queryProvider)
                .V()
                .Not(__ => __.OfType<Authority>())
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().not(__.hasLabel('Company', 'User'))");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_as()
        {
            var query = GremlinQuery
                .Create("g", this._queryProvider)
                .V<User>()
                .As(new StepLabel<User>("a"))
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('User').as('a')");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_as_not_inlined()
        {
            var query = GremlinQuery
                .Create("g", this._queryProvider)
                .V<User>()
                .As(new StepLabel<User>("a"))
                .Serialize(false);

            query.queryString
                .Should()
                .Be("g.V().hasLabel(P1).as(P2)");

            query.parameters
                .Should()
                .Contain("P1", "User")
                .And
                .Contain("P2", "a");
        }

        [Fact]
        public void V_as_select()
        {
            var stepLabel = new StepLabel<User>("a");

            var query = GremlinQuery
                .Create("g", this._queryProvider)
                .V<User>()
                .As(stepLabel)
                .Select(stepLabel)
                .Serialize(true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('User').as('a').select('a')");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_as_select_not_inlined()
        {
            var stepLabel = new StepLabel<User>("a");

            var query = GremlinQuery
                .Create("g", this._queryProvider)
                .V<User>()
                .As(stepLabel)
                .Select(stepLabel)
                .Serialize(false);

            query.queryString
                .Should()
                .Be("g.V().hasLabel(P1).as(P2).select(P2)");

            query.parameters
                .Should()
                .Contain("P1", "User")
                .And
                .Contain("P2", "a");
        }

        [Fact]
        public void Branch()
        {
            var query = GremlinQuery
                .Create("g", this._queryProvider)
                .V<User>()
                .Branch(
                    _ => _.Values(x => x.Name),
                    _ => _.Out<Knows>(),
                    _ => _.In<Knows>())
                .Serialize(false);

            query.queryString
                .Should()
                .Be("g.V().hasLabel(P1).branch(__.values(P2)).option(__.out(P3)).option(__.in(P3))");
        }

        [Fact]
        public void BranchOnIdentity()
        {
            var query = GremlinQuery
                .Create("g", this._queryProvider)
                .V<User>()
                .BranchOnIdentity(
                    _ => _.Out<Knows>(),
                    _ => _.In<Knows>())
                .Serialize(false);

            query.queryString
                .Should()
                .Be("g.V().hasLabel(P1).branch(__.identity()).option(__.out(P2)).option(__.in(P2))");
        }
    }
}