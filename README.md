# ExRam.Gremlinq

ExRam.Gremlinq is a strongly typed Server driver for [Apache TinkerPop™](http://tinkerpop.apache.org/) Gremlin enabled databases.

## Packages

Package  | Link | 
-------- | :------------: | 
ExRam.Gremlinq | [![#](https://img.shields.io/nuget/v/ExRam.Gremlinq.svg)](https://www.nuget.org/packages/ExRam.Gremlinq) |
ExRam.Gremlinq.Providers.WebSocket | [![#](https://img.shields.io/nuget/v/ExRam.Gremlinq.Providers.WebSocket.svg)](https://www.nuget.org/packages/ExRam.Gremlinq.Providers.WebSocket) |
ExRam.Gremlinq.CosmosDb | [![#](https://img.shields.io/nuget/v/ExRam.Gremlinq.CosmosDb.svg)](https://www.nuget.org/packages/ExRam.Gremlinq.CosmosDb) |

## Features

### Fluent Linq-style API:
Build strongly typed gremlin queries:
    
	//Get all vertices with label "SomeVertexType" that have a property "SomeProperty" of value 36.

    var query = GremlinQuery.Create()
        .V<Person>()
        .Has(x => x.Age == 36);

	//Add a vertex with label "SomeVertexType" and add a property "SomeProperty" of value 36.
	var query = GremlinQuery.Create()
	    .AddV(new Person { Age = 36 });

	//Above query can also be written differently:
	var query = GremlinQuery.Create()
	    .AddV<Person>()
        .Property(x => x.Age, 36);

	//Anonymous traversals are supported seamlessly:
	var query = GremlinQuery.Create()
	    .AddV<Person>()
	    .AddE<WorksAt>()
	    .To(__ => __
	        .AddV<Company>());

	//Navigate through the graph:
	var query = GremlinQuery.Create()
        .V<Person>('bob')
	    .Out<WorksAt>();

	//Deal easily with step labels:
	var query = GremlinQuery.Create()
        .V<Person>('bob')
	    .As((p, __ => __
            .Out<WorksAt>()
            .As((c, ___) => ___
                .Select(p, c)));

    //Formulate more complex queries
	var query = GremlinQuery.Create()
        .V<Person>()
        .Has(x => x.Age == 36 && x.Name == "Bob");

	var query = GremlinQuery.Create()
        .V<Person>()
        .Has(x => x.Age != 36);

	var query = GremlinQuery.Create()
        .V<Person>()
        .Has(x => x.Age < 36 && x.Name == "Bob");

	var query = GremlinQuery.Create()
        .V<Person>()
        .Has(x => x.Name.StartsWith("B"));

	var query = GremlinQuery.Create()
        .V<Person>()
        .Has(x => x.Pets.Contains("Daisy"));

	var query = GremlinQuery.Create()
        .V<Person>()
        .Has(x => x.Pets.Any());
		
	var query = GremlinQuery
	    .Create()
        .V<Person>()
        .Where(t => t.PhoneNumbers.Intersects(new[] { "+4912345", "+4923456" }));

	var query = GremlinQuery
	    .Create()
        .V<Person>()
        .Where(t => new[] { 36, 37, 38 }.Contains(t.Age));

### Development

The library is still under development. The API might change without notice. Help on this project is appreciated!
