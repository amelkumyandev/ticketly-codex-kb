# Experiment Comparison Summary Guide

## Purpose

This document defines how to compare `ticketly-plain` with `ticketly-kb`.

## Main hypothesis

The knowledge-base repository may use more context tokens, but should produce more consistent architecture, better separation of concerns, better tests, better documentation, fewer manual fixes, and better SonarQube results.

## Required comparison metrics

| Metric | ticketly-plain | ticketly-kb | Winner | Notes |
|---|---:|---:|---|---|
| Build passed | | | | |
| Tests passed | | | | |
| Number of tests | | | | |
| Coverage percentage | | | | |
| SonarQube bugs | | | | |
| SonarQube vulnerabilities | | | | |
| SonarQube security hotspots | | | | |
| SonarQube code smells | | | | |
| Duplicated lines percentage | | | | |
| Manual fixes required | | | | |
| Architecture consistency | | | | |
| API consistency | | | | |
| Security hygiene | | | | |
| Documentation quality | | | | |
| Input tokens | | | | |
| Output tokens | | | | |
| Total tokens | | | | |

## Qualitative scoring

Use this scale: 1 = Poor, 2 = Weak, 3 = Acceptable, 4 = Good, 5 = Excellent.

Score architecture consistency, coding standards, QA quality, security hygiene, documentation quality, and maintainability.

## Suggested final interpretation

```markdown
## Result

The knowledge-base repository used [more/less/about the same] tokens compared with the plain repository.

However, it produced:
- better architecture consistency
- better test quality
- clearer documentation
- fewer manual fixes
- better/worse/similar SonarQube result

## Conclusion

The experiment suggests that context is not free, but it can reduce rework and improve engineering consistency.
```

## Important note

Do not overclaim. This is a small demo experiment, not a scientific benchmark. Use wording like "In this demo" and "The result suggests".
