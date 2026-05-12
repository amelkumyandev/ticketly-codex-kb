# GitHub Copilot Instructions

This repository is a .NET 10 knowledge-base experiment.

Follow the standards in `AGENTS.md` and all files under `docs/`.

When generating code:

- Keep controllers thin.
- Put business logic in services.
- Use DTOs for API input/output.
- Do not expose EF Core entities directly from the API.
- Use PostgreSQL and EF Core.
- Use async methods and CancellationToken where reasonable.
- Add tests for behavior changes.
- Maintain test coverage support.
- Maintain SonarQube support.
- Update experiment and token-burn tracking files.
- Prefer simple, readable code over complex abstractions.
