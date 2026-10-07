ExRam.Gremlinq 14.1.2 is out - the .NET OGM for TinkerPop-enabled graphs.

Gremlin-level changes in this one:

- A `coalesce()` of a single traversal is now written as `flatMap()`, so a `limit(1)` or `fold()` inside it acts per element instead of on the whole stream
- A trailing `limit(1)` in `map()` and `project().by()`, and a `map()` that wraps a whole `by()`, are left out
- GraphSON 3 reading: `g:BulkSet` and `g:Map` are read correctly into every collection type, `g:Direction`/`g:Merge`/`g:T` deserialize, and enum values Gremlin.Net does not know yet are kept

Targets TinkerPop 3.7, across Gremlin Server, JanusGraph, Neptune and Cosmos DB.

https://github.com/Gremlinq/ExRam.Gremlinq/releases/tag/14.1.2
