# WSC_ERP_App - Tech Stack Implementation Guide

## 🛠️ Complete Tech Stack

- **Dapper** - Micro-ORM for data access
- **Dynamic Parameters** - Parameterized queries with Dapper
- **Stored Procedures** - Complex database operations
- **Unit of Work Pattern** - Transaction management across repositories
- **Redis Caching** - Cache-Aside Pattern implementation
- **Serilog** - Structured logging
- **Transactions** - ACID compliance with Dapper
- **Clean Architecture** - Layered separation of concerns
- **Idempotency** - Duplicate request prevention
- **Global Exception Middleware** - Centralized error handling
- **JWT Authentication** - Token-based security
- **Rate Limiting** - API abuse prevention
- **API Versioning** - v1, v2 route versioning
- **Auditing** - Track changes and user actions
- **Schema Separation** - dbo, audit, cache schemas in database

---

## 📁 Folder Structure & File Organization

```
WSC_App/
├── WSC.CRM/
│   ├── WSC.CRM.Domain/
│   │   ├── Entities/
│   │   │   ├── Customer.cs
│   │   │   ├── Lead.cs
│   │   │   ├── Opportunity.cs
│   │   │   └── Activity.cs
│   │   ├── ValueObjects/
│   │   │   └── Address.cs
│   │   └── Enums/
│   │       ├── LeadStatus.cs
│   │       ├── OpportunityStatus.cs
│   │       └── ActivityType.cs
│   │
│   ├── WSC.CRM.Application/
│   │   ├── DTOs/
│   │   │   ├── CustomerDto.cs
│   │   │   ├── LeadDto.cs
│   │   │   ├── OpportunityDto.cs
│   │   │   └── ActivityDto.cs
│   │   ├── Services/
│   │   │   ├── ICustomerService.cs
│   │   │   ├── CustomerService.cs
│   │   │   ├── ILeadService.cs
│   │   │   ├── LeadService.cs
│   │   │   ├── IOpportunityService.cs
│   │   │   ├── OpportunityService.cs
│   │   │   ├── IActivityService.cs
│   │   │   └── ActivityService.cs
│   │   ├── Caching/
│   │   │   ├── ICacheService.cs
│   │   │   └── CacheService.cs (Cache-Aside Pattern)
│   │   ├── Idempotency/
│   │   │   ├── IIdempotencyService.cs
│   │   │   └── IdempotencyService.cs
│   │   └── Auditing/
│   │       ├── IAuditService.cs
│   │       └── AuditService.cs
│   │
│   ├── WSC.CRM.Infrastructure/
│   │   ├── Data/
│   │   │   ├── DapperContext.cs
│   │   │   ├── DapperConfiguration.cs
│   │   │   └── StoredProcedures.cs (SP definitions)
│   │   ├── Repositories/
│   │   │   ├── IGenericRepository.cs
│   │   │   ├── GenericRepository.cs
│   │   │   ├── ICustomerRepository.cs
│   │   │   ├── CustomerRepository.cs
│   │   │   ├── ILeadRepository.cs
│   │   │   ├── LeadRepository.cs
│   │   │   ├── IOpportunityRepository.cs
│   │   │   ├── OpportunityRepository.cs
│   │   │   ├── IActivityRepository.cs
│   │   │   └── ActivityRepository.cs
│   │   ├── UnitOfWork/
│   │   │   ├── IUnitOfWork.cs
│   │   │   └── UnitOfWork.cs (Manages all repositories & transactions)
│   │   ├── Caching/
│   │   │   ├── ICacheRepository.cs
│   │   │   └── RedisCacheRepository.cs (Redis implementation)
│   │   ├── Persistence/
│   │   │   ├── TransactionManager.cs (Handles transactions)
│   │   │   └── Migrations/
│   │   │       └── [SQL migration scripts]
│   │   └── Configuration/
│   │       └── DependencyInjection.cs
│   │
│   └── WSC.CRM.API/
│       ├── Controllers/
│       │   ├── v1/
│       │   │   ├── CustomersController.cs
│       │   │   ├── LeadsController.cs
│       │   │   ├── OpportunitiesController.cs
│       │   │   └── ActivitiesController.cs
│       │   └── v2/
│       │       └── [Future versions]
│       ├── Middleware/
│       │   ├── GlobalExceptionMiddleware.cs (Exception handling)
│       │   ├── RateLimitingMiddleware.cs (Rate limiting)
│       │   └── AuthenticationMiddleware.cs (JWT validation)
│       ├── Filters/
│       │   ├── IdempotencyFilter.cs
│       │   └── AuditingFilter.cs
│       └── Program.cs
│
├── WSC.Store/
│   ├── WSC.Store.Domain/
│   ├── WSC.Store.Application/
│   ├── WSC.Store.Infrastructure/
│   └── WSC.Store.API/
│
├── WSC.Delivery/
│   ├── WSC.Delivery.Domain/
│   ├── WSC.Delivery.Application/
│   ├── WSC.Delivery.Infrastructure/
│   └── WSC.Delivery.API/
│
├── WSC.Dashboard/
│   ├── WSC.Dashboard.Domain/
│   ├── WSC.Dashboard.Application/
│   ├── WSC.Dashboard.Infrastructure/
│   └── WSC.Dashboard.API/
│
├── WSC.Shared/
│   ├── WSC.Shared.Contracts/
│   │   ├── Models/
│   │   ├── Interfaces/
│   │   └── Constants/
│   └── WSC.Shared.Infrastructure/
│       ├── Logging/
│       │   ├── ILogger.cs
│       │   └── SerilogLogger.cs (Serilog integration)
│       ├── Security/
│       │   ├── ITokenProvider.cs
│       │   └── JwtTokenProvider.cs (JWT token generation)
│       ├── Auditing/
│       │   ├── IAuditProvider.cs
│       │   └── AuditProvider.cs
│       ├── Exceptions/
│       │   ├── ApiException.cs
│       │   ├── NotFoundException.cs
│       │   ├── ValidationException.cs
│       │   └── UnauthorizedAccessException.cs
│       └── Extensions/
│           ├── ServiceCollectionExtensions.cs
│           └── StringExtensions.cs
│
└── WSC.Gateway/
    └── WSC.Gateway.API/
        ├── Program.cs
        └── Middleware/
            └── [Gateway middleware]
```

