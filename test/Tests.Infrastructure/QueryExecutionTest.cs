using System.Collections.Immutable;
using System.ComponentModel;
using ExRam.Gremlinq.Core.GraphElements;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Core.Steps;
using ExRam.Gremlinq.Tests.Entities;
using static ExRam.Gremlinq.Core.Transformation.ConverterFactory;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Support.NewtonsoftJson;
using Newtonsoft.Json.Linq;

namespace ExRam.Gremlinq.Tests.Infrastructure
{
    public abstract class QueryExecutionTest : GremlinqTestBase
    {
        private static readonly string Id = "id";

        protected readonly IGremlinQuerySource _g;

        protected QueryExecutionTest(GremlinqFixture fixture, GremlinQueryVerifier verifier) : base(verifier)
        {
            _g = fixture.G;
        }

        [Fact]
        public virtual Task AddE_from_StepLabel() => _g
            .AddV(new Country { CountryCallingCode = "+49" })
            .As((_, c) => _
                .AddV(new Language { IetfLanguageTag = "en" })
                .AddE<Speaks>()
                .From(c))
            .Verify();

        [Fact]
        public virtual async Task AddE_from_to()
        {
            var now = new DateTime(2020, 4, 7, 14, 43, 36, DateTimeKind.Utc);

            await _g
                .AddE(new WorksFor { From = now, To = now, Role = "Admin" })
                .From(__ => __.AddV<Person>())
                .To(__ => __.AddV<Company>())
                .Verify();
        }

        [Fact]
        public virtual async Task AddE_from_traversal()
        {
            var now = new DateTimeOffset(2020, 4, 7, 14, 43, 36, TimeSpan.Zero);

            await _g
                .AddV(new Person
                {
                    Name = "Bob",
                    RegistrationDate = now
                })
                .AddE(new LivesIn())
                .From(__ => __
                    .V<Country>()
                    .Where(t => t.CountryCallingCode == "+49"))
                .Verify();
        }

        [Fact]
        public virtual Task AddE_InV() => _g
            .AddV<Person>()
            .AddE<LivesIn>()
            .To(__ => __
                .AddV<Country>())
            .InV()
            .Verify();

        [Fact]
        public virtual Task AddE_OutV() => _g
            .AddV<Person>()
            .AddE<LivesIn>()
            .To(__ => __
                .AddV<Country>())
            .OutV()
            .Verify();

        [Fact]
        public virtual Task AddE_property() => _g
            .AddV<Person>()
            .AddE(new LivesIn
            {
                Since = new DateTimeOffset(2020, 4, 7, 14, 43, 36, TimeSpan.Zero)
            })
            .To(__ => __
                .AddV<Country>())
            .Verify();

        [Fact]
        public virtual async Task AddE_to_from()
        {
            var now = new DateTime(2020, 4, 7, 14, 43, 36, DateTimeKind.Utc);

            await _g
                .AddE(new WorksFor { From = now, To = now, Role = "Admin" })
                .To(__ => __.AddV<Company>())
                .From(__ => __.AddV<Person>())
                .Verify();
        }

        [Fact]
        public virtual Task AddE_to_StepLabel() => _g
            .AddV(new Language { IetfLanguageTag = "en" })
            .As((_, l) => _
                .AddV(new Country { CountryCallingCode = "+49" })
                .AddE<Speaks>()
                .To(l))
            .Verify();

        [Fact]
        public virtual async Task AddE_to_traversal()
        {
            var now = new DateTimeOffset(2020, 4, 7, 14, 43, 36, TimeSpan.Zero);

            await _g
                .AddV(new Person
                {
                    Name = "Bob",
                    RegistrationDate = now
                })
                .AddE(new LivesIn())
                .To(__ => __
                    .V<Country>()
                    .Where(t => t.CountryCallingCode == "+49"))
                .Verify();
        }

        [Fact]
        public virtual async Task AddE_With_Ignored()
        {
            var now = new DateTime(2020, 4, 7, 14, 43, 36, DateTimeKind.Utc);

            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureModel(model => model
                        .ConfigureEdges(edges => edges
                            .ConfigureElement<WorksFor>(conf => conf
                                .IgnoreAlways(p => p.From)
                                .IgnoreAlways(p => p.Role)))))
                .AddE(new WorksFor { From = now, To = now, Role = "Admin" })
                .From(__ => __.AddV<Person>())
                .To(__ => __.AddV<Company>())
                .Verify();
        }

        [Fact]
        public virtual Task AddV() => _g
            .AddV(new Language { IetfLanguageTag = "en" })
            .Verify();

        [Fact]
        public virtual Task AddV_ignores_label() => _g
            .AddV(new Language { Label = "Language" })
            .Verify();

        [Fact]
        public virtual Task AddV_TimeFrame() => _g
            .AddV(new TimeFrame
            {
                StartTime = TimeSpan.FromHours(8),
                Duration = TimeSpan.FromHours(2)
            })
            .Verify();

