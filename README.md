![](https://raw.githubusercontent.com/ExRam/ExRam.Gremlinq/master/Assets/Logo.png)

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

#### Build strongly typed gremlin queries
``` csharp 
    var persons = await g
        .V<Person>()
        .Where(x => x.Age == 36)
        .ToArray();

    var person = await g
        .AddV(new Person { Age = 36 })
        .First();
```
#### Inheritance support
```csharp
    var animals = await g
        .V<Animal>()
        .ToArray();     // Gets vertices of type 'Cat' or 'Dog' if they inherit from 'Animal'
```
#### Deal with anonymous traversals, continuation passing style
```csharp
    var edge = await g
        .AddV<Person>()
        .AddE<WorksAt>()
        .To(__ => __
            .AddV<Company>())
        .First();
```
#### The fluent api remembers in- and out-vertices
```csharp
    var person = await g
        .AddV<Person>()
        .AddE<WorksAt>()
        .To(__ => __
            .AddV<Company>())
        .OutV()
        .First();
```
#### Navigate through the graph:
```csharp
    var employers = await g
        .V<Person>('bob')
        .Out<WorksAt>()
        .ToArray();
```
#### Deal easily with step labels, also continuation passing style
```csharp
    var tuples = await g
        .V<Person>('bob')
        .As((p, __ => __
            .Out<WorksAt>()
            .As((c, ___) => ___
                .Select(p, c)))
        .ToArray();
```
#### Support for complex boolean expressions
```csharp
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
```
#### Support for string expressions
```csharp
    var persons = await g
        .V<Person>()
        .Where(x => x.Name.StartsWith("B"))
        .ToArray();
```
#### Support for Linq expressions
```csharp
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
```

## Provider bindings
 - [Generic websocket](https://www.nuget.org/packages/ExRam.Gremlinq.Providers.WebSocket)
 - [Azure CosmosDB / CosmosDB Emulator](https://www.nuget.org/packages/ExRam.Gremlinq.Providers.CosmosDb)

## Development

Help on this project is greatly appreciated! Check out the [issues labelled 'up-for-grabs'](https://github.com/ExRam/ExRam.Gremlinq/issues?q=is%3Aissue+is%3Aopen+label%3Aup-for-grabs) or file your own and tackle them!

## Acknowledgements

The 'Gremlin'-graphic used in this project's logo on top of this page is a trademark of the Apache Software Foundation/Apache TinkerPop.
