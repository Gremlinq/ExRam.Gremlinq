using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.AspNet;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Templates.AspNet;
#if (provider == "GremlinServer")
using ExRam.Gremlinq.Providers.GremlinServer.AspNet;
#elif (provider == "Neptune")
using ExRam.Gremlinq.Providers.Neptune.AspNet;
#elif (provider == "CosmosDb")
using ExRam.Gremlinq.Providers.CosmosDb.AspNet;
#elif (provider == "JanusGraph")
using ExRam.Gremlinq.Providers.JanusGraph.AspNet;
#endif
using ExRam.Gremlinq.Support.NewtonsoftJson.AspNet;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddGremlinq(setup => setup
#if (provider == "GremlinServer")
        .UseGremlinServer<Vertex, Edge>()
#elif (provider == "Neptune")
        .UseNeptune<Vertex, Edge>()
#elif (provider == "CosmosDb")
        .UseCosmosDb<Vertex, Edge>()
#elif (provider == "JanusGraph")
        .UseJanusGraph<Vertex, Edge>()
#endif
        .UseNewtonsoftJson())
    .AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapControllers();

app.Run();
