// ReSharper disable ConsiderUsingConfigureAwait
using System;
using System.Threading.Tasks;
using ExRam.Gremlinq.Core;
// Put this into static scope to access the default GremlinQuerySource as "g". 
using static ExRam.Gremlinq.Core.GremlinQuerySource;

namespace ExRam.Gremlinq.Samples
{
    public class Program
    {
        private Person _marko;
        private readonly IGremlinQuerySource _g;

        public Program()
        {
            _g = g
                .ConfigureEnvironment(env => env
                    //Since the Vertex and Edge classes contained in this sample implement IVertex resp. IEdge,
                    //setting a model is actually not required as long as these classes are discoverable (i.e. they reside
                    //in a currently loaded assembly). We explicitly set a model here anyway.
                    .UseModel(GraphModel.FromBaseTypes<Vertex, Edge>()))

                //Configure Gremlinq to work on a locally running instance of Gremlin server.
                .UseGremlinServer(new Uri("ws://localhost:8182"), GraphsonVersion.V3);

                //Uncomment below, comment above and enter appropriate data to configure Gremlinq to work on CosmosDB!
                //.UseCosmosDb(uri, database, graphName, authKey);
        }

        public async Task Run()
        {
            await Create_the_graph();
            await Create_vertices_and_a_relation_in_one_query();

            await Who_does_Marko_know();
            await Who_is_older_than_30();
            await Whose_name_starts_with_B();
            await Who_knows_who();
            await What_pets_are_around();
            await How_many_pets_does_everybody_have();

            await Set_and_get_metadata_on_Marko();

            Console.Write("Press any key...");
            Console.Read();
        }

        private async Task Create_the_graph()
        {
            // Create a graph very similar to the one
            // found at http://tinkerpop.apache.org/docs/current/reference/#graph-computing.

            // Uncomment to delete the whole graph on every run.
            await _g.V().Drop().ToArrayAsync();

            _marko = await _g
                .AddV(new Person { Name = "Marko", Age = 29 })
                .FirstAsync();

            var vadas = await _g
                .AddV(new Person { Name = "Vadas", Age = 27 })
                .FirstAsync();
            
            var josh = await _g
                .AddV(new Person { Name = "Josh", Age = 32 })
                .FirstAsync();

            var peter = await _g
                .AddV(new Person { Name = "Peter", Age = 35 })
                .FirstAsync();

            var daniel = await _g
                .AddV(new Person { Name = "Daniel", Age = 37 })
                .FirstAsync();

            var charlie = await _g
                .AddV(new Dog {Name = "Charlie", Age = 2})
                .FirstAsync();

            var catmanJohn = await _g
                .AddV(new Cat { Name = "Catman John", Age = 5 })
                .FirstAsync();

            var luna = await _g
                .AddV(new Cat {Name = "Luna", Age = 9})
                .FirstAsync();

            var lop = await _g
                .AddV(new Software { Name = "Lop", Language = ProgrammingLanguage.Java })
                .FirstAsync();

            var ripple = await _g
                .AddV(new Software { Name = "Ripple", Language = ProgrammingLanguage.Java })
                .FirstAsync();

            await _g
                .V(_marko.Id)
                .AddE<Knows>()
                .To(__ => __
                    .V(vadas.Id))
                .FirstAsync();

            await _g
                .V(_marko.Id)
                .AddE<Knows>()
                .To(__ => __
                    .V(josh.Id))
                .FirstAsync();

            await _g
                .V(_marko.Id)
                .AddE<Created>()
                .To(__ => __
                    .V(lop.Id))
                .FirstAsync();

            await _g
                .V(josh.Id)
                .AddE<Created>()
                .To(__ => __
                    .V(ripple.Id))
                .FirstAsync();

            await _g
                .V(josh.Id)
                .AddE<Created>()
                .To(__ => __
                    .V(lop.Id))
                .FirstAsync();

            await _g
                .V(peter.Id)
                .AddE<Created>()
                .To(__ => __
                    .V(lop.Id))
                .FirstAsync();

            await _g
                .V(josh.Id)
                .AddE<Owns>()
                .To(__ => __
                    .V(charlie.Id))
                .FirstAsync();

            await _g
                .V(josh.Id)
                .AddE<Owns>()
                .To(__ => __
                    .V(luna.Id))
                .FirstAsync();

            await _g
                .V(daniel.Id)
                .AddE<Owns>()
                .To(__ => __
                    .V(catmanJohn.Id))
                .FirstAsync();
        }

        private async Task Create_vertices_and_a_relation_in_one_query()
        {
            // This demonstrates how to create 2 vertices and a connecting
            // edge between them in a single query.

            await _g
                .AddV(new Person { Name = "Bob", Age = 36 })
                .AddE<Knows>()
                .To(__ => __
                    .AddV(new Person { Name = "Jeff", Age = 27 }))
                .FirstAsync();
        }

