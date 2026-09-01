# Workflow Details

## `scripts/prerequisites.sh`

Verifies `nbgv`, `git`, `jq` and an authenticated `gh` -- the last two because
`write-release-announcements` runs before the version bump and needs them. Then checks
that the working tree is clean, that a GitHub remote exists, and that the tag for the
version about to be released does not already exist. That last check matters: creating an
existing tag fails halfway through `prepare.sh`, after the version commits have been made,
leaving the branch in a state that has to be unwound by hand.

Prints `remote=`, `branch=` and `version=` on success.

## `scripts/prepare.sh`

    prepare.sh          # no arguments

1. `nbgv prepare-release --format json`, which creates a branch named after the release
   version holding the `Set version to 'X'` commit, and leaves the current branch with the
   follow-up `Set version to 'X+1-preview.{height}'` commit. The increment comes from
   `version.json`'s `release.versionIncrement`, which is `build` and stays `build`: a minor
   or major bump belongs in the pull request that changes the public API, not here.
2. Checks out that branch and amends its commit with `-S`. Signed commits are required on
   the release branches, and the commit nbgv creates is not signed.
3. `git rebase <new branch> <current branch> -Xtheirs`, which also returns to the branch
   we started on.
4. Tags the release commit with the version and deletes the temporary branch.

**The tag points at the earlier of the two version commits.** Everything the release
pipeline reads out of the repository -- `releases/<version>/release-notes.md` and the
announcement texts -- must therefore be committed *before* this script runs.

## After the tag is pushed

    git push <remote> <version>

The table below is ExRam.Gremlinq's own pipeline. A repository that shares this skill
without owning these workflow files (e.g. one that only calls a subset of them via
`workflow_call`) will not see every row -- check that repository's `.github/workflows/`
rather than assuming this list applies verbatim.

| Trigger | Workflow | Effect |
|---|---|---|
| tag push | `pack.yml` | builds, packs, attests, creates a **draft** release with `releases/<version>/release-notes.md` as its body and the packages as assets |
| tag push | `pushPreview.yml` | pushes packages to GitHub Packages once `Pack` succeeds |
| release published (manual) | `pushStable.yml` | pushes to NuGet.org via Trusted Publishing (OIDC) |
| release published (manual) | `publishBlogPost.yml` | copies the release body verbatim into `docs/blog/posts/` in `Gremlinq/docs.gremlinq.net`; also runs on `edited`, overwriting the same post |
| release published (manual) | `openAnnouncementChecklist.yml` | opens an issue with the three channel texts, one checkbox each |
| release published (manual) | `postDiscordAnnouncement.yml` | posts the .NET Discord text by webhook, if `DISCORD_WEBHOOK_URL` is configured |

Nothing is public until the draft is published by hand. Up to that point the tag can be
deleted and the release re-prepared.

If `releases/<version>/release-notes.md` is missing, `pack.yml` falls back to GitHub's own
generated notes and warns. That is a safety net, not the intended outcome -- and it cannot
be corrected after the fact without deleting and re-pushing the tag, because the notes are
read from the tagged commit. Write them before tagging.

## Constraints

- The skill MUST NOT push anything to any remote.
- All operations happen in the current working directory.
- The version commits are not pushed at release time; they reach the release branch with
  the next pull request. The tag is what the pipeline runs on.