        [Fact]
        public virtual Task AddV_with_byte_array_property() => _g
            .AddV(new Person
            {
                Image = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 }
            })
            .Verify();

        [Fact]
        public virtual Task AddV_with_enum_property() => _g
            .AddV(new Person { Gender = Gender.Female })
            .Verify();

        [Fact]
        public virtual async Task AddV_With_Ignored()
        {
            var now = new DateTimeOffset(2020, 4, 7, 14, 43, 36, TimeSpan.Zero);
            var person = new Person { Age = 21, Gender = Gender.Male, Name = "Marko", RegistrationDate = now };

            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureModel(model => model
                        .ConfigureVertices(_ => _
                            .ConfigureElement<Person>(conf => conf
                                .IgnoreAlways(p => p.Age)
                                .IgnoreAlways(p => p.Gender)))))
                .AddV(person)
                .Verify();
        }

        [Fact]
        public virtual Task AddV_with_ignored_id_property() => _g
            .ConfigureEnvironment(env => env
                .ConfigureModel(model => model
                    .ConfigureVertices(_ => _
                        .ConfigureElement<Language>(conf => conf
                            .IgnoreOnAdd(p => p.Id)))))
            .AddV(new Language { Id = 300, IetfLanguageTag = "en" })
            .Verify();

        [Fact]
        public virtual Task AddV_with_ignored_property() => _g
            .ConfigureEnvironment(env => env
                .ConfigureModel(model => model
                    .ConfigureVertices(_ => _
                        .ConfigureElement<Language>(conf => conf
                            .IgnoreOnAdd(p => p.IetfLanguageTag)))))
            .AddV(new Language { IetfLanguageTag = "en" })
            .Verify();

        [Fact]
        public virtual Task AddV_with_Meta_with_properties() => _g
            .AddV(new Country
            {
                Name = new VertexProperty<string>("GER")
                {
                    Properties = new Dictionary<string, object>
                    {
                        { "de", "Deutschland" },
                        { "en", "Germany" }
                    }
                }
            })
            .Verify();

        [Fact]
        public virtual Task AddV_with_Meta_without_properties() => _g
            .AddV(new Country { Name = "GER" })
            .Verify();

        [Fact]
        public virtual Task AddV_with_MetaModel() => _g
            .AddV(new Company
            {
                Locations = new[]
                {
                    new VertexProperty<string, PropertyValidity>("Aachen")
                    {
                        Properties = new PropertyValidity
                        {
                            ValidFrom = new DateTimeOffset(2019, 01, 01, 01, 00, 00, TimeSpan.Zero)
                        }
                    }
                }
            })
            .Verify();

        [Fact]
        public virtual Task AddV_with_multi_property() => _g
            .AddV(new Company { PhoneNumbers = new[] { "+4912345", "+4923456" } })
            .Verify();

        [Fact]
        public virtual Task AddV_with_nulls() => _g
            .AddV(new Language())
            .Verify();

        [Fact]
        public virtual Task AddV_with_overridden_name() => _g
            .ConfigureEnvironment(env => env
                .ConfigureModel(model => model
                    .ConfigureVertices(propModel => propModel
                        .ConfigureElement<Language>(conf => conf
                            .ConfigureName(x => x.IetfLanguageTag, "lang")))))
            .AddV(new Language { IetfLanguageTag = "en" })
            .Verify();

        [Fact]
        public virtual Task AddV_without_id() => _g
            .AddV(new Language { IetfLanguageTag = "en" })
            .Verify();

        [Fact]
        public virtual Task Aggregate_Cap() => _g
            .V<Person>()
            .Aggregate((__, aggregated) => __
                .Cap(aggregated))
            .Verify();

        [Fact]
        public virtual Task Aggregate_Cap_Select() => _g
            .V<Person>()
            .Aggregate((__, aggregated) => __
                .Cap(aggregated)
                .Select(aggregated))
            .Verify();

        [Fact]
        public virtual Task Aggregate_Cap_Select_with_ints() => _g
            .V<Person>()
            .CountLocal()
            .Aggregate((__, aggregated) => __
                .Cap(aggregated)
                .Select(aggregated))
            .Verify();

        [Fact]
        public virtual Task Aggregate_Cap_unfold() => _g
            .V<Person>()
            .Aggregate((__, aggregated) => __
                .Cap(aggregated)
                .Unfold())
            .Verify();

        [Fact]
        public virtual Task Aggregate_Global() => _g
            .V<Person>()
            .Aggregate((__, aggregated) => __)
            .Verify();

        [Fact]
        public virtual Task Aggregate_Global_with_existing_step() => _g
            .V<Person>()
            .Aggregate(new())
            .Verify();

        [Fact]
        public virtual async Task Aggregate_in_multi_subQuery_Select()
        {
            var stepLabel1 = new StepLabel<IArrayGremlinQuery<Person[], Person, IVertexGremlinQuery<Person>>, Person[]>();
            var stepLabel2 = new StepLabel<IArrayGremlinQuery<Person[], Person, IVertexGremlinQuery<Person>>, Person[]>();

            await _g
                .V<Person>()
                .Coalesce(
                    __ => __
                        .Aggregate(stepLabel1),
                    __ => __
                        .Aggregate(stepLabel2))
                .Fold()
                .Select(stepLabel1, stepLabel2)
                .Verify();
        }

        [Fact]
        public virtual async Task Aggregate_in_subQuery_Select()
        {
            var stepLabel = new StepLabel<IArrayGremlinQuery<Person[], Person, IVertexGremlinQuery<Person>>, Person[]>();

            await _g
                .V<Person>()
                .Map(__ => __
                    .Aggregate(stepLabel))
                .Fold()
                .Select(stepLabel)
                .Verify();
        }

        [Fact]
        public virtual Task Aggregate_Local() => _g
            .V<Person>()
            .AggregateLocal((__, aggregated) => __)
            .Verify();

        [Fact]
        public virtual Task Aggregate_Local_with_existing_step() => _g
            .V<Person>()
            .AggregateLocal(new())
            .Verify();

        [Fact]
        public virtual async Task Aggregate_Select()
        {
            var stepLabel = new StepLabel<IArrayGremlinQuery<Person[], Person, IVertexGremlinQuery<Person>>, Person[]>();

            await _g
                .V<Person>()
                .Aggregate(stepLabel)
                .Fold()
                .Select(stepLabel)
                .Verify();
        }

        [Fact]
        public virtual Task And() => _g
            .V<Person>()
            .And(
                __ => __
                    .InE<WorksFor>(),
                __ => __
                    .OutE<LivesIn>())
            .Verify();

        [Fact]
        public virtual Task And_identity() => _g
            .V<Person>()
            .And(
                __ => __)
            .Verify();

        [Fact]
        public virtual Task And_nested() => _g
            .V<Person>()
            .And(
                __ => __
                    .OutE<LivesIn>(),
                __ => __
                    .And(
                        __ => __
                            .InE<WorksFor>(),
                        __ => __
                            .OutE<WorksFor>()))
            .Verify();

        [Fact]
        public virtual Task And_nested_or_optimization() => _g
            .V<Person>()
            .And(
                __ => __.Or(
                    __ => __),
                __ => __.Out())
            .Verify();

        [Fact]
        public virtual Task And_none() => _g
            .V<Person>()
            .And(
                __ => __.None())
            .Verify();

        [Fact]
        public virtual Task And_none_with_sideEffect() => _g
            .V<Person>()
            .And(
                __ => __
                    .Aggregate((__, l) => __
                        .None()),
                __ => __
                    .OutE())
            .Verify();

        [Fact]
        public virtual Task And_optimization() => _g
            .V<Person>()
            .And(
                __ => __,
                __ => __.Out())
            .Verify();

        [Fact]
        public virtual Task And_single() => _g
            .V<Person>()
            .And(
                __ => __.Out())
            .Verify();

        [Fact]
        public virtual Task And_Values_Where1() => _g
            .V<Person>()
            .And(__ => __
                .Values(x => x.Age)
                .Where(age => age > 36))
            .Verify();

        [Fact]
        public virtual Task And_Values_Where2() => _g
            .V<Person>()
            .And(
                __ => __
                    .Values(x => x.Age)
                    .Where(age => age > 36),
                __ => __
                    .Values(x => x.Age)
                    .Where(age => age < 72))
            .Verify();

        [Fact]
        public virtual async Task As_As_with_different_labels()
        {
            var label1 = "label1";
            var label2 = "label2";

            await _g
                .V<Person>()
                .As(label1)
                .As(label2)
                .Verify();
        }

        [Fact]
        public virtual Task As_followed_by_casted_Select() => _g
            .V<Person>()
            .As((_, stepLabel1) => _
                .Out()
                .Select(stepLabel1.Cast<object>()))
            .Verify();

        [Fact]
        public virtual Task As_followed_by_Select() => _g
            .V<Person>()
            .As((_, stepLabel1) => _
                .Select(stepLabel1))
            .Verify();

        [Fact]
        public virtual Task As_idempotency_is_detected() => _g
            .V<Person>()
            .As((_, stepLabel1) => _
                .As((__, stepLabel2) => __
                    .Select(stepLabel1, stepLabel2)))
            .Verify();

        [Fact]
        public virtual Task As_inlined_nested_Select() => _g
            .V<Person>()
            .As((_, stepLabel1) => _
                .Out()
                .OfType<Person>()
                .As((__, stepLabel2) => __
                    .Select(stepLabel1, stepLabel2)))
            .Verify();

        [Fact]
        public virtual Task As_inlined_nested_Select2() => _g
            .V<Person>()
            .As((_, stepLabel1) => _
                .Out()
                .OfType<Person>()
                .As((__, stepLabel2) => __
                    .Out()
                    .Select(stepLabel1, stepLabel2)))
            .Verify();

        [Fact]
        public virtual async Task As_with_same_label()
        {
            var label = "label";

            await _g
                .V<Person>()
                .As(label)
                .As(label)
                .Verify();
        }

        [Fact]
        public virtual async Task As_with_type_change()
        {
            IGremlinQueryBaseRec<Person, IVertexGremlinQuery<Person>> g = _g
                .V<Person>();

            await g
                .As((_, stepLabel1) => _
                    .Count()
                    .Select(stepLabel1))
                .Verify();
        }

        [Fact]
        public virtual Task Choose_one_case() => _g
            .V()
            .Where(__ => __.Properties())
            .Choose(_ => _
                .On(__ => __.Values())
                .Case(3, __ => __.Constant(1)))
            .Verify();

        [Fact]
        public virtual Task Choose_only_default_case() => _g
            .V()
            .Where(__ => __.Properties())
            .Choose(_ => _
                .On(__ => __.Values())
                .Default(__ => __.Constant(1)))
            .Verify();

        [Fact]
        public virtual Task Choose_Predicate1() => _g
            .V()
            .Id()
            .Choose(
                x => x == (object)42,
                _ => _.Constant(true),
                _ => _.Constant(false))
            .Verify();

        [Fact]
        public virtual Task Choose_Predicate2() => _g
            .V()
            .Id()
            .Choose(
                x => x == (object)42,
                _ => _.Constant(true))
            .Verify();

        [Fact]
        public virtual Task Choose_Predicate3() => _g
            .V()
            .Id()
            .Cast<int>()
            .Choose(
                x => x < 42,
                _ => _.Constant(true),
                _ => _.Constant(false))
            .Verify();

        [Fact]
        public virtual Task Choose_Predicate4() => _g
            .V()
            .Id()
            .Cast<int>()
            .Choose(
                x => 42 > x,
                _ => _.Constant(true),
                _ => _.Constant(false))
            .Verify();

        [Fact]
        public virtual Task Choose_Predicate5() => _g
            .V()
            .Id()
            .Cast<int>()
            .Choose(
                x => 0 < x && x < 42,
                _ => _.Constant(true),
                _ => _.Constant(false))
            .Verify();

        [Fact]
        public virtual Task Choose_Predicate6() => _g
            .V()
            .Id()
            .Cast<int>()
            .Choose(
                x => 0 < x && x < 42 || x != 37,
                _ => _.Constant(true),
                _ => _.Constant(false))
            .Verify();

        [Fact]
        public virtual Task Choose_Predicate7() => _g
            .V()
            .Id()
            .Cast<int>()
            .Choose(
                x => 0 < x || x < 42 && x != 37,
                _ => _.Constant(true),
                _ => _.Constant(false))
            .Verify();

        [Fact]
        public virtual Task Choose_Predicate8() => _g
            .V<Vertex>()
            .Choose(
                x => x.Id == (object)42,
                _ => _.Constant(true),
                _ => _.Constant(false))
            .Verify();

        [Fact]
        public virtual Task Choose_Traversal1() => _g
            .V()
            .Choose(
                _ => _.Values(),
                _ => _.Out(),
                _ => _.In())
            .Verify();

        [Fact]
        public virtual Task Choose_Traversal2() => _g
            .V()
            .Choose(
                _ => _.Values(),
                _ => _.Out())
            .Verify();

        [Fact]
        public virtual Task Choose_two_cases() => _g
            .V()
            .Where(__ => __.Properties())
            .Choose(_ => _
                .On(__ => __.Values())
                .Case(3, __ => __.Constant(1))
                .Case(4, __ => __.Constant(2)))
            .Verify();

        [Fact]
        public virtual Task Choose_two_cases_default() => _g
            .V()
            .Where(__ => __.Properties())
            .Choose(_ => _
                .On(__ => __.Values())
                .Case(3, __ => __.Constant(1))
                .Case(4, __ => __.Constant(2))
                .Default(__ => __.Constant(3)))
            .Verify();

        [Fact]
        public virtual Task Coalesce() => _g
            .V()
            .Coalesce(
                _ => _
                    .Out())
            .Verify();

        [Fact]
        public virtual Task Coalesce_identity() => _g
            .V()
            .Coalesce(
                _ => _
                    .Identity())
            .Verify();

        [Fact]
        public virtual Task Coalesce_with_2_not_matching_subQueries() => _g
            .V()
            .Coalesce(
                _ => _.OutE(),
                _ => _.In())
            .Verify();

        [Fact]
        public virtual Task Coalesce_with_2_subQueries() => _g
            .V()
            .Coalesce(
                _ => _.Out(),
                _ => _.In())
            .Verify();

        [Fact]
        public virtual Task Constant() => _g
            .V()
            .Constant(42)
            .Verify();

        [Fact]
        public virtual Task Constant_null() => _g
            .V()
            .Constant<object?>(null)
            .Verify();

        [Fact]
        public virtual Task Constant_string() => _g
            .V()
            .Constant("Hallo")
            .Verify();

        [Fact]
        public virtual Task Constant_empty_array() => _g
            .V()
            .Constant(Array.Empty<object>())
            .Verify();

        [Fact]
        public virtual Task Constant_empty_string_array() => _g
            .V()
            .Constant(Array.Empty<string>())
            .Verify();

        [Fact]
        public virtual Task Constant_empty_bool_array() => _g
            .V()
            .Constant(Array.Empty<bool>())
            .Verify();

        [Fact]
        public virtual Task Constant_single_string_array() => _g
            .V()
            .Constant(new []{ "Hello"})
            .Verify();

        [Fact]
        public virtual Task Constant_single_bool_array() => _g
            .V()
            .Constant(new []{ true })
            .Verify();

        [Fact]
        public virtual Task Count() => _g
            .V()
            .Count()
            .Verify();

        [Fact]
        public virtual Task CountGlobal() => _g
            .V()
            .Count()
            .Verify();

        [Fact]
        public virtual Task CountLocal() => _g
            .V()
            .CountLocal()
            .Verify();

        [Fact]
        public virtual Task CyclicPath() => _g
            .V()
            .Out()
            .Out()
            .CyclicPath()
            .Verify();

        [Fact]
        public virtual Task Dedup_Global() => _g
            .V()
            .Dedup()
            .Verify();

        [Fact]
        public virtual Task Dedup_Local() => _g
            .V()
            .Fold()
            .DedupLocal()
            .Verify();

        [Fact]
        public virtual Task Drop() => _g
            .V<Person>()
            .Drop()
            .Verify();

        [Fact]
        public virtual Task Drop_in_local() => _g
            .Inject(1)
            .Local(__ => __
                .V()
                .Drop())
            .Verify();

        [Fact]
        public virtual Task E_of_all_types1() => _g
            .E<object>()
            .Verify();

        [Fact]
        public virtual Task E_of_all_types2() => _g
            .E()
            .Verify();

        [Fact]
        public virtual Task E_of_concrete_type() => _g
            .E<WorksFor>()
            .Verify();

        [Fact]
        public virtual Task E_Properties() => _g
            .E()
            .Properties()
            .Verify();

        [Fact]
        public virtual Task E_baseType_Properties() => _g
            .E()
            .AsAdmin()
            .ChangeQueryType<IEdgeGremlinQueryBase>()
            .Properties()
            .Verify();

        [Fact]
        public virtual Task E_Properties_member() => _g
            .E<LivesIn>()
            .Properties(x => x.Since!)
            .Verify();

        [Fact]
        public virtual Task Emit_Repeat() => _g
            .V<Person>()
            .Cast<object>()
            .Loop(_ => _
                .Emit()
                .Repeat(__ => __
                    .InE()
                    .OutV()
                    .Cast<object>()))
            .Verify();

        [Fact]
        public virtual Task Emit_Repeat_Times() => _g
            .V<Person>()
            .Cast<object>()
            .Loop(_ => _
                .Emit()
                .Repeat(__ => __
                    .InE()
                    .OutV()
                    .Cast<object>())
                .Times(10))
            .Verify();

        [Fact]
        public virtual Task Emit_Repeat_Until() => _g
            .V<Person>()
            .Cast<object>()
            .Loop(_ => _
                .Emit()
                .Repeat(__ => __
                    .InE()
                    .OutV()
                    .Cast<object>())
                .Until(__ => __
                    .V<Company>()
                    .Cast<object>()))
            .Verify();

        [Fact]
        public virtual async Task Explicit_As()
        {
            var stepLabel = new StepLabel<Person>();

            await _g
                .V<Person>()
                .As(stepLabel)
                .Select(stepLabel)
                .Verify();
        }

        [Fact]
        public virtual Task Explicit_As_with_string() => _g
            .V<Person>()
            .As("stepLabel")
            .Select<Person>("stepLabel")
            .Verify();

        [Fact]
        public virtual Task Fail_with_message() => _g
            .V<object>()
            .Fail("There's been an error.")
            .Verify();

        [Fact]
        public virtual Task Fail_without_message() => _g
            .V<object>()
            .Fail()
            .Verify();

        [Fact]
        public virtual Task FlatMap() => _g
            .V<Person>()
            .FlatMap(__ => __.Out<WorksFor>())
            .Verify();

        [Fact]
        public virtual Task Fold() => _g
            .V()
            .Fold()
            .Verify();

        [Fact]
        public virtual Task Fold_Fold_Unfold_Unfold() => _g
            .V()
            .Fold()
            .Fold()
            .Unfold()
            .Unfold()
            .Verify();

        [Fact]
        public virtual Task Fold_SideEffect() => _g
            .V()
            .Fold()
            .SideEffect(x => x.Identity())
            .Unfold()
            .Verify();

        [Fact]
        public virtual Task Fold_Unfold() => _g
            .V()
            .Fold()
            .Unfold()
            .Verify();

        [Fact]
        public virtual Task Group() => _g
            .V<Person>()
            .Group()
            .Verify();

        [Fact]
        public virtual Task Group_with_key() => _g
            .V()
            .Group(_ => _
                .ByKey(_ => _.Label()))
            .Verify();

        [Fact]
        public virtual Task Group_with_key_and_value1() => _g
            .V<Person>()
            .Group(_ => _
                .ByKey(_ => _
                    .Label())
                .ByValue(_ => _
                    .Out<LivesIn>()
                    .OfType<Country>()))
            .Verify();

        [Fact]
        public virtual Task Group_with_key_and_value2() => _g
            .V()
            .Group(_ => _
                .ByKey(_ => _.Label())
                .ByValue(_ => _.Values()))
            .Verify();

        [Fact]
        public virtual Task Group_with_key_select() => _g
            .V()
            .Group(_ => _
                .ByKey(__ => __.Label()))
            .Select(x => x["Person"])
            .CountLocal()
            .Verify();

        [Fact]
        public virtual Task Group_with_key_identity() => _g
            .V()
            .Group(_ => _
                .ByKey(_ => _))
            .Verify();

        [Fact]
        public virtual Task Identity() => _g
            .V<Person>()
            .Identity()
            .Verify();

        [Fact]
        public virtual Task Identity_Identity() => _g
            .V<Person>()
            .Identity()
            .Identity()
            .Verify();

        [Fact]
        public virtual Task In() => _g
            .V<Person>()
            .In<WorksFor>()
            .Verify();

        [Fact]
        public virtual Task In_of_all_types_max() => _g
            .V()
            .In<object>()
            .Verify();

        [Fact]
        public virtual Task In_of_all_types_min() => _g
            .ConfigureEnvironment(env => env
                .ConfigureOptions(o => o
                    .SetValue(GremlinqOption.FilterLabelsVerbosity, FilterLabelsVerbosity.Minimum)))
            .V()
            .In<object>()
            .Verify();

        [Fact]
        public virtual Task InE_of_all_types_max() => _g
            .V()
            .InE<object>()
            .Verify();

        [Fact]
        public virtual Task InE_of_all_types_min() => _g
            .ConfigureEnvironment(env => env
                .ConfigureOptions(x => x
                    .SetValue(GremlinqOption.FilterLabelsVerbosity, FilterLabelsVerbosity.Minimum)))
            .V()
            .InE<object>()
            .Verify();

        [Fact]
        public virtual Task Inject() => _g
            .Inject(36, 37, 38)
            .Verify();

        [Fact]
        public virtual Task Inject_Coin() => _g
            .Inject(42)
            .Coin(1)
            .Verify();

        [Fact]
        public virtual Task Label() => _g
            .V()
            .Label()
            .Verify();

        [Fact]
        public virtual Task LimitGlobal() => _g
            .V()
            .Limit(1)
            .Verify();

        [Fact]
        public virtual Task LimitLocal() => _g
            .Inject(42, 43)
            .Fold()
            .LimitLocal(1)
            .Verify();

        [Fact]
        public virtual Task Local_identity() => _g
            .V()
            .Local(__ => __)
            .Verify();

        [Fact]
        public virtual Task Map() => _g
            .V<Person>()
            .Map(__ => __.Out<WorksFor>())
            .Verify();

        [Fact]
        public virtual Task Map_Identity() => _g
            .V<Person>()
            .Map(__ => __)
            .Verify();

        [Fact]
        public virtual Task Map_Select_operation() => _g
            .V<Person>()
            .As((_, stepLabel1) => _
                .As((__, stepLabel2) => __
                    .Map(__ => __
                        .Select(stepLabel1, stepLabel2))))
            .Verify();

        [Fact]
        public virtual Task MaxGlobal() => _g
            .V<Person>()
            .Values(x => x.Age)
            .Max()
            .Verify();

        [Fact]
        public virtual Task MaxLocal() => _g
            .V<Person>()
            .Values(x => x.Age)
            .Fold()
            .MaxLocal()
            .Verify();

        [Fact]
        public virtual Task MeanGlobal() => _g
            .V<Person>()
            .Values(x => x.Age)
            .Mean()
            .Verify();

        [Fact]
        public virtual Task MeanLocal() => _g
            .V<Person>()
            .Values(x => x.Age)
            .Fold()
            .MeanLocal()
            .Verify();

        [Fact]
        public virtual Task MinGlobal() => _g
            .V<Person>()
            .Values(x => x.Age)
            .Min()
            .Verify();

        [Fact]
        public virtual Task MinLocal() => _g
            .V<Person>()
            .Values(x => x.Age)
            .Fold()
            .MinLocal()
            .Verify();

        [Fact]
        public virtual Task Multi_Inject_V() => _g
            .Inject(36, 37, 38)
            .V<Person>()
            .Verify();

        [Fact]
        public Task Multi_step_serialization() => _g
            .ConfigureEnvironment(env => env
                .ConfigureSerializer(ser => ser
                    .Add(Create<EStep, Step[]>((step, env, _, recurse) => recurse
                        .TransformTo<Step[]>()
                        .From(
                            new Step[]
                            {
                                new VStep(ImmutableArray<object>.Empty),
                                new OutEStep(ImmutableArray<string>.Empty)
                            },
                            env)))))
            .E()
            .Verify();

        [Fact]
        public Task Multi_step_serialization_with_forgotten_serialize() => _g
            .ConfigureEnvironment(env => env
                .ConfigureSerializer(ser => ser
                    .Add(Create<EStep, Step[]>((step, env, _, recurse) =>
                        new Step[]
                        {
                            new VStep(ImmutableArray<object>.Empty),
                            new OutEStep(ImmutableArray<string>.Empty)
                        }))))
            .E()
            .Verify();

        [Fact]
        public virtual Task Nested_contradicting_Select_operations_does_not_throw() => _g
            .V<Person>()
            .As((__, stepLabel1) => __
                .As((__, stepLabel2) => __
                    .Select(stepLabel1, stepLabel2)
                    .As((__, tuple) => __
                        .Select(tuple, stepLabel1))))
            .Verify();

        [Fact]
        public virtual Task Nested_Select_operations() => _g
            .V<Person>()
            .As((__, stepLabel1) => __
                .As((__, stepLabel2) => __
                    .Select(stepLabel1, stepLabel2)
                    .As((__, tuple) => __
                        .Select(stepLabel1, tuple))))
            .Verify();

        [Fact]
        public virtual Task None() => _g
            .V<Person>()
            .None()
            .Verify();

        [Fact]
        public virtual Task None_None() => _g
            .V<Person>()
            .None()
            .None()
            .Verify();

        [Fact]
        public virtual Task Not1() => _g
            .V()
            .Not(__ => __.Out<WorksFor>())
            .Verify();

        [Fact]
        public virtual Task Not2() => _g
            .V()
            .Not(__ => __.OfType<Language>())
            .Verify();

        [Fact]
        public virtual Task Not3() => _g
            .V()
            .Not(__ => __.OfType<Authority>())
            .Verify();

        [Fact]
        public virtual Task OfType_abstract() => _g
            .V()
            .OfType<Authority>()
            .Verify();

        [Fact]
        public virtual Task OfType_redundant1() => _g
            .V()
            .OfType<Company>()
            .OfType<Authority>()
            .Verify();

        [Fact]
        public virtual Task OfType_redundant2() => _g
            .V()
            .OfType<Company>()
            .OfType<object>()
            .Verify();

        [Fact]
        public virtual Task OfType_redundant3() => _g
            .V()
            .OfType<Company>()
            .Cast<object>()
            .OfType<Authority>()
            .Verify();

        [Fact]
        public virtual Task OfType_redundant4() => _g
            .V()
            .OfType<Authority>()
            .OfType<Company>()
            .Verify();

        [Fact]
        public virtual Task Optional() => _g
            .V()
            .Optional(
                __ => __.Out<WorksFor>())
            .Verify();

        [Fact]
        public virtual Task Or() => _g
            .V<Person>()
            .Or(
                __ => __
                    .InE<WorksFor>(),
                __ => __
                    .OutE<LivesIn>())
            .Verify();

        [Fact]
        public virtual Task Or_identity() => _g
            .V<Person>()
            .Or(
                __ => __,
                __ => __
                    .OutE<LivesIn>())
            .Verify();

        [Fact]
        public virtual Task Or_nested() => _g
            .V<Person>()
            .Or(
                __ => __
                    .OutE<LivesIn>(),
                __ => __
                    .Or(
                        __ => __
                            .InE<WorksFor>(),
                        __ => __
                            .OutE<WorksFor>()))
            .Verify();

        [Fact]
        public virtual Task Or_nested_and_optimization() => _g
            .V<Person>()
            .Or(
                __ => __
                    .OutE<LivesIn>(),
                __ => __
                    .And(
                        __ => __,
                        __ => __))
            .Verify();

        [Fact]
        public virtual Task Or_none() => _g
            .V<Person>()
            .Or(
                __ => __
                    .OutE()
                    .None(),
                __ => __
                    .OutE())
            .Verify();

        [Fact]
        public virtual Task Or_none_with_predicate() => _g
            .V<Person>()
            .Or(
                __ => __
                    .None(),
                __ => __
                    .Where(x => x.Age > 36))
            .Verify();

        [Fact]
        public virtual Task Or_none_with_sideEffect() => _g
            .V<Person>()
            .Or(
                __ => __
                    .Aggregate((__, l) => __
                        .None()),
                __ => __
                    .OutE())
            .Verify();

        [Fact]
        public virtual Task Or_two_step_traversal() => _g
            .V<Person>()
            .Or(
                __ => __
                    .Out<LivesIn>(),
                __ => __
                    .OutE<LivesIn>()
                    .InV())
            .Verify();

        [Fact]
        public virtual Task Or_Values_Where1() => _g
            .V<Person>()
            .Or(__ => __
                .Values(x => x.Age)
                .Where(age => age > 36))
            .Verify();

        [Fact]
        public virtual Task Or_Values_Where2() => _g
            .V<Person>()
            .Or(
                __ => __
                    .Values(x => x.Age)
                    .Where(age => age > 36),
                __ => __
                    .Values(x => x.Age)
                    .Where(age => age < 72))
            .Verify();

        [Fact]
        public virtual Task Order_Fold_Unfold() => _g
            .V<Person>()
            .Where(x => x.Name != null)
            .Order(b => b
                .By(x => x.Name))
            .Fold()
            .Unfold()
            .Verify();

        [Fact]
        public virtual Task Order_scalars() => _g
            .V<Person>()
            .Local(__ => __.Count())
            .Order(b => b
                .By(__ => __))
            .Verify();

        [Fact]
        public virtual Task Order_scalars_local() => _g
            .V<Person>()
            .Local(__ => __.Count())
            .OrderLocal(b => b
                .By(__ => __))
            .Verify();

        [Fact]
        public virtual Task OrderBy_member() => _g
            .V<Person>()
            .Where(x => x.Name != null)
            .Order(b => b
                .By(x => x.Name))
            .Verify();

        [Fact]
        public virtual Task OrderBy_member_ThenBy_member() => _g
            .V<Person>()
            .Where(x => x.Name != null)
            .Where(x => x
                .Values(y => y.Age))
            .Order(b => b
                .By(x => x.Name)
                .By(x => x.Age))
            .Verify();

        [Fact]
        public virtual Task OrderBy_ThenByDescending_member() => _g
            .V<Person>()
            .Where(x => x.Name != null)
            .Where(x => x.Values(y => y.Age))
            .Order(b => b
                .By(x => x.Name)
                .ByDescending(x => x.Age))
            .Verify();

        [Fact]
        public virtual Task OrderBy_ThenByDescending_traversal() => _g
            .V<Person>()
            .Where(x => x.Name != null)
            .Where(x => x.Values(y => y.Gender))
            .Order(b => b
                .By(__ => __.Values(x => x.Name!))
                .ByDescending(__ => __.Gender))
            .Verify();

        [Fact]
        public virtual Task OrderBy_traversal() => _g
            .V<Person>()
            .Where(x => x.Name != null)
            .Order(b => b
                .By(__ => __.Values(x => x.Name!)))
            .Verify();

        [Fact]
        public virtual Task OrderBy_traversal_ThenBy() => _g
            .V<Person>()
            .Where(x => x.Name != null)
            .Where(x => x.Values(y => y.Gender))
            .Order(b => b
                .By(__ => __.Values(x => x.Name!))
                .By(__ => __.Gender))
            .Verify();

        [Fact]
        public virtual Task OrderBy_traversal_ThenBy_traversal() => _g
            .V<Person>()
            .Where(x => x.Name != null)
            .Where(x => x.Values(y => y.Gender))
            .Order(b => b
                .By(__ => __.Values(x => x.Name!))
                .By(__ => __.Values(x => x.Gender)))
            .Verify();

        [Fact]
        public virtual Task OrderByDescending_member() => _g
            .V<Person>()
            .Where(x => x.Name != null)
            .Order(b => b
                .ByDescending(x => x.Name))
            .Verify();

        [Fact]
        public virtual Task OrderByDescending_traversal() => _g
            .V<Person>()
            .Where(x => x.Name != null)
            .Order(b => b
                .ByDescending(__ => __.Values(x => x.Name!)))
            .Verify();

        [Fact]
        public virtual Task OrderLocal_by_member() => _g
            .V<Person>()
            .Where(x => x.Name != null)
            .OrderLocal(b => b
                .By(x => x.Name))
            .Verify();

        [Fact]
        public virtual Task Out() => _g
            .V<Person>()
            .Out<WorksFor>()
            .Verify();

        [Fact]
        public virtual Task Out_does_not_include_abstract_edge() => _g
            .V<Person>()
            .Out<Edge>()
            .Verify();

        [Fact]
        public virtual Task Out_of_all_types_max() => _g
            .V()
            .Out<object>()
            .Verify();

        [Fact]
        public virtual Task Out_of_all_types_min() => _g
            .ConfigureEnvironment(env => env
                .ConfigureOptions(o => o
                    .SetValue(GremlinqOption.FilterLabelsVerbosity, FilterLabelsVerbosity.Minimum)))
            .V()
            .Out<object>()
            .Verify();

        [Fact]
        public virtual Task OutE_of_all_types_max() => _g
            .V()
            .OutE<object>()
            .Verify();

        [Fact]
        public virtual Task OutE_of_all_types_min() => _g
            .ConfigureEnvironment(env => env
                .ConfigureOptions(o => o
                    .SetValue(GremlinqOption.FilterLabelsVerbosity, FilterLabelsVerbosity.Minimum)))
            .V()
            .OutE<object>()
            .Verify();

        [Fact]
        public virtual Task OutE_of_no_derived_types() => _g
            .V()
            .OutE<string>()
            .Verify();

        [Fact]
        public virtual Task Path() => _g
            .V()
            .Out()
            .Out()
            .Path()
            .Verify();

        [Fact]
        public virtual Task Project_to_property_with_builder() => _g
            .V<Person>()
            .Order(b => b
                .By(x => x.Id))
            .Limit(1)
            .Project(_ => _
                .ToDynamic()
                .By("item1!", __ => __.Constant("item1"))
                .By(x => x.Id!))
            .Verify();

        [Fact]
        public virtual Task Project_to_type() => _g
            .Inject(42)
            .Project(_ => _
                .To<ProjectRecord>()
                .By(x => x.In, __ => __.Constant("in_value"))
                .By(x => x.Out, __ => __.Constant("out_value"))
                .By(x => x.Count, __ => __.Constant("count_value"))
                .By(x => x.Properties, __ => __.Constant("properties_value")))
            .Verify();

        [Fact]
        public virtual Task Project_to_type_from_empty_traversal() => _g
            .Inject(42)
            .Limit(0)
            .Project(_ => _
                .To<ProjectRecordStruct>()
                .By(x => x.In, __ => __.Identity())
                .By(x => x.Out, __ => __.Identity())
                .By(x => x.Count, __ => __.Identity())
                .By(x => x.Properties, __ => __.Identity()))
            .Verify();

        [Fact]
        public virtual Task Project_to_type_with_identity() => _g
            .Inject(42)
            .Project(_ => _
                .To<ProjectRecordStruct>()
                .By(x => x.In, __ => __.Identity())
                .By(x => x.Out, __ => __.Identity())
                .By(x => x.Count, __ => __.Identity())
                .By(x => x.Properties, __ => __.Identity()))
            .Verify();

        [Fact]
        public virtual Task Project_to_type_with_select() => _g
            .Inject(42)
            .Project(_ => _
                .To<ProjectRecord>()
                .By(x => x.In, __ => __.Constant("in_value"))
                .By(x => x.Out, __ => __.Constant("out_value"))
                .By(x => x.Count, __ => __.Constant("count_value"))
                .By(x => x.Properties, __ => __.Constant("properties_value")))
            .Select(x => x.In, x => x.Out)
            .Verify();

        [Fact]
        public virtual Task Project_to_type_with_struct() => _g
            .Inject(42)
            .Project(_ => _
                .To<ProjectRecordStruct>()
                .By(x => x.In, __ => __.Constant("in_value"))
                .By(x => x.Out, __ => __.Constant("out_value"))
                .By(x => x.Count, __ => __.Constant("count_value"))
                .By(x => x.Properties, __ => __.Constant("properties_value")))
            .Verify();

        [Fact]
        public virtual Task Project_to_type_without_explicit_identity() => _g
            .Inject(42)
            .Project(_ => _
                .To<ProjectRecordStruct>()
                .By(x => x.In, __ => __)
                .By(x => x.Out, __ => __)
                .By(x => x.Count, __ => __)
                .By(x => x.Properties, __ => __))
            .Verify();

        [Fact]
        public virtual Task Project_with_builder_1() => _g
            .Inject(42)
            .Project(_ => _
                .ToDynamic()
                .By("item1!", __ => __.Constant("item1")))
            .Verify();

        [Fact]
        public virtual Task Project_with_builder_4() => _g
            .Inject(42)
            .Project(_ => _
                .ToDynamic()
                .By("item1!", __ => __.Constant("item1"))
                .By("item2!", __ => __.Constant("item2"))
                .By("item3!", __ => __.Constant("item3"))
                .By("item4!", __ => __.Constant("item4")))
            .Verify();

        [Fact]
        public virtual Task Project_with_cast() => _g
            .Inject(42)
            .Project(_ => _
                .ToDynamic()
                .By("in", __ => __.Constant("in_value"))
                .By("out", __ => __.Constant("out_value"))
                .By("count", __ => __.Constant("count_value"))
                .By("properties", __ => __.Constant("properties_value")))
            .Cast<ProjectRecord>()
            .Verify();

        [Fact]
        public virtual Task Project_with_identity() => _g
            .Inject(42)
            .Project(__ => __
                .ToTuple()
                .By(__ => __)
                .By(__ => __.Constant("item2")))
            .Verify();

        [Fact]
        public virtual Task Project_to_tuple_maximum_expressions() => _g
            .V<Country>()
            .Where(x => x.CountryCallingCode != null)
            .Project(__ => __
                .ToTuple()
                .By(x => x.CountryCallingCode)
                .By(x => x.CountryCallingCode)
                .By(x => x.CountryCallingCode)
                .By(x => x.CountryCallingCode)
                .By(x => x.CountryCallingCode)
                .By(x => x.CountryCallingCode)
                .By(x => x.CountryCallingCode)
                .By(x => x.CountryCallingCode)
                .By(x => x.CountryCallingCode)
                .By(x => x.CountryCallingCode)
                .By(x => x.CountryCallingCode)
                .By(x => x.CountryCallingCode)
                .By(x => x.CountryCallingCode)
                .By(x => x.CountryCallingCode)
                .By(x => x.CountryCallingCode)
                .By(x => x.CountryCallingCode))
            .Verify();

        [Fact]
        public virtual Task Project_with_local() => _g
            .Inject(42)
            .Project(__ => __
                .ToDynamic()
                .By("name", __ => __)
                .By(__ => __
                    .Local(__ => __
                        .Constant("item2"))))
            .Verify();

        [Fact]
        public virtual Task Project_with_named_identity() => _g
            .Inject(42)
            .Project(__ => __
                .ToDynamic()
                .By("name", __ => __)
                .By(__ => __.Constant("item2")))
            .Verify();

        [Fact]
        public virtual Task Project2() => _g
            .Inject(42)
            .Project(__ => __
                .ToTuple()
                .By(__ => __.Constant("item1"))
                .By(__ => __.Constant("item2")))
            .Verify();

        [Fact]  //TODO: Should this be named unguarded??
        public Task Project2_unguarded() => _g
            .ConfigureEnvironment(_ => _
                .ConfigureOptions(o => o.SetValue(GremlinqOption.EnableEmptyProjectionValueProtection, true)))
            .Inject(42)
            .Project(__ => __
                .ToTuple()
                .By(__ => __.Constant("item1"))
                .By(__ => __.None()))
            .Verify();

        [Fact]
        public virtual Task Project2_Where() => _g
            .Inject(42)
            .Project(__ => __
                .ToTuple()
                .By(__ => __.Constant("item1"))
                .By(__ => __.Fold()))
            .Where(x => x.Item2.Length == 3)
            .Verify();

