ExRam.Gremlinq 14.2.0 is out. It's an object-graph-mapper for Apache TinkerPop Gremlin databases: you write strongly-typed C#, it emits Gremlin, and the results come back as your own types. Works against Cosmos DB, Neptune, JanusGraph and Gremlin Server.

New in this release: TinkerPop 3.8 support. Tell Gremlinq what your server speaks, and it sends the right form of every step and type:

```csharp
var g = GremlinQuerySource.g
    .UseNeptune<Vertex, Edge>(_ => _
        .At(new Uri("wss://my-cluster:8182"))
        .WithEngineVersion(NeptuneEngineVersions.V1_5_0_0));

var people = await g
    .V<Person>()
    .Where(x => x.Age > 30)
    .ToArrayAsync();
```

Also in 14.2.0:
🚀 Serializing a query takes about a third less time, building one allocates less
🧰 A round of deserialization fixes: any dictionary type, nulls kept, enums checked
⚙️ The range also comes from appsettings: `Gremlinq:ServerCompatibilityRange:Lowest` and `:Highest`

https://github.com/Gremlinq/ExRam.Gremlinq/releases/tag/14.2.0
