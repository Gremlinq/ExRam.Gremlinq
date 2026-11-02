If your graph runs on Amazon Neptune 1.5 or any other TinkerPop 3.8 server, ExRam.Gremlinq 14.2.0 now speaks its language.

ExRam.Gremlinq is the .NET library for Apache TinkerPop graph databases: you write ordinary strongly-typed C#, and it talks to Cosmos DB, Amazon Neptune, JanusGraph or Gremlin Server for you.

TinkerPop 3.8 changed enough of the wire protocol that an application written for 3.7 gets rejected queries or silently wrong answers from a 3.8 server. With 14.2.0 you tell Gremlinq once which server you run, on Neptune simply the engine version, and it sends what that server understands. Existing setups keep working exactly as before.

📌 Works with TinkerPop 3.8 and Amazon Neptune engine 1.5
📌 One setting, in code or in appsettings, and no query changes
⚡ Queries are built and sent noticeably faster
🛠 Many fixes in how results come back into your own types

https://github.com/Gremlinq/ExRam.Gremlinq/releases/tag/14.2.0

#dotnet #csharp #graphdatabases #ApacheTinkerPop #AmazonNeptune
