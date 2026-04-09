using AutoMapper;
using Microsoft.Extensions.Logging;
using WSC.Delivery.Application.Dtos;
using WSC.Delivery.Application.Interfaces.RepositoryInterfaces;
using WSC.Delivery.Application.Interfaces.ServiceInterfaces;
using WSC.Delivery.Domain.Entities;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.DeliveryLayer;


namespace WSC.Delivery.Application.Services
{
    public class DeliveryTrackingService : IDeliveryTrackingService
    {
        private readonly IDeliveryTrackingRepository _trackingRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DeliveryTrackingService> _logger;

        public DeliveryTrackingService(
            IDeliveryTrackingRepository trackingRepository,
            IMapper mapper,
            ILogger<DeliveryTrackingService> logger)
        {
            _trackingRepository = trackingRepository ?? throw new ArgumentNullException(nameof(trackingRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ApiResponse<int>> CreateTrackingRecordAsync(CreateDeliveryTrackingDto dto, CancellationToken ct)
        {
            try
            {
                if (dto == null)
                    return ApiResponse<int>.Failed("Invalid tracking data.");

                _logger.LogInformation("Creating new tracking record for delivery ID: {DeliveryId}", dto.DeliveryId);

                var tracking = _mapper.Map<DeliveryTracking>(dto);
                var trackingId = await _trackingRepository.CreateTrackingRecordAsync(tracking, ct);

                _logger.LogInformation("Tracking record created successfully. ID: {TrackingId}", trackingId);
                return ApiResponse<int>.Ok(trackingId, "Tracking record created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating tracking record");
                return ApiResponse<int>.Failed("Failed to create tracking record. Please try again later.");
            }
        }

        public async Task<ApiResponse<IEnumerable<DeliveryTrackingResponseDto>>> GetTrackingByDeliveryIdAsync(int deliveryId, CancellationToken ct)
        {
            try
            {
                if (deliveryId <= 0)
                    return ApiResponse<IEnumerable<DeliveryTrackingResponseDto>>.Failed("Invalid delivery ID.");

                _logger.LogInformation("Fetching tracking records for delivery ID: {DeliveryId}", deliveryId);

                var trackingRecords = await _trackingRepository.GetTrackingByDeliveryIdAsync(deliveryId, ct);

                if (trackingRecords == null || !trackingRecords.Any())
                {
                    _logger.LogInformation("No tracking records found for delivery ID: {DeliveryId}", deliveryId);
                    return ApiResponse<IEnumerable<DeliveryTrackingResponseDto>>.Ok(
                        new List<DeliveryTrackingResponseDto>(),
                        "No tracking records found.");
                }

                return ApiResponse<IEnumerable<DeliveryTrackingResponseDto>>.Ok(
                    trackingRecords,
                    "Tracking records retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching tracking records for delivery ID: {DeliveryId}", deliveryId);
                return ApiResponse<IEnumerable<DeliveryTrackingResponseDto>>.Failed(
                    "Failed to retrieve tracking records. Please try again later.");
            }
        }

        public async Task<ApiResponse<DeliveryTrackingResponseDto>> GetTrackingByIdAsync(int trackingId, CancellationToken ct)
        {
            try
            {
                if (trackingId <= 0)
                    return ApiResponse<DeliveryTrackingResponseDto>.Failed("Invalid tracking ID.");

                _logger.LogInformation("Fetching tracking record with ID: {TrackingId}", trackingId);

                var tracking = await _trackingRepository.GetTrackingByIdAsync(trackingId, ct);

                if (tracking == null)
                {
                    _logger.LogWarning("Tracking record not found. ID: {TrackingId}", trackingId);
                    return ApiResponse<DeliveryTrackingResponseDto>.Failed("Tracking record not found.");
                }

                return ApiResponse<DeliveryTrackingResponseDto>.Ok(tracking, "Tracking record retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching tracking record {TrackingId}", trackingId);
                return ApiResponse<DeliveryTrackingResponseDto>.Failed(
                    "Failed to retrieve tracking record. Please try again later.");
            }
        }

        public async Task<ApiResponse<IEnumerable<DeliveryTrackingResponseDto>>> GetLatestTrackingAsync(int deliveryId, int count, CancellationToken ct)
        {
            try
            {
                if (deliveryId <= 0 || count <= 0)
                    return ApiResponse<IEnumerable<DeliveryTrackingResponseDto>>.Failed("Invalid parameters.");

                _logger.LogInformation("Fetching latest {Count} tracking records for delivery ID: {DeliveryId}", count, deliveryId);

                var trackingRecords = await _trackingRepository.GetLatestTrackingByDeliveryIdAsync(deliveryId, count, ct);

                if (trackingRecords == null || !trackingRecords.Any())
                {
                    _logger.LogInformation("No tracking records found for delivery ID: {DeliveryId}", deliveryId);
                    return ApiResponse<IEnumerable<DeliveryTrackingResponseDto>>.Ok(
                        new List<DeliveryTrackingResponseDto>(),
                        "No tracking records found.");
                }

                return ApiResponse<IEnumerable<DeliveryTrackingResponseDto>>.Ok(
                    trackingRecords,
                    "Latest tracking records retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching latest tracking records for delivery ID: {DeliveryId}", deliveryId);
                return ApiResponse<IEnumerable<DeliveryTrackingResponseDto>>.Failed(
                    "Failed to retrieve tracking records. Please try again later.");
            }
        }
    }
}
