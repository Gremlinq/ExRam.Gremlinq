using System;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Tests;
using ExRam.Gremlinq.Tests.Entities;
using Xunit;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Providers.Neptune.Tests
{
    public class NeptuneGroovySerializationTest
    {
        private readonly IGremlinQuerySource _g;

        public NeptuneGroovySerializationTest() : base()
        {
            _g = g
                .ConfigureEnvironment(env => env
                    .UseNeptune(new Uri("ws://localhost:8182"))
                    .ConfigureSerializer(s => s
                        .ToGroovy()));
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
        public void AddV_with_multi_property()
        {
            _g
                .AddV(new Company { Id = 1, PhoneNumbers = new[] { "+4912345", "+4923456" } })
                .Should()
                .SerializeToGroovy("addV(_a).property(single, _b, _c).property(id, _d).property(set, _e, _f).property(set, _e, _g).project(_h, _i, _j, _k).by(id).by(label).by(__.constant(_l)).by(__.properties().group().by(__.label()).by(__.project(_h, _i, _m).by(id).by(__.label()).by(__.value()).fold()))")
                .WithParameters("Company", "FoundingDate", DateTime.MinValue, 1, "PhoneNumbers", "+4912345", "+4923456", "id", "label", "type", "properties", "vertex", "value");
        }
    }
}
