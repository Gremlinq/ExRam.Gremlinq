using System;
using System.Reflection;
using FluentAssertions;
using Xunit;

namespace ExRam.Gremlinq.Tests
{
    public class GremlinQueryLanguageTest
    {
        private readonly IGraphModel _model;

        public GremlinQueryLanguageTest()
        {
            this._model = GraphModel.FromAssembly(Assembly.GetExecutingAssembly(), typeof(Vertex), typeof(Edge), GraphElementNamingStrategy.Simple);
        }

        [Fact]
        public void AddV()
        {
            var query = GremlinQuery
                .Create("g")
                .AddV(new Language { Id = "id", IetfLanguageTag = "en" })
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.addV(_P1).property(_P2, _P3).property(_P4, _P5)");

            query.parameters
                .Should()
                .Contain("_P1", "Language").And
                .Contain("_P2", "Id").And
                .Contain("_P3", "id").And
                .Contain("_P4", "IetfLanguageTag").And
                .Contain("_P5", "en");
        }

        [Fact]
        public void AddV_with_nulls()
        {
            var query = GremlinQuery
                .Create("g")
                .AddV(new Language {Id = "id"})
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.addV(_P1).property(_P2, _P3)");

            query.parameters
                .Should()
                .Contain("_P1", "Language").And
                .Contain("_P2", "Id").And
                .Contain("_P3", "id");
        }

        [Fact]
        public void V_ofType_has_disjunction()
        {
            var query = GremlinQuery
                .Create("g")
                .V<User>()
                .Where(t => t.Age == 36 || t.Age == 42)
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1).or(__.has(_P2, eq(_P3)), __.has(_P2, eq(_P4)))");

            query.parameters
                .Should()
                .Contain("_P1", "User").And
                .Contain("_P2", "Age").And
                .Contain("_P3", 36).And
                .Contain("_P4", 42);
        }

        [Fact]
        public void V_ofType_has_disjunction_with_different_fields()
        {
            var query = GremlinQuery
                .Create("g")
                .V<User>()
                .Where(t => t.Name == "Some name" || t.Age == 42)
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1).or(__.has(_P2, eq(_P3)), __.has(_P4, eq(_P5)))");

