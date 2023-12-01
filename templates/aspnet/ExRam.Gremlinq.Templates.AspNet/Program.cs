using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.AspNet;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Templates.AspNet;
using ExRam.Gremlinq.Providers.TEMPLATEPROVIDER.AspNet;
using ExRam.Gremlinq.Support.NewtonsoftJson.AspNet;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddGremlinq(setup => setup
        .UseTEMPLATEPROVIDER<Vertex, Edge>()
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
