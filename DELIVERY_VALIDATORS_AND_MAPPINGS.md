# Delivery Module - FluentValidation & AutoMapper Implementation

## Summary
Complete FluentValidation validators and AutoMapper profiles have been created for all Delivery module entities and DTOs, following Clean Architecture patterns and consistent with the existing CRM module implementation.

---

## 📋 FluentValidation Validators Created

### DeliveryAgent Validators
**Location:** `WSC.Delivery\WSC.Delivery.Application\Validators\DeliveryAgentValidators\`

#### CreateDeliveryAgentValidator
- **AgentName**: Required, max 100 characters
- **AgentPhone**: Required, valid phone format (E.164)
- **VehicleNumber**: Required, max 50 characters

#### UpdateDeliveryAgentValidator
- **DeliveryAgentId**: Required, must be > 0
- **AgentName**: Optional, max 100 characters (when provided)
- **AgentPhone**: Optional, valid phone format (when provided)
- **VehicleNumber**: Optional, max 50 characters (when provided)
- **IsAvailable**: Optional boolean flag

---

### DeliveryItem Validators
**Location:** `WSC.Delivery\WSC.Delivery.Application\Validators\DeliveryItemValidators\`

#### CreateDeliveryItemValidator
- **DeliveryId**: Required, must be > 0
- **ProductId**: Required, must be > 0
- **Quantity**: Required, must be > 0

#### UpdateDeliveryItemValidator
- **DeliveryItemId**: Required, must be > 0
- **ProductId**: Optional, must be > 0 (when provided)
- **Quantity**: Optional, must be > 0 (when provided)

---

### OrderDelivery Validators
**Location:** `WSC.Delivery\WSC.Delivery.Application\Validators\OrderDeliveryValidators\`

#### CreateOrderDeliveryValidator
- **OrderId**: Required, must be > 0
- **CustomerId**: Required, must be > 0
- **TrackingNumber**: Required, max 100 characters
- **ScheduledDate**: Required, must be in the future
- **DeliveryAddress**: Required, max 500 characters

#### UpdateOrderDeliveryValidator
- **DeliveryId**: Required, must be > 0
- **TrackingNumber**: Optional, max 100 characters (when provided)
- **ScheduledDate**: Optional, must be in future (when provided)
- **DeliveryAddress**: Optional, max 500 characters (when provided)

---

### DeliveryTracking Validators
**Location:** `WSC.Delivery\WSC.Delivery.Application\Validators\DeliveryTrackingValidators\`

#### CreateDeliveryTrackingValidator
- **DeliveryId**: Required, must be > 0
- **Status**: Required, must be valid DeliveryStatus enum value
- **Location**: Required, max 255 characters
- **Remarks**: Optional, max 500 characters (when provided)

---

## 🔄 AutoMapper Profiles Created

### DeliveryAgentProfile
**Location:** `WSC.Delivery\WSC.Delivery.Application\Mappings\DeliveryAgentProfile.cs`

**Mappings:**
```csharp
DeliveryAgent → DeliveryAgentResponseDto          // Read mapping
CreateDeliveryAgentDto → DeliveryAgent            // Create mapping (auto-sets CreatedAt)
UpdateDeliveryAgentDto → DeliveryAgent            // Update mapping (preserves key fields)
```

**Features:**
- Auto-generates CreatedAt timestamp for new agents
- Preserves DeliveryAgentId and CreatedAt on updates

---

### DeliveryItemProfile
**Location:** `WSC.Delivery\WSC.Delivery.Application\Mappings\DeliveryItemProfile.cs`

**Mappings:**
```csharp
DeliveryItem → DeliveryItemResponseDto            // Read mapping
CreateDeliveryItemDto → DeliveryItem              // Create mapping
UpdateDeliveryItemDto → DeliveryItem              // Update mapping (preserves ID and DeliveryId)
```

**Features:**
- Preserves DeliveryItemId and DeliveryId on updates
- Simple 1:1 mapping for create operations

---

### OrderDeliveryProfile
**Location:** `WSC.Delivery\WSC.Delivery.Application\Mappings\OrderDeliveryProfile.cs`

**Mappings:**
```csharp
OrderDelivery → OrderDeliveryResponseDto                    // Read mapping
CreateOrderDeliveryDto → OrderDelivery                      // Create mapping (auto-initializes)
UpdateOrderDeliveryDto → OrderDelivery                      // Update mapping (preserves core fields)
```

**Features:**
- Auto-sets Status to `DeliveryStatus.Pending` for new deliveries
- Auto-generates CreatedAt timestamp
- Auto-sets IsActive to `true` for new deliveries
- Preserves DeliveryId, OrderId, CustomerId, and CreatedAt on updates

---

### DeliveryTrackingProfile
**Location:** `WSC.Delivery\WSC.Delivery.Application\Mappings\DeliveryTrackingProfile.cs`

**Mappings:**
```csharp
DeliveryTracking → DeliveryTrackingResponseDto             // Read mapping
CreateDeliveryTrackingDto → DeliveryTracking               // Create mapping (auto-sets Timestamp)
```

**Features:**
- Auto-generates Timestamp for tracking records
- Read-only mapping (no update scenario needed)

---

## 🔧 Dependency Injection Setup

### Application Layer
**File:** `WSC.Delivery\WSC.Delivery.Application\DependencyInjection\ApplicationService.cs`

```csharp
public static IServiceCollection AddDeliveryApplicationService(
    this IServiceCollection services)
{
    services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    return services;
}
```

**Usage in Program.cs:**
```csharp
builder.Services.AddDeliveryApplicationService();
```

### Infrastructure Layer
**File:** `WSC.Delivery\WSC.Delivery.Infrastructure\DependencyInjection\InfrastructureService.cs`

```csharp
public static IServiceCollection AddDeliveryInfrastructureService(
    this IServiceCollection services)
{
    services.AddScoped<DapperContext>();
    services.AddScoped<IDeliveryRepository, DeliveryRepository>();
    services.AddScoped<IDeliveryAgentRepository, DeliveryAgentRepository>();
    services.AddScoped<IDeliveryItemRepository, DeliveryItemRepository>();
    services.AddScoped<IDeliveryTrackingRepository, DeliveryTrackingRepository>();
    return services;
}
```

**Usage in Program.cs:**
```csharp
builder.Services.AddDeliveryInfrastructureService();
```

---

## 📁 File Structure

```
WSC.Delivery\
├── WSC.Delivery.Application\
│   ├── Validators\
│   │   ├── DeliveryAgentValidators\
│   │   │   ├── CreateDeliveryAgentValidator.cs
│   │   │   └── UpdateDeliveryAgentValidator.cs
│   │   ├── DeliveryItemValidators\
│   │   │   ├── CreateDeliveryItemValidator.cs
│   │   │   └── UpdateDeliveryItemValidator.cs
│   │   ├── OrderDeliveryValidators\
│   │   │   ├── CreateOrderDeliveryValidator.cs
│   │   │   └── UpdateOrderDeliveryValidator.cs
│   │   └── DeliveryTrackingValidators\
│   │       └── CreateDeliveryTrackingValidator.cs
│   ├── Mappings\
│   │   ├── DeliveryAgentProfile.cs
│   │   ├── DeliveryItemProfile.cs
│   │   ├── OrderDeliveryProfile.cs
│   │   └── DeliveryTrackingProfile.cs
│   └── DependencyInjection\
│       └── ApplicationService.cs
└── WSC.Delivery.Infrastructure\
    └── DependencyInjection\
        └── InfrastructureService.cs
