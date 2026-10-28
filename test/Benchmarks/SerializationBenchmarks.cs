using BenchmarkDotNet.Attributes;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Core.Serialization;
using ExRam.Gremlinq.Tests.Entities;

using Gremlin.Net.Driver.Messages;
using Gremlin.Net.Process.Traversal;

using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Benchmarks
{
    // Serialization of a built query to Bytecode, RequestMessage and Groovy. Every benchmark but the
    // last serializes the same query instance again and again, as a cached query is.
    [MemoryDiagnoser]
    public class SerializationBenchmarks
    {
        private IGremlinQuerySource _g = null!;
        private IGremlinQueryBase _v = null!;
        private IGremlinQueryBase _vTyped = null!;
        private IGremlinQueryBase _whereOne = null!;
        private IGremlinQueryBase _whereTwo = null!;
        private IGremlinQueryBase _addV = null!;
        private IGremlinQueryBase _project = null!;
        private IGremlinQueryBase _subTraversal = null!;
        private IGremlinQueryEnvironment _environment = null!;

        [GlobalSetup]
        public void Setup()
        {
            _g = g
                .ConfigureEnvironment(env => env
                    .UseModel(GraphModel.FromBaseTypes<Vertex, Edge>()));

            _environment = _g.AsAdmin().Environment;

            _v = _g.V();
            _vTyped = _g.V<RichVertex>();
            _whereOne = _g.V<RichVertex>().Where(x => x.Age == 25);
            _whereTwo = _g.V<RichVertex>().Where(x => x.Age == 25 && x.Name!.Value == "Alice");
            _addV = _g.AddV(new RichVertex { Age = 30, RegistrationDate = DateTimeOffset.UtcNow, Enum = SomeEnum.Zero });
            _project = _g.V<RichVertex>()
                .Project(__ => __.ToTuple()
                    .By(__ => __.Id())
                    .By(__ => __.Label())
                    .By(__ => __.Values(x => x.Age)));
            _subTraversal = _g.V<RichVertex>().Where(__ => __.Out<RichEdge>());
        }

        [Benchmark]
        public Bytecode V_to_bytecode() => Serialize<Bytecode>(_v);

        [Benchmark]
        public Bytecode V_typed_to_bytecode() => Serialize<Bytecode>(_vTyped);

        [Benchmark]
        public Bytecode Where_one_to_bytecode() => Serialize<Bytecode>(_whereOne);

        [Benchmark]
        public Bytecode Where_two_to_bytecode() => Serialize<Bytecode>(_whereTwo);

        [Benchmark]
        public Bytecode AddV_to_bytecode() => Serialize<Bytecode>(_addV);

        [Benchmark]
        public Bytecode Project_to_bytecode() => Serialize<Bytecode>(_project);

        [Benchmark]
        public Bytecode Sub_traversal_to_bytecode() => Serialize<Bytecode>(_subTraversal);

        [Benchmark]
        public RequestMessage Where_one_to_request_message() => Serialize<RequestMessage>(_whereOne);

        [Benchmark]
        public GroovyGremlinScript Where_one_to_groovy() => Serialize<GroovyGremlinScript>(_whereOne);

        // Builds the query first, so that its first serialization is measured along with the build.
        [Benchmark]
        public Bytecode Build_and_serialize_where_one_to_bytecode() => Serialize<Bytecode>(_g.V<RichVertex>().Where(x => x.Age == 25));

        private TTarget Serialize<TTarget>(IGremlinQueryBase query) => _environment.Serializer
            .TransformTo<TTarget>()
            .From(query, _environment);
    }
}
