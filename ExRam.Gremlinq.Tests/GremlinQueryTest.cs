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
                .V<User>()
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1)")
                .WithParameters("User");
        }

        [Fact]
        public void V_of_abstract_type()
        {
            g
                .V<Authority>()
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1, _P2)")
                .WithParameters("Company", "User");
        }

        [Fact]
        public void AddV()
        {
            g
                .AddV(new Language { Id = "id", IetfLanguageTag = "en" })
                .Resolve(_model)
                .Should()
                .SerializeTo("g.addV(_P1).property(Cardinality.single, _P2, _P3).property(Cardinality.single, T.id, _P4)")
                .WithParameters("Language", "IetfLanguageTag", "en", "id");
        }

        [Fact]
        public void AddV_without_id()
        {
            g
                .AddV(new Language { IetfLanguageTag = "en" })
                .Resolve(_model)
                .Should()
                .SerializeTo("g.addV(_P1).property(Cardinality.single, _P2, _P3)")
                .WithParameters("Language", "IetfLanguageTag", "en");
        }

        [Fact]
        public void AddV_with_nulls()
        {
            g
                .AddV(new Language {Id = "id"})
                .Resolve(_model)
                .Should()
                .SerializeTo("g.addV(_P1).property(Cardinality.single, T.id, _P2)")
                .WithParameters("Language", "id");
        }

        [Fact]
        public void AddV_with_multi_property()
        {
            g
                .AddV(new User { Id = "id", PhoneNumbers = new[] { "+4912345", "+4923456" } })
                .Resolve(_model)
                .Should()
                .SerializeTo("g.addV(_P1).property(Cardinality.single, _P2, _P3).property(Cardinality.single, _P4, _P5).property(Cardinality.single, _P6, _P7).property(Cardinality.list, _P8, _P9).property(Cardinality.list, _P8, _P10).property(Cardinality.single, T.id, _P11)")
                .WithParameters("User", "Age", 0, "Gender", 0, "RegistrationDate", DateTimeOffset.MinValue, "PhoneNumbers", "+4912345", "+4923456", "id");
        }

        [Fact]
        public void AddV_with_Meta_without_properties()
        {
            g
                .AddV(new Country { Id = "id", Name = "GER"})
                .Resolve(_model)
                .Should()
                .SerializeTo("g.addV(_P1).property(Cardinality.single, _P2, _P3).property(Cardinality.single, T.id, _P4)")
                .WithParameters("Country", "Name", "GER", "id");
        }

        [Fact]
        public void AddV_with_Meta_with_properties()
        {
            g
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
                .Resolve(_model)
                .Should()
                .SerializeTo("g.addV(_P1).property(Cardinality.single, _P2, _P3, _P4, _P5, _P6, _P7).property(Cardinality.single, T.id, _P8)")
                .WithParameters("Country", "Name", "GER", "de", "Deutschland", "en", "Germany", "id");
        }
        
        [Fact]
        public void AddV_with_enum_property()
        {
            g
                .AddV(new User { Id = "id", Gender = Gender.Female })
                .Resolve(_model)
                .Should()
                .SerializeTo("g.addV(_P1).property(Cardinality.single, _P2, _P3).property(Cardinality.single, _P4, _P5).property(Cardinality.single, _P6, _P7).property(Cardinality.single, T.id, _P8)")
                .WithParameters("User", "Age", 0, "Gender" , 1, "RegistrationDate", DateTimeOffset.MinValue, "id");
        }

        [Fact]
        public void Where_property_array_contains_element()
        {
            g
                .V<User>()
                .Where(t => t.PhoneNumbers.Contains("+4912345"))
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).has(_P2, P.eq(_P3))")
                .WithParameters("User", "PhoneNumbers", "+4912345");
        }

        [Fact]
        public void Where_property_array_does_not_contain_element()
        {
            g
                .V<User>()
                .Where(t => !t.PhoneNumbers.Contains("+4912345"))
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).not(__.has(_P2, P.eq(_P3)))")
                .WithParameters("User", "PhoneNumbers", "+4912345");
        }

        [Fact]
        public void Where_property_array_is_not_empty()
        {
            g
                .V<User>()
                .Where(t => t.PhoneNumbers.Any())
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).has(_P2)")
                .WithParameters("User", "PhoneNumbers");
        }

        [Fact]
        public void Where_property_array_is_empty()
        {
            g
                .V<User>()
                .Where(t => !t.PhoneNumbers.Any())
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).not(__.has(_P2))")
                .WithParameters("User", "PhoneNumbers");
        }

        [Fact]
        public void Where_property_array_intersects_aray()
        {
            g
                .V<User>()
                .Where(t => t.PhoneNumbers.Intersects(new[] { "+4912345", "+4923456" }))
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).has(_P2, P.within(_P3, _P4))")
                .WithParameters("User", "PhoneNumbers", "+4912345", "+4923456");
        }

        [Fact]
        public void Where_property_array_does_not_intersect_array()
        {
            g
                .V<User>()
                .Where(t => !t.PhoneNumbers.Intersects(new[] { "+4912345", "+4923456" }))
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).not(__.has(_P2, P.within(_P3, _P4)))")
                .WithParameters("User", "PhoneNumbers", "+4912345", "+4923456");
        }

        [Fact]
        public void Where_property_is_contained_in_array()
        {
            g
                .V<User>()
                .Where(t => new[] { 36, 37, 38 }.Contains(t.Age))
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).has(_P2, P.within(_P3, _P4, _P5))")
                .WithParameters("User", "Age", 36, 37, 38);
        }

        [Fact]
        public void Where_property_is_contained_in_enumerable()
        {
            var enumerable = new[] { "36", "37", "38" }
                .Select(int.Parse);

            g
                .V<User>()
                .Where(t => enumerable.Contains(t.Age))
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).has(_P2, P.within(_P3, _P4, _P5))")
                .WithParameters("User", "Age", 36, 37, 38);
        }

        [Fact]
        public void Where_property_is_contained_in_empty_enumerable()
        {
            var enumerable = Enumerable.Empty<int>();

            g
                .V<User>()
                .Where(t => enumerable.Contains(t.Age))
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).has(_P2, __.not(__.identity()))")
                .WithParameters("User", "Age");
        }

        [Fact]
        public void Where_property_is_not_contained_in_array()
        {
            g
                .V<User>()
                .Where(t => !new[] { 36, 37, 38 }.Contains(t.Age))
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).not(__.has(_P2, P.within(_P3, _P4, _P5)))")
                .WithParameters("User", "Age", 36, 37, 38);
        }
        
        [Fact]
        public void Where_property_is_not_contained_in_enumerable()
        {
            var enumerable = new[] { "36", "37", "38" }
                .Select(int.Parse);

            g
                .V<User>()
                .Where(t => !enumerable.Contains(t.Age))
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).not(__.has(_P2, P.within(_P3, _P4, _P5)))")
                .WithParameters("User", "Age", 36, 37, 38);
        }

        [Fact]
        public void Where_property_is_not_contained_in_empty_enumerable()
        {
            var enumerable = Enumerable.Empty<int>();

            g
                .V<User>()
                .Where(t => !enumerable.Contains(t.Age))
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1)")
                .WithParameters("User");
        }

        [Fact]
        public void Where_property_is_prefix_of_constant()
        {
            g
                .V<CountryCallingCode>()
                .Where(c => "+49123".StartsWith(c.Prefix))
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).has(_P2, P.within(_P3, _P4, _P5, _P6, _P7, _P8, _P9))")
                .WithParameters("CountryCallingCode", "Prefix", "", "+", "+4", "+49", "+491", "+4912", "+49123");
        }

        [Fact]
        public void Where_property_is_prefix_of_variable()
        {
            const string str = "+49123";

            g
                .V<CountryCallingCode>()
                .Where(c => str.StartsWith(c.Prefix))
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).has(_P2, P.within(_P3, _P4, _P5, _P6, _P7, _P8, _P9))")
                .WithParameters("CountryCallingCode", "Prefix", "", "+", "+4", "+49", "+491", "+4912", "+49123");
        }

        [Fact]
        public void Where_property_is_prefix_of_expression()
        {
            const string str = "+49123xxx";

            g
                .V<CountryCallingCode>()
                .Where(c => str.Substring(0, 6).StartsWith(c.Prefix))
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).has(_P2, P.within(_P3, _P4, _P5, _P6, _P7, _P8, _P9))")
                .WithParameters("CountryCallingCode", "Prefix", "", "+", "+4", "+49", "+491", "+4912", "+49123");
        }

        [Fact]
        public void Where_property_starts_with_constant()
        {
            g
                .V<User>()
                .Where(c => c.PhoneNumber.StartsWith("+49123"))
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).has(_P2, P.between(_P3, _P4))")
                .WithParameters("User", "PhoneNumber", "+49123", "+49124");
        }

        [Fact]
        public void Where_property_starts_with_empty_constant()
        {
            g
                .V<User>()
                .Where(c => c.PhoneNumber.StartsWith(""))
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).has(_P2, P.between(_P3, _P4))")
                .WithParameters("User", "PhoneNumber", "", char.MinValue.ToString());
        }

        [Fact]
        public void Where_disjunction()
        {
            g
                .V<User>()
                .Where(t => t.Age == 36 || t.Age == 42)
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).or(__.has(_P2, P.eq(_P3)), __.has(_P2, P.eq(_P4)))")
                .WithParameters("User", "Age", 36, 42);
        }

        [Fact]
        public void Where_disjunction_with_different_fields()
        {
            g
                .V<User>()
                .Where(t => t.Name == "Some name" || t.Age == 42)
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).or(__.has(_P2, P.eq(_P3)), __.has(_P4, P.eq(_P5)))")
                .WithParameters("User", "Name", "Some name", "Age", 42);
        }

        [Fact]
        public void Where_conjunction()
        {
            g
                .V<User>()
                .Where(t => t.Age == 36 && t.Age == 42)
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).and(__.has(_P2, P.eq(_P3)), __.has(_P2, P.eq(_P4)))")
                .WithParameters("User", "Age", 36, 42);
        }

        [Fact]
        public void Where_complex_logical_expression()
        {
            g
                .V<User>()
                .Where(t => t.Name == "Some name" && (t.Age == 42 || t.Age == 99))
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).and(__.has(_P2, P.eq(_P3)), __.or(__.has(_P4, P.eq(_P5)), __.has(_P4, P.eq(_P6))))")
                .WithParameters("User", "Name", "Some name", "Age", 42, 99);
        }

        [Fact]
        public void Where_complex_logical_expression_with_null()
        {
            g
                .V<User>()
                .Where(t => t.Name == null && (t.Age == 42 || t.Age == 99))
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).and(__.not(__.has(_P2)), __.or(__.has(_P3, P.eq(_P4)), __.has(_P3, P.eq(_P5))))")
                .WithParameters("User", "Name", "Age", 42, 99);
        }

        [Fact]
        public void Where_has_conjunction_of_three()
        {
            g
                .V<User>()
                .Where(t => t.Age == 36 && t.Age == 42 && t.Age == 99)
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).and(__.has(_P2, P.eq(_P3)), __.has(_P2, P.eq(_P4)), __.has(_P2, P.eq(_P5)))")
                .WithParameters("User", "Age", 36, 42, 99);
        }

        [Fact]
        public void Where_has_disjunction_of_three()
        {
            g
                .V<User>()
                .Where(t => t.Age == 36 || t.Age == 42 || t.Age == 99)
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).or(__.has(_P2, P.eq(_P3)), __.has(_P2, P.eq(_P4)), __.has(_P2, P.eq(_P5)))")
                .WithParameters("User", "Age", 36, 42, 99);
        }

        [Fact]
        public void Where_conjunction_with_different_fields()
        {
            g
                .V<User>()
                .Where(t => t.Name == "Some name" && t.Age == 42)
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).and(__.has(_P2, P.eq(_P3)), __.has(_P4, P.eq(_P5)))")
                .WithParameters("User", "Name", "Some name", "Age", 42);
        }

        [Fact]
        public void Where_property_equals_constant()
        {
            g
                .V<User>()
                .Where(t => t.Age == 36)
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).has(_P2, P.eq(_P3))")
                .WithParameters("User", "Age", 36);
        }

        [Fact]
        public void Where_property_equals_expression()
        {
            const int i = 18;

            g
                .V<User>()
                .Where(t => t.Age == i + i)
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).has(_P2, P.eq(_P3))")
                .WithParameters("User", "Age", 36);
        }

        [Fact]
        public void Where_property_equals_converted_expression()
        {
            g
                .V<User>()
                .Where(t => (object)t.Age == (object)36)
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).has(_P2, P.eq(_P3))")
                .WithParameters("User", "Age", 36);
        }

        [Fact]
        public void Where_property_not_equals_constant()
        {
            g
                .V<User>()
                .Where(t => t.Age != 36)
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).has(_P2, P.neq(_P3))")
                .WithParameters("User", "Age", 36);
        }

        [Fact]
        public void Where_property_is_not_present()
        {
            g
                .V<User>()
                .Where(t => t.Name == null)
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).not(__.has(_P2))")
                .WithParameters("User", "Name");
        }

        [Fact]
        public void Where_property_is_present()
        {
            g
                .V<User>()
                .Where(t => t.Name != null)
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).has(_P2)")
                .WithParameters("User", "Name");
        }

        [Fact]
        public void Where_property_is_lower_than_constant()
        {
            g
                .V<User>()
                .Where(t => t.Age < 36)
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).has(_P2, P.lt(_P3))")
                .WithParameters("User", "Age", 36);
        }

        [Fact]
        public void Where_property_is_lower_or_equal_than_constant()
        {
            g
                .V<User>()
                .Where(t => t.Age <= 36)
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).has(_P2, P.lte(_P3))")
                .WithParameters("User", "Age", 36);
        }

        [Fact]
        public void Where_bool_property_explicit_comparison1()
        {
            g
                .V<TimeFrame>()
                // ReSharper disable once RedundantBoolCompare
                .Where(t => t.Enabled == true)
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).has(_P2, P.eq(_P3))")
                .WithParameters("TimeFrame", "Enabled", true);
        }

        [Fact]
        public void Where_bool_property_explicit_comparison2()
        {
            g
                .V<TimeFrame>()
                .Where(t => t.Enabled == false)
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).has(_P2, P.eq(_P3))")
                .WithParameters("TimeFrame", "Enabled", false);
        }

        [Fact]
        public void Where_bool_property_implicit_comparison1()
        {
            g
                .V<TimeFrame>()
                .Where(t => t.Enabled)
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).has(_P2, P.eq(_P3))")
                .WithParameters("TimeFrame", "Enabled", true);
        }

        [Fact]
        public void Where_bool_property_implicit_comparison2()
        {
            g
                .V<TimeFrame>()
                .Where(t => !t.Enabled)
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).not(__.has(_P2, P.eq(_P3)))")
                .WithParameters("TimeFrame", "Enabled", true);
        }

        [Fact]
        public void Where_property_is_greater_than_constant()
        {
            g
                .V<User>()
                .Where(t => t.Age > 36)
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).has(_P2, P.gt(_P3))")
                .WithParameters("User", "Age", 36);
        }

        [Fact]
        public void Where_property_is_greater_or_equal_than_constant()
        {
            g
                .V<User>()
                .Where(t => t.Age >= 36)
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).has(_P2, P.gte(_P3))")
                .WithParameters("User", "Age", 36);
        }

        [Fact]
        public void Where_property_equals_string_constant()
        {
            g
                .V<Language>()
                .Where(t => t.Id == "languageId")
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).has(T.id, P.eq(_P2))")
                .WithParameters("Language", "languageId");
        }

        [Fact]
        public void Where_property_equals_local_string_constant()
        {
            const string local = "languageId";

            g
                .V<Language>()
                .Where(t => t.Id == local)
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).has(T.id, P.eq(_P2))")
                .WithParameters("Language", "languageId");
        }

        [Fact]
        public void Where_property_equals_value_of_anonymous_object()
        {
            var local = new { Value = "languageId" };

            g
                .V<Language>()
                .Where(t => t.Id == local.Value)
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).has(T.id, P.eq(_P2))")
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
                .V<Language>()
                .As(l)
                .V<Language>()
                .Where(l2 => l2 == l)
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).as(_P2).V().hasLabel(_P1).where(P.eq(_P2))")
                .WithParameters("Language", "l1");
        }

        [Fact]
        public void Where_property_equals_stepLabel()
        {
            var l = new StepLabel<string>();

            g
                .V<Language>()
                .Values(x => x.IetfLanguageTag)
                .As(l)
                .V<Language>()
                .Where(l2 => l2.IetfLanguageTag == l)
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).values(_P2).as(_P3).V().hasLabel(_P1).has(_P2, __.where(P.eq(_P3)))")
                .WithParameters("Language", "IetfLanguageTag", "l1");
        }

        [Fact]
        public void Where_scalar_element_equals_constant()
        {
            g
                .V<User>()
                .Values(x => x.Age)
                .Where(_ => _ == 36)
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).values(_P2).is(P.eq(_P3))")
                .WithParameters("User", "Age", 36);
        }

        [Fact]
        public void Where_traversal()
        {
            g
                .V<User>()
                .Where(_ => _.Out<LivesIn>())
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).where(__.out(_P2))")
                .WithParameters("User", "LivesIn");
        }

        [Fact]
        public void Where_property_traversal()
        {
            g
                .V<User>()
                .Where(
                    x => x.Age,
                    _ => _
                        .Inject(36))
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).has(_P2, __.inject(_P3))")
                .WithParameters("User", "Age", 36);
        }

        [Fact]
        public void AddE_to_traversal()
        {
            var now = DateTimeOffset.UtcNow;

            g
                .AddV(new User
                {
                    Name = "Bob",
                    RegistrationDate = now
                })
                .AddE(new LivesIn())
                .To(__ => __
                    .V<Country>()
                    .Where(t => t.CountryCallingCode == "+49"))
                .Resolve(_model)
                .Should()
                .SerializeTo("g.addV(_P1).property(Cardinality.single, _P2, _P3).property(Cardinality.single, _P4, _P5).property(Cardinality.single, _P6, _P7).property(Cardinality.single, _P8, _P9).addE(_P10).to(__.V().hasLabel(_P11).has(_P12, P.eq(_P13)))")
                .WithParameters("User", "Age", 0, "Gender", 0, "RegistrationDate", now, "Name", "Bob", "LivesIn", "Country", "CountryCallingCode", "+49");
        }

        [Fact]
        public void AddE_to_StepLabel()
        {
            g
                .AddV(new Language { IetfLanguageTag = "en" })
                .As((_, l) => _
                    .AddV(new Country { CountryCallingCode = "+49" })
                    .AddE(new IsDescribedIn { Text = "Germany" })
                    .To(l))
                .Resolve(_model)
                .Should()
                .SerializeTo("g.addV(_P1).property(Cardinality.single, _P2, _P3).as(_P4).addV(_P5).property(Cardinality.single, _P6, _P7).addE(_P8).property(Cardinality.single, _P9, _P10).to(_P4)")
                .WithParameters("Language", "IetfLanguageTag", "en", "l1", "Country", "CountryCallingCode", "+49", "IsDescribedIn", "Text", "Germany");
        }

        [Fact]
        public void AddE_from_traversal()
        {
            var now = DateTimeOffset.UtcNow;

            g
                .AddV(new User
                {
                    Name = "Bob",
                    RegistrationDate = now
                })
                .AddE(new LivesIn())
                .From(__ => __
                    .V<Country>()
                    .Where(t => t.CountryCallingCode == "+49"))
                .Resolve(_model)
                .Should()
                .SerializeTo("g.addV(_P1).property(Cardinality.single, _P2, _P3).property(Cardinality.single, _P4, _P5).property(Cardinality.single, _P6, _P7).property(Cardinality.single, _P8, _P9).addE(_P10).from(__.V().hasLabel(_P11).has(_P12, P.eq(_P13)))")
                .WithParameters("User", "Age", 0, "Gender", 0, "RegistrationDate", now, "Name", "Bob", "LivesIn", "Country", "CountryCallingCode", "+49");
        }

        [Fact]
        public void AddE_from_StepLabel()
        {
            g
                .AddV(new Country { CountryCallingCode = "+49" })
                .As((_, c) => _
                    .AddV(new Language { IetfLanguageTag = "en" })
                    .AddE(new IsDescribedIn { Text = "Germany" })
                    .From(c))
                .Resolve(_model)
                .Should()
                .SerializeTo("g.addV(_P1).property(Cardinality.single, _P2, _P3).as(_P4).addV(_P5).property(Cardinality.single, _P6, _P7).addE(_P8).property(Cardinality.single, _P9, _P10).from(_P4)")
                .WithParameters("Country", "CountryCallingCode", "+49", "l1", "Language", "IetfLanguageTag", "en", "IsDescribedIn", "Text", "Germany");
        }

        [Fact]
        public void AddE_InV()
        {
            g
                .AddV<User>()
                .AddE<LivesIn>()
                .To(__ => __
                    .V<Country>("id"))
                .InV()
                .Resolve(_model)
                .Should()
                .SerializeTo("g.addV(_P1).property(Cardinality.single, _P2, _P3).property(Cardinality.single, _P4, _P5).property(Cardinality.single, _P6, _P7).addE(_P8).to(__.V(_P9).hasLabel(_P10)).inV()");
        }

        [Fact]
        public void AddE_OutV()
        {
            g
                .AddV<User>()
                .AddE<LivesIn>()
                .To(__ => __
                    .V<Country>("id"))
                .OutV()
                .Resolve(_model)
                .Should()
                .SerializeTo("g.addV(_P1).property(Cardinality.single, _P2, _P3).property(Cardinality.single, _P4, _P5).property(Cardinality.single, _P6, _P7).addE(_P8).to(__.V(_P9).hasLabel(_P10)).outV()");
        }

        [Fact]
        public void And()
        {
            g
                .V<User>()
                .And(
                    __ => __
                        .InE<Knows>(),
                    __ => __
                        .OutE<LivesIn>())
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).and(__.inE(_P2), __.outE(_P3))")
                .WithParameters("User", "Knows", "LivesIn");
        }

        [Fact]
        public void Drop()
        {
            g
                .V<User>()
                .Drop()
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).drop()")
                .WithParameters("User");
        }

        [Fact]
        public void FilterWithLambda()
        {
            g
                .V<User>()
                .Filter("it.property('str').value().length() == 2")
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).filter({it.property('str').value().length() == 2})")
                .WithParameters("User");
        }

        [Fact]
        public void Out()
        {
            g
                .V<User>()
                .Out<Knows>()
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).out(_P2)")
                .WithParameters("User", "Knows");
        }

        [Fact]
        public void Out_does_not_include_abstract_edge()
        {
            g
                .V<User>()
                .Out<Edge>()
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).out(_P2, _P3, _P4, _P5, _P6)")
                .WithParameters("User", "IsDescribedIn", "Knows", "LivesIn", "Speaks", "WorksFor");
        }

        [Fact]
        public void OrderBy_member()
        {
            g
                .V<User>()
                .OrderBy(x => x.Name)
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).order().by(_P2, Order.incr)")
                .WithParameters("User", "Name");
        }

        [Fact]
        public void OrderBy_traversal()
        {
            g
                .V<User>()
                .OrderBy(__ => __.Values(x => x.Name))
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).order().by(__.values(_P2), Order.incr)")
                .WithParameters("User", "Name");
        }

        [Fact]
        public void OrderBy_lambda()
        {
            g
                .V<User>()
                .OrderBy("it.property('str').value().length()")
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).order().by({it.property('str').value().length()})")
                .WithParameters("User");
        }

        [Fact]
        public void SumLocal()
        {
            g
                .V<User>()
                .Values(x => x.Age)
                .SumLocal()
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).values(_P2).sum(Scope.local)")
                .WithParameters("User", "Age");
        }

        [Fact]
        public void SumGlobal()
        {
            g
                .V<User>()
                .Values(x => x.Age)
                .SumGlobal()
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).values(_P2).sum(Scope.global)")
                .WithParameters("User", "Age");
        }

        [Fact]
        public void Values_one_member()
        {
            g
                .V<User>()
                .Values(x => x.Name)
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).values(_P2)")
                .WithParameters("User", "Name");
        }

        [Fact]
        public void Values_two_members()
        {
            g
                .V<User>()
                .Values(x => x.Name, x => x.Id)
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).union(__.values(_P2), __.id())")
                .WithParameters("User", "Name");
        }

        [Fact]
        public void Values_three_members()
        {
            g
                .V<User>()
                .Values(x => (object)x.Name, x => x.Gender, x => x.Id)
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).union(__.values(_P2, _P3), __.id())")
                .WithParameters("User", "Name", "Gender");
        }

        [Fact]
        public void Values_id_member()
        {
            g
                .V<User>()
                .Values(x => x.Id)
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).id()")
                .WithParameters("User");
        }

        [Fact]
        public void V_untyped()
        {
            g
                .V()
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V()")
                .WithoutParameters();
        }

        [Fact]
        public void OfType_abstract()
        {
            g
                .V()
                .OfType<Authority>()
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1, _P2)")
                .WithParameters("Company", "User");
        }

        [Fact]
        public void Repeat_Out()
        {
            g
                .V<User>()
                .Repeat(__ => __
                    .Out<Knows>()
                    .OfType<User>())
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).repeat(__.out(_P2).hasLabel(_P1))")
                .WithParameters("User", "Knows");
        }

        [Fact]
        public void Union()
        {
            g
                .V<User>()
                .Union(
                    __ => __.Out<Knows>(),
                    __ => __.Out<LivesIn>())
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).union(__.out(_P2), __.out(_P3))")
                .WithParameters("User", "Knows", "LivesIn");
        }

        [Fact]
        public void Optional()
        {
            g
                .V()
                .Optional(
                    __ => __.Out<Knows>())
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().optional(__.out(_P1))")
                .WithParameters("Knows");
        }

        [Fact]
        public void Not1()
        {
            g
                .V()
                .Not(__ => __.Out<Knows>())
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().not(__.out(_P1))")
                .WithParameters("Knows");
        }

        [Fact]
        public void Not2()
        {
            g
                .V()
                .Not(__ => __.OfType<Language>())
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().not(__.hasLabel(_P1))")
                .WithParameters("Language");
        }

        [Fact]
        public void Not3()
        {
            g
                .V()
                .Not(__ => __.OfType<Authority>())
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().not(__.hasLabel(_P1, _P2))")
                .WithParameters("Company", "User");
        }

        [Fact]
        public void As_explicit_label()
        {
            g
                .V<User>()
                .As(new StepLabel<User>())
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).as(_P2)")
                .WithParameters("User", "l1");
        }

        [Fact]
        public void Select()
        {
            var stepLabel = new StepLabel<User>();

            g
                .V<User>()
                .As(stepLabel)
                .Select(stepLabel)
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).as(_P2).select(_P2)")
                .WithParameters("User", "l1");
        }

        [Fact]
        public void As_inlined_nested_Select()
        {
            g
                .V<User>()
                .As((_, stepLabel1) => _
                    .As((__, stepLabel2) => __
                        .Select(stepLabel1, stepLabel2)))
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).as(_P2).as(_P3).select(_P2, _P3)")
                .WithParameters("User", "Item1", "Item2");
        }

        //[Fact]
        //public void Branch()
        //{
        //    var query = g
        //        .V<User>()
        //        .Branch(
        //            _ => _.Values(x => x.Name),
        //            _ => _.Out<Knows>(),
        //            _ => _.In<Knows>())
        //        .Resolve(_model)
        //        .Serialize();

        //    query.queryString
        //        .Should()
        //        .Be("g.V().hasLabel(_P1).branch(__.values(_P2)).option(__.out(_P3)).option(__.in(_P3))");

        //    query.parameters
        //        .Should()
        //        .Contain("_P1", "User").And
        //        .Contain("_P2", "Name").And
        //        .Contain("_P3", "Knows");
        //}

        //[Fact]
        //public void BranchOnIdentity()
        //{
        //    var query = g
        //        .V<User>()
        //        .BranchOnIdentity(
        //            _ => _.Out<Knows>(),
        //            _ => _.In<Knows>())
        //        .Resolve(_model)
        //        .Serialize();

        //    query.queryString
        //        .Should()
        //        .Be("g.V().hasLabel(_P1).branch(__.identity()).option(__.out(_P2)).option(__.in(_P2))");

        //    query.parameters
        //        .Should()
        //        .Contain("_P1", "User").And
        //        .Contain("_P2", "Knows");
        //}

        [Fact]
        public void Property_single()
        {
            g
                .V<User>()
                .Property(x => x.Age, 36)
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).property(Cardinality.single, _P2, _P3)")
                .WithParameters("User", "Age", 36);
        }

        [Fact]
        public void Property_list()
        {
            g
                .V<User>("id")
                .Property(x => x.PhoneNumbers, "+4912345")
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V(_P1).hasLabel(_P2).property(Cardinality.list, _P3, _P4)")
                .WithParameters("id", "User", "PhoneNumbers", "+4912345");
        }

        [Fact]
        public void Coalesce()
        {
            g
                .V()
                .Coalesce(
                     _ => _
                        .Identity())
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().coalesce(__.identity())")
                .WithoutParameters();
        }

        [Fact]
        public void Properties()
        {
            g
                .V()
                .Properties()
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().properties()")
                .WithoutParameters();
        }

        [Fact]
        public void Properties_of_member()
        {
            g
                .V<Country>()
                .Properties(x => x.Name)
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).properties(_P2)")
                .WithParameters("Country", "Name");
        }

        [Fact]
        public void Properties_Where()
        {
            g
                .V<Country>()
                .Properties(x => x.Languages)
                .Where(x => x.Value == "de")
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).properties(_P2).hasValue(P.eq(_P3))")
                .WithParameters("Country", "Languages", "de");
        }

        [Fact]
        public void Properties_Where_reversed()
        {
            g
                .V<Country>()
                .Properties(x => x.Languages)
                .Where(x => "de" == x.Value)
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).properties(_P2).hasValue(P.eq(_P3))")
                .WithParameters("Country", "Languages", "de");
        }

        [Fact]
        public void Meta_Properties()
        {
            g
                .V<Country>()
                .Properties(x => x.Name)
                .Properties()
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).properties(_P2).properties()")
                .WithParameters("Country", "Name");
        }

        [Fact]
        public void Meta_Properties_with_key()
        {
            g
                .V<Country>()
                .Properties(x => x.Name)
                .Properties("metaKey")
                .Resolve(_model)
                .Should()
                .SerializeTo("g.V().hasLabel(_P1).properties(_P2).properties(_P3)")
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
                .Resolve(_model)
                .Should()
                .SerializeTo("__.identity()")
                .WithoutParameters();
        }

        [Fact]
        public void Inject()
        {
            g
                .Inject(36, 37, 38)
                .Resolve(_model)
                .Should()
                .SerializeTo("g.inject(_P1, _P2, _P3)")
                .WithParameters(36, 37, 38);
        }

        [Fact]
        public void WithSubgraphStrategy()
        {
            g
                .WithSubgraphStrategy(_ => _.OfType<User>(), _ => _)
                .Resolve(_model)
                .Should()
                .SerializeTo("g.withStrategies(SubgraphStrategy.build().vertices(__.hasLabel(_P1)).edges(__.identity()).create())")
                .WithParameters("User");
        }

        [Fact]
        public void WithSubgraphStrategy_empty()
        {
            g
                .WithSubgraphStrategy(_ => _, _ => _)
                .Resolve(GraphModel.Empty)
                .Should()
                .SerializeTo("g")
                .WithoutParameters();
        }
    }
}
