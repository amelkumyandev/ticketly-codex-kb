# Token-Burn Tracking Standard

## Purpose

This document defines how the knowledge-base repository tracks token burn so it can be compared with `ticketly-plain`.

## Important limitation

Exact token usage is available only if the platform exposes it. If exact token usage is not available, use the local estimate method. This is acceptable as long as both repositories use the same method.

## Required files

Maintain `TOKEN_BURN.md`, `EXPERIMENT_LOG.md`, `EXPERIMENT_RESULT.md`, and `scripts/estimate-token-burn.ps1`.

## Required tracking method

For each task, save the task prompt as input text, save the assistant implementation summary as output text, record files created/modified, record commands executed, record build/test/coverage/SonarQube results, record exact token usage if available, and otherwise run the estimate script.

## Exact usage

If exact usage is available, record input tokens, output tokens, cached tokens if available, total tokens, and model name if available. Tracking method should be `exact platform usage`.

## Estimated usage

If exact usage is unavailable, estimate with `estimated tokens = character count / 4`. Tracking method should be `estimated local usage`.

## TOKEN_BURN.md table

```markdown
| Task | Tracking Method | Input Tokens | Output Tokens | Total Tokens | Notes |
|---|---|---:|---:|---:|---|
```

## Input/output sections

Use sections like:

```markdown
## Task 1 Input
<task prompt here>

## Task 1 Output Summary
<summary of files changed and commands run here>
```

## PowerShell estimator requirements

The estimator should read `TOKEN_BURN.md`, count characters under input sections, count characters under output sections, estimate tokens as `Ceiling(characters / 4)`, and print input, output, and total estimates.

## Final comparison

Record tracking method, input tokens, output tokens, total tokens, and notes about accuracy in `EXPERIMENT_RESULT.md`.

## Talk explanation

Use this wording:

```text
Exact billing tokens were not available in my local environment, so I used a repeatable local approximation: characters divided by four. This is not a billing measurement, but it gives a consistent comparison between the plain repository and the knowledge-base repository.
```
