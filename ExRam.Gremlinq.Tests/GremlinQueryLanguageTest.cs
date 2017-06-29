using System;
using System.Reflection;
using FluentAssertions;
using Moq;
using Xunit;

namespace ExRam.Gremlinq.Tests
{
    public class GremlinQueryLanguageTest
    {
        private readonly IGraphModel _model;
        private readonly IGremlinQueryProvider _queryProvider;

        public GremlinQueryLanguageTest()
        {
            this._model = GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple);
            
            this._queryProvider = Mock
                .Of<IGremlinQueryProvider>()
                .WithModel(this._model);
        }

        [Fact]
        public void AddV()
        {
            var query = GremlinQuery
                .Create("g")
                .AddV(new Language { Id = "id", IetfLanguageTag = "en" })
                .Serialize(this._model, true);

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
                .Create("g")
                .AddV(new Language {Id = "id"})
                .Serialize(this._model, true);

            query.queryString
                .Should()
                .Be("g.addV('Language').property('Id', 'id')");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType_has_disjunction()
        {
            var query = GremlinQuery
                .Create("g")
                .V<User>()
                .Where(t => t.Age == 36 || t.Age == 42)
                .Serialize(this._model, true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('User').or(__.has('Age', eq(36)), __.has('Age', eq(42)))");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType_has_disjunction_with_different_fields()
        {
            var query = GremlinQuery
                .Create("g")
                .V<User>()
                .Where(t => t.Name == "Some name" || t.Age == 42)
                .Serialize(this._model, true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('User').or(__.has('Name', eq('Some name')), __.has('Age', eq(42)))");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType_has_conjunction()
        {
            var query = GremlinQuery
                .Create("g")
                .V<User>()
                .Where(t => t.Age == 36 && t.Age == 42)
                .Serialize(this._model, true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('User').and(__.has('Age', eq(36)), __.has('Age', eq(42)))");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType_complex_logical_expression()
        {
            var query = GremlinQuery
                .Create("g")
                .V<User>()
                .Where(t => t.Name == "Some name" && (t.Age == 42 || t.Age == 99))
                .Serialize(this._model, true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('User').and(__.has('Name', eq('Some name')), __.or(__.has('Age', eq(42)), __.has('Age', eq(99))))");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType_complex_logical_expression_with_null()
        {
            var query = GremlinQuery
                .Create("g")
                .V<User>()
                .Where(t => t.Name == null && (t.Age == 42 || t.Age == 99))
                .Serialize(this._model, true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('User').and(__.hasNot('Name'), __.or(__.has('Age', eq(42)), __.has('Age', eq(99))))");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType_has_conjunction_of_three()
        {
            var query = GremlinQuery
                .Create("g")
                .V<User>()
                .Where(t => t.Age == 36 && t.Age == 42 && t.Age == 99)
                .Serialize(this._model, true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('User').and(__.has('Age', eq(36)), __.has('Age', eq(42)), __.has('Age', eq(99)))");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType_has_disjunction_of_three()
        {
            var query = GremlinQuery
                .Create("g")
                .V<User>()
                .Where(t => t.Age == 36 || t.Age == 42 || t.Age == 99)
                .Serialize(this._model, true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('User').or(__.has('Age', eq(36)), __.has('Age', eq(42)), __.has('Age', eq(99)))");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType_has_conjunction_with_different_fields()
        {
            var query = GremlinQuery
                .Create("g")
                .V<User>()
                .Where(t => t.Name == "Some name" && t.Age == 42)
                .Serialize(this._model, true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('User').and(__.has('Name', eq('Some name')), __.has('Age', eq(42)))");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType()
        {
            var query = GremlinQuery
                .Create("g")
                .V<User>()
                .Serialize(this._model, true);

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
                .Create("g")
                .V<Authority>()
                .Serialize(this._model, true);

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
                .Create("g")
                .V<User>()
                .Where(t => t.Age == 36)
                .Serialize(this._model, true);

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
                .Create("g")
                .V<User>()
                .Where(t => (object)t.Age == (object)36)
                .Serialize(this._model, true);

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
                .Create("g")
                .V<User>()
                .Where(t => t.Age != 36)
                .Serialize(this._model, true);

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
                .Create("g")
                .V<User>()
                .Where(t => t.Name == null)
                .Serialize(this._model, true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('User').hasNot('Name')");

            query.parameters
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void V_ofType_string_property_exists()
        {
            var query = GremlinQuery
                .Create("g")
                .V<User>()
                .Where(t => t.Name != null)
                .Serialize(this._model, true);

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
                .Create("g")
                .V<User>()
                .Where(t => t.Age < 36)
                .Serialize(this._model, true);

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
                .Create("g")
                .V<User>()
                .Where(t => t.Age <= 36)
                .Serialize(this._model, true);

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
                .Create("g")
                .V<User>()
                .Where(t => t.Age > 36)
                .Serialize(this._model, true);

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
                .Create("g")
                .V<User>()
                .Where(t => t.Age >= 36)
                .Serialize(this._model, true);

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
                .Create("g")
                .V<Language>()
                .Where(t => t.Id == "languageId")
                .Serialize(this._model, true);

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
                .Create("g")
                .V<Language>()
                .Where(t => t.Id == local)
                .Serialize(this._model, true);

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
                .Create("g")
                .V<Language>()
                .Where(t => t.Id == local.Value)
                .Serialize(this._model, true);

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
                .Create("g")
                .V<Language>()
                .Invoking(query => query.Where(t => t.Id == t.IetfLanguageTag))
                .ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void V_of_type_where_with_stepLabel()
        {
            var l = new StepLabel<Language>("l");

            var query = GremlinQuery
                .Create("g")
                .V<Language>()
                .As(l)
                .V<Language>()
                .Where(l2 => l2 == l)
                .Serialize(this._model, true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('Language').as('l').V().hasLabel('Language').where(eq('l'))");
        }

        [Fact]
        public void V_of_type_where_with_stepLabel_not_inlined()
        {
            var l = new StepLabel<Language>("l");

            var query = GremlinQuery
                .Create("g")
                .V<Language>()
                .As(l)
                .V<Language>()
                .Where(l2 => l2 == l)
                .Serialize(this._model, false);

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
                .Create("g")
                .AddV(new User { Name = "Bob" })
                .AddE(new LivesIn())
                .To(__ => __
                    .V<Country>()
                    .Where(t => t.CountryCallingCode == "+49"))
                .Serialize(this._model, true);

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
                .Create("g")
                .AddV(new Language { IetfLanguageTag = "en" })
                .As((_, l) => _
                    .AddV(new Country { CountryCallingCode = "+49" })
                    .AddE(new IsDescribedIn { Text = "Germany" })
                    .To(l))
                .Serialize(this._model, false);

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
                .Create("g")
                .AddV(new Country { CountryCallingCode = "+49" })
                .As((_, c) => _
                    .AddV(new Language { IetfLanguageTag = "en" })
                    .AddE(new IsDescribedIn { Text = "Germany" })
                    .From(c))
                .Serialize(this._model, false);

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
                .Create("g")
                .V<User>()
                .And(
                    __ => __
                        .InE<Knows>(),
                    __ => __
                        .OutE<LivesIn>())
                .Serialize(this._model, true);

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
                .Create("g")
                .V<User>()
                .Drop()
                .Serialize(this._model, true);

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
                .Create("g")
                .V<User>()
                .FilterWithLambda("it.property('str').value().length() == 2")
                .Serialize(this._model, true);

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
                .Create("g")
                .V<User>()
                .Out<Knows>()
                .Serialize(this._model, true);

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
                .Create("g")
                .V<User>()
                .Out<Edge>()
                .Serialize(this._model, true);

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
                .Create("g")
                .V<User>()
                .Order()
                .ByMember(x => x.Name)
                .Serialize(this._model, true);

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
                .Create("g")
                .V<User>()
                .Order()
                .ByTraversal(__ => __.Values(x => x.Name))
                .Serialize(this._model, true);

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
                .Create("g")
                .V<User>()
                .Order()
                .ByLambda("it.property('str').value().length()")
                .Serialize(this._model, true);

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
                .Create("g")
                .V<User>()
                .Values(x => x.Name)
                .Serialize(this._model, true);

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
                .Create("g")
                .V<User>()
                .Values(x => x.Name, x => x.Id)
                .Serialize(this._model, true);

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
                .Create("g")
                .V()
                .Serialize(this._model, true);

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
                .Create("g")
                .V()
                .OfType<Authority>()
                .Serialize(this._model, true);

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
                .Create("g")
                .V<User>()
                .Repeat(__ => __.Out<Knows>())
                .Serialize(this._model, true);

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
                .Create("g")
                .V<User>()
                .Union(
                    __ => __.Out<Knows>(),
                    __ => __.Out<LivesIn>())
                .Serialize(this._model, true);

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
                .Create("g")
                .V()
                .Optional(
                    __ => __.Out<Knows>())
                .Serialize(this._model, true);

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
                .Create("g")
                .V()
                .Not(__ => __.Out<Knows>())
                .Serialize(this._model, true);

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
                .Create("g")
                .V()
                .Not(__ => __.OfType<Language>())
                .Serialize(this._model, true);

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
                .Create("g")
                .V()
                .Not(__ => __.OfType<Authority>())
                .Serialize(this._model, true);

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
                .Create("g")
                .V<User>()
                .As(new StepLabel<User>("a"))
                .Serialize(this._model, true);

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
                .Create("g")
                .V<User>()
                .As(new StepLabel<User>("a"))
                .Serialize(this._model, false);

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
                .Create("g")
                .V<User>()
                .As(stepLabel)
                .Select(stepLabel)
                .Serialize(this._model, true);

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
                .Create("g")
                .V<User>()
                .As(stepLabel)
                .Select(stepLabel)
                .Serialize(this._model, false);

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
                .Create("g")
                .V<User>()
                .Branch(
                    _ => _.Values(x => x.Name),
                    _ => _.Out<Knows>(),
                    _ => _.In<Knows>())
                .Serialize(this._model, false);

            query.queryString
                .Should()
                .Be("g.V().hasLabel(P1).branch(__.values(P2)).option(__.out(P3)).option(__.in(P3))");
        }

        [Fact]
        public void BranchOnIdentity()
        {
            var query = GremlinQuery
                .Create("g")
                .V<User>()
                .BranchOnIdentity(
                    _ => _.Out<Knows>(),
                    _ => _.In<Knows>())
                .Serialize(this._model, false);

            query.queryString
                .Should()
                .Be("g.V().hasLabel(P1).branch(__.identity()).option(__.out(P2)).option(__.in(P2))");
        }

        [Fact]
        public void Set_Property()
        {
            var query = GremlinQuery
                .Create("g")
                .V<User>()
                .Property(x => x.Age, 36)
                .Serialize(this._model, true);

            query.queryString
                .Should()
                .Be("g.V().hasLabel('User').property('Age', 36)");
        }
    }
}