using WSC.Shared.Contracts.Dtos.StoreLayer;

namespace WSC.Shared.Contracts.Interfaces.StoreClients
{
    public interface IProductClient
    {
        Task<ProductResponseDto?> GetProductByIdAsync(int productId, CancellationToken ct);
    }
}
