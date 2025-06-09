
# Accidenta – Incident Management System

Accidenta is a modular, event-driven incident management system designed for efficient creation and tracking of incidents associated with customer accounts and contacts. It is built using modern .NET technologies and adheres to Clean Architecture and Domain-Driven Design principles.

## ✨ Features

- **Account & Contact Management**
  - Retrieve accounts by ID or name.
  - Validate account existence using the Specification pattern.
  - Automatically update existing contacts by email.
  - Create new contacts and link them to accounts if they don't exist.

- **Incident Creation Workflow**
  - Ensure account exists; otherwise, return `404 Not Found`.
  - Validate and manage contact records intelligently.
  - Link contacts to accounts only when needed.
  - Create incidents with descriptive metadata.

- **Robust Domain Logic**
  - Business rules encapsulated via Specifications (e.g., `AccountExistsSpecification`).
  - Clear separation of concerns between commands and queries (CQRS).

- **Structured Logging**
  - Uses Serilog to log significant workflow events and warnings.

## 🧰 Tech Stack

- **.NET 7**, **C#**
- **ASP.NET Core Web API**
- **Entity Framework Core** (SQL Server)
- **MediatR** (CQRS implementation)
- **Serilog** (logging)
- **Clean Architecture**
- **Specification Pattern**
- **Repository & Unit of Work Patterns**


## 📌 Example Workflow

1. A `CreateIncidentRequest` is submitted with:
   - Account name
   - Contact email
   - First and last name
   - Incident description

2. The system:
   - Validates the account exists (`404` if not).
   - Retrieves or creates a contact by email.
   - Links the contact to the account if not already linked.
   - Creates an incident and logs the outcome.

## ✅ Future Improvements

- Implement unit and integration tests for `CreateIncidentHandler`
- Add user authentication and authorization
- Extend incident lifecycle tracking (status updates, audit logs, etc.)
