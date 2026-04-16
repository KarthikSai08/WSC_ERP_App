using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.StoreLayer;
using WSC.Store.Application.Dtos;

namespace WSC.Store.Application.Interfaces.ServiceInterfaces
{
    public interface IInventoryService
    {
        Task<ApiResponse<IEnumerable<InventoryResponseDto>>> GetAllInventoryRecordsAsync(CancellationToken ct);
        Task<ApiResponse<InventoryResponseDto>> GetInventoryRecordByIdAsync(int id, CancellationToken ct);
        Task<ApiResponse<bool>> DeleteInventoryRecordAsync(int id, CancellationToken ct);
        Task<ApiResponse<bool>> UpdateStockAsync(int id, int quantity, CancellationToken ct);
        Task<ApiResponse<int>> CreateInventoryRecordAsync(CreateInventoryRecordDto record, CancellationToken ct);
    }
}
