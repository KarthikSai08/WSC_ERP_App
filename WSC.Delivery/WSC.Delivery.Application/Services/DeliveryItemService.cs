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
	public sealed class DeliveryItemService : IDeliveryItemService
	{
		private readonly IDeliveryItemRepository _itemRepo;
		private readonly IMapper _mapper;
		private readonly IProductClient _prdClient;
		private readonly IDeliveryRepository _deliveryRepo;
		private readonly ILogger<DeliveryItemService> _logger;

		public DeliveryItemService(
			IDeliveryRepository deliveryRepo,
			IDeliveryItemRepository itemRepo,
			IMapper mapper,
			IProductClient prdClient,
			ILogger<DeliveryItemService> logger)
		{
			_deliveryRepo = deliveryRepo ?? throw new ArgumentException(nameof(deliveryRepo));
			_itemRepo = itemRepo ?? throw new ArgumentNullException(nameof(itemRepo));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_prdClient = prdClient ?? throw new ArgumentNullException(nameof(prdClient));
		}

		public async Task<ApiResponse<int>> CreateDeliveryItemAsync(CreateDeliveryItemDto dto, CancellationToken ct)
		{
            var prdTask = _prdClient.GetProductByIdAsync(dto.ProductId, ct);
			var deliveryTask = _deliveryRepo.GetByDeliveryIdAsync(dto.DeliveryId, ct); 
			
			await Task.WhenAll(prdTask, deliveryTask); 

			var prd = await prdTask; 
			var delivery = await deliveryTask;

            if (prd == null)
				throw new NotFoundException("Product", dto.ProductId);

			if (delivery == null)
				throw new NotFoundException("Delivery", dto.DeliveryId);

			if (dto == null)
				return ApiResponse<int>.Failed("Invalid delivery item data.");

			var item = _mapper.Map<DeliveryItem>(dto);
			var itemId = await _itemRepo.CreateDeliveryItemAsync(item, ct);

			return ApiResponse<int>.Ok(itemId, "Delivery item created successfully");
		}

		public async Task<ApiResponse<IEnumerable<DeliveryItemResponseDto>>> GetDeliveryItemsByDeliveryIdAsync(int deliveryId, CancellationToken ct)
		{
			if (deliveryId <= 0)
				return ApiResponse<IEnumerable<DeliveryItemResponseDto>>.Failed("Invalid delivery ID.");

			var items = await _itemRepo.GetItemsByDeliveryIdAsync(deliveryId, ct);

			if (items == null || !items.Any())
				return ApiResponse<IEnumerable<DeliveryItemResponseDto>>.Ok(
						new List<DeliveryItemResponseDto>(),"No delivery items found.");
			return ApiResponse<IEnumerable<DeliveryItemResponseDto>>.Ok(items,"Delivery items retrieved successfully");
		}

		public async Task<ApiResponse<DeliveryItemResponseDto>> GetDeliveryItemByIdAsync(int itemId, CancellationToken ct)
		{
			if (itemId <= 0)
				return ApiResponse<DeliveryItemResponseDto>.Failed("Invalid item ID.");

			var item = await _itemRepo.GetItemByIdAsync(itemId, ct);
			if (item == null)
				throw new NotFoundException("OrderItem", itemId);
			
			return ApiResponse<DeliveryItemResponseDto>.Ok(item, "Delivery item retrieved successfully");
		}

		public async Task<ApiResponse<bool>> UpdateDeliveryItemAsync(UpdateDeliveryItemDto dto, CancellationToken ct)
		{
				if (dto == null || dto.DeliveryItemId <= 0)
					return ApiResponse<bool>.Failed("Invalid delivery item data.");

				var existingItem = await _itemRepo.GetItemEntityByIdAsync(dto.DeliveryItemId, ct);
				if (existingItem == null)
					throw new NotFoundException("Delivery Item", dto.DeliveryItemId);

				_mapper.Map(dto, existingItem);
				var updated = await _itemRepo.UpdateDeliveryItemAsync(existingItem, ct);

				if (!updated)
					return ApiResponse<bool>.Failed("Failed to update delivery item.");
				return ApiResponse<bool>.Ok(true, "Delivery item updated successfully");
		}

		public async Task<ApiResponse<bool>> DeleteDeliveryItemAsync(int itemId, CancellationToken ct)
		{
			if (itemId <= 0)
				return ApiResponse<bool>.Failed("Invalid item ID.");
			var exists = await _itemRepo.GetItemByIdAsync(itemId, ct);
			if (exists == null)
				throw new NotFoundException("OrderItem", itemId);

			var deleted = await _itemRepo.DeleteDeliveryItemAsync(itemId, ct);

			if (!deleted)
				return ApiResponse<bool>.Failed("Delivery item not found or already deleted.");
			return ApiResponse<bool>.Ok(true, "Delivery item deleted successfully");
		}
	}
}