```

---

## ✅ Validation Rules Summary

| DTO | Rules |
|-----|-------|
| **CreateDeliveryAgentDto** | Name(100), Phone(E.164), VehicleNumber(50) |
| **UpdateDeliveryAgentDto** | ID(>0), Name(100-opt), Phone(E.164-opt), VehicleNumber(50-opt) |
| **CreateDeliveryItemDto** | DeliveryID(>0), ProductID(>0), Quantity(>0) |
| **UpdateDeliveryItemDto** | ItemID(>0), ProductID(>0-opt), Quantity(>0-opt) |
| **CreateOrderDeliveryDto** | OrderID(>0), CustomerID(>0), Tracking(100), ScheduledDate(future), Address(500) |
| **UpdateOrderDeliveryDto** | DeliveryID(>0), Tracking(100-opt), ScheduledDate(future-opt), Address(500-opt) |
| **CreateDeliveryTrackingDto** | DeliveryID(>0), Status(enum), Location(255), Remarks(500-opt) |

---

## 🎯 Best Practices Implemented

✅ **Separation of Concerns** - Validators and mappings in dedicated folders  
✅ **DRY Principle** - Auto-assembly registration of validators  
✅ **Validation Strategy** - Conditional validation for optional fields  
✅ **Mapping Strategy** - Auto-initialization of audit fields (CreatedAt, Timestamp)  
✅ **Field Protection** - Key fields ignored during updates to prevent data corruption  
✅ **Enum Validation** - Strict enum validation for status fields  
✅ **Phone Format** - E.164 international standard for phone validation  
✅ **Future Date Validation** - ScheduledDate must be in future for deliveries  
✅ **Dependency Injection** - Extension methods for clean DI registration  

---

## 🚀 Next Steps

1. **Create Service Layer** - Implement DeliveryService, DeliveryAgentService, DeliveryItemService, DeliveryTrackingService
2. **Create API Controllers** - REST endpoints for all entities
3. **Register in Program.cs** - Add DI registrations to API startup
4. **Database Migration** - Execute Delivery_Schema.sql to create tables
5. **Unit Tests** - Create xUnit tests for validators and mappings
6. **Integration Tests** - Test complete flow from API → Service → Repository → Database

---

## 📝 Notes

- All validators follow FluentValidation best practices
- All profiles use AutoMapper 13.x conventions
- Validators are automatically discovered and registered via reflection
- Optional fields use `.When()` conditional validation
- Timestamps are auto-generated using `DateTime.UtcNow`
- Phone validation uses E.164 international format: `^\+?[1-9]\d{1,14}$`
