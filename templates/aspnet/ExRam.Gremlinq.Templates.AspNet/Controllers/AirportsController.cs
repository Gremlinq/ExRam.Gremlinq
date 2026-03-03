using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ExRam.Gremlinq.Core;

namespace ExRam.Gremlinq.Templates.AspNet
{
#if (true)  // --8<-- [start:airportsController]
    [ApiController]
    [Route("/airports")]
    public class AirportsController : ControllerBase
    {
        private readonly IGremlinQuerySource _g;

        public AirportsController(IGremlinQuerySource g)
        {
            _g = g;
        }

        [HttpGet]
        public async Task<IActionResult> Index() => Ok(await _g
             .V<Airport>()
             .ToArrayAsync());

        [HttpGet("/{airPortCode}")]
        public async Task<IActionResult> Single(string airPortCode)
        {
            var maybeAirport = await _g
                .V<Airport>()
                .Where(airport => airport.Code == airPortCode)
                .FirstOrDefaultAsync();

            return maybeAirport is { } airport
                ? Ok(airport)
                : NotFound();
        }
    }
#if (true)  // --8<-- [end:airportsController]
}
