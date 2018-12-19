using System.Linq;
using System.Threading.Tasks;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Providers;
using ExRam.Gremlinq.Providers.WebSocket;
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Samples
{
    class Program
    {
        private readonly IConfigurableGremlinQuerySource _g;

        public Program()
        {
            //Configure Gremlinq to work on a locally running instance of Gremlin server.
            _g = g
                //Since the Vertex and Edge classes contained in this sample implement IVertex resp. IEdge,
                //setting a model is actually not required as long as these classes are discoverable (i.e. they reside
                //in a currently loaded assembly). We explicitly set a model here anyway.
                .WithModel(GraphModel.FromBaseTypes<Vertex, Edge>(x => x.Id, x => x.Id))
                .WithRemote("localhost", GraphsonVersion.V3);

            //Uncomment to configure Gremlinq to work on CosmosDB!
            //_g = g
            //    .WithCosmosDbRemote(hostname, database, graphName, authKey);
        }

        public async Task CreateGraph()
        {
            var marko = await _g
                .AddV(new Person { Name = "marko", Age = 29 })
                .First();

            var vadas = await _g
                .AddV(new Person { Name = "vadas", Age = 27 })
                .First();
            
            var josh = await _g
                .AddV(new Person { Name = "josh", Age = 32 })
                .First();

            var peter = await _g
                .AddV(new Person { Name = "peter", Age = 29 })
                .First();

            var lop = await _g
                .AddV(new Software { Name = "lop", Language = ProgrammingLanguage.Java })
                .First();

            var ripple = await _g
                .AddV(new Software { Name = "ripple", Language = ProgrammingLanguage.Java })
                .First();

            await _g
                .V(marko.Id)
                .AddE<Knows>()
                .To(__ => __.V(vadas.Id))
                .First();

            await _g
                .V(marko.Id)
                .AddE<Knows>()
                .To(__ => __.V(josh.Id))
                .First();

            await _g
                .V(marko.Id)
                .AddE<Created>()
                .To(__ => __.V(lop.Id))
                .First();

            await _g
                .V(josh.Id)
                .AddE<Created>()
                .To(__ => __.V(ripple.Id))
                .First();

            await _g
                .V(josh.Id)
                .AddE<Created>()
                .To(__ => __.V(lop.Id))
                .First();

            await _g
                .V(peter.Id)
                .AddE<Created>()
                .To(__ => __.V(lop.Id))
                .First();
        }

        public async Task CreateKnowsRelationInOneQuery()
        {
            await _g
                .AddV(new Person { Name = "bob", Age = 36 })
                .AddE<Knows>()
                .To(__ => __
                    .AddV(new Person { Name = "jeff", Age = 27 }))
                .First();
        }

        public async Task WhoDoesMarkoKnow()
        {
            var knownToMarko = await _g
                .V<Person>()
                .Where(x => x.Name == "marko")
                .Out<Knows>()
                .OfType<Person>()
                .Values(x => x.Name)
                .ToArray();

            var knownToMarkoSorted = await _g
                .V<Person>()
                .Where(x => x.Name == "marko")
                .Out<Knows>()
                .OfType<Person>()
                .OrderBy(x => x.Name)
                .Values(x => x.Name)
                .ToArray();
        }

        static async Task Main(string[] args)
        {
            var program = new Program();

            await program.CreateGraph();
            await program.CreateKnowsRelationInOneQuery();
            await program.WhoDoesMarkoKnow();
        }
    }
}
