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
reachable from it. Both `pack.yml` and `announcementKit.yml` read `releases/<version>/`
out of the tagged commit. Prepare the texts late and they will simply not be there.

## What happens after this skill

The skill pushes nothing. Report these steps and let the user run them:

1. **Push the tag.** `git push <remote> <version>`. That is all the release needs to
   start: `pack.yml` triggers on the tag, builds, packs, attests, and creates a **draft**
   release whose body is `releases/<version>/release-notes.md`.
2. **Review and publish the draft** on GitHub. Nothing is public until then.
3. Publishing fires `pushStable.yml` (NuGet.org via Trusted Publishing), `announce.yml`
   (copies the release body into the blog on docs.gremlinq.net) and `announcementKit.yml`
   (opens an issue with the LinkedIn and Discord texts, ready to post).

The two version commits stay on the local branch and reach the release branch later, with
whatever pull request is opened next. That is deliberate: the tag is what drives the
release, and it carries everything the workflows read.

## Key requirements

- **MUST NOT push anything** -- no branches, no tags. The user pushes the tag.
- The git remote is resolved dynamically, never hardcoded: this repository also has a
  non-GitHub backup remote.
- `scripts/prepare.sh` takes the version increment as an argument: `build` (the default),
  `minor` or `major`. A minor or major release also needs the announcement texts; a patch
  release gets release notes only.
- Do not write the release notes yourself. That is `write-release-announcements`, and it
  is the skill that knows the channel formats.
- `Prepare release` is a reserved pull request title in this repository -- it skips the
  test matrix and the description check. The current flow does not open such a pull
  request, but the title stays reserved.

## References

- [Workflow Details](references/workflow.md) - what each script does, and what the release
  pipeline does afterwards
