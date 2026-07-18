# Summary and Title Requirements

## Summary Requirements

- MUST be a proper narrative paragraph, NOT bullet points
- Start with a clear statement of purpose
- Mention specific files changed and types of changes ONLY if relevant
- Include technical details from the code
- Categorize changes (feature, bug fix, refactoring, tests, etc.)
- Be concise but informative

## Title Requirements

- Take ALL commit messages into account
- Make it concise and descriptive
- Capitalize properly
- Remove trailing punctuation

## Example

**Input:**
```json
{
  "pr_number": "123",
  "title": "",
  "node_id": "PR_kwDO...",
  "commit_messages": "Add async support for GremlinServer\nFix null reference in VertexSerializer",
  "code_changes": "Commit: abc123\n src/Providers.GremlinServer/GremlinServerQueryExecutorAsync.cs | 150 ++++++\n test/Providers.GremlinServer.Tests/AsyncTests.cs | 200 +++++++\n 2 files changed, 350 insertions(+)"
}
```

**Good Output:**
- Title: "Add async query execution support for GremlinServer provider"
- Summary: "This pull request adds comprehensive async query execution support for the GremlinServer provider. A new `GremlinServerQueryExecutorAsync` class has been introduced with 150 lines of code, implementing async versions of all query methods. Corresponding tests have been added in `AsyncTests.cs` with 200 lines of test coverage. The implementation maintains parity with the existing sync executor while following proper async/await patterns throughout."

**Bad Output:**
- Title: "false"
- Summary: "- Add async support\n- Fix null reference"
