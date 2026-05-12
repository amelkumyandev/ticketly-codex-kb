# EXPERIMENT_LOG.md

Repository: `ticketly-kb`  
Experiment type: With knowledge base

## Task log

| Task | Name | Start | Finish | Commands Run | Build | Tests | Coverage | SonarQube | Notes |
|---|---|---|---|---|---|---|---|---|---|
| 0 | Initialize solution using knowledge base | 2026-05-11 19:52 +04:00 | 2026-05-11 20:06 +04:00 | `dotnet --info`; `dotnet new sln --name Ticketly`; `dotnet new webapi --framework net10.0`; `dotnet new classlib --framework net10.0`; `dotnet new xunit --framework net10.0`; `dotnet sln Ticketly.slnx add ...`; `dotnet add ... reference ...`; `dotnet build`; `dotnet test`; `dotnet format`; `.\scripts\run-tests-with-coverage.ps1`; `powershell -ExecutionPolicy Bypass -File .\scripts\run-tests-with-coverage.ps1`; `docker compose config`; `powershell -ExecutionPolicy Bypass -File .\scripts\estimate-token-burn.ps1` | Passed, 0 warnings, 0 errors | Passed, 1 test | Script completed through process-level execution policy bypass; no `coverage.opencover.xml` was generated in Task 0 | Not run; no SonarQube token provided and Task 0 does not require analysis | Created .NET 10 `Ticketly.slnx`, layered project scaffold, health endpoint, Dockerfile, Docker Compose PostgreSQL setup, README, xUnit scaffold, and fixed token estimator section matching. Token estimate: 222 input, 428 output, 650 total. |
| 1 | Implement Ticketly API | 2026-05-11 20:07 +04:00 | 2026-05-11 20:20 +04:00 | `dotnet add ... package ...`; `dotnet add ... reference ...`; `dotnet new tool-manifest`; `dotnet tool install dotnet-ef --version 10.0.2`; `dotnet build`; `dotnet tool run dotnet-ef migrations add InitialCreate ...`; `dotnet test`; `dotnet format`; `powershell -ExecutionPolicy Bypass -File .\scripts\run-tests-with-coverage.ps1`; `docker compose config`; `powershell -ExecutionPolicy Bypass -File .\scripts\estimate-token-burn.ps1 -Task 1` | Passed, 0 warnings, 0 errors | Passed, 8 tests | Coverage script completed through execution-policy bypass, but no `coverage.opencover.xml` was generated | Not run; no SonarQube token provided and Task 1 does not require analysis | Implemented required API endpoints, domain entities, application services, EF Core PostgreSQL infrastructure, initial migration, README/API examples, and business behavior tests. Token estimate: 387 input, 614 output, 1001 total. |
| 2 | Add tests and coverage | 2026-05-11 23:27 +04:00 | 2026-05-11 23:32 +04:00 | `dotnet add tests\Ticketly.Tests\Ticketly.Tests.csproj package coverlet.msbuild --version 6.0.4`; `dotnet build`; `dotnet test`; `.\scripts\run-tests-with-coverage.ps1`; `powershell -ExecutionPolicy Bypass -File .\scripts\run-tests-with-coverage.ps1`; `dotnet format`; final `dotnet build`; final `dotnet test`; `powershell -ExecutionPolicy Bypass -File .\scripts\estimate-token-burn.ps1 -Task 2` | Passed, 0 warnings, 0 errors | Passed, 8 tests | Passed with process-level execution-policy bypass; generated `TestResults\coverage.opencover.xml`; total line coverage 22.83% | Not run; Task 2 does not require SonarQube analysis and no token was provided | Added `coverlet.msbuild`, fixed coverage script generation/validation, aligned SonarQube coverage output prefix, documented coverage workflow in README. Token estimate: 261 input, 386 output, 647 total. |
| 3 | Add SonarQube local analysis | 2026-05-12 00:59 +04:00 | 2026-05-12 01:07 +04:00 | `dotnet tool install dotnet-sonarscanner --version 11.2.1`; `docker compose -f docker-compose.sonarqube.yml config`; `dotnet tool restore`; `dotnet build`; `dotnet test`; `powershell -ExecutionPolicy Bypass -File .\scripts\run-tests-with-coverage.ps1`; `dotnet format`; `powershell -ExecutionPolicy Bypass -File .\scripts\estimate-token-burn.ps1 -Task 3` | Passed, 0 warnings, 0 errors | Passed, 8 tests | Passed with process-level execution-policy bypass; generated `TestResults\coverage.opencover.xml`; total line coverage 22.83% | Not run; no SonarQube token environment variable was available | Added repo-local SonarScanner tool, hardened `run-sonarqube-analysis.ps1`, validated SonarQube compose config, and documented local SonarQube setup/run commands in README. Token estimate: 265 input, 390 output, 655 total. |
| 4 | Final self-review and comparison summary | 2026-05-12 01:08 +04:00 | 2026-05-12 01:11 +04:00 | `dotnet build`; `dotnet test`; `.\scripts\estimate-token-burn.ps1`; `powershell -ExecutionPolicy Bypass -File .\scripts\estimate-token-burn.ps1`; `.\scripts\run-tests-with-coverage.ps1`; `powershell -ExecutionPolicy Bypass -File .\scripts\run-tests-with-coverage.ps1`; SonarQube token environment check; `powershell -ExecutionPolicy Bypass -File .\scripts\estimate-token-burn.ps1 -Task 4` | Passed, 0 warnings, 0 errors | Passed, 8 tests | Passed with process-level execution-policy bypass; generated `TestResults\coverage.opencover.xml`; total line coverage 22.83% | Not run; no SonarQube token environment variable was available | Updated final experiment result, comparison placeholders, limitations, and token summary. Token estimate: 228 input, 421 output, 649 total. |