---

## 🗄️ Database Schema Separation

```sql
-- Schema Organization
CREATE SCHEMA dbo;          -- Main application data
CREATE SCHEMA audit;        -- Audit logs & history
CREATE SCHEMA cache;        -- Cache metadata (optional)

-- Example: Audit table in audit schema
CREATE TABLE audit.CustomerAudit (
    AuditId INT PRIMARY KEY IDENTITY(1,1),
    CustomerId INT,
    Action NVARCHAR(50),
    ChangedBy NVARCHAR(255),
    ChangedAt DATETIME,
    OldValues NVARCHAR(MAX),
    NewValues NVARCHAR(MAX)
);
```

---

## 🔄 Implementation Workflow

### 1️⃣ Domain Layer (Entities, Value Objects)
```
✓ Define entities (Customer, Lead, Opportunity, Activity)
✓ Create value objects (Address)
✓ Define enums (LeadStatus, OpportunityStatus, ActivityType)
```

### 2️⃣ Infrastructure Layer
```
✓ Set up DapperContext & connection management
✓ Create stored procedures in database
✓ Implement GenericRepository with Dapper
✓ Create entity-specific repositories
✓ Implement Unit of Work pattern
✓ Set up Redis cache repository
✓ Implement transaction management
```

### 3️⃣ Application Layer
```
✓ Create DTOs for each entity
✓ Implement service interfaces & classes
✓ Add caching logic (Cache-Aside Pattern)
✓ Implement idempotency service
✓ Add auditing service
```

### 4️⃣ API Layer
```
✓ Create versioned controllers (v1, v2)
✓ Add global exception middleware
✓ Implement rate limiting middleware
✓ Add JWT authentication middleware
✓ Add auditing & idempotency filters
✓ Configure Serilog logging in Program.cs
```

### 5️⃣ Shared Infrastructure
```
✓ Set up Serilog logging
✓ Create JWT token provider
✓ Implement audit provider
✓ Add custom exception classes
✓ Create service collection extensions
```

