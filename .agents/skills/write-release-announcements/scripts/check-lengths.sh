#!/usr/bin/env bash
#
# Checks the announcement texts for a release against the limits of their channels.
#
# Usage: check-lengths.sh <releases/VERSION directory>

set -euo pipefail

[ $# -eq 1 ] || { echo "Usage: $(basename "$0") <releases/VERSION directory>" >&2; exit 2; }
dir="$1"
[ -d "$dir" ] || { echo "ERROR: no such directory: $dir" >&2; exit 2; }

status=0

count() { LC_ALL=C.UTF-8 wc -m < "$1" | tr -d ' '; }
words() { wc -w < "$1" | tr -d ' '; }

check() {
    local file="$dir/$1" label="$2" min_words="$3" max_words="$4" max_chars="${5:-}"

    if [ ! -s "$file" ]; then
        echo "MISSING  $label ($file)"
        return
    fi

    local c w note=''
    c="$(count "$file")"
    w="$(words "$file")"

    # Discord rejects a message body over 2000 characters outright, so this one is fatal.
    if [ -n "$max_chars" ] && [ "$c" -gt "$max_chars" ]; then
        note="  <-- OVER THE $max_chars CHARACTER DISCORD LIMIT, WILL BE REJECTED"
        status=1
    elif [ "$w" -lt "$min_words" ] || [ "$w" -gt "$max_words" ]; then
        note="  <-- outside the $min_words-$max_words word guideline"
    fi

    printf '%-22s %5s chars %5s words%s\n' "$label" "$c" "$w" "$note"
}

check 'release-notes.md'      'release notes'     0   100000
check 'linkedin.md'           'LinkedIn'          90  200
check 'discord-tinkerpop.md'  'Discord TinkerPop' 50  120 2000
check 'discord-dotnet.md'     'Discord .NET'      70  160 2000

# LinkedIn renders no markdown, so markup would appear literally in the post. A leading
# "- " is fine and is the recommended marker -- it simply shows up as a dash. Emphasis,
# backticks and headings are not: they show up as asterisks, backticks and hashes.
markup='(\*\*|__|`|^#{1,6} |\]\()'
if [ -s "$dir/linkedin.md" ] && grep -qE "$markup" "$dir/linkedin.md"; then
    echo
    echo "WARNING  linkedin.md contains markdown. LinkedIn renders none of it -- these"
    echo "         characters will show up literally in the post."
    grep -nE "$markup" "$dir/linkedin.md" | sed 's/^/         /'
fi

exit $status
