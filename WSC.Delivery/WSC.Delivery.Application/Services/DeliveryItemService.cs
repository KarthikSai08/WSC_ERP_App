using AutoMapper;
using Microsoft.Extensions.Logging;
using WSC.Delivery.Application.Dtos;
using WSC.Delivery.Application.Interfaces.RepositoryInterfaces;
using WSC.Delivery.Application.Interfaces.ServiceInterfaces;
using WSC.Delivery.Domain.Entities;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.DeliveryLayer;
using WSC.Shared.Contracts.Exceptions;
using WSC.Shared.Contracts.Interfaces.StoreClients;

namespace WSC.Delivery.Application.Services
{
    public class DeliveryItemService : IDeliveryItemService
    {
        private readonly IDeliveryItemRepository _itemRepository;
        private readonly IMapper _mapper;
        private readonly IProductClient _prdClient;
        private readonly ILogger<DeliveryItemService> _logger;

        public DeliveryItemService(
            IDeliveryItemRepository itemRepository,
            IMapper mapper,
            IProductClient prdClient,
            ILogger<DeliveryItemService> logger)
        {
            _itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _prdClient = prdClient ?? throw new ArgumentNullException(nameof(prdClient));
        }

        public async Task<ApiResponse<int>> CreateDeliveryItemAsync(CreateDeliveryItemDto dto, CancellationToken ct)
        {
            try
            {
                var prd = await _prdClient.GetProductByIdAsync(dto.ProductId, ct);
                if (prd == null)
                    return ApiResponse<int>.Failed($"Product with ID {dto.ProductId} not found.");

                if (dto == null)
                    return ApiResponse<int>.Failed("Invalid delivery item data.");

                _logger.LogInformation("Creating new delivery item for delivery ID: {DeliveryId}", dto.DeliveryId);

                var item = _mapper.Map<DeliveryItem>(dto);
                var itemId = await _itemRepository.CreateDeliveryItemAsync(item, ct);

                _logger.LogInformation("Delivery item created successfully. ID: {ItemId}", itemId);
                return ApiResponse<int>.Ok(itemId, "Delivery item created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating delivery item");
                return ApiResponse<int>.Failed("Failed to create delivery item. Please try again later.");
            }
        }

        public async Task<ApiResponse<IEnumerable<DeliveryItemResponseDto>>> GetDeliveryItemsByDeliveryIdAsync(int deliveryId, CancellationToken ct)
        {
            try
            {
                if (deliveryId <= 0)
                    return ApiResponse<IEnumerable<DeliveryItemResponseDto>>.Failed("Invalid delivery ID.");

                _logger.LogInformation("Fetching delivery items for delivery ID: {DeliveryId}", deliveryId);

                var items = await _itemRepository.GetItemsByDeliveryIdAsync(deliveryId, ct);

                if (items == null || !items.Any())
                {
                    _logger.LogInformation("No delivery items found for delivery ID: {DeliveryId}", deliveryId);
                    return ApiResponse<IEnumerable<DeliveryItemResponseDto>>.Ok(
                        new List<DeliveryItemResponseDto>(),
                        "No delivery items found.");
                }

                return ApiResponse<IEnumerable<DeliveryItemResponseDto>>.Ok(
                    items,
                    "Delivery items retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching delivery items for delivery ID: {DeliveryId}", deliveryId);
                return ApiResponse<IEnumerable<DeliveryItemResponseDto>>.Failed(
                    "Failed to retrieve delivery items. Please try again later.");
            }
        }

        public async Task<ApiResponse<DeliveryItemResponseDto>> GetDeliveryItemByIdAsync(int itemId, CancellationToken ct)
        {
            try
            {
                if (itemId <= 0)
                    return ApiResponse<DeliveryItemResponseDto>.Failed("Invalid item ID.");

                _logger.LogInformation("Fetching delivery item with ID: {ItemId}", itemId);

                var item = await _itemRepository.GetItemByIdAsync(itemId, ct);

                if (item == null)
                {
                    _logger.LogWarning("Delivery item not found. ID: {ItemId}", itemId);
                    return ApiResponse<DeliveryItemResponseDto>.Failed("Delivery item not found.");
                }

                return ApiResponse<DeliveryItemResponseDto>.Ok(item, "Delivery item retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching delivery item {ItemId}", itemId);
                return ApiResponse<DeliveryItemResponseDto>.Failed(
                    "Failed to retrieve delivery item. Please try again later.");
            }
        }

        public async Task<ApiResponse<bool>> UpdateDeliveryItemAsync(UpdateDeliveryItemDto dto, CancellationToken ct)
        {
            try
            {
                if (dto == null || dto.DeliveryItemId <= 0)
                    return ApiResponse<bool>.Failed("Invalid delivery item data.");

                _logger.LogInformation("Updating delivery item with ID: {ItemId}", dto.DeliveryItemId);

                var existingItem = await _itemRepository.GetItemEntityByIdAsync(dto.DeliveryItemId, ct);
                if (existingItem == null)
                {
                    _logger.LogWarning("Delivery item not found for update. ID: {ItemId}", dto.DeliveryItemId);
                    throw new NotFoundException("Delivery Item", dto.DeliveryItemId);
                }

                _mapper.Map(dto, existingItem);
                var updated = await _itemRepository.UpdateDeliveryItemAsync(existingItem, ct);

                if (!updated)
                    return ApiResponse<bool>.Failed("Failed to update delivery item.");

                _logger.LogInformation("Delivery item updated successfully. ID: {ItemId}", dto.DeliveryItemId);
                return ApiResponse<bool>.Ok(true, "Delivery item updated successfully");
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Delivery item not found");
                return ApiResponse<bool>.Failed("Delivery item not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating delivery item {ItemId}", dto?.DeliveryItemId);
                return ApiResponse<bool>.Failed("Failed to update delivery item. Please try again later.");
            }
        }

        public async Task<ApiResponse<bool>> DeleteDeliveryItemAsync(int itemId, CancellationToken ct)
        {
            try
            {
                if (itemId <= 0)
                    return ApiResponse<bool>.Failed("Invalid item ID.");

                _logger.LogInformation("Deleting delivery item with ID: {ItemId}", itemId);

                var deleted = await _itemRepository.DeleteDeliveryItemAsync(itemId, ct);

                if (!deleted)
                    return ApiResponse<bool>.Failed("Delivery item not found or already deleted.");

                _logger.LogInformation("Delivery item deleted successfully. ID: {ItemId}", itemId);
                return ApiResponse<bool>.Ok(true, "Delivery item deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting delivery item {ItemId}", itemId);
                return ApiResponse<bool>.Failed("Failed to delete delivery item. Please try again later.");
            }
        }
    }
}
