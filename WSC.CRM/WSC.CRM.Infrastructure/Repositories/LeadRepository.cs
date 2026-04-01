using Dapper;
using System.Data;
using System.Collections.Generic;
using System.Text;
using WSC.CRM.Application.Interfaces.Repository;
using WSC.CRM.Domain.Entities;
using WSC.CRM.Infrastructure.Persistence.Context;
using WSC.Shared.Contracts.Enums;

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

            parameters.Add("@LeadId", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

            await con.ExecuteAsync(
                "sp_CreateLead",
                parameters, commandType: CommandType.StoredProcedure );

            var leadId = parameters.Get<int>("@LeadId");
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
            var sql = @"SELECT COUNT(1) FROM crm.Leads
                        WHERE LeadEmail = @Email AND IsActive = 1"; 
            var exists = await con.QueryFirstOrDefaultAsync<int?>(new CommandDefinition(sql, new { Email = email }, cancellationToken: ct));

            return exists > 0;
        }

        public async Task<IEnumerable<Lead>> GetAllLeadsAsync(CancellationToken ct)
        {
            using var con = _context.CreateConnection();
            var sql = @"SELECT LeadId, LeadName, LeadEmail, LeadPhone, Status, CreatedAt, UpdatedAt, CustomerId
                        FROM crm.Leads
                        WHERE IsActive = 1";
            var leads =await con.QueryAsync<Lead>(new CommandDefinition(sql, cancellationToken: ct));
            return leads;
        }

        public async Task<Lead?> GetLeadByIdAsync(int id, CancellationToken ct)
        {
            using var con = _context.CreateConnection();    
            var sql = @"SELECT LeadId, LeadName, LeadEmail, LeadPhone, Status, CreatedAt, UpdatedAt, CustomerId
                        FROM crm.Leads
                        WHERE LeadId = @Id AND IsActive = 1";

            var lead =await con.QueryFirstOrDefaultAsync<Lead>(new CommandDefinition(sql, new { Id = id }, cancellationToken: ct));

            return lead;
        }

        public async Task<bool> UpdateLeadAsync(Lead lead, CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var sql = new StringBuilder(@"UPDATE crm.Leads 
                                        SET UpdatedAt = SYSUTCDATETIME()");

            var parameters = new DynamicParameters();
            parameters.Add("@LeadId", lead.LeadId);

            if(!string.IsNullOrEmpty(lead.LeadName))
            {
                sql.Append(", LeadName = @LeadName");
                parameters.Add("@LeadName", lead.LeadName);
            }
            if(!string.IsNullOrEmpty(lead.LeadEmail))
            {
                sql.Append(", LeadEmail = @LeadEmail");
                parameters.Add("@LeadEmail", lead.LeadEmail);
            }
            if(!string.IsNullOrEmpty(lead.LeadPhone))
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

            var affectedrows =await con.ExecuteAsync(new CommandDefinition(sql, new { Id = id, Status = newStatus }, cancellationToken: ct));

            return affectedrows > 0;
        }
    }
}
