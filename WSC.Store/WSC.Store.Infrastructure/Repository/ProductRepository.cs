using Dapper;
using System.Data;
using System.Text;
using WSC.Store.Application.Interfaces.RepositoryInterfaces;
using WSC.Store.Domain.Entities;
using WSC.Store.Infrastructure.Persistence.Context;

namespace WSC.Store.Infrastructure.Repository
{
    internal sealed class ProductRepository : IProductRepository
    {
        private DapperContext _context;
        public ProductRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<int> CreateProductAsync(Product prd, CancellationToken ct)
        {
            using var con = _context.CreateConnection();
            var parameters = new DynamicParameters();

            parameters.Add("@ProductName", prd.ProductName);
            parameters.Add("@SKU", prd.SKU);
            parameters.Add("@Category", prd.Category);
            parameters.Add("@Price", prd.Price);

            parameters.Add("@NewId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var created = await con.ExecuteAsync(new CommandDefinition(
                                                                "store.sp_CreateProduct",
                                                                parameters,
                                                                commandType: CommandType.StoredProcedure,
                                                                cancellationToken: ct));

            var newId = parameters.Get<int>("@NewId");
            return newId;
        }

        public async Task<bool> DeleteProductAsync(int id, CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var sql = @"UPDATE store.Products
                        SET IsActive = 0
                        WHERE ProductId = @Id";

            var deleted = await con.ExecuteAsync(new CommandDefinition(sql, new { Id = id }, cancellationToken: ct));
            return deleted > 0;
        }

        public async Task<bool> ExistsBySKUAsync(string sku, CancellationToken ct)
        {
            using var con = _context.CreateConnection();
            var sql = @"SELECT COUNT(1) FROM store.Products WHERE SKU = @SKU AND IsActive = 1";

            var exists = await con.ExecuteScalarAsync<int>(new CommandDefinition(sql, new { SKU = sku }, cancellationToken: ct));
            return exists > 0;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken ct)
        {
            using var con = _context.CreateConnection();
            var sql = @"SELECT ProductId, ProductName, SKU, Category, Price, IsActive
                        FROM store.Products WHERE IsActive = 1";

            var products = await con.QueryAsync<Product>(new CommandDefinition(sql, cancellationToken: ct));
            return products;
        }

        public async Task<Product?> GetProductByIdAsync(int id, CancellationToken ct)
        {
            using var con = _context.CreateConnection();
            var sql = @"SELECT ProductId, ProductName, SKU, Category, Price, IsActive
                        FROM store.Products WHERE IsActive = 1 AND ProductId = @Id";

            var product = await con.QuerySingleOrDefaultAsync<Product>(new CommandDefinition(sql, new { Id = id }, cancellationToken: ct));

            return product;

        }

        public async Task<bool> UpdateProductAsync(Product prd, CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var sql = new StringBuilder(@"UPDATE store.Products 
                                            SET UpdatedAt = SYSUTCDATETIME()");

            var parameters = new DynamicParameters();
            parameters.Add("@ProductId", prd.ProductId);

            if (!string.IsNullOrEmpty(prd.ProductName))
            {
                sql.Append(", ProductName = @ProductName");
                parameters.Add("@ProductName", prd.ProductName);
            }
            if (!string.IsNullOrEmpty(prd.SKU))
            {
                sql.Append(", SKU = @SKU");
                parameters.Add("@SKU", prd.SKU);
            }
            if (!string.IsNullOrEmpty(prd.Category))
            {
                sql.Append(", Category = @Category");
                parameters.Add("@Category", prd.Category);
            }
            if (prd.Price > 0)
            {
                sql.Append(", Price = @Price");
                parameters.Add("@Price", prd.Price);
            }
            sql.Append(" WHERE ProductId = @ProductId AND IsActive = 1");

            var updated = await con.ExecuteAsync(new CommandDefinition(sql.ToString(), parameters, cancellationToken: ct));

            return updated > 0;
        }
    }
}
