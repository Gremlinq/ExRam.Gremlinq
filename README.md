![](https://github.com/apache/tinkerpop/blob/master/docs/static/images/gremlin-help-wanted.png)

# ExRam.Gremlinq

ExRam.Gremlinq is .NET Object-graph-mapper for [Apache TinkerPop™](http://tinkerpop.apache.org/) Gremlin enabled databases.

## Packages

Package  | Link | 
-------- | :------------: | 
ExRam.Gremlinq.Core | [![#](https://img.shields.io/nuget/v/ExRam.Gremlinq.Core.svg)](https://www.nuget.org/packages/ExRam.Gremlinq.Core) |
ExRam.Gremlinq.Providers.WebSocket | [![#](https://img.shields.io/nuget/v/ExRam.Gremlinq.Providers.WebSocket.svg)](https://www.nuget.org/packages/ExRam.Gremlinq.Providers.WebSocket) |
ExRam.Gremlinq.Providers.CosmosDb | [![#](https://img.shields.io/nuget/v/ExRam.Gremlinq.Providers.CosmosDb.svg)](https://www.nuget.org/packages/ExRam.Gremlinq.Providers.CosmosDb) |

## Features

### Fluent Linq-style API:
Build strongly typed gremlin queries:
    
	//Get all vertices with label "Person" that have a property "Age" of value 36.

    var query = g
        .V<Person>()
        .Has(x => x.Age == 36);

	//Add a vertex with label "Person" and add a property "Age" of value 36.
	var query = g
	    .AddV(new Person { Age = 36 });

	//Above query can also be written differently:
	var query = g
	    .AddV<Person>()
        .Property(x => x.Age, 36);

	//Anonymous traversals are supported seamlessly:
	var query = g
	    .AddV<Person>()
	    .AddE<WorksAt>()
	    .To(__ => __
	        .AddV<Company>());

	//Navigate through the graph:
	var query = g
        .V<Person>('bob')
	    .Out<WorksAt>();

	//Deal easily with step labels:
	var query = g
        .V<Person>('bob')
	    .As((p, __ => __
            .Out<WorksAt>()
            .As((c, ___) => ___
                .Select(p, c)));

    //Formulate more complex queries
	var query = g
        .V<Person>()
        .Has(x => x.Age == 36 && x.Name == "Bob");

	var query = g
        .V<Person>()
        .Has(x => x.Age != 36);

	var query = g
        .V<Person>()
        .Has(x => x.Age < 36 && x.Name == "Bob");

	var query = g
        .V<Person>()
        .Has(x => x.Name.StartsWith("B"));

	var query = g
        .V<Person>()
        .Has(x => x.Pets.Contains("Daisy"));

	var query = g
        .V<Person>()
        .Has(x => x.Pets.Any());
		
	var query = g
        .V<Person>()
        .Where(t => t.PhoneNumbers.Intersects(new[] { "+4912345", "+4923456" }));

	var query = g
        .V<Person>()
        .Where(t => new[] { 36, 37, 38 }.Contains(t.Age));

### Development

The library is still under development. The API might change without notice. Help on this project is appreciated!

### Acknowledgements

The graphic on top of this page is a trademark of the Apache Software Foundation/Apache TinkerPop.
