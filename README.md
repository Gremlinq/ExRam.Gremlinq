![](https://github.com/apache/tinkerpop/blob/master/docs/static/images/gremlin-help-wanted.png)

# ExRam.Gremlinq

ExRam.Gremlinq is a .NET object-graph-mapper for [Apache TinkerPop™](http://tinkerpop.apache.org/) Gremlin enabled databases.

## Packages

Package  | Link | 
-------- | :------------: | 
ExRam.Gremlinq.Core | [![#](https://img.shields.io/nuget/v/ExRam.Gremlinq.Core.svg)](https://www.nuget.org/packages/ExRam.Gremlinq.Core) |
ExRam.Gremlinq.Providers.WebSocket | [![#](https://img.shields.io/nuget/v/ExRam.Gremlinq.Providers.WebSocket.svg)](https://www.nuget.org/packages/ExRam.Gremlinq.Providers.WebSocket) |
ExRam.Gremlinq.Providers.CosmosDb | [![#](https://img.shields.io/nuget/v/ExRam.Gremlinq.Providers.CosmosDb.svg)](https://www.nuget.org/packages/ExRam.Gremlinq.Providers.CosmosDb) |

## Sample project

A sample project can be found at https://github.com/ExRam/ExRam.Gremlinq.Samples.

## Features

### Fluent Linq-style API:
Build strongly typed gremlin queries:
    
    //Get all vertices with label "Person" that have a property "Age" of value 36.

    var persons = await g
        .V<Person>()
        .Where(x => x.Age == 36)
        .ToArray();

    // Add a vertex with label "Person" and add a property "Age" of value 36.
    var person = await g
        .AddV(new Person { Age = 36 })
        .First();

    // Above query can also be written differently:
    var person = await g
        .AddV<Person>()
        .Property(x => x.Age, 36)
        .First();

    // Anonymous traversals are supported seamlessly:
    var edge = await g
        .AddV<Person>()
        .AddE<WorksAt>()
        .To(__ => __
            .AddV<Company>())
        .First();

    // Use a fluent syntax that remembers in- and out-vertices:
    var person = await g
        .AddV<Person>()
        .AddE<WorksAt>()
        .To(__ => __
            .AddV<Company>())
        .OutV()
        .First();

    // Navigate through the graph:
    var employers = await g
        .V<Person>('bob')
        .Out<WorksAt>()
        .ToArray();

    // Deal easily with step labels:
    var tuples = await g
        .V<Person>('bob')
        .As((p, __ => __
            .Out<WorksAt>()
            .As((c, ___) => ___
                .Select(p, c)))
        .ToArray();

    // Formulate more complex queries...
    var persons = await g
        .V<Person>()
        .Where(x => x.Age == 36 && x.Name == "Bob")
        .ToArray();

    var persons = await g
        .V<Person>()
        .Where(x => x.Age != 36)
        .ToArray();

    var persons = await g
        .V<Person>()
        .Where(x => x.Age < 36 && x.Name == "Bob")
        .ToArray();

    // ...even involving simple string operations
    var persons = await g
        .V<Person>()
        .Where(x => x.Name.StartsWith("B"))
        .ToArray();

    // Use it like Linq!
    var persons = await g
        .V<Person>()
        .Where(x => x.Pets.Contains("Daisy"))
        .ToArray();

    var persons = await g
        .V<Person>()
        .Where(x => x.Pets.Any())
        .ToArray();
        
    var persons = await g
        .V<Person>()
        .Where(t => t.PhoneNumbers.Intersects(new[] { "+4912345", "+4923456" }))
        .ToArray();

    var persons = await g
        .V<Person>()
        .Where(t => new[] { 36, 37, 38 }.Contains(t.Age))
        .ToArray();

### Development

The library is still under development. The API might change without notice. Help on this project is appreciated!

### Acknowledgements

The graphic on top of this page is a trademark of the Apache Software Foundation/Apache TinkerPop.
