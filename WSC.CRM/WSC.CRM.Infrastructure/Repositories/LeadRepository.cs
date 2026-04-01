using Dapper;
using System.Data;
using System.Collections.Generic;
using System.Text;
using WSC.CRM.Application.Interfaces.Repository;
using WSC.CRM.Domain.Entities;
using WSC.CRM.Infrastructure.Persistence.Context;
using WSC.Shared.Contracts.Enums;
using WSC.Shared.Contracts.Dtos;

namespace WSC.CRM.Infrastructure.Repositories
{
    public class LeadRepository : ILeadRepository
    {
        private readonly DapperContext _context;
        public LeadRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<int> CreateLeadAsync(Lead lead, CancellationToken ct)
        {
            using var con = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@LeadName", lead.LeadName);
            parameters.Add("@LeadEmail", lead.LeadEmail);
            parameters.Add("@LeadPhone", lead.LeadPhone);
            parameters.Add("@CustomerId", lead.CustomerId);

            parameters.Add("@NewId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await con.ExecuteAsync(
                "crm.sp_CreateLead",
                parameters, commandType: CommandType.StoredProcedure );

            var leadId = parameters.Get<int>("@NewId");
            return leadId;
        }

        public async Task<bool> DeleteLeadAsync(int id, CancellationToken ct)
        {
            using var con = _context.CreateConnection();
            var sql = @"UPDATE crm.Leads
                        SET IsActive = 0, UpdatedAt = SYSUTCDATETIME()
                        WHERE LeadId = @Id AND IsActive = 1";
            var affectedrows = await con.ExecuteAsync(new CommandDefinition(sql, new { Id = id }, cancellationToken: ct));

            return affectedrows > 0;
        }

        public async Task<bool> ExistsByLeadAsync(string email, CancellationToken ct)
        {
            using var con = _context.CreateConnection();
            var sql = @"SELECT 1 
                        WHERE EXISTS (
                            SELECT 1 FROM crm.Leads
                            WHERE LeadEmail = @Email AND IsActive = 1
                        )"; 
            var result = await con.QueryFirstOrDefaultAsync<int?>(
                new CommandDefinition(sql, new { Email = email }, cancellationToken: ct));

            return result.HasValue;
        }

        public async Task<IEnumerable<LeadResponseDto>> GetAllLeadsAsync(CancellationToken ct)
        {
            using var con = _context.CreateConnection();
            var sql = @"SELECT l.LeadId, l.LeadName, l.LeadEmail, l.LeadPhone, l.Status, l.CreatedAt, l.UpdatedAt, l.CustomerId, c.CxName AS CustomerName, l.IsActive
                        FROM crm.Leads l
                        INNER JOIN crm.Customers c ON l.CustomerId = c.CxId
                        WHERE l.IsActive = 1";
            var leads =await con.QueryAsync<LeadResponseDto>(new CommandDefinition(sql, cancellationToken: ct));
            return leads;
        }

        public async Task<LeadResponseDto?> GetLeadByIdAsync(int id, CancellationToken ct)
        {
            using var con = _context.CreateConnection();    
            var sql = @"SELECT l.LeadId, l.LeadName, l.LeadEmail, l.LeadPhone, l.Status, l.CreatedAt, l.UpdatedAt, l.CustomerId, c.CxName AS CustomerName, l.IsActive
                        FROM crm.Leads l
                        INNER JOIN crm.Customers c ON l.CustomerId = c.CxId
                        WHERE l.IsActive = 1 and LeadId = @Id";

            var lead =await con.QueryFirstOrDefaultAsync<LeadResponseDto>(new CommandDefinition(sql, new { Id = id }, cancellationToken: ct));

            return lead;
        }

        public async Task<bool> UpdateLeadAsync(Lead lead, CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var sql = new StringBuilder(@"UPDATE crm.Leads 
                                        SET UpdatedAt = SYSUTCDATETIME()");

            var parameters = new DynamicParameters();
            parameters.Add("@LeadId", lead.LeadId);

            if(!string.IsNullOrWhiteSpace(lead.LeadName))
            {
                sql.Append(", LeadName = @LeadName");
                parameters.Add("@LeadName", lead.LeadName);
            }
            if(!string.IsNullOrWhiteSpace(lead.LeadEmail))
            {
                sql.Append(", LeadEmail = @LeadEmail");
                parameters.Add("@LeadEmail", lead.LeadEmail);
            }
            if(!string.IsNullOrWhiteSpace(lead.LeadPhone))
            {
                sql.Append(", LeadPhone = @LeadPhone");
                parameters.Add("@LeadPhone", lead.LeadPhone);
            }
            sql.Append(" WHERE LeadId = @LeadId AND IsActive = 1");

            var updated = await con.ExecuteAsync(new CommandDefinition(sql.ToString(), parameters, cancellationToken: ct));
            return updated > 0;
        }

        public async Task<bool> UpdateLeadStatusAsync(int id, LeadStatus newStatus, CancellationToken ct)
        {
            using var con = _context.CreateConnection();
            var sql = @"UPDATE crm.Leads 
                        SET Status = @Status, UpdatedAt = SYSUTCDATETIME() 
                        WHERE LeadId = @Id AND IsActive = 1";

            var affectedrows =await con.ExecuteAsync(new CommandDefinition(sql, new { Id = id, Status = (int)newStatus }, cancellationToken: ct));

            return affectedrows > 0;
        }
        public async Task<Lead?> GetLeadEntityByIdAsync(int id, CancellationToken ct)
        {
            using var con = _context.CreateConnection();
            var sql = @"SELECT LeadId, LeadName, LeadEmail, LeadPhone,
                       Status, CreatedAt, UpdatedAt, CustomerId
                FROM crm.Leads
                WHERE LeadId = @Id AND IsActive = 1";

            return await con.QueryFirstOrDefaultAsync<Lead>(sql, new { Id = id });
        }
    }
}
