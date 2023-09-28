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
#if ProviderIsGremlinServer
        .UseGremlinServer(setup => setup
            .UseNewtonsoftJson())
#elif ProviderIsNeptune
        .UseNeptune(setup => setup
            .UseNewtonsoftJson())
#elif ProviderIsCosmosDb
        .UseCosmosDb<Vertex, Edge>(
            x => x.PartitionKey!,
            setup => setup
                .UseNewtonsoftJson())
#elif ProviderIsJanusGraph
        .UseJanusGraph(setup => setup
            .UseNewtonsoftJson())
#endif
#if !ProviderIsCosmosDb                   
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
