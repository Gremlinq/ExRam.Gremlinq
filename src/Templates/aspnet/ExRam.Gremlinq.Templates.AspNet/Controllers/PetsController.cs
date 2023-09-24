using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Templates.AspNet
{
    [ApiController]
    [Route("/pets")]
    public class PetsController : ControllerBase
    {
        private readonly IGremlinQuerySource _g;

        public PetsController(IGremlinQuerySource g)
        {
            _g = g;
        }

        [HttpGet]
        public async Task<IActionResult> Index() => Ok(await _g
            .V<Pet>()
            .ToArrayAsync());

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromQuery] string ownerName, [FromQuery] string petName, [FromQuery] int petAge) => Ok(await _g
            .V<Person>()
            .Where(person => person.Name == ownerName)
            .AddE<Owns>()
            .To(__ => __
                .AddV(new Pet { Name = petName, Age = petAge }))
            .InV()
            .FirstAsync());
    }
}
