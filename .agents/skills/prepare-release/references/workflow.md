# Workflow Details

## Preparation Steps

The `scripts/prepare.sh` script performs these operations:

1. Runs `nbgv prepare-release --format json --versionIncrement build`
2. Extracts branch names from JSON output:
   - `new_branch`: The release branch name (becomes the tag)
   - `current_branch`: The branch being released from
3. Checks out the new branch
4. Amends the commit with `--no-edit -S` (signs the commit)
5. Rebases the current branch onto the new branch with `-Xtheirs` strategy
6. Tags the new branch with its name
7. Deletes the new branch
8. Returns to the original branch

## Prerequisites

The `scripts/prerequisites.sh` script validates:

1. **Nerdbank.GitVersioning (nbgv)**: Must be installed globally
2. **Git**: Must be installed
3. **Working Directory**: Must be in ExRam.Gremlinq repository root
4. **Clean Working Tree**: No uncommitted changes allowed

## Important Constraints

- The skill MUST NOT push any tags to any remote
- All operations are performed in the current working directory
- The skill ONLY handles version bumping and tagging
