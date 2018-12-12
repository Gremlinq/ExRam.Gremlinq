using System;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Xunit;

namespace ExRam.Gremlinq.Tests
{
    public class GremlinQueryTest
    {
        private readonly IGraphModel _model;

        public GremlinQueryTest()
        {
            _model = GraphModel
                .FromAssembly<Vertex, Edge>(Assembly.GetExecutingAssembly(), GraphElementNamingStrategy.Simple)
                .WithIdPropertyName("Id");
        }

        [Fact]
        public void V_of_concrete_type()
        {
            g
                .WithModel(_model)
                .V<User>()
                .Should()
                .SerializeTo("g.V().hasLabel(_a)")
                .WithParameters("User");
        }

        [Fact]
        public void V_of_abstract_type()
        {
            g
                .WithModel(_model)
                .V<Authority>()
                .Should()
                .SerializeTo("g.V().hasLabel(_a, _b)")
                .WithParameters("Company", "User");
        }

        [Fact]
        public void AddE_types1()
        {
            g
                .AddE<LivesIn>()
                .From(_ => _.AddV<User>())
                .To(_ => _.AddV<Country>())
                .Should()
                .BeAssignableTo<IEGremlinQuery<LivesIn, User, Country>>();
        }

        [Fact]
        public void AddE_types2()
        {
            g
                .AddE<LivesIn>()
                .To(_ => _.AddV<Country>())
                .From(_ => _.AddV<User>())
                .Should()
                .BeAssignableTo<IEGremlinQuery<LivesIn, User, Country>>();
        }

        [Fact]
        public void AddV()
        {
            g
                .WithModel(_model)
                .AddV(new Language { Id = "id", IetfLanguageTag = "en" })
                .Should()
                .SerializeTo("g.addV(_a).property(Cardinality.single, _b, _c).property(T.id, _d)")
                .WithParameters("Language", "IetfLanguageTag", "en", "id");
        }

        [Fact]
        public void AddV_list_cardinality_id()
        {
            g
                .WithModel(GraphModel
                    .FromAssembly<Vertex, Edge>(Assembly.GetExecutingAssembly(), GraphElementNamingStrategy.Simple)
                    .WithIdPropertyName("PhoneNumbers"))
                .AddV(new User { Id = "id", PhoneNumbers = new[] { "123", "456" } })
                .Invoking(x => x.Serialize())
                .Should()
                .Throw<NotSupportedException>();
        }

        [Fact]
        public void AddV_without_id()
        {
            g
                .WithModel(_model)
                .AddV(new Language { IetfLanguageTag = "en" })
                .Should()
                .SerializeTo("g.addV(_a).property(Cardinality.single, _b, _c)")
                .WithParameters("Language", "IetfLanguageTag", "en");
        }

        [Fact]
        public void AddV_with_nulls()
        {
            g
                .WithModel(_model)
                .AddV(new Language {Id = "id"})
                .Should()
                .SerializeTo("g.addV(_a).property(T.id, _b)")
                .WithParameters("Language", "id");
        }

        [Fact]
        public void AddV_with_multi_property()
        {
            g
                .WithModel(_model)
                .AddV(new User { Id = "id", PhoneNumbers = new[] { "+4912345", "+4923456" } })
                .Should()
                .SerializeTo("g.addV(_a).property(Cardinality.single, _b, _c).property(Cardinality.single, _d, _e).property(Cardinality.single, _f, _g).property(Cardinality.list, _h, _i).property(Cardinality.list, _h, _j).property(T.id, _k)")
                .WithParameters("User", "Age", 0, "Gender", 0, "RegistrationDate", DateTimeOffset.MinValue, "PhoneNumbers", "+4912345", "+4923456", "id");
        }

        [Fact]
        public void AddV_with_Meta_without_properties()
        {
            g
                .WithModel(_model)
                .AddV(new Country { Id = "id", Name = "GER"})
                .Should()
                .SerializeTo("g.addV(_a).property(Cardinality.single, _b, _c).property(T.id, _d)")
                .WithParameters("Country", "Name", "GER", "id");
        }

        [Fact]
        public void AddV_with_Meta_with_properties()
        {
            g
                .WithModel(_model)
                .AddV(new Country
                {
                    Id = "id",
                    Name = new Meta<string>("GER")
                    {
                        Properties =
                        {
                            { "de", "Deutschland" },
                            { "en", "Germany" }
                        }
                    }
                })
                .Should()
                .SerializeTo("g.addV(_a).property(Cardinality.single, _b, _c, _d, _e, _f, _g).property(T.id, _h)")
                .WithParameters("Country", "Name", "GER", "de", "Deutschland", "en", "Germany", "id");
        }
        
        [Fact]
        public void AddV_with_enum_property()
        {
            g
                .WithModel(_model)
                .AddV(new User { Id = "id", Gender = Gender.Female })
                .Should()
                .SerializeTo("g.addV(_a).property(Cardinality.single, _b, _c).property(Cardinality.single, _d, _e).property(Cardinality.single, _f, _g).property(T.id, _h)")
                .WithParameters("User", "Age", 0, "Gender" , 1, "RegistrationDate", DateTimeOffset.MinValue, "id");
        }

        [Fact]
        public void Where_property_array_contains_element()
        {
            g
                .WithModel(_model)
                .V<User>()
                .Where(t => t.PhoneNumbers.Contains("+4912345"))
                .Should()
                .SerializeTo("g.V().hasLabel(_a).has(_b, _c)")
                .WithParameters("User", "PhoneNumbers", "+4912345");
        }

