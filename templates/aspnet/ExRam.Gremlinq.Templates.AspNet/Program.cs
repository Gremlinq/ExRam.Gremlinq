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
        .UseGremlinServer(setup => setup
            .UseNewtonsoftJson())
#elif (provider == "Neptune")
        .UseNeptune(setup => setup
            .UseNewtonsoftJson())
#elif (provider == "CosmosDb")
        .UseCosmosDb<Vertex, Edge>(
            x => x.PartitionKey!,
            setup => setup
                .UseNewtonsoftJson())
#elif (provider == "JanusGraph")
        .UseJanusGraph(setup => setup
            .UseNewtonsoftJson())
#endif
#if (provider != "CosmosDb")          
        .ConfigureEnvironment(env => env
            .UseModel(GraphModel
                .FromBaseTypes<Vertex, Edge>(lookup => lookup
                    .IncludeAssembliesOfBaseTypes()))))
#endif
    .AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapControllers();

app.Run();
