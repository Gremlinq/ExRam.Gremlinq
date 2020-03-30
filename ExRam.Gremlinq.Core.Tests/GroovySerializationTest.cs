using System;
using System.Linq;
using System.Threading.Tasks;
using ExRam.Gremlinq.Core.GraphElements;
using ExRam.Gremlinq.Tests.Entities;
using FluentAssertions;
using Gremlin.Net.Process.Traversal;
using Gremlin.Net.Process.Traversal.Strategy.Decoration;
using Xunit;
using Xunit.Abstractions;
using VerifyXunit;

namespace ExRam.Gremlinq.Core.Tests
{
    public abstract class GroovySerializationTest : VerifyBase
    {
        protected readonly IGremlinQuerySource _g;

        private static readonly string id = "id";

        protected GroovySerializationTest(IGremlinQuerySource g, ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _g = g
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel.FromBaseTypes<Vertex, Edge>(lookup => lookup
                        .IncludeAssembliesOfBaseTypes())));
        }

        private IVertexGremlinQuery<TVertex> V2<TVertex>(IGremlinQuerySource source) where TVertex : IVertex
        {
            return source.V<TVertex>();
        }

        [Fact]
        public async Task StepLabel_of_array_contains_element_graphson()
        {
            await _g
                .Inject(1, 2, 3)
                .Fold()
                .As((_, ints) => _
                    .V<Person>()
                    .Where(person => ints.Value.Contains(person.Age)))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task AddE_from_StepLabel()
        {
            await _g
                .AddV(new Country { CountryCallingCode = "+49" })
                .As((_, c) => _
                    .AddV(new Language { IetfLanguageTag = "en" })
                    .AddE<Speaks>()
                    .From(c))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task AddE_from_traversal()
        {
            var now = DateTimeOffset.UtcNow;

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
                .VerifyQuery(this);
        }

        [Fact]
        public async Task AddE_InV()
        {
            await _g
                .AddV<Person>()
                .AddE<LivesIn>()
                .To(__ => __
                    .V<Country>("id"))
                .InV()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task AddE_OutV()
        {
            await _g
                .AddV<Person>()
                .AddE<LivesIn>()
                .To(__ => __
                    .V<Country>("id"))
                .OutV()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task AddE_property()
        {
            await _g
                .AddV<Person>()
                .AddE(new LivesIn
                {
                    Since = DateTimeOffset.Now
                })
                .To(__ => __
                    .V<Country>("id"))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task AddE_to_StepLabel()
        {
            await _g
                .AddV(new Language { IetfLanguageTag = "en" })
                .As((_, l) => _
                    .AddV(new Country { CountryCallingCode = "+49" })
                    .AddE<Speaks>()
                    .To(l))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task AddE_to_traversal()
        {
            var now = DateTimeOffset.UtcNow;

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
                .VerifyQuery(this);
        }

        [Fact]
        public async Task AddE_With_Ignored()
        {
            var now = DateTime.UtcNow;

            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureModel(model => model
                        .ConfigureProperties(_ => _
                            .ConfigureElement<WorksFor>(conf => conf
                                .IgnoreAlways(p => p.From)
                                .IgnoreAlways(p => p.Role)))))
                .AddE(new WorksFor { From = now, To = now, Role = "Admin" })
                .VerifyQuery(this);
        }

        [Fact]
        public async Task AddV()
        {
            await _g
                .AddV(new Language { Id = 1, IetfLanguageTag = "en" })
                .VerifyQuery(this);
        }

        [Fact]
        public async Task AddV_ignores_label()
        {
            await _g
                .AddV(new Language {Label = "Language"})
                .VerifyQuery(this);
        }

        [Fact]
        public async Task AddV_list_cardinality_id()
        {
            _g
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel
                        .FromBaseTypes<VertexWithListAsId, Edge>(lookup => lookup
                            .IncludeAssembliesOfBaseTypes())))
                .AddV(new VertexWithListAsId { Id = new[] { "123", "456" } })
                .Awaiting(async x => await x.FirstAsync())
                .Should()
                .Throw<NotSupportedException>();
        }

        [Fact]
        public async Task AddV_with_enum_property()
        {
            await _g
                .AddV(new Person { Id = 1, Gender = Gender.Female })
                .VerifyQuery(this);
        }

        [Fact]
        public async Task AddV_With_Ignored()
        {
            var now = DateTimeOffset.UtcNow;
            var person = new Person { Age = 21, Gender = Gender.Male, Name = "Marko", RegistrationDate = now };

            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureModel(model => model
                        .ConfigureProperties(_ => _
                            .ConfigureElement<Person>(conf => conf
                                .IgnoreAlways(p => p.Age)
                                .IgnoreAlways(p => p.Gender)))))
                .AddV(person)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task AddV_with_ignored_id_property()
        {
            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureModel(model => model
                        .ConfigureProperties(_ => _
                            .ConfigureElement<Language>(conf => conf
                                .IgnoreOnAdd(p => p.Id)))))
                .AddV(new Language { Id = 1, IetfLanguageTag = "en" })
                .VerifyQuery(this);
        }

        [Fact]
        public async Task AddV_with_ignored_property()
        {
            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureModel(model => model
                        .ConfigureProperties(_ => _
                            .ConfigureElement<Language>(conf => conf
                                .IgnoreOnAdd(p => p.IetfLanguageTag)))))
                .AddV(new Language { Id = 1, IetfLanguageTag = "en" })
                .VerifyQuery(this);
        }

        [Fact]
        public async Task AddV_with_Meta_with_properties()
        {
            await _g
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
                .VerifyQuery(this);
        }

        [Fact]
        public async Task AddV_with_Meta_without_properties()
        {
            await _g
                .AddV(new Country { Id = 1, Name = "GER"})
                .VerifyQuery(this);
        }

        [Fact]
        public async Task AddV_with_MetaModel()
        {
           await _g
               .AddV(new Company
               {
                   Id = 1,
                   Names = new[]
                   {
                       new VertexProperty<string, PropertyValidity>("Bob")
                       {
                           Properties = new PropertyValidity
                           {
                               ValidFrom = DateTimeOffset.Parse("01.01.2019 08:00")
                           }
                       }
                   }
               })
               .VerifyQuery(this);
        }

        [Fact]
        public async Task AddV_with_multi_property()
        {
            await _g
                .AddV(new Company { Id = 1, PhoneNumbers = new[] { "+4912345", "+4923456" } })
                .VerifyQuery(this);
        }

        [Fact]
        public async Task AddV_with_nulls()
        {
            await _g
                .AddV(new Language { Id = 1 })
                .VerifyQuery(this);
        }

        [Fact]
        public async Task AddV_with_overridden_name()
        {
            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureModel(model => model
                        .ConfigureProperties(propModel => propModel
                            .ConfigureElement<Language>(conf => conf
                                .ConfigureName(x => x.IetfLanguageTag, "lang")))))
                .AddV(new Language { Id = 1, IetfLanguageTag = "en" })
                .VerifyQuery(this);
        }

        [Fact]
        public async Task AddV_without_id()
        {
            await _g
                .AddV(new Language { IetfLanguageTag = "en" })
                .VerifyQuery(this);
        }

        [Fact]
        public async Task AddV_without_model()
        {
            _g
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel.Empty))
                .AddV(new Language { Id = 1, IetfLanguageTag = "en" })
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Aggregate_Global()
        {
            await _g
                .V()
                .AggregateGlobal((__, aggregated) => __)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Aggregate_Local()
        {
            await _g
                .V()
                .Aggregate((__, aggregated) => __)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Aggregate_Cap()
        {
            await _g
                .V<Person>()
                .Aggregate((__, aggregated) => __
                    .Cap(aggregated))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Aggregate_Cap_unfold()
        {
            await _g
                .V<Person>()
                .Aggregate((__, aggregated) => __
                    .Cap(aggregated)
                    .Unfold())
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Aggregate_Cap_type()
        {
            _g
                .V<Person>()
                .Aggregate((__, aggregated) => __
                    .Cap(aggregated))
                .Should()
                .BeAssignableTo<IGremlinQueryBase<Person[]>>();
        }

        [Fact]
        public async Task And()
        {
            await _g
                .V<Person>()
                .And(
                    __ => __
                        .InE<WorksFor>(),
                    __ => __
                        .OutE<LivesIn>())
                .VerifyQuery(this);
        }

        [Fact]
        public async Task And_identity()
        {
            await _g
                .V<Person>()
                .And(
                    __ => __)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task And_infix()
        {
            await _g
                .V<Person>()
                .And()
                .Out()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task And_nested()
        {
            await _g
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
                .VerifyQuery(this);
        }

        [Fact]
        public async Task And_nested_or_optimization()
        {
            await _g
                .V<Person>()
                .And(
                    __ => __.Or(
                        __ => __),
                    __ => __.Out())
                .VerifyQuery(this);
        }

        [Fact]
        public async Task And_none()
        {
            await _g
                .V<Person>()
                .And(
                    __ => __.None())
                .VerifyQuery(this);
        }

        [Fact]
        public async Task And_optimization()
        {
            await _g
                .V<Person>()
                .And(
                    __ => __,
                    __ => __.Out())
                .VerifyQuery(this);
        }

        [Fact]
        public async Task As_followed_by_Select()
        {
            await _g
                .V<Person>()
                .As((_, stepLabel1) => _
                    .Select(stepLabel1))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task As_idempotency_is_detected()
        {
            await _g
                .V<Person>()
                .As((_, stepLabel1) => _
                    .As((__, stepLabel2) => __
                        .Select(stepLabel1, stepLabel2)))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task As_inlined_nested_Select()
        {
            await _g
                .V<Person>()
                .As((_, stepLabel1) => _
                    .Out()
                    .OfType<Person>()
                    .As((__, stepLabel2) => __
                        .Select(stepLabel1, stepLabel2)))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task As_inlined_nested_Select2()
        {
            await _g
                .V<Person>()
                .As((_, stepLabel1) => _
                    .Out()
                    .OfType<Person>()
                    .As((__, stepLabel2) => __
                        .Count()
                        .Select(stepLabel1, stepLabel2)))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task As_with_type_change()
        {
            IGremlinQueryBaseRec<Person, IVertexGremlinQuery<Person>> g = _g
                .V<Person>();

            await g
                .As((_, stepLabel1) => _
                    .Count()
                    .Select(stepLabel1))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Choose_one_case()
        {
            await _g
                .V()
                .Choose(_ => _
                    .On(__ => __.Values())
                    .Case(3, __ => __.Constant(1)))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Choose_only_default_case()
        {
            await _g
                .V()
                .Choose(_ => _
                    .On(__ => __.Values())
                    .Default(__ => __.Constant(1)))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Choose_Predicate1()
        {
            await _g
                .V()
                .Id()
                .Choose(
                    x => x == (object)42,
                    _ => _.Constant(true),
                    _ => _.Constant(false))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Choose_Predicate2()
        {
            await _g
                .V()
                .Id()
                .Choose(
                    x => x == (object)42,
                    _ => _.Constant(true))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Choose_Predicate3()
        {
            await _g
                .V()
                .Id()
                .Cast<int>()
                .Choose(
                    x => x < 42,
                    _ => _.Constant(true),
                    _ => _.Constant(false))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Choose_Predicate4()
        {
            await _g
                .V()
                .Id()
                .Cast<int>()
                .Choose(
                    x => 42 > x,
                    _ => _.Constant(true),
                    _ => _.Constant(false))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Choose_Predicate5()
        {
            await _g
                .V()
                .Id()
                .Cast<int>()
                .Choose(
                    x => 0 < x && x < 42,
                    _ => _.Constant(true),
                    _ => _.Constant(false))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Choose_Predicate6()
        {
            await _g
                .V()
                .Id()
                .Cast<int>()
                .Choose(
                    x => 0 < x && x < 42 || x != 37,
                    _ => _.Constant(true),
                    _ => _.Constant(false))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Choose_Predicate7()
        {
            await _g
                .V()
                .Id()
                .Cast<int>()
                .Choose(
                    x => 0 < x || x < 42 && x != 37,
                    _ => _.Constant(true),
                    _ => _.Constant(false))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Choose_Traversal1()
        {
            await _g
                .V()
                .Choose(
                    _ => _.Values(),
                    _ => _.Out(),
                    _ => _.In())
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Choose_Traversal2()
        {
            await _g
                .V()
                .Choose(
                    _ => _.Values(),
                    _ => _.Out())
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Choose_two_cases()
        {
            await _g
                .V()
                .Choose(_ => _
                    .On(__ => __.Values())
                    .Case(3, __ => __.Constant(1))
                    .Case(4, __ => __.Constant(2)))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Choose_two_cases_default()
        {
            await _g
                .V()
                .Choose(_ => _
                    .On(__ => __.Values())
                    .Case(3, __ => __.Constant(1))
                    .Case(4, __ => __.Constant(2))
                    .Default(__ => __.Constant(3)))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Coalesce()
        {
            await _g
                .V()
                .Coalesce(
                    _ => _
                        .Out())
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Coalesce_empty()
        {
            _g
                .V()
                .Invoking(__ => __.Coalesce<IGremlinQueryBase>())
                .Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public async Task Coalesce_identity()
        {
            await _g
                .V()
                .Coalesce(
                    _ => _
                        .Identity())
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Constant()
        {
            await _g
                .V()
                .Constant(42)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Count()
        {
            await _g
                .V()
                .Count()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task CountGlobal()
        {
            await _g
                .V()
                .Count()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task CountLocal()
        {
            await _g
                .V()
                .CountLocal()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Dedup_Global()
        {
            await _g
                .V()
                .Dedup()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Dedup_Local()
        {
            await _g
                .V()
                .DedupLocal()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Drop()
        {
            await _g
                .V<Person>()
                .Drop()
                .VerifyQuery(this);
        }


        [Fact]
        public async Task Drop_in_local()
        {
            await _g
                .Inject(1)
                .Local(__ => __
                    .V()
                    .Drop())
                .VerifyQuery(this);
        }

        [Fact]
        public async Task E_of_all_types1()
        {
            await _g
                .E<object>()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task E_of_all_types2()
        {
            await _g
                .E()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task E_of_concrete_type()
        {
            await _g
                .E<WorksFor>()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task E_Properties()
        {
            await _g
                .E()
                .Properties()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task E_Properties_member()
        {
            await _g
                .E<LivesIn>()
                .Properties(x => x.Since)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Explicit_As()
        {
            var stepLabel = new StepLabel<Person>();

            await _g
                .V<Person>()
                .As(stepLabel)
                .Select(stepLabel)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task FilterWithLambda()
        {
            await _g
                .V<Person>()
                .Where(Lambda.Groovy("it.property('str').value().length() == 2"))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task FlatMap()
        {
            await _g
                .V<Person>()
                .FlatMap(__ => __.Out<WorksFor>())
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Fold()
        {
            await _g
                .V()
                .Fold()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Fold_Fold_Unfold_Unfold()
        {
            await _g
                .V()
                .Fold()
                .Fold()
                .Unfold()
                .Unfold()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Fold_SideEffect()
        {
            await _g
                .V()
                .Fold()
                .SideEffect(x => x.Identity())
                .Unfold()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Fold_Unfold()
        {
            await _g
                .V()
                .Fold()
                .Unfold()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Generic_constraint()
        {
            await V2<Person>(_g)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Group_with_key()
        {
            await _g
                .V()
                .Group(_ => _
                    .ByKey(_ => _.Label()))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Identity()
        {
            await _g
                .V<Person>()
                .Identity()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Identity_Identity()
        {
            await _g
                .V<Person>()
                .Identity()
                .Identity()
                .VerifyQuery(this);
        }


        [Fact]
        public async Task In()
        {
            await _g
                .V<Person>()
                .In<WorksFor>()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task In_of_all_types_max()
        {
            await _g
                .V()
                .In<object>()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task In_of_all_types_min()
        {
            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(o => o
                        .SetItem(GremlinqOption.FilterLabelsVerbosity, FilterLabelsVerbosity.Minimum)))
                .V()
                .In<object>()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task InE_of_all_types_max()
        {
            await _g
                .V()
                .InE<object>()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task InE_of_all_types_min()
        {
            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(x => x
                        .SetItem(GremlinqOption.FilterLabelsVerbosity, FilterLabelsVerbosity.Minimum)))
                .V()
                .InE<object>()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Inject()
        {
            await _g
                .Inject(36, 37, 38)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Label()
        {
            await _g
                .V()
                .Label()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Limit_underflow()
        {
            _g
                .V()
                .Invoking(_ => _.Limit(-1))
                .Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public async Task LimitGlobal()
        {
            await _g
                .V()
                .Limit(1)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task LimitLocal()
        {
            await _g
                .V()
                .LimitLocal(1)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Local_identity()
        {
            await _g
                .V()
                .Local(__ => __)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Map()
        {
            await _g
                .V<Person>()
                .Map(__ => __.Out<WorksFor>())
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Map_Identity()
        {
            await _g
                .V<Person>()
                .Map(__ => __)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Map_Select_operation()
        {
            await _g
                .V<Person>()
                .As((_, stepLabel1) => _
                    .As((__, stepLabel2) => __
                        .Map(__ => __
                            .Select(stepLabel1, stepLabel2))))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task MaxGlobal()
        {
            await _g
                .V<Person>()
                .Values(x => x.Age)
                .Max()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task MaxLocal()
        {
            await _g
                .V<Person>()
                .Values(x => x.Age)
                .MaxLocal()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task MeanGlobal()
        {
            await _g
                .V<Person>()
                .Values(x => x.Age)
                .Mean()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task MeanLocal()
        {
            await _g
                .V<Person>()
                .Values(x => x.Age)
                .MeanLocal()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task MinGlobal()
        {
            await _g
                .V<Person>()
                .Values(x => x.Age)
                .Min()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task MinLocal()
        {
            await _g
                .V<Person>()
                .Values(x => x.Age)
                .MinLocal()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Nested_contradicting_Select_operations_does_not_throw()
        {
            await _g
                .V<Person>()
                .As((__, stepLabel1) => __
                    .As((__, stepLabel2) => __
                        .Select(stepLabel1, stepLabel2)
                        .As((__, tuple) => __
                            .Select(tuple, stepLabel1))))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Nested_Select_operations()
        {
            await _g
                .V<Person>()
                .As((__, stepLabel1) => __
                    .As((__, stepLabel2) => __
                        .Select(stepLabel1, stepLabel2)
                        .As((__, tuple) => __
                            .Select(stepLabel1, tuple))))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Not1()
        {
            await _g
                .V()
                .Not(__ => __.Out<WorksFor>())
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Not2()
        {
            await _g
                .V()
                .Not(__ => __.OfType<Language>())
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Not3()
        {
            await _g
                .V()
                .Not(__ => __.OfType<Authority>())
                .VerifyQuery(this);
        }

        [Fact]
        public async Task OfType_abstract()
        {
            await _g
                .V()
                .OfType<Authority>()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task OfType_redundant1()
        {
            await _g
                .V()
                .OfType<Company>()
                .OfType<Authority>()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task OfType_redundant2()
        {
            await _g
                .V()
                .OfType<Company>()
                .OfType<object>()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task OfType_redundant3()
        {
            await _g
                .V()
                .OfType<Company>()
                .Cast<object>()
                .OfType<Authority>()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task OfType_redundant4()
        {
            await _g
                .V()
                .OfType<Authority>()
                .OfType<Company>()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Optional()
        {
            await _g
                .V()
                .Optional(
                    __ => __.Out<WorksFor>())
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Or()
        {
            await _g
                .V<Person>()
                .Or(
                    __ => __
                        .InE<WorksFor>(),
                    __ => __
                        .OutE<LivesIn>())
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Or_identity()
        {
            await _g
                .V<Person>()
                .Or(
                    __ => __,
                    __ => __
                        .OutE<LivesIn>())
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Or_infix()
        {
            await _g
                .V<Person>()
                .Out()
                .Or()
                .In()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Or_nested()
        {
            await _g
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
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Or_nested_and_optimization()
        {
            await _g
                .V<Person>()
                .Or(
                    __ => __
                        .OutE<LivesIn>(),
                    __ => __
                        .And(
                            __ => __,
                            __ => __))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Or_none()
        {
            await _g
                .V<Person>()
                .Or(
                    __ => __
                        .None(),
                    __ => __
                        .OutE())
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Order_scalars()
        {
            await _g
                .V<Person>()
                .Local(__ => __.Count())
                .Order(b => b
                    .By(__ => __))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Order_scalars_local()
        {
            await _g
                .V<Person>()
                .Local(__ => __.Count())
                .OrderLocal(b => b
                    .By(__ => __))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Order_Fold_Unfold()
        {
            await _g
                .V<IVertex>()
                .Order(b => b
                    .By(x => x.Id))
                .Fold()
                .Unfold()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task OrderBy_lambda()
        {
            await _g
                .V<Person>()
                .Order(b => b
                    .By(Lambda.Groovy("it.property('str').value().length()")))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task OrderBy_member()
        {
            await _g
                .V<Person>()
                .Order(b => b
                    .By(x => x.Name))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task OrderLocal_by_member()
        {
            await _g
                .V<Person>()
                .OrderLocal(b => b
                    .By(x => x.Name))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task OrderBy_member_ThenBy_member()
        {
            await _g
                .V<Person>()
                .Order(b => b
                    .By(x => x.Name)
                    .By(x => x.Age))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task OrderBy_ThenBy_lambda()
        {
            await _g
                .V<Person>()
                .Order(b => b
                    .By(Lambda.Groovy("it.property('str1').value().length()"))
                    .By(Lambda.Groovy("it.property('str2').value().length()")))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task OrderBy_ThenByDescending_member()
        {
            await _g
                .V<Person>()
                .Order(b => b
                    .By(x => x.Name)
                    .ByDescending(x => x.Age))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task OrderBy_ThenByDescending_traversal()
        {
            await _g
                .V<Person>()
                .Order(b => b
                    .By(__ => __.Values(x => x.Name))
                    .ByDescending(__ => __.Gender))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task OrderBy_traversal()
        {
            await _g
                .V<Person>()
                .Order(b => b
                    .By(__ => __.Values(x => x.Name)))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task OrderBy_traversal_ThenBy()
        {
            await _g
                .V<Person>()
                .Order(b => b
                    .By(__ => __.Values(x => x.Name))
                    .By(__ => __.Gender))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task OrderBy_traversal_ThenBy_traversal()
        {
            await _g
                .V<Person>()
                .Order(b => b
                    .By(__ => __.Values(x => x.Name))
                    .By(__ => __.Values(x => x.Gender)))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task OrderByDescending_member()
        {
            await _g
                .V<Person>()
                .Order(b => b
                    .ByDescending(x => x.Name))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task OrderByDescending_traversal()
        {
            await _g
                .V<Person>()
                .Order(b => b
                    .ByDescending(__ => __.Values(x => x.Name)))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Out()
        {
            await _g
                .V<Person>()
                .Out<WorksFor>()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Out_does_not_include_abstract_edge()
        {
            await _g
                .V<Person>()
                .Out<Edge>()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Out_of_all_types_max()
        {
            await _g
                .V()
                .Out<object>()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Out_of_all_types_min()
        {
            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(o => o
                        .SetItem(GremlinqOption.FilterLabelsVerbosity, FilterLabelsVerbosity.Minimum)))
                .V()
                .Out<object>()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task OutE_of_all_types_max()
        {
            await _g
                .V()
                .OutE<object>()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task OutE_of_all_types_min()
        {
            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(o => o
                        .SetItem(GremlinqOption.FilterLabelsVerbosity, FilterLabelsVerbosity.Minimum)))
                .V()
                .OutE<object>()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task OutE_of_no_derived_types()
        {
            await _g
                .V()
                .OutE<string>()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Project_to_property_with_builder()
        {
            await _g
                .V<Person>()
                .Project(_ => _
                    .ToDynamic()
                    .By("in!", __ => __.In())
                    .By(x => x.Age))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Project_with_builder_1()
        {
            await _g
                .V()
                .Project(_ => _
                    .ToDynamic()
                    .By("in!", __ => __.In()))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Project_with_builder_4()
        {
            await _g
                .V()
                .Project(_ => _
                    .ToDynamic()
                    .By("in!", __ => __.In())
                    .By("out!", __ => __.Out())
                    .By("count!", __ => __.Count())
                    .By("properties!", __ => __.Properties()))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Project2()
        {
            await _g
                .V()
                .Project(__ => __
                    .ToTuple()
                    .By(__ => __.In())
                    .By(__ => __.Out()))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Project3()
        {
            await _g
                .V()
                .Project(__ => __
                    .ToTuple()
                    .By(__ => __.In())
                    .By(__ => __.Out())
                    .By(__ => __.Count()))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Project4()
        {
            await _g
                .V()
                .Project(__ => __
                    .ToTuple()
                    .By(__ => __.In())
                    .By(__ => __.Out())
                    .By(__ => __.Count())
                    .By(__ => __.Properties()))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Properties_Meta()
        {
            await _g
                .V<Country>()
                .Properties(x => x.Name)
                .Meta<PropertyValidity>()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Properties_Meta_ValueMap()
        {
            await _g
                .V()
                .Properties()
                .Meta<PropertyValidity>()
                .ValueMap()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Properties_Meta_Values()
        {
            await _g
                .V()
                .Properties()
                .Meta<PropertyValidity>()
                .Values()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Properties_Meta_Values_Projected()
        {
            await _g
                .V()
                .Properties()
                .Meta<PropertyValidity>()
                .Values(x => x.ValidFrom)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Properties_Meta_Where1()
        {
            await _g
                .V<Country>()
                .Properties(x => x.Name)
                .Meta<PropertyValidity>()
                .Where(x => x.Properties.ValidFrom >= DateTimeOffset.Parse("01.01.2019 08:00"))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Properties_of_member()
        {
            await _g
                .V<Country>()
                .Properties(x => x.Name)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Properties_of_three_members()
        {
            await _g
                .V<Country>()
                .Properties(x => x.Name, x => x.CountryCallingCode, x => x.Languages)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Properties_of_two_members1()
        {
            await _g
                .V<Country>()
                .Properties(x => x.Name, x => x.CountryCallingCode)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Properties_of_two_members2()
        {
            await _g
                .V<Country>()
                .Properties(x => x.Name, x => x.Languages)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Properties_Properties_as_select()
        {
            await _g
                .V<Country>()
                .Properties(x => x.Name)
                .Properties()
                .As((__, s) => __
                    .Select(s))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Properties_Properties_key()
        {
            await _g
                .V<Country>()
                .Properties(x => x.Name)
                .Properties()
                .Key()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Properties_Properties_Value()
        {
            await _g
                .V<Company>()
                .Properties(x => x.Names)
                .Properties()
                .Value()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Properties_Properties_Where_key()
        {
            await _g
                .V<Company>()
                .Properties(x => x.Names)
                .Properties()
                .Where(x => x.Key == "someKey")
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Properties_Properties_Where_key_equals_stepLabel()
        {
            await _g
                .Inject("hello")
                .As((__, stepLabel) => __
                    .V<Company>()
                    .Properties(x => x.Names)
                    .Properties()
                    .Where(x => x.Key == stepLabel))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Properties_Properties1()
        {
            await _g
                .V<Country>()
                .Properties(x => x.Name)
                .Properties()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Properties_Properties2()
        {
            await _g
                .V<Company>()
                .Properties(x => x.Names)
                .Properties()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Properties_typed_no_parameters()
        {
            await _g
                .V()
                .Properties<string>()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Properties_Values_typed()
        {
            await _g
                .V()
                .Properties()
                .Values<string>()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Properties_ValueMap_typed()
        {
            await _g
                .V()
                .Properties()
                .ValueMap<string>()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Properties_ValueMap_untyped()
        {
            await _g
                .V()
                .Properties()
                .ValueMap()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Properties_Values_Id()
        {
            await _g
                .V()
                .Properties()
                .Values(x => x.Id)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Properties_Values_Id_Label()
        {
            await _g
                .V()
                .Properties()
                .Values(
                    x => x.Label,
                    x => x.Id)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Properties_Values_Label()
        {
            await _g
                .V()
                .Properties()
                .Values(x => x.Label)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Properties_Values_untyped()
        {
            await _g
                .V()
                .Properties()
                .Values()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Properties_Values2()
        {
            await _g
                .V()
                .Properties()
                .Values<int>("MetaProperty")
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Properties_Where_Dictionary_key1()
        {
            await _g
                .V<Person>()
                .Properties()
#pragma warning disable 252,253
                .Where(x => x.Properties["MetaKey"] == "MetaValue")
#pragma warning restore 252,253
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Properties_Where_Dictionary_key2()
        {
            await _g
                .V<Person>()
                .Properties()
                .Where(x => (int)x.Properties["MetaKey"] < 100)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Properties_Where_Id()
        {
            await _g
                .V<Country>()
                .Properties(x => x.Languages)
#pragma warning disable 252,253
                .Where(x => x.Id == "id")
#pragma warning restore 252,253
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Properties_Where_Id_equals_static_field()
        {
            await _g
                .V<Country>()
                .Properties(x => x.Languages)
#pragma warning disable 252,253
                .Where(x => x.Id == id)
#pragma warning restore 252,253
                .VerifyQuery(this);
        }

        [Fact]
        public async Task VertexProperties_Where_label()
        {
            await _g
                .V<Company>()
                .Properties(x => x.Names)
                .Where(x => x.Label == "someKey")
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Properties_Where_Label_2()
        {
            await _g
                .V<Country>()
                .Properties(x => x.Languages)
                .Where(x => x.Label == "label")
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Properties_Where_Meta_key()
        {
            await _g
                .V<Company>()
                .Properties(x => x.Names)
                .Where(x => x.Properties.ValidFrom == DateTimeOffset.Parse("01.01.2019 08:00"))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Properties_Where_Meta_key_reversed()
        {
            await _g
                .V<Company>()
                .Properties(x => x.Names)
                .Where(x => DateTimeOffset.Parse("01.01.2019 08:00") == x.Properties.ValidFrom)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Properties_Where_reversed()
        {
            await _g
                .V<Country>()
                .Properties(x => x.Languages)
                .Where(x => "de" == x.Value)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Properties_Where1()
        {
            await _g
                .V<Country>()
                .Properties(x => x.Languages)
                .Where(x => x.Value == "de")
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Properties_Where2()
        {
            await _g
                .V<Country>()
                .Properties()
                .Where(x => (int)x.Value < 10)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Properties1()
        {
            await _g
                .V()
                .Properties()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Properties2()
        {
            await _g
                .E()
                .Properties()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Property_list()
        {
            await _g
                .V<Company>("id")
                .Property(x => x.PhoneNumbers, "+4912345")
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Property_null()
        {
            await _g
                .V<Company>("id")
                .Property<string>(x => x.PhoneNumbers, null)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Property_single()
        {
            await _g
                .V<Person>()
                .Property(x => x.Age, 36)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Range_underflow()
        {
            _g
                .V()
                .Invoking(_ => _.Range(-1, 0))
                .Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public async Task RangeGlobal()
        {
            await _g
                .V()
                .Range(1, 3)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task RangeLocal()
        {
            await _g
                .V()
                .RangeLocal(1, 3)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Repeat_Out()
        {
            await _g
                .V<Person>()
                .Repeat(__ => __
                    .Out<WorksFor>()
                    .OfType<Person>())
                .VerifyQuery(this);
        }

        [Fact]
        public async Task RepeatUntil()
        {
            await _g
                .V<Person>()
                .Cast<object>()
                .RepeatUntil(
                    __ => __.InE().OutV().Cast<object>(),
                    __ => __.V<Company>().Cast<object>())
                .VerifyQuery(this);
        }

        [Fact]
        public async Task ReplaceE()
        {
            var now = DateTime.UtcNow;
            var id = Guid.NewGuid();

            var worksFor = new WorksFor { Id = id, From = now, To = now, Role = "Admin" };

            await _g
                .ReplaceE(worksFor)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task ReplaceE_With_Config()
        {
            var now = DateTime.UtcNow;
            var id = Guid.NewGuid();
            var worksFor = new WorksFor { Id = id, From = now, To = now, Role = "Admin" };

            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureModel(model => model
                        .ConfigureProperties(_ => _
                            .ConfigureElement<WorksFor>(conf => conf
                                .IgnoreOnUpdate(p => p.Id)))))
                .ReplaceE(worksFor)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task ReplaceV()
        {
            var now = DateTimeOffset.UtcNow;
            var id = Guid.NewGuid();
            var person = new Person { Id = id, Age = 21, Gender = Gender.Male, Name = "Marko", RegistrationDate = now };

            await _g
                .ReplaceV(person)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task ReplaceV_With_Config()
        {
            var now = DateTimeOffset.UtcNow;
            var id = Guid.NewGuid();
            var person = new Person { Id = id, Age = 21, Gender = Gender.Male, Name = "Marko", RegistrationDate = now };

            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureModel(model => model
                        .ConfigureProperties(_ => _
                            .ConfigureElement<Person>(conf => conf
                                .IgnoreOnUpdate(p => p.RegistrationDate)))))
                .ReplaceV(person)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Set_Meta_Property_to_null()
        {
            await _g
                .V<Country>()
                .Properties(x => x.Name)
                .Property("metaKey", null)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Set_Meta_Property1()
        {
            await _g
                .V<Country>()
                .Properties(x => x.Name)
                .Property("metaKey", 1)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Set_Meta_Property2()
        {
            var d = DateTimeOffset.Now;

            await _g
                .V<Person>()
                .Properties(x => x.Name)
                .Property(x => x.ValidFrom, d)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Skip_underflow()
        {
            _g
                .V()
                .Invoking(_ => _.Skip(-1))
                .Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public async Task SkipGlobal()
        {
            await _g
                .V()
                .Skip(1)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task SkipLocal()
        {
            await _g
                .V()
                .SkipLocal(1)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task StepLabel_of_array_contains_element()
        {
            await _g
                .Inject(1, 2, 3)
                .Fold()
                .As((_, ints) => _
                    .V<Person>()
                    .Where(person => ints.Value.Contains(person.Age)))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task StepLabel_of_array_contains_vertex()
        {
            await _g
                .V()
                .Fold()
                .As((_, v) => _
                    .V<Person>()
                    .Where(person => v.Value.Contains(person)))
                .Count()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task StepLabel_of_array_does_not_contain_vertex()
        {
            await _g
                .V()
                .Fold()
                .As((_, v) => _
                    .V<Person>()
                    .Where(person => !v.Value.Contains(person)))
                .Count()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task StepLabel_of_object_array_contains_element()
        {
            await _g
                .Inject(1, 2, 3)
                .Cast<object>()
                .Fold()
                .As((_, ints) => _
                    .V<Person>()
                    .Where(person => ints.Value.Contains(person.Age)))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task StringKey()
        {
            await _g
                .V<Person>("id")
                .VerifyQuery(this);
        }

        [Fact]
        public async Task SumGlobal()
        {
            await _g
                .V<Person>()
                .Values(x => x.Age)
                .Sum()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task SumLocal()
        {
            await _g
                .V<Person>()
                .Values(x => x.Age)
                .SumLocal()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task SumLocal_Where1()
        {
            await _g
                .V<Person>()
                .Values(x => x.Age)
                .SumLocal()
                .Where(x => x == 100)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task SumLocal_Where2()
        {
            await _g
                .V<Person>()
                .Values(x => x.Age)
                .SumLocal()
                .Where(x => x < 100)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Tail_underflow()
        {
            _g
                .V()
                .Invoking(_ => _.Tail(-1))
                .Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public async Task TailGlobal()
        {
            await _g
                .V()
                .Tail(1)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task TailLocal()
        {
            await _g
                .V()
                .TailLocal(1)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Union()
        {
            await _g
                .V<Person>()
                .Union(
                    __ => __.Out<WorksFor>(),
                    __ => __.Out<LivesIn>())
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Union_different_types()
        {
            await _g
                .V<Person>()
                .Union<IGremlinQueryBase>(
                    __ => __.Out<WorksFor>(),
                    __ => __.OutE<LivesIn>())
                .VerifyQuery(this);
        }


        [Fact]
        public async Task Union_different_types2()
        {
            await _g
                .V<Person>()
                .Union(
                    __ => __.Out<WorksFor>().Lower(),
                    __ => __.OutE<LivesIn>().Lower().Cast<object>())
                .VerifyQuery(this);
        }

        [Fact]
        public async Task UntilRepeat()
        {
            await _g
                .V<Person>()
                .Cast<object>()
                .UntilRepeat(
                    __ => __.InE().OutV().Cast<object>(),
                    __ => __.V<Company>().Cast<object>())
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Update_Vertex_And_Edge_No_Config()
        {
            var now = DateTimeOffset.UtcNow;
            var edgeNow = DateTime.UtcNow;
            var person = new Person { Age = 21, Gender = Gender.Male, Name = "Marko", RegistrationDate = now };
            var worksFor = new WorksFor { From = edgeNow, To = edgeNow, Role = "Admin" };

            await _g
                .V<Person>()
                .Update(person)
                .OutE<WorksFor>()
                .Update(worksFor)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Update_Vertex_And_Edge_With_Config()
        {
            var now = DateTimeOffset.UtcNow;
            var edgeNow = DateTime.UtcNow;
            var person = new Person { Age = 21, Gender = Gender.Male, Name = "Marko", RegistrationDate = now };
            var worksFor = new WorksFor { From = edgeNow, To = edgeNow, Role = "Admin" };

            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureModel(model => model
                        .ConfigureProperties(_ => _
                            .ConfigureElement<Person>(conf => conf
                                .IgnoreOnUpdate(p => p.Age)
                                .IgnoreAlways(p => p.Name))
                        .ConfigureElement<WorksFor>(conf => conf
                            .IgnoreAlways(p => p.From)
                            .IgnoreOnUpdate(p => p.Role)))))
                .V<Person>()
                .Update(person)
                .OutE<WorksFor>()
                .Update(worksFor)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task UpdateE_With_Ignored()
        {
            var now = DateTime.UtcNow;

            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureModel(model => model
                        .ConfigureProperties(_ => _
                            .ConfigureElement<WorksFor>(conf => conf
                                .IgnoreAlways(p => p.From)
                                .IgnoreAlways(p => p.Role)))))
                .E<WorksFor>()
                .Update(new WorksFor { From = now, To = now, Role = "Admin" })
                .VerifyQuery(this);
        }

        [Fact]
        public async Task UpdateE_With_Mixed()
        {
            var now = DateTime.UtcNow;

            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureModel(model => model
                        .ConfigureProperties(_ => _
                            .ConfigureElement<WorksFor>(conf => conf
                                .IgnoreAlways(p => p.From)
                                .IgnoreOnUpdate(p => p.Role)))))
                .E<WorksFor>()
                .Update(new WorksFor { From = now, To = now, Role = "Admin" })
                .VerifyQuery(this);
        }

        [Fact]
        public async Task UpdateE_With_Readonly()
        {
            var now = DateTime.UtcNow;

            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureModel(model => model
                        .ConfigureProperties(_ => _
                            .ConfigureElement<WorksFor>(conf => conf
                                .IgnoreOnUpdate(p => p.From)
                                .IgnoreOnUpdate(p => p.Role)))))
                .E<WorksFor>()
                .Update(new WorksFor { From = now, To = now, Role = "Admin" })
                .VerifyQuery(this);
        }

        [Fact]
        public async Task UpdateV_No_Config()
        {
            var now = DateTimeOffset.UtcNow;

            await _g
                .V<Person>()
                .Update(new Person { Age = 21, Gender = Gender.Male, Name = "Marko", RegistrationDate = now })
                .VerifyQuery(this);
        }

        [Fact]
        public async Task UpdateV_With_Ignored()
        {
            var now = DateTimeOffset.UtcNow;
            var person = new Person { Age = 21, Gender = Gender.Male, Name = "Marko", RegistrationDate = now };

            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureModel(model => model
                        .ConfigureProperties(_ => _
                            .ConfigureElement<Person>(conf => conf
                                .IgnoreAlways(p => p.Age)
                                .IgnoreAlways(p => p.Gender)))))
                .V<Person>()
                .Update(person)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task UpdateV_With_Mixed()
        {
            var now = DateTimeOffset.UtcNow;
            var person = new Person { Age = 21, Gender = Gender.Male, Name = "Marko", RegistrationDate = now };

            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureModel(model => model
                        .ConfigureProperties(_ => _
                            .ConfigureElement<Person>(conf => conf
                                .IgnoreOnUpdate(p => p.Age)
                                .IgnoreAlways(p => p.Gender)))))
                .V<Person>()
                .Update(person)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task UpdateV_With_Readonly()
        {
            var now = DateTimeOffset.UtcNow;
            var person = new Person { Age = 21, Gender = Gender.Male, Name = "Marko", RegistrationDate = now };

            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureModel(model => model
                        .ConfigureProperties(_ => _
                            .ConfigureElement<Person>(conf => conf
                                .IgnoreOnUpdate(p => p.Age)
                                .IgnoreOnUpdate(p => p.Gender)))))
                .V<Person>()
                .Update(person)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task V_Both()
        {
            await _g
                .V()
                .Both<Edge>()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task V_IAuthority()
        {
            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureModel(model => model
                        .ConfigureProperties(_ => _
                            .ConfigureElement<Authority>(__ => __
                                .ConfigureName(x => x.Name, "n")))))
                .V<IAuthority>()
                .Where(x => x.Name.Value == "some name")
                .VerifyQuery(this);
        }

        [Fact]
        public async Task V_of_abstract_type()
        {
            await _g
                .V<Authority>()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task V_of_all_types1()
        {
            await _g
                .V<object>()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task V_of_all_types2()
        {
            await _g
                .V()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task V_of_concrete_type()
        {
            await _g
                .V<Person>()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task V_untyped()
        {
            await _g
                .V()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task V_untyped_without_metaproperties()
        {
            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureFeatureSet(set => set.ConfigureVertexFeatures(features => features & ~VertexFeatures.MetaProperties)))
                .V()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Value()
        {
            await _g
                .V()
                .Properties()
                .Value()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task ValueMap_typed()
        {
            await _g
                .V<Person>()
                .ValueMap(x => x.Age)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Values_1_member()
        {
            await _g
                .V<Person>()
                .Values(x => x.Age)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Values_2_members()
        {
            await _g
                .V<Person>()
                .Values(x => x.Name, x => x.Id)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Values_3_members()
        {
            await _g
                .V<Person>()
                .Values(x => x.Name, x => x.Gender, x => x.Id)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Values_id_member()
        {
            await _g
                .V<Person>()
                .Values(x => x.Id)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Values_no_member()
        {
            await _g
                .V<Person>()
                .Values()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Values_of_Edge()
        {
            await _g
                .E<LivesIn>()
                .Values(x => x.Since)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Values_of_Vertex1()
        {
            await _g
                .V<Person>()
                .Values(x => x.Name)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Values_of_Vertex2()
        {
            await _g
                .V<Person>()
                .Values(x => x.Name)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Variable_wrap()
        {
            await _g
                .V()
                .Properties()
                .Properties("1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30")
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_anonymous()
        {
            await _g
                .V<Person>()
                .Where(_ => _)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_array_does_not_intersect_property_array()
        {
            await _g
                .V<Company>()
                .Where(t => !new[] { "+4912345", "+4923456" }.Intersect(t.PhoneNumbers).Any())
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_array_intersects_property_aray()
        {
            await _g
                .V<Company>()
                .Where(t => new[] { "+4912345", "+4923456" }.Intersect(t.PhoneNumbers).Any())
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_bool_property_explicit_comparison1()
        {
            await _g
                .V<TimeFrame>()
                // ReSharper disable once RedundantBoolCompare
                .Where(t => t.Enabled == true)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_bool_property_explicit_comparison2()
        {
            await _g
                .V<TimeFrame>()
                .Where(t => t.Enabled == false)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_bool_property_implicit_comparison1()
        {
            await _g
                .V<TimeFrame>()
                .Where(t => t.Enabled)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_bool_property_implicit_comparison2()
        {
            await _g
                .V<TimeFrame>()
                .Where(t => !t.Enabled)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_complex_logical_expression()
        {
            await _g
                .V<Person>()
                .Where(t => t.Name.Value == "Some name" && (t.Age == 42 || t.Age == 99))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_complex_logical_expression_with_null()
        {
            await _g
                .V<Person>()
                .Where(t => t.Name == null && (t.Age == 42 || t.Age == 99))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_conjunction()
        {
            await _g
                .V<Person>()
                .Where(t => t.Age == 36 && t.Age == 42)
                .VerifyQuery(this);
        }

        [Fact(Skip="Optimizable")]
        public async Task Where_conjunction_optimizable()
        {
            await _g
                .V<Person>()
                .Where(t => (t.Age == 36 && t.Name.Value == "Hallo") && t.Age == 42)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_conjunction_with_different_fields()
        {
            await _g
                .V<Person>()
                .Where(t => t.Name.Value == "Some name" && t.Age == 42)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_converted_Id_equals_constant()
        {
            await _g
                .V<Language>()
                .Where(t => (int)t.Id == 1)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_current_element_equals_stepLabel1()
        {
            await _g
                .V<Language>()
                .As((__, l) => __
                    .V<Language>()
                    .Where(l2 => l2 == l))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_current_element_equals_stepLabel2()
        {
            await _g
                .V<Language>()
                .As((__, l) => __
                    .V<Language>()
                    .Where(l2 => l == l2))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_current_element_not_equals_stepLabel1()
        {
            await _g
                .V<Language>()
                .As((__, l) => __
                    .V<Language>()
                    .Where(l2 => l2 != l))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_current_element_not_equals_stepLabel2()
        {
            await _g
                .V<Language>()
                .As((__, l) => __
                    .V<Language>()
                    .Where(l2 => l != l2))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_disjunction()
        {
            await _g
                .V<Person>()
                .Where(t => t.Age == 36 || t.Age == 42)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_disjunction_with_different_fields()
        {
            await _g
                .V<Person>()
                .Where(t => t.Name.Value == "Some name" || t.Age == 42)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_empty_array_does_not_intersect_property_array()
        {
            await _g
                .V<Company>()
                .Where(t => !new string[0].Intersect(t.PhoneNumbers).Any())
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_empty_array_intersects_property_array()
        {
            await _g
                .V<Company>()
                .Where(t => new string[0].Intersect(t.PhoneNumbers).Any())
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_has_conjunction_of_three()
        {
            await _g
                .V<Person>()
                .Where(t => t.Age == 36 && t.Age == 42 && t.Age == 99)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_has_disjunction_of_three()
        {
            await _g
                .V<Person>()
                .Where(t => t.Age == 36 || t.Age == 42 || t.Age == 99)
                .VerifyQuery(this);
        }

        [Fact(Skip = "Optimization opportunity.")]
        public async Task Where_has_disjunction_of_three_with_or()
        {
            await _g
                .V<Person>()
                .Or(
                    __ => __.Where(t => t.Age == 36),
                    __ => __.Where(t => t.Age == 42),
                    __ => __.Where(t => t.Age == 99))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_Id_equals_constant()
        {
            await _g
                .V<Language>()
                .Where(t => t.Id == (object)1)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_identity()
        {
            await _g
                .V<Person>()
                .Where(_ => _.Identity())
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_identity_with_type_change()
        {
            await _g
                .V<Person>()
                .Where(_ => _.OfType<Authority>())
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_none_traversal()
        {
            await _g
                .V<Person>()
                .Where(_ => _.None())
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_not_none()
        {
            await _g
                .V<Person>()
                .Where(_ => _
                    .Not(_ => _
                        .None()))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_or_dead_traversal()
        {
            await _g
                .V<Person>()
                .Where(_ => _
                    .Or(_ => _
                        .Where(x => new object[0].Contains(x.Id))))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_or_identity()
        {
            await _g
                .V<Person>()
                .Where(_ => _
                    .Or(_ => _))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_or_none_traversal()
        {
            await _g
                .V<Person>()
                .Where(_ => _
                    .Or(_ => _
                        .None()))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_outside_model()
        {
            await _g
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel.FromBaseTypes<VertexWithStringId, EdgeWithStringId>(lookup => lookup
                        .IncludeAssembliesOfBaseTypes())))
                .V<VertexWithStringId>()
#pragma warning disable 252,253
                .Where(x => x.Id == "hallo")
#pragma warning restore 252,253
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_array_contains_element()
        {
            await _g
                .V<Company>()
                .Where(t => t.PhoneNumbers.Contains("+4912345"))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_array_contains_stepLabel()
        {
            await _g
                .Inject("+4912345")
                .As((__, t) => __
                    .V<Company>()
                    .Where(c => c.PhoneNumbers.Contains(t)))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_array_does_not_contain_element()
        {
            await _g
                .V<Company>()
                .Where(t => !t.PhoneNumbers.Contains("+4912345"))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_array_does_not_intersect_array()
        {
            await _g
                .V<Company>()
                .Where(t => !t.PhoneNumbers.Intersect(new[] { "+4912345", "+4923456" }).Any())
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_array_does_not_intersect_empty_array()
        {
            await _g
                .V<Company>()
                .Where(t => !t.PhoneNumbers.Intersect(new string[0]).Any())
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_array_intersects_array1()
        {
            await _g
                .V<Company>()
                .Where(t => t.PhoneNumbers.Intersect(new[] { "+4912345", "+4923456" }).Any())
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_array_intersects_array2()
        {
            await _g
                .V<Company>()
                .Where(t => new[] { "+4912345", "+4923456" }.Intersect(t.PhoneNumbers).Any())
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_array_intersects_empty_array()
        {
            await _g
                .V<Company>()
                .Where(t => t.PhoneNumbers.Intersect(new string[0]).Any())
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_array_intersects_stepLabel1()
        {
            await _g
                .Inject("+4912345")
                .Fold()
                .As((__, t) => __
                    .V<Company>()
                    .Where(c => c.PhoneNumbers.Intersect(t.Value).Any()))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_array_intersects_stepLabel2()
        {
            await _g
                .Inject("+4912345")
                .Fold()
                .As((__, t) => __
                    .V<Company>()
                    .Where(c => t.Value.Intersect(c.PhoneNumbers).Any()))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_array_is_empty()
        {
            await _g
                .V<Company>()
                .Where(t => !t.PhoneNumbers.Any())
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_array_is_not_empty()
        {
            await _g
                .V<Company>()
                .Where(t => t.PhoneNumbers.Any())
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_contains_constant_with_TextP_support()
        {
            await _g
                .V<Country>()
                .Where(c => c.CountryCallingCode.Contains("456"))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_contains_constant_without_TextP_support()
        {
            _g
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(c => c
                        .SetItem(GremlinqOption.DisabledTextPredicates, DisabledTextPredicates.Containing)))
                .V<Country>()
                .Invoking(_ =>
                    _.Where(c => c.CountryCallingCode.Contains("456")))
                .Should()
                .Throw<ExpressionNotSupportedException>();
        }

        [Fact]
        public async Task Where_property_contains_empty_string_with_TextP_support()
        {
            await _g
                .V<Country>()
                .Where(c => c.CountryCallingCode.Contains(""))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_contains_empty_string_without_TextP_support()
        {
            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(c => c
                        .SetItem(GremlinqOption.DisabledTextPredicates, DisabledTextPredicates.StartingWith)))
                .V<Country>()
                .Where(c => c.CountryCallingCode.Contains(""))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_ends_with_constant_with_TextP_support()
        {
            await _g
                .V<Country>()
                .Where(c => c.CountryCallingCode.EndsWith("7890"))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_ends_with_constant_without_TextP_support()
        {
            _g
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(c => c
                        .SetItem(GremlinqOption.DisabledTextPredicates, DisabledTextPredicates.EndingWith)))
                .V<Country>()
                .Invoking(_ => _
                    .Where(c => c.CountryCallingCode.EndsWith("7890")))
                .Should()
                .Throw<ExpressionNotSupportedException>();
        }

        [Fact]
        public async Task Where_property_ends_with_empty_string_with_TextP_support()
        {
            await _g
                .V<Country>()
                .Where(c => c.CountryCallingCode.EndsWith(""))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_ends_with_empty_string_without_TextP_support()
        {
            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(c => c
                        .SetItem(GremlinqOption.DisabledTextPredicates, DisabledTextPredicates.EndingWith)))
                .V<Country>()
                .Where(c => c.CountryCallingCode.EndsWith(""))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_equals_constant()
        {
            await _g
                .V<Person>()
                .Where(t => t.Age == 36)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_equals_converted_expression()
        {
            await _g
                .V<Person>()
                .Where(t => (object)t.Age == (object)36)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_equals_expression()
        {
            const int i = 18;

            await _g
                .V<Person>()
                .Where(t => t.Age == i + i)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_equals_local_string_constant()
        {
            const int local = 1;

            await _g
                .V<Language>()
                .Where(t => t.Id == (object)local)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_equals_stepLabel()
        {
            await _g
                .V<Language>()
                .Values(x => x.IetfLanguageTag)
                .As((__, l) => __
                    .V<Language>()
                    .Where(l2 => l2.IetfLanguageTag == l))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_equals_value_of_anonymous_object()
        {
            var local = new { Value = 1 };

            await _g
                .V<Language>()
                .Where(t => t.Id == (object)local.Value)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_is_contained_in_array()
        {
            await _g
                .V<Person>()
                .Where(t => new[] { 36, 37, 38 }.Contains(t.Age))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_is_contained_in_empty_enumerable()
        {
            var enumerable = Enumerable.Empty<int>();

            await _g
                .V<Person>()
                .Where(t => enumerable.Contains(t.Age))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_is_contained_in_enumerable()
        {
            var enumerable = new[] { "36", "37", "38" }
                .Select(int.Parse);

            await _g
                .V<Person>()
                .Where(t => enumerable.Contains(t.Age))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_is_greater_or_equal_than_constant()
        {
            await _g
                .V<Person>()
                .Where(t => t.Age >= 36)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_is_greater_than_constant()
        {
            await _g
                .V<Person>()
                .Where(t => t.Age > 36)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_is_greater_than_or_equal_stepLabel()
        {
            await _g
                .V<Person>()
                .Values(x => x.Age)
                .As((__, a) => __
                    .V<Person>()
                    .Where(l2 => l2.Age >= a))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_is_greater_than_or_equal_stepLabel_value()
        {
            await _g
                .V<Person>()
                .As((__, person1) => __
                    .V<Person>()
                    .Where(person2 => person2.Age >= person1.Value.Age))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_is_greater_than_stepLabel()
        {
            await _g
                .V<Person>()
                .Values(x => x.Age)
                .As((__, a) => __
                    .V<Person>()
                    .Where(l2 => l2.Age > a))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_is_lower_or_equal_than_constant()
        {
            await _g
                .V<Person>()
                .Where(t => t.Age <= 36)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_is_lower_than_constant()
        {
            await _g
                .V<Person>()
                .Where(t => t.Age < 36)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_is_lower_than_or_equal_stepLabel()
        {
            await _g
                .V<Person>()
                .Values(x => x.Age)
                .As((__, a) => __
                    .V<Person>()
                    .Where(l2 => l2.Age <= a))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_is_lower_than_stepLabel()
        {
            await _g
                .V<Person>()
                .Values(x => x.Age)
                .As((__, a) => __
                    .V<Person>()
                    .Where(l2 => l2.Age < a))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_is_not_contained_in_array()
        {
            await _g
                .V<Person>()
                .Where(t => !new[] { 36, 37, 38 }.Contains(t.Age))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_is_not_contained_in_empty_enumerable()
        {
            var enumerable = Enumerable.Empty<int>();

            await _g
                .V<Person>()
                .Where(t => !enumerable.Contains(t.Age))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_is_not_contained_in_enumerable()
        {
            var enumerable = new[] { "36", "37", "38" }
                .Select(int.Parse);

            await _g
                .V<Person>()
                .Where(t => !enumerable.Contains(t.Age))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_is_not_present()
        {
            await _g
                .V<Person>()
                .Where(t => t.Name == null)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_is_prefix_of_constant()
        {
            await _g
                .V<Country>()
                .Where(c => "+49123".StartsWith(c.CountryCallingCode))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_is_prefix_of_empty_string()
        {
            await _g
                .V<Country>()
                .Where(c => "".StartsWith(c.CountryCallingCode))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_is_prefix_of_expression()
        {
            const string str = "+49123xxx";

            await _g
                .V<Country>()
                .Where(c => str.Substring(0, 6).StartsWith(c.CountryCallingCode))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_is_prefix_of_variable()
        {
            const string str = "+49123";

            await _g
                .V<Country>()
                .Where(c => str.StartsWith(c.CountryCallingCode))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_is_present()
        {
            await _g
                .V<Person>()
                .Where(t => t.Name != null)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_not_equals_constant()
        {
            await _g
                .V<Person>()
                .Where(t => t.Age != 36)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_starts_with_constant_with_TextP_support()
        {
            await _g
                .V<Country>()
                .Where(c => c.CountryCallingCode.StartsWith("+49123"))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_starts_with_constant_without_TextP_support()
        {
            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(c => c
                        .SetItem(GremlinqOption.DisabledTextPredicates, DisabledTextPredicates.StartingWith)))
                .V<Country>()
                .Where(c => c.CountryCallingCode.StartsWith("+49123"))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_starts_with_empty_string_with_TextP_support()
        {
            await _g
                .V<Country>()
                .Where(c => c.CountryCallingCode.StartsWith(""))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_starts_with_empty_string_without_TextP_support()
        {
            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(c => c
                        .SetItem(GremlinqOption.DisabledTextPredicates, DisabledTextPredicates.StartingWith)))
                .V<Country>()
                .Where(c => c.CountryCallingCode.StartsWith(""))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_property_traversal()
        {
            await _g
                .V<Person>()
                .Where(
                    x => x.Age,
                    _ => _
                        .Inject(36))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_scalar_element_equals_constant()
        {
            await _g
                .V<Person>()
                .Values(x => x.Age)
                .Where(_ => _ == 36)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_sequential()
        {
            await _g
                .V<Person>()
                .Where(t => t.Age == 36)
                .Where(t => t.Age == 42)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_source_expression_on_both_sides1()
        {
            await _g
                .V<Country>()
                .Where(t => t.Name.Value == t.CountryCallingCode)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_source_expression_on_both_sides2()
        {
            await _g
                .V<EntityWithTwoIntProperties>()
                .Where(x => x.IntProperty1 > x.IntProperty2)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_stepLabel_is_lower_than_stepLabel()
        {
            await _g
                .V<Person>()
                .As((__, person1) => __
                    .Values(x => x.Age)
                    .As((__, age1) => __
                        .Select(person1)
                        .Where(_ => _
                            .Out<WorksFor>()
                            .OfType<Person>()
                            .As((__, person2) => __
                                .Values(x => x.Age)
                                .As((__, age2) => __
                                    .Select(person2)
                                    .Where(p => age1 < age2))))))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_stepLabel_value_is_greater_than_or_equal_stepLabel_value()
        {
            await _g
                .V<Person>()
                .As((__, person1) => __
                    .V<Person>()
                    .As((__, person2) => __
                        .Where(_ => person1.Value.Age >= person2.Value.Age)))
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_traversal()
        {
            await _g
                .V<Person>()
                .Where(_ => _.Out<LivesIn>())
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_true()
        {
            await _g
                .V<Person>()
                .Where(_ => true)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_VertexProperty_Value1()
        {
            await _g
                .V<Person>()
                .Where(x => x.Name.Value == "SomeName")
                .VerifyQuery(this);
        }

        [Fact]
        public async Task Where_VertexProperty_Value2()
        {
            await _g
                .V<Person>()
                .Where(x => ((int)(object)x.Name.Value) > 36)
                .VerifyQuery(this);
        }

        [Fact(Skip="Feature!")]
        public async Task Where_VertexProperty_Value3()
        {
            await _g
                .V<Person>()
                .Where(x => (int)x.Name.Id == 36)
                .VerifyQuery(this);
        }

        [Fact]
        public async Task WithoutStrategies1()
        {
            await _g
                .RemoveStrategies(typeof(SubgraphStrategy))
                .V()
                .VerifyQuery(this);
        }

        [Fact]
        public async Task WithoutStrategies2()
        {
            await _g
                .RemoveStrategies(typeof(SubgraphStrategy), typeof(ElementIdStrategy))
                .V()
                .VerifyQuery(this);
        }

        //[Fact(Skip = "Can't handle currently!")]
        //public async Task WithSubgraphStrategy()
        //{
        //    _g
        //        .AddStrategies(new SubgraphQueryStrategy(_ => _.OfType<Person>(), _ => _.OfType<WorksFor>()))
        //        .V()
        //        .VerifyQuery(this);
        //        .SerializeToGroovy("withStrategies(SubgraphStrategy.build().vertices(__.hasLabel(_a)).edges(__.hasLabel(_b)).create()).V()")
        //        .WithParameters("Person", "WorksFor");
        //}

        //[Fact(Skip = "Can't handle currently!")]
        //public async Task WithSubgraphStrategy_empty()
        //{
        //    _g
        //        .AddStrategies(new SubgraphQueryStrategy(_ => _, _ => _))
        //        .V()
        //        .VerifyQuery(this);
        //        .SerializeToGroovy("V()")
        //        .WithoutParameters();
        //}

        //[Fact(Skip = "Can't handle currently!")]
        //public async Task WithSubgraphStrategy_only_edges()
        //{
        //    _g
        //        .AddStrategies(new SubgraphQueryStrategy(_ => _, _ => _.OfType<WorksFor>()))
        //        .V()
        //        .VerifyQuery(this);
        //        .SerializeToGroovy("withStrategies(SubgraphStrategy.build().edges(__.hasLabel(_a)).create()).V()")
        //        .WithParameters("WorksFor");
        //}

        //[Fact(Skip = "Can't handle currently!")]
        //public async Task WithSubgraphStrategy_only_vertices()
        //{
        //    _g
        //        .AddStrategies(new SubgraphQueryStrategy(_ => _.OfType<Person>(), _ => _))
        //        .V()
        //        .VerifyQuery(this);
        //        .SerializeToGroovy("withStrategies(SubgraphStrategy.build().vertices(__.hasLabel(_a)).create()).V()")
        //        .WithParameters("Person");
        //}
    }
}
