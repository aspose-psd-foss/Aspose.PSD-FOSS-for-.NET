# AGENTS.md

## C# and API Rules

- Each class must live in its own module/file.
- Keep one clear responsibility per type.
- Prefer small, explicit types over broad utility containers.
- Keep public API minimal and stable.
- Use `PascalCase` for types and members, `camelCase` for locals and parameters, and descriptive names over abbreviations.
- Throw specific exceptions for invalid input and malformed PSD/PSB structures.
- Preserve big-endian and PSD/PSB version-specific behavior explicitly; do not hide format differences behind vague helpers.

## File and Module Structure

- Production code lives under `src/Aspose.PSD.FOSS/`.
- Tests live under `src/Aspose.PSD.FOSS.Test/`.
- Samples live under `samples/`.
- Markdown documentation lives under `documentation/`.
- Every class, including internal helper classes, must live in its own file.

## Documentation Rules

- Every public class, enum, method, property, field, and constant must have XML summary documentation.
- Add concise internal summaries where project conventions already expect them.
- Keep README and markdown docs aligned with actual supported scope.
- Document limitations explicitly; do not imply unsupported features are partially available unless tests prove that behavior.

## Testing Expectations

- Use NUnit in `Aspose.PSD.FOSS.Test`.
- Keep acceptance coverage in the real test project, not in demo apps or console utilities.
- Store PSD/PSB fixtures under `src/Aspose.PSD.FOSS.Test/testdata/` when persistent binary fixtures are needed.
- Reference test data via `TestContext.CurrentContext.TestDirectory`.
- Preserve strict no-mutation round-trip tests; do not weaken them to “file is still loadable”.
- Add negative tests for malformed length fields and boundary violations when parser logic changes.
- Add PSB-specific tests whenever touching version-aware lengths or layer/channel handling.

## OpenSpec Workflow

- Keep `openspec/changes/.../tasks.md` in sync with implementation progress.
- Read proposal, design, specs, and tasks before continuing a change.
- Make focused changes that map cleanly to an OpenSpec task.
- If implementation reveals a spec gap or design conflict, update the artifacts instead of silently drifting from them.
- Do not mark tasks complete without verification evidence.

## Change Discipline

- Prefer minimal diffs over broad refactors.
- Do not rename, relocate, or reshape public surface area without a concrete reason and tests.
- Do not introduce rendering-oriented abstractions into this library.
- Do not replace raw-preserve behavior with partial semantic rewriting unless the rewritten contract is covered by tests.
- Keep samples and docs current when user-visible behavior changes.
