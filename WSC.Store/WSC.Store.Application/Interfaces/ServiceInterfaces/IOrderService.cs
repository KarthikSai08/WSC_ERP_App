using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.StoreLayer;
using WSC.Store.Application.Dtos;

namespace WSC.Store.Application.Interfaces.ServiceInterfaces
{
    public interface IOrderService
    {
        Task<ApiResponse<IEnumerable<OrderResponseDto>>> GetAllOrdersAsync(CancellationToken ct);
        Task<ApiResponse<OrderResponseDto>> GetByIdAsync(int id, CancellationToken ct);
        Task<ApiResponse<int>> CreateOrderAsync(CreateOrderDto dto, CancellationToken ct);
        Task<ApiResponse<bool>> UpdateOrderAsync(UpdateOrderDto dto, CancellationToken ct);
        Task<ApiResponse<bool>> DeleteOrderAsync(int id, CancellationToken ct);
    }
}