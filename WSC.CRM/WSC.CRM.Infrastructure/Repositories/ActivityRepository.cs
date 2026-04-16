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
    public class ActivityRepository : IActivityRepository
    {
        private readonly DapperContext _context;
        public ActivityRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<int> CreateActivityAsync(Activity act, CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@Title", act.Title);
            parameters.Add("@Description", act.Description);
            parameters.Add("@Type", (int)act.Type);
            parameters.Add("@ScheduledAt", act.ScheduledAt);
            parameters.Add("@LeadId", act.LeadId);
            parameters.Add("@OpportunityId", act.OpportunityId);
            parameters.Add("@CustomerId", act.CustomerId);

            parameters.Add("@@NewActivityId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await con.ExecuteAsync(
                "crm.sp_CreateActivity",
                parameters,
                commandType: CommandType.StoredProcedure);

            var id = parameters.Get<int>("@@NewActivityId");
            return id;
        }

        public async Task<bool> DeleteActivityAsync(int id, CancellationToken ct)
        {
            using var con = _context.CreateConnection();
            var sql = @"UPDATE crm.Activities
                        SET IsActive = 0 , UpdatedAt = SYSUTCDATETIME()
                        WHERE ActivityId = @Id AND IsActive = 1
                        ";

            var affectedRows = await con.ExecuteAsync(new CommandDefinition(sql, new { Id = id }, cancellationToken: ct));
            return affectedRows > 0;
        }

        public async Task<IEnumerable<ActivityResponseDto>> GetActivitiesByLeadIdAsync(int leadId, CancellationToken ct)
        {
            using var con = _context.CreateConnection();
            var sql = @"SELECT a.ActivityId, a.Title, a.Description, a.Type, a.ScheduledAt, a.CompletedAt, a.IsActive, a.LeadId, l.LeadName
                        FROM crm.Activities a
                        LEFT JOIN crm.Leads l ON a.LeadId = l.LeadId
                        WHERE a.LeadId = @Id AND a.IsActive = 1 ";

            var activity = await con.QueryAsync<ActivityResponseDto>(new CommandDefinition(sql, new { Id = leadId }, cancellationToken: ct));

            return activity;
        }

        public async Task<ActivityResponseDto?> GetActivityByIdAsync(int id, CancellationToken ct)
        {
            using var con = _context.CreateConnection();
            var sql = @"SELECT a.ActivityId, a.Title, a.Description, a.Type, a.ScheduledAt, a.CompletedAt, a.IsActive, a.LeadId, l.LeadName
                        FROM crm.Activities a
                        LEFT JOIN crm.Leads l ON a.LeadId = l.LeadId
                        WHERE a.ActivityId = @Id AND a.IsActive = 1";

            var activity = await con.QueryFirstOrDefaultAsync<ActivityResponseDto>(new CommandDefinition(sql, new { Id = id }, cancellationToken: ct));

            return activity;
        }

        public async Task<Activity?> GetActivityEntityByIdAsync(int id, CancellationToken ct)
        {
            using var con = _context.CreateConnection();
            var sql = @"SELECT ActivityId, Title, Description, Type, ScheduledAt, CompletedAt, IsActive, LeadId, CustomerId, OpportunityId
                        FROM crm.Activities 
                        WHERE ActivityId = @Id AND IsActive = 1";

            var activity = await con.QueryFirstOrDefaultAsync<Activity>(new CommandDefinition(sql, new { Id = id }, cancellationToken: ct));
            return activity;
        }

        public async Task<IEnumerable<ActivityResponseDto>> GetAllActivitiesAsync(CancellationToken ct)
        {
            using var con = _context.CreateConnection();
            var sql = @"SELECT a.ActivityId, a.Title, a.Description, a.Type, a.ScheduledAt, a.CompletedAt, a.IsActive, a.LeadId, l.LeadName
                        FROM crm.Activities a
                        LEFT JOIN crm.Leads l ON a.LeadId = l.LeadId
                        WHERE a.IsActive = 1";

            var activities = await con.QueryAsync<ActivityResponseDto>(new CommandDefinition(sql, cancellationToken: ct));
            return activities;
        }

        public Task<IEnumerable<ActivityResponseDto>> GetAllActivitiesByFilterAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<(IEnumerable<ActivityResponseDto> Data, int TotalCount)> GetPagedActivitiesAsync(PaginationRequest request, CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var sql = @"
                        SELECT 
                            a.ActivityId,
                            a.Title,
                            a.Description,
                            a.Type,
                            a.ScheduledAt,
                            a.CompletedAt,
                            a.LeadId,
                            l.LeadName AS LeadName,
                            a.IsActive
                        FROM crm.Activities a
                        LEFT JOIN crm.Leads l ON a.LeadId = l.LeadId
                        WHERE a.IsActive = 1
                        ORDER BY a.ScheduledAt DESC
                        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

                        SELECT COUNT(1)
                        FROM crm.Activities a
                        WHERE a.IsActive = 1;
                    ";

            using var multi = await con.QueryMultipleAsync(
                new CommandDefinition(sql, new
                {
                    Offset = (request.PageNumber - 1) * request.PageSize,
                    request.PageSize
                }, cancellationToken: ct));

            var data = await multi.ReadAsync<ActivityResponseDto>();
            var totalCount = await multi.ReadFirstAsync<int>();

            return (data, totalCount);
        }

        public async Task<bool> UpdateActivityAsync(Activity act, CancellationToken ct)
        {
            using var con = _context.CreateConnection();
            var sql = new StringBuilder(@"UPDATE crm.Activities 
                                            SET UpdatedAt = SYSUTCDATETIME()");

            var parameters = new DynamicParameters();
            parameters.Add("ActivityId", act.ActivityId);

            if (!string.IsNullOrWhiteSpace(act.Title))
            {
                sql.Append(", Title = @Title");
                parameters.Add("Title", act.Title);
            }
            if (!string.IsNullOrWhiteSpace(act.Description))
            {
                sql.Append(", Description = @Description");
                parameters.Add("Description", act.Description);
            }
            if (act.Type != default)
            {
                sql.Append(", Type = @Type");
                parameters.Add("Type", (int)act.Type);
            }
            sql.Append(" WHERE ActivityId = @ActivityId AND IsActive = 1");

            var updated = await con.ExecuteAsync(new CommandDefinition(sql.ToString(), parameters, cancellationToken: ct));

            return updated > 0;
        }

        public async Task<bool> UpdateCompletedAtAsync(int actId, CancellationToken ct)
        {
            using var con = _context.CreateConnection();
            var sql = @"UPDATE crm.Activities 
                        SET CompletedAt = SYSUTCDATETIME(),
                            UpdatedAt = SYSUTCDATETIME()
                        WHERE ActivityId = @Id AND IsActive = 1";

            var updated = await con.ExecuteAsync(new CommandDefinition(sql, new { Id = actId }, cancellationToken: ct));

            return updated > 0;

        }
    }
}
