using Dapper;
using System.Text;
using WSC.CRM.Application.Interfaces.Repository;
using WSC.CRM.Domain.Entities;
using WSC.CRM.Infrastructure.Persistence.Context;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.CRMLayer;

namespace WSC.CRM.Infrastructure.Repositories
{
    public class OpportunityRepository : IOpportunityRepository
    {
        private readonly DapperContext _context;
        public OpportunityRepository(DapperContext context) => _context = context;
        public async Task<int> CreateOpportunityAsync(Opportunity opp, CancellationToken ct)
        {
            using var con = _context.CreateConnection();
            var parameters = new DynamicParameters();

            parameters.Add("@OpportunityName", opp.OpportunityName);
            parameters.Add("@Stage", opp.Stage);
            parameters.Add("@Amount", opp.Amount);

            parameters.Add("@ClosedAt", opp.ClosedAt);
            parameters.Add("@CustomerId", opp.CustomerId);
            parameters.Add("@LeadId", opp.LeadId);
            parameters.Add(
                            "@@NewOpportunityId",
                            dbType: System.Data.DbType.Int32,
                            direction: System.Data.ParameterDirection.Output);

            var created = await con.ExecuteAsync("crm.sp_CreateOpportunity",
                                       parameters,
                                       commandType: System.Data.CommandType.StoredProcedure);

            var newId = parameters.Get<int>("@@NewOpportunityId");
            return newId;

        }

        public async Task<bool> DeleteOpportunityAsync(int id, CancellationToken ct)
        {
            using var con = _context.CreateConnection();
            var sql = @"UPDATE crm.Opportunities
                        SET IsActive = 0
                        WHERE OpportunityId = @Id AND IsActive = 1
                        ";

            var affectedRows = await con.ExecuteAsync(new CommandDefinition(sql, new { Id = id }, cancellationToken: ct));
            return affectedRows > 0;
        }

        public async Task<IEnumerable<OpportunityResponseDto>> GetAllAOpportunitiesByFilterAsync(CancellationToken ct)
        {
            using var con = _context.CreateConnection();
            var sql = @"SELECT o.OpportunityId, o.OpportunityName, o.Stage, o.Amount, o.CreatedAt, o.ClosedAt, o.CustomerId, c.CxName
                        FROM crm.Opportunities o
                        LEFT JOIN crm.Customers c ON o.CustomerId = c.CxId
                        WHERE o.IsActive = 1 AND o.Stage IN (0,1)
                        ORDER BY CreatedAt DESC";
            var opportunities = await con.QueryAsync<OpportunityResponseDto>(new CommandDefinition(sql, cancellationToken: ct));
            return opportunities;
        }

        public async Task<IEnumerable<OpportunityResponseDto>> GetAllOpportunitiesAsync(CancellationToken ct)
        {
            using var con = _context.CreateConnection();
            var sql = @"SELECT o.OpportunityId, o.OpportunityName, o.Stage, o.Amount, o.CreatedAt, o.ClosedAt, o.CustomerId, c.CxName
                        FROM crm.Opportunities o
                        LEFT JOIN crm.Customers c ON o.CustomerId = c.CxId
                        WHERE o.IsActive = 1
                        ORDER BY CreatedAt DESC";

            var opportunities = await con.QueryAsync<OpportunityResponseDto>(new CommandDefinition(sql, cancellationToken: ct));
            return opportunities;
        }

        public async Task<IEnumerable<OpportunityResponseDto>> GetOpportunitiesByCustomerIdAsync(int cxId, CancellationToken ct)
        {
            using var con = _context.CreateConnection();
            var sql = @"SELECT o.OpportunityId, o.OpportunityName, o.Stage, o.Amount, o.CreatedAt, o.ClosedAt, o.CustomerId, c.CxName
                        FROM crm.Opportunities o
                        LEFT JOIN crm.Customers c ON o.CustomerId = c.CxId
                        WHERE o.IsActive = 1 AND o.CustomerId = @CustomerId
                        ORDER BY o.CreatedAt DESC";

            var opportunities = await con.QueryAsync<OpportunityResponseDto>(new CommandDefinition(sql, new { CustomerId = cxId }, cancellationToken: ct));
            return opportunities;
        }

