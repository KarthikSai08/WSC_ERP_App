using Dapper;
using System.Data;
using System.Text;
using WSC.Delivery.Application.Interfaces.RepositoryInterfaces;
using WSC.Delivery.Domain.Entities;
using WSC.Delivery.Infrastructure.Persistence.Context;
using WSC.Shared.Contracts.Dtos.DeliveryLayer;

namespace WSC.Delivery.Infrastructure.Repositories
{
    internal class DeliveryAgentRepository : IDeliveryAgentRepository
    {
        private readonly DapperContext _context;
        public DeliveryAgentRepository(DapperContext context) => _context = context;

        public async Task<int> CreateDeliveryAgentAsync(DeliveryAgent agent, CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@AgentName", agent.AgentName);
            parameters.Add("@AgentPhone", agent.AgentPhone);
            parameters.Add("@VehicleNumber", agent.VehicleNumber);
            parameters.Add("@IsAvailable", agent.IsAvailable);
            parameters.Add("@IsActive", agent.IsActive);
            parameters.Add("@CreatedAt", DateTime.UtcNow);
            parameters.Add("@DeliveryAgentId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var sql = @"INSERT INTO delivery.DeliveryAgents 
                        (AgentName, AgentPhone, VehicleNumber, IsAvailable, IsActive, CreatedAt)
                        VALUES (@AgentName, @AgentPhone, @VehicleNumber, @IsAvailable, @IsActive, @CreatedAt);
                        SET @DeliveryAgentId = SCOPE_IDENTITY();";

            await con.ExecuteAsync(new CommandDefinition(sql, parameters, cancellationToken: ct));

            return parameters.Get<int>("@DeliveryAgentId");
        }

        public async Task<IEnumerable<DeliveryAgentResponseDto>> GetAllAgentsAsync(CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var sql = @"SELECT DeliveryAgentId, AgentName, AgentPhone, VehicleNumber, IsAvailable, IsActive, CreatedAt
                        FROM delivery.DeliveryAgents
                        WHERE IsActive = 1
                        ORDER BY DeliveryAgentId";

            var agents = await con.QueryAsync<DeliveryAgentResponseDto>(new CommandDefinition(sql, cancellationToken: ct));

            return agents;
        }

        public async Task<DeliveryAgentResponseDto> GetAgentByIdAsync(int agentId, CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var sql = @"SELECT DeliveryAgentId, AgentName, AgentPhone, VehicleNumber, IsAvailable, IsActive, CreatedAt
                        FROM delivery.DeliveryAgents
                        WHERE DeliveryAgentId = @AgentId AND IsActive = 1";

            var agent = await con.QueryFirstOrDefaultAsync<DeliveryAgentResponseDto>(
                new CommandDefinition(sql, new { AgentId = agentId }, cancellationToken: ct));

            return agent;
        }

        public async Task<DeliveryAgent> GetAgentEntityByIdAsync(int agentId, CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var sql = @"SELECT DeliveryAgentId, AgentName, AgentPhone, VehicleNumber, IsAvailable, IsActive, CreatedAt
                        FROM delivery.DeliveryAgents
                        WHERE DeliveryAgentId = @AgentId AND IsActive = 1";

            var agent = await con.QueryFirstOrDefaultAsync<DeliveryAgent>(
                new CommandDefinition(sql, new { AgentId = agentId }, cancellationToken: ct));

            return agent;
        }

        public async Task<IEnumerable<DeliveryAgent>> GetAvailableAgentsAsync(CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var sql = @"SELECT DeliveryAgentId, AgentName, AgentPhone, VehicleNumber, IsAvailable, IsActive, CreatedAt
                        FROM delivery.DeliveryAgents
                        WHERE IsAvailable = 1 AND IsActive = 1
                        ORDER BY DeliveryAgentId";

            var agents = await con.QueryAsync<DeliveryAgent>(new CommandDefinition(sql, cancellationToken: ct));

            return agents;
        }

        public async Task<bool> UpdateAgentAsync(DeliveryAgent agent, CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var sql = new StringBuilder(@"UPDATE delivery.DeliveryAgents SET");

            var parameters = new DynamicParameters();
            parameters.Add("@DeliveryAgentId", agent.DeliveryAgentId);

            var updateFields = new List<string>();

            if (!string.IsNullOrWhiteSpace(agent.AgentName))
            {
                updateFields.Add("AgentName = @AgentName");
                parameters.Add("@AgentName", agent.AgentName);
            }

            if (!string.IsNullOrWhiteSpace(agent.AgentPhone))
            {
                updateFields.Add("AgentPhone = @AgentPhone");
                parameters.Add("@AgentPhone", agent.AgentPhone);
            }

            if (!string.IsNullOrWhiteSpace(agent.VehicleNumber))
            {
                updateFields.Add("VehicleNumber = @VehicleNumber");
                parameters.Add("@VehicleNumber", agent.VehicleNumber);
            }

            updateFields.Add("IsAvailable = @IsAvailable");
            parameters.Add("@IsAvailable", agent.IsAvailable);

            if (updateFields.Count == 0)
                return true;

            sql.Append(" " + string.Join(", ", updateFields));
            sql.Append(" WHERE DeliveryAgentId = @DeliveryAgentId AND IsActive = 1");

            var affectedRows = await con.ExecuteAsync(new CommandDefinition(sql.ToString(), parameters, cancellationToken: ct));

            return affectedRows > 0;
        }

        public async Task<bool> DeleteAgentByIdAsync(int agentId, CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var sql = @"UPDATE delivery.DeliveryAgents
                        SET IsActive = 0
                        WHERE DeliveryAgentId = @AgentId AND IsActive = 1";

            var affectedRows = await con.ExecuteAsync(
                new CommandDefinition(sql, new { AgentId = agentId }, cancellationToken: ct));

            return affectedRows > 0;
        }
    }
}
