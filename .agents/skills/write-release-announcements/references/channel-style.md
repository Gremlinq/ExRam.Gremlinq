# Announcement Channel Style

Four texts come out of a release, and they are not four renderings of the same text. The
release notes are a record; the other three are messages to specific rooms with specific
expectations. Writing one and reformatting it three times produces three bad posts.

All four are written in **English**, matching the repository and the pull requests.

## When each one is written

| Version bump | Release notes | LinkedIn | Discord |
|---|---|---|---|
| Major (`15.0.0`) | yes | yes | yes |
| Minor (`14.2.0`) | yes | yes | yes |
| Patch (`14.1.2`) | yes | no | no |

Patch releases get notes and nothing else. Both Discord servers belong to communities the
maintainer does not run, and in a room you are a guest in, frequency is the thing that
turns an announcement into spam -- not the content. Ask before deviating.

---

## Release notes -- `release-notes.md`

The record. Becomes the GitHub release body, and `.github/workflows/publishBlogPost.yml` copies
that body verbatim into the blog on docs.gremlinq.net, so it is also the published
changelog.

```markdown
## Features

- Short sentence about the change, in the past tense, from the reader's point of view. ([#2417](url))

## Fixes

## Performance

## Maintenance

**Full changelog**: <compare url>
```

- Drop any section that would be empty.
- One entry per pull request, drawn from the text above the first `##` heading in its
  body. **Summarise when that text is longer than about two sentences** -- the notes are
  scanned, not read.
- Every entry ends with a link to its pull request, `([#2417](url))`.
- **No authors.** Nearly every pull request here is the maintainer's own; attribution adds
  noise and no information.
- Entries describe the effect on someone using the library. "`Traversal` now reuses its
  underlying list" is a fact about the code; "cuts an allocation per traversal step" is
  the same change described as a reader would experience it.
- Keep backticks around type and member names. They survive into the blog.

---

## LinkedIn -- `linkedin.md`

**Audience:** .NET developers, architects, and a professional network that is only partly
technical.
**Goal:** visibility and credibility. Nobody upgrades a NuGet package because of a
LinkedIn post; they remember that the project is alive and maintained.

| | |
|---|---|
| Length | 90-200 words |
| Markup | **none** -- LinkedIn renders no markdown. `**bold**` appears literally, as do backticks. Line breaks are the only formatting available |
| Emoji | 3-4 at most, as line markers at the start of a line, never inside a sentence |
| Hashtags | 3-5, on their own line at the end |
| Image | optional, 1200x627 landscape or 1200x1200 square |

**Structure**

1. **Hook, 1-2 lines.** LinkedIn truncates at roughly 200 characters behind a "...more"
   link, and that is the entire decision the reader makes. Open with the concrete thing
   that is now possible or the problem that is now gone. Never open with "I'm happy to
   announce".
2. **One paragraph** on what shipped and why it matters. Plain language; assume the reader
   knows .NET but not Gremlin.
3. **2-4 marker lines**, one per headline item, each a single line.
4. **Link** on its own line at the end.
5. **Hashtags** on the final line. `#dotnet #csharp #graphdatabases #ApacheTinkerPop` plus
   at most one situational tag (`#CosmosDB`, `#Neo4j`, `#performance`).

Posts carrying an outbound link tend to reach fewer people. The alternative is to end the
post without a link and put it in the first comment. Worth doing for a major release; not
worth the extra step for a minor one.

**Example**

```
Disposing a query source now tears down its Testcontainers container with it. No more
orphaned Docker containers after an integration test run.

ExRam.Gremlinq 14.2.0 is out. It's a .NET object-graph-mapper for Apache TinkerPop
Gremlin databases - you write strongly-typed C# and it produces Gremlin, against Cosmos
DB, Neptune, JanusGraph or a plain Gremlin Server.

This release is mostly about the sharp edges you only hit in a real test suite:

- Deterministic container teardown through IAsyncDisposable
- A dispose/creation race during container creation, fixed
- Fewer allocations in traversal building

https://github.com/Gremlinq/ExRam.Gremlinq/releases/tag/14.2.0

#dotnet #csharp #graphdatabases #ApacheTinkerPop
```

