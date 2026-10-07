One unreadable value in a query result no longer takes the other thousand down with it.

ExRam.Gremlinq 14.1.2 is out. It's a .NET object-graph-mapper for Apache TinkerPop Gremlin databases - you write strongly-typed C# and it produces Gremlin, against Cosmos DB, Neptune, JanusGraph or a plain Gremlin Server.

This release is about reading results back. Seventeen fixes make GraphSON deserialization skip what it cannot read instead of failing, keep nulls where they arrived, and fill every collection type correctly, immutable and non-generic ones included.

🔹 A converter that throws now fails the query instead of leaving it hanging
🔹 Coalesce with a single sub-query returns what that sub-query returns
🔹 Shorter Gremlin from Map and Project, with redundant steps left out

https://github.com/Gremlinq/ExRam.Gremlinq/releases/tag/14.1.2

#dotnet #csharp #graphdatabases #ApacheTinkerPop
