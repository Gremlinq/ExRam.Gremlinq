using System;
using System.Linq;
using ExRam.Gremlinq.Core.GraphElements;
using ExRam.Gremlinq.Tests.Entities;
using FluentAssertions;
using Gremlin.Net.Process.Traversal;
using Gremlin.Net.Process.Traversal.Strategy.Decoration;
using Xunit;

namespace ExRam.Gremlinq.Core.Tests
{
    public abstract class GroovySerializationTest
    {
        protected readonly IGremlinQuerySource _g;

        private static string id = "id";

        protected GroovySerializationTest(IGremlinQuerySource g)
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
        public void AddE_from_StepLabel()
        {
            _g
                .AddV(new Country { CountryCallingCode = "+49" })
                .As((_, c) => _
                    .AddV(new Language { IetfLanguageTag = "en" })
                    .AddE<Speaks>()
                    .From(c))
                .Should()
                .SerializeToGroovy("addV(_a).property(single, _b, _c).as(_d).addV(_e).property(single, _f, _g).addE(_h).from(_d).project(_i, _j, _k, _l).by(id).by(label).by(__.constant(_m)).by(__.valueMap())")
                .WithParameters("Country", "CountryCallingCode", "+49", "l1", "Language", "IetfLanguageTag", "en", "Speaks", "id", "label", "type", "properties", "edge");
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
                .Should()
                .SerializeToGroovy("addV(_a).property(single, _b, _c).property(single, _d, _e).property(single, _f, _g).property(single, _h, _i).addE(_j).from(__.V().hasLabel(_k).has(_l, _m)).project(_n, _o, _p, _q).by(id).by(label).by(__.constant(_r)).by(__.valueMap())")
                .WithParameters("Person", "Age", 0, "Gender", 0, "Name", "Bob", "RegistrationDate", now, "LivesIn", "Country", "CountryCallingCode", "+49", "id", "label", "type", "properties", "edge");
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
                .Should()
                .SerializeToGroovy("addV(_a).property(single, _b, _c).property(single, _d, _e).addE(_f).to(__.V(_g).hasLabel(_h)).inV().project(_g, _i, _j, _k).by(id).by(label).by(__.constant(_l)).by(__.properties().group().by(__.label()).by(__.project(_g, _i, _m, _k).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))");
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
                .Should()
                .SerializeToGroovy("addV(_a).property(single, _b, _c).property(single, _d, _e).addE(_f).to(__.V(_g).hasLabel(_h)).outV().project(_g, _i, _j, _k).by(id).by(label).by(__.constant(_l)).by(__.properties().group().by(__.label()).by(__.project(_g, _i, _m, _k).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))");
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
                .Should()
                .SerializeToGroovy("addV(_a).property(single, _b, _c).property(single, _d, _e).addE(_f).property(_g, _h).to(__.V(_i).hasLabel(_j)).project(_i, _k, _l, _m).by(id).by(label).by(__.constant(_n)).by(__.valueMap())");
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
                .Should()
                .SerializeToGroovy("addV(_a).property(single, _b, _c).as(_d).addV(_e).property(single, _f, _g).addE(_h).to(_d).project(_i, _j, _k, _l).by(id).by(label).by(__.constant(_m)).by(__.valueMap())")
                .WithParameters("Language", "IetfLanguageTag", "en", "l1", "Country", "CountryCallingCode", "+49", "Speaks", "id", "label", "type", "properties", "edge");
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
                .Should()
                .SerializeToGroovy("addV(_a).property(single, _b, _c).property(single, _d, _e).property(single, _f, _g).property(single, _h, _i).addE(_j).to(__.V().hasLabel(_k).has(_l, _m)).project(_n, _o, _p, _q).by(id).by(label).by(__.constant(_r)).by(__.valueMap())")
                .WithParameters("Person", "Age", 0, "Gender", 0, "Name", "Bob", "RegistrationDate", now, "LivesIn", "Country", "CountryCallingCode", "+49", "id", "label", "type", "properties", "edge");
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
               .Should()
               .SerializeToGroovy("addE(_a).property(_b, _c).project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.valueMap())")
               .WithParameters("WorksFor", nameof(WorksFor.To), now, "id", "label", "type", "properties", "edge");
        }