        [Fact]
        public void Where_property_array_does_not_contain_element()
        {
            g
                .WithModel(_model)
                .V<User>()
                .Where(t => !t.PhoneNumbers.Contains("+4912345"))
                .Should()
                .SerializeTo("g.V().hasLabel(_a).not(__.has(_b, _c))")
                .WithParameters("User", "PhoneNumbers", "+4912345");
        }

        [Fact]
        public void Where_property_array_is_not_empty()
        {
            g
                .WithModel(_model)
                .V<User>()
                .Where(t => t.PhoneNumbers.Any())
                .Should()
                .SerializeTo("g.V().hasLabel(_a).has(_b)")
                .WithParameters("User", "PhoneNumbers");
        }

        [Fact]
        public void Where_property_array_is_empty()
        {
            g
                .V<User>()
                .Where(t => !t.PhoneNumbers.Any())
                .WithModel(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_a).not(__.has(_b))")
                .WithParameters("User", "PhoneNumbers");
        }

        [Fact]
        public void Where_property_array_intersects_aray()
        {
            g
                .WithModel(_model)
                .V<User>()
                .Where(t => t.PhoneNumbers.Intersects(new[] { "+4912345", "+4923456" }))
                .Should()
                .SerializeTo("g.V().hasLabel(_a).has(_b, P.within(_c, _d))")
                .WithParameters("User", "PhoneNumbers", "+4912345", "+4923456");
        }

        [Fact]
        public void Where_property_array_does_not_intersect_array()
        {
            g
                .WithModel(_model)
                .V<User>()
                .Where(t => !t.PhoneNumbers.Intersects(new[] { "+4912345", "+4923456" }))
                .Should()
                .SerializeTo("g.V().hasLabel(_a).not(__.has(_b, P.within(_c, _d)))")
                .WithParameters("User", "PhoneNumbers", "+4912345", "+4923456");
        }

        [Fact]
        public void Where_property_array_intersects_empty_array()
        {
            g
                .WithModel(_model)
                .V<User>()
                .Where(t => t.PhoneNumbers.Intersects(new string[0]))
                .Should()
                .SerializeTo("g.V().hasLabel(_a).not(__.identity())")
                .WithParameters("User");
        }

        [Fact]
        public void Where_property_array_does_not_intersect_empty_array()
        {
            g
                .WithModel(_model)
                .V<User>()
                .Where(t => !t.PhoneNumbers.Intersects(new string[0]))
                .Should()
                .SerializeTo("g.V().hasLabel(_a)")
                .WithParameters("User");
        }

        [Fact]
        public void Where_property_is_contained_in_array()
        {
            g
                .WithModel(_model)
                .V<User>()
                .Where(t => new[] { 36, 37, 38 }.Contains(t.Age))
                .Should()
                .SerializeTo("g.V().hasLabel(_a).has(_b, P.within(_c, _d, _e))")
                .WithParameters("User", "Age", 36, 37, 38);
        }

        [Fact]
        public void Where_property_is_contained_in_enumerable()
        {
            var enumerable = new[] { "36", "37", "38" }
                .Select(int.Parse);

            g
                .WithModel(_model)
                .V<User>()
                .Where(t => enumerable.Contains(t.Age))
                .Should()
                .SerializeTo("g.V().hasLabel(_a).has(_b, P.within(_c, _d, _e))")
                .WithParameters("User", "Age", 36, 37, 38);
        }

        [Fact]
        public void Where_property_is_contained_in_empty_enumerable()
        {
            var enumerable = Enumerable.Empty<int>();

            g
                .WithModel(_model)
                .V<User>()
                .Where(t => enumerable.Contains(t.Age))
                .Should()
                .SerializeTo("g.V().hasLabel(_a).not(__.identity())")
                .WithParameters("User");
        }

        [Fact]
        public void Where_property_is_not_contained_in_array()
        {
            g
                .WithModel(_model)
                .V<User>()
                .Where(t => !new[] { 36, 37, 38 }.Contains(t.Age))
                .Should()
                .SerializeTo("g.V().hasLabel(_a).not(__.has(_b, P.within(_c, _d, _e)))")
                .WithParameters("User", "Age", 36, 37, 38);
        }
        
        [Fact]
        public void Where_property_is_not_contained_in_enumerable()
        {
            var enumerable = new[] { "36", "37", "38" }
                .Select(int.Parse);

            g
                .WithModel(_model)
                .V<User>()
                .Where(t => !enumerable.Contains(t.Age))
                .Should()
                .SerializeTo("g.V().hasLabel(_a).not(__.has(_b, P.within(_c, _d, _e)))")
                .WithParameters("User", "Age", 36, 37, 38);
        }

        [Fact]
        public void Where_property_is_not_contained_in_empty_enumerable()
        {
            var enumerable = Enumerable.Empty<int>();

            g
                .WithModel(_model)
                .V<User>()
                .Where(t => !enumerable.Contains(t.Age))
                .Should()
                .SerializeTo("g.V().hasLabel(_a)")
                .WithParameters("User");
        }

        [Fact]
        public void Where_property_is_prefix_of_constant()
        {
            g
                .WithModel(_model)
                .V<CountryCallingCode>()
                .Where(c => "+49123".StartsWith(c.Prefix))
                .Should()
                .SerializeTo("g.V().hasLabel(_a).has(_b, P.within(_c, _d, _e, _f, _g, _h, _i))")
                .WithParameters("CountryCallingCode", "Prefix", "", "+", "+4", "+49", "+491", "+4912", "+49123");
        }