#if (NET7_0_OR_GREATER) //TODO: What's up with them snapshots having a different order on < .NET 7 ?
        [Fact]
        public virtual Task Project2_Where_lower() => _g
            .Inject(42)
            .Project(__ => __
                .ToTuple()
                .By(__ => __.Constant("item1"))
                .By(__ => __.Fold()))
            .Where(x => x.Item2.Length < 3)
            .Cast<(string A, object[] B)>()
            .Verify();
#endif

        [Fact]
        public virtual Task Project2_with_Property() => _g
            .V<Person>()
            .Order(b => b
                .By(x => x.Id))
            .Limit(1)
            .Project(__ => __
                .ToTuple()
                .By(__ => __.Constant("item1"))
                .By(__ => __.Id!))
            .Verify();

        [Fact]
        public Task Project2_with_Property_unguarded() => _g
            .ConfigureEnvironment(_ => _
                .ConfigureOptions(o => o.SetValue(GremlinqOption.EnableEmptyProjectionValueProtection, true)))
            .V<Person>()
            .Order(b => b
                .By(x => x.Id))
            .Limit(1)
            .Project(__ => __
                .ToTuple()
                .By(__ => __.Constant("item1"))
                .By(__ => __.Id!))
            .Verify();

        [Fact]
        public virtual Task Project3() => _g
            .Inject(42)
            .Project(__ => __
                .ToTuple()
                .By(__ => __.Constant("item1"))
                .By(__ => __.Constant("item2"))
                .By(__ => __.Constant("item3")))
            .Verify();

        [Fact]
        public virtual Task Project3_Select1() => _g
            .Inject(42)
            .Project(__ => __
                .ToTuple()
                .By(__ => __.Constant("item1"))
                .By(__ => __.Constant("item2"))
                .By(__ => __.Constant("item3")))
            .Select(x => x.Item1)
            .Verify();

        [Fact]
        public Task Project3_Select1_unguarded() => _g
            .ConfigureEnvironment(_ => _
                .ConfigureOptions(o => o.SetValue(GremlinqOption.EnableEmptyProjectionValueProtection, true)))
            .Inject(42)
            .Project(__ => __
                .ToTuple()
                .By(__ => __.Constant("item1"))
                .By(__ => __.Constant("item2"))
                .By(__ => __.Constant("item3")))
            .Select(x => x.Item1)
            .Verify();

        [Fact]
        public virtual Task Project3_Select2() => _g
            .Inject(42)
            .Project(__ => __
                .ToTuple()
                .By(__ => __.Constant("item1"))
                .By(__ => __.Constant("item2"))
                .By(__ => __.Constant("item3")))
            .Select(
                x => x.Item1,
                x => x.Item2)
            .Verify();

        [Fact]
        public Task Project3_Select2_unguarded() => _g
            .ConfigureEnvironment(_ => _
                .ConfigureOptions(o => o.SetValue(GremlinqOption.EnableEmptyProjectionValueProtection, true)))
            .Inject(42)
            .Project(__ => __
                .ToTuple()
                .By(__ => __.Constant("item1"))
                .By(__ => __.Constant("item2"))
                .By(__ => __.None()))
            .Select(
                x => x.Item1,
                x => x.Item2)
            .Verify();

        [Fact]
        public Task Project3_unguarded() => _g
            .ConfigureEnvironment(_ => _
                .ConfigureOptions(o => o.SetValue(GremlinqOption.EnableEmptyProjectionValueProtection, true)))
            .Inject(42)
            .Project(__ => __
                .ToTuple()
                .By(__ => __.Constant("item1"))
                .By(__ => __.Constant("item2"))
                .By(__ => __.None()))
            .Verify();

        [Fact]
        public virtual Task Project3_with_Property() => _g
            .V<Person>()
            .Order(b => b
                .By(x => x.Label))
            .Limit(1)
            .Project(__ => __
                .ToTuple()
                .By(__ => __.Constant("item1"))
                .By(__ => __.Constant("item2"))
                .By(__ => __.Label!))
            .Verify();

        [Fact]
        public virtual Task Project3_with_Property_Select2() => _g
            .V<Person>()
            .Order(b => b
                .By(x => x.Id))
            .Limit(1)
            .Project(__ => __
                .ToTuple()
                .By(__ => __.Constant("item1"))
                .By(__ => __.Constant("item2"))
                .By(__ => __.Id!))
            .Select(
                x => x.Item1,
                x => x.Item3)
            .Verify();

        [Fact]
        public Task Project3_with_Property_Select2_unguarded() => _g
            .ConfigureEnvironment(_ => _
                .ConfigureOptions(o => o.SetValue(GremlinqOption.EnableEmptyProjectionValueProtection, true)))
            .V<Person>()
            .Order(b => b
                .By(x => x.Id))
            .Limit(1)
            .Project(__ => __
                .ToTuple()
                .By(__ => __.Constant("item1"))
                .By(__ => __.Constant("item2"))
                .By(__ => __.Id!))
            .Select(
                x => x.Item1,
                x => x.Item3)
            .Verify();

        [Fact]
        public virtual Task Project4() => _g
            .Inject(42)
            .Project(__ => __
                .ToTuple()
                .By(__ => __.Constant("item1"))
                .By(__ => __.Constant("item2"))
                .By(__ => __.Constant("item3"))
                .By(__ => __.Constant("item4")))
            .Verify();

        [Fact]
        public Task Project4_unguarded() => _g
            .ConfigureEnvironment(_ => _
                .ConfigureOptions(o => o.SetValue(GremlinqOption.EnableEmptyProjectionValueProtection, true)))
            .Inject(42)
            .Project(__ => __
                .ToTuple()
                .By(__ => __.Constant("item1"))
                .By(__ => __.Constant("item2"))
                .By(__ => __.Constant("item3"))
                .By(__ => __.None()))
            .Verify();

        [Fact]
        public virtual Task Project4_with_Property() => _g
            .V<Person>()
            .Order(b => b
                .By(x => x.Id))
            .Limit(1)
            .Project(__ => __
                .ToTuple()
                .By(__ => __.Constant("item1"))
                .By(__ => __.Constant("item2"))
                .By(x => x.Id!)
                .By(__ => __.Constant("item4")))
            .Verify();

        [Fact]
        public Task Project4_with_Property_unguarded() => _g
            .ConfigureEnvironment(_ => _
                .ConfigureOptions(o => o.SetValue(GremlinqOption.EnableEmptyProjectionValueProtection, true)))
            .V<Person>()
            .Order(b => b
                .By(x => x.Id))
            .Limit(1)
            .Project(__ => __
                .ToTuple()
                .By(__ => __.Constant("item1"))
                .By(__ => __.Constant("item2"))
                .By(x => x.Id!)
                .By(__ => __.Constant("item4")))
            .Verify();

        [Fact]
        public virtual Task Properties_Meta() => _g
            .V<Country>()
            .Properties(x => x.Name!)
            .Meta<PropertyValidity>()
            .Verify();

        [Fact]
        public virtual Task Properties_Meta_ValueMap() => _g
            .V()
            .Properties()
            .Meta<PropertyValidity>()
            .ValueMap()
            .Verify();

        [Fact]
        public virtual Task Properties_Meta_Values() => _g
            .V()
            .Properties()
            .Meta<PropertyValidity>()
            .Values()
            .Verify();

        [Fact]
        public virtual Task Properties_Meta_Values_Projected() => _g
            .V()
            .Properties()
            .Meta<PropertyValidity>()
            .Values(x => x.ValidFrom)
            .Verify();

        [Fact]
        public virtual Task Properties_Meta_Where1() => _g
            .V<Country>()
            .Properties(x => x.Name!)
            .Meta<PropertyValidity>()
            .Where(x => x.Properties!.ValidFrom >= new DateTimeOffset(2019, 01, 01, 01, 00, 00, TimeSpan.Zero))
            .Verify();

        [Fact]
        public virtual Task Properties_of_member() => _g
            .V<Country>()
            .Properties(x => x.Name!)
            .Verify();

        [Fact]
        public virtual Task Properties_of_three_members() => _g
            .V<Country>()
            .Properties(
                x => x.Name!,
                x => x.CountryCallingCode!,
                x => x.Languages!)
            .Verify();

        [Fact]
        public virtual Task Properties_of_two_members1() => _g
            .V<Country>()
            .Properties(
                x => x.Name!,
                x => x.CountryCallingCode!)
            .Verify();

        [Fact]
        public virtual Task Properties_of_two_members2() => _g
            .V<Country>()
            .Properties(
                x => x.Name!,
                x => x.Languages!)
            .Verify();

        [Fact]
        public virtual Task Properties_Properties_as_select() => _g
            .V<Country>()
            .Properties(x => x.Name!)
            .Properties()
            .As((__, s) => __
                .Select(s))
            .Verify();

        [Fact]
        public virtual Task Properties_Properties_key() => _g
            .V<Country>()
            .Properties(x => x.Name!)
            .Properties()
            .Key()
            .Verify();

        [Fact]
        public virtual Task Properties_Properties_Value() => _g
            .V<Company>()
            .Properties(x => x.Locations!)
            .Properties()
            .Value()
            .Verify();

        [Fact]
        public virtual Task Properties_Properties_Where_key() => _g
            .V<Company>()
            .Properties(x => x.Locations!)
            .Properties()
            .Where(x => x.Key == "someKey")
            .Verify();

        [Fact]
        public virtual Task Properties_Properties_Where_key_equals_stepLabel() => _g
            .Inject("hello")
            .As((__, stepLabel) => __
                .V<Company>()
                .Properties(x => x.Locations!)
                .Properties()
                .Where(x => x.Key == stepLabel.Value))
            .Verify();

        [Fact]
        public virtual Task Properties_Properties1() => _g
            .V<Country>()
            .Properties(x => x.Name!)
            .Properties()
            .Verify();

        [Fact]
        public virtual Task Properties_Properties2() => _g
            .V<Company>()
            .Properties(x => x.Locations!)
            .Properties()
            .Verify();

        [Fact]
        public virtual Task Properties_typed_no_parameters() => _g
            .V()
            .Properties<string>()
            .Verify();

        [Fact]
        public virtual Task Properties_ValueMap_typed() => _g
            .V()
            .Properties()
            .ValueMap<string>()
            .Verify();

        [Fact]
        public virtual Task Properties_ValueMap_untyped() => _g
            .V()
            .Properties()
            .ValueMap()
            .Verify();

        [Fact]
        public virtual Task Properties_Values_Id() => _g
            .V()
            .Properties()
            .Values(x => x.Id)
            .Verify();

        [Fact]
        public virtual Task Properties_Values_Id_Label() => _g
            .V()
            .Properties()
            .Values(
                x => x.Label,
                x => x.Id)
            .Verify();

        [Fact]
        public virtual Task Properties_Values_Label() => _g
            .V()
            .Properties()
            .Values(x => x.Label)
            .Verify();

        [Fact]
        public virtual Task Properties_Values_typed() => _g
                .V()
                .Properties()
                .Values<string>()
                .Verify();

        [Fact]
        public virtual Task Properties_Values_untyped() => _g
            .V()
            .Properties()
            .Values()
            .Verify();

        [Fact]
        public virtual Task Properties_Values2() => _g
            .V()
            .Properties()
            .Values<int>("MetaProperty")
            .Verify();

        [Fact]
        public virtual Task Properties_Where_Dictionary_key1() => _g
            .V<Person>()
            .Properties()