        [Fact]
        public void AddV()
        {
            _g
                .AddV(new Language { Id = 1, IetfLanguageTag = "en" })
                .Should()
                .SerializeToGroovy("addV(_a).property(id, _b).property(single, _c, _d).project(_e, _f, _g, _h).by(id).by(label).by(__.constant(_i)).by(__.properties().group().by(__.label()).by(__.project(_e, _f, _j, _h).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Language", 1, "IetfLanguageTag", "en", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void AddV_ignores_label()
        {
            _g
                .AddV(new Language {Label = "Language"})
                .Should()
                .SerializeToGroovy("addV(_a).project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_b, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Language", "id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("addV(_a).property(single, _b, _c).property(single, _d, _e).property(id, _f).project(_g, _h, _i, _j).by(id).by(label).by(__.constant(_k)).by(__.properties().group().by(__.label()).by(__.project(_g, _h, _l, _j).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "Age", 0, "Gender", 1, 1, "id", "label", "type", "properties", "vertex", "value");
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
               .Should()
               .SerializeToGroovy("addV(_a).property(single, _b, _c).property(single, _d, _e).project(_f, _g, _h, _i).by(id).by(label).by(__.constant(_j)).by(__.properties().group().by(__.label()).by(__.project(_f, _g, _k, _i).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
               .WithParameters("Person", "Name", "Marko", "RegistrationDate", now, "id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("addV(_a).property(single, _b, _c).project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.properties().group().by(__.label()).by(__.project(_d, _e, _i, _g).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Language", "IetfLanguageTag", "en", "id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("addV(_a).property(id, _b).project(_c, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_c, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Language", 1, "id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("addV(_a).property(id, _b).property(single, _c, _d, _e, _f, _g, _h).project(_i, _j, _k, _l).by(id).by(label).by(__.constant(_m)).by(__.properties().group().by(__.label()).by(__.project(_i, _j, _n, _l).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Country", 1, "Name", "GER", "de", "Deutschland", "en", "Germany", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void AddV_with_Meta_without_properties()
        {
            _g
                .AddV(new Country { Id = 1, Name = "GER"})
                .Should()
                .SerializeToGroovy("addV(_a).property(id, _b).property(single, _c, _d).project(_e, _f, _g, _h).by(id).by(label).by(__.constant(_i)).by(__.properties().group().by(__.label()).by(__.project(_e, _f, _j, _h).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Country", 1, "Name", "GER", "id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("addV(_a).property(single, _b, _c).property(id, _d).property(list, _e, _f, _g, _h).project(_i, _j, _k, _l).by(id).by(label).by(__.constant(_m)).by(__.properties().group().by(__.label()).by(__.project(_i, _j, _n, _l).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Company", "FoundingDate", DateTime.MinValue, 1, "Names", "Bob", "ValidFrom", DateTimeOffset.Parse("01.01.2019 08:00"), "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void AddV_with_multi_property()
        {
            _g
                .AddV(new Company { Id = 1, PhoneNumbers = new[] { "+4912345", "+4923456" } })
                .Should()
                .SerializeToGroovy("addV(_a).property(single, _b, _c).property(id, _d).property(list, _e, _f).property(list, _e, _g).project(_h, _i, _j, _k).by(id).by(label).by(__.constant(_l)).by(__.properties().group().by(__.label()).by(__.project(_h, _i, _m, _k).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Company", "FoundingDate", DateTime.MinValue, 1, "PhoneNumbers", "+4912345", "+4923456", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void AddV_with_nulls()
        {
            _g
                .AddV(new Language { Id = 1 })
                .Should()
                .SerializeToGroovy("addV(_a).property(id, _b).project(_c, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_c, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Language", 1, "id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("addV(_a).property(id, _b).property(single, _c, _d).project(_e, _f, _g, _h).by(id).by(label).by(__.constant(_i)).by(__.properties().group().by(__.label()).by(__.project(_e, _f, _j, _h).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Language", 1, "lang", "en", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void AddV_without_id()
        {
            _g
                .AddV(new Language { IetfLanguageTag = "en" })
                .Should()
                .SerializeToGroovy("addV(_a).property(single, _b, _c).project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.properties().group().by(__.label()).by(__.project(_d, _e, _i, _g).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Language", "IetfLanguageTag", "en", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void AddV_without_model()
        {
            _g
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel.Empty))
                .AddV(new Language { Id = 1, IetfLanguageTag = "en" })
                .Should()
                .SerializeToGroovy("addV(_a).property(id, _b).property(single, _c, _d).project(_e, _f, _g, _h).by(id).by(label).by(__.constant(_i)).by(__.properties().group().by(__.label()).by(__.project(_e, _f, _j, _h).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Language", 1, "IetfLanguageTag", "en", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Aggregate_Global()
        {
            _g
                .V()
                .AggregateGlobal((__, aggregated) =>
                    __.Out())
                .Count()
                .Should()
                .SerializeToGroovy("V().aggregate(global, _a).out().count()")
                .WithParameters("l1");
        }

        [Fact]
        public void Aggregate_Local()
        {
            _g
                .V()
                .Aggregate((__, aggregated) =>
                    __.Out())
                .Count()
                .Should()
                .SerializeToGroovy("V().aggregate(_a).out().count()")
                .WithParameters("l1");
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
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).and(__.inE(_b), __.outE(_c)).project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.properties().group().by(__.label()).by(__.project(_d, _e, _i, _g).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "WorksFor", "LivesIn", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void And_identity()
        {
            _g
                .V<Person>()
                .And(
                    __ => __)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_b, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void And_infix()
        {
            _g
                .V<Person>()
                .And()
                .Out()
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).and().out().project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_b, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).and(__.outE(_b), __.inE(_c), __.outE(_c)).project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.properties().group().by(__.label()).by(__.project(_d, _e, _i, _g).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "LivesIn", "WorksFor", "id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).and(__.out()).project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_b, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void And_none()
        {
            _g
                .V<Person>()
                .And(
                    __ => __.None())
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).not(__.identity()).project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_b, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void And_optimization()
        {
            _g
                .V<Person>()
                .And(
                    __ => __,
                    __ => __.Out())
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).and(__.out()).project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_b, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void As_followed_by_Select()
        {
            _g
                .V<Person>()
                .As((_, stepLabel1) => _
                    .Select(stepLabel1))
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).as(_b).project(_c, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_c, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "l1", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void As_idempotency_is_detected()
        {
            _g
                .V<Person>()
                .As((_, stepLabel1) => _
                    .As((__, stepLabel2) => __
                        .Select(stepLabel1, stepLabel2)))
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).as(_b).project(_c, _d).by(__.select(_b).project(_e, _f, _g, _h).by(id).by(label).by(__.constant(_i)).by(__.properties().group().by(__.label()).by(__.project(_e, _f, _j, _h).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))).by(__.select(_b).project(_e, _f, _g, _h).by(id).by(label).by(__.constant(_i)).by(__.properties().group().by(__.label()).by(__.project(_e, _f, _j, _h).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold())))")
                .WithParameters("Person", "l1", "Item1", "Item2", "id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).as(_b).out().hasLabel(_a).as(_c).project(_d, _e).by(__.select(_b).project(_f, _g, _h, _i).by(id).by(label).by(__.constant(_j)).by(__.properties().group().by(__.label()).by(__.project(_f, _g, _k, _i).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))).by(__.select(_c).project(_f, _g, _h, _i).by(id).by(label).by(__.constant(_j)).by(__.properties().group().by(__.label()).by(__.project(_f, _g, _k, _i).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold())))")
                .WithParameters("Person", "l1", "l2", "Item1", "Item2", "id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).as(_b).out().hasLabel(_a).as(_c).count().project(_d, _e).by(__.select(_b).project(_f, _g, _h, _i).by(id).by(label).by(__.constant(_j)).by(__.properties().group().by(__.label()).by(__.project(_f, _g, _k, _i).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))).by(__.select(_c).project(_f, _g, _h, _i).by(id).by(label).by(__.constant(_j)).by(__.properties().group().by(__.label()).by(__.project(_f, _g, _k, _i).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold())))")
                .WithParameters("Person", "l1", "l2", "Item1", "Item2", "id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).as(_b).count().select(_b).project(_c, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_c, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "l1", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Choose_one_case()
        {
            _g
                .V()
                .Choose(_ => _
                    .On(__ => __.Values())
                    .Case(3, __ => __.Constant(1)))
                .Should()
                .SerializeToGroovy("V().choose(__.values()).option(_a, __.constant(_b))")
                .WithParameters(3, 1);
        }

        [Fact]
        public void Choose_only_default_case()
        {
            _g
                .V()
                .Choose(_ => _
                    .On(__ => __.Values())
                    .Default(__ => __.Constant(1)))
                .Should()
                .SerializeToGroovy("V().choose(__.values()).option(none, __.constant(_a))")
                .WithParameters(1);
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
                .Should()
                .SerializeToGroovy("V().id().choose(eq(_a), __.constant(_b), __.constant(_c))")
                .WithParameters(42, true, false);
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
                .Should()
                .SerializeToGroovy("V().id().choose(eq(_a), __.constant(_b))")
                .WithParameters(42, true);
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
                .Should()
                .SerializeToGroovy("V().id().choose(lt(_a), __.constant(_b), __.constant(_c))")
                .WithParameters(42, true, false);
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
                .Should()
                .SerializeToGroovy("V().id().choose(lt(_a), __.constant(_b), __.constant(_c))")
                .WithParameters(42, true, false);
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
                .Should()
                .SerializeToGroovy("V().id().choose(gt(_a).and(lt(_b)), __.constant(_c), __.constant(_d))")
                .WithParameters(0, 42, true, false);
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
                .Should()
                .SerializeToGroovy("V().id().choose(gt(_a).and(lt(_b)).or(neq(_c)), __.constant(_d), __.constant(_e))")
                .WithParameters(0, 42, 37, true, false);
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
                .Should()
                .SerializeToGroovy("V().id().choose(gt(_a).or(lt(_b).and(neq(_c))), __.constant(_d), __.constant(_e))")
                .WithParameters(0, 42, 37, true, false);
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
                .Should()
                .SerializeToGroovy("V().choose(__.values(), __.out(), __.in()).project(_a, _b, _c, _d).by(id).by(label).by(__.constant(_e)).by(__.properties().group().by(__.label()).by(__.project(_a, _b, _f, _d).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Choose_Traversal2()
        {
            _g
                .V()
                .Choose(
                    _ => _.Values(),
                    _ => _.Out())
                .Should()
                .SerializeToGroovy("V().choose(__.values(), __.out()).project(_a, _b, _c, _d).by(id).by(label).by(__.constant(_e)).by(__.properties().group().by(__.label()).by(__.project(_a, _b, _f, _d).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("V().choose(__.values()).option(_a, __.constant(_b)).option(_c, __.constant(_d))")
                .WithParameters(3, 1, 4, 2);
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
                .Should()
                .SerializeToGroovy("V().choose(__.values()).option(_a, __.constant(_b)).option(_c, __.constant(_d)).option(none, __.constant(_a))")
                .WithParameters(3, 1, 4, 2);
        }

        [Fact]
        public void Coalesce()
        {
            _g
                .V()
                .Coalesce(
                    _ => _
                        .Out())
                .Should()
                .SerializeToGroovy("V().coalesce(__.out()).project(_a, _b, _c, _d).by(id).by(label).by(__.constant(_e)).by(__.properties().group().by(__.label()).by(__.project(_a, _b, _f, _d).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("V().project(_a, _b, _c, _d).by(id).by(label).by(__.constant(_e)).by(__.properties().group().by(__.label()).by(__.project(_a, _b, _f, _d).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Constant()
        {
            _g
                .V()
                .Constant(42)
                .Should()
                .SerializeToGroovy("V().constant(_a)")
                .WithParameters(42);
        }

        [Fact]
        public void Count()
        {
            _g
                .V()
                .Count()
                .Should()
                .SerializeToGroovy("V().count()")
                .WithoutParameters();
        }

        [Fact]
        public void CountGlobal()
        {
            _g
                .V()
                .Count()
                .Should()
                .SerializeToGroovy("V().count()")
                .WithoutParameters();
        }

        [Fact]
        public void CountLocal()
        {
            _g
                .V()
                .CountLocal()
                .Should()
                .SerializeToGroovy("V().count(local)")
                .WithoutParameters();
        }

        [Fact]
        public void Drop()
        {
            _g
                .V<Person>()
                .Drop()
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).drop()")
                .WithParameters("Person");
        }


        [Fact]
        public void Drop_in_local()
        {
            _g
                .Inject(1)
                .Local(__ => __
                    .V()
                    .Drop())
                .Should()
                .SerializeToGroovy("inject(_a).local(__.V().drop())")
                .WithParameters(1);
        }

        [Fact]
        public void E_of_all_types1()
        {
            _g
                .E<object>()
                .Should()
                .SerializeToGroovy("E().project(_a, _b, _c, _d).by(id).by(label).by(__.constant(_e)).by(__.valueMap())")
                .WithParameters("id", "label", "type", "properties", "edge");
        }

        [Fact]
        public void E_of_all_types2()
        {
            _g
                .E()
                .Should()
                .SerializeToGroovy("E().project(_a, _b, _c, _d).by(id).by(label).by(__.constant(_e)).by(__.valueMap())")
                .WithParameters("id", "label", "type", "properties", "edge");
        }

        [Fact]
        public void E_of_concrete_type()
        {
            _g
                .E<WorksFor>()
                .Should()
                .SerializeToGroovy("E().hasLabel(_a).project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.valueMap())")
                .WithParameters("WorksFor", "id", "label", "type", "properties", "edge");
        }

        [Fact]
        public void E_Properties()
        {
            _g
                .E()
                .Properties()
                .Should()
                .SerializeToGroovy("E().properties()")
                .WithoutParameters();
        }

        [Fact]
        public void E_Properties_member()
        {
            _g
                .E<LivesIn>()
                .Properties(x => x.Since)
                .Should()
                .SerializeToGroovy("E().hasLabel(_a).properties(_b)")
                .WithParameters("LivesIn", "Since");
        }

        [Fact]
        public void Explicit_As()
        {
            var stepLabel = new StepLabel<Person>();

            _g
                .V<Person>()
                .As(stepLabel)
                .Select(stepLabel)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).as(_b).select(_b).project(_c, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_c, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "l1", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void FilterWithLambda()
        {
            _g
                .V<Person>()
                .Where(Lambda.Groovy("it.property('str').value().length() == 2"))
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).filter({it.property('str').value().length() == 2}).project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_b, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void FlatMap()
        {
            _g
                .V<Person>()
                .FlatMap(__ => __.Out<WorksFor>())
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).flatMap(__.out(_b)).project(_c, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_c, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "WorksFor", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Fold()
        {
            _g
                .V()
                .Fold()
                .Should()
                .SerializeToGroovy("V().fold()")
                .WithoutParameters();
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
                .Should()
                .SerializeToGroovy("V().fold().fold().unfold().unfold().project(_a, _b, _c, _d).by(id).by(label).by(__.constant(_e)).by(__.properties().group().by(__.label()).by(__.project(_a, _b, _f, _d).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Fold_SideEffect()
        {
            _g
                .V()
                .Fold()
                .SideEffect(x => x.Identity())
                .Unfold()
                .Should()
                .SerializeToGroovy("V().fold().sideEffect(__.identity()).unfold().project(_a, _b, _c, _d).by(id).by(label).by(__.constant(_e)).by(__.properties().group().by(__.label()).by(__.project(_a, _b, _f, _d).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Fold_Unfold()
        {
            _g
                .V()
                .Fold()
                .Unfold()
                .Should()
                .SerializeToGroovy("V().fold().unfold().project(_a, _b, _c, _d).by(id).by(label).by(__.constant(_e)).by(__.properties().group().by(__.label()).by(__.project(_a, _b, _f, _d).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Generic_constraint()
        {
            V2<Person>(_g)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_b, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Group_with_key()
        {
            _g
                .V()
                .Group(_ => _
                    .ByKey(_ => _.Label()))
                .Should()
                .SerializeToGroovy("V().group().by(__.label())");
        }

        [Fact]
        public void Group_with_key_and_value()
        {
            _g
                .V()
                .Group(_ => _
                    .ByKey(_ => _.Label())
                    .ByValue(_ => _.Values("v")))
                .Should()
                .SerializeToGroovy("V().group().by(__.label()).by(__.values(_a))")
                .WithParameters("v");
        }

        [Fact]
        public void Identity()
        {
            _g
                .V<Person>()
                .Identity()
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_b, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Identity_Identity()
        {
            _g
                .V<Person>()
                .Identity()
                .Identity()
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_b, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "id", "label", "type", "properties", "vertex", "value");
        }


        [Fact]
        public void In()
        {
            _g
                .V<Person>()
                .In<WorksFor>()
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).in(_b).project(_c, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_c, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "WorksFor", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void In_of_all_types()
        {
            _g
                .V()
                .In<object>()
                .Should()
                .SerializeToGroovy("V().in(_a, _b, _c, _d).project(_e, _f, _g, _h).by(id).by(label).by(__.constant(_i)).by(__.properties().group().by(__.label()).by(__.project(_e, _f, _j, _h).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Knows", "LivesIn", "Speaks", "WorksFor", "id", "label", "type", "properties", "vertex", "value");

            _g
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(o => o
                        .SetItem(GremlinqOption.FilterLabelsVerbosity, FilterLabelsVerbosity.Minimum)))
                .V()
                .In<object>()
                .Should()
                .SerializeToGroovy("V().in().project(_a, _b, _c, _d).by(id).by(label).by(__.constant(_e)).by(__.properties().group().by(__.label()).by(__.project(_a, _b, _f, _d).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void InE_of_all_types()
        {
            _g
                .V()
                .InE<object>()
                .Should()
                .SerializeToGroovy("V().inE(_a, _b, _c, _d).project(_e, _f, _g, _h).by(id).by(label).by(__.constant(_i)).by(__.valueMap())")
                .WithParameters("Knows", "LivesIn", "Speaks", "WorksFor", "id", "label", "type", "properties", "edge");

            _g
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(x => x
                        .SetItem(GremlinqOption.FilterLabelsVerbosity, FilterLabelsVerbosity.Minimum)))
                .V()
                .InE<object>()
                .Should()
                .SerializeToGroovy("V().inE().project(_a, _b, _c, _d).by(id).by(label).by(__.constant(_e)).by(__.valueMap())")
                .WithParameters("id", "label", "type", "properties", "edge");
        }

        [Fact]
        public void Inject()
        {
            _g
                .Inject(36, 37, 38)
                .Should()
                .SerializeToGroovy("inject(_a, _b, _c)")
                .WithParameters(36, 37, 38);
        }

        [Fact]
        public void Label()
        {
            _g
                .V()
                .Label()
                .Should()
                .SerializeToGroovy("V().label()")
                .WithoutParameters();
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
                .Should()
                .SerializeToGroovy("V().limit(_a).project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_b, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters(1, "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void LimitLocal()
        {
            _g
                .V()
                .LimitLocal(1)
                .Should()
                .SerializeToGroovy("V().limit(local, _a).project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_b, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters(1, "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Local_identity()
        {
            _g
                .V()
                .Local(__ => __)
                .Should()
                .SerializeToGroovy("V().project(_a, _b, _c, _d).by(id).by(label).by(__.constant(_e)).by(__.properties().group().by(__.label()).by(__.project(_a, _b, _f, _d).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Map()
        {
            _g
                .V<Person>()
                .Map(__ => __.Out<WorksFor>())
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).map(__.out(_b)).project(_c, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_c, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "WorksFor", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Map_Identity()
        {
            _g
                .V<Person>()
                .Map(__ => __)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_b, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).as(_b).map(__.project(_c, _d).by(__.select(_b).project(_e, _f, _g, _h).by(id).by(label).by(__.constant(_i)).by(__.properties().group().by(__.label()).by(__.project(_e, _f, _j, _h).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))).by(__.select(_b).project(_e, _f, _g, _h).by(id).by(label).by(__.constant(_i)).by(__.properties().group().by(__.label()).by(__.project(_e, _f, _j, _h).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))))")
                .WithParameters("Person", "l1", "Item1", "Item2", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void MaxGlobal()
        {
            _g
                .V<Person>()
                .Values(x => x.Age)
                .Max()
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).values(_b).max()")
                .WithParameters("Person", "Age");
        }

        [Fact]
        public void MaxLocal()
        {
            _g
                .V<Person>()
                .Values(x => x.Age)
                .MaxLocal()
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).values(_b).max(local)")
                .WithParameters("Person", "Age");
        }

        [Fact]
        public void MeanGlobal()
        {
            _g
                .V<Person>()
                .Values(x => x.Age)
                .Mean()
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).values(_b).mean()")
                .WithParameters("Person", "Age");
        }

        [Fact]
        public void MeanLocal()
        {
            _g
                .V<Person>()
                .Values(x => x.Age)
                .MeanLocal()
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).values(_b).mean(local)")
                .WithParameters("Person", "Age");
        }

        [Fact]
        public void MinGlobal()
        {
            _g
                .V<Person>()
                .Values(x => x.Age)
                .Min()
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).values(_b).min()")
                .WithParameters("Person", "Age");
        }

        [Fact]
        public void MinLocal()
        {
            _g
                .V<Person>()
                .Values(x => x.Age)
                .MinLocal()
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).values(_b).min(local)")
                .WithParameters("Person", "Age");
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
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).as(_b).project(_c, _d).by(__.select(_b).project(_e, _f, _g, _h).by(id).by(label).by(__.constant(_i)).by(__.properties().group().by(__.label()).by(__.project(_e, _f, _j, _h).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))).by(__.select(_b).project(_e, _f, _g, _h).by(id).by(label).by(__.constant(_i)).by(__.properties().group().by(__.label()).by(__.project(_e, _f, _j, _h).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))).as(_k).project(_c, _d).by(__.select(_k)).by(__.select(_b).project(_e, _f, _g, _h).by(id).by(label).by(__.constant(_i)).by(__.properties().group().by(__.label()).by(__.project(_e, _f, _j, _h).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold())))")
                .WithParameters("Person", "l1", "Item1", "Item2", "id", "label", "type", "properties", "vertex", "value", "l2");
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
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).as(_b).project(_c, _d).by(__.select(_b).project(_e, _f, _g, _h).by(id).by(label).by(__.constant(_i)).by(__.properties().group().by(__.label()).by(__.project(_e, _f, _j, _h).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))).by(__.select(_b).project(_e, _f, _g, _h).by(id).by(label).by(__.constant(_i)).by(__.properties().group().by(__.label()).by(__.project(_e, _f, _j, _h).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))).as(_k).project(_c, _d).by(__.select(_b).project(_e, _f, _g, _h).by(id).by(label).by(__.constant(_i)).by(__.properties().group().by(__.label()).by(__.project(_e, _f, _j, _h).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))).by(__.select(_k))")
                .WithParameters("Person", "l1", "Item1", "Item2", "id", "label", "type", "properties", "vertex", "value", "l2");
        }

        [Fact]
        public void Not1()
        {
            _g
                .V()
                .Not(__ => __.Out<WorksFor>())
                .Should()
                .SerializeToGroovy("V().not(__.out(_a)).project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_b, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("WorksFor", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Not2()
        {
            _g
                .V()
                .Not(__ => __.OfType<Language>())
                .Should()
                .SerializeToGroovy("V().not(__.hasLabel(_a)).project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_b, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Language", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Not3()
        {
            _g
                .V()
                .Not(__ => __.OfType<Authority>())
                .Should()
                .SerializeToGroovy("V().not(__.hasLabel(_a, _b)).project(_c, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_c, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Company", "Person", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void OfType_abstract()
        {
            _g
                .V()
                .OfType<Authority>()
                .Should()
                .SerializeToGroovy("V().hasLabel(_a, _b).project(_c, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_c, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Company", "Person", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void OfType_redundant1()
        {
            _g
                .V()
                .OfType<Company>()
                .OfType<Authority>()
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_b, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Company", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void OfType_redundant2()
        {
            _g
                .V()
                .OfType<Company>()
                .OfType<object>()
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_b, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Company", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void OfType_redundant3()
        {
            _g
                .V()
                .OfType<Company>()
                .Cast<object>()
                .OfType<Authority>()
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_b, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Company", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void OfType_redundant4()
        {
            _g
                .V()
                .OfType<Authority>()
                .OfType<Company>()
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_b, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Company", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Optional()
        {
            _g
                .V()
                .Optional(
                    __ => __.Out<WorksFor>())
                .Should()
                .SerializeToGroovy("V().optional(__.out(_a)).project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_b, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("WorksFor", "id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).or(__.inE(_b), __.outE(_c)).project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.properties().group().by(__.label()).by(__.project(_d, _e, _i, _g).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "WorksFor", "LivesIn", "id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_b, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Or_infix()
        {
            _g
                .V<Person>()
                .Out()
                .Or()
                .In()
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).out().or().in().project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_b, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).or(__.outE(_b), __.inE(_c), __.outE(_c)).project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.properties().group().by(__.label()).by(__.project(_d, _e, _i, _g).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "LivesIn", "WorksFor", "id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_b, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).or(__.outE()).project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_b, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Order_scalars()
        {
            _g
                .V<Person>()
                .Local(__ => __.Count())
                .Order(b => b
                    .By(__ => __))
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).local(__.count()).order().by(__.identity(), incr)");
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
                .Should()
                .SerializeToGroovy("V().hasLabel(_a, _b, _c, _d, _e).order().by(id, incr).fold().unfold().project(_f, _g, _h, _i).by(id).by(label).by(__.constant(_j)).by(__.properties().group().by(__.label()).by(__.project(_f, _g, _k, _i).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))");
        }

        [Fact]
        public void OrderBy_lambda()
        {
            _g
                .V<Person>()
                .Order(b => b
                    .By(Lambda.Groovy("it.property('str').value().length()")))
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).order().by({it.property('str').value().length()}).project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_b, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void OrderBy_member()
        {
            _g
                .V<Person>()
                .Order(b => b
                    .By(x => x.Name))
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).order().by(_b, incr).project(_c, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_c, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "Name", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void OrderBy_member_ThenBy_member()
        {
            _g
                .V<Person>()
                .Order(b => b
                    .By(x => x.Name)
                    .By(x => x.Age))
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).order().by(_b, incr).by(_c, incr).project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.properties().group().by(__.label()).by(__.project(_d, _e, _i, _g).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "Name", "Age", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void OrderBy_ThenBy_lambda()
        {
            _g
                .V<Person>()
                .Order(b => b
                    .By(Lambda.Groovy("it.property('str1').value().length()"))
                    .By(Lambda.Groovy("it.property('str2').value().length()")))
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).order().by({it.property('str1').value().length()}).by({it.property('str2').value().length()}).project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_b, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void OrderBy_ThenByDescending_member()
        {
            _g
                .V<Person>()
                .Order(b => b
                    .By(x => x.Name)
                    .ByDescending(x => x.Age))
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).order().by(_b, incr).by(_c, decr).project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.properties().group().by(__.label()).by(__.project(_d, _e, _i, _g).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "Name", "Age", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void OrderBy_ThenByDescending_traversal()
        {
            _g
                .V<Person>()
                .Order(b => b
                    .By(__ => __.Values(x => x.Name))
                    .ByDescending(__ => __.Gender))
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).order().by(__.values(_b), incr).by(_c, decr).project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.properties().group().by(__.label()).by(__.project(_d, _e, _i, _g).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "Name", "Gender", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void OrderBy_traversal()
        {
            _g
                .V<Person>()
                .Order(b => b
                    .By(__ => __.Values(x => x.Name)))
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).order().by(__.values(_b), incr).project(_c, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_c, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "Name", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void OrderBy_traversal_ThenBy()
        {
            _g
                .V<Person>()
                .Order(b => b
                    .By(__ => __.Values(x => x.Name))
                    .By(__ => __.Gender))
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).order().by(__.values(_b), incr).by(_c, incr).project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.properties().group().by(__.label()).by(__.project(_d, _e, _i, _g).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "Name", "Gender", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void OrderBy_traversal_ThenBy_traversal()
        {
            _g
                .V<Person>()
                .Order(b => b
                    .By(__ => __.Values(x => x.Name))
                    .By(__ => __.Values(x => x.Gender)))
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).order().by(__.values(_b), incr).by(__.values(_c), incr).project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.properties().group().by(__.label()).by(__.project(_d, _e, _i, _g).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "Name", "Gender", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void OrderByDescending_member()
        {
            _g
                .V<Person>()
                .Order(b => b
                    .ByDescending(x => x.Name))
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).order().by(_b, decr).project(_c, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_c, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "Name", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void OrderByDescending_traversal()
        {
            _g
                .V<Person>()
                .Order(b => b
                    .ByDescending(__ => __.Values(x => x.Name)))
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).order().by(__.values(_b), decr).project(_c, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_c, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "Name", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Out()
        {
            _g
                .V<Person>()
                .Out<WorksFor>()
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).out(_b).project(_c, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_c, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "WorksFor", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Out_does_not_include_abstract_edge()
        {
            _g
                .V<Person>()
                .Out<Edge>()
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).out(_b, _c, _d, _e).project(_f, _g, _h, _i).by(id).by(label).by(__.constant(_j)).by(__.properties().group().by(__.label()).by(__.project(_f, _g, _k, _i).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "Knows", "LivesIn", "Speaks", "WorksFor", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Out_of_all_types()
        {
            _g
                .V()
                .Out<object>()
                .Should()
                .SerializeToGroovy("V().out(_a, _b, _c, _d).project(_e, _f, _g, _h).by(id).by(label).by(__.constant(_i)).by(__.properties().group().by(__.label()).by(__.project(_e, _f, _j, _h).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Knows", "LivesIn", "Speaks", "WorksFor", "id", "label", "type", "properties", "vertex", "value");

            _g
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(o => o
                        .SetItem(GremlinqOption.FilterLabelsVerbosity, FilterLabelsVerbosity.Minimum)))
                .V()
                .Out<object>()
                .Should()
                .SerializeToGroovy("V().out().project(_a, _b, _c, _d).by(id).by(label).by(__.constant(_e)).by(__.properties().group().by(__.label()).by(__.project(_a, _b, _f, _d).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void OutE_of_all_types()
        {
            _g
                .V()
                .OutE<object>()
                .Should()
                .SerializeToGroovy("V().outE(_a, _b, _c, _d).project(_e, _f, _g, _h).by(id).by(label).by(__.constant(_i)).by(__.valueMap())")
                .WithParameters("Knows", "LivesIn", "Speaks", "WorksFor", "id", "label", "type", "properties", "edge");

            _g
                .ConfigureEnvironment(env => env
                    .ConfigureOptions(o => o
                        .SetItem(GremlinqOption.FilterLabelsVerbosity, FilterLabelsVerbosity.Minimum)))
                .V()
                .OutE<object>()
                .Should()
                .SerializeToGroovy("V().outE().project(_a, _b, _c, _d).by(id).by(label).by(__.constant(_e)).by(__.valueMap())")
                .WithParameters("id", "label", "type", "properties", "edge");
        }

        [Fact]
        public void OutE_of_no_derived_types()
        {
            _g
                .V()
                .OutE<string>()
                .Should()
                .SerializeToGroovy("V().outE(_a).project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.valueMap())")
                .WithParameters("String", "id", "label", "type", "properties", "edge");
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
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).project(_b, _c).by(__.values(_b)).by(__.in().project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.properties().group().by(__.label()).by(__.project(_d, _e, _i, _g).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold())))")
                .WithParameters("Person", "Age", "in!", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Project_with_builder_1()
        {
            _g
                .V()
                .Project(_ => _
                    .ToDynamic()
                    .By("in!", __ => __.In()))
                .Should()
                .SerializeToGroovy("V().project(_a).by(__.in().project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_b, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold())))")
                .WithParameters("in!", "id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("V().project(_a, _b, _c, _d).by(__.count()).by(__.in().project(_e, _f, _g, _h).by(id).by(label).by(__.constant(_i)).by(__.properties().group().by(__.label()).by(__.project(_e, _f, _j, _h).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))).by(__.out().project(_e, _f, _g, _h).by(id).by(label).by(__.constant(_i)).by(__.properties().group().by(__.label()).by(__.project(_e, _f, _j, _h).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))).by(__.properties())")
                .WithParameters("count!", "in!", "out!", "properties!", "id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("V().project(_a, _b).by(__.in().project(_c, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_c, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))).by(__.out().project(_c, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_c, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold())))")
                .WithParameters("Item1", "Item2", "id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("V().project(_a, _b, _c).by(__.in().project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.properties().group().by(__.label()).by(__.project(_d, _e, _i, _g).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))).by(__.out().project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.properties().group().by(__.label()).by(__.project(_d, _e, _i, _g).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))).by(__.count())")
                .WithParameters("Item1", "Item2", "Item3", "id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("V().project(_a, _b, _c, _d).by(__.in().project(_e, _f, _g, _h).by(id).by(label).by(__.constant(_i)).by(__.properties().group().by(__.label()).by(__.project(_e, _f, _j, _h).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))).by(__.out().project(_e, _f, _g, _h).by(id).by(label).by(__.constant(_i)).by(__.properties().group().by(__.label()).by(__.project(_e, _f, _j, _h).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))).by(__.count()).by(__.properties())")
                .WithParameters("Item1", "Item2", "Item3", "Item4", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Properties_Meta()
        {
            _g
                .V<Country>()
                .Properties(x => x.Name)
                .Meta<PropertyValidity>()
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).properties(_b)")
                .WithParameters("Country", "Name");
        }

        [Fact]
        public void Properties_Meta_ValueMap()
        {
            _g
                .V()
                .Properties()
                .Meta<PropertyValidity>()
                .ValueMap()
                .Should()
                .SerializeToGroovy("V().properties().valueMap()")
                .WithoutParameters();
        }

        [Fact]
        public void Properties_Meta_Values()
        {
            _g
                .V()
                .Properties()
                .Meta<PropertyValidity>()
                .Values()
                .Should()
                .SerializeToGroovy("V().properties().values()")
                .WithoutParameters();
        }

        [Fact]
        public void Properties_Meta_Values_Projected()
        {
            _g
                .V()
                .Properties()
                .Meta<PropertyValidity>()
                .Values(x => x.ValidFrom)
                .Should()
                .SerializeToGroovy("V().properties().values(_a)")
                .WithParameters("ValidFrom");
        }

        [Fact]
        public void Properties_Meta_Where1()
        {
            _g
                .V<Country>()
                .Properties(x => x.Name)
                .Meta<PropertyValidity>()
                .Where(x => x.Properties.ValidFrom >= DateTimeOffset.Parse("01.01.2019 08:00"))
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).properties(_b).has(_c, gte(_d))")
                .WithParameters("Country", "Name", "ValidFrom", DateTimeOffset.Parse("01.01.2019 08:00"));
        }

        [Fact]
        public void Properties_name_typed()
        {
            _g
                .V()
                .Properties<int>("propertyName")
                .Should()
                .SerializeToGroovy("V().properties(_a)")
                .WithParameters("propertyName");
        }

        [Fact]
        public void Properties_name_untyped()
        {
            _g
                .V()
                .Properties("propertyName")
                .Should()
                .SerializeToGroovy("V().properties(_a)")
                .WithParameters("propertyName");
        }

        [Fact]
        public void Properties_of_member()
        {
            _g
                .V<Country>()
                .Properties(x => x.Name)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).properties(_b)")
                .WithParameters("Country", "Name");
        }

        [Fact]
        public void Properties_of_three_members()
        {
            _g
                .V<Country>()
                .Properties(x => x.Name, x => x.CountryCallingCode, x => x.Languages)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).properties(_b, _c, _d)")
                .WithParameters("Country", "Name", "CountryCallingCode", "Languages");
        }

        [Fact]
        public void Properties_of_two_members1()
        {
            _g
                .V<Country>()
                .Properties(x => x.Name, x => x.CountryCallingCode)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).properties(_b, _c)")
                .WithParameters("Country", "Name", "CountryCallingCode");
        }

        [Fact]
        public void Properties_of_two_members2()
        {
            _g
                .V<Country>()
                .Properties(x => x.Name, x => x.Languages)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).properties(_b, _c)")
                .WithParameters("Country", "Name", "Languages");
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
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).properties(_b).properties().as(_c)")
                .WithParameters("Country", "Name", "l1");
        }

        [Fact]
        public void Properties_Properties_key()
        {
            _g
                .V<Country>()
                .Properties(x => x.Name)
                .Properties()
                .Key()
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).properties(_b).properties().key()")
                .WithParameters("Country", "Name");
        }

        [Fact]
        public void Properties_Properties_Value()
        {
            _g
                .V<Company>()
                .Properties(x => x.Names)
                .Properties()
                .Value()
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).properties(_b).properties().value()")
                .WithParameters("Company", "Names");
        }

        [Fact]
        public void Properties_Properties_Where_key()
        {
            _g
                .V<Company>()
                .Properties(x => x.Names)
                .Properties()
                .Where(x => x.Key == "someKey")
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).properties(_b).properties().where(__.key().is(_c))")
                .WithParameters("Company", "Names", "someKey");
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
                .Should()
                .SerializeToGroovy("inject(_a).as(_b).V().hasLabel(_c).properties(_d).properties().where(__.key().where(eq(_b)))")
                .WithParameters("hello", "l1", "Company", "Names");
        }

        [Fact]
        public void Properties_Properties1()
        {
            _g
                .V<Country>()
                .Properties(x => x.Name)
                .Properties()
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).properties(_b).properties()")
                .WithParameters("Country", "Name");
        }

        [Fact]
        public void Properties_Properties2()
        {
            _g
                .V<Company>()
                .Properties(x => x.Names)
                .Properties()
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).properties(_b).properties()")
                .WithParameters("Company", "Names");
        }

        [Fact]
        public void Properties_typed_no_parameters()
        {
            _g
                .V()
                .Properties<string>()
                .Should()
                .SerializeToGroovy("V().properties()")
                .WithoutParameters();
        }

        [Fact]
        public void Properties_ValueMap_typed()
        {
            _g
                .V()
                .Properties()
                .ValueMap<string>()
                .Should()
                .SerializeToGroovy("V().properties().valueMap()")
                .WithoutParameters();
        }

        [Fact]
        public void Properties_ValueMap_untyped()
        {
            _g
                .V()
                .Properties()
                .ValueMap()
                .Should()
                .SerializeToGroovy("V().properties().valueMap()")
                .WithoutParameters();
        }

        [Fact]
        public void Properties_Values_Id()
        {
            _g
                .V()
                .Properties()
                .Values(x => x.Id)
                .Should()
                .SerializeToGroovy("V().properties().id()")
                .WithoutParameters();
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
                .Should()
                .SerializeToGroovy("V().properties().union(__.label(), __.id())")
                .WithoutParameters();
        }

        [Fact]
        public void Properties_Values_Label()
        {
            _g
                .V()
                .Properties()
                .Values(x => x.Label)
                .Should()
                .SerializeToGroovy("V().properties().label()")
                .WithoutParameters();
        }

        [Fact]
        public void Properties_Values_untyped()
        {
            _g
                .V()
                .Properties()
                .Values()
                .Should()
                .SerializeToGroovy("V().properties().values()")
                .WithoutParameters();
        }

        [Fact]
        public void Properties_Values2()
        {
            _g
                .V()
                .Properties()
                .Values<int>("MetaProperty")
                .Should()
                .SerializeToGroovy("V().properties().values(_a)")
                .WithParameters("MetaProperty");
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
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).properties().has(_b, _c)")
                .WithParameters("Person", "MetaKey", "MetaValue");
        }

        [Fact]
        public void Properties_Where_Dictionary_key2()
        {
            _g
                .V<Person>()
                .Properties()
                .Where(x => (int)x.Properties["MetaKey"] < 100)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).properties().has(_b, lt(_c))")
                .WithParameters("Person", "MetaKey", 100);
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
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).properties(_b).has(id, _c)")
                .WithParameters("Country", "Languages", "id");
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
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).properties(_b).has(id, _c)")
                .WithParameters("Country", "Languages", "id");
        }

        [Fact]
        public void Properties_Where_label()
        {
            _g
                .V<Company>()
                .Properties(x => x.Names)
                .Where(x => x.Label == "someKey")
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).properties(_b).where(__.label().is(_c))")
                .WithParameters("Company", "Names", "someKey");
        }

        [Fact]
        public void Properties_Where_Label()
        {
            _g
                .V<Country>()
                .Properties(x => x.Languages)
                .Where(x => x.Label == "label")
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).properties(_b).where(__.label().is(_c))")
                .WithParameters("Country", "Languages", "label");
        }

        [Fact]
        public void Properties_Where_Meta_key()
        {
            _g
                .V<Company>()
                .Properties(x => x.Names)
                .Where(x => x.Properties.ValidFrom == DateTimeOffset.Parse("01.01.2019 08:00"))
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).properties(_b).has(_c, _d)")
                .WithParameters("Company", "Names", "ValidFrom", DateTimeOffset.Parse("01.01.2019 08:00"));
        }

        [Fact]
        public void Properties_Where_Meta_key_reversed()
        {
            _g
                .V<Company>()
                .Properties(x => x.Names)
                .Where(x => DateTimeOffset.Parse("01.01.2019 08:00") == x.Properties.ValidFrom)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).properties(_b).has(_c, _d)")
                .WithParameters("Company", "Names", "ValidFrom", DateTimeOffset.Parse("01.01.2019 08:00"));
        }

        [Fact]
        public void Properties_Where_reversed()
        {
            _g
                .V<Country>()
                .Properties(x => x.Languages)
                .Where(x => "de" == x.Value)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).properties(_b).hasValue(_c)")
                .WithParameters("Country", "Languages", "de");
        }

        [Fact]
        public void Properties_Where1()
        {
            _g
                .V<Country>()
                .Properties(x => x.Languages)
                .Where(x => x.Value == "de")
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).properties(_b).hasValue(_c)")
                .WithParameters("Country", "Languages", "de");
        }

        [Fact]
        public void Properties_Where2()
        {
            _g
                .V<Country>()
                .Properties()
                .Where(x => (int)x.Value < 10)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).properties().hasValue(lt(_b))")
                .WithParameters("Country", 10);
        }

        [Fact]
        public void Properties1()
        {
            _g
                .V()
                .Properties()
                .Should()
                .SerializeToGroovy("V().properties()")
                .WithoutParameters();
        }

        [Fact]
        public void Properties2()
        {
            _g
                .E()
                .Properties()
                .Should()
                .SerializeToGroovy("E().properties()")
                .WithoutParameters();
        }

        [Fact]
        public void Property_list()
        {
            _g
                .V<Company>("id")
                .Property(x => x.PhoneNumbers, "+4912345")
                .Should()
                .SerializeToGroovy("V(_a).hasLabel(_b).property(list, _c, _d).project(_a, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.properties().group().by(__.label()).by(__.project(_a, _e, _i, _g).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("id", "Company", "PhoneNumbers", "+4912345", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Property_null()
        {
            _g
                .V<Company>("id")
                .Property<string>(x => x.PhoneNumbers, null)
                .Should()
                .SerializeToGroovy("V(_a).hasLabel(_b).sideEffect(__.properties(_c).drop()).project(_a, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_a, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("id", "Company", "PhoneNumbers", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Property_single()
        {
            _g
                .V<Person>()
                .Property(x => x.Age, 36)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).property(single, _b, _c).project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.properties().group().by(__.label()).by(__.project(_d, _e, _i, _g).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "Age", 36, "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Repeat_Out()
        {
            _g
                .V<Person>()
                .Repeat(__ => __
                    .Out<WorksFor>()
                    .OfType<Person>())
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).repeat(__.out(_b).hasLabel(_a)).project(_c, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_c, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "WorksFor", "id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).repeat(__.inE().outV()).until(__.V().hasLabel(_b)).project(_c, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_c, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "Company", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void ReplaceE()
        {
            var now = DateTime.UtcNow;
            var id = Guid.NewGuid();

            var worksFor = new WorksFor { Id = id, From = now, To = now, Role = "Admin" };

            _g
                .ReplaceE(worksFor)
                .Should()
                .SerializeToGroovy("E(_a).hasLabel(_b).sideEffect(__.properties(_c, _d, _e).drop()).property(_c, _f).property(_d, _g).property(_e, _f).project(_h, _i, _j, _k).by(id).by(label).by(__.constant(_l)).by(__.valueMap())")
                .WithParameters(id, nameof(WorksFor), nameof(WorksFor.From), nameof(WorksFor.Role), nameof(WorksFor.To), now, "Admin", "id", "label", "type", "properties", "edge");
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
                .Should()
                .SerializeToGroovy("E(_a).hasLabel(_b).sideEffect(__.properties(_c, _d, _e).drop()).property(_c, _f).property(_d, _g).property(_e, _f).project(_h, _i, _j, _k).by(id).by(label).by(__.constant(_l)).by(__.valueMap())")
                .WithParameters(id, nameof(WorksFor), nameof(WorksFor.From), nameof(WorksFor.Role), nameof(WorksFor.To), now, "Admin", "id", "label", "type", "properties", "edge");
        }

        [Fact]
        public void ReplaceV()
        {
            var now = DateTimeOffset.UtcNow;
            var id = Guid.NewGuid();
            var person = new Person { Id = id, Age = 21, Gender = Gender.Male, Name = "Marko", RegistrationDate = now };

            _g
                .ReplaceV(person)
                .Should()
                .SerializeToGroovy("V(_a).hasLabel(_b).sideEffect(__.properties(_c, _d, _e, _f).drop()).property(single, _c, _g).property(single, _d, _h).property(single, _e, _i).property(single, _f, _j).project(_k, _l, _m, _n).by(id).by(label).by(__.constant(_o)).by(__.properties().group().by(__.label()).by(__.project(_k, _l, _p, _n).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters(id, nameof(Person), nameof(Person.Age), nameof(Person.Gender), nameof(Person.Name), nameof(Person.RegistrationDate), 21, Gender.Male, "Marko", now, "id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("V(_a).hasLabel(_b).sideEffect(__.properties(_c, _d, _e).drop()).property(single, _c, _f).property(single, _d, _g).property(single, _e, _h).project(_i, _j, _k, _l).by(id).by(label).by(__.constant(_m)).by(__.properties().group().by(__.label()).by(__.project(_i, _j, _n, _l).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters(id, nameof(Person), nameof(Person.Age), nameof(Person.Gender), nameof(Person.Name), 21, Gender.Male, "Marko", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Set_Meta_Property_to_null()
        {
            _g
                .V<Country>()
                .Properties(x => x.Name)
                .Property("metaKey", null)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).properties(_b).sideEffect(__.properties(_c).drop())")
                .WithParameters("Country", "Name", "metaKey");
        }

        [Fact]
        public void Set_Meta_Property1()
        {
            _g
                .V<Country>()
                .Properties(x => x.Name)
                .Property("metaKey", 1)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).properties(_b).property(_c, _d)")
                .WithParameters("Country", "Name", "metaKey", 1);
        }

        [Fact]
        public void Set_Meta_Property2()
        {
            var d = DateTimeOffset.Now;

            _g
                .V<Person>()
                .Properties(x => x.Name)
                .Property(x => x.ValidFrom, d)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).properties(_b).property(_c, _d)")
                .WithParameters("Person", "Name", "ValidFrom", d);
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
                .Should()
                .SerializeToGroovy("inject(_a, _b, _c).fold().as(_d).V().hasLabel(_e).has(_f, __.where(within(_d))).project(_g, _h, _i, _j).by(id).by(label).by(__.constant(_k)).by(__.properties().group().by(__.label()).by(__.project(_g, _h, _l, _j).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters(1, 2, 3, "l1", "Person", "Age", "id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("V().fold().as(_a).V().hasLabel(_b).where(within(_a)).count()")
                .WithParameters("l1", "Person");
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
                .Should()
                .SerializeToGroovy("V().fold().as(_a).V().hasLabel(_b).not(__.where(within(_a))).count()")
                .WithParameters("l1", "Person");
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
                .Should()
                .SerializeToGroovy("inject(_a, _b, _c).fold().as(_d).V().hasLabel(_e).has(_f, __.where(within(_d))).project(_g, _h, _i, _j).by(id).by(label).by(__.constant(_k)).by(__.properties().group().by(__.label()).by(__.project(_g, _h, _l, _j).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters(1, 2, 3, "l1", "Person", "Age", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void SumGlobal()
        {
            _g
                .V<Person>()
                .Values(x => x.Age)
                .Sum()
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).values(_b).sum()")
                .WithParameters("Person", "Age");
        }

        [Fact]
        public void SumLocal()
        {
            _g
                .V<Person>()
                .Values(x => x.Age)
                .SumLocal()
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).values(_b).sum(local)")
                .WithParameters("Person", "Age");
        }

        [Fact]
        public void SumLocal_Where1()
        {
            _g
                .V<Person>()
                .Values(x => x.Age)
                .SumLocal()
                .Where(x => x == 100)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).values(_b).sum(local).is(_c)")
                .WithParameters("Person", "Age", 100);
        }

        [Fact]
        public void SumLocal_Where2()
        {
            _g
                .V<Person>()
                .Values(x => x.Age)
                .SumLocal()
                .Where(x => x < 100)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).values(_b).sum(local).is(lt(_c))")
                .WithParameters("Person", "Age", 100);
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
                .Should()
                .SerializeToGroovy("V().tail(_a).project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_b, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters(1, "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void TailLocal()
        {
            _g
                .V()
                .TailLocal(1)
                .Should()
                .SerializeToGroovy("V().tail(local, _a).project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_b, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters(1, "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Union()
        {
            _g
                .V<Person>()
                .Union(
                    __ => __.Out<WorksFor>(),
                    __ => __.Out<LivesIn>())
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).union(__.out(_b), __.out(_c)).project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.properties().group().by(__.label()).by(__.project(_d, _e, _i, _g).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "WorksFor", "LivesIn", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Union_different_types()
        {
            _g
                .V<Person>()
                .Union<IGremlinQueryBase>(
                    __ => __.Out<WorksFor>(),
                    __ => __.OutE<LivesIn>())
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).union(__.out(_b), __.outE(_c))")
                .WithParameters("Person", "WorksFor", "LivesIn");
        }


        [Fact]
        public void Union_different_types2()
        {
            _g
                .V<Person>()
                .Union(
                    __ => __.Out<WorksFor>().Lower(),
                    __ => __.OutE<LivesIn>().Lower().Cast<object>())
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).union(__.out(_b), __.outE(_c))")
                .WithParameters("Person", "WorksFor", "LivesIn");
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
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).until(__.V().hasLabel(_b)).repeat(__.inE().outV()).project(_c, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_c, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "Company", "id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).sideEffect(__.properties(_b, _c, _d, _e).drop()).property(single, _b, _f).property(single, _c, _g).property(single, _d, _h).property(single, _e, _i).outE(_j).sideEffect(__.properties(_k, _l, _m).drop()).property(_k, _n).property(_l, _o).property(_m, _n).project(_p, _q, _r, _s).by(id).by(label).by(__.constant(_t)).by(__.valueMap())")
                .WithParameters(nameof(Person), nameof(Person.Age), nameof(Person.Gender), nameof(Person.Name), nameof(Person.RegistrationDate), person.Age, person.Gender, "Marko", person.RegistrationDate, nameof(WorksFor), nameof(WorksFor.From), nameof(WorksFor.Role), nameof(WorksFor.To), worksFor.From, worksFor.Role, "id", "label", "type", "properties", "edge");
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
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).sideEffect(__.properties(_b, _c).drop()).property(single, _b, _d).property(single, _c, _e).outE(_f).sideEffect(__.properties(_g).drop()).property(_g, _h).project(_i, _j, _k, _l).by(id).by(label).by(__.constant(_m)).by(__.valueMap())")
                .WithParameters(nameof(Person), nameof(Person.Gender), nameof(Person.RegistrationDate), person.Gender, person.RegistrationDate, nameof(WorksFor), nameof(WorksFor.To), worksFor.To, "id", "label", "type", "properties", "edge");
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
                .Should()
                .SerializeToGroovy("E().hasLabel(_a).sideEffect(__.properties(_b).drop()).property(_b, _c).project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.valueMap())")
                .WithParameters(nameof(WorksFor), nameof(WorksFor.To), now, "id", "label", "type", "properties", "edge");
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
                .Should()
                .SerializeToGroovy("E().hasLabel(_a).sideEffect(__.properties(_b).drop()).property(_b, _c).project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.valueMap())")
                .WithParameters(nameof(WorksFor), nameof(WorksFor.To), now, "id", "label", "type", "properties", "edge");
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
                .Should()
                .SerializeToGroovy("E().hasLabel(_a).sideEffect(__.properties(_b).drop()).property(_b, _c).project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.valueMap())")
                .WithParameters(nameof(WorksFor), nameof(WorksFor.To), now, "id", "label", "type", "properties", "edge");
        }

        [Fact]
        public void UpdateV_No_Config()
        {
            var now = DateTimeOffset.UtcNow;

            _g
                .V<Person>()
                .Update(new Person { Age = 21, Gender = Gender.Male, Name = "Marko", RegistrationDate = now })
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).sideEffect(__.properties(_b, _c, _d, _e).drop()).property(single, _b, _f).property(single, _c, _g).property(single, _d, _h).property(single, _e, _i).project(_j, _k, _l, _m).by(id).by(label).by(__.constant(_n)).by(__.properties().group().by(__.label()).by(__.project(_j, _k, _o, _m).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters(nameof(Person), nameof(Person.Age), nameof(Person.Gender), nameof(Person.Name), nameof(Person.RegistrationDate), 21, Gender.Male, "Marko", now, "id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).sideEffect(__.properties(_b, _c).drop()).property(single, _b, _d).property(single, _c, _e).project(_f, _g, _h, _i).by(id).by(label).by(__.constant(_j)).by(__.properties().group().by(__.label()).by(__.project(_f, _g, _k, _i).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters(nameof(Person), nameof(Person.Name), nameof(Person.RegistrationDate), "Marko", now, "id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).sideEffect(__.properties(_b, _c).drop()).property(single, _b, _d).property(single, _c, _e).project(_f, _g, _h, _i).by(id).by(label).by(__.constant(_j)).by(__.properties().group().by(__.label()).by(__.project(_f, _g, _k, _i).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters(nameof(Person), nameof(Person.Name), nameof(Person.RegistrationDate), "Marko", now, "id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).sideEffect(__.properties(_b, _c).drop()).property(single, _b, _d).property(single, _c, _e).project(_f, _g, _h, _i).by(id).by(label).by(__.constant(_j)).by(__.properties().group().by(__.label()).by(__.project(_f, _g, _k, _i).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters(nameof(Person), nameof(Person.Name), nameof(Person.RegistrationDate), "Marko", now, "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void V_Both()
        {
            _g
                .V()
                .Both<Edge>()
                .Should()
                .SerializeToGroovy("V().both(_a, _b, _c, _d).project(_e, _f, _g, _h).by(id).by(label).by(__.constant(_i)).by(__.properties().group().by(__.label()).by(__.project(_e, _f, _j, _h).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Knows", "LivesIn", "Speaks", "WorksFor", "id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("V().hasLabel(_a, _b).has(_c, _d).project(_e, _f, _g, _h).by(id).by(label).by(__.constant(_i)).by(__.properties().group().by(__.label()).by(__.project(_e, _f, _j, _h).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Company", "Person", "n", "some name", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void V_of_abstract_type()
        {
            _g
                .V<Authority>()
                .Should()
                .SerializeToGroovy("V().hasLabel(_a, _b).project(_c, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_c, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Company", "Person", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void V_of_all_types1()
        {
            _g
                .V<object>()
                .Should()
                .SerializeToGroovy("V().project(_a, _b, _c, _d).by(id).by(label).by(__.constant(_e)).by(__.properties().group().by(__.label()).by(__.project(_a, _b, _f, _d).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void V_of_all_types2()
        {
            _g
                .V()
                .Should()
                .SerializeToGroovy("V().project(_a, _b, _c, _d).by(id).by(label).by(__.constant(_e)).by(__.properties().group().by(__.label()).by(__.project(_a, _b, _f, _d).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void V_of_concrete_type()
        {
            _g
                .V<Person>()
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_b, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void V_untyped()
        {
            _g
                .V()
                .Should()
                .SerializeToGroovy("V().project(_a, _b, _c, _d).by(id).by(label).by(__.constant(_e)).by(__.properties().group().by(__.label()).by(__.project(_a, _b, _f, _d).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void V_untyped_without_metaproperties()
        {
            _g
                .ConfigureEnvironment(env => env
                    .ConfigureFeatureSet(set => set.ConfigureVertexFeatures(features => features & ~VertexFeatures.MetaProperties)))
                .V()
                .Should()
                .SerializeToGroovy("V().project(_a, _b, _c, _d).by(id).by(label).by(__.constant(_e)).by(__.properties().group().by(__.label()).by(__.project(_a, _b, _f).by(id).by(__.label()).by(__.value()).fold()))")
                .WithParameters("id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Value()
        {
            _g
                .V()
                .Properties()
                .Value()
                .Should()
                .SerializeToGroovy("V().properties().value()");
        }

        [Fact]
        public void ValueMap_typed()
        {
            _g
                .V<Person>()
                .ValueMap(x => x.Age)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).valueMap(_b)")
                .WithParameters("Person", "Age");
        }

        [Fact]
        public void ValueMap_untyped()
        {
            _g
                .V<Person>()
                .ValueMap("key")
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).valueMap(_b)")
                .WithParameters("Person", "key");
        }

        [Fact]
        public void Values_1_member()
        {
            _g
                .V<Person>()
                .Values(x => x.Age)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).values(_b)")
                .WithParameters("Person", "Age");
        }

        [Fact]
        public void Values_2_members()
        {
            _g
                .V<Person>()
                .Values(x => x.Name, x => x.Id)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).union(__.id(), __.values(_b))")
                .WithParameters("Person", "Name");
        }

        [Fact]
        public void Values_3_members()
        {
            _g
                .V<Person>()
                .Values(x => x.Name, x => x.Gender, x => x.Id)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).union(__.id(), __.values(_b, _c))")
                .WithParameters("Person", "Name", "Gender");
        }

        [Fact]
        public void Values_id_member()
        {
            _g
                .V<Person>()
                .Values(x => x.Id)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).id()")
                .WithParameters("Person");
        }

        [Fact]
        public void Values_no_member()
        {
            _g
                .V<Person>()
                .Values()
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).values()")
                .WithParameters("Person");
        }

        [Fact]
        public void Values_of_Edge()
        {
            _g
                .E<LivesIn>()
                .Values(x => x.Since)
                .Should()
                .SerializeToGroovy("E().hasLabel(_a).values(_b)")
                .WithParameters("LivesIn", "Since");
        }

        [Fact]
        public void Values_of_Vertex1()
        {
            _g
                .V<Person>()
                .Values(x => x.Name)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).values(_b)")
                .WithParameters("Person", "Name");
        }

        [Fact]
        public void Values_of_Vertex2()
        {
            _g
                .V<Person>()
                .Values(x => x.Name)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).values(_b)")
                .WithParameters("Person", "Name");
        }

        [Fact]
        public void Values_string_key()
        {
            _g
                .V<Person>()
                .Values("key")
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).values(_b)")
                .WithParameters("Person", "key");
        }

        [Fact]
        public void Variable_wrap()
        {
            _g
                .V()
                .Properties()
                .Properties("1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30")
                .Should()
                .SerializeToGroovy("V().properties().properties(_a, _b, _c, _d, _e, _f, _g, _h, _i, _j, _k, _l, _m, _n, _o, _p, _q, _r, _s, _t, _u, _v, _w, _x, _y, _z, _ba, _bb, _bc, _bd)")
                .WithParameters("1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30");
        }

        [Fact]
        public void Where_anonymous()
        {
            _g
                .V<Person>()
                .Where(_ => _)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_b, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_array_does_not_intersect_property_array()
        {
            _g
                .V<Company>()
                .Where(t => !new[] { "+4912345", "+4923456" }.Intersect(t.PhoneNumbers).Any())
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).not(__.has(_b, within(_c, _d))).project(_e, _f, _g, _h).by(id).by(label).by(__.constant(_i)).by(__.properties().group().by(__.label()).by(__.project(_e, _f, _j, _h).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Company", "PhoneNumbers", "+4912345", "+4923456", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_array_intersects_property_aray()
        {
            _g
                .V<Company>()
                .Where(t => new[] { "+4912345", "+4923456" }.Intersect(t.PhoneNumbers).Any())
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(_b, within(_c, _d)).project(_e, _f, _g, _h).by(id).by(label).by(__.constant(_i)).by(__.properties().group().by(__.label()).by(__.project(_e, _f, _j, _h).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Company", "PhoneNumbers", "+4912345", "+4923456", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_bool_property_explicit_comparison1()
        {
            _g
                .V<TimeFrame>()
                // ReSharper disable once RedundantBoolCompare
                .Where(t => t.Enabled == true)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(_b, _c).project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.properties().group().by(__.label()).by(__.project(_d, _e, _i, _g).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("TimeFrame", "Enabled", true, "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_bool_property_explicit_comparison2()
        {
            _g
                .V<TimeFrame>()
                .Where(t => t.Enabled == false)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(_b, _c).project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.properties().group().by(__.label()).by(__.project(_d, _e, _i, _g).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("TimeFrame", "Enabled", false, "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_bool_property_implicit_comparison1()
        {
            _g
                .V<TimeFrame>()
                .Where(t => t.Enabled)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(_b, _c).project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.properties().group().by(__.label()).by(__.project(_d, _e, _i, _g).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("TimeFrame", "Enabled", true, "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_bool_property_implicit_comparison2()
        {
            _g
                .V<TimeFrame>()
                .Where(t => !t.Enabled)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).not(__.has(_b, _c)).project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.properties().group().by(__.label()).by(__.project(_d, _e, _i, _g).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("TimeFrame", "Enabled", true, "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_complex_logical_expression()
        {
            _g
                .V<Person>()
                .Where(t => t.Name.Value == "Some name" && (t.Age == 42 || t.Age == 99))
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(_b, _c).has(_d, eq(_e).or(eq(_f))).project(_g, _h, _i, _j).by(id).by(label).by(__.constant(_k)).by(__.properties().group().by(__.label()).by(__.project(_g, _h, _l, _j).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "Name", "Some name", "Age", 42, 99, "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_complex_logical_expression_with_null()
        {
            _g
                .V<Person>()
                .Where(t => t.Name == null && (t.Age == 42 || t.Age == 99))
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).hasNot(_b).has(_c, eq(_d).or(eq(_e))).project(_f, _g, _h, _i).by(id).by(label).by(__.constant(_j)).by(__.properties().group().by(__.label()).by(__.project(_f, _g, _k, _i).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "Name", "Age", 42, 99, "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_conjunction()
        {
            _g
                .V<Person>()
                .Where(t => t.Age == 36 && t.Age == 42)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(_b, eq(_c).and(eq(_d))).project(_e, _f, _g, _h).by(id).by(label).by(__.constant(_i)).by(__.properties().group().by(__.label()).by(__.project(_e, _f, _j, _h).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "Age", 36, 42, "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact(Skip="Optimizable")]
        public void Where_conjunction_optimizable()
        {
            _g
                .V<Person>()
                .Where(t => (t.Age == 36 && t.Name.Value == "Hallo") && t.Age == 42)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(_b, eq(_c).and(eq(_d))).has(_b, eq(_e))")
                .WithParameters("Person", "Age", 36, "Name", 42);
        }

        [Fact]
        public void Where_conjunction_with_different_fields()
        {
            _g
                .V<Person>()
                .Where(t => t.Name.Value == "Some name" && t.Age == 42)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(_b, _c).has(_d, _e).project(_f, _g, _h, _i).by(id).by(label).by(__.constant(_j)).by(__.properties().group().by(__.label()).by(__.project(_f, _g, _k, _i).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "Name", "Some name", "Age", 42, "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_converted_Id_equals_constant()
        {
            _g
                .V<Language>()
                .Where(t => (int)t.Id == 1)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(id, _b).project(_c, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_c, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Language", 1, "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_current_element_equals_stepLabel1()
        {
            _g
                .V<Language>()
                .As((__, l) => __
                    .V<Language>()
                    .Where(l2 => l2 == l))
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).as(_b).V().hasLabel(_a).where(eq(_b)).project(_c, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_c, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Language", "l1", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_current_element_equals_stepLabel2()
        {
            _g
                .V<Language>()
                .As((__, l) => __
                    .V<Language>()
                    .Where(l2 => l == l2))
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).as(_b).V().hasLabel(_a).where(eq(_b)).project(_c, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_c, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Language", "l1", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_current_element_not_equals_stepLabel1()
        {
            _g
                .V<Language>()
                .As((__, l) => __
                    .V<Language>()
                    .Where(l2 => l2 != l))
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).as(_b).V().hasLabel(_a).where(neq(_b)).project(_c, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_c, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Language", "l1", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_current_element_not_equals_stepLabel2()
        {
            _g
                .V<Language>()
                .As((__, l) => __
                    .V<Language>()
                    .Where(l2 => l != l2))
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).as(_b).V().hasLabel(_a).where(neq(_b)).project(_c, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_c, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Language", "l1", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_disjunction()
        {
            _g
                .V<Person>()
                .Where(t => t.Age == 36 || t.Age == 42)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(_b, eq(_c).or(eq(_d))).project(_e, _f, _g, _h).by(id).by(label).by(__.constant(_i)).by(__.properties().group().by(__.label()).by(__.project(_e, _f, _j, _h).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "Age", 36, 42, "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_disjunction_with_different_fields()
        {
            _g
                .V<Person>()
                .Where(t => t.Name.Value == "Some name" || t.Age == 42)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).or(__.has(_b, _c), __.has(_d, _e)).project(_f, _g, _h, _i).by(id).by(label).by(__.constant(_j)).by(__.properties().group().by(__.label()).by(__.project(_f, _g, _k, _i).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "Name", "Some name", "Age", 42, "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_empty_array_does_not_intersect_property_array()
        {
            _g
                .V<Company>()
                .Where(t => !new string[0].Intersect(t.PhoneNumbers).Any())
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_b, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Company", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_empty_array_intersects_property_array()
        {
            _g
                .V<Company>()
                .Where(t => new string[0].Intersect(t.PhoneNumbers).Any())
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).not(__.identity()).project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_b, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Company", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_has_conjunction_of_three()
        {
            _g
                .V<Person>()
                .Where(t => t.Age == 36 && t.Age == 42 && t.Age == 99)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(_b, eq(_c).and(eq(_d)).and(eq(_e))).project(_f, _g, _h, _i).by(id).by(label).by(__.constant(_j)).by(__.properties().group().by(__.label()).by(__.project(_f, _g, _k, _i).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "Age", 36, 42, 99, "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_has_disjunction_of_three()
        {
            _g
                .V<Person>()
                .Where(t => t.Age == 36 || t.Age == 42 || t.Age == 99)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(_b, eq(_c).or(eq(_d)).or(eq(_e))).project(_f, _g, _h, _i).by(id).by(label).by(__.constant(_j)).by(__.properties().group().by(__.label()).by(__.project(_f, _g, _k, _i).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "Age", 36, 42, 99, "id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(_b, eq(_c).or(eq(_d)).or(eq(_e))).project(_f, _g, _h, _i).by(id).by(label).by(__.constant(_j)).by(__.properties().group().by(__.label()).by(__.project(_f, _g, _k, _i).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "Age", 36, 42, 99, "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_Id_equals_constant()
        {
            _g
                .V<Language>()
                .Where(t => t.Id == (object)1)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(id, _b).project(_c, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_c, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Language", 1, "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_identity()
        {
            _g
                .V<Person>()
                .Where(_ => _.Identity())
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_b, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_identity_with_type_change()
        {
            _g
                .V<Person>()
                .Where(_ => _.OfType<Authority>())
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_b, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_none_traversal()
        {
            _g
                .V<Person>()
                .Where(_ => _.None())
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).not(__.identity()).project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_b, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_not_none()
        {
            _g
                .V<Person>()
                .Where(_ => _
                    .Not(_ => _
                        .None()))
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_b, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_or_dead_traversal()
        {
            _g
                .V<Person>()
                .Where(_ => _
                    .Or(_ => _
                        .Where(x => new object[0].Contains(x.Id))))
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).not(__.identity()).project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_b, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_or_identity()
        {
            _g
                .V<Person>()
                .Where(_ => _
                    .Or(_ => _))
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_b, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_or_none_traversal()
        {
            _g
                .V<Person>()
                .Where(_ => _
                    .Or(_ => _
                        .None()))
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).not(__.identity()).project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_b, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(id, _b).project(_c, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_c, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))");
        }

        [Fact]
        public void Where_property_array_contains_element()
        {
            _g
                .V<Company>()
                .Where(t => t.PhoneNumbers.Contains("+4912345"))
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(_b, _c).project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.properties().group().by(__.label()).by(__.project(_d, _e, _i, _g).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Company", "PhoneNumbers", "+4912345", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_property_array_contains_stepLabel()
        {
            _g
                .Inject("+4912345")
                .As((__, t) => __
                    .V<Company>()
                    .Where(c => c.PhoneNumbers.Contains(t)))
                .Should()
                .SerializeToGroovy("inject(_a).as(_b).V().hasLabel(_c).has(_d, __.where(eq(_b))).project(_e, _f, _g, _h).by(id).by(label).by(__.constant(_i)).by(__.properties().group().by(__.label()).by(__.project(_e, _f, _j, _h).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("+4912345", "l1", "Company", "PhoneNumbers", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_property_array_does_not_contain_element()
        {
            _g
                .V<Company>()
                .Where(t => !t.PhoneNumbers.Contains("+4912345"))
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).not(__.has(_b, _c)).project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.properties().group().by(__.label()).by(__.project(_d, _e, _i, _g).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Company", "PhoneNumbers", "+4912345", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_property_array_does_not_intersect_array()
        {
            _g
                .V<Company>()
                .Where(t => !t.PhoneNumbers.Intersect(new[] { "+4912345", "+4923456" }).Any())
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).not(__.has(_b, within(_c, _d))).project(_e, _f, _g, _h).by(id).by(label).by(__.constant(_i)).by(__.properties().group().by(__.label()).by(__.project(_e, _f, _j, _h).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Company", "PhoneNumbers", "+4912345", "+4923456", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_property_array_does_not_intersect_empty_array()
        {
            _g
                .V<Company>()
                .Where(t => !t.PhoneNumbers.Intersect(new string[0]).Any())
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_b, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Company", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_property_array_intersects_array1()
        {
            _g
                .V<Company>()
                .Where(t => t.PhoneNumbers.Intersect(new[] { "+4912345", "+4923456" }).Any())
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(_b, within(_c, _d)).project(_e, _f, _g, _h).by(id).by(label).by(__.constant(_i)).by(__.properties().group().by(__.label()).by(__.project(_e, _f, _j, _h).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Company", "PhoneNumbers", "+4912345", "+4923456", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_property_array_intersects_array2()
        {
            _g
                .V<Company>()
                .Where(t => new[] { "+4912345", "+4923456" }.Intersect(t.PhoneNumbers).Any())
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(_b, within(_c, _d)).project(_e, _f, _g, _h).by(id).by(label).by(__.constant(_i)).by(__.properties().group().by(__.label()).by(__.project(_e, _f, _j, _h).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Company", "PhoneNumbers", "+4912345", "+4923456", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_property_array_intersects_empty_array()
        {
            _g
                .V<Company>()
                .Where(t => t.PhoneNumbers.Intersect(new string[0]).Any())
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).not(__.identity()).project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_b, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Company", "id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("inject(_a).fold().as(_b).V().hasLabel(_c).has(_d, __.where(within(_b))).project(_e, _f, _g, _h).by(id).by(label).by(__.constant(_i)).by(__.properties().group().by(__.label()).by(__.project(_e, _f, _j, _h).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("+4912345", "l1", "Company", "PhoneNumbers", "id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("inject(_a).fold().as(_b).V().hasLabel(_c).has(_d, __.where(within(_b))).project(_e, _f, _g, _h).by(id).by(label).by(__.constant(_i)).by(__.properties().group().by(__.label()).by(__.project(_e, _f, _j, _h).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("+4912345", "l1", "Company", "PhoneNumbers", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_property_array_is_empty()
        {
            _g
                .V<Company>()
                .Where(t => !t.PhoneNumbers.Any())
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).not(__.has(_b)).project(_c, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_c, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Company", "PhoneNumbers", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_property_array_is_not_empty()
        {
            _g
                .V<Company>()
                .Where(t => t.PhoneNumbers.Any())
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(_b).project(_c, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_c, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Company", "PhoneNumbers", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_property_contains_constant_with_TextP_support()
        {
            _g
                .V<Country>()
                .Where(c => c.CountryCallingCode.Contains("456"))
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(_b, containing(_c)).project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.properties().group().by(__.label()).by(__.project(_d, _e, _i, _g).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Country", "CountryCallingCode", "456", "id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(_b).project(_c, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_c, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Country", "CountryCallingCode", "id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(_b).project(_c, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_c, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Country", "CountryCallingCode", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_property_ends_with_constant_with_TextP_support()
        {
            _g
                .V<Country>()
                .Where(c => c.CountryCallingCode.EndsWith("7890"))
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(_b, endingWith(_c)).project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.properties().group().by(__.label()).by(__.project(_d, _e, _i, _g).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Country", "CountryCallingCode", "7890", "id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(_b).project(_c, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_c, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Country", "CountryCallingCode", "id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(_b).project(_c, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_c, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Country", "CountryCallingCode", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_property_equals_constant()
        {
            _g
                .V<Person>()
                .Where(t => t.Age == 36)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(_b, _c).project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.properties().group().by(__.label()).by(__.project(_d, _e, _i, _g).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "Age", 36, "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_property_equals_converted_expression()
        {
            _g
                .V<Person>()
                .Where(t => (object)t.Age == (object)36)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(_b, _c).project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.properties().group().by(__.label()).by(__.project(_d, _e, _i, _g).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "Age", 36, "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_property_equals_expression()
        {
            const int i = 18;

            _g
                .V<Person>()
                .Where(t => t.Age == i + i)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(_b, _c).project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.properties().group().by(__.label()).by(__.project(_d, _e, _i, _g).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "Age", 36, "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_property_equals_local_string_constant()
        {
            const int local = 1;

            _g
                .V<Language>()
                .Where(t => t.Id == (object)local)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(id, _b).project(_c, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_c, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Language", local, "id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).values(_b).as(_c).V().hasLabel(_a).has(_b, __.where(eq(_c))).project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.properties().group().by(__.label()).by(__.project(_d, _e, _i, _g).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Language", "IetfLanguageTag", "l1", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_property_equals_value_of_anonymous_object()
        {
            var local = new { Value = 1 };

            _g
                .V<Language>()
                .Where(t => t.Id == (object)local.Value)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(id, _b).project(_c, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_c, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Language", 1, "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_property_is_contained_in_array()
        {
            _g
                .V<Person>()
                .Where(t => new[] { 36, 37, 38 }.Contains(t.Age))
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(_b, within(_c, _d, _e)).project(_f, _g, _h, _i).by(id).by(label).by(__.constant(_j)).by(__.properties().group().by(__.label()).by(__.project(_f, _g, _k, _i).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "Age", 36, 37, 38, "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_property_is_contained_in_empty_enumerable()
        {
            var enumerable = Enumerable.Empty<int>();

            _g
                .V<Person>()
                .Where(t => enumerable.Contains(t.Age))
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).not(__.identity()).project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_b, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_property_is_contained_in_enumerable()
        {
            var enumerable = new[] { "36", "37", "38" }
                .Select(int.Parse);

            _g
                .V<Person>()
                .Where(t => enumerable.Contains(t.Age))
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(_b, within(_c, _d, _e)).project(_f, _g, _h, _i).by(id).by(label).by(__.constant(_j)).by(__.properties().group().by(__.label()).by(__.project(_f, _g, _k, _i).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "Age", 36, 37, 38, "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_property_is_greater_or_equal_than_constant()
        {
            _g
                .V<Person>()
                .Where(t => t.Age >= 36)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(_b, gte(_c)).project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.properties().group().by(__.label()).by(__.project(_d, _e, _i, _g).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "Age", 36, "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_property_is_greater_than_constant()
        {
            _g
                .V<Person>()
                .Where(t => t.Age > 36)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(_b, gt(_c)).project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.properties().group().by(__.label()).by(__.project(_d, _e, _i, _g).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "Age", 36, "id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).values(_b).as(_c).V().hasLabel(_a).has(_b, __.where(gte(_c))).project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.properties().group().by(__.label()).by(__.project(_d, _e, _i, _g).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "Age", "l1", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_property_is_greater_than_or_equal_stepLabel_value()
        {
            _g
                .V<Person>()
                .As((__, person1) => __
                    .V<Person>()
                    .Where(person2 => person2.Age >= person1.Value.Age))
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).as(_b).V().hasLabel(_a).where(gte(_b)).by(_c).project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.properties().group().by(__.label()).by(__.project(_d, _e, _i, _g).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "l1", "Age", "id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).values(_b).as(_c).V().hasLabel(_a).has(_b, __.where(gt(_c))).project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.properties().group().by(__.label()).by(__.project(_d, _e, _i, _g).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "Age", "l1", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_property_is_lower_or_equal_than_constant()
        {
            _g
                .V<Person>()
                .Where(t => t.Age <= 36)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(_b, lte(_c)).project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.properties().group().by(__.label()).by(__.project(_d, _e, _i, _g).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "Age", 36, "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_property_is_lower_than_constant()
        {
            _g
                .V<Person>()
                .Where(t => t.Age < 36)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(_b, lt(_c)).project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.properties().group().by(__.label()).by(__.project(_d, _e, _i, _g).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "Age", 36, "id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).values(_b).as(_c).V().hasLabel(_a).has(_b, __.where(lte(_c))).project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.properties().group().by(__.label()).by(__.project(_d, _e, _i, _g).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "Age", "l1", "id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).values(_b).as(_c).V().hasLabel(_a).has(_b, __.where(lt(_c))).project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.properties().group().by(__.label()).by(__.project(_d, _e, _i, _g).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "Age", "l1", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_property_is_not_contained_in_array()
        {
            _g
                .V<Person>()
                .Where(t => !new[] { 36, 37, 38 }.Contains(t.Age))
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).not(__.has(_b, within(_c, _d, _e))).project(_f, _g, _h, _i).by(id).by(label).by(__.constant(_j)).by(__.properties().group().by(__.label()).by(__.project(_f, _g, _k, _i).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "Age", 36, 37, 38, "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_property_is_not_contained_in_empty_enumerable()
        {
            var enumerable = Enumerable.Empty<int>();

            _g
                .V<Person>()
                .Where(t => !enumerable.Contains(t.Age))
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_b, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_property_is_not_contained_in_enumerable()
        {
            var enumerable = new[] { "36", "37", "38" }
                .Select(int.Parse);

            _g
                .V<Person>()
                .Where(t => !enumerable.Contains(t.Age))
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).not(__.has(_b, within(_c, _d, _e))).project(_f, _g, _h, _i).by(id).by(label).by(__.constant(_j)).by(__.properties().group().by(__.label()).by(__.project(_f, _g, _k, _i).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "Age", 36, 37, 38, "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_property_is_not_present()
        {
            _g
                .V<Person>()
                .Where(t => t.Name == null)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).hasNot(_b).project(_c, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_c, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "Name", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_property_is_prefix_of_constant()
        {
            _g
                .V<Country>()
                .Where(c => "+49123".StartsWith(c.CountryCallingCode))
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(_b, within(_c, _d, _e, _f, _g, _h, _i)).project(_j, _k, _l, _m).by(id).by(label).by(__.constant(_n)).by(__.properties().group().by(__.label()).by(__.project(_j, _k, _o, _m).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Country", "CountryCallingCode", "", "+", "+4", "+49", "+491", "+4912", "+49123", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_property_is_prefix_of_empty_string()
        {
            _g
                .V<Country>()
                .Where(c => "".StartsWith(c.CountryCallingCode))
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(_b, within(_c)).project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.properties().group().by(__.label()).by(__.project(_d, _e, _i, _g).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Country", "CountryCallingCode", "", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_property_is_prefix_of_expression()
        {
            const string str = "+49123xxx";

            _g
                .V<Country>()
                .Where(c => str.Substring(0, 6).StartsWith(c.CountryCallingCode))
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(_b, within(_c, _d, _e, _f, _g, _h, _i)).project(_j, _k, _l, _m).by(id).by(label).by(__.constant(_n)).by(__.properties().group().by(__.label()).by(__.project(_j, _k, _o, _m).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Country", "CountryCallingCode", "", "+", "+4", "+49", "+491", "+4912", "+49123", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_property_is_prefix_of_variable()
        {
            const string str = "+49123";

            _g
                .V<Country>()
                .Where(c => str.StartsWith(c.CountryCallingCode))
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(_b, within(_c, _d, _e, _f, _g, _h, _i)).project(_j, _k, _l, _m).by(id).by(label).by(__.constant(_n)).by(__.properties().group().by(__.label()).by(__.project(_j, _k, _o, _m).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Country", "CountryCallingCode", "", "+", "+4", "+49", "+491", "+4912", "+49123", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_property_is_present()
        {
            _g
                .V<Person>()
                .Where(t => t.Name != null)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(_b).project(_c, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_c, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "Name", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_property_not_equals_constant()
        {
            _g
                .V<Person>()
                .Where(t => t.Age != 36)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(_b, neq(_c)).project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.properties().group().by(__.label()).by(__.project(_d, _e, _i, _g).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "Age", 36, "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_property_starts_with_constant_with_TextP_support()
        {
            _g
                .V<Country>()
                .Where(c => c.CountryCallingCode.StartsWith("+49123"))
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(_b, startingWith(_c)).project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.properties().group().by(__.label()).by(__.project(_d, _e, _i, _g).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Country", "CountryCallingCode", "+49123", "id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(_b, between(_c, _d)).project(_e, _f, _g, _h).by(id).by(label).by(__.constant(_i)).by(__.properties().group().by(__.label()).by(__.project(_e, _f, _j, _h).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Country", "CountryCallingCode", "+49123", "+49124", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_property_starts_with_empty_string_with_TextP_support()
        {
            _g
                .V<Country>()
                .Where(c => c.CountryCallingCode.StartsWith(""))
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(_b).project(_c, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_c, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Country", "CountryCallingCode", "id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(_b).project(_c, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_c, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Country", "CountryCallingCode", "id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(_b, __.inject(_c)).project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.properties().group().by(__.label()).by(__.project(_d, _e, _i, _g).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "Age", 36, "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_scalar_element_equals_constant()
        {
            _g
                .V<Person>()
                .Values(x => x.Age)
                .Where(_ => _ == 36)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).values(_b).is(_c)")
                .WithParameters("Person", "Age", 36);
        }

        [Fact]
        public void Where_sequential()
        {
            _g
                .V<Person>()
                .Where(t => t.Age == 36)
                .Where(t => t.Age == 42)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(_b, eq(_c).and(eq(_d))).project(_e, _f, _g, _h).by(id).by(label).by(__.constant(_i)).by(__.properties().group().by(__.label()).by(__.project(_e, _f, _j, _h).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "Age", 36, 42, "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_source_expression_on_both_sides1()
        {
            _g
                .V<Country>()
                .Where(t => t.Name.Value == t.CountryCallingCode)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).as(_b).where(eq(_b)).by(_c).by(_d).project(_e, _f, _g, _h).by(id).by(label).by(__.constant(_i)).by(__.properties().group().by(__.label()).by(__.project(_e, _f, _j, _h).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Country", "l1", "Name", "CountryCallingCode", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_source_expression_on_both_sides2()
        {
            _g
                .V<EntityWithTwoIntProperties>()
                .Where(x => x.IntProperty1 > x.IntProperty2)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).as(_b).where(gt(_b)).by(_c).by(_d).project(_e, _f, _g, _h).by(id).by(label).by(__.constant(_i)).by(__.properties().group().by(__.label()).by(__.project(_e, _f, _j, _h).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("EntityWithTwoIntProperties", "l1", "IntProperty1", "IntProperty2", "id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).as(_b).values(_c).as(_d).select(_b).where(__.out(_e).hasLabel(_a).as(_f).values(_c).as(_g).select(_f).where(_d, lt(_g))).project(_h, _i, _j, _k).by(id).by(label).by(__.constant(_l)).by(__.properties().group().by(__.label()).by(__.project(_h, _i, _m, _k).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "l1", "Age", "l2", "WorksFor", "l3", "l4", "id", "label", "type", "properties", "vertex", "value");
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
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).as(_b).V().hasLabel(_a).as(_c).where(_b, gte(_c)).by(_d).by(_d).project(_e, _f, _g, _h).by(id).by(label).by(__.constant(_i)).by(__.properties().group().by(__.label()).by(__.project(_e, _f, _j, _h).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "l1", "l2", "Age", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_traversal()
        {
            _g
                .V<Person>()
                .Where(_ => _.Out<LivesIn>())
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).where(__.out(_b)).project(_c, _d, _e, _f).by(id).by(label).by(__.constant(_g)).by(__.properties().group().by(__.label()).by(__.project(_c, _d, _h, _f).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "LivesIn", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_true()
        {
            _g
                .V<Person>()
                .Where(_ => true)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).project(_b, _c, _d, _e).by(id).by(label).by(__.constant(_f)).by(__.properties().group().by(__.label()).by(__.project(_b, _c, _g, _e).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_VertexProperty_Value1()
        {
            _g
                .V<Person>()
                .Where(x => x.Name.Value == "SomeName")
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(_b, _c).project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.properties().group().by(__.label()).by(__.project(_d, _e, _i, _g).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "Name", "SomeName", "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void Where_VertexProperty_Value2()
        {
            _g
                .V<Person>()
                .Where(x => ((int)(object)x.Name.Value) > 36)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(_b, gt(_c)).project(_d, _e, _f, _g).by(id).by(label).by(__.constant(_h)).by(__.properties().group().by(__.label()).by(__.project(_d, _e, _i, _g).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("Person", "Name", 36, "id", "label", "type", "properties", "vertex", "value");
        }

        [Fact(Skip="Feature!")]
        public void Where_VertexProperty_Value3()
        {
            _g
                .V<Person>()
                .Where(x => (int)x.Name.Id == 36)
                .Should()
                .SerializeToGroovy("V().hasLabel(_a).has(_b, gt(_c))")
                .WithParameters("Person", "Name", 36);
        }

        [Fact]
        public void WithoutStrategies1()
        {
            _g
                .RemoveStrategies(typeof(SubgraphStrategy))
                .V()
                .Should()
                .SerializeToGroovy("withoutStrategies(SubgraphStrategy).V().project(_a, _b, _c, _d).by(id).by(label).by(__.constant(_e)).by(__.properties().group().by(__.label()).by(__.project(_a, _b, _f, _d).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("id", "label", "type", "properties", "vertex", "value");
        }

        [Fact]
        public void WithoutStrategies2()
        {
            _g
                .RemoveStrategies(typeof(SubgraphStrategy), typeof(ElementIdStrategy))
                .V()
                .Should()
                .SerializeToGroovy("withoutStrategies(SubgraphStrategy, ElementIdStrategy).V().project(_a, _b, _c, _d).by(id).by(label).by(__.constant(_e)).by(__.properties().group().by(__.label()).by(__.project(_a, _b, _f, _d).by(id).by(__.label()).by(__.value()).by(__.valueMap()).fold()))")
                .WithParameters("id", "label", "type", "properties", "vertex", "value");
        }

        [Fact(Skip = "Can't handle currently!")]
        public void WithSubgraphStrategy()
        {
            _g
                .AddStrategies(new SubgraphQueryStrategy(_ => _.OfType<Person>(), _ => _.OfType<WorksFor>()))
                .V()
                .Should()
                .SerializeToGroovy("withStrategies(SubgraphStrategy.build().vertices(__.hasLabel(_a)).edges(__.hasLabel(_b)).create()).V()")
                .WithParameters("Person", "WorksFor");
        }

        [Fact(Skip = "Can't handle currently!")]
        public void WithSubgraphStrategy_empty()
        {
            _g
                .AddStrategies(new SubgraphQueryStrategy(_ => _, _ => _))
                .V()
                .Should()
                .SerializeToGroovy("V()")
                .WithoutParameters();
        }

        [Fact(Skip = "Can't handle currently!")]
        public void WithSubgraphStrategy_only_edges()
        {
            _g
                .AddStrategies(new SubgraphQueryStrategy(_ => _, _ => _.OfType<WorksFor>()))
                .V()
                .Should()
                .SerializeToGroovy("withStrategies(SubgraphStrategy.build().edges(__.hasLabel(_a)).create()).V()")
                .WithParameters("WorksFor");
        }

        [Fact(Skip = "Can't handle currently!")]
        public void WithSubgraphStrategy_only_vertices()
        {
            _g
                .AddStrategies(new SubgraphQueryStrategy(_ => _.OfType<Person>(), _ => _))
                .V()
                .Should()
                .SerializeToGroovy("withStrategies(SubgraphStrategy.build().vertices(__.hasLabel(_a)).create()).V()")
                .WithParameters("Person");
        }
    }
}
