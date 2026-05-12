# Ticketly QA Standards

## Purpose

This document defines quality assurance standards for the Ticketly knowledge-base experiment.

## Test framework

Use xUnit. FluentAssertions is acceptable if available or worth adding. ASP.NET Core test host or PostgreSQL-backed tests are acceptable if practical.

## Required tests

At minimum, cover:

1. Creating an event succeeds with valid data.
2. Creating a ticket type succeeds for an existing event.
3. Ticket type `AvailableQuantity` initially equals `TotalQuantity`.
4. Creating a reservation succeeds when enough tickets are available.
5. Creating a reservation reduces `AvailableQuantity`.
6. Creating a reservation fails when not enough tickets are available.
7. Creating a reservation fails when quantity is zero or negative.

## Test naming

Use readable names such as `CreateReservation_WhenEnoughTicketsAvailable_ReducesAvailableQuantity()`.

Avoid `Test1()`, `ShouldWork()`, and `ReservationTest()`.

## Coverage

Add coverage support with `coverlet.collector` or `coverlet.msbuild`.

The repository must provide a Windows-friendly script:

```text
scripts/run-tests-with-coverage.ps1
```

Preferred coverage output:

```text
TestResults/coverage.opencover.xml
```

## Quality gates for demo

The demo target is not 100% coverage. Acceptance: build passes, tests pass, required business behaviors are tested, coverage report is generated, SonarQube can consume coverage, and missing coverage is documented.

## Manual QA checklist

Verify health endpoint, event creation, event listing, event by id, ticket type creation, reservation creation, availability reduction, insufficient availability failure, Docker PostgreSQL startup, and README accuracy.

## Experiment comparison

Record number of tests, test pass/fail, coverage percentage if available, manual fixes, build errors, test errors, and SonarQube issues.