---

## 📦 NuGet Packages to Install

```powershell
# Domain Layer
# (No external dependencies)

# Infrastructure Layer
Install-Package Dapper
Install-Package StackExchange.Redis
Install-Package Dapper.Contrib

# Application Layer
# (Uses Infrastructure & Domain)

# API Layer
Install-Package Serilog
Install-Package Serilog.Sinks.Console
Install-Package Serilog.Sinks.File
Install-Package System.IdentityModel.Tokens.Jwt
Install-Package Microsoft.IdentityModel.Tokens
Install-Package Microsoft.AspNetCore.Authentication.JwtBearer

# Shared
Install-Package Microsoft.AspNetCore.RateLimiting
```

---

## 🔒 JWT Authentication Setup

```csharp
// Program.cs
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "your-issuer",
            ValidAudience = "your-audience",
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("your-secret-key"))
        };
    });
```

---

## ⚡ Caching Implementation (Cache-Aside Pattern)

```csharp
public class CacheService : ICacheService
{
    private readonly IDistributedCache _cache;
    private readonly IGenericRepository<T> _repository;

    public async Task<T> GetAsync(string cacheKey, 
        Func<Task<T>> getFromDb)
    {
        // Try to get from cache
        var cachedData = await _cache.GetStringAsync(cacheKey);
        if (!string.IsNullOrEmpty(cachedData))
            return JsonSerializer.Deserialize<T>(cachedData);

        // Cache miss - fetch from database
        var data = await getFromDb();

        // Store in cache
        await _cache.SetStringAsync(cacheKey, 
            JsonSerializer.Serialize(data));

        return data;
    }
}
```

---

## 🔄 Unit of Work Pattern

```csharp
public interface IUnitOfWork : IDisposable
{
    ICustomerRepository Customers { get; }
    ILeadRepository Leads { get; }
    IOpportunityRepository Opportunities { get; }
    IActivityRepository Activities { get; }
    
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}
```

---

## 📊 Auditing Implementation

```csharp
public class AuditService : IAuditService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AuditService> _logger;

    public async Task LogChangeAsync(string entityType, 
        int entityId, string action, string userId, 
        object oldValues, object newValues)
    {
        // Log to audit schema table
        // Also log via Serilog
        _logger.LogInformation(
            "Audit: {Entity} {Action} by {User}", 
            entityType, action, userId);
    }
}
```

---

## ✅ Implementation Checklist

### Phase 1: Foundation
- [ ] Set up Dapper & DapperContext
- [ ] Create stored procedures
- [ ] Implement GenericRepository
- [ ] Set up Unit of Work

### Phase 2: Caching & Performance
- [ ] Configure Redis
- [ ] Implement Cache-Aside Pattern
- [ ] Add cache invalidation strategies

### Phase 3: Security & Logging
- [ ] Configure JWT authentication
- [ ] Set up Serilog
- [ ] Implement global exception middleware
- [ ] Add rate limiting

### Phase 4: Auditing & Idempotency
- [ ] Set up audit schema & tables
- [ ] Implement auditing filters
- [ ] Implement idempotency service
- [ ] Add request deduplication

### Phase 5: API & Versioning
- [ ] Create versioned controllers (v1)
- [ ] Add API versioning middleware
- [ ] Implement all middleware

---

## 🚀 Quick Start Commands

```powershell
# Install packages for Infrastructure
cd WSC.CRM\WSC.CRM.Infrastructure
dotnet add package Dapper
dotnet add package StackExchange.Redis

# Install packages for API
cd WSC.CRM\WSC.CRM.API
dotnet add package Serilog
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Microsoft.AspNetCore.RateLimiting

# Build solution
dotnet build
```

---

## 📝 Notes

- **Dapper** is lightweight and perfect for microservices
- **Redis** provides high-speed caching for frequently accessed data
- **Unit of Work** ensures transaction consistency across repositories
- **Serilog** provides structured logging for better debugging
- **Schema Separation** keeps audit logs isolated from main data
- **JWT** provides stateless authentication suitable for microservices
- **Rate Limiting** protects APIs from abuse

---

**Ready to implement!** Follow the folder structure and use this guide as reference. 🚀
