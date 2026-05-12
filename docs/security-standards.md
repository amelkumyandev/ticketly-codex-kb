# Ticketly Security Standards

## Purpose

This document defines minimal security standards for the Ticketly demo.

The project is intentionally small and does not include authentication or payment processing, but it should avoid obvious security mistakes.

## Scope

In scope: input validation, safe error responses, safe configuration, safe logging, basic dependency hygiene, and avoiding secret leakage.

Out of scope: user accounts, authentication, authorization, payment security, PCI compliance, email verification, rate limiting, and full production hardening.

## Input validation

Validate required strings, positive quantities, non-negative prices, existing parent resources, and reservation availability. Do not rely only on database constraints.

## Error handling

Do not return stack traces, connection strings, environment variables, full exception dumps, or internal class names when avoidable.

## Secrets and configuration

Local Docker Compose may use demo credentials. Do not hardcode secrets inside application code. Use environment variables for Docker Compose.

## Logging

Do not log database passwords, SonarQube tokens, full connection strings, or sensitive customer information. Customer email exists in the demo model, but avoid unnecessary logging of it.

## Database

Use EF Core parameterized queries by default. Avoid raw SQL unless necessary. If raw SQL is used, parameterize it.

## SonarQube security review

SonarQube should be used to check vulnerabilities, security hotspots, code smells, duplications, and reliability issues. Record results in `EXPERIMENT_RESULT.md`.

## Dependency hygiene

Avoid unnecessary dependencies. If a dependency is added, it should support PostgreSQL EF Core, testing, coverage, SonarQube analysis, or Swagger/OpenAPI.

## Demo security statement

Add this to README or experiment result:

```text
This project is a demo and intentionally does not implement authentication, authorization, payment handling, or production-grade security hardening.
```
