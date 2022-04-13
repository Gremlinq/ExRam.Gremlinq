using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ExRam.Gremlinq.Core.GraphElements;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Tests.Entities;
using FluentAssertions;
using Gremlin.Net.Process.Traversal;
using Gremlin.Net.Process.Traversal.Strategy.Decoration;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace ExRam.Gremlinq.Core.Tests
{
    [TestCaseOrderer("ExRam.Gremlinq.Core.Tests.SideEffectTestCaseOrderer", "ExRam.Gremlinq.Core.Tests")]
    public abstract class QueryExecutionTest : GremlinqTestBase
    {
        private sealed class XunitLogger : ILogger, IDisposable
        {
            private readonly ITestOutputHelper _output;

            public XunitLogger(ITestOutputHelper output)
            {
                _output = output;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception, string>? formatter)
            {
                _output.WriteLine(state?.ToString());
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                return this;
            }

            public void Dispose()
            {
            }
        }

        protected readonly IGremlinQuerySource _g;

        private static readonly string Id = "id";

        // ReSharper disable once ExplicitCallerInfoArgument
        protected QueryExecutionTest(IGremlinqTestFixture fixture, ITestOutputHelper testOutputHelper, [CallerFilePath] string callerFilePath = "") : base(testOutputHelper, callerFilePath)
        {
            _g = fixture.GremlinQuerySource
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(options => options
                        .SetValue(GremlinqOption.StringComparisonTranslationStrictness, StringComparisonTranslationStrictness.Lenient))
                    .UseLogger(new XunitLogger(testOutputHelper))
                    .UseModel(GraphModel.FromBaseTypes<Vertex, Edge>(lookup => lookup
                        .IncludeAssembliesOfBaseTypes())));
        }

        [Fact]
        public virtual async Task AddE_from_StepLabel()
        {
            await _g
                .AddV(new Country { CountryCallingCode = "+49" })
                .As((_, c) => _
                    .AddV(new Language { IetfLanguageTag = "en" })
                    .AddE<Speaks>()
                    .From(c))
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
        public virtual async Task AddE_InV()
        {
            await _g
                .AddV<Person>()
                .AddE<LivesIn>()
                .To(__ => __
                    .AddV<Country>())
                .InV()
                .Verify();
        }

        [Fact]
        public virtual async Task AddE_OutV()
        {
            await _g
                .AddV<Person>()
                .AddE<LivesIn>()
                .To(__ => __
                    .AddV<Country>())
                .OutV()
                .Verify();
        }

        [Fact]
        public virtual async Task AddE_property()
        {
            await _g
                .AddV<Person>()
                .AddE(new LivesIn
                {
                    Since = new DateTimeOffset(2020, 4, 7, 14, 43, 36, TimeSpan.Zero)
                })
                .To(__ => __
                    .AddV<Country>())
                .Verify();
        }

        [Fact]
        public virtual async Task AddE_to_StepLabel()
        {
            await _g
                .AddV(new Language { IetfLanguageTag = "en" })
                .As((_, l) => _
                    .AddV(new Country { CountryCallingCode = "+49" })
                    .AddE<Speaks>()
                    .To(l))
                .Verify();
        }

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
                        .ConfigureProperties(_ => _
                            .ConfigureElement<WorksFor>(conf => conf
                                .IgnoreAlways(p => p.From)
                                .IgnoreAlways(p => p.Role)))))
                .AddE(new WorksFor { From = now, To = now, Role = "Admin" })
                .From(__ => __.AddV<Person>())
                .To(__ => __.AddV<Company>())
                .Verify();
        }

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
        public virtual async Task AddV()
        {
            await _g
                .AddV(new Language { IetfLanguageTag = "en" })
                .Verify();
        }

        [Fact]
        public virtual async Task AddV_ignores_label()
        {
            await _g
                .AddV(new Language {Label = "Language"})
                .Verify();
        }

        [Fact]
        public virtual async Task AddV_list_cardinality_id()
        {
            if (_g.Environment.FeatureSet.Supports(VertexFeatures.UserSuppliedIds))
            {
                await _g
                    .ConfigureEnvironment(env => env
                        .UseModel(GraphModel
                            .FromBaseTypes<VertexWithListId, Edge>(lookup => lookup
                                .IncludeAssembliesOfBaseTypes())))
                    .AddV(new VertexWithListId { Id = new[] { "123", "456" } })
                    .Awaiting(x => x.FirstAsync())
                    .Should()
                    .ThrowAsync<NotSupportedException>();
            }
        }

        [Fact]
        public virtual async Task AddV_TimeFrame()
        {
            await _g
                .AddV(new TimeFrame
                {
                    StartTime = TimeSpan.FromHours(8),
                    Duration = TimeSpan.FromHours(2)
                })
                .Verify();
        }

        [Fact]
        public virtual async Task AddV_with_enum_property()
        {
            await _g
                .AddV(new Person { Gender = Gender.Female })
                .Verify();
        }

        [Fact]
        public virtual async Task AddV_with_byte_array_property()
        {
            await _g
                .AddV(new Person
                {
                    Image = new byte [] { 1, 2, 3, 4, 5, 6, 7, 8 }
                })
                .Verify();
        }

        [Fact]
        public virtual async Task AddV_With_Ignored()
        {
            var now = new DateTimeOffset(2020, 4, 7, 14, 43, 36, TimeSpan.Zero);
            var person = new Person { Age = 21, Gender = Gender.Male, Name = "Marko", RegistrationDate = now };

            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureModel(model => model
                        .ConfigureProperties(_ => _
                            .ConfigureElement<Person>(conf => conf
                                .IgnoreAlways(p => p.Age)
                                .IgnoreAlways(p => p.Gender)))))
                .AddV(person)
                .Verify();
        }

        [Fact]
        public virtual async Task AddV_with_ignored_id_property()
        {
            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureModel(model => model
                        .ConfigureProperties(_ => _
                            .ConfigureElement<Language>(conf => conf
                                .IgnoreOnAdd(p => p.Id)))))
                .AddV(new Language { Id = 300, IetfLanguageTag = "en" })
                .Verify();
        }

        [Fact]
        public virtual async Task AddV_with_ignored_property()
        {
            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureModel(model => model
                        .ConfigureProperties(_ => _
                            .ConfigureElement<Language>(conf => conf
                                .IgnoreOnAdd(p => p.IetfLanguageTag)))))
                .AddV(new Language { IetfLanguageTag = "en" })
                .Verify();
        }

        [Fact]
        public virtual async Task AddV_with_Meta_with_properties()
        {
            await _g
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
        }

        [Fact]
        public virtual async Task AddV_with_Meta_without_properties()
        {
            await _g
                .AddV(new Country { Name = "GER"})
                .Verify();
        }

        [Fact]
        public virtual void VertexProperty_throws_on_null_value()
        {
            default(int)
                .Invoking(_ => new VertexProperty<string>(null!))
                .Should()
                .Throw<ArgumentNullException>();

            new VertexProperty<string>("")
                .Invoking(_ => _.Value = null!)
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public virtual async Task AddV_with_MetaModel()
        {
            await _g
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
        }

        [Fact]
        public virtual async Task AddV_with_multi_property()
        {
            await _g
                .AddV(new Company { PhoneNumbers = new[] { "+4912345", "+4923456" } })
                .Verify();
        }

        [Fact]
        public virtual async Task AddV_with_nulls()
        {
            await _g
                .AddV(new Language())
                .Verify();
        }

        [Fact]
        public virtual async Task AddV_with_overridden_name()
        {
            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureModel(model => model
                        .ConfigureProperties(propModel => propModel
                            .ConfigureElement<Language>(conf => conf
                                .ConfigureName(x => x.IetfLanguageTag, "lang")))))
                .AddV(new Language { IetfLanguageTag = "en" })
                .Verify();
        }

        [Fact]
        public virtual async Task AddV_without_id()
        {
            await _g
                .AddV(new Language { IetfLanguageTag = "en" })
                .Verify();
        }

        [Fact]
        public virtual async Task AddV_without_model()
        {
            await _g
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel.Empty))
                .AddV(new Language { IetfLanguageTag = "en" })
                .Verify();
        }

        [Fact]
        public virtual async Task Aggregate_Cap_Select()
        {
            await _g
                .V<Person>()
                .Aggregate((__, aggregated) => __
                    .Cap(aggregated)
                    .Select(aggregated))
                .Verify();
        }

        [Fact]
        public virtual async Task Aggregate_Cap_Select_with_ints()
        {
            await _g
                .V<Person>()
                .CountLocal()
                .Aggregate((__, aggregated) => __
                    .Cap(aggregated)
                    .Select(aggregated))
                .Verify();
        }

        [Fact]
        public virtual async Task Aggregate_Cap()
        {
            await _g
                .V<Person>()
                .Aggregate((__, aggregated) => __
                    .Cap(aggregated))
                .Verify();
        }

        [Fact]
        public virtual async Task Aggregate_Cap_type()
        {
            _g
                .V<Person>()
                .Aggregate((__, aggregated) => __
                    .Cap(aggregated))
                .Should()
                .BeAssignableTo<IGremlinQueryBase<Person[]>>();
        }

        [Fact]
        public virtual async Task Aggregate_Cap_unfold()
        {
            var a = _g
                .V<Person>()
                .Aggregate((__, aggregated) => __
                    .Cap(aggregated)
                    .Unfold());

            await a
                .Verify();
        }

        [Fact]
        public virtual async Task Aggregate_Global()
        {
            await _g
                .V<Person>()
                .Aggregate((__, aggregated) => __)
                .Verify();
        }

        [Fact]
        public virtual async Task Aggregate_Local()
        {
            await _g
                .V<Person>()
                .AggregateLocal((__, aggregated) => __)
                .Verify();
        }

        [Fact]
        public virtual async Task Aggregate_Global_with_existing_step()
        {
            await _g
                .V<Person>()
                .Aggregate(new())
                .Verify();
        }

        [Fact]
        public virtual async Task Aggregate_Local_with_existing_step()
        {
            await _g
                .V<Person>()
                .AggregateLocal(new())
                .Verify();
        }

        [Fact]
        public virtual async Task Aggregate_Select()
        {
            var stepLabel = new StepLabel<IArrayGremlinQuery<Person[], Person, IVertexGremlinQuery<Person>>, Person[]>();

            await _g
                .V<Person>()
                .Aggregate(stepLabel)
                .Select(stepLabel)
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
                .Select(stepLabel)
                .Verify();
        }

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
                .Select(stepLabel1, stepLabel2)
                .Verify();
        }

        [Fact]
        public virtual async Task And()
        {
            await _g
                .V<Person>()
                .And(
                    __ => __
                        .InE<WorksFor>(),
                    __ => __
                        .OutE<LivesIn>())
                .Verify();
        }

        [Fact]
        public virtual async Task And_identity()
        {
            await _g
                .V<Person>()
                .And(
                    __ => __)
                .Verify();
        }

        [Fact]
        public virtual async Task And_infix()
        {
            await _g
                .V<Person>()
                .And()
                .Out()
                .Verify();
        }

        [Fact]
        public virtual async Task And_nested()
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
                .Verify();
        }

        [Fact]
        public virtual async Task And_nested_or_optimization()
        {
            await _g
                .V<Person>()
                .And(
                    __ => __.Or(
                        __ => __),
                    __ => __.Out())
                .Verify();
        }

        [Fact]
        public virtual async Task And_none()
        {
            await _g
                .V<Person>()
                .And(
                    __ => __.None())
                .Verify();
        }

        [Fact]
        public virtual async Task And_none_with_sideEffect()
        {
            await _g
                .V<Person>()
                .And(
                    __ => __
                        .Aggregate((__, l) => __
                            .None()),
                    __ => __
                        .OutE())
                .Verify();
        }

        [Fact]
        public virtual async Task And_optimization()
        {
            await _g
                .V<Person>()
                .And(
                    __ => __,
                    __ => __.Out())
                .Verify();
        }

        [Fact]
        public virtual async Task And_single()
        {
            await _g
                .V<Person>()
                .And(
                    __ => __.Out())
                .Verify();
        }

        [Fact]
        public virtual async Task As_followed_by_Select()
        {
            await _g
                .V<Person>()
                .As((_, stepLabel1) => _
                    .Select(stepLabel1))
                .Verify();
        }

        [Fact]
        public virtual async Task As_followed_by_casted_Select()
        {
            await _g
                .V<Person>()
                .As((_, stepLabel1) => _
                    .Out()
                    .Select(stepLabel1.Cast<object>()))
                .Verify();
        }

        [Fact]
        public virtual async Task As_idempotency_is_detected()
        {
            await _g
                .V<Person>()
                .As((_, stepLabel1) => _
                    .As((__, stepLabel2) => __
                        .Select(stepLabel1, stepLabel2)))
                .Verify();
        }

        [Fact]
        public virtual async Task As_inlined_nested_Select()
        {
            await _g
                .V<Person>()
                .As((_, stepLabel1) => _
                    .Out()
                    .OfType<Person>()
                    .As((__, stepLabel2) => __
                        .Select(stepLabel1, stepLabel2)))
                .Verify();
        }

        [Fact]
        public virtual async Task As_inlined_nested_Select2()
        {
            await _g
                .V<Person>()
                .As((_, stepLabel1) => _
                    .Out()
                    .OfType<Person>()
                    .As((__, stepLabel2) => __
                        .Out()
                        .Select(stepLabel1, stepLabel2)))
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
        public virtual void Cast_to_same_type_yields_same_query()
        {
            var original = _g
                .V<Person>();

            var cast = original
                .Cast<Person>();

            original
                .Should()
                .BeSameAs(cast);
        }

        [Fact]
        public virtual async Task Choose_one_case()
        {
            await _g
                .V()
                .Where(__ => __.Properties())
                .Choose(_ => _
                    .On(__ => __.Values())
                    .Case(3, __ => __.Constant(1)))
                .Verify();
        }

        [Fact]
        public virtual async Task Choose_only_default_case()
        {
            await _g
                .V()
                .Where(__ => __.Properties())
                .Choose(_ => _
                    .On(__ => __.Values())
                    .Default(__ => __.Constant(1)))
                .Verify();
        }

        [Fact]
        public virtual async Task Choose_Predicate1()
        {
            await _g
                .V()
                .Id()
                .Choose(
                    x => x == (object)42,
                    _ => _.Constant(true),
                    _ => _.Constant(false))
                .Verify();
        }

        [Fact]
        public virtual async Task Choose_Predicate2()
        {
            await _g
                .V()
                .Id()
                .Choose(
                    x => x == (object)42,
                    _ => _.Constant(true))
                .Verify();
        }

        //[Fact]
        //public virtual async Task Choose_Predicate_not_matching()
        //{
        //    await _g
        //        .V<Vertex>()
        //        .Choose(
        //            x => x.Id == (object)42,
        //            _ => _.Out().Lower().Lower().Lower(),
        //            _ => _.Constant(false).Lower().Cast<object>())
        //        .Verify();
        //}

        [Fact]
        public virtual async Task Choose_Predicate3()
        {
            await _g
                .V()
                .Id()
                .Cast<int>()
                .Choose(
                    x => x < 42,
                    _ => _.Constant(true),
                    _ => _.Constant(false))
                .Verify();
        }

        [Fact]
        public virtual async Task Choose_Predicate4()
        {
            await _g
                .V()
                .Id()
                .Cast<int>()
                .Choose(
                    x => 42 > x,
                    _ => _.Constant(true),
                    _ => _.Constant(false))
                .Verify();
        }

        [Fact]
        public virtual async Task Choose_Predicate5()
        {
            await _g
                .V()
                .Id()
                .Cast<int>()
                .Choose(
                    x => 0 < x && x < 42,
                    _ => _.Constant(true),
                    _ => _.Constant(false))
                .Verify();
        }

        [Fact]
        public virtual async Task Choose_Predicate6()
        {
            await _g
                .V()
                .Id()
                .Cast<int>()
                .Choose(
                    x => 0 < x && x < 42 || x != 37,
                    _ => _.Constant(true),
                    _ => _.Constant(false))
                .Verify();
        }

        [Fact]
        public virtual async Task Choose_Predicate7()
        {
            await _g
                .V()
                .Id()
                .Cast<int>()
                .Choose(
                    x => 0 < x || x < 42 && x != 37,
                    _ => _.Constant(true),
                    _ => _.Constant(false))
                .Verify();
        }

        [Fact]
        public virtual async Task Choose_Predicate8()
        {
            await _g
                .V<Vertex>()
                .Choose(
                    x => x.Id == (object)42,
                    _ => _.Constant(true),
                    _ => _.Constant(false))
                .Verify();
        }

        [Fact]
        public virtual async Task Choose_Traversal1()
        {
            await _g
                .V()
                .Choose(
                    _ => _.Values(),
                    _ => _.Out(),
                    _ => _.In())
                .Verify();
        }

        [Fact]
        public virtual async Task Choose_Traversal2()
        {
            await _g
                .V()
                .Choose(
                    _ => _.Values(),
                    _ => _.Out())
                .Verify();
        }

        [Fact]
        public virtual async Task Choose_two_cases()
        {
            await _g
                .V()
                .Where(__ => __.Properties())
                .Choose(_ => _
                    .On(__ => __.Values())
                    .Case(3, __ => __.Constant(1))
                    .Case(4, __ => __.Constant(2)))
                .Verify();
        }

        [Fact]
        public virtual async Task Choose_two_cases_default()
        {
            await _g
                .V()
                .Where(__ => __.Properties())
                .Choose(_ => _
                    .On(__ => __.Values())
                    .Case(3, __ => __.Constant(1))
                    .Case(4, __ => __.Constant(2))
                    .Default(__ => __.Constant(3)))
                .Verify();
        }

        [Fact]
        public virtual async Task Coalesce()
        {
            await _g
                .V()
                .Coalesce(
                    _ => _
                        .Out())
                .Verify();
        }

        [Fact]
        public virtual async Task Coalesce_empty()
        {
            _g
                .V()
                .Invoking(__ => __.Coalesce<IGremlinQueryBase>())
                .Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public virtual async Task Coalesce_identity()
        {
            await _g
                .V()
                .Coalesce(
                    _ => _
                        .Identity())
                .Verify();
        }

        [Fact]
        public virtual Task Coalesce_with_2_subQueries()
        {
            return _g
                .ConfigureEnvironment(_ => _)
                .V()
                .Coalesce(
                    _ => _.Out(),
                    _ => _.In())
                .Verify();
        }

        [Fact]
        public virtual Task Coalesce_with_2_not_matching_subQueries()
        {
            return _g
                .ConfigureEnvironment(_ => _)
                .V()
                .Coalesce(
                    _ => _.OutE(),
                    _ => _.In())
                .Verify();
        }

        [Fact]
        public virtual async Task Constant()
        {
            await _g
                .V()
                .Constant(42)
                .Verify();
        }

        [Fact]
        public virtual async Task Count()
        {
            await _g
                .V()
                .Count()
                .Verify();
        }

        [Fact]
        public virtual async Task CountGlobal()
        {
            await _g
                .V()
                .Count()
                .Verify();
        }

        [Fact]
        public virtual async Task CountLocal()
        {
            await _g
                .V()
                .CountLocal()
                .Verify();
        }

        [Fact]
        public virtual async Task CyclicPath()
        {
            await _g
                .V()
                .Out()
                .Out()
                .CyclicPath()
                .Verify();
        }

        [Fact]
        public virtual async Task Dedup_Global()
        {
            await _g
                .V()
                .Dedup()
                .Verify();
        }

        [Fact]
        public virtual async Task Dedup_Local()
        {
            await _g
                .V()
                .Fold()
                .DedupLocal()
                .Verify();
        }

        [Fact]
        public virtual async Task Drop()
        {
            await _g
                .V<Person>()
                .Drop()
                .Verify();
        }

        [Fact]
        public virtual async Task Drop_in_local()
        {
            await _g
                .Inject(1)
                .Local(__ => __
                    .V()
                    .Drop())
                .Verify();
        }

        [Fact]
        public virtual async Task E_of_all_types1()
        {
            await _g
                .E<object>()
                .Verify();
        }

        [Fact]
        public virtual async Task E_of_all_types2()
        {
            await _g
                .E()
                .Verify();
        }

        [Fact]
        public virtual async Task E_of_concrete_type()
        {
            await _g
                .E<WorksFor>()
                .Verify();
        }

        [Fact]
        public virtual async Task E_Properties()
        {
            await _g
                .E()
                .Properties()
                .Verify();
        }

        [Fact]
        public virtual async Task E_Properties_member()
        {
            await _g
                .E<LivesIn>()
                .Properties(x => x.Since!)
                .Verify();
        }

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
        public virtual async Task Explicit_As_with_string()
        {
            await _g
                .V<Person>()
                .As("stepLabel")
                .Select<Person>("stepLabel")
                .Verify();
        }

        [Fact]
        public virtual async Task FilterWithLambda()
        {
            await _g
                .V<Person>()
                .Where(x => x.Name != null)
                .Where(Lambda.Groovy("it.get().property('Name').value().length() == 2"))
                .Verify();
        }

        [Fact]
        public virtual async Task FlatMap()
        {
            await _g
                .V<Person>()
                .FlatMap(__ => __.Out<WorksFor>())
                .Verify();
        }

        [Fact]
        public virtual async Task Fold()
        {
            await _g
                .V()
                .Fold()
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit0_Fold_LimitLocal_0()
        {
            await _g
                .V()
				.Limit(0)
                .Fold()
                .LimitLocal(0)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit0_Fold_LimitLocal_1()
        {
            await _g
                .V()
				.Limit(0)
                .Fold()
                .LimitLocal(1)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit0_Fold_LimitLocal_2()
        {
            await _g
                .V()
				.Limit(0)
                .Fold()
                .LimitLocal(2)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit0_Fold_TailLocal_0()
        {
            await _g
                .V()
				.Limit(0)
                .Fold()
                .TailLocal(0)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit0_Fold_TailLocal_1()
        {
            await _g
                .V()
				.Limit(0)
                .Fold()
                .TailLocal(1)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit0_Fold_TailLocal_2()
        {
            await _g
                .V()
				.Limit(0)
                .Fold()
                .TailLocal(2)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit0_Fold_RangeLocal_0()
        {
            await _g
                .V()
				.Limit(0)
                .Fold()
                .RangeLocal(0, 0)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit0_Fold_RangeLocal_1()
        {
            await _g
                .V()
				.Limit(0)
                .Fold()
                .RangeLocal(0, 1)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit0_Fold_RangeLocal_2()
        {
            await _g
                .V()
				.Limit(0)
                .Fold()
                .RangeLocal(0, 2)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit0_V_LimitLocal_0()
        {
            await _g
                .V()
				.Limit(0)
                .LimitLocal(0)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit0_V_LimitLocal_1()
        {
            await _g
                .V()
				.Limit(0)
                .LimitLocal(1)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit0_V_LimitLocal_2()
        {
            await _g
                .V()
				.Limit(0)
                .LimitLocal(2)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit0_V_TailLocal_0()
        {
            await _g
                .V()
				.Limit(0)
                .TailLocal(0)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit0_V_TailLocal_1()
        {
            await _g
                .V()
				.Limit(0)
                .TailLocal(1)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit0_V_TailLocal_2()
        {
            await _g
                .V()
				.Limit(0)
                .TailLocal(2)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit0_V_RangeLocal_0()
        {
            await _g
                .V()
				.Limit(0)
                .RangeLocal(0, 0)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit0_V_RangeLocal_1()
        {
            await _g
                .V()
				.Limit(0)
                .RangeLocal(0, 1)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit0_V_RangeLocal_2()
        {
            await _g
                .V()
				.Limit(0)
                .RangeLocal(0, 2)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit1_Fold_LimitLocal_0()
        {
            await _g
                .V()
				.Limit(1)
                .Fold()
                .LimitLocal(0)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit1_Fold_LimitLocal_1()
        {
            await _g
                .V()
				.Limit(1)
                .Fold()
                .LimitLocal(1)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit1_Fold_LimitLocal_2()
        {
            await _g
                .V()
				.Limit(1)
                .Fold()
                .LimitLocal(2)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit1_Fold_TailLocal_0()
        {
            await _g
                .V()
				.Limit(1)
                .Fold()
                .TailLocal(0)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit1_Fold_TailLocal_1()
        {
            await _g
                .V()
				.Limit(1)
                .Fold()
                .TailLocal(1)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit1_Fold_TailLocal_2()
        {
            await _g
                .V()
				.Limit(1)
                .Fold()
                .TailLocal(2)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit1_Fold_RangeLocal_0()
        {
            await _g
                .V()
				.Limit(1)
                .Fold()
                .RangeLocal(0, 0)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit1_Fold_RangeLocal_1()
        {
            await _g
                .V()
				.Limit(1)
                .Fold()
                .RangeLocal(0, 1)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit1_Fold_RangeLocal_2()
        {
            await _g
                .V()
				.Limit(1)
                .Fold()
                .RangeLocal(0, 2)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit1_V_LimitLocal_0()
        {
            await _g
                .V()
				.Limit(1)
                .LimitLocal(0)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit1_V_LimitLocal_1()
        {
            await _g
                .V()
				.Limit(1)
                .LimitLocal(1)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit1_V_LimitLocal_2()
        {
            await _g
                .V()
				.Limit(1)
                .LimitLocal(2)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit1_V_TailLocal_0()
        {
            await _g
                .V()
				.Limit(1)
                .TailLocal(0)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit1_V_TailLocal_1()
        {
            await _g
                .V()
				.Limit(1)
                .TailLocal(1)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit1_V_TailLocal_2()
        {
            await _g
                .V()
				.Limit(1)
                .TailLocal(2)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit1_V_RangeLocal_0()
        {
            await _g
                .V()
				.Limit(1)
                .RangeLocal(0, 0)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit1_V_RangeLocal_1()
        {
            await _g
                .V()
				.Limit(1)
                .RangeLocal(0, 1)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit1_V_RangeLocal_2()
        {
            await _g
                .V()
				.Limit(1)
                .RangeLocal(0, 2)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit2_Fold_LimitLocal_0()
        {
            await _g
                .V()
				.Limit(2)
                .Fold()
                .LimitLocal(0)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit2_Fold_LimitLocal_1()
        {
            await _g
                .V()
				.Limit(2)
                .Fold()
                .LimitLocal(1)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit2_Fold_LimitLocal_2()
        {
            await _g
                .V()
				.Limit(2)
                .Fold()
                .LimitLocal(2)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit2_Fold_TailLocal_0()
        {
            await _g
                .V()
				.Limit(2)
                .Fold()
                .TailLocal(0)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit2_Fold_TailLocal_1()
        {
            await _g
                .V()
				.Limit(2)
                .Fold()
                .TailLocal(1)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit2_Fold_TailLocal_2()
        {
            await _g
                .V()
				.Limit(2)
                .Fold()
                .TailLocal(2)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit2_Fold_RangeLocal_0()
        {
            await _g
                .V()
				.Limit(2)
                .Fold()
                .RangeLocal(0, 0)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit2_Fold_RangeLocal_1()
        {
            await _g
                .V()
				.Limit(2)
                .Fold()
                .RangeLocal(0, 1)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit2_Fold_RangeLocal_2()
        {
            await _g
                .V()
				.Limit(2)
                .Fold()
                .RangeLocal(0, 2)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit2_V_LimitLocal_0()
        {
            await _g
                .V()
				.Limit(2)
                .LimitLocal(0)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit2_V_LimitLocal_1()
        {
            await _g
                .V()
				.Limit(2)
                .LimitLocal(1)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit2_V_LimitLocal_2()
        {
            await _g
                .V()
				.Limit(2)
                .LimitLocal(2)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit2_V_TailLocal_0()
        {
            await _g
                .V()
				.Limit(2)
                .TailLocal(0)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit2_V_TailLocal_1()
        {
            await _g
                .V()
				.Limit(2)
                .TailLocal(1)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit2_V_TailLocal_2()
        {
            await _g
                .V()
				.Limit(2)
                .TailLocal(2)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit2_V_RangeLocal_0()
        {
            await _g
                .V()
				.Limit(2)
                .RangeLocal(0, 0)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit2_V_RangeLocal_1()
        {
            await _g
                .V()
				.Limit(2)
                .RangeLocal(0, 1)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Limit2_V_RangeLocal_2()
        {
            await _g
                .V()
				.Limit(2)
                .RangeLocal(0, 2)
                .Verify();
        }

        [Fact]
        public virtual async Task Fold_Fold_Unfold_Unfold()
        {
            await _g
                .V()
                .Fold()
                .Fold()
                .Unfold()
                .Unfold()
                .Verify();
        }

        [Fact]
        public virtual async Task Fold_SideEffect()
        {
            await _g
                .V()
                .Fold()
                .SideEffect(x => x.Identity())
                .Unfold()
                .Verify();
        }

        [Fact]
        public virtual async Task Fold_Unfold()
        {
            await _g
                .V()
                .Fold()
                .Unfold()
                .Verify();
        }

        [Fact]
        public virtual async Task Group_with_key()
        {
            await _g
                .V()
                .Group(_ => _
                    .ByKey(_ => _.Label()))
                .Verify();
        }

        [Fact]
        public virtual async Task Group_with_key_identity()
        {
            await _g
                .V()
                .Group(_ => _
                    .ByKey(_ => _))
                .Verify();
        }

        [Fact]
        public virtual async Task Group_with_key_and_value1()
        {
            await _g
                .V()
                .Group(_ => _
                    .ByKey(_ => _.Label())
                    .ByValue(_ => _.Out()))
                .Verify();
        }

        [Fact]
        public virtual async Task Group_with_key_and_value2()
        {
            await _g
                .V()
                .Group(_ => _
                    .ByKey(_ => _.Label())
                    .ByValue(_ => _.Values()))
                .Verify();
        }

        [Fact]
        public virtual async Task Identity()
        {
            await _g
                .V<Person>()
                .Identity()
                .Verify();
        }

        [Fact]
        public virtual async Task Identity_Identity()
        {
            await _g
                .V<Person>()
                .Identity()
                .Identity()
                .Verify();
        }


        [Fact]
        public virtual async Task In()
        {
            await _g
                .V<Person>()
                .In<WorksFor>()
                .Verify();
        }

        [Fact]
        public virtual async Task In_of_all_types_max()
        {
            await _g
                .V()
                .In<object>()
                .Verify();
        }

        [Fact]
        public virtual async Task In_of_all_types_min()
        {
            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(o => o
                        .SetValue(GremlinqOption.FilterLabelsVerbosity, FilterLabelsVerbosity.Minimum)))
                .V()
                .In<object>()
                .Verify();
        }

        [Fact]
        public virtual async Task InE_of_all_types_max()
        {
            await _g
                .V()
                .InE<object>()
                .Verify();
        }

        [Fact]
        public virtual async Task InE_of_all_types_min()
        {
            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(x => x
                        .SetValue(GremlinqOption.FilterLabelsVerbosity, FilterLabelsVerbosity.Minimum)))
                .V()
                .InE<object>()
                .Verify();
        }

        [Fact]
        public virtual async Task Inject()
        {
            await _g
                .Inject(36, 37, 38)
                .Verify();
        }

        [Fact]
        public virtual async Task Label()
        {
            await _g
                .V()
                .Label()
                .Verify();
        }

        [Fact]
        public virtual async Task Limit_underflow()
        {
            _g
                .V()
                .Invoking(_ => _.Limit(-1))
                .Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public virtual async Task LimitGlobal()
        {
            await _g
                .V()
                .Limit(1)
                .Verify();
        }

        [Fact]
        public virtual async Task LimitLocal()
        {
            await _g
                .V()
                .LimitLocal(1)
                .Verify();
        }

        [Fact]
        public virtual async Task Local_identity()
        {
            await _g
                .V()
                .Local(__ => __)
                .Verify();
        }

        [Fact]
        public virtual async Task Map()
        {
            await _g
                .V<Person>()
                .Map(__ => __.Out<WorksFor>())
                .Verify();
        }

        [Fact]
        public virtual async Task Map_Identity()
        {
            await _g
                .V<Person>()
                .Map(__ => __)
                .Verify();
        }

        [Fact]
        public virtual async Task Map_Select_operation()
        {
            await _g
                .V<Person>()
                .As((_, stepLabel1) => _
                    .As((__, stepLabel2) => __
                        .Map(__ => __
                            .Select(stepLabel1, stepLabel2))))
                .Verify();
        }

        [Fact]
        public virtual async Task MaxGlobal()
        {
            await _g
                .V<Person>()
                .Values(x => x.Age)
                .Max()
                .Verify();
        }

        [Fact]
        public virtual async Task MaxLocal()
        {
            await _g
                .V<Person>()
                .Values(x => x.Age)
                .Fold()
                .MaxLocal()
                .Verify();
        }

        [Fact]
        public virtual async Task MeanGlobal()
        {
            await _g
                .V<Person>()
                .Values(x => x.Age)
                .Mean()
                .Verify();
        }

        [Fact]
        public virtual async Task MeanLocal()
        {
            await _g
                .V<Person>()
                .Values(x => x.Age)
                .Fold()
                .MeanLocal()
                .Verify();
        }

        [Fact]
        public virtual void Mid_query_g_throws()
        {
            _g
                .V()
                .Invoking(_ => _
                    .Coalesce(
                        __ => _g.V<object>(),
                        __ => __.AddV<object>()))
                .Should()
                .Throw<InvalidOperationException>();
        }

        [Fact]
        public virtual async Task MinGlobal()
        {
            await _g
                .V<Person>()
                .Values(x => x.Age)
                .Min()
                .Verify();
        }

        [Fact]
        public virtual async Task MinLocal()
        {
            await _g
                .V<Person>()
                .Values(x => x.Age)
                .Fold()
                .MinLocal()
                .Verify();
        }

        [Fact]
        public virtual async Task Mute()
        {
            await _g
                .V()
                .Mute()
                .AddV(new Language { IetfLanguageTag = "en" })
                .Verify();
        }

        [Fact]
        public virtual async Task Nested_contradicting_Select_operations_does_not_throw()
        {
            await _g
                .V<Person>()
                .As((__, stepLabel1) => __
                    .As((__, stepLabel2) => __
                        .Select(stepLabel1, stepLabel2)
                        .As((__, tuple) => __
                            .Select(tuple, stepLabel1))))
                .Verify();
        }

        [Fact]
        public virtual async Task Nested_Select_operations()
        {
            await _g
                .V<Person>()
                .As((__, stepLabel1) => __
                    .As((__, stepLabel2) => __
                        .Select(stepLabel1, stepLabel2)
                        .As((__, tuple) => __
                            .Select(stepLabel1, tuple))))
                .Verify();
        }

        [Fact]
        public virtual async Task None()
        {
            await _g
                .V<Person>()
                .None()
                .Verify();
        }

        [Fact]
        public virtual async Task None_None()
        {
            await _g
                .V<Person>()
                .None()
                .None()
                .Verify();
        }

        [Fact]
        public virtual async Task Not1()
        {
            await _g
                .V()
                .Not(__ => __.Out<WorksFor>())
                .Verify();
        }

        [Fact]
        public virtual async Task Not2()
        {
            await _g
                .V()
                .Not(__ => __.OfType<Language>())
                .Verify();
        }

        [Fact]
        public virtual async Task Not3()
        {
            await _g
                .V()
                .Not(__ => __.OfType<Authority>())
                .Verify();
        }

        [Fact]
        public virtual void NullGuard_works()
        {
            _g
                .Invoking(_ => _
                    .V<Company>(default!))
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public virtual async Task OfType_abstract()
        {
            await _g
                .V()
                .OfType<Authority>()
                .Verify();
        }

        [Fact]
        public virtual async Task OfType_redundant1()
        {
            await _g
                .V()
                .OfType<Company>()
                .OfType<Authority>()
                .Verify();
        }

        [Fact]
        public virtual async Task OfType_redundant2()
        {
            await _g
                .V()
                .OfType<Company>()
                .OfType<object>()
                .Verify();
        }

        [Fact]
        public virtual async Task OfType_redundant3()
        {
            await _g
                .V()
                .OfType<Company>()
                .Cast<object>()
                .OfType<Authority>()
                .Verify();
        }

        [Fact]
        public virtual async Task OfType_redundant4()
        {
            await _g
                .V()
                .OfType<Authority>()
                .OfType<Company>()
                .Verify();
        }

        [Fact]
        public virtual async Task Optional()
        {
            await _g
                .V()
                .Optional(
                    __ => __.Out<WorksFor>())
                .Verify();
        }

        [Fact]
        public virtual async Task Or()
        {
            await _g
                .V<Person>()
                .Or(
                    __ => __
                        .InE<WorksFor>(),
                    __ => __
                        .OutE<LivesIn>())
                .Verify();
        }

        [Fact]
        public virtual async Task Or_identity()
        {
            await _g
                .V<Person>()
                .Or(
                    __ => __,
                    __ => __
                        .OutE<LivesIn>())
                .Verify();
        }

        [Fact]
        public virtual async Task Or_infix()
        {
            await _g
                .V<Person>()
                .Out()
                .Or()
                .In()
                .Verify();
        }

        [Fact]
        public virtual async Task Or_nested()
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
                .Verify();
        }

        [Fact]
        public virtual async Task Or_nested_and_optimization()
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
                .Verify();
        }

        [Fact]
        public virtual async Task Or_none()
        {
            await _g
                .V<Person>()
                .Or(
                    __ => __
                        .OutE()
                        .None(),
                    __ => __
                        .OutE())
                .Verify();
        }

        [Fact]
        public virtual async Task Or_none_with_predicate()
        {
            await _g
                .V<Person>()
                .Or(
                    __ => __
                        .None(),
                    __ => __
                        .Where(x => x.Age > 36))
                .Verify();
        }

        [Fact]
        public virtual async Task Or_none_with_sideEffect()
        {
            await _g
                .V<Person>()
                .Or(
                    __ => __
                        .Aggregate((__, l) => __
                            .None()),
                    __ => __
                        .OutE())
                .Verify();
        }

        [Fact]
        public virtual async Task Or_two_step_traversal()
        {
            await _g
                .V<Person>()
                .Or(
                    __ => __
                        .Out<LivesIn>(),
                    __ => __
                        .OutE<LivesIn>()
                        .InV())
                .Verify();
        }

        [Fact]
        public virtual async Task Order_Fold_Unfold()
        {
            await _g
                .V<Vertex>()
                .Order(b => b
                    .By(x => x.Id))
                .Fold()
                .Unfold()
                .Verify();
        }

        [Fact]
        public virtual async Task Order_scalars()
        {
            await _g
                .V<Person>()
                .Local(__ => __.Count())
                .Order(b => b
                    .By(__ => __))
                .Verify();
        }

        [Fact]
        public virtual async Task Order_scalars_local()
        {
            await _g
                .V<Person>()
                .Local(__ => __.Count())
                .OrderLocal(b => b
                    .By(__ => __))
                .Verify();
        }

        [Fact]
        public virtual async Task OrderBy_lambda()
        {
            await _g
                .V<Person>()
                .Where(x => x.Name != null)
                .Order(b => b
                    .By(Lambda.Groovy("it.property('Name').value().length()")))
                .Verify();
        }

        [Fact]
        public virtual async Task OrderBy_member()
        {
            await _g
                .V<Person>()
                .Where(x => x.Name != null)
                .Order(b => b
                    .By(x => x.Name))
                .Verify();
        }

        [Fact]
        public virtual async Task OrderBy_member_ThenBy_member()
        {
            await _g
                .V<Person>()
                .Where(x => x.Name != null)
                .Where(x => x
                    .Values(y => y.Age))
                .Order(b => b
                    .By(x => x.Name)
                    .By(x => x.Age))
                .Verify();
        }

        [Fact]
        public virtual async Task OrderBy_ThenBy_lambda()
        {
            await _g
                .V<Person>()
                .Where(x => x.Name != null)
                .Where(x => x.Values(y => y.Age))
                .Order(b => b
                    .By(Lambda.Groovy("it.property('Name').value().length()"))
                    .By(Lambda.Groovy("it.property('Age').value()")))
                .Verify();
        }

        [Fact]
        public virtual async Task OrderBy_ThenByDescending_member()
        {
            await _g
                .V<Person>()
                .Where(x => x.Name != null)
                .Where(x => x.Values(y => y.Age))
                .Order(b => b
                    .By(x => x.Name)
                    .ByDescending(x => x.Age))
                .Verify();
        }

        [Fact]
        public virtual async Task OrderBy_ThenByDescending_traversal()
        {
            await _g
                .V<Person>()
                .Where(x => x.Name != null)
                .Where(x => x.Values(y => y.Gender))
                .Order(b => b
                    .By(__ => __.Values(x => x.Name!))
                    .ByDescending(__ => __.Gender))
                .Verify();
        }

        [Fact]
        public virtual async Task OrderBy_traversal()
        {
            await _g
                .V<Person>()
                .Where(x => x.Name != null)
                .Order(b => b
                    .By(__ => __.Values(x => x.Name!)))
                .Verify();
        }

        [Fact]
        public virtual async Task OrderBy_traversal_ThenBy()
        {
            await _g
                .V<Person>()
                .Where(x => x.Name != null)
                .Where(x => x.Values(y => y.Gender))
                .Order(b => b
                    .By(__ => __.Values(x => x.Name!))
                    .By(__ => __.Gender))
                .Verify();
        }

        [Fact]
        public virtual async Task OrderBy_traversal_ThenBy_traversal()
        {
            await _g
                .V<Person>()
                .Where(x => x.Name != null)
                .Where(x => x.Values(y => y.Gender))
                .Order(b => b
                    .By(__ => __.Values(x => x.Name!))
                    .By(__ => __.Values(x => x.Gender)))
                .Verify();
        }

        [Fact]
        public virtual async Task OrderByDescending_member()
        {
            await _g
                .V<Person>()
                .Where(x => x.Name != null)
                .Order(b => b
                    .ByDescending(x => x.Name))
                .Verify();
        }

        [Fact]
        public virtual async Task OrderByDescending_traversal()
        {
            await _g
                .V<Person>()
                .Where(x => x.Name != null)
                .Order(b => b
                    .ByDescending(__ => __.Values(x => x.Name!)))
                .Verify();
        }

        [Fact]
        public virtual async Task OrderLocal_by_member()
        {
            await _g
                .V<Person>()
                .Where(x => x.Name != null)
                .OrderLocal(b => b
                    .By(x => x.Name))
                .Verify();
        }

        [Fact]
        public virtual async Task Out()
        {
            await _g
                .V<Person>()
                .Out<WorksFor>()
                .Verify();
        }

        [Fact]
        public virtual async Task Out_does_not_include_abstract_edge()
        {
            await _g
                .V<Person>()
                .Out<Edge>()
                .Verify();
        }

        [Fact]
        public virtual async Task Out_of_all_types_max()
        {
            await _g
                .V()
                .Out<object>()
                .Verify();
        }

        [Fact]
        public virtual async Task Out_of_all_types_min()
        {
            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(o => o
                        .SetValue(GremlinqOption.FilterLabelsVerbosity, FilterLabelsVerbosity.Minimum)))
                .V()
                .Out<object>()
                .Verify();
        }

        [Fact]
        public virtual async Task OutE_of_all_types_max()
        {
            await _g
                .V()
                .OutE<object>()
                .Verify();
        }

        [Fact]
        public virtual async Task OutE_of_all_types_min()
        {
            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(o => o
                        .SetValue(GremlinqOption.FilterLabelsVerbosity, FilterLabelsVerbosity.Minimum)))
                .V()
                .OutE<object>()
                .Verify();
        }

        [Fact]
        public virtual async Task OutE_of_no_derived_types()
        {
            await _g
                .V()
                .OutE<string>()
                .Verify();
        }

        [Fact]
        public virtual async Task Path()
        {
            await _g
                .V()
                .Out()
                .Out()
                .Path()
                .Verify();
        }

        [Fact]
        public virtual async Task Project_to_property_with_builder()
        {
            await _g
                .V<Person>()
                .Where(__ => __.In())
                .Project(_ => _
                    .ToDynamic()
                    .By("in!", __ => __.In())
                    .By(x => x.Age))
                .Verify();
        }

        [Fact]
        public virtual async Task Project_with_builder_1()
        {
            await _g
                .V()
                .Where(__ => __.In())
                .Project(_ => _
                    .ToDynamic()
                    .By("in!", __ => __.In()))
                .Verify();
        }

        [Fact]
        public virtual async Task Project_with_builder_4()
        {
            await _g
                .V()
                .Where(__ => __.In())
                .Where(__ => __.Out())
                .Where(__ => __.Properties())
                .Project(_ => _
                    .ToDynamic()
                    .By("in!", __ => __.In())
                    .By("out!", __ => __.Out())
                    .By("count!", __ => __.Count())
                    .By("properties!", __ => __.Properties()))
                .Verify();
        }

        [Fact]
        public virtual async Task Project_with_identity()
        {
            await _g
                .V()
                .Where(__ => __.Properties())
                .Project(__ => __
                    .ToTuple()
                    .By(__ => __)
                    .By(__ => __.Properties()))
                .Verify();
        }

        [Fact]
        public virtual async Task Project_with_named_identity()
        {
            await _g
                .V()
                .Where(__ => __.Properties())
                .Project(__ => __
                    .ToDynamic()
                    .By("name", __ => __)
                    .By(__ => __.Properties()))
                .Verify();
        }

        [Fact]
        public virtual async Task Project_with_local()
        {
            await _g
                .V()
                .Where(__ => __.Properties())
                .Project(__ => __
                    .ToDynamic()
                    .By("name", __ => __)
                    .By(__ => __
                        .Local(__ => __
                            .Properties())))
                .Verify();
        }

        [Fact]
        public virtual async Task Project2()
        {
            await _g
                .V()
                .Where(__ => __.In())
                .Where(__ => __.Out())
                .Project(__ => __
                    .ToTuple()
                    .By(__ => __.In())
                    .By(__ => __.Out()))
                .Verify();
        }


        [Fact]
        public virtual async Task Project2_with_Property()
        {
            await _g
                .V<Person>()
                .Where(__ => __.In())
                .Project(__ => __
                    .ToTuple()
                    .By(__ => __.In())
                    .By(__ => __.Age))
                .Verify();
        }

        [Fact]
        public virtual async Task Project2_Where()
        {
            await _g
                .V()
                .Project(__ => __
                    .ToTuple()
                    .By(__ => __.Label())
                    .By(__ => __.Fold()))
                .Where(x => x.Item2.Length == 3)
                .Verify();
        }

        [Fact]
        public virtual async Task Project2_Where_lower()
        {
            await _g
                .V()
                .Project(__ => __
                    .ToTuple()
                    .By(__ => __.Label())
                    .By(__ => __.Fold()))
                .Where(x => x.Item2.Length < 3)
                .Verify();
        }

        [Fact]
        public virtual async Task Project3()
        {
            await _g
                .V()
                .Where(__ => __.In())
                .Where(__ => __.Out())
                .Project(__ => __
                    .ToTuple()
                    .By(__ => __.In())
                    .By(__ => __.Out())
                    .By(__ => __.Count()))
                .Verify();
        }

        [Fact]
        public virtual async Task Project3_Select1()
        {
            await _g
                .V()
                .Where(__ => __.In())
                .Where(__ => __.Out())
                .Project(__ => __
                    .ToTuple()
                    .By(__ => __.In())
                    .By(__ => __.Out())
                    .By(__ => __.Count()))
                .Select(x => x.Item1)
                .Verify();
        }
        
        [Fact]
        public virtual async Task Project3_Select2()
        {
            await _g
                .V()
                .Where(__ => __.In())
                .Where(__ => __.Out())
                .Project(__ => __
                    .ToTuple()
                    .By(__ => __.In())
                    .By(__ => __.Out())
                    .By(__ => __.Count()))
                .Select(
                    x => x.Item1,
                    x => x.Item2)
                .Verify();
        }

        [Fact]
        public virtual async Task Project3_with_Property()
        {
            await _g
                .V<Person>()
                .Where(__ => __.In())
                .Where(__ => __.Out())
                .Project(__ => __
                    .ToTuple()
                    .By(__ => __.In())
                    .By(__ => __.Out())
                    .By(__ => __.Age))
                .Verify();
        }

        [Fact]
        public virtual async Task Project3_with_Property_Select2()
        {
            await _g
                .V<Person>()
                .Where(__ => __.In())
                .Where(__ => __.Out())
                .Project(__ => __
                    .ToTuple()
                    .By(__ => __.In())
                    .By(__ => __.Out())
                    .By(__ => __.Age))
                .Select(
                    x => x.Item1,
                    x => x.Item3)
                .Verify();
        }

        [Fact]
        public virtual async Task Project4()
        {
            await _g
                .V()
                .Where(__ => __.In())
                .Where(__ => __.Out())
                .Where(__ => __.Properties())
                .Project(__ => __
                    .ToTuple()
                    .By(__ => __.In())
                    .By(__ => __.Out())
                    .By(__ => __.Count())
                    .By(__ => __.Properties()))
                .Verify();
        }

        [Fact]
        public virtual async Task Project4_with_Property()
        {
            await _g
                .V<Person>()
                .Where(__ => __.In())
                .Where(__ => __.Out())
                .Where(__ => __.Properties())
                .Project(__ => __
                    .ToTuple()
                    .By(__ => __.In())
                    .By(__ => __.Out())
                    .By(__ => __.Age)
                    .By(__ => __.Properties()))
                .Verify();
        }

        [Fact]
        public virtual async Task Project_on_muted()
        {
            await _g
                .V()
                .Mute()
                .Project(__ => __
                    .ToTuple()
                    .By(__ => __.In())
                    .By(__ => __.Out())
                    .By(__ => __.Count()))
                .Verify();
        }

        [Fact]
        public async Task Project2_unguarded()
        {
            await _g
                .ConfigureEnvironment(_ => _
                    .ConfigureOptions(o => o.SetValue(GremlinqOption.EnableEmptyProjectionValueProtection, true)))
                .V()
                .Project(__ => __
                    .ToTuple()
                    .By(__ => __.In())
                    .By(__ => __.Out()))
                .Verify();
        }

        [Fact]
        public async Task Project2_with_Property_unguarded()
        {
            await _g
                .ConfigureEnvironment(_ => _
                    .ConfigureOptions(o => o.SetValue(GremlinqOption.EnableEmptyProjectionValueProtection, true)))
                .V<Person>()
                .Project(__ => __
                    .ToTuple()
                    .By(__ => __.In())
                    .By(__ => __.Age))
                .Verify();
        }

        [Fact]
        public async Task Project3_unguarded()
        {
            await _g
                .ConfigureEnvironment(_ => _
                    .ConfigureOptions(o => o.SetValue(GremlinqOption.EnableEmptyProjectionValueProtection, true)))
                .V()
                .Project(__ => __
                    .ToTuple()
                    .By(__ => __.In())
                    .By(__ => __.Out())
                    .By(__ => __.Count()))
                .Verify();
        }

        [Fact]
        public async Task Project3_Select1_unguarded()
        {
            await _g
                .ConfigureEnvironment(_ => _
                    .ConfigureOptions(o => o.SetValue(GremlinqOption.EnableEmptyProjectionValueProtection, true)))
                .V()
                .Project(__ => __
                    .ToTuple()
                    .By(__ => __.In())
                    .By(__ => __.Out())
                    .By(__ => __.Count()))
                .Select(x => x.Item1)
                .Verify();
        }

        [Fact]
        public async Task Project3_Select2_unguarded()
        {
            await _g
                .ConfigureEnvironment(_ => _
                    .ConfigureOptions(o => o.SetValue(GremlinqOption.EnableEmptyProjectionValueProtection, true)))
                .V()
                .Project(__ => __
                    .ToTuple()
                    .By(__ => __.In())
                    .By(__ => __.Out())
                    .By(__ => __.Count()))
                .Select(
                    x => x.Item1,
                    x => x.Item2)
                .Verify();
        }

        [Fact]
        public async Task Project3_with_Property_Select2_unguarded()
        {
            await _g
                .ConfigureEnvironment(_ => _
                    .ConfigureOptions(o => o.SetValue(GremlinqOption.EnableEmptyProjectionValueProtection, true)))
                .V<Person>()
                .Project(__ => __
                    .ToTuple()
                    .By(__ => __.In())
                    .By(__ => __.Out())
                    .By(__ => __.Age))
                .Select(
                    x => x.Item1,
                    x => x.Item3)
                .Verify();
        }

        [Fact]
        public async Task Project4_unguarded()
        {
            await _g
                .ConfigureEnvironment(_ => _
                    .ConfigureOptions(o => o.SetValue(GremlinqOption.EnableEmptyProjectionValueProtection, true)))
                .V()
                .Project(__ => __
                    .ToTuple()
                    .By(__ => __.In())
                    .By(__ => __.Out())
                    .By(__ => __.Count())
                    .By(__ => __.Properties()))
                .Verify();
        }

        [Fact]
        public async Task Project4_with_Property_unguarded()
        {
            await _g
                .ConfigureEnvironment(_ => _
                    .ConfigureOptions(o => o.SetValue(GremlinqOption.EnableEmptyProjectionValueProtection, true)))
                .V<Person>()
                .Project(__ => __
                    .ToTuple()
                    .By(__ => __.In())
                    .By(__ => __.Out())
                    .By(__ => __.Age)
                    .By(__ => __.Properties()))
                .Verify();
        }

        [Fact]
        public virtual async Task Properties_Meta()
        {
            await _g
                .V<Country>()
                .Properties(x => x.Name!)
                .Meta<PropertyValidity>()
                .Verify();
        }

        [Fact]
        public virtual async Task Properties_Meta_ValueMap()
        {
            await _g
                .V()
                .Properties()
                .Meta<PropertyValidity>()
                .ValueMap()
                .Verify();
        }

        [Fact]
        public virtual async Task Properties_Meta_Values()
        {
            await _g
                .V()
                .Properties()
                .Meta<PropertyValidity>()
                .Values()
                .Verify();
        }

        [Fact]
        public virtual async Task Properties_Meta_Values_Projected()
        {
            await _g
                .V()
                .Properties()
                .Meta<PropertyValidity>()
                .Values(x => x.ValidFrom)
                .Verify();
        }

        [Fact]
        public virtual async Task Properties_Meta_Where1()
        {
            await _g
                .V<Country>()
                .Properties(x => x.Name!)
                .Meta<PropertyValidity>()
                .Where(x => x.Properties!.ValidFrom >= new DateTimeOffset(2019, 01, 01, 01, 00, 00, TimeSpan.Zero))
                .Verify();
        }

        [Fact]
        public virtual async Task Properties_of_member()
        {
            await _g
                .V<Country>()
                .Properties(x => x.Name!)
                .Verify();
        }

        [Fact]
        public virtual async Task Properties_of_three_members()
        {
            await _g
                .V<Country>()
                .Properties(
                    x => x.Name!,
                    x => x.CountryCallingCode!,
                    x => x.Languages!)
                .Verify();
        }

        [Fact]
        public virtual async Task Properties_of_two_members1()
        {
            await _g
                .V<Country>()
                .Properties(
                    x => x.Name!,
                    x => x.CountryCallingCode!)
                .Verify();
        }

        [Fact]
        public virtual async Task Properties_of_two_members2()
        {
            await _g
                .V<Country>()
                .Properties(
                    x => x.Name!,
                    x => x.Languages!)
                .Verify();
        }

        [Fact]
        public virtual async Task Properties_Properties_as_select()
        {
            await _g
                .V<Country>()
                .Properties(x => x.Name!)
                .Properties()
                .As((__, s) => __
                    .Select(s))
                .Verify();
        }

        [Fact]
        public virtual async Task Properties_Properties_key()
        {
            await _g
                .V<Country>()
                .Properties(x => x.Name!)
                .Properties()
                .Key()
                .Verify();
        }

        [Fact]
        public virtual async Task Properties_Properties_Value()
        {
            await _g
                .V<Company>()
                .Properties(x => x.Locations!)
                .Properties()
                .Value()
                .Verify();
        }

        [Fact]
        public virtual async Task Properties_Properties_Where_key()
        {
            await _g
                .V<Company>()
                .Properties(x => x.Locations!)
                .Properties()
                .Where(x => x.Key == "someKey")
                .Verify();
        }

        [Fact]
        public virtual async Task Properties_Properties_Where_key_equals_stepLabel()
        {
            await _g
                .Inject("hello")
                .As((__, stepLabel) => __
                    .V<Company>()
                    .Properties(x => x.Locations!)
                    .Properties()
                    .Where(x => x.Key == stepLabel.Value))
                .Verify();
        }

        [Fact]
        public virtual async Task Properties_Properties1()
        {
            await _g
                .V<Country>()
                .Properties(x => x.Name!)
                .Properties()
                .Verify();
        }

        [Fact]
        public virtual async Task Properties_Properties2()
        {
            await _g
                .V<Company>()
                .Properties(x => x.Locations!)
                .Properties()
                .Verify();
        }

        [Fact]
        public virtual async Task Properties_typed_no_parameters()
        {
            await _g
                .V()
                .Properties<string>()
                .Verify();
        }

        [Fact]
        public virtual async Task Properties_ValueMap_typed()
        {
            await _g
                .V()
                .Properties()
                .ValueMap<string>()
                .Verify();
        }

        [Fact]
        public virtual async Task Properties_ValueMap_untyped()
        {
            await _g
                .V()
                .Properties()
                .ValueMap()
                .Verify();
        }

        [Fact]
        public virtual async Task Properties_Values_Id()
        {
            await _g
                .V()
                .Properties()
                .Values(x => x.Id)
                .Verify();
        }

        [Fact]
        public virtual async Task Properties_Values_Id_Label()
        {
            await _g
                .V()
                .Properties()
                .Values(
                    x => x.Label,
                    x => x.Id)
                .Verify();
        }

        [Fact]
        public virtual async Task Properties_Values_Label()
        {
            await _g
                .V()
                .Properties()
                .Values(x => x.Label)
                .Verify();
        }

        [Fact]
        public virtual async Task Properties_Values_typed()
        {
            await _g
                .V()
                .Properties()
                .Values<string>()
                .Verify();
        }

        [Fact]
        public virtual async Task Properties_Values_untyped()
        {
            await _g
                .V()
                .Properties()
                .Values()
                .Verify();
        }

        [Fact]
        public virtual async Task Properties_Values2()
        {
            await _g
                .V()
                .Properties()
                .Values<int>("MetaProperty")
                .Verify();
        }

        [Fact]
        public virtual async Task Properties_Where_Dictionary_key1()
        {
            await _g
                .V<Person>()
                .Properties()
#pragma warning disable 252,253
                .Where(x => x.Properties!["MetaKey"] == "MetaValue")
#pragma warning restore 252,253
                .Verify();
        }

        [Fact]
        public virtual async Task Properties_Where_Dictionary_key2()
        {
            await _g
                .V<Person>()
                .Properties()
                .Where(x => (int)x.Properties!["MetaKey"] < 100)
                .Verify();
        }

        [Fact]
        public virtual async Task Properties_Where_Id()
        {
            await _g
                .V<Country>()
                .Properties(x => x.Languages!)
#pragma warning disable 252,253
                .Where(x => x.Id == "id")
#pragma warning restore 252,253
                .Verify();
        }

        [Fact]
        public virtual async Task Properties_Where_Id_equals_static_field()
        {
            await _g
                .V<Country>()
                .Properties(x => x.Languages!)
#pragma warning disable 252,253
                .Where(x => x.Id == Id)
#pragma warning restore 252,253
                .Verify();
        }

        [Fact]
        public virtual async Task Properties_Where_Label_2()
        {
            await _g
                .V<Country>()
                .Properties(x => x.Languages!)
                .Where(x => x.Label == "label")
                .Verify();
        }

        [Fact]
        public virtual async Task Properties_Where_Label_equals_StepLabel()
        {
            await _g
                .Inject("label")
                .As((__, l) => __
                    .V<Country>()
                    .Properties(x => x.Languages!)
                    .Where(x => x.Label == l.Value))
                .Verify();
        }

        [Fact]
        public virtual async Task Properties_Where_Meta_key()
        {
            await _g
                .V<Company>()
                .Properties(x => x.Locations!)
                .Where(x => x.Properties!.ValidFrom == new DateTimeOffset(2019, 01, 01, 01, 00, 00, TimeSpan.Zero))
                .Verify();
        }

        [Fact]
        public virtual async Task Properties_Where_Meta_key_reversed()
        {
            await _g
                .V<Company>()
                .Properties(x => x.Locations!)
                .Where(x => new DateTimeOffset(2019, 01, 01, 01, 00, 00, TimeSpan.Zero) == x.Properties!.ValidFrom)
                .Verify();
        }

        [Fact]
        public virtual async Task Properties_Where_neq_Label()
        {
            await _g
                .V<Country>()
                .Properties(x => x.Languages!)
                .Where(x => x.Label != "label")
                .Verify();
        }

        [Fact]
        public virtual async Task Properties_Where_neq_Label_workaround()
        {
            await _g
                .V<Country>()
                .Properties(x => x.Languages!)
                .Where(x => x
                    .Label()
                    .Where(l => l != "label"))
                .Verify();
        }

        [Fact]
        public virtual async Task Properties_Where_reversed()
        {
            await _g
                .V<Country>()
                .Properties(x => x.Languages!)
                .Where(x => "de" == x.Value)
                .Verify();
        }

        [Fact]
        public virtual async Task Properties_Where1()
        {
            await _g
                .V<Country>()
                .Properties(x => x.Languages!)
                .Where(x => x.Value == "de")
                .Verify();
        }

        [Fact]
        public virtual async Task Properties_Where2()
        {
            await _g
                .V<Person>()
                .Properties()
                .Where(x => x.Label == "Age")
                .Where(x => (int)x.Value < 10)
                .Verify();
        }

        [Fact]
        public virtual async Task Properties1()
        {
            await _g
                .V()
                .Properties()
                .Verify();
        }

        [Fact]
        public virtual async Task Properties2()
        {
            await _g
                .E()
                .Properties()
                .Verify();
        }

        [Fact]
        public virtual async Task Property_list()
        {
            await _g
                .V<Company>()
                .Limit(1)
                .Property(x => x.PhoneNumbers!, "+4912345")
                .Verify();
        }

        [Fact]
        public virtual async Task Property_null()
        {
            await _g
                .V<Company>()
                .Limit(1)
                .Property(x => x.PhoneNumbers!, (string)null!)
                .Verify();
        }

        [Fact]
        public virtual async Task Property_single()
        {
            await _g
                .V<Person>()
                .Property(x => x.Age, 36)
                .Verify();
        }

        [Fact]
        public virtual async Task Property_stringKey()
        {
            await _g
                .V<Person>()
                .Property("StringKey1", 36)
                .Verify();
        }

        [Fact]
        public virtual async Task Property_Guid_value()
        {
            await _g
                .V<Person>()
                .Property("GuidKey", Guid.Parse("{AEBACDFB-2C00-4808-A8B6-8D62217A8059}"))
                .Verify();
        }

        [Fact]
        public virtual async Task Property_stringKey_traversal()
        {
            await _g
                .V<Person>()
                .Property(
                    "StringKey2",
                    __ => __.Constant(36))
                .Verify();
        }

        [Fact]
        public virtual async Task Property_single_from_stepLabel()
        {
            await _g
                .Inject(36)
                .As((__, age) => __
                    .V<Person>()
                    .Property(x => x.Age, age))
                .Verify();
        }

        [Fact]
        public virtual async Task Property_single_traversal()
        {
            await _g
                .V<Person>()
                .Property(
                    x => x.Age,
                    __ => __
                        .Values(x => x.Age))
                .Verify();
        }

        [Fact]
        public virtual async Task Property_single_with_meta()
        {
            await _g
                .V<Person>()
                .Property(x => x.Age, new VertexProperty<int>(36)
                {
                    Properties = new Dictionary<string, object>
                    {
                        { "Meta", "value" }
                    }
                })
                .Verify();
        }

        [Fact]
        public virtual async Task Property_single_with_dictionary_meta1()
        {
            await _g
                .V<Country>()
                .Property(x => x.LocalizableDescription, new VertexProperty<object, IDictionary<string, string>>("")
                {
                    Properties = new Dictionary<string, string>
                    {
                        { "someKey", "value" }
                    }
                })
                .Verify();
        }

        [Fact]
        public virtual void Property_single_with_dictionary_meta2()
        {
            _g
                .V<Country>()
                .Invoking(_ => _
                    .Property(x => x.LocalizableDescription, new VertexProperty<string, IDictionary<string, string>>("")
                    {
                        Properties = new Dictionary<string, string>
                        {
                            { "someKey", "value" }
                        }
                    }))
                .Should()
                .Throw<InvalidOperationException>();
        }

        [Fact]
        public virtual void Range_underflow()
        {
            _g
                .V()
                .Invoking(_ => _.Range(-1, 0))
                .Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public virtual async Task RangeGlobal()
        {
            await _g
                .V()
                .Range(1, 3)
                .Verify();
        }

        [Fact]
        public virtual async Task RangeLocal()
        {
            await _g
                .V()
                .RangeLocal(1, 3)
                .Verify();
        }

        [Fact]
        public virtual async Task Repeat_Out()
        {
            await _g
                .V<Person>()
                .Loop(_ => _
                    .Repeat(__ => __
                        .Out<WorksFor>()
                        .OfType<Person>()))
                .Verify();
        }

        [Fact]
        public virtual async Task RepeatUntil()
        {
            await _g
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
        }

        [Fact]
        public virtual async Task Repeat_Times()
        {
            await _g
                .V<Person>()
                .Cast<object>()
                .Loop(_ => _
                    .Repeat(__ => __
                        .InE()
                        .OutV()
                        .Cast<object>())
                    .Times(10))
                .Verify();
        }

        [Fact]
        public virtual async Task Emit_Repeat_Until()
        {
            await _g
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
        }

        [Fact]
        public virtual async Task Emit_Repeat_Times()
        {
            await _g
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
        }

        [Fact]
        public virtual async Task Emit_Repeat()
        {
            await _g
                .V<Person>()
                .Cast<object>()
                .Loop(_ => _
                    .Emit()
                    .Repeat(__ => __
                        .InE()
                        .OutV()
                        .Cast<object>()))
                .Verify();
        }

        [Fact]
        public virtual async Task Repeat_Emit_Until()
        {
            await _g
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
        }

        [Fact]
        public virtual async Task Repeat_Emit_Times()
        {
            await _g
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
        }

        [Fact]
        public virtual async Task Repeat_Emit()
        {
            await _g
                .V<Person>()
                .Cast<object>()
                .Loop(_ => _
                    .Repeat(__ => __
                        .InE()
                        .OutV()
                        .Cast<object>())
                    .Emit())
                .Verify();
        }

        [Fact]
        public virtual async Task RepeatUntil_true()
        {
            await _g
                .V<Person>()
                .Cast<object>()
                .Loop(_ => _
                    .Repeat(__ => __
                        .InE()
                        .OutV()
                        .Cast<object>())
                    .Until(__ => __))
                .Verify();
        }

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
                        .ConfigureProperties(_ => _
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
                        .ConfigureProperties(_ => _
                            .ConfigureElement<Person>(conf => conf
                                .IgnoreOnUpdate(p => p.RegistrationDate)))))
                .ReplaceV(person)
                .Verify();
        }

        [Fact]
        public virtual async Task Set_Meta_Property_to_null()
        {
            await _g
                .V<Country>()
                .Properties(x => x.Name!)
                .Property("metaKey", default(object))
                .Verify();
        }

        [Fact]
        public virtual async Task Set_Meta_Property1()
        {
            await _g
                .V<Country>()
                .Properties(x => x.Name!)
                .Property("metaKey", 1)
                .Verify();
        }

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
        public virtual async Task SimplePath()
        {
            await _g
                .V()
                .Out()
                .Out()
                .SimplePath()
                .Verify();
        }

        [Fact]
        public virtual void Skip_underflow()
        {
            _g
                .V()
                .Invoking(_ => _.Skip(-1))
                .Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public virtual async Task SkipGlobal()
        {
            await _g
                .V()
                .Skip(1)
                .Verify();
        }

        [Fact]
        public virtual async Task SkipLocal()
        {
            await _g
                .V()
                .SkipLocal(1)
                .Verify();
        }

        [Fact]
        public virtual async Task StepLabel_of_array_contains_element()
        {
            await _g
                .Inject(1, 2, 3)
                .Fold()
                .As((_, ints) => _
                    .V<Person>()
                    .Where(person => ints.Value.Contains(person.Age)))
                .Verify();
        }

        [Fact]
        public virtual async Task StepLabel_of_array_contains_element_graphson()
        {
            await _g
                .Inject(1, 2, 3)
                .Fold()
                .As((_, ints) => _
                    .V<Person>()
                    .Where(person => ints.Value.Contains(person.Age)))
                .Verify();
        }

        [Fact]
        public virtual async Task StepLabel_of_array_contains_vertex()
        {
            await _g
                .V()
                .Fold()
                .As((_, v) => _
                    .V<Person>()
                    .Where(person => v.Value.Contains(person)))
                .Count()
                .Verify();
        }

        [Fact]
        public virtual async Task StepLabel_of_array_does_not_contain_vertex()
        {
            await _g
                .V()
                .Fold()
                .As((_, v) => _
                    .V<Person>()
                    .Where(person => !v.Value.Contains(person)))
                .Count()
                .Verify();
        }

        [Fact]
        public virtual async Task StepLabel_of_object_array_contains_element()
        {
            await _g
                .Inject(1, 2, 3)
                .Cast<object>()
                .Fold()
                .As((_, ints) => _
                    .V<Person>()
                    .Where(person => ints.Value.Contains(person.Age)))
                .Verify();
        }

        [Fact]
        public virtual async Task SumGlobal()
        {
            await _g
                .V<Person>()
                .Values(x => x.Age)
                .Sum()
                .Verify();
        }

        [Fact]
        public virtual async Task SumLocal()
        {
            await _g
                .V<Person>()
                .Values(x => x.Age)
                .Fold()
                .SumLocal()
                .Verify();
        }

        [Fact]
        public virtual async Task SumLocal_Where1()
        {
            await _g
                .V<Person>()
                .Values(x => x.Age)
                .Fold()
                .SumLocal()
                .Where(x => x == 100)
                .Verify();
        }

        [Fact]
        public virtual async Task SumLocal_Where2()
        {
            await _g
                .V<Person>()
                .Values(x => x.Age)
                .Fold()
                .SumLocal()
                .Where(x => x < 100)
                .Verify();
        }

        [Fact]
        public virtual void Tail_underflow()
        {
            _g
                .V()
                .Invoking(_ => _.Tail(-1))
                .Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public virtual async Task TailGlobal()
        {
            await _g
                .V()
                .Tail(1)
                .Verify();
        }

        [Fact]
        public virtual async Task TailLocal()
        {
            await _g
                .V()
                .TailLocal(1)
                .Verify();
        }

        [Fact]
        public virtual async Task Union()
        {
            await _g
                .V<Person>()
                .Union(
                    __ => __.Out<WorksFor>(),
                    __ => __.Out<LivesIn>())
                .Verify();
        }

        [Fact]
        public virtual async Task Union_different_types()
        {
            await _g
                .V<Person>()
                .Union(
                    __ => __.Out<WorksFor>(),
                    __ => __.OutE<LivesIn>())
                .Verify();
        }

        [Fact]
        public virtual async Task Union_different_types2()
        {
            await _g
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
        }

        [Fact]
        public virtual async Task UntilRepeat()
        {
            await _g
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
        }

        [Fact]
        public virtual async Task Until_Emit_Repeat()
        {
            await _g
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
        }

        [Fact]
        public virtual async Task Until_Repeat_Emit()
        {
            await _g
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
        }

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
                .Verify();
        }

        [Fact]
        public virtual async Task UpdateE_With_Ignored()
        {
            var now = new DateTime(2020, 4, 7, 14, 43, 36, DateTimeKind.Utc);

            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureModel(model => model
                        .ConfigureProperties(_ => _
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
                        .ConfigureProperties(_ => _
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
                        .ConfigureProperties(_ => _
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
                .Update(new Person { Age = 21, Gender = Gender.Male, Name = "Marko", RegistrationDate = now })
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
                        .ConfigureProperties(_ => _
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
                        .ConfigureProperties(_ => _
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
                        .ConfigureProperties(_ => _
                            .ConfigureElement<Person>(conf => conf
                                .IgnoreOnUpdate(p => p.Age)
                                .IgnoreOnUpdate(p => p.Gender)))))
                .V<Person>()
                .Update(person)
                .Verify();
        }

        [Fact]
        public virtual async Task V_Both()
        {
            await _g
                .V()
                .Both<Edge>()
                .Verify();
        }

        [Fact]
        public virtual async Task V_IAuthority()
        {
            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureModel(model => model
                        .ConfigureProperties(_ => _
                            .ConfigureElement<Authority>(__ => __
                                .ConfigureName(x => x.Name, "n")))))
                .V<IAuthority>()
                .Where(x => x.Name!.Value == "some name")
                .Verify();
        }

        [Fact]
        public virtual async Task V_of_abstract_type()
        {
            await _g
                .V<Authority>()
                .Verify();
        }

        [Fact]
        public virtual async Task V_of_all_types1()
        {
            await _g
                .V<object>()
                .Verify();
        }

        [Fact]
        public virtual async Task V_of_all_types2()
        {
            await _g
                .V()
                .Verify();
        }

        [Fact]
        public virtual async Task V_of_concrete_type()
        {
            await _g
                .V<Person>()
                .Verify();
        }

        [Fact]
        public virtual async Task V_untyped()
        {
            await _g
                .V()
                .Verify();
        }

        [Fact]
        public virtual async Task Value()
        {
            await _g
                .V()
                .Properties()
                .Value()
                .Verify();
        }

        [Fact]
        public virtual async Task ValueMap_typed()
        {
            await _g
                .V<Person>()
                .ValueMap(x => x.Age)
                .Verify();
        }

        [Fact]
        public virtual async Task Values_1_member()
        {
            await _g
                .V<Person>()
                .Values(x => x.Age)
                .Verify();
        }

        [Fact]
        public virtual async Task Values_ToString()
        {
            await _g
                .V<Person>()
                .Values(x => x.Name!.ToString())
                .Verify();
        }

        [Fact]
        public virtual async Task Values_2_members()
        {
            await _g
                .V<Person>()
                .Values(x => x.Name, x => x.Id)
                .Verify();
        }

        [Fact]
        public virtual async Task Values_3_members()
        {
            await _g
                .V<Person>()
                .Values(x => x.Name, x => x.Gender, x => x.Id)
                .Verify();
        }

        [Fact]
        public virtual async Task Values_id_member()
        {
            await _g
                .V<Person>()
                .Values(x => x.Id)
                .Verify();
        }

        [Fact]
        public virtual async Task Values_no_member()
        {
            await _g
                .V<Person>()
                .Values()
                .Verify();
        }

        [Fact]
        public virtual async Task Values_of_Edge()
        {
            await _g
                .E<LivesIn>()
                .Values(x => x.Since!)
                .Verify();
        }

        [Fact]
        public virtual async Task Values_of_Vertex1()
        {
            await _g
                .V<Person>()
                .Values(x => x.Name!)
                .Verify();
        }

        [Fact]
        public virtual async Task Values_of_Vertex2()
        {
            await _g
                .V<Person>()
                .Values(x => x.Name!)
                .Verify();
        }

        [Fact]
        public virtual async Task Variable_wrap()
        {
            await _g
                .V()
                .Properties()
                .Properties(Enumerable
                    .Range(1, 1000)
                    .Select(x => x.ToString())
                    .ToArray())
                .Verify();
        }

        [Fact]
        public virtual void Vertex_comparison_with_null_throws()
        {
            _g
                .V<Person>()
                .Invoking(x => x
                    .Where(y => y != null)
                    .Debug())
                .Should()
                .Throw<NotSupportedException>();
        }

        [Fact]
        public virtual async Task VertexProperties_Where_label()
        {
            await _g
                .V<Company>()
                .Properties(x => x.Locations!)
                .Where(x => x.Label == "someKey")
                .Verify();
        }

        [Fact]
        public virtual async Task Where_anonymous()
        {
            await _g
                .V<Person>()
                .Where(_ => _)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_array_does_not_intersect_property_array()
        {
            await _g
                .V<Company>()
                .Where(t => !new[] { "+4912345", "+4923456" }.Intersect(t.PhoneNumbers!).Any())
                .Verify();
        }

        [Fact]
        public virtual async Task Where_array_intersects_property_aray()
        {
            await _g
                .V<Company>()
                .Where(t => new[] { "+4912345", "+4923456" }.Intersect(t.PhoneNumbers!).Any())
                .Verify();
        }

        [Fact]
        public virtual async Task Where_with_nested_as()
        {
            await _g
                .V<Company>()
                .Where(__ => __
                    .As((__, a) => __))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_bool_property_explicit_comparison1()
        {
            await _g
                .V<TimeFrame>()
                // ReSharper disable once RedundantBoolCompare
                .Where(t => t.Enabled == true)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_bool_property_explicit_comparison2()
        {
            await _g
                .V<TimeFrame>()
                .Where(t => t.Enabled == false)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_bool_property_implicit_comparison1()
        {
            await _g
                .V<TimeFrame>()
                .Where(t => t.Enabled)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_bool_property_implicit_comparison2()
        {
            await _g
                .V<TimeFrame>()
                .Where(t => !t.Enabled)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_lower_than_string()
        {
            await _g
                .V<Person>()
                .Where(t => t.Name!.Value.CompareTo("Some name") < 0)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_nullable_enum_property_equals_null()
        {
            await _g
                .V<Person>()
                .Where(t => t.Gender == null)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_nullable_enum_property_not_equals_null()
        {
            await _g
                .V<Person>()
                .Where(t => t.Gender != null)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_lower_than_or_equal_string()
        {
            await _g
                .V<Person>()
                .Where(t => t.Name!.Value.CompareTo("Some name") <= 0)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_equals_string()
        {
            await _g
                .V<Person>()
                .Where(t => t.Name!.Value.CompareTo("Some name") == 0)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_not_equals_string()
        {
            await _g
                .V<Person>()
                .Where(t => t.Name!.Value.CompareTo("Some name") != 0)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_greater_than_string()
        {
            await _g
                .V<Person>()
                .Where(t => t.Name!.Value.CompareTo("Some name") > 0)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_greater_than_or_equal_string()
        {
            await _g
                .V<Person>()
                .Where(t => t.Name!.Value.CompareTo("Some name") >= 0)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_greater_than_or_equal_string_with_IComparable()
        {
            await _g
                .V<Person>()
                // ReSharper disable once RedundantCast
                .Where(t => ((IComparable<string>)t.Name!.Value).CompareTo("Some name") >= 0)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_compared_to_string_always_false()
        {
            await _g
                .V<Person>()
                .Where(t => t.Name!.Value.CompareTo("Some name") < -1)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_lower_than_string_2()
        {
            await _g
                .V<Person>()
                .Where(t => t.Name!.Value.CompareTo("Some name") <= -1)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_lower_than_string_3()
        {
            await _g
                .V<Person>()
                .Where(t => t.Name!.Value.CompareTo("Some name") == -1)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_not_lower_than_string_3()
        {
            await _g
                .V<Person>()
                .Where(t => t.Name!.Value.CompareTo("Some name") != -1)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_greater_or_equal_string_2()
        {
            await _g
                .V<Person>()
                .Where(t => t.Name!.Value.CompareTo("Some name") > -1)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_compared_to_string_always_true()
        {
            await _g
                .V<Person>()
                .Where(t => t.Name!.Value.CompareTo("Some name") >= -1)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_lower_than_or_equal_string_2()
        {
            await _g
                .V<Person>()
                .Where(t => t.Name!.Value.CompareTo("Some name") < 1)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_comparison_to_string_always_true()
        {
            await _g
                .V<Person>()
                .Where(t => t.Name!.Value.CompareTo("Some name") <= 1)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_greater_than_string_2()
        {
            await _g
                .V<Person>()
                .Where(t => t.Name!.Value.CompareTo("Some name") == 1)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_not_greater_than_string_2()
        {
            await _g
                .V<Person>()
                .Where(t => t.Name!.Value.CompareTo("Some name") != 1)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_comparison_to_string_always_false()
        {
            await _g
                .V<Person>()
                .Where(t => t.Name!.Value.CompareTo("Some name") > 1)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_greater_than_string_3()
        {
            await _g
                .V<Person>()
                .Where(t => t.Name!.Value.CompareTo("Some name") >= 1)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_comparison_to_string_always_true_2()
        {
            await _g
                .V<Person>()
                .Where(t => t.Name!.Value.CompareTo("Some name") < 2)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_comparison_to_string_always_true_3()
        {
            await _g
                .V<Person>()
                .Where(t => t.Name!.Value.CompareTo("Some name") <= 2)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_comparison_to_string_always_false_2()
        {
            await _g
                .V<Person>()
                .Where(t => t.Name!.Value.CompareTo("Some name") == 2)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_comparison_to_string_not_always_false_2()
        {
            await _g
                .V<Person>()
                .Where(t => t.Name!.Value.CompareTo("Some name") != 2)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_comparison_to_string_always_false_3()
        {
            await _g
                .V<Person>()
                .Where(t => t.Name!.Value.CompareTo("Some name") > 2)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_comparison_to_string_always_false_4()
        {
            await _g
                .V<Person>()
                .Where(t => t.Name!.Value.CompareTo("Some name") >= 2)
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
        public virtual async Task Where_property_comparison_to_string_with_cast_enum_variable()
        {
            var variable = ListSortDirection.Ascending;

            await _g
                .V<Person>()
                .Where(t => t.Name!.Value.CompareTo("Some name") == (int)variable)
                .Verify();
        }

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
        public virtual async Task Where_complex_logical_expression()
        {
            await _g
                .V<Person>()
                .Where(t => t.Name!.Value == "Some name" && (t.Age == 42 || t.Age == 99))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_complex_logical_expression_with_null()
        {
            await _g
                .V<Person>()
                .Where(t => t.Name == null && (t.Age == 42 || t.Age == 99))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_conjunction()
        {
            await _g
                .V<Person>()
                .Where(t => t.Age == 36 && t.Age == 42)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_out_vertex_property()
        {
            await _g
                .V<Person>()
                .Where(__ => __
                    .Out<WorksFor>()
                    .OfType<Company>()
                    .Values(x => x.Name!.Value)
                    .Where(x => x == "MyCompany"))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_value_of_property_is_null()
        {
            await _g
                .V<Person>()
                .Where(__ => __
                    .Values(x => x.Name!.Value)
                    .Where(x => x == null!))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_value_of_property_is_null_in2()
        {
            await _g
                .V<Person>()
                .Where(__ => __
                    .Values(x => x.Name!.Value)
                    .Where(__ => __
                        .Where(x => x == null)))
                .Verify();
        }

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
        public virtual async Task Where_value_of_property_is_greater_than_null()
        {
            await _g
                .V<Person>()
                .Where(__ => __
                    .Values(x => x.Name!.Value)
                    .Where(x => (int)(object)x > (int)(object)null!))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_value_of_property_is_null_or_string()
        {
            await _g
                .V<Person>()
                .Where(__ => __
                    .Values(x => x.Name!.Value)
                    .Where(x => x == null! || x == "hello"))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_value_of_property_is_null_and_string()
        {
            await _g
                .V<Person>()
                .Where(__ => __
                    .Values(x => x.Name!.Value)
                    .Where(x => x == null! && x == "hello"))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_value_of_property_is_null_or_string_reversed()
        {
            await _g
                .V<Person>()
                .Where(__ => __
                    .Values(x => x.Name!.Value)
                    .Where(x => x == "hello" || x == null!))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_Values_Or_WhereWhere()
        {
            await _g
                .V<Person>()
                .Where(__ => __
                    .Values(x => x.Name!.Value)
                    .Or(
                        __ => __.Where(x => x! == null!),
                        __ => __.Where(x => (object)x! == "")))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_value_of_property_is_null_and_string_reversed()
        {
            await _g
                .V<Person>()
                .Where(__ => __
                    .Values(x => x.Name!.Value)
                    .Where(x => x == "hello" && x == null!))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_value_of_property_is_not_null_or_string()
        {
            await _g
                .V<Person>()
                .Where(__ => __
                    .Values(x => x.Name!.Value)
                    .Where(x => x != null! || x == "hello"))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_value_of_property_is_not_null_and_string()
        {
            await _g
                .V<Person>()
                .Where(__ => __
                    .Values(x => x.Name!.Value)
                    .Where(x => x != null! && x == "hello"))
                .Verify();
        }

        [Fact(Skip="Optimizable")]
        public virtual async Task Where_conjunction_optimizable()
        {
            await _g
                .V<Person>()
                .Where(t => (t.Age == 36 && t.Name!.Value == "Hallo") && t.Age == 42)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_conjunction_with_different_fields()
        {
            await _g
                .V<Person>()
                .Where(t => t.Name!.Value == "Some name" && t.Age == 42)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_converted_Id_equals_constant()
        {
            await _g
                .V<Language>()
                .Where(t => (int)t.Id! == 1)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_current_element_equals_stepLabel1()
        {
            await _g
                .V<Language>()
                .As((__, l) => __
                    .V<Language>()
                    .Where(l2 => l2 == l.Value))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_current_element_equals_stepLabel2()
        {
            await _g
                .V<Language>()
                .As((__, l) => __
                    .V<Language>()
                    .Where(l2 => l.Value == l2))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_current_element_not_equals_stepLabel1()
        {
            await _g
                .V<Language>()
                .As((__, l) => __
                    .V<Language>()
                    .Where(l2 => l2 != l.Value))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_current_element_not_equals_stepLabel2()
        {
            await _g
                .V<Language>()
                .As((__, l) => __
                    .V<Language>()
                    .Where(l2 => l.Value != l2))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_disjunction()
        {
            await _g
                .V<Person>()
                .Where(t => t.Age == 36 || t.Age == 42)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_disjunction_with_different_fields()
        {
            await _g
                .V<Person>()
                .Where(t => t.Name!.Value == "Some name" || t.Age == 42)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_empty_array_does_not_intersect_property_array()
        {
            await _g
                .V<Company>()
                .Where(t => !Array.Empty<string>().Intersect(t.PhoneNumbers!).Any())
                .Verify();
        }

        [Fact]
        public virtual async Task Where_empty_array_intersects_property_array()
        {
            await _g
                .V<Company>()
                .Where(t => Array.Empty<string>().Intersect(t.PhoneNumbers!).Any())
                .Verify();
        }

        [Fact]
        public virtual async Task Where_Has()
        {
            await _g
                .V<Person>()
                .Where(__ => __
                   .Where(t => t.Age == 36))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_has_conjunction_of_three()
        {
            await _g
                .V<Person>()
                .Where(t => t.Age == 36 && t.Age == 42 && t.Age == 99)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_has_disjunction_of_three()
        {
            await _g
                .V<Person>()
                .Where(t => t.Age == 36 || t.Age == 42 || t.Age == 99)
                .Verify();
        }

        [Fact(Skip = "Optimization opportunity.")]
        public virtual async Task Where_has_disjunction_of_three_with_or()
        {
            await _g
                .V<Person>()
                .Or(
                    __ => __.Where(t => t.Age == 36),
                    __ => __.Where(t => t.Age == 42),
                    __ => __.Where(t => t.Age == 99))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_Id_equals_constant()
        {
            await _g
                .V<Language>()
                .Where(t => t.Id == (object)1)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_identity()
        {
            await _g
                .V<Person>()
                .Where(_ => _.Identity())
                .Verify();
        }

        [Fact]
        public virtual async Task Where_identity_with_type_change()
        {
            await _g
                .V<Person>()
                .Where(_ => _.OfType<Authority>())
                .Verify();
        }

        [Fact]
        public virtual async Task Where_none_traversal()
        {
            await _g
                .V<Person>()
                .Where(_ => _.None())
                .Verify();
        }

        [Fact]
        public virtual async Task Where_not_none()
        {
            await _g
                .V<Person>()
                .Where(_ => _
                    .Not(_ => _
                        .None()))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_or_dead_traversal()
        {
            await _g
                .V<Person>()
                .Where(_ => _
                    .Or(_ => _
                        .Where(x => Array.Empty<object>().Contains(x.Id))))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_or_identity()
        {
            await _g
                .V<Person>()
                .Where(_ => _
                    .Or(_ => _))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_or_none_traversal()
        {
            await _g
                .V<Person>()
                .Where(_ => _
                    .Or(_ => _
                        .None()))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_outside_model()
        {
            await _g
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel.FromBaseTypes<VertexWithStringId, EdgeWithStringId>(lookup => lookup
                        .IncludeAssembliesOfBaseTypes())))
                .V<VertexWithStringId>()
                .Where(x => x.Id == (object)0)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_properties_length()
        {
            await _g
                .V<Person>()
                .Where(t => t.PhoneNumbers!.Length == 3)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_native_type_property_length()
        {
            _g
                .V<Person>()
                .Invoking(_ => _.Where(t => t.Image!.Length == 3))
                .Should()
                .Throw<ExpressionNotSupportedException>();
        }

        [Fact]
        public virtual async Task Where_property_array_contains_element()
        {
            await _g
                .V<Company>()
                .Where(t => t.PhoneNumbers!.Contains("+4912345"))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_array_contains_stepLabel()
        {
            await _g
                .Inject("+4912345")
                .As((__, t) => __
                    .V<Company>()
                    .Where(c => c.PhoneNumbers!.Contains(t.Value)))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_array_does_not_contain_element()
        {
            await _g
                .V<Company>()
                .Where(t => !t.PhoneNumbers!.Contains("+4912345"))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_array_does_not_intersect_array()
        {
            await _g
                .V<Company>()
                .Where(t => !t.PhoneNumbers!.Intersect(new[] { "+4912345", "+4923456" }).Any())
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_array_does_not_intersect_empty_array()
        {
            await _g
                .V<Company>()
                .Where(t => !t.PhoneNumbers!.Intersect(Array.Empty<string>()).Any())
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_array_intersects_array1()
        {
            await _g
                .V<Company>()
                .Where(t => t.PhoneNumbers!.Intersect(new[] { "+4912345", "+4923456" }).Any())
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_array_intersects_array2()
        {
            await _g
                .V<Company>()
                .Where(t => new[] { "+4912345", "+4923456" }.Intersect(t.PhoneNumbers!).Any())
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_array_intersects_empty_array()
        {
            await _g
                .V<Company>()
                .Where(t => t.PhoneNumbers!.Intersect(Array.Empty<string>()).Any())
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_array_intersects_stepLabel1()
        {
            await _g
                .Inject("+4912345")
                .Fold()
                .As((__, t) => __
                    .V<Company>()
                    .Where(c => c.PhoneNumbers!.Intersect(t.Value).Any()))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_array_intersects_stepLabel2()
        {
            await _g
                .Inject("+4912345")
                .Fold()
                .As((__, t) => __
                    .V<Company>()
                    .Where(c => t.Value.Intersect(c.PhoneNumbers!).Any()))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_array_is_empty()
        {
            await _g
                .V<Company>()
                .Where(t => !t.PhoneNumbers!.Any())
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_array_is_not_empty()
        {
            await _g
                .V<Company>()
                .Where(t => t.PhoneNumbers!.Any())
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_contains_constant_with_TextP_support()
        {
            await _g
                .V<Country>()
                .Where(c => c.CountryCallingCode!.Contains("456"))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_contains_constant_with_TextP_support_case_insensitive()
        {
            await _g
                .V<Country>()
                .Where(c => c.CountryCallingCode!.Contains("456", StringComparison.OrdinalIgnoreCase))
                .Verify();
        }

        [Fact]
        public virtual void Where_property_contains_constant_without_TextP_support()
        {
            _g
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(c => c
                        .SetValue(GremlinqOption.DisabledTextPredicates, DisabledTextPredicates.Containing)))
                .V<Country>()
                .Invoking(_ =>
                    _.Where(c => c.CountryCallingCode!.Contains("456")))
                .Should()
                .Throw<ExpressionNotSupportedException>();
        }

        [Fact]
        public virtual async Task Where_property_contains_empty_string_with_TextP_support()
        {
            await _g
                .V<Country>()
                .Where(c => c.CountryCallingCode!.Contains(""))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_contains_empty_string_with_TextP_support_case_insensitive()
        {
            await _g
                .V<Country>()
                .Where(c => c.CountryCallingCode!.Contains("", StringComparison.OrdinalIgnoreCase))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_contains_empty_string_without_TextP_support()
        {
            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(c => c
                        .SetValue(GremlinqOption.DisabledTextPredicates, DisabledTextPredicates.StartingWith)))
                .V<Country>()
                .Where(c => c.CountryCallingCode!.Contains(""))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_ends_with_constant_with_TextP_support()
        {
            await _g
                .V<Country>()
                .Where(c => c.CountryCallingCode!.EndsWith("7890"))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_ends_with_constant_with_TextP_support_case_insensitive()
        {
            await _g
                .V<Country>()
                .Where(c => c.CountryCallingCode!.EndsWith("7890", StringComparison.OrdinalIgnoreCase))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_ends_with_constant_without_TextP_support()
        {
            _g
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(c => c
                        .SetValue(GremlinqOption.DisabledTextPredicates, DisabledTextPredicates.EndingWith)))
                .V<Country>()
                .Invoking(_ => _
                    .Where(c => c.CountryCallingCode!.EndsWith("7890")))
                .Should()
                .Throw<ExpressionNotSupportedException>();
        }

        [Fact]
        public virtual async Task Where_property_ends_with_empty_string_with_TextP_support()
        {
            await _g
                .V<Country>()
                .Where(c => c.CountryCallingCode!.EndsWith(""))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_ends_with_empty_string_with_TextP_support_case_insensitive()
        {
            await _g
                .V<Country>()
                .Where(c => c.CountryCallingCode!.EndsWith("", StringComparison.OrdinalIgnoreCase))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_ends_with_empty_string_without_TextP_support()
        {
            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(c => c
                        .SetValue(GremlinqOption.DisabledTextPredicates, DisabledTextPredicates.EndingWith)))
                .V<Country>()
                .Where(c => c.CountryCallingCode!.EndsWith(""))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_equals_constant()
        {
            await _g
                .V<Person>()
                .Where(t => t.Age == 36)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_equals_constant_with_Equals()
        {
            await _g
                .V<Person>()
                .Where(t => t.Age.Equals(36))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_equals_converted_expression()
        {
            await _g
                .V<Person>()
                .Where(t => (object)t.Age == (object)36)
                .Verify();
        }

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
        public virtual async Task Where_property_equals_stepLabel()
        {
            await _g
                .V<Language>()
                .Values(x => x.IetfLanguageTag)
                .As((__, l) => __
                    .V<Language>()
                    .Where(l2 => l2.IetfLanguageTag == l.Value))
                .Verify();
        }

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
        public virtual async Task Where_property_is_contained_in_array()
        {
            await _g
                .V<Person>()
                .Where(t => new[] { 36, 37, 38 }.Contains(t.Age))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_is_contained_in_list()
        {
            await _g
                .V<Person>()
                .Where(t => new List<int> { 36, 37, 38 }.Contains(t.Age))
                .Verify();
        }

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
        public virtual async Task Where_property_is_greater_or_equal_than_constant()
        {
            await _g
                .V<Person>()
                .Where(t => t.Age >= 36)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_is_greater_than_constant()
        {
            await _g
                .V<Person>()
                .Where(t => t.Age > 36)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_is_greater_than_or_equal_stepLabel()
        {
            await _g
                .V<Person>()
                .Values(x => x.Age)
                .As((__, a) => __
                    .V<Person>()
                    .Where(l2 => l2.Age >= a.Value))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_is_greater_than_or_equal_stepLabel_value()
        {
            await _g
                .V<Person>()
                .As((__, person1) => __
                    .V<Person>()
                    .Where(person2 => person2.Age >= person1.Value.Age))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_is_greater_than_stepLabel()
        {
            await _g
                .V<Person>()
                .Values(x => x.Age)
                .As((__, a) => __
                    .V<Person>()
                    .Where(l2 => l2.Age > a.Value))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_is_lower_or_equal_than_constant()
        {
            await _g
                .V<Person>()
                .Where(t => t.Age <= 36)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_is_lower_than_constant()
        {
            await _g
                .V<Person>()
                .Where(t => t.Age < 36)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_is_lower_than_or_equal_stepLabel()
        {
            await _g
                .V<Person>()
                .Values(x => x.Age)
                .As((__, a) => __
                    .V<Person>()
                    .Where(l2 => l2.Age <= a.Value))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_is_lower_than_stepLabel()
        {
            await _g
                .V<Person>()
                .Values(x => x.Age)
                .As((__, a) => __
                    .V<Person>()
                    .Where(l2 => l2.Age < a.Value))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_is_not_contained_in_array()
        {
            await _g
                .V<Person>()
                .Where(t => !new[] { 36, 37, 38 }.Contains(t.Age))
                .Verify();
        }

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
        public virtual async Task Where_property_is_not_present()
        {
            await _g
                .V<Person>()
                .Where(t => t.Name == null)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_is_prefix_of_constant()
        {
            await _g
                .V<Country>()
                .Where(c => "+49123".StartsWith(c.CountryCallingCode!))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_is_prefix_of_empty_string()
        {
            await _g
                .V<Country>()
                .Where(c => "".StartsWith(c.CountryCallingCode!))
                .Verify();
        }

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
        public virtual async Task Where_property_is_prefix_of_variable()
        {
            const string str = "+49123";

            await _g
                .V<Country>()
                .Where(c => str.StartsWith(c.CountryCallingCode!))
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
        public virtual async Task Where_property_is_prefix_of_constant_case_insensitive()
        {
            await _g
                .V<Country>()
                .Where(c => "+49123".StartsWith(c.CountryCallingCode!, StringComparison.OrdinalIgnoreCase))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_is_prefix_of_empty_string_case_insensitive()
        {
            await _g
                .V<Country>()
                .Where(c => "".StartsWith(c.CountryCallingCode!, StringComparison.OrdinalIgnoreCase))
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
        public virtual async Task Where_property_is_prefix_of_variable_case_insensitive()
        {
            const string str = "+49123";

            await _g
                .V<Country>()
                .Where(c => str.StartsWith(c.CountryCallingCode!, StringComparison.OrdinalIgnoreCase))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_is_present()
        {
            await _g
                .V<Person>()
                .Where(t => t.Name != null)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_not_equals_constant()
        {
            await _g
                .V<Person>()
                .Where(t => t.Age != 36)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_string_property_equals()
        {
            await _g
                .V<Country>()
                .Where(c => c.CountryCallingCode!.Equals("+49123"))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_string_property_equals_case_insensitive()
        {
            await _g
                .V<Country>()
                .Where(c => c.CountryCallingCode!.StartsWith("+49123", StringComparison.OrdinalIgnoreCase))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_starts_with_constant_with_TextP_support()
        {
            await _g
                .V<Country>()
                .Where(c => c.CountryCallingCode!.StartsWith("+49123"))
                .Verify();
        }

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
        public virtual async Task Where_property_starts_with_constant_without_TextP_support()
        {
            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(c => c
                        .SetValue(GremlinqOption.DisabledTextPredicates, DisabledTextPredicates.StartingWith)))
                .V<Country>()
                .Where(c => c.CountryCallingCode!.StartsWith("+49123"))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_starts_with_empty_string_with_TextP_support()
        {
            await _g
                .V<Country>()
                .Where(c => c.CountryCallingCode!.StartsWith(""))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_starts_with_empty_string_without_TextP_support()
        {
            await _g
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(c => c
                        .SetValue(GremlinqOption.DisabledTextPredicates, DisabledTextPredicates.StartingWith)))
                .V<Country>()
                .Where(c => c.CountryCallingCode!.StartsWith(""))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_ToString()
        {
            await _g
                .V<Country>()
                .Where(x => x.CountryCallingCode!.ToString() == "some_string")
                .Verify();
        }

        [Fact]
        public virtual async Task Where_cast_property_ToString()
        {
            await _g
                .V<Country>()
                .Where(x => ((Exception)(object)x.CountryCallingCode!).ToString() == "some_string")
                .Verify();
        }

        [Fact]
        public virtual async Task Where_property_traversal()
        {
            await _g
                .V<Person>()
                .Where(
                    x => x.Age,
                    _ => _
                        .Inject(36))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_scalar_element_equals_constant()
        {
            await _g
                .V<Person>()
                .Values(x => x.Age)
                .Where(_ => _ == 36)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_sequential()
        {
            await _g
                .V<Person>()
                .Where(t => t.Age == 36)
                .Where(t => t.Age == 42)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_source_expression_on_both_sides1()
        {
            await _g
                .V<Country>()
                .Where(x => x.Name != null)
                .Where(x => x.CountryCallingCode != null)
                .Where(t => t.Name!.Value == t.CountryCallingCode)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_source_expression_on_both_sides2()
        {
            await _g
                .V<EntityWithTwoIntProperties>()
                .Where(x => x.IntProperty1 > x.IntProperty2)
                .Verify();
        }

        [Fact(Skip="Gremlin server fuckup")]
        public virtual async Task Where_stepLabel_is_lower_than_stepLabel()
        {
            await _g
                .V<Person>()
                .Where(__ => __
                    .As((__, person1) => __
                        .Values(x => x.Gender)
                        .As((__, gender1) => __
                            .V<Person>()
                            .Values(x => x.Gender)
                                .As((__, gender2) => __
                                    .Where(p => gender1.Value == gender2.Value)))))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_stepLabel_value_is_greater_than_or_equal_stepLabel_value()
        {
            await _g
                .V<Person>()
                .As((__, person1) => __
                    .V<Person>()
                    .As((__, person2) => __
                        .Where(_ => person1.Value.Age >= person2.Value.Age)))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_traversal()
        {
            await _g
                .V<Person>()
                .Where(_ => _.Out<LivesIn>())
                .Verify();
        }

        [Fact]
        public virtual async Task Where_true()
        {
            await _g
                .V<Person>()
                .Where(_ => true)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_Values_Where()
        {
            await _g
                .V<Person>()
                .Where(__ => __
                    .Values(x => x.Age)
                    .Where(age => age > 36))
                .Verify();
        }

        [Fact]
        public virtual async Task Or_Values_Where1()
        {
            await _g
                .V<Person>()
                .Or(__ => __
                    .Values(x => x.Age)
                    .Where(age => age > 36))
                .Verify();
        }

        [Fact]
        public virtual async Task Or_Values_Where2()
        {
            await _g
                .V<Person>()
                .Or(
                    __ => __
                        .Values(x => x.Age)
                        .Where(age => age > 36),
                    __ => __
                        .Values(x => x.Age)
                        .Where(age => age < 72))
                .Verify();
        }

        [Fact]
        public virtual async Task And_Values_Where1()
        {
            await _g
                .V<Person>()
                .And(__ => __
                    .Values(x => x.Age)
                    .Where(age => age > 36))
                .Verify();
        }

        [Fact]
        public virtual async Task And_Values_Where2()
        {
            await _g
                .V<Person>()
                .And(
                    __ => __
                        .Values(x => x.Age)
                        .Where(age => age > 36),
                    __ => __
                        .Values(x => x.Age)
                        .Where(age => age < 72))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_Values_Id_Where()
        {
            await _g
                .V<Person>()
                .Where(x => x
                    .Values(x => x.Id)
                    .Where(id => (long)id! == 1L))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_Values_Label_Where()
        {
            await _g
                .V<Vertex>()
                .Where(x => x
                    .Values(x => x.Label)
                    .Where(label => label == "Person"))
                .Verify();
        }

        [Fact]
        public virtual async Task Where_VertexProperty_Value1()
        {
            await _g
                .V<Person>()
                .Where(x => x.Name!.Value == "SomeName")
                .Verify();
        }

        [Fact]
        public virtual async Task Where_VertexProperty_Value2()
        {
            await _g
                .V<Person>()
                .Where(x => ((string)(object)x.Name!.Value) == "SomeName")
                .Verify();
        }

        [Fact(Skip="Feature!")]
        public virtual async Task Where_VertexProperty_Value3()
        {
            await _g
                .V<Person>()
                .Where(x => (int)x.Name!.Id! == 36)
                .Verify();
        }

        [Fact]
        public virtual async Task Where_Where()
        {
            await _g
                .V<Person>()
                .Where(_ => _
                    .Where(_ => _.Out()))
                .Verify();
        }

        [Fact]
        public virtual async Task WithoutStrategies1()
        {
            await _g
                .WithoutStrategies(typeof(SubgraphStrategy))
                .V()
                .Verify();
        }

        [Fact]
        public virtual async Task WithoutStrategies2()
        {
            await _g
                .WithoutStrategies(typeof(SubgraphStrategy), typeof(ElementIdStrategy))
                .V()
                .Verify();
        }

        [Fact]
        public virtual async Task WithoutStrategies3()
        {
            await _g
                .WithoutStrategies(typeof(SubgraphStrategy))
                .WithoutStrategies(typeof(ElementIdStrategy))
                .V()
                .Verify();
        }

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
        public virtual async Task WithSideEffect2()
        {
            await _g
                .WithSideEffect("sideEffectLabel", 36)
                .V()
                .Verify();
        }

        [Fact]
        public virtual async Task WithSideEffect_continuation()
        {
            await _g
                .WithSideEffect(
                    36,
                    (__, label) => __.V())
                .Verify();
        }

        [Fact]
        public virtual async Task WithSideEffect_label_can_be_selected()
        {
            await _g
                .WithSideEffect(
                    36,
                    (__, label) => __
                        .V()
                        .Select(label))
                .Verify();
        }
    }
}
