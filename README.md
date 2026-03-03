[![#](assets/logo.png)]()

[![#](https://img.shields.io/github/license/Gremlinq/ExRam.Gremlinq?style=flat-square)]()
[![#](https://img.shields.io/nuget/v/ExRam.Gremlinq.Core?style=flat-square&logo=nuget)](https://www.nuget.org/packages?q=ExRam.Gremlinq)

# Synopsis

ExRam.Gremlinq is a .NET object-graph-mapper that bridges the gap between .NET applications and Apache TinkerPop™ Gremlin-enabled graph databases, functioning as the graph database equivalent of traditional ORMs. The library translates strongly-typed C# queries into valid Gremlin queries, automatically handling serialization and deserialization while preserving type information throughout the query pipeline.

The primary benefits include type safety through compile-time validation and IntelliSense support that minimizes runtime errors, an enhanced developer experience with familiar .NET syntax that eliminates manual Gremlin query construction, database agnostic compatibility with various graph databases including AWS Neptune and Azure Cosmos DB, and a professional ecosystem with commercial extensions and expert support available.

ExRam.Gremlinq is best suited for .NET applications working with highly interconnected data models where relationships are as important as the entities themselves, particularly when type safety and developer productivity are priorities. Created in 2017, the library has established itself as a mature solution for graph database development in the .NET ecosystem.

Head over to the official docs for an introduction on how to get started with Gremlinq and lots of example queries.

[![#](https://img.shields.io/badge/Read_the_docs!-EA6F1B?style=for-the-badge)](https://docs.gremlinq.net)

# Support

Schedule a video session with [@danielcweber](https://github.com/danielcweber) to get assistance with Gremlinq setup and
configuration, query writing, debugging or review.

For those desiring deeper understanding, workshops on the Gremlin language are offered, including core concepts and translation
into the Gremlinq domain-specific language. Schedule a call to discuss matters, your requirements, pricing conditions and get a
discount if a workshop materializes.

[![#](https://img.shields.io/badge/Schedule_a_call!-EA6F1B?style=for-the-badge)](https://docs.gremlinq.net/support/)

# Extensions

Add some of the most-wanted features missing from the Core library to your app, like

* System.Text.Json deserialization
* Request charge diagnostics for Azure CosmosDb
* OpenTelemetry instrumentation
* Traversal strategies
* Groovy script execution
* Transactions (in development)

[![#](https://img.shields.io/badge/Check_out_Gremlinq.Extensions!-EA6F1B?style=for-the-badge)](https://docs.gremlinq.net/extensions/)
