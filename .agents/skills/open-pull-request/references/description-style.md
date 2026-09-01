# Pull Request Description Style

## Structure

```markdown
<one to three sentences: what changed and why>

## Changes

- **<Project or area>**: <what changed there>

## Notes

- <public API impact, breaking changes, benchmark numbers, follow-ups>
```

**Everything above the first `##` heading is the release note.** `write-release-announcements`
reads exactly that paragraph when it assembles `releases/<version>/release-notes.md`. In
ExRam.Gremlinq the GitHub release body is additionally copied verbatim into the blog on
docs.gremlinq.net -- other repositories that share this skill may not publish it any
further, but the paragraph is still the release note either way. Write it for someone
scanning a changelog, not for someone reading the diff.

The `## Changes` and `## Notes` sections are optional and exist for reviewers. A small,
self-contained change is perfectly well served by the opening paragraph alone.

## The opening paragraph

It has to answer two questions, in this order:

1. **What is now true that was not true before?** Phrase it as an effect on the library,
   not as an edit to a file.
2. **Why was that worth doing?** The problem it solves, the constraint it lifts, the
   allocation it saves.

It must not contain:

- **Diff statistics.** "Reduces the file by 98 lines while adding 139" describes the
  patch, not the change.
- **A restatement of the title.** If the body says what the title already said, the body
  is empty in every way that matters.
- **A file tour.** "Modifies `Traversal.cs`, `Step.cs` and three test files" belongs in
  `## Changes` at most, and usually nowhere.
- **Invented rationale.** If you do not know why an approach was chosen, ask.

## Titles

The title is the entry point in the release notes and in the GitHub release list.

- Imperative or descriptive, no trailing punctuation.
- Name the effect, not the mechanism: "Save allocation of a separate StepLabel identity"
  beats "Change StepLabel constructor".
- Backticks around type and member names survive into the notes and are worth using.

## Worked examples from this repository

### Good, and long enough to need sections -- #2417

> Adds a deterministic teardown path for Testcontainers-backed resources: the
> client-factory/executor chain now implements `IAsyncDisposable`, so disposing the query
> source's executor stops and disposes the underlying container.
>
> ## Changes
>
> - **Support.TestContainers**: `ContainerGremlinqClientFactory` implements
>   `IAsyncDisposable`; disposal stops (before disposing) and disposes the container ...
>
> ## Notes
>
> - `IGremlinQueryExecutor` and `IGremlinqClientFactory` are unchanged -- no public API
>   break; disposal is discovered via `is IAsyncDisposable` checks.

The opening paragraph stands alone as a changelog entry. The `## Notes` section answers
the question every consumer of a library asks about a change to an interface hierarchy.

### Good, and short enough not to -- #2420

> This is done in preparation of a possible anti-constraint (allow ref struct) on these
> interfaces. The anti-constraint need not be replicated everywhere if the implementations
> are explicit.

Two sentences, no sections, and it explains a change that the diff alone would make look
arbitrary. This is the floor for a description that is genuinely complete.

### Not good enough -- #2416

> Dispose a freshly built container when the CompareExchange loses to a concurrent
> dispose/creation.

An accurate summary of the mechanism that never says what went wrong before -- a leaked
container -- or that it was a race a user could actually hit.

### Not good enough -- #2398

> ## Changes
>
> - **Performance Optimization**:
>   - Modified `Traversal` to reuse the underlying list instead of creating a new list in
>     every case
>   - Reduces memory allocations and improves performance by avoiding unnecessary list
>     creation

Bullets only, so there is no opening paragraph for the release notes to draw on. The
second bullet is the generic claim that any allocation change could make; a measured
number would have been worth more than both bullets.

### The failure mode this skill exists to prevent -- #2404

> This pull request refactors the skill definitions and workflow documentation to improve
> maintainability and clarity ... reducing the SKILL.md file by 98 lines while adding 139
> lines ...

Written after the fact from commit messages and a diffstat. It is fluent, it is about the
right change, and it tells the reader nothing they could not see in the file list.

## Chores

Not every change belongs in the release notes. CI tweaks, dependency bumps and internal
refactorings with no user visible effect should carry the `skip-changelog` label, which
exempts them from the description check and keeps them out of the notes. Reach for the
label rather than padding a chore up to the character threshold.

Dependabot pull requests are exempt automatically; they need neither label nor prose.
