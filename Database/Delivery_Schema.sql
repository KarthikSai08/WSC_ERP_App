-- Delivery Module Database Schema
-- This script creates all tables for the Delivery module

-- Create delivery schema
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'delivery')
BEGIN
    EXEC sp_executesql N'CREATE SCHEMA delivery'
END
GO

-- Create DeliveryAgents table
CREATE TABLE delivery.DeliveryAgents
(
    DeliveryAgentId INT PRIMARY KEY IDENTITY(1,1),
    AgentName NVARCHAR(100) NOT NULL,
    AgentPhone NVARCHAR(20) NOT NULL,
    VehicleNumber NVARCHAR(50) NOT NULL,
    IsAvailable BIT DEFAULT 1,
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME DEFAULT SYSUTCDATETIME(),
    INDEX IX_IsActive (IsActive),
    INDEX IX_IsAvailable (IsAvailable)
)
GO

-- Create OrderDeliveries table
CREATE TABLE delivery.OrderDeliveries
(
    DeliveryId INT PRIMARY KEY IDENTITY(1,1),
    OrderId INT NOT NULL,
    CustomerId INT NOT NULL,
    TrackingNumber NVARCHAR(100),
    Status NVARCHAR(50) NOT NULL,
    AssignedAgentId INT,
    ScheduledDate DATETIME,
    DeliveredDate DATETIME NULL,
    DeliveryAddress NVARCHAR(MAX),
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME DEFAULT SYSUTCDATETIME(),
    UpdatedAt DATETIME NULL,
    FOREIGN KEY (AssignedAgentId) REFERENCES delivery.DeliveryAgents(DeliveryAgentId),
    INDEX IX_OrderId (OrderId),
    INDEX IX_CustomerId (CustomerId),
    INDEX IX_Status (Status),
    INDEX IX_IsActive (IsActive),
    INDEX IX_TrackingNumber (TrackingNumber)
)
GO

-- Create DeliveryItems table
CREATE TABLE delivery.DeliveryItems
(
    DeliveryItemId INT PRIMARY KEY IDENTITY(1,1),
    DeliveryId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL,
    FOREIGN KEY (DeliveryId) REFERENCES delivery.OrderDeliveries(DeliveryId),
    INDEX IX_DeliveryId (DeliveryId),
    INDEX IX_ProductId (ProductId)
)
GO

-- Create DeliveryTracking table
CREATE TABLE delivery.DeliveryTracking
(
    TrackingId INT PRIMARY KEY IDENTITY(1,1),
    DeliveryId INT NOT NULL,
    Status NVARCHAR(50) NOT NULL,
    Location NVARCHAR(MAX),
    Remarks NVARCHAR(MAX),
    Timestamp DATETIME DEFAULT SYSUTCDATETIME(),
    FOREIGN KEY (DeliveryId) REFERENCES delivery.OrderDeliveries(DeliveryId),
    INDEX IX_DeliveryId (DeliveryId),
    INDEX IX_Status (Status),
    INDEX IX_Timestamp (Timestamp)
)
GO

PRINT 'Delivery module database schema created successfully!'
