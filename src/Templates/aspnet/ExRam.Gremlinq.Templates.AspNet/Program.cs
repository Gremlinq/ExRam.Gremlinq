using ExRam.Gremlinq.Core.AspNet;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddGremlinq(setup => setup
#if ProviderIsGremlinServer
        .UseGremlinServer())
#elif ProviderIsNeptune
        .UseNeptune())
#elif ProviderIsCosmosDb
        .UseCosmosDb())
#elif ProviderIsJanusGraph
        .UseJanusGraph())
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
