using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.StoreLayer;
using WSC.Store.Application.Dtos;

namespace WSC.Store.Application.Interfaces.ServiceInterfaces
{
    public interface IOrderItemsService
    {
        Task<ApiResponse<IEnumerable<OrderItemResponseDto>>> GetAllOrderItemsAsync(CancellationToken ct);
        Task<ApiResponse<OrderItemResponseDto>> GetItemByIdAsync(int orderItemId, CancellationToken ct);
        Task<ApiResponse<int>> CreateOrderItemAsync(CreateItemsDto items, CancellationToken ct);
        Task<ApiResponse<bool>> UpdateOrderItemAsync(UpdateItemsDto items, CancellationToken ct);
        Task<ApiResponse<bool>> DeleteOrderItemAsync(int orderItemId, CancellationToken ct);
    }
}