## Assumptions

- This is a demo project, not a production ticketing system.
- Authentication, authorization, payments, and emails are out of scope.
- PostgreSQL is required.
- SonarQube local analysis is required.
- Token tracking should be exact if available, otherwise estimated consistently.

## Manual fixes

Record manual fixes here.

- Task 0: Retried the coverage script with `powershell -ExecutionPolicy Bypass -File ...` because direct script execution is blocked by the local PowerShell execution policy.
- Task 0: Fixed `scripts/estimate-token-burn.ps1` because the original regex used multiline `$`, which caused local estimates to stop at section heading line endings.
- Task 1: Re-added missing package references sequentially after parallel `dotnet add package` calls overwrote parts of `Ticketly.Infrastructure.csproj`.
- Task 1: Changed the design-time DbContext factory fallback connection string to avoid a hardcoded password in source code.
- Task 2: Retried the coverage script with `powershell -ExecutionPolicy Bypass -File ...` because direct script execution is blocked by the local PowerShell execution policy.
- Task 3: Did not run SonarQube analysis because no local SonarQube token was available; documented exact commands to run once a token is created.
- Task 4: Retried token and coverage scripts with `powershell -ExecutionPolicy Bypass -File ...` because direct script execution is blocked by the local PowerShell execution policy.

## Environment notes

Record local environment limitations here.

- Task 0: Exact platform token counts were not available in the local environment, so local token estimation was used.
- Task 0: The direct command `.\scripts\run-tests-with-coverage.ps1` failed because the script is not digitally signed under the current execution policy.
- Task 1: Exact platform token counts were not available in the local environment, so local token estimation was used.
- Task 1: Coverage script reported success, but no `coverage.opencover.xml` file was found. Coverage tooling is scheduled for Task 2.
- Task 2: Exact platform token counts were not available in the local environment, so local token estimation was used.
- Task 2: Direct `.\scripts\run-tests-with-coverage.ps1` execution remains blocked by local unsigned-script policy; the script succeeds with process-level execution-policy bypass.
- Task 3: Exact platform token counts were not available in the local environment, so local token estimation was used.
- Task 3: No `SONAR_TOKEN` or `SONARQUBE_TOKEN` environment variable was present.
- Task 4: Exact platform token counts were not available in the local environment, so local token estimation was used.
- Task 4: SonarQube analysis was skipped because no `SONAR_TOKEN` or `SONARQUBE_TOKEN` environment variable was present.
