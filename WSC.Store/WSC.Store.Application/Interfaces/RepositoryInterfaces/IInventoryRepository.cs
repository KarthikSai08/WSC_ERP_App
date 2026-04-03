using System;
using System.Collections.Generic;
using System.Text;
using WSC.Store.Domain.Entities;

namespace WSC.Store.Application.Interfaces.RepositoryInterfaces
{
    public interface IInventoryRepository
    {
        Task<IEnumerable<Inventory>> GetAllInventoryRecordsAsync(CancellationToken ct);
        Task<Inventory> GetInventoryRecordByIdAsync(int id, CancellationToken ct);
        //Task<bool> UpdateInventoryRecordAsync(Inventory inv, CancellationToken ct);
        Task<bool> DeleteInventoryRecordAsync(int id, CancellationToken ct);
        Task<bool> UpdateStockAsync(int id, int quantity, CancellationToken ct);
        Task<bool> RecordExistsByProductAsync(int prdId, CancellationToken ct);
        Task<int> CreateInventoryRecordAsync(Inventory record, CancellationToken ct);
    }
}
