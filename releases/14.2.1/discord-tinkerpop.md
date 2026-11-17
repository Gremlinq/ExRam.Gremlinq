ExRam.Gremlinq 14.2.1 is out - a patch release of the .NET OGM for TinkerPop-enabled graphs.

- A branch ending in `fail()` no longer counts towards the result type of `coalesce()`, `union()` or `choose()`. So `coalesce(<check>, fail())` keeps the vertex type, and the `property()` after it still goes out with `single` instead of appending on a `set`-cardinality provider.
- `coalesce()` drops every branch after its first `identity()` branch. The step itself stays, since it adds a path entry.
- The connection pool keeps all requests of a session (`processor: session`) on the connection that opened it, `op: close` included.

https://github.com/Gremlinq/ExRam.Gremlinq/releases/tag/14.2.1