        [Fact]
        public void Where_property_is_prefix_of_empty_string()
        {
            g
                .WithModel(_model)
                .V<CountryCallingCode>()
                .Where(c => "".StartsWith(c.Prefix))
                .Should()
                .SerializeTo("g.V().hasLabel(_a).has(_b, P.within(_c))")
                .WithParameters("CountryCallingCode", "Prefix", "");
        }

        [Fact]
        public void Where_property_is_prefix_of_variable()
        {
            const string str = "+49123";

            g
                .WithModel(_model)
                .V<CountryCallingCode>()
                .Where(c => str.StartsWith(c.Prefix))
                .Should()
                .SerializeTo("g.V().hasLabel(_a).has(_b, P.within(_c, _d, _e, _f, _g, _h, _i))")
                .WithParameters("CountryCallingCode", "Prefix", "", "+", "+4", "+49", "+491", "+4912", "+49123");
        }

        [Fact]
        public void Where_property_is_prefix_of_expression()
        {
            const string str = "+49123xxx";

            g
                .WithModel(_model)
                .V<CountryCallingCode>()
                .Where(c => str.Substring(0, 6).StartsWith(c.Prefix))
                .Should()
                .SerializeTo("g.V().hasLabel(_a).has(_b, P.within(_c, _d, _e, _f, _g, _h, _i))")
                .WithParameters("CountryCallingCode", "Prefix", "", "+", "+4", "+49", "+491", "+4912", "+49123");
        }

        [Fact]
        public void Where_property_starts_with_constant()
        {
            g
                .WithModel(_model)
                .V<User>()
                .Where(c => c.PhoneNumber.StartsWith("+49123"))
                .Should()
                .SerializeTo("g.V().hasLabel(_a).has(_b, P.between(_c, _d))")
                .WithParameters("User", "PhoneNumber", "+49123", "+49124");
        }

        [Fact]
        public void Where_property_starts_with_empty_string()
        {
            g
                .WithModel(_model)
                .V<User>()
                .Where(c => c.PhoneNumber.StartsWith(""))
                .Should()
                .SerializeTo("g.V().hasLabel(_a).has(_b)")
                .WithParameters("User", "PhoneNumber");
        }

        [Fact]
        public void Where_disjunction()
        {
            g
                .WithModel(_model)
                .V<User>()
                .Where(t => t.Age == 36 || t.Age == 42)
                .Should()
                .SerializeTo("g.V().hasLabel(_a).or(__.has(_b, _c), __.has(_b, _d))")
                .WithParameters("User", "Age", 36, 42);
        }

        [Fact]
        public void Where_disjunction_with_different_fields()
        {
            g
                .WithModel(_model)
                .V<User>()
                .Where(t => t.Name == "Some name" || t.Age == 42)
                .Should()
                .SerializeTo("g.V().hasLabel(_a).or(__.has(_b, _c), __.has(_d, _e))")
                .WithParameters("User", "Name", "Some name", "Age", 42);
        }

        [Fact]
        public void Where_conjunction()
        {
            g
                .WithModel(_model)
                .V<User>()
                .Where(t => t.Age == 36 && t.Age == 42)
                .Should()
                .SerializeTo("g.V().hasLabel(_a).and(__.has(_b, _c), __.has(_b, _d))")
                .WithParameters("User", "Age", 36, 42);
        }

        [Fact]
        public void Where_complex_logical_expression()
        {
            g
                .WithModel(_model)
                .V<User>()
                .Where(t => t.Name == "Some name" && (t.Age == 42 || t.Age == 99))
                .Should()
                .SerializeTo("g.V().hasLabel(_a).and(__.has(_b, _c), __.or(__.has(_d, _e), __.has(_d, _f)))")
                .WithParameters("User", "Name", "Some name", "Age", 42, 99);
        }

        [Fact]
        public void Where_complex_logical_expression_with_null()
        {
            g
                .WithModel(_model)
                .V<User>()
                .Where(t => t.Name == null && (t.Age == 42 || t.Age == 99))
                .Should()
                .SerializeTo("g.V().hasLabel(_a).and(__.hasNot(_b), __.or(__.has(_c, _d), __.has(_c, _e)))")
                .WithParameters("User", "Name", "Age", 42, 99);
        }

        [Fact]
        public void Where_has_conjunction_of_three()
        {
            g
                .WithModel(_model)
                .V<User>()
                .Where(t => t.Age == 36 && t.Age == 42 && t.Age == 99)
                .Should()
                .SerializeTo("g.V().hasLabel(_a).and(__.has(_b, _c), __.has(_b, _d), __.has(_b, _e))")
                .WithParameters("User", "Age", 36, 42, 99);
        }

        [Fact]
        public void Where_has_disjunction_of_three()
        {
            g
                .WithModel(_model)
                .V<User>()
                .Where(t => t.Age == 36 || t.Age == 42 || t.Age == 99)
                .Should()
                .SerializeTo("g.V().hasLabel(_a).or(__.has(_b, _c), __.has(_b, _d), __.has(_b, _e))")
                .WithParameters("User", "Age", 36, 42, 99);
        }

        [Fact]
        public void Where_conjunction_with_different_fields()
        {
            g
                .WithModel(_model)
                .V<User>()
                .Where(t => t.Name == "Some name" && t.Age == 42)
                .Should()
                .SerializeTo("g.V().hasLabel(_a).and(__.has(_b, _c), __.has(_d, _e))")
                .WithParameters("User", "Name", "Some name", "Age", 42);
        }

        [Fact]
        public void Where_property_equals_constant()
        {
            g
                .WithModel(_model)
                .V<User>()
                .Where(t => t.Age == 36)
                .Should()
                .SerializeTo("g.V().hasLabel(_a).has(_b, _c)")
                .WithParameters("User", "Age", 36);
        }

