using Dapper;
using System.Data;
using System.Text;
using WSC.CRM.Application.Interfaces.Repository;
using WSC.CRM.Domain.Entities;
using WSC.CRM.Infrastructure.Persistence.Context;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.CRMLayer;

namespace WSC.CRM.Infrastructure.Repositories
{
    internal sealed class CustomerRepository : ICustomerRepository
    {
        private readonly DapperContext _context;
        public CustomerRepository(DapperContext context) => _context = context;

        public async Task<int> CreateCustomerAsync(Customer cx, CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@CxName", cx.CxName);
            parameters.Add("@CxEmail", cx.CxEmail);
            parameters.Add("@CxPhone", cx.CxPhone);
            parameters.Add("@Street", cx.CxAddress?.Street);
            parameters.Add("@City", cx.CxAddress?.City);
            parameters.Add("@State", cx.CxAddress?.State);
            parameters.Add("@ZipCode", cx.CxAddress?.ZipCode);
            parameters.Add("@Country", cx.CxAddress?.Country);
            parameters.Add("@CreatedAt", DateTime.UtcNow);

            parameters.Add("@NewId",
                            dbType: DbType.Int32,
                            direction: ParameterDirection.Output);

            await con.ExecuteAsync("crm.sp_CreateCustomer",
                                    parameters, commandType: CommandType.StoredProcedure);

            var id = parameters.Get<int>("@NewId");
            return id;
        }

        public async Task<bool> DeleteCustomerAsync(int id, CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var sql = @"UPDATE crm.Customers
                        SET IsActive = 0, UpdatedAt = SYSUTCDATETIME()
                        WHERE CxId = @Id AND IsActive = 1";

            var affectedRows = await con.ExecuteAsync(new CommandDefinition(sql, new { Id = id }, cancellationToken: ct));
            return affectedRows > 0;
        }

        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken ct)
        {
            using var con = _context.CreateConnection();
            var sql = @"SELECT 1
                        FROM crm.Customers 
                        WHERE CxEmail = @Email AND IsActive = 1";

            var exists = await con.QueryFirstOrDefaultAsync<int?>(new CommandDefinition(sql, new { Email = email }, cancellationToken: ct));
            return exists.HasValue;

        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync(CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var sql = @"SELECT CxId, CxName, CxEmail, CxPhone, Street, City, State, ZipCode, Country FROM crm.Customers";
            var customers = await con.QueryAsync<Customer>(new CommandDefinition(sql, cancellationToken: ct));

            return customers;
        }

        public async Task<Customer?> GetCustomerByIdAsync(int id, CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var sql = @"SELECT CxId, CxName, CxEmail, CxPhone, Street, City, State, ZipCode, Country
                        FROM crm.Customers 
                        WHERE CxId = @Id";

            var customer = await con.QueryFirstOrDefaultAsync<Customer>(new CommandDefinition(sql, new { Id = id }, cancellationToken: ct));
            return customer;
        }

        public async Task<(IEnumerable<CustomerResponseDto> Data, int TotalCount)> GetPagedCustomersAsync(PaginationRequest request, CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var sql = @"
                        SELECT CxId, CxName, CxEmail, CxPhone, Street, City, State, ZipCode, Country
                        FROM crm.Customers 
                        WHERE IsActive = 1
                        ORDER BY CxId
                        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

                        SELECT COUNT(1)
                        FROM crm.Customers
                        WHERE IsActive = 1;
                    ";

            using var multi = await con.QueryMultipleAsync(
                new CommandDefinition(sql, new
                {
                    Offset = (request.PageNumber - 1) * request.PageSize,
                    request.PageSize
                }, cancellationToken: ct));

            var data = await multi.ReadAsync<CustomerResponseDto>();
            var totalCount = await multi.ReadFirstAsync<int>();

            return (data, totalCount);
        }

        public async Task<bool> UpdateCustomerAsync(Customer cx, CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var sql = new StringBuilder(@"UPDATE crm.Customers
                        SET UpdatedAt = SYSUTCDATETIME()");

            var parameters = new DynamicParameters();
            parameters.Add("CxId", cx.CxId);

            if (!string.IsNullOrWhiteSpace(cx.CxName))
            {
                sql.Append(", CxName = @CxName");
                parameters.Add("CxName", cx.CxName);
            }
            if (!string.IsNullOrWhiteSpace(cx.CxEmail))
            {
                sql.Append(", CxEmail = @CxEmail");
                parameters.Add("CxEmail", cx.CxEmail);
            }
            if (!string.IsNullOrWhiteSpace(cx.CxPhone))
            {
                sql.Append(", CxPhone = @CxPhone");
                parameters.Add("CxPhone", cx.CxPhone);
            }
            if (!string.IsNullOrWhiteSpace(cx.CxAddress?.Street))
            {
                sql.Append(", Street = @Street");
                parameters.Add("Street", cx.CxAddress.Street);
            }
            if (!string.IsNullOrWhiteSpace(cx.CxAddress?.City))
            {
                sql.Append(", City = @City");
                parameters.Add("City", cx.CxAddress.City);
            }
            if (!string.IsNullOrWhiteSpace(cx.CxAddress?.State))
            {
                sql.Append(", State = @State");
                parameters.Add("State", cx.CxAddress.State);
            }

            if (!string.IsNullOrWhiteSpace(cx.CxAddress?.ZipCode))
            {
                sql.Append(", ZipCode = @ZipCode");
                parameters.Add("ZipCode", cx.CxAddress.ZipCode);
            }
            if (!string.IsNullOrWhiteSpace(cx.CxAddress?.Country))
            {
                sql.Append(", Country = @Country");
                parameters.Add("Country", cx.CxAddress.Country);
            }
            sql.Append(" WHERE CxId = @CxId AND IsActive = 1");

            var updated = await con.ExecuteAsync(new CommandDefinition(sql.ToString(), parameters, cancellationToken: ct));
            return updated > 0;
        }
    }
}