#pragma warning disable 252,253
            .Where(x => x.Properties!["MetaKey"] == "MetaValue")
#pragma warning restore 252,253
            .Verify();

        [Fact]
        public virtual Task Properties_Where_Dictionary_key2() => _g
            .V<Person>()
            .Properties()
            .Where(x => (int)x.Properties!["MetaKey"] < 100)
            .Verify();

        [Fact]
        public virtual Task Properties_Where_Id() => _g
            .V<Country>()
            .Properties(x => x.Languages!)
#pragma warning disable 252,253
            .Where(x => x.Id == "id")
#pragma warning restore 252,253
            .Verify();

        [Fact]
        public virtual Task Properties_Where_Id_equals_static_field() => _g
            .V<Country>()
            .Properties(x => x.Languages!)
#pragma warning disable 252,253
            .Where(x => x.Id == Id)
#pragma warning restore 252,253
            .Verify();

        [Fact]
        public virtual Task Properties_Where_Label_2() => _g
            .V<Country>()
            .Properties(x => x.Languages!)
            .Where(x => x.Label == "label")
            .Verify();

        [Fact]
        public virtual Task Properties_Where_Label_equals_StepLabel() => _g
            .Inject("label")
            .As((__, l) => __
                .V<Country>()
                .Properties(x => x.Languages!)
                .Where(x => x.Label == l.Value))
            .Verify();

        [Fact]
        public virtual Task Properties_Where_Meta_key() => _g
            .V<Company>()
            .Properties(x => x.Locations!)
            .Where(x => x.Properties!.ValidFrom == new DateTimeOffset(2019, 01, 01, 01, 00, 00, TimeSpan.Zero))
            .Verify();

        [Fact]
        public virtual Task Properties_Where_Meta_key_reversed() => _g
            .V<Company>()
            .Properties(x => x.Locations!)
            .Where(x => new DateTimeOffset(2019, 01, 01, 01, 00, 00, TimeSpan.Zero) == x.Properties!.ValidFrom)
            .Verify();

        [Fact]
        public virtual Task Properties_Where_neq_Label() => _g
            .V<Country>()
            .Properties(x => x.Languages!)
            .Where(x => x.Label != "label")
            .Verify();

        [Fact]
        public virtual Task Properties_Where_neq_Label_workaround() => _g
            .V<Country>()
            .Properties(x => x.Languages!)
            .Where(x => x
                .Label()
                .Where(l => l != "label"))
            .Verify();

        [Fact]
        public virtual Task Properties_Where_reversed() => _g
            .V<Country>()
            .Properties(x => x.Languages!)
            .Where(x => "de" == x.Value)
            .Verify();

        [Fact]
        public virtual Task Properties_Where1() => _g
            .V<Country>()
            .Properties(x => x.Languages!)
            .Where(x => x.Value == "de")
            .Verify();

        [Fact]
        public virtual Task Properties_Where2() => _g
            .V<Person>()
            .Properties()
            .Where(x => x.Label == "Age")
            .Where(x => (int)x.Value < 10)
            .Verify();

        [Fact]
        public virtual Task Properties1() => _g
            .V()
            .Properties()
            .Verify();

        [Fact]
        public virtual Task Properties2() => _g
            .E()
            .Properties()
            .Verify();

        [Fact]
        public virtual async Task Property_Guid_value()
        {
            var guid = Guid.Parse("{AEBACDFB-2C00-4808-A8B6-8D62217A8059}");

            await _g
                .V<Person>()
                .Property("GuidKey", guid)
                .Verify();
        }

        [Fact]
        public virtual Task Property_list() => _g
            .V<Company>()
            .Limit(1)
            .Property(x => x.PhoneNumbers!, "+4912345")
            .Verify();

        [Fact]
        public virtual Task Property_null() => _g
            .V<Company>()
            .Limit(1)
            .Property(x => x.PhoneNumbers!, (string)null!)
            .Verify();

        [Fact]
        public virtual Task Property_single() => _g
            .V<Person>()
            .Property(x => x.Age, 36)
            .Verify();

        [Fact]
        public virtual Task Property_single_from_stepLabel() => _g
            .Inject(36)
            .As((__, age) => __
                .V<Person>()
                .Property(x => x.Age, age))
            .Verify();

        [Fact]
        public virtual Task Property_single_traversal() => _g
            .V<Person>()
            .Property(
                x => x.Age,
                __ => __
                    .Values(x => x.Age))
            .Verify();

        [Fact]
        public virtual Task Property_single_with_dictionary_meta1() => _g
            .V<Country>()
            .Property(x => x.LocalizableDescription, new VertexProperty<object, IDictionary<string, string>>("")
            {
                Properties = new Dictionary<string, string>
                {
                    { "someKey", "value" }
                }
            })
            .Verify();

        [Fact]
        public virtual Task Property_single_with_meta() => _g
            .V<Person>()
            .Property(x => x.Age, new VertexProperty<int>(36)
            {
                Properties = new Dictionary<string, object>
                {
                    { "Meta", "value" }
                }
            })
            .Verify();

        [Fact]
        public virtual Task Property_stringKey() => _g
            .V<Person>()
            .Property("StringKey1", 36)
            .Verify();

        [Fact]
        public virtual Task Property_stringKey_traversal() => _g
            .V<Person>()
            .Property(
                "StringKey2",
                __ => __.Constant(36))
            .Verify();

        [Fact]
        public virtual Task RangeGlobal() => _g
            .V()
            .Range(1, 3)
            .Verify();

        [Fact]
        public virtual Task RangeLocal() => _g
            .Inject(42, 43)
            .Fold()
            .RangeLocal(1, 3)
            .Verify();

        [Fact]
        public virtual Task Repeat_Emit() => _g
            .V<Person>()
            .Cast<object>()
            .Loop(_ => _
                .Repeat(__ => __
                    .InE()
                    .OutV()
                    .Cast<object>())
                .Emit())
            .Verify();

        [Fact]
        public virtual Task Repeat_Emit_Times() => _g
            .V<Person>()
            .Cast<object>()
            .Loop(_ => _
                .Repeat(__ => __
                    .InE()
                    .OutV()
                    .Cast<object>())
                .Emit()
                .Times(10))
            .Verify();

        [Fact]
        public virtual Task Repeat_Emit_Until() => _g
            .V<Person>()
            .Cast<object>()
            .Loop(_ => _
                .Repeat(__ => __
                    .InE()
                    .OutV()
                    .Cast<object>())
                .Emit()
                .Until(__ => __
                    .V<Company>()
                    .Cast<object>()))
            .Verify();

        [Fact]
        public virtual Task Repeat_Out() => _g
            .V<Person>()
            .Loop(_ => _
                .Repeat(__ => __
                    .Out<WorksFor>()
                    .OfType<Person>()))
            .Verify();

        [Fact]
        public virtual Task Repeat_Times() => _g
            .V<Person>()
            .Cast<object>()
            .Loop(_ => _
                .Repeat(__ => __
                    .InE()
                    .OutV()
                    .Cast<object>())
                .Times(10))
            .Verify();

        [Fact]
        public virtual Task RepeatUntil() => _g
            .V<Person>()
            .Cast<object>()
            .Loop(_ => _
                .Repeat(__ => __
                    .InE()
                    .OutV()
                    .Cast<object>())
                .Until(__ => __
                    .V<Company>()
                    .Cast<object>()))
            .Verify();

        [Fact]
        public virtual async Task RepeatUntil_false()
        {
            var empty = Array.Empty<int>();

            await _g
                .V<Person>()
                .Loop(_ => _
                    .Repeat(__ => __
                        .InE()
                        .OutV<Person>())
                    .Until(__ => __
                        .Where(x => empty.Contains(x.Age))))
                .Verify();
        }

        [Fact]
        public virtual Task RepeatUntil_true() => _g
            .V<Person>()
            .Cast<object>()
            .Loop(_ => _
                .Repeat(__ => __
                    .InE()
                    .OutV()
                    .Cast<object>())
                .Until(__ => __))
            .Verify();

        [Fact]
        public virtual async Task ReplaceE()
        {
            var now = new DateTime(2020, 4, 7, 14, 43, 36, DateTimeKind.Utc);

            var worksFor = new WorksFor { Id = 0, From = now, To = now, Role = "Admin" };

            await _g
                .ReplaceE(worksFor)
                .Verify();
        }

        [Fact]
        public virtual async Task ReplaceE_With_Config()
        {
            var now = new DateTime(2020, 4, 7, 14, 43, 36, DateTimeKind.Utc);
            var worksFor = new WorksFor { Id = 0, From = now, To = now, Role = "Admin" };

            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureModel(model => model
                        .ConfigureEdges(_ => _
                            .ConfigureElement<WorksFor>(conf => conf
                                .IgnoreOnUpdate(p => p.Id)))))
                .ReplaceE(worksFor)
                .Verify();
        }

        [Fact]
        public virtual async Task ReplaceV()
        {
            var now = new DateTimeOffset(2020, 4, 7, 14, 43, 36, TimeSpan.Zero);
            var person = new Person { Id = 0, Age = 21, Gender = Gender.Male, Name = "Marko", RegistrationDate = now };

            await _g
                .ReplaceV(person)
                .Verify();
        }

        [Fact]
        public virtual async Task ReplaceV_With_Config()
        {
            var now = new DateTimeOffset(2020, 4, 7, 14, 43, 36, TimeSpan.Zero);
            var person = new Person { Id = 0, Age = 21, Gender = Gender.Male, Name = "Marko", RegistrationDate = now };

            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureModel(model => model
                        .ConfigureVertices(_ => _
                            .ConfigureElement<Person>(conf => conf
                                .IgnoreOnUpdate(p => p.RegistrationDate)))))
                .ReplaceV(person)
                .Verify();
        }

        [Fact]
        public virtual Task Set_Meta_Property_to_null() => _g
            .V<Country>()
            .Properties(x => x.Name!)
            .Property("metaKey", default(object))
            .Verify();

        [Fact]
        public virtual Task Set_Meta_Property1() => _g
            .V<Country>()
            .Properties(x => x.Name!)
            .Property("metaKey", 1)
            .Verify();

        [Fact]
        public virtual async Task Set_Meta_Property2()
        {
            var d = new DateTimeOffset(2020, 4, 7, 14, 43, 36, TimeSpan.Zero);

            await _g
                .V<Person>()
                .Properties(x => x.Name!)
                .Property(x => x.ValidFrom, d)
                .Verify();
        }

        [Fact]
        public virtual Task SimplePath() => _g
            .V()
            .Out()
            .Out()
            .SimplePath()
            .Verify();

        [Fact]
        public virtual Task Single_Inject_V() => _g
            .Inject(42)
            .V()
            .Verify();

        [Fact]
        public virtual Task WithSideEffect_Single_Inject_V() => _g
            .WithSideEffect("stepLabel", "sideEffect")
            .Inject(42)
            .V()
            .Verify();

        [Fact]
        public virtual Task WithSideEffect_Override() => _g
            .WithSideEffect("stepLabel", "sideEffect1")
            .WithSideEffect("stepLabel", "sideEffect2")
            .Inject(0)
            .Verify();

        [Fact]
        public virtual Task SkipGlobal() => _g
            .V()
            .Skip(1)
            .Verify();

        [Fact]
        public virtual Task SkipLocal() => _g
            .Inject(42, 43)
            .Fold()
            .SkipLocal(1)
            .Verify();

        [Fact]
        public virtual Task StepLabel_of_array_contains_element() => _g
            .Inject(1, 2, 3)
            .Fold()
            .As((_, ints) => _
                .V<Person>()
                .Where(person => ints.Value.Contains(person.Age)))
            .Verify();

        [Fact]
        public virtual Task StepLabel_of_array_contains_element_graphson() => _g
            .Inject(1, 2, 3)
            .Fold()
            .As((_, ints) => _
                .V<Person>()
                .Where(person => ints.Value.Contains(person.Age)))
            .Verify();

        [Fact]
        public virtual Task StepLabel_of_array_contains_vertex() => _g
            .V()
            .Fold()
            .As((_, v) => _
                .V<Person>()
                .Where(person => v.Value.Contains(person)))
            .Count()
            .Verify();

        [Fact]
        public virtual Task StepLabel_of_array_does_not_contain_vertex() => _g
            .V()
            .Fold()
            .As((_, v) => _
                .V<Person>()
                .Where(person => !v.Value.Contains(person)))
            .Count()
            .Verify();

        [Fact]
        public virtual Task StepLabel_of_object_array_contains_element() => _g
            .Inject(1, 2, 3)
            .Cast<object>()
            .Fold()
            .As((_, ints) => _
                .V<Person>()
                .Where(person => ints.Value.Contains(person.Age)))
            .Verify();

        [Fact]
        public Task StringKey() => _g
            .V<Person>("id")
            .Verify();

        [Fact]
        public virtual Task SumGlobal() => _g
            .V<Person>()
            .Values(x => x.Age)
            .Sum()
            .Verify();

        [Fact]
        public virtual Task SumLocal() => _g
            .V<Person>()
            .Values(x => x.Age)
            .Fold()
            .SumLocal()
            .Verify();

        [Fact]
        public virtual Task SumLocal_Where1() => _g
            .V<Person>()
            .Values(x => x.Age)
            .Fold()
            .SumLocal()
            .Where(x => x == 100)
            .Verify();

        [Fact]
        public virtual Task SumLocal_Where2() => _g
            .V<Person>()
            .Values(x => x.Age)
            .Fold()
            .SumLocal()
            .Where(x => x < 100)
            .Verify();

        [Fact]
        public virtual Task TailGlobal() => _g
            .V()
            .Tail(1)
            .Verify();

        [Fact]
        public virtual Task TailLocal() => _g
            .Inject(42, 43)
            .Fold()
            .TailLocal(1)
            .Verify();

        [Fact]
        public virtual Task Union() => _g
            .V<Person>()
            .Union(
                __ => __.Out<WorksFor>(),
                __ => __.Out<LivesIn>())
            .Verify();

        [Fact]
        public virtual Task Union_different_types() => _g
            .V<Person>()
            .Union(
                __ => __.Out<WorksFor>(),
                __ => __.OutE<LivesIn>())
            .Verify();

        [Fact]
        public virtual Task Union_different_types2() => _g
            .V<Person>()
            .Union(
                __ => __
                    .Out<WorksFor>()
                    .Lower(),
                __ => __
                    .OutE<LivesIn>()
                    .Lower()
                    .Lower()
                    .Cast<object>())
            .Verify();

        [Fact]
        public virtual Task Until_Emit_Repeat() => _g
            .V<Person>()
            .Cast<object>()
            .Loop(_ => _
                .Until(__ => __
                    .V<Company>()
                    .Cast<object>())
                .Emit()
                .Repeat(__ => __
                    .InE().OutV().Cast<object>()))
            .Verify();

        [Fact]
        public virtual Task Until_Repeat_Emit() => _g
            .V<Person>()
            .Cast<object>()
            .Loop(_ => _
                .Until(__ => __
                    .V<Company>()
                    .Cast<object>())
                .Repeat(__ => __
                    .InE()
                    .OutV()
                    .Cast<object>())
                .Emit())
            .Verify();

        [Fact]
        public virtual Task UntilRepeat() => _g
            .V<Person>()
            .Cast<object>()
            .Loop(_ => _
                .Until(__ => __
                    .V<Company>()
                    .Cast<object>())
                .Repeat(__ => __
                    .InE()
                    .OutV()
                    .Cast<object>()))
            .Verify();

        [Fact]
        public virtual async Task UntilRepeat_false()
        {
            var empty = Array.Empty<int>();

            await _g
                .V<Person>()
                .Loop(_ => _
                    .Until(__ => __
                        .Where(x => empty.Contains(x.Age)))
                    .Repeat(__ => __
                        .InE()
                        .OutV<Person>()))
                .Verify();
        }

        [Fact]
        public virtual async Task UntilRepeat_false_on_element()
        {
            var empty = Array.Empty<object>();

            await _g
                .V<Person>()
                .Loop(_ => _
                    .Until(__ => __
                        .Where(x => empty.Contains(x)))
                    .Repeat(__ => __
                        .InE()
                        .OutV<Person>()))
                .Verify();
        }

        [Fact]
        public virtual async Task Update_Vertex_And_Edge_No_Config()
        {
            var now = new DateTimeOffset(2020, 4, 7, 14, 43, 36, TimeSpan.Zero);
            var edgeNow = new DateTime(2020, 4, 7, 14, 43, 36, DateTimeKind.Utc);
            var person = new Person { Age = 21, Gender = Gender.Male, Name = "Marko", RegistrationDate = now };
            var worksFor = new WorksFor { From = edgeNow, To = edgeNow, Role = "Admin" };

            await _g
                .V<Person>()
                .Update(person)
                .OutE<WorksFor>()
                .Update(worksFor)
                .Verify();
        }

        [Fact]
        public virtual async Task Update_Vertex_And_Edge_With_Config()
        {
            var now = new DateTimeOffset(2020, 4, 7, 14, 43, 36, TimeSpan.Zero);
            var edgeNow = new DateTime(2020, 4, 7, 14, 43, 36, DateTimeKind.Utc);
            var person = new Person { Age = 21, Gender = Gender.Male, Name = "Marko", RegistrationDate = now };
            var worksFor = new WorksFor { From = edgeNow, To = edgeNow, Role = "Admin" };

            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureModel(model => model
                        .ConfigureVertices(_ => _
                            .ConfigureElement<Person>(conf => conf
                                .IgnoreOnUpdate(p => p.Age)
                                .IgnoreAlways(p => p.Name)))
                        .ConfigureEdges(_ => _
                            .ConfigureElement<WorksFor>(conf => conf
                                .IgnoreAlways(p => p.From)
                                .IgnoreOnUpdate(p => p.Role)))))
                .V<Person>()
                .Update(person)
                .OutE<WorksFor>()
                .Update(worksFor)
                .Verify();
        }

        [Fact]
        public virtual async Task UpdateE_With_Ignored()
        {
            var now = new DateTime(2020, 4, 7, 14, 43, 36, DateTimeKind.Utc);

            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureModel(model => model
                        .ConfigureEdges(_ => _
                            .ConfigureElement<WorksFor>(conf => conf
                                .IgnoreAlways(p => p.From)
                                .IgnoreAlways(p => p.Role)))))
                .E<WorksFor>()
                .Update(new WorksFor { From = now, To = now, Role = "Admin" })
                .Verify();
        }

        [Fact]
        public virtual async Task UpdateE_With_Mixed()
        {
            var now = new DateTime(2020, 4, 7, 14, 43, 36, DateTimeKind.Utc);

            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureModel(model => model
                        .ConfigureEdges(_ => _
                            .ConfigureElement<WorksFor>(conf => conf
                                .IgnoreAlways(p => p.From)
                                .IgnoreOnUpdate(p => p.Role)))))
                .E<WorksFor>()
                .Update(new WorksFor { From = now, To = now, Role = "Admin" })
                .Verify();
        }

        [Fact]
        public virtual async Task UpdateE_With_Readonly()
        {
            var now = new DateTime(2020, 4, 7, 14, 43, 36, DateTimeKind.Utc);

            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureModel(model => model
                        .ConfigureEdges(_ => _
                            .ConfigureElement<WorksFor>(conf => conf
                                .IgnoreOnUpdate(p => p.From)
                                .IgnoreOnUpdate(p => p.Role)))))
                .E<WorksFor>()
                .Update(new WorksFor { From = now, To = now, Role = "Admin" })
                .Verify();
        }

        [Fact]
        public virtual async Task UpdateV_No_Config()
        {
            var now = new DateTimeOffset(2020, 4, 7, 14, 43, 36, TimeSpan.Zero);

            await _g
                .V<Person>()
                .Update(new Person { Age = 21, Gender = Gender.Male, Name = "Marko", RegistrationDate = now, PhoneNumbers = new[] { new VertexProperty<string>("012345") } })
                .Verify();
        }

        [Fact]
        public virtual async Task UpdateV_With_Ignored()
        {
            var now = new DateTimeOffset(2020, 4, 7, 14, 43, 36, TimeSpan.Zero);
            var person = new Person { Age = 21, Gender = Gender.Male, Name = "Marko", RegistrationDate = now };

            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureModel(model => model
                        .ConfigureVertices(_ => _
                            .ConfigureElement<Person>(conf => conf
                                .IgnoreAlways(p => p.Age)
                                .IgnoreAlways(p => p.Gender)))))
                .V<Person>()
                .Update(person)
                .Verify();
        }

        [Fact]
        public virtual async Task UpdateV_With_Mixed()
        {
            var now = new DateTimeOffset(2020, 4, 7, 14, 43, 36, TimeSpan.Zero);
            var person = new Person { Age = 21, Gender = Gender.Male, Name = "Marko", RegistrationDate = now };

            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureModel(model => model
                        .ConfigureVertices(_ => _
                            .ConfigureElement<Person>(conf => conf
                                .IgnoreOnUpdate(p => p.Age)
                                .IgnoreAlways(p => p.Gender)))))
                .V<Person>()
                .Update(person)
                .Verify();
        }

        [Fact]
        public virtual async Task UpdateV_With_Readonly()
        {
            var now = new DateTimeOffset(2020, 4, 7, 14, 43, 36, TimeSpan.Zero);
            var person = new Person { Age = 21, Gender = Gender.Male, Name = "Marko", RegistrationDate = now };

            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureModel(model => model
                        .ConfigureVertices(_ => _
                            .ConfigureElement<Person>(conf => conf
                                .IgnoreOnUpdate(p => p.Age)
                                .IgnoreOnUpdate(p => p.Gender)))))
                .V<Person>()
                .Update(person)
                .Verify();
        }

        [Fact]
        public virtual Task V_Both() => _g
            .V()
            .Both<Edge>()
            .Verify();

        [Fact]
        public virtual Task V_BothE() => _g
            .V()
            .BothE<Edge>()
            .Verify();

        [Fact]
        public virtual Task V_BothV() => _g
            .E()
            .BothV()
            .Verify();

        [Fact]
        public virtual Task V_Both_typed() => _g
            .E()
            .BothV<Person>()
            .Verify();

        [Fact]
        public virtual Task V_InE_InV() => _g
            .V()
            .InE()
            .InV()
            .Verify();

        [Fact]
        public virtual Task V_InE_InV_typed() => _g
            .V()
            .InE()
            .InV<Person>()
            .Verify();

        [Fact]
        public virtual Task V_InE_OtherV() => _g
           .V()
           .InE()
           .OtherV()
           .Verify();

        [Fact]
        public virtual Task V_InE_OtherV_typed() => _g
           .V()
           .InE()
           .OtherV<Person>()
           .Verify();

        [Fact]
        public virtual Task V_IAuthority() => _g
            .ConfigureEnvironment(env => env
                .ConfigureModel(model => model
                    .ConfigureVertices(_ => _
                        .ConfigureElement<Authority>(__ => __
                            .ConfigureName(x => x.Name, "n")))))
            .V<IAuthority>()
            .Where(x => x.Name!.Value == "some name")
            .Verify();

        [Fact]
        public virtual Task V_Limit0_Fold_LimitLocal_0() => _g
            .V()
            .Limit(0)
            .Fold()
            .LimitLocal(0)
            .Verify();

        [Fact]
        public virtual Task V_Limit0_Fold_LimitLocal_1() => _g
            .V()
            .Limit(0)
            .Fold()
            .LimitLocal(1)
            .Verify();

        [Fact]
        public virtual Task V_Limit0_Fold_LimitLocal_2() => _g
            .V()
            .Limit(0)
            .Fold()
            .LimitLocal(2)
            .Verify();

        [Fact]
        public virtual Task V_Limit0_Fold_RangeLocal_0() => _g
            .V()
            .Limit(0)
            .Fold()
            .RangeLocal(0, 0)
            .Verify();

        [Fact]
        public virtual Task V_Limit0_Fold_RangeLocal_1() => _g
            .V()
            .Limit(0)
            .Fold()
            .RangeLocal(0, 1)
            .Verify();

        [Fact]
        public virtual Task V_Limit0_Fold_RangeLocal_2() => _g
            .V()
            .Limit(0)
            .Fold()
            .RangeLocal(0, 2)
            .Verify();

        [Fact]
        public virtual Task V_Limit0_Fold_TailLocal_0() => _g
            .V()
            .Limit(0)
            .Fold()
            .TailLocal(0)
            .Verify();

        [Fact]
        public virtual Task V_Limit0_Fold_TailLocal_1() => _g
            .V()
            .Limit(0)
            .Fold()
            .TailLocal(1)
            .Verify();

        [Fact]
        public virtual Task V_Limit0_Fold_TailLocal_2() => _g
            .V()
            .Limit(0)
            .Fold()
            .TailLocal(2)
            .Verify();

        [Fact]
        public virtual Task V_Limit0_V_LimitLocal_0() => _g
            .V()
            .Limit(0)
            .Fold()
            .LimitLocal(0)
            .Verify();

        [Fact]
        public virtual Task V_Limit0_V_LimitLocal_1() => _g
            .V()
            .Limit(0)
            .Fold()
            .LimitLocal(1)
            .Verify();

        [Fact]
        public virtual Task V_Limit0_V_LimitLocal_2() => _g
             .V()
             .Limit(0)
             .Fold()
             .LimitLocal(2)
             .Verify();

        [Fact]
        public virtual Task V_Limit0_V_RangeLocal_0() => _g
            .V()
            .Limit(0)
            .Fold()
            .RangeLocal(0, 0)
            .Verify();

        [Fact]
        public virtual Task V_Limit0_V_RangeLocal_1() => _g
            .V()
            .Limit(0)
            .Fold()
            .RangeLocal(0, 1)
            .Verify();

        [Fact]
        public virtual Task V_Limit0_V_RangeLocal_2() => _g
            .V()
            .Limit(0)
            .Fold()
            .RangeLocal(0, 2)
            .Verify();

        [Fact]
        public virtual Task V_Limit0_V_TailLocal_0() => _g
            .V()
            .Limit(0)
            .Fold()
            .TailLocal(0)
            .Verify();

        [Fact]
        public virtual Task V_Limit0_V_TailLocal_1() => _g
            .V()
            .Limit(0)
            .Fold()
            .TailLocal(1)
            .Verify();

        [Fact]
        public virtual Task V_Limit0_V_TailLocal_2() => _g
            .V()
            .Limit(0)
            .Fold()
            .TailLocal(2)
            .Verify();

        [Fact]
        public virtual Task V_Limit1_Fold_LimitLocal_0() => _g
            .V()
            .Limit(1)
            .Fold()
            .LimitLocal(0)
            .Verify();

        [Fact]
        public virtual Task V_Limit1_Fold_LimitLocal_1() => _g
            .V()
            .Limit(1)
            .Fold()
            .LimitLocal(1)
            .Verify();

        [Fact]
        public virtual Task V_Limit1_Fold_LimitLocal_2() => _g
            .V()
            .Limit(1)
            .Fold()
            .LimitLocal(2)
            .Verify();

        [Fact]
        public virtual Task V_Limit1_Fold_RangeLocal_0() => _g
            .V()
            .Limit(1)
            .Fold()
            .RangeLocal(0, 0)
            .Verify();

        [Fact]
        public virtual Task V_Limit1_Fold_RangeLocal_1() => _g
            .V()
            .Limit(1)
            .Fold()
            .RangeLocal(0, 1)
            .Verify();

        [Fact]
        public virtual Task V_Limit1_Fold_RangeLocal_2() => _g
            .V()
            .Limit(1)
            .Fold()
            .RangeLocal(0, 2)
            .Verify();

        [Fact]
        public virtual Task V_Limit1_Fold_TailLocal_0() => _g
            .V()
            .Limit(1)
            .Fold()
            .TailLocal(0)
            .Verify();

        [Fact]
        public virtual Task V_Limit1_Fold_TailLocal_1() => _g
            .V()
            .Limit(1)
            .Fold()
            .TailLocal(1)
            .Verify();

        [Fact]
        public virtual Task V_Limit1_Fold_TailLocal_2() => _g
            .V()
            .Limit(1)
            .Fold()
            .TailLocal(2)
            .Verify();

        [Fact]
        public virtual Task V_Limit1_V_LimitLocal_0() => _g
            .V()
            .Limit(1)
            .Fold()
            .LimitLocal(0)
            .Verify();

        [Fact]
        public virtual Task V_Limit1_V_LimitLocal_1() => _g
            .V()
            .Limit(1)
            .Fold()
            .LimitLocal(1)
            .Verify();

        [Fact]
        public virtual Task V_Limit1_V_LimitLocal_2() => _g
            .V()
            .Limit(1)
            .Fold()
            .LimitLocal(2)
            .Verify();

        [Fact]
        public virtual Task V_Limit1_V_RangeLocal_0() => _g
            .V()
            .Limit(1)
            .Fold()
            .RangeLocal(0, 0)
            .Verify();

        [Fact]
        public virtual Task V_Limit1_V_RangeLocal_1() => _g
            .V()
            .Limit(1)
            .Fold()
            .RangeLocal(0, 1)
            .Verify();

        [Fact]
        public virtual Task V_Limit1_V_RangeLocal_2() => _g
            .V()
            .Limit(1)
            .Fold()
            .RangeLocal(0, 2)
            .Verify();

        [Fact]
        public virtual Task V_Limit1_V_TailLocal_0() => _g
            .V()
            .Limit(1)
            .Fold()
            .TailLocal(0)
            .Verify();

        [Fact]
        public virtual Task V_Limit1_V_TailLocal_1() => _g
            .V()
            .Limit(1)
            .Fold()
            .TailLocal(1)
            .Verify();

        [Fact]
        public virtual Task V_Limit1_V_TailLocal_2() => _g
            .V()
            .Limit(1)
            .Fold()
            .TailLocal(2)
            .Verify();

        [Fact]
        public virtual Task V_Limit2_Fold_LimitLocal_0() => _g
            .V()
            .Limit(2)
            .Fold()
            .LimitLocal(0)
            .Verify();

        [Fact]
        public virtual Task V_Limit2_Fold_LimitLocal_1() => _g
            .V()
            .Limit(2)
            .Fold()
            .LimitLocal(1)
            .Verify();

        [Fact]
        public virtual Task V_Limit2_Fold_LimitLocal_2() => _g
            .V()
            .Limit(2)
            .Fold()
            .LimitLocal(2)
            .Verify();

        [Fact]
        public virtual Task V_Limit2_Fold_RangeLocal_0() => _g
            .V()
            .Limit(2)
            .Fold()
            .RangeLocal(0, 0)
            .Verify();

        [Fact]
        public virtual Task V_Limit2_Fold_RangeLocal_1() => _g
            .V()
            .Limit(2)
            .Fold()
            .RangeLocal(0, 1)
            .Verify();

        [Fact]
        public virtual Task V_Limit2_Fold_RangeLocal_2() => _g
            .V()
            .Limit(2)
            .Fold()
            .RangeLocal(0, 2)
            .Verify();

        [Fact]
        public virtual Task V_Limit2_Fold_TailLocal_0() => _g
            .V()
            .Limit(2)
            .Fold()
            .TailLocal(0)
            .Verify();

        [Fact]
        public virtual Task V_Limit2_Fold_TailLocal_1() => _g
            .V()
            .Limit(2)
            .Fold()
            .TailLocal(1)
            .Verify();

        [Fact]
        public virtual Task V_Limit2_Fold_TailLocal_2() => _g
            .V()
            .Limit(2)
            .Fold()
            .TailLocal(2)
            .Verify();

        [Fact]
        public virtual Task V_Limit2_V_LimitLocal_0() => _g
            .V()
            .Limit(2)
            .Fold()
            .LimitLocal(0)
            .Verify();

        [Fact]
        public virtual Task V_Limit2_V_LimitLocal_1() => _g
            .V()
            .Limit(2)
            .Fold()
            .LimitLocal(1)
            .Verify();

        [Fact]
        public virtual Task V_Limit2_V_LimitLocal_2() => _g
            .V()
            .Limit(2)
            .Fold()
            .LimitLocal(2)
            .Verify();

        [Fact]
        public virtual Task V_Limit2_V_RangeLocal_0() => _g
            .V()
            .Limit(2)
            .Fold()
            .RangeLocal(0, 0)
            .Verify();

        [Fact]
        public virtual Task V_Limit2_V_RangeLocal_1() => _g
            .V()
            .Limit(2)
            .Fold()
            .RangeLocal(0, 1)
            .Verify();

        [Fact]
        public virtual Task V_Limit2_V_RangeLocal_2() => _g
            .V()
            .Limit(2)
            .Fold()
            .RangeLocal(0, 2)
            .Verify();

        [Fact]
        public virtual Task V_Limit2_V_TailLocal_0() => _g
            .V()
            .Limit(2)
            .Fold()
            .TailLocal(0)
            .Verify();

        [Fact]
        public virtual Task V_Limit2_V_TailLocal_1() => _g
            .V()
            .Limit(2)
            .Fold()
            .TailLocal(1)
            .Verify();

        [Fact]
        public virtual Task V_Limit2_V_TailLocal_2() => _g
            .V()
            .Limit(2)
            .Fold()
            .TailLocal(2)
            .Verify();

        [Fact]
        public virtual Task V_of_abstract_type() => _g
            .V<Authority>()
            .Verify();

        [Fact]
        public virtual Task V_of_all_types1() => _g
            .V<object>()
            .Count()
            .Verify();

        [Fact]
        public virtual Task V_of_all_types2() => _g
            .V()
            .Count()
            .Verify();

        [Fact]
        public virtual Task V_of_concrete_type() => _g
            .V<Person>()
            .Verify();

        [Fact]
        public virtual Task V_untyped() => _g
            .V()
            .Count()
            .Verify();

        [Fact]
        public virtual Task Value() => _g
            .V()
            .Properties()
            .Value()
            .Verify();

        [Fact]
        public virtual Task ValueMap_typed() => _g
            .V<Person>()
            .ValueMap(x => x.Age)
            .Verify();

        [Fact]
        public virtual Task Values_1_member() => _g
             .V<Person>()
             .Values(x => x.Age)
             .Verify();

        [Fact]
        public virtual Task Values_2_members() => _g
            .V<Person>()
            .Values(x => x.Name, x => x.Id)
            .Verify();

        [Fact]
        public virtual Task Values_3_members() => _g
            .V<Person>()
            .Values(x => x.Name, x => x.Gender, x => x.Id)
            .Verify();

        [Fact]
        public virtual Task Values_id_member() => _g
            .V<Person>()
            .Values(x => x.Id)
            .Verify();

        [Fact]
        public virtual Task Values_no_member() => _g
            .V<Person>()
            .Values()
            .Verify();

        [Fact]
        public virtual Task Values_of_Edge() => _g
            .E<LivesIn>()
            .Values(x => x.Since!)
            .Verify();

        [Fact]
        public virtual Task Values_of_Vertex1() => _g
            .V<Person>()
            .Values(x => x.Name!)
            .Verify();

        [Fact]
        public virtual Task Values_of_Vertex2() => _g
            .V<Person>()
            .Values(x => x.Name!)
            .Verify();

        [Fact]
        public virtual Task Values_ToString() => _g
            .V<Person>()
            .Values(x => x.Name!.ToString())
            .Verify();

        [Fact]
        public virtual Task VertexProperties_Where_label() => _g
            .V<Company>()
            .Properties(x => x.Locations!)
            .Where(x => x.Label == "someKey")
            .Verify();

        [Fact]
        public virtual Task Where_anonymous() => _g
            .V<Person>()
            .Where(_ => _)
            .Verify();

        [Fact]
        public virtual Task Where_array_does_not_intersect_property_array() => _g
            .V<Company>()
            .Where(t => !new[] { "+4912345", "+4923456" }.Intersect(t.PhoneNumbers!).Any())
            .Verify();

        [Fact]
        public virtual Task Where_array_intersects_property_aray() => _g
            .V<Company>()
            .Where(t => new[] { "+4912345", "+4923456" }.Intersect(t.PhoneNumbers!).Any())
            .Verify();

        [Fact]
        public virtual Task Where_bool_property_explicit_comparison1() => _g
            .V<TimeFrame>()
            // ReSharper disable once RedundantBoolCompare
            .Where(t => t.Enabled == true)
            .Verify();

        [Fact]
        public virtual Task Where_bool_property_explicit_comparison2() => _g
            .V<TimeFrame>()
            .Where(t => t.Enabled == false)
            .Verify();

        [Fact]
        public virtual Task Where_bool_property_implicit_comparison1() => _g
            .V<TimeFrame>()
            .Where(t => t.Enabled)
            .Verify();

        [Fact]
        public virtual Task Where_bool_property_implicit_comparison2() => _g
            .V<TimeFrame>()
            .Where(t => !t.Enabled)
            .Verify();

        [Fact]
        public virtual Task Where_cast_property_ToString() => _g
            .V<Country>()
            .Where(x => ((Exception)(object)x.CountryCallingCode!).ToString() == "some_string")
            .Verify();

        [Fact]
        public virtual Task Where_complex_logical_expression() => _g
            .V<Person>()
            .Where(t => t.Name!.Value == "Some name" && (t.Age == 42 || t.Age == 99))
            .Verify();

        [Fact]
        public virtual Task Where_complex_logical_expression_with_null() => _g
            .V<Person>()
            .Where(t => t.Name == null && (t.Age == 42 || t.Age == 99))
            .Verify();

        [Fact]
        public virtual Task Where_conjunction() => _g
            .V<Person>()
            .Where(t => t.Age == 36 && t.Age == 42)
            .Verify();

        [Fact]
        public virtual Task Where_conjunction_optimizable() => _g
            .V<Person>()
            .Where(t => (t.Age == 36 && t.Name!.Value == "Hallo") && t.Age == 42)
            .Verify();

        [Fact]
        public virtual Task Where_conjunction_with_different_fields() => _g
            .V<Person>()
            .Where(t => t.Name!.Value == "Some name" && t.Age == 42)
            .Verify();

        [Fact]
        public virtual Task Where_converted_Id_equals_constant() => _g
            .V<Language>()
            .Where(t => (int)t.Id! == 1)
            .Verify();

        [Fact]
        public virtual Task Where_current_element_equals_stepLabel1() => _g
            .V<Language>()
            .As((__, l) => __
                .V<Language>()
                .Where(l2 => l2 == l.Value))
            .Verify();

        [Fact]
        public virtual Task Where_current_element_equals_stepLabel2() => _g
            .V<Language>()
            .As((__, l) => __
                .V<Language>()
                .Where(l2 => l.Value == l2))
            .Verify();

        [Fact]
        public virtual Task Where_current_element_not_equals_stepLabel1() => _g
            .V<Language>()
            .As((__, l) => __
                .V<Language>()
                .Where(l2 => l2 != l.Value))
            .Verify();

        [Fact]
        public virtual Task Where_current_element_not_equals_stepLabel2() => _g
            .V<Language>()
            .As((__, l) => __
                .V<Language>()
                .Where(l2 => l.Value != l2))
            .Verify();

        [Fact]
        public virtual Task Where_disjunction() => _g
            .V<Person>()
            .Where(t => t.Age == 36 || t.Age == 42)
            .Verify();

        [Fact]
        public virtual Task Where_disjunction_with_different_fields() => _g
            .V<Person>()
            .Where(t => t.Name!.Value == "Some name" || t.Age == 42)
            .Verify();

        [Fact]
        public virtual Task Where_empty_array_does_not_intersect_property_array() => _g
            .V<Company>()
            .Where(t => !Array.Empty<string>().Intersect(t.PhoneNumbers!).Any())
            .Verify();

        [Fact]
        public virtual Task Where_empty_array_intersects_property_array() => _g
            .V<Company>()
            .Where(t => Array.Empty<string>().Intersect(t.PhoneNumbers!).Any())
            .Verify();

        [Fact]
        public virtual Task Where_Has() => _g
            .V<Person>()
            .Where(__ => __
               .Where(t => t.Age == 36))
            .Verify();

        [Fact]
        public virtual Task Where_has_conjunction_of_three() => _g
            .V<Person>()
            .Where(t => t.Age == 36 && t.Age == 42 && t.Age == 99)
            .Verify();

        [Fact]
        public virtual Task Where_has_disjunction_of_three() => _g
            .V<Person>()
            .Where(t => t.Age == 36 || t.Age == 42 || t.Age == 99)
            .Verify();

        [Fact]
        public virtual Task Where_has_disjunction_of_three_with_or() => _g
            .V<Person>()
            .Or(
                __ => __.Where(t => t.Age == 36),
                __ => __.Where(t => t.Age == 42),
                __ => __.Where(t => t.Age == 99))
            .Verify();

        [Fact]
        public virtual Task Where_Id_equals_constant() => _g
            .V<Language>()
            .Where(t => t.Id == (object)1)
            .Verify();

        [Fact]
        public virtual Task Where_Id_equals_toStringed_Guid() => _g
            .V<Language>()
            .Where(t => (string?)t.Id == Guid.Parse("{105A7662-6400-4723-A08A-6837B8FEA6E6}").ToString())
            .Verify();

        [Fact]
        public virtual Task Where_identity() => _g
            .V<Person>()
            .Where(_ => _.Identity())
            .Verify();

        [Fact]
        public virtual Task Where_identity_with_type_change() => _g
            .V<Person>()
            .Where(_ => _.OfType<Authority>())
            .Verify();

        [Fact]
        public virtual Task Where_none_traversal() => _g
            .V<Person>()
            .Where(_ => _.None())
            .Verify();

        [Fact]
        public virtual Task Where_not_none() => _g
            .V<Person>()
            .Where(_ => _
                .Not(_ => _
                    .None()))
            .Verify();

        [Fact]
        public virtual Task Where_nullable_enum_property_equals_null() => _g
            .V<Person>()
            .Where(t => t.Gender == null)
            .Verify();

        [Fact]
        public virtual Task Where_nullable_enum_property_not_equals_null() => _g
            .V<Person>()
            .Where(t => t.Gender != null)
            .Verify();

        [Fact]
        public virtual async Task Where_Nullable_equals_nullable()
        {
            DateTime? dateTime = DateTime.MinValue;

            await _g
                .V<Person>()
                .Where(t => t.RegistrationDate!.Value == dateTime)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_Nullable_equals_nullable_null()
        {
            DateTime? dateTime = null;

            await _g
                .V<Person>()
                .Where(t => t.RegistrationDate!.Value == dateTime)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_Nullable_equals_nullable_null_Value()
        {
            DateTime? dateTime = null;

            await _g
                .V<Person>()
                .Where(t => t.RegistrationDate!.Value == dateTime!.Value)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_Nullable_equals_nullable_Value()
        {
            DateTime? dateTime = DateTime.MinValue;

            await _g
                .V<Person>()
                .Where(t => t.RegistrationDate!.Value == dateTime.Value)
                .Verify();
        }

        [Fact]
        public virtual Task Where_Nullable_Value() => _g
            .V<Person>()
            .Where(t => t.RegistrationDate!.Value == DateTime.MinValue)
            .Verify();

        [Fact]
        public virtual Task Where_or_dead_traversal() => _g
            .V<Person>()
            .Where(_ => _
                .Or(_ => _
                    .Where(x => Array.Empty<object>().Contains(x.Id))))
            .Verify();

        [Fact]
        public virtual Task Where_or_identity() => _g
            .V<Person>()
            .Where(_ => _
                .Or(_ => _))
            .Verify();

        [Fact]
        public virtual Task Where_or_none_traversal() => _g
            .V<Person>()
            .Where(_ => _
                .Or(_ => _
                    .None()))
            .Verify();

        [Fact]
        public virtual Task Where_out_vertex_property() => _g
            .V<Person>()
            .Where(__ => __
                .Out<WorksFor>()
                .OfType<Company>()
                .Values(x => x.Name!.Value)
                .Where(x => x == "MyCompany"))
            .Verify();

        [Fact]
        public virtual Task Where_outside_model() => _g
            .ConfigureEnvironment(env => env
                .UseModel(GraphModel.FromBaseTypes<VertexWithStringId, EdgeWithStringId>()))
            .V<VertexWithStringId>()
            .Where(x => x.Id == (object)0)
            .Verify();

        [Fact]
        public virtual Task Where_properties_length() => _g
            .V<Person>()
            .Where(t => t.PhoneNumbers!.Length == 3)
            .Verify();

        [Fact]
        public virtual Task Where_property_array_contains_element() => _g
            .V<Company>()
            .Where(t => t.PhoneNumbers!.Contains("+4912345"))
            .Verify();

        [Fact]
        public virtual Task Where_property_array_contains_stepLabel() => _g
            .Inject("+4912345")
            .As((__, t) => __
                .V<Company>()
                .Where(c => c.PhoneNumbers!.Contains(t.Value)))
            .Verify();

        [Fact]
        public virtual Task Where_property_array_does_not_contain_element() => _g
            .V<Company>()
            .Where(t => !t.PhoneNumbers!.Contains("+4912345"))
            .Verify();

        [Fact]
        public virtual Task Where_property_array_does_not_intersect_array() => _g
            .V<Company>()
            .Where(t => !t.PhoneNumbers!.Intersect(new[] { "+4912345", "+4923456" }).Any())
            .Verify();

        [Fact]
        public virtual Task Where_property_array_does_not_intersect_empty_array() => _g
            .V<Company>()
            .Where(t => !t.PhoneNumbers!.Intersect(Array.Empty<string>()).Any())
            .Verify();

        [Fact]
        public virtual Task Where_property_array_intersects_array1() => _g
            .V<Company>()
            .Where(t => t.PhoneNumbers!.Intersect(new[] { "+4912345", "+4923456" }).Any())
            .Verify();

        [Fact]
        public virtual Task Where_property_array_intersects_array2() => _g
            .V<Company>()
            .Where(t => new[] { "+4912345", "+4923456" }.Intersect(t.PhoneNumbers!).Any())
            .Verify();

        [Fact]
        public virtual Task Where_property_array_intersects_empty_array() => _g
            .V<Company>()
            .Where(t => t.PhoneNumbers!.Intersect(Array.Empty<string>()).Any())
            .Verify();

        [Fact]
        public virtual Task Where_property_array_intersects_stepLabel1() => _g
            .Inject("+4912345")
            .Fold()
            .As((__, t) => __
                .V<Company>()
                .Where(c => c.PhoneNumbers!.Intersect(t.Value).Any()))
            .Verify();

        [Fact]
        public virtual Task Where_property_array_intersects_stepLabel2() => _g
            .Inject("+4912345")
            .Fold()
            .As((__, t) => __
                .V<Company>()
                .Where(c => t.Value.Intersect(c.PhoneNumbers!).Any()))
            .Verify();

        [Fact]
        public virtual Task Where_property_array_is_empty() => _g
            .V<Company>()
            .Where(t => !t.PhoneNumbers!.Any())
            .Verify();

        [Fact]
        public virtual Task Where_property_array_is_not_empty() => _g
            .V<Company>()
            .Where(t => t.PhoneNumbers!.Any())
            .Verify();

        [Fact]
        public virtual Task Where_property_compared_to_string_always_false() => _g
            .V<Person>()
            .Where(t => t.Name!.Value.CompareTo("Some name") < -1)
            .Verify();

        [Fact]
        public virtual Task Where_property_compared_to_string_always_true() => _g
            .V<Person>()
            .Where(t => t.Name!.Value.CompareTo("Some name") >= -1)
            .Verify();

        [Fact]
        public virtual Task Where_property_comparison_to_string_always_false() => _g
            .V<Person>()
            .Where(t => t.Name!.Value.CompareTo("Some name") > 1)
            .Verify();

        [Fact]
        public virtual Task Where_property_comparison_to_string_always_false_2() => _g
            .V<Person>()
            .Where(t => t.Name!.Value.CompareTo("Some name") == 2)
            .Verify();

        [Fact]
        public virtual Task Where_property_comparison_to_string_always_false_3() => _g
            .V<Person>()
            .Where(t => t.Name!.Value.CompareTo("Some name") > 2)
            .Verify();

        [Fact]
        public virtual Task Where_property_comparison_to_string_always_false_4() => _g
            .V<Person>()
            .Where(t => t.Name!.Value.CompareTo("Some name") >= 2)
            .Verify();

        [Fact]
        public virtual Task Where_property_comparison_to_string_always_true() => _g
            .V<Person>()
            .Where(t => t.Name!.Value.CompareTo("Some name") <= 1)
            .Verify();

        [Fact]
        public virtual Task Where_property_comparison_to_string_always_true_2() => _g
            .V<Person>()
            .Where(t => t.Name!.Value.CompareTo("Some name") < 2)
            .Verify();

        [Fact]
        public virtual Task Where_property_comparison_to_string_always_true_3() => _g
            .V<Person>()
            .Where(t => t.Name!.Value.CompareTo("Some name") <= 2)
            .Verify();

        [Fact]
        public virtual Task Where_property_comparison_to_string_not_always_false_2() => _g
            .V<Person>()
            .Where(t => t.Name!.Value.CompareTo("Some name") != 2)
            .Verify();

        [Fact]
        public virtual async Task Where_property_comparison_to_string_with_cast_enum_on_field()
        {
            var variable = new
            {
                Field = ListSortDirection.Ascending
            };

            await _g
                .V<Person>()
                .Where(t => t.Name!.Value.CompareTo("Some name") == (int)variable.Field)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_comparison_to_string_with_cast_enum_variable()
        {
            var variable = ListSortDirection.Ascending;

            await _g
                .V<Person>()
                .Where(t => t.Name!.Value.CompareTo("Some name") == (int)variable)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_comparison_to_string_with_variable()
        {
            var variable = 0;

            await _g
                .V<Person>()
                .Where(t => t.Name!.Value.CompareTo("Some name") == variable)
                .Verify();
        }

        [Fact]
        public virtual Task Where_property_contains_constant_with_TextP_support() => _g
            .V<Country>()
            .Where(c => c.CountryCallingCode!.Contains("456"))
            .Verify();

        [Fact]
        public virtual Task Where_property_contains_constant_with_TextP_support_case_insensitive() => _g
            .V<Country>()
            .Where(c => c.CountryCallingCode!.Contains("456", StringComparison.OrdinalIgnoreCase))
            .Verify();

        [Fact]
        public virtual Task Where_property_contains_empty_string_with_TextP_support() => _g
            .V<Country>()
            .Where(c => c.CountryCallingCode!.Contains(""))
            .Verify();

        [Fact]
        public virtual Task Where_property_contains_empty_string_with_TextP_support_case_insensitive() => _g
            .V<Country>()
            .Where(c => c.CountryCallingCode!.Contains("", StringComparison.OrdinalIgnoreCase))
            .Verify();

        [Fact]
        public virtual Task Where_property_contains_empty_string_without_TextP_support() => _g
            .ConfigureEnvironment(env => env
                .ConfigureOptions(c => c
                    .SetValue(GremlinqOption.DisabledTextPredicates, DisabledTextPredicates.StartingWith)))
            .V<Country>()
            .Where(c => c.CountryCallingCode!.Contains(""))
            .Verify();

        [Fact]
        public virtual Task Where_property_ends_with_constant_with_TextP_support() => _g
            .V<Country>()
            .Where(c => c.CountryCallingCode!.EndsWith("7890"))
            .Verify();

        [Fact]
        public virtual Task Where_property_ends_with_constant_with_TextP_support_case_insensitive() => _g
            .V<Country>()
            .Where(c => c.CountryCallingCode!.EndsWith("7890", StringComparison.OrdinalIgnoreCase))
            .Verify();

        [Fact]
        public virtual Task Where_property_ends_with_empty_string_with_TextP_support() => _g
            .V<Country>()
            .Where(c => c.CountryCallingCode!.EndsWith(""))
            .Verify();

        [Fact]
        public virtual Task Where_property_ends_with_empty_string_with_TextP_support_case_insensitive() => _g
            .V<Country>()
            .Where(c => c.CountryCallingCode!.EndsWith("", StringComparison.OrdinalIgnoreCase))
            .Verify();

        [Fact]
        public virtual Task Where_property_ends_with_empty_string_without_TextP_support() => _g
            .ConfigureEnvironment(env => env
                .ConfigureOptions(c => c
                    .SetValue(GremlinqOption.DisabledTextPredicates, DisabledTextPredicates.EndingWith)))
            .V<Country>()
            .Where(c => c.CountryCallingCode!.EndsWith(""))
            .Verify();

        [Fact]
        public virtual Task Where_property_equals_constant() => _g
            .V<Person>()
            .Where(t => t.Age == 36)
            .Verify();

        [Fact]
        public virtual Task Where_property_equals_constant_with_Equals() => _g
            .V<Person>()
            .Where(t => t.Age.Equals(36))
            .Verify();

        [Fact]
        public virtual Task Where_property_equals_converted_expression() => _g
            .V<Person>()
            .Where(t => (object)t.Age == (object)36)
            .Verify();

        [Fact]
        public virtual async Task Where_property_equals_expression()
        {
            const int i = 18;

            await _g
                .V<Person>()
                .Where(t => t.Age == i + i)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_equals_local_string_constant()
        {
            const int local = 1;

            await _g
                .V<Language>()
                .Where(t => t.Id == (object)local)
                .Verify();
        }

        [Fact]
        public virtual Task Where_property_equals_stepLabel() => _g
            .Inject("en")
            .As((__, l) => __
                .V<Language>()
                .Where(l2 => l2.IetfLanguageTag == l.Value)
                .Order(b => b
                    .By(x => x.Id)))
            .Verify();

        [Fact]
        public virtual Task Where_property_equals_string() => _g
            .V<Person>()
            .Where(t => t.Name!.Value.CompareTo("Some name") == 0)
            .Verify();

        [Fact]
        public virtual async Task Where_property_equals_value_of_anonymous_object()
        {
            var local = new { Value = 1 };

            await _g
                .V<Language>()
                .Where(t => t.Id == (object)local.Value)
                .Verify();
        }

        [Fact]
        public virtual Task Where_property_greater_or_equal_string_2() => _g
            .V<Person>()
            .Where(t => t.Name!.Value.CompareTo("Some name") > -1)
            .Verify();

        [Fact]
        public virtual Task Where_property_greater_than_or_equal_string() => _g
            .V<Person>()
            .Where(t => t.Name!.Value.CompareTo("Some name") >= 0)
            .Verify();

        [Fact]
        public virtual Task Where_property_greater_than_or_equal_string_with_IComparable() => _g
            .V<Person>()
            // ReSharper disable once RedundantCast
            .Where(t => ((IComparable<string>)t.Name!.Value).CompareTo("Some name") >= 0)
            .Verify();

        [Fact]
        public virtual Task Where_property_greater_than_string() => _g
            .V<Person>()
            .Where(t => t.Name!.Value.CompareTo("Some name") > 0)
            .Verify();

        [Fact]
        public virtual Task Where_property_greater_than_string_2() => _g
            .V<Person>()
            .Where(t => t.Name!.Value.CompareTo("Some name") == 1)
            .Verify();

        [Fact]
        public virtual Task Where_property_greater_than_string_3() => _g
            .V<Person>()
            .Where(t => t.Name!.Value.CompareTo("Some name") >= 1)
            .Verify();

        [Fact]
        public virtual Task Where_property_is_contained_in_array() => _g
            .V<Person>()
            .Where(t => new[] { 36, 37, 38 }.Contains(t.Age))
            .Verify();

        [Fact]
        public virtual async Task Where_property_is_contained_in_empty_enumerable()
        {
            var enumerable = Enumerable.Empty<int>();

            await _g
                .V<Person>()
                .Where(t => enumerable.Contains(t.Age))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_is_contained_in_enumerable()
        {
            var enumerable = new[] { "36", "37", "38" }
                .Select(int.Parse);

            await _g
                .V<Person>()
                .Where(t => enumerable.Contains(t.Age))
                .Verify();
        }

        [Fact]
        public virtual Task Where_property_is_contained_in_list() => _g
            .V<Person>()
            .Where(t => new List<int> { 36, 37, 38 }.Contains(t.Age))
            .Verify();

        [Fact]
        public virtual Task Where_property_is_greater_or_equal_than_constant() => _g
            .V<Person>()
            .Where(t => t.Age >= 36)
            .Verify();

        [Fact]
        public virtual Task Where_property_is_greater_than_constant() => _g
            .V<Person>()
            .Where(t => t.Age > 36)
            .Verify();

        //TODO: Add Persons with different ages.
        [Fact]
        public virtual Task Where_property_is_greater_than_or_equal_stepLabel() => _g
            .Inject(20)
            .As((__, a) => __
                .V<Person>()
                .Where(l2 => l2.Age >= a.Value)
                .Values(x => x.Age))
            .Verify();

        //TODO: Add Persons with different ages.
        [Fact]
        public virtual Task Where_property_is_greater_than_or_equal_stepLabel_value() => _g
            .V<Person>()
            .Order(b => b
                .By(x => x.Age))
            .As((__, person1) => __
                .Map(__ => __
                    .V<Person>()
                    .Where(person2 => person2.Age >= person1.Value.Age)
                    .Order(b => b
                        .By(x => x.Age))
                    .Values(x => x.Age)
                    .Fold()))
            .Verify();

        [Fact]
        public virtual Task Where_property_is_greater_than_stepLabel() => _g
            .V<Person>()
            .Values(x => x.Age)
            .As((__, a) => __
                .V<Person>()
                .Where(l2 => l2.Age > a.Value))
            .Verify();

        [Fact]
        public virtual Task Where_property_is_lower_or_equal_than_constant() => _g
            .V<Person>()
            .Where(t => t.Age <= 36)
            .Verify();

        [Fact]
        public virtual Task Where_property_is_lower_than_constant() => _g
            .V<Person>()
            .Where(t => t.Age < 36)
            .Verify();

        //TODO: Add Persons with different ages.
        [Fact]
        public virtual Task Where_property_is_lower_than_or_equal_stepLabel() => _g
            .Inject(36)
            .As((__, a) => __
                .V<Person>()
                .Where(l2 => l2.Age <= a.Value)
                .Values(x => x.Age))
            .Verify();

        [Fact]
        public virtual Task Where_property_is_lower_than_stepLabel() => _g
            .V<Person>()
            .Values(x => x.Age)
            .As((__, a) => __
                .V<Person>()
                .Where(l2 => l2.Age < a.Value))
            .Verify();

        [Fact]
        public virtual Task Where_property_is_not_contained_in_array() => _g
            .V<Person>()
            .Where(t => !new[] { 36, 37, 38 }.Contains(t.Age))
            .Verify();

        [Fact]
        public virtual async Task Where_property_is_not_contained_in_empty_enumerable()
        {
            var enumerable = Enumerable.Empty<int>();

            await _g
                .V<Person>()
                .Where(t => !enumerable.Contains(t.Age))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_is_not_contained_in_enumerable()
        {
            var enumerable = new[] { "36", "37", "38" }
                .Select(int.Parse);

            await _g
                .V<Person>()
                .Where(t => !enumerable.Contains(t.Age))
                .Verify();
        }

        [Fact]
        public virtual Task Where_property_is_not_present() => _g
            .V<Person>()
            .Where(t => t.Name == null)
            .Verify();

        [Fact]
        public virtual Task Where_property_is_prefix_of_constant() => _g
            .V<Country>()
            .Where(c => "+49123".StartsWith(c.CountryCallingCode!))
            .Verify();

        [Fact]
        public virtual Task Where_property_is_prefix_of_constant_case_insensitive() => _g
            .V<Country>()
            .Where(c => "+49123".StartsWith(c.CountryCallingCode!, StringComparison.OrdinalIgnoreCase))
            .Verify();

        [Fact]
        public virtual Task Where_property_is_prefix_of_empty_string() => _g
            .V<Country>()
            .Where(c => "".StartsWith(c.CountryCallingCode!))
            .Verify();

        [Fact]
        public virtual Task Where_property_is_prefix_of_empty_string_case_insensitive() => _g
            .V<Country>()
            .Where(c => "".StartsWith(c.CountryCallingCode!, StringComparison.OrdinalIgnoreCase))
            .Verify();

        [Fact]
        public virtual async Task Where_property_is_prefix_of_expression()
        {
            const string str = "+49123xxx";

            await _g
                .V<Country>()
                .Where(c => str.Substring(0, 6).StartsWith(c.CountryCallingCode!))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_is_prefix_of_expression_case_insensitive()
        {
            const string str = "+49123xxx";

            await _g
                .V<Country>()
                .Where(c => str.Substring(0, 6).StartsWith(c.CountryCallingCode!, StringComparison.OrdinalIgnoreCase))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_is_prefix_of_UriToString()
        {
            var uri = new Uri("tel:+49123");

            await _g
                .V<Country>()
                .Where(c => uri.ToString().StartsWith(c.CountryCallingCode!))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_is_prefix_of_variable()
        {
            const string str = "+49123";

            await _g
                .V<Country>()
                .Where(c => str.StartsWith(c.CountryCallingCode!))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_is_prefix_of_variable_case_insensitive()
        {
            const string str = "+49123";

            await _g
                .V<Country>()
                .Where(c => str.StartsWith(c.CountryCallingCode!, StringComparison.OrdinalIgnoreCase))
                .Verify();
        }

        [Fact]
        public virtual Task Where_property_is_present() => _g
            .V<Person>()
            .Where(t => t.Name != null)
            .Verify();

        [Fact]
        public virtual Task Where_property_lower_than_or_equal_string() => _g
            .V<Person>()
            .Where(t => t.Name!.Value.CompareTo("Some name") <= 0)
            .Verify();

        [Fact]
        public virtual Task Where_property_lower_than_or_equal_string_2() => _g
            .V<Person>()
            .Where(t => t.Name!.Value.CompareTo("Some name") < 1)
            .Verify();

        [Fact]
        public virtual Task Where_property_lower_than_string() => _g
            .V<Person>()
            .Where(t => t.Name!.Value.CompareTo("Some name") < 0)
            .Verify();

        [Fact]
        public virtual Task Where_property_lower_than_string_2() => _g
            .V<Person>()
            .Where(t => t.Name!.Value.CompareTo("Some name") <= -1)
            .Verify();

        [Fact]
        public virtual Task Where_property_lower_than_string_3() => _g
            .V<Person>()
            .Where(t => t.Name!.Value.CompareTo("Some name") == -1)
            .Verify();

        [Fact]
        public virtual Task Where_property_not_equals_constant() => _g
            .V<Person>()
            .Where(t => t.Age != 36)
            .Verify();

        [Fact]
        public virtual Task Where_property_not_equals_string() => _g
            .V<Person>()
            .Where(t => t.Name!.Value.CompareTo("Some name") != 0)
            .Verify();

        [Fact]
        public virtual Task Where_property_not_greater_than_string_2() => _g
            .V<Person>()
            .Where(t => t.Name!.Value.CompareTo("Some name") != 1)
            .Verify();

        [Fact]
        public virtual Task Where_property_not_lower_than_string_3() => _g
            .V<Person>()
            .Where(t => t.Name!.Value.CompareTo("Some name") != -1)
            .Verify();

        [Fact]
        public virtual Task Where_property_starts_with_constant_with_TextP_support() => _g
            .V<Country>()
            .Where(c => c.CountryCallingCode!.StartsWith("+49123"))
            .Verify();

        [Fact]
        public virtual Task Where_property_starts_with_char_with_TextP_support() => _g
            .V<Country>()
            .Where(c => c.CountryCallingCode!.StartsWith('+'))
            .Verify();

        [Fact]
        public virtual Task Where_property_starts_with_constant_without_TextP_support() => _g
            .ConfigureEnvironment(env => env
                .ConfigureOptions(c => c
                    .SetValue(GremlinqOption.DisabledTextPredicates, DisabledTextPredicates.StartingWith)))
            .V<Country>()
            .Where(c => c.CountryCallingCode!.StartsWith("+49123"))
            .Verify();

        [Fact]
        public virtual Task Where_property_starts_with_empty_string_with_TextP_support() => _g
            .V<Country>()
            .Where(c => c.CountryCallingCode!.StartsWith(""))
            .Verify();

        [Fact]
        public virtual Task Where_property_starts_with_empty_string_without_TextP_support() => _g
            .ConfigureEnvironment(env => env
                .ConfigureOptions(c => c
                    .SetValue(GremlinqOption.DisabledTextPredicates, DisabledTextPredicates.StartingWith)))
            .V<Country>()
            .Where(c => c.CountryCallingCode!.StartsWith(""))
            .Verify();

        [Fact]
        public virtual Task Where_property_ToString() => _g
            .V<Country>()
            .Where(x => x.CountryCallingCode!.ToString() == "some_string")
            .Verify();

        [Fact]
        public virtual Task Where_property_traversal() => _g
            .V<Person>()
            .Where(
                x => x.Age,
                _ => _
                    .Inject(36))
            .Verify();

        [Fact]
        public virtual Task Where_scalar_element_equals_constant() => _g
            .V<Person>()
            .Values(x => x.Age)
            .Where(_ => _ == 36)
            .Verify();

        [Fact]
        public virtual Task Where_sequential() => _g
            .V<Person>()
            .Where(t => t.Age == 36)
            .Where(t => t.Age == 42)
            .Verify();

        [Fact]
        public virtual Task Where_source_expression_on_both_sides1() => _g
            .V<Country>()
            .Where(x => x.Name != null)
            .Where(x => x.CountryCallingCode != null)
            .Where(t => t.Name!.Value == t.CountryCallingCode)
            .Verify();

        [Fact]
        public virtual Task Where_source_expression_on_both_sides2() => _g
            .V<EntityWithTwoIntProperties>()
            .Where(x => x.IntProperty1 > x.IntProperty2)
            .Verify();

        [Fact]
        public virtual Task Where_stepLabel_is_lower_than_stepLabel() => _g
            .V<Person>()
            .Where(__ => __
                .As((__, person1) => __
                    .Values(x => x.Gender)
                    .As((__, gender1) => __
                        .V<Person>()
                        .Values(x => x.Gender)
                            .As((__, gender2) => __
                                .Where(p => gender1.Value < gender2.Value)))))
            .Verify();

        [Fact]
        public virtual Task Where_stepLabel_property_equals_stepLabel() => _g
            .V<Person>()
            .As((__, person) => __
                .V<Person>()
                .Values(x => x.Age)
                    .As((__, age) => __
                        .Where(p => person.Value.Age < age.Value)))
            .Verify();

        [Fact]
        public virtual Task Where_stepLabel_equals_stepLabel_property() => _g
            .V<Person>()
            .As((__, person) => __
                .V<Person>()
                .Values(x => x.Age)
                    .As((__, age) => __
                        .Where(p => age.Value < person.Value.Age)))
            .Verify();

        [Fact]
        public virtual Task Where_stepLabel_value_is_greater_than_or_equal_stepLabel_value() => _g
            .V<Person>()
            .As((__, person1) => __
                .V<Person>()
                .As((__, person2) => __
                    .Where(_ => person1.Value.Age >= person2.Value.Age)))
            .Count()
            .Verify();

        [Fact]
        public virtual Task Where_string_property_equals() => _g
            .V<Country>()
            .Where(c => c.CountryCallingCode!.Equals("+49123"))
            .Verify();

        [Fact]
        public virtual Task Where_string_property_equals_case_insensitive() => _g
            .V<Country>()
            .Where(c => c.CountryCallingCode!.Equals("+49123", StringComparison.OrdinalIgnoreCase))
            .Verify();

        [Fact]
        public virtual Task Where_string_property_startsWith() => _g
            .V<Country>()
            .Where(c => c.CountryCallingCode!.StartsWith("+49123"))
            .Verify();

        [Fact]
        public virtual Task Where_string_property_startsWith_case_insensitive() => _g
            .V<Country>()
            .Where(c => c.CountryCallingCode!.StartsWith("+49123", StringComparison.OrdinalIgnoreCase))
            .Verify();

        [Fact]
        public virtual Task Where_traversal() => _g
            .V<Person>()
            .Where(_ => _.Out<LivesIn>())
            .Verify();

        [Fact]
        public virtual Task Where_true() => _g
            .V<Person>()
            .Where(_ => true)
            .Verify();

        [Fact]
        public virtual Task Where_value_of_property_is_greater_than_null() => _g
            .V<Person>()
            .Where(__ => __
                .Values(x => x.Name!.Value)
                .Where(x => (int)(object)x > (int)(object)null!))
            .Verify();

        [Fact]
        public virtual async Task Where_value_of_property_is_greater_than_null_variable()
        {
            string? variable = null;

            await _g
                .V<Person>()
                .Where(__ => __
                    .Values(x => x.Name!.Value)
                    .Where(x => (int)(object)x > (int)(object)variable!))
                .Verify();
        }

        [Fact]
        public virtual Task Where_value_of_property_is_not_null_and_string() => _g
            .V<Person>()
            .Where(__ => __
                .Values(x => x.Name!.Value)
                .Where(x => x != null! && x == "hello"))
            .Verify();

        [Fact]
        public virtual Task Where_value_of_property_is_not_null_or_string() => _g
            .V<Person>()
            .Where(__ => __
                .Values(x => x.Name!.Value)
                .Where(x => x != null! || x == "hello"))
            .Verify();

        [Fact]
        public virtual Task Where_value_of_property_is_null() => _g
            .V<Person>()
            .Where(__ => __
                .Values(x => x.Name!.Value)
                .Where(x => x == null!))
            .Verify();

        [Fact]
        public virtual Task Where_value_of_property_is_null_and_string() => _g
            .V<Person>()
            .Where(__ => __
                .Values(x => x.Name!.Value)
                .Where(x => x == null! && x == "hello"))
            .Verify();

        [Fact]
        public virtual Task Where_value_of_property_is_null_and_string_reversed() => _g
            .V<Person>()
            .Where(__ => __
                .Values(x => x.Name!.Value)
                .Where(x => x == "hello" && x == null!))
            .Verify();

        [Fact]
        public virtual Task Where_value_of_property_is_null_in2() => _g
            .V<Person>()
            .Where(__ => __
                .Values(x => x.Name!.Value)
                .Where(__ => __
                    .Where(x => x == null)))
            .Verify();

        [Fact]
        public virtual Task Where_value_of_property_is_null_or_string() => _g
            .V<Person>()
            .Where(__ => __
                .Values(x => x.Name!.Value)
                .Where(x => x == null! || x == "hello"))
            .Verify();

        [Fact]
        public virtual Task Where_value_of_property_is_null_or_string_reversed() => _g
                .V<Person>()
                .Where(__ => __
                    .Values(x => x.Name!.Value)
                    .Where(x => x == "hello" || x == null!))
                .Verify();

        [Fact]
        public virtual Task Where_Values_Id_Where() => _g
            .V<Person>()
            .Where(x => x
                .Values(x => x.Id)
                .Where(id => (long)id! == 1L))
            .Verify();

        [Fact]
        public virtual Task Where_Values_Label_Where() => _g
            .V<Vertex>()
            .Where(x => x
                .Values(x => x.Label)
                .Where(label => label == "Person"))
            .Verify();

        [Fact]
        public virtual Task Where_Values_Or_WhereWhere() => _g
            .V<Person>()
            .Where(__ => __
                .Values(x => x.Name!.Value)
                .Or(
                    __ => __.Where(x => x! == null!),
                    __ => __.Where(x => (object)x! == "")))
            .Verify();

        [Fact]
        public virtual Task Where_Values_Where() => _g
            .V<Person>()
            .Where(__ => __
                .Values(x => x.Age)
                .Where(age => age > 36))
            .Verify();

        [Fact]
        public virtual Task Where_Values_WhereWhere() => _g
            .V<Person>()
            .Where(__ => __
                .Values(x => x.Name!.Value)
                .Where(x => x == "hallo1")
                .Where(x => x == "hallo2"))
            .Verify();

        [Fact]
        public virtual async Task Where_VertexProperty_starts_with_constant_with_TextP_support_indirection()
        {
            var tuple = ("456", 36);

            await _g
                .V<Country>()
                .Where(c => c.Name!.Value.StartsWith(tuple.Item1))
                .Verify();
        }

        [Fact]
        public virtual Task Where_VertexProperty_Value1() => _g
            .V<Person>()
            .Where(x => x.Name!.Value == "SomeName")
            .Verify();

        [Fact]
        public virtual Task Where_VertexProperty_Value2() => _g
            .V<Person>()
            .Where(x => ((string)(object)x.Name!.Value) == "SomeName")
            .Verify();

        [Fact]
        public virtual Task Where_VertexProperty_Id() => _g
            .V<Person>()
            .Where(x => (int)x.Name!.Id! == 36)
            .Verify();

        [Fact]
        public virtual Task Where_Where() => _g
            .V<Person>()
            .Where(_ => _
                .Where(_ => _.Out()))
            .Verify();

        [Fact]
        public virtual Task Where_with_nested_as() => _g
            .V<Company>()
            .Where(__ => __
                .As((__, a) => __))
            .Verify();

        [Fact]
        public virtual async Task WithSideEffect_assigns_projection()
        {
            var stepLabel = new StepLabel<IArrayGremlinQuery<Person[], Person, IVertexGremlinQuery<Person>>, Person[]>();

            await _g
                .WithSideEffect(stepLabel, Array.Empty<Person>())
                .V<Person>()
                .Aggregate(stepLabel)
                .Fold()
                .Select(stepLabel)
                .Verify();
        }

        [Fact]
        public virtual Task WithSideEffect_continuation() => _g
            .WithSideEffect(
                36,
                (__, label) => __.V())
            .Verify();

        [Fact]
        public virtual Task WithSideEffect_empty_array() => _g
            .WithSideEffect("sideEffectLabel", Array.Empty<Vertex>())
            .V()
            .Verify();

        [Fact]
        public virtual Task WithSideEffect_label_can_be_selected() => _g
            .WithSideEffect(
                36,
                (__, label) => __
                    .V()
                    .Select(label))
            .Verify();

        [Fact]
        public virtual async Task WithSideEffect1()
        {
            var stepLabel = new StepLabel<int>();

            await _g
                .WithSideEffect(stepLabel, 36)
                .V()
                .Verify();
        }

        [Fact]
        public virtual Task WithSideEffect2() => _g
            .WithSideEffect("sideEffectLabel", 36)
            .V()
            .Verify();

        [Fact]
        public virtual async Task RegisterNativeType()
        {
            await _g
                .ConfigureEnvironment(env => env
                    .RegisterNativeType(
                        (languageCode, env, _, recurse) => languageCode.ToString().ToLower(),
                        (valueToken, env, _, recurse) => Enum.TryParse<DateTimeKind>(valueToken.Value<string>(), true, out var res)
                            ? res
                            : default))
                .Inject("Utc")
                .Cast<DateTimeKind>()
                .Verify();
        }
    }
}