        [Fact]
        public void Where_property_equals_expression()
        {
            const int i = 18;

            g
                .WithModel(_model)
                .V<User>()
                .Where(t => t.Age == i + i)
                .Should()
                .SerializeTo("g.V().hasLabel(_a).has(_b, _c)")
                .WithParameters("User", "Age", 36);
        }

        [Fact]
        public void Where_property_equals_converted_expression()
        {
            g
                .WithModel(_model)
                .V<User>()
                .Where(t => (object)t.Age == (object)36)
                .Should()
                .SerializeTo("g.V().hasLabel(_a).has(_b, _c)")
                .WithParameters("User", "Age", 36);
        }

        [Fact]
        public void Where_property_not_equals_constant()
        {
            g
                .WithModel(_model)
                .V<User>()
                .Where(t => t.Age != 36)
                .Should()
                .SerializeTo("g.V().hasLabel(_a).has(_b, P.neq(_c))")
                .WithParameters("User", "Age", 36);
        }

        [Fact]
        public void Where_property_is_not_present()
        {
            g
                .WithModel(_model)
                .V<User>()
                .Where(t => t.Name == null)
                .Should()
                .SerializeTo("g.V().hasLabel(_a).hasNot(_b)")
                .WithParameters("User", "Name");
        }

        [Fact]
        public void Where_property_is_present()
        {
            g
                .WithModel(_model)
                .V<User>()
                .Where(t => t.Name != null)
                .Should()
                .SerializeTo("g.V().hasLabel(_a).has(_b)")
                .WithParameters("User", "Name");
        }

        [Fact]
        public void Where_property_is_lower_than_constant()
        {
            g
                .WithModel(_model)
                .V<User>()
                .Where(t => t.Age < 36)
                .Should()
                .SerializeTo("g.V().hasLabel(_a).has(_b, P.lt(_c))")
                .WithParameters("User", "Age", 36);
        }

        [Fact]
        public void Where_property_is_lower_or_equal_than_constant()
        {
            g
                .WithModel(_model)
                .V<User>()
                .Where(t => t.Age <= 36)
                .Should()
                .SerializeTo("g.V().hasLabel(_a).has(_b, P.lte(_c))")
                .WithParameters("User", "Age", 36);
        }

        [Fact]
        public void Where_bool_property_explicit_comparison1()
        {
            g
                .WithModel(_model)
                .V<TimeFrame>()
                // ReSharper disable once RedundantBoolCompare
                .Where(t => t.Enabled == true)
                .Should()
                .SerializeTo("g.V().hasLabel(_a).has(_b, _c)")
                .WithParameters("TimeFrame", "Enabled", true);
        }

        [Fact]
        public void Where_bool_property_explicit_comparison2()
        {
            g
                .WithModel(_model)
                .V<TimeFrame>()
                .Where(t => t.Enabled == false)
                .Should()
                .SerializeTo("g.V().hasLabel(_a).has(_b, _c)")
                .WithParameters("TimeFrame", "Enabled", false);
        }

        [Fact]
        public void Where_bool_property_implicit_comparison1()
        {
            g
                .WithModel(_model)
                .V<TimeFrame>()
                .Where(t => t.Enabled)
                .Should()
                .SerializeTo("g.V().hasLabel(_a).has(_b, _c)")
                .WithParameters("TimeFrame", "Enabled", true);
        }

        [Fact]
        public void Where_bool_property_implicit_comparison2()
        {
            g
                .WithModel(_model)
                .V<TimeFrame>()
                .Where(t => !t.Enabled)
                .Should()
                .SerializeTo("g.V().hasLabel(_a).not(__.has(_b, _c))")
                .WithParameters("TimeFrame", "Enabled", true);
        }

        [Fact]
        public void Where_property_is_greater_than_constant()
        {
            g
                .WithModel(_model)
                .V<User>()
                .Where(t => t.Age > 36)
                .Should()
                .SerializeTo("g.V().hasLabel(_a).has(_b, P.gt(_c))")
                .WithParameters("User", "Age", 36);
        }

        [Fact]
        public void Where_property_is_greater_or_equal_than_constant()
        {
            g
                .WithModel(_model)
                .V<User>()
                .Where(t => t.Age >= 36)
                .Should()
                .SerializeTo("g.V().hasLabel(_a).has(_b, P.gte(_c))")
                .WithParameters("User", "Age", 36);
        }

        [Fact]
        public void Where_property_equals_string_constant()
        {
            g
                .WithModel(_model)
                .V<Language>()
                .Where(t => t.Id == "languageId")
                .Should()
                .SerializeTo("g.V().hasLabel(_a).has(T.id, _b)")
                .WithParameters("Language", "languageId");
        }

        [Fact]
        public void Where_property_equals_local_string_constant()
        {
            const string local = "languageId";

            g
                .WithModel(_model)
                .V<Language>()
                .Where(t => t.Id == local)
                .Should()
                .SerializeTo("g.V().hasLabel(_a).has(T.id, _b)")
                .WithParameters("Language", "languageId");
        }

        [Fact]
        public void Where_property_equals_value_of_anonymous_object()
        {
            var local = new { Value = "languageId" };

            g
                .WithModel(_model)
                .V<Language>()
                .Where(t => t.Id == local.Value)
                .Should()
                .SerializeTo("g.V().hasLabel(_a).has(T.id, _b)")
                .WithParameters("Language", "languageId");
        }

