using Dapper;
using System.Data;
using WSC.Shared.Contracts.Dtos.StoreLayer;
using WSC.Shared.Contracts.Exceptions;
using WSC.Store.Application.Interfaces.RepositoryInterfaces;
using WSC.Store.Domain.Entities;
using WSC.Store.Infrastructure.Persistence.Context;

namespace WSC.Store.Infrastructure.Repository
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly DapperContext _context;
        public InventoryRepository(DapperContext context)
        {
            _context = context;
        }
        public async Task<int> CreateInventoryRecordAsync(Inventory record, CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var sql = @"
                        INSERT INTO store.Inventory (ProductId, InStock, MinStock, CreatedAt) 
                        VALUES (@ProductId, @InStock, @MinStock, SYSUTCDATETIME());

                        SELECT CAST(SCOPE_IDENTITY() as int);
                        ";

            var parameters = new
            {
                record.ProductId,
                record.InStock,
                record.MinStock
            };

            var id = await con.ExecuteScalarAsync<int>(
                new CommandDefinition(sql, parameters, cancellationToken: ct)
            );

            return id;
        }

        public async Task<bool> DeleteInventoryRecordAsync(int id, CancellationToken ct)
        {
            using var con = _context.CreateConnection();
            var sql = @"UPDATE store.Inventory 
                        SET IsDeleted = 1
                        WHERE InventoryId = @Id";

            var affectedRows = await con.ExecuteAsync(new CommandDefinition(sql, new { Id = id }, cancellationToken: ct));

            return affectedRows > 0;
        }

        public async Task<IEnumerable<InventoryResponseDto>> GetAllInventoryRecordsAsync(CancellationToken ct)
        {
            using var con = _context.CreateConnection();
            var sql = @"SELECT i.InventoryId, i.ProductId, i.InStock, i.MinStock, i.IsDeleted, p.ProductName 
                        FROM store.Inventory i
                        INNER JOIN store.Products p ON i.ProductId = p.ProductId
                        WHERE i.IsDeleted = 0";

            var inventoryRecords = await con.QueryAsync<InventoryResponseDto>(new CommandDefinition(sql, cancellationToken: ct));

            return inventoryRecords.ToList();
        }


        public async Task<InventoryResponseDto?> GetInventoryRecordByIdAsync(int id, CancellationToken ct)
        {
            using var con = _context.CreateConnection();
            var sql = @"SELECT i.InventoryId, i.ProductId, i.InStock, i.MinStock, i.IsDeleted, p.ProductName
                        FROM store.Inventory i
                        INNER JOIN store.Products p ON i.ProductId = p.ProductId
                        WHERE i.InventoryId = @Id AND i.IsDeleted = 0";

            var inventoryRecord = await con.QuerySingleOrDefaultAsync<InventoryResponseDto>(new CommandDefinition(sql, new { Id = id }, cancellationToken: ct));
            if (inventoryRecord == null)
                throw new NotFoundException("Inventory", id);
            return inventoryRecord;
        }

        public async Task<Inventory> GetInventoryRecordEntityByIdAsync(int id, CancellationToken ct)
        {
            using var con = _context.CreateConnection();
            var sql = @"SELECT InventoryId, ProductId, InStock, MinStock, IsDeleted
                        FROM store.Inventory 
                        WHERE InventoryId = @Id AND IsDeleted = 0";

            var inventoryRecord = await con.QuerySingleOrDefaultAsync<Inventory>(new CommandDefinition(sql, new { Id = id }, cancellationToken: ct));
            if (inventoryRecord == null)
                throw new NotFoundException("Inventory", id);
            return inventoryRecord;
        }


        public async Task<bool> RecordExistsByProductAsync(int prdId, CancellationToken ct)
        {
            using var con = _context.CreateConnection();
            var sql = @"SELECT COUNT(1) FROM store.Inventory WHERE ProductId = @ProductId AND IsDeleted = 0";

            var exists = await con.ExecuteScalarAsync<int>(new CommandDefinition(sql, new { ProductId = prdId }, cancellationToken: ct));

            return exists > 0;
        }

        public async Task<bool> ReduceStockAsync(int productId, int quantity, IDbTransaction transaction, CancellationToken ct)
        {
            var sql = @"
                        UPDATE store.Inventory
                        SET InStock = InStock - @Quantity,
                            UpdatedAt = SYSUTCDATETIME()
                        WHERE ProductId = @ProductId
                          AND InStock >= @Quantity
                          AND IsDeleted = 0;";

            var rows = await transaction.Connection.ExecuteAsync(new CommandDefinition(sql,
                                                                    new { ProductId = productId, Quantity = quantity },
                                                                    transaction,
                                                                    cancellationToken: ct));
            return rows > 0;
        }

        /* public Task<bool> UpdateInventoryRecordAsync(Inventory inv, CancellationToken ct)
         {
             using var con = _context.CreateConnection();
             var sql = new StringBuilder(@"UPDATE store.Inventory 
                                             SET UpdatedAt = SYSUTCDATETIME()");

             var parameters = new DynamicParameters();
             parameters.Add("@InventoryId", inv.InventoryId);

             if(inv.InStock > 0)
             {
                 sql.Append(", InStock = @InStock");
                 parameters.Add("@InStock", inv.InStock);
             }

         }*/

        public async Task<bool> UpdateStockAsync(int id, int quantity, IDbTransaction transaction, CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var sql = @"UPDATE store.Inventory
                        SET InStock = InStock - @Quantity,
                            UpdatedAt = SYSUTCDATETIME()
                        WHERE ProductId = @ProductId
                          AND InStock >= @Quantity
                          AND IsDeleted = 0;";

            var parameters = new
            {
                ProductId = id,
                Quantity = quantity
            };

            var affectedRows = await transaction.Connection.ExecuteAsync(new CommandDefinition(sql, parameters, transaction, cancellationToken: ct));

            return affectedRows > 0;
        }

    }
}
