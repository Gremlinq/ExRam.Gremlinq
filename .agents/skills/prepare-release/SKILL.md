---
name: prepare-release
description: Use this skill when preparing a new release. Writes the release notes and announcement texts, then bumps the version and creates the release tag. Invokes when asked to "prepare release" or similar. MUST NOT push anything to any remote.
---

# Prepare Release

Prepares a release of ExRam.Gremlinq: the texts first, then the version bump and the tag.

## Usage

    prepare release

## Workflow

1. Run `scripts/prerequisites.sh`. Every check must pass. It reports the remote, the
   current branch and the version this release will carry.
2. **Invoke the `write-release-announcements` skill via the Skill tool and wait for it to
   complete.** It collects the pull requests merged since the previous tag, writes
   `releases/<version>/release-notes.md` plus the announcement texts, and commits them.
3. Run `scripts/prepare.sh` to bump the version and create the tag.
4. Report what to do next -- see below. Do not do it.

**Step 2 must happen before step 3**, and the reason is not stylistic. `nbgv
prepare-release` creates a branch carrying the "Set version to 'X'" commit, and
`prepare.sh` tags that branch and rebases the current branch *onto* it. The tag therefore
points at the earlier of the two version commits, and anything committed afterwards is not
reachable from it. In ExRam.Gremlinq, `pack.yml`, `openAnnouncementChecklist.yml` and
`postDiscordAnnouncement.yml` all read `releases/<version>/` out of the tagged commit; a
repository that shares this skill but not those workflows still benefits from the same
ordering, since `pack.yml`'s release-body fallback is worse than having written the notes.
Prepare the texts late and they will simply not be there.

## What happens after this skill

The skill pushes nothing. Report these steps and let the user run them:

1. **Push the tag.** `git push <remote> <version>`. That is all the release needs to
   start: `pack.yml` triggers on the tag, builds, packs, attests, and creates a **draft**
   release whose body is `releases/<version>/release-notes.md`.
2. **Review and publish the draft** on GitHub. Nothing is public until then.
3. What publishing fires next is repository-specific -- check that repository's
   `.github/workflows/` rather than assuming. In ExRam.Gremlinq it is `pushStable.yml`
   (NuGet.org via Trusted Publishing), `publishBlogPost.yml` (copies the release body into
   the blog on docs.gremlinq.net), `openAnnouncementChecklist.yml` (opens an issue with the
   three channel texts, ready to post) and `postDiscordAnnouncement.yml` (posts the .NET
   one by webhook, if configured). A repository that only reuses ExRam.Gremlinq's `pack.yml`
   and `announce.yml` via `workflow_call` may fire a different subset of these effects, or
   none of them beyond the NuGet push -- report what you can confirm, not this list by
   default.

The two version commits stay on the local branch and reach the release branch later, with
whatever pull request is opened next. That is deliberate: the tag is what drives the
release, and it carries everything the workflows read.

## Key requirements

- **MUST NOT push anything** -- no branches, no tags. The user pushes the tag.
- The git remote is resolved dynamically, never hardcoded: this repository also has a
  non-GitHub backup remote.
- `scripts/prepare.sh` takes **no arguments**, and there is no choice to make here. The
  released version is the current preview version with its suffix dropped
  (`14.1.2-preview.{height}` releases as `14.1.2`), and the branch then moves on by one
  build number. Preparing a release cannot produce a minor or a major.
- **A minor or major bump is made in the pull request that changes the public API**, by
  raising `version.json` there. By the time a release is prepared the version has long
  been decided, so there is nothing left to decide -- and nothing prepare-release could do
  about it if the bump had been forgotten.
- Whether *this* release is a patch, minor or major is reported as `release_kind` by
  `write-release-announcements`, which derives it by comparing the previous tag with the
  version being released. That is the only signal for it, and it decides whether the
  announcement texts are needed.
- Do not write the release notes yourself. That is `write-release-announcements`, and it
  is the skill that knows the channel formats.

## References

- [Workflow Details](references/workflow.md) - what each script does, and what the release
  pipeline does afterwards
