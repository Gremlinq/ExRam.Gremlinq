using System;
using System.Linq;
using ExRam.Gremlinq.Core.GraphElements;
using ExRam.Gremlinq.Core.Serialization;
using FluentAssertions;
using Xunit;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public abstract class GroovySerializationTest<TVisitor> where TVisitor : IGremlinQueryElementVisitor<SerializedGremlinQuery>, new()
    {
        [Fact]
        public void V_of_concrete_type()
        {
            g
                .V<User>()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a)")
                .WithParameters("User");
        }

        [Fact]
        public void V_of_abstract_type()
        {
            g
                .V<Authority>()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a, _b)")
                .WithParameters("Company", "User");
        }

        [Fact]
        public void V_of_all_types1()
        {
            g
                .V<object>()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V()")
                .WithoutParameters();
        }

        [Fact]
        public void V_of_all_types2()
        {
            g
                .V<IVertex>()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V()")
                .WithoutParameters();
        }

        [Fact]
        public void V_of_type_outside_model()
        {
            g
                .Invoking(_ => _
                    .V<string>())
                .Should()
                .Throw<GraphModelException>();
        }

        [Fact]
        public void E_of_concrete_type()
        {
            g
                .E<Knows>()
                .Should()
                .SerializeToGroovy<TVisitor>("g.E().hasLabel(_a)")
                .WithParameters("Knows");
        }

        [Fact]
        public void E_of_all_types1()
        {
            g
                .E<object>()
                .Should()
                .SerializeToGroovy<TVisitor>("g.E()")
                .WithoutParameters();
        }

        [Fact]
        public void E_of_all_types2()
        {
            g
                .E<IEdge>()
                .Should()
                .SerializeToGroovy<TVisitor>("g.E()")
                .WithoutParameters();
        }

        [Fact]
        public void E_of_type_outside_model()
        {
            g
                .Invoking(_ => _
                    .E<string>())
                .Should()
                .Throw<GraphModelException>();
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
                .AddV(new Language { Id = 1, IetfLanguageTag = "en" })
                .Should()
                .SerializeToGroovy<TVisitor>("g.addV(_a).property(T.id, _b).property(Cardinality.single, _c, _d)")
                .WithParameters("Language", 1, "IetfLanguageTag", "en");
        }

        [Fact]
        public void AddV_without_model()
        {
            g
                .WithModel(GraphModel.Empty)
                .AddV(new Language { Id = 1, IetfLanguageTag = "en" })
                .Should()
                .SerializeToGroovy<TVisitor>("g.addV(_a).property(Cardinality.single, _b, _c).property(Cardinality.single, _d, _e)")
                .WithParameters("Language", "IetfLanguageTag", "en", "Id", 1);
        }

        [Fact]
        public void AddV_list_cardinality_id()
        {
            g
                .WithModel(GraphModel
                    .FromExecutingAssembly<User, Edge>(x => x.PhoneNumbers, x => x.Id))
                .AddV(new User { Id = 1, PhoneNumbers = new[] { "123", "456" } })
                .Invoking(x => new GroovyGremlinQueryElementVisitor().Visit(x))
                .Should()
                .Throw<NotSupportedException>();
        }

        [Fact]
        public void AddV_without_id()
        {
            g
                .AddV(new Language { IetfLanguageTag = "en" })
                .Should()
                .SerializeToGroovy<TVisitor>("g.addV(_a).property(Cardinality.single, _b, _c)")
                .WithParameters("Language", "IetfLanguageTag", "en");
        }

        [Fact]
        public void AddV_with_nulls()
        {
            g
                .AddV(new Language { Id = 1 })
                .Should()
                .SerializeToGroovy<TVisitor>("g.addV(_a).property(T.id, _b)")
                .WithParameters("Language", 1);
        }

        [Fact]
        public void AddV_with_multi_property()
        {
            g
                .AddV(new User { Id = 1, PhoneNumbers = new[] { "+4912345", "+4923456" } })
                .Should()
                .SerializeToGroovy<TVisitor>("g.addV(_a).property(T.id, _b).property(Cardinality.single, _c, _d).property(Cardinality.single, _e, _f).property(Cardinality.single, _g, _h).property(Cardinality.list, _i, _j).property(Cardinality.list, _i, _k)")
                .WithParameters("User", 1, "Age", 0, "Gender", 0, "RegistrationDate", DateTimeOffset.MinValue, "PhoneNumbers", "+4912345", "+4923456");
        }

        [Fact]
        public void AddV_with_Meta_without_properties()
        {
            g
                .AddV(new Country { Id = 1, Name = "GER"})
                .Should()
                .SerializeToGroovy<TVisitor>("g.addV(_a).property(T.id, _b).property(Cardinality.single, _c, _d)")
                .WithParameters("Country", 1, "Name", "GER");
        }

        [Fact]
        public void AddV_with_Meta_with_properties()
        {
            g
                .AddV(new Country
                {
                    Id = 1,
                    Name = new VertexProperty<string>("GER")
                    {
                        Properties =
                        {
                            { "de", "Deutschland" },
                            { "en", "Germany" }
                        }
                    }
                })
                .Should()
                .SerializeToGroovy<TVisitor>("g.addV(_a).property(T.id, _b).property(Cardinality.single, _c, _d, _e, _f, _g, _h)")
                .WithParameters("Country", 1, "Name", "GER", "de", "Deutschland", "en", "Germany");
        }

        [Fact]
        public void AddV_with_MetaModel()
        {
            g
                .AddV(new User
                {
                    Id = 1,
                    Name = new VertexProperty<string, MetaModel>("Bob")
                    {
                        Properties = new MetaModel
                        {
                            MetaKey = "MetaValue"
                        }
                    }
                })
                .Should()
                .SerializeToGroovy<TVisitor>("g.addV(_a).property(T.id, _b).property(Cardinality.single, _c, _d).property(Cardinality.single, _e, _f).property(Cardinality.single, _g, _h).property(Cardinality.single, _i, _j, _k, _l)")
                .WithParameters("User", 1, "Age", 0, "Gender", 0, "RegistrationDate", DateTimeOffset.MinValue, "Name", "Bob", "MetaKey", "MetaValue");
        }

        [Fact]
        public void AddV_with_enum_property()
        {
            g
                .AddV(new User { Id = 1, Gender = Gender.Female })
                .Should()
                .SerializeToGroovy<TVisitor>("g.addV(_a).property(T.id, _b).property(Cardinality.single, _c, _d).property(Cardinality.single, _e, _f).property(Cardinality.single, _g, _h)")
                .WithParameters("User", 1, "Age", 0, "Gender" , 1, "RegistrationDate", DateTimeOffset.MinValue);
        }

        [Fact]
        public void Where_property_array_contains_element()
        {
            g
                .V<User>()
                .Where(t => t.PhoneNumbers.Contains("+4912345"))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, _c)")
                .WithParameters("User", "PhoneNumbers", "+4912345");
        }

        [Fact]
        public void Where_property_array_does_not_contain_element()
        {
            g
                .V<User>()
                .Where(t => !t.PhoneNumbers.Contains("+4912345"))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).not(__.has(_b, _c))")
                .WithParameters("User", "PhoneNumbers", "+4912345");
        }

        [Fact]
        public void Where_property_array_is_not_empty()
        {
            g
                .V<User>()
                .Where(t => t.PhoneNumbers.Any())
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b)")
                .WithParameters("User", "PhoneNumbers");
        }

        [Fact]
        public void Where_property_array_is_empty()
        {
            g
                .V<User>()
                .Where(t => !t.PhoneNumbers.Any())
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).not(__.has(_b))")
                .WithParameters("User", "PhoneNumbers");
        }

        [Fact]
        public void Where_property_array_intersects_aray()
        {
            g
                .V<User>()
                .Where(t => t.PhoneNumbers.Intersect(new[] { "+4912345", "+4923456" }).Any())
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, P.within(_c, _d))")
                .WithParameters("User", "PhoneNumbers", "+4912345", "+4923456");
        }

        [Fact]
        public void Where_property_array_does_not_intersect_array()
        {
            g
                .V<User>()
                .Where(t => !t.PhoneNumbers.Intersect(new[] { "+4912345", "+4923456" }).Any())
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).not(__.has(_b, P.within(_c, _d)))")
                .WithParameters("User", "PhoneNumbers", "+4912345", "+4923456");
        }

        [Fact]
        public void Where_property_array_does_not_intersect_empty_array()
        {
            g
                .V<User>()
                .Where(t => !t.PhoneNumbers.Intersect(new string[0]).Any())
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a)")
                .WithParameters("User");
        }

        [Fact]
        public void Where_array_intersects_property_aray()
        {
            g
                .V<User>()
                .Where(t => new[] { "+4912345", "+4923456" }.Intersect(t.PhoneNumbers).Any())
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, P.within(_c, _d))")
                .WithParameters("User", "PhoneNumbers", "+4912345", "+4923456");
        }

        [Fact]
        public void Where_array_does_not_intersect_property_array()
        {
            g
                .V<User>()
                .Where(t => !new[] { "+4912345", "+4923456" }.Intersect(t.PhoneNumbers).Any())
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).not(__.has(_b, P.within(_c, _d)))")
                .WithParameters("User", "PhoneNumbers", "+4912345", "+4923456");
        }

        [Fact]
        public void Where_empty_array_does_not_intersect_property_array()
        {
            g
                .V<User>()
                .Where(t => !new string[0].Intersect(t.PhoneNumbers).Any())
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a)")
                .WithParameters("User");
        }

        [Fact]
        public void Where_property_is_contained_in_array()
        {
            g
                .V<User>()
                .Where(t => new[] { 36, 37, 38 }.Contains(t.Age))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, P.within(_c, _d, _e))")
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
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, P.within(_c, _d, _e))")
                .WithParameters("User", "Age", 36, 37, 38);
        }

        [Fact]
        public void Where_property_is_not_contained_in_array()
        {
            g
                .V<User>()
                .Where(t => !new[] { 36, 37, 38 }.Contains(t.Age))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).not(__.has(_b, P.within(_c, _d, _e)))")
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
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).not(__.has(_b, P.within(_c, _d, _e)))")
                .WithParameters("User", "Age", 36, 37, 38);
        }

        [Fact]
        public void Where_property_is_not_contained_in_empty_enumerable()
        {
            var enumerable = Enumerable.Empty<int>();

            g
                .V<User>()
                .Where(t => !enumerable.Contains(t.Age))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a)")
                .WithParameters("User");
        }

        [Fact]
        public void Where_property_is_prefix_of_constant()
        {
            g
                .V<CountryCallingCode>()
                .Where(c => "+49123".StartsWith(c.Prefix))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, P.within(_c, _d, _e, _f, _g, _h, _i))")
                .WithParameters("CountryCallingCode", "Prefix", "", "+", "+4", "+49", "+491", "+4912", "+49123");
        }

        [Fact]
        public void Where_property_is_prefix_of_empty_string()
        {
            g
                .V<CountryCallingCode>()
                .Where(c => "".StartsWith(c.Prefix))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, P.within(_c))")
                .WithParameters("CountryCallingCode", "Prefix", "");
        }

        [Fact]
        public void Where_property_is_prefix_of_variable()
        {
            const string str = "+49123";

            g
                .V<CountryCallingCode>()
                .Where(c => str.StartsWith(c.Prefix))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, P.within(_c, _d, _e, _f, _g, _h, _i))")
                .WithParameters("CountryCallingCode", "Prefix", "", "+", "+4", "+49", "+491", "+4912", "+49123");
        }

        [Fact]
        public void Where_property_is_prefix_of_expression()
        {
            const string str = "+49123xxx";

            g
                .V<CountryCallingCode>()
                .Where(c => str.Substring(0, 6).StartsWith(c.Prefix))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, P.within(_c, _d, _e, _f, _g, _h, _i))")
                .WithParameters("CountryCallingCode", "Prefix", "", "+", "+4", "+49", "+491", "+4912", "+49123");
        }

        [Fact]
        public void Where_property_starts_with_constant()
        {
            g
                .V<User>()
                .Where(c => c.PhoneNumber.StartsWith("+49123"))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, P.between(_c, _d))")
                .WithParameters("User", "PhoneNumber", "+49123", "+49124");
        }

        [Fact]
        public void Where_property_starts_with_empty_string()
        {
            g
                .V<User>()
                .Where(c => c.PhoneNumber.StartsWith(""))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b)")
                .WithParameters("User", "PhoneNumber");
        }

        [Fact]
        public void Where_disjunction()
        {
            g
                .V<User>()
                .Where(t => t.Age == 36 || t.Age == 42)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).or(__.has(_b, _c), __.has(_b, _d))")
                .WithParameters("User", "Age", 36, 42);
        }

        [Fact]
        public void Where_disjunction_with_different_fields()
        {
            g
                .V<User>()
                .Where(t => t.Name == "Some name" || t.Age == 42)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).or(__.has(_b, _c), __.has(_d, _e))")
                .WithParameters("User", "Name", "Some name", "Age", 42);
        }

        [Fact]
        public void Where_conjunction()
        {
            g
                .V<User>()
                .Where(t => t.Age == 36 && t.Age == 42)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).and(__.has(_b, _c), __.has(_b, _d))")
                .WithParameters("User", "Age", 36, 42);
        }

        [Fact]
        public void Where_complex_logical_expression()
        {
            g
                .V<User>()
                .Where(t => t.Name == "Some name" && (t.Age == 42 || t.Age == 99))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).and(__.has(_b, _c), __.or(__.has(_d, _e), __.has(_d, _f)))")
                .WithParameters("User", "Name", "Some name", "Age", 42, 99);
        }

        [Fact]
        public void Where_complex_logical_expression_with_null()
        {
            g
                .V<User>()
                .Where(t => t.Name == null && (t.Age == 42 || t.Age == 99))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).and(__.hasNot(_b), __.or(__.has(_c, _d), __.has(_c, _e)))")
                .WithParameters("User", "Name", "Age", 42, 99);
        }

        [Fact]
        public void Where_has_conjunction_of_three()
        {
            g
                .V<User>()
                .Where(t => t.Age == 36 && t.Age == 42 && t.Age == 99)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).and(__.has(_b, _c), __.has(_b, _d), __.has(_b, _e))")
                .WithParameters("User", "Age", 36, 42, 99);
        }

        [Fact]
        public void Where_has_disjunction_of_three()
        {
            g
                .V<User>()
                .Where(t => t.Age == 36 || t.Age == 42 || t.Age == 99)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).or(__.has(_b, _c), __.has(_b, _d), __.has(_b, _e))")
                .WithParameters("User", "Age", 36, 42, 99);
        }

        [Fact]
        public void Where_conjunction_with_different_fields()
        {
            g
                .V<User>()
                .Where(t => t.Name == "Some name" && t.Age == 42)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).and(__.has(_b, _c), __.has(_d, _e))")
                .WithParameters("User", "Name", "Some name", "Age", 42);
        }

        [Fact]
        public void Where_property_equals_constant()
        {
            g
                .V<User>()
                .Where(t => t.Age == 36)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, _c)")
                .WithParameters("User", "Age", 36);
        }

        [Fact]
        public void Where_property_equals_expression()
        {
            const int i = 18;

            g
                .V<User>()
                .Where(t => t.Age == i + i)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, _c)")
                .WithParameters("User", "Age", 36);
        }

        [Fact]
        public void Where_property_equals_converted_expression()
        {
            g
                .V<User>()
                .Where(t => (object)t.Age == (object)36)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, _c)")
                .WithParameters("User", "Age", 36);
        }

        [Fact]
        public void Where_property_not_equals_constant()
        {
            g
                .V<User>()
                .Where(t => t.Age != 36)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, P.neq(_c))")
                .WithParameters("User", "Age", 36);
        }

        [Fact]
        public void Where_property_is_not_present()
        {
            g
                .V<User>()
                .Where(t => t.Name == null)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).hasNot(_b)")
                .WithParameters("User", "Name");
        }

        [Fact]
        public void Where_property_is_present()
        {
            g
                .V<User>()
                .Where(t => t.Name != null)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b)")
                .WithParameters("User", "Name");
        }

        [Fact]
        public void Where_property_is_lower_than_constant()
        {
            g
                .V<User>()
                .Where(t => t.Age < 36)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, P.lt(_c))")
                .WithParameters("User", "Age", 36);
        }

        [Fact]
        public void Where_property_is_lower_or_equal_than_constant()
        {
            g
                .V<User>()
                .Where(t => t.Age <= 36)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, P.lte(_c))")
                .WithParameters("User", "Age", 36);
        }

        [Fact]
        public void Where_bool_property_explicit_comparison1()
        {
            g
                .V<TimeFrame>()
                // ReSharper disable once RedundantBoolCompare
                .Where(t => t.Enabled == true)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, _c)")
                .WithParameters("TimeFrame", "Enabled", true);
        }

        [Fact]
        public void Where_bool_property_explicit_comparison2()
        {
            g
                .V<TimeFrame>()
                .Where(t => t.Enabled == false)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, _c)")
                .WithParameters("TimeFrame", "Enabled", false);
        }

        [Fact]
        public void Where_bool_property_implicit_comparison1()
        {
            g
                .V<TimeFrame>()
                .Where(t => t.Enabled)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, _c)")
                .WithParameters("TimeFrame", "Enabled", true);
        }

        [Fact]
        public void Where_bool_property_implicit_comparison2()
        {
            g
                .V<TimeFrame>()
                .Where(t => !t.Enabled)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).not(__.has(_b, _c))")
                .WithParameters("TimeFrame", "Enabled", true);
        }

        [Fact]
        public void Where_property_is_greater_than_constant()
        {
            g
                .V<User>()
                .Where(t => t.Age > 36)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, P.gt(_c))")
                .WithParameters("User", "Age", 36);
        }

        [Fact]
        public void Where_property_is_greater_or_equal_than_constant()
        {
            g
                .V<User>()
                .Where(t => t.Age >= 36)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, P.gte(_c))")
                .WithParameters("User", "Age", 36);
        }

        [Fact]
        public void Where_Id_equals_constant()
        {
            g
                .V<Language>()
                .Where(t => t.Id == (object)1)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(T.id, _b)")
                .WithParameters("Language", 1);
        }

        [Fact]
        public void Where_converted_Id_equals_constant()
        {
            g
                .V<Language>()
                .Where(t => (int)t.Id == 1)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(T.id, _b)")
                .WithParameters("Language", 1);
        }

        [Fact]
        public void Where_property_equals_local_string_constant()
        {
            const int local = 1;

            g
                .V<Language>()
                .Where(t => t.Id == (object)local)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(T.id, _b)")
                .WithParameters("Language", local);
        }

        [Fact]
        public void Where_property_equals_value_of_anonymous_object()
        {
            var local = new { Value = 1 };

            g
                .V<Language>()
                .Where(t => t.Id == (object)local.Value)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(T.id, _b)")
                .WithParameters("Language", 1);
        }

        [Fact]
        public void Where_source_expression_on_both_sides()
        {
            g
                .V<User>()
                .Invoking(query => query.Where(t => t.Name == t.PhoneNumber))
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
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).as(_b).V().hasLabel(_a).where(P.eq(_b))")
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
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).values(_b).as(_c).V().hasLabel(_a).has(_b, __.where(P.eq(_c)))")
                .WithParameters("Language", "IetfLanguageTag", "l1");
        }

        [Fact]
        public void Where_scalar_element_equals_constant()
        {
            g
                .V<User>()
                .Values(x => x.Age)
                .Where(_ => _ == 36)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).values(_b).is(_c)")
                .WithParameters("User", "Age", 36);
        }

        [Fact]
        public void Where_traversal()
        {
            g
                .V<User>()
                .Where(_ => _.Out<LivesIn>())
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).where(__.out(_b))")
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
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, __.inject(_c))")
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
                .Should()
                .SerializeToGroovy<TVisitor>("g.addV(_a).property(Cardinality.single, _b, _c).property(Cardinality.single, _d, _e).property(Cardinality.single, _f, _g).property(Cardinality.single, _h, _i).addE(_j).to(__.V().hasLabel(_k).has(_l, _m))")
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
                .Should()
                .SerializeToGroovy<TVisitor>("g.addV(_a).property(Cardinality.single, _b, _c).as(_d).addV(_e).property(Cardinality.single, _f, _g).addE(_h).property(Cardinality.single, _i, _j).to(_d)")
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
                .Should()
                .SerializeToGroovy<TVisitor>("g.addV(_a).property(Cardinality.single, _b, _c).property(Cardinality.single, _d, _e).property(Cardinality.single, _f, _g).property(Cardinality.single, _h, _i).addE(_j).from(__.V().hasLabel(_k).has(_l, _m))")
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
                .Should()
                .SerializeToGroovy<TVisitor>("g.addV(_a).property(Cardinality.single, _b, _c).as(_d).addV(_e).property(Cardinality.single, _f, _g).addE(_h).property(Cardinality.single, _i, _j).from(_d)")
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
                .Should()
                .SerializeToGroovy<TVisitor>("g.addV(_a).property(Cardinality.single, _b, _c).property(Cardinality.single, _d, _e).property(Cardinality.single, _f, _g).addE(_h).to(__.V(_i).hasLabel(_j)).inV()");
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
                .Should()
                .SerializeToGroovy<TVisitor>("g.addV(_a).property(Cardinality.single, _b, _c).property(Cardinality.single, _d, _e).property(Cardinality.single, _f, _g).addE(_h).to(__.V(_i).hasLabel(_j)).outV()");
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
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).and(__.inE(_b), __.outE(_c))")
                .WithParameters("User", "Knows", "LivesIn");
        }

        [Fact]
        public void And_nested()
        {
            g
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
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).and(__.outE(_b), __.inE(_c), __.outE(_c))")
                .WithParameters("User", "LivesIn", "Knows");
        }

        [Fact]
        public void Or()
        {
            g
                .V<User>()
                .Or(
                    __ => __
                        .InE<Knows>(),
                    __ => __
                        .OutE<LivesIn>())
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).or(__.inE(_b), __.outE(_c))")
                .WithParameters("User", "Knows", "LivesIn");
        }

        [Fact]
        public void Or_nested()
        {
            g
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
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).or(__.outE(_b), __.inE(_c), __.outE(_c))")
                .WithParameters("User", "LivesIn", "Knows");
        }

        [Fact]
        public void Drop()
        {
            g
                .V<User>()
                .Drop()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).drop()")
                .WithParameters("User");
        }

        [Fact]
        public void FilterWithLambda()
        {
            g
                .V<User>()
                .Filter("it.property('str').value().length() == 2")
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).filter({it.property('str').value().length() == 2})")
                .WithParameters("User");
        }

        [Fact]
        public void Out()
        {
            g
                .V<User>()
                .Out<Knows>()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).out(_b)")
                .WithParameters("User", "Knows");
        }

        [Fact]
        public void Out_of_all_types()
        {
            g
                .V()
                .Out<object>()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().out()")
                .WithoutParameters();
        }

        [Fact]
        public void Out_of_type_outside_model1()
        {
            g
                .V()
                .Invoking(_ => _.Out<string>())
                .Should()
                .Throw<GraphModelException>();
        }

        [Fact]
        public void Out_of_type_outside_model2()
        {
            g
                .V()
                .Invoking(_ => _.Out<IVertex>())
                .Should()
                .Throw<GraphModelException>();
        }
        
        [Fact]
        public void Out_does_not_include_abstract_edge()
        {
            g
                .V<User>()
                .Out<Edge>()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).out(_b, _c, _d, _e, _f)")
                .WithParameters("User", "IsDescribedIn", "Knows", "LivesIn", "Speaks", "WorksFor");
        }

        [Fact]
        public void OutE_of_all_types()
        {
            g
                .V()
                .OutE<object>()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().outE()")
                .WithoutParameters();
        }

        [Fact]
        public void In_of_all_types()
        {
            g
                .V()
                .In<object>()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().in()")
                .WithoutParameters();
        }

        [Fact]
        public void InE_of_all_types()
        {
            g
                .V()
                .InE<object>()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().inE()")
                .WithoutParameters();
        }

        [Fact]
        public void OrderBy_member()
        {
            g
                .V<User>()
                .OrderBy(x => x.Name)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).order().by(_b, Order.incr)")
                .WithParameters("User", "Name");
        }

        [Fact]
        public void OrderBy_traversal()
        {
            g
                .V<User>()
                .OrderBy(__ => __.Values(x => x.Name))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).order().by(__.values(_b), Order.incr)")
                .WithParameters("User", "Name");
        }

        [Fact]
        public void OrderBy_lambda()
        {
            g
                .V<User>()
                .OrderBy("it.property('str').value().length()")
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).order().by({it.property('str').value().length()})")
                .WithParameters("User");
        }

        [Fact]
        public void OrderByDescending_member()
        {
            g
                .V<User>()
                .OrderByDescending(x => x.Name)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).order().by(_b, Order.decr)")
                .WithParameters("User", "Name");
        }

        [Fact]
        public void OrderByDescending_traversal()
        {
            g
                .V<User>()
                .OrderByDescending(__ => __.Values(x => x.Name))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).order().by(__.values(_b), Order.decr)")
                .WithParameters("User", "Name");
        }
        
        [Fact]
        public void OrderBy_ThenBy_member()
        {
            g
                .V<User>()
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Age)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).order().by(_b, Order.incr).by(_c, Order.incr)")
                .WithParameters("User", "Name", "Age");
        }

        [Fact]
        public void OrderBy_ThenBy_traversal()
        {
            g
                .V<User>()
                .OrderBy(__ => __.Values(x => x.Name))
                .ThenBy(__ => __.Gender)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).order().by(__.values(_b), Order.incr).by(_c, Order.incr)")
                .WithParameters("User", "Name", "Gender");
        }

        [Fact]
        public void OrderBy_ThenBy_lambda()
        {
            g
                .V<User>()
                .OrderBy("it.property('str1').value().length()")
                .ThenBy("it.property('str2').value().length()")
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).order().by({it.property('str1').value().length()}).by({it.property('str2').value().length()})")
                .WithParameters("User");
        }

        [Fact]
        public void OrderBy_ThenByDescending_member()
        {
            g
                .V<User>()
                .OrderBy(x => x.Name)
                .ThenByDescending(x => x.Age)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).order().by(_b, Order.incr).by(_c, Order.decr)")
                .WithParameters("User", "Name", "Age");
        }

        [Fact]
        public void OrderBy_ThenByDescending_traversal()
        {
            g
                .V<User>()
                .OrderBy(__ => __.Values(x => x.Name))
                .ThenByDescending(__ => __.Gender)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).order().by(__.values(_b), Order.incr).by(_c, Order.decr)")
                .WithParameters("User", "Name", "Gender");
        }

        [Fact]
        public void SumLocal()
        {
            g
                .V<User>()
                .Values(x => x.Age)
                .SumLocal()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).values(_b).sum(Scope.local)")
                .WithParameters("User", "Age");
        }

        [Fact]
        public void SumGlobal()
        {
            g
                .V<User>()
                .Values(x => x.Age)
                .SumGlobal()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).values(_b).sum(Scope.global)")
                .WithParameters("User", "Age");
        }

        [Fact]
        public void Values_one_member()
        {
            g
                .V<User>()
                .Values(x => x.Age)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).values(_b)")
                .WithParameters("User", "Age");
        }

        [Fact]
        public void Values_VertexProperty_Member1()
        {
            typeof(User)
                .GetProperty(nameof(User.Name))
                .PropertyType
                .Should()
                .Be(typeof(VertexProperty<string, MetaModel>));

            g
                .V<User>()
                .Values(x => x.Name)
                .Should()
                .BeAssignableTo<IGremlinQuery<string>>();

            g
                .V<User>()
                .Values(x => x.Name)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).values(_b)")
                .WithParameters("User", "Name");
        }

        [Fact]
        public void Values_VertexProperty_Member2()
        {
            typeof(User)
                .GetProperty(nameof(User.SomeObscureProperty))
                .PropertyType
                .Should()
                .Be(typeof(VertexProperty<object>));

            g
                .V<User>()
                .Values(x => x.SomeObscureProperty)
                .Should()
                .BeAssignableTo<IGremlinQuery<object>>();

            g
                .V<User>()
                .Values(x => x.Name)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).values(_b)")
                .WithParameters("User", "Name");
        }

        [Fact]
        public void Values_EdgeProperty_Member()
        {
            typeof(LivesIn)
                .GetProperty(nameof(LivesIn.Since))
                .PropertyType
                .Should()
                .Be(typeof(Property<DateTimeOffset>));

            g
                .E<LivesIn>()
                .Values(x => x.Since)
                .Should()
                .BeAssignableTo<IGremlinQuery<DateTimeOffset>>();

            g
                .E<LivesIn>()
                .Values(x => x.Since)
                .Should()
                .SerializeToGroovy<TVisitor>("g.E().hasLabel(_a).values(_b)")
                .WithParameters("LivesIn", "Since");
        }

        [Fact]
        public void Values_two_members()
        {
            g
                .V<User>()
                .Values(x => x.Name, x => x.Id)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).union(__.values(_b), __.id())")
                .WithParameters("User", "Name");
        }

        [Fact]
        public void Values_three_members()
        {
            g
                .V<User>()
                .Values(x => x.Name, x => x.Gender, x => x.Id)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).union(__.values(_b, _c), __.id())")
                .WithParameters("User", "Name", "Gender");
        }

        [Fact]
        public void Values_id_member()
        {
            g
                .V<User>()
                .Values(x => x.Id)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).id()")
                .WithParameters("User");
        }

        [Fact]
        public void V_untyped()
        {
            g
                .V()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V()")
                .WithoutParameters();
        }

        [Fact]
        public void OfType_abstract()
        {
            g
                .V()
                .OfType<Authority>()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a, _b)")
                .WithParameters("Company", "User");
        }

        [Fact]
        public void OfType_redundant1()
        {
            g
                .V()
                .OfType<Company>()
                .OfType<Authority>()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a)")
                .WithParameters("Company");
        }

        [Fact]
        public void OfType_redundant2()
        {
            g
                .V()
                .OfType<Company>()
                .OfType<object>()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a)")
                .WithParameters("Company");
        }

        [Fact]
        public void Repeat_Out()
        {
            g
                .V<User>()
                .Repeat(__ => __
                    .Out<Knows>()
                    .OfType<User>())
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).repeat(__.out(_b).hasLabel(_a))")
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
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).union(__.out(_b), __.out(_c))")
                .WithParameters("User", "Knows", "LivesIn");
        }

        [Fact]
        public void Optional()
        {
            g
                .V()
                .Optional(
                    __ => __.Out<Knows>())
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().optional(__.out(_a))")
                .WithParameters("Knows");
        }

        [Fact]
        public void Not1()
        {
            g
                .V()
                .Not(__ => __.Out<Knows>())
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().not(__.out(_a))")
                .WithParameters("Knows");
        }

        [Fact]
        public void Not2()
        {
            g
                .V()
                .Not(__ => __.OfType<Language>())
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().not(__.hasLabel(_a))")
                .WithParameters("Language");
        }

        [Fact]
        public void Not3()
        {
            g
                .V()
                .Not(__ => __.OfType<Authority>())
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().not(__.hasLabel(_a, _b))")
                .WithParameters("Company", "User");
        }

        [Fact]
        public void As_explicit_label()
        {
            g
                .V<User>()
                .As(new StepLabel<User>())
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).as(_b)")
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
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).as(_b).select(_b)")
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
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).as(_b).as(_c).select(_b, _c)")
                .WithParameters("User", "Item1", "Item2");
        }

        [Fact]
        public void Map_Select_operation()
        {
            g
                .V<User>()
                .As((_, stepLabel1) => _
                    .As((__, stepLabel2) => __
                        .Map(___ => ___
                            .Select(stepLabel1, stepLabel2))))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).as(_b).as(_c).map(__.select(_b, _c))")
                .WithParameters("User", "Item1", "Item2");
        }

        [Fact]
        public void Nested_Select_operations()
        {
            g
                .V<User>()
                .As((_, stepLabel1) => _
                    .As((__, stepLabel2) => __
                        .Select(stepLabel1, stepLabel2)
                        .As((___, tuple) => ___
                            .Select(stepLabel1, tuple))))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).as(_b).as(_c).select(_b, _c).as(_c).select(_b, _c)")
                .WithParameters("User", "Item1", "Item2");
        }

        [Fact]
        public void Nested_contradicting_Select_operations_throw()
        {
            g
                .V<User>()
                .Invoking(x => x
                    .As((_, stepLabel1) => _
                        .As((__, stepLabel2) => __
                            .Select(stepLabel1, stepLabel2)
                            .As((___, tuple) => ___
                                .Select(tuple, stepLabel1)))))
                .Should()
                .Throw<InvalidOperationException>();
        }

        [Fact]
        public void Property_single()
        {
            g
                .V<User>()
                .Property(x => x.Age, 36)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).property(Cardinality.single, _b, _c)")
                .WithParameters("User", "Age", 36);
        }

        [Fact]
        public void Property_list()
        {
            g
                .V<User>("id")
                .Property(x => x.PhoneNumbers, "+4912345")
                .Should()
                .SerializeToGroovy<TVisitor>("g.V(_a).hasLabel(_b).property(Cardinality.list, _c, _d)")
                .WithParameters("id", "User", "PhoneNumbers", "+4912345");
        }

        [Fact]
        public void Property_null()
        {
            g
                .V<User>("id")
                .Property(x => x.PhoneNumber, null)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V(_a).hasLabel(_b).sideEffect(__.properties(_c).drop())")
                .WithParameters("id", "User", "PhoneNumber");
        }

        [Fact]
        public void Coalesce()
        {
            g
                .V()
                .Coalesce(
                     _ => _
                        .Identity())
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().coalesce(__.identity())")
                .WithoutParameters();
        }

        [Fact]
        public void V_Properties()
        {
            g
                .V()
                .Properties()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().properties()")
                .WithoutParameters();
        }

        [Fact]
        public void V_Properties_Values()
        {
            g
                .V()
                .Properties()
                .Values()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().properties().values()")
                .WithoutParameters();
        }

        [Fact]
        public void E_Properties()
        {
            g
                .E()
                .Properties()
                .Should()
                .SerializeToGroovy<TVisitor>("g.E().properties()")
                .WithoutParameters();
        }

        [Fact]
        public void E_Properties_member()
        {
            g
                .E<LivesIn>()
                .Properties(x => x.Since)
                .Should()
                .SerializeToGroovy<TVisitor>("g.E().hasLabel(_a).properties(_b)")
                .WithParameters("LivesIn", "Since");
        }

        [Fact]
        public void Properties_Values_untyped()
        {
            g
                .V()
                .Properties()
                .Values()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().properties().values()")
                .WithoutParameters();
        }

        [Fact]
        public void MetaProperties_Values()
        {
            g
                .V()
                .Properties()
                .Meta<MetaModel>()
                .Values()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().properties().values()")
                .WithoutParameters();
        }

        [Fact]
        public void MetaProperties_Values_Projected()
        {
            g
                .V()
                .Properties()
                .Meta<MetaModel>()
                .Values(x => x.MetaKey)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().properties().values(_a)")
                .WithParameters("MetaKey");
        }

        [Fact]
        public void Properties_of_member()
        {
            g
                .V<Country>()
                .Properties(x => x.Name)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).properties(_b)")
                .WithParameters("Country", "Name");
        }

        [Fact]
        public void Properties_Where()
        {
            g
                .V<Country>()
                .Properties(x => x.Languages)
                .Where(x => x.Value == "de")
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).properties(_b).hasValue(_c)")
                .WithParameters("Country", "Languages", "de");
        }

        [Fact]
        public void Properties_Where_reversed()
        {
            g
                .V<Country>()
                .Properties(x => x.Languages)
                .Where(x => "de" == x.Value)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).properties(_b).hasValue(_c)")
                .WithParameters("Country", "Languages", "de");
        }

        [Fact]
        public void Properties_ValueMap()
        {
            g
                .V()
                .Properties()
                .Meta<MetaModel>()
                .ValueMap()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().properties().valueMap()")
                .WithoutParameters();
        }

        [Fact]
        public void Set_Meta_Property()
        {
            g
                .V<Country>()
                .Properties(x => x.Name)
                .Property("metaKey", 1)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).properties(_b).property(_c, _d)")
                .WithParameters("Country", "Name", "metaKey", 1);
        }

        [Fact]
        public void Set_Meta_Property_to_null()
        {
            g
                .V<Country>()
                .Properties(x => x.Name)
                .Property("metaKey", null)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).properties(_b).sideEffect(__.properties(_c).drop())")
                .WithParameters("Country", "Name", "metaKey");
        }

        [Fact]
        public void Meta_Properties()
        {
            g
                .V<Country>()
                .Properties(x => x.Name)
                .Properties()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).properties(_b).properties()")
                .WithParameters("Country", "Name");
        }

        [Fact]
        public void Meta_Properties_with_key()
        {
            g
                .V<Country>()
                .Properties(x => x.Name)
                .Meta<MetaModel>()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).properties(_b)")
                .WithParameters("Country", "Name");
        }

        [Fact]
        public void Limit_underflow()
        {
            g
                .V()
                .Invoking(_ => _.Limit(-1))
                .Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public void Anonymous()
        {
            GremlinQuery.Anonymous(GraphModel.Empty)
                .Should()
                .SerializeToGroovy<TVisitor>("__.identity()")
                .WithoutParameters();
        }

        [Fact]
        public void Inject()
        {
            g
                .Inject(36, 37, 38)
                .Should()
                .SerializeToGroovy<TVisitor>("g.inject(_a, _b, _c)")
                .WithParameters(36, 37, 38);
        }

        [Fact]
        public void WithSubgraphStrategy()
        {
            g
                .WithStrategies(new SubgraphQueryStrategy(_ => _.OfType<User>(), _ => _.OfType<Knows>()))
                .V()
                .Should()
                .SerializeToGroovy<TVisitor>("g.withStrategies(SubgraphStrategy.build().vertices(__.hasLabel(_a)).edges(__.hasLabel(_b)).create()).V()")
                .WithParameters("User", "Knows");
        }

        [Fact]
        public void WithSubgraphStrategy_only_vertices()
        {
            g
                .WithStrategies(new SubgraphQueryStrategy(_ => _.OfType<User>(), _ => _))
                .V()
                .Should()
                .SerializeToGroovy<TVisitor>("g.withStrategies(SubgraphStrategy.build().vertices(__.hasLabel(_a)).create()).V()")
                .WithParameters("User");
        }

        [Fact]
        public void WithSubgraphStrategy_only_edges()
        {
            g
                .WithStrategies(new SubgraphQueryStrategy(_ => _, _ => _.OfType<Knows>()))
                .V()
                .Should()
                .SerializeToGroovy<TVisitor>("g.withStrategies(SubgraphStrategy.build().edges(__.hasLabel(_a)).create()).V()")
                .WithParameters("Knows");
        }

        [Fact]
        public void WithSubgraphStrategy_empty()
        {
            g
                .WithStrategies(new SubgraphQueryStrategy(_ => _, _ => _))
                .V()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V()")
                .WithoutParameters();
        }
    }
}
