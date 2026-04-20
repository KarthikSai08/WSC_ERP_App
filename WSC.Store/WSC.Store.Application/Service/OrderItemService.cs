using AutoMapper;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.StoreLayer;
using WSC.Shared.Contracts.Exceptions;
using WSC.Store.Application.Dtos;
using WSC.Store.Application.Interfaces.RepositoryInterfaces;
using WSC.Store.Application.Interfaces.ServiceInterfaces;
using WSC.Store.Domain.Entities;

namespace WSC.Store.Application.Service
{
    public sealed class OrderItemService : IOrderItemsService
    {
        private readonly IOrderItemsRepository _itemsRepo;
        private readonly IOrderRepository _orderRepo;
        private readonly IProductRepository _productRepo;
        private readonly IInventoryRepository _inventoryRepo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public OrderItemService(IOrderItemsRepository itemsRepo,
                                IOrderRepository orderRepo,
                                IProductRepository productRepo,
                                IInventoryRepository inventoryRepo,
                                IMapper mapper,
                                IUnitOfWork uow)
        {
            _itemsRepo = itemsRepo;
            _orderRepo = orderRepo;
            _productRepo = productRepo;
            _inventoryRepo = inventoryRepo;
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<ApiResponse<int>> CreateOrderItemAsync(CreateItemsDto items, CancellationToken ct)
        {
            await _uow.BeginAsync();
            try
            {
                var prdTask = _productRepo.GetProductByIdAsync(items.ProductId, ct);
                var orderTask = _orderRepo.GetOrderByIdAsync(items.OrderId, ct);

                await Task.WhenAll(prdTask, orderTask);

                var prd = await prdTask;
                var order = await orderTask;

                if (prd == null)
                    throw new NotFoundException("Product", items.ProductId);
                if (order == null)
                    throw new NotFoundException("Order", items.OrderId);

                var updatedStock = await _inventoryRepo.ReduceStockAsync(items.ProductId, items.Quantity, _uow.Transaction, ct);

                var created = _mapper.Map<OrderItems>(items);
                var orderItems = await _itemsRepo.CreateOrderItemAsync(created, _uow.Transaction, ct);

                await _uow.CommitAsync();

                return ApiResponse<int>.Ok(orderItems, "OrdeItem created Successfully");

            }
            catch
            {
                await _uow.RollbackAsync();
                throw;
            }
        }

        public async Task<ApiResponse<bool>> DeleteOrderItemAsync(int orderItemId, CancellationToken ct)
        {
            if (orderItemId < 0) throw new ArgumentOutOfRangeException("Enter a valid Id");
            var orderItem = await _itemsRepo.GetItemByIdAsync(orderItemId, ct);

            if (orderItem == null)
                throw new NotFoundException("OrderItem ", orderItemId);

            var deleted = await _itemsRepo.DeleteOrderItemAsync(orderItemId, ct);
            if (!deleted)
                return ApiResponse<bool>.Failed("Failed to Delete ");
            return ApiResponse<bool>.Ok(deleted, "The Item is Deleted Successfully");
        }

        public async Task<ApiResponse<IEnumerable<OrderItemResponseDto>>> GetAllOrderItemsAsync(CancellationToken ct)
        {
            var orderItems = await _itemsRepo.GetAllOrderItemsAsync(ct);

            if (orderItems == null || !orderItems.Any())
                return ApiResponse<IEnumerable<OrderItemResponseDto>>.Ok(new List<OrderItemResponseDto>(), "No orders found.");
            return ApiResponse<IEnumerable<OrderItemResponseDto>>.Ok(orderItems, "Order Items Fetched Successfully");
        }

        public async Task<ApiResponse<OrderItemResponseDto>> GetItemByIdAsync(int orderItemId, CancellationToken ct)
        {
            if (orderItemId < 0)
                throw new ArgumentOutOfRangeException("Enter a Valid Id ");
            var orderItem = await _itemsRepo.GetItemByIdAsync(orderItemId, ct);
            if (orderItem == null)
                return ApiResponse<OrderItemResponseDto>.Failed("Order Item not found");

            return ApiResponse<OrderItemResponseDto>.Ok(orderItem, "OrderItem Fetched Successfully");
        }

        public async Task<ApiResponse<bool>> UpdateOrderItemAsync(UpdateItemsDto items, CancellationToken ct)
        {
            var orderItem = await _itemsRepo.GetOrderItemEntityByIdAsync(items.OrderItemId, ct);
            if (orderItem == null)
                throw new NotFoundException("OrderItem", items.OrderItemId);

            _mapper.Map(items, orderItem);
            var updated = await _itemsRepo.UpdateOrderItemAsync(orderItem, ct);

            if (!updated)
                return ApiResponse<bool>.Failed("Failed To Update OrderItem");
            return ApiResponse<bool>.Ok(true, "Order Item Updated Successfully");

        }
    }
}
