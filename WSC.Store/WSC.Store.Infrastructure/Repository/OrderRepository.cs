using Dapper;
using System.Text;
using WSC.Shared.Contracts.Dtos.StoreLayer;
using WSC.Store.Application.Interfaces.RepositoryInterfaces;
using WSC.Store.Domain.Entities;
using WSC.Store.Infrastructure.Persistence.Context;

namespace WSC.Store.Infrastructure.Repository
{
    internal sealed class OrderRepository : IOrderRepository
    {
        private readonly DapperContext _context;
        public OrderRepository(DapperContext context)
        {
            _context = context;
        }
        public async Task<int> CreateOrderAsync(Order order, CancellationToken ct)
        {
            using var con = _context.CreateConnection();
            var sql = @"INSERT INTO store.Orders (CustomerId, TotalAmount, Status, CreatedAt, IsActive)
                        VALUES (@CustomerId, @TotalAmount, @Status, SYSUTCDATETIME(), 1);
                        SELECT CAST(SCOPE_IDENTITY() as int);";

            var parameters = new
            {
                order.CustomerId,
                order.TotalAmount,
                order.Status,
                order.CreatedAt,
                order.UpdatedAt,
                order.IsActive
            };

            var created = await con.QuerySingleAsync<int>(new CommandDefinition(sql, parameters, cancellationToken: ct));
            return created;
        }

        public async Task<bool> DeleteOrderAsync(int id, CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var sql = @"UPDATE store.Orders 
                        SET IsActive = 0
                        WHERE OrderId = @Id";

            var deleted = await con.ExecuteAsync(new CommandDefinition(sql, new { Id = id }, cancellationToken: ct));
            return deleted > 0;
        }

        public async Task<IEnumerable<OrderResponseDto>> GetAllOrdersAsync(CancellationToken ct)
        {
            using var con = _context.CreateConnection();
            var sql = @"SELECT o.OrderId, o.CustomerId,c.CxName As CustomerName,  o.TotalAmount, o.Status, o.CreatedAt, o.UpdatedAt, o.IsActive
                        FROM store.Orders o
                        LEFT JOIN crm.Customers c ON o.CustomerId = c.CxId
                        WHERE o.IsActive = 1";

            var orders = await con.QueryAsync<OrderResponseDto>(new CommandDefinition(sql, cancellationToken: ct));
            return orders.ToList();
        }

        public async Task<OrderResponseDto> GetOrderByIdAsync(int id, CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var sql = @"SELECT o.OrderId, o.CustomerId,c.CxName As CustomerName,  o.TotalAmount, o.Status, o.CreatedAt, o.UpdatedAt, o.IsActive
                        FROM store.Orders o
                        LEFT JOIN crm.Customers c ON o.CustomerId = c.CxId
                        WHERE o.OrderId = @Id AND o.IsActive = 1";

            var order = await con.QuerySingleOrDefaultAsync<OrderResponseDto>(new CommandDefinition(sql, new { Id = id }, cancellationToken: ct));

            return order;
        }

        public async Task<Order> GetOrderEntityByIdAsync(int id, CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var sql = @"SELECT OrderId, CustomerId, TotalAmount, Status, CreatedAt, UpdatedAt, IsActive
                        FROM store.Orders 
                        WHERE OrderId = @Id AND IsActive = 1";

            var order = await con.QuerySingleOrDefaultAsync<Order>(new CommandDefinition(sql, new { Id = id }, cancellationToken: ct));

            return order;
        }

        public async Task<bool> UpdateOrderAsync(Order order, CancellationToken ct)
        {
            using var con = _context.CreateConnection();
            var sql = new StringBuilder(@"UPDATE store.Orders SET 
                                          UpdatedAt = SYSUTCDATETIME()");

            var parameters = new DynamicParameters(sql);

            parameters.Add("@OrderId", order.OrderId);

            if (order.CustomerId != default)
            {
                sql.Append(", CustomerId = @CustomerId");
                parameters.Add("@CustomerId", order.CustomerId);
            }
            if (order.TotalAmount != default)
            {
                sql.Append(", TotalAmount = @TotalAmount");
                parameters.Add("@TotalAmount", order.TotalAmount);
            }
            if (order.Status != default)
            {
                sql.Append(", Status = @Status");
                parameters.Add("@Status", order.Status);
            }
            sql.Append(" WHERE OrderId = @OrderId");

            var updated = await con.ExecuteAsync(new CommandDefinition(sql.ToString(), parameters, cancellationToken: ct));

            return updated > 0;
        }
    }
}
