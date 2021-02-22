using System.IO;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Samples.Shared;

namespace ExRam.Gremlinq.Samples.AspNet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GremlinqController : ControllerBase
    {
        private readonly IGremlinQuerySource _source;
        private readonly ILogger<GremlinqController> _logger;

        public GremlinqController(IGremlinQuerySource source, ILogger<GremlinqController> logger)
        {
            _source = source;
            _logger = logger;
        }

        [HttpGet]
        public async Task Index()
        {
            Response.ContentType = "text/plain";

            var writer = new StreamWriter(Response.Body, Encoding.UTF8)
            {
                AutoFlush = true
            };

            await new Logic(_source, writer)
                .Run();
        }
    }
}
