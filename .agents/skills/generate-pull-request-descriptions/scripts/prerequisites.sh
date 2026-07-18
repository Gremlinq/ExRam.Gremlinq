#!/bin/bash

# Prerequisite Checks for generate-pull-request-descriptions skill

# 1. GitHub CLI (gh) Check
if ! command -v gh &> /dev/null; then
    echo "ERROR: GitHub CLI (gh) is not installed."
    echo "Installation instructions: https://cli.github.com/manual/installation"
    exit 1
fi

if ! gh auth status &> /dev/null; then
    echo "ERROR: GitHub CLI is not authenticated."
    echo "Run: gh auth login"
    exit 1
fi

# 2. jq Check
if ! command -v jq &> /dev/null; then
    echo "ERROR: jq is not installed."
    echo "Installation instructions:"
    echo "  Ubuntu/Debian: sudo apt-get install jq"
    echo "  macOS: brew install jq"
    echo "  Windows: choco install jq or winget install jqlang.jq"
    exit 1
fi

# 3. Git Check
if ! command -v git &> /dev/null; then
    echo "ERROR: Git is not installed."
    echo "Installation instructions: https://git-scm.com/downloads"
    exit 1
fi

# 4. Working Directory Check
if [ ! -f "ExRam.Gremlinq.slnx" ] || [ ! -d ".git" ]; then
    echo "ERROR: Not in the root of the ExRam.Gremlinq repository."
    echo "Please navigate to the repository root directory."
    exit 1
fi

# 5. Clean Working Tree Check
# Only check for staged or modified files, ignore untracked files
if ! git diff --quiet || ! git diff --cached --quiet; then
    echo "ERROR: Working directory has uncommitted changes."
    echo "Status:"
    git status --short
    echo ""
    echo "Please commit or stash all changes before generating pull request descriptions."
    exit 1
fi

# 6. GitHub Authentication Token Check
if [ -z "$(gh auth status --show-token 2>/dev/null)" ]; then
    echo "ERROR: GitHub CLI token not available or expired."
    echo "Run: gh auth login --with-token"
    echo "Or: gh auth refresh -h github.com"
    exit 1
fi

# 7. GitHub Remote Check
# Dynamically determine the Git remote that points to github.com
github_remote=$(git remote -v | grep github.com | head -1 | awk '{print $1}')

if [ -z "$github_remote" ]; then
    echo "ERROR: No Git remote pointing to github.com found."
    echo "Available remotes:"
    git remote -v
    exit 1
fi

echo "All prerequisite checks passed."