using Dapper;
using System.Data;
using WSC.Delivery.Application.Interfaces.RepositoryInterfaces;
using WSC.Delivery.Domain.Entities;
using WSC.Delivery.Infrastructure.Persistence.Context;
using WSC.Shared.Contracts.Dtos.DeliveryLayer;

namespace WSC.Delivery.Infrastructure.Repositories
{
    internal class DeliveryTrackingRepository : IDeliveryTrackingRepository
    {
        private readonly DapperContext _context;
        public DeliveryTrackingRepository(DapperContext context) => _context = context;

        public async Task<int> CreateTrackingRecordAsync(DeliveryTracking tracking, CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@DeliveryId", tracking.DeliveryId);
            parameters.Add("@Status", tracking.Status.ToString());
            parameters.Add("@Location", tracking.Location);
            parameters.Add("@Remarks", tracking.Remarks);
            parameters.Add("@Timestamp", DateTime.UtcNow);

            parameters.Add("@TrackingId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var sql = @"INSERT INTO delivery.DeliveryTracking 
                        (DeliveryId, Status, Location, Remarks, Timestamp)
                        VALUES (@DeliveryId, @Status, @Location, @Remarks, @Timestamp);
                        SET @TrackingId = SCOPE_IDENTITY();";

            await con.ExecuteAsync(new CommandDefinition(sql, parameters, cancellationToken: ct));

            return parameters.Get<int>("@TrackingId");
        }

        public async Task<IEnumerable<DeliveryTrackingResponseDto>> GetTrackingByDeliveryIdAsync(int deliveryId, CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var sql = @"SELECT TrackingId, DeliveryId, Status, Location, Remarks, Timestamp
                        FROM delivery.DeliveryTracking
                        WHERE DeliveryId = @DeliveryId
                        ORDER BY Timestamp DESC";

            var tracking = await con.QueryAsync<DeliveryTrackingResponseDto>(
                new CommandDefinition(sql, new { DeliveryId = deliveryId }, cancellationToken: ct));

            return tracking;
        }

        public async Task<DeliveryTrackingResponseDto> GetTrackingByIdAsync(int trackingId, CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var sql = @"SELECT TrackingId, DeliveryId, Status, Location, Remarks, Timestamp
                        FROM delivery.DeliveryTracking
                        WHERE TrackingId = @TrackingId";

            var tracking = await con.QueryFirstOrDefaultAsync<DeliveryTrackingResponseDto>(
                new CommandDefinition(sql, new { TrackingId = trackingId }, cancellationToken: ct));

            return tracking;
        }

        public async Task<DeliveryTracking> GetTrackingEntityByIdAsync(int trackingId, CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var sql = @"SELECT TrackingId, DeliveryId, Status, Location, Remarks, Timestamp
                        FROM delivery.DeliveryTracking
                        WHERE TrackingId = @TrackingId";

            var tracking = await con.QueryFirstOrDefaultAsync<DeliveryTracking>(
                new CommandDefinition(sql, new { TrackingId = trackingId }, cancellationToken: ct));

            return tracking;
        }
        public async Task<IEnumerable<DeliveryTrackingResponseDto>> GetLatestTrackingByDeliveryIdAsync(int deliveryId, int count, CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var sql = @"SELECT TOP (@Count) TrackingId, DeliveryId, Status, Location, Remarks, Timestamp
                        FROM delivery.DeliveryTracking
                        WHERE DeliveryId = @DeliveryId
                        ORDER BY Timestamp DESC";

            var tracking = await con.QueryAsync<DeliveryTrackingResponseDto>(
                new CommandDefinition(sql, new { DeliveryId = deliveryId, Count = count }, cancellationToken: ct));

            return tracking;
        }

        public async Task<bool> DeleteTrackingRecordAsync(int trackingId, CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var sql = @"DELETE FROM delivery.DeliveryTracking
                        WHERE TrackingId = @TrackingId";

            var affectedRows = await con.ExecuteAsync(
                new CommandDefinition(sql, new { TrackingId = trackingId }, cancellationToken: ct));

            return affectedRows > 0;
        }
    }
}
