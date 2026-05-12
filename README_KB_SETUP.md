# Ticketly KB Artifact Setup

Copy these files into the root of your `ticketly-kb` repository.

Expected root structure after copy:

```text
ticketly-kb/
  AGENTS.md
  TICKETLY_KB_CODEX_TASKS.txt
  README_KB_SETUP.md
  TOKEN_BURN.md
  EXPERIMENT_LOG.md
  EXPERIMENT_RESULT.md
  docker-compose.sonarqube.yml
  docs/
  scripts/
  .github/
```

## How to use with Codex

Tell Codex:

```text
Read TICKETLY_KB_CODEX_TASKS.txt.

Start with Task 0 only.
Before coding, read AGENTS.md and all docs under /docs.
Implement Task 0, run required commands, update EXPERIMENT_LOG.md and TOKEN_BURN.md, and stop.
Do not continue to the next task until I ask.
```

Then continue:

```text
Continue with Task 1 only from TICKETLY_KB_CODEX_TASKS.txt.
Follow AGENTS.md and /docs.
Update EXPERIMENT_LOG.md and TOKEN_BURN.md.
Stop after the task.
```

Repeat until Task 4.

## Token-burn tracking

If exact token usage is unavailable, Codex must use:

```powershell
.\scripts\estimate-token-burn.ps1
```

The estimate method is `tokens = characters / 4`. This is not billing-accurate. It is a consistent comparison method.