            query.parameters
                .Should()
                .Contain("_P1", "User").And
                .Contain("_P2", "Name").And
                .Contain("_P3", "Some name").And
                .Contain("_P4", "Age").And
                .Contain("_P5", 42);
        }

        [Fact]
        public void V_ofType_has_conjunction()
        {
            var query = GremlinQuery
                .Create("g")
                .V<User>()
                .Where(t => t.Age == 36 && t.Age == 42)
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1).and(__.has(_P2, eq(_P3)), __.has(_P2, eq(_P4)))");

            query.parameters
                .Should()
                .Contain("_P1", "User").And
                .Contain("_P2", "Age").And
                .Contain("_P3", 36).And
                .Contain("_P4", 42);
        }

        [Fact]
        public void V_ofType_complex_logical_expression()
        {
            var query = GremlinQuery
                .Create("g")
                .V<User>()
                .Where(t => t.Name == "Some name" && (t.Age == 42 || t.Age == 99))
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1).and(__.has(_P2, eq(_P3)), __.or(__.has(_P4, eq(_P5)), __.has(_P4, eq(_P6))))");

            query.parameters
                .Should()
                .Contain("_P1", "User").And
                .Contain("_P2", "Name").And
                .Contain("_P3", "Some name").And
                .Contain("_P4", "Age").And
                .Contain("_P5", 42).And
                .Contain("_P6", 99);
        }

        [Fact]
        public void V_ofType_complex_logical_expression_with_null()
        {
            var query = GremlinQuery
                .Create("g")
                .V<User>()
                .Where(t => t.Name == null && (t.Age == 42 || t.Age == 99))
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1).and(__.hasNot(_P2), __.or(__.has(_P3, eq(_P4)), __.has(_P3, eq(_P5))))");

            query.parameters
                .Should()
                .Contain("_P1", "User").And
                .Contain("_P2", "Name").And
                .Contain("_P3", "Age").And
                .Contain("_P4", 42).And
                .Contain("_P5", 99);
        }

        [Fact]
        public void V_ofType_has_conjunction_of_three()
        {
            var query = GremlinQuery
                .Create("g")
                .V<User>()
                .Where(t => t.Age == 36 && t.Age == 42 && t.Age == 99)
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1).and(__.has(_P2, eq(_P3)), __.has(_P2, eq(_P4)), __.has(_P2, eq(_P5)))");

            query.parameters
                .Should()
                .Contain("_P1", "User").And
                .Contain("_P2", "Age").And
                .Contain("_P3", 36).And
                .Contain("_P4", 42).And
                .Contain("_P5", 99);
        }

        [Fact]
        public void V_ofType_has_disjunction_of_three()
        {
            var query = GremlinQuery
                .Create("g")
                .V<User>()
                .Where(t => t.Age == 36 || t.Age == 42 || t.Age == 99)
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1).or(__.has(_P2, eq(_P3)), __.has(_P2, eq(_P4)), __.has(_P2, eq(_P5)))");

            query.parameters
                .Should()
                .Contain("_P1", "User").And
                .Contain("_P2", "Age").And
                .Contain("_P3", 36).And
                .Contain("_P4", 42).And
                .Contain("_P5", 99);
        }

        [Fact]
        public void V_ofType_has_conjunction_with_different_fields()
        {
            var query = GremlinQuery
                .Create("g")
                .V<User>()
                .Where(t => t.Name == "Some name" && t.Age == 42)
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1).and(__.has(_P2, eq(_P3)), __.has(_P4, eq(_P5)))");

            query.parameters
                .Should()
                .Contain("_P1", "User").And
                .Contain("_P2", "Name").And
                .Contain("_P3", "Some name").And
                .Contain("_P4", "Age").And
                .Contain("_P5", 42);
        }

        [Fact]
        public void V_ofType()
        {
            var query = GremlinQuery
                .Create("g")
                .V<User>()
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1)");

            query.parameters
                .Should()
                .Contain("_P1", "User");
        }

        [Fact]
        public void V_ofType_does_not_include_abstract_types()
        {
            var query = GremlinQuery
                .Create("g")
                .V<Authority>()
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1, _P2)");

            query.parameters
                .Should()
                .Contain("_P1", "Company").And
                .Contain("_P2", "User");
        }

        [Fact]
        public void V_ofType_has_int_property()
        {
            var query = GremlinQuery
                .Create("g")
                .V<User>()
                .Where(t => t.Age == 36)
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1).has(_P2, eq(_P3))");

            query.parameters
                .Should()
                .Contain("_P1", "User").And
                .Contain("_P2", "Age").And
                .Contain("_P3", 36);
        }

        [Fact]
        public void V_ofType_has_converted_int_property()
        {
            var query = GremlinQuery
                .Create("g")
                .V<User>()
                .Where(t => (object)t.Age == (object)36)
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1).has(_P2, eq(_P3))");

            query.parameters
                .Should()
                .Contain("_P1", "User").And
                .Contain("_P2", "Age").And
                .Contain("_P3", 36);
        }

        [Fact]
        public void V_ofType_has_unequal_int_property()
        {
            var query = GremlinQuery
                .Create("g")
                .V<User>()
                .Where(t => t.Age != 36)
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1).has(_P2, neq(_P3))");

            query.parameters
                .Should()
                .Contain("_P1", "User").And
                .Contain("_P2", "Age").And
                .Contain("_P3", 36);
        }

        [Fact]
        public void V_ofType_has_no_string_property()
        {
            var query = GremlinQuery
                .Create("g")
                .V<User>()
                .Where(t => t.Name == null)
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1).hasNot(_P2)");

            query.parameters
                .Should()
                .Contain("_P1", "User").And
                .Contain("_P2", "Name");
        }

        [Fact]
        public void V_ofType_string_property_exists()
        {
            var query = GremlinQuery
                .Create("g")
                .V<User>()
                .Where(t => t.Name != null)
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1).has(_P2)");

            query.parameters
                .Should()
                .Contain("_P1", "User").And
                .Contain("_P2", "Name");
        }

        [Fact]
        public void V_ofType_has_lower_int_property()
        {
            var query = GremlinQuery
                .Create("g")
                .V<User>()
                .Where(t => t.Age < 36)
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1).has(_P2, lt(_P3))");

            query.parameters
                .Should()
                .Contain("_P1", "User").And
                .Contain("_P2", "Age").And
                .Contain("_P3", 36);
        }

        [Fact]
        public void V_ofType_has_lower_or_equal_int_property()
        {
            var query = GremlinQuery
                .Create("g")
                .V<User>()
                .Where(t => t.Age <= 36)
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1).has(_P2, lte(_P3))");

            query.parameters
                .Should()
                .Contain("_P1", "User").And
                .Contain("_P2", "Age").And
                .Contain("_P3", 36);
        }

        [Fact]
        public void V_ofType_has_bool_property_with_explicit_comparison1()
        {
            var query = GremlinQuery
                .Create("g")
                .V<TimeFrame>()
                // ReSharper disable once RedundantBoolCompare
                .Where(t => t.Enabled == true)
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1).has(_P2, eq(_P3))");

            query.parameters
                .Should()
                .Contain("_P1", "TimeFrame").And
                .Contain("_P2", "Enabled").And
                .Contain("_P3", true);
        }

        [Fact]
        public void V_ofType_has_bool_property_with_explicit_comparison2()
        {
            var query = GremlinQuery
                .Create("g")
                .V<TimeFrame>()
                .Where(t => t.Enabled == false)
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1).has(_P2, eq(_P3))");

            query.parameters
                .Should()
                .Contain("_P1", "TimeFrame").And
                .Contain("_P2", "Enabled").And
                .Contain("_P3", false);
        }

        [Fact]
        public void V_ofType_has_bool_property_with_implicit_comparison1()
        {
            var query = GremlinQuery
                .Create("g")
                .V<TimeFrame>()
                .Where(t => t.Enabled)
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1).has(_P2, eq(_P3))");

            query.parameters
                .Should()
                .Contain("_P1", "TimeFrame").And
                .Contain("_P2", "Enabled").And
                .Contain("_P3", true);
        }

        [Fact]
        public void V_ofType_has_bool_property_with_implicit_comparison2()
        {
            var query = GremlinQuery
                .Create("g")
                .V<TimeFrame>()
                .Where(t => !t.Enabled)
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1).has(_P2, eq(_P3))");

            query.parameters
                .Should()
                .Contain("_P1", "TimeFrame").And
                .Contain("_P2", "Enabled").And
                .Contain("_P3", false);
        }

        [Fact]
        public void V_ofType_has_greater_int_property()
        {
            var query = GremlinQuery
                .Create("g")
                .V<User>()
                .Where(t => t.Age > 36)
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1).has(_P2, gt(_P3))");

            query.parameters
                .Should()
                .Contain("_P1", "User").And
                .Contain("_P2", "Age").And
                .Contain("_P3", 36);
        }

        [Fact]
        public void V_ofType_has_greater_or_equal_int_property()
        {
            var query = GremlinQuery
                .Create("g")
                .V<User>()
                .Where(t => t.Age >= 36)
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1).has(_P2, gte(_P3))");

            query.parameters
                .Should()
                .Contain("_P1", "User").And
                .Contain("_P2", "Age").And
                .Contain("_P3", 36);
        }

        [Fact]
        public void V_ofType_has_string_property()
        {
            var query = GremlinQuery
                .Create("g")
                .V<Language>()
                .Where(t => t.Id == "languageId")
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1).has(_P2, eq(_P3))");

            query.parameters
                .Should()
                .Contain("_P1", "Language").And
                .Contain("_P2", "Id").And
                .Contain("_P3", "languageId");
        }

        [Fact]
        public void V_ofType_where_with_local_string()
        {
            var local = "languageId";

            var query = GremlinQuery
                .Create("g")
                .V<Language>()
                .Where(t => t.Id == local)
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1).has(_P2, eq(_P3))");

            query.parameters
                .Should()
                .Contain("_P1", "Language").And
                .Contain("_P2", "Id").And
                .Contain("_P3", "languageId");
        }

        [Fact]
        public void V_ofType_where_with_local_anonymous_type()
        {
            var local = new { Value = "languageId" };

            var query = GremlinQuery
                .Create("g")
                .V<Language>()
                .Where(t => t.Id == local.Value)
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1).has(_P2, eq(_P3))");

            query.parameters
                .Should()
                .Contain("_P1", "Language").And
                .Contain("_P2", "Id").And
                .Contain("_P3", "languageId");
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
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1).as(_P2).V().hasLabel(_P1).where(eq(_P2))");

            query.parameters
                .Should()
                .Contain("_P1", "Language").And
                .Contain("_P2", "l");
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
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1).as(_P2).V().hasLabel(_P1).where(eq(_P2))");

            query
                .parameters
                .Should()
                .Contain("_P1", "Language").And
                .Contain("_P2", "l");
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
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.addV(_P1).property(_P2, _P3).property(_P4, _P5).addE(_P6).to(__.V().hasLabel(_P7).has(_P8, eq(_P9)))");

            query
                .parameters
                .Should()
                .Contain("_P1", "User").And
                .Contain("_P2", "Age").And
                .Contain("_P3", 0).And
                .Contain("_P4", "Name").And
                .Contain("_P5", "Bob").And
                .Contain("_P6", "LivesIn").And
                .Contain("_P7", "Country").And
                .Contain("_P8", "CountryCallingCode").And
                .Contain("_P9", "+49");
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
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.addV(_P1).property(_P2, _P3).as(_P4).addV(_P5).property(_P6, _P7).addE(_P8).property(_P9, _P10).to(_P4)");

            query
                .parameters
                .Should()
                .Contain("_P1", "Language").And
                .Contain("_P2", "IetfLanguageTag").And
                .Contain("_P3", "en").And
                .Contain("_P4", "l1").And
                .Contain("_P5", "Country").And
                .Contain("_P6", "CountryCallingCode").And
                .Contain("_P7", "+49").And
                .Contain("_P8", "IsDescribedIn").And
                .Contain("_P9", "Text").And
                .Contain("_P10", "Germany");
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
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.addV(_P1).property(_P2, _P3).as(_P4).addV(_P5).property(_P6, _P7).addE(_P8).property(_P9, _P10).from(_P4)");

            query.parameters
                .Should()
                .Contain("_P1", "Country").And
                .Contain("_P2", "CountryCallingCode").And
                .Contain("_P3", "+49").And
                .Contain("_P4", "l1").And
                .Contain("_P5", "Language").And
                .Contain("_P6", "IetfLanguageTag").And
                .Contain("_P7", "en").And
                .Contain("_P8", "IsDescribedIn").And
                .Contain("_P9", "Text").And
                .Contain("_P10", "Germany");
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
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1).and(__.inE(_P2), __.outE(_P3))");

            query.parameters
                .Should()
                .Contain("_P1", "User").And
                .Contain("_P2", "Knows").And
                .Contain("_P3", "LivesIn");
        }

        [Fact]
        public void Drop()
        {
            var query = GremlinQuery
                .Create("g")
                .V<User>()
                .Drop()
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1).drop()");

            query.parameters
                .Should()
                .Contain("_P1", "User");
        }

        [Fact]
        public void FilterWithLambda()
        {
            var query = GremlinQuery
                .Create("g")
                .V<User>()
                .FilterWithLambda("it.property('str').value().length() == 2")
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1).filter({it.property('str').value().length() == 2})");

            query.parameters
                .Should()
                .Contain("_P1", "User");
        }

        [Fact]
        public void Out()
        {
            var query = GremlinQuery
                .Create("g")
                .V<User>()
                .Out<Knows>()
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1).out(_P2)");

            query.parameters
                .Should()
                .Contain("_P1", "User").And
                .Contain("_P2", "Knows");
        }

        [Fact]
        public void Out_does_not_include_abstract_edge()
        {
            var query = GremlinQuery
                .Create("g")
                .V<User>()
                .Out<Edge>()
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1).out(_P2, _P3, _P4, _P5, _P6)");

            query.parameters
                .Should()
                .Contain("_P1", "User").And
                .Contain("_P2", "IsDescribedIn").And
                .Contain("_P3", "Knows").And
                .Contain("_P4", "LivesIn").And
                .Contain("_P5", "Speaks").And
                .Contain("_P6", "WorksFor");
        }

        [Fact]
        public void V_ofType_order_ByMember()
        {
            var query = GremlinQuery
                .Create("g")
                .V<User>()
                .Order()
                .ByMember(x => x.Name)
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1).order().by(_P2, incr)");

            query.parameters
                .Should()
                .Contain("_P1", "User").And
                .Contain("_P2", "Name");
        }

        [Fact]
        public void V_ofType_order_ByTraversal()
        {
            var query = GremlinQuery
                .Create("g")
                .V<User>()
                .Order()
                .ByTraversal(__ => __.Values(x => x.Name))
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1).order().by(__.values(_P2), incr)");

            query.parameters
                .Should()
                .Contain("_P1", "User").And
                .Contain("_P2", "Name");
        }

        [Fact]
        public void V_ofType_order_ByLambda()
        {
            var query = GremlinQuery
                .Create("g")
                .V<User>()
                .Order()
                .ByLambda("it.property('str').value().length()")
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1).order().by({it.property('str').value().length()})");

            query.parameters
                .Should()
                .Contain("_P1", "User");
        }

        [Fact]
        public void V_ofType_values_of_one_property()
        {
            var query = GremlinQuery
                .Create("g")
                .V<User>()
                .Values(x => x.Name)
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1).values(_P2)");

            query.parameters
                .Should()
                .Contain("_P1", "User").And
                .Contain("_P2", "Name");
        }

        [Fact]
        public void V_ofType_values_of_two_properties()
        {
            var query = GremlinQuery
                .Create("g")
                .V<User>()
                .Values(x => x.Name, x => x.Id)
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1).values(_P2, _P3)");

            query.parameters
                .Should()
                .Contain("_P1", "User").And
                .Contain("_P2", "Name").And
                .Contain("_P3", "Id");
        }

        [Fact]
        public void V_without_type()
        {
            var query = GremlinQuery
                .Create("g")
                .V()
                .Resolve(this._model)
                .Serialize();

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
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1, _P2)");

            query.parameters
                .Should()
                .Contain("_P1", "Company").And
                .Contain("_P2", "User");
        }

        [Fact]
        public void V_ofType_Repeat_out_traversal()
        {
            var query = GremlinQuery
                .Create("g")
                .V<User>()
                .Repeat(__ => __.Out<Knows>())
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1).repeat(__.out(_P2))");

            query.parameters
                .Should()
                .Contain("_P1", "User").And
                .Contain("_P2", "Knows");
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
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1).union(__.out(_P2), __.out(_P3))");

            query.parameters
                .Should()
                .Contain("_P1", "User").And
                .Contain("_P2", "Knows").And
                .Contain("_P3", "LivesIn");
        }

        [Fact]
        public void V_ofType_Optional_one_out_traversal()
        {
            var query = GremlinQuery
                .Create("g")
                .V()
                .Optional(
                    __ => __.Out<Knows>())
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().optional(__.out(_P1))");

            query.parameters
                .Should()
                .Contain("_P1", "Knows");
        }

        [Fact]
        public void V_ofType_Not_one_out_traversal()
        {
            var query = GremlinQuery
                .Create("g")
                .V()
                .Not(__ => __.Out<Knows>())
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().not(__.out(_P1))");

            query.parameters
                .Should()
                .Contain("_P1", "Knows");
        }

        [Fact]
        public void V_ofType_Optional_one_out_traversal_1()
        {
            var query = GremlinQuery
                .Create("g")
                .V()
                .Not(__ => __.OfType<Language>())
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().not(__.hasLabel(_P1))");

            query.parameters
                .Should()
                .Contain("_P1", "Language");
        }

        [Fact]
        public void V_ofType_Optional_one_out_traversal_2()
        {
            var query = GremlinQuery
                .Create("g")
                .V()
                .Not(__ => __.OfType<Authority>())
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().not(__.hasLabel(_P1, _P2))");

            query.parameters
                .Should()
                .Contain("_P1", "Company").And
                .Contain("_P2", "User");
        }

        [Fact]
        public void V_as()
        {
            var query = GremlinQuery
                .Create("g")
                .V<User>()
                .As(new StepLabel<User>("a"))
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1).as(_P2)");

            query.parameters
                .Should()
                .Contain("_P1", "User").And
                .Contain("_P2", "a");
        }

        [Fact]
        public void V_as_not_inlined()
        {
            var query = GremlinQuery
                .Create("g")
                .V<User>()
                .As(new StepLabel<User>("a"))
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1).as(_P2)");

            query.parameters
                .Should()
                .Contain("_P1", "User").And
                .Contain("_P2", "a");
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
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1).as(_P2).select(_P2)");

            query.parameters
                .Should()
                .Contain("_P1", "User").And
                .Contain("_P2", "a");
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
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1).as(_P2).select(_P2)");

            query.parameters
                .Should()
                .Contain("_P1", "User").And
                .Contain("_P2", "a");
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
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1).branch(__.values(_P2)).option(__.out(_P3)).option(__.in(_P3))");

            query.parameters
                .Should()
                .Contain("_P1", "User").And
                .Contain("_P2", "Name").And
                .Contain("_P3", "Knows");
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
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1).branch(__.identity()).option(__.out(_P2)).option(__.in(_P2))");

            query.parameters
                .Should()
                .Contain("_P1", "User").And
                .Contain("_P2", "Knows");
        }

        [Fact]
        public void Set_Property()
        {
            var query = GremlinQuery
                .Create("g")
                .V<User>()
                .Property(x => x.Age, 36)
                .Resolve(this._model)
                .Serialize();

            query.queryString
                .Should()
                .Be("g.V().hasLabel(_P1).property(_P2, _P3)");

            query.parameters
                .Should()
                .Contain("_P1", "User").And
                .Contain("_P2", "Age").And
                .Contain("_P3", 36);
        }
    }
}