        [Fact]
        public void Where_source_expression_on_both_sides()
        {
            g
                .V<Language>()
                .Invoking(query => query.Where(t => t.Id == t.IetfLanguageTag))
                .Should()
                .Throw<InvalidOperationException>();
        }

        [Fact]
        public void Where_current_element_equals_stepLabel()
        {
            var l = new StepLabel<Language>();

            g
                .WithModel(_model)
                .V<Language>()
                .As(l)
                .V<Language>()
                .Where(l2 => l2 == l)
                .Should()
                .SerializeTo("g.V().hasLabel(_a).as(_b).V().hasLabel(_a).where(P.eq(_b))")
                .WithParameters("Language", "l1");
        }

        [Fact]
        public void Where_property_equals_stepLabel()
        {
            var l = new StepLabel<string>();

            g
                .WithModel(_model)
                .V<Language>()
                .Values(x => x.IetfLanguageTag)
                .As(l)
                .V<Language>()
                .Where(l2 => l2.IetfLanguageTag == l)
                .Should()
                .SerializeTo("g.V().hasLabel(_a).values(_b).as(_c).V().hasLabel(_a).has(_b, __.where(P.eq(_c)))")
                .WithParameters("Language", "IetfLanguageTag", "l1");
        }

        [Fact]
        public void Where_scalar_element_equals_constant()
        {
            g
                .WithModel(_model)
                .V<User>()
                .Values(x => x.Age)
                .Where(_ => _ == 36)
                .Should()
                .SerializeTo("g.V().hasLabel(_a).values(_b).is(_c)")
                .WithParameters("User", "Age", 36);
        }

        [Fact]
        public void Where_traversal()
        {
            g
                .WithModel(_model)
                .V<User>()
                .Where(_ => _.Out<LivesIn>())
                .Should()
                .SerializeTo("g.V().hasLabel(_a).where(__.out(_b))")
                .WithParameters("User", "LivesIn");
        }

        [Fact]
        public void Where_property_traversal()
        {
            g
                .WithModel(_model)
                .V<User>()
                .Where(
                    x => x.Age,
                    _ => _
                        .Inject(36))
                .Should()
                .SerializeTo("g.V().hasLabel(_a).has(_b, __.inject(_c))")
                .WithParameters("User", "Age", 36);
        }

        [Fact]
        public void AddE_to_traversal()
        {
            var now = DateTimeOffset.UtcNow;

            g
                .WithModel(_model)
                .AddV(new User
                {
                    Name = "Bob",
                    RegistrationDate = now
                })
                .AddE(new LivesIn())
                .To(__ => __
                    .V<Country>()
                    .Where(t => t.CountryCallingCode == "+49"))
                .Should()
                .SerializeTo("g.addV(_a).property(Cardinality.single, _b, _c).property(Cardinality.single, _d, _e).property(Cardinality.single, _f, _g).property(Cardinality.single, _h, _i).addE(_j).to(__.V().hasLabel(_k).has(_l, _m))")
                .WithParameters("User", "Age", 0, "Gender", 0, "RegistrationDate", now, "Name", "Bob", "LivesIn", "Country", "CountryCallingCode", "+49");
        }

        [Fact]
        public void AddE_to_StepLabel()
        {
            g
                .WithModel(_model)
                .AddV(new Language { IetfLanguageTag = "en" })
                .As((_, l) => _
                    .AddV(new Country { CountryCallingCode = "+49" })
                    .AddE(new IsDescribedIn { Text = "Germany" })
                    .To(l))
                .Should()
                .SerializeTo("g.addV(_a).property(Cardinality.single, _b, _c).as(_d).addV(_e).property(Cardinality.single, _f, _g).addE(_h).property(Cardinality.single, _i, _j).to(_d)")
                .WithParameters("Language", "IetfLanguageTag", "en", "l1", "Country", "CountryCallingCode", "+49", "IsDescribedIn", "Text", "Germany");
        }

        [Fact]
        public void AddE_from_traversal()
        {
            var now = DateTimeOffset.UtcNow;

            g
                .WithModel(_model)
                .AddV(new User
                {
                    Name = "Bob",
                    RegistrationDate = now
                })
                .AddE(new LivesIn())
                .From(__ => __
                    .V<Country>()
                    .Where(t => t.CountryCallingCode == "+49"))
                .Should()
                .SerializeTo("g.addV(_a).property(Cardinality.single, _b, _c).property(Cardinality.single, _d, _e).property(Cardinality.single, _f, _g).property(Cardinality.single, _h, _i).addE(_j).from(__.V().hasLabel(_k).has(_l, _m))")
                .WithParameters("User", "Age", 0, "Gender", 0, "RegistrationDate", now, "Name", "Bob", "LivesIn", "Country", "CountryCallingCode", "+49");
        }

        [Fact]
        public void AddE_from_StepLabel()
        {
            g
                .WithModel(_model)
                .AddV(new Country { CountryCallingCode = "+49" })
                .As((_, c) => _
                    .AddV(new Language { IetfLanguageTag = "en" })
                    .AddE(new IsDescribedIn { Text = "Germany" })
                    .From(c))
                .Should()
                .SerializeTo("g.addV(_a).property(Cardinality.single, _b, _c).as(_d).addV(_e).property(Cardinality.single, _f, _g).addE(_h).property(Cardinality.single, _i, _j).from(_d)")
                .WithParameters("Country", "CountryCallingCode", "+49", "l1", "Language", "IetfLanguageTag", "en", "IsDescribedIn", "Text", "Germany");
        }

