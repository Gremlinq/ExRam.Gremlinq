## Features

### TinkerPop 3.8

- Gremlinq can now be told which TinkerPop versions the server understands: `IGremlinQueryEnvironment.ServerCompatibilityRange` holds that range. Its first user is the step that filters out everything: for a range from 3.8 on, Gremlinq sends `discard()`, the name TinkerPop 3.8 gave `none()`. `Discard()` is new, `None()` is obsolete. Without configuration nothing changes. ([#2561](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2561))
- The range is configured on a provider configurator with the same name and shape as on the environment, `ConfigureServerCompatibilityRange(Func<VersionRange, VersionRange>)`. The transformation is given the range set before it, the provider's default included, so it can also change one end of it. ([#2573](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2573))
- A Neptune user names the engine version with `WithEngineVersion(...)` and gets the TinkerPop versions AWS documents for it: 3.7.1 for engines 1.3.2.0 up to 1.4.x, 3.8.0 to 3.8.1 for 1.5.x. `NeptuneEngineVersions` names the known engines. ([#2572](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2572))
- Both can be set from `IConfiguration`: `Gremlinq:ServerCompatibilityRange:Lowest` and `:Highest` for every provider set up through the AspNet packages, `Gremlinq:Neptune:EngineVersion` for Neptune. ([#2579](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2579))
- Gremlinq now depends on Gremlin.Net 3.8.2. It sends the same requests as before, so TinkerPop 3.7 servers keep working. ([#2559](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2559))
- TinkerPop 3.8 removed `has(key, traversal)`; a 3.8 server accepts it, but it never matches. For a range from 3.8 on, `Where(x => x.Prop, continuation)` and a member compared with a step label now send the `where()` filter a 3.7 server executes for it. ([#2564](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2564))
- `AggregateLocal` sends `local(aggregate(x))` to servers from 3.8 on, since TinkerPop 3.8 removed `aggregate(local, x)`. ([#2565](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2565))
- TinkerPop 3.8 lets a traverser that no `choose()` option matches, or whose choice traversal yields nothing, pass through unchanged. For a range from 3.8 on, `Choose` adds the two options that keep the semantics of 3.7: the first is filtered, the second fails. ([#2566](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2566))
- From TinkerPop 3.8 on, `limit(local, 1)`, `tail(local, 1)` and `range(local, n, n + 1)` return a list themselves, so a server from 3.8 on gets the plain step instead of the `map(unfold().limit(1).fold())` workaround, even with `WorkaroundRangeInconsistencies` set. ([#2567](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2567))
- A `gx:OffsetDateTime`, which a 3.8 server answers for `asDate()` and `dateAdd()`, is now read as a `DateTimeOffset` that keeps its offset. Before, the offset was lost or an `object` got a string. ([#2569](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2569))
- For a range from 3.8 on, a `DateTimeOffset` is written as `gx:OffsetDateTime` instead of `g:Date`. ([#2571](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2571))
- Bytes are read and written by the range. TinkerPop's `Byte` is signed, so from 3.8 on an `sbyte` is written as `gx:Byte`, a `byte` as a bare number, and a `gx:Byte` is read as an `sbyte`. ([#2570](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2570))
- For a range from 3.8 on, strategies and strategy types are written as Gremlin.Net 3.8 writes them. Gremlin Server 3.8.2 cannot read the 3.7 form. ([#2575](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2575))

### Deserialization

- A `g:Map` is now built into any dictionary class with a public parameterless constructor, such as `SortedDictionary<,>` or `SortedList<,>`, keys as they were typed. ([#2511](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2511))
- A dictionary is also built through a constructor that takes an `IReadOnlyDictionary<,>`, where only a constructor taking `IDictionary<,>` counted before. ([#2526](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2526))
- A value requested as a generic interface of a scalar type, such as `IEquatable<int>` or `INumber<int>`, is now read as that scalar type instead of being declined. ([#2518](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2518))

## Fixes

- Newtonsoft no longer throws for input it cannot read, such as a number too large for the requested `byte` or a `null` for an `int`. It declines the value, or leaves out only the part it cannot read. A type of your own that derives from `Property<T>` is read like `Property<T>`. ([#2505](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2505))
- A dictionary keeps a `null` value when its value type can hold one, from a `g:Map` as from a plain object. It is a real `null`, never a `JValue`. ([#2506](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2506))
- A typed value such as a `g:Int32` is no longer read as a dictionary of its `@type` and `@value`. Only a `g:Map` is a map, and a collection of key-value pairs gets the same answer as a `Dictionary<string, object>`. ([#2507](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2507))
- A `g:Map` whose keys are not names keeps its entries when requested as the non-generic `IDictionary`, `ICollection` or `IEnumerable`. ([#2508](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2508))
- A `g:List` requested as an `ImmutableArray<T>` now holds its items instead of coming back uninitialized. A generic collection interface of `object` gets a `List<object>`, so an `IList<object>` takes `Add` and `Remove`. ([#2512](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2512))
- A number is read as an enum only where it fits the enum's underlying type. Before, it was cut to that size and became another value. `true` and `false` are no enum values any more, and a `ulong` enum can hold its largest value. ([#2513](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2513))
- A dictionary without a public parameterless constructor, such as an `ImmutableSortedDictionary<int, string>` or a `ReadOnlyDictionary<int, string>`, keeps all entries of a `g:Map` whose keys are not names. ([#2520](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2520))
- The same holds for a class that implements `IReadOnlyDictionary<,>` but no `IDictionary<,>`. ([#2524](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2524))
- A `where()` on a scalar no longer drops its `is()` step when an `IPFactory` returns `P.Without(...)` with values. ([#2582](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2582))
- A converter reached through two nested `defer` calls now gets the whole converter chain as its `recurse` transformer, so a `recurse` call from there no longer skips the converters registered after that point. ([#2585](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2585))
- `GraphModel.FromBaseTypes` now says which type is the vertex base type and which the edge base type when it rejects base types in each other's hierarchy. ([#2583](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2583))

## Performance

- Typed `g:Tree` values are read faster: the tree converter reads them itself, without a detour through the typed-value converter per level. ([#2527](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2527))
- Serializing a query takes about a third less time: the converter chain is found in two thirds of the time, and the serializer looks an instruction's expansion up once instead of twice. ([#2586](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2586))
- Serializing a query that writes properties, projects a tuple or has a collection predicate allocates less. ([#2587](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2587))
- Building a query allocates less and takes less time: a continuation creates its anonymous query only when it runs one, and the label projections of a continuation that added no label are not merged. ([#2588](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2588))
- A constant that is an expression, as `x.Age > age + 1` or `ids[0]`, is read in about a microsecond instead of a hundred: it is read directly or interpreted instead of being compiled into a method that runs once. ([#2589](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2589))
- On the request path, a request is serialized before the connection's send lock is taken, the `Task.Yield()` per response is gone, the `MetaResponse<T>` decision is made once per result type, and a default cancellation token no longer links a token source. ([#2590](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2590))
- A received WebSocket message costs one pooled object instead of an owner per doubling and a `Task` per receive, and the send and the receive of a request run as pooled `ValueTask`s. ([#2591](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2591))

## Maintenance

- The docs of `Diff` say that the result is in the unit the server uses: seconds up to TinkerPop 3.7, milliseconds from 3.8 on. ([#2568](https://github.com/Gremlinq/ExRam.Gremlinq/pull/2568))

**Full changelog**: https://github.com/Gremlinq/ExRam.Gremlinq/compare/14.1.2...14.2.0
