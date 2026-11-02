ExRam.Gremlinq 14.2.0 is out - the .NET OGM for TinkerPop-enabled graphs.

This release adds TinkerPop 3.8 support, on Gremlin.Net 3.8.2. Gremlinq takes a server compatibility range and sends the form its lowest version understands:

- `discard()` instead of `none()`, `where()` instead of the removed `has(key, traversal)`, `local(aggregate())` instead of `aggregate(local)`
- `choose()` keeps filtering unmatched traversers and failing on unproductive ones, as it did on 3.7
- `limit(local, 1)` and its siblings go out plain, 3.8 returns the list itself
- `gx:OffsetDateTime` for `DateTimeOffset`, signed `gx:Byte`, strategies in Gremlin.Net 3.8's form

Tested against Gremlin Server 3.7.7 and 3.8.2. Neptune engines map to their range by version, 1.5.x to 3.8.0 - 3.8.1. Without a configured range, nothing changes.

https://github.com/Gremlinq/ExRam.Gremlinq/releases/tag/14.2.0
