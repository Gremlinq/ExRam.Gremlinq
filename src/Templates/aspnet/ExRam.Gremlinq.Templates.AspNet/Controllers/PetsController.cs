using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Templates.AspNet
{
    [ApiController]
    [Route("/Pets")]
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

        [HttpPost]
        public async Task<IActionResult> Add(string ownerName, string petName, int petAge) => Ok(await _g
            .V<Person>()
            .Where(person => person.Name == ownerName)
            .AddE<Owns>()
            .To(__ => __
                .AddV(new Pet { Name = petName, Age = petAge }))
            .InV()
            .FirstAsync());
    }
}
