# Expert Architecture Checklist

## Structure

- [ ] Solution has clear project boundaries.
- [ ] API concerns are separated from business rules.
- [ ] EF Core concerns are isolated from controllers.
- [ ] DTOs are used for API input/output.
- [ ] Entities are not exposed directly from endpoints.
- [ ] Configuration is environment-friendly.

## Business rules

- [ ] Event name is required.
- [ ] Ticket type belongs to an event.
- [ ] Ticket type price cannot be negative.
- [ ] Ticket type total quantity is greater than zero.
- [ ] Available quantity initially equals total quantity.
- [ ] Reservation quantity is greater than zero.
- [ ] Reservation requires customer email.
- [ ] Reservation fails if not enough tickets are available.
- [ ] Reservation reduces available quantity.

## Quality

- [ ] Build passes.
- [ ] Tests pass.
- [ ] Coverage command works.
- [ ] SonarQube setup exists.
- [ ] README is accurate.
- [ ] Token tracking is updated.
- [ ] Experiment result is written.

## Security

- [ ] No secrets hardcoded in application code.
- [ ] Errors do not leak internals.
- [ ] Inputs are validated.
- [ ] Logs do not expose sensitive values.
- [ ] Demo security limitations are documented.
