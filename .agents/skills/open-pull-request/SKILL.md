---
name: open-pull-request
description: Use this skill when opening a pull request for the current branch, or when an existing pull request needs a better description - including when the `check-description` CI check has failed. Writes the description that later becomes this change's release note. Invokes when asked to "open a pull request", "create a PR", "fix the PR description" or similar. Does NOT bump versions, create tags or push tags.
---

# Open Pull Request

Opens a pull request for the current branch with a description good enough to become a
release note, or repairs the description of an existing one.

## Why this exists

The release notes for this repository are assembled from pull request bodies, and the
GitHub release body is copied verbatim into the blog on docs.gremlinq.net. A pull request
description is therefore not review scaffolding that can be skipped -- it is the only
draft of the published changelog entry that will ever be written.

The `check-description` status check enforces a floor on this
(`.github/workflows/checkPullRequestDescription.yml`). Clearing that floor is the minimum
bar, not the goal.

## Usage

    open a pull request

## Workflow

1. Run `scripts/prerequisites.sh`. It verifies the tooling, resolves the base branch and
   reports whether a pull request for the current branch already exists.
2. Pick the mode from what the script reported and from what you already know:
   - **An open pull request already exists** -> mode C.
   - **You made the changes on this branch in this session** -> mode A.
   - **Otherwise** -> mode B.
3. Write the description according to [the style guide](references/description-style.md).
4. Self-check it: run `scripts/check-description.sh <file>`. It applies the same
   normalisation and the same threshold as the CI check, so passing here means passing
   there. It does not know about the exemptions the CI applies on top -- the
   `skip-changelog` label and bot authors -- so a failure here is not necessarily a
   failure there.
5. Push the branch, if `prerequisites.sh` reported `pushed=false`:

       git push -u <remote> HEAD

   **Always with `-u`.** It sets the upstream to `<remote>/<branch>`, which also repairs
   the tracking a branch gets when it was created from the base branch -- see the note
   under *Key requirements*.
6. Create or update the pull request. Always pass the body through a file
   (`--body-file`), never inline via `--body`.

## The three modes

### Mode A -- you did the work

This is the good case and the reason this skill exists: the intent is still in context,
so nothing has to be reconstructed.

Write the description from what you know -- the motivation, the design decisions, the
alternatives you rejected and why, the public API impact. Use the diff only to check
that you have not forgotten a part of the change, never as the source of the narrative.

### Mode B -- the branch is cold

You are looking at someone else's work, or your own from another session.

    git log --no-merges --format='%h %s%n%b' <base>..HEAD
    git diff <base>...HEAD --stat

Then actually read the diff of the files that carry the change. Commit subjects tell you
*what was touched*; only the code tells you *what it is for*.

Two rules for this mode:

- **Never describe the shape of the diff.** "Reduces the file by 98 lines while adding
  139" is not information about the change. If that is all you can say, you have not
  understood it yet.
- **Do not guess.** If the intent is not recoverable from the code -- why an approach was
  chosen, whether a behaviour change is deliberate -- ask the user rather than inventing
  a plausible rationale. A confidently wrong changelog entry is worse than a question.

### Mode C -- an existing pull request needs a better description

This is the path out of a failed `check-description` run. Nothing has to be closed and
reopened.

    gh pr view --json number,title,body,labels
    gh pr edit <number> --body-file <file>

Editing the body raises the `edited` event, which re-runs the check on its own.

If the change genuinely does not belong in the release notes -- a CI tweak, a dependency
bump, a refactoring with no user visible effect -- the honest fix is the label rather
than padded prose:

    gh pr edit <number> --add-label skip-changelog

## Key requirements

- The repository is `Gremlinq/ExRam.Gremlinq`. The git remote pointing at it is resolved
  by `scripts/prerequisites.sh`; do not hardcode a remote name.
- The base branch is the current release branch (`14.x` at the time of writing), resolved
  by `scripts/prerequisites.sh`. Do not assume `main`.
- Write the body to a file and pass `--body-file`. Inline `--body` mangles multi-line
  markdown and leaves the text at the mercy of shell quoting.
- **Branch off the base with `--no-track`:**

      git checkout -b <name> --no-track <remote>/<base>

  Without it, `branch.autoSetupMerge` (which defaults to true) makes the new branch track
  the *base* branch rather than itself -- `branch.<name>.merge` ends up as
  `refs/heads/<base>`. Nothing complains, but `git pull` then merges the base branch into
  the feature branch, and once the merged branch is deleted upstream the local one looks
  as though it had turned into the base branch. `git push -u` repairs it after the fact;
  `--no-track` stops it happening.
- This skill does not bump versions, create tags or push tags. That is `prepare-release`.

## References

- [Description Style](references/description-style.md) - structure, what makes a good
  entry, worked examples from this repository
