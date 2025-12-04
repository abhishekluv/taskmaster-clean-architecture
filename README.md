# TaskMaster – Clean Architecture Task Management System

TaskMaster is a real-world Task Management system built with **ASP.NET Core**, **Clean Architecture**, **CQRS**, **EF Core**, and **JWT Authentication**.

> Status: Work in progress. This repository is being developed as a flagship portfolio project to demonstrate real-world .NET architecture, domain modeling, and API design.

---

## Goals

- Showcase **Clean Architecture** in ASP.NET Core
- Demonstrate **CQRS + MediatR** with a real Task Management domain
- Use **EF Core** with proper domain modeling, migrations, and configurations
- Implement **JWT-based authentication** and authorization
- Provide **documentation, ADRs, and diagrams** like a real consultant project

---

## High-Level Architecture (Planned)

- `TaskMaster.Domain` – Core domain entities, value objects, and domain logic
- `TaskMaster.Application` – Application layer with CQRS (commands/queries), DTOs, and interfaces
- `TaskMaster.Infrastructure` – EF Core, database configurations, repositories, and external integrations
- `TaskMaster.WebApi` – ASP.NET Core 9 Web API, endpoints, filters, middleware, and DI setup

---

## Solution Structure (Current)

```text
src/
  TaskMaster.Domain/
  TaskMaster.Application/
  TaskMaster.Infrastructure/
  TaskMaster.WebApi/

tests/
  (to be added)

docs/
  (architecture, domain, diagrams – coming soon)

adr/
  (Architecture Decision Records – coming soon)