        [Fact]
        public void AddE_InV()
        {
            g
                .WithModel(_model)
                .AddV<User>()
                .AddE<LivesIn>()
                .To(__ => __
                    .V<Country>("id"))
                .InV()
                .Should()
                .SerializeTo("g.addV(_a).property(Cardinality.single, _b, _c).property(Cardinality.single, _d, _e).property(Cardinality.single, _f, _g).addE(_h).to(__.V(_i).hasLabel(_j)).inV()");
        }

        [Fact]
        public void AddE_OutV()
        {
            g
                .WithModel(_model)
                .AddV<User>()
                .AddE<LivesIn>()
                .To(__ => __
                    .V<Country>("id"))
                .OutV()
                .Should()
                .SerializeTo("g.addV(_a).property(Cardinality.single, _b, _c).property(Cardinality.single, _d, _e).property(Cardinality.single, _f, _g).addE(_h).to(__.V(_i).hasLabel(_j)).outV()");
        }

        [Fact]
        public void And()
        {
            g
                .WithModel(_model)
                .V<User>()
                .And(
                    __ => __
                        .InE<Knows>(),
                    __ => __
                        .OutE<LivesIn>())
                .Should()
                .SerializeTo("g.V().hasLabel(_a).and(__.inE(_b), __.outE(_c))")
                .WithParameters("User", "Knows", "LivesIn");
        }

        [Fact]
        public void And_nested()
        {
            g
                .WithModel(_model)
                .V<User>()
                .And(
                    __ => __
                        .OutE<LivesIn>(),
                    __ => __
                        .And(
                            ___ => ___
                                .InE<Knows>(),
                            ___ => ___
                                .OutE<Knows>()))
                .Should()
                .SerializeTo("g.V().hasLabel(_a).and(__.outE(_b), __.inE(_c), __.outE(_c))")
                .WithParameters("User", "LivesIn", "Knows");
        }

        [Fact]
        public void Or()
        {
            g
                .WithModel(_model)
                .V<User>()
                .Or(
                    __ => __
                        .InE<Knows>(),
                    __ => __
                        .OutE<LivesIn>())
                .Should()
                .SerializeTo("g.V().hasLabel(_a).or(__.inE(_b), __.outE(_c))")
                .WithParameters("User", "Knows", "LivesIn");
        }

        [Fact]
        public void Or_nested()
        {
            g
                .WithModel(_model)
                .V<User>()
                .Or(
                    __ => __
                        .OutE<LivesIn>(),
                    __ => __
                        .Or(
                            ___ => ___
                                .InE<Knows>(),
                            ___ => ___
                                .OutE<Knows>()))
                .Should()
                .SerializeTo("g.V().hasLabel(_a).or(__.outE(_b), __.inE(_c), __.outE(_c))")
                .WithParameters("User", "LivesIn", "Knows");
        }

        [Fact]
        public void Drop()
        {
            g
                .WithModel(_model)
                .V<User>()
                .Drop()
                .Should()
                .SerializeTo("g.V().hasLabel(_a).drop()")
                .WithParameters("User");
        }

        [Fact]
        public void FilterWithLambda()
        {
            g
                .WithModel(_model)
                .V<User>()
                .Filter("it.property('str').value().length() == 2")
                .Should()
                .SerializeTo("g.V().hasLabel(_a).filter({it.property('str').value().length() == 2})")
                .WithParameters("User");
        }

        [Fact]
        public void Out()
        {
            g
                .WithModel(_model)
                .V<User>()
                .Out<Knows>()
                .Should()
                .SerializeTo("g.V().hasLabel(_a).out(_b)")
                .WithParameters("User", "Knows");
        }

        [Fact]
        public void Out_does_not_include_abstract_edge()
        {
            g
                .WithModel(_model)
                .V<User>()
                .Out<Edge>()
                .Should()
                .SerializeTo("g.V().hasLabel(_a).out(_b, _c, _d, _e, _f)")
                .WithParameters("User", "IsDescribedIn", "Knows", "LivesIn", "Speaks", "WorksFor");
        }

        [Fact]
        public void OrderBy_member()
        {
            g
                .WithModel(_model)
                .V<User>()
                .OrderBy(x => x.Name)
                .Should()
                .SerializeTo("g.V().hasLabel(_a).order().by(_b, Order.incr)")
                .WithParameters("User", "Name");
        }

        [Fact]
        public void OrderBy_traversal()
        {
            g
                .WithModel(_model)
                .V<User>()
                .OrderBy(__ => __.Values(x => x.Name))
                .Should()
                .SerializeTo("g.V().hasLabel(_a).order().by(__.values(_b), Order.incr)")
                .WithParameters("User", "Name");
        }

        [Fact]
        public void OrderBy_lambda()
        {
            g
                .WithModel(_model)
                .V<User>()
                .OrderBy("it.property('str').value().length()")
                .Should()
                .SerializeTo("g.V().hasLabel(_a).order().by({it.property('str').value().length()})")
                .WithParameters("User");
        }

        [Fact]
        public void OrderByDescending_member()
        {
            g
                .WithModel(_model)
                .V<User>()
                .OrderByDescending(x => x.Name)
                .Should()
                .SerializeTo("g.V().hasLabel(_a).order().by(_b, Order.decr)")
                .WithParameters("User", "Name");
        }

        [Fact]
        public void OrderByDescending_traversal()
        {
            g
                .WithModel(_model)
                .V<User>()
                .OrderByDescending(__ => __.Values(x => x.Name))
                .Should()
                .SerializeTo("g.V().hasLabel(_a).order().by(__.values(_b), Order.decr)")
                .WithParameters("User", "Name");
        }
        
