A check that fails a graph query on purpose no longer changes what the rest of the query does. ExRam.Gremlinq 14.2.1 fixes that and two more things you would otherwise only notice in production.

ExRam.Gremlinq is the .NET library for Apache TinkerPop graph databases: you write ordinary strongly-typed C#, and it talks to Cosmos DB, Amazon Neptune, JanusGraph or Gremlin Server for you.

This patch is about correctness. Asserting a condition inside a query made the library lose track of what the query returns, so an update right after it could add a value instead of replacing it. And requests of one server session could land on different connections, where the server no longer knew the session.

🛠 Checks inside a query no longer change the write that follows them
🔗 A server session stays on its connection, and a lost one fails loudly instead of quietly starting over
✂️ Branches that can never run are no longer sent to the server

Upgrading from 14.2.0 needs no code changes.

https://github.com/Gremlinq/ExRam.Gremlinq/releases/tag/14.2.1

#dotnet #csharp #graphdatabases #ApacheTinkerPop
