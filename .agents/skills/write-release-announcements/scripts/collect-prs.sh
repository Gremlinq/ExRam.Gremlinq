#!/usr/bin/env bash
#
# Collects the pull requests that were merged since the previous release, so their bodies
# can be turned into release notes.
#
# Merges in this repository are rebase-only, so there are no merge commits and
# 'git log --merges' finds nothing. The mapping from commits to pull requests only exists
# on GitHub, which is why this goes through the API.
#
# Usage: collect-prs.sh [<previous tag>]
#
# Writes JSON to stdout:
#   { "previous_tag", "previous_tag_date", "version", "base", "compare_url",
#     "pull_requests": [ { number, title, body, url, labels, mergedAt } ],
#     "excluded":      [ { number, title, reason } ] }

set -euo pipefail

fail() { echo "ERROR: $*" >&2; exit 1; }

for tool in git gh jq nbgv; do
    command -v "$tool" >/dev/null 2>&1 || fail "$tool is not installed."
done

remote="$(git remote -v | grep -m1 'github\.com' | awk '{print $1}')"
[ -n "$remote" ] || fail "No git remote pointing at github.com."

base="$(gh repo view --json defaultBranchRef --jq '.defaultBranchRef.name')"
repo="$(gh repo view --json nameWithOwner --jq '.nameWithOwner')"

# Only x.y.z tags are releases. Sorting by version rather than by date matters: a patch on
# an older line can be tagged after a newer minor.
if [ $# -ge 1 ]; then
    previous_tag="$1"
else
    previous_tag="$(git tag --list --merged "$remote/$base" --sort=-v:refname \
        | grep -m1 -E '^[0-9]+\.[0-9]+\.[0-9]+$')" \
        || fail "Could not find a previous release tag reachable from $remote/$base."
fi

git rev-parse --verify --quiet "$previous_tag^{commit}" >/dev/null \
    || fail "Tag '$previous_tag' does not exist locally. Fetch tags first."

previous_tag_date="$(git log -1 --format=%cI "$previous_tag")"

# The version this release will carry: the current preview version with its suffix
# dropped, which is exactly what 'nbgv prepare-release' will stamp.
version="$(nbgv get-version --format json | jq -r '.SimpleVersion')"
[ -n "$version" ] && [ "$version" != 'null' ] || fail "Could not determine the version via nbgv."

# 'merged:>' takes the tag's commit date. Anything merged at or before it is in the
# previous release. The base filter keeps pull requests targeting other release lines out.
all="$(gh pr list \
    --state merged \
    --base "$base" \
    --search "merged:>$previous_tag_date" \
    --limit 200 \
    --json number,title,body,url,labels,mergedAt)"

# Excluded rather than silently dropped, so the agent can see what it is not writing about
# and can catch a chore that was mislabelled.
jq -n \
    --arg previous_tag "$previous_tag" \
    --arg previous_tag_date "$previous_tag_date" \
    --arg version "$version" \
    --arg base "$base" \
    --arg compare_url "https://github.com/$repo/compare/$previous_tag...$version" \
    --argjson all "$all" \
    '
    def excluded_reason:
        if ([.labels[].name] | index("skip-changelog")) then "skip-changelog label"
        elif (.title | test("^Prepare release$")) then "release preparation"
        elif (.title | test("^Preview-bump$")) then "release preparation"
        else null end;

    # The changelog entry for a pull request is the prose above its first level-2 heading.
    # Authors routinely leave the pull request template comment in the body, and it sits
    # above that heading -- without stripping it the template text would travel into the
    # release notes and, verbatim, into the blog post.
    # Note the "m" flag: in jq'"'"'s Oniguruma flavour that is dotall -- the one that makes "."
    # match newlines. jq'"'"'s "s" means single line mode and would leave every multi-line
    # comment in place.
    def lead:
        ("\n" + ((.body // "") | gsub("\r"; "")))
        | gsub("<!--.*?-->"; ""; "m")
        | split("\n## ")[0]
        | sub("^\\s+"; "") | sub("\\s+$"; "");

    # The only place the kind of release can be determined. Preparing a release always
    # just drops the preview suffix and moves the branch on by one build number, so the
    # version being released was already fixed in version.json -- raised there by whichever
    # pull request changed the public API. Comparing it with the previous tag is what makes
    # that decision visible again.
    def release_kind($previous; $next):
        ($previous | split(".") | map(tonumber)) as $p
        | ($next | split(".") | map(tonumber)) as $n
        | if $n[0] != $p[0] then "major"
          elif $n[1] != $p[1] then "minor"
          else "patch" end;

    {
        previous_tag: $previous_tag,
        previous_tag_date: $previous_tag_date,
        version: $version,
        release_kind: release_kind($previous_tag; $version),
        base: $base,
        compare_url: $compare_url,
        pull_requests: [ $all[] | select(excluded_reason == null) | . + { lead: lead } ]
            | sort_by(.number),
        excluded: [ $all[] | select(excluded_reason != null)
            | { number, title, reason: excluded_reason } ]
            | sort_by(.number)
    }'
