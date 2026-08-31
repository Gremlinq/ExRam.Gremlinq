#!/usr/bin/env bash
#
# Applies the same normalisation and the same threshold as the 'check-description' status
# check to a description held in a file, so a description can be validated before it is
# pushed anywhere.
#
# KEEP IN SYNC with .github/workflows/checkPullRequestDescription.yml. If the perl program
# or MIN_CHARS below diverges from the workflow, this script stops predicting CI.
#
# Usage: check-description.sh <file>

set -euo pipefail

MIN_CHARS=150

[ $# -eq 1 ] || { echo "Usage: $(basename "$0") <file>" >&2; exit 2; }
[ -f "$1" ] || { echo "ERROR: no such file: $1" >&2; exit 2; }

prose="$(
  perl -0777 -pe '
    s/<!--.*?-->//gs;              # HTML comments, including template hints
    s/^```.*?^```//gms;            # fenced code blocks and stack traces
    s/^\s{0,3}#{1,6}\s+//gm;       # heading markers, the heading text is kept
    s/^\s*[-*+]\s+//gm;            # list bullets
    s{https?://\S+}{}g;            # bare links
    s/#\d+//g;                     # issue and pull request references
    s/[`*_>|\[\]()~]//g;           # remaining markdown punctuation
    s/\s+/ /g; s/^ //; s/ $//;     # collapse and trim whitespace
  ' < "$1"
)"

chars="$(printf '%s' "$prose" | LC_ALL=C.UTF-8 wc -m | tr -d ' ')"

if [ "$chars" -lt "$MIN_CHARS" ]; then
    echo "FAIL: $chars characters of prose, minimum $MIN_CHARS." >&2
    echo "The CI check would reject this. Say what changed and why, or use the 'skip-changelog' label." >&2
    exit 1
fi

echo "OK: $chars characters of prose (minimum $MIN_CHARS)."
