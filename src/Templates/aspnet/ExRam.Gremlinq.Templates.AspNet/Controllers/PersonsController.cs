using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Templates.AspNet
{
    [ApiController]
    [Route("/Persons")]
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

        [HttpPost]
        public async Task<IActionResult> Add(string name, int age) => Ok(await _g
            .AddV(new Person { Name = name, Age = age })
            .FirstAsync());
    }
}
