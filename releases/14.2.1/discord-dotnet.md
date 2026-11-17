ExRam.Gremlinq 14.2.1 is out. It's an object-graph-mapper for Apache TinkerPop Gremlin databases: you write strongly-typed C#, it emits Gremlin, and the results come back as your own types. Works against Cosmos DB, Neptune, JanusGraph and Gremlin Server.

A patch release with three fixes:
🛠 `Coalesce(__ => __.Where(…), __ => __.Fail(…))`, the usual way to assert a condition, keeps the query's vertex type, so the `Property(…)` after it replaces a value instead of appending one
🔗 Overlapping requests of one server session stay on one pooled connection, and a session whose connection is gone throws instead of silently starting over
✂️ `Coalesce` no longer sends the branches after an identity branch, which never run

https://github.com/Gremlinq/ExRam.Gremlinq/releases/tag/14.2.1
