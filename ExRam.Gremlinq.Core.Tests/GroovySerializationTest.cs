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
using Verify;
using VerifyXunit;

namespace ExRam.Gremlinq.Core.Tests
{
    public static class GremlinQueryExtensions
    {
        public static void VerifyQuery(this IGremlinQueryBase query, VerifyBase verifyBase)
        {
            var verifySettings = new VerifySettings();
            verifySettings.UseExtension("json");

            var environment = query.AsAdmin().Environment;
            var serializedQuery = environment.Serializer
                .Serialize(query);

            Task.Run(() => verifyBase.Verify(serializedQuery, verifySettings)).Wait();
        }
    }

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
        public void StepLabel_of_array_contains_element_graphson()
        {
            _g
                .Inject(1, 2, 3)
                .Fold()
                .As((_, ints) => _
                    .V<Person>()
                    .Where(person => ints.Value.Contains(person.Age)))
                .VerifyQuery(this);
        }

        [Fact]
        public void AddE_from_StepLabel()
        {
            _g
                .AddV(new Country { CountryCallingCode = "+49" })
                .As((_, c) => _
                    .AddV(new Language { IetfLanguageTag = "en" })
                    .AddE<Speaks>()
                    .From(c))
                .VerifyQuery(this);
        }

        [Fact]
        public void AddE_from_traversal()
        {
            var now = DateTimeOffset.UtcNow;

            _g
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
        public void AddE_InV()
        {
            _g
                .AddV<Person>()
                .AddE<LivesIn>()
                .To(__ => __
                    .V<Country>("id"))
                .InV()
                .VerifyQuery(this);
        }

        [Fact]
        public void AddE_OutV()
        {
            _g
                .AddV<Person>()
                .AddE<LivesIn>()
                .To(__ => __
                    .V<Country>("id"))
                .OutV()
                .VerifyQuery(this);
        }

        [Fact]
        public void AddE_property()
        {
            _g
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
        public void AddE_to_StepLabel()
        {
            _g
                .AddV(new Language { IetfLanguageTag = "en" })
                .As((_, l) => _
                    .AddV(new Country { CountryCallingCode = "+49" })
                    .AddE<Speaks>()
                    .To(l))
                .VerifyQuery(this);
        }

        [Fact]
        public void AddE_to_traversal()
        {
            var now = DateTimeOffset.UtcNow;

            _g
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
        public void AddE_With_Ignored()
        {
            var now = DateTime.UtcNow;

            _g
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
        public void AddV()
        {
            _g
                .AddV(new Language { Id = 1, IetfLanguageTag = "en" })
                .VerifyQuery(this);
        }

        [Fact]
        public void AddV_ignores_label()
        {
            _g
                .AddV(new Language {Label = "Language"})
                .VerifyQuery(this);
        }

        [Fact]
        public void AddV_list_cardinality_id()
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
        public void AddV_with_enum_property()
        {
            _g
                .AddV(new Person { Id = 1, Gender = Gender.Female })
                .VerifyQuery(this);
        }

        [Fact]
        public void AddV_With_Ignored()
        {
            var now = DateTimeOffset.UtcNow;
            var person = new Person { Age = 21, Gender = Gender.Male, Name = "Marko", RegistrationDate = now };

            _g
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
        public void AddV_with_ignored_id_property()
        {
            _g
                .ConfigureEnvironment(env => env
                    .ConfigureModel(model => model
                        .ConfigureProperties(_ => _
                            .ConfigureElement<Language>(conf => conf
                                .IgnoreOnAdd(p => p.Id)))))
                .AddV(new Language { Id = 1, IetfLanguageTag = "en" })
                .VerifyQuery(this);
        }

        [Fact]
        public void AddV_with_ignored_property()
        {
            _g
                .ConfigureEnvironment(env => env
                    .ConfigureModel(model => model
                        .ConfigureProperties(_ => _
                            .ConfigureElement<Language>(conf => conf
                                .IgnoreOnAdd(p => p.IetfLanguageTag)))))
                .AddV(new Language { Id = 1, IetfLanguageTag = "en" })
                .VerifyQuery(this);
        }

        [Fact]
        public void AddV_with_Meta_with_properties()
        {
            _g
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
        public void AddV_with_Meta_without_properties()
        {
            _g
                .AddV(new Country { Id = 1, Name = "GER"})
                .VerifyQuery(this);
        }

        [Fact]
        public void AddV_with_MetaModel()
        {
           _g
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
        public void AddV_with_multi_property()
        {
            _g
                .AddV(new Company { Id = 1, PhoneNumbers = new[] { "+4912345", "+4923456" } })
                .VerifyQuery(this);
        }

        [Fact]
        public void AddV_with_nulls()
        {
            _g
                .AddV(new Language { Id = 1 })
                .VerifyQuery(this);
        }

        [Fact]
        public void AddV_with_overridden_name()
        {
            _g
                .ConfigureEnvironment(env => env
                    .ConfigureModel(model => model
                        .ConfigureProperties(propModel => propModel
                            .ConfigureElement<Language>(conf => conf
                                .ConfigureName(x => x.IetfLanguageTag, "lang")))))
                .AddV(new Language { Id = 1, IetfLanguageTag = "en" })
                .VerifyQuery(this);
        }

        [Fact]
        public void AddV_without_id()
        {
            _g
                .AddV(new Language { IetfLanguageTag = "en" })
                .VerifyQuery(this);
        }

        [Fact]
        public void AddV_without_model()
        {
            _g
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel.Empty))
                .AddV(new Language { Id = 1, IetfLanguageTag = "en" })
                .VerifyQuery(this);
        }

        [Fact]
        public void Aggregate_Global()
        {
            _g
                .V()
                .AggregateGlobal((__, aggregated) => __)
                .VerifyQuery(this);
        }

        [Fact]
        public void Aggregate_Local()
        {
            _g
                .V()
                .Aggregate((__, aggregated) => __)
                .VerifyQuery(this);
        }

        [Fact]
        public void Aggregate_Cap()
        {
            _g
                .V<Person>()
                .Aggregate((__, aggregated) => __
                    .Cap(aggregated))
                .VerifyQuery(this);
        }

        [Fact]
        public void Aggregate_Cap_unfold()
        {
            _g
                .V<Person>()
                .Aggregate((__, aggregated) => __
                    .Cap(aggregated)
                    .Unfold())
                .VerifyQuery(this);
        }

        [Fact]
        public void Aggregate_Cap_type()
        {
            _g
                .V<Person>()
                .Aggregate((__, aggregated) => __
                    .Cap(aggregated))
                .Should()
                .BeAssignableTo<IGremlinQueryBase<Person[]>>();
        }

        [Fact]
        public void And()
        {
            _g
                .V<Person>()
                .And(
                    __ => __
                        .InE<WorksFor>(),
                    __ => __
                        .OutE<LivesIn>())
                .VerifyQuery(this);
        }

        [Fact]
        public void And_identity()
        {
            _g
                .V<Person>()
                .And(
                    __ => __)
                .VerifyQuery(this);
        }

        [Fact]
        public void And_infix()
        {
            _g
                .V<Person>()
                .And()
                .Out()
                .VerifyQuery(this);
        }

        [Fact]
        public void And_nested()
        {
            _g
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
        public void And_nested_or_optimization()
        {
            _g
                .V<Person>()
                .And(
                    __ => __.Or(
                        __ => __),
                    __ => __.Out())
                .VerifyQuery(this);
        }

        [Fact]
        public void And_none()
        {
            _g
                .V<Person>()
                .And(
                    __ => __.None())
                .VerifyQuery(this);
        }

        [Fact]
        public void And_optimization()
        {
            _g
                .V<Person>()
                .And(
                    __ => __,
                    __ => __.Out())
                .VerifyQuery(this);
        }

        [Fact]
        public void As_followed_by_Select()
        {
            _g
                .V<Person>()
                .As((_, stepLabel1) => _
                    .Select(stepLabel1))
                .VerifyQuery(this);
        }

        [Fact]
        public void As_idempotency_is_detected()
        {
            _g
                .V<Person>()
                .As((_, stepLabel1) => _
                    .As((__, stepLabel2) => __
                        .Select(stepLabel1, stepLabel2)))
                .VerifyQuery(this);
        }

        [Fact]
        public void As_inlined_nested_Select()
        {
            _g
                .V<Person>()
                .As((_, stepLabel1) => _
                    .Out()
                    .OfType<Person>()
                    .As((__, stepLabel2) => __
                        .Select(stepLabel1, stepLabel2)))
                .VerifyQuery(this);
        }

        [Fact]
        public void As_inlined_nested_Select2()
        {
            _g
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
        public void As_with_type_change()
        {
            IGremlinQueryBaseRec<Person, IVertexGremlinQuery<Person>> g = _g
                .V<Person>();

            g
                .As((_, stepLabel1) => _
                    .Count()
                    .Select(stepLabel1))
                .VerifyQuery(this);
        }

        [Fact]
        public void Choose_one_case()
        {
            _g
                .V()
                .Choose(_ => _
                    .On(__ => __.Values())
                    .Case(3, __ => __.Constant(1)))
                .VerifyQuery(this);
        }

        [Fact]
        public void Choose_only_default_case()
        {
            _g
                .V()
                .Choose(_ => _
                    .On(__ => __.Values())
                    .Default(__ => __.Constant(1)))
                .VerifyQuery(this);
        }

        [Fact]
        public void Choose_Predicate1()
        {
            _g
                .V()
                .Id()
                .Choose(
                    x => x == (object)42,
                    _ => _.Constant(true),
                    _ => _.Constant(false))
                .VerifyQuery(this);
        }

        [Fact]
        public void Choose_Predicate2()
        {
            _g
                .V()
                .Id()
                .Choose(
                    x => x == (object)42,
                    _ => _.Constant(true))
                .VerifyQuery(this);
        }

        [Fact]
        public void Choose_Predicate3()
        {
            _g
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
        public void Choose_Predicate4()
        {
            _g
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
        public void Choose_Predicate5()
        {
            _g
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
        public void Choose_Predicate6()
        {
            _g
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
        public void Choose_Predicate7()
        {
            _g
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
        public void Choose_Traversal1()
        {
            _g
                .V()
                .Choose(
                    _ => _.Values(),
                    _ => _.Out(),
                    _ => _.In())
                .VerifyQuery(this);
        }

        [Fact]
        public void Choose_Traversal2()
        {
            _g
                .V()
                .Choose(
                    _ => _.Values(),
                    _ => _.Out())
                .VerifyQuery(this);
        }

        [Fact]
        public void Choose_two_cases()
        {
            _g
                .V()
                .Choose(_ => _
                    .On(__ => __.Values())
                    .Case(3, __ => __.Constant(1))
                    .Case(4, __ => __.Constant(2)))
                .VerifyQuery(this);
        }

        [Fact]
        public void Choose_two_cases_default()
        {
            _g
                .V()
                .Choose(_ => _
                    .On(__ => __.Values())
                    .Case(3, __ => __.Constant(1))
                    .Case(4, __ => __.Constant(2))
                    .Default(__ => __.Constant(3)))
                .VerifyQuery(this);
        }

        [Fact]
        public void Coalesce()
        {
            _g
                .V()
                .Coalesce(
                    _ => _
                        .Out())
                .VerifyQuery(this);
        }

        [Fact]
        public void Coalesce_empty()
        {
            _g
                .V()
                .Invoking(__ => __.Coalesce<IGremlinQueryBase>())
                .Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public void Coalesce_identity()
        {
            _g
                .V()
                .Coalesce(
                    _ => _
                        .Identity())
                .VerifyQuery(this);
        }

        [Fact]
        public void Constant()
        {
            _g
                .V()
                .Constant(42)
                .VerifyQuery(this);
        }

        [Fact]
        public void Count()
        {
            _g
                .V()
                .Count()
                .VerifyQuery(this);
        }

        [Fact]
        public void CountGlobal()
        {
            _g
                .V()
                .Count()
                .VerifyQuery(this);
        }

        [Fact]
        public void CountLocal()
        {
            _g
                .V()
                .CountLocal()
                .VerifyQuery(this);
        }

        [Fact]
        public void Dedup_Global()
        {
            _g
                .V()
                .Dedup()
                .VerifyQuery(this);
        }

        [Fact]
        public void Dedup_Local()
        {
            _g
                .V()
                .DedupLocal()
                .VerifyQuery(this);
        }

        [Fact]
        public void Drop()
        {
            _g
                .V<Person>()
                .Drop()
                .VerifyQuery(this);
        }


        [Fact]
        public void Drop_in_local()
        {
            _g
                .Inject(1)
                .Local(__ => __
                    .V()
                    .Drop())
                .VerifyQuery(this);
        }

        [Fact]
        public void E_of_all_types1()
        {
            _g
                .E<object>()
                .VerifyQuery(this);
        }

        [Fact]
        public void E_of_all_types2()
        {
            _g
                .E()
                .VerifyQuery(this);
        }

        [Fact]
        public void E_of_concrete_type()
        {
            _g
                .E<WorksFor>()
                .VerifyQuery(this);
        }

        [Fact]
        public void E_Properties()
        {
            _g
                .E()
                .Properties()
                .VerifyQuery(this);
        }

        [Fact]
        public void E_Properties_member()
        {
            _g
                .E<LivesIn>()
                .Properties(x => x.Since)
                .VerifyQuery(this);
        }

        [Fact]
        public void Explicit_As()
        {
            var stepLabel = new StepLabel<Person>();

            _g
                .V<Person>()
                .As(stepLabel)
                .Select(stepLabel)
                .VerifyQuery(this);
        }

        [Fact]
        public void FilterWithLambda()
        {
            _g
                .V<Person>()
                .Where(Lambda.Groovy("it.property('str').value().length() == 2"))
                .VerifyQuery(this);
        }

        [Fact]
        public void FlatMap()
        {
            _g
                .V<Person>()
                .FlatMap(__ => __.Out<WorksFor>())
                .VerifyQuery(this);
        }

        [Fact]
        public void Fold()
        {
            _g
                .V()
                .Fold()
                .VerifyQuery(this);
        }

        [Fact]
        public void Fold_Fold_Unfold_Unfold()
        {
            _g
                .V()
                .Fold()
                .Fold()
                .Unfold()
                .Unfold()
                .VerifyQuery(this);
        }

        [Fact]
        public void Fold_SideEffect()
        {
            _g
                .V()
                .Fold()
                .SideEffect(x => x.Identity())
                .Unfold()
                .VerifyQuery(this);
        }

        [Fact]
        public void Fold_Unfold()
        {
            _g
                .V()
                .Fold()
                .Unfold()
                .VerifyQuery(this);
        }

        [Fact]
        public void Generic_constraint()
        {
            V2<Person>(_g)
                .VerifyQuery(this);
        }

        [Fact]
        public void Group_with_key()
        {
            _g
                .V()
                .Group(_ => _
                    .ByKey(_ => _.Label()))
                .VerifyQuery(this);
        }

        [Fact]
        public void Identity()
        {
            _g
                .V<Person>()
                .Identity()
                .VerifyQuery(this);
        }

        [Fact]
        public void Identity_Identity()
        {
            _g
                .V<Person>()
                .Identity()
                .Identity()
                .VerifyQuery(this);
        }


        [Fact]
        public void In()
        {
            _g
                .V<Person>()
                .In<WorksFor>()
                .VerifyQuery(this);
        }

        [Fact]
        public void In_of_all_types_max()
        {
            _g
                .V()
                .In<object>()
                .VerifyQuery(this);
        }

        [Fact]
        public void In_of_all_types_min()
        {
            _g
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(o => o
                        .SetItem(GremlinqOption.FilterLabelsVerbosity, FilterLabelsVerbosity.Minimum)))
                .V()
                .In<object>()
                .VerifyQuery(this);
        }

        [Fact]
        public void InE_of_all_types_max()
        {
            _g
                .V()
                .InE<object>()
                .VerifyQuery(this);
        }

        [Fact]
        public void InE_of_all_types_min()
        {
            _g
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(x => x
                        .SetItem(GremlinqOption.FilterLabelsVerbosity, FilterLabelsVerbosity.Minimum)))
                .V()
                .InE<object>()
                .VerifyQuery(this);
        }

        [Fact]
        public void Inject()
        {
            _g
                .Inject(36, 37, 38)
                .VerifyQuery(this);
        }

        [Fact]
        public void Label()
        {
            _g
                .V()
                .Label()
                .VerifyQuery(this);
        }

        [Fact]
        public void Limit_underflow()
        {
            _g
                .V()
                .Invoking(_ => _.Limit(-1))
                .Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public void LimitGlobal()
        {
            _g
                .V()
                .Limit(1)
                .VerifyQuery(this);
        }

        [Fact]
        public void LimitLocal()
        {
            _g
                .V()
                .LimitLocal(1)
                .VerifyQuery(this);
        }

        [Fact]
        public void Local_identity()
        {
            _g
                .V()
                .Local(__ => __)
                .VerifyQuery(this);
        }

        [Fact]
        public void Map()
        {
            _g
                .V<Person>()
                .Map(__ => __.Out<WorksFor>())
                .VerifyQuery(this);
        }

        [Fact]
        public void Map_Identity()
        {
            _g
                .V<Person>()
                .Map(__ => __)
                .VerifyQuery(this);
        }

        [Fact]
        public void Map_Select_operation()
        {
            _g
                .V<Person>()
                .As((_, stepLabel1) => _
                    .As((__, stepLabel2) => __
                        .Map(__ => __
                            .Select(stepLabel1, stepLabel2))))
                .VerifyQuery(this);
        }

        [Fact]
        public void MaxGlobal()
        {
            _g
                .V<Person>()
                .Values(x => x.Age)
                .Max()
                .VerifyQuery(this);
        }

        [Fact]
        public void MaxLocal()
        {
            _g
                .V<Person>()
                .Values(x => x.Age)
                .MaxLocal()
                .VerifyQuery(this);
        }

        [Fact]
        public void MeanGlobal()
        {
            _g
                .V<Person>()
                .Values(x => x.Age)
                .Mean()
                .VerifyQuery(this);
        }

        [Fact]
        public void MeanLocal()
        {
            _g
                .V<Person>()
                .Values(x => x.Age)
                .MeanLocal()
                .VerifyQuery(this);
        }

        [Fact]
        public void MinGlobal()
        {
            _g
                .V<Person>()
                .Values(x => x.Age)
                .Min()
                .VerifyQuery(this);
        }

        [Fact]
        public void MinLocal()
        {
            _g
                .V<Person>()
                .Values(x => x.Age)
                .MinLocal()
                .VerifyQuery(this);
        }

        [Fact]
        public void Nested_contradicting_Select_operations_does_not_throw()
        {
            _g
                .V<Person>()
                .As((__, stepLabel1) => __
                    .As((__, stepLabel2) => __
                        .Select(stepLabel1, stepLabel2)
                        .As((__, tuple) => __
                            .Select(tuple, stepLabel1))))
                .VerifyQuery(this);
        }

        [Fact]
        public void Nested_Select_operations()
        {
            _g
                .V<Person>()
                .As((__, stepLabel1) => __
                    .As((__, stepLabel2) => __
                        .Select(stepLabel1, stepLabel2)
                        .As((__, tuple) => __
                            .Select(stepLabel1, tuple))))
                .VerifyQuery(this);
        }

        [Fact]
        public void Not1()
        {
            _g
                .V()
                .Not(__ => __.Out<WorksFor>())
                .VerifyQuery(this);
        }

        [Fact]
        public void Not2()
        {
            _g
                .V()
                .Not(__ => __.OfType<Language>())
                .VerifyQuery(this);
        }

        [Fact]
        public void Not3()
        {
            _g
                .V()
                .Not(__ => __.OfType<Authority>())
                .VerifyQuery(this);
        }

        [Fact]
        public void OfType_abstract()
        {
            _g
                .V()
                .OfType<Authority>()
                .VerifyQuery(this);
        }

        [Fact]
        public void OfType_redundant1()
        {
            _g
                .V()
                .OfType<Company>()
                .OfType<Authority>()
                .VerifyQuery(this);
        }

        [Fact]
        public void OfType_redundant2()
        {
            _g
                .V()
                .OfType<Company>()
                .OfType<object>()
                .VerifyQuery(this);
        }

        [Fact]
        public void OfType_redundant3()
        {
            _g
                .V()
                .OfType<Company>()
                .Cast<object>()
                .OfType<Authority>()
                .VerifyQuery(this);
        }

        [Fact]
        public void OfType_redundant4()
        {
            _g
                .V()
                .OfType<Authority>()
                .OfType<Company>()
                .VerifyQuery(this);
        }

        [Fact]
        public void Optional()
        {
            _g
                .V()
                .Optional(
                    __ => __.Out<WorksFor>())
                .VerifyQuery(this);
        }

        [Fact]
        public void Or()
        {
            _g
                .V<Person>()
                .Or(
                    __ => __
                        .InE<WorksFor>(),
                    __ => __
                        .OutE<LivesIn>())
                .VerifyQuery(this);
        }

        [Fact]
        public void Or_identity()
        {
            _g
                .V<Person>()
                .Or(
                    __ => __,
                    __ => __
                        .OutE<LivesIn>())
                .VerifyQuery(this);
        }

        [Fact]
        public void Or_infix()
        {
            _g
                .V<Person>()
                .Out()
                .Or()
                .In()
                .VerifyQuery(this);
        }

        [Fact]
        public void Or_nested()
        {
            _g
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
        public void Or_nested_and_optimization()
        {
            _g
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
        public void Or_none()
        {
            _g
                .V<Person>()
                .Or(
                    __ => __
                        .None(),
                    __ => __
                        .OutE())
                .VerifyQuery(this);
        }

        [Fact]
        public void Order_scalars()
        {
            _g
                .V<Person>()
                .Local(__ => __.Count())
                .Order(b => b
                    .By(__ => __))
                .VerifyQuery(this);
        }

        [Fact]
        public void Order_scalars_local()
        {
            _g
                .V<Person>()
                .Local(__ => __.Count())
                .OrderLocal(b => b
                    .By(__ => __))
                .VerifyQuery(this);
        }

        [Fact]
        public void Order_Fold_Unfold()
        {
            _g
                .V<IVertex>()
                .Order(b => b
                    .By(x => x.Id))
                .Fold()
                .Unfold()
                .VerifyQuery(this);
        }

        [Fact]
        public void OrderBy_lambda()
        {
            _g
                .V<Person>()
                .Order(b => b
                    .By(Lambda.Groovy("it.property('str').value().length()")))
                .VerifyQuery(this);
        }

        [Fact]
        public void OrderBy_member()
        {
            _g
                .V<Person>()
                .Order(b => b
                    .By(x => x.Name))
                .VerifyQuery(this);
        }

        [Fact]
        public void OrderLocal_by_member()
        {
            _g
                .V<Person>()
                .OrderLocal(b => b
                    .By(x => x.Name))
                .VerifyQuery(this);
        }

        [Fact]
        public void OrderBy_member_ThenBy_member()
        {
            _g
                .V<Person>()
                .Order(b => b
                    .By(x => x.Name)
                    .By(x => x.Age))
                .VerifyQuery(this);
        }

        [Fact]
        public void OrderBy_ThenBy_lambda()
        {
            _g
                .V<Person>()
                .Order(b => b
                    .By(Lambda.Groovy("it.property('str1').value().length()"))
                    .By(Lambda.Groovy("it.property('str2').value().length()")))
                .VerifyQuery(this);
        }

        [Fact]
        public void OrderBy_ThenByDescending_member()
        {
            _g
                .V<Person>()
                .Order(b => b
                    .By(x => x.Name)
                    .ByDescending(x => x.Age))
                .VerifyQuery(this);
        }

        [Fact]
        public void OrderBy_ThenByDescending_traversal()
        {
            _g
                .V<Person>()
                .Order(b => b
                    .By(__ => __.Values(x => x.Name))
                    .ByDescending(__ => __.Gender))
                .VerifyQuery(this);
        }

        [Fact]
        public void OrderBy_traversal()
        {
            _g
                .V<Person>()
                .Order(b => b
                    .By(__ => __.Values(x => x.Name)))
                .VerifyQuery(this);
        }

        [Fact]
        public void OrderBy_traversal_ThenBy()
        {
            _g
                .V<Person>()
                .Order(b => b
                    .By(__ => __.Values(x => x.Name))
                    .By(__ => __.Gender))
                .VerifyQuery(this);
        }

        [Fact]
        public void OrderBy_traversal_ThenBy_traversal()
        {
            _g
                .V<Person>()
                .Order(b => b
                    .By(__ => __.Values(x => x.Name))
                    .By(__ => __.Values(x => x.Gender)))
                .VerifyQuery(this);
        }

        [Fact]
        public void OrderByDescending_member()
        {
            _g
                .V<Person>()
                .Order(b => b
                    .ByDescending(x => x.Name))
                .VerifyQuery(this);
        }

        [Fact]
        public void OrderByDescending_traversal()
        {
            _g
                .V<Person>()
                .Order(b => b
                    .ByDescending(__ => __.Values(x => x.Name)))
                .VerifyQuery(this);
        }

        [Fact]
        public void Out()
        {
            _g
                .V<Person>()
                .Out<WorksFor>()
                .VerifyQuery(this);
        }

        [Fact]
        public void Out_does_not_include_abstract_edge()
        {
            _g
                .V<Person>()
                .Out<Edge>()
                .VerifyQuery(this);
        }

        [Fact]
        public void Out_of_all_types_max()
        {
            _g
                .V()
                .Out<object>()
                .VerifyQuery(this);
        }

        [Fact]
        public void Out_of_all_types_min()
        {
            _g
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(o => o
                        .SetItem(GremlinqOption.FilterLabelsVerbosity, FilterLabelsVerbosity.Minimum)))
                .V()
                .Out<object>()
                .VerifyQuery(this);
        }

        [Fact]
        public void OutE_of_all_types_max()
        {
            _g
                .V()
                .OutE<object>()
                .VerifyQuery(this);
        }

        [Fact]
        public void OutE_of_all_types_min()
        {
            _g
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(o => o
                        .SetItem(GremlinqOption.FilterLabelsVerbosity, FilterLabelsVerbosity.Minimum)))
                .V()
                .OutE<object>()
                .VerifyQuery(this);
        }

        [Fact]
        public void OutE_of_no_derived_types()
        {
            _g
                .V()
                .OutE<string>()
                .VerifyQuery(this);
        }

        [Fact]
        public void Project_to_property_with_builder()
        {
            _g
                .V<Person>()
                .Project(_ => _
                    .ToDynamic()
                    .By("in!", __ => __.In())
                    .By(x => x.Age))
                .VerifyQuery(this);
        }

        [Fact]
        public void Project_with_builder_1()
        {
            _g
                .V()
                .Project(_ => _
                    .ToDynamic()
                    .By("in!", __ => __.In()))
                .VerifyQuery(this);
        }

        [Fact]
        public void Project_with_builder_4()
        {
            _g
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
        public void Project2()
        {
            _g
                .V()
                .Project(__ => __
                    .ToTuple()
                    .By(__ => __.In())
                    .By(__ => __.Out()))
                .VerifyQuery(this);
        }

        [Fact]
        public void Project3()
        {
            _g
                .V()
                .Project(__ => __
                    .ToTuple()
                    .By(__ => __.In())
                    .By(__ => __.Out())
                    .By(__ => __.Count()))
                .VerifyQuery(this);
        }

        [Fact]
        public void Project4()
        {
            _g
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
        public void Properties_Meta()
        {
            _g
                .V<Country>()
                .Properties(x => x.Name)
                .Meta<PropertyValidity>()
                .VerifyQuery(this);
        }

        [Fact]
        public void Properties_Meta_ValueMap()
        {
            _g
                .V()
                .Properties()
                .Meta<PropertyValidity>()
                .ValueMap()
                .VerifyQuery(this);
        }

        [Fact]
        public void Properties_Meta_Values()
        {
            _g
                .V()
                .Properties()
                .Meta<PropertyValidity>()
                .Values()
                .VerifyQuery(this);
        }

        [Fact]
        public void Properties_Meta_Values_Projected()
        {
            _g
                .V()
                .Properties()
                .Meta<PropertyValidity>()
                .Values(x => x.ValidFrom)
                .VerifyQuery(this);
        }

        [Fact]
        public void Properties_Meta_Where1()
        {
            _g
                .V<Country>()
                .Properties(x => x.Name)
                .Meta<PropertyValidity>()
                .Where(x => x.Properties.ValidFrom >= DateTimeOffset.Parse("01.01.2019 08:00"))
                .VerifyQuery(this);
        }

        [Fact]
        public void Properties_of_member()
        {
            _g
                .V<Country>()
                .Properties(x => x.Name)
                .VerifyQuery(this);
        }

        [Fact]
        public void Properties_of_three_members()
        {
            _g
                .V<Country>()
                .Properties(x => x.Name, x => x.CountryCallingCode, x => x.Languages)
                .VerifyQuery(this);
        }

        [Fact]
        public void Properties_of_two_members1()
        {
            _g
                .V<Country>()
                .Properties(x => x.Name, x => x.CountryCallingCode)
                .VerifyQuery(this);
        }

        [Fact]
        public void Properties_of_two_members2()
        {
            _g
                .V<Country>()
                .Properties(x => x.Name, x => x.Languages)
                .VerifyQuery(this);
        }

        [Fact]
        public void Properties_Properties_as_select()
        {
            _g
                .V<Country>()
                .Properties(x => x.Name)
                .Properties()
                .As((__, s) => __
                    .Select(s))
                .VerifyQuery(this);
        }

        [Fact]
        public void Properties_Properties_key()
        {
            _g
                .V<Country>()
                .Properties(x => x.Name)
                .Properties()
                .Key()
                .VerifyQuery(this);
        }

        [Fact]
        public void Properties_Properties_Value()
        {
            _g
                .V<Company>()
                .Properties(x => x.Names)
                .Properties()
                .Value()
                .VerifyQuery(this);
        }

        [Fact]
        public void Properties_Properties_Where_key()
        {
            _g
                .V<Company>()
                .Properties(x => x.Names)
                .Properties()
                .Where(x => x.Key == "someKey")
                .VerifyQuery(this);
        }

        [Fact]
        public void Properties_Properties_Where_key_equals_stepLabel()
        {
            _g
                .Inject("hello")
                .As((__, stepLabel) => __
                    .V<Company>()
                    .Properties(x => x.Names)
                    .Properties()
                    .Where(x => x.Key == stepLabel))
                .VerifyQuery(this);
        }

        [Fact]
        public void Properties_Properties1()
        {
            _g
                .V<Country>()
                .Properties(x => x.Name)
                .Properties()
                .VerifyQuery(this);
        }

        [Fact]
        public void Properties_Properties2()
        {
            _g
                .V<Company>()
                .Properties(x => x.Names)
                .Properties()
                .VerifyQuery(this);
        }

        [Fact]
        public void Properties_typed_no_parameters()
        {
            _g
                .V()
                .Properties<string>()
                .VerifyQuery(this);
        }

        [Fact]
        public void Properties_Values_typed()
        {
            _g
                .V()
                .Properties()
                .Values<string>()
                .VerifyQuery(this);
        }

        [Fact]
        public void Properties_ValueMap_typed()
        {
            _g
                .V()
                .Properties()
                .ValueMap<string>()
                .VerifyQuery(this);
        }

        [Fact]
        public void Properties_ValueMap_untyped()
        {
            _g
                .V()
                .Properties()
                .ValueMap()
                .VerifyQuery(this);
        }

        [Fact]
        public void Properties_Values_Id()
        {
            _g
                .V()
                .Properties()
                .Values(x => x.Id)
                .VerifyQuery(this);
        }

        [Fact]
        public void Properties_Values_Id_Label()
        {
            _g
                .V()
                .Properties()
                .Values(
                    x => x.Label,
                    x => x.Id)
                .VerifyQuery(this);
        }

        [Fact]
        public void Properties_Values_Label()
        {
            _g
                .V()
                .Properties()
                .Values(x => x.Label)
                .VerifyQuery(this);
        }

        [Fact]
        public void Properties_Values_untyped()
        {
            _g
                .V()
                .Properties()
                .Values()
                .VerifyQuery(this);
        }

        [Fact]
        public void Properties_Values2()
        {
            _g
                .V()
                .Properties()
                .Values<int>("MetaProperty")
                .VerifyQuery(this);
        }

        [Fact]
        public void Properties_Where_Dictionary_key1()
        {
            _g
                .V<Person>()
                .Properties()
#pragma warning disable 252,253
                .Where(x => x.Properties["MetaKey"] == "MetaValue")
#pragma warning restore 252,253
                .VerifyQuery(this);
        }

        [Fact]
        public void Properties_Where_Dictionary_key2()
        {
            _g
                .V<Person>()
                .Properties()
                .Where(x => (int)x.Properties["MetaKey"] < 100)
                .VerifyQuery(this);
        }

        [Fact]
        public void Properties_Where_Id()
        {
            _g
                .V<Country>()
                .Properties(x => x.Languages)
#pragma warning disable 252,253
                .Where(x => x.Id == "id")
#pragma warning restore 252,253
                .VerifyQuery(this);
        }

        [Fact]
        public void Properties_Where_Id_equals_static_field()
        {
            _g
                .V<Country>()
                .Properties(x => x.Languages)
#pragma warning disable 252,253
                .Where(x => x.Id == id)
#pragma warning restore 252,253
                .VerifyQuery(this);
        }

        [Fact]
        public void VertexProperties_Where_label()
        {
            _g
                .V<Company>()
                .Properties(x => x.Names)
                .Where(x => x.Label == "someKey")
                .VerifyQuery(this);
        }

        [Fact]
        public void Properties_Where_Label_2()
        {
            _g
                .V<Country>()
                .Properties(x => x.Languages)
                .Where(x => x.Label == "label")
                .VerifyQuery(this);
        }

        [Fact]
        public void Properties_Where_Meta_key()
        {
            _g
                .V<Company>()
                .Properties(x => x.Names)
                .Where(x => x.Properties.ValidFrom == DateTimeOffset.Parse("01.01.2019 08:00"))
                .VerifyQuery(this);
        }

        [Fact]
        public void Properties_Where_Meta_key_reversed()
        {
            _g
                .V<Company>()
                .Properties(x => x.Names)
                .Where(x => DateTimeOffset.Parse("01.01.2019 08:00") == x.Properties.ValidFrom)
                .VerifyQuery(this);
        }

        [Fact]
        public void Properties_Where_reversed()
        {
            _g
                .V<Country>()
                .Properties(x => x.Languages)
                .Where(x => "de" == x.Value)
                .VerifyQuery(this);
        }

        [Fact]
        public void Properties_Where1()
        {
            _g
                .V<Country>()
                .Properties(x => x.Languages)
                .Where(x => x.Value == "de")
                .VerifyQuery(this);
        }

        [Fact]
        public void Properties_Where2()
        {
            _g
                .V<Country>()
                .Properties()
                .Where(x => (int)x.Value < 10)
                .VerifyQuery(this);
        }

        [Fact]
        public void Properties1()
        {
            _g
                .V()
                .Properties()
                .VerifyQuery(this);
        }

        [Fact]
        public void Properties2()
        {
            _g
                .E()
                .Properties()
                .VerifyQuery(this);
        }

        [Fact]
        public void Property_list()
        {
            _g
                .V<Company>("id")
                .Property(x => x.PhoneNumbers, "+4912345")
                .VerifyQuery(this);
        }

        [Fact]
        public void Property_null()
        {
            _g
                .V<Company>("id")
                .Property<string>(x => x.PhoneNumbers, null)
                .VerifyQuery(this);
        }

        [Fact]
        public void Property_single()
        {
            _g
                .V<Person>()
                .Property(x => x.Age, 36)
                .VerifyQuery(this);
        }

        [Fact]
        public void Range_underflow()
        {
            _g
                .V()
                .Invoking(_ => _.Range(-1, 0))
                .Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public void RangeGlobal()
        {
            _g
                .V()
                .Range(1, 3)
                .VerifyQuery(this);
        }

        [Fact]
        public void RangeLocal()
        {
            _g
                .V()
                .RangeLocal(1, 3)
                .VerifyQuery(this);
        }

        [Fact]
        public void Repeat_Out()
        {
            _g
                .V<Person>()
                .Repeat(__ => __
                    .Out<WorksFor>()
                    .OfType<Person>())
                .VerifyQuery(this);
        }

        [Fact]
        public void RepeatUntil()
        {
            _g
                .V<Person>()
                .Cast<object>()
                .RepeatUntil(
                    __ => __.InE().OutV().Cast<object>(),
                    __ => __.V<Company>().Cast<object>())
                .VerifyQuery(this);
        }

        [Fact]
        public void ReplaceE()
        {
            var now = DateTime.UtcNow;
            var id = Guid.NewGuid();

            var worksFor = new WorksFor { Id = id, From = now, To = now, Role = "Admin" };

            _g
                .ReplaceE(worksFor)
                .VerifyQuery(this);
        }

        [Fact]
        public void ReplaceE_With_Config()
        {
            var now = DateTime.UtcNow;
            var id = Guid.NewGuid();
            var worksFor = new WorksFor { Id = id, From = now, To = now, Role = "Admin" };

            _g
                .ConfigureEnvironment(env => env
                    .ConfigureModel(model => model
                        .ConfigureProperties(_ => _
                            .ConfigureElement<WorksFor>(conf => conf
                                .IgnoreOnUpdate(p => p.Id)))))
                .ReplaceE(worksFor)
                .VerifyQuery(this);
        }

        [Fact]
        public void ReplaceV()
        {
            var now = DateTimeOffset.UtcNow;
            var id = Guid.NewGuid();
            var person = new Person { Id = id, Age = 21, Gender = Gender.Male, Name = "Marko", RegistrationDate = now };

            _g
                .ReplaceV(person)
                .VerifyQuery(this);
        }

        [Fact]
        public void ReplaceV_With_Config()
        {
            var now = DateTimeOffset.UtcNow;
            var id = Guid.NewGuid();
            var person = new Person { Id = id, Age = 21, Gender = Gender.Male, Name = "Marko", RegistrationDate = now };

            _g
                .ConfigureEnvironment(env => env
                    .ConfigureModel(model => model
                        .ConfigureProperties(_ => _
                            .ConfigureElement<Person>(conf => conf
                                .IgnoreOnUpdate(p => p.RegistrationDate)))))
                .ReplaceV(person)
                .VerifyQuery(this);
        }

        [Fact]
        public void Set_Meta_Property_to_null()
        {
            _g
                .V<Country>()
                .Properties(x => x.Name)
                .Property("metaKey", null)
                .VerifyQuery(this);
        }

        [Fact]
        public void Set_Meta_Property1()
        {
            _g
                .V<Country>()
                .Properties(x => x.Name)
                .Property("metaKey", 1)
                .VerifyQuery(this);
        }

        [Fact]
        public void Set_Meta_Property2()
        {
            var d = DateTimeOffset.Now;

            _g
                .V<Person>()
                .Properties(x => x.Name)
                .Property(x => x.ValidFrom, d)
                .VerifyQuery(this);
        }

        [Fact]
        public void Skip_underflow()
        {
            _g
                .V()
                .Invoking(_ => _.Skip(-1))
                .Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public void SkipGlobal()
        {
            _g
                .V()
                .Skip(1)
                .VerifyQuery(this);
        }

        [Fact]
        public void SkipLocal()
        {
            _g
                .V()
                .SkipLocal(1)
                .VerifyQuery(this);
        }

        [Fact]
        public void StepLabel_of_array_contains_element()
        {
            _g
                .Inject(1, 2, 3)
                .Fold()
                .As((_, ints) => _
                    .V<Person>()
                    .Where(person => ints.Value.Contains(person.Age)))
                .VerifyQuery(this);
        }

        [Fact]
        public void StepLabel_of_array_contains_vertex()
        {
            _g
                .V()
                .Fold()
                .As((_, v) => _
                    .V<Person>()
                    .Where(person => v.Value.Contains(person)))
                .Count()
                .VerifyQuery(this);
        }

        [Fact]
        public void StepLabel_of_array_does_not_contain_vertex()
        {
            _g
                .V()
                .Fold()
                .As((_, v) => _
                    .V<Person>()
                    .Where(person => !v.Value.Contains(person)))
                .Count()
                .VerifyQuery(this);
        }

        [Fact]
        public void StepLabel_of_object_array_contains_element()
        {
            _g
                .Inject(1, 2, 3)
                .Cast<object>()
                .Fold()
                .As((_, ints) => _
                    .V<Person>()
                    .Where(person => ints.Value.Contains(person.Age)))
                .VerifyQuery(this);
        }

        [Fact]
        public void StringKey()
        {
            _g
                .V<Person>("id")
                .VerifyQuery(this);
        }

        [Fact]
        public void SumGlobal()
        {
            _g
                .V<Person>()
                .Values(x => x.Age)
                .Sum()
                .VerifyQuery(this);
        }

        [Fact]
        public void SumLocal()
        {
            _g
                .V<Person>()
                .Values(x => x.Age)
                .SumLocal()
                .VerifyQuery(this);
        }

        [Fact]
        public void SumLocal_Where1()
        {
            _g
                .V<Person>()
                .Values(x => x.Age)
                .SumLocal()
                .Where(x => x == 100)
                .VerifyQuery(this);
        }

        [Fact]
        public void SumLocal_Where2()
        {
            _g
                .V<Person>()
                .Values(x => x.Age)
                .SumLocal()
                .Where(x => x < 100)
                .VerifyQuery(this);
        }

        [Fact]
        public void Tail_underflow()
        {
            _g
                .V()
                .Invoking(_ => _.Tail(-1))
                .Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public void TailGlobal()
        {
            _g
                .V()
                .Tail(1)
                .VerifyQuery(this);
        }

        [Fact]
        public void TailLocal()
        {
            _g
                .V()
                .TailLocal(1)
                .VerifyQuery(this);
        }

        [Fact]
        public void Union()
        {
            _g
                .V<Person>()
                .Union(
                    __ => __.Out<WorksFor>(),
                    __ => __.Out<LivesIn>())
                .VerifyQuery(this);
        }

        [Fact]
        public void Union_different_types()
        {
            _g
                .V<Person>()
                .Union<IGremlinQueryBase>(
                    __ => __.Out<WorksFor>(),
                    __ => __.OutE<LivesIn>())
                .VerifyQuery(this);
        }


        [Fact]
        public void Union_different_types2()
        {
            _g
                .V<Person>()
                .Union(
                    __ => __.Out<WorksFor>().Lower(),
                    __ => __.OutE<LivesIn>().Lower().Cast<object>())
                .VerifyQuery(this);
        }

        [Fact]
        public void UntilRepeat()
        {
            _g
                .V<Person>()
                .Cast<object>()
                .UntilRepeat(
                    __ => __.InE().OutV().Cast<object>(),
                    __ => __.V<Company>().Cast<object>())
                .VerifyQuery(this);
        }

        [Fact]
        public void Update_Vertex_And_Edge_No_Config()
        {
            var now = DateTimeOffset.UtcNow;
            var edgeNow = DateTime.UtcNow;
            var person = new Person { Age = 21, Gender = Gender.Male, Name = "Marko", RegistrationDate = now };
            var worksFor = new WorksFor { From = edgeNow, To = edgeNow, Role = "Admin" };

            _g
                .V<Person>()
                .Update(person)
                .OutE<WorksFor>()
                .Update(worksFor)
                .VerifyQuery(this);
        }

        [Fact]
        public void Update_Vertex_And_Edge_With_Config()
        {
            var now = DateTimeOffset.UtcNow;
            var edgeNow = DateTime.UtcNow;
            var person = new Person { Age = 21, Gender = Gender.Male, Name = "Marko", RegistrationDate = now };
            var worksFor = new WorksFor { From = edgeNow, To = edgeNow, Role = "Admin" };

            _g
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
        public void UpdateE_With_Ignored()
        {
            var now = DateTime.UtcNow;

            _g
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
        public void UpdateE_With_Mixed()
        {
            var now = DateTime.UtcNow;

            _g
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
        public void UpdateE_With_Readonly()
        {
            var now = DateTime.UtcNow;

            _g
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
        public void UpdateV_No_Config()
        {
            var now = DateTimeOffset.UtcNow;

            _g
                .V<Person>()
                .Update(new Person { Age = 21, Gender = Gender.Male, Name = "Marko", RegistrationDate = now })
                .VerifyQuery(this);
        }

        [Fact]
        public void UpdateV_With_Ignored()
        {
            var now = DateTimeOffset.UtcNow;
            var person = new Person { Age = 21, Gender = Gender.Male, Name = "Marko", RegistrationDate = now };

            _g
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
        public void UpdateV_With_Mixed()
        {
            var now = DateTimeOffset.UtcNow;
            var person = new Person { Age = 21, Gender = Gender.Male, Name = "Marko", RegistrationDate = now };

            _g
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
        public void UpdateV_With_Readonly()
        {
            var now = DateTimeOffset.UtcNow;
            var person = new Person { Age = 21, Gender = Gender.Male, Name = "Marko", RegistrationDate = now };

            _g
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
        public void V_Both()
        {
            _g
                .V()
                .Both<Edge>()
                .VerifyQuery(this);
        }

        [Fact]
        public void V_IAuthority()
        {
            _g
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
        public void V_of_abstract_type()
        {
            _g
                .V<Authority>()
                .VerifyQuery(this);
        }

        [Fact]
        public void V_of_all_types1()
        {
            _g
                .V<object>()
                .VerifyQuery(this);
        }

        [Fact]
        public void V_of_all_types2()
        {
            _g
                .V()
                .VerifyQuery(this);
        }

        [Fact]
        public void V_of_concrete_type()
        {
            _g
                .V<Person>()
                .VerifyQuery(this);
        }

        [Fact]
        public void V_untyped()
        {
            _g
                .V()
                .VerifyQuery(this);
        }

        [Fact]
        public void V_untyped_without_metaproperties()
        {
            _g
                .ConfigureEnvironment(env => env
                    .ConfigureFeatureSet(set => set.ConfigureVertexFeatures(features => features & ~VertexFeatures.MetaProperties)))
                .V()
                .VerifyQuery(this);
        }

        [Fact]
        public void Value()
        {
            _g
                .V()
                .Properties()
                .Value()
                .VerifyQuery(this);
        }

        [Fact]
        public void ValueMap_typed()
        {
            _g
                .V<Person>()
                .ValueMap(x => x.Age)
                .VerifyQuery(this);
        }

        [Fact]
        public void Values_1_member()
        {
            _g
                .V<Person>()
                .Values(x => x.Age)
                .VerifyQuery(this);
        }

        [Fact]
        public void Values_2_members()
        {
            _g
                .V<Person>()
                .Values(x => x.Name, x => x.Id)
                .VerifyQuery(this);
        }

        [Fact]
        public void Values_3_members()
        {
            _g
                .V<Person>()
                .Values(x => x.Name, x => x.Gender, x => x.Id)
                .VerifyQuery(this);
        }

        [Fact]
        public void Values_id_member()
        {
            _g
                .V<Person>()
                .Values(x => x.Id)
                .VerifyQuery(this);
        }

        [Fact]
        public void Values_no_member()
        {
            _g
                .V<Person>()
                .Values()
                .VerifyQuery(this);
        }

        [Fact]
        public void Values_of_Edge()
        {
            _g
                .E<LivesIn>()
                .Values(x => x.Since)
                .VerifyQuery(this);
        }

        [Fact]
        public void Values_of_Vertex1()
        {
            _g
                .V<Person>()
                .Values(x => x.Name)
                .VerifyQuery(this);
        }

        [Fact]
        public void Values_of_Vertex2()
        {
            _g
                .V<Person>()
                .Values(x => x.Name)
                .VerifyQuery(this);
        }

        [Fact]
        public void Variable_wrap()
        {
            _g
                .V()
                .Properties()
                .Properties("1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30")
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_anonymous()
        {
            _g
                .V<Person>()
                .Where(_ => _)
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_array_does_not_intersect_property_array()
        {
            _g
                .V<Company>()
                .Where(t => !new[] { "+4912345", "+4923456" }.Intersect(t.PhoneNumbers).Any())
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_array_intersects_property_aray()
        {
            _g
                .V<Company>()
                .Where(t => new[] { "+4912345", "+4923456" }.Intersect(t.PhoneNumbers).Any())
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_bool_property_explicit_comparison1()
        {
            _g
                .V<TimeFrame>()
                // ReSharper disable once RedundantBoolCompare
                .Where(t => t.Enabled == true)
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_bool_property_explicit_comparison2()
        {
            _g
                .V<TimeFrame>()
                .Where(t => t.Enabled == false)
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_bool_property_implicit_comparison1()
        {
            _g
                .V<TimeFrame>()
                .Where(t => t.Enabled)
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_bool_property_implicit_comparison2()
        {
            _g
                .V<TimeFrame>()
                .Where(t => !t.Enabled)
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_complex_logical_expression()
        {
            _g
                .V<Person>()
                .Where(t => t.Name.Value == "Some name" && (t.Age == 42 || t.Age == 99))
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_complex_logical_expression_with_null()
        {
            _g
                .V<Person>()
                .Where(t => t.Name == null && (t.Age == 42 || t.Age == 99))
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_conjunction()
        {
            _g
                .V<Person>()
                .Where(t => t.Age == 36 && t.Age == 42)
                .VerifyQuery(this);
        }

        [Fact(Skip="Optimizable")]
        public void Where_conjunction_optimizable()
        {
            _g
                .V<Person>()
                .Where(t => (t.Age == 36 && t.Name.Value == "Hallo") && t.Age == 42)
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_conjunction_with_different_fields()
        {
            _g
                .V<Person>()
                .Where(t => t.Name.Value == "Some name" && t.Age == 42)
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_converted_Id_equals_constant()
        {
            _g
                .V<Language>()
                .Where(t => (int)t.Id == 1)
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_current_element_equals_stepLabel1()
        {
            _g
                .V<Language>()
                .As((__, l) => __
                    .V<Language>()
                    .Where(l2 => l2 == l))
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_current_element_equals_stepLabel2()
        {
            _g
                .V<Language>()
                .As((__, l) => __
                    .V<Language>()
                    .Where(l2 => l == l2))
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_current_element_not_equals_stepLabel1()
        {
            _g
                .V<Language>()
                .As((__, l) => __
                    .V<Language>()
                    .Where(l2 => l2 != l))
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_current_element_not_equals_stepLabel2()
        {
            _g
                .V<Language>()
                .As((__, l) => __
                    .V<Language>()
                    .Where(l2 => l != l2))
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_disjunction()
        {
            _g
                .V<Person>()
                .Where(t => t.Age == 36 || t.Age == 42)
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_disjunction_with_different_fields()
        {
            _g
                .V<Person>()
                .Where(t => t.Name.Value == "Some name" || t.Age == 42)
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_empty_array_does_not_intersect_property_array()
        {
            _g
                .V<Company>()
                .Where(t => !new string[0].Intersect(t.PhoneNumbers).Any())
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_empty_array_intersects_property_array()
        {
            _g
                .V<Company>()
                .Where(t => new string[0].Intersect(t.PhoneNumbers).Any())
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_has_conjunction_of_three()
        {
            _g
                .V<Person>()
                .Where(t => t.Age == 36 && t.Age == 42 && t.Age == 99)
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_has_disjunction_of_three()
        {
            _g
                .V<Person>()
                .Where(t => t.Age == 36 || t.Age == 42 || t.Age == 99)
                .VerifyQuery(this);
        }

        [Fact(Skip = "Optimization opportunity.")]
        public void Where_has_disjunction_of_three_with_or()
        {
            _g
                .V<Person>()
                .Or(
                    __ => __.Where(t => t.Age == 36),
                    __ => __.Where(t => t.Age == 42),
                    __ => __.Where(t => t.Age == 99))
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_Id_equals_constant()
        {
            _g
                .V<Language>()
                .Where(t => t.Id == (object)1)
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_identity()
        {
            _g
                .V<Person>()
                .Where(_ => _.Identity())
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_identity_with_type_change()
        {
            _g
                .V<Person>()
                .Where(_ => _.OfType<Authority>())
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_none_traversal()
        {
            _g
                .V<Person>()
                .Where(_ => _.None())
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_not_none()
        {
            _g
                .V<Person>()
                .Where(_ => _
                    .Not(_ => _
                        .None()))
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_or_dead_traversal()
        {
            _g
                .V<Person>()
                .Where(_ => _
                    .Or(_ => _
                        .Where(x => new object[0].Contains(x.Id))))
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_or_identity()
        {
            _g
                .V<Person>()
                .Where(_ => _
                    .Or(_ => _))
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_or_none_traversal()
        {
            _g
                .V<Person>()
                .Where(_ => _
                    .Or(_ => _
                        .None()))
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_outside_model()
        {
            _g
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
        public void Where_property_array_contains_element()
        {
            _g
                .V<Company>()
                .Where(t => t.PhoneNumbers.Contains("+4912345"))
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_array_contains_stepLabel()
        {
            _g
                .Inject("+4912345")
                .As((__, t) => __
                    .V<Company>()
                    .Where(c => c.PhoneNumbers.Contains(t)))
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_array_does_not_contain_element()
        {
            _g
                .V<Company>()
                .Where(t => !t.PhoneNumbers.Contains("+4912345"))
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_array_does_not_intersect_array()
        {
            _g
                .V<Company>()
                .Where(t => !t.PhoneNumbers.Intersect(new[] { "+4912345", "+4923456" }).Any())
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_array_does_not_intersect_empty_array()
        {
            _g
                .V<Company>()
                .Where(t => !t.PhoneNumbers.Intersect(new string[0]).Any())
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_array_intersects_array1()
        {
            _g
                .V<Company>()
                .Where(t => t.PhoneNumbers.Intersect(new[] { "+4912345", "+4923456" }).Any())
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_array_intersects_array2()
        {
            _g
                .V<Company>()
                .Where(t => new[] { "+4912345", "+4923456" }.Intersect(t.PhoneNumbers).Any())
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_array_intersects_empty_array()
        {
            _g
                .V<Company>()
                .Where(t => t.PhoneNumbers.Intersect(new string[0]).Any())
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_array_intersects_stepLabel1()
        {
            _g
                .Inject("+4912345")
                .Fold()
                .As((__, t) => __
                    .V<Company>()
                    .Where(c => c.PhoneNumbers.Intersect(t.Value).Any()))
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_array_intersects_stepLabel2()
        {
            _g
                .Inject("+4912345")
                .Fold()
                .As((__, t) => __
                    .V<Company>()
                    .Where(c => t.Value.Intersect(c.PhoneNumbers).Any()))
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_array_is_empty()
        {
            _g
                .V<Company>()
                .Where(t => !t.PhoneNumbers.Any())
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_array_is_not_empty()
        {
            _g
                .V<Company>()
                .Where(t => t.PhoneNumbers.Any())
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_contains_constant_with_TextP_support()
        {
            _g
                .V<Country>()
                .Where(c => c.CountryCallingCode.Contains("456"))
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_contains_constant_without_TextP_support()
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
        public void Where_property_contains_empty_string_with_TextP_support()
        {
            _g
                .V<Country>()
                .Where(c => c.CountryCallingCode.Contains(""))
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_contains_empty_string_without_TextP_support()
        {
            _g
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(c => c
                        .SetItem(GremlinqOption.DisabledTextPredicates, DisabledTextPredicates.StartingWith)))
                .V<Country>()
                .Where(c => c.CountryCallingCode.Contains(""))
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_ends_with_constant_with_TextP_support()
        {
            _g
                .V<Country>()
                .Where(c => c.CountryCallingCode.EndsWith("7890"))
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_ends_with_constant_without_TextP_support()
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
        public void Where_property_ends_with_empty_string_with_TextP_support()
        {
            _g
                .V<Country>()
                .Where(c => c.CountryCallingCode.EndsWith(""))
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_ends_with_empty_string_without_TextP_support()
        {
            _g
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(c => c
                        .SetItem(GremlinqOption.DisabledTextPredicates, DisabledTextPredicates.EndingWith)))
                .V<Country>()
                .Where(c => c.CountryCallingCode.EndsWith(""))
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_equals_constant()
        {
            _g
                .V<Person>()
                .Where(t => t.Age == 36)
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_equals_converted_expression()
        {
            _g
                .V<Person>()
                .Where(t => (object)t.Age == (object)36)
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_equals_expression()
        {
            const int i = 18;

            _g
                .V<Person>()
                .Where(t => t.Age == i + i)
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_equals_local_string_constant()
        {
            const int local = 1;

            _g
                .V<Language>()
                .Where(t => t.Id == (object)local)
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_equals_stepLabel()
        {
            _g
                .V<Language>()
                .Values(x => x.IetfLanguageTag)
                .As((__, l) => __
                    .V<Language>()
                    .Where(l2 => l2.IetfLanguageTag == l))
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_equals_value_of_anonymous_object()
        {
            var local = new { Value = 1 };

            _g
                .V<Language>()
                .Where(t => t.Id == (object)local.Value)
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_is_contained_in_array()
        {
            _g
                .V<Person>()
                .Where(t => new[] { 36, 37, 38 }.Contains(t.Age))
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_is_contained_in_empty_enumerable()
        {
            var enumerable = Enumerable.Empty<int>();

            _g
                .V<Person>()
                .Where(t => enumerable.Contains(t.Age))
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_is_contained_in_enumerable()
        {
            var enumerable = new[] { "36", "37", "38" }
                .Select(int.Parse);

            _g
                .V<Person>()
                .Where(t => enumerable.Contains(t.Age))
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_is_greater_or_equal_than_constant()
        {
            _g
                .V<Person>()
                .Where(t => t.Age >= 36)
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_is_greater_than_constant()
        {
            _g
                .V<Person>()
                .Where(t => t.Age > 36)
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_is_greater_than_or_equal_stepLabel()
        {
            _g
                .V<Person>()
                .Values(x => x.Age)
                .As((__, a) => __
                    .V<Person>()
                    .Where(l2 => l2.Age >= a))
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_is_greater_than_or_equal_stepLabel_value()
        {
            _g
                .V<Person>()
                .As((__, person1) => __
                    .V<Person>()
                    .Where(person2 => person2.Age >= person1.Value.Age))
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_is_greater_than_stepLabel()
        {
            _g
                .V<Person>()
                .Values(x => x.Age)
                .As((__, a) => __
                    .V<Person>()
                    .Where(l2 => l2.Age > a))
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_is_lower_or_equal_than_constant()
        {
            _g
                .V<Person>()
                .Where(t => t.Age <= 36)
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_is_lower_than_constant()
        {
            _g
                .V<Person>()
                .Where(t => t.Age < 36)
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_is_lower_than_or_equal_stepLabel()
        {
            _g
                .V<Person>()
                .Values(x => x.Age)
                .As((__, a) => __
                    .V<Person>()
                    .Where(l2 => l2.Age <= a))
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_is_lower_than_stepLabel()
        {
            _g
                .V<Person>()
                .Values(x => x.Age)
                .As((__, a) => __
                    .V<Person>()
                    .Where(l2 => l2.Age < a))
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_is_not_contained_in_array()
        {
            _g
                .V<Person>()
                .Where(t => !new[] { 36, 37, 38 }.Contains(t.Age))
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_is_not_contained_in_empty_enumerable()
        {
            var enumerable = Enumerable.Empty<int>();

            _g
                .V<Person>()
                .Where(t => !enumerable.Contains(t.Age))
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_is_not_contained_in_enumerable()
        {
            var enumerable = new[] { "36", "37", "38" }
                .Select(int.Parse);

            _g
                .V<Person>()
                .Where(t => !enumerable.Contains(t.Age))
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_is_not_present()
        {
            _g
                .V<Person>()
                .Where(t => t.Name == null)
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_is_prefix_of_constant()
        {
            _g
                .V<Country>()
                .Where(c => "+49123".StartsWith(c.CountryCallingCode))
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_is_prefix_of_empty_string()
        {
            _g
                .V<Country>()
                .Where(c => "".StartsWith(c.CountryCallingCode))
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_is_prefix_of_expression()
        {
            const string str = "+49123xxx";

            _g
                .V<Country>()
                .Where(c => str.Substring(0, 6).StartsWith(c.CountryCallingCode))
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_is_prefix_of_variable()
        {
            const string str = "+49123";

            _g
                .V<Country>()
                .Where(c => str.StartsWith(c.CountryCallingCode))
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_is_present()
        {
            _g
                .V<Person>()
                .Where(t => t.Name != null)
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_not_equals_constant()
        {
            _g
                .V<Person>()
                .Where(t => t.Age != 36)
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_starts_with_constant_with_TextP_support()
        {
            _g
                .V<Country>()
                .Where(c => c.CountryCallingCode.StartsWith("+49123"))
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_starts_with_constant_without_TextP_support()
        {
            _g
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(c => c
                        .SetItem(GremlinqOption.DisabledTextPredicates, DisabledTextPredicates.StartingWith)))
                .V<Country>()
                .Where(c => c.CountryCallingCode.StartsWith("+49123"))
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_starts_with_empty_string_with_TextP_support()
        {
            _g
                .V<Country>()
                .Where(c => c.CountryCallingCode.StartsWith(""))
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_starts_with_empty_string_without_TextP_support()
        {
            _g
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(c => c
                        .SetItem(GremlinqOption.DisabledTextPredicates, DisabledTextPredicates.StartingWith)))
                .V<Country>()
                .Where(c => c.CountryCallingCode.StartsWith(""))
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_property_traversal()
        {
            _g
                .V<Person>()
                .Where(
                    x => x.Age,
                    _ => _
                        .Inject(36))
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_scalar_element_equals_constant()
        {
            _g
                .V<Person>()
                .Values(x => x.Age)
                .Where(_ => _ == 36)
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_sequential()
        {
            _g
                .V<Person>()
                .Where(t => t.Age == 36)
                .Where(t => t.Age == 42)
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_source_expression_on_both_sides1()
        {
            _g
                .V<Country>()
                .Where(t => t.Name.Value == t.CountryCallingCode)
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_source_expression_on_both_sides2()
        {
            _g
                .V<EntityWithTwoIntProperties>()
                .Where(x => x.IntProperty1 > x.IntProperty2)
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_stepLabel_is_lower_than_stepLabel()
        {
            _g
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
        public void Where_stepLabel_value_is_greater_than_or_equal_stepLabel_value()
        {
            _g
                .V<Person>()
                .As((__, person1) => __
                    .V<Person>()
                    .As((__, person2) => __
                        .Where(_ => person1.Value.Age >= person2.Value.Age)))
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_traversal()
        {
            _g
                .V<Person>()
                .Where(_ => _.Out<LivesIn>())
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_true()
        {
            _g
                .V<Person>()
                .Where(_ => true)
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_VertexProperty_Value1()
        {
            _g
                .V<Person>()
                .Where(x => x.Name.Value == "SomeName")
                .VerifyQuery(this);
        }

        [Fact]
        public void Where_VertexProperty_Value2()
        {
            _g
                .V<Person>()
                .Where(x => ((int)(object)x.Name.Value) > 36)
                .VerifyQuery(this);
        }

        [Fact(Skip="Feature!")]
        public void Where_VertexProperty_Value3()
        {
            _g
                .V<Person>()
                .Where(x => (int)x.Name.Id == 36)
                .VerifyQuery(this);
        }

        [Fact]
        public void WithoutStrategies1()
        {
            _g
                .RemoveStrategies(typeof(SubgraphStrategy))
                .V()
                .VerifyQuery(this);
        }

        [Fact]
        public void WithoutStrategies2()
        {
            _g
                .RemoveStrategies(typeof(SubgraphStrategy), typeof(ElementIdStrategy))
                .V()
                .VerifyQuery(this);
        }

        //[Fact(Skip = "Can't handle currently!")]
        //public void WithSubgraphStrategy()
        //{
        //    _g
        //        .AddStrategies(new SubgraphQueryStrategy(_ => _.OfType<Person>(), _ => _.OfType<WorksFor>()))
        //        .V()
        //        .VerifyQuery(this);
        //        .SerializeToGroovy("withStrategies(SubgraphStrategy.build().vertices(__.hasLabel(_a)).edges(__.hasLabel(_b)).create()).V()")
        //        .WithParameters("Person", "WorksFor");
        //}

        //[Fact(Skip = "Can't handle currently!")]
        //public void WithSubgraphStrategy_empty()
        //{
        //    _g
        //        .AddStrategies(new SubgraphQueryStrategy(_ => _, _ => _))
        //        .V()
        //        .VerifyQuery(this);
        //        .SerializeToGroovy("V()")
        //        .WithoutParameters();
        //}

        //[Fact(Skip = "Can't handle currently!")]
        //public void WithSubgraphStrategy_only_edges()
        //{
        //    _g
        //        .AddStrategies(new SubgraphQueryStrategy(_ => _, _ => _.OfType<WorksFor>()))
        //        .V()
        //        .VerifyQuery(this);
        //        .SerializeToGroovy("withStrategies(SubgraphStrategy.build().edges(__.hasLabel(_a)).create()).V()")
        //        .WithParameters("WorksFor");
        //}

        //[Fact(Skip = "Can't handle currently!")]
        //public void WithSubgraphStrategy_only_vertices()
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
