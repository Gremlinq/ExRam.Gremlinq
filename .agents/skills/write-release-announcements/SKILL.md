---
name: write-release-announcements
description: Use this skill to write the release notes and the announcement texts for an upcoming release, from the pull requests merged since the previous tag. Produces releases/<version>/release-notes.md plus LinkedIn and Discord texts. Invokes when asked to "write release announcements", "write the release notes" or similar, and is invoked by the prepare-release skill. Does NOT bump versions, create tags or publish anything.
---

# Write Release Announcements

Turns the pull requests merged since the last release into the four texts a release needs.

## Usage

    write release announcements

Runs standalone whenever the texts need to be written or rewritten, and is invoked by
`prepare-release` as the step before the version is bumped.

## Where the texts go, and why the order matters

Everything lands in `releases/<version>/` -- for example `releases/14.2.0/`:

| File | Becomes |
|---|---|
| `release-notes.md` | the GitHub release body (`pack.yml` passes it to `gh release create --notes-file`); in ExRam.Gremlinq, `publishBlogPost.yml` then also copies it verbatim into the blog on docs.gremlinq.net |
| `linkedin.md` | a manual LinkedIn post |
| `discord-tinkerpop.md` | a manual post in the TinkerPop Discord |
| `discord-dotnet.md` | a post in the .NET Discord, optionally sent by webhook in ExRam.Gremlinq |

The LinkedIn and Discord channels described here, and the worked examples in
[the channel style guide](references/channel-style.md), are ExRam.Gremlinq's own --
written for that library's audience and communities. A repository that shares this skill
but does not run its own LinkedIn/Discord announcements should confirm with the user which
of the four texts, if any, are wanted before writing all of them.

**These files must be committed before `nbgv prepare-release` runs.** That command creates
the branch the release tag is put on, and the current branch is then rebased *onto* it --
so the tag points at the earlier state. In ExRam.Gremlinq, `pack.yml`,
`openAnnouncementChecklist.yml` and `postDiscordAnnouncement.yml` all read these files out
of the tag; a repository without those workflows still benefits from the same ordering,
since a tag whose release-notes.md is missing falls back to a worse, auto-generated body.
Commit them late and they will not be there.

That is why this skill commits its own output rather than leaving it staged.

## Workflow

1. Run `scripts/collect-prs.sh`. It resolves the previous release tag, the target version
   (via `nbgv`) and the pull requests merged since, and prints JSON. Pass a tag as an
   argument to collect for a different range.
2. **Read the `excluded` list out loud to the user.** The script drops anything labelled
   `skip-changelog` and the release-preparation pull requests. That is a floor, not
   curation -- see below.
3. Propose a grouping of the remaining pull requests into Features / Fixes / Performance /
   Maintenance, together with anything you think should be dropped, and get agreement
   before writing.
4. Write the texts into `releases/<version>/` following
   [the channel style guide](references/channel-style.md). `release_kind` in the JSON says
   whether this is a `patch`, `minor` or `major` release: a patch gets `release-notes.md`
   only, unless the user asks for the rest.
5. Run `scripts/check-lengths.sh releases/<version>` and fix what it reports. Missing
   release notes fail it; the three channel texts are optional, so a patch release passes
   with only the notes. A Discord text over 2000 characters is rejected by Discord
   outright.
6. Commit: `git add releases/<version> && git commit -m "Add release notes and announcements for <version>"`.

## Curation is not automatic

The `skip-changelog` label removes the obvious chores. It does not decide what is worth
announcing, and the history shows why: the hand-written notes for 14.1.1 covered 7 of the
13 pull requests in range. Kept were a CI change (Trusted Publishing) and a test-reporting
change; dropped were dependency bumps and Actions maintenance. There is no rule that
separates those -- it is a judgement about what a reader of the changelog would care
about.

So: propose, show what you would drop and why, and let the user decide. Do not quietly
truncate, and do not pad the notes with everything in range either.

## Writing the entries

Each pull request in the JSON carries a **`lead`** field: the text above its first `##`
heading, with the pull request template's HTML comment already stripped. That is the
change's changelog entry. Use `lead`, not `body` -- authors routinely leave the template
comment in, and it sits above that heading, so reading `body` by hand would carry
"Replace this comment with one to three sentences..." into the published notes and from
there, verbatim, into the blog post.

If a `lead` runs longer than about two sentences, summarise it -- the notes are scanned,
not read.

If a `lead` is empty or useless, the entry cannot be reconstructed honestly
from the title alone. Say so, and either read the diff for that one change or ask. Do not
paraphrase the title back as if it were a description; that is the failure mode the
`open-pull-request` skill exists to prevent, and it should not be reintroduced here.

## Key requirements

- Version comes from `nbgv get-version`, never from a guess or from `version.json` read by
  hand.
- Every release-note entry links to its pull request as `([#2417](url))`.
- **No author names.** Nearly every pull request here is the maintainer's own.
- Do not create tags, do not bump `version.json`, do not push. That is `prepare-release`.
- Do not post anything anywhere. Delivery is manual, plus the optional webhook in
  `postDiscordAnnouncement.yml` where that workflow exists.

## References

- [Channel Style](references/channel-style.md) - what each of the four texts is for, its
  limits, and a worked example of each
