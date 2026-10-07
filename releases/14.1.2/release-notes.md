## Fixes

- A value the requested type cannot read, such as `"not a date"` asked for as a `DateTime`, is now skipped like any other unconvertible item instead of failing the whole result set. ([#2438](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2438))
- A deserialization converter that throws no longer leaves a query waiting forever. The query now fails with the converter's exception, and the pool builds a fresh client for the next one. ([#2439](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2439))
- `Direction`, `Merge`, `T` and Gremlin.Net's other `EnumWrapper` types are now deserialized, where they used to fail or come back as the bare string. ([#2440](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2440))
- A value of a Gremlin enumeration that Gremlin.Net has no name for, such as a newer server's new value, is now read as a value of that enumeration instead of being lost. ([#2470](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2470))
- GraphSON arrays now deserialize into `ArrayList`, `Queue` and `Stack`, with their items converted like those of every other collection. ([#2444](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2444))
- A `g:Map` requested as an `IImmutableDictionary<,>` or `ImmutableDictionary<,>` no longer comes back empty. Building a `Dictionary<,>` from a map also allocates less. ([#2445](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2445))
- A `g:Map` keyed by something other than strings, such as ints, keeps its entries when requested as an `object`. It now comes back as a `Dictionary<object, object>`. ([#2459](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2459))
- A `g:Map` with a key that appears twice is now read, the last occurrence counting, instead of failing the whole result set with an `ArgumentException`. ([#2480](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2480))
- Traversers are now expanded into every collection type, such as `ImmutableList<T>`, `ISet<T>` or `Queue<T>`, where a bulked result used to throw. ([#2447](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2447))
- A `g:BulkSet` read into anything other than an array, such as a `List<int>`, now expands its bulks instead of returning the counts mixed in with the values. ([#2453](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2453))
- A `null` in a result array is now kept instead of being dropped, so the script and bytecode paths finally return the same results. ([#2448](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2448))
- A `null` in a `g:BulkSet` is now kept, together with its bulk, instead of being dropped. ([#2449](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2449))
- A value/bulk pair in a `g:BulkSet` whose bulk cannot be read is now skipped, instead of the value being emitted once. ([#2451](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2451))
- A `g:BulkSet` with an odd number of entries no longer throws an `ArgumentOutOfRangeException`. The well-formed pairs are read and the dangling element is skipped. ([#2452](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2452))
- A traverser is now only recognised when its envelope is spelled `@type` and `@value`, as every other typed value already was. Server output is unaffected. ([#2460](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2460))
- GraphSON's own member names (`key`, `value`, `id`, `label`, `properties`, `bulk`) are now matched exactly. A map of your own with members named `Key` and `Value` is no longer mistaken for a property. ([#2466](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2466))
- A `g:T` whose value is not exactly `id`, `key`, `label` or `value` no longer makes deserialization throw. ([#2467](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2467))
- `Coalesce(...)` with a single sub-query no longer changes what that sub-query returns. `Coalesce(__ => __.Out<WorksFor>().Limit(1))` used to return one vertex in total instead of up to one per incoming element; it is now emitted as a `flatMap(...)`. ([#2474](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2474))
- Named step labels no longer change the names of generated step labels, so the text of a query no longer depends on how many named labels come before. ([#2497](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2497))

## Performance

- `Map(...)` and `Project(...).By(...)` no longer emit a trailing `limit(1)`, since both only take the first result anyway. ([#2473](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2473))
- `Project(...).By(...)` no longer emits a `map()` that wraps the whole `by()` traversal. ([#2475](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2475))
- Slicing a `Traversal` no longer recounts the write steps of the slice, so cutting a read-only traversal no longer costs as much as it is long. ([#2472](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2472))

**Full changelog**: https://github.com/Gremlinq/ExRam.Gremlinq/compare/14.1.1...14.1.2
