---
tags:
  - git
---
# Conventional Commits
## Status
Approved
## Description
Commits made to this repository must follow a conventional commit standard.
## Decision
The convention is not set in stone, however at minimum a commit subject **must** be in the following format:

```
<commit-type>([scope])(! to indicate if breaking):<single spae><commit subject>
[commit message]
[footer]
```

`<commit-type>` should be short at discretion, eg, features use `feat`, bugfixes would be `fix`, documentation would be `docs`. For other common types such as `chore`, `test` and similarly terse types they can remain as is.

`[scope]` is optional, and if present **must** be surrounded in parenthesis. It **should** be `lower-kebab-case` and kept short and terse. Common scopes may exist and should be reused as much as possible.

If the change is breaking, the commit type and scope must have an `!` at the end. On merging to default or release branches, if any commit in a tree contains a breaking commit, the final merge commit must carry this indicator.

`<commit subject>` is required and must have a single space before the start of it, and the preceding colon.

A commit message and footer are entirely optional, however if a footer is used it is preferred to add additional tag information, covered in the examples below

## Examples
For a feature touching a frontend location for attributes
`feat(frontend): update attribute tag styling`

Extended usage to reduce ambiguity
```
feat(frontend): update attribute tag styling

- updated spacing between tags
- ensure long tags do not wrap lines where possible

tags: attributes, styling
```

## Sources
[https://www.conventionalcommits.org/en/v1.0.0/](https://www.conventionalcommits.org/en/v1.0.0/)