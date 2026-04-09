using Dapper;
using System.Data;
using WSC.Delivery.Application.Interfaces.RepositoryInterfaces;
using WSC.Delivery.Domain.Entities;
using WSC.Delivery.Infrastructure.Persistence.Context;
using WSC.Shared.Contracts.Dtos.DeliveryLayer;

namespace WSC.Delivery.Infrastructure.Repositories
{
    internal class DeliveryItemRepository : IDeliveryItemRepository
    {
        private readonly DapperContext _context;
        public DeliveryItemRepository(DapperContext context) => _context = context;

        public async Task<int> CreateDeliveryItemAsync(DeliveryItem item, CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@DeliveryId", item.DeliveryId);
            parameters.Add("@ProductId", item.ProductId);
            parameters.Add("@Quantity", item.Quantity);
            parameters.Add("@DeliveryItemId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var sql = @"INSERT INTO delivery.DeliveryItems 
                        (DeliveryId, ProductId, Quantity)
                        VALUES (@DeliveryId, @ProductId, @Quantity);
                        SET @DeliveryItemId = SCOPE_IDENTITY();";

            await con.ExecuteAsync(new CommandDefinition(sql, parameters, cancellationToken: ct));

            return parameters.Get<int>("@DeliveryItemId");
        }

        public async Task<IEnumerable<DeliveryItemResponseDto>> GetItemsByDeliveryIdAsync(int deliveryId, CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var sql = @"SELECT DeliveryItemId, DeliveryId, ProductId, Quantity
                        FROM delivery.DeliveryItems
                        WHERE DeliveryId = @DeliveryId
                        ORDER BY DeliveryItemId";

            var items = await con.QueryAsync<DeliveryItemResponseDto>(
                new CommandDefinition(sql, new { DeliveryId = deliveryId }, cancellationToken: ct));

            return items;
        }

        public async Task<DeliveryItemResponseDto> GetItemByIdAsync(int itemId, CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var sql = @"SELECT DeliveryItemId, DeliveryId, ProductId, Quantity
                        FROM delivery.DeliveryItems
                        WHERE DeliveryItemId = @ItemId";

            var item = await con.QueryFirstOrDefaultAsync<DeliveryItemResponseDto>(
                new CommandDefinition(sql, new { ItemId = itemId }, cancellationToken: ct));

            return item;
        }

        public async Task<DeliveryItem> GetItemEntityByIdAsync(int itemId, CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var sql = @"SELECT DeliveryItemId, DeliveryId, ProductId, Quantity
                        FROM delivery.DeliveryItems
                        WHERE DeliveryItemId = @ItemId";

            var item = await con.QueryFirstOrDefaultAsync<DeliveryItem>(
                new CommandDefinition(sql, new { ItemId = itemId }, cancellationToken: ct));

            return item;
        }

        public async Task<bool> UpdateDeliveryItemAsync(DeliveryItem item, CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var sql = @"UPDATE delivery.DeliveryItems
                        SET ProductId = @ProductId, Quantity = @Quantity
                        WHERE DeliveryItemId = @DeliveryItemId";

            var affectedRows = await con.ExecuteAsync(
                new CommandDefinition(sql, item, cancellationToken: ct));

            return affectedRows > 0;
        }

        public async Task<bool> DeleteDeliveryItemAsync(int itemId, CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var sql = @"DELETE FROM delivery.DeliveryItems
                        WHERE DeliveryItemId = @ItemId";

            var affectedRows = await con.ExecuteAsync(
                new CommandDefinition(sql, new { ItemId = itemId }, cancellationToken: ct));

            return affectedRows > 0;
        }
    }
}
