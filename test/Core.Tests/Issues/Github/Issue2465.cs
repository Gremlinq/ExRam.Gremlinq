using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Tests.Entities;
using ExRam.Gremlinq.Tests.Infrastructure;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Core.Tests
{
    public class Issue2465 : GremlinqTestBase
    {
        private readonly IGremlinQuerySource _g;

        public Issue2465() : base(new DebugGremlinQueryVerifier())
        {
            _g = g
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel
                        .FromBaseTypes<Vertex, Edge>()));
        }

        [Fact]
        public Task Map_Values_Limit() => _g
            .V<Person>()
            .Map(__ => __
                .Values(x => x.Name!)
                .Limit(1))
            .Verify();

        [Fact]
        public Task Map_Out_Id_Limit_Limit() => _g
            .V<Person>()
            .Map(__ => __
                .Out<WorksFor>()
                .Id()
                .Limit(1)
                .Limit(1))
            .Verify();

        [Fact]
        public Task Map_Out_Id_Limit_Fold() => _g
            .V<Person>()
            .Map(__ => __
                .Out<WorksFor>()
                .Id()
                .Limit(1)
                .Fold())
            .Verify();

        [Fact]
        public Task Map_Out_Id_Limit_of_two() => _g
            .V<Person>()
            .Map(__ => __
                .Out<WorksFor>()
                .Id()
                .Limit(2))
            .Verify();

        [Fact]
        public Task Map_Limit() => _g
            .V<Person>()
            .Id()
            .Map(__ => __
                .Limit(1))
            .Verify();

        [Fact]
        public Task Local_Out_Id_Limit() => _g
            .V<Person>()
            .Local(__ => __
                .Out<WorksFor>()
                .Id()
                .Limit(1))
            .Verify();

        [Fact]
        public Task FlatMap_Out_Id_Limit() => _g
            .V<Person>()
            .FlatMap(__ => __
                .Out<WorksFor>()
                .Id()
                .Limit(1))
            .Verify();

        [Fact]
        public Task By_Out_Id_Limit() => _g
            .V<Person>()
            .Project(__ => __
                .ToTuple()
                .By(__ => __
                    .Out<WorksFor>()
                    .Id()
                    .Limit(1))
                .By(__ => __
                    .Id()))
            .Verify();

        [Fact]
        public Task By_Out_Id_Limit_Limit() => _g
            .V<Person>()
            .Project(__ => __
                .ToTuple()
                .By(__ => __
                    .Out<WorksFor>()
                    .Id()
                    .Limit(1)
                    .Limit(1))
                .By(__ => __
                    .Id()))
            .Verify();

        [Fact]
        public Task By_Values_Limit() => _g
            .V<Person>()
            .Project(__ => __
                .ToTuple()
                .By(__ => __
                    .Values(x => x.Name!)
                    .Limit(1))
                .By(__ => __
                    .Id()))
            .Verify();

        [Fact]
        public Task By_Map_Values_Limit() => _g
            .V<Person>()
            .Project(__ => __
                .ToTuple()
                .By(__ => __
                    .Map(__ => __
                        .Values(x => x.Name!)
                        .Limit(1)))
                .By(__ => __
                    .Id()))
            .Verify();

        [Fact]
        public Task By_Local_Out_Id_Limit() => _g
            .V<Person>()
            .Project(__ => __
                .ToTuple()
                .By(__ => __
                    .Local(__ => __
                        .Out<WorksFor>()
                        .Id()
                        .Limit(1)))
                .By(__ => __
                    .Id()))
            .Verify();

        [Fact]
        public Task By_Map_Out_Id_followed_by_Limit() => _g
            .V<Person>()
            .Project(__ => __
                .ToTuple()
                .By(__ => __
                    .Map(__ => __
                        .Out<WorksFor>()
                        .Id())
                    .Limit(1))
                .By(__ => __
                    .Id()))
            .Verify();

        [Fact]
        public Task By_Out_Id_Limit_Fold() => _g
            .V<Person>()
            .Project(__ => __
                .ToTuple()
                .By(__ => __
                    .Out<WorksFor>()
                    .Id()
                    .Limit(1)
                    .Fold())
                .By(__ => __
                    .Id()))
            .Verify();

        [Fact]
        public Task By_Out_Id_Limit_of_two() => _g
            .V<Person>()
            .Project(__ => __
                .ToTuple()
                .By(__ => __
                    .Out<WorksFor>()
                    .Id()
                    .Limit(2))
                .By(__ => __
                    .Id()))
            .Verify();

        [Fact]
        public Task By_Limit() => _g
            .V<Person>()
            .Id()
            .Project(__ => __
                .ToTuple()
                .By(__ => __
                    .Limit(1))
                .By(__ => __
                    .Constant(1)))
            .Verify();

        [Fact]
        public Task By_Out_Id_Limit_with_empty_projection_value_protection() => _g
            .ConfigureEnvironment(env => env
                .ConfigureOptions(options => options
                    .SetValue(GremlinqOption.EnableEmptyProjectionValueProtection, true)))
            .V<Person>()
            .Project(__ => __
                .ToTuple()
                .By(__ => __
                    .Out<WorksFor>()
                    .Id()
                    .Limit(1))
                .By(__ => __
                    .Id()))
            .Verify();
    }
}
