using Dapper;
using System.Data;
using System.Text;
using WSC.Delivery.Application.Interfaces.RepositoryInterfaces;
using WSC.Delivery.Domain.Entities;
using WSC.Delivery.Infrastructure.Persistence.Context;
using WSC.Shared.Contracts.Dtos.DeliveryLayer;

namespace WSC.Delivery.Infrastructure.Repositories
{
    internal sealed class DeliveryRepository : IDeliveryRepository
    {
        private readonly DapperContext _context;
        public DeliveryRepository(DapperContext context) => _context = context;

        public async Task<int> CreateDeliveryAsync(OrderDelivery delivery, CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@OrderId", delivery.OrderId);
            parameters.Add("@CustomerId", delivery.CustomerId);
            parameters.Add("@TrackingNumber", delivery.TrackingNumber);
            parameters.Add("@Status", delivery.Status.ToString());
            parameters.Add("@AssignedAgentId", delivery.AssignedAgentId);
            parameters.Add("@ScheduledDate", delivery.ScheduledDate);
            parameters.Add("@DeliveryAddress", delivery.DeliveryAddress);
            parameters.Add("@CreatedAt", DateTime.UtcNow);
            parameters.Add("@IsActive", delivery.IsActive);
            parameters.Add("@NewId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var sql = @"INSERT INTO delivery.OrderDeliveries 
                        (OrderId, CustomerId, TrackingNumber, Status, AssignedAgentId, ScheduledDate, DeliveryAddress, CreatedAt, IsActive)
                        VALUES (@OrderId, @CustomerId, @TrackingNumber, @Status, @AssignedAgentId, @ScheduledDate, @DeliveryAddress, @CreatedAt, @IsActive)
                        SET @NewId = SCOPE_IDENTITY()";

            var affectedRows = await con.ExecuteAsync(new CommandDefinition(sql, parameters, cancellationToken: ct));

            return parameters.Get<int>("@NewId");
        }

        public async Task<bool> DeleteDeliveryByIdAsync(int deliveryId, CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var sql = @"UPDATE delivery.OrderDeliveries
                        SET IsActive = 0, UpdatedAt = SYSUTCDATETIME()
                        WHERE DeliveryId = @DeliveryId AND IsActive = 1";

            var affectedRows = await con.ExecuteAsync(new CommandDefinition(sql, new { DeliveryId = deliveryId }, cancellationToken: ct));

            return affectedRows > 0;
        }

        public async Task<IEnumerable<OrderDeliveryResponseDto>> GetAllDeliveriesAsync(CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var sql = @"SELECT DeliveryId, OrderId, CustomerId, TrackingNumber, Status, AssignedAgentId, ScheduledDate, DeliveredDate, IsActive
                        FROM delivery.OrderDeliveries
                        WHERE IsActive = 1
                        ORDER BY DeliveryId";

            var deliveries = await con.QueryAsync<OrderDeliveryResponseDto>(new CommandDefinition(sql, cancellationToken: ct));

            return deliveries;
        }

        public async Task<OrderDeliveryResponseDto> GetByDeliveryIdAsync(int deliveryId, CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var sql = @"SELECT DeliveryId, OrderId, CustomerId, TrackingNumber, Status, AssignedAgentId, ScheduledDate, DeliveredDate, IsActive
                        FROM delivery.OrderDeliveries
                        WHERE DeliveryId = @DeliveryId AND IsActive = 1";

            var delivery = await con.QueryFirstOrDefaultAsync<OrderDeliveryResponseDto>(new CommandDefinition(sql, new { DeliveryId = deliveryId }, cancellationToken: ct));

            return delivery;
        }

        public async Task<OrderDelivery> GetDeliveryEntityByIdAsync(int deliveryId, CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var sql = @"SELECT DeliveryId, OrderId, CustomerId, TrackingNumber, Status, AssignedAgentId, ScheduledDate, DeliveredDate, DeliveryAddress, IsActive, CreatedAt, UpdatedAt
                        FROM delivery.OrderDeliveries
                        WHERE DeliveryId = @DeliveryId AND IsActive = 1";

            var delivery = await con.QueryFirstOrDefaultAsync<OrderDelivery>(new CommandDefinition(sql, new { DeliveryId = deliveryId }, cancellationToken: ct));

            return delivery;
        }

        public async Task<IEnumerable<OrderDelivery>> GetByDeliveryStatusAsync(string status, CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var sql = @"SELECT DeliveryId, OrderId, CustomerId, TrackingNumber, Status, AssignedAgentId, ScheduledDate, DeliveredDate, DeliveryAddress, IsActive, CreatedAt, UpdatedAt
                        FROM delivery.OrderDeliveries
                        WHERE Status = @Status AND IsActive = 1
                        ORDER BY DeliveryId
                        OFFSET 0 ROWS FETCH NEXT 1 ROWS ONLY";

            var delivery = await con.QueryFirstOrDefaultAsync<IEnumerable<OrderDelivery>>(new CommandDefinition(sql, new { Status = status }, cancellationToken: ct));

            return delivery;
        }

        public async Task<bool> UpdateDeliveryDetailsAsync(OrderDelivery delivery, CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var sql = new StringBuilder(@"UPDATE delivery.OrderDeliveries
                        SET UpdatedAt = SYSUTCDATETIME()");

            var parameters = new DynamicParameters();
            parameters.Add("@DeliveryId", delivery.DeliveryId);

            if (!string.IsNullOrWhiteSpace(delivery.TrackingNumber))
            {
                sql.Append(", TrackingNumber = @TrackingNumber");
                parameters.Add("@TrackingNumber", delivery.TrackingNumber);
            }

            if (delivery.AssignedAgentId.HasValue)
            {
                sql.Append(", AssignedAgentId = @AssignedAgentId");
                parameters.Add("@AssignedAgentId", delivery.AssignedAgentId);
            }

            if (delivery.ScheduledDate != default)
            {
                sql.Append(", ScheduledDate = @ScheduledDate");
                parameters.Add("@ScheduledDate", delivery.ScheduledDate);
            }

            if (!string.IsNullOrWhiteSpace(delivery.DeliveryAddress))
            {
                sql.Append(", DeliveryAddress = @DeliveryAddress");
                parameters.Add("@DeliveryAddress", delivery.DeliveryAddress);
            }

            sql.Append(" WHERE DeliveryId = @DeliveryId AND IsActive = 1");

            var affectedRows = await con.ExecuteAsync(new CommandDefinition(sql.ToString(), parameters, cancellationToken: ct));

            return affectedRows > 0;
        }

        public async Task<bool> UpdateDeliveryStatusAsync(string status, CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var sql = @"UPDATE delivery.OrderDeliveries
                        SET Status = @Status, 
                            DeliveredDate = CASE WHEN @Status = 'Delivered' THEN SYSUTCDATETIME() ELSE DeliveredDate END,
                            UpdatedAt = SYSUTCDATETIME()
                        WHERE IsActive = 1";

            var affectedRows = await con.ExecuteAsync(new CommandDefinition(sql, new { Status = status }, cancellationToken: ct));

            return affectedRows > 0;
        }
    }
}
