using System.Data;
using WSC.Shared.Contracts.Dtos.StoreLayer;
using WSC.Store.Domain.Entities;

namespace WSC.Store.Application.Interfaces.RepositoryInterfaces
{
    public interface IInventoryRepository
    {
        Task<IEnumerable<InventoryResponseDto>> GetAllInventoryRecordsAsync(CancellationToken ct);
        Task<InventoryResponseDto> GetInventoryRecordByIdAsync(int id, CancellationToken ct);

        Task<Inventory> GetInventoryRecordEntityByIdAsync(int id, CancellationToken ct);
        //Task<bool> UpdateInventoryRecordAsync(Inventory inv, CancellationToken ct);
        Task<bool> DeleteInventoryRecordAsync(int id, CancellationToken ct);
        Task<bool> UpdateStockAsync(int id, int quantity, IDbTransaction transaction, CancellationToken ct);
        Task<bool> RecordExistsByProductAsync(int prdId, CancellationToken ct);
        Task<int> CreateInventoryRecordAsync(Inventory record, CancellationToken ct);
        Task<bool> ReduceStockAsync(int productId, int quantity, IDbTransaction transaction, CancellationToken ct);
    }
}
