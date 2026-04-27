<div align="center">

# 🏢 WSC App — Wholesale & Supply Chain ERP

**A modular, microservices-based enterprise backend built with .NET 10 & Clean Architecture**

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-13-239120?style=for-the-badge&logo=csharp&logoColor=white)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![SQL Server](https://img.shields.io/badge/SQL_Server-2022-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)](https://www.microsoft.com/en-us/sql-server)
[![Redis](https://img.shields.io/badge/Redis-Cache-DC382D?style=for-the-badge&logo=redis&logoColor=white)](https://redis.io/)
[![Docker](https://img.shields.io/badge/Docker-Ready-2496ED?style=for-the-badge&logo=docker&logoColor=white)](https://www.docker.com/)
[![License](https://img.shields.io/badge/License-MIT-F7DF1E?style=for-the-badge)](LICENSE)

<br/>

<img src="https://readme-typing-svg.demolab.com?font=Fira+Code&weight=600&size=22&pause=1000&color=512BD4&center=true&vCenter=true&multiline=true&repeat=false&width=700&height=50&lines=CRM+%E2%80%A2+Store+%E2%80%A2+Delivery+%E2%80%A2+Dashboard+%E2%80%A2+Gateway" alt="Modules" />

<br/>

[Architecture](#-architecture) · [Modules](#-service-modules) · [Tech Stack](#-tech-stack) · [Getting Started](#-getting-started) · [API Docs](#-api-documentation) · [Roadmap](#-roadmap)

</div>

---

## 📖 About

**WSC App** is a backend-focused enterprise application that simulates a real-world **B2B Wholesale & Supply Chain** platform. It is built as a learning project to deeply explore **.NET microservices**, **Clean Architecture**, and production-grade backend patterns.

The system is composed of **5 independent service modules** + a **shared contract library**, each following a strict 4-layer Clean Architecture:

```
Client (HTTP) → API Layer → Application Layer → Infrastructure Layer → SQL Server
```

> **Key Highlights:**
> - Each business domain runs as a separate API service with its own database schema
> - Typed HTTP clients enable inter-service communication
> - Redis is used for caching, idempotency, and token revocation
> - Serilog provides structured logging with correlation ID propagation
> - JWT-based authentication with token blocklist via Redis

---

## 🏗 Architecture

### Clean Architecture — 4 Layers Per Module

Every module follows the same layer separation, ensuring **no upward dependencies** and clear boundaries:

```
┌──────────────────────────────────────────────────────┐
│  WSC.[Module].API            ← Controllers, Middleware, Filters       │
│    ↑                                                                   │
│  WSC.[Module].Infrastructure ← Repositories, Dapper, Redis, DI        │
│    ↑                                                                   │
│  WSC.[Module].Application    ← Services, Interfaces, DTOs, Validators │
│    ↑                                                                   │
│  WSC.[Module].Domain         ← Entities, Enums, Value Objects (no deps)│
└──────────────────────────────────────────────────────┘
```

### Request Lifecycle

```
HTTP Request
  │
  ├─► Correlation Middleware (X-Correlation-ID)
  ├─► Idempotency Middleware (Redis key check)
  ├─► JWT Revocation Middleware (token blocklist)
  ├─► Validation Filter (FluentValidation)
  ├─► Controller → Service Layer (business logic)
  ├─► Repository (Dapper + SQL / Stored Procedures)
  └─► ApiResponse<T> (standardised response envelope)
```

### Database Schema Strategy

All modules share a single **`WSC_App`** SQL Server database but are isolated by **named schemas**:

| Schema | Module | Description |
|--------|--------|-------------|
| `crm.*` | WSC.CRM | Customer relationship management tables |
| `store.*` | WSC.Store | Products, orders, inventory tables |
| `delivery.*` | WSC.Delivery | Deliveries, agents, tracking tables |

### Inter-Service Communication

```
ICustomerClient  →  CustomerClient  →  GET crm.Customers/{id}
IProductClient   →  ProductClient   →  GET store.Products/GetById/{id}
IOrderClient     →  OrderClient     →  GET store.Orders/order/{id}
```

> Typed HTTP clients are defined in `WSC.Shared.Contracts` (interfaces) and `WSC.Shared.Infrastructure` (implementations), registered per consuming service.

---

## 📦 Service Modules

<table>
<tr>
<td width="50%">

### 🔵 WSC.CRM
Customer Relationship Management — manages **Customers**, **Leads**, **Opportunities**, and **Activities**. Includes idempotency middleware, paging, and Redis cache integration.

`Customers` · `Leads` · `Opportunities` · `Activities`

</td>
<td width="50%">

### 🟢 WSC.Store
E-Commerce & Inventory — handles **Products**, **Orders**, **OrderItems**, and **Inventory**. Uses UnitOfWork + transactions for atomic stock reduction on order creation.

`Products` · `Orders` · `Inventory` · `UnitOfWork`

</td>
</tr>
<tr>
<td width="50%">

### 🟣 WSC.Delivery
Logistics & Tracking — tracks **Deliveries**, **Agents**, **DeliveryItems**, and **TrackingHistory**. Integrates with CRM and Store via typed HTTP clients.

`Agents` · `Deliveries` · `Tracking` · `Items`

</td>
<td width="50%">

### 🟡 WSC.Dashboard
Aggregation Layer — provides a unified read-only view via a complex **5-table multi-map Dapper query**: Customer → Orders → Items → Delivery → Agent.

`Multi-join Query` · `Read-only View` · `Redis Connected`

</td>
</tr>
<tr>
<td width="50%">

### 🔴 WSC.Gateway
API Gateway — central entry point with **JWT authentication**, user registration/login, token revocation, and typed HttpClient routing to all downstream services.

`JWT Auth` · `HttpClient` · `CustomerClient` · `Aggregator`

</td>
<td width="50%">

### ⚪ WSC.Shared
Shared Contract Library — common **contracts**, **DTOs**, **exceptions**, **enums**, **value objects**, and infrastructure helpers (logging, clients, idempotency, JWT services).

`ApiResponse<T>` · `Exceptions` · `Clients` · `JWT`

</td>
</tr>
</table>

---

## 🛠 Tech Stack

<table>
<tr><th>Category</th><th>Technology</th><th>Version</th></tr>
<tr><td><b>Runtime</b></td><td>.NET / ASP.NET Core</td><td>10.0</td></tr>
<tr><td><b>Language</b></td><td>C#</td><td>13</td></tr>
<tr><td rowspan="4"><b>Data Access</b></td><td>Dapper</td><td>2.1.72</td></tr>
<tr><td>Microsoft.Data.SqlClient</td><td>7.0.0</td></tr>
<tr><td>StackExchange.Redis</td><td>2.12.14</td></tr>
<tr><td>EF Core (Gateway auth)</td><td>10.0.6</td></tr>
<tr><td rowspan="5"><b>API & Security</b></td><td>FluentValidation</td><td>12.1.1</td></tr>
<tr><td>AutoMapper</td><td>16.1.1</td></tr>
<tr><td>AspNetCoreRateLimit</td><td>5.0.0</td></tr>
<tr><td>JWT Bearer Authentication</td><td>10.0.6</td></tr>
<tr><td>BCrypt.Net-Next</td><td>4.1.0</td></tr>
<tr><td rowspan="3"><b>Logging</b></td><td>Serilog (Console + File + Seq)</td><td>4.3.1</td></tr>
<tr><td>Serilog.AspNetCore</td><td>10.0.0</td></tr>
<tr><td>Serilog.Sinks.Seq</td><td>9.0.0</td></tr>
<tr><td rowspan="2"><b>API Docs</b></td><td>Scalar.AspNetCore</td><td>2.13.22</td></tr>
<tr><td>Swashbuckle (Swagger)</td><td>10.1.7</td></tr>
<tr><td><b>Containerization</b></td><td>Docker</td><td>Multi-stage builds</td></tr>
</table>

---

## 🧩 Design Patterns

| Pattern | Usage |
|---------|-------|
| **Repository** | Dedicated interface per entity in Application; Dapper-backed implementation in Infrastructure |
| **Unit of Work** | `IUnitOfWork` wraps `IDbConnection` + `IDbTransaction` for atomic multi-repo writes |
| **Service Layer** | Business logic in Application services — controllers stay thin |
| **Middleware Pipeline** | `ExceptionMiddleware`, `IdempotencyMiddleware`, `CorrelationMiddleware`, `JwtRevocationMiddleware` |
| **Validation Filter** | `IAsyncActionFilter` resolves FluentValidation validators and short-circuits with 400s |
| **Typed HTTP Clients** | `IHttpClientFactory` with typed clients for cross-service communication |
| **Cache-Aside (Redis)** | Check Redis → fallback to DB → populate cache with TTL |
| **Shared Contracts** | `ApiResponse<T>`, `PagedResponse<T>`, shared DTOs and exception types |
| **AutoMapper Profiles** | Per-module mapping profiles for Entity ↔ DTO conversions |

---

## 🗂 Project Structure

```
WSC_App/
│
├── WSC.CRM/                          # CRM Microservice
│   ├── WSC.CRM.Domain/               #   Entities, Enums
│   ├── WSC.CRM.Application/          #   Services, DTOs, Validators, Interfaces
│   ├── WSC.CRM.Infrastructure/       #   Repositories, Dapper, Redis, DI
│   └── WSC.CRM.API/                  #   Controllers, Middleware, Program.cs
│
├── WSC.Store/                         # Store Microservice
│   ├── WSC.Store.Domain/
│   ├── WSC.Store.Application/
│   ├── WSC.Store.Infrastructure/
│   └── WSC.Store.API/
│
├── WSC.Delivery/                      # Delivery Microservice
│   ├── WSC.Delivery.Domain/
│   ├── WSC.Delivery.Application/
│   ├── WSC.Delivery.Infrastructure/
│   └── WSC.Delivery.API/
│
├── WSC.Dashboard/                     # Dashboard Aggregation Service
│   ├── WSC.Dashboard.Domain/
│   ├── WSC.Dashboard.Application/
│   ├── WSC.Dashboard.Infrastructure/
│   └── WSC.Dashboard.API/
│
├── WSC.Gateway/                       # API Gateway + Auth
│   └── WSC.Gateway.API/
├── WSC.Gateway.Domain/
├── WSC.Gateway.Application/
├── WSC.Gateway.Infrastructure/
│
├── WSC.Shared/                        # Shared Libraries
│   ├── WSC.Shared.Contracts/          #   Interfaces, DTOs, Exceptions, Enums
│   └── WSC.Shared.Infrastructure/     #   HTTP Clients, Redis, JWT, Logging
│
└── WSC_App.slnx                       # Solution file
```

---

## 🚀 Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [SQL Server 2022](https://www.microsoft.com/en-us/sql-server) (LocalDB or full instance)
- [Redis](https://redis.io/download/) (local or Docker)
- [Docker](https://www.docker.com/) (optional, for containerized runs)

### Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/KarthikSai08/WSC_ERP_App.git
   cd WSC_ERP_App
   ```

2. **Configure connection strings**
   
   Update `appsettings.json` in each API project, or use [User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets):
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=.;Database=WSC_App;Trusted_Connection=True;TrustServerCertificate=True;"
     },
     "Redis": {
       "ConnectionString": "localhost:6379"
     }
   }
   ```

3. **Run Redis** (via Docker)
   ```bash
   docker run -d --name redis -p 6379:6379 redis:latest
   ```

4. **Run the services**
   
   Each module runs independently. Use the multi-project launch or start individually:
   ```bash
   # Run a specific service
   dotnet run --project WSC.CRM/WSC.CRM.API

   # Or run all services (via Visual Studio multi-startup)
   ```

5. **Explore the APIs**
   
   Each service exposes Swagger UI and Scalar docs at:
   - Swagger: `https://localhost:{port}/swagger`
   - Scalar: `https://localhost:{port}/scalar/v1`

---

## 📚 API Documentation

Each module exposes RESTful endpoints. Here's a summary:

| Module | Endpoints | Description |
|--------|-----------|-------------|
| **CRM** | `/api/Customers`, `/api/Leads`, `/api/Activities`, `/api/Opportunities` | Full CRUD with paging |
| **Store** | `/api/Products`, `/api/Orders`, `/api/OrderItems`, `/api/Inventory` | Orders with transactional items |
| **Delivery** | `/api/DeliveryAgents`, `/api/DeliveryOrders`, `/api/DeliveryItems`, `/api/TrackingHistory` | Full delivery lifecycle |
| **Dashboard** | `/api/Dashboard` | Aggregated cross-module view |
| **Gateway** | `/api/Auth/register`, `/api/Auth/login`, `/api/Auth/logout` | JWT auth flow |

> All responses use a unified `ApiResponse<T>` envelope with `Success`, `Message`, and `Data` fields.

---

## 📊 Module Status

| Feature | Module | Status |
|---------|--------|--------|
| CRUD — Customers, Leads, Activities, Opportunities | CRM | ✅ Complete |
| Paged responses with multi-query | CRM / Store | ✅ Complete |
| UnitOfWork + transactional order items | Store | ✅ Complete |
| Redis caching (orders) | Store | ✅ Complete |
| Delivery CRUD — Agents, Orders, Items, Tracking | Delivery | ✅ Complete |
| Dashboard aggregate query | Dashboard | ✅ Complete |
| Gateway JWT Auth & User Registration | Gateway | ✅ Complete |
| ExceptionMiddleware | All | ✅ Complete |
| CorrelationMiddleware + Serilog | All | ✅ Complete |
| Redis caching in CRM | CRM | 🟡 Wired, not active |
| IdempotencyMiddleware | CRM | 🟡 Partial |
| Docker Compose — full stack | Infra | 🟡 Partial |
| JWT [Authorize] on all controllers | All | 🔲 Pending |
| Rate Limiting configuration | All | 🔲 Pending |
| Health Check Endpoints | All | 🔲 Pending |
| Unit / Integration Tests | All | 🔲 Pending |

---

## 🗺 Roadmap

- [ ] 🔐 Add `[Authorize]` attribute to all service controllers
- [ ] 🛡️ Configure AspNetCoreRateLimit across all modules
- [ ] 💾 Activate Redis caching in CRM services
- [ ] 🔧 Fix inverted IdempotencyMiddleware logic
- [ ] 🏥 Add `/health` endpoints with SQL + Redis probes
- [ ] 🧪 Add unit tests (xUnit + Moq) and integration tests
- [ ] 🐳 Containerize all remaining services (Store, Delivery, Dashboard, Gateway)
- [ ] 📝 Extract magic strings to static constants
- [ ] 🔒 Move connection strings to User Secrets / Azure Key Vault
- [ ] 🕵️ Add audit fields (`UpdatedAt`, `DeletedAt`) to CRM entities

---

## 🧠 What I Learned

This project was a hands-on deep dive into building enterprise .NET applications. Key learnings include:

- **Clean Architecture** — enforcing strict layer boundaries and dependency inversion
- **Dapper** — raw SQL, stored procedures, multi-map queries, and output parameters
- **Redis** — cache-aside pattern, idempotency keys, and JWT token blocklists
- **Middleware Pipeline** — custom middleware for cross-cutting concerns
- **FluentValidation** — centralized DTO validation with action filters
- **Inter-Service Communication** — typed HTTP clients with `IHttpClientFactory`
- **Serilog** — structured logging with correlation IDs and Seq integration
- **Unit of Work** — atomic transactions without EF Core overhead
- **JWT Auth** — token generation, BCrypt hashing, and Redis-based revocation
- **Docker** — multi-stage builds and docker-compose for local development

---

## 📄 License

This project is open source and available under the [MIT License](LICENSE).

---

<div align="center">

**Built with ❤️ using .NET 10 · Clean Architecture · Microservices**

*⭐ Star this repo if you found it helpful!*

</div>