        [Fact]
        public void OrderBy_ThenBy_member()
        {
            g
                .WithModel(_model)
                .V<User>()
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Age)
                .Should()
                .SerializeTo("g.V().hasLabel(_a).order().by(_b, Order.incr).by(_c, Order.incr)")
                .WithParameters("User", "Name", "Age");
        }

        [Fact]
        public void OrderBy_ThenBy_traversal()
        {
            g
                .WithModel(_model)
                .V<User>()
                .OrderBy(__ => __.Values(x => x.Name))
                .ThenBy(__ => __.Gender)
                .Should()
                .SerializeTo("g.V().hasLabel(_a).order().by(__.values(_b), Order.incr).by(_c, Order.incr)")
                .WithParameters("User", "Name", "Gender");
        }

        [Fact]
        public void OrderBy_ThenBy_lambda()
        {
            g
                .WithModel(_model)
                .V<User>()
                .OrderBy("it.property('str1').value().length()")
                .ThenBy("it.property('str2').value().length()")
                .Should()
                .SerializeTo("g.V().hasLabel(_a).order().by({it.property('str1').value().length()}).by({it.property('str2').value().length()})")
                .WithParameters("User");
        }

        [Fact]
        public void OrderBy_ThenByDescending_member()
        {
            g
                .WithModel(_model)
                .V<User>()
                .OrderBy(x => x.Name)
                .ThenByDescending(x => x.Age)
                .Should()
                .SerializeTo("g.V().hasLabel(_a).order().by(_b, Order.incr).by(_c, Order.decr)")
                .WithParameters("User", "Name", "Age");
        }

        [Fact]
        public void OrderBy_ThenByDescending_traversal()
        {
            g
                .WithModel(_model)
                .V<User>()
                .OrderBy(__ => __.Values(x => x.Name))
                .ThenByDescending(__ => __.Gender)
                .Should()
                .SerializeTo("g.V().hasLabel(_a).order().by(__.values(_b), Order.incr).by(_c, Order.decr)")
                .WithParameters("User", "Name", "Gender");
        }

        [Fact]
        public void SumLocal()
        {
            g
                .WithModel(_model)
                .V<User>()
                .Values(x => x.Age)
                .SumLocal()
                .Should()
                .SerializeTo("g.V().hasLabel(_a).values(_b).sum(Scope.local)")
                .WithParameters("User", "Age");
        }

        [Fact]
        public void SumGlobal()
        {
            g
                .WithModel(_model)
                .V<User>()
                .Values(x => x.Age)
                .SumGlobal()
                .Should()
                .SerializeTo("g.V().hasLabel(_a).values(_b).sum(Scope.global)")
                .WithParameters("User", "Age");
        }

        [Fact]
        public void Values_one_member()
        {
            g
                .WithModel(_model)
                .V<User>()
                .Values(x => x.Name)
                .Should()
                .SerializeTo("g.V().hasLabel(_a).values(_b)")
                .WithParameters("User", "Name");
        }

        [Fact]
        public void Values_two_members()
        {
            g
                .WithModel(_model)
                .V<User>()
                .Values(x => x.Name, x => x.Id)
                .Should()
                .SerializeTo("g.V().hasLabel(_a).union(__.values(_b), __.id())")
                .WithParameters("User", "Name");
        }

        [Fact]
        public void Values_three_members()
        {
            g
                .WithModel(_model)
                .V<User>()
                .Values(x => (object)x.Name, x => x.Gender, x => x.Id)
                .Should()
                .SerializeTo("g.V().hasLabel(_a).union(__.values(_b, _c), __.id())")
                .WithParameters("User", "Name", "Gender");
        }

        [Fact]
        public void Values_id_member()
        {
            g
                .WithModel(_model)
                .V<User>()
                .Values(x => x.Id)
                .Should()
                .SerializeTo("g.V().hasLabel(_a).id()")
                .WithParameters("User");
        }

        [Fact]
        public void V_untyped()
        {
            g
                .WithModel(_model)
                .V()
                .Should()
                .SerializeTo("g.V()")
                .WithoutParameters();
        }

        [Fact]
        public void OfType_abstract()
        {
            g
                .WithModel(_model)
                .V()
                .OfType<Authority>()
                .Should()
                .SerializeTo("g.V().hasLabel(_a, _b)")
                .WithParameters("Company", "User");
        }

        [Fact]
        public void Repeat_Out()
        {
            g
                .WithModel(_model)
                .V<User>()
                .Repeat(__ => __
                    .Out<Knows>()
                    .OfType<User>())
                .Should()
                .SerializeTo("g.V().hasLabel(_a).repeat(__.out(_b).hasLabel(_a))")
                .WithParameters("User", "Knows");
        }

        [Fact]
        public void Union()
        {
            g
                .WithModel(_model)
                .V<User>()
                .Union(
                    __ => __.Out<Knows>(),
                    __ => __.Out<LivesIn>())
                .Should()
                .SerializeTo("g.V().hasLabel(_a).union(__.out(_b), __.out(_c))")
                .WithParameters("User", "Knows", "LivesIn");
        }

        [Fact]
        public void Optional()
        {
            g
                .WithModel(_model)
                .V()
                .Optional(
                    __ => __.Out<Knows>())
                .Should()
                .SerializeTo("g.V().optional(__.out(_a))")
                .WithParameters("Knows");
        }

        [Fact]
        public void Not1()
        {
            g
                .WithModel(_model)
                .V()
                .Not(__ => __.Out<Knows>())
                .Should()
                .SerializeTo("g.V().not(__.out(_a))")
                .WithParameters("Knows");
        }

