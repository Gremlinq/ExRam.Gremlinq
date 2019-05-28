using System;
using System.Linq;
using ExRam.Gremlinq.Core.GraphElements;
using ExRam.Gremlinq.Core.Serialization;
using ExRam.Gremlinq.Tests.Entities;
using FluentAssertions;
using Xunit;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public abstract class GroovySerializationTest<TVisitor> where TVisitor : IGremlinQueryElementVisitor<SerializedGremlinQuery>, new()
    {
        protected GroovySerializationTest()
        {
            var vertexType = typeof(Vertex);
        }

        [Fact]
        public void AddE_from_StepLabel()
        {
            g
                .AddV(new Country { CountryCallingCode = "+49" })
                .As((_, c) => _
                    .AddV(new Language { IetfLanguageTag = "en" })
                    .AddE<Speaks>()
                    .From(c))
                .Should()
                .SerializeToGroovy<TVisitor>("g.addV(_a).property(single, _b, _c).as(_d).addV(_e).property(single, _f, _g).addE(_h).from(_d)")
                .WithParameters("Country", "CountryCallingCode", "+49", "l1", "Language", "IetfLanguageTag", "en", "Speaks");
        }

        [Fact]
        public void AddE_from_traversal()
        {
            var now = DateTimeOffset.UtcNow;

            g
                .AddV(new Person
                {
                    Name = "Bob",
                    RegistrationDate = now
                })
                .AddE(new LivesIn())
                .From(__ => __
                    .V<Country>()
                    .Where(t => t.CountryCallingCode == "+49"))
                .Should()
                .SerializeToGroovy<TVisitor>("g.addV(_a).property(single, _b, _c).property(single, _d, _e).property(single, _f, _g).property(single, _h, _i).addE(_j).from(__.V().hasLabel(_k).has(_l, _m))")
                .WithParameters("Person", "Age", 0, "Gender", 0, "Name", "Bob", "RegistrationDate", now, "LivesIn", "Country", "CountryCallingCode", "+49");
        }

        [Fact]
        public void AddE_InV()
        {
            g
                .AddV<Person>()
                .AddE<LivesIn>()
                .To(__ => __
                    .V<Country>("id"))
                .InV()
                .Should()
                .SerializeToGroovy<TVisitor>("g.addV(_a).property(single, _b, _c).property(single, _d, _e).property(single, _f, _g).addE(_h).to(__.V(_i).hasLabel(_j)).inV()");
        }

        [Fact]
        public void AddE_OutV()
        {
            g
                .AddV<Person>()
                .AddE<LivesIn>()
                .To(__ => __
                    .V<Country>("id"))
                .OutV()
                .Should()
                .SerializeToGroovy<TVisitor>("g.addV(_a).property(single, _b, _c).property(single, _d, _e).property(single, _f, _g).addE(_h).to(__.V(_i).hasLabel(_j)).outV()");
        }

        [Fact]
        public void AddE_property()
        {
            g
                .AddV<Person>()
                .AddE(new LivesIn
                {
                    Since = DateTimeOffset.Now
                })
                .To(__ => __
                    .V<Country>("id"))
                .Should()
                .SerializeToGroovy<TVisitor>("g.addV(_a).property(single, _b, _c).property(single, _d, _e).property(single, _f, _g).addE(_h).property(_i, _j).to(__.V(_k).hasLabel(_l))");
        }

        [Fact]
        public void AddE_to_StepLabel()
        {
            g
                .AddV(new Language { IetfLanguageTag = "en" })
                .As((_, l) => _
                    .AddV(new Country { CountryCallingCode = "+49" })
                    .AddE<Speaks>()
                    .To(l))
                .Should()
                .SerializeToGroovy<TVisitor>("g.addV(_a).property(single, _b, _c).as(_d).addV(_e).property(single, _f, _g).addE(_h).to(_d)")
                .WithParameters("Language", "IetfLanguageTag", "en", "l1", "Country", "CountryCallingCode", "+49", "Speaks");
        }

        [Fact]
        public void AddE_to_traversal()
        {
            var now = DateTimeOffset.UtcNow;

            g
                .AddV(new Person
                {
                    Name = "Bob",
                    RegistrationDate = now
                })
                .AddE(new LivesIn())
                .To(__ => __
                    .V<Country>()
                    .Where(t => t.CountryCallingCode == "+49"))
                .Should()
                .SerializeToGroovy<TVisitor>("g.addV(_a).property(single, _b, _c).property(single, _d, _e).property(single, _f, _g).property(single, _h, _i).addE(_j).to(__.V().hasLabel(_k).has(_l, _m))")
                .WithParameters("Person", "Age", 0, "Gender", 0, "Name", "Bob", "RegistrationDate", now, "LivesIn", "Country", "CountryCallingCode", "+49");
        }

        [Fact]
        public void AddE_With_Ignored()
        {
            var now = DateTime.UtcNow;

            g
                .ConfigureModel(model => model
                    .ConfigureProperties(_ => _
                        .ConfigureElement<WorksFor>(builder => builder
                            .IgnoreAlways(p => p.From)
                            .IgnoreAlways(p => p.Role))))
               .AddE(new WorksFor { From = now, To = now, Role = "Admin" })
               .Should()
               .SerializeToGroovy<TVisitor>("g.addE(_a).property(_b, _c)")
               .WithParameters("WorksFor", nameof(WorksFor.To), now);
        }

        [Fact]
        public void AddV()
        {
            g
                .AddV(new Language { Id = 1, IetfLanguageTag = "en" })
                .Should()
                .SerializeToGroovy<TVisitor>("g.addV(_a).property(id, _b).property(single, _c, _d)")
                .WithParameters("Language", 1, "IetfLanguageTag", "en");
        }

        [Fact]
        public void AddV_list_cardinality_id()
        {
            g
                .WithModel(GraphModel
                    .FromBaseTypes<VertexWithListAsId, Edge>())
                .AddV(new VertexWithListAsId { Id = new[] { "123", "456" } })
                .Invoking(x => new GroovyGremlinQueryElementVisitor().Visit(x))
                .Should()
                .Throw<NotSupportedException>();
        }

        [Fact]
        public void AddV_with_enum_property()
        {
            g
                .AddV(new Person { Id = 1, Gender = Gender.Female })
                .Should()
                .SerializeToGroovy<TVisitor>("g.addV(_a).property(id, _b).property(single, _c, _d).property(single, _e, _f).property(single, _g, _h)")
                .WithParameters("Person", 1, "Age", 0, "Gender" , 1, "RegistrationDate", DateTimeOffset.MinValue);
        }

        [Fact]
        public void AddV_With_Ignored()
        {
            var now = DateTimeOffset.UtcNow;
            var person = new Person { Age = 21, Gender = Gender.Male, Name = "Marko", RegistrationDate = now };

            g
                .ConfigureModel(model => model
                    .ConfigureProperties(_ => _
                        .ConfigureElement<Person>(builder => builder
                            .IgnoreAlways(p => p.Age)
                            .IgnoreAlways(p => p.Gender))))
               .AddV(person)
               .Should()
               .SerializeToGroovy<TVisitor>("g.addV(_a).property(single, _b, _c).property(single, _d, _e)")
               .WithParameters("Person", "Name", "Marko", "RegistrationDate", now);
        }

        [Fact]
        public void AddV_with_ignored_property()
        {
            g
                .ConfigureModel(model => model
                    .ConfigureProperties(_ => _
                        .ConfigureElement<Language>(builder => builder
                            .IgnoreOnAdd(p => p.IetfLanguageTag))))
                .AddV(new Language { Id = 1, IetfLanguageTag = "en" })
                .Should()
                .SerializeToGroovy<TVisitor>("g.addV(_a).property(id, _b)")
                .WithParameters("Language", 1);
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
                .SerializeToGroovy<TVisitor>("g.addV(_a).property(id, _b).property(single, _c, _d, _e, _f, _g, _h)")
                .WithParameters("Country", 1, "Name", "GER", "de", "Deutschland", "en", "Germany");
        }

        [Fact]
        public void AddV_with_Meta_without_properties()
        {
            g
                .AddV(new Country { Id = 1, Name = "GER"})
                .Should()
                .SerializeToGroovy<TVisitor>("g.addV(_a).property(id, _b).property(single, _c, _d)")
                .WithParameters("Country", 1, "Name", "GER");
        }

        [Fact]
        public void AddV_with_MetaModel()
        {
            g
                .AddV(new Company
                {
                    Id = 1,
                    Name = new[]
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
                .Should()
                .SerializeToGroovy<TVisitor>("g.addV(_a).property(id, _b).property(single, _c, _d).property(list, _e, _f, _g, _h)")
                .WithParameters("Company", 1, "FoundingDate", DateTime.MinValue, "Name", "Bob", "ValidFrom", DateTimeOffset.Parse("01.01.2019 08:00"));
        }

        [Fact]
        public void AddV_with_multi_property()
        {
            g
                .AddV(new Company { Id = 1, PhoneNumbers = new[] { "+4912345", "+4923456" } })
                .Should()
                .SerializeToGroovy<TVisitor>("g.addV(_a).property(id, _b).property(single, _c, _d).property(list, _e, _f).property(list, _e, _g)")
                .WithParameters("Company", 1, "FoundingDate", DateTime.MinValue, "PhoneNumbers", "+4912345", "+4923456");
        }

        [Fact]
        public void AddV_with_nulls()
        {
            g
                .AddV(new Language { Id = 1 })
                .Should()
                .SerializeToGroovy<TVisitor>("g.addV(_a).property(id, _b)")
                .WithParameters("Language", 1);
        }

        [Fact]
        public void AddV_with_overridden_name()
        {
            g
                .ConfigureModel(model => model
                    .ConfigureProperties(propModel => propModel
                        .ConfigureElement<Language>(conf => conf
                            .ConfigureName(x => x.IetfLanguageTag, "lang"))))
                .AddV(new Language { Id = 1, IetfLanguageTag = "en" })
                .Should()
                .SerializeToGroovy<TVisitor>("g.addV(_a).property(id, _b).property(single, _c, _d)")
                .WithParameters("Language", 1, "lang", "en");
        }

        [Fact]
        public void AddV_without_id()
        {
            g
                .AddV(new Language { IetfLanguageTag = "en" })
                .Should()
                .SerializeToGroovy<TVisitor>("g.addV(_a).property(single, _b, _c)")
                .WithParameters("Language", "IetfLanguageTag", "en");
        }

        [Fact]
        public void AddV_without_model()
        {
            g
                .WithModel(GraphModel.Empty)
                .AddV(new Language { Id = 1, IetfLanguageTag = "en" })
                .Should()
                .SerializeToGroovy<TVisitor>("g.addV(_a).property(id, _b).property(single, _c, _d)")
                .WithParameters("Language", 1, "IetfLanguageTag", "en");
        }

        [Fact]
        public void And()
        {
            g
                .V<Person>()
                .And(
                    __ => __
                        .InE<WorksFor>(),
                    __ => __
                        .OutE<LivesIn>())
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).and(__.inE(_b), __.outE(_c))")
                .WithParameters("Person", "WorksFor", "LivesIn");
        }

        [Fact]
        public void And_nested()
        {
            g
                .V<Person>()
                .And(
                    __ => __
                        .OutE<LivesIn>(),
                    __ => __
                        .And(
                            ___ => ___
                                .InE<WorksFor>(),
                            ___ => ___
                                .OutE<WorksFor>()))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).and(__.outE(_b), __.inE(_c), __.outE(_c))")
                .WithParameters("Person", "LivesIn", "WorksFor");
        }

        [Fact]
        public void Anonymous()
        {
            GremlinQuery.Anonymous()
                .Should()
                .SerializeToGroovy<TVisitor>("__.identity()")
                .WithoutParameters();
        }

        [Fact]
        public void As_explicit_label1()
        {
            g
                .V<Person>()
                .As(new StepLabel<Person>())
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).as(_b)")
                .WithParameters("Person", "l1");
        }

        [Fact]
        public void As_explicit_label2()
        {
            g
                .V<Person>()
                .As(new StepLabel<Person>(), new StepLabel<Person>())
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).as(_b, _c)")
                .WithParameters("Person", "l1", "l2");
        }

        [Fact]
        public void As_inlined_nested_Select()
        {
            g
                .V<Person>()
                .As((_, stepLabel1) => _
                    .As((__, stepLabel2) => __
                        .Select(stepLabel1, stepLabel2)))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).as(_b).as(_c).select(_b, _c)")
                .WithParameters("Person", "<1>Item1", "<2>Item2");
        }

        [Fact]
        public void Choose_Predicate1()
        {
            g
                .V()
                .Id()
                .Choose(
                    x => x == (object)42,
                    _ => _.Constant(true),
                    _ => _.Constant(false))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().id().choose(eq(_a), __.constant(_b), __.constant(_c))")
                .WithParameters(42, true, false);
        }

        [Fact]
        public void Choose_Predicate2()
        {
            g
                .V()
                .Id()
                .Choose(
                    x => x == (object)42,
                    _ => _.Constant(true))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().id().choose(eq(_a), __.constant(_b))")
                .WithParameters(42, true);
        }

        [Fact]
        public void Choose_Predicate3()
        {
            g
                .V()
                .Id()
                .Cast<int>()
                .Choose(
                    x => x < 42,
                    _ => _.Constant(true),
                    _ => _.Constant(false))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().id().choose(lt(_a), __.constant(_b), __.constant(_c))")
                .WithParameters(42, true, false);
        }

        [Fact]
        public void Choose_Predicate4()
        {
            g
                .V()
                .Id()
                .Cast<int>()
                .Choose(
                    x => 42 > x,
                    _ => _.Constant(true),
                    _ => _.Constant(false))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().id().choose(lt(_a), __.constant(_b), __.constant(_c))")
                .WithParameters(42, true, false);
        }

        [Fact]
        public void Choose_Predicate5()
        {
            g
                .V()
                .Id()
                .Cast<int>()
                .Choose(
                    x => 0 < x && x < 42,
                    _ => _.Constant(true),
                    _ => _.Constant(false))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().id().choose(gt(_a).and(lt(_b)), __.constant(_c), __.constant(_d))")
                .WithParameters(0, 42, true, false);
        }

        [Fact]
        public void Choose_Predicate6()
        {
            g
                .V()
                .Id()
                .Cast<int>()
                .Choose(
                    x => 0 < x && x < 42 || x != 37,
                    _ => _.Constant(true),
                    _ => _.Constant(false))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().id().choose(gt(_a).and(lt(_b)).or(neq(_c)), __.constant(_d), __.constant(_e))")
                .WithParameters(0, 42, 37, true, false);
        }

        [Fact]
        public void Choose_Predicate7()
        {
            g
                .V()
                .Id()
                .Cast<int>()
                .Choose(
                    x => 0 < x || x < 42 && x != 37,
                    _ => _.Constant(true),
                    _ => _.Constant(false))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().id().choose(gt(_a).or(lt(_b).and(neq(_c))), __.constant(_d), __.constant(_e))")
                .WithParameters(0, 42, 37, true, false);
        }

        [Fact]
        public void Choose_Traversal1()
        {
            g
                .V()
                .Choose(
                    _ => _.Values(),
                    _ => _.Out(),
                    _ => _.In())
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().choose(__.values(), __.out(), __.in())")
                .WithoutParameters();
        }

        [Fact]
        public void Choose_Traversal2()
        {
            g
                .V()
                .Choose(
                    _ => _.Values(),
                    _ => _.Out())
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().choose(__.values(), __.out())")
                .WithoutParameters();
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
        public void Constant()
        {
            g
                .V()
                .Constant(42)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().constant(_a)")
                .WithParameters(42);
        }

        [Fact]
        public void Count()
        {
            g
                .V()
                .Count()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().count()")
                .WithoutParameters();
        }

        [Fact]
        public void CountGlobal()
        {
            g
                .V()
                .Count()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().count()")
                .WithoutParameters();
        }

        [Fact]
        public void CountLocal()
        {
            g
                .V()
                .CountLocal()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().count(local)")
                .WithoutParameters();
        }

        [Fact]
        public void Drop()
        {
            g
                .V<Person>()
                .Drop()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).drop()")
                .WithParameters("Person");
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
        public void E_of_concrete_type()
        {
            g
                .E<WorksFor>()
                .Should()
                .SerializeToGroovy<TVisitor>("g.E().hasLabel(_a)")
                .WithParameters("WorksFor");
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
        public void FilterWithLambda()
        {
            g
                .V<Person>()
                .Where("it.property('str').value().length() == 2")
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).filter({it.property('str').value().length() == 2})")
                .WithParameters("Person");
        }

        [Fact]
        public void FlatMap()
        {
            g
                .V<Person>()
                .FlatMap(__ => __.Out<WorksFor>())
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).flatMap(__.out(_b))")
                .WithParameters("Person", "WorksFor");
        }

        [Fact]
        public void Fold()
        {
            g
                .V()
                .Fold()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().fold()")
                .WithoutParameters();
        }

        [Fact]
        public void Fold_Fold_Unfold_Unfold()
        {
            g
                .V()
                .Fold()
                .Fold()
                .Unfold()
                .Unfold()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().fold().fold().unfold().unfold()")
                .WithoutParameters();
        }

        [Fact]
        public void Fold_SideEffect()
        {
            g
                .V()
                .Fold()
                .SideEffect(x => x.Identity())
                .Unfold()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().fold().sideEffect(__.identity()).unfold()")
                .WithoutParameters();
        }

        [Fact]
        public void Fold_Unfold()
        {
            g
                .V()
                .Fold()
                .Unfold()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().fold().unfold()")
                .WithoutParameters();
        }

        [Fact]
        public void In()
        {
            g
                .V<Person>()
                .In<WorksFor>()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).in(_b)")
                .WithParameters("Person", "WorksFor");
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
        public void Inject()
        {
            g
                .Inject(36, 37, 38)
                .Should()
                .SerializeToGroovy<TVisitor>("g.inject(_a, _b, _c)")
                .WithParameters(36, 37, 38);
        }

        [Fact]
        public void Label()
        {
            g
                .V()
                .Label()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().label()")
                .WithoutParameters();
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
        public void LimitGlobal()
        {
            g
                .V()
                .Limit(1)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().limit(_a)")
                .WithParameters(1);
        }

        [Fact]
        public void LimitLocal()
        {
            g
                .V()
                .LimitLocal(1)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().limit(local, _a)")
                .WithParameters(1);
        }

        [Fact]
        public void Map()
        {
            g
                .V<Person>()
                .Map(__ => __.Out<WorksFor>())
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).map(__.out(_b))")
                .WithParameters("Person", "WorksFor");
        }

        [Fact]
        public void Map_Select_operation()
        {
            g
                .V<Person>()
                .As((_, stepLabel1) => _
                    .As((__, stepLabel2) => __
                        .Map(___ => ___
                            .Select(stepLabel1, stepLabel2))))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).as(_b).as(_c).map(__.select(_b, _c))")
                .WithParameters("Person", "<1>Item1", "<2>Item2");
        }

        [Fact]
        public void Nested_contradicting_Select_operations_throw()
        {
            g
                .V<Person>()
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
        public void Nested_Select_operations()
        {
            g
                .V<Person>()
                .As((_, stepLabel1) => _
                    .As((__, stepLabel2) => __
                        .Select(stepLabel1, stepLabel2)
                        .As((___, tuple) => ___
                            .Select(stepLabel1, tuple))))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).as(_b).as(_c).select(_b, _c).as(_d).select(_b, _d)")
                .WithParameters("Person", "<1>Item1", "<2>Item2", "<3>Item2");
        }

        [Fact]
        public void Not1()
        {
            g
                .V()
                .Not(__ => __.Out<WorksFor>())
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().not(__.out(_a))")
                .WithParameters("WorksFor");
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
                .WithParameters("Company", "Person");
        }

        [Fact]
        public void OfType_abstract()
        {
            g
                .V()
                .OfType<Authority>()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a, _b)")
                .WithParameters("Company", "Person");
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
        public void Optional()
        {
            g
                .V()
                .Optional(
                    __ => __.Out<WorksFor>())
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().optional(__.out(_a))")
                .WithParameters("WorksFor");
        }

        [Fact]
        public void Or()
        {
            g
                .V<Person>()
                .Or(
                    __ => __
                        .InE<WorksFor>(),
                    __ => __
                        .OutE<LivesIn>())
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).or(__.inE(_b), __.outE(_c))")
                .WithParameters("Person", "WorksFor", "LivesIn");
        }

        [Fact]
        public void Or_nested()
        {
            g
                .V<Person>()
                .Or(
                    __ => __
                        .OutE<LivesIn>(),
                    __ => __
                        .Or(
                            ___ => ___
                                .InE<WorksFor>(),
                            ___ => ___
                                .OutE<WorksFor>()))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).or(__.outE(_b), __.inE(_c), __.outE(_c))")
                .WithParameters("Person", "LivesIn", "WorksFor");
        }

        [Fact]
        public void Order_Fold_Unfold()
        {
            g
                .V()
                .OrderBy(x => x.Id)
                .Fold()
                .Unfold()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().order().by(id, incr).fold().unfold()");
        }

        [Fact]
        public void OrderBy_lambda()
        {
            g
                .V<Person>()
                .OrderBy("it.property('str').value().length()")
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).order().by({it.property('str').value().length()})")
                .WithParameters("Person");
        }

        [Fact]
        public void OrderBy_member()
        {
            g
                .V<Person>()
                .OrderBy(x => x.Name)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).order().by(_b, incr)")
                .WithParameters("Person", "Name");
        }

        [Fact]
        public void OrderBy_member_ThenBy_member()
        {
            g
                .V<Person>()
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Age)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).order().by(_b, incr).by(_c, incr)")
                .WithParameters("Person", "Name", "Age");
        }

        [Fact]
        public void OrderBy_ThenBy_lambda()
        {
            g
                .V<Person>()
                .OrderBy("it.property('str1').value().length()")
                .ThenBy("it.property('str2').value().length()")
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).order().by({it.property('str1').value().length()}).by({it.property('str2').value().length()})")
                .WithParameters("Person");
        }

        [Fact]
        public void OrderBy_ThenByDescending_member()
        {
            g
                .V<Person>()
                .OrderBy(x => x.Name)
                .ThenByDescending(x => x.Age)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).order().by(_b, incr).by(_c, decr)")
                .WithParameters("Person", "Name", "Age");
        }

        [Fact]
        public void OrderBy_ThenByDescending_traversal()
        {
            g
                .V<Person>()
                .OrderBy(__ => __.Values(x => x.Name))
                .ThenByDescending(__ => __.Gender)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).order().by(__.values(_b), incr).by(_c, decr)")
                .WithParameters("Person", "Name", "Gender");
        }

        [Fact]
        public void OrderBy_traversal()
        {
            g
                .V<Person>()
                .OrderBy(__ => __.Values(x => x.Name))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).order().by(__.values(_b), incr)")
                .WithParameters("Person", "Name");
        }

        [Fact]
        public void OrderBy_traversal_ThenBy()
        {
            g
                .V<Person>()
                .OrderBy(__ => __.Values(x => x.Name))
                .ThenBy(__ => __.Gender)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).order().by(__.values(_b), incr).by(_c, incr)")
                .WithParameters("Person", "Name", "Gender");
        }

        [Fact]
        public void OrderBy_traversal_ThenBy_traversal()
        {
            g
                .V<Person>()
                .OrderBy(__ => __.Values(x => x.Name))
                .ThenBy(__ => __.Values(x => x.Gender))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).order().by(__.values(_b), incr).by(__.values(_c), incr)")
                .WithParameters("Person", "Name", "Gender");
        }

        [Fact]
        public void OrderByDescending_member()
        {
            g
                .V<Person>()
                .OrderByDescending(x => x.Name)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).order().by(_b, decr)")
                .WithParameters("Person", "Name");
        }

        [Fact]
        public void OrderByDescending_traversal()
        {
            g
                .V<Person>()
                .OrderByDescending(__ => __.Values(x => x.Name))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).order().by(__.values(_b), decr)")
                .WithParameters("Person", "Name");
        }

        [Fact]
        public void Out()
        {
            g
                .V<Person>()
                .Out<WorksFor>()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).out(_b)")
                .WithParameters("Person", "WorksFor");
        }

        [Fact]
        public void Out_does_not_include_abstract_edge()
        {
            g
                .V<Person>()
                .Out<Edge>()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).out(_b, _c, _d)")
                .WithParameters("Person", "LivesIn", "Speaks", "WorksFor");
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
        public void Project2()
        {
            g
                .V()
                .Project(
                    __ => __.In(),
                    __ => __.Out())
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().project(_a, _b).by(__.in()).by(__.out())")
                .WithParameters("Item1", "Item2");
        }

        [Fact]
        public void Project3()
        {
            g
                .V()
                .Project(
                    __ => __.In(),
                    __ => __.Out(),
                    __ => __.Count())
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().project(_a, _b, _c).by(__.in()).by(__.out()).by(__.count())")
                .WithParameters("Item1", "Item2", "Item3");
        }

        [Fact]
        public void Project4()
        {
            g
                .V()
                .Project(
                    __ => __.In(),
                    __ => __.Out(),
                    __ => __.Count(),
                    __ => __.Properties())
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().project(_a, _b, _c, _d).by(__.in()).by(__.out()).by(__.count()).by(__.properties())")
                .WithParameters("Item1", "Item2", "Item3", "Item4");
        }

        [Fact]
        public void Properties_Meta()
        {
            g
                .V<Country>()
                .Properties(x => x.Name)
                .Meta<PropertyValidity>()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).properties(_b)")
                .WithParameters("Country", "Name");
        }

        [Fact]
        public void Properties_Meta_ValueMap()
        {
            g
                .V()
                .Properties()
                .Meta<PropertyValidity>()
                .ValueMap()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().properties().valueMap()")
                .WithoutParameters();
        }

        [Fact]
        public void Properties_Meta_Values()
        {
            g
                .V()
                .Properties()
                .Meta<PropertyValidity>()
                .Values()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().properties().values()")
                .WithoutParameters();
        }

        [Fact]
        public void Properties_Meta_Values_Projected()
        {
            g
                .V()
                .Properties()
                .Meta<PropertyValidity>()
                .Values(x => x.ValidFrom)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().properties().values(_a)")
                .WithParameters("ValidFrom");
        }

        [Fact]
        public void Properties_Meta_Where1()
        {
            g
                .V<Country>()
                .Properties(x => x.Name)
                .Meta<PropertyValidity>()
                .Where(x => x.Properties.ValidFrom >= DateTimeOffset.Parse("01.01.2019 08:00"))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).properties(_b).has(_c, gte(_d))")
                .WithParameters("Country", "Name", "ValidFrom", DateTimeOffset.Parse("01.01.2019 08:00"));
        }

        [Fact]
        public void Properties_name_typed()
        {
            g
                .V()
                .Properties<int>("propertyName")
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().properties(_a)")
                .WithParameters("propertyName");
        }

        [Fact]
        public void Properties_name_untyped()
        {
            g
                .V()
                .Properties("propertyName")
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().properties(_a)")
                .WithParameters("propertyName");
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
        public void Properties_of_three_members()
        {
            g
                .V<Country>()
                .Properties(x => x.Name, x => x.CountryCallingCode, x => x.Languages)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).properties(_b, _c, _d)")
                .WithParameters("Country", "Name", "CountryCallingCode", "Languages");
        }

        [Fact]
        public void Properties_of_two_members1()
        {
            g
                .V<Country>()
                .Properties(x => x.Name, x => x.CountryCallingCode)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).properties(_b, _c)")
                .WithParameters("Country", "Name", "CountryCallingCode");
        }

        [Fact]
        public void Properties_of_two_members2()
        {
            g
                .V<Country>()
                .Properties(x => x.Name, x => x.Languages)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).properties(_b, _c)")
                .WithParameters("Country", "Name", "Languages");
        }

        [Fact]
        public void Properties_Properties_as_select()
        {
            g
                .V<Country>()
                .Properties(x => x.Name)
                .Properties()
                .As((__, s) => __
                    .Select(s))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).properties(_b).properties().as(_c).select(_c)")
                .WithParameters("Country", "Name", "l1");
        }

        [Fact]
        public void Properties_Properties_key()
        {
            g
                .V<Country>()
                .Properties(x => x.Name)
                .Properties()
                .Key()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).properties(_b).properties().key()")
                .WithParameters("Country", "Name");
        }

        [Fact]
        public void Properties_Properties_Value()
        {
            g
                .V<Company>()
                .Properties(x => x.Name)
                .Properties()
                .Value()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).properties(_b).properties().value()")
                .WithParameters("Company", "Name");
        }

        [Fact]
        public void Properties_Properties_Where_key()
        {
            g
                .V<Company>()
                .Properties(x => x.Name)
                .Properties()
                .Where(x => x.Key == "someKey")
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).properties(_b).properties().where(__.key().is(_c))")
                .WithParameters("Company", "Name", "someKey");
        }

        [Fact]
        public void Properties_Properties_Where_key_equals_stepLabel()
        {
            var stepLabel = new StepLabel<string>();

            g
                .V<Company>()
                .Properties(x => x.Name)
                .Properties()
                .Where(x => x.Key == stepLabel)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).properties(_b).properties().where(__.key().where(eq(_c)))")
                .WithParameters("Company", "Name", "l1");
        }

        [Fact]
        public void Properties_Properties1()
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
        public void Properties_Properties2()
        {
            g
                .V<Company>()
                .Properties(x => x.Name)
                .Properties()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).properties(_b).properties()")
                .WithParameters("Company", "Name");
        }

        [Fact]
        public void Properties_typed_no_parameters()
        {
            g
                .V()
                .Properties<string>()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().properties()")
                .WithoutParameters();
        }

        [Fact]
        public void Properties_ValueMap_typed()
        {
            g
                .V()
                .Properties()
                .ValueMap<string>()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().properties().valueMap()")
                .WithoutParameters();
        }

        [Fact]
        public void Properties_ValueMap_untyped()
        {
            g
                .V()
                .Properties()
                .ValueMap()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().properties().valueMap()")
                .WithoutParameters();
        }

        [Fact]
        public void Properties_Values_Id()
        {
            g
                .V()
                .Properties()
                .Values(x => x.Id)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().properties().id()")
                .WithoutParameters();
        }

        [Fact]
        public void Properties_Values_Id_Label()
        {
            g
                .V()
                .Properties()
                .Values(
                    x => x.Label,
                    x => x.Id)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().properties().union(__.label(), __.id())")
                .WithoutParameters();
        }

        [Fact]
        public void Properties_Values_Label()
        {
            g
                .V()
                .Properties()
                .Values(x => x.Label)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().properties().label()")
                .WithoutParameters();
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
        public void Properties_Values2()
        {
            g
                .V()
                .Properties()
                .Values<int>("MetaProperty")
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().properties().values(_a)")
                .WithParameters("MetaProperty");
        }

        [Fact]
        public void Properties_Where_Dictionary_key1()
        {
            g
                .V<Person>()
                .Properties()
                .Where(x => x.Properties["MetaKey"] == "MetaValue")
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).properties().has(_b, _c)")
                .WithParameters("Person", "MetaKey", "MetaValue");
        }

        [Fact]
        public void Properties_Where_Dictionary_key2()
        {
            g
                .V<Person>()
                .Properties()
                .Where(x => (int)x.Properties["MetaKey"] < 100)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).properties().has(_b, lt(_c))")
                .WithParameters("Person", "MetaKey", 100);
        }

        [Fact]
        public void Properties_Where_Id()
        {
            g
                .V<Country>()
                .Properties(x => x.Languages)
                .Where(x => x.Id == "id")
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).properties(_b).has(id, _c)")
                .WithParameters("Country", "Languages", "id");
        }

        [Fact]
        public void Properties_Where_label()
        {
            g
                .V<Company>()
                .Properties(x => x.Name)
                .Where(x => x.Label == "someKey")
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).properties(_b).where(__.label().is(_c))")
                .WithParameters("Company", "Name", "someKey");
        }

        [Fact]
        public void Properties_Where_Label()
        {
            g
                .V<Country>()
                .Properties(x => x.Languages)
                .Where(x => x.Label == "label")
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).properties(_b).where(__.label().is(_c))")
                .WithParameters("Country", "Languages", "label");
        }

        [Fact]
        public void Properties_Where_Meta_key()
        {
            g
                .V<Company>()
                .Properties(x => x.Name)
                .Where(x => x.Properties.ValidFrom == DateTimeOffset.Parse("01.01.2019 08:00"))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).properties(_b).has(_c, _d)")
                .WithParameters("Company", "Name", "ValidFrom", DateTimeOffset.Parse("01.01.2019 08:00"));
        }

        [Fact]
        public void Properties_Where_Meta_key_reversed()
        {
            g
                .V<Company>()
                .Properties(x => x.Name)
                .Where(x => DateTimeOffset.Parse("01.01.2019 08:00") == x.Properties.ValidFrom)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).properties(_b).has(_c, _d)")
                .WithParameters("Company", "Name", "ValidFrom", DateTimeOffset.Parse("01.01.2019 08:00"));
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
        public void Properties_Where1()
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
        public void Properties_Where2()
        {
            g
                .V<Country>()
                .Properties()
                .Where(x => (int)x.Value < 10)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).properties().hasValue(lt(_b))")
                .WithParameters("Country", 10);
        }


        [Fact]
        public void Properties1()
        {
            g
                .V()
                .Properties()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().properties()")
                .WithoutParameters();
        }


        [Fact]
        public void Properties2()
        {
            g
                .E()
                .Properties()
                .Should()
                .SerializeToGroovy<TVisitor>("g.E().properties()")
                .WithoutParameters();
        }

        [Fact]
        public void Property_list()
        {
            g
                .V<Company>("id")
                .Property(x => x.PhoneNumbers, "+4912345")
                .Should()
                .SerializeToGroovy<TVisitor>("g.V(_a).hasLabel(_b).property(list, _c, _d)")
                .WithParameters("id", "Company", "PhoneNumbers", "+4912345");
        }

        [Fact]
        public void Property_null()
        {
            g
                .V<Company>("id")
                .Property<string>(x => x.PhoneNumbers, null)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V(_a).hasLabel(_b).sideEffect(__.properties(_c).drop())")
                .WithParameters("id", "Company", "PhoneNumbers");
        }

        [Fact]
        public void Property_single()
        {
            g
                .V<Person>()
                .Property(x => x.Age, 36)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).property(single, _b, _c)")
                .WithParameters("Person", "Age", 36);
        }

        [Fact]
        public void Repeat_Out()
        {
            g
                .V<Person>()
                .Repeat(__ => __
                    .Out<WorksFor>()
                    .OfType<Person>())
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).repeat(__.out(_b).hasLabel(_a))")
                .WithParameters("Person", "WorksFor");
        }

        [Fact]
        public void ReplaceE()
        {
            var now = DateTime.UtcNow;
            var id = Guid.NewGuid();

            var worksFor = new WorksFor { Id = id, From = now, To = now, Role = "Admin" };

            g
                .ReplaceE(worksFor)
                .Should()
                .SerializeToGroovy<TVisitor>("g.E(_a).hasLabel(_b).sideEffect(__.properties(_c, _d, _e).drop()).property(_c, _f).property(_d, _g).property(_e, _f)")
                .WithParameters(id, nameof(WorksFor), nameof(WorksFor.From), nameof(WorksFor.Role), nameof(WorksFor.To), now, "Admin");
        }

        [Fact]
        public void ReplaceE_With_Config()
        {
            var now = DateTime.UtcNow;
            var id = Guid.NewGuid();
            var worksFor = new WorksFor { Id = id, From = now, To = now, Role = "Admin" };

            g
                .ConfigureModel(model => model
                    .ConfigureProperties(_ => _
                        .ConfigureElement<WorksFor>(builder => builder
                            .IgnoreOnUpdate(p => p.Id))))
                .ReplaceE(worksFor)
                .Should()
                .SerializeToGroovy<TVisitor>("g.E(_a).hasLabel(_b).sideEffect(__.properties(_c, _d, _e).drop()).property(_c, _f).property(_d, _g).property(_e, _f)")
                .WithParameters(id, nameof(WorksFor), nameof(WorksFor.From), nameof(WorksFor.Role), nameof(WorksFor.To), now, "Admin");
        }

        [Fact]
        public void ReplaceV()
        {
            var now = DateTimeOffset.UtcNow;
            var id = Guid.NewGuid();
            var person = new Person { Id = id, Age = 21, Gender = Gender.Male, Name = "Marko", RegistrationDate = now };

            g
                .ReplaceV(person)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V(_a).hasLabel(_b).sideEffect(__.properties(_c, _d, _e, _f).drop()).property(single, _c, _g).property(single, _d, _h).property(single, _e, _i).property(single, _f, _j)")
                .WithParameters(id, nameof(Person), nameof(Person.Age), nameof(Person.Gender), nameof(Person.Name), nameof(Person.RegistrationDate), 21, Gender.Male, "Marko", now);
        }

        [Fact]
        public void ReplaceV_With_Config()
        {
            var now = DateTimeOffset.UtcNow;
            var id = Guid.NewGuid();
            var person = new Person { Id = id, Age = 21, Gender = Gender.Male, Name = "Marko", RegistrationDate = now };

            g
                .ConfigureModel(model => model
                    .ConfigureProperties(_ => _
                        .ConfigureElement<Person>(builder => builder
                            .IgnoreOnUpdate(p => p.RegistrationDate))))
                .ReplaceV(person)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V(_a).hasLabel(_b).sideEffect(__.properties(_c, _d, _e).drop()).property(single, _c, _f).property(single, _d, _g).property(single, _e, _h)")
                .WithParameters(id, nameof(Person), nameof(Person.Age), nameof(Person.Gender), nameof(Person.Name), 21, Gender.Male, "Marko");
        }

        [Fact]
        public void Select()
        {
            var stepLabel = new StepLabel<Person>();

            g
                .V<Person>()
                .As(stepLabel)
                .Select(stepLabel)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).as(_b).select(_b)")
                .WithParameters("Person", "l1");
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
        public void Set_Meta_Property1()
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
        public void Set_Meta_Property2()
        {
            var d = DateTimeOffset.Now;

            g
                .V<Person>()
                .Properties(x => x.Name)
                .Property(x => x.ValidFrom, d)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).properties(_b).property(_c, _d)")
                .WithParameters("Person", "Name", "ValidFrom", d);
        }

        [Fact]
        public void SumGlobal()
        {
            g
                .V<Person>()
                .Values(x => x.Age)
                .SumGlobal()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).values(_b).sum(global)")
                .WithParameters("Person", "Age");
        }

        [Fact]
        public void SumLocal()
        {
            g
                .V<Person>()
                .Values(x => x.Age)
                .SumLocal()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).values(_b).sum(local)")
                .WithParameters("Person", "Age");
        }


        [Fact]
        public void SumLocal_Where1()
        {
            g
                .V<Person>()
                .Values(x => x.Age)
                .SumLocal()
                .Where(x => x == 100)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).values(_b).sum(local).is(_c)")
                .WithParameters("Person", "Age", 100);
        }

        [Fact]
        public void SumLocal_Where2()
        {
            g
                .V<Person>()
                .Values(x => x.Age)
                .SumLocal()
                .Where(x => x < 100)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).values(_b).sum(local).is(lt(_c))")
                .WithParameters("Person", "Age", 100);
        }

        [Fact]
        public void Tail_underflow()
        {
            g
                .V()
                .Invoking(_ => _.Tail(-1))
                .Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public void TailGlobal()
        {
            g
                .V()
                .Tail(1)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().tail(_a)")
                .WithParameters(1);
        }

        [Fact]
        public void TailLocal()
        {
            g
                .V()
                .TailLocal(1)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().tail(local, _a)")
                .WithParameters(1);
        }

        [Fact]
        public void Union()
        {
            g
                .V<Person>()
                .Union(
                    __ => __.Out<WorksFor>(),
                    __ => __.Out<LivesIn>())
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).union(__.out(_b), __.out(_c))")
                .WithParameters("Person", "WorksFor", "LivesIn");
        }

        [Fact]
        public void Union_untyped()
        {
            IVertexGremlinQuery q = g
                .V<Person>();

            q
                .Union(
                    __ => __,
                    __ => __.Out<LivesIn>())
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).union(__.identity(), __.out(_b))")
                .WithParameters("Person", "LivesIn");
        }

        [Fact]
        public void Update_Vertex_And_Edge_No_Config()
        {
            var now = DateTimeOffset.UtcNow;
            var edgeNow = DateTime.UtcNow;
            var person = new Person { Age = 21, Gender = Gender.Male, Name = "Marko", RegistrationDate = now };
            var worksFor = new WorksFor { From = edgeNow, To = edgeNow, Role = "Admin" };

            g
                .V<Person>()
                .Update(person)
                .OutE<WorksFor>()
                .Update(worksFor)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).sideEffect(__.properties(_b, _c, _d, _e).drop()).property(single, _b, _f).property(single, _c, _g).property(single, _d, _h).property(single, _e, _i).outE(_j).sideEffect(__.properties(_k, _l, _m).drop()).property(_k, _n).property(_l, _o).property(_m, _n)")
                .WithParameters(nameof(Person), nameof(Person.Age), nameof(Person.Gender), nameof(Person.Name), nameof(Person.RegistrationDate), person.Age, person.Gender, "Marko", person.RegistrationDate, nameof(WorksFor), nameof(WorksFor.From), nameof(WorksFor.Role), nameof(WorksFor.To), worksFor.From, worksFor.Role);
        }

        [Fact]
        public void Update_Vertex_And_Edge_With_Config()
        {
            var now = DateTimeOffset.UtcNow;
            var edgeNow = DateTime.UtcNow;
            var person = new Person { Age = 21, Gender = Gender.Male, Name = "Marko", RegistrationDate = now };
            var worksFor = new WorksFor { From = edgeNow, To = edgeNow, Role = "Admin" };

            g
                .ConfigureModel(model => model
                    .ConfigureProperties(_ => _
                        .ConfigureElement<Person>(builder => builder
                            .IgnoreOnUpdate(p => p.Age)
                            .IgnoreAlways(p => p.Name))
                    .ConfigureElement<WorksFor>(builder => builder
                        .IgnoreAlways(p => p.From)
                        .IgnoreOnUpdate(p => p.Role))))
                .V<Person>()
                .Update(person)
                .OutE<WorksFor>()
                .Update(worksFor)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).sideEffect(__.properties(_b, _c).drop()).property(single, _b, _d).property(single, _c, _e).outE(_f).sideEffect(__.properties(_g).drop()).property(_g, _h)")
                .WithParameters(nameof(Person), nameof(Person.Gender), nameof(Person.RegistrationDate), person.Gender, person.RegistrationDate, nameof(WorksFor), nameof(WorksFor.To), worksFor.To);
        }

        [Fact]
        public void UpdateE_With_Ignored()
        {
            var now = DateTime.UtcNow;

            g
                .ConfigureModel(model => model
                    .ConfigureProperties(_ => _
                        .ConfigureElement<WorksFor>(builder => builder
                            .IgnoreAlways(p => p.From)
                            .IgnoreAlways(p => p.Role))))
                .E<WorksFor>()
                .Update(new WorksFor { From = now, To = now, Role = "Admin" })
                .Should()
                .SerializeToGroovy<TVisitor>("g.E().hasLabel(_a).sideEffect(__.properties(_b).drop()).property(_b, _c)")
                .WithParameters(nameof(WorksFor), nameof(WorksFor.To), now);
        }

        [Fact]
        public void UpdateE_With_Mixed()
        {
            var now = DateTime.UtcNow;

            g
                .ConfigureModel(model => model
                    .ConfigureProperties(_ => _
                        .ConfigureElement<WorksFor>(builder => builder
                            .IgnoreAlways(p => p.From)
                            .IgnoreOnUpdate(p => p.Role))))
                .E<WorksFor>()
                .Update(new WorksFor { From = now, To = now, Role = "Admin" })
                .Should()
                .SerializeToGroovy<TVisitor>("g.E().hasLabel(_a).sideEffect(__.properties(_b).drop()).property(_b, _c)")
                .WithParameters(nameof(WorksFor), nameof(WorksFor.To), now);
        }

        [Fact]
        public void UpdateE_With_Readonly()
        {
            var now = DateTime.UtcNow;

            g
                .ConfigureModel(model => model
                    .ConfigureProperties(_ => _
                        .ConfigureElement<WorksFor>(builder => builder
                            .IgnoreOnUpdate(p => p.From)
                            .IgnoreOnUpdate(p => p.Role))))
                .E<WorksFor>()
                .Update(new WorksFor { From = now, To = now, Role = "Admin" })
                .Should()
                .SerializeToGroovy<TVisitor>("g.E().hasLabel(_a).sideEffect(__.properties(_b).drop()).property(_b, _c)")
                .WithParameters(nameof(WorksFor), nameof(WorksFor.To), now);
        }

        [Fact]
        public void UpdateV_No_Config()
        {
            var now = DateTimeOffset.UtcNow;

            g
                .V<Person>()
                .Update(new Person { Age = 21, Gender = Gender.Male, Name = "Marko", RegistrationDate = now })
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).sideEffect(__.properties(_b, _c, _d, _e).drop()).property(single, _b, _f).property(single, _c, _g).property(single, _d, _h).property(single, _e, _i)")
                .WithParameters(nameof(Person), nameof(Person.Age), nameof(Person.Gender), nameof(Person.Name), nameof(Person.RegistrationDate), 21, Gender.Male, "Marko", now);
        }

        [Fact]
        public void UpdateV_With_Ignored()
        {
            var now = DateTimeOffset.UtcNow;
            var person = new Person { Age = 21, Gender = Gender.Male, Name = "Marko", RegistrationDate = now };

            g
                .ConfigureModel(model => model
                    .ConfigureProperties(_ => _
                        .ConfigureElement<Person>(builder => builder
                            .IgnoreAlways(p => p.Age)
                            .IgnoreAlways(p => p.Gender))))
                .V<Person>()
                .Update(person)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).sideEffect(__.properties(_b, _c).drop()).property(single, _b, _d).property(single, _c, _e)")
                .WithParameters(nameof(Person), nameof(Person.Name), nameof(Person.RegistrationDate), "Marko", now);
        }

        [Fact]
        public void UpdateV_With_Mixed()
        {
            var now = DateTimeOffset.UtcNow;
            var person = new Person { Age = 21, Gender = Gender.Male, Name = "Marko", RegistrationDate = now };

            g
                .ConfigureModel(model => model
                    .ConfigureProperties(_ => _
                        .ConfigureElement<Person>(builder => builder
                            .IgnoreOnUpdate(p => p.Age)
                            .IgnoreAlways(p => p.Gender))))
                .V<Person>()
                .Update(person)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).sideEffect(__.properties(_b, _c).drop()).property(single, _b, _d).property(single, _c, _e)")
                .WithParameters(nameof(Person), nameof(Person.Name), nameof(Person.RegistrationDate), "Marko", now);
        }

        [Fact]
        public void UpdateV_With_Readonly()
        {
            var now = DateTimeOffset.UtcNow;
            var person = new Person { Age = 21, Gender = Gender.Male, Name = "Marko", RegistrationDate = now };

            g
                .ConfigureModel(model => model
                    .ConfigureProperties(_ => _
                        .ConfigureElement<Person>(builder => builder
                            .IgnoreOnUpdate(p => p.Age)
                            .IgnoreOnUpdate(p => p.Gender))))
                .V<Person>()
                .Update(person)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).sideEffect(__.properties(_b, _c).drop()).property(single, _b, _d).property(single, _c, _e)")
                .WithParameters(nameof(Person), nameof(Person.Name), nameof(Person.RegistrationDate), "Marko", now);
        }

        [Fact]
        public void V_of_abstract_type()
        {
            g
                .V<Authority>()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a, _b)")
                .WithParameters("Company", "Person");
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
        public void V_of_concrete_type()
        {
            g
                .V<Person>()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a)")
                .WithParameters("Person");
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
        public void Value()
        {
            g
                .V()
                .Properties()
                .Value()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().properties().value()");
        }

        [Fact]
        public void ValueMap_typed()
        {
            g
                .V<Person>()
                .ValueMap(x => x.Age)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).valueMap(_b)")
                .WithParameters("Person", "Age");
        }

        [Fact]
        public void ValueMap_untyped()
        {
            g
                .V<Person>()
                .ValueMap("key")
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).valueMap(_b)")
                .WithParameters("Person", "key");
        }

        [Fact]
        public void Values_1_member()
        {
            g
                .V<Person>()
                .Values(x => x.Age)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).values(_b)")
                .WithParameters("Person", "Age");
        }

        [Fact]
        public void Values_2_members()
        {
            g
                .V<Person>()
                .Values(x => x.Name, x => x.Id)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).union(__.id(), __.values(_b))")
                .WithParameters("Person", "Name");
        }

        [Fact]
        public void Values_3_members()
        {
            g
                .V<Person>()
                .Values(x => x.Name, x => x.Gender, x => x.Id)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).union(__.id(), __.values(_b, _c))")
                .WithParameters("Person", "Name", "Gender");
        }

        [Fact]
        public void Values_id_member()
        {
            g
                .V<Person>()
                .Values(x => x.Id)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).id()")
                .WithParameters("Person");
        }

        [Fact]
        public void Values_no_member()
        {
            g
                .V<Person>()
                .Values()
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).values()")
                .WithParameters("Person");
        }

        [Fact]
        public void Values_of_Edge()
        {
            g
                .E<LivesIn>()
                .Values(x => x.Since)
                .Should()
                .SerializeToGroovy<TVisitor>("g.E().hasLabel(_a).values(_b)")
                .WithParameters("LivesIn", "Since");
        }

        [Fact]
        public void Values_of_Vertex1()
        {
            g
                .V<Person>()
                .Values(x => x.Name)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).values(_b)")
                .WithParameters("Person", "Name");
        }

        [Fact]
        public void Values_of_Vertex2()
        {
            g
                .V<Person>()
                .Values(x => x.Name)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).values(_b)")
                .WithParameters("Person", "Name");
        }

        [Fact]
        public void Values_string_key()
        {
            g
                .V<Person>()
                .Values("key")
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).values(_b)")
                .WithParameters("Person", "key");
        }

        [Fact]
        public void Variable_wrap()
        {
            g
                .V()
                .Properties()
                .Properties("1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30")
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().properties().properties(_a, _b, _c, _d, _e, _f, _g, _h, _i, _j, _k, _l, _m, _n, _o, _p, _q, _r, _s, _t, _u, _v, _w, _x, _y, _z, _ba, _bb, _bc, _bd)")
                .WithParameters("1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30");
        }

        [Fact]
        public void Where_array_does_not_intersect_property_array()
        {
            g
                .V<Company>()
                .Where(t => !new[] { "+4912345", "+4923456" }.Intersect(t.PhoneNumbers).Any())
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).not(__.has(_b, within(_c, _d)))")
                .WithParameters("Company", "PhoneNumbers", "+4912345", "+4923456");
        }

        [Fact]
        public void Where_array_intersects_property_aray()
        {
            g
                .V<Company>()
                .Where(t => new[] { "+4912345", "+4923456" }.Intersect(t.PhoneNumbers).Any())
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, within(_c, _d))")
                .WithParameters("Company", "PhoneNumbers", "+4912345", "+4923456");
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
        public void Where_complex_logical_expression()
        {
            g
                .V<Person>()
                .Where(t => t.Name.Value == "Some name" && (t.Age == 42 || t.Age == 99))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, _c).has(_d, eq(_e).or(eq(_f)))")
                .WithParameters("Person", "Name", "Some name", "Age", 42, 99);
        }

        [Fact]
        public void Where_complex_logical_expression_with_null()
        {
            g
                .V<Person>()
                .Where(t => t.Name == null && (t.Age == 42 || t.Age == 99))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).hasNot(_b).has(_c, eq(_d).or(eq(_e)))")
                .WithParameters("Person", "Name", "Age", 42, 99);
        }

        [Fact]
        public void Where_conjunction()
        {
            g
                .V<Person>()
                .Where(t => t.Age == 36 && t.Age == 42)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, eq(_c).and(eq(_d)))")
                .WithParameters("Person", "Age", 36, 42);
        }

        [Fact(Skip="Optimizable")]
        public void Where_conjunction_optimizable()
        {
            g
                .V<Person>()
                .Where(t => (t.Age == 36 && t.Name.Value == "Hallo") && t.Age == 42)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, eq(_c).and(eq(_d))).has(_b, eq(_e))")
                .WithParameters("Person", "Age", 36, "Name", 42);
        }

        [Fact]
        public void Where_conjunction_with_different_fields()
        {
            g
                .V<Person>()
                .Where(t => t.Name.Value == "Some name" && t.Age == 42)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, _c).has(_d, _e)")
                .WithParameters("Person", "Name", "Some name", "Age", 42);
        }

        [Fact]
        public void Where_converted_Id_equals_constant()
        {
            g
                .V<Language>()
                .Where(t => (int)t.Id == 1)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(id, _b)")
                .WithParameters("Language", 1);
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
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).as(_b).V().hasLabel(_a).where(eq(_b))")
                .WithParameters("Language", "l1");
        }

        [Fact]
        public void Where_disjunction()
        {
            g
                .V<Person>()
                .Where(t => t.Age == 36 || t.Age == 42)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, eq(_c).or(eq(_d)))")
                .WithParameters("Person", "Age", 36, 42);
        }

        [Fact]
        public void Where_disjunction_with_different_fields()
        {
            g
                .V<Person>()
                .Where(t => t.Name.Value == "Some name" || t.Age == 42)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).or(__.has(_b, _c), __.has(_d, _e))")
                .WithParameters("Person", "Name", "Some name", "Age", 42);
        }

        [Fact]
        public void Where_empty_array_does_not_intersect_property_array()
        {
            g
                .V<Company>()
                .Where(t => !new string[0].Intersect(t.PhoneNumbers).Any())
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a)")
                .WithParameters("Company");
        }

        [Fact]
        public void Where_has_conjunction_of_three()
        {
            g
                .V<Person>()
                .Where(t => t.Age == 36 && t.Age == 42 && t.Age == 99)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, eq(_c).and(eq(_d)).and(eq(_e)))")
                .WithParameters("Person", "Age", 36, 42, 99);
        }

        [Fact]
        public void Where_has_disjunction_of_three()
        {
            g
                .V<Person>()
                .Where(t => t.Age == 36 || t.Age == 42 || t.Age == 99)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, eq(_c).or(eq(_d)).or(eq(_e)))")
                .WithParameters("Person", "Age", 36, 42, 99);
        }

        [Fact]
        public void Where_Id_equals_constant()
        {
            g
                .V<Language>()
                .Where(t => t.Id == (object)1)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(id, _b)")
                .WithParameters("Language", 1);
        }

        [Fact]
        public void Where_property_array_contains_element()
        {
            g
                .V<Company>()
                .Where(t => t.PhoneNumbers.Contains("+4912345"))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, _c)")
                .WithParameters("Company", "PhoneNumbers", "+4912345");
        }

        [Fact]
        public void Where_property_array_contains_stepLabel()
        {
            g
                .Inject("+4912345")
                .As((__, t) => __
                    .V<Company>()
                    .Where(c => c.PhoneNumbers.Contains(t)))
                .Should()
                .SerializeToGroovy<TVisitor>("g.inject(_a).as(_b).V().hasLabel(_c).has(_d, __.where(eq(_b)))")
                .WithParameters("+4912345", "l1", "Company", "PhoneNumbers");
        }

        [Fact]
        public void Where_property_array_does_not_contain_element()
        {
            g
                .V<Company>()
                .Where(t => !t.PhoneNumbers.Contains("+4912345"))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).not(__.has(_b, _c))")
                .WithParameters("Company", "PhoneNumbers", "+4912345");
        }

        [Fact]
        public void Where_property_array_does_not_intersect_array()
        {
            g
                .V<Company>()
                .Where(t => !t.PhoneNumbers.Intersect(new[] { "+4912345", "+4923456" }).Any())
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).not(__.has(_b, within(_c, _d)))")
                .WithParameters("Company", "PhoneNumbers", "+4912345", "+4923456");
        }

        [Fact]
        public void Where_property_array_does_not_intersect_empty_array()
        {
            g
                .V<Company>()
                .Where(t => !t.PhoneNumbers.Intersect(new string[0]).Any())
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a)")
                .WithParameters("Company");
        }

        [Fact]
        public void Where_property_array_intersects_aray()
        {
            g
                .V<Company>()
                .Where(t => t.PhoneNumbers.Intersect(new[] { "+4912345", "+4923456" }).Any())
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, within(_c, _d))")
                .WithParameters("Company", "PhoneNumbers", "+4912345", "+4923456");
        }

        [Fact]
        public void Where_property_array_is_empty()
        {
            g
                .V<Company>()
                .Where(t => !t.PhoneNumbers.Any())
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).not(__.has(_b))")
                .WithParameters("Company", "PhoneNumbers");
        }

        [Fact]
        public void Where_property_array_is_not_empty()
        {
            g
                .V<Company>()
                .Where(t => t.PhoneNumbers.Any())
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b)")
                .WithParameters("Company", "PhoneNumbers");
        }

        [Fact]
        public void Where_property_equals_constant()
        {
            g
                .V<Person>()
                .Where(t => t.Age == 36)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, _c)")
                .WithParameters("Person", "Age", 36);
        }

        [Fact]
        public void Where_property_equals_converted_expression()
        {
            g
                .V<Person>()
                .Where(t => (object)t.Age == (object)36)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, _c)")
                .WithParameters("Person", "Age", 36);
        }

        [Fact]
        public void Where_property_equals_expression()
        {
            const int i = 18;

            g
                .V<Person>()
                .Where(t => t.Age == i + i)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, _c)")
                .WithParameters("Person", "Age", 36);
        }

        [Fact]
        public void Where_property_equals_local_string_constant()
        {
            const int local = 1;

            g
                .V<Language>()
                .Where(t => t.Id == (object)local)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(id, _b)")
                .WithParameters("Language", local);
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
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).values(_b).as(_c).V().hasLabel(_a).has(_b, __.where(eq(_c)))")
                .WithParameters("Language", "IetfLanguageTag", "l1");
        }

        [Fact]
        public void Where_property_equals_value_of_anonymous_object()
        {
            var local = new { Value = 1 };

            g
                .V<Language>()
                .Where(t => t.Id == (object)local.Value)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(id, _b)")
                .WithParameters("Language", 1);
        }

        [Fact]
        public void Where_property_is_contained_in_array()
        {
            g
                .V<Person>()
                .Where(t => new[] { 36, 37, 38 }.Contains(t.Age))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, within(_c, _d, _e))")
                .WithParameters("Person", "Age", 36, 37, 38);
        }

        [Fact]
        public void Where_property_is_contained_in_enumerable()
        {
            var enumerable = new[] { "36", "37", "38" }
                .Select(int.Parse);

            g
                .V<Person>()
                .Where(t => enumerable.Contains(t.Age))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, within(_c, _d, _e))")
                .WithParameters("Person", "Age", 36, 37, 38);
        }

        [Fact]
        public void Where_property_is_greater_or_equal_than_constant()
        {
            g
                .V<Person>()
                .Where(t => t.Age >= 36)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, gte(_c))")
                .WithParameters("Person", "Age", 36);
        }

        [Fact]
        public void Where_property_is_greater_than_constant()
        {
            g
                .V<Person>()
                .Where(t => t.Age > 36)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, gt(_c))")
                .WithParameters("Person", "Age", 36);
        }

        [Fact]
        public void Where_property_is_lower_or_equal_than_constant()
        {
            g
                .V<Person>()
                .Where(t => t.Age <= 36)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, lte(_c))")
                .WithParameters("Person", "Age", 36);
        }

        [Fact]
        public void Where_property_is_lower_than_constant()
        {
            g
                .V<Person>()
                .Where(t => t.Age < 36)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, lt(_c))")
                .WithParameters("Person", "Age", 36);
        }

        [Fact]
        public void Where_property_is_not_contained_in_array()
        {
            g
                .V<Person>()
                .Where(t => !new[] { 36, 37, 38 }.Contains(t.Age))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).not(__.has(_b, within(_c, _d, _e)))")
                .WithParameters("Person", "Age", 36, 37, 38);
        }

        [Fact]
        public void Where_property_is_not_contained_in_empty_enumerable()
        {
            var enumerable = Enumerable.Empty<int>();

            g
                .V<Person>()
                .Where(t => !enumerable.Contains(t.Age))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a)")
                .WithParameters("Person");
        }

        [Fact]
        public void Where_property_is_not_contained_in_enumerable()
        {
            var enumerable = new[] { "36", "37", "38" }
                .Select(int.Parse);

            g
                .V<Person>()
                .Where(t => !enumerable.Contains(t.Age))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).not(__.has(_b, within(_c, _d, _e)))")
                .WithParameters("Person", "Age", 36, 37, 38);
        }

        [Fact]
        public void Where_property_is_not_present()
        {
            g
                .V<Person>()
                .Where(t => t.Name == null)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).hasNot(_b)")
                .WithParameters("Person", "Name");
        }

        [Fact]
        public void Where_property_is_prefix_of_constant()
        {
            g
                .V<Country>()
                .Where(c => "+49123".StartsWith(c.CountryCallingCode))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, within(_c, _d, _e, _f, _g, _h, _i))")
                .WithParameters("Country", "CountryCallingCode", "", "+", "+4", "+49", "+491", "+4912", "+49123");
        }

        [Fact]
        public void Where_property_is_prefix_of_empty_string()
        {
            g
                .V<Country>()
                .Where(c => "".StartsWith(c.CountryCallingCode))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, within(_c))")
                .WithParameters("Country", "CountryCallingCode", "");
        }

        [Fact]
        public void Where_property_is_prefix_of_expression()
        {
            const string str = "+49123xxx";

            g
                .V<Country>()
                .Where(c => str.Substring(0, 6).StartsWith(c.CountryCallingCode))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, within(_c, _d, _e, _f, _g, _h, _i))")
                .WithParameters("Country", "CountryCallingCode", "", "+", "+4", "+49", "+491", "+4912", "+49123");
        }

        [Fact]
        public void Where_property_is_prefix_of_variable()
        {
            const string str = "+49123";

            g
                .V<Country>()
                .Where(c => str.StartsWith(c.CountryCallingCode))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, within(_c, _d, _e, _f, _g, _h, _i))")
                .WithParameters("Country", "CountryCallingCode", "", "+", "+4", "+49", "+491", "+4912", "+49123");
        }

        [Fact]
        public void Where_property_is_present()
        {
            g
                .V<Person>()
                .Where(t => t.Name != null)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b)")
                .WithParameters("Person", "Name");
        }

        [Fact]
        public void Where_property_not_equals_constant()
        {
            g
                .V<Person>()
                .Where(t => t.Age != 36)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, neq(_c))")
                .WithParameters("Person", "Age", 36);
        }

        [Fact]
        public void Where_property_starts_with_constant()
        {
            g
                .V<Country>()
                .Where(c => c.CountryCallingCode.StartsWith("+49123"))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, between(_c, _d))")
                .WithParameters("Country", "CountryCallingCode", "+49123", "+49124");
        }

        [Fact]
        public void Where_property_starts_with_empty_string()
        {
            g
                .V<Country>()
                .Where(c => c.CountryCallingCode.StartsWith(""))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b)")
                .WithParameters("Country", "CountryCallingCode");
        }

        [Fact]
        public void Where_property_traversal()
        {
            g
                .V<Person>()
                .Where(
                    x => x.Age,
                    _ => _
                        .Inject(36))
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, __.inject(_c))")
                .WithParameters("Person", "Age", 36);
        }

        [Fact]
        public void Where_scalar_element_equals_constant()
        {
            g
                .V<Person>()
                .Values(x => x.Age)
                .Where(_ => _ == 36)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).values(_b).is(_c)")
                .WithParameters("Person", "Age", 36);
        }

        [Fact]
        public void Where_source_expression_on_both_sides()
        {
            g
                .V<Country>()
                .Invoking(query => query.Where(t => t.Name.Value == t.CountryCallingCode))
                .Should()
                .Throw<InvalidOperationException>();
        }

        [Fact]
        public void Where_traversal()
        {
            g
                .V<Person>()
                .Where(_ => _.Out<LivesIn>())
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).where(__.out(_b))")
                .WithParameters("Person", "LivesIn");
        }

        [Fact]
        public void Where_VertexProperty_Value1()
        {
            g
                .V<Person>()
                .Where(x => x.Name.Value == "SomeName")
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, _c)")
                .WithParameters("Person", "Name", "SomeName");
        }

        [Fact]
        public void Where_VertexProperty_Value2()
        {
            g
                .V<Person>()
                .Where(x => ((int)(object)x.Name.Value) > 36)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, gt(_c))")
                .WithParameters("Person", "Name", 36);
        }

        [Fact(Skip="Feature!")]
        public void Where_VertexProperty_Value3()
        {
            g
                .V<Person>()
                .Where(x => (int)x.Name.Id == 36)
                .Should()
                .SerializeToGroovy<TVisitor>("g.V().hasLabel(_a).has(_b, gt(_c))")
                .WithParameters("Person", "Name", 36);
        }

        [Fact]
        public void WithoutStrategies1()
        {
            g
                .WithoutStrategies("ReferenceElementStrategy")
                .V()
                .Should()
                .SerializeToGroovy<TVisitor>("g.withoutStrategies(ReferenceElementStrategy).V()")
                .WithoutParameters();
        }

        [Fact]
        public void WithoutStrategies2()
        {
            g
                .WithoutStrategies("ReferenceElementStrategy", "SomeOtherStrategy")
                .V()
                .Should()
                .SerializeToGroovy<TVisitor>("g.withoutStrategies(ReferenceElementStrategy, SomeOtherStrategy).V()")
                .WithoutParameters();
        }

        [Fact]
        public void WithSubgraphStrategy()
        {
            g
                .WithStrategies(new SubgraphQueryStrategy(_ => _.OfType<Person>(), _ => _.OfType<WorksFor>()))
                .V()
                .Should()
                .SerializeToGroovy<TVisitor>("g.withStrategies(SubgraphStrategy.build().vertices(__.hasLabel(_a)).edges(__.hasLabel(_b)).create()).V()")
                .WithParameters("Person", "WorksFor");
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

        [Fact]
        public void WithSubgraphStrategy_only_edges()
        {
            g
                .WithStrategies(new SubgraphQueryStrategy(_ => _, _ => _.OfType<WorksFor>()))
                .V()
                .Should()
                .SerializeToGroovy<TVisitor>("g.withStrategies(SubgraphStrategy.build().edges(__.hasLabel(_a)).create()).V()")
                .WithParameters("WorksFor");
        }

        [Fact]
        public void WithSubgraphStrategy_only_vertices()
        {
            g
                .WithStrategies(new SubgraphQueryStrategy(_ => _.OfType<Person>(), _ => _))
                .V()
                .Should()
                .SerializeToGroovy<TVisitor>("g.withStrategies(SubgraphStrategy.build().vertices(__.hasLabel(_a)).create()).V()")
                .WithParameters("Person");
        }
    }
}
