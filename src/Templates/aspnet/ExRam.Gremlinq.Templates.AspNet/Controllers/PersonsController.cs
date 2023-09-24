using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Templates.AspNet
{
    [ApiController]
    [Route("/persons")]
    public class PersonsController : ControllerBase
    {
        private readonly IGremlinQuerySource _g;

        public PersonsController(IGremlinQuerySource g)
        {
            _g = g;
        }

        [HttpGet]
        public async Task<IActionResult> Index() => Ok(await _g
            .V<Person>()
            .ToArrayAsync());

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromQuery] string name, [FromQuery] int age) => Ok(await _g
            .AddV(new Person { Name = name, Age = age })
            .FirstAsync());
    }
}