        [Fact]
        public void Not2()
        {
            g
                .WithModel(_model)
                .V()
                .Not(__ => __.OfType<Language>())
                .Should()
                .SerializeTo("g.V().not(__.hasLabel(_a))")
                .WithParameters("Language");
        }

        [Fact]
        public void Not3()
        {
            g
                .WithModel(_model)
                .V()
                .Not(__ => __.OfType<Authority>())
                .Should()
                .SerializeTo("g.V().not(__.hasLabel(_a, _b))")
                .WithParameters("Company", "User");
        }

        [Fact]
        public void As_explicit_label()
        {
            g
                .WithModel(_model)
                .V<User>()
                .As(new StepLabel<User>())
                .Should()
                .SerializeTo("g.V().hasLabel(_a).as(_b)")
                .WithParameters("User", "l1");
        }

        [Fact]
        public void Select()
        {
            var stepLabel = new StepLabel<User>();

            g
                .WithModel(_model)
                .V<User>()
                .As(stepLabel)
                .Select(stepLabel)
                .Should()
                .SerializeTo("g.V().hasLabel(_a).as(_b).select(_b)")
                .WithParameters("User", "l1");
        }

        [Fact]
        public void As_inlined_nested_Select()
        {
            g
                .WithModel(_model)
                .V<User>()
                .As((_, stepLabel1) => _
                    .As((__, stepLabel2) => __
                        .Select(stepLabel1, stepLabel2)))
                .Should()
                .SerializeTo("g.V().hasLabel(_a).as(_b).as(_c).select(_b, _c)")
                .WithParameters("User", "Item1", "Item2");
        }

        [Fact]
        public void Property_single()
        {
            g
                .WithModel(_model)
                .V<User>()
                .Property(x => x.Age, 36)
                .Should()
                .SerializeTo("g.V().hasLabel(_a).property(Cardinality.single, _b, _c)")
                .WithParameters("User", "Age", 36);
        }

        [Fact]
        public void Property_list()
        {
            g
                .WithModel(_model)
                .V<User>("id")
                .Property(x => x.PhoneNumbers, "+4912345")
                .Should()
                .SerializeTo("g.V(_a).hasLabel(_b).property(Cardinality.list, _c, _d)")
                .WithParameters("id", "User", "PhoneNumbers", "+4912345");
        }

        [Fact]
        public void Coalesce()
        {
            g
                .WithModel(_model)
                .V()
                .Coalesce(
                     _ => _
                        .Identity())
                .Should()
                .SerializeTo("g.V().coalesce(__.identity())")
                .WithoutParameters();
        }

        [Fact]
        public void Properties()
        {
            g
                .WithModel(_model)
                .V()
                .Properties()
                .Should()
                .SerializeTo("g.V().properties()")
                .WithoutParameters();
        }

        [Fact]
        public void Properties_of_member()
        {
            g
                .WithModel(_model)
                .V<Country>()
                .Properties(x => x.Name)
                .Should()
                .SerializeTo("g.V().hasLabel(_a).properties(_b)")
                .WithParameters("Country", "Name");
        }

        [Fact]
        public void Properties_Where()
        {
            g
                .WithModel(_model)
                .V<Country>()
                .Properties(x => x.Languages)
                .Where(x => x.Value == "de")
                .Should()
                .SerializeTo("g.V().hasLabel(_a).properties(_b).hasValue(_c)")
                .WithParameters("Country", "Languages", "de");
        }

        [Fact]
        public void Properties_Where_reversed()
        {
            g
                .WithModel(_model)
                .V<Country>()
                .Properties(x => x.Languages)
                .Where(x => "de" == x.Value)
                .Should()
                .SerializeTo("g.V().hasLabel(_a).properties(_b).hasValue(_c)")
                .WithParameters("Country", "Languages", "de");
        }

        [Fact]
        public void Meta_Properties()
        {
            g
                .WithModel(_model)
                .V<Country>()
                .Properties(x => x.Name)
                .Properties()
                .Should()
                .SerializeTo("g.V().hasLabel(_a).properties(_b).properties()")
                .WithParameters("Country", "Name");
        }

        [Fact]
        public void Meta_Properties_with_key()
        {
            g
                .WithModel(_model)
                .V<Country>()
                .Properties(x => x.Name)
                .Properties("metaKey")
                .Should()
                .SerializeTo("g.V().hasLabel(_a).properties(_b).properties(_c)")
                .WithParameters("Country", "Name", "metaKey");
        }

        [Fact]
        public void Limit_overflow()
        {
            g
                .V()
                .Invoking(_ => _.Limit((long)int.MaxValue + 1))
                .Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public void Anonymous()
        {
            GremlinQuery.Anonymous
                .Should()
                .SerializeTo("__.identity()")
                .WithoutParameters();
        }

        [Fact]
        public void Inject()
        {
            g
                .WithModel(_model)
                .Cast<int>()
                .Inject(36, 37, 38)
                .Should()
                .SerializeTo("g.inject(_a, _b, _c)")
                .WithParameters(36, 37, 38);
        }

        [Fact]
        public void WithSubgraphStrategy()
        {
            g
                .WithSubgraphStrategy(_ => _.OfType<User>(), _ => _)
                .WithModel(_model)
                .Should()
                .SerializeTo("g.withStrategies(SubgraphStrategy.build().vertices(__.hasLabel(_a)).edges(__.identity()).create())")
                .WithParameters("User");
        }

        [Fact]
        public void WithSubgraphStrategy_empty()
        {
            g
                .WithSubgraphStrategy(_ => _, _ => _)
                .WithModel(GraphModel.Empty)
                .Should()
                .SerializeTo("g")
                .WithoutParameters();
        }
    }
}