        public async Task<OpportunityResponseDto?> GetOpportunityByIdAsync(int id, CancellationToken ct)
        {
            using var con = _context.CreateConnection();
            var sql = @"SELECT o.OpportunityId, o.OpportunityName, o.Stage, o.Amount, o.CreatedAt, o.ClosedAt, o.CustomerId, c.CxName
                        FROM crm.Opportunities o
                        LEFT JOIN crm.Customers c ON o.CustomerId = c.CxId
                        WHERE o.IsActive = 1 AND o.OpportunityId = @OpportunityId";

            var opportunity = await con.QueryFirstOrDefaultAsync<OpportunityResponseDto>(new CommandDefinition(sql, new { OpportunityId = id }, cancellationToken: ct));
            return opportunity;

        }

        public async Task<Opportunity?> GetOpportunityEntityByIdAsync(int id, CancellationToken ct)
        {
            using var con = _context.CreateConnection();
            var sql = @"SELECT OpportunityId, OpportunityName, Stage, Amount, CreatedAt, ClosedAt, CustomerId
                        FROM crm.Opportunities 
                        WHERE IsActive = 1 AND OpportunityId = @OpportunityId";

            var opportunity = await con.QueryFirstOrDefaultAsync<Opportunity>(new CommandDefinition(sql, new { OpportunityId = id }, cancellationToken: ct));
            return opportunity;
        }

        public async Task<(IEnumerable<OpportunityResponseDto> Data, int TotalCount)> GetPagedOpportunitiesAsync(PaginationRequest request, CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var sql = @"
                        SELECT 
                            o.OpportunityId,
                            o.OpportunityName,
                            o.Stage, o.Amount,
                            o.CreatedAt,
                            o.ClosedAt,
                            o.CustomerId,
                            c.CxName
                        FROM crm.Opportunities o
                        LEFT JOIN crm.Customers c ON o.CustomerId = c.CxId
                        WHERE o.IsActive = 1
                        ORDER BY CreatedAt DESC
                        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

                        SELECT COUNT(1)
                        FROM crm.Opportunities o
                        WHERE o.IsActive = 1;
                    ";

            using var multi = await con.QueryMultipleAsync(
                new CommandDefinition(sql, new
                {
                    Offset = (request.PageNumber - 1) * request.PageSize,
                    request.PageSize
                }, cancellationToken: ct));

            var data = await multi.ReadAsync<OpportunityResponseDto>();
            var totalCount = await multi.ReadFirstAsync<int>();

            return (data, totalCount);
        }

        public async Task<bool> UpdateClosedAtAsync(int oppId, CancellationToken ct)
        {
            using var con = _context.CreateConnection();
            var sql = @"UPDATE crm.Opportunities
                        SET ClosedAt = SYSUTCDATETIME()
                        WHERE OpportunityId = @OpportunityId AND IsActive = 1";

            var affectedRows = await con.ExecuteAsync(new CommandDefinition(sql, new { OpportunityId = oppId }, cancellationToken: ct));
            return affectedRows > 0;
        }

        public async Task<bool> UpdateOpportunityAsync(Opportunity opp, CancellationToken ct)
        {
            using var con = _context.CreateConnection();
            var sql = new StringBuilder(@"UPDATE crm.Opportunities 
                                          SET ");

            var parameters = new DynamicParameters();
            parameters.Add("@OpportunityId", opp.OpportunityId);

            if (!string.IsNullOrEmpty(opp.OpportunityName))
            {
                sql.Append("OpportunityName = @OpportunityName");
                parameters.Add("@OpportunityName", opp.OpportunityName);
            }
            if (opp.Stage != default)
            {
                sql.Append(", Stage = @Stage");
                parameters.Add("@Stage", opp.Stage);
            }
            if (opp.Amount != default)
            {
                sql.Append(", Amount = @Amount");
                parameters.Add("@Amount", opp.Amount);
            }

            sql.Append(" WHERE OpportunityId = @OpportunityId AND IsActive = 1");

            var updatedRows = await con.ExecuteAsync(new CommandDefinition(sql.ToString(), parameters, cancellationToken: ct));

            return updatedRows > 0;
        }
    }
}
