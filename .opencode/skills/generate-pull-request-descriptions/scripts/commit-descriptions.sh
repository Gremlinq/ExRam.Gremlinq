#!/bin/bash

# Commit pull request descriptions to current branch

# Add all pull request descriptions files
git add releases/

# Commit the pull request descriptions to the current branch
git commit -m "Generate pull request descriptions"

echo "Pull request descriptions committed to current branch"