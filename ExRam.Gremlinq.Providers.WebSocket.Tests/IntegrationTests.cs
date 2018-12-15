using System;
using System.Linq;
using System.Threading.Tasks;
using ExRam.Gremlinq.GraphElements;
using ExRam.Gremlinq.Tests;
using FluentAssertions;
using Xunit;
using static ExRam.Gremlinq.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.WebSocket.Tests
{
    public class IntegrationTests
    {
        private readonly IGremlinQuerySource _g;

        public IntegrationTests()
        {
            _g = g
                .WithRemote("localhost", GraphsonVersion.V3);
        }

        [Fact(Skip = "Integration Test")]
        public async Task AddV()
        {
            var data = await _g
                .AddV(new Language { Id = 1, IetfLanguageTag = "en" })
                .ToArray();

            data.Should().HaveCount(1);
            data[0].Id.Should().Be(1);
            data[0].IetfLanguageTag.Should().Be("en");
        }

        [Fact(Skip = "Integration Test")]
        public async Task AddV_without_id()
        {
            var data = await _g
                .AddV(new Language { IetfLanguageTag = "en" })
                .ToArray();

            data.Should().HaveCount(1);
            data[0].IetfLanguageTag.Should().Be("en");
        }

        [Fact(Skip = "Integration Test")]
        public async Task AddV_with_nulls()
        {
            var data = await _g
                .AddV(new Language { Id = 2 })
                .ToArray();

            data.Should().HaveCount(1);
            data[0].Id.Should().Be(2);
            data[0].IetfLanguageTag.Should().BeNull();
        }

        [Fact(Skip = "Integration Test")]
        public async Task AddV_with_multi_property()
        {
            var data = await _g
                .AddV(new User { Id = 3, PhoneNumbers = new[] { "+4912345", "+4923456" } })
                .ToArray();

            data.Should().HaveCount(1);
            data[0].Id.Should().Be(3);
            data[0].PhoneNumbers.Should().BeEquivalentTo("+4912345", "+4923456");
        }

        [Fact(Skip = "Integration Test")]
        public async Task AddV_with_Meta_without_properties()
        {
            var data = await _g
                .AddV(new Country { Id = 4, Name = "GER" })
                .ToArray();

            data.Should().HaveCount(1);
            data[0].Id.Should().Be(4);
            data[0].Name.Value.Should().Be("GER");
        }

        [Fact(Skip = "Integration Test")]
        public async Task AddV_with_Meta_with_properties()
        {
            var data = await _g
                .AddV(new Country
                {
                    Id = 5,
                    Name = new Meta<string>("GER")
                    {
                        Properties =
                        {
                            { "de", "Deutschland" },
                            { "en", "Germany" }
                        }
                    }
                })
                .ToArray();

            data.Should().HaveCount(1);
            data[0].Id.Should().Be(5);
        }
        
        [Fact(Skip = "Integration Test")]
        public async Task AddV_with_enum_property()
        {
            await _g
                .AddV(new User { Id = 1, Gender = Gender.Female })
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_property_array_contains_element()
        {
            await _g
                .V<User>()
                .Where(t => t.PhoneNumbers.Contains("+4912345"))
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_property_array_does_not_contain_element()
        {
            await _g
                .V<User>()
                .Where(t => !t.PhoneNumbers.Contains("+4912345"))
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_property_array_is_not_empty()
        {
            await _g
                .V<User>()
                .Where(t => t.PhoneNumbers.Any())
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_property_array_is_empty()
        {
            await _g
                .V<User>()
                .Where(t => !t.PhoneNumbers.Any())
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_property_array_intersects_aray()
        {
            await _g
                .V<User>()
                .Where(t => t.PhoneNumbers.Intersect(new[] { "+4912345", "+4923456" }).Any())
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_property_array_does_not_intersect_array()
        {
            await _g
                .V<User>()
                .Where(t => !t.PhoneNumbers.Intersect(new[] { "+4912345", "+4923456" }).Any())
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_property_array_intersects_empty_array()
        {
            await _g
                .V<User>()
                .Where(t => t.PhoneNumbers.Intersect(new string[0]).Any())
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_property_array_does_not_intersect_empty_array()
        {
            await _g
                .V<User>()
                .Where(t => !t.PhoneNumbers.Intersect(new string[0]).Any())
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_property_is_contained_in_array()
        {
            await _g
                .V<User>()
                .Where(t => new[] { 36, 37, 38 }.Contains(t.Age))
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_property_is_contained_in_enumerable()
        {
            var enumerable = new[] { "36", "37", "38" }
                .Select(int.Parse);

            await _g
                .V<User>()
                .Where(t => enumerable.Contains(t.Age))
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_property_is_contained_in_empty_enumerable()
        {
            var enumerable = Enumerable.Empty<int>();

            await _g
                .V<User>()
                .Where(t => enumerable.Contains(t.Age))
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_property_is_not_contained_in_array()
        {
            await _g
                .V<User>()
                .Where(t => !new[] { 36, 37, 38 }.Contains(t.Age))
                .ToArray();
        }
        
        [Fact(Skip = "Integration Test")]
        public async Task Where_property_is_not_contained_in_enumerable()
        {
            var enumerable = new[] { "36", "37", "38" }
                .Select(int.Parse);

            await _g
                .V<User>()
                .Where(t => !enumerable.Contains(t.Age))
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_property_is_not_contained_in_empty_enumerable()
        {
            var enumerable = Enumerable.Empty<int>();

            await _g
                .V<User>()
                .Where(t => !enumerable.Contains(t.Age))
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_property_is_prefix_of_constant()
        {
            await _g
                .V<CountryCallingCode>()
                .Where(c => "+49123".StartsWith(c.Prefix))
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_property_is_prefix_of_empty_string()
        {
            await _g
                .V<CountryCallingCode>()
                .Where(c => "".StartsWith(c.Prefix))
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_property_is_prefix_of_variable()
        {
            const string str = "+49123";

            await _g
                .V<CountryCallingCode>()
                .Where(c => str.StartsWith(c.Prefix))
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_property_is_prefix_of_expression()
        {
            const string str = "+49123xxx";

            await _g
                .V<CountryCallingCode>()
                .Where(c => str.Substring(0, 6).StartsWith(c.Prefix))
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_property_starts_with_constant()
        {
            await _g
                .V<User>()
                .Where(c => c.PhoneNumber.StartsWith("+49123"))
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_property_starts_with_empty_string()
        {
            await _g
                .V<User>()
                .Where(c => c.PhoneNumber.StartsWith(""))
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_disjunction()
        {
            await _g
                .V<User>()
                .Where(t => t.Age == 36 || t.Age == 42)
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_disjunction_with_different_fields()
        {
            await _g
                .V<User>()
                .Where(t => t.Name == "Some name" || t.Age == 42)
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_conjunction()
        {
            await _g
                .V<User>()
                .Where(t => t.Age == 36 && t.Age == 42)
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_complex_logical_expression()
        {
            await _g
                .V<User>()
                .Where(t => t.Name == "Some name" && (t.Age == 42 || t.Age == 99))
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_complex_logical_expression_with_null()
        {
            await _g
                .V<User>()
                .Where(t => t.Name == null && (t.Age == 42 || t.Age == 99))
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_has_conjunction_of_three()
        {
            await _g
                .V<User>()
                .Where(t => t.Age == 36 && t.Age == 42 && t.Age == 99)
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_has_disjunction_of_three()
        {
            await _g
                .V<User>()
                .Where(t => t.Age == 36 || t.Age == 42 || t.Age == 99)
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_conjunction_with_different_fields()
        {
            await _g
                .V<User>()
                .Where(t => t.Name == "Some name" && t.Age == 42)
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_property_equals_constant()
        {
            await _g
                .V<User>()
                .Where(t => t.Age == 36)
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_property_equals_expression()
        {
            const int i = 18;

            await _g
                .V<User>()
                .Where(t => t.Age == i + i)
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_property_equals_converted_expression()
        {
            await _g
                .V<User>()
                .Where(t => (object)t.Age == (object)36)
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_property_not_equals_constant()
        {
            await _g
                .V<User>()
                .Where(t => t.Age != 36)
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_property_is_not_present()
        {
            await _g
                .V<User>()
                .Where(t => t.Name == null)
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_property_is_present()
        {
            await _g
                .V<User>()
                .Where(t => t.Name != null)
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_property_is_lower_than_constant()
        {
            await _g
                .V<User>()
                .Where(t => t.Age < 36)
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_property_is_lower_or_equal_than_constant()
        {
            await _g
                .V<User>()
                .Where(t => t.Age <= 36)
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_bool_property_explicit_comparison1()
        {
            await _g
                .V<TimeFrame>()
                // ReSharper disable once RedundantBoolCompare
                .Where(t => t.Enabled == true)
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_bool_property_explicit_comparison2()
        {
            await _g
                .V<TimeFrame>()
                .Where(t => t.Enabled == false)
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_bool_property_implicit_comparison1()
        {
            await _g
                .V<TimeFrame>()
                .Where(t => t.Enabled)
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_bool_property_implicit_comparison2()
        {
            await _g
                .V<TimeFrame>()
                .Where(t => !t.Enabled)
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_property_is_greater_than_constant()
        {
            await _g
                .V<User>()
                .Where(t => t.Age > 36)
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_property_is_greater_or_equal_than_constant()
        {
            await _g
                .V<User>()
                .Where(t => t.Age >= 36)
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_property_equals_string_constant()
        {
            await _g
                .V<Language>()
                .Where(t => t.Id == (object)1)
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_property_equals_local_string_constant()
        {
            const int local = 1;

            await _g
                .V<Language>()
                .Where(t => t.Id == (object)local)
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_property_equals_value_of_anonymous_object()
        {
            var local = new { Value = 1 };

            await _g
                .V<Language>()
                .Where(t => t.Id == (object)local.Value)
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_current_element_equals_stepLabel()
        {
            var l = new StepLabel<Language>();

            await _g
                .V<Language>()
                .As(l)
                .V<Language>()
                .Where(l2 => l2 == l)
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_property_equals_stepLabel()
        {
            var l = new StepLabel<string>();

            await _g
                .V<Language>()
                .Values(x => x.IetfLanguageTag)
                .As(l)
                .V<Language>()
                .Where(l2 => l2.IetfLanguageTag == l)
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_scalar_element_equals_constant()
        {
            await _g
                .V<User>()
                .Values(x => x.Age)
                .Where(_ => _ == 36)
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_traversal()
        {
            await _g
                .V<User>()
                .Where(_ => _.Out<LivesIn>())
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Where_property_traversal()
        {
            await _g
                .V<User>()
                .Where(
                    x => x.Age,
                    _ => _
                        .Inject(36))
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task AddE_to_traversal()
        {
            var now = DateTimeOffset.UtcNow;

            await _g
                .AddV(new User
                {
                    Name = "Bob",
                    RegistrationDate = now
                })
                .AddE(new LivesIn())
                .To(__ => __
                    .V<Country>()
                    .Where(t => t.CountryCallingCode == "+49"))
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task AddE_to_StepLabel()
        {
            await _g
                .AddV(new Language { IetfLanguageTag = "en" })
                .As((_, l) => _
                    .AddV(new Country { CountryCallingCode = "+49" })
                    .AddE(new IsDescribedIn { Text = "Germany" })
                    .To(l))
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task AddE_from_traversal()
        {
            var now = DateTimeOffset.UtcNow;

            await _g
                .AddV(new User
                {
                    Name = "Bob",
                    RegistrationDate = now
                })
                .AddE(new LivesIn())
                .From(__ => __
                    .V<Country>()
                    .Where(t => t.CountryCallingCode == "+49"))
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task AddE_from_StepLabel()
        {
            await _g
                .AddV(new Country { CountryCallingCode = "+49" })
                .As((_, c) => _
                    .AddV(new Language { IetfLanguageTag = "en" })
                    .AddE(new IsDescribedIn { Text = "Germany" })
                    .From(c))
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task AddE_InV()
        {
            await _g
                .AddV<User>()
                .AddE<LivesIn>()
                .To(__ => __
                    .V<Country>("id"))
                .InV()
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task AddE_OutV()
        {
            await _g
                .AddV<User>()
                .AddE<LivesIn>()
                .To(__ => __
                    .V<Country>("id"))
                .OutV()
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task And()
        {
            await _g
                .V<User>()
                .And(
                    __ => __
                        .InE<Knows>(),
                    __ => __
                        .OutE<LivesIn>())
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task And_nested()
        {
            await _g
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
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Or()
        {
            await _g
                .V<User>()
                .Or(
                    __ => __
                        .InE<Knows>(),
                    __ => __
                        .OutE<LivesIn>())
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Or_nested()
        {
            await _g
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
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Drop()
        {
            await _g
                .V<User>()
                .Drop()
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task FilterWithLambda()
        {
            await _g
                .V<User>()
                .Filter("it.property('str').value().length() == 2")
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Out()
        {
            await _g
                .V<User>()
                .Out<Knows>()
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Out_does_not_include_abstract_edge()
        {
            await _g
                .V<User>()
                .Out<Edge>()
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task OrderBy_member()
        {
            await _g
                .V<User>()
                .OrderBy(x => x.Name)
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task OrderBy_traversal()
        {
            await _g
                .V<User>()
                .OrderBy(__ => __.Values(x => x.Name))
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task OrderBy_lambda()
        {
            await _g
                .V<User>()
                .OrderBy("it.property('str').value().length()")
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task OrderByDescending_member()
        {
            await _g
                .V<User>()
                .OrderByDescending(x => x.Name)
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task OrderByDescending_traversal()
        {
            await _g
                .V<User>()
                .OrderByDescending(__ => __.Values(x => x.Name))
                .ToArray();
        }
        
        [Fact(Skip = "Integration Test")]
        public async Task OrderBy_ThenBy_member()
        {
            await _g
                .V<User>()
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Age)
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task OrderBy_ThenBy_traversal()
        {
            await _g
                .V<User>()
                .OrderBy(__ => __.Values(x => x.Name))
                .ThenBy(__ => __.Gender)
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task OrderBy_ThenBy_lambda()
        {
            await _g
                .V<User>()
                .OrderBy("it.property('str1').value().length()")
                .ThenBy("it.property('str2').value().length()")
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task OrderBy_ThenByDescending_member()
        {
            await _g
                .V<User>()
                .OrderBy(x => x.Name)
                .ThenByDescending(x => x.Age)
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task OrderBy_ThenByDescending_traversal()
        {
            await _g
                .V<User>()
                .OrderBy(__ => __.Values(x => x.Name))
                .ThenByDescending(__ => __.Gender)
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task SumLocal()
        {
            await _g
                .V<User>()
                .Values(x => x.Age)
                .SumLocal()
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task SumGlobal()
        {
            await _g
                .V<User>()
                .Values(x => x.Age)
                .SumGlobal()
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Values_one_member()
        {
            await _g
                .V<User>()
                .Values(x => x.Name)
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Values_two_members()
        {
            await _g
                .V<User>()
                .Values<object>(x => x.Name, x => x.Id)
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Values_three_members()
        {
            await _g
                .V<User>()
                .Values(x => (object)x.Name, x => x.Gender, x => x.Id)
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Values_id_member()
        {
            await _g
                .V<User>()
                .Values(x => x.Id)
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task V_untyped()
        {
            await _g
                .V()
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task OfType_abstract()
        {
            await _g
                .V()
                .OfType<Authority>()
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Repeat_Out()
        {
            await _g
                .V<User>()
                .Repeat(__ => __
                    .Out<Knows>()
                    .OfType<User>())
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Union()
        {
            await _g
                .V<User>()
                .Union(
                    __ => __.Out<Knows>(),
                    __ => __.Out<LivesIn>())
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Optional()
        {
            await _g
                .V()
                .Optional(
                    __ => __.Out<Knows>())
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Not1()
        {
            await _g
                .V()
                .Not(__ => __.Out<Knows>())
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Not2()
        {
            await _g
                .V()
                .Not(__ => __.OfType<Language>())
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Not3()
        {
            await _g
                .V()
                .Not(__ => __.OfType<Authority>())
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task As_explicit_label()
        {
            await _g
                .V<User>()
                .As(new StepLabel<User>())
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Select()
        {
            var stepLabel = new StepLabel<User>();

            await _g
                .V<User>()
                .As(stepLabel)
                .Select(stepLabel)
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task As_inlined_nested_Select()
        {
            await _g
                .V<User>()
                .As((_, stepLabel1) => _
                    .As((__, stepLabel2) => __
                        .Select(stepLabel1, stepLabel2)))
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Property_single()
        {
            await _g
                .V<User>()
                .Property(x => x.Age, 36)
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Property_list()
        {
            await _g
                .V<User>("id")
                .Property(x => x.PhoneNumbers, "+4912345")
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Coalesce()
        {
            await _g
                .V()
                .Coalesce(
                     _ => _
                        .Identity())
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Properties()
        {
            await _g
                .V()
                .Properties()
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Properties_of_member()
        {
            await _g
                .V<Country>()
                .Properties(x => x.Name)
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Properties_Where()
        {
            await _g
                .V<Country>()
                .Properties(x => x.Languages)
                .Where(x => x.Value == "de")
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Properties_Where_reversed()
        {
            await _g
                .V<Country>()
                .Properties(x => x.Languages)
                .Where(x => "de" == x.Value)
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Meta_Properties()
        {
            await _g
                .V<Country>()
                .Properties(x => x.Name)
                .Properties()
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Meta_Properties_with_key()
        {
            await _g
                .V<Country>()
                .Properties(x => x.Name)
                .Properties("metaKey")
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task Inject()
        {
            await _g
                .Inject(36, 37, 38)
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task WithSubgraphStrategy()
        {
            await _g
                .WithStrategies(new SubgraphQueryStrategy(_ => _.OfType<User>(), _ => _))
                .V()
                .ToArray();
        }

        [Fact(Skip = "Integration Test")]
        public async Task WithSubgraphStrategy_empty()
        {
            await _g
                .WithStrategies(new SubgraphQueryStrategy(_ => _, _ => _))
                .V()
                .ToArray();
        }
    }
}
