# EXPERIMENT_RESULT.md

Repository: `ticketly-kb`  
Experiment type: With knowledge base

## Summary

This repository implements the Ticketly demo API using the repository knowledge base.

The project should be compared with `ticketly-plain`.

## What was built

- [x] .NET 10 Web API
- [x] PostgreSQL database through Docker Compose
- [x] EF Core persistence
- [x] Event creation/listing
- [x] Ticket type creation/listing
- [x] Ticket reservation
- [x] xUnit tests
- [x] Test coverage
- [x] SonarQube local analysis setup
- [x] Token-burn tracking
- [x] Experiment summary

## Knowledge Base Influence

- `AGENTS.md`: defined the required scope, out-of-scope features, layered architecture, required commands, and token tracking rules.
- `docs/architecture.md`: drove the `Api`, `Application`, `Domain`, and `Infrastructure` project boundaries.
- `docs/coding-standards.md`: kept business rules out of endpoints, enabled nullable reference types, used async APIs, DTOs, and small services.
- `docs/api-guidelines.md`: guided endpoint names, status codes, DTO boundaries, and error response shape.
- `docs/qa-standards.md`: defined the required business behavior tests and OpenCover output path.
- `docs/security-standards.md`: kept authentication/payments out of scope, avoided source-code secrets, and used controlled error messages.
- `docs/sonarqube-standards.md`: guided local SonarQube Compose support, scanner script, and coverage report pattern.
- `docs/token-burn-tracking.md`: defined estimated local token tracking because exact platform usage was unavailable.
- `docs/comparison-summary.md`: shaped the final comparison placeholders and restrained conclusion.

## Build and Test Result

| Metric | Result |
|---|---|
| Build passed | Yes, final `dotnet build` passed with 0 warnings and 0 errors. |
| Tests passed | Yes, final `dotnet test` passed. |
| Number of tests | 8 business behavior tests |
| Coverage percentage | 22.83% line coverage |
| Coverage report path | `TestResults/coverage.opencover.xml` |

## SonarQube Result

| Metric | Result |
|---|---|
| Bugs | Not available; analysis not run because no SonarQube token was available. |
| Vulnerabilities | Not available; analysis not run because no SonarQube token was available. |
| Security hotspots | Not available; analysis not run because no SonarQube token was available. |
| Code smells | Not available; analysis not run because no SonarQube token was available. |
| Duplicated lines percentage | Not available; analysis not run because no SonarQube token was available. |
| Maintainability rating | Not available; analysis not run because no SonarQube token was available. |
| Reliability rating | Not available; analysis not run because no SonarQube token was available. |
| Security rating | Not available; analysis not run because no SonarQube token was available. |

SonarQube setup exists in `docker-compose.sonarqube.yml`, and analysis can be run after creating a token:

```powershell
docker compose -f docker-compose.sonarqube.yml up -d
dotnet tool restore
powershell -ExecutionPolicy Bypass -File .\scripts\run-sonarqube-analysis.ps1 -Token "YOUR_TOKEN_HERE"
```

## Token-Burn Result

| Metric | Result |
|---|---|
| Tracking method | estimated local usage |
| Input tokens | 1361 cumulative estimate across task sections |
| Output tokens | 2355 cumulative estimate across task sections |
| Total tokens | 3716 cumulative estimate across task sections |
| Notes | Exact billing tokens were not available in my local environment, so I used a repeatable local approximation: characters divided by four. This is not a billing measurement, but it gives a consistent comparison between the plain repository and the knowledge-base repository. |

## Manual Fixes Required

- Task 0: Retried the coverage script with `powershell -ExecutionPolicy Bypass -File ...` because direct script invocation was blocked by local execution policy.
- Task 0: Fixed the token estimator regex so full markdown task sections are counted.
- Task 1: Re-added package references sequentially after parallel `dotnet add package` calls overwrote part of the infrastructure project file.
- Task 1: Changed the design-time DbContext factory fallback connection string to avoid a hardcoded password in source code.
- Task 2: Retried the coverage script with process-level execution-policy bypass.
- Task 3: Did not run SonarQube analysis because no local SonarQube token was available; exact local commands were documented.
- Task 4: Retried token and coverage scripts with process-level execution-policy bypass.

## Assumptions

- This is a demo project, not a production ticketing system.
- PostgreSQL local demo credentials in Docker Compose are acceptable for local development.
- Exact platform token counts were unavailable, so local estimation is used consistently.
- SonarQube analysis requires a user-created local token.

## Known Limitations

- No authentication.
- No authorization.
- No payments.
- No frontend.
- No email notifications.
- No reservation expiration, confirmation, or cancellation.
- No production hardening.
- SonarQube metrics are unavailable until local analysis is run with a token.
- Token count is estimated, not billing-accurate.

## Comparison With Plain Repository

| Metric | ticketly-plain | ticketly-kb | Winner | Notes |
|---|---:|---:|---|---|
| Build passed | | Yes | | Placeholder for plain repo comparison |
| Tests passed | | Yes | | Placeholder for plain repo comparison |
| Number of tests | | 8 | | Business behavior tests |
| Coverage percentage | | 22.83% | | OpenCover report generated |
| SonarQube bugs | | Not run | | Requires local token |
| SonarQube vulnerabilities | | Not run | | Requires local token |
| SonarQube security hotspots | | Not run | | Requires local token |
| SonarQube code smells | | Not run | | Requires local token |
| Duplicated lines percentage | | Not run | | Requires local token |
| Manual fixes required | | 7 recorded items | | Includes environment workarounds |
| Architecture consistency | | 4 | | Layered architecture implemented |
| API consistency | | 4 | | Required endpoints and status mapping implemented |
| Security hygiene | | 4 | | No auth by design; avoids source-code secrets |
| Documentation quality | | 4 | | README and experiment files updated |
| Input tokens | | 1361 | | Estimated local usage |
| Output tokens | | 2355 | | Estimated local usage |
| Total tokens | | 3716 | | Estimated local usage |

## Final Conclusion

In this demo, the knowledge-base repository produced a complete scoped Ticketly API with layered architecture, PostgreSQL EF Core persistence, behavior tests, coverage output, local SonarQube setup, and tracking artifacts.

The result suggests that the extra context helped keep the implementation aligned with architecture, QA, security, documentation, and experiment-tracking expectations. The tradeoff is higher context and process overhead, which is captured through estimated local token tracking.
