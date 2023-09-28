using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.AspNet;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Providers.Core.AspNet;
using ExRam.Gremlinq.Templates.AspNet;
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
