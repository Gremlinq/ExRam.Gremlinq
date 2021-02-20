using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Samples.AspNet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GremlinqController : ControllerBase
    {
        private int _created;
        private readonly IGremlinQuerySource _source;
        private readonly ILogger<GremlinqController> _logger;

        public GremlinqController(IGremlinQuerySource source, ILogger<GremlinqController> logger)
        {
            _source = source;
            _logger = logger;
        }

        [HttpGet("persons/")]
        public async Task<string[]> GetPersons()
        {
            await CreateGraph();

            return await _source
                .V<Person>()
                .Values(x => x.Name)
                .ToArrayAsync();
        }

        private async Task CreateGraph()
        {
            if (Interlocked.CompareExchange(ref _created, 1, 0) != 0)
                return;

            // Create a graph very similar to the one
            // found at http://tinkerpop.apache.org/docs/current/reference/#graph-computing.

            await _source.V().Drop();

            var marko = await _source
                .AddV(new Person { Name = "Marko", Age = 29 })
                .FirstAsync();

            var vadas = await _source
                .AddV(new Person { Name = "Vadas", Age = 27 })
                .FirstAsync();

            var josh = await _source
               .AddV(new Person { Name = "Josh", Age = 32 })
               .FirstAsync();

            var peter = await _source
               .AddV(new Person { Name = "Peter", Age = 35 })
               .FirstAsync();

            var daniel = await _source
               .AddV(new Person
               {
                   Name = "Daniel",
                   Age = 37,
                   PhoneNumbers = new[]
                   {
                        "+491234567",
                        "+492345678"
                   }
               })
               .FirstAsync();

            var charlie = await _source
                .AddV(new Dog { Name = "Charlie", Age = 2 })
                .FirstAsync();

            var catmanJohn = await _source
                .AddV(new Cat { Name = "Catman John", Age = 5 })
                .FirstAsync();

            var luna = await _source
                .AddV(new Cat { Name = "Luna", Age = 9 })
                .FirstAsync();

            var lop = await _source
                .AddV(new Software { Name = "Lop", Language = ProgrammingLanguage.Java })
                .FirstAsync();

            var ripple = await _source
                .AddV(new Software { Name = "Ripple", Language = ProgrammingLanguage.Java })
                .FirstAsync();

            await _source
                .V(marko.Id!)
                .AddE<Knows>()
                .To(__ => __
                    .V(vadas.Id!))
                .FirstAsync();

            await _source
                .V(marko.Id!)
                .AddE<Knows>()
                .To(__ => __
                    .V(josh.Id!))
                .FirstAsync();

            await _source
                .V(marko.Id!)
                .AddE<Created>()
                .To(__ => __
                    .V(lop.Id!))
                .FirstAsync();

            await _source
                .V(josh.Id!)
                .AddE<Created>()
                .To(__ => __
                    .V(ripple.Id!))
                .FirstAsync();

            // Creates multiple edges in a single query
            // Note that query ends with ToArrayAsync

            await _source
              .V(josh.Id!, peter.Id!)
              .AddE<Created>()
              .To(__ => __
                  .V(lop.Id!))
              .ToArrayAsync();

            await _source
                .V(josh.Id!)
                .AddE<Owns>()
                .To(__ => __
                    .V(charlie.Id!))
                .FirstAsync();

            await _source
                .V(josh.Id!)
                .AddE<Owns>()
                .To(__ => __
                    .V(luna.Id!))
                .FirstAsync();

            await _source
                .V(daniel.Id!)
                .AddE<Owns>()
                .To(__ => __
                    .V(catmanJohn.Id!))
                .FirstAsync();
        }
    }
}
