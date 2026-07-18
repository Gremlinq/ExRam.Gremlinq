#!/bin/bash

# Prerequisite Checks for prepare-release skill

# 1. Nerdbank.GitVersioning (nbgv) Check
if ! command -v nbgv &> /dev/null; then
    echo "ERROR: Nerdbank.GitVersioning CLI (nbgv) is not installed."
    echo "Install with: dotnet tool install -g nbgv"
    exit 1
fi

# 2. Git Check
if ! command -v git &> /dev/null; then
    echo "ERROR: Git is not installed."
    echo "Install from: https://git-scm.com/downloads"
    exit 1
fi

# 3. Working Directory Check
if [ ! -f "ExRam.Gremlinq.slnx" ] || [ ! -d ".git" ]; then
    echo "ERROR: Not in the root of the ExRam.Gremlinq repository."
    echo "Please navigate to the repository root directory."
    exit 1
fi

# 4. Clean Working Tree Check
if ! git diff --quiet || ! git diff --cached --quiet; then
    echo "ERROR: Working directory has uncommitted changes."
    echo "Status:"
    git status --short
    echo ""
    echo "Please commit or stash all changes before creating a release."
    exit 1
fi

echo "All prerequisite checks passed."