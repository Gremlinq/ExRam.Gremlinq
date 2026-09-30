using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Tests.Entities;
using ExRam.Gremlinq.Tests.Infrastructure;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public class Issue2464 : GremlinqTestBase
    {
        private readonly IGremlinQuerySource _g;

        public Issue2464() : base(new DebugGremlinQueryVerifier())
        {
            _g = g
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel
                        .FromBaseTypes<Vertex, Edge>()));
        }

        [Fact]
        public Task Repro() => _g
            .V<Person>()
            .Project(__ => __
                .ToTuple()
                .By(__ => __
                    .Map(__ => __
                        .Values(x => x.Name!)
                        .Limit(1)))
                .By(__ => __
                    .Map(__ => __
                        .Id())))
            .Verify();

        [Fact]
        public Task Map_Out_Count() => _g
            .V<Person>()
            .Project(__ => __
                .ToTuple()
                .By(__ => __
                    .Map(__ => __
                        .Out<WorksFor>()
                        .Count()))
                .By(__ => __
                    .Id()))
            .Verify();

        [Fact]
        public Task Map_Map_Out_Count() => _g
            .V<Person>()
            .Project(__ => __
                .ToTuple()
                .By(__ => __
                    .Map(__ => __
                        .Map(__ => __
                            .Out<WorksFor>()
                            .Count())))
                .By(__ => __
                    .Id()))
            .Verify();

        [Fact]
        public Task Map_Values() => _g
            .V<Person>()
            .Project(__ => __
                .ToTuple()
                .By(__ => __
                    .Map(__ => __
                        .Values(x => x.Name!)))
                .By(__ => __
                    .Id()))
            .Verify();

        [Fact]
        public Task Map_Out() => _g
            .V<Person>()
            .Project(__ => __
                .ToTuple()
                .By(__ => __
                    .Map(__ => __
                        .Out<WorksFor>()))
                .By(__ => __
                    .Id()))
            .Verify();

        [Fact]
        public Task Local_Out_Count() => _g
            .V<Person>()
            .Project(__ => __
                .ToTuple()
                .By(__ => __
                    .Local(__ => __
                        .Out<WorksFor>()
                        .Count()))
                .By(__ => __
                    .Id()))
            .Verify();

        [Fact]
        public Task Local_Out_Count_with_empty_projection_value_protection() => _g
            .ConfigureEnvironment(env => env
                .ConfigureOptions(options => options
                    .SetValue(GremlinqOption.EnableEmptyProjectionValueProtection, true)))
            .V<Person>()
            .Project(__ => __
                .ToTuple()
                .By(__ => __
                    .Local(__ => __
                        .Out<WorksFor>()
                        .Count()))
                .By(__ => __
                    .Id()))
            .Verify();

        [Fact]
        public Task Local_Values() => _g
            .V<Person>()
            .Project(__ => __
                .ToTuple()
                .By(__ => __
                    .Local(__ => __
                        .Values(x => x.Name!)))
                .By(__ => __
                    .Id()))
            .Verify();

        [Fact]
        public Task Map_Local_Values() => _g
            .V<Person>()
            .Project(__ => __
                .ToTuple()
                .By(__ => __
                    .Map(__ => __
                        .Local(__ => __
                            .Values(x => x.Name!))))
                .By(__ => __
                    .Id()))
            .Verify();

        [Fact]
        public Task Local_Map_Out_Count() => _g
            .V<Person>()
            .Project(__ => __
                .ToTuple()
                .By(__ => __
                    .Local(__ => __
                        .Map(__ => __
                            .Out<WorksFor>()
                            .Count())))
                .By(__ => __
                    .Id()))
            .Verify();

        [Fact]
        public Task Map_Out_Count_followed_by_step() => _g
            .V<Person>()
            .Project(__ => __
                .ToTuple()
                .By(__ => __
                    .Map(__ => __
                        .Out<WorksFor>()
                        .Count())
                    .Fold())
                .By(__ => __
                    .Id()))
            .Verify();

        [Fact]
        public Task Map_Out_Count_with_empty_projection_value_protection() => _g
            .ConfigureEnvironment(env => env
                .ConfigureOptions(options => options
                    .SetValue(GremlinqOption.EnableEmptyProjectionValueProtection, true)))
            .V<Person>()
            .Project(__ => __
                .ToTuple()
                .By(__ => __
                    .Map(__ => __
                        .Out<WorksFor>()
                        .Count()))
                .By(__ => __
                    .Id()))
            .Verify();

        [Fact]
        public Task Map_Out_Count_by_name() => _g
            .V<Person>()
            .Project(__ => __
                .ToDynamic()
                .By("count", __ => __
                    .Map(__ => __
                        .Out<WorksFor>()
                        .Count()))
                .By("id", __ => __
                    .Id()))
            .Verify();
    }
}