        private async Task Who_does_Marko_know()
        {
            // From Marko, walk all the 'Knows' edge to all the persons
            // that he knows and order them by their name.
            var knownPersonsToMarko = await _g
                .V(_marko.Id)
                .Out<Knows>()
                .OfType<Person>()
                .Order(x => x
                    .By(x => x.Name))
                .Values(x => x.Name)
                .ToArrayAsync();

            Console.WriteLine("Who does Marko know?");

            foreach (var person in knownPersonsToMarko)
            {
                Console.WriteLine($" Marko knows {person}.");
            }

            Console.WriteLine();
        }

        private async Task Who_is_older_than_30()
        {
            // Gremlinq supports boolean expressions like you're used to use them
            // in your Linq-queries. Under the hood, they will be translated to the
            // corresponding Gremlin-expressions, like
            //
            //   "g.hasLabel('Person').has('Age', gt(30))"
            //
            // in this case

            // Also, this sample demonstrates that instead of calling ToArrayAsync
            // and awaiting that, you may just await the IGremlinQuery<Person>!

            var personsOlderThan30 = await _g
                .V<Person>()
                .Where(x => x.Age > 30);

            Console.WriteLine("Who is older than 30?");

            foreach (var person in personsOlderThan30)
            {
                Console.WriteLine($" {person.Name.Value} is older than 30.");
            }

            Console.WriteLine();
        }

        private async Task Whose_name_starts_with_B()
        {
            // This sample demonstrates the power of ExRam.Gremlinq! Even an expression
            // like 'StartsWith' on a string will be recognized by ExRam.Gremlinq and translated
            // to proper Gremlin syntax!

            var nameStartsWithB = await _g
                .V<Person>()
                .Where(x => x.Name.Value.StartsWith("B"))
                .ToArrayAsync();

            Console.WriteLine("Whose name starts with 'B'?");

            foreach (var person in nameStartsWithB)
            {
                Console.WriteLine($" {person.Name.Value}'s name starts with a 'B'.");
            }

            Console.WriteLine();
        }

        private async Task Who_knows_who()
        {
            // Here, we demonstrate how to deal with Gremlin step labels. Instead of
            // dealing with raw strings, ExRam.Gremlinq uses a dedicated 'StepLabel'-type
            // for these. And you don't even need to declare them upfront, as the
            // As-operator of ExRam.Gremlinq will put them in scope for you, along
            // with a continuation-query that you can further build upon!

            // Also, ExRam.Gremlinq's Select operators will not leave you with
            // raw dictionaries (or maps, as Java calls them). Instead, you'll get
            // nice ValueTuples!

            var friendTuples = await _g
                .V<Person>()
                .As((__, person) => __
                    .Out<Knows>()
                    .OfType<Person>()
                    .As((___, friend) => ___
                        .Select(person, friend)));

            Console.WriteLine("Who knows who?");

            foreach (var (person1, person2) in friendTuples)
            {
                Console.WriteLine($" {person1.Name.Value} knows {person2.Name.Value}.");
            }

            Console.WriteLine();
        }

        private async Task Who_does_what()
        {
            // So far, we have only been dealing with vertices. ExRam.Gremlinq is cool
            // with edges too!

            var tuples = await _g
                .V<Person>()
                .As((__, person) => __
                    .OutE<Edge>()
                    .As((__, edge) => __
                        .InV<Vertex>()
                        .As((__, what) => __
                            .Select(person, edge, what))));

            Console.WriteLine("Who does what?");

            foreach (var (person, does, what) in tuples)
            {
                Console.WriteLine($" {person} {does.Label} {what}.");
            }

            Console.WriteLine();
        }

        private async Task What_pets_are_around()
        {
            // ExRam.Gremlinq supports inheritance! Below query will find all the dogs
            // and all the cats and instantiate the right type, as 'pet.GetType()' proves.

            var pets = await _g
                .V<Pet>();

            Console.WriteLine("What pets are around?");

            foreach (var pet in pets)
            {
                Console.WriteLine($" There's a {pet.GetType().Name} named {pet.Name.Value}.");
            }

            Console.WriteLine();
        }

        private async Task How_many_pets_does_everybody_have()
        {
            // This sample demonstrates how to fluently build projections with
            // ExRam.Gremlinq. It can project to a ValueTuple or to a dynamic.
            // In the latter case, the user may specify the name of each projection.

            var dynamics = await _g
                .V<Person>()
                .Project(b => b
                    .ToDynamic()
                    .By(person => person.Name)
                    .By(
                        "count",
                        __ => __
                            .Out<Owns>()
                            .OfType<Pet>()
                            .Count()));

            foreach (var d in dynamics)
            {
                Console.WriteLine($"{d.Name} owns {d.count} pets.");
            }

            Console.WriteLine();
        }

        private async Task Set_and_get_metadata_on_Marko()
        {
            await _g
                .V<Person>(_marko.Id)
                .Properties(x => x.Name)
                .Property(x => x.Creator, "Stephen")
                .Property(x => x.Date, DateTimeOffset.Now)
                .ToArrayAsync();

            var metaProperties = await _g
                .V()
                .Properties()
                .Properties()
                .ToArrayAsync();

            Console.WriteLine("Meta properties on Vertex properties:");

            foreach (var metaProperty in metaProperties)
            {
                Console.WriteLine($" {metaProperty}");
            }

            Console.WriteLine();
        }

        static async Task Main()
        {
            await new Program().Run();
        }
    }
}