---

## TinkerPop Discord -- `discord-tinkerpop.md`

**Audience:** Gremlin and graph practitioners, polyglot -- Java, Python, JavaScript, Go
-- including TinkerPop committers. They know Gremlin far better than they know .NET.
**Goal:** peer-level information inside the ecosystem. This is a room full of experts;
marketing language is noticed and held against you.

| | |
|---|---|
| Length | 50-120 words |
| Limit | **2000 characters**, hard -- Discord rejects longer messages |
| Markup | Discord markdown: `**bold**`, `` `code` ``, ```` ```csharp ```` blocks, `-#` subtext |
| Emoji | 0-1 |
| Channel | the release or general channel. Never a support channel |

**What belongs in it**

- What is new **at the Gremlin level**: newly supported steps and operators, semantics
  that changed, serialisation formats. `elementMap()` support is news here; a .NET
  allocation win is not.
- Which TinkerPop version is targeted.
- Which providers are covered, when that changed.
- A link to the release.

Say what changed and stop. No hook, no call to action.

**Example**

```
ExRam.Gremlinq 14.2.0 is out - the .NET OGM for TinkerPop-enabled graphs.

Gremlin-level changes in this one:

- `elementMap()` is now supported as a first-class operator, including deserialisation
  straight back into mapped element types
- `ElementMapStep` is public, so custom serialisers can special-case it

Targets TinkerPop 3.7, and the same query surface works across Gremlin Server,
JanusGraph, Neptune and Cosmos DB.

https://github.com/Gremlinq/ExRam.Gremlinq/releases/tag/14.2.0
```

---

## .NET Discord -- `discord-dotnet.md`

**Audience:** C# developers, most of whom have never written a Gremlin traversal and do
not know what an OGM for graphs would even do.
**Goal:** discovery. The reader has to understand what this is inside one sentence, or
they scroll past.

| | |
|---|---|
| Length | 70-160 words including the snippet |
| Limit | **2000 characters**, hard |
| Markup | Discord markdown, and a ```` ```csharp ```` block |
| Emoji | 2-3 |
| Channel | the designated self-promotion or show-and-tell channel. Check the server rules; posting a release outside it is a rule violation, not a faux pas |

**What belongs in it**

1. **One sentence** establishing what the library is. Every post needs it -- the audience
   turns over and nobody is following the project.
2. **A code snippet, 5-8 lines**, showing the new capability as C#. This is the part that
   does the work: it demonstrates the fluent, strongly-typed surface in a way no
   description can.
3. **2-3 bullets** on the rest of the release.
4. **A link.**

Keep the prose tight -- the snippet eats a large part of the character budget.

**Example**

````
ExRam.Gremlinq 14.2.0 is out. It's an object-graph-mapper for Apache TinkerPop Gremlin
databases: you write strongly-typed C#, it emits Gremlin, and the results come back as
your own types. Works against Cosmos DB, Neptune, JanusGraph and Gremlin Server.

New in this release, `elementMap()` deserialises straight back into your model:

```csharp
var people = await g
    .V<Person>()
    .Where(x => x.Age > 30)
    .ElementMap()
    .ToArrayAsync();
```

Also in 14.2.0:
- Testcontainers containers are now torn down when you dispose the query source
- Fewer allocations when building traversals

https://github.com/Gremlinq/ExRam.Gremlinq/releases/tag/14.2.0
````

---

## Checks before finishing

- Both Discord texts under 2000 characters. `scripts/check-lengths.sh` measures them.
- The LinkedIn text contains no `*`, `_`, `#` or backtick used as markup.
- Every text links to the release, and every link is the real tag URL.
- No author names anywhere.
- The C# snippet compiles conceptually against the API this release actually ships -- do
  not invent a fluent method to make the example read better.
